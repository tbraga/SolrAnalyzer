using System.Web.Mvc;
using Sitecore.SharedSource.SolrAnalyzer.Factories;

namespace Sitecore.SharedSource.SolrAnalyzer.Controllers.SolrAnalyzer
{
    public class QueryController : Controller
    {
        protected readonly IQueryFactory _factory;
        public QueryController(IQueryFactory factory)
        {
            _factory = factory;
        }

        public ActionResult QueryAnalysis()
        {
            var model = _factory.GetStatisticsBoard(HttpContext.Request.QueryString["idx"]);
            return View("QueryAnalyzer", model);
        }
    }
}
 