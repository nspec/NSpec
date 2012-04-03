using Orchard.ContentManagement;

namespace Vandelay.Favicon.Models {
    public class FaviconSettingsPart : ContentPart<FaviconSettingsPartRecord> {
        public string FaviconUrl {
            get { return Record.FaviconUrl; }
            set { Record.FaviconUrl = value; }
        }
    }
}
