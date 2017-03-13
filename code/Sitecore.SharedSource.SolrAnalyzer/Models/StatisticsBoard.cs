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

        public void Process()
        {
            throw new NotImplementedException();
        }

        public void GetQueries()
        {
            string path = Sitecore.Configuration.Settings.GetSetting("SolrAnalyzer.LogFile.Location");
            string queryIdentifier = Sitecore.Configuration.Settings.GetSetting("SolrAnalyzer.LogFile.QueryIdentifier");

            // This text is added only once to the file.
            if (System.IO.File.Exists(path))
            {
                List<ISolrQuery> logEntries = WriteSafeReadAllLines(path)
                    .Where(x => x.Contains(queryIdentifier))
                    .Select(x => (ISolrQuery)new SolrQuery(x))
                    .Where(x => x.IsValid)
                    .ToList();

                logEntries = Filter(logEntries);

                return logEntries;
            }

            return new List<ISolrQuery>();
        }
    }
}