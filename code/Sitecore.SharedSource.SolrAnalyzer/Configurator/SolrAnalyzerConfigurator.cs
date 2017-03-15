using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;
using Sitecore.Foundation.DependencyInjection;
using Sitecore.SharedSource.SolrAnalyzer.Factories;
using Sitecore.SharedSource.SolrAnalyzer.Models.Boards;

namespace Sitecore.SharedSource.SolrAnalyzer.Configurator
{
    public class SolrAnalyzerConfigurator4 : ConfigHelper, IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            SetConfigure(serviceCollection, 4);
        }
    }

    public class SolrAnalyzerConfigurator5 : ConfigHelper, IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            SetConfigure(serviceCollection, 5);
        }
    }

    public class SolrAnalyzerConfigurator6 : ConfigHelper, IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            SetConfigure(serviceCollection, 6);
        }
    }

    public class ConfigHelper
    {
        public void SetConfigure(IServiceCollection serviceCollection, int version)
        {
            if (version == 4)
            {
                serviceCollection.AddTransient<IStatisticsBoard, StatisticsBoardSolr4>();
            }
            else if (version == 5)
            {
                //serviceCollection.AddTransient<IStatisticsBoard, StatisticsBoardSolr5>();
            }
            else if (version == 6)
            {
                serviceCollection.AddTransient<IStatisticsBoard, StatisticsBoardSolr6>();
            }

            serviceCollection.AddTransient<IQueryFactory, QueryFactory>();

            serviceCollection.AddMvcControllersInCurrentAssembly();
        }
    }
}