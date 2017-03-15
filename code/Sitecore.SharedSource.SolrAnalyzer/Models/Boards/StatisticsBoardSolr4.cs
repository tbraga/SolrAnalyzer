using Sitecore.SharedSource.SolrAnalyzer.Models.Queries;

namespace Sitecore.SharedSource.SolrAnalyzer.Models.Boards
{
    public class StatisticsBoardSolr4 : AStatisticsBoard
    {
        protected override string QueryIndentifier
        {
            get { return "/select?"; }
        }

        protected override ISolrQuery GetSolrQuery(string logEntry)
        {
            return new Solr4Query(logEntry);
        }
    }
}