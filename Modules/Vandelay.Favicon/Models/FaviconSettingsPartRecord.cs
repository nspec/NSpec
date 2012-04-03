using Orchard.ContentManagement.Records;

namespace Vandelay.Favicon.Models {
    public class FaviconSettingsPartRecord : ContentPartRecord {
        public virtual string FaviconUrl { get; set; }
    }
}