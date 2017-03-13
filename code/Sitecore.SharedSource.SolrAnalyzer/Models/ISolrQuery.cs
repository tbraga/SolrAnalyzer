using System;

namespace Sitecore.SharedSource.SolrAnalyzer.Models
{
    public interface ISolrQuery
    {
        string Raw { get; set; }
        DateTime Date { get; set; }
        string Index { get; set; }
        string Query { get; set; }
        bool IsValid { get; set; }
    }
}