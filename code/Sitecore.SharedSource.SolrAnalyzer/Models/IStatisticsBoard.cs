﻿using System.Collections.Generic;

namespace Sitecore.SharedSource.SolrAnalyzer.Models
{
    public interface IStatisticsBoard
    {
        long TotalNumDocsReturned { get; set; }
        long TotalPayload { get; set; }
        double TotalTime { get; set; }
        IList<ISolrQuery> Queries { get; set; }
        string SelectedIndex { get; set; }
        List<string> Indexes { get; set; }

        void Process();
        void GetQueries();
    }
}
