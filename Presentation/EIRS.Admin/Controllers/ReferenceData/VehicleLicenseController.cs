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
    public class VehicleLicenseController : BaseController
    {
        public ActionResult List()
        {
            Vehicle_Licenses mObjVehicleLicense = new Vehicle_Licenses()
            {
                IntStatus = 2
            };

            IList<usp_GetVehicleLicenseList_Result> lstVehicleLicense = new BLVehicleLicense().BL_GetVehicleLicenseList(mObjVehicleLicense);
            return View(lstVehicleLicense);
        }

        public void UI_FillDropDown(VehicleLicenseViewModel pObjVehicleLicenseModel = null)
        {
            if (pObjVehicleLicenseModel == null)
                pObjVehicleLicenseModel = new VehicleLicenseViewModel();

            UI_FillVehicleDropDown();
            UI_FillVehicleInsuranceDropDown(new Vehicle_Insurance() { IntStatus = 1, VehicleID = pObjVehicleLicenseModel.VehicleID });
        }

        public ActionResult Add()
        {
            UI_FillDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(VehicleLicenseViewModel pObjVehicleLicenseModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown();
                return View(pObjVehicleLicenseModel);
            }
            else
            {
                Vehicle_Licenses mObjVehicleLicense = new Vehicle_Licenses()
                {
                    VehicleLicenseID = 0,
                    VehicleID = pObjVehicleLicenseModel.VehicleID,
                    LicenseNumber = pObjVehicleLicenseModel.LicenseNumber,
                    StartDate = pObjVehicleLicenseModel.StartDate,
                    ExpiryDate = pObjVehicleLicenseModel.ExpiryDate,
                    VehicleInsuranceID = pObjVehicleLicenseModel.VehicleInsuranceID,
                    LicenseStatusID = 1,
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLVehicleLicense().BL_InsertUpdateVehicleLicense(mObjVehicleLicense);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "VehicleLicense");
                    }
                    else
                    {
                        UI_FillDropDown();
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjVehicleLicenseModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillDropDown();
                    ViewBag.Message = "Error occurred while saving vehicle license";
                    return View(pObjVehicleLicenseModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Vehicle_Licenses mObjVehicleLicense = new Vehicle_Licenses()
                {
                    VehicleLicenseID = id.GetValueOrDefault(),
                    IntStatus = 2
                };

                usp_GetVehicleLicenseList_Result mObjVehicleLicenseData = new BLVehicleLicense().BL_GetVehicleLicenseDetails(mObjVehicleLicense);

                if (mObjVehicleLicenseData != null)
                {
                    VehicleLicenseViewModel mObjVehicleLicenseModelView = new VehicleLicenseViewModel()
                    {
                        VehicleLicenseID = mObjVehicleLicenseData.VehicleLicenseID.GetValueOrDefault(),
                        VehicleID = mObjVehicleLicenseData.VehicleID.GetValueOrDefault(),
                        LicenseNumber = mObjVehicleLicenseData.LicenseNumber,
                        StartDate = mObjVehicleLicenseData.StartDate.Value,
                        ExpiryDate = mObjVehicleLicenseData.ExpiryDate.Value,
                        VehicleInsuranceID = mObjVehicleLicenseData.VehicleInsuranceID.GetValueOrDefault(),
                        LicenseStatusID = mObjVehicleLicenseData.LicenseStatusID.GetValueOrDefault(),
                        Active = mObjVehicleLicenseData.Active.GetValueOrDefault(),
                    };

                    UI_FillDropDown();
                    return View(mObjVehicleLicenseModelView);
                }
                else
                {
                    return RedirectToAction("List", "VehicleLicense");
                }
            }
            else
            {
                return RedirectToAction("List", "VehicleLicense");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(VehicleLicenseViewModel pObjVehicleLicenseModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown();
                return View(pObjVehicleLicenseModel);
            }
            else
            {
                Vehicle_Licenses mObjVehicleLicense = new Vehicle_Licenses()
                {
                    VehicleLicenseID = pObjVehicleLicenseModel.VehicleLicenseID,
                    VehicleID = pObjVehicleLicenseModel.VehicleID,
                    LicenseNumber = pObjVehicleLicenseModel.LicenseNumber,
                    StartDate = pObjVehicleLicenseModel.StartDate,
                    ExpiryDate = pObjVehicleLicenseModel.ExpiryDate,
                    VehicleInsuranceID = pObjVehicleLicenseModel.VehicleInsuranceID,
                    LicenseStatusID = pObjVehicleLicenseModel.LicenseStatusID,
                    Active = pObjVehicleLicenseModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLVehicleLicense().BL_InsertUpdateVehicleLicense(mObjVehicleLicense);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "VehicleLicense");
                    }
                    else
                    {
                        UI_FillDropDown();
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjVehicleLicenseModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillDropDown();
                    ViewBag.Message = "Error occurred while saving vehicle insurance";
                    return View(pObjVehicleLicenseModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Vehicle_Licenses mObjVehicleLicense = new Vehicle_Licenses()
                {
                    VehicleLicenseID = id.GetValueOrDefault(),
                    IntStatus = 2
                };

                usp_GetVehicleLicenseList_Result mObjVehicleLicenseData = new BLVehicleLicense().BL_GetVehicleLicenseDetails(mObjVehicleLicense);

                if (mObjVehicleLicenseData != null)
                {
                    VehicleLicenseViewModel mObjVehicleLicenseModelView = new VehicleLicenseViewModel()
                    {
                        VehicleLicenseID = mObjVehicleLicenseData.VehicleLicenseID.GetValueOrDefault(),
                        VehicleRIN = mObjVehicleLicenseData.VehicleRIN,
                        LicenseNumber = mObjVehicleLicenseData.LicenseNumber,
                        StartDate = mObjVehicleLicenseData.StartDate.Value,
                        ExpiryDate = mObjVehicleLicenseData.ExpiryDate.Value,
                        InsuranceCertificateNumber = mObjVehicleLicenseData.InsuranceCertificateNumber,
                        InsuranceStatusName = mObjVehicleLicenseData.InsuraceStatusName,
                        LicenseStatusName = mObjVehicleLicenseData.LicenseStatusName,
                        ActiveText = mObjVehicleLicenseData.ActiveText,
                    };

                    return View(mObjVehicleLicenseModelView);
                }
                else
                {
                    return RedirectToAction("List", "VehicleLicense");
                }
            }
            else
            {
                return RedirectToAction("List", "VehicleLicense");
            }
        }

        public JsonResult UpdateStatus(Vehicle_Licenses pObjVehicleLicenseData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjVehicleLicenseData.VehicleLicenseID != 0)
            {
                FuncResponse mObjFuncResponse = new BLVehicleLicense().BL_UpdateStatus(pObjVehicleLicenseData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["VehicleLicenseList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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