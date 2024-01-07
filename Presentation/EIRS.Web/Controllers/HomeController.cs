using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using static EIRS.Web.Controllers.Filters;

namespace EIRS.Web.Controllers
{
    [SessionTimeout]
    public class HomeController : BaseController
    {
        public ActionResult GetCentralTopMenuList(int pIntParentMenuID)
        {
            IList<usp_GetCentralMenuUserBased_Result> lstMenuData = new BLCentralMenu().BL_GetCentralMenuUserBased(SessionManager.UserID, pIntParentMenuID);
            return PartialView(lstMenuData);
        }

        public ActionResult GetCentralSideMenuList(int pIntParentMenuID)
        {
            IList<usp_GetCentralMenuUserBased_Result> lstMenuData = new BLCentralMenu().BL_GetCentralMenuUserBased(SessionManager.UserID, pIntParentMenuID);
            return PartialView(lstMenuData);
        }

        public ActionResult Unauthorised()
        {
            return View();
        }


        public ActionResult Dashboard()
        {
            UI_FillSettlementStatusDropDown();
            UI_FillTaxPayerTypeDropDown();
            return View();
        }


        public ActionResult DataCubes()
        {
            UI_FillSettlementStatusDropDown();
            UI_FillTaxPayerTypeDropDown();
            return View();
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult ChangePassword(ChangePasswordViewModel pObjChangePasswordModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjChangePasswordModel);
            }
            FuncResponse mObjFuncResponse;
            if (SessionManager.TaxPayerTypeID == (int)EnumList.TaxPayerType.Individual)
            {
                Individual objIndividual = new Individual()
                {
                    IndividualRIN = SessionManager.RIN,
                    OldPassword = EncryptDecrypt.Encrypt(pObjChangePasswordModel.OldPassword),
                    Password = EncryptDecrypt.Encrypt(pObjChangePasswordModel.NewPassword)
                };

                mObjFuncResponse = new BLIndividual().BL_ChangePassword(objIndividual);
            }
            else if (SessionManager.TaxPayerTypeID == (int)EnumList.TaxPayerType.Companies)
            {
                Company objCompany = new Company()
                {
                    CompanyRIN = SessionManager.RIN,
                    OldPassword = EncryptDecrypt.Encrypt(pObjChangePasswordModel.OldPassword),
                    Password = EncryptDecrypt.Encrypt(pObjChangePasswordModel.NewPassword)
                };

                mObjFuncResponse = new BLCompany().BL_ChangePassword(objCompany);

            }
            else if (SessionManager.TaxPayerTypeID == (int)EnumList.TaxPayerType.Government)
            {
                Government objGovernment = new Government()
                {
                    GovernmentRIN = SessionManager.RIN,
                    OldPassword = EncryptDecrypt.Encrypt(pObjChangePasswordModel.OldPassword),
                    Password = EncryptDecrypt.Encrypt(pObjChangePasswordModel.NewPassword)
                };

                mObjFuncResponse = new BLGovernment().BL_ChangePassword(objGovernment);

            }
            else
            {
                MST_Users objUsers = new MST_Users()
                {
                    UserID = SessionManager.UserID,
                    OldPassword = EncryptDecrypt.Encrypt(pObjChangePasswordModel.OldPassword),
                    Password = EncryptDecrypt.Encrypt(pObjChangePasswordModel.NewPassword)
                };

                mObjFuncResponse = new BLUser().BL_ChangePassword(objUsers);
            }
            if (mObjFuncResponse.Success)
            {
                ViewBag.SMessage = "Password Changed Successfully.";
                return View();
            }
            else
            {
                ViewBag.FMessage = mObjFuncResponse.Message;
                return View(pObjChangePasswordModel);
            }


        }

        public ActionResult DownloadFile(string type, int documentId)
        {
            if (type == "APIDocument")
            {
                var vDocument = new BLAPI().BL_GetAPIDetails(new MST_API() { APIID = documentId });
                return File(GlobalDefaultValues.DocumentLocation + vDocument.DocumentPath, "application/force-download", vDocument.APIName.Replace(" ", "_") + "_API_Documentation.pdf");
            }

            return Content("Invalid Request");
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            return RedirectToAction("Home", "Default");
        }

        public JsonResult GetBillChart(int BillTypeID, int StatusID, int FilterTypeID)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>
            {
                ["FilterTypeID"] = FilterTypeID
            };
            IList<usp_GetBillChart_Result> lstBillChart = new BLDashboard().BL_GetBillChart(BillTypeID, StatusID, FilterTypeID);
            dcResponse["ChartData"] = lstBillChart;
            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTaxPayerBillChart(int BillTypeID, int TaxPayerTypeID, int FilterTypeID)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();
            dcResponse["FilterTypeID"] = FilterTypeID;
            IList<usp_GetTaxPayerBillChart_Result> lstTaxPayerBillChart = new BLDashboard().BL_GetTaxPayerBillChart(BillTypeID, TaxPayerTypeID, FilterTypeID);
            dcResponse["ChartData"] = lstTaxPayerBillChart;
            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTaxPayerSettlementChart(int BillTypeID, int TaxPayerTypeID, int FilterTypeID)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();
            dcResponse["FilterTypeID"] = FilterTypeID;
            IList<usp_GetTaxPayerSettlementChart_Result> lstTaxPayerSettlementChart = new BLDashboard().BL_GetTaxPayerSettlementChart(BillTypeID, TaxPayerTypeID, FilterTypeID);
            dcResponse["ChartData"] = lstTaxPayerSettlementChart;
            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBillAgingChart(int BillTypeID, int TaxPayerTypeID, int FilterTypeID)
        {
            IList<usp_GetBillAgingChart_Result> lstBillAgingChart = new BLDashboard().BL_GetBillAgingChart(BillTypeID, TaxPayerTypeID, FilterTypeID);
            return Json(lstBillAgingChart, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBuildingChart(int FilterType)
        {
            IList<usp_GetBuildingChart_Result> lstBuildingChart = new BLDashboard().BL_GetBuildingChart(FilterType);
            return Json(lstBuildingChart, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBusinessChart(int FilterType)
        {
            IList<usp_GetBusinessChart_Result> lstBusinessChart = new BLDashboard().BL_GetBusinessChart(FilterType);
            return Json(lstBusinessChart, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLandChart(int FilterType)
        {
            IList<usp_GetLandChart_Result> lstLandChart = new BLDashboard().BL_GetLandChart(FilterType);
            return Json(lstLandChart, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetVehicleChart(int FilterType)
        {
            IList<usp_GetVehicleChart_Result> lstVehicleChart = new BLDashboard().BL_GetVehicleChart(FilterType);
            return Json(lstVehicleChart, JsonRequestBehavior.AllowGet);
        }
    }


}