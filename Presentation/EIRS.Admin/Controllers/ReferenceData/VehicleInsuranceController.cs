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
    public class VehicleInsuranceController : BaseController
    {
        public ActionResult List()
        {
            Vehicle_Insurance mObjVehicleInsurance = new Vehicle_Insurance()
            {
                IntStatus = 2
            };

            IList<usp_GetVehicleInsuranceList_Result> lstVehicleInsurance = new BLVehicleInsurance().BL_GetVehicleInsuranceList(mObjVehicleInsurance);
            return View(lstVehicleInsurance);
        }

        public void UI_FillDropDown()
        {
            UI_FillVehicleDropDown();
            UI_FillCoverTypeDropDown();
            UI_FillInsuranceStatusDropDown();
        }

        public ActionResult Add()
        {
            UI_FillDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(VehicleInsuranceViewModel pObjVehicleInsuranceModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown();
                return View(pObjVehicleInsuranceModel);
            }
            else
            {
                Vehicle_Insurance mObjVehicleInsurance = new Vehicle_Insurance()
                {
                    VehicleInsuranceID = 0,
                    VehicleID = pObjVehicleInsuranceModel.VehicleID,
                    InsuranceCertificateNumber = pObjVehicleInsuranceModel.InsuranceCertificateNumber,
                    StartDate = pObjVehicleInsuranceModel.StartDate,
                    ExpiryDate = pObjVehicleInsuranceModel.ExpiryDate,
                    CoverTypeID = pObjVehicleInsuranceModel.CoverTypeID,
                    InsuranceStatusID = pObjVehicleInsuranceModel.InsuranceStatusID,
                    PremiumAmount = pObjVehicleInsuranceModel.PremiumAmount,
                    VerificationAmount = pObjVehicleInsuranceModel.VerificationAmount,
                    BrokerAmount = pObjVehicleInsuranceModel.BrokerAmount,
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLVehicleInsurance().BL_InsertUpdateVehicleInsurance(mObjVehicleInsurance);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "VehicleInsurance");
                    }
                    else
                    {
                        UI_FillDropDown();
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjVehicleInsuranceModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillDropDown();
                    ViewBag.Message = "Error occurred while saving vehicle insurance";
                    return View(pObjVehicleInsuranceModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Vehicle_Insurance mObjVehicleInsurance = new Vehicle_Insurance()
                {
                    VehicleInsuranceID = id.GetValueOrDefault(),
                    IntStatus = 2
                };

                usp_GetVehicleInsuranceList_Result mObjVehicleInsuranceData = new BLVehicleInsurance().BL_GetVehicleInsuranceDetails(mObjVehicleInsurance);

                if (mObjVehicleInsuranceData != null)
                {
                    VehicleInsuranceViewModel mObjVehicleInsuranceModelView = new VehicleInsuranceViewModel()
                    {
                        VehicleInsuranceID = mObjVehicleInsuranceData.VehicleInsuranceID.GetValueOrDefault(),
                        VehicleID = mObjVehicleInsuranceData.VehicleID.GetValueOrDefault(),
                        InsuranceCertificateNumber = mObjVehicleInsuranceData.InsuranceCertificateNumber,
                        StartDate = mObjVehicleInsuranceData.StartDate.Value,
                        ExpiryDate = mObjVehicleInsuranceData.ExpiryDate.Value,
                        CoverTypeID = mObjVehicleInsuranceData.CoverTypeID.GetValueOrDefault(),
                        InsuranceStatusID = mObjVehicleInsuranceData.InsuranceStatusID.GetValueOrDefault(),
                        PremiumAmount = mObjVehicleInsuranceData.PremiumAmount.GetValueOrDefault(),
                        VerificationAmount = mObjVehicleInsuranceData.VerificationAmount.GetValueOrDefault(),
                        BrokerAmount = mObjVehicleInsuranceData.BrokerAmount.GetValueOrDefault(),
                        Active = mObjVehicleInsuranceData.Active.GetValueOrDefault(),
                    };

                    UI_FillDropDown();
                    return View(mObjVehicleInsuranceModelView);
                }
                else
                {
                    return RedirectToAction("List", "VehicleInsurance");
                }
            }
            else
            {
                return RedirectToAction("List", "VehicleInsurance");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(VehicleInsuranceViewModel pObjVehicleInsuranceModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown();
                return View(pObjVehicleInsuranceModel);
            }
            else
            {
                Vehicle_Insurance mObjVehicleInsurance = new Vehicle_Insurance()
                {
                    VehicleInsuranceID = pObjVehicleInsuranceModel.VehicleInsuranceID,
                    VehicleID = pObjVehicleInsuranceModel.VehicleID,
                    InsuranceCertificateNumber = pObjVehicleInsuranceModel.InsuranceCertificateNumber,
                    StartDate = pObjVehicleInsuranceModel.StartDate,
                    ExpiryDate = pObjVehicleInsuranceModel.ExpiryDate,
                    CoverTypeID = pObjVehicleInsuranceModel.CoverTypeID,
                    InsuranceStatusID = pObjVehicleInsuranceModel.InsuranceStatusID,
                    PremiumAmount = pObjVehicleInsuranceModel.PremiumAmount,
                    VerificationAmount = pObjVehicleInsuranceModel.VerificationAmount,
                    BrokerAmount = pObjVehicleInsuranceModel.BrokerAmount,
                    Active = pObjVehicleInsuranceModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLVehicleInsurance().BL_InsertUpdateVehicleInsurance(mObjVehicleInsurance);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "VehicleInsurance");
                    }
                    else
                    {
                        UI_FillDropDown();
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjVehicleInsuranceModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillDropDown();
                    ViewBag.Message = "Error occurred while saving vehicle insurance";
                    return View(pObjVehicleInsuranceModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Vehicle_Insurance mObjVehicleInsurance = new Vehicle_Insurance()
                {
                    VehicleInsuranceID = id.GetValueOrDefault(),
                    IntStatus = 2
                };

                usp_GetVehicleInsuranceList_Result mObjVehicleInsuranceData = new BLVehicleInsurance().BL_GetVehicleInsuranceDetails(mObjVehicleInsurance);

                if (mObjVehicleInsuranceData != null)
                {
                    VehicleInsuranceViewModel mObjVehicleInsuranceModelView = new VehicleInsuranceViewModel()
                    {
                        VehicleInsuranceID = mObjVehicleInsuranceData.VehicleInsuranceID.GetValueOrDefault(),
                        VehicleRIN = mObjVehicleInsuranceData.VehicleRIN,
                        InsuranceCertificateNumber = mObjVehicleInsuranceData.InsuranceCertificateNumber,
                        StartDate = mObjVehicleInsuranceData.StartDate.Value,
                        ExpiryDate = mObjVehicleInsuranceData.ExpiryDate.Value,
                        CoverTypeName = mObjVehicleInsuranceData.CoverTypeName,
                        InsuranceStatusName = mObjVehicleInsuranceData.InsuranceStatusName,
                        PremiumAmount = mObjVehicleInsuranceData.PremiumAmount.GetValueOrDefault(),
                        VerificationAmount = mObjVehicleInsuranceData.VerificationAmount.GetValueOrDefault(),
                        BrokerAmount = mObjVehicleInsuranceData.BrokerAmount.GetValueOrDefault(),
                        ActiveText = mObjVehicleInsuranceData.ActiveText
                    };

                    return View(mObjVehicleInsuranceModelView);
                }
                else
                {
                    return RedirectToAction("List", "VehicleInsurance");
                }
            }
            else
            {
                return RedirectToAction("List", "VehicleInsurance");
            }
        }

        public JsonResult UpdateStatus(Vehicle_Insurance pObjVehicleInsuranceData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjVehicleInsuranceData.VehicleInsuranceID != 0)
            {
                FuncResponse mObjFuncResponse = new BLVehicleInsurance().BL_UpdateStatus(pObjVehicleInsuranceData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["VehicleInsuranceList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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