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
    public class VehicleSubTypeController : BaseController
    {
        public ActionResult List()
        {
            Vehicle_SubTypes mObjVehicleSubType = new Vehicle_SubTypes()
            {
                intStatus = 2
            };

            IList<usp_GetVehicleSubTypeList_Result> lstVehicleSubType = new BLVehicleSubType().BL_GetVehicleSubTypeList(mObjVehicleSubType);
            return View(lstVehicleSubType);
        }

        public ActionResult Add()
        {
            UI_FillVehicleTypeDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(VehicleSubTypeViewModel pObjVehicleSubTypeModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillVehicleTypeDropDown();
                return View(pObjVehicleSubTypeModel);
            }
            else
            {
                Vehicle_SubTypes mObjVehicleSubType = new Vehicle_SubTypes()
                {
                    VehicleSubTypeID = 0,
                    VehicleSubTypeName = pObjVehicleSubTypeModel.VehicleSubTypeName.Trim(),     
                    VehicleTypeID = pObjVehicleSubTypeModel.VehicleTypeID,               
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLVehicleSubType().BL_InsertUpdateVehicleSubType(mObjVehicleSubType);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "VehicleSubType");
                    }
                    else
                    {
                        UI_FillVehicleTypeDropDown();
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjVehicleSubTypeModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillVehicleTypeDropDown();
                    ViewBag.Message = "Error occurred while saving vehicle sub type";
                    return View(pObjVehicleSubTypeModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Vehicle_SubTypes mObjVehicleSubType = new Vehicle_SubTypes()
                {
                    VehicleSubTypeID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetVehicleSubTypeList_Result mObjVehicleSubTypeData = new BLVehicleSubType().BL_GetVehicleSubTypeDetails(mObjVehicleSubType);

                if (mObjVehicleSubTypeData != null)
                {
                    VehicleSubTypeViewModel mObjVehicleSubTypeModelView = new VehicleSubTypeViewModel()
                    {
                        VehicleSubTypeID = mObjVehicleSubTypeData.VehicleSubTypeID.GetValueOrDefault(),
                        VehicleSubTypeName = mObjVehicleSubTypeData.VehicleSubTypeName,
                        VehicleTypeID = mObjVehicleSubTypeData.VehicleTypeID.GetValueOrDefault(),
                        Active = mObjVehicleSubTypeData.Active.GetValueOrDefault(),
                    };

                    UI_FillVehicleTypeDropDown(new Vehicle_Types() { intStatus = 1, IncludeVehicleTypeIds = mObjVehicleSubTypeData.VehicleTypeID.GetValueOrDefault().ToString() });

                    return View(mObjVehicleSubTypeModelView);
                }
                else
                {
                    return RedirectToAction("List", "VehicleSubType");
                }
            }
            else
            {
                return RedirectToAction("List", "VehicleSubType");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(VehicleSubTypeViewModel pObjVehicleSubTypeModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillVehicleTypeDropDown(new Vehicle_Types() { intStatus = 1, IncludeVehicleTypeIds = pObjVehicleSubTypeModel.VehicleTypeID.ToString() });
                return View(pObjVehicleSubTypeModel);
            }
            else
            {
                Vehicle_SubTypes mObjVehicleSubType = new Vehicle_SubTypes()
                {
                    VehicleSubTypeID = pObjVehicleSubTypeModel.VehicleSubTypeID,
                    VehicleSubTypeName = pObjVehicleSubTypeModel.VehicleSubTypeName.Trim(),
                    VehicleTypeID = pObjVehicleSubTypeModel.VehicleTypeID,
                    Active = pObjVehicleSubTypeModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLVehicleSubType().BL_InsertUpdateVehicleSubType(mObjVehicleSubType);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "VehicleSubType");
                    }
                    else
                    {
                        UI_FillVehicleTypeDropDown(new Vehicle_Types() { intStatus = 1, IncludeVehicleTypeIds = pObjVehicleSubTypeModel.VehicleTypeID.ToString() });
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjVehicleSubTypeModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillVehicleTypeDropDown(new Vehicle_Types() { intStatus = 1, IncludeVehicleTypeIds = pObjVehicleSubTypeModel.VehicleTypeID.ToString() });
                    ViewBag.Message = "Error occurred while saving vehicle sub type";
                    return View(pObjVehicleSubTypeModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Vehicle_SubTypes mObjVehicleSubType = new Vehicle_SubTypes()
                {
                    VehicleSubTypeID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetVehicleSubTypeList_Result mObjVehicleSubTypeData = new BLVehicleSubType().BL_GetVehicleSubTypeDetails(mObjVehicleSubType);

                if (mObjVehicleSubTypeData != null)
                {
                    VehicleSubTypeViewModel mObjVehicleSubTypeModelView = new VehicleSubTypeViewModel()
                    {
                        VehicleSubTypeID = mObjVehicleSubTypeData.VehicleSubTypeID.GetValueOrDefault(),
                        VehicleSubTypeName = mObjVehicleSubTypeData.VehicleSubTypeName,
                        VehicleTypeName = mObjVehicleSubTypeData.VehicleTypeName,
                        ActiveText = mObjVehicleSubTypeData.ActiveText
                    };

                    return View(mObjVehicleSubTypeModelView);
                }
                else
                {
                    return RedirectToAction("List", "VehicleSubType");
                }
            }
            else
            {
                return RedirectToAction("List", "VehicleSubType");
            }
        }

        public JsonResult UpdateStatus(Vehicle_SubTypes pObjVehicleSubTypeData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjVehicleSubTypeData.VehicleSubTypeID != 0)
            {
                FuncResponse mObjFuncResponse = new BLVehicleSubType().BL_UpdateStatus(pObjVehicleSubTypeData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["VehicleSubTypeList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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