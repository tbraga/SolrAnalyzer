using Sitecore.SharedSource.SolrAnalyzer.Models.Queries;

namespace Sitecore.SharedSource.SolrAnalyzer.Models.Boards
{
    public class StatisticsBoardSolr6 : AStatisticsBoard
    {
        protected override string QueryIndentifier
        {
            get
            {
                return "path=/select params={";
            }
        }

        protected override ISolrQuery GetSolrQuery(string logEntry)
        {
            return new Solr6Query(logEntry);
        }
    }
}