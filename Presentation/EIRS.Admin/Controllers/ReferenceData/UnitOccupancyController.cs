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
    public class UnitOccupancyController : BaseController
    {
        public ActionResult List()
        {
            Unit_Occupancy mObjUnitOccupancy = new Unit_Occupancy()
            {
                intStatus = 2
            };

            IList<usp_GetUnitOccupancyList_Result> lstUnitOccupancy = new BLUnitOccupancy().BL_GetUnitOccupancyList(mObjUnitOccupancy);
            return View(lstUnitOccupancy);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(UnitOccupancyViewModel pObjUnitOccupancyModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjUnitOccupancyModel);
            }
            else
            {
                Unit_Occupancy mObjUnitOccupancy = new Unit_Occupancy()
                {
                    UnitOccupancyID = 0,
                    UnitOccupancyName = pObjUnitOccupancyModel.UnitOccupancyName.Trim(),                    
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLUnitOccupancy().BL_InsertUpdateUnitOccupancy(mObjUnitOccupancy);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "UnitOccupancy");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjUnitOccupancyModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving unit occupancy";
                    return View(pObjUnitOccupancyModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Unit_Occupancy mObjUnitOccupancy = new Unit_Occupancy()
                {
                    UnitOccupancyID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetUnitOccupancyList_Result mObjUnitOccupancyData = new BLUnitOccupancy().BL_GetUnitOccupancyDetails(mObjUnitOccupancy);

                if (mObjUnitOccupancyData != null)
                {
                    UnitOccupancyViewModel mObjUnitOccupancyModelView = new UnitOccupancyViewModel()
                    {
                        UnitOccupancyID = mObjUnitOccupancyData.UnitOccupancyID.GetValueOrDefault(),
                        UnitOccupancyName = mObjUnitOccupancyData.UnitOccupancyName,
                        Active = mObjUnitOccupancyData.Active.GetValueOrDefault(),
                    };

                    return View(mObjUnitOccupancyModelView);
                }
                else
                {
                    return RedirectToAction("List", "UnitOccupancy");
                }
            }
            else
            {
                return RedirectToAction("List", "UnitOccupancy");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(UnitOccupancyViewModel pObjUnitOccupancyModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjUnitOccupancyModel);
            }
            else
            {
                Unit_Occupancy mObjUnitOccupancy = new Unit_Occupancy()
                {
                    UnitOccupancyID = pObjUnitOccupancyModel.UnitOccupancyID,
                    UnitOccupancyName = pObjUnitOccupancyModel.UnitOccupancyName.Trim(),
                    Active = pObjUnitOccupancyModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLUnitOccupancy().BL_InsertUpdateUnitOccupancy(mObjUnitOccupancy);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "UnitOccupancy");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjUnitOccupancyModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving unit occupancy";
                    return View(pObjUnitOccupancyModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Unit_Occupancy mObjUnitOccupancy = new Unit_Occupancy()
                {
                    UnitOccupancyID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetUnitOccupancyList_Result mObjUnitOccupancyData = new BLUnitOccupancy().BL_GetUnitOccupancyDetails(mObjUnitOccupancy);

                if (mObjUnitOccupancyData != null)
                {
                    UnitOccupancyViewModel mObjUnitOccupancyModelView = new UnitOccupancyViewModel()
                    {
                        UnitOccupancyID = mObjUnitOccupancyData.UnitOccupancyID.GetValueOrDefault(),
                        UnitOccupancyName = mObjUnitOccupancyData.UnitOccupancyName,
                        ActiveText = mObjUnitOccupancyData.ActiveText
                    };

                    return View(mObjUnitOccupancyModelView);
                }
                else
                {
                    return RedirectToAction("List", "UnitOccupancy");
                }
            }
            else
            {
                return RedirectToAction("List", "UnitOccupancy");
            }
        }

        public JsonResult UpdateStatus(Unit_Occupancy pObjUnitOccupancyData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjUnitOccupancyData.UnitOccupancyID != 0)
            {
                FuncResponse mObjFuncResponse = new BLUnitOccupancy().BL_UpdateStatus(pObjUnitOccupancyData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["UnitOccupancyList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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