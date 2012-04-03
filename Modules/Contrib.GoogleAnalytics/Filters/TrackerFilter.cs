using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Contrib.GoogleAnalytics.Services;
using Orchard;
using Orchard.Caching;
using Orchard.Mvc.Filters;
using Orchard.UI.Admin;

namespace Contrib.GoogleAnalytics.Filters {
    public class TrackerFilter : FilterProvider, IResultFilter {
        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly ISettingsService _settingsService;
        private readonly ICacheManager _cacheManager;
        private readonly ISignals _signals;

        public TrackerFilter(
            IWorkContextAccessor workContextAccessor,
            ISettingsService settingsService,
            ICacheManager cacheManager,
            ISignals signals) {
            _workContextAccessor = workContextAccessor;
            _settingsService = settingsService;
            _cacheManager = cacheManager;
            _signals = signals;
        }

        public void OnResultExecuting(ResultExecutingContext filterContext) {
            // ignore tracker on admin pages
            if (AdminFilter.IsApplied(filterContext.RequestContext)) {
                return;
            }

            // should only run on a full view rendering result
            if (!(filterContext.Result is ViewResult))
                return;

            var script = _cacheManager.Get("GoogleAnalytics.Settings",
                              ctx => {
                                  ctx.Monitor(_signals.When("GoogleAnalytics.SettingsChanged"));
                                  var settings = _settingsService.Get();
                                  return !settings.Enable ? null : settings.Script;
                              });

            if(String.IsNullOrEmpty(script)) {
                return;
            }

            var context = _workContextAccessor.GetContext();
            var tail = context.Layout.Tail;
            tail.Add(new MvcHtmlString(script));
        }

        public void OnResultExecuted(ResultExecutedContext filterContext) {
        }
    }
}