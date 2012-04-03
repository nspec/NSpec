using Orchard.UI.Resources;

namespace Heikura.Orchard.Modules.SyntaxHighlighter {
    public class ResourceManifest : IResourceManifestProvider {
        public const string NamePrefix = "sh_";

        public const string CoreScript = NamePrefix + "script_core";
        public const string CoreStyle = NamePrefix + "style_core";

        public const string ShAutoloaderScript = NamePrefix + "script_shloader";
        public const string AutoloaderScript = NamePrefix + "script_loader";

        // brushes
        public const string BrushCSharp = NamePrefix + "brush_csharp";
        public const string BrushJScript = NamePrefix + "brush_jscript";

        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();

            // core scripts
            manifest.DefineScript(AutoloaderScript).SetUrl("autoloader.js");
            manifest.DefineScript(CoreScript).SetUrl("shCore.js");
            manifest.DefineScript(ShAutoloaderScript).SetUrl("shAutoloader.js");

            // core styles
            manifest.DefineStyle(CoreStyle).SetUrl("shCore.css");

            // define styles
            manifest.DefineStyle("shThemeDefault.css").SetUrl("shThemeDefault.css");
            manifest.DefineStyle("shThemeDefault.css").SetUrl("shThemeDefault.css");
            manifest.DefineStyle("shThemeDjango.css").SetUrl("shThemeDjango.css");
            manifest.DefineStyle("shThemeEclipse.css").SetUrl("shThemeEclipse.css");
            manifest.DefineStyle("shThemeEmacs.css").SetUrl("shThemeEmacs.css");
            manifest.DefineStyle("shThemeFadeToGrey.css").SetUrl("shThemeFadeToGrey.css");
            manifest.DefineStyle("shThemeMDUltra.css").SetUrl("shThemeMDUltra.css");
            manifest.DefineStyle("shThemeMidnight.css").SetUrl("shThemeMidnight.css");
            manifest.DefineStyle("shThemeRDark.css").SetUrl("shThemeRDark.css");
    }
    }
}
