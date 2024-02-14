using EIRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using EIRS.BOL;
using EIRS.Common;
using EIRS.BLL;
using Elmah;

namespace EIRS.Web.Models
{
    public class LoginController : Controller
    {
        // GET: Login
        [HttpGet]
        public ActionResult Individual()
        {
            //Clearing Session Before Login
            //FormsAuthentication.SignOut();
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();

            TaxPayerPanelLoginViewModel mObjLoginViewModel = new TaxPayerPanelLoginViewModel()
            {
                returnUrl = Request.QueryString["returnUrl"] ?? ""
            };

            FormsAuthentication.SignOut();
            return View(mObjLoginViewModel);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Individual(TaxPayerPanelLoginViewModel pObjLoginModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjLoginModel);
            }
            else
            {
                Individual mObjIndividual = new Individual()
                {
                    IndividualRIN = pObjLoginModel.RIN,
                    MobileNumber1 = pObjLoginModel.RIN,
                    Password = EncryptDecrypt.Encrypt(pObjLoginModel.Password)
                };

                FuncResponse<Individual> mObjFuncResponse = new BLIndividual().BL_CheckIndividualLoginDetails(mObjIndividual);

                if (mObjFuncResponse.Success)
                {

                    SessionManager.RIN = mObjFuncResponse.AdditionalData.IndividualRIN;
                    SessionManager.TaxPayerID = mObjFuncResponse.AdditionalData.IndividualID;
                    SessionManager.ContactName = mObjFuncResponse.AdditionalData.FirstName + " " + mObjFuncResponse.AdditionalData.LastName;
                    SessionManager.ContactNumber = mObjFuncResponse.AdditionalData.MobileNumber1;
                    SessionManager.EmailAddress = mObjFuncResponse.AdditionalData.EmailAddress1;
                    SessionManager.TaxPayerTypeID = mObjFuncResponse.AdditionalData.TaxPayerTypeID.GetValueOrDefault();
                    SessionManager.UserTypeID = 0;

                    FormsAuthenticationTicket objTicket = null;
                    HttpCookie objCookie = null;
                    objTicket = new FormsAuthenticationTicket(1, SessionManager.UserID.ToString(), CommUtil.GetCurrentDateTime(), CommUtil.GetCurrentDateTime().AddMinutes(600), false, "Admin");
                    objCookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(objTicket));
                    Response.Cookies.Add(objCookie);

                    if (Url.IsLocalUrl(pObjLoginModel.returnUrl) && pObjLoginModel.returnUrl.Length > 1 && pObjLoginModel.returnUrl.StartsWith("/") && !pObjLoginModel.returnUrl.StartsWith("//") && !pObjLoginModel.returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(pObjLoginModel.returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Dashboard", "IndividualPanel");
                    }

                }
                else
                {
                    ViewBag.Message = mObjFuncResponse.Message;
                    return View(pObjLoginModel);
                }
            }
        }

        [HttpGet]
        public ActionResult Company()
        {
            //Clearing Session Before Login
            //FormsAuthentication.SignOut();
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();

            TaxPayerPanelLoginViewModel mObjLoginViewModel = new TaxPayerPanelLoginViewModel()
            {
                returnUrl = Request.QueryString["returnUrl"] ?? ""
            };

            FormsAuthentication.SignOut();
            return View(mObjLoginViewModel);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Company(TaxPayerPanelLoginViewModel pObjLoginModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjLoginModel);
            }
            else
            {
                Company mObjCompany = new Company()
                {
                    CompanyRIN = pObjLoginModel.RIN,
                    Password = EncryptDecrypt.Encrypt(pObjLoginModel.Password)
                };

                FuncResponse<Company> mObjFuncResponse = new BLCompany().BL_CheckCompanyLoginDetails(mObjCompany);

                if (mObjFuncResponse.Success)
                {

                    SessionManager.RIN = mObjFuncResponse.AdditionalData.CompanyRIN;
                    SessionManager.TaxPayerID = mObjFuncResponse.AdditionalData.CompanyID;
                    SessionManager.ContactName = mObjFuncResponse.AdditionalData.CompanyName;
                    SessionManager.ContactNumber = mObjFuncResponse.AdditionalData.MobileNumber1;
                    SessionManager.EmailAddress = mObjFuncResponse.AdditionalData.EmailAddress1;
                    SessionManager.TaxPayerTypeID = mObjFuncResponse.AdditionalData.TaxPayerTypeID.GetValueOrDefault();
                    SessionManager.UserTypeID = 0;

                    FormsAuthenticationTicket objTicket = null;
                    HttpCookie objCookie = null;
                    objTicket = new FormsAuthenticationTicket(1, SessionManager.UserID.ToString(), CommUtil.GetCurrentDateTime(), CommUtil.GetCurrentDateTime().AddMinutes(600), false, "Admin");
                    objCookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(objTicket));
                    Response.Cookies.Add(objCookie);

                    if (Url.IsLocalUrl(pObjLoginModel.returnUrl) && pObjLoginModel.returnUrl.Length > 1 && pObjLoginModel.returnUrl.StartsWith("/") && !pObjLoginModel.returnUrl.StartsWith("//") && !pObjLoginModel.returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(pObjLoginModel.returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Dashboard", "CorporatePanel");
                    }

                }
                else
                {
                    ViewBag.Message = mObjFuncResponse.Message;
                    return View(pObjLoginModel);
                }
            }
        }

        [HttpGet]
        public ActionResult Government()
        {
            //Clearing Session Before Login
            //FormsAuthentication.SignOut();
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();

            TaxPayerPanelLoginViewModel mObjLoginViewModel = new TaxPayerPanelLoginViewModel()
            {
                returnUrl = Request.QueryString["returnUrl"] ?? ""
            };

            FormsAuthentication.SignOut();
            return View(mObjLoginViewModel);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Government(TaxPayerPanelLoginViewModel pObjLoginModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjLoginModel);
            }
            else
            {
                Government mObjGovernment = new Government()
                {
                    GovernmentRIN = pObjLoginModel.RIN,
                    Password = EncryptDecrypt.Encrypt(pObjLoginModel.Password)
                };

                FuncResponse<Government> mObjFuncResponse = new BLGovernment().BL_CheckGovernmentLoginDetails(mObjGovernment);

                if (mObjFuncResponse.Success)
                {

                    SessionManager.RIN = mObjFuncResponse.AdditionalData.GovernmentRIN;
                    SessionManager.TaxPayerID = mObjFuncResponse.AdditionalData.GovernmentID;
                    SessionManager.ContactName = mObjFuncResponse.AdditionalData.GovernmentName;
                    SessionManager.ContactNumber = mObjFuncResponse.AdditionalData.ContactNumber;
                    SessionManager.EmailAddress = mObjFuncResponse.AdditionalData.ContactEmail;
                    SessionManager.TaxPayerTypeID = mObjFuncResponse.AdditionalData.TaxPayerTypeID.GetValueOrDefault();
                    SessionManager.UserTypeID = 0;

                    FormsAuthenticationTicket objTicket = null;
                    HttpCookie objCookie = null;
                    objTicket = new FormsAuthenticationTicket(1, SessionManager.UserID.ToString(), CommUtil.GetCurrentDateTime(), CommUtil.GetCurrentDateTime().AddMinutes(600), false, "Admin");
                    objCookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(objTicket));
                  
                    Response.Cookies.Add(objCookie);

                    if (Url.IsLocalUrl(pObjLoginModel.returnUrl) && pObjLoginModel.returnUrl.Length > 1 && pObjLoginModel.returnUrl.StartsWith("/") && !pObjLoginModel.returnUrl.StartsWith("//") && !pObjLoginModel.returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(pObjLoginModel.returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Dashboard", "GovernmentPanel");
                    }

                }
                else
                {
                    ViewBag.Message = mObjFuncResponse.Message;
                    return View(pObjLoginModel);
                }
            }
        }

        [HttpGet]
        public ActionResult AdminAccess()
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
        public ActionResult AdminAccess(LoginViewModel pObjLoginModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjLoginModel);
            }
            else
            {
                MST_Users mObjUser = new MST_Users()
                {
                    UserName = pObjLoginModel.EmailAddress,
                    Password = EncryptDecrypt.Encrypt(pObjLoginModel.Password),
                    UserTypeID = (int)EnumList.UserType.Admin
                };

                FuncResponse<MST_Users> mObjFuncResponse = new BLUser().BL_CheckUserLoginDetails(mObjUser);

                if (mObjFuncResponse.Success)
                {
                    SessionManager.UserID = mObjFuncResponse.AdditionalData.UserID;
                    SessionManager.ContactName = mObjFuncResponse.AdditionalData.ContactName;
                    SessionManager.ContactNumber = mObjFuncResponse.AdditionalData.ContactNumber;
                    SessionManager.EmailAddress = mObjFuncResponse.AdditionalData.EmailAddress;
                    SessionManager.UserTypeID = mObjFuncResponse.AdditionalData.UserTypeID.GetValueOrDefault();
                    //if (SessionManager.UserID == 32)
                    //    ViewBag.DetSideBar = "1";
                    FormsAuthenticationTicket objTicket = null;
                    HttpCookie objCookie = null;
                    objTicket = new FormsAuthenticationTicket(1, SessionManager.UserID.ToString(), CommUtil.GetCurrentDateTime(), CommUtil.GetCurrentDateTime().AddMinutes(600), false, "Admin");
                    objCookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(objTicket));
                    Response.Cookies.Add(objCookie);

                    IList<usp_GetScreenUserList_Result> lstScreens = new BLScreen().BL_GetScreenUserList(new MST_Screen() { UserID = mObjFuncResponse.AdditionalData.UserID });
                    SessionManager.LstScreensToSee = lstScreens.ToList();

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
                    ViewBag.Message = mObjFuncResponse.Message;
                    return View(pObjLoginModel);
                }
            }
        }

        [HttpGet]
        public ActionResult StaffAccess()
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
        public ActionResult StaffAccess(LoginViewModel pObjLoginModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjLoginModel);
            }
            else
            {
                MST_Users mObjUser = new MST_Users()
                {
                    UserName = pObjLoginModel.EmailAddress,
                    Password = EncryptDecrypt.Encrypt(pObjLoginModel.Password),
                    UserTypeID = (int)EnumList.UserType.Staff
                };

                FuncResponse<MST_Users> mObjFuncResponse = new BLUser().BL_CheckUserLoginDetails(mObjUser);

                if (mObjFuncResponse.Success)
                {

                    SessionManager.UserID = mObjFuncResponse.AdditionalData.UserID;
                    SessionManager.ContactName = mObjFuncResponse.AdditionalData.ContactName;
                    SessionManager.ContactNumber = mObjFuncResponse.AdditionalData.ContactNumber;
                    SessionManager.EmailAddress = mObjFuncResponse.AdditionalData.EmailAddress;
                    SessionManager.UserTypeID = mObjFuncResponse.AdditionalData.UserTypeID.GetValueOrDefault();

                    FormsAuthenticationTicket objTicket = null;
                    HttpCookie objCookie = null;
                    objTicket = new FormsAuthenticationTicket(1, SessionManager.UserID.ToString(), CommUtil.GetCurrentDateTime(), CommUtil.GetCurrentDateTime().AddMinutes(600), false, "Staff");
                    objCookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(objTicket));
                    Response.Cookies.Add(objCookie);
                    IList<usp_GetScreenUserList_Result> lstScreens = new BLScreen().BL_GetScreenUserList(new MST_Screen() { UserID = mObjFuncResponse.AdditionalData.UserID });
                    SessionManager.LstScreensToSee = lstScreens.ToList();

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
                    ViewBag.Message = mObjFuncResponse.Message;
                    return View(pObjLoginModel);
                }
            }
        }

        [HttpGet]
        public ActionResult PartnerAccess()
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
        public ActionResult PartnerAccess(LoginViewModel pObjLoginModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjLoginModel);
            }
            else
            {
                MST_Users mObjUser = new MST_Users()
                {
                    UserName = pObjLoginModel.EmailAddress,
                    Password = EncryptDecrypt.Encrypt(pObjLoginModel.Password),
                    UserTypeID = (int)EnumList.UserType.Partner
                };

                FuncResponse<MST_Users> mObjFuncResponse = new BLUser().BL_CheckUserLoginDetails(mObjUser);

                if (mObjFuncResponse.Success)
                {

                    SessionManager.UserID = mObjFuncResponse.AdditionalData.UserID;
                    SessionManager.ContactName = mObjFuncResponse.AdditionalData.ContactName;
                    SessionManager.ContactNumber = mObjFuncResponse.AdditionalData.ContactNumber;
                    SessionManager.EmailAddress = mObjFuncResponse.AdditionalData.EmailAddress;
                    SessionManager.UserTypeID = mObjFuncResponse.AdditionalData.UserTypeID.GetValueOrDefault();

                    FormsAuthenticationTicket objTicket = null;
                    HttpCookie objCookie = null;
                    objTicket = new FormsAuthenticationTicket(1, SessionManager.UserID.ToString(), CommUtil.GetCurrentDateTime(), CommUtil.GetCurrentDateTime().AddMinutes(600), false, "Partners");
                    objCookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(objTicket));
                    Response.Cookies.Add(objCookie);
                    IList<usp_GetScreenUserList_Result> lstScreens = new BLScreen().BL_GetScreenUserList(new MST_Screen() { UserID = mObjFuncResponse.AdditionalData.UserID });
                    SessionManager.LstScreensToSee = lstScreens.ToList();

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
                    ViewBag.Message = mObjFuncResponse.Message;
                    return View(pObjLoginModel);
                }
            }
        }

        [HttpGet]
        public ActionResult DataSubmitter()
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult DataSubmitter(LoginViewModel pObjLoginModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjLoginModel);
            }
            else
            {
                SFTP_DataSubmitter mObjDataSubmitter = new SFTP_DataSubmitter()
                {
                    UserName = pObjLoginModel.EmailAddress,
                    Password = EncryptDecrypt.Encrypt(pObjLoginModel.Password),
                };

                FuncResponse<SFTP_DataSubmitter> mObjFuncResponse = new BLSFTPDataSubmitter().BL_CheckUserLoginDetails(mObjDataSubmitter);

                if (mObjFuncResponse.Success)
                {

                    SessionManager.DataSubmitterID = mObjFuncResponse.AdditionalData.DataSubmitterID;
                    SessionManager.ContactName = mObjFuncResponse.AdditionalData.UserName;

                    FormsAuthenticationTicket objTicket = null;
                    HttpCookie objCookie = null;
                    objTicket = new FormsAuthenticationTicket(1, SessionManager.DataSubmitterID.ToString(), CommUtil.GetCurrentDateTime(), CommUtil.GetCurrentDateTime().AddMinutes(600), false, "DataSubmitter");
                    objCookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(objTicket));
                    Response.Cookies.Add(objCookie);
                    return RedirectToAction("Log", "DataSubmitterPanel");


                }
                else
                {
                    ViewBag.Message = mObjFuncResponse.Message;
                    return View(pObjLoginModel);
                }
            }
        }
    }
}