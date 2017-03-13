using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
    }
}