using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vereyon.Web;
using Elmah;

namespace EIRS.Admin.Controllers.ReferenceData
{
    public class NotificationMethodController : BaseController
    {
        // GET: NotificationMethod
        public ActionResult List()
        {
            Notification_Method mObjNotificationMethod = new Notification_Method()
            {
                intStatus = 2
            };

            IList<usp_GetNotificationMethodList_Result> lstNotificationMethod = new BLNotificationMethod().BL_GetNotificationMethodList(mObjNotificationMethod);
            return View(lstNotificationMethod);
        }

        public JsonResult UpdateStatus(Notification_Method pObjNotificationMethodData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjNotificationMethodData.NotificationMethodID != 0)
            {
                FuncResponse mObjFuncResponse = new BLNotificationMethod().BL_UpdateStatus(pObjNotificationMethodData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["NotificationMethodList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
                }

            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Action";
            }
            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(NotificationMethodViewModel pObjNotificationMethodModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjNotificationMethodModel);
            }
            else
            {
                Notification_Method mObjNotificationMethod = new Notification_Method()
                {
                    NotificationMethodID = 0,
                    NotificationMethodName = pObjNotificationMethodModel.NotificationMethodName.Trim(),
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLNotificationMethod().BL_InsertUpdateNotificationMethod(mObjNotificationMethod);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "NotificationMethod");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjNotificationMethodModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving Notification Method";
                    return View(pObjNotificationMethodModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Notification_Method mObjNotificationMethod = new Notification_Method()
                {
                    NotificationMethodID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetNotificationMethodList_Result mObjNotificationMethodData = new BLNotificationMethod().BL_GetNotificationMethodDetails(mObjNotificationMethod);

                if (mObjNotificationMethodData != null)
                {
                    NotificationMethodViewModel mObjNotificationMethodModelView = new NotificationMethodViewModel()
                    {
                        NotificationMethodID = mObjNotificationMethodData.NotificationMethodID.GetValueOrDefault(),
                        NotificationMethodName = mObjNotificationMethodData.NotificationMethodName,
                        Active = mObjNotificationMethodData.Active.GetValueOrDefault(),
                    };

                    return View(mObjNotificationMethodModelView);
                }
                else
                {
                    return RedirectToAction("List", "NotificationMethod");
                }
            }
            else
            {
                return RedirectToAction("List", "NotificationMethod");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(NotificationMethodViewModel pObjNotificationMethodModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjNotificationMethodModel);
            }
            else
            {
                Notification_Method mObjNotificationMethod = new Notification_Method()
                {
                    NotificationMethodID = pObjNotificationMethodModel.NotificationMethodID,
                    NotificationMethodName = pObjNotificationMethodModel.NotificationMethodName.Trim(),
                    Active = pObjNotificationMethodModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLNotificationMethod().BL_InsertUpdateNotificationMethod(mObjNotificationMethod);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "NotificationMethod");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjNotificationMethodModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving Notification Method";
                    return View(pObjNotificationMethodModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Notification_Method mObjNotificationMethod = new Notification_Method()
                {
                    NotificationMethodID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetNotificationMethodList_Result mObjNotificationMethodData = new BLNotificationMethod().BL_GetNotificationMethodDetails(mObjNotificationMethod);

                if (mObjNotificationMethodData != null)
                {
                    NotificationMethodViewModel mObjNotificationMethodModelView = new NotificationMethodViewModel()
                    {
                        NotificationMethodID = mObjNotificationMethodData.NotificationMethodID.GetValueOrDefault(),
                        NotificationMethodName = mObjNotificationMethodData.NotificationMethodName,
                        ActiveText = mObjNotificationMethodData.ActiveText
                    };

                    return View(mObjNotificationMethodModelView);
                }
                else
                {
                    return RedirectToAction("List", "NotificationMethod");
                }
            }
            else
            {
                return RedirectToAction("List", "NotificationMethod");
            }
        }

    }
}