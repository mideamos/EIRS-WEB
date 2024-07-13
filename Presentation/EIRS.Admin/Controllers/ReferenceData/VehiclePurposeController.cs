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
    public class VehiclePurposeController : BaseController
    {
        public ActionResult List()
        {
            Vehicle_Purpose mObjVehiclePurpose = new Vehicle_Purpose()
            {
                intStatus = 2
            };

            IList<usp_GetVehiclePurposeList_Result> lstVehiclePurpose = new BLVehiclePurpose().BL_GetVehiclePurposeList(mObjVehiclePurpose);
            return View(lstVehiclePurpose);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(VehiclePurposeViewModel pObjVehiclePurposeModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjVehiclePurposeModel);
            }
            else
            {
                Vehicle_Purpose mObjVehiclePurpose = new Vehicle_Purpose()
                {
                    VehiclePurposeID = 0,
                    VehiclePurposeName = pObjVehiclePurposeModel.VehiclePurposeName.Trim(),                    
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLVehiclePurpose().BL_InsertUpdateVehiclePurpose(mObjVehiclePurpose);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "VehiclePurpose");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjVehiclePurposeModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving vehicle purpose";
                    return View(pObjVehiclePurposeModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Vehicle_Purpose mObjVehiclePurpose = new Vehicle_Purpose()
                {
                    VehiclePurposeID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetVehiclePurposeList_Result mObjVehiclePurposeData = new BLVehiclePurpose().BL_GetVehiclePurposeDetails(mObjVehiclePurpose);

                if (mObjVehiclePurposeData != null)
                {
                    VehiclePurposeViewModel mObjVehiclePurposeModelView = new VehiclePurposeViewModel()
                    {
                        VehiclePurposeID = mObjVehiclePurposeData.VehiclePurposeID.GetValueOrDefault(),
                        VehiclePurposeName = mObjVehiclePurposeData.VehiclePurposeName,
                        Active = mObjVehiclePurposeData.Active.GetValueOrDefault(),
                    };

                    return View(mObjVehiclePurposeModelView);
                }
                else
                {
                    return RedirectToAction("List", "VehiclePurpose");
                }
            }
            else
            {
                return RedirectToAction("List", "VehiclePurpose");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(VehiclePurposeViewModel pObjVehiclePurposeModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjVehiclePurposeModel);
            }
            else
            {
                Vehicle_Purpose mObjVehiclePurpose = new Vehicle_Purpose()
                {
                    VehiclePurposeID = pObjVehiclePurposeModel.VehiclePurposeID,
                    VehiclePurposeName = pObjVehiclePurposeModel.VehiclePurposeName.Trim(),
                    Active = pObjVehiclePurposeModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLVehiclePurpose().BL_InsertUpdateVehiclePurpose(mObjVehiclePurpose);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "VehiclePurpose");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjVehiclePurposeModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving vehicle purpose";
                    return View(pObjVehiclePurposeModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Vehicle_Purpose mObjVehiclePurpose = new Vehicle_Purpose()
                {
                    VehiclePurposeID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetVehiclePurposeList_Result mObjVehiclePurposeData = new BLVehiclePurpose().BL_GetVehiclePurposeDetails(mObjVehiclePurpose);

                if (mObjVehiclePurposeData != null)
                {
                    VehiclePurposeViewModel mObjVehiclePurposeModelView = new VehiclePurposeViewModel()
                    {
                        VehiclePurposeID = mObjVehiclePurposeData.VehiclePurposeID.GetValueOrDefault(),
                        VehiclePurposeName = mObjVehiclePurposeData.VehiclePurposeName,
                        ActiveText = mObjVehiclePurposeData.ActiveText
                    };

                    return View(mObjVehiclePurposeModelView);
                }
                else
                {
                    return RedirectToAction("List", "VehiclePurpose");
                }
            }
            else
            {
                return RedirectToAction("List", "VehiclePurpose");
            }
        }

        public JsonResult UpdateStatus(Vehicle_Purpose pObjVehiclePurposeData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjVehiclePurposeData.VehiclePurposeID != 0)
            {
                FuncResponse mObjFuncResponse = new BLVehiclePurpose().BL_UpdateStatus(pObjVehiclePurposeData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["VehiclePurposeList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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