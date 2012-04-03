using System.Web.Mvc;
using Contrib.GoogleAnalytics.Services;
using Contrib.GoogleAnalytics.ViewModels;
using Orchard;
using Orchard.Localization;
using Orchard.Security;
using Orchard.UI.Admin;
using Orchard.UI.Notify;

namespace Contrib.GoogleAnalytics.Controllers
{
    [ValidateInput(false), Admin]
    public class AdminController : Controller {
        private readonly ISettingsService _settingsService;

        public AdminController(ISettingsService settingsService, IOrchardServices orchardServices) {
            _settingsService = settingsService;
            Services = orchardServices;
            T = NullLocalizer.Instance;
        }

        public IOrchardServices Services { get; set; }
        public Localizer T { get; set; }

        public ActionResult Index() {
            if (!Services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Cannot manage analytics")))
                return new HttpUnauthorizedResult();

            var siteKey = _settingsService.Get();

            var viewModel = new AdminIndexViewModel {
                                    Enable = siteKey.Enable,
                                    Script = siteKey.Script
                                };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(AdminIndexViewModel viewModel) {
            if (!Services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Cannot manage analytics")))
                return new HttpUnauthorizedResult();

            if (_settingsService.Set(viewModel.Enable, viewModel.Script)) {
                Services.Notifier.Information(T("Google Analytics settings successfully saved"));
            }

            return View(viewModel);
        }
    }
}