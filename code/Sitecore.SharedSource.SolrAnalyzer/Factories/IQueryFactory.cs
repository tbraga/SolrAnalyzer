using Sitecore.SharedSource.SolrAnalyzer.Models.Boards;

namespace Sitecore.SharedSource.SolrAnalyzer.Factories
{
    public interface IQueryFactory
    {
        IStatisticsBoard GetStatisticsBoard();

        IStatisticsBoard GetStatisticsBoard(string index);
    }
}
