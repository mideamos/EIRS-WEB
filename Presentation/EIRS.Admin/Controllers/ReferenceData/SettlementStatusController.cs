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
    public class SettlementStatusController : BaseController
    {
        // GET: SettlementStatus
        public ActionResult List()
        {
            Settlement_Status mObjSettlementStatus = new Settlement_Status()
            {
                intStatus = 2
            };

            IList<usp_GetSettlementStatusList_Result> lstSettlementStatus = new BLSettlementStatus().BL_GetSettlementStatusList(mObjSettlementStatus);
            return View(lstSettlementStatus);
        }

        public JsonResult UpdateStatus(Settlement_Status pObjSettlementStatusData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjSettlementStatusData.SettlementStatusID != 0)
            {
                FuncResponse mObjFuncResponse = new BLSettlementStatus().BL_UpdateStatus(pObjSettlementStatusData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["SettlementStatusList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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
        public ActionResult Add(SettlementStatusViewModel pObjSettlementStatusModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjSettlementStatusModel);
            }
            else
            {
                Settlement_Status mObjSettlementStatus = new Settlement_Status()
                {
                    SettlementStatusID = 0,
                    SettlementStatusName = pObjSettlementStatusModel.SettlementStatusName.Trim(),
                    StatusDescription = pObjSettlementStatusModel.StatusDescription.TrimEnd(),
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLSettlementStatus().BL_InsertUpdateSettlementStatus(mObjSettlementStatus);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "SettlementStatus");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjSettlementStatusModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving Settlement Status";
                    return View(pObjSettlementStatusModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Settlement_Status mObjSettlementStatus = new Settlement_Status()
                {
                    SettlementStatusID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetSettlementStatusList_Result mObjSettlementStatusData = new BLSettlementStatus().BL_GetSettlementStatusDetails(mObjSettlementStatus);

                if (mObjSettlementStatusData != null)
                {
                    SettlementStatusViewModel mObjSettlementStatusModelView = new SettlementStatusViewModel()
                    {
                        SettlementStatusID = mObjSettlementStatusData.SettlementStatusID.GetValueOrDefault(),
                        SettlementStatusName = mObjSettlementStatusData.SettlementStatusName,
                        StatusDescription = mObjSettlementStatusData.StatusDescription,
                        Active = mObjSettlementStatusData.Active.GetValueOrDefault(),
                    };

                    return View(mObjSettlementStatusModelView);
                }
                else
                {
                    return RedirectToAction("List", "SettlementStatus");
                }
            }
            else
            {
                return RedirectToAction("List", "SettlementStatus");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(SettlementStatusViewModel pObjSettlementStatusModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjSettlementStatusModel);
            }
            else
            {
                Settlement_Status mObjSettlementStatus = new Settlement_Status()
                {
                    SettlementStatusID = pObjSettlementStatusModel.SettlementStatusID,
                    SettlementStatusName = pObjSettlementStatusModel.SettlementStatusName.Trim(),
                    StatusDescription = pObjSettlementStatusModel.StatusDescription.TrimEnd(),
                    Active = pObjSettlementStatusModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLSettlementStatus().BL_InsertUpdateSettlementStatus(mObjSettlementStatus);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "SettlementStatus");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjSettlementStatusModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving Settlement Status";
                    return View(pObjSettlementStatusModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Settlement_Status mObjSettlementStatus = new Settlement_Status()
                {
                    SettlementStatusID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetSettlementStatusList_Result mObjSettlementStatusData = new BLSettlementStatus().BL_GetSettlementStatusDetails(mObjSettlementStatus);

                if (mObjSettlementStatusData != null)
                {
                    SettlementStatusViewModel mObjSettlementStatusModelView = new SettlementStatusViewModel()
                    {
                        SettlementStatusID = mObjSettlementStatusData.SettlementStatusID.GetValueOrDefault(),
                        SettlementStatusName = mObjSettlementStatusData.SettlementStatusName,
                        StatusDescription = mObjSettlementStatusData.StatusDescription,
                        ActiveText = mObjSettlementStatusData.ActiveText
                    };

                    return View(mObjSettlementStatusModelView);
                }
                else
                {
                    return RedirectToAction("List", "SettlementStatus");
                }
            }
            else
            {
                return RedirectToAction("List", "SettlementStatus");
            }
        }
    }
}