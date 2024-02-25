using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;
using EIRS.Web.GISModels;
using Google.Protobuf.WellKnownTypes;
using SixLabors.ImageSharp.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using static EIRS.Common.EnumList;
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
        public ActionResult Pending()
        {
            var retList = new List<ApprovalFlow>();
            using (var _db = new EIRSEntities())
            {
                int det = 0;
                var doc = new Dictionary<string, string>();
                var statusList = new List<int?>();
                statusList.Add(6);
                statusList.Add(8);
                var allTax = _db.Tax_Offices.Where(o => o.OfficeManagerID == SessionManager.UserID);
                if (!allTax.Any())
                { allTax = _db.Tax_Offices.Where(o => o.IncomeDirector == SessionManager.UserID); det = 1; }
                else
                    det = 2;
                if (!allTax.Any())
                    return RedirectToAction("Unauthorised");
                var allTaxOffice = allTax.Select(o => o.TaxOfficeID);

                var allAss = _db.Assessments.Where(o => statusList.Contains(o.SettlementStatusID));
                switch (det)
                {
                    case 1:
                        allAss = allAss.Where(o => o.AssessmentAmount > 100000);
                        break;
                    case 2:
                        allAss = allAss.Where(o => o.AssessmentAmount <= 100000);
                        break;
                    default:
                        break;
                }
                var possibleCompanies = allAss.Where(o => o.TaxPayerTypeID == 2).Select(o => o.TaxPayerID);
                var allCom = _db.Companies.Where(o => possibleCompanies.Contains(o.CompanyID) && allTaxOffice.Contains(o.TaxOfficeID.Value));
                var possibleIndividuals = allAss.Where(o => o.TaxPayerTypeID == 1).Select(o => o.TaxPayerID);
                var allInd = _db.Individuals.Where(o => possibleIndividuals.Contains(o.IndividualID));
                allInd = allInd.Where(o => allTaxOffice.Contains(o.TaxOfficeID.Value));
                var possibleGov = allAss.Where(o => o.TaxPayerTypeID == 4).Select(o => o.TaxPayerID);
                var allGov = _db.Governments.Where(o => possibleGov.Contains(o.GovernmentID) && allTaxOffice.Contains(o.TaxOfficeID.Value));
                foreach (var status in allCom)
                {
                    var ass = allAss.Where(o => o.TaxPayerID == status.CompanyID);
                    foreach (var a in ass)
                    {
                        ApprovalFlow af = new ApprovalFlow();
                        af.Status = System.Enum.GetName(typeof(SettlementStatus), a.SettlementStatusID);
                        af.AssessmentId = a.AssessmentID;
                        af.Amount = a.AssessmentAmount.GetValueOrDefault();
                        af.TaxPayerName = status.CompanyName;
                        af.Rin = status.CompanyRIN;
                        af.AssessmentRefNo = a.AssessmentRefNo;
                        af.Id = status.CompanyID;
                        retList.Add(af);
                    }
                }
                foreach (var status in allInd)
                {
                    var ass = allAss.Where(o => o.TaxPayerID == status.IndividualID);
                    foreach (var a in ass)
                    {
                        ApprovalFlow af = new ApprovalFlow();
                        af.Status = System.Enum.GetName(typeof(SettlementStatus), a.SettlementStatusID);
                        af.AssessmentId = a.AssessmentID;
                        af.Amount = a.AssessmentAmount.GetValueOrDefault();
                        af.TaxPayerName = $"{status.FirstName} {status.LastName}";
                        af.Rin = status.IndividualRIN;
                        af.AssessmentRefNo = a.AssessmentRefNo;

                        af.Id = status.IndividualID;
                        retList.Add(af);
                    }
                }
                foreach (var status in allGov)
                {
                    var ass = allAss.Where(o => o.TaxPayerID == status.GovernmentID);
                    foreach (var a in ass)
                    {
                        ApprovalFlow af = new ApprovalFlow();
                        af.Status = System.Enum.GetName(typeof(SettlementStatus), a.SettlementStatusID);
                        af.AssessmentId = a.AssessmentID;
                        af.Amount = a.AssessmentAmount.GetValueOrDefault();
                        af.TaxPayerName = status.GovernmentName;
                        af.Rin = status.GovernmentRIN;
                        af.AssessmentRefNo = a.AssessmentRefNo;
                        af.Id = status.GovernmentID;
                        retList.Add(af);
                    }
                }
            }
            ViewBag.ProfileInformation = retList;
            return View(retList);
        }
        public ActionResult Declined()
        {
            var retList = new List<ApprovalFlow>();
            using (var _db = new EIRSEntities())
            {
                int det = 0;
                var doc = new Dictionary<string, string>();
               
                var allTax = _db.Tax_Offices.Where(o => o.OfficeManagerID == SessionManager.UserID);
                if (!allTax.Any())
                { allTax = _db.Tax_Offices.Where(o => o.IncomeDirector == SessionManager.UserID); det = 1; }
                else
                    det = 2;
                if (!allTax.Any())
                    return RedirectToAction("Unauthorised");
                var allTaxOffice = allTax.Select(o => o.TaxOfficeID);

                var allAss = _db.Assessments.Where(o => o.SettlementStatusID == (int)SettlementStatus.Disapproved);
                switch (det)
                {
                    case 1:
                        allAss = allAss.Where(o => o.AssessmentAmount > 100000);
                        break;
                    case 2:
                        allAss = allAss.Where(o => o.AssessmentAmount <= 100000);
                        break;
                    default:
                        break;
                }
                var possibleCompanies = allAss.Where(o => o.TaxPayerTypeID == 2).Select(o => o.TaxPayerID);
                var allCom = _db.Companies.Where(o => possibleCompanies.Contains(o.CompanyID) && allTaxOffice.Contains(o.TaxOfficeID.Value));
                var possibleIndividuals = allAss.Where(o => o.TaxPayerTypeID == 1).Select(o => o.TaxPayerID);
                var allInd = _db.Individuals.Where(o => possibleIndividuals.Contains(o.IndividualID));
                allInd = allInd.Where(o => allTaxOffice.Contains(o.TaxOfficeID.Value));
                var possibleGov = allAss.Where(o => o.TaxPayerTypeID == 4).Select(o => o.TaxPayerID);
                var allGov = _db.Governments.Where(o => possibleGov.Contains(o.GovernmentID) && allTaxOffice.Contains(o.TaxOfficeID.Value));
                foreach (var status in allCom)
                {
                    var ass = allAss.Where(o => o.TaxPayerID == status.CompanyID);
                    foreach (var a in ass)
                    {
                        ApprovalFlow af = new ApprovalFlow();
                        af.Status = System.Enum.GetName(typeof(SettlementStatus), a.SettlementStatusID);
                        af.AssessmentId = a.AssessmentID;
                        af.Amount = a.AssessmentAmount.GetValueOrDefault();
                        af.TaxPayerName = status.CompanyName;
                        af.Rin = status.CompanyRIN;
                        af.AssessmentRefNo = a.AssessmentRefNo;
                        af.Id = status.CompanyID;
                        retList.Add(af);
                    }
                }
                foreach (var status in allInd)
                {
                    var ass = allAss.Where(o => o.TaxPayerID == status.IndividualID);
                    foreach (var a in ass)
                    {
                        ApprovalFlow af = new ApprovalFlow();
                        af.Status = System.Enum.GetName(typeof(SettlementStatus), a.SettlementStatusID);
                        af.AssessmentId = a.AssessmentID;
                        af.Amount = a.AssessmentAmount.GetValueOrDefault();
                        af.TaxPayerName = $"{status.FirstName} {status.LastName}";
                        af.Rin = status.IndividualRIN;
                        af.AssessmentRefNo = a.AssessmentRefNo;

                        af.Id = status.IndividualID;
                        retList.Add(af);
                    }
                }
                foreach (var status in allGov)
                {
                    var ass = allAss.Where(o => o.TaxPayerID == status.GovernmentID);
                    foreach (var a in ass)
                    {
                        ApprovalFlow af = new ApprovalFlow();
                        af.Status = System.Enum.GetName(typeof(SettlementStatus), a.SettlementStatusID);
                        af.AssessmentId = a.AssessmentID;
                        af.Amount = a.AssessmentAmount.GetValueOrDefault();
                        af.TaxPayerName = status.GovernmentName;
                        af.Rin = status.GovernmentRIN;
                        af.AssessmentRefNo = a.AssessmentRefNo;
                        af.Id = status.GovernmentID;
                        retList.Add(af);
                    }
                }
            }
            ViewBag.ProfileInformation = retList;
            return View(retList);
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