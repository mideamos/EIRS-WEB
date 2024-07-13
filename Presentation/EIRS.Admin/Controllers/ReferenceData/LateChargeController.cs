using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;
using Elmah;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Vereyon.Web;

namespace EIRS.Admin.Controllers
{
    public class LateChargeController : BaseController
    {
        public ActionResult List()
        {
            Late_Charges mObjLateCharge = new Late_Charges()
            {

            };

            IList<usp_GetLateChargeList_Result> lstLateCharge = new BLLateCharge().BL_GetLateChargeList(mObjLateCharge);
            return View(lstLateCharge);
        }

        public void UI_FillDropDown(LateChargeViewModel pObjLateChargeModel = null)
        {
            if (pObjLateChargeModel == null)
            {
                pObjLateChargeModel = new LateChargeViewModel();
            }

            UI_FillRevenueStreamDropDown(new Revenue_Stream() { intStatus = 1, IncludeRevenueStreamIds = pObjLateChargeModel.RevenueStreamID.ToString() });
            UI_FillYearDropDown();
        }

        public ActionResult Add()
        {
            UI_FillDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(LateChargeViewModel pObjLateChargeModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown(pObjLateChargeModel);
                return View(pObjLateChargeModel);
            }
            else
            {
                Late_Charges mObjLateCharge = new Late_Charges()
                {
                    LateChargeID = 0,
                    RevenueStreamID = pObjLateChargeModel.RevenueStreamID,
                    TaxYear = pObjLateChargeModel.TaxYear,
                    Interest = pObjLateChargeModel.Interest,
                    Penalty = pObjLateChargeModel.Penalty,
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLLateCharge().BL_InsertUpdateLateCharge(mObjLateCharge);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "LateCharge");
                    }
                    else
                    {
                        UI_FillDropDown(pObjLateChargeModel);
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjLateChargeModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillDropDown(pObjLateChargeModel);
                    ViewBag.Message = "Error occurred while saving Late Charge";
                    return View(pObjLateChargeModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Late_Charges mObjLateCharge = new Late_Charges()
                {
                    LateChargeID = id.GetValueOrDefault(),
                    
                };

                usp_GetLateChargeList_Result mObjLateChargeData = new BLLateCharge().BL_GetLateChargeDetails(mObjLateCharge);

                if (mObjLateChargeData != null)
                {
                    LateChargeViewModel mObjLateChargeModelView = new LateChargeViewModel()
                    {
                        LateChargeID = mObjLateChargeData.LateChargeID.GetValueOrDefault(),
                        RevenueStreamID = mObjLateChargeData.RevenueStreamID.GetValueOrDefault(),
                        TaxYear = mObjLateChargeData.TaxYear.GetValueOrDefault(),
                        Interest = mObjLateChargeData.Interest.GetValueOrDefault(),
                        Penalty = mObjLateChargeData.Penalty.GetValueOrDefault(),
                        Active = mObjLateChargeData.Active.GetValueOrDefault(),
                    };

                    UI_FillDropDown(mObjLateChargeModelView);
                    return View(mObjLateChargeModelView);
                }
                else
                {
                    return RedirectToAction("List", "LateCharge");
                }
            }
            else
            {
                return RedirectToAction("List", "LateCharge");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(LateChargeViewModel pObjLateChargeModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown(pObjLateChargeModel);
                return View(pObjLateChargeModel);
            }
            else
            {
                Late_Charges mObjLateCharge = new Late_Charges()
                {
                    LateChargeID = pObjLateChargeModel.LateChargeID,
                    RevenueStreamID = pObjLateChargeModel.RevenueStreamID,
                    TaxYear = pObjLateChargeModel.TaxYear,
                    Interest = pObjLateChargeModel.Interest,
                    Penalty = pObjLateChargeModel.Penalty,
                    Active = pObjLateChargeModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLLateCharge().BL_InsertUpdateLateCharge(mObjLateCharge);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "LateCharge");
                    }
                    else
                    {
                        UI_FillDropDown(pObjLateChargeModel);
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjLateChargeModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillDropDown(pObjLateChargeModel);
                    ViewBag.Message = "Error occurred while saving Late Charge";
                    return View(pObjLateChargeModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Late_Charges mObjLateCharge = new Late_Charges()
                {
                    LateChargeID = id.GetValueOrDefault(),
                };

                usp_GetLateChargeList_Result mObjLateChargeData = new BLLateCharge().BL_GetLateChargeDetails(mObjLateCharge);

                if (mObjLateChargeData != null)
                {
                    return View(mObjLateChargeData);
                }
                else
                {
                    return RedirectToAction("List", "LateCharge");
                }
            }
            else
            {
                return RedirectToAction("List", "LateCharge");
            }
        }

        public JsonResult UpdateStatus(Late_Charges pObjLateChargeData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjLateChargeData.LateChargeID != 0)
            {
                FuncResponse mObjFuncResponse = new BLLateCharge().BL_UpdateStatus(pObjLateChargeData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["LateChargeList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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