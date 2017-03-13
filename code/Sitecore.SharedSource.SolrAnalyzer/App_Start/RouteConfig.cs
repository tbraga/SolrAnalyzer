using System.Web.Mvc;
using System.Web.Routing;
using Sitecore.Pipelines;

namespace Sitecore.SharedSource.SolrAnalyzer
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)

        {
            routes.MapRoute(
                name: "SolrAnalyzer", 
                url: "SolrAnalyzer/{controller}/{action}"
            );
        }
    }

    public class LoadRoutes
    {
        public void Process(PipelineArgs args)
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}