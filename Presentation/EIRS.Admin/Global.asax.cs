using EIRS.Common;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;

namespace EIRS.Admin
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_BeginRequest()
        {
            CultureInfo info = new CultureInfo(System.Threading.Thread.CurrentThread.CurrentCulture.ToString());
            info.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            System.Threading.Thread.CurrentThread.CurrentCulture = info;
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            AntiForgeryConfig.CookieName = "__EIRSWORKBENCH_RequestVerificationToken" + "_" + HttpServerUtility.UrlTokenEncode(Encoding.UTF8.GetBytes("/shared-secured"));
            AntiForgeryConfig.UniqueClaimTypeIdentifier = "urn:eirsworkbenchidentityclaim";
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
