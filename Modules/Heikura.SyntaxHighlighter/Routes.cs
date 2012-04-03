using Orchard.Mvc.Routes;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

namespace Heikura.Orchard.Modules.SyntaxHighlighter
{
    public class Routes : IRouteProvider
    {
        public IEnumerable<RouteDescriptor> GetRoutes() {
            return new[]
                {
                    new RouteDescriptor()
                        {
                            Route = new Route(
                                "Admin/Themes/SyntaxHighlighter/ChangeTheme",
                                new RouteValueDictionary {
                                                            {"area", "Heikura.SyntaxHighlighter"},
                                                            {"controller", "Admin"},
                                                            {"action", "ChangeTheme"}
                                                        },
                                new RouteValueDictionary(),
                                new RouteValueDictionary {
                                                            {"area", "Heikura.SyntaxHighlighter"}
                                                        },
                                new MvcRouteHandler())
                        }
                };
        }

        public void GetRoutes(ICollection<RouteDescriptor> routes) {
            foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);
        }
    }
}