using Orchard.Localization;
using Orchard.Security;
using Orchard.UI.Navigation;

namespace Contrib.GoogleAnalytics {
    public class AdminMenu : INavigationProvider {
        public Localizer T { get; set; }
        public string MenuName { get { return "admin"; } }

        public void GetNavigation(NavigationBuilder builder) {
            builder.Add(T("Analytics"), "4",
                        menu => menu
                                    .Add(T("Google Analytics"), "1.0", item => item.Action("Index", "Admin", new { area = "Contrib.GoogleAnalytics" }).Permission(StandardPermissions.SiteOwner))
                                    );
        }
    }
}
