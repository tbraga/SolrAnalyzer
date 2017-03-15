using Sitecore.SharedSource.SolrAnalyzer.Models;

namespace Sitecore.SharedSource.SolrAnalyzer.Factories
{
    public interface IQueryFactory
    {
        IStatisticsBoard GetStatisticsBoard();

        IStatisticsBoard GetStatisticsBoard(string index);
    }
}
