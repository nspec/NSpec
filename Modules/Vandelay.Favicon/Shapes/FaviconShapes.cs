using System.Linq;
using Orchard;
using Orchard.DisplayManagement.Descriptors;
using Orchard.UI.Resources;
using Vandelay.Favicon.Models;
using Vandelay.Favicon.Service;

namespace Vandelay.Favicon.Shapes {
    public class FaviconShapes : IShapeTableProvider {
        private readonly IWorkContextAccessor _wca;
        private readonly IFaviconService _faviconService;

        public FaviconShapes(IWorkContextAccessor wca, IFaviconService faviconService) {
            _wca = wca;
            _faviconService = faviconService;
        }

        public void Discover(ShapeTableBuilder builder) {
            builder.Describe("HeadLinks")
                .OnDisplaying(shapeDisplayingContext => {
                    string faviconUrl = _faviconService.GetFaviconUrl();
                    if (!string.IsNullOrWhiteSpace(faviconUrl)) {
                        // Get the current favicon from head
                        var resourceManager = _wca.GetContext().Resolve<IResourceManager>();
                        var links = resourceManager.GetRegisteredLinks();
                        var currentFavicon = links
                            .Where(l => l.Rel == "shortcut icon" && l.Type == "image/x-icon")
                            .FirstOrDefault();
                        // Modify if found
                        if (currentFavicon != default(LinkEntry)) {
                            currentFavicon.Href = faviconUrl;
                        }
                        else {
                            // Add the new one
                            resourceManager.RegisterLink(new LinkEntry {
                                Type = "image/x-icon",
                                Rel = "shortcut icon",
                                Href = faviconUrl
                            });
                        }
                    }
                });
        }
    }
}