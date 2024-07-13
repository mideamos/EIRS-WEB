using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Vereyon.Web;
using Elmah;

namespace EIRS.Admin.Controllers.ReferenceData
{
    public class NotificationTypeController : BaseController
    {
        // GET: NotificationType
        public ActionResult List()
        {
            Notification_Type mObjNotificationType = new Notification_Type()
            {
                intStatus = 2
            };

            IList<usp_GetNotificationTypeList_Result> lstNotificationType = new BLNotificationType().BL_GetNotificationTypeList(mObjNotificationType);
            return View(lstNotificationType);
        }

        public JsonResult UpdateStatus(Notification_Type pObjNotificationTypeData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjNotificationTypeData.NotificationTypeID != 0)
            {
                FuncResponse mObjFuncResponse = new BLNotificationType().BL_UpdateStatus(pObjNotificationTypeData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["NotificationTypeList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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
        public ActionResult Add(NotificationTypeViewModel pObjNotificationTypeModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjNotificationTypeModel);
            }
            else
            {
                Notification_Type mObjNotificationType = new Notification_Type()
                {
                    NotificationTypeID = 0,
                    NotificationTypeName = pObjNotificationTypeModel.NotificationTypeName.Trim(),
                    TypeDescription = pObjNotificationTypeModel.TypeDescription.TrimEnd(),
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLNotificationType().BL_InsertUpdateNotificationType(mObjNotificationType);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "NotificationType");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjNotificationTypeModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving Notification Type";
                    return View(pObjNotificationTypeModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Notification_Type mObjNotificationType = new Notification_Type()
                {
                    NotificationTypeID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetNotificationTypeList_Result mObjNotificationTypeData = new BLNotificationType().BL_GetNotificationTypeDetails(mObjNotificationType);

                if (mObjNotificationTypeData != null)
                {
                    NotificationTypeViewModel mObjNotificationTypeModelView = new NotificationTypeViewModel()
                    {
                        NotificationTypeID = mObjNotificationTypeData.NotificationTypeID.GetValueOrDefault(),
                        NotificationTypeName = mObjNotificationTypeData.NotificationTypeName,
                        TypeDescription=mObjNotificationTypeData.TypeDescription,
                        Active = mObjNotificationTypeData.Active.GetValueOrDefault(),
                    };

                    return View(mObjNotificationTypeModelView);
                }
                else
                {
                    return RedirectToAction("List", "NotificationType");
                }
            }
            else
            {
                return RedirectToAction("List", "NotificationType");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(NotificationTypeViewModel pObjNotificationTypeModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjNotificationTypeModel);
            }
            else
            {
                Notification_Type mObjNotificationType = new Notification_Type()
                {
                    NotificationTypeID = pObjNotificationTypeModel.NotificationTypeID,
                    NotificationTypeName = pObjNotificationTypeModel.NotificationTypeName.Trim(),
                    TypeDescription = pObjNotificationTypeModel.TypeDescription.TrimEnd(),
                    Active = pObjNotificationTypeModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLNotificationType().BL_InsertUpdateNotificationType(mObjNotificationType);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "NotificationType");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjNotificationTypeModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving Notification Type";
                    return View(pObjNotificationTypeModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Notification_Type mObjNotificationType = new Notification_Type()
                {
                    NotificationTypeID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetNotificationTypeList_Result mObjNotificationTypeData = new BLNotificationType().BL_GetNotificationTypeDetails(mObjNotificationType);

                if (mObjNotificationTypeData != null)
                {
                    NotificationTypeViewModel mObjNotificationTypeModelView = new NotificationTypeViewModel()
                    {
                        NotificationTypeID = mObjNotificationTypeData.NotificationTypeID.GetValueOrDefault(),
                        NotificationTypeName = mObjNotificationTypeData.NotificationTypeName,
                        TypeDescription=mObjNotificationTypeData.TypeDescription,
                        ActiveText = mObjNotificationTypeData.ActiveText
                    };

                    return View(mObjNotificationTypeModelView);
                }
                else
                {
                    return RedirectToAction("List", "NotificationType");
                }
            }
            else
            {
                return RedirectToAction("List", "NotificationType");
            }
        }
    }
}