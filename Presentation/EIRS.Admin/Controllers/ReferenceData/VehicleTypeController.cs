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
    public class VehicleTypeController : BaseController
    {
        public ActionResult List()
        {
            Vehicle_Types mObjVehicleType = new Vehicle_Types()
            {
                intStatus = 2
            };

            IList<usp_GetVehicleTypeList_Result> lstVehicleType = new BLVehicleType().BL_GetVehicleTypeList(mObjVehicleType);
            return View(lstVehicleType);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(VehicleTypeViewModel pObjVehicleTypeModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjVehicleTypeModel);
            }
            else
            {
                Vehicle_Types mObjVehicleType = new Vehicle_Types()
                {
                    VehicleTypeID = 0,
                    VehicleTypeName = pObjVehicleTypeModel.VehicleTypeName.Trim(),                    
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLVehicleType().BL_InsertUpdateVehicleType(mObjVehicleType);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "VehicleType");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjVehicleTypeModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving vehicle type";
                    return View(pObjVehicleTypeModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Vehicle_Types mObjVehicleType = new Vehicle_Types()
                {
                    VehicleTypeID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetVehicleTypeList_Result mObjVehicleTypeData = new BLVehicleType().BL_GetVehicleTypeDetails(mObjVehicleType);

                if (mObjVehicleTypeData != null)
                {
                    VehicleTypeViewModel mObjVehicleTypeModelView = new VehicleTypeViewModel()
                    {
                        VehicleTypeID = mObjVehicleTypeData.VehicleTypeID.GetValueOrDefault(),
                        VehicleTypeName = mObjVehicleTypeData.VehicleTypeName,
                        Active = mObjVehicleTypeData.Active.GetValueOrDefault(),
                    };

                    return View(mObjVehicleTypeModelView);
                }
                else
                {
                    return RedirectToAction("List", "VehicleType");
                }
            }
            else
            {
                return RedirectToAction("List", "VehicleType");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(VehicleTypeViewModel pObjVehicleTypeModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjVehicleTypeModel);
            }
            else
            {
                Vehicle_Types mObjVehicleType = new Vehicle_Types()
                {
                    VehicleTypeID = pObjVehicleTypeModel.VehicleTypeID,
                    VehicleTypeName = pObjVehicleTypeModel.VehicleTypeName.Trim(),
                    Active = pObjVehicleTypeModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLVehicleType().BL_InsertUpdateVehicleType(mObjVehicleType);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "VehicleType");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjVehicleTypeModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving vehicle type";
                    return View(pObjVehicleTypeModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Vehicle_Types mObjVehicleType = new Vehicle_Types()
                {
                    VehicleTypeID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetVehicleTypeList_Result mObjVehicleTypeData = new BLVehicleType().BL_GetVehicleTypeDetails(mObjVehicleType);

                if (mObjVehicleTypeData != null)
                {
                    VehicleTypeViewModel mObjVehicleTypeModelView = new VehicleTypeViewModel()
                    {
                        VehicleTypeID = mObjVehicleTypeData.VehicleTypeID.GetValueOrDefault(),
                        VehicleTypeName = mObjVehicleTypeData.VehicleTypeName,
                        ActiveText = mObjVehicleTypeData.ActiveText
                    };

                    return View(mObjVehicleTypeModelView);
                }
                else
                {
                    return RedirectToAction("List", "VehicleType");
                }
            }
            else
            {
                return RedirectToAction("List", "VehicleType");
            }
        }

        public JsonResult UpdateStatus(Vehicle_Types pObjVehicleTypeData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjVehicleTypeData.VehicleTypeID != 0)
            {
                FuncResponse mObjFuncResponse = new BLVehicleType().BL_UpdateStatus(pObjVehicleTypeData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["VehicleTypeList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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