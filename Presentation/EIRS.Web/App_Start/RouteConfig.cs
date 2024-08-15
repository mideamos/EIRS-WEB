using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EIRS.Web
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("elmah.axd");

            routes.MapRoute(
                name: "DefaultController",
                url: "{action}.aspx",
                defaults: new { controller = "Default", action = "Home" },
                namespaces: new string[] { "EIRS.Web.Controllers" }
            );

            //routes.MapRoute(
            //   name: "LoginUrl",
            //   url: "Login.aspx",
            //   defaults: new { controller = "Default", action = "Login" }
            //   );

            routes.MapRoute(
              name: "DefaultPage",
              url: "",
              defaults: new { controller = "Default", action = "Home" }
              );

            routes.MapRoute(
               name: "ED1",
               url: "{controller}/{action}/{id}/{name}",
               defaults: new { id = UrlParameter.Optional, name = UrlParameter.Optional }
           );

            // routes.MapRoute(
            //    name: "TEST1",
            //    url: "{controller}/{action}/{id}/{name}&aid={aid}",
            //    defaults: new { id = UrlParameter.Optional, name = UrlParameter.Optional, aid = UrlParameter.Optional }
            //);

            // routes.MapRoute(
            //    name: "TEST2",
            //    url: "{controller}/{action}/{id}/{name}&billid={billid}&billrefno={billrefno}",
            //    defaults: new { id = UrlParameter.Optional, name = UrlParameter.Optional, billid = UrlParameter.Optional, billrefno = UrlParameter.Optional }
            //);

            // New route for RevenueStreamByTaxOfficeTargetDetails
            routes.MapRoute(
                name: "RevenueStreamDetails",
                url: "OperationManager/RevenueStreamByTaxOfficeTargetDetails/{RevenueStreamID}/{Year}/{Month}/{taxofficeId}",
                defaults: new { controller = "OperationManager", action = "RevenueStreamByTaxOfficeTargetDetails", RevenueStreamID = UrlParameter.Optional, Year = UrlParameter.Optional, Month = UrlParameter.Optional, taxofficeId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "taxOfficeTargetByMonthDetails",
                url: "OperationManager/TaxOfficeTargetByMonthDetails/{Year}/{Month}",
                defaults: new { controller = "OperationManager", action = "TaxOfficeTargetByMonthDetails", Year = UrlParameter.Optional, Month = UrlParameter.Optional }
            );


            //routes.MapRoute(
            //    name: "LA1",
            //    url: "{controller}/{action}.aspx",
            //    defaults: new { action = "List" }
            //);
        }
    }
}
