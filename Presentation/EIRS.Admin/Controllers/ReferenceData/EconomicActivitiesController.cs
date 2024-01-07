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
    public class EconomicActivitiesController : BaseController
    {
        public ActionResult List()
        {
            Economic_Activities mObjEconomicActivities = new Economic_Activities()
            {
                intStatus = 2
            };

            IList<usp_GetEconomicActivitiesList_Result> lstEconomicActivities = new BLEconomicActivities().BL_GetEconomicActivitiesList(mObjEconomicActivities);
            return View(lstEconomicActivities);
        }

        public ActionResult Add()
        {
            UI_FillTaxPayerTypeDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(EconomicActivitiesViewModel pObjEconomicActivitiesModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillTaxPayerTypeDropDown();
                return View(pObjEconomicActivitiesModel);
            }
            else
            {
                Economic_Activities mObjEconomicActivities = new Economic_Activities()
                {
                    EconomicActivitiesID = 0,
                    EconomicActivitiesName = pObjEconomicActivitiesModel.EconomicActivitiesName.Trim(), 
                    TaxPayerTypeID = pObjEconomicActivitiesModel.TaxPayerTypeID,                   
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLEconomicActivities().BL_InsertUpdateEconomicActivities(mObjEconomicActivities);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "EconomicActivities");
                    }
                    else
                    {
                        UI_FillTaxPayerTypeDropDown();
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjEconomicActivitiesModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillTaxPayerTypeDropDown();
                    ViewBag.Message = "Error occurred while saving economic activities";
                    return View(pObjEconomicActivitiesModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Economic_Activities mObjEconomicActivities = new Economic_Activities()
                {
                    EconomicActivitiesID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetEconomicActivitiesList_Result mObjEconomicActivitiesData = new BLEconomicActivities().BL_GetEconomicActivitiesDetails(mObjEconomicActivities);

                if (mObjEconomicActivitiesData != null)
                {
                    EconomicActivitiesViewModel mObjEconomicActivitiesModelView = new EconomicActivitiesViewModel()
                    {
                        EconomicActivitiesID = mObjEconomicActivitiesData.EconomicActivitiesID.GetValueOrDefault(),
                        EconomicActivitiesName = mObjEconomicActivitiesData.EconomicActivitiesName,
                        TaxPayerTypeID = mObjEconomicActivitiesData.TaxPayerTypeID.GetValueOrDefault(),
                        Active = mObjEconomicActivitiesData.Active.GetValueOrDefault(),
                    };

                    UI_FillTaxPayerTypeDropDown(new TaxPayer_Types() { intStatus = 1, IncludeTaxPayerTypeIds = mObjEconomicActivitiesData.TaxPayerTypeID.GetValueOrDefault().ToString() });

                    return View(mObjEconomicActivitiesModelView);
                }
                else
                {
                    return RedirectToAction("List", "EconomicActivities");
                }
            }
            else
            {
                return RedirectToAction("List", "EconomicActivities");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(EconomicActivitiesViewModel pObjEconomicActivitiesModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillTaxPayerTypeDropDown(new TaxPayer_Types() { intStatus = 1, IncludeTaxPayerTypeIds = pObjEconomicActivitiesModel.TaxPayerTypeID.ToString() });
                return View(pObjEconomicActivitiesModel);
            }
            else
            {
                Economic_Activities mObjEconomicActivities = new Economic_Activities()
                {
                    EconomicActivitiesID = pObjEconomicActivitiesModel.EconomicActivitiesID,
                    EconomicActivitiesName = pObjEconomicActivitiesModel.EconomicActivitiesName.Trim(),
                    TaxPayerTypeID = pObjEconomicActivitiesModel.TaxPayerTypeID,
                    Active = pObjEconomicActivitiesModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLEconomicActivities().BL_InsertUpdateEconomicActivities(mObjEconomicActivities);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "EconomicActivities");
                    }
                    else
                    {
                        UI_FillTaxPayerTypeDropDown(new TaxPayer_Types() { intStatus = 1, IncludeTaxPayerTypeIds = pObjEconomicActivitiesModel.TaxPayerTypeID.ToString() });
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjEconomicActivitiesModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillTaxPayerTypeDropDown(new TaxPayer_Types() { intStatus = 1, IncludeTaxPayerTypeIds = pObjEconomicActivitiesModel.TaxPayerTypeID.ToString() });
                    ViewBag.Message = "Error occurred while saving economic activities";
                    return View(pObjEconomicActivitiesModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Economic_Activities mObjEconomicActivities = new Economic_Activities()
                {
                    EconomicActivitiesID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetEconomicActivitiesList_Result mObjEconomicActivitiesData = new BLEconomicActivities().BL_GetEconomicActivitiesDetails(mObjEconomicActivities);

                if (mObjEconomicActivitiesData != null)
                {
                    EconomicActivitiesViewModel mObjEconomicActivitiesModelView = new EconomicActivitiesViewModel()
                    {
                        EconomicActivitiesID = mObjEconomicActivitiesData.EconomicActivitiesID.GetValueOrDefault(),
                        EconomicActivitiesName = mObjEconomicActivitiesData.EconomicActivitiesName,
                        TaxPayerTypeName = mObjEconomicActivitiesData.TaxPayerTypeName,
                        ActiveText = mObjEconomicActivitiesData.ActiveText
                    };

                    return View(mObjEconomicActivitiesModelView);
                }
                else
                {
                    return RedirectToAction("List", "EconomicActivities");
                }
            }
            else
            {
                return RedirectToAction("List", "EconomicActivities");
            }
        }

        public JsonResult UpdateStatus(Economic_Activities pObjEconomicActivitiesData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjEconomicActivitiesData.EconomicActivitiesID != 0)
            {
                FuncResponse mObjFuncResponse = new BLEconomicActivities().BL_UpdateStatus(pObjEconomicActivitiesData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["EconomicActivitiesList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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