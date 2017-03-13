using System;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.Sheer;

namespace Sitecore.SharedSource.SolrAnalyzer.Commands
{
    [Serializable]
    public class QueryAnalyzer : Command
    {
        protected static readonly string idParam = "idValue";
        protected static readonly string heightParam = "heightValue";
        protected static readonly string widthParam = "widthValue";
        protected static readonly string languageParam = "language";

        public override void Execute(CommandContext context)
        {
            Sitecore.Context.ClientPage.Start(this, "Run", context.Parameters);
        }

        protected void Run(ClientPipelineArgs args)
        {
            if (args.IsPostBack)
                return;

            string height = args.Parameters[heightParam];
            string width = args.Parameters[widthParam];

            ModalDialogOptions mdo = new ModalDialogOptions($"/SolrAnalyzer/Query/QueryAnalysis")
            {
                Header = "Query Analyzer Analysis",
                Height = height,
                Width = width,
                Message = "Solr Query Analysis",
                Response = true
            };
            SheerResponse.ShowModalDialog(mdo);
            args.WaitForPostBack();
        }
    }
}