using System.Web.Mvc;
using Sitecore.SharedSource.SolrAnalyzer.Factories;

namespace Sitecore.SharedSource.SolrAnalyzer.Controllers.SolrAnalyzer
{
    public class QueryController : Controller
    {
        public ActionResult QueryAnalysis()
        {
            var model = new QueryFactory().GetStatisticsBoard(HttpContext.Request.QueryString["idx"]);
            return View("QueryAnalyzer", model);
        }
    }
}
 