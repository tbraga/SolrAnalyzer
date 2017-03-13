using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Sitecore.SharedSource.SolrAnalyzer.Models;

namespace Sitecore.SharedSource.SolrAnalyzer.Factories
{
    public class QueryFactory : IQueryFactory
    {
        private IStatisticsBoard _board;
        public IStatisticsBoard GetStatisticsBoard()
        {
            _board = new StatisticsBoard();
        }

        protected virtual void GetLogEntries()
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

                _board.Queries = logEntries;

                litTotalNumDocsReturned.Text = string.Format("{0:n0}", TotalNumDocsReturned);
                litTotalPayload.Text = string.Format("{0:n0}", TotalPayload);
                litTotalTime.Text = string.Format("{0:n0}", TotalTime);
            }
        }

        protected virtual List<ISolrQuery> Filter(List<ISolrQuery> entries)
        {
            //distinct queries
            entries = entries
                .GroupBy(p => p.Query)
                .Select(g => g.First())
                .ToList();

            return entries;
        }

        private string[] WriteSafeReadAllLines(String path)
        {
            using (var csv = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var sr = new StreamReader(csv))
            {
                List<string> file = new List<string>();
                while (!sr.EndOfStream)
                {
                    file.Add(sr.ReadLine());
                }

                return file.ToArray();
            }
        }
    }
}