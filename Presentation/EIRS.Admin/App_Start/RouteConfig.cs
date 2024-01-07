using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EIRS.Admin
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.LowercaseUrls = true;

            routes.MapRoute(
               name: "PageNotFound",
               url: "404.aspx",
               defaults: new { controller = "Default", action = "PageNotFound" }
               ).RouteHandler = new DashRouteHandler();

            routes.MapRoute(
                name: "LoginUrl",
                url: "Login.aspx",
                defaults: new { controller = "Default", action = "Login" }
                ).RouteHandler = new DashRouteHandler();

            routes.MapRoute(
              name: "DefaultPage",
              url: "",
              defaults: new { controller = "Default", action = "Login" }
              ).RouteHandler = new DashRouteHandler();

            routes.MapRoute(
               name: "ED1",
               url: "{controller}/{action}/{id}/{name}",
               defaults: new { id = UrlParameter.Optional, name = UrlParameter.Optional }
           ).RouteHandler = new DashRouteHandler();


            routes.MapRoute(
                name: "LA1",
                url: "{controller}/{action}.aspx",
                defaults: new { action = "List" }
            ).RouteHandler = new DashRouteHandler();

            routes.MapRoute(
                name: "LA2",
                url: "{controller}",
                defaults: new { action = "List" }
            ).RouteHandler = new DashRouteHandler();

            routes.MapRoute("Default", "{controller}/{action}/{id}", new
            {
                controller = "Home",
                action = "Index",
                id = UrlParameter.Optional
            }).RouteHandler = new DashRouteHandler();
        }
    }
}
