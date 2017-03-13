using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.SharedSource.SolrAnalyzer.Models;

namespace Sitecore.SharedSource.SolrAnalyzer.Factories
{
    public interface IQueryFactory
    {
        List<ISolrQuery> GetQueries();
    }
}
