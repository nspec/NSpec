using Orchard.Data;
using Orchard.ContentManagement.Handlers;
using Vandelay.Favicon.Models;

namespace Vandelay.Favicon.Handlers {
    public class FaviconSettingsPartHandler : ContentHandler {
        public FaviconSettingsPartHandler(IRepository<FaviconSettingsPartRecord> repository) {
            Filters.Add(new ActivatingFilter<FaviconSettingsPart>("Site"));
            Filters.Add(StorageFilter.For(repository));
        }
    }
}