using System.Web.Mvc;
using Sitecore.SharedSource.SolrAnalyzer.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using Sitecore.Configuration;
using Sitecore.Diagnostics;

namespace Sitecore.SharedSource.SolrAnalyzer.Controllers.SolrAnalyzer
{
    public class QueryController : Controller
    {
        public ActionResult QueryAnalysis()
        {

            var model = new QueryAnalyzer();
            return View("QueryAnalyzer", model);
        }

        private SolrLogEntry[] FilterByIndex(SolrLogEntry[] entries)
        {
            //distinct queries
            entries = entries
                .GroupBy(p => p.Query)
                .Select(g => g.First())
                .ToArray();


            return entries;
        }

        protected void btnSolrLog_OnClick(object sender, EventArgs e)
        {
            string path = Sitecore.Configuration.Settings.GetSetting("SolrAnalyzer.LogFileLocation");

            // This text is added only once to the file.
            if (System.IO.File.Exists(path))
            {
                SolrLogEntry[] logEntries = WriteSafeReadAllLines(path)
                    .Where(x => x.Contains("path=/select params={"))
                    .Select(x => new SolrLogEntry(x))
                    .Where(x => x.IsValid)
                    .ToArray();

                logEntries = FilterByIndex(logEntries);
                logEntries = Sort(logEntries);

                rptTable.DataSource = logEntries;
                rptTable.DataBind();

                litTotalNumDocsReturned.Text = string.Format("{0:n0}", TotalNumDocsReturned);
                litTotalPayload.Text = string.Format("{0:n0}", TotalPayload);
                litTotalTime.Text = string.Format("{0:n0}", TotalTime);
            }
        }

        protected void btnSubmit_OnClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtLogs.Text))
            {
                return;
            }

            var logs = txtLogs.Text.Split(new[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            if (logs.Length == 0)
            {
                return;
            }

            logs = logs.Where(x => !string.IsNullOrEmpty(x)).ToArray();

            SolrLogEntry[] logEntries = logs
                    .Where(x => x.Contains("path=/select params={"))
                    .Select(x => new SolrLogEntry(x))
                    .Where(x => x.IsValid)
                    .ToArray();

            logEntries = FilterByIndex(logEntries);
            logEntries = Sort(logEntries);

            rptTable.DataSource = logEntries;
            rptTable.DataBind();

            litTotalNumDocsReturned.Text = string.Format("{0:n0}", TotalNumDocsReturned);
            litTotalPayload.Text = string.Format("{0:n0}", TotalPayload);
            litTotalTime.Text = string.Format("{0:n0}", TotalTime);
        }

        public string[] WriteSafeReadAllLines(String path)
        {
            using (var csv = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var sr = new StreamReader(csv))
            {
                List<string> file = new List<string>();
                while (!sr.EndOfStream)
                {
                    file.Add(sr.ReadLine());
                }

                return file.ToArray();
            }
        }

        public long TotalNumDocsReturned { get; set; }
        public long TotalPayload { get; set; }
        public double TotalTime { get; set; }

        protected void rptTable_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item == null || e.Item.DataItem == null)
            {
                return;
            }

            SolrLogEntry logEntry = e.Item.DataItem as SolrLogEntry;
            if (logEntry == null)
            {
                return;
            }

            string solrAddress = Settings.GetSetting("ContentSearch.Solr.ServiceBaseAddress");
            string fullUrl = string.Format("{0}/{1}/select?{2}", solrAddress, logEntry.Index, logEntry.Query);
            fullUrl = fullUrl.Replace("/solr/solr/", "/solr/");
            fullUrl = fullUrl.Replace("/solr//solr//", "/solr/");

            Log.Debug("Aparecium - Url: " + fullUrl);

            Literal litQuery = (Literal)e.Item.FindControl("litQuery");
            Literal litNumDocsFound = (Literal)e.Item.FindControl("litNumDocsFound");
            Literal litNumDocsReturned = (Literal)e.Item.FindControl("litNumDocsReturned");
            Literal litTime = (Literal)e.Item.FindControl("litTime");
            Literal litPayload = (Literal)e.Item.FindControl("litPayload");
            Literal litRows = (Literal)e.Item.FindControl("litRows");

            string showQuery = logEntry.Query;
            if (showQuery.Length > 150)
            {
                showQuery = showQuery.Substring(0, 150);
            }

            string anchorHref = fullUrl.Replace("\"", "'");
            string title = logEntry.Query.Replace("\"", "'");
            litQuery.Text = string.Format("<a href=\"{2}\" target=\"_blank\" title=\"{0}\">[{3}] - {1}</a>",
                title,
                showQuery,
                anchorHref,
                logEntry.Index);

            DateTime start = DateTime.Now;

            try
            {
                WebRequest webRequest = WebRequest.Create(fullUrl);
                webRequest.Timeout = 5000;
                using (var wrs = (HttpWebResponse)webRequest.GetResponse())
                {
                    Stream receiveStream = wrs.GetResponseStream();
                    if (receiveStream != null)
                    {
                        StreamReader readStream = new StreamReader(receiveStream);

                        string data = readStream.ReadToEnd();
                        TotalPayload += data.Length;
                        litPayload.Text = string.Format("{0:n0}", data.Length);

                        TimeSpan span = DateTime.Now.Subtract(start);
                        litTime.Text = string.Format("{0:n0}", span.TotalMilliseconds);
                        TotalTime += span.TotalMilliseconds;
                        //number of rows requested
                        int rowStartingPoint = data.IndexOf("<str name=\"rows\">", StringComparison.Ordinal);
                        if (rowStartingPoint > 0)
                        {
                            string temp = data.Substring(rowStartingPoint + 17, 10);
                            foreach (char c in temp)
                            {
                                int n;
                                if (int.TryParse(c.ToString(), out n))
                                {
                                    litRows.Text += c;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }

                        //number of documents returned in the query
                        var numberOfDocs = Regex.Matches(data, "<doc>").Count;
                        TotalNumDocsReturned += numberOfDocs;
                        litNumDocsReturned.Text = string.Format("{0:n0}", numberOfDocs);

                        //number of documents found, not returned
                        int docStartingPoint = data.IndexOf("<result name=\"response\" numFound=\"", StringComparison.Ordinal);
                        if (docStartingPoint > 0)
                        {
                            string temp = data.Substring(docStartingPoint + 34, 10);
                            foreach (char c in temp)
                            {
                                int n;
                                if (int.TryParse(c.ToString(), out n))
                                {
                                    litNumDocsFound.Text += c;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }

                        readStream.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warn(string.Format("Error making an http request to: {0}, {1}", fullUrl, ex.Message), this);

                //throw new Exception(string.Format("Error making an http request to: {0}, {1}", fullUrl, ex.Message));
            }
        }
    }
}
 