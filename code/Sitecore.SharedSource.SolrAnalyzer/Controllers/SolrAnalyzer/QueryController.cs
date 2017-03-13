using System.Web.Mvc;
using Sitecore.SharedSource.SolrAnalyzer.Factories;

namespace Sitecore.SharedSource.SolrAnalyzer.Controllers.SolrAnalyzer
{
    public class QueryController : Controller
    {
        public ActionResult QueryAnalysis()
        {
            var model = new QueryFactory().GetStatisticsBoard();
            return View("QueryAnalyzer", model);
        }
    }
}
 