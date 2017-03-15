using System;
using System.Web;
using Sitecore.Configuration;

namespace Sitecore.SharedSource.SolrAnalyzer.Models.Queries
{
    public abstract class ASolrQuery : ISolrQuery
    {
        protected ASolrQuery(string logEntry)
        {
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
            int queryDisplayLength = Settings.GetIntSetting("SolrAnalyzer.QueryDisplayLength", 115);
            if (showQuery.Length > queryDisplayLength)
            {
                showQuery = showQuery.Substring(0, queryDisplayLength);
            }

            string url = GetQueryUrl();
            string anchorHref = url.Replace("\"", "'");
            string title = Query.Replace("\"", "'");
            string anchor = string.Format("<a href=\"{2}\" target=\"_blank\" title=\"{0}\">{1}</a>",
                title,
                showQuery,
                anchorHref);

            return new HtmlString(anchor);
        }

        public string GetQueryUrl()
        {
            string solrAddress = Settings.GetSetting("ContentSearch.Solr.ServiceBaseAddress");
            string url = string.Format("{0}/{1}/select?{2}", solrAddress, Index, Query);
            url = url.Replace("/solr/solr/", "/solr/");
            url = url.Replace("/solr//solr//", "/solr/");
            url = url.Replace("/solr//", "/solr/");
            url = url.Replace("/solr///", "/solr/");

            url = url.Replace("wt=json", "wt=xml");

            return url;
        }
    }
}