using System.Collections.Generic;
using Sitecore.SharedSource.SolrAnalyzer.Models.Queries;

namespace Sitecore.SharedSource.SolrAnalyzer.Models.Boards
{
    public interface IStatisticsBoard
    {
        long TotalNumDocsReturned { get; set; }
        long TotalPayload { get; set; }
        double TotalTime { get; set; }
        List<ISolrQuery> Queries { get; set; }
        string SelectedIndex { get; set; }
        List<string> Indexes { get; set; }

        void Process();
        void GetQueries();
    }
}
