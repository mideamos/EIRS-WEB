using System.Collections.Generic;
using System.Web.Mvc;
using EIRS.BOL;
using EIRS.BLL;
using EIRS.Common;
using System;
using EIRS.Web.Models;
using Elmah;
using Vereyon.Web;
using EIRS.Models;
using System.IO;
using static EIRS.Web.Controllers.Filters;
using EIRS.Web.Utility;

namespace EIRS.Web.Controllers
{
    [SessionTimeout]
    public class UserController : BaseController
    {
        BLUser mObjBLUser;

        public ActionResult List()
        {
            IList<DropDownListResult> lstUserType = new BLCommon().BL_GetUserTypeDropDownList();
            ViewBag.UserTypeList = new SelectList(lstUserType, "id", "text");

            IList<SelectListItem> lstStatus = new List<SelectListItem>
            {
                new SelectListItem() { Value = "2", Text = "All" },
                new SelectListItem() { Value = "1", Text = "Active", Selected = true },
                new SelectListItem() { Value = "0", Text = "In Active" }
            };

            ViewBag.StatusList = lstStatus;

            MST_Users mObjUsers = new MST_Users()
            {
                intStatus = 1
            };

            if (mObjBLUser == null)
                mObjBLUser = new BLUser();

            IList<usp_GetUserList_Result> lstUsers = mObjBLUser.BL_GetUserList(mObjUsers);

            return View(lstUsers);
        }

        [HttpPost]
        public ActionResult List(FormCollection p_ObjFormCollection)
        {
            int mIntUserTypeID = TrynParse.parseInt(p_ObjFormCollection.Get("cboUserType"));
            int mIntStatus = TrynParse.parseInt(p_ObjFormCollection.Get("cboStatus"));

            MST_Users mObjUsers = new MST_Users()
            {
                intStatus = mIntStatus,
                UserTypeID = mIntUserTypeID
            };

            if (mObjBLUser == null)
                mObjBLUser = new BLUser();

            IList<usp_GetUserList_Result> lstUsers = mObjBLUser.BL_GetUserList(mObjUsers);
            return PartialView("SearchData", lstUsers);
        }

        private void UI_FillDropDown()
        {
            IList<DropDownListResult> lstUserType = new BLCommon().BL_GetUserTypeDropDownList();
            ViewBag.UserTypeList = new SelectList(lstUserType, "id", "text");

            UI_FillTaxOfficeDropDown();

            IList<DropDownListResult> lstTaxOfficeManager = new BLCommon().BL_GetTaxOfficeManagerList();
            ViewBag.ManagerList = new SelectList(lstTaxOfficeManager, "id", "text");
        }

        public ActionResult Add()
        {
            UI_FillDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(UserViewModel pObjUserModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown();
                return View(pObjUserModel);
            }
            else
            {
                try
                {
                    string strSignaturePath = "";

                    if (pObjUserModel.SignatureFileUpload != null && pObjUserModel.SignatureFileUpload.ContentLength > 0)
                    {
                        string strDirectory = GlobalDefaultValues.DocumentLocation + "Staff/Signatures/";
                        string mstrFileName = DateTime.Now.ToString("ddMMyyyyhhmmss_") + Path.GetFileName(pObjUserModel.SignatureFileUpload.FileName);
                        if (!Directory.Exists(strDirectory))
                            Directory.CreateDirectory(strDirectory);
                        string mStrSignaturePath = Path.Combine(strDirectory, mstrFileName);
                        pObjUserModel.SignatureFileUpload.SaveAs(mStrSignaturePath);

                        strSignaturePath = "Staff/Signatures/" + mstrFileName;
                    }

                    MST_Users mObjUsers = new MST_Users()
                    {
                        UserID = 0,
                        UserName = pObjUserModel.UserName,
                        ContactName = pObjUserModel.ContactName,
                        UserTypeID = pObjUserModel.UserTypeID,
                        ContactNumber = pObjUserModel.ContactNumber,
                        EmailAddress = pObjUserModel.Email,
                        Password = EncryptDecrypt.Encrypt(pObjUserModel.Password),
                        IsTOManager = pObjUserModel.IsTOManager,
                        TaxOfficeID = pObjUserModel.TaxOfficeID,
                        TOManagerID = pObjUserModel.TOManagerID,
                        IsDirector = pObjUserModel.IsDirector,
                        SignaturePath = strSignaturePath,
                        Active = true,
                        CreatedBy = SessionManager.UserID,
                        CreatedDate = CommUtil.GetCurrentDateTime()
                    };

                    if (mObjBLUser == null)
                        mObjBLUser = new BLUser();

                    FuncResponse mObjResponse = mObjBLUser.BL_InsertUpdateUser(mObjUsers);

                    if (mObjResponse.Success)
                    {
                        Audit_Log mObjAuditLog = new Audit_Log()
                        {
                            LogDate = CommUtil.GetCurrentDateTime(),
                            ASLID = (int)EnumList.ALScreen.Admin_Users_Add,
                            Comment = $"New User Added - {pObjUserModel.ContactName}",
                            IPAddress = CommUtil.GetIPAddress(),
                            StaffID = SessionManager.UserID,
                        };

                        new BLAuditLog().BL_InsertAuditLog(mObjAuditLog);

                        return RedirectToAction("List", "User");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjUserModel);
                    }
                }
                catch (Exception ex)
                {
                    Logger.SendErrorToText(ex);
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving users";
                    return View(pObjUserModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                MST_Users mObjUsers = new MST_Users()
                {
                    UserID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                if (mObjBLUser == null)
                    mObjBLUser = new BLUser();

                usp_GetUserList_Result mObjUserData = mObjBLUser.BL_GetUserDetails(mObjUsers);

                if (mObjUserData != null)
                {
                    UserViewModel mObjUserModel = new UserViewModel()
                    {
                        UserID = mObjUserData.UserID.GetValueOrDefault(),
                        UserName = mObjUserData.UserName,
                        ContactName = mObjUserData.ContactName,
                        UserTypeID = mObjUserData.UserTypeID.GetValueOrDefault(),
                        ContactNumber = mObjUserData.ContactNumber,
                        Email = mObjUserData.EmailAddress,
                        Password = EncryptDecrypt.Decrypt(mObjUserData.Password),
                        ConfirmPassword = EncryptDecrypt.Decrypt(mObjUserData.Password),
                        Active = mObjUserData.Active.GetValueOrDefault(),
                        IsTOManager = mObjUserData.IsTOManager.GetValueOrDefault(),
                        IsDirector = mObjUserData.IsDirector.GetValueOrDefault(),
                        TaxOfficeID = mObjUserData.TaxOfficeID.GetValueOrDefault(),
                        TOManagerID = mObjUserData.TOManagerID.GetValueOrDefault(),
                        SignatureFilePath = mObjUserData.SignaturePath,
                    };

                    UI_FillDropDown();
                    return View(mObjUserModel);
                }
                else
                {
                    return RedirectToAction("List", "User");
                }
            }
            else
            {
                return RedirectToAction("List", "User");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(UserViewModel pObjUserModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown();
                return View(pObjUserModel);
            }
            else
            {
                try
                {
                    string strSignaturePath = pObjUserModel.SignatureFilePath;

                    if (pObjUserModel.SignatureFileUpload != null && pObjUserModel.SignatureFileUpload.ContentLength > 0)
                    {
                        string strDirectory = GlobalDefaultValues.DocumentLocation + "Staff/Signatures/";
                        string mstrFileName = DateTime.Now.ToString("ddMMyyyyhhmmss_") + Path.GetFileName(pObjUserModel.SignatureFileUpload.FileName);
                        if (!Directory.Exists(strDirectory))
                            Directory.CreateDirectory(strDirectory);
                        string mStrSignaturePath = Path.Combine(strDirectory, mstrFileName);
                        pObjUserModel.SignatureFileUpload.SaveAs(mStrSignaturePath);

                        strSignaturePath = "Staff/Signatures/" + mstrFileName;
                    }

                    MST_Users mObjUsers = new MST_Users()
                    {
                        UserID = pObjUserModel.UserID,
                        UserName = pObjUserModel.UserName,
                        ContactName = pObjUserModel.ContactName,
                        UserTypeID = pObjUserModel.UserTypeID,
                        ContactNumber = pObjUserModel.ContactNumber,
                        EmailAddress = pObjUserModel.Email,
                        Password = EncryptDecrypt.Encrypt(pObjUserModel.Password),
                        IsTOManager = pObjUserModel.IsTOManager,
                        IsDirector = pObjUserModel.IsDirector,
                        TaxOfficeID = pObjUserModel.TaxOfficeID,
                        TOManagerID = pObjUserModel.TOManagerID,
                        SignaturePath = strSignaturePath,
                        Active = pObjUserModel.Active,
                        CreatedBy = SessionManager.UserID,
                        CreatedDate = CommUtil.GetCurrentDateTime()
                    };

                    if (mObjBLUser == null)
                        mObjBLUser = new BLUser();

                    FuncResponse mObjResponse = mObjBLUser.BL_InsertUpdateUser(mObjUsers);

                    if (mObjResponse.Success)
                    {
                        Audit_Log mObjAuditLog = new Audit_Log()
                        {
                            LogDate = CommUtil.GetCurrentDateTime(),
                            ASLID = (int)EnumList.ALScreen.Admin_Users_Edit,
                            Comment = $"User Update - {pObjUserModel.ContactName}",
                            IPAddress = CommUtil.GetIPAddress(),
                            StaffID = SessionManager.UserID,
                        };

                        new BLAuditLog().BL_InsertAuditLog(mObjAuditLog);

                        return RedirectToAction("List", "User");
                    }
                    else
                    {
                        UI_FillDropDown();
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjUserModel);
                    }
                }
                catch (Exception ex)
                {
                    Logger.SendErrorToText(ex);
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillDropDown();
                    ViewBag.Message = "Error occurred while saving users";
                    return View(pObjUserModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                MST_Users mObjUsers = new MST_Users()
                {
                    UserID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                if (mObjBLUser == null)
                    mObjBLUser = new BLUser();

                usp_GetUserList_Result mObjUserData = mObjBLUser.BL_GetUserDetails(mObjUsers);

                if (mObjUserData != null)
                {
                    return View(mObjUserData);
                }
                else
                {
                    return RedirectToAction("List", "User");
                }
            }
            else
            {
                return RedirectToAction("List", "User");
            }
        }

        public ActionResult ScreenList(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                MST_Users mObjUsers = new MST_Users()
                {
                    UserID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                if (mObjBLUser == null)
                    mObjBLUser = new BLUser();

                usp_GetUserList_Result mObjUserData = mObjBLUser.BL_GetUserDetails(mObjUsers);

                if (mObjUserData != null)
                {
                    UserViewModel mObjUserModel = new UserViewModel()
                    {
                        UserID = mObjUserData.UserID.GetValueOrDefault(),
                        UserName = mObjUserData.UserName,
                        ContactName = mObjUserData.ContactName,
                        ContactNumber = mObjUserData.ContactNumber,
                        Email = mObjUserData.EmailAddress,
                        Password = EncryptDecrypt.Decrypt(mObjUserData.Password),
                        ActiveText = mObjUserData.ActiveText,
                        UserTypeName = mObjUserData.UserTypeName,
                    };

                    IList<usp_GetScreenUserList_Result> lstScreens = new BLScreen().BL_GetScreenUserList(new MST_Screen() { UserID = id.GetValueOrDefault() });
                    ViewBag.ScreenList = lstScreens;

                    return View(mObjUserModel);
                }
                else
                {
                    return RedirectToAction("List", "User");
                }
            }
            else
            {
                return RedirectToAction("List", "User");
            }
        }

        public ActionResult AddScreen(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                MST_Users mObjUsers = new MST_Users()
                {
                    UserID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                if (mObjBLUser == null)
                    mObjBLUser = new BLUser();

                usp_GetUserList_Result mObjUserData = mObjBLUser.BL_GetUserDetails(mObjUsers);

                if (mObjUserData != null)
                {
                    UserViewModel mObjUserModel = new UserViewModel()
                    {
                        UserID = mObjUserData.UserID.GetValueOrDefault(),
                        UserName = mObjUserData.UserName,
                        ContactName = mObjUserData.ContactName,
                        ContactNumber = mObjUserData.ContactNumber,
                        Email = mObjUserData.EmailAddress,
                        Password = EncryptDecrypt.Decrypt(mObjUserData.Password),
                        ActiveText = mObjUserData.ActiveText,
                        UserTypeName = mObjUserData.UserTypeName,
                    };

                    IList<usp_GetScreenList_Result> lstScreens = new BLScreen().BL_GetScreenList(new MST_Screen() { intStatus = 1 });
                    ViewBag.ScreenList = lstScreens;
                    ViewBag.UserData = mObjUserModel;

                    UserScreenViewModel mObjScreenModel = new UserScreenViewModel()
                    {
                        UserID = mObjUserData.UserID.GetValueOrDefault()
                    };

                    return View(mObjScreenModel);
                }
                else
                {
                    return RedirectToAction("List", "User");
                }
            }
            else
            {
                return RedirectToAction("List", "User");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddScreen(UserScreenViewModel pObjScreenModel)
        {
            if (string.IsNullOrWhiteSpace(pObjScreenModel.ScreenIds))
            {
                MST_Users mObjUsers = new MST_Users()
                {
                    UserID = pObjScreenModel.UserID,
                    intStatus = 2
                };

                if (mObjBLUser == null)
                    mObjBLUser = new BLUser();

                usp_GetUserList_Result mObjUserData = mObjBLUser.BL_GetUserDetails(mObjUsers);

                UserViewModel mObjUserModel = new UserViewModel()
                {
                    UserID = mObjUserData.UserID.GetValueOrDefault(),
                    UserName = mObjUserData.UserName,
                    ContactName = mObjUserData.ContactName,
                    ContactNumber = mObjUserData.ContactNumber,
                    Email = mObjUserData.EmailAddress,
                    Password = EncryptDecrypt.Decrypt(mObjUserData.Password),
                    ActiveText = mObjUserData.ActiveText,
                    UserTypeName = mObjUserData.UserTypeName,
                };

                IList<usp_GetScreenList_Result> lstScreens = new BLScreen().BL_GetScreenList(new MST_Screen() { intStatus = 1 });
                ViewBag.ScreenList = lstScreens;
                ViewBag.UserData = mObjUserModel;

                return View(pObjScreenModel);
            }
            else
            {
                BLScreen mobjBLScreen = new BLScreen();
                string[] strScreenIds = pObjScreenModel.ScreenIds.Split(',');

                foreach (var vScreenID in strScreenIds)
                {
                    if (!string.IsNullOrWhiteSpace(vScreenID))
                    {
                        MAP_User_Screen mObjUserScreen = new MAP_User_Screen()
                        {

                            UserID = pObjScreenModel.UserID,
                            ScreenID = TrynParse.parseInt(vScreenID),
                            Active = true,
                            CreatedBy = SessionManager.UserID,
                            CreatedDate = CommUtil.GetCurrentDateTime(),
                        };

                        FuncResponse mObjResponse = mobjBLScreen.BL_InsertUserScreen(mObjUserScreen);
                    }
                }

                FlashMessage.Info("Screen added Successfully");
                return RedirectToAction("ScreenList", "User", new { id = pObjScreenModel.UserID });
            }
        }


        public JsonResult RemoveScreen(MAP_User_Screen pObjUserScreen)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjUserScreen.USID != 0)
            {
                FuncResponse<IList<usp_GetScreenUserList_Result>> mObjFuncResponse = new BLScreen().BL_RemoveUserScreen(pObjUserScreen);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["ScreenList"] = CommUtil.RenderPartialToString("_ScreenTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
                }

            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Action";
            }



            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }
    }
}