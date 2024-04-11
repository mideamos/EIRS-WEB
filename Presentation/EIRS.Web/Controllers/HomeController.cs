using Aspose.Pdf.Operators;
using DocumentFormat.OpenXml.Wordprocessing;
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
using System.Security.Cryptography;
using System.Web.Mvc;
using static EIRS.Common.EnumList;
using static EIRS.Web.Controllers.Filters;

namespace EIRS.Web.Controllers
{
    [SessionTimeout]
    public class HomeController : BaseController
    {
        EIRSContext _appDbContext = new EIRSContext();
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
        public ActionResult Pending2()
        {
            var mST_Users = new List<MST_Users>();
            using (var _db2 = new ERASEntities())
            {
                mST_Users = _db2.MST_Users.ToList();
            }
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
                var allAdj = _db.MAP_Assessment_Adjustment.Where(o => allAss.Select(x => x.AssessmentID).Contains(o.AAIID.Value)).ToList();
                var allLate = _db.MAP_Assessment_LateCharge.Where(o => allAss.Select(x => x.AssessmentID).Contains(o.AAIID.Value)).ToList();
                var allCom = _db.Companies.ToList();
                var allInd = _db.Individuals.ToList();
                var allGov = _db.Governments.ToList();

                foreach (var a in allAss)
                {
                    decimal? adjAmount = allAdj.Where(o => o.AAIID == a.AssessmentID)?.Sum(o => o.Amount);
                    decimal? ltAmount = allLate.Where(o => o.AAIID == a.AssessmentID)?.Sum(o => o.TotalAmount);
                    ApprovalFlow af = new ApprovalFlow();
                    switch (a.TaxPayerTypeID)
                    {
                        case 1:
                            var ind = allInd.FirstOrDefault(o => o.IndividualID == a.TaxPayerID);
                            af.TaxPayerName = $"{ind?.FirstName} {ind?.LastName}";
                            af.Rin = ind?.IndividualRIN;
                            af.Id = ind?.IndividualID;
                            break;
                        case 2:
                            var com = allCom.FirstOrDefault(o => o.CompanyID == a.TaxPayerID);
                            af.TaxPayerName = com?.CompanyName;
                            af.Rin = com?.CompanyRIN;
                            af.Id = com?.CompanyID;
                            break;
                        case 4:
                            var gov = allGov.FirstOrDefault(o => o.GovernmentID == a.TaxPayerID);
                            af.TaxPayerName = gov?.GovernmentName;
                            af.Rin = gov?.GovernmentRIN;
                            af.Id = gov?.GovernmentID;
                            break;
                    }
                    af.Status = System.Enum.GetName(typeof(SettlementStatus), a.SettlementStatusID);
                    af.AssessmentId = a.AssessmentID;
                    af.Amount = Math.Round(Convert.ToDouble(a.AssessmentAmount.GetValueOrDefault() + adjAmount + ltAmount), 2);
                    af.AssessmentRefNo = a.AssessmentRefNo;
                    retList.Add(af);
                }
            }
            ViewBag.ProfileInformation = retList;
            return View(retList);
        }
        public ActionResult Pending()
        {
            var lll = new List<long>();
            int det = 0;
            using (var _con = new EIRSEntities())
            {
                var allTax = _con.Tax_Offices.Where(o => o.OfficeManagerID == SessionManager.UserID);
                if (!allTax.Any())
                { allTax = _con.Tax_Offices.Where(o => o.IncomeDirector == SessionManager.UserID); det = 1; }
                else
                    det = 2;
                if (!allTax.Any())
                    return RedirectToAction("Unauthorised");
            }
            var retList = new List<usp_GetAssessmentForPendingOrDeclined_Result>();

            var doc = new Dictionary<string, string>();

            retList= getSPList().Where(o => o.SettlementStatusID != (int)SettlementStatus.Disapproved).ToList();

            switch (det)
            {
                case 1:
                    retList = retList.Where(o => o.Amount > 100000).ToList();
                    break;
                case 2:
                    retList = retList.Where(o => o.Amount <= 100000).ToList();
                    break;
                default:
                    break;
            }
            var k = retList.Select(o => o.AssessmentID).ToList();
            lll.AddRange(k);
            using (var _db = new EIRSEntities())
            {
                var allAdj = _db.MAP_Assessment_Adjustment.Where(o => lll.Contains(o.AAIID.Value)).ToList();
                var allLate = _db.MAP_Assessment_LateCharge.Where(o => lll.Contains(o.AAIID.Value)).ToList();
                foreach (var ri in retList)
                    ri.Amount = Convert.ToDouble(ri.Amount + (Convert.ToDouble(allAdj.Where(o => o.AAIID == ri.AssessmentID)?.Sum(o => o.Amount) + allLate.Where(o => o.AAIID == ri.AssessmentID)?.Sum(o => o.TotalAmount))));
            }
            ViewBag.ProfileInformation = retList;
            return View(retList);
        }
        public ActionResult Declined()
        {
            var lll = new List<long>();
            int det = 0;
            using (var _con = new EIRSEntities())
            {
                var allTax = _con.Tax_Offices.Where(o => o.OfficeManagerID == SessionManager.UserID);
                if (!allTax.Any())
                { allTax = _con.Tax_Offices.Where(o => o.IncomeDirector == SessionManager.UserID); det = 1; }
                else
                    det = 2;
                if (!allTax.Any())
                    return RedirectToAction("Unauthorised");
            }
            var retList = new List<usp_GetAssessmentForPendingOrDeclined_Result>();

            retList = getSPList().Where(o => o.SettlementStatusID == (int)SettlementStatus.Disapproved).ToList();

            switch (det)
            {
                case 1:
                    retList = retList.Where(o => o.Amount > 100000).ToList();
                    break;
                case 2:
                    retList = retList.Where(o => o.Amount <= 100000).ToList();
                    break;
                default:
                    break;
            }
            var k = retList.Select(o => o.AssessmentID).ToList();
            lll.AddRange(k);
            using (var _db = new EIRSEntities())
            {
                var allAdj = _db.MAP_Assessment_Adjustment.Where(o => lll.Contains(o.AAIID.Value)).ToList();
                var allLate = _db.MAP_Assessment_LateCharge.Where(o => lll.Contains(o.AAIID.Value)).ToList();
                foreach (var ri in retList)
                    ri.Amount = Convert.ToDouble(ri.Amount + (Convert.ToDouble(allAdj.Where(o => o.AAIID == ri.AssessmentID)?.Sum(o => o.Amount) + allLate.Where(o => o.AAIID == ri.AssessmentID)?.Sum(o => o.TotalAmount))));
            }
            ViewBag.ProfileInformation = retList;
            return View(retList);
            //var retList = new List<usp_GetAssessmentForPendingOrDeclined_Result>();
            //using (var _db = new EIRSEntities())
            //{
            //    int det = 0;
            //    var doc = new Dictionary<string, string>();

            //    var allTax = _db.Tax_Offices.Where(o => o.OfficeManagerID == SessionManager.UserID);
            //    if (!allTax.Any())
            //    { allTax = _db.Tax_Offices.Where(o => o.IncomeDirector == SessionManager.UserID); det = 1; }
            //    else
            //        det = 2;
            //    if (!allTax.Any())
            //        return RedirectToAction("Unauthorised");
            //    retList = _db.usp_GetAssessmentForPendingOrDeclined().Where(o => o.Status == ((int)SettlementStatus.Disapproved).ToString()).ToList();

            //    switch (det)
            //    {
            //        case 1:
            //            retList = retList.Where(o => o.Amount > 100000).ToList();
            //            break;
            //        case 2:
            //            retList = retList.Where(o => o.Amount <= 100000).ToList();
            //            break;
            //        default:
            //            break;
            //    }

            //    var allAdj = _db.MAP_Assessment_Adjustment.Where(o => retList.Select(x => x.AssessmentID).Contains(o.AAIID.Value)).ToList();
            //    var allLate = _db.MAP_Assessment_LateCharge.Where(o => retList.Select(x => x.AssessmentID).Contains(o.AAIID.Value)).ToList();
            //    foreach (var ri in retList)
            //        ri.Amount = Convert.ToDouble(ri.Amount + (Convert.ToDouble(allAdj.Where(o => o.AAIID == ri.AssessmentID)?.Sum(o => o.Amount) + allLate.Where(o => o.AAIID == ri.AssessmentID)?.Sum(o => o.TotalAmount))));

            //}
            //ViewBag.ProfileInformation = retList;
            //return View(retList);
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

        [NonAction]
        private List<usp_GetAssessmentForPendingOrDeclined_Result> getSPList()
        {

           var res= _appDbContext.usp_GetAssessmentForPendingOrDeclined();
            return res.GroupBy(o=>o.AssessmentID)

                .Select(group => group.First()).ToList();
        }
    }


}