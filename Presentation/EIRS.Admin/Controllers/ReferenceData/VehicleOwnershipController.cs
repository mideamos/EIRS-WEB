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
    public class VehicleOwnershipController : BaseController
    {
        public ActionResult List()
        {
            Vehicle_Ownership mObjVehicleOwnership = new Vehicle_Ownership()
            {
                intStatus = 2
            };

            IList<usp_GetVehicleOwnershipList_Result> lstVehicleOwnership = new BLVehicleOwnership().BL_GetVehicleOwnershipList(mObjVehicleOwnership);
            return View(lstVehicleOwnership);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(VehicleOwnershipViewModel pObjVehicleOwnershipModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjVehicleOwnershipModel);
            }
            else
            {
                Vehicle_Ownership mObjVehicleOwnership = new Vehicle_Ownership()
                {
                    VehicleOwnershipID = 0,
                    VehicleOwnershipName = pObjVehicleOwnershipModel.VehicleOwnershipName.Trim(),                    
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLVehicleOwnership().BL_InsertUpdateVehicleOwnership(mObjVehicleOwnership);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "VehicleOwnership");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjVehicleOwnershipModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving vehicle ownership";
                    return View(pObjVehicleOwnershipModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Vehicle_Ownership mObjVehicleOwnership = new Vehicle_Ownership()
                {
                    VehicleOwnershipID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetVehicleOwnershipList_Result mObjVehicleOwnershipData = new BLVehicleOwnership().BL_GetVehicleOwnershipDetails(mObjVehicleOwnership);

                if (mObjVehicleOwnershipData != null)
                {
                    VehicleOwnershipViewModel mObjVehicleOwnershipModelView = new VehicleOwnershipViewModel()
                    {
                        VehicleOwnershipID = mObjVehicleOwnershipData.VehicleOwnershipID.GetValueOrDefault(),
                        VehicleOwnershipName = mObjVehicleOwnershipData.VehicleOwnershipName,
                        Active = mObjVehicleOwnershipData.Active.GetValueOrDefault(),
                    };

                    return View(mObjVehicleOwnershipModelView);
                }
                else
                {
                    return RedirectToAction("List", "VehicleOwnership");
                }
            }
            else
            {
                return RedirectToAction("List", "VehicleOwnership");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(VehicleOwnershipViewModel pObjVehicleOwnershipModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjVehicleOwnershipModel);
            }
            else
            {
                Vehicle_Ownership mObjVehicleOwnership = new Vehicle_Ownership()
                {
                    VehicleOwnershipID = pObjVehicleOwnershipModel.VehicleOwnershipID,
                    VehicleOwnershipName = pObjVehicleOwnershipModel.VehicleOwnershipName.Trim(),
                    Active = pObjVehicleOwnershipModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLVehicleOwnership().BL_InsertUpdateVehicleOwnership(mObjVehicleOwnership);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "VehicleOwnership");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjVehicleOwnershipModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving vehicle ownership";
                    return View(pObjVehicleOwnershipModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Vehicle_Ownership mObjVehicleOwnership = new Vehicle_Ownership()
                {
                    VehicleOwnershipID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetVehicleOwnershipList_Result mObjVehicleOwnershipData = new BLVehicleOwnership().BL_GetVehicleOwnershipDetails(mObjVehicleOwnership);

                if (mObjVehicleOwnershipData != null)
                {
                    VehicleOwnershipViewModel mObjVehicleOwnershipModelView = new VehicleOwnershipViewModel()
                    {
                        VehicleOwnershipID = mObjVehicleOwnershipData.VehicleOwnershipID.GetValueOrDefault(),
                        VehicleOwnershipName = mObjVehicleOwnershipData.VehicleOwnershipName,
                        ActiveText = mObjVehicleOwnershipData.ActiveText
                    };

                    return View(mObjVehicleOwnershipModelView);
                }
                else
                {
                    return RedirectToAction("List", "VehicleOwnership");
                }
            }
            else
            {
                return RedirectToAction("List", "VehicleOwnership");
            }
        }

        public JsonResult UpdateStatus(Vehicle_Ownership pObjVehicleOwnershipData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjVehicleOwnershipData.VehicleOwnershipID != 0)
            {
                FuncResponse mObjFuncResponse = new BLVehicleOwnership().BL_UpdateStatus(pObjVehicleOwnershipData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["VehicleOwnershipList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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