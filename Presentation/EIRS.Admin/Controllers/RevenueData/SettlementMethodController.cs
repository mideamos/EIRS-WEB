using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Vereyon.Web;
using Elmah;

namespace EIRS.Admin.Controllers
{
    public class SettlementMethodController : BaseController
    {
        public ActionResult List()
        {
            Settlement_Method mObjSettlementMethod = new Settlement_Method()
            {
                intStatus = 2
            };

            IList<usp_GetSettlementMethodList_Result> lstSettlementMethod = new BLSettlementMethod().BL_GetSettlementMethodList(mObjSettlementMethod);
            return View(lstSettlementMethod);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(SettlementMethodViewModel pObjSettlementMethodModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjSettlementMethodModel);
            }
            else
            {
                Settlement_Method mObjSettlementMethod = new Settlement_Method()
                {
                    SettlementMethodID = 0,
                    SettlementMethodName = pObjSettlementMethodModel.SettlementMethodName.Trim(),                    
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLSettlementMethod().BL_InsertUpdateSettlementMethod(mObjSettlementMethod);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "SettlementMethod");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjSettlementMethodModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving settlement method";
                    return View(pObjSettlementMethodModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Settlement_Method mObjSettlementMethod = new Settlement_Method()
                {
                    SettlementMethodID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetSettlementMethodList_Result mObjSettlementMethodData = new BLSettlementMethod().BL_GetSettlementMethodDetails(mObjSettlementMethod);

                if (mObjSettlementMethodData != null)
                {
                    SettlementMethodViewModel mObjSettlementMethodModelView = new SettlementMethodViewModel()
                    {
                        SettlementMethodID = mObjSettlementMethodData.SettlementMethodID.GetValueOrDefault(),
                        SettlementMethodName = mObjSettlementMethodData.SettlementMethodName,
                        Active = mObjSettlementMethodData.Active.GetValueOrDefault(),
                    };

                    return View(mObjSettlementMethodModelView);
                }
                else
                {
                    return RedirectToAction("List", "SettlementMethod");
                }
            }
            else
            {
                return RedirectToAction("List", "SettlementMethod");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(SettlementMethodViewModel pObjSettlementMethodModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjSettlementMethodModel);
            }
            else
            {
                Settlement_Method mObjSettlementMethod = new Settlement_Method()
                {
                    SettlementMethodID = pObjSettlementMethodModel.SettlementMethodID,
                    SettlementMethodName = pObjSettlementMethodModel.SettlementMethodName.Trim(),
                    Active = pObjSettlementMethodModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLSettlementMethod().BL_InsertUpdateSettlementMethod(mObjSettlementMethod);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "SettlementMethod");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjSettlementMethodModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving settlement method";
                    return View(pObjSettlementMethodModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Settlement_Method mObjSettlementMethod = new Settlement_Method()
                {
                    SettlementMethodID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetSettlementMethodList_Result mObjSettlementMethodData = new BLSettlementMethod().BL_GetSettlementMethodDetails(mObjSettlementMethod);

                if (mObjSettlementMethodData != null)
                {
                    SettlementMethodViewModel mObjSettlementMethodModelView = new SettlementMethodViewModel()
                    {
                        SettlementMethodID = mObjSettlementMethodData.SettlementMethodID.GetValueOrDefault(),
                        SettlementMethodName = mObjSettlementMethodData.SettlementMethodName,
                        ActiveText = mObjSettlementMethodData.ActiveText
                    };

                    return View(mObjSettlementMethodModelView);
                }
                else
                {
                    return RedirectToAction("List", "SettlementMethod");
                }
            }
            else
            {
                return RedirectToAction("List", "SettlementMethod");
            }
        }

        public JsonResult UpdateStatus(Settlement_Method pObjSettlementMethodData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjSettlementMethodData.SettlementMethodID != 0)
            {
                FuncResponse mObjFuncResponse = new BLSettlementMethod().BL_UpdateStatus(pObjSettlementMethodData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["SettlementMethodList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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