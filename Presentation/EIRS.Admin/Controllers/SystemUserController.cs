using EIRS.Admin.Models;
using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using Elmah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vereyon.Web;

namespace EIRS.Admin.Controllers
{
    public class SystemUserController : BaseController
    {
        // GET: SystemUser
        public ActionResult List()
        {
            SystemUser mObjUser = new SystemUser()
            {
                intStatus = 2
            };

            IList<usp_GetSystemUserList_Result> lstUser = new BLSystemUser().BL_GetSystemUserList(mObjUser);
            return View(lstUser);
        }


        public void UI_FillDropDown()
        {
            IList<DropDownListResult> lstRole = new BLCommon().BL_GetSystemRoleDropDownList();
            ViewBag.SystemRoleList = new SelectList(lstRole, "id", "text");
        }

        public ActionResult Add()
        {
            UI_FillDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(SystemUserViewModel pObjSysUser)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown();
                return View(pObjSysUser);
            }
            else
            {
                SystemUser mObjSystemUser = new SystemUser()
                {
                    SystemUserID = 0,
                    SystemUserName = pObjSysUser.SystemUserName,
                    SystemRoleID = pObjSysUser.SystemRoleID,
                    UserLogin = pObjSysUser.UserLogin,
                    UserPassword = EncryptDecrypt.Encrypt(pObjSysUser.UserPassword),
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {
                    FuncResponse mObjResponse = new BLSystemUser().BL_InsertUpdateSystemUser(mObjSystemUser);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "SystemUser");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;

                        if (mObjResponse.Exception != null)
                        {
                            ErrorSignal.FromCurrentContext().Raise(mObjResponse.Exception);
                        }
                        UI_FillDropDown();
                        return View(pObjSysUser);
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Error occurred while saving System User";
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillDropDown();
                    return View(pObjSysUser);
                }
            }
        }

        public ActionResult Edit(int? id)
        {
            if (id.GetValueOrDefault() > 0)
            {
                SystemUser mObjUser = new SystemUser()
                {
                    SystemUserID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetSystemUserList_Result mObjUserData = new BLSystemUser().BL_GetSystemUserDetails(mObjUser);

                if (mObjUserData != null)
                {
                    SystemUserViewModel mObjUserView = new SystemUserViewModel()
                    {
                        SystemUserID = mObjUserData.SystemUserID.GetValueOrDefault(),
                        SystemUserName = mObjUserData.SystemUserName,
                        UserLogin = mObjUserData.UserLogin,
                        SystemRoleID = mObjUserData.SystemRoleID.GetValueOrDefault(),
                        UserPassword = EncryptDecrypt.Decrypt(mObjUserData.UserPassword),
                        ConfirmPassword = EncryptDecrypt.Decrypt(mObjUserData.UserPassword),
                        Active = mObjUserData.Active.GetValueOrDefault()
                    };

                    UI_FillDropDown();
                    return View(mObjUserView);
                }
                else
                {
                    return RedirectToAction("List", "SystemUser");
                }
            }
            else
            {
                return RedirectToAction("List", "SystemUser");
            }
        }

        /// <summary>
        /// Add Function to update system user
        /// </summary>
        /// <param name="pObjSysUser"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(SystemUserViewModel pObjSysUser)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown();
                return View(pObjSysUser);
            }
            else
            {
                SystemUser mObjSystemUser = new SystemUser()
                {
                    SystemUserID = pObjSysUser.SystemUserID,
                    SystemUserName = pObjSysUser.SystemUserName,
                    SystemRoleID = pObjSysUser.SystemRoleID,
                    UserLogin = pObjSysUser.UserLogin,
                    UserPassword = EncryptDecrypt.Encrypt(pObjSysUser.UserPassword),
                    Active = pObjSysUser.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {
                    FuncResponse mObjResponse = new BLSystemUser().BL_InsertUpdateSystemUser(mObjSystemUser);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "SystemUser");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;

                        if (mObjResponse.Exception != null)
                        {
                            ErrorSignal.FromCurrentContext().Raise(mObjResponse.Exception);
                        }
                        UI_FillDropDown();
                        return View(pObjSysUser);
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Error occurred while updating System User";
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillDropDown();
                    return View(pObjSysUser);
                }
            }
        }

        
        public ActionResult Details(int? id)
        {
            if (id.GetValueOrDefault() > 0)
            {
                SystemUser mObjUser = new SystemUser()
                {
                    SystemUserID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetSystemUserList_Result mObjUserData = new BLSystemUser().BL_GetSystemUserDetails(mObjUser);

                if (mObjUserData != null)
                {
                    return View(mObjUserData);
                }
                else
                {
                    return RedirectToAction("List", "SystemUser");
                }
            }
            else
            {
                return RedirectToAction("List", "SystemUser");
            }
        }

        /// <summary>
        /// JSON Result to update Acttive status of System User
        /// </summary>
        /// <param name="pObjSysUser"></param>
        /// <returns></returns>
        public JsonResult UpdateStatus(SystemUser pObjSysUser)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjSysUser.SystemUserID != 0 && pObjSysUser.SystemUserID != (int)EnumList.SystemUserRole.Admin)
            {
                pObjSysUser.ModifiedBy = SessionManager.SystemUserID;
                pObjSysUser.ModifiedDate = CommUtil.GetCurrentDateTime();
                FuncResponse mObjFuncResponse = new BLSystemUser().BL_UpdateStatus(pObjSysUser);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["SystemUserList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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