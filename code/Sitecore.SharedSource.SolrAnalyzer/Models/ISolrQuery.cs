using System;
using System.Web;
using Sitecore.Syndication;

namespace Sitecore.SharedSource.SolrAnalyzer.Models
{
    public interface ISolrQuery
    {
        string Raw { get; set; }
        DateTime Date { get; set; }
        string Index { get; set; }
        string Query { get; set; }
        bool IsValid { get; set; }
        int Bytes { get; set; }
        TimeSpan Timespan { get; set; }
        int RowsRequested { get; set; }
        int DocumentsReturned { get; set; }
        int DocumentsFound { get; set; }

        HtmlString GetAnchor();

        string GetQueryUrl();
    }
}