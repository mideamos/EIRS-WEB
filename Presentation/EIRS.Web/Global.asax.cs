using System;
using System.Globalization;
using System.Web.Routing;

namespace EIRS.Web
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
            //AntiForgeryConfig.SuppressIdentityHeuristicChecks = true;
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        //void Session_Start(object sender, EventArgs e)
        //{
        //    // Code that runs when a new session is started    
        //    if (Session.SessionID == null)
        //    {
        //        //http://localhost:56883/Login/Individual
        //        //Redirect to Welcome Page if Session is not null
        //        Response.Redirect("Login/Individual");
        //    }
        //    else
        //    {
        //        //Redirect to Login Page if Session is null & Expires     
        //        Response.Redirect("Home/DashBoard");

        //    }
        //}


    }
}
