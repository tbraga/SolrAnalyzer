using System;
using System.Web;
using Sitecore.Configuration;

namespace Sitecore.SharedSource.SolrAnalyzer.Models
{
    public class SolrQuery : ISolrQuery
    {
        public SolrQuery(string logEntry)
        {
            logEntry = HttpUtility.HtmlDecode(logEntry);
            Raw = logEntry;

            if (logEntry == null)
            {
                return;
            }

            //get index name
            int idxNameStart = logEntry.IndexOf("o.a.s.c.S.Request [", StringComparison.Ordinal);
            if (idxNameStart <= 0)
            {
                return;
            }

            int idxNameEnd = logEntry.IndexOf("]  webapp=/solr path=/select", StringComparison.Ordinal);
            if (idxNameEnd <= 0)
            {
                return;
            }

            idxNameStart += 19;
            Index = logEntry.Substring(idxNameStart, idxNameEnd - idxNameStart);

            //get query
            int paramsStart = logEntry.IndexOf("path=/select params={", StringComparison.Ordinal);
            if (paramsStart <= 0)
            {
                return;
            }

            int paramsEnd = logEntry.IndexOf("version=2.2", StringComparison.Ordinal);
            if (paramsEnd <= 0)
            {
                return;
            }

            paramsStart += 21;
            paramsEnd += 11;

            Query = logEntry.Substring(paramsStart, paramsEnd - paramsStart);

            //get date
            string dateString = logEntry.Substring(0, logEntry.IndexOf(" INFO", StringComparison.Ordinal));

            DateTime date;
            if (!DateTime.TryParse(dateString, out date))
            {
                return;
            }

            Date = date;

            IsValid = true;
        }

        public string Raw { get; set; }
        public DateTime Date { get; set; }
        public string Index { get; set; }
        public string Query { get; set; }
        public bool IsValid { get; set; }
        public int Bytes { get; set; }
        public TimeSpan Timespan { get; set; }
        public int RowsRequested { get; set; }
        public int DocumentsReturned { get; set; }
        public int DocumentsFound { get; set; }

        public HtmlString GetAnchor()
        {
            string showQuery = Query;
            int queryDisplayLength = Settings.GetIntSetting("SolrAnalyzer.QueryDisplayLength", 150);
            if (showQuery.Length > queryDisplayLength)
            {
                showQuery = showQuery.Substring(0, queryDisplayLength);
            }

            string url = GetQueryUrl();
            string anchorHref = url.Replace("\"", "'");
            string title = Query.Replace("\"", "'");
            string anchor = string.Format("<a href=\"{2}\" target=\"_blank\" title=\"{0}\">[{3}] - {1}</a>",
                title,
                showQuery,
                anchorHref,
                Index);

            return new HtmlString(anchor);
        }

        public string GetQueryUrl()
        {
            string solrAddress = Settings.GetSetting("ContentSearch.Solr.ServiceBaseAddress");
            string url = string.Format("{0}/{1}/select?{2}", solrAddress, Index, Query);
            url = url.Replace("/solr/solr/", "/solr/");
            url = url.Replace("/solr//solr//", "/solr/");

            return url;
        }
    }
}