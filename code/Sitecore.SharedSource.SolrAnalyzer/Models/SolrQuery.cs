using System;
using System.Web;

namespace Sitecore.SharedSource.SolrAnalyzer.Models
{
    public class SolrQuery : ISolrQuery
    {
        public SolrQuery(string logEntry)
        {
            logEntry = HttpUtility.HtmlDecode(logEntry);
            Raw = logEntry;

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
    }
}