using System.Collections.Generic;
using System.Web.Mvc;
using Heikura.Orchard.Modules.SyntaxHighlighter.Models;
using Heikura.Orchard.Modules.SyntaxHighlighter.Services;
using Orchard;
using Orchard.Localization;
using Orchard.Themes;

namespace Heikura.Orchard.Modules.SyntaxHighlighter.Controllers
{
    public class AdminController : Controller {
        private readonly ISyntaxHighlighterService _syntaxHighlighterService;
        public IOrchardServices Services { get; set; }

        public AdminController(IOrchardServices services, ISyntaxHighlighterService syntaxHighlighterService) {
            _syntaxHighlighterService = syntaxHighlighterService;
            Services = services;
            T = NullLocalizer.Instance;
        }

        public Localizer T {
            get;
            set;
        }

        [HttpGet]
        public ActionResult ChangeTheme() {
            var themes = new List<string>();
            themes.AddRange(_syntaxHighlighterService.GetSupportedThemes());

            var currentTheme = _syntaxHighlighterService.GetCurrentTheme();

            var model = new IndexModel() {
                Theme = currentTheme,
                Themes = themes
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult ChangeTheme(IndexModel model) {
            if (!Services.Authorizer.Authorize(Permissions.ApplyTheme))
                return new HttpUnauthorizedResult();


            if (!ModelState.IsValid) {
                return RedirectToAction("ChangeTheme");
            }

            _syntaxHighlighterService.SetCurrentTheme(model.Theme);

            return RedirectToAction("ChangeTheme");
        }
    }
}