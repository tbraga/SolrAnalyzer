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
            _board.GetQueries();
            _board.Process();

            return _board;
        }

        protected virtual List<ISolrQuery GetLogEntries()


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