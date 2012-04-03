using System.Web.Mvc;
using Heikura.Orchard.Modules.SyntaxHighlighter.Services;
using Orchard.Environment.Extensions;
using Orchard.Mvc.Filters;
using Orchard.UI.Resources;

namespace Heikura.Orchard.Modules.SyntaxHighlighter.Filters {
    [OrchardFeature("Heikura.SyntaxHighlighter.ResultFilter")]
    public class SyntaxHighlighterFilter : FilterProvider, IResultFilter {
        private readonly IResourceManager _resourceManager;
        private readonly ISyntaxHighlighterService _syntaxHighlighterService;

        public SyntaxHighlighterFilter(IResourceManager resourceManager, ISyntaxHighlighterService syntaxHighlighterService) {
            _resourceManager = resourceManager;
            _syntaxHighlighterService = syntaxHighlighterService;
        }

        public void OnResultExecuting(ResultExecutingContext filterContext) {
            var viewResult = filterContext.Result as ViewResult;
            if (viewResult == null)
                return;

            _resourceManager.Require("stylesheet", ResourceManifest.CoreStyle).AtHead();

            // todo: (pekkah) read the theme from configuration (needs a UI)
            var currentTheme = _syntaxHighlighterService.GetCurrentTheme();
            _resourceManager.Require("stylesheet", currentTheme).AtHead();
            
            var coreScriptRequire = _resourceManager.Require("script", ResourceManifest.CoreScript).AtHead();
            _resourceManager.Require("script", ResourceManifest.ShAutoloaderScript).AtHead();
            _resourceManager.Require("script", ResourceManifest.AutoloaderScript).AtHead();

            // todo: (pekkah) there probably  is a much better way of getting the scripts path ?
            var coreScriptResource = _resourceManager.FindResource(coreScriptRequire);
            var appPath = filterContext.HttpContext.Request.ApplicationPath;
            var coreScriptPath = coreScriptResource.ResolveUrl(coreScriptRequire, appPath).Replace("shCore.js", "");

            // this will activate syntax highlighter using shAutoloader to dynamically load needed brushes.
            _resourceManager.RegisterFootScript(
                "<script type=\"text/javascript\">syntaxHighlight('$scripts$')</script>".Replace("$scripts$", 
                coreScriptPath));
        }

        public void OnResultExecuted(ResultExecutedContext filterContext) {
        }
    }
}