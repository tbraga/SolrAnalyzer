using System;
using System.Web.Mvc;
using NSubstitute;
using NUnit.Framework;
using Sitecore.SharedSource.CognitiveServices.Controllers;
using Sitecore.SharedSource.CognitiveServices.Factories;
using Sitecore.SharedSource.CognitiveServices.Foundation;
using Sitecore.SharedSource.CognitiveServices.Search;

namespace Sitecore.SharedSource.CognitiveServices.Tests.Controllers
{
    [TestFixture]
    public class CognitiveAnalysisControllerTests
    {
        private ICognitiveSearchContext Searcher;
        private ICognitiveImageAnalysisFactory ImageAnalysisFactory;
        private ICognitiveTextAnalysisFactory TextAnalysisFactory;
        private IReanalyzeAllFactory ReanalyzeAllFactory;
        private ISitecoreDataService DataService;

        [SetUp]
        public void Setup()
        {
            Searcher = Substitute.For<ICognitiveSearchContext>();
            ImageAnalysisFactory = Substitute.For<ICognitiveImageAnalysisFactory>();
            TextAnalysisFactory = Substitute.For<ICognitiveTextAnalysisFactory>();
            ReanalyzeAllFactory = Substitute.For<IReanalyzeAllFactory>();
            DataService = Substitute.For<ISitecoreDataService>();
        }

        [Test]
        public void Constructor_NullParameters_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => new CognitiveAnalysisController(null, ImageAnalysisFactory, TextAnalysisFactory, ReanalyzeAllFactory, DataService));
            Assert.Throws<InvalidOperationException>(() => new CognitiveAnalysisController(Searcher, null, TextAnalysisFactory, ReanalyzeAllFactory, DataService));
            Assert.Throws<InvalidOperationException>(() => new CognitiveAnalysisController(Searcher, ImageAnalysisFactory, null, ReanalyzeAllFactory, DataService));
            Assert.Throws<InvalidOperationException>(() => new CognitiveAnalysisController(Searcher, ImageAnalysisFactory, TextAnalysisFactory, null, DataService));
            Assert.Throws<InvalidOperationException>(() => new CognitiveAnalysisController(Searcher, ImageAnalysisFactory, TextAnalysisFactory, ReanalyzeAllFactory, null));
        }

        [Test]
        public void ID_Empty_Returns_NullModel()
        {
            //arrange
            CognitiveAnalysisController controller = new CognitiveAnalysisController(Searcher, ImageAnalysisFactory, TextAnalysisFactory, ReanalyzeAllFactory, DataService);

            //act
            var result = controller.Reanalyze(string.Empty, "en", "master") as ViewResult;

            //assert
            Assert.IsNotNull(result);
            Assert.IsNull(result.Model);
        }

        [Test]
        public void ValidID_Returns_NoChoices()
        {
            //arrange
            CognitiveAnalysisController controller = new CognitiveAnalysisController(Searcher, ImageAnalysisFactory, TextAnalysisFactory, ReanalyzeAllFactory, DataService);

            //act
            var result = controller.Reanalyze(string.Empty, "en", "master") as ViewResult;

            //assert
            Assert.IsNotNull(result);
            Assert.IsNull(result.Model);
        }
    }
}
