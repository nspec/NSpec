using System;
using System.Collections.Generic;
using System.Linq;
using Orchard.Caching;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Localization;
using Orchard.Media.Services;
using Vandelay.Favicon.Models;
using Vandelay.Favicon.ViewModels;

namespace Vandelay.Favicon.Drivers {
    public class FaviconSettingsPartDriver : ContentPartDriver<FaviconSettingsPart> {
        private const string _faviconMediaFolder = "favicon";
        private readonly IMediaService _mediaService;
        private readonly ISignals _signals;

        public FaviconSettingsPartDriver(IMediaService mediaService, ISignals signals) {
            T = NullLocalizer.Instance;
            _mediaService = mediaService;
            _signals = signals;
        }

        public Localizer T { get; set; }

        protected override string Prefix { get { return "FaviconSettings"; } }

        protected override DriverResult Editor(FaviconSettingsPart part, dynamic shapeHelper) {
            List<string> faviconSuggestions = null;
            var rootMediaFolders = _mediaService
                .GetMediaFolders(".")
                .Where(f => f.Name.Equals(_faviconMediaFolder, StringComparison.OrdinalIgnoreCase));
            if (rootMediaFolders.Any()) {
                faviconSuggestions = new List<string>(
                    _mediaService.GetMediaFiles(_faviconMediaFolder)
                    .Select(f => _mediaService.GetPublicUrl(_faviconMediaFolder + "/" + f.Name)));
            }

            return ContentShape("Parts_Favicon_SiteSettings",
                               () => shapeHelper.EditorTemplate(
                                   TemplateName: "Parts.Favicon.SiteSettings",
                                   Model: new FaviconSettingsViewModel {
                                       FaviconUrl = part.Record.FaviconUrl,
                                       FaviconUrlSuggestions = faviconSuggestions
                                   },
                                   Prefix: Prefix));
        }

        protected override DriverResult Editor(FaviconSettingsPart part, IUpdateModel updater, dynamic shapeHelper) {
            updater.TryUpdateModel(part.Record, Prefix, null, null);
            _signals.Trigger("Vandelay.Favicon.Changed");
            return Editor(part, shapeHelper);
        }
    }
}