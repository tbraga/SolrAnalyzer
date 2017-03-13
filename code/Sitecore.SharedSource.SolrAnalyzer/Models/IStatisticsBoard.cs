using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.SharedSource.SolrAnalyzer.Models
{
    public interface IStatisticsBoard
    {
        long TotalNumDocsReturned { get; set; }
        long TotalPayload { get; set; }
        double TotalTime { get; set; }

        IList<ISolrQuery> Queries { get; set; }
    }
}
