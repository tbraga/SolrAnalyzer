using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;
using Sitecore.Foundation.DependencyInjection;
using Sitecore.SharedSource.SolrAnalyzer.Factories;
using Sitecore.SharedSource.SolrAnalyzer.Models;

namespace Sitecore.SharedSource.SolrAnalyzer.Configurator
{
    public class SolrAnalyzerConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IQueryFactory, QueryFactory>();
            serviceCollection.AddTransient<IStatisticsBoard, StatisticsBoard>();

            serviceCollection.AddMvcControllersInCurrentAssembly();
        }
    }
}