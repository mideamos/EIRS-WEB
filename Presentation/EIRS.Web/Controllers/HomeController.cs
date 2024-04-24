using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;
using EIRS.Repository;
using EIRS.Web.GISModels;
using Google.Protobuf.WellKnownTypes;
using SixLabors.ImageSharp.Drawing;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using static EIRS.Common.EnumList;
using static EIRS.Web.Controllers.Filters;

namespace EIRS.Web.Controllers
{
    [SessionTimeout]
    public class HomeController : BaseController
    {
        EIRSContext _appDbContext = new EIRSContext();
        IAssessmentRepository _AssessmentRepository;
        IAdjustmentRepository _AdjustmentRepository;

        public HomeController()
        {
            _AssessmentRepository = new AssessmentRepository();
            _AdjustmentRepository = new AdjustmentRepository();
        }
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
        public async Task<ActionResult> Pending()
        {
            decimal? newAmount = 0;
            decimal? totalnewAmount = 0;
            decimal? totalnewAmountLateCharge = 0;
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
            retList = getSPList();


            //   retList = await getSPListAsync();
            retList = retList.Where(o => o.SettlementStatusID != (int)SettlementStatus.Disapproved).ToList();


            var lstOfBillId = retList.Select(o => o.AssessmentID).ToList();

            var lstAAIID = _AdjustmentRepository.GetListOfItemId(lstOfBillId);
            //lstAAIID
            var aaiid = lstAAIID.Select(o => o.AAIID).ToList();
            var adjustmentResponse = _AdjustmentRepository.GetAdjustmentResponse(aaiid);
            var lateChargeResponse = _AdjustmentRepository.GetLateChargeResponse(aaiid);
            foreach (var ass in retList)
            {
                var naaiid = lstAAIID.Where(o => o.BillId == ass.AssessmentID).ToList();
                if (naaiid.Count > 1)
                {
                    foreach (var aa in naaiid)
                    {
                        var lti = lateChargeResponse.Where(o => o.AAIID == aa.AAIID).Select(x => x.TotalAmount).FirstOrDefault();
                        newAmount = adjustmentResponse.Where(o => o.AAIID == aa.AAIID).Select(x => x.TotalAmount).FirstOrDefault();
                        totalnewAmount = newAmount == null ? totalnewAmount + 0 : totalnewAmount + newAmount;
                        totalnewAmountLateCharge = lti == null ? totalnewAmountLateCharge + 0 : totalnewAmountLateCharge + lti;

                    }
                }
                else
                {
                    var lt = lateChargeResponse.Where(o => o.AAIID == naaiid.FirstOrDefault().AAIID).Select(x => x.TotalAmount).FirstOrDefault();
                    newAmount = adjustmentResponse.Where(o => o.AAIID == naaiid.FirstOrDefault().AAIID).Select(x => x.TotalAmount).FirstOrDefault();
                    totalnewAmount = newAmount == null ? totalnewAmount + 0 : totalnewAmount + newAmount;
                    totalnewAmountLateCharge = lt == null ? totalnewAmountLateCharge + 0 : totalnewAmountLateCharge + lt;
                }

                ass.Amount = ass.Amount + totalnewAmount.GetValueOrDefault() + totalnewAmountLateCharge.GetValueOrDefault();
                totalnewAmount = 0;
                totalnewAmountLateCharge = 0;
            }
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
            ViewBag.ProfileInformation = retList;
            return View(retList);
        }
        public async Task<ActionResult> Declined()
        {
            decimal? newAmount = 0;
            decimal? totalnewAmount = 0;
            decimal? totalnewAmountLateCharge = 0;
            var lll = new List<long>();
            //int det = 0;
            //using (var _con = new EIRSEntities())
            //{
            //    var allTax = _con.Tax_Offices.Where(o => o.OfficeManagerID == SessionManager.UserID);
            //    if (!allTax.Any())
            //    { allTax = _con.Tax_Offices.Where(o => o.IncomeDirector == SessionManager.UserID); det = 1; }
            //    else
            //        det = 2;
            //    if (!allTax.Any())
            //        return RedirectToAction("Unauthorised");
            //}
            var retList = new List<usp_GetAssessmentForPendingOrDeclined_Result>();

            retList = getSPList();
            retList = retList.Where(o => o.SettlementStatusID == (int)SettlementStatus.Disapproved).ToList();


            var lstOfBillId = retList.Select(o => o.AssessmentID).ToList();

            var lstAAIID = _AdjustmentRepository.GetListOfItemId(lstOfBillId);
            //lstAAIID
            var aaiid = lstAAIID.Select(o => o.AAIID).ToList();
            var adjustmentResponse = _AdjustmentRepository.GetAdjustmentResponse(aaiid);
            var lateChargeResponse = _AdjustmentRepository.GetLateChargeResponse(aaiid);
            foreach (var ass in retList)
            {
                var naaiid = lstAAIID.Where(o => o.BillId == ass.AssessmentID).ToList();
                if (naaiid.Count > 1)
                {
                    foreach (var aa in naaiid)
                    {
                        var lti = lateChargeResponse.Where(o => o.AAIID == aa.AAIID).Select(x => x.TotalAmount).FirstOrDefault();
                        newAmount = adjustmentResponse.Where(o => o.AAIID == aa.AAIID).Select(x => x.TotalAmount).FirstOrDefault();
                        totalnewAmount = newAmount == null ? totalnewAmount + 0 : totalnewAmount + newAmount;
                        totalnewAmountLateCharge = lti == null ? totalnewAmountLateCharge + 0 : totalnewAmountLateCharge + lti;

                    }
                }
                else
                {
                    var lt = lateChargeResponse.Where(o => o.AAIID == naaiid.FirstOrDefault().AAIID).Select(x => x.TotalAmount).FirstOrDefault();
                    newAmount = adjustmentResponse.Where(o => o.AAIID == naaiid.FirstOrDefault().AAIID).Select(x => x.TotalAmount).FirstOrDefault();
                    totalnewAmount = newAmount == null ? totalnewAmount + 0 : totalnewAmount + newAmount;
                    totalnewAmountLateCharge = lt == null ? totalnewAmountLateCharge + 0 : totalnewAmountLateCharge + lt;
                }

                ass.Amount = ass.Amount + totalnewAmount.GetValueOrDefault() + totalnewAmountLateCharge.GetValueOrDefault();
                totalnewAmount = 0;
                totalnewAmountLateCharge = 0;
            }
            retList = retList.Where(o => o.UserID == SessionManager.UserID).ToList();

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

        [NonAction]
        private List<usp_GetAssessmentForPendingOrDeclined_Result> getSPList()
        {
            //var res = _appDbContext.usp_GetAssessmentForPendingOrDeclined();
            var rawQuery = "SELECT ast.AssessmentID,ss.SettlementStatusID,office.UserID, ast.AssessmentRefNo,ast.assessmentamount as Amount,ss.SettlementStatusName as Status,office.ContactName as TaxOfficerName,ast.TaxPayerID as ID,dbo.GetTaxPayerName(ast.TaxPayerID,ast.TaxPayerTypeID) as TaxPayerName,dbo.GetTaxPayerRIN(ast.TaxPayerID,ast.TaxPayerTypeID) as TaxPayerRIN,dbo.GetTaxPayerTaxOfficeName(ast.TaxPayerID,ast.TaxPayerTypeID) as TaxOfficeName FROM Assessment ast left JOIN Settlement_Status ss  ON ast.SettlementStatusID = ss.SettlementStatusID left JOIN TaxPayer_Types tptype ON ast.TaxPayerTypeID = tptype.TaxPayerTypeID   left JOIN ERAS.DBO.MST_Users  office ON ast.CreatedBy = office.UserID where ast.SettlementStatusID = 6  or ast.SettlementStatusID =8 or ast.SettlementStatusID =7";
            // List to hold the results
            List<usp_GetAssessmentForPendingOrDeclined_Result> results = new List<usp_GetAssessmentForPendingOrDeclined_Result>();

            string con = ConfigurationManager.ConnectionStrings["DbEntities"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(con))
            {
                using (SqlCommand command = new SqlCommand(rawQuery, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(new usp_GetAssessmentForPendingOrDeclined_Result
                            {
                                AssessmentID = Convert.ToInt64(reader["AssessmentID"]),
                                SettlementStatusID = Convert.ToInt32(reader["SettlementStatusID"]),
                                ID = Convert.ToInt32(reader["ID"]),
                                UserID = Convert.ToInt32(reader["UserID"]),
                                Amount = Convert.ToDecimal(reader["Amount"]),
                                AssessmentRefNo = reader["AssessmentRefNo"].ToString(),
                                Status = reader["Status"].ToString(),
                                TaxOfficeName = reader["TaxOfficeName"].ToString(),
                                TaxOfficerName = reader["TaxOfficerName"].ToString(),
                                TaxPayerName = reader["TaxPayerName"].ToString(),
                                TaxPayerRIN = reader["TaxPayerRIN"].ToString(),
                            });

                        }
                    }
                }
            }

            return results;
        }
    }
}