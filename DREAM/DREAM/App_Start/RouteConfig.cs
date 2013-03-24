using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DREAM
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("UsersAdmin/{*pathInfo}");
            routes.IgnoreRoute("LogsAdmin/{*pathInfo}");

            routes.MapRoute(
                name: "UsersAdmin",
                url: "Admin/Users/{action}/{username}",
                defaults: new { controller = "UsersAdmin", action = "Index", username = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "LogsAdmin",
                url: "Admin/Logs",
                defaults: new { controller = "LogsAdmin", action = "Index" }
            );
            routes.MapRoute(
                name: "Search",
                url: "Search",
                defaults: new { controller = "Requests", action = "Search" }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            // Must be last
            routes.MapRoute(
                name: "PageNotFound",
                url: "{*catchall}",
                defaults: new { controller = "Home", action = "PageNotFound" }
            );
        }
    }
}