using System;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.SharedSource.SolrAnalyzer.Models.Boards;

namespace Sitecore.SharedSource.SolrAnalyzer.Factories
{
    public class QueryFactory : IQueryFactory
    {
        private IStatisticsBoard _board;
        protected readonly IServiceProvider Provider;


        public QueryFactory(
            IStatisticsBoard board,
            IServiceProvider provider)
        {
            _board = board;
            Provider = provider;
        }

        public virtual IStatisticsBoard Create()
        {
            return Provider.GetService<IStatisticsBoard>();
        }

        public IStatisticsBoard GetStatisticsBoard()
        {
            _board = Create();
            _board.GetQueries();
            _board.Process();

            return _board;
        }

        public IStatisticsBoard GetStatisticsBoard(string index)
        {
            _board = Create();
            _board.SelectedIndex = index;
            _board.GetQueries();
            _board.Process();

            return _board;
        }
    }
}