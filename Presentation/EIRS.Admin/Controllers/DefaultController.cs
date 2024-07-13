using System.Web;
using System.Web.Mvc;
using EIRS.Models;
using System.Web.Security;
using EIRS.BOL;
using EIRS.Common;
using EIRS.BLL;
using Elmah;

namespace EIRS.Admin.Controllers
{
    public class DefaultController : Controller
    {
        public ActionResult Login()
        {
            //Clearing Session Before Login
            //FormsAuthentication.SignOut();
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();

            LoginViewModel mObjLoginViewModel = new LoginViewModel()
            {
                returnUrl = Request.QueryString["returnUrl"] ?? ""
            };

            FormsAuthentication.SignOut();
            return View(mObjLoginViewModel);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel pObjLoginModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjLoginModel);
            }
            else
            {
                SystemUser mObjSystemUser = new SystemUser()
                {
                    UserLogin = pObjLoginModel.EmailAddress,
                    UserPassword = EncryptDecrypt.Encrypt(pObjLoginModel.Password)
                };

                FuncResponse<SystemUser> mObjResponse = new BLSystemUser().BL_CheckSystemUserLoginDetails(mObjSystemUser);

                if (mObjResponse.Success)
                {
                   
                    SessionManager.SystemUserID = mObjResponse.AdditionalData.SystemUserID;
                    SessionManager.SystemRoleID = mObjResponse.AdditionalData.SystemRoleID.GetValueOrDefault();
                    SessionManager.UserName = mObjResponse.AdditionalData.SystemUserName;
                    SessionManager.UserEmail = mObjResponse.AdditionalData.UserLogin;

                    //FormsAuthenticationTicket objTicket = null;
                    //HttpCookie objCookie = null;
                    //objTicket = new FormsAuthenticationTicket(1, SessionManager.SystemUserID.ToString(), CommUtil.GetCurrentDateTime(), CommUtil.GetCurrentDateTime().AddMinutes(600), false, "admin");
                    //objCookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(objTicket));
                    //Response.Cookies.Add(objCookie);

                    if (Url.IsLocalUrl(pObjLoginModel.returnUrl) && pObjLoginModel.returnUrl.Length > 1 && pObjLoginModel.returnUrl.StartsWith("/") && !pObjLoginModel.returnUrl.StartsWith("//") && !pObjLoginModel.returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(pObjLoginModel.returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Dashboard", "Home");
                    }

                }
                else
                {
                    ViewBag.Message = mObjResponse.Message;
                    return View(pObjLoginModel);
                }
            }
        }

        public ActionResult PageNotFound()
        {
            return View();
        }
    }
}

