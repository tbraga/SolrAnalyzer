using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Sitecore.Configuration;
using Sitecore.Diagnostics;

namespace Sitecore.SharedSource.SolrAnalyzer.Models
{
    public class StatisticsBoard : IStatisticsBoard
    {
        public StatisticsBoard()
        {
            Queries = new List<ISolrQuery>();
        }

        public long TotalNumDocsReturned { get; set; }
        public long TotalPayload { get; set; }
        public double TotalTime { get; set; }
        public IList<ISolrQuery> Queries { get; set; }

        public void Process()
        {
            foreach (ISolrQuery solrQuery in Queries)
            {
                ProcessItem(solrQuery);
            }
        }

        public void ProcessItem(ISolrQuery query)
        {
            string fullUrl = query.GetQueryUrl();

            Log.Debug("Aparecium - Url: " + fullUrl);

            try
            {
                DateTime start = DateTime.Now;
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
                        query.Bytes = data.Length;

                        TimeSpan span = DateTime.Now.Subtract(start);
                        TotalTime += span.TotalMilliseconds;
                        query.Timespan = span;
                        
                        //number of rows/documents requested
                        query.RowsRequested = GetRowsRequestedCount(data);

                        //number of documents found, not the number returned.
                        query.DocumentsFound = GetDocumentsFoundCount(data);
                        
                        //number of documents returned in the query
                        query.DocumentsReturned = GetDocumentsReturnedCount(data);
                        TotalNumDocsReturned += query.DocumentsReturned;

                        readStream.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warn($"Error making an http request to: {fullUrl}, {ex.Message}", this);
            }
        }

        private int GetDocumentsReturnedCount(string data)
        {
            return Regex.Matches(data, "<doc>").Count;
        }

        private int GetDocumentsFoundCount(string data)
        {
            string intString = "";

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
                        intString += c;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            int returnValue;
            if (int.TryParse(intString, out returnValue))
            {
                return returnValue;
            }

            return 0;
        }

        private int GetRowsRequestedCount(string data)
        {
            string intString = "";
            int rowStartingPoint = data.IndexOf("<str name=\"rows\">", StringComparison.Ordinal);
            if (rowStartingPoint > 0)
            {
                string temp = data.Substring(rowStartingPoint + 17, 10);
                foreach (char c in temp)
                {
                    int n;
                    if (int.TryParse(c.ToString(), out n))
                    {
                        intString += c;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            int returnValue;
            if (int.TryParse(intString, out returnValue))
            {
                return returnValue; 
            }

            return 0;
        }

        public void GetQueries()
        {
            string path = Settings.GetSetting("SolrAnalyzer.LogFile.Location");
            string queryIdentifier = Settings.GetSetting("SolrAnalyzer.LogFile.QueryIdentifier");

            // This text is added only once to the file.
            if (File.Exists(path))
            {
                List<ISolrQuery> logEntries = WriteSafeReadAllLines(path)
                    .Where(x => x.Contains(queryIdentifier))
                    .Select(x => (ISolrQuery)new SolrQuery(x))
                    .Where(x => x.IsValid)
                    .ToList();

                logEntries = Filter(logEntries);

                Queries = logEntries;
            }
        }

        protected virtual List<ISolrQuery> Filter(List<ISolrQuery> entries)
        {
            //distinct queries
            entries = entries
                .GroupBy(p => p.Query)
                .Select(g => g.First())
                .ToList();

            return entries;
        }

        private string[] WriteSafeReadAllLines(String path)
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
    }
}