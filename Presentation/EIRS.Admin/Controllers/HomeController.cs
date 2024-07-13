using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Elmah;

namespace EIRS.Admin.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Home
        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            return Redirect("/");
        }

        public ActionResult PageNotFound()
        {
            return View();
        }
    }
}