using System;
using System.Globalization;
using System.Web;

namespace Sitecore.SharedSource.SolrAnalyzer.Models.Queries
{
    public class Solr4Query : ASolrQuery
    {
        public Solr4Query(string logEntry) : base(logEntry)
        {
            logEntry = HttpUtility.HtmlDecode(logEntry);
            Raw = logEntry;

            if (logEntry == null)
            {
                return;
            }

            //get index name
            int idxNameStart = logEntry.IndexOf("GET /solr/", StringComparison.Ordinal);
            if (idxNameStart <= 0)
            {
                return;
            }

            int idxNameEnd = logEntry.IndexOf("/select?", StringComparison.Ordinal);
            if (idxNameEnd <= 0)
            {
                return;
            }

            idxNameStart += 10;
            Index = logEntry.Substring(idxNameStart, idxNameEnd - idxNameStart);

            //get query
            int paramsStart = logEntry.IndexOf("/select?", StringComparison.Ordinal);
            if (paramsStart <= 0)
            {
                return;
            }

            int paramsEnd = logEntry.IndexOf(" HTTP/1.1", StringComparison.Ordinal);
            if (paramsEnd <= 0)
            {
                return;
            }

            paramsStart += 8;

            Query = logEntry.Substring(paramsStart, paramsEnd - paramsStart);

            //get date
            //int dateStart = logEntry.IndexOf(" - - [", StringComparison.Ordinal);
            //dateStart += 6;

            //int dateEnd = logEntry.IndexOf("GET /solr/", StringComparison.Ordinal);
            //dateEnd += -3;

            //string dateString = logEntry.Substring(dateStart, dateEnd - dateStart);

            ////need to apply patch where we need the colon in the timezone offset
            //dateString = dateString.Substring(0, dateString.Length - 2) + ":00";

            //string format = "dd/MMM/yyyy:hh:mm:ss zzz";
            //DateTime date;
            //if (!DateTime.TryParseExact(dateString, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            //{
            //    return;
            //}

            //Date = date;

            IsValid = true;
        }
    }
}