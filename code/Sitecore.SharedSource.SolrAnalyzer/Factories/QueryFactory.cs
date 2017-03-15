﻿using Sitecore.SharedSource.SolrAnalyzer.Models;

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

        public IStatisticsBoard GetStatisticsBoard(string index)
        {
            _board = new StatisticsBoard();
            _board.SelectedIndex = index;
            _board.GetQueries();
            _board.Process();

            return _board;
        }
    }
}