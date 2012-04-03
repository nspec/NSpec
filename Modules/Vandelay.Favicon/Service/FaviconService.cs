using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard;
using Orchard.Caching;
using Vandelay.Favicon.Models;

namespace Vandelay.Favicon.Service {
    public interface IFaviconService : IDependency {
        string GetFaviconUrl();
    }

    public class FaviconService : IFaviconService {
        private readonly IWorkContextAccessor _wca;
        private readonly ICacheManager _cacheManager;
        private readonly ISignals _signals;

        public FaviconService(IWorkContextAccessor wca, ICacheManager cacheManager, ISignals signals) {
            _wca = wca;
            _cacheManager = cacheManager;
            _signals = signals;
        }

        public string GetFaviconUrl() {
            return _cacheManager.Get(
                "Vandelay.Favicon.Url",
                ctx => {
                    ctx.Monitor(_signals.When("Vandelay.Favicon.Changed"));
                    WorkContext workContext = _wca.GetContext();
                    var faviconSettings =
                        (FaviconSettingsPart) workContext
                                                  .CurrentSite
                                                  .ContentItem
                                                  .Get(typeof (FaviconSettingsPart));
                    return faviconSettings.FaviconUrl;
                });
        }
    }
}