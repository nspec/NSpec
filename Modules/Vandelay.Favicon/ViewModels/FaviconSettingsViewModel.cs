using System.Collections.Generic;

namespace Vandelay.Favicon.ViewModels {
    public class FaviconSettingsViewModel {
        public string FaviconUrl { get; set; }
        public IEnumerable<string> FaviconUrlSuggestions { get; set; }
    }
}