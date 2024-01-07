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
    public class VehicleFunctionController : BaseController
    {
        public ActionResult List()
        {
            Vehicle_Function mObjVehicleFunction = new Vehicle_Function()
            {
                intStatus = 2
            };

            IList<usp_GetVehicleFunctionList_Result> lstVehicleFunction = new BLVehicleFunction().BL_GetVehicleFunctionList(mObjVehicleFunction);
            return View(lstVehicleFunction);
        }

        public ActionResult Add()
        {
            UI_FillVehiclePurposeDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(VehicleFunctionViewModel pObjVehicleFunctionModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillVehiclePurposeDropDown();
                return View(pObjVehicleFunctionModel);
            }
            else
            {
                Vehicle_Function mObjVehicleFunction = new Vehicle_Function()
                {
                    VehicleFunctionID = 0,
                    VehicleFunctionName = pObjVehicleFunctionModel.VehicleFunctionName.Trim(),  
                    VehiclePurposeID = pObjVehicleFunctionModel.VehiclePurposeID,                  
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLVehicleFunction().BL_InsertUpdateVehicleFunction(mObjVehicleFunction);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "VehicleFunction");
                    }
                    else
                    {
                        UI_FillVehiclePurposeDropDown();
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjVehicleFunctionModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillVehiclePurposeDropDown();
                    ViewBag.Message = "Error occurred while saving vehicle function";
                    return View(pObjVehicleFunctionModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Vehicle_Function mObjVehicleFunction = new Vehicle_Function()
                {
                    VehicleFunctionID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetVehicleFunctionList_Result mObjVehicleFunctionData = new BLVehicleFunction().BL_GetVehicleFunctionDetails(mObjVehicleFunction);

                if (mObjVehicleFunctionData != null)
                {
                    VehicleFunctionViewModel mObjVehicleFunctionModelView = new VehicleFunctionViewModel()
                    {
                        VehicleFunctionID = mObjVehicleFunctionData.VehicleFunctionID.GetValueOrDefault(),
                        VehicleFunctionName = mObjVehicleFunctionData.VehicleFunctionName,
                        VehiclePurposeID = mObjVehicleFunctionData.VehiclePurposeID.GetValueOrDefault(),
                        Active = mObjVehicleFunctionData.Active.GetValueOrDefault(),
                    };

                    UI_FillVehiclePurposeDropDown(new Vehicle_Purpose() { intStatus = 1, IncludeVehiclePurposeIds = mObjVehicleFunctionModelView.VehiclePurposeID.ToString() });
                    return View(mObjVehicleFunctionModelView);
                }
                else
                {
                    return RedirectToAction("List", "VehicleFunction");
                }
            }
            else
            {
                return RedirectToAction("List", "VehicleFunction");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(VehicleFunctionViewModel pObjVehicleFunctionModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillVehiclePurposeDropDown(new Vehicle_Purpose() { intStatus = 1, IncludeVehiclePurposeIds = pObjVehicleFunctionModel.VehiclePurposeID.ToString() });
                return View(pObjVehicleFunctionModel);
            }
            else
            {
                Vehicle_Function mObjVehicleFunction = new Vehicle_Function()
                {
                    VehicleFunctionID = pObjVehicleFunctionModel.VehicleFunctionID,
                    VehicleFunctionName = pObjVehicleFunctionModel.VehicleFunctionName.Trim(),
                    VehiclePurposeID = pObjVehicleFunctionModel.VehiclePurposeID,
                    Active = pObjVehicleFunctionModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLVehicleFunction().BL_InsertUpdateVehicleFunction(mObjVehicleFunction);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "VehicleFunction");
                    }
                    else
                    {
                        UI_FillVehiclePurposeDropDown(new Vehicle_Purpose() { intStatus = 1, IncludeVehiclePurposeIds = pObjVehicleFunctionModel.VehiclePurposeID.ToString() });
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjVehicleFunctionModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillVehiclePurposeDropDown(new Vehicle_Purpose() { intStatus = 1, IncludeVehiclePurposeIds = pObjVehicleFunctionModel.VehiclePurposeID.ToString() });
                    ViewBag.Message = "Error occurred while saving vehicle function";
                    return View(pObjVehicleFunctionModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Vehicle_Function mObjVehicleFunction = new Vehicle_Function()
                {
                    VehicleFunctionID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetVehicleFunctionList_Result mObjVehicleFunctionData = new BLVehicleFunction().BL_GetVehicleFunctionDetails(mObjVehicleFunction);

                if (mObjVehicleFunctionData != null)
                {
                    VehicleFunctionViewModel mObjVehicleFunctionModelView = new VehicleFunctionViewModel()
                    {
                        VehicleFunctionID = mObjVehicleFunctionData.VehicleFunctionID.GetValueOrDefault(),
                        VehicleFunctionName = mObjVehicleFunctionData.VehicleFunctionName,
                        VehiclePurposeName = mObjVehicleFunctionData.VehiclePurposeName,
                        ActiveText = mObjVehicleFunctionData.ActiveText
                    };

                    return View(mObjVehicleFunctionModelView);
                }
                else
                {
                    return RedirectToAction("List", "VehicleFunction");
                }
            }
            else
            {
                return RedirectToAction("List", "VehicleFunction");
            }
        }

        public JsonResult UpdateStatus(Vehicle_Function pObjVehicleFunctionData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjVehicleFunctionData.VehicleFunctionID != 0)
            {
                FuncResponse mObjFuncResponse = new BLVehicleFunction().BL_UpdateStatus(pObjVehicleFunctionData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["VehicleFunctionList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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