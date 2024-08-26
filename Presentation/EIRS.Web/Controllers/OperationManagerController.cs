

using ClosedXML.Excel;
using EFCore.BulkExtensions;
using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;
using EIRS.Web.GISModels;
using EIRS.Web.Models;
using Elmah;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Reflection;
using System.Transactions;
using System.Configuration;
using Microsoft.Data.SqlClient;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using Twilio.TwiML.Voice;
using Vereyon.Web;
using static EIRS.Web.Controllers.Filters;
using DocumentFormat.OpenXml.Bibliography;
using System.Security.Policy;

namespace EIRS.Web.Controllers
{
    [SessionTimeout]
    public class OperationManagerController : BaseController
    {
        EIRSEntities _db = new EIRSEntities();
        EIRSContext _appDbContext = new EIRSContext();

        public string getUrl()
        {
            string actionName = ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = ControllerContext.RouteData.Values["controller"].ToString();
            var ret = $"/{controllerName}/{actionName}";
            return ret;
        }
        [HttpGet]
        public ActionResult List()
        {
            string url = getUrl();
            bool itCan = new UtilityController().CheckAccess(url);
            if (!itCan) { return RedirectToAction("AccessDenied", "Utility"); }
            return View();
        }

        [HttpGet]
        public ActionResult LateChargeList()
        {
            List<MapReveuneStreamWithLateCharge> lstLateCharges = new List<MapReveuneStreamWithLateCharge>();
            var ret = from r in _db.Late_Charges
                      from p in _db.Revenue_Stream
                      where r.RevenueStreamID == p.RevenueStreamID
                      select new
                      {
                          Id = p.RevenueStreamID,
                          RevenueStreamName = p.RevenueStreamName,
                          TaxYear = r.TaxYear,
                          Penalty = r.Penalty,
                          Interest = r.Interest
                      };
            foreach (var l in ret)
            {
                MapReveuneStreamWithLateCharge mplt = new MapReveuneStreamWithLateCharge();
                mplt.Interest = (decimal)l.Interest;
                mplt.Penalty = (decimal)l.Penalty;
                mplt.RevenueStreamName = l.RevenueStreamName;
                mplt.TaxYear = (int)l.TaxYear;
                mplt.Id = l.Id;
                lstLateCharges.Add(mplt);
            }

            return View(lstLateCharges);
        }
        [HttpGet]
        public ActionResult TccDownload()
        {
            string url = getUrl();
            bool itCan = new UtilityController().CheckAccess(url);
            if (!itCan) { return RedirectToAction("AccessDenied", "Utility"); }
            var res = getSPList();
            return View(res);
        }

        [NonAction]
        private List<usp_GetTccDownloadByYearResult> getSPList()
        {
            long curentyear = DateTime.Now.Year - 1;
            var rawQuery = $"SELECT notf.TCCRequestID ,(nm.FirstName +' '+ nm.LastName) as Fullname ,nm.IndividualRIN ,notf.RequestRefNo,notf.Isdownloaded,  CASE WHEN ISNULL(notf.IsDownloaded, 0) = 0 THEN 'Awaiting Download'       ELSE 'Downloaded'   END as DownloadStatus,notf.RequestDate FROM TCC_Request  notf Left JOIN Individual  nm ON notf.TaxPayerID  = nm.IndividualID WHERE notf.TaxYear  = {curentyear} and notf.StatusID = 14 order by RequestDate desc ";
            // List to hold the results
            List<usp_GetTccDownloadByYearResult> results = new List<usp_GetTccDownloadByYearResult>();

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
                            results.Add(new usp_GetTccDownloadByYearResult
                            {
                                FullName = reader["Fullname"] != DBNull.Value ? reader["Fullname"].ToString() : "",
                                IndividualRIn = reader["IndividualRIN"] != DBNull.Value ? reader["IndividualRIN"].ToString() : "",
                                TccRefNo = reader["RequestRefNo"] != DBNull.Value ? reader["RequestRefNo"].ToString() : "",
                                RequestDate = reader["RequestDate"] != DBNull.Value ? Convert.ToDateTime(reader["RequestDate"]) : DateTime.Now,
                                TccId = reader["TCCRequestID"] != DBNull.Value ? Convert.ToInt64(reader["TCCRequestID"]) : 0,
                                IsDownloaded = reader["IsDownloaded"] != DBNull.Value ? Convert.ToBoolean(reader["IsDownloaded"]) : false,
                                DownloadStatus = reader["DownloadStatus"] != DBNull.Value ? reader["DownloadStatus"].ToString() : ""
                            });

                        }
                    }
                }
            }

            return results;
        }

        [HttpGet]
        public ActionResult Download(long? reqid)
        {
            if (reqid.GetValueOrDefault() > 0)
            {
                TCC_Request mObjRequestData = _db.TCC_Request.FirstOrDefault(o => o.TCCRequestID == reqid);

                if (mObjRequestData != null)
                {
                    mObjRequestData.IsDownloaded = true;
                    _db.SaveChanges();

                    return File(GlobalDefaultValues.DocumentLocation + mObjRequestData.SignedVisiblePath, "application/force-download", mObjRequestData.RequestRefNo.Trim() + ".pdf");
                }
                else
                {
                    return Content("Document Not Found");
                }
            }
            else
            {
                return Content("Document Not Found");
            }
        }
        [HttpGet]
        public ActionResult LateChargeDetail(string profName, int txYear, int id)
        {
            Profile profile = new Profile();
            List<map_assessmet_rule_response> lstmAARS = new List<map_assessmet_rule_response>();
            MAP_Assessment_AssessmentRule mAAR = new MAP_Assessment_AssessmentRule();
            Late_Charges lc = new Late_Charges();
            List<MAP_Assessment_AssessmentRule> lstMAAR = new List<MAP_Assessment_AssessmentRule>();
            List<Profile> lstprofile = new List<Profile>();
            List<int> listOfProfileIds = new List<int>();
            Profile_Types profTypes = new Profile_Types();
            //3a
            profTypes = _db.Profile_Types.FirstOrDefault(o => o.ProfileTypeName == profName);

            //3b
            listOfProfileIds = _db.Profiles.Where(o => o.ProfileTypeID == profTypes.ProfileTypeID).Select(x => x.ProfileID).ToList();
            //3c
            lstMAAR = _db.MAP_Assessment_AssessmentRule.Where(o => o.AssessmentYear == txYear).ToList();
            lstMAAR = lstMAAR.Where(x => listOfProfileIds.Contains(x.ProfileID.Value)).ToList();



            lc = _db.Late_Charges.FirstOrDefault(o => o.TaxYear == txYear && o.RevenueStreamID == id);
            //3e and 3f
            var glcl = _db.usp_GetLateChargeList(lc.LateChargeID, id).ToList();

            foreach (var item in lstMAAR)
            {
                map_assessmet_rule_response utl = new map_assessmet_rule_response();
                utl.AARID = item.AARID;
                utl.ProfileID = item.ProfileID;
                utl.AssetID = item.AssetID;
                utl.AssetTypeID = item.AssetTypeID;
                utl.AssessmentYear = item.AssessmentYear;
                utl.AssessmentAmount = item.AssessmentAmount;
                utl.AssessmentID = item.AssessmentID;
                utl.AssessmentRuleID = item.AssessmentRuleID;
                lstmAARS.Add(utl);
            }
            SessionManager.LstmAAR = lstmAARS;
            SessionManager.Lstlcr = glcl;
            SessionManager.lCharge = lc;
            SessionManager.revenueName = profName;
            SessionManager.revenueTaxYear = txYear.ToString();
            return View();
        }
        [HttpGet]
        public ActionResult LateChargeDetailII(int step)
        {
            //IDictionary<string, object> dcResponse = new Dictionary<string, object>();
            //3d
            List<usp_GetAssessmentRuleItemListForLateCharges_Result> resVal = new List<usp_GetAssessmentRuleItemListForLateCharges_Result>();
            List<map_assessmet_rule_response> lstmAARS = new List<map_assessmet_rule_response>();
            List<usp_GetLateChargeList_Result> Lstlcr = new List<usp_GetLateChargeList_Result>();

            lstmAARS = SessionManager.LstmAAR ?? null;
            Lstlcr = SessionManager.Lstlcr ?? null;
            if (step != 1)
                resVal = SessionManager.LstRil;

            if (lstmAARS.Count > 0)
            {
                if (step == 1)
                {
                    var newlstmAARS = lstmAARS.Skip(0).Take(1000).ToList();
                    foreach (var item in newlstmAARS)
                    {
                        var res = _db.usp_GetAssessmentRuleItemListForLateCharges(Convert.ToInt32(item.AssessmentID)).ToList();
                        //var resII = _db.Assessments.FirstOrDefault(o => o.AssessmentID == item.AssessmentID);
                        //LstAss.Add(resII);
                        resVal.AddRange(res);
                    }
                    SessionManager.LstRil = resVal;
                    //  SessionManager.lstAssessment = LstAss;
                }
                else if (step == 2)
                {
                    if (resVal.Count > 0)
                    {
                        int k = lstmAARS.Count;
                        k = k - 1000;
                        lstmAARS = lstmAARS.Skip(1000).Take(k).ToList();
                        foreach (var item in lstmAARS)
                        {
                            var res = _db.usp_GetAssessmentRuleItemListForLateCharges(Convert.ToInt32(item.AssessmentID)).ToList();
                            // var resII = _db.Assessments.FirstOrDefault(o => o.AssessmentID == item.AssessmentID);
                            //LstAss.Add(resII);
                            resVal.AddRange(res);
                        }
                        SessionManager.LstRil = resVal;
                        // SessionManager.lstAssessment = LstAss;
                        return RedirectToAction("LateChargeDetailStep3");
                    }
                }
                else
                {
                    var newResVal = resVal.Where(x => x.PendingAmount > 0).ToList();
                    foreach (var r in newResVal)
                    {
                        r.TotalAmount = r.PendingAmount + r.LC_Penatly + r.LC_Interest;
                        r.LC_Interest = r.PendingAmount * r.LC_Interest;
                        r.PendingAmount = r.PendingAmount * r.LC_Penatly;
                    }
                }
            }


            //return Json(dcResponse, JsonRequestBehavior.AllowGet);
            return View(resVal);
        }

        public ActionResult LateChargeDetailStep3()
        {
            return View();
        }

        [HttpGet]
        public ActionResult LateChargeDetailStep4()
        {
            List<Assessment> LstAss = new List<Assessment>();
            List<LateChargeResponse> lstlateChargeResponses = new List<LateChargeResponse>();

            List<map_assessmet_rule_response> lstmAARS = new List<map_assessmet_rule_response>();
            List<usp_GetAssessmentRuleItemListForLateCharges_Result> resVal = new List<usp_GetAssessmentRuleItemListForLateCharges_Result>();
            List<usp_GetAssessmentRuleItemListForLateCharges_Result> lstAvr = new List<usp_GetAssessmentRuleItemListForLateCharges_Result>();
            resVal = SessionManager.LstRil;
            lstmAARS = SessionManager.LstmAAR;
            if (resVal.Count > 0)
            {
                var newResVal = resVal.Where(x => x.PendingAmount > 0).ToList();
                foreach (var r in newResVal)
                {

                    LateChargeResponse Ass = new LateChargeResponse();
                    var avr = new usp_GetAssessmentRuleItemListForLateCharges_Result();
                    //avr.TotalAmount = r.PendingAmount + (r.LC_Penatly /100)+( r.LC_Interest/100);
                    avr.LC_Interest = r.PendingAmount * (r.LC_Interest / 100);
                    avr.LC_Penatly = r.PendingAmount * (r.LC_Penatly / 100);
                    avr.PendingAmount = r.PendingAmount;

                    lstAvr.Add(avr);

                    Ass = (from a in _db.Assessments
                           join m in _db.MAP_Assessment_AssessmentRule
                           on a.AssessmentID equals m.AssessmentID
                           join t in _db.MAP_Assessment_AssessmentItem
                           on m.AARID equals t.AARID
                           join i in _db.Individuals
                           on a.TaxPayerID equals i.IndividualID
                           where t.AAIID == r.AAIID
                           select new LateChargeResponse
                           {

                               TotalAmount = r.TotalAmount.Value,
                               AssessmentRefNo = a.AssessmentRefNo,
                               TaxPayerId = a.TaxPayerID.ToString(),
                               TaxPayerName = i.FirstName + " " + i.LastName,
                               TaxPayerRIN = i.IndividualRIN,
                               AssessmentItemID = (int)t.AssessmentItemID
                           }).FirstOrDefault();

                    if (!lstlateChargeResponses.Any(o => o.AssessmentRefNo == Ass.AssessmentRefNo) && Ass.AssessmentItemID != 2569)
                    {
                        Ass.AAIID = r.AAIID.Value;
                        Ass.PendingAmount = avr.PendingAmount.Value;
                        Ass.SettlementAmount = r.SettlementAmount.Value;
                        Ass.LateChargeAmount = avr.LC_Interest.Value + avr.LC_Penatly.Value;
                        Ass.LC_Penatly = avr.LC_Penatly.Value;
                        Ass.LC_Interest = avr.LC_Interest.Value;
                        lstlateChargeResponses.Add(Ass);
                    }
                }
            }


            ViewBag.revName = SessionManager.revenueName;
            ViewBag.txYear = SessionManager.revenueTaxYear;
            SessionManager.LateChargeResponse = lstlateChargeResponses;
            return View(lstlateChargeResponses);
        }
        public ActionResult InserttoMapassessmentLateCharge()
        {
            List<LateChargeResponse> lstcharge = new List<LateChargeResponse>();
            lstcharge = SessionManager.LateChargeResponse;
            List<MAP_Assessment_LateCharge> lstassLC = new List<MAP_Assessment_LateCharge>();


            foreach (var item in lstcharge)
            {
                MAP_Assessment_LateCharge MLC = new MAP_Assessment_LateCharge();
                MLC.AAIID = item.AAIID;
                MLC.Penalty = item.LC_Penatly;
                MLC.Interest = item.LC_Interest;
                MLC.TotalAmount = item.LC_Interest + item.LC_Penatly;
                MLC.ChargeDate = item.ChargeDate;
                MLC.CreatedDate = item.CreatedDate;
                MLC.CreatedBy = Convert.ToInt32(item.CreatedBy);
                lstassLC.Add(MLC);
            }
            _db.MAP_Assessment_LateCharge.AddRange(lstassLC);
            _db.SaveChanges();
            return View(lstassLC);

        }

        #region OM001 : PoA Transfer
        private void UI_FillDropDown(PaymentTransferViewModel pobjPaymentViewModel = null)
        {
            if (pobjPaymentViewModel == null)
            {
                pobjPaymentViewModel = new PaymentTransferViewModel();
            }

            UI_FillTaxPayerTypeDropDown();
            UI_FillRevenueStreamDropDown();
        }
        [HttpGet]
        public ActionResult PoAOperationList()
        {
            string url = getUrl();
            bool itCan = new UtilityController().CheckAccess(url);
            if (!itCan) { return RedirectToAction("AccessDenied", "Utility"); }
            return View();
        }
        public JsonResult PoAOperationLoadData()
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? Convert.ToInt32(vLength) : 0;
            int IntSkip = vStart != null ? Convert.ToInt32(vStart) : 0;
            int IntTotalRecords = 0;


            IList<vw_PaymentAccountOperation> lstPaymentAccountOperation = new BLPaymentAccount().BL_GetPaymentAccountOperationList();
            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstPaymentAccountOperation = lstPaymentAccountOperation.Where(t => t.OperationDate != null && t.OperationDate.Value.ToString("dd-MM-yyyy").Contains(vFilter)
                || t.Operation_TypesName != null && t.Operation_TypesName.ToLower().Trim().Contains(vFilter.ToLower().Trim())
                || t.FromTaxPayer != null && t.FromTaxPayer.ToLower().Trim().Contains(vFilter.ToLower().Trim())
                || t.FromName != null && t.FromName.ToLower().Trim().Contains(vFilter.ToLower().Trim())
                || t.Amount != null && t.Amount.ToString().Contains(vFilter)
                || t.ToTaxPayer != null && t.ToTaxPayer.ToLower().Trim().Contains(vFilter.ToLower().Trim())
                || t.ToName != null && t.ToName.ToLower().Trim().Contains(vFilter.ToLower().Trim())).ToList();
            }


            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstPaymentAccountOperation = lstPaymentAccountOperation.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstPaymentAccountOperation.Count();
            var data = lstPaymentAccountOperation.Skip(IntSkip).Take(IntPageSize).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PoATransfer(int? taxpayerid, int? taxpayertypeid)
        {
            string url = getUrl();
            bool itCan = new UtilityController().CheckAccess(url);
            if (!itCan) { return RedirectToAction("AccessDenied", "Utility"); }
            if (taxpayerid > 0)
            {
                PaymentTransferViewModel mObjPaymentTransferViewModel = new PaymentTransferViewModel()
                {
                    FromTaxPayerID = taxpayerid.GetValueOrDefault(),
                    FromTaxPayerTypeID = taxpayertypeid.GetValueOrDefault(),
                    ToTaxPayerName = "",
                };

                if (mObjPaymentTransferViewModel.FromTaxPayerTypeID == (int)EnumList.TaxPayerType.Individual)
                {
                    usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(new Individual() { intStatus = 2, IndividualID = taxpayerid.GetValueOrDefault() });
                    mObjPaymentTransferViewModel.FromTaxPayerName = mObjIndividualData.FirstName + " " + mObjIndividualData.LastName;
                }
                else if (mObjPaymentTransferViewModel.FromTaxPayerTypeID == (int)EnumList.TaxPayerType.Companies)
                {
                    mObjPaymentTransferViewModel.FromTaxPayerName = new BLCompany().BL_GetCompanyDetails(new Company() { intStatus = 2, CompanyID = taxpayerid.GetValueOrDefault() }).CompanyName;
                }
                else if (mObjPaymentTransferViewModel.FromTaxPayerTypeID == (int)EnumList.TaxPayerType.Government)
                {
                    mObjPaymentTransferViewModel.FromTaxPayerName = new BLGovernment().BL_GetGovernmentDetails(new Government() { intStatus = 2, GovernmentID = taxpayerid.GetValueOrDefault() }).GovernmentName;
                }
                else if (mObjPaymentTransferViewModel.FromTaxPayerTypeID == (int)EnumList.TaxPayerType.Special)
                {
                    mObjPaymentTransferViewModel.FromTaxPayerName = new BLSpecial().BL_GetSpecialDetails(new Special() { intStatus = 2, SpecialID = taxpayerid.GetValueOrDefault() }).SpecialTaxPayerName;
                }

                UI_FillDropDown(mObjPaymentTransferViewModel);
                return View(mObjPaymentTransferViewModel);
            }
            else
            {

                UI_FillDropDown();
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult PoATransfer(PaymentTransferViewModel pobjPaymentViewModel)
        {

            if (pobjPaymentViewModel.FromTaxPayerID != pobjPaymentViewModel.ToTaxPayerID)
            {
                PoaMyClass myClass = SessionManager.poaMyClass ?? new PoaMyClass();

                if (myClass.PaymentAccountID <= 0)
                {
                    UI_FillDropDown();
                    ViewBag.Message = "Please Enter Transaction Ref No";
                    return View(pobjPaymentViewModel);
                }
                if (pobjPaymentViewModel.Amount <= 0 || pobjPaymentViewModel.Amount == null)
                {
                    UI_FillDropDown();
                    ViewBag.Message = "Please Enter Amount";
                    return View(pobjPaymentViewModel);
                }
                MAP_PaymentAccount_Operation mObjPaymentTransfer = new MAP_PaymentAccount_Operation()
                {
                    OperationTypeID = (int)EnumList.OperationType.Transfer,
                    From_TaxPayerTypeID = pobjPaymentViewModel.FromTaxPayerTypeID,
                    From_TaxPayerID = pobjPaymentViewModel.FromTaxPayerID,
                    To_TaxPayerTypeID = pobjPaymentViewModel.ToTaxPayerTypeID,
                    POAAccountId = Convert.ToInt32(myClass.PaymentAccountID),
                    To_TaxPayerID = pobjPaymentViewModel.ToTaxPayerID,
                    TransactionRefNo = myClass.TransactionRefNo,
                    Amount = pobjPaymentViewModel.Amount,
                    OperationDate = CommUtil.GetCurrentDateTime(),
                    CreatedBy = SessionManager.UserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                //decimal Balance = new BLPaymentAccount().BL_GetWalletBalance(pobjPaymentViewModel.FromTaxPayerTypeID, pobjPaymentViewModel.FromTaxPayerID);

                try
                {
                    if (myClass.BA >= pobjPaymentViewModel.Amount)
                    {
                        FuncResponse mObjResponse = new BLPaymentAccount().BL_InsertPaymentOperation(mObjPaymentTransfer);

                        if (mObjResponse.Success)
                        {
                            Audit_Log mObjAuditLog = new Audit_Log()
                            {
                                LogDate = CommUtil.GetCurrentDateTime(),
                                ASLID = (int)EnumList.ALScreen.Operation_Manager_PoA_Transfer,
                                Comment = $"PoA Transfer from {pobjPaymentViewModel.FromTaxPayerName} to {pobjPaymentViewModel.ToTaxPayerName} of Amount {pobjPaymentViewModel.Amount}",
                                IPAddress = CommUtil.GetIPAddress(),
                                StaffID = SessionManager.UserID,
                            };

                            new BLAuditLog().BL_InsertAuditLog(mObjAuditLog);

                            UI_FillDropDown();
                            ModelState.Clear();
                            ViewBag.Message = "Transaction Done Successful";
                            Session.Remove("poaMyClass");
                            return View();
                        }
                        else
                        {
                            UI_FillDropDown();
                            ViewBag.Message = mObjResponse.Message;
                            return View(pobjPaymentViewModel);
                        }
                    }
                    else
                    {
                        UI_FillDropDown();
                        ViewBag.Message = "Insufficient Balance";
                        return View(pobjPaymentViewModel);
                    }
                }
                catch (Exception ex)
                {
                    UI_FillDropDown();
                    ViewBag.Message = "Error occurred while Transfer";
                    return View(pobjPaymentViewModel);
                }
            }
            else
            {
                UI_FillDropDown();
                ViewBag.Message = "Can't send transfer to self";
                return View(pobjPaymentViewModel);
            }
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken()]
        //public ActionResult PoATransferValidate(PaymentTransferViewModel pobjPaymentViewModel)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        UI_FillDropDown();
        //        return View(pobjPaymentViewModel);
        //    }
        //    else
        //    {
        //        if (pobjPaymentViewModel.FromTaxPayerID != pobjPaymentViewModel.ToTaxPayerID)
        //        {
        //            MAP_PaymentAccount_Operation mObjPaymentTransfer = new MAP_PaymentAccount_Operation()
        //            {
        //                OperationTypeID = (int)EnumList.OperationType.Transfer,
        //                From_TaxPayerTypeID = pobjPaymentViewModel.FromTaxPayerTypeID,
        //                From_TaxPayerID = pobjPaymentViewModel.FromTaxPayerID,
        //                To_TaxPayerTypeID = pobjPaymentViewModel.ToTaxPayerTypeID,
        //                POAAccountId = pobjPaymentViewModel.POAAccountId,
        //                To_TaxPayerID = pobjPaymentViewModel.ToTaxPayerID,
        //                Amount = pobjPaymentViewModel.Amount,
        //                OperationDate = CommUtil.GetCurrentDateTime(),
        //                CreatedBy = SessionManager.UserID,
        //                CreatedDate = CommUtil.GetCurrentDateTime()
        //            };

        //            decimal Balance = new BLPaymentAccount().BL_GetWalletBalance(pobjPaymentViewModel.FromTaxPayerTypeID, pobjPaymentViewModel.FromTaxPayerID);

        //            try
        //            {
        //                if (Balance >= pobjPaymentViewModel.Amount)
        //                {
        //                    FuncResponse mObjResponse = new BLPaymentAccount().BL_InsertPaymentOperation(mObjPaymentTransfer);

        //                    if (mObjResponse.Success)
        //                    {
        //                        Audit_Log mObjAuditLog = new Audit_Log()
        //                        {
        //                            LogDate = CommUtil.GetCurrentDateTime(),
        //                            ASLID = (int)EnumList.ALScreen.Operation_Manager_PoA_Transfer,
        //                            Comment = $"PoA Transfer from {pobjPaymentViewModel.FromTaxPayerName} to {pobjPaymentViewModel.ToTaxPayerName} of Amount {pobjPaymentViewModel.Amount}",
        //                            IPAddress = CommUtil.GetIPAddress(),
        //                            StaffID = SessionManager.UserID,
        //                        };

        //                        new BLAuditLog().BL_InsertAuditLog(mObjAuditLog);

        //                        UI_FillDropDown();
        //                        ModelState.Clear();
        //                        return View();
        //                    }
        //                    else
        //                    {
        //                        UI_FillDropDown();
        //                        ViewBag.Message = mObjResponse.Message;
        //                        return View(pobjPaymentViewModel);
        //                    }
        //                }
        //                else
        //                {
        //                    UI_FillDropDown();
        //                    ViewBag.Message = "Insufficient Balance";
        //                    return View(pobjPaymentViewModel);
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                UI_FillDropDown();
        //                ViewBag.Message = "Error occurred while Transfer";
        //                return View(pobjPaymentViewModel);
        //            }
        //        }
        //        else
        //        {
        //            UI_FillDropDown();
        //            ViewBag.Message = "Can't send transfer to self";
        //            return View(pobjPaymentViewModel);
        //        }
        //    }
        //} 


        public JsonResult PoATransferValidate(string pid, int? pIntTaxPayerTypeID, int? pIntTaxPayerID)
        {
            decimal? reciedAmount = 0; decimal? sentAmount = 0; decimal? newbalance = 0;
            string noUser = "";
            List<MAP_PaymentAccount_Operation> lstret = new List<MAP_PaymentAccount_Operation>();
            MAP_PaymentAccount_Operation ret = new MAP_PaymentAccount_Operation();
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(pid))
            {
                var res = _db.Payment_Account.FirstOrDefault(o => o.TransactionRefNo == pid);
                if (res != null)
                {
                    if (pIntTaxPayerID.Value == 0)
                    {
                        pIntTaxPayerID = SessionManager.TaxPayerIDForPoa;
                        if (pIntTaxPayerID.Value == 0)
                        {
                            dcResponse["success"] = false;
                            dcResponse["noUser"] = "Taxpayer Number not Found. Please Try again";
                            return Json(dcResponse, JsonRequestBehavior.AllowGet);
                        }
                    }
                    lstret = _db.MAP_PaymentAccount_Operation.Where(o => o.POAAccountId == res.PaymentAccountID).ToList();
                    if (lstret.Count > 0)
                    {

                        reciedAmount = lstret.Where(o => o.To_TaxPayerID == pIntTaxPayerID && o.To_TaxPayerTypeID == pIntTaxPayerTypeID).Sum(o => o.Amount);
                        sentAmount = lstret.Where(o => o.From_TaxPayerID == pIntTaxPayerID && o.From_TaxPayerTypeID == pIntTaxPayerTypeID).Sum(o => o.Amount);
                        newbalance = reciedAmount - sentAmount;

                        PoaMyClass myClass = new PoaMyClass() { TransactionRefNo = pid, BA = newbalance.Value, PaymentAccountID = res.PaymentAccountID, SA = sentAmount.Value, RA = reciedAmount.Value };
                        SessionManager.poaMyClass = myClass;
                    }

                    dcResponse["reciedAmount"] = CommUtil.GetFormatedCurrency(reciedAmount);
                    dcResponse["sentAmount"] = CommUtil.GetFormatedCurrency(sentAmount);
                    dcResponse["newbalance"] = CommUtil.GetFormatedCurrency(newbalance);
                    dcResponse["success"] = true;
                }
                else
                {

                    dcResponse["success"] = false;
                    dcResponse["noUser"] = "Transaction Doesnt Exist With This Reference Number";
                }
            }
            else
            {
                dcResponse["success"] = false;
            }
            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetBalance(int? pIntTaxPayerTypeID, int? pIntTaxPayerID)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();
            if (pIntTaxPayerTypeID != null && pIntTaxPayerID != null)
            {
                SessionManager.TaxPayerIDForPoa = pIntTaxPayerID.Value;
                decimal Balance = new BLPaymentAccount().BL_GetWalletBalance(pIntTaxPayerTypeID.GetValueOrDefault(), pIntTaxPayerID.GetValueOrDefault());

                dcResponse["Balance"] = "Your account Balance is " + CommUtil.GetFormatedCurrency(Balance);
                dcResponse["success"] = true;
            }
            else
            {
                dcResponse["success"] = false;
            }
            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region OM002 : Settlement Via PoA


        public ActionResult PoASettlement()
        {
            string url = getUrl();
            bool itCan = new UtilityController().CheckAccess(url);
            if (!itCan) { return RedirectToAction("AccessDenied", "Utility"); }
            return View();
        }


        public ActionResult PoASettlementData()
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]").FirstOrDefault();
            int IntPageSize = vLength != null ? Convert.ToInt32(vLength) : 0;
            int IntSkip = vStart != null ? Convert.ToInt32(vStart) : 0;
            int IntTotalRecords = 0;


            IList<vw_BillForPoASettlement> lstBillForPoASettlement = new BLAssessment().BL_GetBillForPoASettlementList();
            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstBillForPoASettlement = lstBillForPoASettlement.Where(t => t.BillDate != null && t.BillDate.Value.ToString("dd-MM-yyyy").Contains(vFilter)
                || t.BillRefNo != null && t.BillRefNo.ToLower().Trim().Contains(vFilter.ToLower().Trim())
                || t.TaxPayerName != null && t.TaxPayerName.ToLower().Trim().Contains(vFilter.ToLower().Trim())
                || t.BillAmount != null && t.BillAmount.ToString().Contains(vFilter)
                || t.SettlementStatusName != null && t.SettlementStatusName.ToLower().Trim().Contains(vFilter.ToLower().Trim())).ToList();

            }
            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstBillForPoASettlement = lstBillForPoASettlement.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstBillForPoASettlement.Count();
            var data = lstBillForPoASettlement.Skip(IntSkip).Take(IntPageSize).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateAssessmentNewItem(string rowdata)
        {
            int holderId = SessionManager.DataSubmitterID;
            if (rowdata != null && !rowdata.Contains("@@"))
            {
                //List NewPoASettlementViewModel newpoa = new List NewPoASettlementViewModel();
                IList<Assessment_AssessmentRule> lstAssessmentRules = (IList<Assessment_AssessmentRule>)ViewBag.AssessmentRuleList;
                IList<NewPoASettlementViewModel> poa = (IList<NewPoASettlementViewModel>)ViewBag.POS;
                ViewBag.AssessmentRuleList = SessionManager.lstAssessmentRule.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
                ViewBag.pos = SessionManager.LstPOS.ToList();
                IList<Assessment_AssessmentItem> lstAssessmentItems = SessionManager.lstAssessmentItem ?? new List<Assessment_AssessmentItem>();
                BLAssessment mObjBLAssessment = new BLAssessment();
                usp_GetAssessmentList_Result mObjAssessmentData = mObjBLAssessment.BL_GetAssessmentDetails(new Assessment() { AssessmentID = holderId, IntStatus = 1 });

                decimal dcBalance = new BLPaymentAccount().BL_GetWalletBalance(mObjAssessmentData.TaxPayerTypeID.GetValueOrDefault(), mObjAssessmentData.TaxPayerID.GetValueOrDefault());

                NewPoASettlementViewModel mObjSettlementModel2 = new NewPoASettlementViewModel()
                {
                    AssessmentID = mObjAssessmentData.AssessmentID.GetValueOrDefault(),
                    TaxPayerID = mObjAssessmentData.TaxPayerID.GetValueOrDefault(),
                    TaxPayerTypeID = mObjAssessmentData.TaxPayerTypeID.GetValueOrDefault(),
                    TaxPayerName = mObjAssessmentData.TaxPayerName,
                    TaxPayerTypeName = mObjAssessmentData.TaxPayerTypeName,
                    TaxPayerRIN = mObjAssessmentData.TaxPayerRIN,
                    BillDate = mObjAssessmentData.AssessmentDate,
                    DueDate = mObjAssessmentData.SettlementDueDate,
                    BillRefNo = mObjAssessmentData.AssessmentRefNo,
                    StatusName = mObjAssessmentData.SettlementStatusName,
                    BillNotes = mObjAssessmentData.AssessmentNotes,
                    BillAmount = mObjAssessmentData.AssessmentAmount,
                    TotalPaid = mObjAssessmentData.SettlementAmount,
                };
                ViewBag.PoABalance = dcBalance;

                SessionManager.LstPOS = poa;
                SessionManager.lstAssessmentRule = lstAssessmentRules;
                SessionManager.lstAssessmentRule = ViewBag.AssessmentRuleList;
                return View(mObjSettlementModel2);
            }
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            IList<Assessment_AssessmentItem> lstMDAServiceItems = SessionManager.lstAssessmentItem ?? new List<Assessment_AssessmentItem>();
            IList<Assessment_AssessmentRule> lstMDAServices = SessionManager.lstAssessmentRule ?? new List<Assessment_AssessmentRule>();
            Assessment_AssessmentRule mObjUpdateMDAService = new Assessment_AssessmentRule();
            Assessment_AssessmentItem mObjUpdateMDAServiceItem = new Assessment_AssessmentItem();
            if (!string.IsNullOrWhiteSpace(rowdata))
            {
                string[] strRowData = rowdata.Split(new string[] { "~~" }, StringSplitOptions.RemoveEmptyEntries);

                if (strRowData.Length > 0)
                {
                    foreach (var vRowData in strRowData)
                    {
                        string[] strServiceItemData = vRowData.Split(new string[] { "@@" }, StringSplitOptions.RemoveEmptyEntries);
                        if (strServiceItemData.Length == 2)
                        {
                            mObjUpdateMDAServiceItem = lstMDAServiceItems.Where(t => t.RowID == TrynParse.parseInt(strServiceItemData[0])).FirstOrDefault();

                            if (mObjUpdateMDAServiceItem != null)
                            {
                                decimal ServiceBaseAmount = TrynParse.parseDecimal(strServiceItemData[1]);

                                mObjUpdateMDAServiceItem.TaxBaseAmount = ServiceBaseAmount;

                                mObjUpdateMDAServiceItem.intTrack = mObjUpdateMDAServiceItem.TablePKID > 0 ? EnumList.Track.UPDATE : EnumList.Track.INSERT;

                                //Assessment_Items
                                mObjUpdateMDAService = lstMDAServices.Where(t => t.RowID == mObjUpdateMDAServiceItem.AssessmentRule_RowID).FirstOrDefault();

                                mObjUpdateMDAService.AssessmentRuleAmount = lstMDAServiceItems.Where(t => t.RowID == mObjUpdateMDAService.RowID).Sum(t => t.TaxBaseAmount);
                                mObjUpdateMDAService.intTrack = mObjUpdateMDAService.TablePKID > 0 ? EnumList.Track.UPDATE : EnumList.Track.INSERT;
                            }
                        }
                    }

                    ViewBag.AssessmentRuleList = lstMDAServices.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
                    IList<Assessment_AssessmentRule> lstAssessmentRules = (IList<Assessment_AssessmentRule>)ViewBag.AssessmentRuleList;
                    SessionManager.lstAssessmentRule = lstMDAServices;
                    SessionManager.lstAssessmentItem = lstMDAServiceItems;
                    dcResponse["success"] = true;
                    dcResponse["AssessmentRuleDetails"] = CommUtil.RenderPartialToString("_BindAssessmentRuleForAssessment", null, this.ControllerContext, ViewData);

                    decimal totalSum = lstAssessmentRules.Sum(t => t.AssessmentRuleAmount);
                    decimal totalSumForItem = lstMDAServiceItems.Sum(t => t.TaxBaseAmount);
                    if (holderId > 0)
                    {
                        BLAssessment mObjBLAssessment = new BLAssessment();
                        BLAssessmentItem mObjBLAssessmentItem = new BLAssessmentItem();
                        usp_GetAssessmentList_Result mObjAssessmentData = mObjBLAssessment.BL_GetAssessmentDetails(new Assessment() { AssessmentID = holderId, IntStatus = 1 });

                        IList<NewPoASettlementViewModel> poa = new List<NewPoASettlementViewModel>();
                        if (mObjAssessmentData != null)
                        {
                            decimal dcBalance = new BLPaymentAccount().BL_GetWalletBalance(mObjAssessmentData.TaxPayerTypeID.GetValueOrDefault(), mObjAssessmentData.TaxPayerID.GetValueOrDefault());

                            NewPoASettlementViewModel mObjSettlementModel = new NewPoASettlementViewModel()
                            {
                                AssessmentID = mObjAssessmentData.AssessmentID.GetValueOrDefault(),
                                TaxPayerID = mObjAssessmentData.TaxPayerID.GetValueOrDefault(),
                                TaxPayerTypeID = mObjAssessmentData.TaxPayerTypeID.GetValueOrDefault(),
                                TaxPayerName = mObjAssessmentData.TaxPayerName,
                                TaxPayerTypeName = mObjAssessmentData.TaxPayerTypeName,
                                TaxPayerRIN = mObjAssessmentData.TaxPayerRIN,
                                BillDate = mObjAssessmentData.AssessmentDate,
                                DueDate = mObjAssessmentData.SettlementDueDate,
                                BillRefNo = mObjAssessmentData.AssessmentRefNo,
                                StatusName = mObjAssessmentData.SettlementStatusName,
                                BillNotes = mObjAssessmentData.AssessmentNotes,
                                BillAmount = mObjAssessmentData.AssessmentAmount,
                                AmountToPay = totalSum,
                                TotalPaid = mObjAssessmentData.SettlementAmount,
                                HolderId = holderId,
                            };

                            poa.Add(mObjSettlementModel);
                            SessionManager.UserName = totalSum.ToString();
                            ViewBag.PoABalance = dcBalance;
                            //Assessment_AssessmentItem mObjAssessmentItem = new Assessment_AssessmentItem();
                            IList<Assessment_AssessmentRule> lstAssessmentRule = new List<Assessment_AssessmentRule>();
                            IList<Assessment_AssessmentItem> lstAssessmentItem = new List<Assessment_AssessmentItem>();

                            //Getting Assessment Rules
                            IList<usp_GetAssessment_AssessmentRuleList_Result> lstMAPAssessmentRules = mObjBLAssessment.BL_GetAssessmentRules(mObjAssessmentData.AssessmentID.GetValueOrDefault());
                            IList<usp_GetAssessmentRuleItemList_Result> lstAssessmentItems = mObjBLAssessment.BL_GetAssessmentRuleItem(mObjAssessmentData.AssessmentID.GetValueOrDefault());
                            IList<usp_GetAssessmentRuleBasedSettlement_Result> lstAssessmentRuleSettlement = mObjBLAssessment.BL_GetAssessmentRuleBasedSettlement(mObjAssessmentData.AssessmentID.GetValueOrDefault());

                            foreach (var item in lstMAPAssessmentRules)
                            {
                                Assessment_AssessmentRule assessment_AssessmentRule = new Assessment_AssessmentRule()
                                {
                                    RowID = lstAssessmentRule.Count + 1,
                                    TablePKID = item.AARID.GetValueOrDefault(),
                                    AssetTypeID = item.AssetTypeID.GetValueOrDefault(),
                                    AssetTypeName = item.AssetTypeName,
                                    AssetID = item.AssetID.GetValueOrDefault(),
                                    AssetRIN = item.AssetRIN,
                                    ProfileID = item.ProfileID.GetValueOrDefault(),
                                    ProfileDescription = item.ProfileDescription,
                                    AssessmentRuleID = item.AssessmentRuleID.GetValueOrDefault(),
                                    AssessmentRuleName = item.AssessmentRuleName,
                                    AssessmentRuleAmount = item.AssessmentRuleAmount.GetValueOrDefault(),
                                    TaxYear = item.TaxYear.GetValueOrDefault(),
                                    SettledAmount = item.SettledAmount,
                                    UnSettledAmount = item.AssessmentRuleAmount.GetValueOrDefault() - item.SettledAmount.GetValueOrDefault(),
                                    ToSettleAmount = item.AssessmentRuleAmount.GetValueOrDefault() - item.SettledAmount.GetValueOrDefault(),
                                    intTrack = EnumList.Track.INSERT,
                                };

                                lstAssessmentRule.Add(assessment_AssessmentRule);

                                //foreach (var subitem in lstAssessmentItems.Where(t => t.AARID == item.AARID))
                                //{
                                //    usp_GetAssessmentItemList_Result mObjAssessmentItemData = mObjBLAssessmentItem.BL_GetAssessmentItemDetails(new Assessment_Items() { intStatus = 2, AssessmentItemID = subitem.AssessmentItemID.GetValueOrDefault() });
                                //    Assessment_AssessmentItem mObjAssessmentItem = new Assessment_AssessmentItem()
                                //    {
                                //        RowID = lstAssessmentItem.Count + 1,
                                //        AssessmentRule_RowID = assessment_AssessmentRule.RowID,
                                //        TablePKID = subitem.AAIID.GetValueOrDefault(),
                                //        AssessmentItemID = subitem.AssessmentItemID.GetValueOrDefault(),
                                //        AssessmentItemName = mObjAssessmentItemData.AssessmentItemName,
                                //        AssessmentItemReferenceNo = mObjAssessmentItemData.AssessmentItemReferenceNo,
                                //        ComputationID = mObjAssessmentItemData.ComputationID.GetValueOrDefault(),
                                //        ComputationName = mObjAssessmentItemData.ComputationName,
                                //        TaxAmount = subitem.TaxAmount.GetValueOrDefault(),
                                //        AdjustmentAmount = subitem.AdjustmentAmount.GetValueOrDefault(),
                                //        LateChargeAmount = subitem.LateChargeAmount.GetValueOrDefault(),
                                //        TotalAmount = subitem.TotalAmount.GetValueOrDefault(),
                                //        SettlementAmount = subitem.SettlementAmount.GetValueOrDefault(),
                                //        UnSettledAmount = subitem.PendingAmount.GetValueOrDefault(),
                                //        ToSettleAmount = subitem.PendingAmount.GetValueOrDefault(),
                                //        PaymentStatusID = subitem.PaymentStatusID.GetValueOrDefault(),
                                //        intTrack = EnumList.Track.INSERT,
                                //    };
                                //    lstAssessmentItem.Add(mObjAssessmentItem);

                                //}
                            }
                            if (lstAssessmentRule.Count < 2)
                            {
                                //mObjSettlementModel.BillAmount = mObjAssessmentItem.ToSettleAmount;
                                //foreach (var c in lstAssessmentRule)
                                //{
                                //    c.ToSettleAmount = mObjAssessmentItem.ToSettleAmount;
                                //}
                            }
                            SessionManager.lstAssessmentRule = lstMDAServices;
                            SessionManager.LstPOS = poa;
                            SessionManager.lstAssessmentItem = lstAssessmentItem;
                            //lstAssessmentItems
                            SessionManager.lstAssessmentRuleSettlement = lstAssessmentRuleSettlement;
                            ViewBag.AssessmentRuleList = SessionManager.lstAssessmentRule.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
                            SessionManager.UserID = (int)SessionManager.lstAssessmentItem.Sum(o => o.TaxBaseAmount);
                            ViewBag.POS = SessionManager.LstPOS.ToList();
                            ViewBag.AmountToPay = lstAssessmentRule.Sum(t => t.ToSettleAmount);

                            return Json(dcResponse, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                else
                {
                    return Json(dcResponse, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(dcResponse, JsonRequestBehavior.AllowGet);
            }
            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AssessmentBillDetail(int? id, string name)
        {
            string url = getUrl();
            bool itCan = new UtilityController().CheckAccess(url);
            if (!itCan) { return RedirectToAction("AccessDenied", "Utility"); }
            if (id.GetValueOrDefault() > 0)
            {
                BLAssessment mObjBLAssessment = new BLAssessment();
                BLAssessmentItem mObjBLAssessmentItem = new BLAssessmentItem();
                usp_GetAssessmentList_Result mObjAssessmentData = mObjBLAssessment.BL_GetAssessmentDetails(new Assessment() { AssessmentID = id.GetValueOrDefault(), IntStatus = 1 });

                if (mObjAssessmentData != null)
                {
                    decimal dcBalance = new BLPaymentAccount().BL_GetWalletBalance(mObjAssessmentData.TaxPayerTypeID.GetValueOrDefault(), mObjAssessmentData.TaxPayerID.GetValueOrDefault());

                    PoASettlementViewModel mObjSettlementModel = new PoASettlementViewModel()
                    {
                        AssessmentID = mObjAssessmentData.AssessmentID.GetValueOrDefault(),
                        TaxPayerID = mObjAssessmentData.TaxPayerID.GetValueOrDefault(),
                        TaxPayerTypeID = mObjAssessmentData.TaxPayerTypeID.GetValueOrDefault(),
                        TaxPayerName = mObjAssessmentData.TaxPayerName,
                        TaxPayerTypeName = mObjAssessmentData.TaxPayerTypeName,
                        TaxPayerRIN = mObjAssessmentData.TaxPayerRIN,
                        BillDate = mObjAssessmentData.AssessmentDate,
                        DueDate = mObjAssessmentData.SettlementDueDate,
                        BillRefNo = mObjAssessmentData.AssessmentRefNo,
                        StatusName = mObjAssessmentData.SettlementStatusName,
                        BillNotes = mObjAssessmentData.AssessmentNotes,
                        BillAmount = mObjAssessmentData.AssessmentAmount,
                        TotalPaid = mObjAssessmentData.SettlementAmount,
                    };

                    ViewBag.PoABalance = dcBalance;
                    ViewBag.Determinate = true;
                    if (mObjAssessmentData.SettlementAmount == null)
                        mObjAssessmentData.SettlementAmount = 0;
                    ViewBag.newToSettle = mObjAssessmentData.AssessmentAmount - mObjAssessmentData.SettlementAmount;

                    SessionManager.ContactNumber = mObjAssessmentData.TaxPayerMobile;
                    IList<usp_GetAssessment_AssessmentRuleList_Result> lstAssessmentRule = new List<usp_GetAssessment_AssessmentRuleList_Result>();
                    IList<usp_GetAssessmentRuleItemList_Result> lstAssessmentItem = new List<usp_GetAssessmentRuleItemList_Result>();
                    //IList<Assessment_AssessmentRule> lstAssessmentRule = new List<Assessment_AssessmentRule>();
                    //IList<Assessment_AssessmentItem> lstAssessmentItem = new List<Assessment_AssessmentItem>();
                    //Assessment_AssessmentItem mObjAssessmentItem = new Assessment_AssessmentItem();
                    //Getting Assessment Rules
                    IList<usp_GetAssessment_AssessmentRuleList_Result> lstMAPAssessmentRules = mObjBLAssessment.BL_GetAssessmentRules(mObjAssessmentData.AssessmentID.GetValueOrDefault());
                    IList<usp_GetAssessmentRuleItemList_Result> lstAssessmentItems = mObjBLAssessment.BL_GetAssessmentRuleItem(mObjAssessmentData.AssessmentID.GetValueOrDefault());
                    IList<usp_GetAssessmentRuleBasedSettlement_Result> lstAssessmentRuleSettlement = mObjBLAssessment.BL_GetAssessmentRuleBasedSettlement(mObjAssessmentData.AssessmentID.GetValueOrDefault());

                    //foreach (var item in lstMAPAssessmentRules)
                    //{
                    //    Assessment_AssessmentRule assessment_AssessmentRule = new Assessment_AssessmentRule()
                    //    {
                    //        RowID = lstAssessmentRule.Count + 1,
                    //        TablePKID = item.AARID.GetValueOrDefault(),
                    //        AssetTypeID = item.AssetTypeID.GetValueOrDefault(),
                    //        AssetTypeName = item.AssetTypeName,
                    //        AssetID = item.AssetID.GetValueOrDefault(),
                    //        AssetRIN = item.AssetRIN,
                    //        ProfileID = item.ProfileID.GetValueOrDefault(),
                    //        ProfileDescription = item.ProfileDescription,
                    //        AssessmentRuleID = item.AssessmentRuleID.GetValueOrDefault(),
                    //        AssessmentRuleName = item.AssessmentRuleName,
                    //        AssessmentRuleAmount = item.AssessmentRuleAmount.GetValueOrDefault(),
                    //        TaxYear = item.TaxYear.GetValueOrDefault(),
                    //        SettledAmount = item.SettledAmount,
                    //        UnSettledAmount = item.AssessmentRuleAmount.GetValueOrDefault() - item.SettledAmount.GetValueOrDefault(),
                    //        ToSettleAmount = item.AssessmentRuleAmount.GetValueOrDefault() - item.SettledAmount.GetValueOrDefault(),
                    //        intTrack = EnumList.Track.INSERT,
                    //    };

                    //    lstAssessmentRule.Add(assessment_AssessmentRule);
                    //    //int holderCount = lstAssessmentItems.Where(t => t.AARID == item.AARID).Count();
                    //    foreach (var subitem in lstAssessmentItems.Where(t => t.AARID == item.AARID))
                    //    {
                    //        usp_GetAssessmentItemList_Result mObjAssessmentItemData = mObjBLAssessmentItem.BL_GetAssessmentItemDetails(new Assessment_Items() { intStatus = 2, AssessmentItemID = subitem.AssessmentItemID.GetValueOrDefault() });
                    //        Assessment_AssessmentItem mObjAssessmentItem = new Assessment_AssessmentItem()
                    //        {
                    //            RowID = lstAssessmentItem.Count + 1,
                    //            AssessmentRule_RowID = assessment_AssessmentRule.RowID,
                    //            TablePKID = subitem.AAIID.GetValueOrDefault(),
                    //            AssessmentItemID = subitem.AssessmentItemID.GetValueOrDefault(),
                    //            AssessmentItemName = mObjAssessmentItemData.AssessmentItemName,
                    //            AssessmentItemReferenceNo = mObjAssessmentItemData.AssessmentItemReferenceNo,
                    //            ComputationID = mObjAssessmentItemData.ComputationID.GetValueOrDefault(),
                    //            ComputationName = mObjAssessmentItemData.ComputationName,
                    //            TaxAmount = subitem.TaxAmount.GetValueOrDefault(),
                    //            AdjustmentAmount = subitem.AdjustmentAmount.GetValueOrDefault(),
                    //            LateChargeAmount = subitem.LateChargeAmount.GetValueOrDefault(),
                    //            TotalAmount = subitem.TotalAmount.GetValueOrDefault(),
                    //            SettlementAmount = subitem.SettlementAmount.GetValueOrDefault(),
                    //            UnSettledAmount = subitem.PendingAmount.GetValueOrDefault(),
                    //            ToSettleAmount = subitem.PendingAmount.GetValueOrDefault(),
                    //            PaymentStatusID = subitem.PaymentStatusID.GetValueOrDefault(),
                    //            intTrack = EnumList.Track.INSERT,
                    //        };
                    //        lstAssessmentItem.Add(mObjAssessmentItem);
                    //    }
                    //}
                    // if (lstAssessmentRule.Count < 2)
                    //{
                    //mObjSettlementModel.BillAmount = mObjAssessmentItem.ToSettleAmount;
                    //foreach (var c in lstAssessmentRule)
                    //{
                    //    c.AssessmentRuleAmount = mObjAssessmentItem.ToSettleAmount;
                    //    c.ToSettleAmount = mObjAssessmentItem.ToSettleAmount;
                    //}
                    // }
                    SessionManager.lstAssessmentRules = lstMAPAssessmentRules;
                    SessionManager.lstAssessmentItems = lstAssessmentItems;
                    SessionManager.lstAssessmentRuleSettlement = lstAssessmentRuleSettlement;
                    SessionManager.DataSubmitterID = Convert.ToInt32(id);
                    ViewBag.AssessmentRuleList = lstMAPAssessmentRules;
                    //ViewBag.AssessmentRuleList = SessionManager.lstAssessmentRule.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
                    ViewBag.lstAssessmentItem = lstAssessmentItems;

                    //SessionManager.lstAssessmentRules = ViewBag.AssessmentRuleList;
                    var st = lstAssessmentItems.Sum(t => t.TotalAmount);
                    var sd = lstMAPAssessmentRules.Sum(t => t.SettledAmount);
                    //ViewBag.AmountToPay = lstAssessmentRule.Sum(t => t.ToSettleAmount);
                    ViewBag.AmountToPay = (st - sd);
                    return View(mObjSettlementModel);
                }
                else
                {
                    return RedirectToAction("PoASettlement", "OperationManager");
                }
            }
            else
            {
                return RedirectToAction("PoASettlement", "OperationManager");
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken()]
        public ActionResult AssessmentBillDetail(PoASettlementViewModel pObjSettlementModel)
        {
            decimal dcBalance = new BLPaymentAccount().BL_GetWalletBalance(pObjSettlementModel.TaxPayerTypeID.GetValueOrDefault(), pObjSettlementModel.TaxPayerID);
            IList<usp_GetAssessmentRuleItemList_Result> lstAssessmentItems = SessionManager.lstAssessmentItems ?? new List<usp_GetAssessmentRuleItemList_Result>();
            IList<usp_GetAssessment_AssessmentRuleList_Result> lstAssessmentRule = SessionManager.lstAssessmentRules ?? new List<usp_GetAssessment_AssessmentRuleList_Result>();

            var st = lstAssessmentItems.Sum(t => t.TotalAmount);
            var sd = lstAssessmentRule.Sum(t => t.SettledAmount);
            //ViewBag.AmountToPay = lstAssessmentRule.Sum(t => t.ToSettleAmount);
            var tt = st - sd;

            decimal? newSettleAmount = lstAssessmentItems.Sum(t => t.PendingAmount);
            if (!ModelState.IsValid)
            {

                ViewBag.PoABalance = dcBalance;
                ViewBag.AssessmentRuleList = SessionManager.lstAssessmentRule.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
                ViewBag.AmountToPay = SessionManager.lstAssessmentRule.Where(t => t.intTrack != EnumList.Track.DELETE).Sum(t => t.ToSettleAmount);
                return View(pObjSettlementModel);
            }
            else
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    //SessionManager.lstAssessmentRules

                    if (lstAssessmentRule.Sum(t => t.AssessmentRuleAmount) == 0)
                    {
                        ViewBag.PoABalance = dcBalance;
                        ViewBag.AssessmentRuleList = SessionManager.lstAssessmentRule.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
                        ViewBag.AmountToPay = SessionManager.lstAssessmentRule.Where(t => t.intTrack != EnumList.Track.DELETE).Sum(t => t.ToSettleAmount);
                        ViewBag.Message = "Settlement Amount Cannot be zero";
                        return View(pObjSettlementModel);
                    }
                    else if (newSettleAmount > dcBalance)
                    //else if ((tt - (lstAssessmentRule.Sum(t => t.SettledAmount)) > dcBalance))
                    {
                        ViewBag.PoABalance = dcBalance;
                        ViewBag.AssessmentRuleList = SessionManager.lstAssessmentRule.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
                        ViewBag.AmountToPay = SessionManager.lstAssessmentRule.Where(t => t.intTrack != EnumList.Track.DELETE).Sum(t => t.ToSettleAmount);
                        ViewBag.Message = "Insufficent PoA balance";
                        return View(pObjSettlementModel);
                    }
                    else
                    {

                        BLSettlement mObjBLSettlement = new BLSettlement();

                        Settlement mObjSettlement = new Settlement()
                        {
                            SettlementDate = CommUtil.GetCurrentDateTime(),
                            SettlementAmount = newSettleAmount,
                            SettlementMethodID = 7,
                            SettlementNotes = pObjSettlementModel.Notes,
                            AssessmentID = pObjSettlementModel.AssessmentID,
                            TransactionRefNo = "EIRS-" + CommUtil.GenerateUniqueNumber(),
                            Active = true,
                            CreatedBy = SessionManager.UserID,
                            CreatedDate = CommUtil.GetCurrentDateTime()
                        };

                        decimal? newSum = mObjSettlement.SettlementAmount;

                        try
                        {
                            FuncResponse<Settlement> mObjSettlementResponse = mObjBLSettlement.BL_InsertUpdateSettlement(mObjSettlement);

                            if (mObjSettlementResponse.Success)
                            {
                                if (mObjSettlementResponse.AdditionalData != null && GlobalDefaultValues.SendNotification)
                                {
                                    //Send Notification
                                    EmailDetails mObjEmailDetails = new EmailDetails()
                                    {
                                        TaxPayerID = pObjSettlementModel.TaxPayerID,
                                        TaxPayerTypeID = pObjSettlementModel.TaxPayerTypeID.Value,
                                        TaxPayerName = pObjSettlementModel.TaxPayerName,
                                        TaxPayerMobileNumber = SessionManager.ContactNumber,
                                        BillAmount = mObjSettlementResponse.AdditionalData.SettlementAmount.ToString(),
                                        BillRefNo = mObjSettlementResponse.AdditionalData.SettlementRefNo,
                                        LoggedInUserID = SessionManager.UserID,
                                    };

                                    //if (!string.IsNullOrWhiteSpace(mObjTaxPayerAssetData.TaxPayerEmailAddress))
                                    //{
                                    //    BLEmailHandler.BL_AssetProfileLinked(mObjEmailDetails);
                                    //}

                                    if (!string.IsNullOrWhiteSpace(mObjEmailDetails.TaxPayerMobileNumber))
                                    {
                                        UtilityController.BL_SettlementReceived(mObjEmailDetails);
                                    }
                                }
                                BLAssessment mObjBLAssessment = new BLAssessment();

                                foreach (usp_GetAssessmentRuleItemList_Result mObjAIDetail in lstAssessmentItems)
                                {
                                    if (mObjAIDetail.TotalAmount != mObjAIDetail.SettlementAmount && mObjAIDetail.PendingAmount > 0)
                                    {
                                        var eachAmount = lstAssessmentItems.Sum(o => o.SettlementAmount);
                                        MAP_Settlement_SettlementItem mObjSettlementItem = new MAP_Settlement_SettlementItem()
                                        {
                                            SettlementID = mObjSettlementResponse.AdditionalData.SettlementID,
                                            SettlementAmount = mObjAIDetail.PendingAmount,
                                            //  TaxAmount = eachAmount,
                                            TaxAmount = mObjAIDetail.TaxAmount,
                                            AAIID = mObjAIDetail.AAIID,
                                            CreatedBy = SessionManager.UserID,
                                            CreatedDate = CommUtil.GetCurrentDateTime()
                                        };


                                        FuncResponse mObjSIResponse = mObjBLSettlement.BL_InsertSettlementItem(mObjSettlementItem);
                                        if (mObjSIResponse.Success)
                                        {
                                            var newRet = _db.MAP_Settlement_SettlementItem.Where(o => o.AAIID == mObjAIDetail.AAIID);


                                            MAP_Assessment_AssessmentItem ret = mObjBLAssessment.GetAssessmentItems(mObjAIDetail.AAIID.Value);
                                            MAP_Assessment_AssessmentItem mObjAAI = new MAP_Assessment_AssessmentItem()
                                            {
                                                AAIID = mObjAIDetail.AAIID.Value,
                                                ModifiedBy = SessionManager.UserID,
                                                ModifiedDate = CommUtil.GetCurrentDateTime(),
                                                TaxAmount = ret.TaxAmount,
                                            };

                                            //Update Assessment item Status
                                            if (ret.TaxAmount == newRet.Sum(o => o.SettlementAmount) || ret.TaxAmount < newRet.Sum(o => o.SettlementAmount))
                                            {
                                                mObjAAI.PaymentStatusID = (int)EnumList.PaymentStatus.Paid;
                                            }
                                            else if (ret.TaxAmount > newRet.Sum(o => o.SettlementAmount))
                                            {
                                                mObjAAI.PaymentStatusID = (int)EnumList.PaymentStatus.Partial;
                                            }

                                            if (mObjAAI.PaymentStatusID != null)
                                            {
                                                mObjBLAssessment.BL_UpdateAssessmentItemStatus(mObjAAI);
                                            }
                                        }
                                        else
                                        {
                                            throw (mObjSIResponse.Exception);
                                        }
                                    }
                                }

                                decimal? assAmount = _db.Assessments.FirstOrDefault(o => o.AssessmentID == pObjSettlementModel.AssessmentID).AssessmentAmount;
                                Assessment mObjAssessment = new Assessment()
                                {
                                    AssessmentID = pObjSettlementModel.AssessmentID,
                                    SettlementDate = CommUtil.GetCurrentDateTime(),
                                    ModifiedDate = CommUtil.GetCurrentDateTime(),
                                    ModifiedBy = SessionManager.UserID,
                                    AssessmentAmount = assAmount
                                };

                                if (lstAssessmentItems.Sum(t => t.TotalAmount) == (newSum + lstAssessmentItems.Sum(t => t.SettlementAmount)))
                                {
                                    mObjAssessment.SettlementStatusID = (int)EnumList.SettlementStatus.Settled;
                                }
                                else if (newSum + lstAssessmentItems.Sum(t => t.SettlementAmount) < lstAssessmentItems.Sum(t => t.TotalAmount))
                                {
                                    mObjAssessment.SettlementStatusID = (int)EnumList.SettlementStatus.Partial;
                                }

                                if (mObjAssessment.SettlementStatusID != null)
                                {
                                    mObjBLAssessment.BL_UpdateAssessmentSettlementStatus(mObjAssessment);
                                }

                                //Update Payment Operation Table
                                MAP_PaymentAccount_Operation mObjPoAOperation = new MAP_PaymentAccount_Operation()
                                {
                                    OperationTypeID = 3,
                                    OperationDate = CommUtil.GetCurrentDateTime(),
                                    From_TaxPayerTypeID = pObjSettlementModel.TaxPayerTypeID,
                                    From_TaxPayerID = pObjSettlementModel.TaxPayerID,
                                    To_BillTypeID = 1,
                                    To_BillID = (int)pObjSettlementModel.AssessmentID,
                                    Amount = mObjSettlement.SettlementAmount,
                                    Active = true,
                                    CreatedBy = SessionManager.UserID,
                                    CreatedDate = CommUtil.GetCurrentDateTime(),
                                };

                                new BLPaymentAccount().BL_InsertPaymentOperation(mObjPoAOperation);

                                Audit_Log mObjAuditLog = new Audit_Log()
                                {
                                    LogDate = CommUtil.GetCurrentDateTime(),
                                    ASLID = (int)EnumList.ALScreen.Operation_Manager_PoA_Settlement,
                                    Comment = $"PoA Settlement with transaction ref no. {mObjSettlement.TransactionRefNo} and Amount {mObjSettlement.SettlementAmount}",
                                    IPAddress = CommUtil.GetIPAddress(),
                                    StaffID = SessionManager.UserID,
                                };

                                new BLAuditLog().BL_InsertAuditLog(mObjAuditLog);


                                scope.Complete();
                                FlashMessage.Info(mObjSettlementResponse.Message);
                                return RedirectToAction("PoASettlement", "OperationManager");

                            }
                            else
                            {
                                ViewBag.PoABalance = dcBalance;
                                ViewBag.AssessmentRuleList = SessionManager.lstAssessmentRule.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
                                ViewBag.AmountToPay = SessionManager.lstAssessmentRule.Where(t => t.intTrack != EnumList.Track.DELETE).Sum(t => t.ToSettleAmount);
                                ViewBag.Message = mObjSettlementResponse.Message;
                                Transaction.Current.Rollback();
                                return View(pObjSettlementModel);
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorSignal.FromCurrentContext().Raise(ex);
                            ViewBag.PoABalance = dcBalance;
                            ViewBag.AssessmentRuleList = SessionManager.lstAssessmentRule.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
                            ViewBag.AmountToPay = SessionManager.lstAssessmentRule.Where(t => t.intTrack != EnumList.Track.DELETE).Sum(t => t.ToSettleAmount);
                            ViewBag.Message = "Error occurred while saving settlement";
                            Transaction.Current.Rollback();
                            return View(pObjSettlementModel);
                        }
                    }
                }
            }
        }
        public ActionResult UpdateServiceBillNewItem(string rowdata)
        {
            IList<ServiceBill_MDAServiceItem> lstMDAServiceItems = SessionManager.lstMDAServiceItem ?? new List<ServiceBill_MDAServiceItem>();
            IList<NewPoASettlementViewModel> poa = SessionManager.LstPOS ?? new List<NewPoASettlementViewModel>();
            int holderId = SessionManager.DataSubmitterID;
            if (rowdata != null && !rowdata.Contains("@@"))
            {
                IList<ServiceBill_MDAService> lstMDAServices2 = SessionManager.lstMDAService ?? new List<ServiceBill_MDAService>();
                ViewBag.MDAServiceList = SessionManager.lstMDAService.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
                ViewBag.pos = SessionManager.LstPOS.ToList();
                IList<ServiceBill_MDAServiceItem> lstMDAServiceItem = new List<ServiceBill_MDAServiceItem>();

                BLAssessment mObjBLAssessment = new BLAssessment();
                BLMDAService mObjBLMDAService = new BLMDAService();
                BLMDAServiceItem mObjBLMDAServiceItem = new BLMDAServiceItem();
                BLServiceBill mObjBLServiceBill = new BLServiceBill();

                usp_GetServiceBillList_Result mObjServiceBillData = mObjBLServiceBill.BL_GetServiceBillDetails(new ServiceBill() { ServiceBillID = holderId, IntStatus = 2 });

                decimal dcBalance = new BLPaymentAccount().BL_GetWalletBalance(mObjServiceBillData.TaxPayerTypeID.GetValueOrDefault(), mObjServiceBillData.TaxPayerID.GetValueOrDefault());
                NewPoASettlementViewModel mObjSettlementModel2 = new NewPoASettlementViewModel()
                {
                    ServiceBillID = mObjServiceBillData.ServiceBillID.GetValueOrDefault(),
                    TaxPayerID = mObjServiceBillData.TaxPayerID.GetValueOrDefault(),
                    TaxPayerTypeID = mObjServiceBillData.TaxPayerTypeID.GetValueOrDefault(),
                    TaxPayerName = mObjServiceBillData.TaxPayerName,
                    TaxPayerTypeName = mObjServiceBillData.TaxPayerTypeName,
                    TaxPayerRIN = mObjServiceBillData.TaxPayerRIN,
                    BillDate = mObjServiceBillData.ServiceBillDate,
                    DueDate = mObjServiceBillData.SettlementDueDate,
                    BillRefNo = mObjServiceBillData.ServiceBillRefNo,
                    StatusName = mObjServiceBillData.SettlementStatusName,
                    BillNotes = mObjServiceBillData.Notes,
                    BillAmount = mObjServiceBillData.ServiceBillAmount,
                    TotalPaid = mObjServiceBillData.SettlementAmount,
                };
                ViewBag.PoABalance = dcBalance;
                //Getting MDA Service
                IList<usp_GetServiceBill_MDAServiceList_Result> lstMAPServiceBillServices = mObjBLServiceBill.BL_GetMDAServiceList(mObjServiceBillData.ServiceBillID.GetValueOrDefault());
                IList<usp_GetServiceBillItemList_Result> lstServiceBillItems = mObjBLServiceBill.BL_GetServiceBillItem(mObjServiceBillData.ServiceBillID.GetValueOrDefault());
                IList<usp_GetMDAServiceBasedSettlement_Result> lstMDAServiceSettlement = mObjBLServiceBill.BL_GetMDAServiceBasedSettlement(mObjServiceBillData.ServiceBillID.GetValueOrDefault());

                SessionManager.lstMDAService = lstMDAServices2;
                SessionManager.lstMDAServiceItem = lstMDAServiceItems;
                SessionManager.lstMDAServiceSettlement = lstMDAServiceSettlement;

                ViewBag.MDAServiceList = SessionManager.lstMDAService.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
                ViewBag.AmountToPay = lstMDAServices2.Sum(t => t.ServiceAmount);
                return View(mObjSettlementModel2);
            }
            IList<ServiceBill_MDAService> lstMDAServices = SessionManager.lstMDAService ?? new List<ServiceBill_MDAService>();
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();


            string[] strRowData = rowdata.Split(new string[] { "~~" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var vRowData in strRowData)
            {
                string[] strServiceItemData = vRowData.Split(new string[] { "@@" }, StringSplitOptions.RemoveEmptyEntries);

                if (strServiceItemData.Length == 2)
                {
                    ServiceBill_MDAServiceItem mObjUpdateMDAServiceItem = lstMDAServiceItems.Where(t => t.RowID == TrynParse.parseInt(strServiceItemData[0])).FirstOrDefault();


                    if (mObjUpdateMDAServiceItem != null)
                    {
                        decimal ServiceBaseAmount = TrynParse.parseDecimal(strServiceItemData[1]);

                        mObjUpdateMDAServiceItem.ServiceBaseAmount = ServiceBaseAmount;

                        if (mObjUpdateMDAServiceItem.ComputationID == 2)
                        {
                            mObjUpdateMDAServiceItem.ServiceAmount = ServiceBaseAmount * (mObjUpdateMDAServiceItem.Percentage / 100);
                            mObjUpdateMDAServiceItem.ServiceBaseAmount = ServiceBaseAmount;
                        }
                        else if (mObjUpdateMDAServiceItem.ComputationID == 1 || mObjUpdateMDAServiceItem.ComputationID == 3)
                            mObjUpdateMDAServiceItem.ServiceAmount = ServiceBaseAmount;

                        mObjUpdateMDAServiceItem.intTrack = mObjUpdateMDAServiceItem.TablePKID > 0 ? EnumList.Track.UPDATE : EnumList.Track.INSERT;


                        ServiceBill_MDAService mObjUpdateMDAService = lstMDAServices.Where(t => t.RowID == mObjUpdateMDAServiceItem.MDAService_RowID).FirstOrDefault();

                        mObjUpdateMDAService.ServiceAmount = lstMDAServiceItems.Where(t => t.MDAService_RowID == mObjUpdateMDAService.RowID).Sum(t => t.ServiceAmount);
                        mObjUpdateMDAService.intTrack = mObjUpdateMDAService.TablePKID > 0 ? EnumList.Track.UPDATE : EnumList.Track.INSERT;
                    }
                }
            }

            ViewBag.MDAServiceList = lstMDAServices.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();

            dcResponse["success"] = true;
            dcResponse["MDAServiceDetails"] = CommUtil.RenderPartialToString("_BindMDAServiceForSerivceBill", null, this.ControllerContext, ViewData);

            SessionManager.lstMDAService = lstMDAServices;
            SessionManager.lstMDAServiceItem = lstMDAServiceItems;

            ViewBag.MDAServiceList = lstMDAServices.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
            IList<ServiceBill_MDAService> lstMdaServices = (IList<ServiceBill_MDAService>)ViewBag.MDAServiceList;
            //SessionManager.lstMDAService = lstMDAServices;
            //SessionManager.lstMDAServiceItem = lstMDAServiceItems;

            decimal totalSum = lstMdaServices.Sum(t => t.ServiceAmount);
            if (holderId > 0)
            {
                BLMDAService mObjBLMDAService = new BLMDAService();
                BLMDAServiceItem mObjBLMDAServiceItem = new BLMDAServiceItem();
                BLServiceBill mObjBLServiceBill = new BLServiceBill();

                usp_GetServiceBillList_Result mObjServiceBillData = mObjBLServiceBill.BL_GetServiceBillDetails(new ServiceBill() { ServiceBillID = holderId, IntStatus = 2 });
                IList<NewPoASettlementViewModel> newpoa = new List<NewPoASettlementViewModel>();
                if (mObjServiceBillData != null)
                {
                    decimal dcBalance = new BLPaymentAccount().BL_GetWalletBalance(mObjServiceBillData.TaxPayerTypeID.GetValueOrDefault(), mObjServiceBillData.TaxPayerID.GetValueOrDefault());

                    NewPoASettlementViewModel mObjSettlementModel = new NewPoASettlementViewModel()
                    {
                        ServiceBillID = mObjServiceBillData.ServiceBillID.GetValueOrDefault(),
                        TaxPayerID = mObjServiceBillData.TaxPayerID.GetValueOrDefault(),
                        TaxPayerTypeID = mObjServiceBillData.TaxPayerTypeID.GetValueOrDefault(),
                        TaxPayerName = mObjServiceBillData.TaxPayerName,
                        TaxPayerTypeName = mObjServiceBillData.TaxPayerTypeName,
                        TaxPayerRIN = mObjServiceBillData.TaxPayerRIN,
                        BillDate = mObjServiceBillData.ServiceBillDate,
                        DueDate = mObjServiceBillData.SettlementDueDate,
                        BillRefNo = mObjServiceBillData.ServiceBillRefNo,
                        StatusName = mObjServiceBillData.SettlementStatusName,
                        BillNotes = mObjServiceBillData.Notes,
                        BillAmount = mObjServiceBillData.ServiceBillAmount,
                        TotalPaid = mObjServiceBillData.SettlementAmount,
                    };

                    newpoa.Add(mObjSettlementModel);
                    ViewBag.PoABalance = dcBalance;

                    IList<ServiceBill_MDAService> lstServiceList = new List<ServiceBill_MDAService>();
                    IList<ServiceBill_MDAServiceItem> lstMDAServiceItem = new List<ServiceBill_MDAServiceItem>();

                    //Getting MDA Service
                    IList<usp_GetServiceBill_MDAServiceList_Result> lstMAPServiceBillServices = mObjBLServiceBill.BL_GetMDAServiceList(mObjServiceBillData.ServiceBillID.GetValueOrDefault());
                    IList<usp_GetServiceBillItemList_Result> lstServiceBillItems = mObjBLServiceBill.BL_GetServiceBillItem(mObjServiceBillData.ServiceBillID.GetValueOrDefault());
                    IList<usp_GetMDAServiceBasedSettlement_Result> lstMDAServiceSettlement = mObjBLServiceBill.BL_GetMDAServiceBasedSettlement(mObjServiceBillData.ServiceBillID.GetValueOrDefault());

                    foreach (var item in lstMAPServiceBillServices)
                    {
                        ServiceBill_MDAService mObjMDAService = new ServiceBill_MDAService()
                        {
                            RowID = lstServiceList.Count + 1,
                            TablePKID = item.SBSID.GetValueOrDefault(),
                            MDAServiceID = item.MDAServiceID.GetValueOrDefault(),
                            MDAServiceName = item.MDAServiceName,
                            ServiceAmount = item.ServiceAmount.GetValueOrDefault(),
                            TaxYear = item.TaxYear.GetValueOrDefault(),
                            SettledAmount = item.SettledAmount,
                            UnSettledAmount = item.ServiceAmount.GetValueOrDefault() - item.SettledAmount.GetValueOrDefault(),
                            ToSettleAmount = item.ServiceAmount.GetValueOrDefault() - item.SettledAmount.GetValueOrDefault(),
                            intTrack = EnumList.Track.INSERT,
                        };

                        lstServiceList.Add(mObjMDAService);

                        foreach (var subitem in lstServiceBillItems.Where(t => t.SBSID == item.SBSID))
                        {
                            usp_GetMDAServiceItemList_Result mObjMDAServiceItemData = mObjBLMDAServiceItem.BL_GetMDAServiceItemDetails(new MDA_Service_Items() { intStatus = 2, MDAServiceItemID = subitem.MDAServiceItemID.GetValueOrDefault() });

                            ServiceBill_MDAServiceItem mObjServiceBillItem = new ServiceBill_MDAServiceItem()
                            {
                                RowID = lstMDAServiceItem.Count + 1,
                                MDAService_RowID = mObjMDAService.RowID,
                                TablePKID = subitem.SBSIID.GetValueOrDefault(),
                                MDAServiceItemID = mObjMDAServiceItemData.MDAServiceItemID.GetValueOrDefault(),
                                MDAServiceItemName = mObjMDAServiceItemData.MDAServiceItemName,
                                MDAServiceItemReferenceNo = mObjMDAServiceItemData.MDAServiceItemReferenceNo,
                                ComputationID = mObjMDAServiceItemData.ComputationID.GetValueOrDefault(),
                                ComputationName = mObjMDAServiceItemData.ComputationName,
                                ServiceAmount = subitem.ServiceAmount.GetValueOrDefault(),
                                AdjustmentAmount = subitem.AdjustmentAmount.GetValueOrDefault(),
                                LateChargeAmount = subitem.LateChargeAmount.GetValueOrDefault(),
                                TotalAmount = subitem.TotalAmount.GetValueOrDefault(),
                                SettlementAmount = subitem.SettlementAmount.GetValueOrDefault(),
                                UnSettledAmount = subitem.PendingAmount.GetValueOrDefault(),
                                ToSettleAmount = subitem.PendingAmount.GetValueOrDefault(),
                                PaymentStatusID = subitem.PaymentStatusID.GetValueOrDefault(),
                                intTrack = EnumList.Track.INSERT
                            };

                            lstMDAServiceItem.Add(mObjServiceBillItem);
                        }
                    }

                    SessionManager.lstMDAService = lstMDAServices;
                    SessionManager.lstMDAServiceItem = lstMDAServiceItem;
                    SessionManager.lstMDAServiceSettlement = lstMDAServiceSettlement;

                    SessionManager.LstPOS = newpoa;
                    ViewBag.MDAServiceList = SessionManager.lstMDAService.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
                    ViewBag.MDAServiceItemList = SessionManager.lstMDAServiceItem;
                    ViewBag.AmountToPay = lstServiceList.Sum(t => t.ToSettleAmount);
                    ViewBag.POS = SessionManager.LstPOS.ToList();
                    return View(mObjSettlementModel);

                }
                else
                {
                    return RedirectToAction("PoASettlement", "OperationManager");
                }
            }
            else
            {
                return RedirectToAction("PoASettlement", "OperationManager");
            }
        }
        public ActionResult ServiceBillDetail(int? id, string name)
        {
            string url = getUrl();
            bool itCan = new UtilityController().CheckAccess(url);
            if (!itCan) { return RedirectToAction("AccessDenied", "Utility"); }
            if (id.GetValueOrDefault() > 0)
            {
                BLMDAService mObjBLMDAService = new BLMDAService();
                BLMDAServiceItem mObjBLMDAServiceItem = new BLMDAServiceItem();
                BLServiceBill mObjBLServiceBill = new BLServiceBill();

                usp_GetServiceBillList_Result mObjServiceBillData = mObjBLServiceBill.BL_GetServiceBillDetails(new ServiceBill() { ServiceBillID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjServiceBillData != null)
                {
                    decimal dcBalance = new BLPaymentAccount().BL_GetWalletBalance(mObjServiceBillData.TaxPayerTypeID.GetValueOrDefault(), mObjServiceBillData.TaxPayerID.GetValueOrDefault());

                    PoASettlementViewModel mObjSettlementModel = new PoASettlementViewModel()
                    {
                        ServiceBillID = mObjServiceBillData.ServiceBillID.GetValueOrDefault(),
                        TaxPayerID = mObjServiceBillData.TaxPayerID.GetValueOrDefault(),
                        TaxPayerTypeID = mObjServiceBillData.TaxPayerTypeID.GetValueOrDefault(),
                        TaxPayerName = mObjServiceBillData.TaxPayerName,
                        TaxPayerTypeName = mObjServiceBillData.TaxPayerTypeName,
                        TaxPayerRIN = mObjServiceBillData.TaxPayerRIN,
                        BillDate = mObjServiceBillData.ServiceBillDate,
                        DueDate = mObjServiceBillData.SettlementDueDate,
                        BillRefNo = mObjServiceBillData.ServiceBillRefNo,
                        StatusName = mObjServiceBillData.SettlementStatusName,
                        BillNotes = mObjServiceBillData.Notes,
                        BillAmount = mObjServiceBillData.ServiceBillAmount,
                        TotalPaid = mObjServiceBillData.SettlementAmount,
                    };


                    ViewBag.PoABalance = dcBalance;

                    IList<ServiceBill_MDAService> lstMDAServices = new List<ServiceBill_MDAService>();
                    IList<ServiceBill_MDAServiceItem> lstMDAServiceItem = new List<ServiceBill_MDAServiceItem>();

                    //Getting MDA Service
                    IList<usp_GetServiceBill_MDAServiceList_Result> lstMAPServiceBillServices = mObjBLServiceBill.BL_GetMDAServiceList(mObjServiceBillData.ServiceBillID.GetValueOrDefault());
                    IList<usp_GetServiceBillItemList_Result> lstServiceBillItems = mObjBLServiceBill.BL_GetServiceBillItem(mObjServiceBillData.ServiceBillID.GetValueOrDefault());
                    IList<usp_GetMDAServiceBasedSettlement_Result> lstMDAServiceSettlement = mObjBLServiceBill.BL_GetMDAServiceBasedSettlement(mObjServiceBillData.ServiceBillID.GetValueOrDefault());

                    foreach (var item in lstMAPServiceBillServices)
                    {
                        ServiceBill_MDAService mObjMDAService = new ServiceBill_MDAService()
                        {
                            RowID = lstMDAServices.Count + 1,
                            TablePKID = item.SBSID.GetValueOrDefault(),
                            MDAServiceID = item.MDAServiceID.GetValueOrDefault(),
                            MDAServiceName = item.MDAServiceName,
                            ServiceAmount = item.ServiceAmount.GetValueOrDefault(),
                            TaxYear = item.TaxYear.GetValueOrDefault(),
                            SettledAmount = item.SettledAmount,
                            UnSettledAmount = item.ServiceAmount.GetValueOrDefault() - item.SettledAmount.GetValueOrDefault(),
                            ToSettleAmount = item.ServiceAmount.GetValueOrDefault() - item.SettledAmount.GetValueOrDefault(),
                            intTrack = EnumList.Track.INSERT,
                        };

                        lstMDAServices.Add(mObjMDAService);

                        foreach (var subitem in lstServiceBillItems.Where(t => t.SBSID == item.SBSID))
                        {
                            usp_GetMDAServiceItemList_Result mObjMDAServiceItemData = mObjBLMDAServiceItem.BL_GetMDAServiceItemDetails(new MDA_Service_Items() { intStatus = 2, MDAServiceItemID = subitem.MDAServiceItemID.GetValueOrDefault() });

                            ServiceBill_MDAServiceItem mObjServiceBillItem = new ServiceBill_MDAServiceItem()
                            {
                                RowID = lstMDAServiceItem.Count + 1,
                                MDAService_RowID = mObjMDAService.RowID,
                                TablePKID = subitem.SBSIID.GetValueOrDefault(),
                                MDAServiceItemID = mObjMDAServiceItemData.MDAServiceItemID.GetValueOrDefault(),
                                MDAServiceItemName = mObjMDAServiceItemData.MDAServiceItemName,
                                MDAServiceItemReferenceNo = mObjMDAServiceItemData.MDAServiceItemReferenceNo,
                                ComputationID = mObjMDAServiceItemData.ComputationID.GetValueOrDefault(),
                                ComputationName = mObjMDAServiceItemData.ComputationName,
                                ServiceAmount = subitem.ServiceAmount.GetValueOrDefault(),
                                AdjustmentAmount = subitem.AdjustmentAmount.GetValueOrDefault(),
                                LateChargeAmount = subitem.LateChargeAmount.GetValueOrDefault(),
                                TotalAmount = subitem.TotalAmount.GetValueOrDefault(),
                                SettlementAmount = subitem.SettlementAmount.GetValueOrDefault(),
                                UnSettledAmount = subitem.PendingAmount.GetValueOrDefault(),
                                ToSettleAmount = subitem.PendingAmount.GetValueOrDefault(),
                                PaymentStatusID = subitem.PaymentStatusID.GetValueOrDefault(),
                                intTrack = EnumList.Track.INSERT
                            };

                            lstMDAServiceItem.Add(mObjServiceBillItem);
                        }
                    }

                    SessionManager.lstMDAService = lstMDAServices;
                    SessionManager.lstMDAServiceItem = lstMDAServiceItem;
                    SessionManager.lstMDAServiceSettlement = lstMDAServiceSettlement;

                    SessionManager.DataSubmitterID = Convert.ToInt32(id);
                    ViewBag.MDAServiceList = SessionManager.lstMDAService.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
                    ViewBag.AmountToPay = lstMDAServices.Sum(t => t.ToSettleAmount);
                    return View(mObjSettlementModel);
                }
                else
                {
                    return RedirectToAction("PoASettlement", "OperationManager");
                }
            }
            else
            {
                return RedirectToAction("PoASettlement", "OperationManager");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult ServiceBillDetail(PoASettlementViewModel pObjSettlementModel)
        {
            decimal dcBalance = new BLPaymentAccount().BL_GetWalletBalance(pObjSettlementModel.TaxPayerTypeID.GetValueOrDefault(), pObjSettlementModel.TaxPayerID);

            if (!ModelState.IsValid)
            {

                ViewBag.PoABalance = dcBalance;
                ViewBag.MDAServiceList = SessionManager.lstMDAService.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
                ViewBag.AmountToPay = SessionManager.lstMDAService.Where(t => t.intTrack != EnumList.Track.DELETE).Sum(t => t.ToSettleAmount);
                return View(pObjSettlementModel);
            }
            else
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    IList<ServiceBill_MDAServiceItem> lstMDAServiceItems = SessionManager.lstMDAServiceItem ?? new List<ServiceBill_MDAServiceItem>();
                    IList<ServiceBill_MDAService> lstMDAServices = SessionManager.lstMDAService ?? new List<ServiceBill_MDAService>();

                    if (lstMDAServiceItems.Sum(t => t.ToSettleAmount) == 0)
                    {
                        ViewBag.PoABalance = dcBalance;
                        ViewBag.MDAServiceList = SessionManager.lstMDAService.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
                        ViewBag.AmountToPay = SessionManager.lstMDAService.Where(t => t.intTrack != EnumList.Track.DELETE).Sum(t => t.ToSettleAmount);
                        ViewBag.Message = "Settlement Amount Cannot be zero";
                        return View(pObjSettlementModel);
                    }
                    else if (lstMDAServices.Sum(t => t.ToSettleAmount) > dcBalance)
                    {
                        ViewBag.PoABalance = dcBalance;
                        ViewBag.MDAServiceList = SessionManager.lstMDAService.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
                        ViewBag.AmountToPay = SessionManager.lstMDAService.Where(t => t.intTrack != EnumList.Track.DELETE).Sum(t => t.ToSettleAmount);
                        ViewBag.Message = "Insufficent PoA balance";
                        return View(pObjSettlementModel);
                    }
                    else
                    {
                        BLSettlement mObjBLSettlement = new BLSettlement();

                        Settlement mObjSettlement = new Settlement()
                        {
                            SettlementDate = CommUtil.GetCurrentDateTime(),
                            SettlementAmount = lstMDAServices.Sum(t => t.ToSettleAmount),
                            SettlementMethodID = 7,
                            SettlementNotes = pObjSettlementModel.Notes,
                            ServiceBillID = pObjSettlementModel.ServiceBillID,
                            TransactionRefNo = "EIRS-" + CommUtil.GenerateUniqueNumber(),
                            Active = true,
                            CreatedBy = SessionManager.UserID,
                            CreatedDate = CommUtil.GetCurrentDateTime()
                        };

                        try
                        {
                            FuncResponse<Settlement> mObjSettlementResponse = mObjBLSettlement.BL_InsertUpdateSettlement(mObjSettlement);

                            if (mObjSettlementResponse.Success)
                            {
                                if (mObjSettlementResponse.AdditionalData != null && GlobalDefaultValues.SendNotification)
                                {
                                    //Send Notification
                                    EmailDetails mObjEmailDetails = new EmailDetails()
                                    {
                                        TaxPayerID = pObjSettlementModel.TaxPayerID,
                                        TaxPayerTypeID = pObjSettlementModel.TaxPayerTypeID.Value,
                                        TaxPayerName = pObjSettlementModel.TaxPayerName,
                                        TaxPayerMobileNumber = SessionManager.ContactNumber,
                                        BillAmount = mObjSettlementResponse.AdditionalData.SettlementAmount.ToString(),
                                        BillRefNo = mObjSettlementResponse.AdditionalData.SettlementRefNo,
                                        LoggedInUserID = SessionManager.UserID,
                                    };

                                    //if (!string.IsNullOrWhiteSpace(mObjTaxPayerAssetData.TaxPayerEmailAddress))
                                    //{
                                    //    BLEmailHandler.BL_AssetProfileLinked(mObjEmailDetails);
                                    //}

                                    if (!string.IsNullOrWhiteSpace(mObjEmailDetails.TaxPayerMobileNumber))
                                    {
                                        UtilityController.BL_SettlementReceived(mObjEmailDetails);
                                    }
                                }
                                BLServiceBill mObjBLServiceBill = new BLServiceBill();
                                foreach (ServiceBill_MDAServiceItem mObjSIDetail in lstMDAServiceItems)
                                {
                                    if (mObjSIDetail.PaymentStatusID != (int)EnumList.PaymentStatus.Paid && (mObjSIDetail.ToSettleAmount > 0 || mObjSIDetail.TotalAmount == 0))
                                    {
                                        MAP_Settlement_SettlementItem mObjSettlementItem = new MAP_Settlement_SettlementItem()
                                        {
                                            SettlementID = mObjSettlementResponse.AdditionalData.SettlementID,
                                            SettlementAmount = mObjSIDetail.ToSettleAmount,
                                            TaxAmount = mObjSIDetail.TotalAmount,
                                            SBSIID = mObjSIDetail.TablePKID,
                                            CreatedBy = SessionManager.UserID,
                                            CreatedDate = CommUtil.GetCurrentDateTime()
                                        };

                                        FuncResponse mObjSIResponse = mObjBLSettlement.BL_InsertSettlementItem(mObjSettlementItem);

                                        if (mObjSIResponse.Success)
                                        {
                                            MAP_ServiceBill_MDAServiceItem mObjSBMSI = new MAP_ServiceBill_MDAServiceItem()
                                            {
                                                SBSIID = mObjSIDetail.TablePKID,
                                                ModifiedBy = SessionManager.UserID,
                                                ServiceAmount = mObjSIDetail.TotalAmount,
                                                ServiceBaseAmount = mObjSIDetail.TotalAmount,
                                                ModifiedDate = CommUtil.GetCurrentDateTime()
                                            };

                                            //Update Assessment item Status
                                            if (mObjSIDetail.TotalAmount == (lstMDAServices.Sum(t => t.SettledAmount) + mObjSIDetail.ToSettleAmount) || (lstMDAServices.Sum(t => t.SettledAmount) + mObjSIDetail.ToSettleAmount) > mObjSIDetail.TotalAmount)
                                            {
                                                mObjSBMSI.PaymentStatusID = (int)EnumList.PaymentStatus.Paid;
                                            }
                                            else if ((lstMDAServices.Sum(t => t.SettledAmount) + mObjSIDetail.ToSettleAmount) < mObjSIDetail.TotalAmount)
                                            {
                                                mObjSBMSI.PaymentStatusID = (int)EnumList.PaymentStatus.Partial;
                                            }

                                            if (mObjSBMSI.PaymentStatusID != null)
                                            {
                                                mObjBLServiceBill.BL_UpdateMDAServiceItemStatus(mObjSBMSI);
                                            }
                                        }
                                        else
                                        {
                                            throw (mObjSIResponse.Exception);
                                        }
                                    }
                                }

                                decimal? assAmount = _db.ServiceBills.FirstOrDefault(o => o.ServiceBillID == pObjSettlementModel.ServiceBillID).ServiceBillAmount;
                                //Update Service Bill Status
                                ServiceBill mObjServiceBill = new ServiceBill()
                                {
                                    ServiceBillID = pObjSettlementModel.ServiceBillID,
                                    SettlementDate = CommUtil.GetCurrentDateTime(),
                                    ModifiedDate = CommUtil.GetCurrentDateTime(),
                                    ModifiedBy = SessionManager.UserID,
                                    ServiceBillAmount = assAmount
                                };

                                if (lstMDAServiceItems.Sum(t => t.TotalAmount) == (lstMDAServices.Sum(t => t.SettledAmount) + lstMDAServiceItems.Sum(t => t.ToSettleAmount)) || (lstMDAServices.Sum(t => t.SettledAmount) + lstMDAServiceItems.Sum(t => t.ToSettleAmount)) > lstMDAServiceItems.Sum(t => t.TotalAmount))
                                {
                                    mObjServiceBill.SettlementStatusID = (int)EnumList.SettlementStatus.Settled;
                                }
                                else if ((lstMDAServices.Sum(t => t.SettledAmount) + lstMDAServiceItems.Sum(t => t.ToSettleAmount)) < lstMDAServiceItems.Sum(t => t.TotalAmount))
                                {
                                    mObjServiceBill.SettlementStatusID = (int)EnumList.SettlementStatus.Partial;
                                }

                                if (mObjServiceBill.SettlementStatusID != null)
                                {
                                    mObjBLServiceBill.BL_UpdateServiceBillSettlementStatus(mObjServiceBill);
                                }

                                //Update Payment Operation Table
                                MAP_PaymentAccount_Operation mObjPoAOperation = new MAP_PaymentAccount_Operation()
                                {
                                    OperationTypeID = 3,
                                    OperationDate = CommUtil.GetCurrentDateTime(),
                                    From_TaxPayerTypeID = pObjSettlementModel.TaxPayerTypeID,
                                    From_TaxPayerID = pObjSettlementModel.TaxPayerID,
                                    To_BillTypeID = 2,
                                    To_BillID = (int)pObjSettlementModel.ServiceBillID,
                                    Amount = mObjSettlement.SettlementAmount,
                                    Active = true,
                                    CreatedBy = SessionManager.UserID,
                                    CreatedDate = CommUtil.GetCurrentDateTime(),
                                };

                                new BLPaymentAccount().BL_InsertPaymentOperation(mObjPoAOperation);

                                Audit_Log mObjAuditLog = new Audit_Log()
                                {
                                    LogDate = CommUtil.GetCurrentDateTime(),
                                    ASLID = (int)EnumList.ALScreen.Operation_Manager_PoA_Settlement,
                                    Comment = $"PoA Settlement with transaction ref no. {mObjSettlement.TransactionRefNo} and Amount {mObjSettlement.SettlementAmount}",
                                    IPAddress = CommUtil.GetIPAddress(),
                                    StaffID = SessionManager.UserID,
                                };

                                new BLAuditLog().BL_InsertAuditLog(mObjAuditLog);

                                scope.Complete();
                                FlashMessage.Info(mObjSettlementResponse.Message);
                                return RedirectToAction("PoASettlement", "OperationManager");

                            }
                            else
                            {
                                ViewBag.PoABalance = dcBalance;
                                ViewBag.MDAServiceList = SessionManager.lstMDAService.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
                                ViewBag.Message = mObjSettlementResponse.Message;
                                ViewBag.AmountToPay = SessionManager.lstMDAService.Where(t => t.intTrack != EnumList.Track.DELETE).Sum(t => t.ToSettleAmount);
                                Transaction.Current.Rollback();
                                return View(pObjSettlementModel);
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorSignal.FromCurrentContext().Raise(ex);
                            ViewBag.PoABalance = dcBalance;
                            ViewBag.MDAServiceList = SessionManager.lstMDAService.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
                            ViewBag.Message = "Error occurred while saving settlement";
                            ViewBag.AmountToPay = SessionManager.lstMDAService.Where(t => t.intTrack != EnumList.Track.DELETE).Sum(t => t.ToSettleAmount);
                            Transaction.Current.Rollback();
                            return View(pObjSettlementModel);
                        }
                    }
                }
            }
        }

        #endregion
        #region OM000 : GIS


        [HttpGet]
        public ActionResult GISSearchParameter()
        {
            List<GISFileHolder> gisFile = new List<GISFileHolder>();
            List<ReturnObject> lgisNewFile = new List<ReturnObject>();
            var kk = _appDbContext.GISFileParty.Where(x => x.PartyRelation == "Owner").ToList();

            var assItem = _appDbContext.GISFileAssessmentItem.GroupBy(i => i.FileNumber)
                                                               .Select(g => new
                                                               {
                                                                   fileNumber = g.Key,
                                                                   TotalSum = g.Sum(i => Convert.ToInt64(i.AssessmentAmount))
                                                               }).ToList();
            foreach (var k in kk)
            {
                ReturnObject gisNewFile = new ReturnObject();
                gisNewFile.FileNumber = k.FileNumber;
                gisNewFile.OwnerPhoneNumber = k.PartyPhone1;
                gisNewFile.OwnerFullName = k.PartyFullName;
                gisNewFile.FileNumber = k.FileNumber;
                gisNewFile.PageNo = k.PageNo;
                gisNewFile.AssessmentAmountII = assItem.FirstOrDefault(o => o.fileNumber == k.FileNumber).TotalSum;
                lgisNewFile.Add(gisNewFile);
            }

            ViewBag.gisFile = lgisNewFile;
            return View(lgisNewFile);
        }
        public ActionResult GISDetails(string newId, string id)
        {
            GISFileParty returnObject = new GISFileParty();
            var kkk = _appDbContext.GISFileParty.Where(o => o.PageNo == id && o.FileNumber == newId).ToList();
            ViewBag.PartyInfo = kkk;

            ViewBag.AssetInfo = _appDbContext.GISFileAsset.Where(o => o.PageNo == id && o.FileNumber == newId).ToList();

            var ret = _appDbContext.GISFileAssessment.Where(o => o.PageNo == id && o.FileNumber == newId).ToList();
            ViewBag.AssessmentInfo = ret;
            var retAssItem = _appDbContext.GISFileAssessmentItem.Where(o => o.PageNo == id && o.FileNumber == newId).ToList();
            foreach (var rr in retAssItem)
            {
                rr.AssessmentYear = ret.FirstOrDefault(o => o.AssessmentID == rr.AssessmentID).AssessmentYear;
                rr.DecimalAssessmentAmount = Convert.ToDecimal(rr.AssessmentAmount);
            }
            ViewBag.AssessmentItemInfo = retAssItem;
            var retInvoice = _appDbContext.GISFileInvoice.Where(o => o.PageNo == id && o.FileNumber == newId).ToList();
            ViewBag.InvoiceInfo = retInvoice;
            var retInvoiceItem = _appDbContext.GISFileInvoiceItem.Where(o => o.PageNo == id && o.FileNumber == newId).ToList();
            foreach (var rr in retInvoiceItem)
            {
                rr.InvoiceDate = retInvoice.FirstOrDefault(o => o.InvoiceID == rr.InvoiceID.ToString()).InvoiceDate;
                rr.DecimalAmount = Convert.ToDecimal(rr.Amount);
            }

            ViewBag.InvoiceItemInfo = retInvoiceItem;

            returnObject = kkk.FirstOrDefault(o => o.PartyRelation == "Owner");

            return View(returnObject);
        }
        public ActionResult Party(string newId, string id)
        {
            List<GISFileParty> gisFile = new List<GISFileParty>();
            gisFile = _appDbContext.GISFileParty.Where(o => o.PageNo == id && o.FileNumber == newId).ToList();

            return View(gisFile);
        }
        public ActionResult GetParty(long id)
        {
            GISFileParty gisFile = new GISFileParty();
            gisFile = _appDbContext.GISFileParty.FirstOrDefault(o => o.Id == id);

            return View(gisFile);
        }
        public ActionResult Asset(string newId, string id)
        {
            List<GISFileAsset> gisFile = new List<GISFileAsset>();
            gisFile = _appDbContext.GISFileAsset.Where(o => o.PageNo == id && o.FileNumber == newId).ToList();

            return View(gisFile);
        }
        public ActionResult GetAsset(long id)
        {
            GISFileAsset gisFile = new GISFileAsset();
            gisFile = _appDbContext.GISFileAsset.FirstOrDefault(o => o.Id == id);

            return View(gisFile);
        }
        public ActionResult Assessment(string newId, string id)
        {
            List<GISFileAssessment> gisFile = new List<GISFileAssessment>();
            gisFile = _appDbContext.GISFileAssessment.Where(o => o.PageNo == id && o.FileNumber == newId).ToList();

            return View(gisFile);
        }
        public ActionResult Invoice(string newId, string id)
        {
            List<GISFileInvoice> gisFile = new List<GISFileInvoice>();
            gisFile = _appDbContext.GISFileInvoice.Where(o => o.PageNo == id && o.FileNumber == newId).ToList();

            return View(gisFile);
        }
        public ActionResult GetAssessment(long id)
        {
            GISFileAssessment gisFile = new GISFileAssessment();
            gisFile = _appDbContext.GISFileAssessment.FirstOrDefault(o => o.Id == id);

            return View(gisFile);
        }
        public ActionResult GetInvoice(long id)
        {
            GISFileInvoice gisFile = new GISFileInvoice();
            gisFile = _appDbContext.GISFileInvoice.FirstOrDefault(o => o.Id == id);

            return View(gisFile);
        }
        public ActionResult GetAssessmentItem(long id)
        {
            GISFileAssessmentItem gisFile = new GISFileAssessmentItem();
            gisFile = _appDbContext.GISFileAssessmentItem.FirstOrDefault(o => o.Id == id);
            gisFile.AssessmentYear = _appDbContext.GISFileAssessment.FirstOrDefault(o => o.AssessmentID == gisFile.AssessmentID).AssessmentYear;
            return View(gisFile);
        }
        public ActionResult GetInvoiceItem(long id)
        {
            GISFileInvoiceItem gisFile = new GISFileInvoiceItem();
            gisFile = _appDbContext.GISFileInvoiceItem.FirstOrDefault(o => o.Id == id);
            gisFile.InvoiceDate = _appDbContext.GISFileInvoice.FirstOrDefault(o => o.InvoiceID == gisFile.InvoiceID.ToString()).InvoiceDate;
            return View(gisFile);
        }


        [HttpGet]
        public ActionResult newCallGISApi()
        {
            return View();
            //return RedirectToAction("CallGISApi", new {  PageNo = "1" });
        }
        [HttpPost]
        public ActionResult CallGISApi(string PageNo)
        {
            string ClientID = NewCommon.GISLogin;
            string FromDate = "2022-01-31";
            string ToDate = "2022-12-31";
            ResultModel printObj = new ResultModel();
            List<GISFileHolder> gisFile = new List<GISFileHolder>();
            List<ReturnObject> lgisNewFile = new List<ReturnObject>();
            //var today = DateTime.Now.ToString("dddd");
            //if (today != "SATURDAY")
            var checkIfInDb = _appDbContext.GISFileHolder.FirstOrDefault(o => o.PageNo == PageNo);
            if (checkIfInDb == null)
            {
                var url = NewCommon.GISApi;

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Accept = "application/json";
                httpWebRequest.Method = "POST";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"ClientID\":\"" + ClientID + "\",\"ModifiedFromDate\":\"" + FromDate + "\",\"ModifiedToDate\":\"" + ToDate + "\",\"PageNo\":\"" + PageNo + "\"}";
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    JObject joResponse = JObject.Parse(result);
                    printObj = JsonConvert.DeserializeObject<ResultModel>(result);

                    lgisNewFile = InsertIntoFile(printObj, PageNo);

                }
            }
            else
            {
                lgisNewFile = InsertIntoFile(printObj, PageNo);
            }
            ViewBag.gisFile = lgisNewFile;
            ViewBag.kk = PageNo;
            return RedirectToAction("GISSearchParameter");
        }
        public List<ReturnObject> InsertIntoFile(ResultModel resultModel, string PageNo)
        {
            List<GISFileHolder> gisFile = new List<GISFileHolder>();
            List<ReturnObject> lgisNewFile = new List<ReturnObject>();
            if (resultModel.File != null)
            {
                long f = 0;
                var Phrases = resultModel.File.ToList();
                foreach (var file in Phrases)
                {
                    f++;
                    GISFileHolder gISFileHolder = new GISFileHolder();
                    gISFileHolder.FileId = f;
                    gISFileHolder.PageNo = PageNo;
                    gISFileHolder.FileNumber = file.FileNumber;
                    gISFileHolder.CreationDate = file.CreationDate;
                    gISFileHolder.ModifiedDate = file.ModifiedDate;
                    gISFileHolder.DateSaved = DateTime.Now;
                    _appDbContext.GISFileHolder.Add(gISFileHolder);
                    foreach (var party in file.Party)
                    {
                        GISFileParty gISFileParty = new GISFileParty();
                        gISFileParty.DateSaved = DateTime.Now;
                        gISFileParty.PartyGender = party.PartyGender;
                        gISFileParty.PartyPhone1 = party.PartyPhone1;
                        gISFileParty.PartyTIN = party.PartyTIN;
                        gISFileParty.FileNumber = file.FileNumber;
                        gISFileParty.FileId = f;
                        gISFileParty.PageNo = PageNo;
                        gISFileParty.PartyExtID = party.PartyExtID;
                        gISFileParty.PartyID = party.PartyID;
                        gISFileParty.PartyTitle = party.PartyTitle;
                        gISFileParty.PartyFirstName = party.PartyFirstName;
                        gISFileParty.PartyLastName = party.PartyLastName;
                        gISFileParty.PartyMiddleName = party.PartyMiddleName;
                        gISFileParty.PartyFullName = party.PartyFullName;
                        gISFileParty.PartyType = party.PartyType;
                        gISFileParty.PartyDOB = party.PartyDOB;
                        gISFileParty.PartyNIN = party.PartyNIN;
                        gISFileParty.PartyPhone2 = party.PartyPhone2;
                        gISFileParty.PartyEmail = party.PartyEmail;
                        gISFileParty.PartyNationality = party.PartyNationality;
                        gISFileParty.PartyMaritalStatus = party.PartyMaritalStatus;
                        gISFileParty.PartyOccupation = party.PartyOccupation;
                        gISFileParty.ContactAddress = party.ContactAddress;
                        gISFileParty.PartyRelation = party.PartyRelation;
                        gISFileParty.AcquisitionDate = party.AcquisitionDate;

                        _appDbContext.GISFileParty.Add(gISFileParty);

                    }
                    foreach (var invoice in file.Invoice)
                    {
                        GISFileInvoice gIsFileInvoice = new GISFileInvoice();

                        gIsFileInvoice.FileId = f;
                        gIsFileInvoice.PageNo = PageNo;
                        gIsFileInvoice.FileNumber = file.FileNumber;
                        gIsFileInvoice.InvoiceDate = invoice.InvoiceDate;
                        gIsFileInvoice.InvoiceAmount = invoice.InvoiceAmount;
                        gIsFileInvoice.InvoiceNumber = invoice.InvoiceNumber;
                        gIsFileInvoice.isReversal = invoice.isReversal;
                        gIsFileInvoice.InvoiceID = invoice.InvoiceID;
                        gIsFileInvoice.DateSaved = DateTime.Now;
                        _appDbContext.GISFileInvoice.Add(gIsFileInvoice);
                        foreach (var inVioceitem in invoice.Items)
                        {
                            GISFileInvoiceItem gISFileInvoiceItem = new GISFileInvoiceItem();
                            gISFileInvoiceItem.InvoiceID = Convert.ToInt64(invoice.InvoiceID);
                            gISFileInvoiceItem.Amount = inVioceitem.Amount;
                            gISFileInvoiceItem.FileNumber = file.FileNumber;
                            gISFileInvoiceItem.PageNo = PageNo;
                            gISFileInvoiceItem.RevenueHeadId = inVioceitem.RevenueHeadId;
                            gISFileInvoiceItem.Description = inVioceitem.Description;
                            gISFileInvoiceItem.Year = inVioceitem.Year;
                            gISFileInvoiceItem.DateSaved = DateTime.Now;
                            _appDbContext.GISFileInvoiceItem.Add(gISFileInvoiceItem);
                        }
                    }
                    foreach (var ass in file.Assessment)
                    {
                        GISFileAssessment gISFileAssessment = new GISFileAssessment();
                        gISFileAssessment.FileId = f;
                        gISFileAssessment.PageNo = PageNo;
                        gISFileAssessment.FileNumber = file.FileNumber;
                        gISFileAssessment.AssessmentID = ass.AssessmentID;
                        gISFileAssessment.AssessmentYear = ass.AssessmentYear;
                        gISFileAssessment.DateSaved = DateTime.Now;
                        _appDbContext.GISFileAssessment.Add(gISFileAssessment);
                        foreach (var assitem in ass.LsAssessments)
                        {
                            GISFileAssessmentItem gISFileInvoiceItem = new GISFileAssessmentItem();
                            gISFileInvoiceItem.AssessmentID = ass.AssessmentID;
                            gISFileInvoiceItem.AssetNumber = assitem.AssetNumber;
                            gISFileInvoiceItem.PageNo = PageNo;
                            gISFileInvoiceItem.FileNumber = file.FileNumber;
                            gISFileInvoiceItem.AssessmentAmount = assitem.AssessmentAmount;
                            gISFileInvoiceItem.DateSaved = DateTime.Now;
                            _appDbContext.GISFileAssessmentItem.Add(gISFileInvoiceItem);
                        }
                    }
                    foreach (var asset in file.Asset)
                    {
                        GISFileAsset gISFileParty = new GISFileAsset();
                        gISFileParty.AssetNumber = asset.AssetNumber;
                        gISFileParty.AssetName = asset.AssetName;
                        gISFileParty.FileNumber = file.FileNumber;
                        gISFileParty.AssetType = asset.AssetType;
                        gISFileParty.AssetSubType = asset.AssetSubType;
                        gISFileParty.AssetLGA = asset.AssetLGA;
                        gISFileParty.AssetDistrict = asset.AssetDistrict;
                        gISFileParty.AssetWard = asset.AssetWard;
                        gISFileParty.AssetBillingZone = asset.AssetBillingZone;
                        gISFileParty.AssetSubzone = asset.AssetSubzone;
                        gISFileParty.AssetUse = asset.AssetUse;
                        gISFileParty.AssetPurpose = asset.AssetPurpose;
                        gISFileParty.AssetAddress = asset.AssetAddress;
                        gISFileParty.AssetRoadName = asset.AssetRoadName;
                        gISFileParty.AssetOffStreet = asset.AssetOffStreet;
                        gISFileParty.HoldingType = asset.HoldingType;
                        gISFileParty.AssetCofO = asset.AssetCofO;
                        gISFileParty.TitleDocument = asset.TitleDocument;
                        gISFileParty.SupportingDocument = asset.SupportingDocument;
                        gISFileParty.PartyID = asset.PartyID;
                        gISFileParty.OccupierStatus = asset.OccupierStatus;
                        gISFileParty.AnyOccupants = asset.AnyOccupants;
                        gISFileParty.OccupancyType = asset.OccupancyType;
                        gISFileParty.AssetModifiedDate = asset.AssetModifiedDate;
                        gISFileParty.AssetFootprintPresent = asset.AssetFootprintPresent;
                        gISFileParty.AssetAge = asset.AssetAge;
                        gISFileParty.AssetCompletionYear = asset.AssetCompletionYear;
                        gISFileParty.AssetFurnished = asset.AssetFurnished;
                        gISFileParty.AssetSize = asset.AssetSize;
                        gISFileParty.AssetPerimeter = asset.AssetPerimeter;
                        gISFileParty.NumberOfFloors = asset.NumberOfFloors;
                        gISFileParty.AssetLatitude = asset.AssetLatitude;
                        gISFileParty.AssetLongitude = asset.AssetLongitude;
                        gISFileParty.StateOfRepair = asset.StateOfRepair;
                        gISFileParty.LevelOfCompletion = asset.LevelOfCompletion;
                        gISFileParty.HasGenerator = asset.HasGenerator;
                        gISFileParty.HasSwimmingPool = asset.HasSwimmingPool;
                        gISFileParty.HasFence = asset.HasFence;
                        gISFileParty.HasBuildings = asset.HasBuildings;
                        gISFileParty.NumberOfBldgs = asset.NumberOfBldgs;
                        gISFileParty.WallMaterial = asset.WallMaterial;
                        gISFileParty.RoofMaterial = asset.RoofMaterial;
                        gISFileParty.SewageAccess = asset.SewageAccess;
                        gISFileParty.ElectricConnection = asset.ElectricConnection;
                        gISFileParty.WaterConnectionType = asset.WaterConnectionType;
                        gISFileParty.SolidWasteCollectionType = asset.SolidWasteCollectionType;
                        gISFileParty.DateSaved = DateTime.Now;
                        gISFileParty.FileId = f;
                        gISFileParty.PageNo = PageNo;
                        _appDbContext.GISFileAsset.Add(gISFileParty);

                    }
                }
                _appDbContext.SaveChanges();
            }
            var kk = _appDbContext.GISFileParty.Where(o => o.PageNo == PageNo && o.PartyRelation == "Owner").ToList();

            var assItem = _appDbContext.GISFileAssessmentItem.Where(o => o.PageNo == PageNo).GroupBy(i => i.FileNumber)
                                                               .Select(g => new
                                                               {
                                                                   fileNumber = g.Key,
                                                                   TotalSum = g.Sum(i => Convert.ToInt64(i.AssessmentAmount))
                                                               }).ToList();
            foreach (var k in kk)
            {
                ReturnObject gisNewFile = new ReturnObject();
                gisNewFile.FileNumber = k.FileNumber;
                gisNewFile.OwnerPhoneNumber = k.PartyPhone1;
                gisNewFile.OwnerFullName = k.PartyFullName;
                gisNewFile.FileNumber = k.FileNumber;
                gisNewFile.PageNo = k.PageNo;
                gisNewFile.AssessmentAmount = assItem.FirstOrDefault(o => o.fileNumber == k.FileNumber).TotalSum.ToString();
                lgisNewFile.Add(gisNewFile);
            }

            return lgisNewFile;
        }


        #endregion

        #region OM004 : Manage Payments by Revenue Stream


        [HttpGet]
        public ActionResult PaymentByRevenueStream()
        {
            string url = getUrl();
            bool itCan = new UtilityController().CheckAccess(url);
            if (!itCan) { return RedirectToAction("AccessDenied", "Utility"); }
            IList<SelectListItem> lstPaymentType = new List<SelectListItem>
            {
                new SelectListItem(){Value="0",Text="All",Selected=true},
                new SelectListItem(){Value="1",Text="Settlement"},
                new SelectListItem(){Value="2",Text="Payment On Account"}
            };

            ViewBag.PaymentTypeList = lstPaymentType;

            UI_FillYearDropDown();

            return View();
        }


        [HttpPost]
        public JsonResult PaymentByRevenueStreamLoadData(int TaxYear, DateTime? FromDate, DateTime? ToDate, int PaymentTypeID)
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? TrynParse.parseInt(vLength) : 0;
            int IntSkip = vStart != null ? TrynParse.parseInt(vStart) : 0;
            int IntTotalRecords = 0;

            IList<usp_GetPaymentByRevenueStream_Result> lstPaymentByRevenueStream = new BLOperationManager().BL_GetPaymentByRevenueStream(PaymentTypeID, TaxYear, FromDate, ToDate);

            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstPaymentByRevenueStream = lstPaymentByRevenueStream.Where(t =>
                t.RevenueStreamName != null && t.RevenueStreamName.Trim().Contains(vFilter.ToLower().Trim()) ||
                t.Amount != null && t.Amount.GetValueOrDefault().ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TransactionCount != null && t.TransactionCount.GetValueOrDefault().ToString().Trim().Contains(vFilter.ToLower().Trim())).ToList();
            }

            IDictionary<string, object> dcFooterTotal = new Dictionary<string, object>();

            dcFooterTotal["Amount"] = lstPaymentByRevenueStream.Sum(t => t.Amount);
            dcFooterTotal["TransactionCount"] = lstPaymentByRevenueStream.Sum(t => t.TransactionCount);

            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstPaymentByRevenueStream = lstPaymentByRevenueStream.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstPaymentByRevenueStream.Count;
            var data = lstPaymentByRevenueStream.Skip(IntSkip).Take(IntPageSize).ToList();


            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data, FooterTotal = dcFooterTotal }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PaymentByRevenueStreamExportToExcel(int TaxYear, DateTime? FromDate, DateTime? ToDate, int PaymentTypeID)
        {

            IList<usp_GetPaymentByRevenueStream_Result> lstPaymentByRevenueStreamData = new BLOperationManager().BL_GetPaymentByRevenueStream(PaymentTypeID, TaxYear, FromDate, ToDate);

            string[] strColumns = new string[] { "RevenueStreamName", "Amount", "TransactionCount" };
            string[] strTotalColumns = new string[] { "Amount", "TransactionCount" };
            string method = MethodBase.GetCurrentMethod().Name;

            return ExportToExcel(lstPaymentByRevenueStreamData, this.RouteData, strColumns, true, strTotalColumns, method);

        }


        [HttpGet]
        public ActionResult PaymentByRevenueStreamDetail(int? ptypeId, int tyear, DateTime? fdate, DateTime? tdate, int? rstrmID)
        {
            if (rstrmID > 0)
            {
                ViewBag.FromDate = fdate;
                ViewBag.ToDate = tdate;
                ViewBag.PaymentType = ptypeId;
                ViewBag.RevenueStream = rstrmID;
                ViewBag.TaxYear = tyear;
            }
            else
            {
                return RedirectToAction("PaymentByRevenueStream", "OperationManager");
            }

            return View();

        }

        public JsonResult PaymentByRevenueStreamDetailLoadData(int TaxYear, int? PaymentTypeID, DateTime? FromDate, DateTime? ToDate, int? RevenueStreamID)
        {
            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();

            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? TrynParse.parseInt(vLength) : 0;
            int IntSkip = vStart != null ? TrynParse.parseInt(vStart) : 0;
            int IntTotalRecords = 0;


            IList<usp_GetPaymentByRevenueStreamDetail_Result> lstPaymentByRevenueStreamData = new BLOperationManager().BL_GetPaymentByRevenueStreamDetails(TaxYear, FromDate, ToDate, PaymentTypeID, RevenueStreamID);

            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstPaymentByRevenueStreamData = lstPaymentByRevenueStreamData.Where(t => t.PaymentRef != null && t.PaymentRef.ToLower().Trim().Contains(vFilter.ToLower().Trim())
                || t.TaxPayerName != null && t.TaxPayerName.ToLower().Trim().Contains(vFilter.ToLower().Trim())
                || t.Amount.ToString().Trim().Contains(vFilter.Trim())
                || t.PaymentDate.Value.ToString("dd-MMM-yy").ToLower().Trim().Contains(vFilter.ToLower().Trim())
                || t.PaymentMethodName != null && t.PaymentMethodName.ToLower().Trim().Contains(vFilter.ToLower().Trim())).ToList();
            }


            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstPaymentByRevenueStreamData = lstPaymentByRevenueStreamData.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstPaymentByRevenueStreamData.Count;
            var data = lstPaymentByRevenueStreamData.Skip(IntSkip).Take(IntPageSize).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PaymentByRevenueStreamDetailExportToExcel(int TaxYear, int? PaymentTypeID, DateTime? FromDate, DateTime? ToDate, int? RevenueStreamID)
        {
            IList<usp_GetPaymentByRevenueStreamDetail_Result> lstData = new BLOperationManager().BL_GetPaymentByRevenueStreamDetails(TaxYear, FromDate, ToDate, PaymentTypeID, RevenueStreamID);

            string[] strColumns = new string[] { "TaxPayerTypeName", "TaxPayerRIN", "TaxPayerName", "PaymentDate", "PaymentRef", "PaymentMethodName", "Amount" };
            string[] strTotalColumns = new string[] { "Amount" };
            string method = MethodBase.GetCurrentMethod().Name;


            return ExportToExcel(lstData, this.RouteData, strColumns, true, strTotalColumns, method);

        }

        #endregion

        #region OM005 : Manage Assessments by Revenue Stream


        [HttpGet]
        public ActionResult BillByRevenueStream()
        {

            IList<SelectListItem> lstBillType = new List<SelectListItem>
            {
                new SelectListItem(){Value="0",Text="All",Selected=true},
                new SelectListItem(){Value="1",Text="Assessment"},
                new SelectListItem(){Value="2",Text="Service Bills"}
            };

            ViewBag.BillTypeList = lstBillType;

            IList<SelectListItem> lstBillStatus = new List<SelectListItem>
            {
                new SelectListItem(){Value="0",Text="All",Selected=true},
                new SelectListItem(){Value="1",Text="Assessed"},
                new SelectListItem(){Value="3",Text="Partial"},
                new SelectListItem(){Value="4",Text="Settled"}
            };

            ViewBag.BillStatusList = lstBillStatus;

            UI_FillYearDropDown();
            return View();
        }

        [HttpPost]
        public JsonResult BillByRevenueStreamLoadData(int TaxYear, DateTime? FromDate, DateTime? ToDate, int BillTypeID, int BillStatusID)
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? TrynParse.parseInt(vLength) : 0;
            int IntSkip = vStart != null ? TrynParse.parseInt(vStart) : 0;
            int IntTotalRecords = 0;

            IList<usp_GetBillByRevenueStream_Result> lstBillByRevenueStream = new BLOperationManager().BL_GetBillByRevenueStream(TaxYear, FromDate, ToDate, BillTypeID, BillStatusID);

            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstBillByRevenueStream = lstBillByRevenueStream.Where(t =>
                t.RevenueStreamName != null && t.RevenueStreamName.Trim().Contains(vFilter.ToLower().Trim()) ||
                t.Amount != null && t.Amount.GetValueOrDefault().ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TransactionCount != null && t.TransactionCount.GetValueOrDefault().ToString().Trim().Contains(vFilter.ToLower().Trim())).ToList();
            }

            IDictionary<string, object> dcFooterTotal = new Dictionary<string, object>();

            dcFooterTotal["Amount"] = lstBillByRevenueStream.Sum(t => t.Amount);
            dcFooterTotal["TransactionCount"] = lstBillByRevenueStream.Sum(t => t.TransactionCount);

            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstBillByRevenueStream = lstBillByRevenueStream.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstBillByRevenueStream.Count;
            var data = lstBillByRevenueStream.Skip(IntSkip).Take(IntPageSize).ToList();


            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data, FooterTotal = dcFooterTotal }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult BillByRevenueStreamExportToExcel(int TaxYear, DateTime? FromDate, DateTime? ToDate, int BillTypeID, int BillStatusID)
        {

            IList<usp_GetBillByRevenueStream_Result> lstBillByRevenueStream = new BLOperationManager().BL_GetBillByRevenueStream(TaxYear, FromDate, ToDate, BillTypeID, BillStatusID);

            string[] strColumns = new string[] { "RevenueStreamName", "Amount", "TransactionCount" };
            string[] strTotalColumns = new string[] { "Amount", "TransactionCount" };
            string method = MethodBase.GetCurrentMethod().Name;


            return ExportToExcel(lstBillByRevenueStream, this.RouteData, strColumns, true, strTotalColumns, method);

        }


        [HttpGet]
        public ActionResult BillByRevenueStreamDetail(int tyear, DateTime? fdate, DateTime? tdate, int? rstrmID, int? btypeID, int? bstatID)
        {
            if (rstrmID > 0)
            {
                ViewBag.TaxYear = tyear;
                ViewBag.FromDate = fdate;
                ViewBag.ToDate = tdate;
                ViewBag.BillType = btypeID;
                ViewBag.BillStatus = bstatID;
                ViewBag.RevenueStream = rstrmID;
            }
            else
            {
                return RedirectToAction("BillByRevenueStream", "OperationManager");
            }

            return View();
        }

        [HttpPost]
        public JsonResult BillByRevenueStreamDetailLoadData(int TaxYear, DateTime? FromDate, DateTime? ToDate, int BillStatusID, int BillTypeID, int RevenueStreamID)
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? TrynParse.parseInt(vLength) : 0;
            int IntSkip = vStart != null ? TrynParse.parseInt(vStart) : 0;
            int IntTotalRecords = 0;


            IList<usp_GetBillByRevenueStreamDetail_Result> lstBillByRevenueStreamData = new BLOperationManager().BL_GetBillByRevenueStreamDetails(TaxYear, FromDate, ToDate, BillTypeID, BillStatusID, RevenueStreamID);


            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstBillByRevenueStreamData = lstBillByRevenueStreamData.Where(t => t.BillRef != null && t.BillRef.ToLower().Trim().Contains(vFilter.ToLower().Trim())
                || t.TaxPayerName != null && t.TaxPayerName.ToLower().Trim().Contains(vFilter.ToLower().Trim())
                || t.Amount.ToString().Trim().Contains(vFilter.Trim())
                || t.BillDate.Value.ToString("dd-MMM-yy").ToLower().Trim().Contains(vFilter.ToLower().Trim())
                || t.BillStatusName != null && t.BillStatusName.ToLower().Trim().Contains(vFilter.ToLower().Trim())).ToList();
            }


            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstBillByRevenueStreamData = lstBillByRevenueStreamData.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstBillByRevenueStreamData.Count;
            var data = lstBillByRevenueStreamData.Skip(IntSkip).Take(IntPageSize).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult BillByRevenueStreamDetailExportToExcel(int TaxYear, DateTime? FromDate, DateTime? ToDate, int BillStatusID, int BillTypeID, int RevenueStreamID)
        {
            IList<usp_GetBillByRevenueStreamDetail_Result> lstBillByRevenueStreamData = new BLOperationManager().BL_GetBillByRevenueStreamDetails(TaxYear, FromDate, ToDate, BillTypeID, BillStatusID, RevenueStreamID);

            string[] strColumns = new string[] { "TaxPayerTypeName", "TaxPayerRIN", "TaxPayerName", "BillDate", "BillRef", "BillStatusName", "Amount" };
            string[] strTotalColumns = new string[] { "Amount" };
            string method = MethodBase.GetCurrentMethod().Name;

            return ExportToExcel(lstBillByRevenueStreamData, this.RouteData, strColumns, true, strTotalColumns, method);
        }

        #endregion

        #region OM006 – Manage Bills Aging by Revenue Stream


        [HttpGet]
        public ActionResult BillAgingByRevenueStream()
        {
            ViewBag.BillTypeList = new List<SelectListItem>
            {
                new SelectListItem(){Value="0",Text="All",Selected=true},
                new SelectListItem(){Value="1",Text="Service Bills"},
                new SelectListItem(){Value="2",Text="Assessments"}
            }; ;

            ViewBag.StageList = new List<SelectListItem>
            {
                 new SelectListItem(){Value="0",Text="All",Selected=true},
                new SelectListItem(){Value="1",Text="0 – 3 months"},
                new SelectListItem(){Value="2",Text="3 – 6 months"},
                new SelectListItem(){Value="3",Text="6 – 12 months"},
                new SelectListItem(){Value="4",Text="Greater than 12 months"}
            };

            return View();
        }

        [HttpPost]
        public JsonResult BillAgingByRevenueStreamLoadData(int BillTypeID, int StageID)
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();

            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? TrynParse.parseInt(vLength) : 0;
            int IntSkip = vStart != null ? TrynParse.parseInt(vStart) : 0;
            int IntTotalRecords = 0;

            IList<usp_GetBillAgingByRevenueStream_Result> lstBillAgingByRevenueStream = new BLOperationManager().BL_GetBillAgingByRevenueStream(BillTypeID, StageID);

            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstBillAgingByRevenueStream = lstBillAgingByRevenueStream.Where(t =>
                t.RevenueStreamName != null && t.RevenueStreamName.Trim().Contains(vFilter.ToLower().Trim()) ||
                t.Amount != null && t.Amount.GetValueOrDefault().ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TransactionCount != null && t.TransactionCount.GetValueOrDefault().ToString().Trim().Contains(vFilter.ToLower().Trim())).ToList();
            }

            IDictionary<string, object> dcFooterTotal = new Dictionary<string, object>();

            dcFooterTotal["Amount"] = lstBillAgingByRevenueStream.Sum(t => t.Amount);
            dcFooterTotal["TransactionCount"] = lstBillAgingByRevenueStream.Sum(t => t.TransactionCount);

            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstBillAgingByRevenueStream = lstBillAgingByRevenueStream.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstBillAgingByRevenueStream.Count;
            var data = lstBillAgingByRevenueStream.Skip(IntSkip).Take(IntPageSize).ToList();


            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data, FooterTotal = dcFooterTotal }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult BillAgingByRevenueStreamExportToExcel(int BillTypeID, int StageID)
        {

            IList<usp_GetBillAgingByRevenueStream_Result> lstBillAgingByRevenueStream = new BLOperationManager().BL_GetBillAgingByRevenueStream(BillTypeID, StageID);

            string[] strColumns = new string[] { "RevenueStreamName", "Amount", "TransactionCount" };
            string[] strTotalColumns = new string[] { "Amount", "TransactionCount" };
            string method = MethodBase.GetCurrentMethod().Name;

            return ExportToExcel(lstBillAgingByRevenueStream, this.RouteData, strColumns, true, strTotalColumns, method);

        }


        [HttpGet]
        public ActionResult BillAgingDetailsByRevenueStream(int? btypeId, int? stgid, int? rstrmID)
        {
            if (rstrmID.GetValueOrDefault() > 0)
            {

                ViewBag.BillTypeID = btypeId;
                ViewBag.StageID = stgid;
                ViewBag.RevenueStreamID = rstrmID;
                return View();
            }
            else
            {
                return RedirectToAction("BillAgingByRevenueStream");
            }
        }


        [HttpPost]
        public JsonResult BillAgingDetailsByRevenueStreamLoadData(int? BillTypeID, int? StageID, int? RevenueStreamID)
        {
            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();

            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? TrynParse.parseInt(vLength) : 0;
            int IntSkip = vStart != null ? TrynParse.parseInt(vStart) : 0;
            int IntTotalRecords = 0;


            IList<usp_GetBillAgingDetailByRevenueStream_Result> lstData = new BLOperationManager().BL_GetBillAgingDetailByRevenueStream(BillTypeID, StageID, RevenueStreamID);
            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstData = lstData.Where(t => t.TaxPayerName != null && t.TaxPayerName.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                                             t.TaxPayerRIN != null && t.TaxPayerRIN.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                                             t.TaxPayerTypeName != null && t.TaxPayerTypeName.ToLower().Trim().Contains(vFilter.ToLower().Trim())).ToList();
            }


            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstData = lstData.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstData.Count;
            var data = lstData.Skip(IntSkip).Take(IntPageSize).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult BillAgingDetailsByRevenueStreamExportToExcel(int? BillTypeID, int? StageID, int? RevenueStreamID)
        {

            IList<usp_GetBillAgingDetailByRevenueStream_Result> lstData = new BLOperationManager().BL_GetBillAgingDetailByRevenueStream(BillTypeID, StageID, RevenueStreamID);

            string[] strColumns = new string[] { "TaxPayerTypeName", "TaxPayerName", "TaxPayerTIN", "BillRef", "BillDate", "Amount", "BillStatusName", "ContactNumber", "ContactAddress" };
            string[] strTotalColumns = new string[] { "Amount" };
            string method = MethodBase.GetCurrentMethod().Name;

            return ExportToExcel(lstData, this.RouteData, strColumns, true, strTotalColumns, method);

        }

        #endregion

        #region OM007 – Manage PoA Taxpayer Without Asset


        [HttpGet]
        public ActionResult POATaxPayerWithoutAsset()
        {
            string url = getUrl();
            bool itCan = new UtilityController().CheckAccess(url);
            if (!itCan) { return RedirectToAction("AccessDenied", "Utility"); }
            UI_FillYearDropDown();
            UI_FillTaxOfficeDropDown();
            UI_FillTaxPayerTypeDropDown();

            return View();
        }

        [HttpPost]
        public JsonResult POATaxPayerWithoutAssetLoadData(int TaxYear, DateTime? FromDate, DateTime? ToDate, int TaxPayerTypeID, int TaxOfficeID)
        {
            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();

            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? TrynParse.parseInt(vLength) : 0;
            int IntSkip = vStart != null ? TrynParse.parseInt(vStart) : 0;
            int IntTotalRecords = 0;


            IList<usp_GetPOATaxPayerWithoutAsset_Result> lstPOATaxPayerWithoutAsset = new BLOperationManager().BL_GetPOATaxPayerWithoutAsset(TaxYear, FromDate, ToDate, TaxPayerTypeID, TaxOfficeID);

            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstPOATaxPayerWithoutAsset = lstPOATaxPayerWithoutAsset.Where(t => t.TaxPayerRIN != null && t.TaxPayerRIN.ToLower().Trim().Contains(vFilter.ToLower().Trim())
                || t.TaxPayerName != null && t.TaxPayerName.ToLower().Trim().Contains(vFilter.ToLower().Trim())
                || t.TaxPayerMobileNumber != null && t.TaxPayerMobileNumber.ToLower().Trim().Contains(vFilter.ToLower().Trim())
                || t.TaxPayerAddress != null && t.TaxPayerAddress.ToLower().Trim().Contains(vFilter.ToLower().Trim())).ToList();
            }


            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstPOATaxPayerWithoutAsset = lstPOATaxPayerWithoutAsset.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstPOATaxPayerWithoutAsset.Count;
            var data = lstPOATaxPayerWithoutAsset.Skip(IntSkip).Take(IntPageSize).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public FileResult POATaxPayerWithoutAssetExportToExcel(int TaxYear, DateTime? FromDate, DateTime? ToDate, int TaxPayerTypeID, int TaxOfficeID)
        {
            IList<usp_GetPOATaxPayerWithoutAsset_Result> lstPOATaxPayerWithoutAsset = new BLOperationManager().BL_GetPOATaxPayerWithoutAsset(TaxYear, FromDate, ToDate, TaxPayerTypeID, TaxOfficeID);
            string[] strColumns = new string[] { "TaxPayerTypeName", "TaxPayerRIN", "TaxPayerName", "TaxPayerMobileNumber", "TaxPayerAddress" };
            string method = MethodBase.GetCurrentMethod().Name;

            return ExportToExcel(lstPOATaxPayerWithoutAsset, this.RouteData, strColumns, false, null, method);
        }

        [HttpGet]
        public ActionResult RunAutoProfiler()
        {
            UI_FillYearDropDown();
            UI_FillTaxOfficeDropDown();
            UI_FillTaxPayerTypeDropDown();

            return View();
        }



        #endregion

        #region OM008 : Manage Payments by Tax Office


        [HttpGet]
        public ActionResult PaymentByTaxOffice()
        {
            string url = getUrl();
            bool itCan = new UtilityController().CheckAccess(url);
            if (!itCan) { return RedirectToAction("AccessDenied", "Utility"); }
            IList<SelectListItem> lstPaymentType = new List<SelectListItem>
            {
                new SelectListItem(){Value="0",Text="All",Selected=true},
                new SelectListItem(){Value="1",Text="Settlement"},
                new SelectListItem(){Value="2",Text="Payment On Account"}
            };

            ViewBag.PaymentTypeList = lstPaymentType;

            UI_FillYearDropDown();

            return View();
        }

        [HttpPost]
        public JsonResult PaymentByTaxOfficeLoadData(int TaxYear, DateTime? FromDate, DateTime? ToDate, int PaymentTypeID)
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? TrynParse.parseInt(vLength) : 0;
            int IntSkip = vStart != null ? TrynParse.parseInt(vStart) : 0;
            int IntTotalRecords = 0;

            IList<usp_GetPaymentByTaxOffice_Result> lstPaymentByTaxOffice = new BLOperationManager().BL_GetPaymentByTaxOffice(PaymentTypeID, TaxYear, FromDate, ToDate);

            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstPaymentByTaxOffice = lstPaymentByTaxOffice.Where(t =>
                t.TaxOfficeName != null && t.TaxOfficeName.Trim().Contains(vFilter.ToLower().Trim()) ||
                t.Amount != null && t.Amount.GetValueOrDefault().ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TransactionCount != null && t.TransactionCount.GetValueOrDefault().ToString().Trim().Contains(vFilter.ToLower().Trim())).ToList();
            }

            IDictionary<string, object> dcFooterTotal = new Dictionary<string, object>();

            dcFooterTotal["Amount"] = lstPaymentByTaxOffice.Sum(t => t.Amount);
            dcFooterTotal["TransactionCount"] = lstPaymentByTaxOffice.Sum(t => t.TransactionCount);

            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstPaymentByTaxOffice = lstPaymentByTaxOffice.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstPaymentByTaxOffice.Count;
            var data = lstPaymentByTaxOffice.Skip(IntSkip).Take(IntPageSize).ToList();


            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data, FooterTotal = dcFooterTotal }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PaymentByTaxOfficeExportToExcel(int TaxYear, DateTime? FromDate, DateTime? ToDate, int PaymentTypeID)
        {

            IList<usp_GetPaymentByTaxOffice_Result> lstPaymentByTaxOfficeData = new BLOperationManager().BL_GetPaymentByTaxOffice(PaymentTypeID, TaxYear, FromDate, ToDate);

            string[] strColumns = new string[] { "TaxOfficeName", "Amount", "TransactionCount" };
            string[] strTotalColumns = new string[] { "Amount", "TransactionCount" };
            string method = MethodBase.GetCurrentMethod().Name;

            byte[] ObjExcelData = CommUtil.ToExcel(lstPaymentByTaxOfficeData, $"{method}");
            return File(ObjExcelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{method}" + DateTime.Now.ToString("dd_MM_yy") + ".xlsx");
        }


        [HttpGet]
        public ActionResult PaymentByTaxOfficeDetail(int? ptypeId, int tyear, DateTime? fdate, DateTime? tdate, int? toffID)
        {
            if (toffID > 0)
            {
                ViewBag.FromDate = fdate;
                ViewBag.ToDate = tdate;
                ViewBag.PaymentType = ptypeId;
                ViewBag.TaxOffice = toffID;
                ViewBag.TaxYear = tyear;
            }
            else
            {
                return RedirectToAction("PaymentByTaxOffice", "OperationManager");
            }

            return View();

        }

        [HttpPost]
        public JsonResult PaymentByTaxOfficeDetailLoadData(int TaxYear, int? PaymentTypeID, DateTime? FromDate, DateTime? ToDate, int? TaxOfficeID)
        {
            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();

            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? TrynParse.parseInt(vLength) : 0;
            int IntSkip = vStart != null ? TrynParse.parseInt(vStart) : 0;
            int IntTotalRecords = 0;


            IList<usp_GetPaymentByTaxOfficeDetail_Result> lstPaymentByTaxOfficeData = new BLOperationManager().BL_GetPaymentByTaxOfficeDetails(TaxYear, FromDate, ToDate, PaymentTypeID, TaxOfficeID);

            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstPaymentByTaxOfficeData = lstPaymentByTaxOfficeData.Where(t => t.PaymentRef != null && t.PaymentRef.ToLower().Trim().Contains(vFilter.ToLower().Trim())
                || t.TaxPayerName != null && t.TaxPayerName.ToLower().Trim().Contains(vFilter.ToLower().Trim())
                || t.Amount.ToString().Trim().Contains(vFilter.Trim())
                || t.PaymentDate.Value.ToString("dd-MMM-yy").ToLower().Trim().Contains(vFilter.ToLower().Trim())
                || t.PaymentMethodName != null && t.PaymentMethodName.ToLower().Trim().Contains(vFilter.ToLower().Trim())).ToList();
            }


            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstPaymentByTaxOfficeData = lstPaymentByTaxOfficeData.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstPaymentByTaxOfficeData.Count;
            var data = lstPaymentByTaxOfficeData.Skip(IntSkip).Take(IntPageSize).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PaymentByTaxOfficeDetailExportToExcel(int TaxYear, int? PaymentTypeID, DateTime? FromDate, DateTime? ToDate, int? TaxOfficeID)
        {
            IList<usp_GetPaymentByTaxOfficeDetail_Result> lstData = new BLOperationManager().BL_GetPaymentByTaxOfficeDetails(TaxYear, FromDate, ToDate, PaymentTypeID, TaxOfficeID);

            string[] strColumns = new string[] { "TaxPayerTypeName", "TaxPayerRIN", "TaxPayerName", "PaymentDate", "PaymentRef", "PaymentMethodName", "Amount" };
            string[] strTotalColumns = new string[] { "Amount" };
            string method = MethodBase.GetCurrentMethod().Name;

            return ExportToExcel(lstData, this.RouteData, strColumns, true, strTotalColumns, method);

        }

        #endregion

        #region OM009 : Manage Assessments by Tax Office


        [HttpGet]
        public ActionResult BillByTaxOffice()
        {
            string url = getUrl();
            bool itCan = new UtilityController().CheckAccess(url);
            if (!itCan) { return RedirectToAction("AccessDenied", "Utility"); }
            IList<SelectListItem> lstBillType = new List<SelectListItem>
            {
                new SelectListItem(){Value="0",Text="All",Selected=true},
                new SelectListItem(){Value="1",Text="Assessment"},
                new SelectListItem(){Value="2",Text="Service Bills"}
            };

            ViewBag.BillTypeList = lstBillType;

            IList<SelectListItem> lstBillStatus = new List<SelectListItem>
            {
                new SelectListItem(){Value="0",Text="All",Selected=true},
                new SelectListItem(){Value="1",Text="Assessed"},
                new SelectListItem(){Value="3",Text="Partial"},
                new SelectListItem(){Value="4",Text="Settled"}
            };

            ViewBag.BillStatusList = lstBillStatus;

            UI_FillYearDropDown();
            return View();
        }

        [HttpPost]
        public JsonResult BillByTaxOfficeLoadData(int TaxYear, DateTime? FromDate, DateTime? ToDate, int BillTypeID, int BillStatusID)
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? TrynParse.parseInt(vLength) : 0;
            int IntSkip = vStart != null ? TrynParse.parseInt(vStart) : 0;
            int IntTotalRecords = 0;

            IList<usp_GetBillByTaxOffice_Result> lstBillByTaxOffice = new BLOperationManager().BL_GetBillByTaxOffice(TaxYear, FromDate, ToDate, BillTypeID, BillStatusID);

            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstBillByTaxOffice = lstBillByTaxOffice.Where(t =>
                t.TaxOfficeName != null && t.TaxOfficeName.Trim().Contains(vFilter.ToLower().Trim()) ||
                t.Amount != null && t.Amount.GetValueOrDefault().ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TransactionCount != null && t.TransactionCount.GetValueOrDefault().ToString().Trim().Contains(vFilter.ToLower().Trim())).ToList();
            }

            IDictionary<string, object> dcFooterTotal = new Dictionary<string, object>();

            dcFooterTotal["Amount"] = lstBillByTaxOffice.Sum(t => t.Amount);
            dcFooterTotal["TransactionCount"] = lstBillByTaxOffice.Sum(t => t.TransactionCount);

            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstBillByTaxOffice = lstBillByTaxOffice.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstBillByTaxOffice.Count;
            var data = lstBillByTaxOffice.Skip(IntSkip).Take(IntPageSize).ToList();


            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data, FooterTotal = dcFooterTotal }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult BillByTaxOfficeExportToExcel(int TaxYear, DateTime? FromDate, DateTime? ToDate, int BillTypeID, int BillStatusID)
        {

            IList<usp_GetBillByTaxOffice_Result> lstBillByTaxOffice = new BLOperationManager().BL_GetBillByTaxOffice(TaxYear, FromDate, ToDate, BillTypeID, BillStatusID);

            string[] strColumns = new string[] { "TaxOfficeName", "Amount", "TransactionCount" };
            string[] strTotalColumns = new string[] { "Amount", "TransactionCount" }; string method = MethodBase.GetCurrentMethod().Name;


            return ExportToExcel(lstBillByTaxOffice, this.RouteData, strColumns, true, strTotalColumns, method);

        }


        [HttpGet]
        public ActionResult BillByTaxOfficeDetail(int tyear, DateTime? fdate, DateTime? tdate, int? toffID, int? btypeID, int? bstatID)
        {
            if (toffID > 0)
            {
                ViewBag.TaxYear = tyear;
                ViewBag.FromDate = fdate;
                ViewBag.ToDate = tdate;
                ViewBag.BillType = btypeID;
                ViewBag.BillStatus = bstatID;
                ViewBag.TaxOffice = toffID;
            }
            else
            {
                return RedirectToAction("BillByTaxOffice", "OperationManager");
            }

            return View();
        }

        [HttpPost]
        public JsonResult BillByTaxOfficeDetailLoadData(int TaxYear, DateTime? FromDate, DateTime? ToDate, int BillStatusID, int BillTypeID, int TaxOfficeID)
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? TrynParse.parseInt(vLength) : 0;
            int IntSkip = vStart != null ? TrynParse.parseInt(vStart) : 0;
            int IntTotalRecords = 0;


            IList<usp_GetBillByTaxOfficeDetail_Result> lstBillByTaxOfficeData = new BLOperationManager().BL_GetBillByTaxOfficeDetails(TaxYear, FromDate, ToDate, BillTypeID, BillStatusID, TaxOfficeID);


            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstBillByTaxOfficeData = lstBillByTaxOfficeData.Where(t => t.BillRef != null && t.BillRef.ToLower().Trim().Contains(vFilter.ToLower().Trim())
                || t.TaxPayerName != null && t.TaxPayerName.ToLower().Trim().Contains(vFilter.ToLower().Trim())
                || t.Amount.ToString().Trim().Contains(vFilter.Trim())
                || t.BillDate.Value.ToString("dd-MMM-yy").ToLower().Trim().Contains(vFilter.ToLower().Trim())
                || t.BillStatusName != null && t.BillStatusName.ToLower().Trim().Contains(vFilter.ToLower().Trim())).ToList();
            }


            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstBillByTaxOfficeData = lstBillByTaxOfficeData.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstBillByTaxOfficeData.Count;
            var data = lstBillByTaxOfficeData.Skip(IntSkip).Take(IntPageSize).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult BillByTaxOfficeDetailExportToExcel(int TaxYear, DateTime? FromDate, DateTime? ToDate, int BillStatusID, int BillTypeID, int TaxOfficeID)
        {
            IList<usp_GetBillByTaxOfficeDetail_Result> lstBillByTaxOfficeData = new BLOperationManager().BL_GetBillByTaxOfficeDetails(TaxYear, FromDate, ToDate, BillTypeID, BillStatusID, TaxOfficeID);

            string[] strColumns = new string[] { "TaxPayerTypeName", "TaxPayerRIN", "TaxPayerName", "BillDate", "BillRef", "BillStatusName", "Amount" };
            string[] strTotalColumns = new string[] { "Amount" }; string method = MethodBase.GetCurrentMethod().Name;


            return ExportToExcel(lstBillByTaxOfficeData, this.RouteData, strColumns, true, strTotalColumns, method);
        }

        #endregion

        #region OM0010 – Manage Bills Aging by Tax Office


        [HttpGet]
        public ActionResult BillAgingByTaxOffice()
        {
            ViewBag.BillTypeList = new List<SelectListItem>
            {
                new SelectListItem(){Value="0",Text="All",Selected=true},
                new SelectListItem(){Value="1",Text="Service Bills"},
                new SelectListItem(){Value="2",Text="Assessments"}
            }; ;

            ViewBag.StageList = new List<SelectListItem>
            {
                 new SelectListItem(){Value="0",Text="All",Selected=true},
                new SelectListItem(){Value="1",Text="0 – 3 months"},
                new SelectListItem(){Value="2",Text="3 – 6 months"},
                new SelectListItem(){Value="3",Text="6 – 12 months"},
                new SelectListItem(){Value="4",Text="Greater than 12 months"}
            };

            return View();
        }

        [HttpPost]
        public JsonResult BillAgingByTaxOfficeLoadData(int BillTypeID, int StageID)
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();

            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? TrynParse.parseInt(vLength) : 0;
            int IntSkip = vStart != null ? TrynParse.parseInt(vStart) : 0;
            int IntTotalRecords = 0;

            IList<usp_GetBillAgingByTaxOffice_Result> lstBillAgingByTaxOffice = new BLOperationManager().BL_GetBillAgingByTaxOffice(BillTypeID, StageID);

            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstBillAgingByTaxOffice = lstBillAgingByTaxOffice.Where(t =>
                t.TaxOfficeName != null && t.TaxOfficeName.Trim().Contains(vFilter.ToLower().Trim()) ||
                t.Amount != null && t.Amount.GetValueOrDefault().ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TransactionCount != null && t.TransactionCount.GetValueOrDefault().ToString().Trim().Contains(vFilter.ToLower().Trim())).ToList();
            }

            IDictionary<string, object> dcFooterTotal = new Dictionary<string, object>();

            dcFooterTotal["Amount"] = lstBillAgingByTaxOffice.Sum(t => t.Amount);
            dcFooterTotal["TransactionCount"] = lstBillAgingByTaxOffice.Sum(t => t.TransactionCount);

            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstBillAgingByTaxOffice = lstBillAgingByTaxOffice.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstBillAgingByTaxOffice.Count;
            var data = lstBillAgingByTaxOffice.Skip(IntSkip).Take(IntPageSize).ToList();


            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data, FooterTotal = dcFooterTotal }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult BillAgingByTaxOfficeExportToExcel(int BillTypeID, int StageID)
        {

            IList<usp_GetBillAgingByTaxOffice_Result> lstBillAgingByTaxOffice = new BLOperationManager().BL_GetBillAgingByTaxOffice(BillTypeID, StageID);

            string[] strColumns = new string[] { "TaxOfficeName", "Amount", "TransactionCount" };
            string[] strTotalColumns = new string[] { "Amount", "TransactionCount" };
            string method = MethodBase.GetCurrentMethod().Name;


            return ExportToExcel(lstBillAgingByTaxOffice, this.RouteData, strColumns, true, strTotalColumns, method);

        }


        [HttpGet]
        public ActionResult BillAgingDetailsByTaxOffice(int? btypeId, int? stgid, int? toffID)
        {
            if (toffID.GetValueOrDefault() > 0)
            {

                ViewBag.BillTypeID = btypeId;
                ViewBag.StageID = stgid;
                ViewBag.TaxOfficeID = toffID;
                return View();
            }
            else
            {
                return RedirectToAction("BillAgingByTaxOffice");
            }
        }


        [HttpPost]
        public JsonResult BillAgingDetailsByTaxOfficeLoadData(int? BillTypeID, int? StageID, int? TaxOfficeID)
        {
            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();

            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? TrynParse.parseInt(vLength) : 0;
            int IntSkip = vStart != null ? TrynParse.parseInt(vStart) : 0;
            int IntTotalRecords = 0;


            IList<usp_GetBillAgingDetailByTaxOffice_Result> lstData = new BLOperationManager().BL_GetBillAgingDetailByTaxOffice(BillTypeID, StageID, TaxOfficeID);
            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstData = lstData.Where(t => t.TaxPayerName != null && t.TaxPayerName.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                                             t.TaxPayerRIN != null && t.TaxPayerRIN.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                                             t.TaxPayerTypeName != null && t.TaxPayerTypeName.ToLower().Trim().Contains(vFilter.ToLower().Trim())).ToList();
            }


            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstData = lstData.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstData.Count;
            var data = lstData.Skip(IntSkip).Take(IntPageSize).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult BillAgingDetailsByTaxOfficeExportToExcel(int? BillTypeID, int? StageID, int? TaxOfficeID)
        {

            IList<usp_GetBillAgingDetailByTaxOffice_Result> lstData = new BLOperationManager().BL_GetBillAgingDetailByTaxOffice(BillTypeID, StageID, TaxOfficeID);

            string[] strColumns = new string[] { "TaxPayerTypeName", "TaxPayerName", "TaxPayerTIN", "BillRef", "BillDate", "Amount", "BillStatusName", "ContactNumber", "ContactAddress" };
            string[] strTotalColumns = new string[] { "Amount" };
            string method = MethodBase.GetCurrentMethod().Name;

            return ExportToExcel(lstData, this.RouteData, strColumns, true, strTotalColumns, method);

        }

        #endregion

        #region OM011 – Manage Revenue Stream Assessments by Tax Office


        [HttpGet]
        public ActionResult RevenueStreamAssessmentsbyTaxOffice()
        {
            IList<DropDownListResult> lstRevenueStream = new BLRevenueStream().BL_GetRevenueStreamDropDownList(new Revenue_Stream() { intStatus = 1 });
            lstRevenueStream = lstRevenueStream.Where(t => t.id != 8).ToList();
            ViewBag.RevenueStreamList = new SelectList(lstRevenueStream, "id", "text");

            UI_FillYearDropDown();

            return View();
        }

        [HttpPost]
        public JsonResult RevenueStreamAssessmentsbyTaxOfficeLoadData(int TaxYear, DateTime? FromDate, DateTime? ToDate, int RevenueStreamID)
        {

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? TrynParse.parseInt(vLength) : 0;
            int IntSkip = vStart != null ? TrynParse.parseInt(vStart) : 0;
            int IntTotalRecords = 0;

            IList<usp_GetRevenueStreamBillByTaxOffice_Result> lstRevenueStreamAssessmentsbyTaxOffice = new BLOperationManager().BL_GetRevenueStreamAssessmentsbyTaxOffice(TaxYear, FromDate, ToDate, RevenueStreamID);

            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstRevenueStreamAssessmentsbyTaxOffice = lstRevenueStreamAssessmentsbyTaxOffice.Where(t =>
                t.TaxOfficeName != null && t.TaxOfficeName.Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TaxPayerCount != null && t.TaxPayerCount.GetValueOrDefault().ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.AssetCount != null && t.AssetCount.GetValueOrDefault().ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.BillAmount != null && t.BillAmount.GetValueOrDefault().ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.SettlementAmount != null && t.SettlementAmount.GetValueOrDefault().ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.PoAAmount != null && t.PoAAmount.GetValueOrDefault().ToString().Trim().Contains(vFilter.ToLower().Trim())).ToList();
            }

            IDictionary<string, object> dcFooterTotal = new Dictionary<string, object>();

            dcFooterTotal["TaxPayerCount"] = lstRevenueStreamAssessmentsbyTaxOffice.Sum(t => t.TaxPayerCount);
            dcFooterTotal["AssetCount"] = lstRevenueStreamAssessmentsbyTaxOffice.Sum(t => t.AssetCount);
            dcFooterTotal["BillAmount"] = lstRevenueStreamAssessmentsbyTaxOffice.Sum(t => t.BillAmount);
            dcFooterTotal["SettlementAmount"] = lstRevenueStreamAssessmentsbyTaxOffice.Sum(t => t.SettlementAmount);
            dcFooterTotal["PoAAmount"] = lstRevenueStreamAssessmentsbyTaxOffice.Sum(t => t.PoAAmount);

            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstRevenueStreamAssessmentsbyTaxOffice = lstRevenueStreamAssessmentsbyTaxOffice.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstRevenueStreamAssessmentsbyTaxOffice.Count;
            var data = lstRevenueStreamAssessmentsbyTaxOffice.Skip(IntSkip).Take(IntPageSize).ToList();


            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data, FooterTotal = dcFooterTotal }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult RevenueStreamAssessmentsbyTaxOfficeExportToExcel(int TaxYear, DateTime? FromDate, DateTime? ToDate, int RevenueStreamID)
        {
            IList<usp_GetRevenueStreamBillByTaxOffice_Result> lstRevenueStreamAssessmentsbyTaxOffice = new BLOperationManager().BL_GetRevenueStreamAssessmentsbyTaxOffice(TaxYear, FromDate, ToDate, RevenueStreamID);

            string[] strColumns = new string[] { "TaxOfficeName", "TaxPayerCount", "AssetCount", "BillAmount", "SettlementAmount", "PoAAmount" };
            string[] strTotalColumns = new string[] { "TaxPayerCount", "AssetCount", "BillAmount", "SettlementAmount", "PoAAmount" };
            string method = MethodBase.GetCurrentMethod().Name;

            return ExportToExcel(lstRevenueStreamAssessmentsbyTaxOffice, this.RouteData, strColumns, true, strTotalColumns, method);
        }

        [HttpGet]
        public ActionResult viewRevenue(int TaxYear, DateTime? FromDate, DateTime? ToDate, int? RevenueStreamID, int? TaxOfficeID)
        {
            var target = 1000; 
            var totalCollection = 800; 
            var differential = totalCollection - target;
            var performance = target > 0 ? (totalCollection / (float)target) * 100 : 0;

            ViewBag.RevenueStream = RevenueStreamID;
            ViewBag.AmountAssessed = target;
            ViewBag.TotalCollection = totalCollection;
            ViewBag.Differential = differential;
            ViewBag.Performance = performance;

            return View();
        }

        [HttpPost]
        //public JsonResult RevenueStreamViewLoadData( int RevenueStreamID, int AmountAssessed, int TotalCollection, int Differential, int Performance)
        //{
        //    // Get paging parameters
        //    var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
        //    var vStart = Request.Form.GetValues("start").FirstOrDefault();
        //    var vLength = Request.Form.GetValues("length").FirstOrDefault();
        //    // Get sort columns values
        //    var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
        //    var vSortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
        //    var vFilter = Request.Form.GetValues("search[value]").FirstOrDefault();
        //    int IntPageSize = !string.IsNullOrEmpty(vLength) ? int.Parse(vLength) : 0;
        //    int IntSkip = !string.IsNullOrEmpty(vStart) ? int.Parse(vStart) : 0;
        //    int IntTotalRecords = 0;

        //    IList<usp_GetRevenueStreamBillByTaxOffice_Result> lstRevenueStreamAssessmentsbyTaxOffice = new BLOperationManager().BL_GetRevenueStreamAssessmentsbyTaxOffice(TaxYear, FromDate, ToDate, RevenueStreamID);

        //    // Filtering/Search
        //    if (!string.IsNullOrEmpty(vFilter))
        //    {
        //        lstRevenueStreamAssessmentsbyTaxOffice = lstRevenueStreamAssessmentsbyTaxOffice.Where(t =>
        //            t.BillAmount != null && t.BillAmount.GetValueOrDefault().ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
        //            t.SettlementAmount != null && t.SettlementAmount.GetValueOrDefault().ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
        //            t.PoAAmount != null && t.PoAAmount.GetValueOrDefault().ToString().Trim().Contains(vFilter.ToLower().Trim())).ToList();
        //    }

        //    // Calculate footer totals
        //    IDictionary<string, object> dcFooterTotal = new Dictionary<string, object>
        //    {
        //        ["BillAmount"] = lstRevenueStreamAssessmentsbyTaxOffice.Sum(t => t.BillAmount),
        //        ["SettlementAmount"] = lstRevenueStreamAssessmentsbyTaxOffice.Sum(t => t.SettlementAmount),
        //        ["PoAAmount"] = lstRevenueStreamAssessmentsbyTaxOffice.Sum(t => t.PoAAmount)
        //    };

        //    // Sorting
        //    if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
        //    {
        //        lstRevenueStreamAssessmentsbyTaxOffice = lstRevenueStreamAssessmentsbyTaxOffice.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
        //    }

        //    IntTotalRecords = lstRevenueStreamAssessmentsbyTaxOffice.Count;
        //    var data = lstRevenueStreamAssessmentsbyTaxOffice.Skip(IntSkip).Take(IntPageSize).ToList();

        //    return Json(new
        //    {
        //        draw = vDraw,
        //        recordsFiltered = IntTotalRecords,
        //        recordsTotal = IntTotalRecords,
        //        data = data,
        //        FooterTotal = dcFooterTotal
        //    }, JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult RevenueStreamViewLoadData(RevenueValues revenueValues, int RevenueStreamID, int AmountAssessed, int TotalCollection, int Differential, int Performance)
        //{
        //    IList<DropDownListResult> lstRevenueStream = new BLRevenueStream().BL_GetRevenueStreamDropDownList(new Revenue_Stream() { intStatus = 1 });
        //    lstRevenueStream = lstRevenueStream.Where(t => t.id != 8).ToList();
        //    ViewBag.RevenueStreamList = new SelectList(lstRevenueStream, "id", "text");

        //    // Fetch data based on parameters
        //    var query = from item in _db.Revenue_Stream
        //                where (RevenueStreamID == 0 || item.RevenueStreamID == RevenueStreamID) &&
        //                      (AmountAssessed == 0 || item.AmountAssessed == AmountAssessed) &&
        //                      (TotalCollection == 0 || item.TotalCollection == TotalCollection) &&
        //                      (Differential == 0 || item.Differential == Differential) &&
        //                      (Performance == 0 || item.Performance == Performance)
        //                select new
        //                {
        //                    item.RevenueStreamID,
        //                    item.AmountAssessed,
        //                    TargetAmount = item.TargetAmount,
        //                    RevenueAmount = item.RevenueAmount,
        //                    Differential = item.RevenueAmount - item.TargetAmount,
        //                    Performance = item.TargetAmount > 0 ? (item.RevenueAmount / (float)item.TargetAmount) * 100 : 0
        //                };

        //    // Apply sorting
        //    if (revenueValues.Order.Count > 0)
        //    {
        //        var sortColumn = revenueValues.Columns[revenueValues.Order[0].Column].Data;
        //        var sortDirection = revenueValues.Order[0].Dir;
        //        query = sortDirection == "asc" ? query.OrderBy(sortColumn) : query.OrderByDescending(sortColumn);
        //    }

        //    var totalRecords = query.Count();
        //    var data = query
        //        .Skip(revenueValues.Start)
        //        .Take(revenueValues.Length)
        //        .ToList();

        //    var result = new
        //    {
        //        draw = revenueValues.Draw,
        //        recordsTotal = totalRecords,
        //        recordsFiltered = totalRecords,
        //        data = data
        //    };

        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}



        [HttpGet]
        public ActionResult RevenueStreamBillDetailByTaxOffice(int TaxYear, DateTime? FromDate, DateTime? ToDate, int? RevenueStreamID, int? TaxOfficeID)
        {
            ViewBag.RevenueStream = RevenueStreamID;
            ViewBag.TaxOffice = TaxOfficeID;
            ViewBag.TaxYear = TaxYear;
            ViewBag.FromDate = FromDate;
            ViewBag.ToDate = ToDate;

            return View();
        }

        [HttpPost]
        public JsonResult RevenueStreamBillDetailByTaxOfficeLoadData(int? RevenueStreamID, int? TaxOfficeID, int TaxYear, DateTime? FromDate, DateTime? ToDate)
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? TrynParse.parseInt(vLength) : 0;
            int IntSkip = vStart != null ? TrynParse.parseInt(vStart) : 0;
            int IntTotalRecords = 0;


            IList<usp_GetRevenueStreamBillDetailByTaxOffice_Result> lstRevenueStreamBillDetailByTaxOffice = new BLOperationManager().BL_GetRevenueStreamBillDetailByTaxOffice(TaxYear, FromDate, ToDate, RevenueStreamID, TaxOfficeID);
            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstRevenueStreamBillDetailByTaxOffice = lstRevenueStreamBillDetailByTaxOffice.Where(t =>
                t.TaxPayerName != null && t.TaxPayerName.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.AssetRIN != null && t.AssetRIN.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.AssetTypeName != null && t.AssetTypeName.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.AssetName != null && t.AssetName.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TaxPayerRINNumber != null && t.TaxPayerRINNumber.ToLower().Trim().Contains(vFilter.ToLower().Trim())
                ).ToList();
            }


            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstRevenueStreamBillDetailByTaxOffice = lstRevenueStreamBillDetailByTaxOffice.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstRevenueStreamBillDetailByTaxOffice.Count;
            var data = lstRevenueStreamBillDetailByTaxOffice.Skip(IntSkip).Take(IntPageSize).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);

        }


        [HttpGet]
        public ActionResult RevenueStreamBillDetailByTaxOfficeExportToExcel(int? RevenueStreamID, int? TaxOfficeID, int TaxYear, DateTime? FromDate, DateTime? ToDate)
        {
            IList<usp_GetRevenueStreamBillDetailByTaxOffice_Result> lstRevenueStreamBillDetailByTaxOfficeData = new BLOperationManager().BL_GetRevenueStreamBillDetailByTaxOffice(TaxYear, FromDate, ToDate, RevenueStreamID, TaxOfficeID);

            string[] strColumns = new string[] { "TaxPayerTypeName", "TaxPayerName", "TaxPayerRINNumber", "ContactAddress", "AssetTypeName", "AssetName", "AssetRIN", "AssessmentYear", "BillAmount", "SettlementAmount", "OutstandingAmount" };
            string method = MethodBase.GetCurrentMethod().Name;

            return ExportToExcel(lstRevenueStreamBillDetailByTaxOfficeData, this.RouteData, strColumns, method);

        }

        #endregion

        #region OM012 – Manage Paye Revenue Stream Assessments by Tax Office


        [HttpGet]
        public ActionResult PayeRevenueStreamAssessmentsbyTaxOffice()
        {
            UI_FillYearDropDown();
            return View();
        }

        [HttpPost]
        public JsonResult PayeRevenueStreamAssessmentsbyTaxOfficeLoadData(int TaxYear, DateTime? FromDate, DateTime? ToDate)
        {

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();

            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? TrynParse.parseInt(vLength) : 0;
            int IntSkip = vStart != null ? TrynParse.parseInt(vStart) : 0;
            int IntTotalRecords = 0;

            IList<usp_GetPAYERevenueStreamBillByTaxOffice_Result> lstPayeRevenueStreamAssessmentsbyTaxOffice = new BLOperationManager().BL_GetPayeRevenueStreamAssessmentsbyTaxOffice(TaxYear, FromDate, ToDate);

            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstPayeRevenueStreamAssessmentsbyTaxOffice = lstPayeRevenueStreamAssessmentsbyTaxOffice.Where(t =>
                t.TaxOfficeName != null && t.TaxOfficeName.Trim().Contains(vFilter.ToLower().Trim()) ||
                t.EmployeeCount != null && t.EmployeeCount.GetValueOrDefault().ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.EmployerCount != null && t.EmployerCount.GetValueOrDefault().ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.BusinessCount != null && t.BusinessCount.GetValueOrDefault().ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.BillAmount != null && t.BillAmount.GetValueOrDefault().ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.SettlementAmount != null && t.SettlementAmount.GetValueOrDefault().ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.PoAAmount != null && t.PoAAmount.GetValueOrDefault().ToString().Trim().Contains(vFilter.ToLower().Trim())).ToList();
            }

            IDictionary<string, object> dcFooterTotal = new Dictionary<string, object>();

            dcFooterTotal["EmployeeCount"] = lstPayeRevenueStreamAssessmentsbyTaxOffice.Sum(t => t.EmployeeCount);
            dcFooterTotal["EmployerCount"] = lstPayeRevenueStreamAssessmentsbyTaxOffice.Sum(t => t.EmployerCount);
            dcFooterTotal["BusinessCount"] = lstPayeRevenueStreamAssessmentsbyTaxOffice.Sum(t => t.BusinessCount);
            dcFooterTotal["BillAmount"] = lstPayeRevenueStreamAssessmentsbyTaxOffice.Sum(t => t.BillAmount);
            dcFooterTotal["SettlementAmount"] = lstPayeRevenueStreamAssessmentsbyTaxOffice.Sum(t => t.SettlementAmount);
            dcFooterTotal["PoAAmount"] = lstPayeRevenueStreamAssessmentsbyTaxOffice.Sum(t => t.PoAAmount);

            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstPayeRevenueStreamAssessmentsbyTaxOffice = lstPayeRevenueStreamAssessmentsbyTaxOffice.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstPayeRevenueStreamAssessmentsbyTaxOffice.Count;
            var data = lstPayeRevenueStreamAssessmentsbyTaxOffice.Skip(IntSkip).Take(IntPageSize).ToList();


            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data, FooterTotal = dcFooterTotal }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PayeRevenueStreamAssessmentsbyTaxOfficeExportToExcel(int TaxYear, DateTime? FromDate, DateTime? ToDate)
        {
            IList<usp_GetPAYERevenueStreamBillByTaxOffice_Result> lstPayeRevenueStreamAssessmentsbyTaxOffice = new BLOperationManager().BL_GetPayeRevenueStreamAssessmentsbyTaxOffice(TaxYear, FromDate, ToDate);

            string[] strColumns = new string[] { "TaxOfficeName", "EmployerCount", "EmployeeCount", "BusinessCount", "BillAmount", "SettlementAmount", "PoAAmount" };
            string[] strTotalColumns = new string[] { "EmployerCount", "EmployeeCount", "BusinessCount", "BillAmount", "SettlementAmount", "PoAAmount" };
            string method = MethodBase.GetCurrentMethod().Name;

            return ExportToExcel(lstPayeRevenueStreamAssessmentsbyTaxOffice, this.RouteData, strColumns, true, strTotalColumns, method);
        }


        [HttpGet]
        public ActionResult PayeRevenueStreamBillDetailByTaxOffice(int TaxYear, DateTime? FromDate, DateTime? ToDate, int? TaxOfficeID)
        {

            ViewBag.TaxYear = TaxYear;
            ViewBag.TaxOffice = TaxOfficeID;
            ViewBag.FromDate = FromDate;
            ViewBag.ToDate = ToDate;

            return View();
        }

        [HttpPost]
        public JsonResult PayeRevenueStreamBillDetailByTaxOfficeLoadData(int? TaxOfficeID, int TaxYear, DateTime? FromDate, DateTime? ToDate)
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? TrynParse.parseInt(vLength) : 0;
            int IntSkip = vStart != null ? TrynParse.parseInt(vStart) : 0;
            int IntTotalRecords = 0;

            IList<usp_GetPAYERevenueStreamBillDetailByTaxOffice_Result> lstPayeRevenueStreamBillDetailByTaxOffice;
            //ObjectCache cache = MemoryCache.Default;

            //if(cache.Contains("PayeRevenueStreamBillDetailByTaxOffice") && TrynParse.parseInt(vDraw) > 1)
            //{
            //    lstPayeRevenueStreamBillDetailByTaxOffice = (IList<usp_GetPAYERevenueStreamBillDetailByTaxOffice_Result>)cache.Get("PayeRevenueStreamBillDetailByTaxOffice");
            //}
            //else
            //{
            lstPayeRevenueStreamBillDetailByTaxOffice = new BLOperationManager().BL_GetPayeRevenueStreamBillDetailbyTaxOffice(TaxYear, FromDate, ToDate, TaxOfficeID);
            //CacheItemPolicy cacheItemPolicy = new CacheItemPolicy
            //{
            //    AbsoluteExpiration = DateTime.Now.AddHours(1.0)
            //};

            //cache.Add("PayeRevenueStreamBillDetailByTaxOffice", lstPayeRevenueStreamBillDetailByTaxOffice, cacheItemPolicy);
            //}


            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstPayeRevenueStreamBillDetailByTaxOffice = lstPayeRevenueStreamBillDetailByTaxOffice.Where(t =>
                t.TaxPayerName != null && t.TaxPayerName.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                 t.TaxPayerTypeName != null && t.TaxPayerTypeName.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                 t.AssetRIN != null && t.AssetRIN.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                 t.TaxPayerTIN != null && t.TaxPayerTIN.ToLower().Trim().Contains(vFilter.ToLower().Trim())).ToList();
            }


            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstPayeRevenueStreamBillDetailByTaxOffice = lstPayeRevenueStreamBillDetailByTaxOffice.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstPayeRevenueStreamBillDetailByTaxOffice.Count;
            var data = lstPayeRevenueStreamBillDetailByTaxOffice.Skip(IntSkip).Take(IntPageSize).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);

        }


        [HttpGet]
        public ActionResult PayeRevenueStreamBillDetailByTaxOfficeExportToExcel(int TaxYear, int? TaxOfficeID, DateTime? FromDate, DateTime? ToDate)
        {
            IList<usp_GetPAYERevenueStreamBillDetailByTaxOffice_Result> lstPayeRevenueStreamBillDetailByTaxOffice = new BLOperationManager().BL_GetPayeRevenueStreamBillDetailbyTaxOffice(TaxYear, FromDate, ToDate, TaxOfficeID);

            string[] strColumns = new string[] { "TaxPayerTypeName", "TaxPayerName", "TaxPayerRINNumber", "TaxPayerTIN", "AssetTypeName", "AssetName", "AssetRIN", "AssessmentYear", "BillAmount", "SettlementAmount", "OutstandingAmount" };
            string method = MethodBase.GetCurrentMethod().Name;

            return ExportToExcel(lstPayeRevenueStreamBillDetailByTaxOffice, this.RouteData, strColumns, method);
        }

        #endregion

        #region OM013 – Manage Taxpayer Liability Status


        [HttpGet]
        public ActionResult TaxPayerLiabilityStatus()
        {
            UI_FillYearDropDown();
            return View();
        }

        [HttpPost]
        public ActionResult TaxPayerLiabilityStatusSearchData(FormCollection pObjFormCollection)
        {
            string mIntTaxPayerID = pObjFormCollection.Get("txtTaxPayerRIN");
            int mIntTaxYear = TrynParse.parseInt(pObjFormCollection.Get("cboTaxYear"));
            DateTime? FromDate = TrynParse.parseNullableDate(pObjFormCollection.Get("txtFromDate"));
            DateTime? ToDate = TrynParse.parseNullableDate(pObjFormCollection.Get("txtToDate"));

            IList<usp_RPT_TaxPayerLiabilityStatus_Result> lstTaxPayerLiabilityStatus = new BLOperationManager().BL_GetTaxPayerLiabilityStatus(mIntTaxPayerID, mIntTaxYear, FromDate, ToDate);

            if (lstTaxPayerLiabilityStatus != null)
            {
                ViewBag.TaxYear = mIntTaxYear;
                ViewBag.FromDate = pObjFormCollection.Get("txtFromDate");
                ViewBag.ToDate = pObjFormCollection.Get("txtToDate");
            }

            return PartialView("_TaxPayerLiabilityStatusBindTable", lstTaxPayerLiabilityStatus);
        }



        [HttpGet]
        public ActionResult TaxPayerLiabilityStatusAssessment(int? TaxPayerID, int? TaxPayerTypeID, int TaxYear, DateTime? FromDate, DateTime? ToDate)
        {
            ViewBag.TaxPayer = TaxPayerID;
            ViewBag.TaxPayerType = TaxPayerTypeID;
            ViewBag.TaxYear = TaxYear;
            ViewBag.FromDate = FromDate;
            ViewBag.ToDate = ToDate;

            return View();
        }



        [HttpPost]
        public JsonResult TaxPayerLiabilityStatusAssessmentLoadData(int? TaxPayerID, int? TaxPayerTypeID, int TaxYear, DateTime? FromDate, DateTime? ToDate)
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();

            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? TrynParse.parseInt(vLength) : 0;
            int IntSkip = vStart != null ? TrynParse.parseInt(vStart) : 0;
            int IntTotalRecords = 0;


            IList<usp_RPT_TaxPayerLiabilityStatus_Bills_Result> lstTaxPayerLiabilityStatus = new BLOperationManager().BL_GetTaxPayerLiabilityStatusDetails(TaxPayerID, TaxPayerTypeID, TaxYear, FromDate, ToDate);

            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstTaxPayerLiabilityStatus = lstTaxPayerLiabilityStatus.Where(t =>
                t.TaxPayerName != null && t.TaxPayerName.ToLower().Trim().Contains(vFilter.ToLower().Trim())).ToList();
            }


            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstTaxPayerLiabilityStatus = lstTaxPayerLiabilityStatus.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstTaxPayerLiabilityStatus.Count;
            var data = lstTaxPayerLiabilityStatus.Skip(IntSkip).Take(IntPageSize).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);

        }


        [HttpGet]
        public ActionResult TaxPayerLiabilityStatusAssessmentExportToExcel(int? TaxPayerID, int? TaxPayerTypeID, int TaxYear, DateTime? FromDate, DateTime? ToDate)
        {
            IList<usp_RPT_TaxPayerLiabilityStatus_Bills_Result> lstTaxPayerLiabilityStatusAssessmentData = new BLOperationManager().BL_GetTaxPayerLiabilityStatusDetails(TaxPayerID, TaxPayerTypeID, TaxYear, FromDate, ToDate);
            string[] strColumns = new string[] { "TaxPayerName", "BillRefNo", "BillDate", "BillAmount", "SettlementStatusName" };
            string method = MethodBase.GetCurrentMethod().Name;

            return ExportToExcel(lstTaxPayerLiabilityStatusAssessmentData, this.RouteData, strColumns, method);
        }




        [HttpGet]
        public ActionResult TaxPayerLiabilityStatusPayment(int? TaxPayerID, int? TaxPayerTypeID, int TaxYear, DateTime? FromDate, DateTime? ToDate)
        {
            ViewBag.TaxPayer = TaxPayerID;
            ViewBag.TaxPayerType = TaxPayerTypeID;
            ViewBag.TaxYear = TaxYear;
            ViewBag.FromDate = FromDate;
            ViewBag.ToDate = ToDate;

            return View();
        }


        [HttpPost]
        public JsonResult TaxPayerLiabilityStatusPaymentLoadData(int? TaxPayerID, int? TaxPayerTypeID, int TaxYear, DateTime? FromDate, DateTime? ToDate)
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? TrynParse.parseInt(vLength) : 0;
            int IntSkip = vStart != null ? TrynParse.parseInt(vStart) : 0;
            int IntTotalRecords = 0;

            IList<usp_RPT_TaxPayerLiabilityStatus_Payment_Result> lstTaxPayerLiabilityStatusPayment = new BLOperationManager().BL_GetTaxPayerLiabilityStatusPaymentDetails(TaxPayerID, TaxPayerTypeID, TaxYear, FromDate, ToDate);
            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstTaxPayerLiabilityStatusPayment = lstTaxPayerLiabilityStatusPayment.Where(t =>
                t.TaxPayerName != null && t.TaxPayerName.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.PaymentRefNo != null && t.PaymentRefNo.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.PaymentDate != null && t.PaymentDate.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.Amount != null && t.Amount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim())

                ).ToList();
            }


            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstTaxPayerLiabilityStatusPayment = lstTaxPayerLiabilityStatusPayment.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstTaxPayerLiabilityStatusPayment.Count;
            var data = lstTaxPayerLiabilityStatusPayment.Skip(IntSkip).Take(IntPageSize).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);

        }


        [HttpGet]
        public ActionResult TaxPayerLiabilityStatusPaymentExportToExcel(int? TaxPayerID, int? TaxPayerTypeID, int TaxYear, DateTime? FromDate, DateTime? ToDate)
        {
            IList<usp_RPT_TaxPayerLiabilityStatus_Payment_Result> lstTaxPayerLiabilityStatusPaymentData = new BLOperationManager().BL_GetTaxPayerLiabilityStatusPaymentDetails(TaxPayerID, TaxPayerTypeID, TaxYear, FromDate, ToDate);
            string[] strColumns = new string[] { "TaxPayerName", "PaymentRefNo", "PaymentDate", "Amount" };
            string method = MethodBase.GetCurrentMethod().Name;

            return ExportToExcel(lstTaxPayerLiabilityStatusPaymentData, this.RouteData, strColumns, method);
        }



        #endregion

        #region OM014 – Unallocated Taxpayers


        [HttpGet]
        public ActionResult UnAllocatedTaxPayerList()
        {
            UI_FillTaxOfficeDropDown();
            UI_FillTaxPayerTypeDropDown();
            return View();
        }


        [HttpPost]
        public JsonResult UnAllocatedTaxPayerLoadData(int? TaxOfficeID, int? TaxPayerTypeID)
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? TrynParse.parseInt(vLength) : 0;
            int IntSkip = vStart != null ? TrynParse.parseInt(vStart) : 0;
            int IntTotalRecords = 0;



            IList<usp_RPT_GetUnallocatedTaxPayerList_Result> lstTaxPayer = new BLOperationManager().BL_GetUnAllocatedTaxPayerList(TaxOfficeID.GetValueOrDefault(), TaxPayerTypeID.GetValueOrDefault());
            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstTaxPayer = lstTaxPayer.Where(t =>
                t.TaxPayerName != null && t.TaxPayerName.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TaxPayerRIN != null && t.TaxPayerRIN.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TotalAssessmentAmount != null && t.TotalAssessmentAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim())

                ).ToList();
            }


            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstTaxPayer = lstTaxPayer.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstTaxPayer.Count();
            var data = lstTaxPayer.Skip(IntSkip).Take(IntPageSize).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public FileResult UnAllocatedTaxPayerExportToExcel(int TaxOfficeID, int TaxPayerTypeID)
        {
            IList<usp_RPT_GetUnallocatedTaxPayerList_Result> lstTaxPayer = new BLOperationManager().BL_GetUnAllocatedTaxPayerList(TaxOfficeID, TaxPayerTypeID);
            string[] strColumns = new string[] { "TaxPayerRIN", "TaxPayerName", "TotalAssessmentAmount", "TotalPaymentAmount" };
            string method = MethodBase.GetCurrentMethod().Name;


            return ExportToExcel(lstTaxPayer, this.RouteData, strColumns, method);

        }

        #endregion

        #region OM015 - Allocate Tax Payers to Tax Officers


        [HttpGet]
        public ActionResult AllocateTaxPayerList()
        {
            UI_FillTaxOfficeDropDown();
            UI_FillTaxPayerTypeDropDown();
            return View();
        }


        [HttpPost]
        public JsonResult AllocateTaxPayerLoadData(int? TaxOfficeID, int? TaxOfficerID)
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? Convert.ToInt32(vLength) : 0;
            int IntSkip = vStart != null ? Convert.ToInt32(vStart) : 0;
            int IntTotalRecords = 0;


            IList<usp_RPT_GetAllocatedTaxPayerList_Result> lstTaxPayer = new BLOperationManager().BL_GetAllocatedTaxPayerList(TaxOfficeID.GetValueOrDefault(), TaxOfficerID.GetValueOrDefault());
            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstTaxPayer = lstTaxPayer.Where(t =>
                t.TaxPayerName != null && t.TaxPayerName.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TaxPayerRIN != null && t.TaxPayerRIN.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TotalAssessmentAmount != null && t.TotalAssessmentAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim())

                ).ToList();
            }


            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstTaxPayer = lstTaxPayer.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstTaxPayer.Count();
            var data = lstTaxPayer.Skip(IntSkip).Take(IntPageSize).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult AllocateTaxPayerToTaxOfficer(int? tptid, int? toid, int? tofid)
        {
            if (tptid.GetValueOrDefault() > 0 && tofid.GetValueOrDefault() > 0 && toid.GetValueOrDefault() > 0)
            {
                AllocateTaxPayerViewModel mObjAllocateTaxPayerModel = new AllocateTaxPayerViewModel()
                {
                    TaxPayerTypeID = tptid.GetValueOrDefault(),
                    TaxOfficeID = toid.GetValueOrDefault(),
                    TaxOfficerID = tofid.GetValueOrDefault(),
                };

                return View(mObjAllocateTaxPayerModel);
            }
            else
            {
                return RedirectToAction("AllocateTaxPayerList", "OperationManager");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]

        public ActionResult AllocateTaxPayerToTaxOfficer(AllocateTaxPayerViewModel pObjAllocateTPModel)
        {
            if (ModelState.IsValid)
            {
                //Update Tax Payer as per tax payer type and officer
                string[] strTaxPayerIds = pObjAllocateTPModel.TaxPayerIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                Individual mObjIndividual; BLIndividual mObjBLIndividual = new BLIndividual();
                Company mObjCompany; BLCompany mObjBLCompany = new BLCompany();
                Government mObjGovernment; BLGovernment mObjBLGovernment = new BLGovernment();
                Special mObjSpecial; BLSpecial mObjBLSpecial = new BLSpecial();
                foreach (string strTaxPayerID in strTaxPayerIds)
                {
                    if (pObjAllocateTPModel.TaxPayerTypeID == (int)EnumList.TaxPayerType.Individual)
                    {
                        mObjIndividual = new Individual()
                        {
                            IndividualID = TrynParse.parseInt(strTaxPayerID),
                            TaxOfficerID = pObjAllocateTPModel.TaxOfficerID
                        };

                        mObjBLIndividual.BL_UpdateTaxOfficer(mObjIndividual);
                    }
                    else if (pObjAllocateTPModel.TaxPayerTypeID == (int)EnumList.TaxPayerType.Companies)
                    {
                        mObjCompany = new Company()
                        {
                            CompanyID = TrynParse.parseInt(strTaxPayerID),
                            TaxOfficerID = pObjAllocateTPModel.TaxOfficerID
                        };

                        mObjBLCompany.BL_UpdateTaxOfficer(mObjCompany);
                    }
                    else if (pObjAllocateTPModel.TaxPayerTypeID == (int)EnumList.TaxPayerType.Government)
                    {
                        mObjGovernment = new Government()
                        {
                            GovernmentID = TrynParse.parseInt(strTaxPayerID),
                            TaxOfficerID = pObjAllocateTPModel.TaxOfficerID
                        };

                        mObjBLGovernment.BL_UpdateTaxOfficer(mObjGovernment);
                    }
                    else if (pObjAllocateTPModel.TaxPayerTypeID == (int)EnumList.TaxPayerType.Special)
                    {
                        mObjSpecial = new Special()
                        {
                            SpecialID = TrynParse.parseInt(strTaxPayerID),
                            TaxOfficerID = pObjAllocateTPModel.TaxOfficerID
                        };

                        mObjBLSpecial.BL_UpdateTaxOfficer(mObjSpecial);
                    }
                }

                return RedirectToAction("AllocateTaxPayerList", "OperationManager");
            }
            else
            {
                return View(pObjAllocateTPModel);
            }
        }

        #endregion

        #region OM016 – Taxpayer Status Manager


        [HttpGet]
        public ActionResult TaxPayerStatusManager()
        {
            IList<usp_RPT_GetTaxPayerStatusManager_Result> lstTaxPayerStatusManager = new BLOperationManager().BL_GetTaxPayerStatusManagerList();
            return View(lstTaxPayerStatusManager);
        }

        [HttpGet]
        public FileResult TaxPayerStatusManagerExportToExcel()
        {
            IList<usp_RPT_GetTaxPayerStatusManager_Result> lstTaxPayerStatusManager = new BLOperationManager().BL_GetTaxPayerStatusManagerList();

            string[] strColumns = new string[] { "TaxOfficeName", "TaxPayerCount" };
            string[] strTotalColumns = new string[] { "TaxPayerCount" };
            string method = MethodBase.GetCurrentMethod().Name;

            return ExportToExcel(lstTaxPayerStatusManager, this.RouteData, strColumns, true, strTotalColumns, method);
        }


        [HttpGet]
        public ActionResult TaxPayerStatusManagerTaxPayer(int? toid)
        {
            if (toid.GetValueOrDefault() > 0)
            {
                usp_GetTaxOfficeList_Result mObjTaxOfficeData = new BLTaxOffice().BL_GetTaxOfficeDetails(new Tax_Offices() { TaxOfficeID = toid.GetValueOrDefault(), intStatus = 2 });

                if (mObjTaxOfficeData != null)
                {
                    return View(mObjTaxOfficeData);
                }
                else
                {
                    return RedirectToAction("TaxPayerStatusManager", "OperationManager");
                }
            }
            else
            {
                return RedirectToAction("TaxPayerStatusManager", "OperationManager");
            }
        }

        [HttpPost]
        public JsonResult TaxPayerStatusManagerTaxPayerLoadData(int? TaxOfficeID)
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? Convert.ToInt32(vLength) : 0;
            int IntSkip = vStart != null ? Convert.ToInt32(vStart) : 0;
            int IntTotalRecords = 0;


            IList<usp_RPT_GetTaxPayerStatusManager_TaxPayerList_Result> lstTaxPayer = new BLOperationManager().BL_GetTaxPayerStatusManager_TaxPayerList(TaxOfficeID.GetValueOrDefault());

            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstTaxPayer = lstTaxPayer.Where(t =>
                t.TaxPayerName != null && t.TaxPayerName.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TaxPayerRIN != null && t.TaxPayerRIN.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TotalAssessmentAmount != null && t.TotalAssessmentAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TotalPaymentAmount != null && t.TotalPaymentAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.OutstandingAmount != null && t.OutstandingAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.ReviewStatusName != null && t.ReviewStatusName.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TaxOfficerName != null && t.TaxOfficerName.ToLower().Trim().Contains(vFilter.ToLower().Trim())).ToList();
            }


            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstTaxPayer = lstTaxPayer.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstTaxPayer.Count;
            var data = lstTaxPayer.Skip(IntSkip).Take(IntPageSize).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public FileResult TaxPayerStatusManagerTaxPayerExportToExcel(int? TaxOfficeID)
        {
            IList<usp_RPT_GetTaxPayerStatusManager_TaxPayerList_Result> lstTaxPayer = new BLOperationManager().BL_GetTaxPayerStatusManager_TaxPayerList(TaxOfficeID.GetValueOrDefault());

            string[] strColumns = new string[] { "TaxPayerTypeName", "TaxPayerRIN", "TaxPayerName", "TaxOfficerName", "ReviewStatusName", "TotalAssessmentAmount", "TotalPaymentAmount", "OutstandingAmount" };
            string[] strTotalColumns = new string[] { "TotalAssessmentAmount", "TotalPaymentAmount", "OutstandingAmount" };
            string method = MethodBase.GetCurrentMethod().Name;

            return ExportToExcel(lstTaxPayer, this.RouteData, strColumns, true, strTotalColumns, method);
        }



        [HttpGet]
        public ActionResult TaxPayerReview(int? tpid, int? tptid)
        {
            if (tpid.GetValueOrDefault() > 0 && tptid.GetValueOrDefault() > 0)
            {
                UI_FillReviewStatus();

                IList<usp_GetTaxPayerReviewNotes_Result> lstReivewNotes = new BLReview().BL_GetReviewNotes(new MAP_TaxPayer_Review() { TaxPayerID = tpid, TaxPayerTypeID = tptid });
                ViewBag.ReviewNotes = lstReivewNotes;

                ReviewViewModel mObjReviewViewModel;
                if (tptid == (int)EnumList.TaxPayerType.Individual)
                {
                    usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(new Individual() { intStatus = 2, IndividualID = tpid.GetValueOrDefault() });

                    if (mObjIndividualData != null)
                    {
                        mObjReviewViewModel = new ReviewViewModel()
                        {
                            TaxPayerID = mObjIndividualData.IndividualID.GetValueOrDefault(),
                            TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                            TaxPayerTypeName = mObjIndividualData.TaxPayerTypeName,
                            TaxPayerRIN = mObjIndividualData.IndividualRIN,
                            TaxPayerTIN = mObjIndividualData.TIN,
                            TaxPayerName = mObjIndividualData.FirstName + " " + mObjIndividualData.LastName,
                            ContactNumber = mObjIndividualData.MobileNumber1,
                            ContactAddress = mObjIndividualData.ContactAddress,
                        };

                        return View(mObjReviewViewModel);
                    }
                    else
                    {
                        return RedirectToAction("TaxPayerStatusManager", "OperationManager");
                    }
                }
                else if (tptid == (int)EnumList.TaxPayerType.Companies)
                {
                    usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(new Company() { intStatus = 2, CompanyID = tpid.GetValueOrDefault() });

                    if (mObjCompanyData != null)
                    {
                        mObjReviewViewModel = new ReviewViewModel()
                        {
                            TaxPayerID = mObjCompanyData.CompanyID.GetValueOrDefault(),
                            TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies,
                            TaxPayerTypeName = mObjCompanyData.TaxPayerTypeName,
                            TaxPayerRIN = mObjCompanyData.CompanyRIN,
                            TaxPayerTIN = mObjCompanyData.TIN,
                            TaxPayerName = mObjCompanyData.CompanyName,
                            ContactNumber = mObjCompanyData.MobileNumber1,
                            ContactAddress = mObjCompanyData.ContactAddress,
                        };
                        return View(mObjReviewViewModel);
                    }
                    else
                    {
                        return RedirectToAction("TaxPayerStatusManager", "OperationManager");
                    }
                }
                else if (tptid == (int)EnumList.TaxPayerType.Government)
                {
                    usp_GetGovernmentList_Result mObjGovernmentData = new BLGovernment().BL_GetGovernmentDetails(new Government() { intStatus = 2, GovernmentID = tpid.GetValueOrDefault() });
                    if (mObjGovernmentData != null)
                    {
                        mObjReviewViewModel = new ReviewViewModel()
                        {
                            TaxPayerID = mObjGovernmentData.GovernmentID.GetValueOrDefault(),
                            TaxPayerTypeID = (int)EnumList.TaxPayerType.Government,
                            TaxPayerTypeName = mObjGovernmentData.TaxPayerTypeName,
                            TaxPayerRIN = mObjGovernmentData.GovernmentRIN,
                            TaxPayerTIN = mObjGovernmentData.TIN,
                            TaxPayerName = mObjGovernmentData.GovernmentName,
                            ContactNumber = mObjGovernmentData.ContactNumber,
                            ContactAddress = mObjGovernmentData.ContactAddress,
                        };
                        return View(mObjReviewViewModel);
                    }
                    else
                    {
                        return RedirectToAction("TaxPayerStatusManager", "OperationManager");
                    }
                }
                else if (tptid == (int)EnumList.TaxPayerType.Special)
                {
                    usp_GetSpecialList_Result mObjSpecialData = new BLSpecial().BL_GetSpecialDetails(new Special() { intStatus = 2, SpecialID = tpid.GetValueOrDefault() });
                    if (mObjSpecialData != null)
                    {
                        mObjReviewViewModel = new ReviewViewModel()
                        {
                            TaxPayerID = mObjSpecialData.SpecialID.GetValueOrDefault(),
                            TaxPayerTypeID = (int)EnumList.TaxPayerType.Special,
                            TaxPayerTypeName = mObjSpecialData.TaxPayerTypeName,
                            TaxPayerRIN = mObjSpecialData.SpecialRIN,
                            TaxPayerTIN = mObjSpecialData.TIN,
                            TaxPayerName = mObjSpecialData.SpecialTaxPayerName,
                            ContactNumber = mObjSpecialData.ContactNumber,
                        };

                        return View(mObjReviewViewModel);
                    }
                    else
                    {
                        return RedirectToAction("TaxPayerStatusManager", "OperationManager");
                    }
                }
                else
                {
                    return RedirectToAction("TaxPayerStatusManager", "OperationManager");
                }
            }
            else
            {
                return RedirectToAction("TaxPayerStatusManager", "OperationManager");
            }
        }


        #endregion

        #region OM017 – Tax Officer Status Report


        public ActionResult TaxOfficerStatus()
        {
            UI_FillTaxOfficeDropDown();
            UI_FillTaxPayerTypeDropDown();
            UI_FillReviewStatus();
            return View();
        }

        [HttpPost]
        public JsonResult TaxOfficerStatusLoadData(int TaxOfficeID, int TaxPayerTypeID, int ReviewStatusID, int TaxOfficerID)
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? Convert.ToInt32(vLength) : 0;
            int IntSkip = vStart != null ? Convert.ToInt32(vStart) : 0;
            int IntTotalRecords = 0;


            IList<usp_RPT_TaxOfficerStatus_Result> lstTaxPayer = new BLOperationManager().BL_GetTaxOfficerStatus(TaxOfficeID, TaxPayerTypeID, ReviewStatusID, TaxOfficerID);
            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstTaxPayer = lstTaxPayer.Where(t =>
                t.TaxPayerName != null && t.TaxPayerName.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TaxPayerRIN != null && t.TaxPayerRIN.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TotalAssessmentAmount != null && t.TotalAssessmentAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TotalPaymentAmount != null && t.TotalPaymentAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.OutstandingAmount != null && t.OutstandingAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.ReviewStatusName != null && t.ReviewStatusName.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TaxOfficerName != null && t.TaxOfficerName.ToLower().Trim().Contains(vFilter.ToLower().Trim())).ToList();
            }


            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstTaxPayer = lstTaxPayer.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstTaxPayer.Count();
            var data = lstTaxPayer.Skip(IntSkip).Take(IntPageSize).ToList();

            decimal dcAssessmentTotal = lstTaxPayer.Sum(t => t.TotalAssessmentAmount.GetValueOrDefault());
            decimal dcPaymentTotal = lstTaxPayer.Sum(t => t.TotalPaymentAmount.GetValueOrDefault());
            decimal dcOutstandingTotal = lstTaxPayer.Sum(t => t.OutstandingAmount.GetValueOrDefault());

            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data, AssessmentTotal = dcAssessmentTotal, PaymentTotal = dcPaymentTotal, OutstandingTotal = dcOutstandingTotal }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult TaxOfficerStatusExportToExcel(int TaxOfficeID, int TaxPayerTypeID, int ReviewStatusID, int TaxOfficerID)
        {
            IList<usp_RPT_TaxOfficerStatus_Result> lstTaxPayer = new BLOperationManager().BL_GetTaxOfficerStatus(TaxOfficeID, TaxPayerTypeID, ReviewStatusID, TaxOfficerID);
            string[] strColumns = new string[] { "TaxPayerTypeName", "TaxPayerName", "TaxPayerRIN", "ReviewStatusName", "TaxOfficerName", "TotalAssessmentAmount", "TotalPaymentAmount", "OutstandingAmount" };
            string[] strTotalColumns = new string[] { "TotalAssessmentAmount", "TotalPaymentAmount", "OutstandingAmount" };
            string method = MethodBase.GetCurrentMethod().Name;
            return ExportToExcel(lstTaxPayer, this.RouteData, strColumns, true, strTotalColumns, method);
        }

        #endregion

        #region OM018 – Tax Officer Manager Status Report

        #endregion

        #region OM019 – Tax Officer Summary Report

        #endregion

        #region OM020 – Tax Officer Manager Summary Report

        #endregion

        #region OM021 – Review Status Summary Report

        #endregion

        #region OM022 – Tax Officer Monthly Payments

        #endregion

        #region OM024 – Monthly Tax Office Targets

        #endregion

        #region OM025 – Tax Office by Revenue Stream Target

        public ActionResult TaxOfficeByRevenueStreamTarget()
        {
            UI_FillTaxOfficeDropDown();
            UI_FillYearDropDown();
            UI_FillMonthDropDown();
            return View();
        }

        public JsonResult TaxOfficeByRevenueStreamTargetLoadData(int TaxOfficeID, int Year, int Month)
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? Convert.ToInt32(vLength) : 0;
            int IntSkip = vStart != null ? Convert.ToInt32(vStart) : 0;
            int IntTotalRecords = 0;


            IList<usp_RPT_TaxOfficeByRevenueStreamTarget_Result> lstSummary = new BLOperationManager().BL_GetTaxOfficeByRevenueStreamTarget(TaxOfficeID, Year, Month);
            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstSummary = lstSummary.Where(t =>
                t.TargetAmount != null && t.TargetAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.AssessedAmount != null && t.AssessedAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.RevenueAmount != null && t.RevenueAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.RevenueStreamName != null && t.RevenueStreamName.ToLower().Trim().Contains(vFilter.ToLower().Trim())).ToList();
            }


            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstSummary = lstSummary.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstSummary.Count();
            var data = lstSummary.Skip(IntSkip).Take(IntPageSize).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region OM026 – Revenue Stream by Tax Office Target


        public ActionResult RevenueStreamByTaxOfficeTarget()
        {
            UI_FillRevenueStreamDropDown();
            UI_FillYearDropDown();
            UI_FillMonthDropDown();
            UI_FillTaxOfficeDropDown();
            return View();
        }
        public ActionResult NewTaxOfficeTarget()
        {
            UI_FillYearDropDown();
            UI_FillMonthDropDown();
            UI_FillTaxOfficeDropDown();
            return View();
        }
        //usp_RPT_TaxOffice_Performance_ByAllRevenueStreamResult

        public ActionResult NewTaxOfficeTargetByMonth()
        {
            UI_FillYearDropDown();
            UI_FillMonthDropDown();
            return View();
        }
        public ActionResult NewTaxOfficeTargetDrillDrown(int? Year, int? Month, int? taxofficeId, int? revenueStreamID)
        {
            string yearr = "0";
            string monthh = "0";
            string taxoffId = "0";
            string revenueStreamId = "0";

            var url = Request.Url.AbsoluteUri.ToLower();

            Uri uri = new Uri(url);
            string[] segments = uri.AbsolutePath.Split('/');
            if (segments.Length > 5)
            // if (segments.Length > 4)
            // if (segments.Length > 6)
            {
                yearr = segments[3];
                monthh = segments[4];
                taxoffId = segments[5];
                revenueStreamId = segments[6];
            }

            int nwYear = Convert.ToInt32(yearr);
            int nwMonth = Convert.ToInt32(monthh);
            int nwtaxoffId = Convert.ToInt32(taxoffId);
            int nwRevenueStreamId = Convert.ToInt32(revenueStreamId);

            ViewBag.Year = nwYear;
            ViewBag.Month = nwMonth;
            ViewBag.Month = nwtaxoffId;
            ViewBag.RevenueStreamID = nwRevenueStreamId;

            var res = NewTaxOfficeTargetDrillDrownCall(nwYear, nwMonth, nwtaxoffId, nwRevenueStreamId);
            return View(res);
        }
        public JsonResult NewTaxOfficeTargetLoadData(int? Year, int? Month, int? taxofficeId)
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? Convert.ToInt32(vLength) : 0;
            int IntSkip = vStart != null ? Convert.ToInt32(vStart) : 0;
            int IntTotalRecords = 0;

            var lstSummary = _appDbContext.usp_RPT_TaxOffice_Performance_ByAllRevenueStream(Year.GetValueOrDefault(), Month.GetValueOrDefault(), taxofficeId.GetValueOrDefault()).ToList();

            if (!string.IsNullOrEmpty(vFilter))
            {
                lstSummary = lstSummary.Where(t =>
                      (t.RevenueStreamName != null && t.RevenueStreamName.ToString().Trim().Contains(vFilter.ToLower().Trim())) ||
                      (t.Targetamount != null && t.Targetamount.ToString().Trim().Contains(vFilter.ToLower().Trim())) ||
                      (t.Settlementamount != null && t.Settlementamount.ToString().Trim().Contains(vFilter.ToLower().Trim())) ||
                      (t.differenitial != null && t.differenitial.ToString().Trim().Contains(vFilter.ToLower().Trim())) ||
                      (t.Perc != null && t.Perc.ToString().ToLower().Trim().Contains(vFilter.ToLower().Trim()))
                  ).ToList();
            }


            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstSummary = lstSummary.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            double Percentage = (double)lstSummary.First().Perc;

            IntTotalRecords = lstSummary.Count();
            //var data = lstSummary.Skip(IntSkip).Take(IntPageSize).ToList();
            var data = lstSummary.Skip(IntSkip).Take(IntPageSize)
             .Select(s => new
             {
                 s.RevenueStreamName,
                 s.Targetamount,
                 s.Settlementamount,
                 s.differenitial,
                 s.RevenueStreamID,
                 Percentage,
                 year = Year,
                 month = Month,
                 taxofficeId = taxofficeId
             }).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult RevenueStreamByTaxOfficeTargetLoadData(int? RevenueStreamID, int? Year, int? Month, int? taxofficeId)
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? Convert.ToInt32(vLength) : 0;
            int IntSkip = vStart != null ? Convert.ToInt32(vStart) : 0;
            int IntTotalRecords = 0;


            var lstSummary = _appDbContext.usp_RPT_RevenueStreamForAllPurpose(RevenueStreamID.GetValueOrDefault(), Year.GetValueOrDefault(), Month.GetValueOrDefault(), taxofficeId.GetValueOrDefault()).ToList();
            // IList<usp_RPT_RevenueStreamByTaxOfficeTarget_Result> lstSummary = new BLOperationManager().BL_GetRevenueStreamByTaxOfficeTarget(RevenueStreamID, Year, Month, taxofficeId);
            //Filtering/Searching data 

            if (!string.IsNullOrEmpty(vFilter))
            {
                lstSummary = lstSummary.Where(t =>
                      (t.TargetAmount != null && t.TargetAmount.ToString().Trim().Contains(vFilter.ToLower().Trim())) ||
                      (t.AssessedAmount != null && t.AssessedAmount.ToString().Trim().Contains(vFilter.ToLower().Trim())) ||
                      (t.RevenueAmount != null && t.RevenueAmount.ToString().Trim().Contains(vFilter.ToLower().Trim())) ||
                      (t.TaxOfficeName != null && t.TaxOfficeName.ToLower().Trim().Contains(vFilter.ToLower().Trim()))
                  ).ToList();
            }


            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstSummary = lstSummary.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstSummary.Count();
            //var data = lstSummary.Skip(IntSkip).Take(IntPageSize).ToList();
            var data = lstSummary.Skip(IntSkip).Take(IntPageSize)
             .Select(s => new
             {
                 s.TaxOfficeName,
                 s.RevenueStreamName,
                 s.TargetAmount,
                 s.RevenueAmount,
                 s.Differential,
                 s.Performance,
                 revenueStreamId = RevenueStreamID,
                 year = Year,
                 month = Month,
                 taxofficeId = taxofficeId
             }).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult uspRPTAllTaxOfficesPerformanceByMonthLoadData(int? Year, int? Month)
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? Convert.ToInt32(vLength) : 0;
            int IntSkip = vStart != null ? Convert.ToInt32(vStart) : 0;
            int IntTotalRecords = 0;


            var lstSummary = _appDbContext.usp_RPT_All_TaxOffices_Performance_ByMonth(Year.GetValueOrDefault(), Month.GetValueOrDefault()).ToList();



            if (!string.IsNullOrEmpty(vFilter))
            {
                lstSummary = lstSummary.Where(t =>
                      (t.Targetamount != null && t.Targetamount.ToString().Trim().Contains(vFilter.ToLower().Trim())) ||
                      (t.Settlementamount != null && t.Settlementamount.ToString().Trim().Contains(vFilter.ToLower().Trim())) ||
                      (t.differenitial != null && t.differenitial.ToString().Trim().Contains(vFilter.ToLower().Trim())) ||
                      (t.Perc != null && t.Perc.ToString().Trim().Contains(vFilter.ToLower().Trim()))
                  ).ToList();
            }

            var totalTargetamount = lstSummary.Sum(t => t.Targetamount); 
            var totalSettlementAmount = lstSummary.Sum(t => t.Settlementamount); 
            var totaldifferenitial = lstSummary.Sum(t => t.differenitial); 
            double totalPercentage = (double)(totalTargetamount != 0 ? (totalSettlementAmount / totalTargetamount) * 100 : 0m);


            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstSummary = lstSummary.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstSummary.Count();
            //var data = lstSummary.Skip(IntSkip).Take(IntPageSize).ToList();
            var data = lstSummary.Skip(IntSkip).Take(IntPageSize)
                 .Select(s => new
                 {
                     monthString = GetMonthName(Month.GetValueOrDefault()),
                     totalTargetamount,
                     totalSettlementAmount,
                     totaldifferenitial,
                     totalPercentage,
                     year = Year,
                     month = Month,
                 }).Distinct().ToList();

            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult uspRPTAllTaxOfficesPerformanceByMonthLoadDataDetails(int? Year, int? Month)
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? Convert.ToInt32(vLength) : 0;
            int IntSkip = vStart != null ? Convert.ToInt32(vStart) : 0;
            int IntTotalRecords = 0;


            var lstSummary = _appDbContext.usp_RPT_All_TaxOffices_Performance_ByMonth(Year.GetValueOrDefault(), Month.GetValueOrDefault()).ToList();


            if (!string.IsNullOrEmpty(vFilter))
            {
                lstSummary = lstSummary.Where(t =>
                      (t.Targetamount != null && t.Targetamount.ToString().Trim().Contains(vFilter.ToLower().Trim())) ||
                      (t.Settlementamount != null && t.Settlementamount.ToString().Trim().Contains(vFilter.ToLower().Trim())) ||
                      (t.differenitial != null && t.differenitial.ToString().Trim().Contains(vFilter.ToLower().Trim())) ||
                      (t.Perc != null && t.Perc.ToString().Trim().Contains(vFilter.ToLower().Trim()))
                  ).ToList();
            }


            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstSummary = lstSummary.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstSummary.Count();
            //var data = lstSummary.Skip(IntSkip).Take(IntPageSize).ToList();
            var data = lstSummary.Skip(IntSkip).Take(IntPageSize)
             .Select(s => new
             {
                 monthString = GetMonthName(Month.GetValueOrDefault()),
                 s.Targetamount,
                 s.Settlementamount,
                 s.differenitial,
                 s.Perc,
                 year = Year,
                 month = Month,
             }).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RevenueStreamByTaxOfficeTargetII()
        {
            UI_FillRevenueStreamDropDown();
            UI_FillYearDropDown();
            UI_FillMonthDropDown();
            return View();
        }
        public ActionResult RevenueStreamByTaxOfficeTargetDetails(int? RevenueStreamID, int? Year, int? Month, int? taxofficeId)
        {
            ViewBag.RevenueStreamID = RevenueStreamID;
            ViewBag.Year = Year;
            ViewBag.Month = Month;
            ViewBag.TaxOfficeID = taxofficeId;

            var res = TaxOfficePerformancebyRevenueStreamdrilldown(RevenueStreamID, Year, Month, taxofficeId);
            return View(res);
        }

        [NonAction]
        private List<RevenueStreamResultDrillDown> TaxOfficePerformancebyRevenueStreamdrilldown(int? RevenueStreamID, int? Year, int? Month, int? taxofficeId)
        {
            var lstDril = _appDbContext.usp_RPT_TaxOffice_Performance_byRevenueStreamdrilldown(RevenueStreamID.GetValueOrDefault(), Year.GetValueOrDefault(), Month.GetValueOrDefault(), taxofficeId.GetValueOrDefault()).ToList();

            List<RevenueStreamResultDrillDown> lstRet = new List<RevenueStreamResultDrillDown>();
            foreach (var item in lstDril)
            {
                RevenueStreamResultDrillDown ret = new RevenueStreamResultDrillDown
                {
                    //TaxOfficeName = item.TaxOfficeName ?? "N/A",
                    TaxpayerName = item.TaxpayerName ?? "N/A",//TaxpayerName
                    AssessmentRefNo = item.AssessmentRefNo ?? "N/A",
                    settlementamount = item.settlementamount ?? 0m // or a default value
                };

                lstRet.Add(ret);
            }

            return lstRet;
        }
        private string GetMonthName(int monthNumber)
        {
            var monthNames = new[]
            {
        "January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"
    };
            return monthNumber >= 1 && monthNumber <= 12 ? monthNames[monthNumber - 1] : "Unknown";
        }

        public ActionResult TaxOfficeTargetByMonthDetails(int? Year, int? Month)
        {
            string yearr = "0"; 
            string monthh = "0";

            var url = Request.Url.AbsoluteUri.ToLower();
            //var routeData = Request.RequestContext.RouteData.Values;
            Uri uri = new Uri(url);
            string[] segments = uri.AbsolutePath.Split('/');
            if (segments.Length > 4)
            {
                yearr = segments[3];
                monthh = segments[4];
            }

            int nwYear = Convert.ToInt32(yearr);
            int nwMonth = Convert.ToInt32(monthh);

            ViewBag.Year = nwYear;
            ViewBag.Month = nwMonth;

            var res = TaxOfficeTargetByMonthDetailsCall(nwYear, nwMonth);
            return View(res);
        }
        [NonAction]
        private List<usp_RPT_All_TaxOffices_Performance_ByMonth_Result> TaxOfficeTargetByMonthDetailsCall(int? Year, int? Month)
        {
            var lstSummary = _appDbContext.usp_RPT_All_TaxOffices_Performance_ByMonth(Year.GetValueOrDefault(), Month.GetValueOrDefault()).ToList();
            List<usp_RPT_All_TaxOffices_Performance_ByMonth_Result> lstRet = new List<usp_RPT_All_TaxOffices_Performance_ByMonth_Result>();
            foreach (var item in lstSummary)
            {
                var targetAmount = item.Targetamount ?? 0m;
                var settlementAmount = item.Settlementamount ?? 0m;
                usp_RPT_All_TaxOffices_Performance_ByMonth_Result ret = new usp_RPT_All_TaxOffices_Performance_ByMonth_Result
                {
                    TaxOfficeName = item.TaxOfficeName ?? "N/A",
                    Targetamount = item.Targetamount ?? 0m,
                    Settlementamount = item.Settlementamount ?? 0m,
                    differenitial = item.differenitial ?? 0m,
                    Perc = (settlementAmount / targetAmount) * 100
                };

                lstRet.Add(ret);
            }

            return lstRet;
        }
        private List<usp_RPT_TaxOffice_Performance_ByAllRevenueStreamdrilldown> NewTaxOfficeTargetDrillDrownCall(int? Year, int? Month, int? TaxOfficeId, int? RevenueStreamID)
        {
            var lstSummary = _appDbContext.usp_RPT_TaxOffice_Performance_ByAllRevenueStreamdrilldown(Year.GetValueOrDefault(), Month.GetValueOrDefault(), TaxOfficeId.GetValueOrDefault(), RevenueStreamID.GetValueOrDefault()).ToList();
            List<usp_RPT_TaxOffice_Performance_ByAllRevenueStreamdrilldown> lstRet = new List<usp_RPT_TaxOffice_Performance_ByAllRevenueStreamdrilldown>();
            foreach (var item in lstSummary)
            {
                usp_RPT_TaxOffice_Performance_ByAllRevenueStreamdrilldown ret = new usp_RPT_TaxOffice_Performance_ByAllRevenueStreamdrilldown
                {
                    TaxpayerRIN = item.TaxpayerRIN ?? "N/A",
                    TaxpayerTypeID = item.TaxpayerTypeID ?? 0,
                    TaxpayerName = item.TaxpayerName ?? "N/A",
                    settlementamount = item.settlementamount ?? 0m,
                };

                lstRet.Add(ret);
            }

            return lstRet;
        }
        public ActionResult TaxOfficeTargetByMonthDetailsDrill(int Year, int Month, int? TaxOfficeID)
        {
            ViewBag.TaxOfficeID = TaxOfficeID;
            ViewBag.Year = Year;
            ViewBag.Month = Month;

            //var res = TaxOfficeTargetByMonthDetailsCallDrillDown(Year, Month, TaxOfficeID);
            return View();
        }

        [NonAction]
        //private List<usp_RPT_TaxOffice_Performance_ByAllRevenueStreamdrilldown> TaxOfficeTargetByMonthDetailsCallDrillDown(int Year, int Month, int? TaxOfficeID)
        //{

        //    var lstSummary = _appDbContext.usp_RPT_TaxOffice_Performance_ByAllRevenueStreamdrilldown(Year, Month, TaxOfficeID.GetValueOrDefault()).ToList();
        //    List<usp_RPT_TaxOffice_Performance_ByAllRevenueStreamdrilldown> lstRet = new List<usp_RPT_TaxOffice_Performance_ByAllRevenueStreamdrilldown>();
        //    foreach (var item in lstSummary)
        //    {
        //        usp_RPT_TaxOffice_Performance_ByAllRevenueStreamdrilldown ret = new usp_RPT_TaxOffice_Performance_ByAllRevenueStreamdrilldown
        //        {
        //            RevenueStreamName = item.TaxOfficeName ?? "N/A",
        //            Targetamount = item.Targetamount ?? 0m,
        //            Settlementamount = item.Settlementamount ?? 0m,
        //            differenitial = item.differenitial ?? 0m,
        //            Perc = item.Perc ?? 0m
        //        };

        //        lstRet.Add(ret);
        //    }

        //    return lstRet;
        //}

        //public JsonResult RevenueStreamByTaxOfficeTargetLoadDataII(int RevenueStreamID, int Year, int Month)
        //{
        //    //Get parameters

        //    // get Start (paging start index) and length (page size for paging)
        //    var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
        //    var vStart = Request.Form.GetValues("start").FirstOrDefault();
        //    var vLength = Request.Form.GetValues("length").FirstOrDefault();
        //    //Get Sort columns value
        //    var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
        //    var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
        //    var vFilter = Request.Form.GetValues("search[value]")[0];
        //    int IntPageSize = vLength != null ? Convert.ToInt32(vLength) : 0;
        //    int IntSkip = vStart != null ? Convert.ToInt32(vStart) : 0;
        //    int IntTotalRecords = 0;


        //    IList<usp_RPT_RevenueStreamByTaxOfficeTarget_Result> lstSummary = new BLOperationManager().BL_GetRevenueStreamByTaxOfficeTarget(RevenueStreamID, Year, Month);
        //    //Filtering/Searching data 
        //    if (!string.IsNullOrEmpty(vFilter))
        //    {
        //        lstSummary = lstSummary.Where(t =>
        //        t.TargetAmount != null && t.TargetAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
        //        t.AssessedAmount != null && t.AssessedAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
        //        t.RevenueAmount != null && t.RevenueAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
        //        t.TaxOfficeName != null && t.TaxOfficeName.ToLower().Trim().Contains(vFilter.ToLower().Trim())).ToList();
        //    }


        //    //Purpose Sorting Data 
        //    if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
        //    {
        //        lstSummary = lstSummary.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
        //    }

        //    IntTotalRecords = lstSummary.Count();
        //    var data = lstSummary.Skip(IntSkip).Take(IntPageSize).ToList();
        //    return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);
        //}

        #endregion

        #region OM028 – Revenue Streams by Payment Channels


        public ActionResult RevenueStreamByPaymentChannel()
        {
            UI_FillSettlementMethodDropDown();
            UI_FillYearDropDown();
            return View();
        }


        public JsonResult RevenueStreamByPaymentChannelLoadData(int SettlementMethodID, int Year, DateTime? FromDate, DateTime? ToDate)
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? Convert.ToInt32(vLength) : 0;
            int IntSkip = vStart != null ? Convert.ToInt32(vStart) : 0;
            int IntTotalRecords = 0;


            IList<usp_RPT_GetRevenueStreamByPaymentChannel_Result> lstSummary = new BLOperationManager().BL_GetRevenueStreamByPaymentChannel(SettlementMethodID, Year, FromDate, ToDate);
            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstSummary = lstSummary.Where(t =>
                t.RevenueStreamName != null && t.RevenueStreamName.Trim().Contains(vFilter.ToLower().Trim()) ||
                t.SettlementAmount != null && t.SettlementAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TotalAmount != null && t.TotalAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.PoAAmount != null && t.PoAAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim())).ToList();
            }

            IDictionary<string, object> dcFooterTotal = new Dictionary<string, object>();

            dcFooterTotal["PoAAmount"] = lstSummary.Sum(t => t.PoAAmount);
            dcFooterTotal["SettlementAmount"] = lstSummary.Sum(t => t.SettlementAmount);
            dcFooterTotal["TotalAmount"] = lstSummary.Sum(t => t.TotalAmount);

            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstSummary = lstSummary.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstSummary.Count();
            var data = lstSummary.Skip(IntSkip).Take(IntPageSize).ToList();


            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data, FooterTotal = dcFooterTotal }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult RevenueStreamByPaymentChannelExportToExcel(int SettlementMethodID, int Year, DateTime? FromDate, DateTime? ToDate)
        {
            IList<usp_RPT_GetRevenueStreamByPaymentChannel_Result> lstSummary = new BLOperationManager().BL_GetRevenueStreamByPaymentChannel(SettlementMethodID, Year, FromDate, ToDate);

            string method = MethodBase.GetCurrentMethod().Name;

            byte[] ObjExcelData = CommUtil.ToExcel(lstSummary, $"{method}");
            return File(ObjExcelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{method}" + DateTime.Now.ToString("dd_MM_yy") + ".xlsx");
        }

        public ActionResult RevenueStreamByPaymentChannelDetail(int smthId, int tyear, int rstrmID, DateTime? fdate, DateTime? tdate)
        {
            ViewBag.SettlementMethodID = smthId;
            ViewBag.TaxYear = tyear;
            ViewBag.RevenueStreamID = rstrmID;
            ViewBag.FromDate = fdate;
            ViewBag.ToDate = tdate;
            return View();
        }

        public JsonResult RevenueStreamByPaymentChannelDetailLoadData(int SettlementMethodID, int TaxYear, int RevenueStreamID, DateTime? FromDate, DateTime? ToDate)
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? Convert.ToInt32(vLength) : 0;
            int IntSkip = vStart != null ? Convert.ToInt32(vStart) : 0;
            int IntTotalRecords = 0;


            IList<usp_RPT_GetPaymentDetail_Result> lstPaymentDetails = new BLOperationManager().BL_GetPaymentChannelDetail(SettlementMethodID, RevenueStreamID, TaxYear, FromDate, ToDate);
            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstPaymentDetails = lstPaymentDetails.Where(t =>
                t.TaxPayerName != null && t.TaxPayerName.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.RevenueStreamName != null && t.RevenueStreamName.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.PaymentChannelName != null && t.PaymentChannelName.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.Amount != null && t.Amount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.PaymentDate != null && t.PaymentDate.Value.ToString("dd-MMM-yyyy").Trim().Contains(vFilter.ToLower().Trim())).ToList();
            }


            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstPaymentDetails = lstPaymentDetails.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstPaymentDetails.Count();
            var data = lstPaymentDetails.Skip(IntSkip).Take(IntPageSize).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RevenueStreamByPaymentChannelDetailExportToExcel(int SettlementMethodID, int TaxYear, int RevenueStreamID, DateTime? Fromdate, DateTime? Todate)
        {

            IList<usp_RPT_GetPaymentDetail_Result> lstPaymentDetails = new BLOperationManager().BL_GetPaymentChannelDetail(SettlementMethodID, RevenueStreamID, TaxYear, Fromdate, Todate);

            string method = MethodBase.GetCurrentMethod().Name;

            byte[] ObjExcelData = CommUtil.ToExcel(lstPaymentDetails, $"{method}");
            return File(ObjExcelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{method}" + DateTime.Now.ToString("dd_MM_yy") + ".xlsx");
        }

        #endregion

        #region OM029 – Payment Channel by Revenue Stream


        public ActionResult PaymentChannelByRevenueStream()
        {
            UI_FillRevenueStreamDropDown();
            UI_FillYearDropDown();
            return View();
        }


        public JsonResult PaymentChannelByRevenueStreamLoadData(int RevenueStreamID, int Year, DateTime? FromDate, DateTime? ToDate)
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? Convert.ToInt32(vLength) : 0;
            int IntSkip = vStart != null ? Convert.ToInt32(vStart) : 0;
            int IntTotalRecords = 0;


            IList<usp_RPT_GetPaymentChannelByRevenueStream_Result> lstSummary = new BLOperationManager().BL_GetPaymentChannelByRevenueStream(RevenueStreamID, Year, FromDate, ToDate);
            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstSummary = lstSummary.Where(t =>
                t.SettlementMethodName != null && t.SettlementMethodName.Trim().Contains(vFilter.ToLower().Trim()) ||
                t.SettlementAmount != null && t.SettlementAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TotalAmount != null && t.TotalAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.PoAAmount != null && t.PoAAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim())).ToList();
            }

            IDictionary<string, object> dcFooterTotal = new Dictionary<string, object>();

            dcFooterTotal["PoAAmount"] = lstSummary.Sum(t => t.PoAAmount);
            dcFooterTotal["SettlementAmount"] = lstSummary.Sum(t => t.SettlementAmount);
            dcFooterTotal["TotalAmount"] = lstSummary.Sum(t => t.TotalAmount);


            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstSummary = lstSummary.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstSummary.Count();
            var data = lstSummary.Skip(IntSkip).Take(IntPageSize).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data, FooterTotal = dcFooterTotal }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult PaymentChannelByRevenueStreamExportToExcel(int RevenueStreamID, int Year, DateTime? FromDate, DateTime? ToDate)
        {
            string path = "";

            string mStrDirectory = GlobalDefaultValues.DocumentLocation + "TaxLiabilityReport/";
            string mStrGeneratedFileName = mStrDirectory + "excel" + ".xlsx";
            string mStrGeneratedDocumentPath = Path.Combine(mStrDirectory, mStrGeneratedFileName);

            if (!Directory.Exists(mStrDirectory))
            {
                Directory.CreateDirectory(mStrDirectory);
            }

            IList<usp_RPT_GetPaymentChannelByRevenueStream_Result> lstSummary = new BLOperationManager().BL_GetPaymentChannelByRevenueStream(RevenueStreamID, Year, FromDate, ToDate);

            var gv = new GridView();
            gv.DataSource = lstSummary;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=PaymentChannelByRevenueStreamExportToExcel.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            gv.RenderControl(objHtmlTextWriter);
            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
            return RedirectToAction("PaymentByRevenueStream", "OperationManager");

        }

        public ActionResult PaymentChannelByRevenueStreamDetail(int smthId, int tyear, int rstrmID, DateTime? fdate, DateTime? tdate)
        {
            ViewBag.SettlementMethodID = smthId;
            ViewBag.TaxYear = tyear;
            ViewBag.RevenueStreamID = rstrmID;
            ViewBag.FromDate = fdate;
            ViewBag.ToDate = tdate;
            return View();
        }

        public JsonResult PaymentChannelByRevenueStreamDetailLoadData(int SettlementMethodID, int TaxYear, int RevenueStreamID, DateTime? FromDate, DateTime? ToDate)
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? Convert.ToInt32(vLength) : 0;
            int IntSkip = vStart != null ? Convert.ToInt32(vStart) : 0;
            int IntTotalRecords = 0;


            IList<usp_RPT_GetPaymentDetail_Result> lstPaymentDetails = new BLOperationManager().BL_GetPaymentChannelDetail(SettlementMethodID, RevenueStreamID, TaxYear, FromDate, ToDate);
            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstPaymentDetails = lstPaymentDetails.Where(t =>
                t.TaxPayerName != null && t.TaxPayerName.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.RevenueStreamName != null && t.RevenueStreamName.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.PaymentChannelName != null && t.PaymentChannelName.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.Amount != null && t.Amount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.PaymentDate != null && t.PaymentDate.Value.ToString("dd-MMM-yyyy").Trim().Contains(vFilter.ToLower().Trim())).ToList();
            }


            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstPaymentDetails = lstPaymentDetails.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstPaymentDetails.Count();
            var data = lstPaymentDetails.Skip(IntSkip).Take(IntPageSize).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PaymentChannelByRevenueStreamDetailExportToExcel(int SettlementMethodID, int TaxYear, int RevenueStreamID, DateTime? FromDate, DateTime? ToDate)
        {

            IList<usp_RPT_GetPaymentDetail_Result> lstPaymentDetails = new BLOperationManager().BL_GetPaymentChannelDetail(SettlementMethodID, RevenueStreamID, TaxYear, FromDate, ToDate);

            string[] strColumns = new string[] { "TaxPayerName", "TaxPayerRIN", "TaxPayerTIN", "TaxPayerTypeName", "TaxOfficeName", "RevenueStreamName", "PaymentChannelName", "PaymentDate", "Amount" };
            string[] strTotalColumns = new string[] { "Amount" };
            var vMemberInfoData = typeof(usp_RPT_GetPaymentDetail_Result)
                    .GetProperties()
                    .Where(pi => strColumns.Contains(pi.Name))
                    .Select(pi => (MemberInfo)pi)
                    .ToArray();
            string method = MethodBase.GetCurrentMethod().Name;

            byte[] ObjExcelData = CommUtil.ToExcel(lstPaymentDetails, $"{method}");
            return File(ObjExcelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{method}" + DateTime.Now.ToString("dd_MM_yy") + ".xlsx");
        }

        #endregion

        #region OM030 – Tax Officer by Revenue Stream Target


        public ActionResult TaxOfficerByRevenueStreamTarget()
        {
            UI_FillTaxOfficeDropDown();
            UI_FillYearDropDown();
            UI_FillMonthDropDown();
            return View();
        }


        public JsonResult TaxOfficerByRevenueStreamTargetLoadData(int TaxOfficerID, int Year, int Month)
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? Convert.ToInt32(vLength) : 0;
            int IntSkip = vStart != null ? Convert.ToInt32(vStart) : 0;
            int IntTotalRecords = 0;


            IList<usp_RPT_TaxOfficerByRevenueStreamTarget_Result> lstSummary = new BLOperationManager().BL_GetTaxOfficerByRevenueStreamTarget(TaxOfficerID, Year, Month);
            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstSummary = lstSummary.Where(t =>
                t.TargetAmount != null && t.TargetAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.AssessedAmount != null && t.AssessedAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.RevenueAmount != null && t.RevenueAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.RevenueStreamName != null && t.RevenueStreamName.ToLower().Trim().Contains(vFilter.ToLower().Trim())).ToList();
            }


            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstSummary = lstSummary.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstSummary.Count();
            var data = lstSummary.Skip(IntSkip).Take(IntPageSize).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult TaxOfficerByRevenueStreamTargetExportToExcel(int TaxOfficerID, int Year, int Month)
        {
            string strTableName = "Tax Officer by Revenue Stream Target";

            IList<usp_RPT_TaxOfficerByRevenueStreamTarget_Result> lstSummary = new BLOperationManager().BL_GetTaxOfficerByRevenueStreamTarget(TaxOfficerID, Year, Month);
            DataTable dt = CommUtil.ConvertToDataTable(lstSummary);

            var ObjExcelData = CommUtil.ConvertDataTableToExcel(dt);
            return File(ObjExcelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "TaxOfficerByRevenueStreamTarget_" + DateTime.Now.ToString("dd_MM_yy") + ".xlsx");

        }

        #endregion

        #region OM031 – Revenue Stream by Tax Officer Target


        public ActionResult RevenueStreamByTaxOfficerTarget()
        {
            UI_FillRevenueStreamDropDown();
            UI_FillTaxOfficeDropDown(null, true);
            UI_FillYearDropDown();
            UI_FillMonthDropDown();
            return View();
        }


        public JsonResult RevenueStreamByTaxOfficerTargetLoadData(int TaxOfficeID, int RevenueStreamID, int Year, int Month)
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? Convert.ToInt32(vLength) : 0;
            int IntSkip = vStart != null ? Convert.ToInt32(vStart) : 0;
            int IntTotalRecords = 0;


            IList<usp_RPT_RevenueStreamByTaxOfficerTarget_Result> lstSummary = new BLOperationManager().BL_GetRevenueStreamByTaxOfficerTarget(TaxOfficeID, RevenueStreamID, Year, Month);
            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstSummary = lstSummary.Where(t =>
                t.TargetAmount != null && t.TargetAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.AssessedAmount != null && t.AssessedAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.RevenueAmount != null && t.RevenueAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TaxOfficerName != null && t.TaxOfficerName.ToLower().Trim().Contains(vFilter.ToLower().Trim())).ToList();
            }


            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstSummary = lstSummary.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstSummary.Count();
            var data = lstSummary.Skip(IntSkip).Take(IntPageSize).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult RevenueStreamByTaxOfficerTargetExportToExcel(int TaxOfficeID, int RevenueStreamID, int Year, int Month)
        {
            string strTableName = "Revenue Stream by Tax Officer Target";

            IList<usp_RPT_RevenueStreamByTaxOfficerTarget_Result> lstSummary = new BLOperationManager().BL_GetRevenueStreamByTaxOfficerTarget(TaxOfficeID, RevenueStreamID, Year, Month);

            DataTable dt = CommUtil.ConvertToDataTable(lstSummary);

            var ObjExcelData = CommUtil.ConvertDataTableToExcel(dt);
            return File(ObjExcelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "RevenueStreamByTaxOfficerTarget_" + DateTime.Now.ToString("dd_MM_yy") + ".xlsx");

        }


        public ActionResult RevenueStreamByTaxOfficerTargetDetailExportToExcel(int TaxOfficerID, int RevenueStreamID, int Year, int Month)
        {
            IList<usp_RPT_GetRevenueStreamByTaxOfficerTargetDetail_Result> lstSummary = new BLOperationManager().BL_GetRevenueStreamByTaxOfficerTargetDetail(TaxOfficerID, RevenueStreamID, Year, Month);


            string method = MethodBase.GetCurrentMethod().Name;

            byte[] ObjExcelData = CommUtil.ToExcel(lstSummary, $"{method}");
            return File(ObjExcelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{method}" + DateTime.Now.ToString("dd_MM_yy") + ".xlsx");
        }

        #endregion

        #region OM033 – Income Tax Liability Report

        #endregion

        #region OM039 – Taxpayer Types by Tax Office

        #endregion

        #region OM040 – Asset Types by Tax Office

        #endregion

        #region OM043 – Tax Office Assessment Summary

        #endregion

        #region OM044 – Business Sector Assessment Summary

        //
        [HttpGet]
        public ActionResult BusinessSectorAssessmentSummary()
        {
            UI_FillYearDropDown();
            UI_FillBusinessTypeDropDown();
            return View();
        }

        [HttpPost]
        public JsonResult BusinessSectorAssessmentSummaryLoadData(int TaxYear, int BusinessTypeID, int BusinessCategoryID)
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();

            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? TrynParse.parseInt(vLength) : 0;
            int IntSkip = vStart != null ? TrynParse.parseInt(vStart) : 0;
            int IntTotalRecords = 0;


            IList<usp_RPT_BusinessSectorAssessmentSummary_Result> lstSummary = new BLOperationManager().BL_GetBusinessSectorAssessmentSummary(TaxYear, BusinessTypeID, BusinessCategoryID);
            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstSummary = lstSummary.Where(t =>
                t.BusinessCategoryName != null && t.BusinessCategoryName.Trim().Contains(vFilter.ToLower().Trim()) ||
                t.BusinessSectorName != null && t.BusinessSectorName.Trim().Contains(vFilter.ToLower().Trim()) ||
                t.BillAmount != null && t.BillAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.SettlementAmount != null && t.SettlementAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.PoAAmount != null && t.PoAAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.OutstandingAmount != null && t.OutstandingAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim())).ToList();
            }

            IDictionary<string, object> dcFooterTotal = new Dictionary<string, object>();

            dcFooterTotal["BillAmount"] = lstSummary.Sum(t => t.BillAmount);
            dcFooterTotal["SettlementAmount"] = lstSummary.Sum(t => t.SettlementAmount);
            dcFooterTotal["PoAAmount"] = lstSummary.Sum(t => t.PoAAmount);
            dcFooterTotal["OutstandingAmount"] = lstSummary.Sum(t => t.OutstandingAmount);

            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstSummary = lstSummary.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstSummary.Count;
            var data = lstSummary.Skip(IntSkip).Take(IntPageSize).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data, FooterTotal = dcFooterTotal }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult BusinessSectorAssessmentSummaryExportToExcel(int TaxYear, int BusinessTypeID, int BusinessCategoryID)
        {
            IList<usp_RPT_BusinessSectorAssessmentSummary_Result> lstSummary = new BLOperationManager().BL_GetBusinessSectorAssessmentSummary(TaxYear, BusinessTypeID, BusinessCategoryID);

            string[] strColumns = new string[] { "BusinessCategoryName", "BusinessSectorName", "BillAmount", "SettlementAmount", "PoAAmount", "OutstandingAmount" };
            string[] strTotalColumns = new string[] { "BillAmount", "SettlementAmount", "PoAAmount", "OutstandingAmount" };
            string method = MethodBase.GetCurrentMethod().Name;

            return ExportToExcel(lstSummary, this.RouteData, strColumns, true, strTotalColumns, method);

        }

        //
        [HttpGet]
        public ActionResult BusinessSectorAssessmentDetail(int year, int bsid)
        {
            ViewBag.TaxYear = year;
            ViewBag.BusinessSectorID = bsid;
            return View();

        }


        [HttpPost]
        public ActionResult BusinessSectorAssessmentDetailLoadData(int TaxYear, int BusinessSectorID)
        {
            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();

            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? TrynParse.parseInt(vLength) : 0;
            int IntSkip = vStart != null ? TrynParse.parseInt(vStart) : 0;
            int IntTotalRecords = 0;


            IList<usp_RPT_BusinessSectorAssessmentDetail_Result> lstDetails = new BLOperationManager().BL_GetBusinessSectorAssessmentDetail(TaxYear, BusinessSectorID);
            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstDetails = lstDetails.Where(t =>
                t.TaxPayerRIN != null && t.TaxPayerRIN.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TaxPayerTypeName != null && t.TaxPayerTypeName.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TaxPayerName != null && t.TaxPayerName.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.BillAmount != null && t.BillAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.SettlementAmount != null && t.SettlementAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.PoAAmount != null && t.PoAAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.OutstandingAmount != null && t.OutstandingAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim())).ToList();
            }

            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstDetails = lstDetails.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstDetails.Count;
            var data = lstDetails.Skip(IntSkip).Take(IntPageSize).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult BusinessSectorAssessmentDetailExportToExcel(int TaxYear, int BusinessSectorID)
        {
            IList<usp_RPT_BusinessSectorAssessmentDetail_Result> lstDetails = new BLOperationManager().BL_GetBusinessSectorAssessmentDetail(TaxYear, BusinessSectorID);

            string[] strColumns = new string[] { "TaxPayerTypeName","TaxPayerRIN","TaxPayerName","TaxPayerTIN","ContactNumber","ContactAddress",
                                                 "BillAmount","SettlementAmount","PoAAmount","OutstandingAmount" };
            string[] strTotalColumns = new string[] { "BillAmount", "SettlementAmount", "PoAAmount", "OutstandingAmount" };
            string method = MethodBase.GetCurrentMethod().Name;

            return ExportToExcel(lstDetails, this.RouteData, strColumns, true, strTotalColumns, method);
        }

        #endregion

        #region OM050 - Audit Log

        [HttpGet]
        public ActionResult AuditLog()
        {
            IList<DropDownListResult> lstALScreen = new BLCommon().BL_GetALScreenDropDownList();
            ViewBag.ALScreenList = new SelectList(lstALScreen, "id", "text");

            IList<DropDownListResult> lstStaff = new BLUser().BL_GetUserDropDownList(new MST_Users() { intStatus = 2 });
            ViewBag.StaffList = new SelectList(lstStaff, "id", "text");


            return View();
        }

        public JsonResult AuditLogLoadData(int StaffID, int ASLID, DateTime? FromDate, DateTime? ToDate)
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? Convert.ToInt32(vLength) : 0;
            int IntSkip = vStart != null ? Convert.ToInt32(vStart) : 0;
            int IntTotalRecords = 0;

            IList<usp_GetAuditLog_Result> lstAuditLog = new BLAuditLog().BL_GetAuditLog(new Audit_Log()
            {
                StaffID = StaffID,
                ASLID = ASLID,
                FromDate = FromDate,
                ToDate = ToDate
            });

            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstAuditLog = lstAuditLog.Where(t =>
                t.Comment != null && t.Comment.Trim().Contains(vFilter.ToLower().Trim()) ||
                t.IPAddress != null && t.IPAddress.Trim().Contains(vFilter.ToLower().Trim()) ||
                t.LogDate != null && t.LogDate.Value.ToString("dd-MMM-yy").ToLower().Trim().Contains(vFilter.ToLower().Trim())).ToList();
            }


            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstAuditLog = lstAuditLog.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstAuditLog.Count;
            var data = lstAuditLog.Skip(IntSkip).Take(IntPageSize).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region OM051 - Settlement Revocation

        public ActionResult SettlementRevocation()
        {
            string url = getUrl();
            bool itCan = new UtilityController().CheckAccess(url);
            if (!itCan) { return RedirectToAction("AccessDenied", "Utility"); }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult SettlementRevocation(ReviseBillViewModel pObjReviseBillModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjReviseBillModel);
            }
            else
            {
                if (pObjReviseBillModel.BillRefNo.StartsWith("AB"))
                {
                    BLAssessment mObjBLAssessment = new BLAssessment();
                    usp_GetAssessmentList_Result mObjAssessmentData = mObjBLAssessment.BL_GetAssessmentDetails(new Assessment() { AssessmentRefNo = pObjReviseBillModel.BillRefNo, IntStatus = 2 });

                    if (mObjAssessmentData != null)
                    {
                        return RedirectToAction("Assessment", "SettlementRevocation", new { id = mObjAssessmentData.AssessmentID, name = mObjAssessmentData.AssessmentRefNo.ToSeoUrl() });
                    }
                    else
                    {
                        ViewBag.Message = "Invalid Bill Ref No";
                        return View(pObjReviseBillModel);
                    }

                }
                else if (pObjReviseBillModel.BillRefNo.StartsWith("SB"))
                {
                    BLServiceBill mObjBLServiceBill = new BLServiceBill();
                    usp_GetServiceBillList_Result mObjServiceBillData = mObjBLServiceBill.BL_GetServiceBillDetails(new ServiceBill() { ServiceBillRefNo = pObjReviseBillModel.BillRefNo, IntStatus = 2 });

                    if (mObjServiceBillData != null)
                    {
                        return RedirectToAction("ServiceBill", "SettlementRevocation", new { id = mObjServiceBillData.ServiceBillID, name = mObjServiceBillData.ServiceBillRefNo.ToSeoUrl() });
                    }
                    else
                    {
                        ViewBag.Message = "Invalid Bill Ref No";
                        return View(pObjReviseBillModel);
                    }
                }
                else
                {
                    ViewBag.Message = "Invalid Bill Ref No";
                    return View(pObjReviseBillModel);
                }
            }
        }

        #endregion

        #region OM052 : Manage Treasury Receipts by Revenue Stream


        [HttpGet]
        public ActionResult TreasuryReceiptByRevenueStream()
        {
            UI_FillYearDropDown();
            return View();
        }


        [HttpPost]
        public JsonResult TreasuryReceiptByRevenueStreamLoadData(int TaxYear, DateTime? FromDate, DateTime? ToDate)
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? TrynParse.parseInt(vLength) : 0;
            int IntSkip = vStart != null ? TrynParse.parseInt(vStart) : 0;
            int IntTotalRecords = 0;

            IList<usp_GetTreasuryReceiptByRevenueStream_Result> lstTreasuryReceiptByRevenueStream = new BLOperationManager().BL_GetTreasuryReceiptByRevenueStream(TaxYear, FromDate, ToDate);

            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstTreasuryReceiptByRevenueStream = lstTreasuryReceiptByRevenueStream.Where(t =>
                t.RevenueStreamName != null && t.RevenueStreamName.Trim().Contains(vFilter.ToLower().Trim()) ||
                t.Amount != null && t.Amount.GetValueOrDefault().ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TransactionCount != null && t.TransactionCount.GetValueOrDefault().ToString().Trim().Contains(vFilter.ToLower().Trim())).ToList();
            }

            IDictionary<string, object> dcFooterTotal = new Dictionary<string, object>();

            dcFooterTotal["Amount"] = lstTreasuryReceiptByRevenueStream.Sum(t => t.Amount);
            dcFooterTotal["TransactionCount"] = lstTreasuryReceiptByRevenueStream.Sum(t => t.TransactionCount);

            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstTreasuryReceiptByRevenueStream = lstTreasuryReceiptByRevenueStream.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstTreasuryReceiptByRevenueStream.Count;
            var data = lstTreasuryReceiptByRevenueStream.Skip(IntSkip).Take(IntPageSize).ToList();


            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data, FooterTotal = dcFooterTotal }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult TreasuryReceiptByRevenueStreamExportToExcel(int TaxYear, DateTime? FromDate, DateTime? ToDate)
        {

            IList<usp_GetTreasuryReceiptByRevenueStream_Result> lstTreasuryReceiptByRevenueStreamData = new BLOperationManager().BL_GetTreasuryReceiptByRevenueStream(TaxYear, FromDate, ToDate);

            string[] strColumns = new string[] { "RevenueStreamName", "Amount", "TransactionCount" };
            string[] strTotalColumns = new string[] { "Amount", "TransactionCount" };
            string method = MethodBase.GetCurrentMethod().Name;

            return ExportToExcel(lstTreasuryReceiptByRevenueStreamData, this.RouteData, strColumns, true, strTotalColumns, method);

        }


        [HttpGet]
        public ActionResult TreasuryReceiptByRevenueStreamDetail(int tyear, DateTime? fdate, DateTime? tdate, int? rstrmID)
        {
            if (rstrmID > 0)
            {
                ViewBag.FromDate = fdate;
                ViewBag.ToDate = tdate;
                ViewBag.RevenueStream = rstrmID;
                ViewBag.TaxYear = tyear;
            }
            else
            {
                return RedirectToAction("TreasuryReceiptByRevenueStream", "OperationManager");
            }

            return View();

        }

        [HttpPost]
        public JsonResult TreasuryReceiptByRevenueStreamDetailLoadData(int TaxYear, DateTime? FromDate, DateTime? ToDate, int? RevenueStreamID)
        {
            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();

            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? TrynParse.parseInt(vLength) : 0;
            int IntSkip = vStart != null ? TrynParse.parseInt(vStart) : 0;
            int IntTotalRecords = 0;


            IList<usp_GetTreasuryReceiptByRevenueStreamDetail_Result> lstTreasuryReceiptByRevenueStreamData = new BLOperationManager().BL_GetTreasuryReceiptByRevenueStreamDetails(TaxYear, FromDate, ToDate, RevenueStreamID);

            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstTreasuryReceiptByRevenueStreamData = lstTreasuryReceiptByRevenueStreamData.
                    Where(t => t.ReceiptRefNo != null && t.ReceiptRefNo.ToLower().Trim().Contains(vFilter.ToLower().Trim())
                || t.TaxPayerName != null && t.TaxPayerName.ToLower().Trim().Contains(vFilter.ToLower().Trim())
                || t.Amount.ToString().Trim().Contains(vFilter.Trim())
                || t.ReceiptDate.Value.ToString("dd-MMM-yy").ToLower().Trim().Contains(vFilter.ToLower().Trim())
                || t.GeneratedBy != null && t.GeneratedBy.ToLower().Trim().Contains(vFilter.ToLower().Trim())).ToList();
            }


            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstTreasuryReceiptByRevenueStreamData = lstTreasuryReceiptByRevenueStreamData.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstTreasuryReceiptByRevenueStreamData.Count;
            var data = lstTreasuryReceiptByRevenueStreamData.Skip(IntSkip).Take(IntPageSize).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult TreasuryReceiptByRevenueStreamDetailExportToExcel(int TaxYear, DateTime? FromDate, DateTime? ToDate, int? RevenueStreamID)
        {
            IList<usp_GetTreasuryReceiptByRevenueStreamDetail_Result> lstData = new BLOperationManager().BL_GetTreasuryReceiptByRevenueStreamDetails(TaxYear, FromDate, ToDate, RevenueStreamID);

            string[] strColumns = new string[] { "TaxPayerTypeName", "TaxPayerRIN", "TaxPayerName", "ReceiptDate", "ReceiptRefNo", "GeneratedBy", "Amount" };
            string[] strTotalColumns = new string[] { "Amount" };
            string method = MethodBase.GetCurrentMethod().Name;


            return ExportToExcel(lstData, this.RouteData, strColumns, true, strTotalColumns, method);

        }

        #endregion

        #region OM053 : Manage TreasuryReceipts by Tax Office


        [HttpGet]
        public ActionResult TreasuryReceiptByTaxOffice()
        {
            UI_FillYearDropDown();
            return View();
        }

        [HttpPost]
        public JsonResult TreasuryReceiptByTaxOfficeLoadData(int TaxYear, DateTime? FromDate, DateTime? ToDate)
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? TrynParse.parseInt(vLength) : 0;
            int IntSkip = vStart != null ? TrynParse.parseInt(vStart) : 0;
            int IntTotalRecords = 0;

            IList<usp_GetTreasuryReceiptByTaxOffice_Result> lstTreasuryReceiptByTaxOffice = new BLOperationManager().BL_GetTreasuryReceiptByTaxOffice(TaxYear, FromDate, ToDate);

            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstTreasuryReceiptByTaxOffice = lstTreasuryReceiptByTaxOffice.Where(t =>
                t.TaxOfficeName != null && t.TaxOfficeName.Trim().Contains(vFilter.ToLower().Trim()) ||
                t.Amount != null && t.Amount.GetValueOrDefault().ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TransactionCount != null && t.TransactionCount.GetValueOrDefault().ToString().Trim().Contains(vFilter.ToLower().Trim())).ToList();
            }

            IDictionary<string, object> dcFooterTotal = new Dictionary<string, object>();

            dcFooterTotal["Amount"] = lstTreasuryReceiptByTaxOffice.Sum(t => t.Amount);
            dcFooterTotal["TransactionCount"] = lstTreasuryReceiptByTaxOffice.Sum(t => t.TransactionCount);

            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstTreasuryReceiptByTaxOffice = lstTreasuryReceiptByTaxOffice.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstTreasuryReceiptByTaxOffice.Count;
            var data = lstTreasuryReceiptByTaxOffice.Skip(IntSkip).Take(IntPageSize).ToList();


            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data, FooterTotal = dcFooterTotal }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult TreasuryReceiptByTaxOfficeExportToExcel(int TaxYear, DateTime? FromDate, DateTime? ToDate)
        {

            IList<usp_GetTreasuryReceiptByTaxOffice_Result> lstTreasuryReceiptByTaxOfficeData = new BLOperationManager().BL_GetTreasuryReceiptByTaxOffice(TaxYear, FromDate, ToDate);

            string[] strColumns = new string[] { "TaxOfficeName", "Amount", "TransactionCount" };
            string[] strTotalColumns = new string[] { "Amount", "TransactionCount" };
            string method = MethodBase.GetCurrentMethod().Name;


            return ExportToExcel(lstTreasuryReceiptByTaxOfficeData, this.RouteData, strColumns, true, strTotalColumns, method);

        }


        [HttpGet]
        public ActionResult TreasuryReceiptByTaxOfficeDetail(int tyear, DateTime? fdate, DateTime? tdate, int? toffID)
        {
            if (toffID > 0)
            {
                ViewBag.FromDate = fdate;
                ViewBag.ToDate = tdate;
                ViewBag.TaxOffice = toffID;
                ViewBag.TaxYear = tyear;
            }
            else
            {
                return RedirectToAction("TreasuryReceiptByTaxOffice", "OperationManager");
            }

            return View();

        }

        [HttpPost]
        public JsonResult TreasuryReceiptByTaxOfficeDetailLoadData(int TaxYear, DateTime? FromDate, DateTime? ToDate, int? TaxOfficeID)
        {
            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();

            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? TrynParse.parseInt(vLength) : 0;
            int IntSkip = vStart != null ? TrynParse.parseInt(vStart) : 0;
            int IntTotalRecords = 0;


            IList<usp_GetTreasuryReceiptByTaxOfficeDetail_Result> lstTreasuryReceiptByTaxOfficeData = new BLOperationManager().BL_GetTreasuryReceiptByTaxOfficeDetails(TaxYear, FromDate, ToDate, TaxOfficeID);

            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstTreasuryReceiptByTaxOfficeData = lstTreasuryReceiptByTaxOfficeData
                    .Where(t => t.ReceiptRefNo != null && t.ReceiptRefNo.ToLower().Trim().Contains(vFilter.ToLower().Trim())
                || t.TaxPayerName != null && t.TaxPayerName.ToLower().Trim().Contains(vFilter.ToLower().Trim())
                || t.Amount.ToString().Trim().Contains(vFilter.Trim())
                || t.ReceiptDate.Value.ToString("dd-MMM-yy").ToLower().Trim().Contains(vFilter.ToLower().Trim())
                || t.GeneratedBy != null && t.GeneratedBy.ToLower().Trim().Contains(vFilter.ToLower().Trim())).ToList();
            }


            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstTreasuryReceiptByTaxOfficeData = lstTreasuryReceiptByTaxOfficeData.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstTreasuryReceiptByTaxOfficeData.Count;
            var data = lstTreasuryReceiptByTaxOfficeData.Skip(IntSkip).Take(IntPageSize).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult TreasuryReceiptByTaxOfficeDetailExportToExcel(int TaxYear, DateTime? FromDate, DateTime? ToDate, int? TaxOfficeID)
        {
            IList<usp_GetTreasuryReceiptByTaxOfficeDetail_Result> lstData = new BLOperationManager().BL_GetTreasuryReceiptByTaxOfficeDetails(TaxYear, FromDate, ToDate, TaxOfficeID);

            string[] strColumns = new string[] { "TaxPayerTypeName", "TaxPayerRIN", "TaxPayerName", "ReceiptDate", "ReceiptRefNo", "GeneratedBy", "Amount" };
            string[] strTotalColumns = new string[] { "Amount" };
            string method = MethodBase.GetCurrentMethod().Name;


            return ExportToExcel(lstData, this.RouteData, strColumns, true, strTotalColumns, method);

        }

        #endregion

        #region OM054 – Employer Liability Report

        [HttpGet]
        public ActionResult EmployerLiability()
        {
            return View();
        }

        public JsonResult EmployerLiabilityLoadData()
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? Convert.ToInt32(vLength) : 0;
            int IntSkip = vStart != null ? Convert.ToInt32(vStart) : 0;
            int IntTotalRecords = 0;


            IList<usp_RPT_EmployerLiability_Result> lstEmployerLiability = new BLOperationManager().BL_GetEmployerLiability();
            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstEmployerLiability = lstEmployerLiability.Where(t =>
                t.TaxPayerRIN != null && t.TaxPayerRIN.Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TaxPayerName != null && t.TaxPayerName.Trim().Contains(vFilter.ToLower().Trim()) ||
                t.MobileNumber != null && t.MobileNumber.Trim().Contains(vFilter.ToLower().Trim()) ||
                t.Balance != null && t.Balance.GetValueOrDefault().ToString().Trim().Contains(vFilter.ToLower().Trim())).ToList();
            }


            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstEmployerLiability = lstEmployerLiability.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstEmployerLiability.Count;
            var data = lstEmployerLiability.Skip(IntSkip).Take(IntPageSize).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult EmployerLiabilityExportToExcel()
        {
            IList<usp_RPT_EmployerLiability_Result> lstEmployerLiability = new BLOperationManager().BL_GetEmployerLiability();

            string[] strColumns = new string[] { "TaxPayerRIN", "TaxPayerName", "MobileNumber", "TotalAssessmentAmount", "TotalPaymentAmount", "Balance" };
            string[] strTotalColumns = new string[] { "TotalAssessmentAmount", "TotalPaymentAmount", "Balance" };
            string method = MethodBase.GetCurrentMethod().Name;

            return ExportToExcel(lstEmployerLiability, this.RouteData, strColumns, false, null, method);
        }

        public ActionResult EmployerLiabilityDetails(int? id)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Company mObjCompany = new Company()
                {
                    CompanyID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(mObjCompany);

                if (mObjCompanyData != null)
                {
                    BLTaxPayerAsset bLTaxPayerAsset = new BLTaxPayerAsset();

                    IList<usp_GetPAYEAssessmentRuleInformation_Result> lstAssessmentRuleInformation = bLTaxPayerAsset.BL_GetPAYEAssessmentRuleInformation((int)EnumList.TaxPayerType.Companies, id.GetValueOrDefault());
                    ViewBag.AssessmentRuleInformation = lstAssessmentRuleInformation;

                    IList<usp_GetPAYEAssessmentBill_Result> lstTaxPayerBill = new BLAssessment().BL_GetPAYEAssessmentBill(id.GetValueOrDefault(), (int)EnumList.TaxPayerType.Companies);
                    ViewBag.TaxPayerBill = lstTaxPayerBill;

                    IList<usp_GetPAYEPayment_Result> lstTaxPayerPayment = new BLSettlement().BL_GetPAYEPayment(id.GetValueOrDefault(), (int)EnumList.TaxPayerType.Companies);
                    ViewBag.TaxPayerPayment = lstTaxPayerPayment;

                    IList<usp_GetPAYEProfileInformation_Result> lstProfileInformation = bLTaxPayerAsset.BL_GetPAYEProfileInformation((int)EnumList.TaxPayerType.Companies, id.GetValueOrDefault());
                    ViewBag.ProfileInformation = lstProfileInformation;


                    return View(mObjCompanyData);
                }
                else
                {
                    return RedirectToAction("EmployerLiability", "OperationManager");
                }
            }
            else
            {
                return RedirectToAction("EmployerLiability", "OperationManager");
            }
        }

        #endregion

        #region OM055 – Employer PAYE Liability Report

        [HttpGet]
        public ActionResult EmployerPAYELiability()
        {
            return View();
        }

        public JsonResult EmployerPAYELiabilityLoadData()
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? Convert.ToInt32(vLength) : 0;
            int IntSkip = vStart != null ? Convert.ToInt32(vStart) : 0;
            int IntTotalRecords = 0;


            IList<usp_RPT_EmployerPAYELiability_Result> lstEmployerLiability = new BLOperationManager().BL_GetEmployerPAYELiability();
            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstEmployerLiability = lstEmployerLiability.Where(t =>
                t.TaxPayerRIN != null && t.TaxPayerRIN.Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TaxPayerName != null && t.TaxPayerName.Trim().Contains(vFilter.ToLower().Trim()) ||
                t.MobileNumber != null && t.MobileNumber.Trim().Contains(vFilter.ToLower().Trim()) ||
                t.AssetName != null && t.AssetName.Trim().Contains(vFilter.ToLower().Trim()) ||
                t.StaffCount != null && t.StaffCount.GetValueOrDefault().ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.BillCount != null && t.BillCount.GetValueOrDefault().ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.Balance != null && t.Balance.GetValueOrDefault().ToString().Trim().Contains(vFilter.ToLower().Trim())).ToList();
            }


            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstEmployerLiability = lstEmployerLiability.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstEmployerLiability.Count;
            var data = lstEmployerLiability.Skip(IntSkip).Take(IntPageSize).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult EmployerPAYELiabilityExportToExcel()
        {
            IList<usp_RPT_EmployerPAYELiability_Result> lstEmployerLiability = new BLOperationManager().BL_GetEmployerPAYELiability();

            string[] strColumns = new string[] { "TaxPayerRIN", "TaxPayerName", "MobileNumber", "AssetName", "StaffCount", "BillCount", "BillAmount", "PaymentAmount", "Balance" };
            string[] strTotalColumns = new string[] { "StaffCount", "BillCount", "BillAmount", "PaymentAmount", "Balance" };
            string method = MethodBase.GetCurrentMethod().Name;

            return ExportToExcel(lstEmployerLiability, this.RouteData, strColumns, false, null, method);
        }

        public ActionResult EmployerPAYELiabilityDetails(int tpid, int aid)
        {
            if (tpid > 0)
            {
                Company mObjCompany = new Company()
                {
                    CompanyID = tpid,
                    intStatus = 2
                };

                usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(mObjCompany);

                if (mObjCompanyData != null)
                {
                    BLTaxPayerAsset bLTaxPayerAsset = new BLTaxPayerAsset();

                    IList<usp_GetPAYEAssessmentRuleInformation_Result> lstAssessmentRuleInformation = bLTaxPayerAsset.BL_GetPAYEAssessmentRuleInformation((int)EnumList.TaxPayerType.Companies, tpid);
                    ViewBag.AssessmentRuleInformation = lstAssessmentRuleInformation;

                    IList<usp_GetPAYEAssessmentBill_Result> lstTaxPayerBill = new BLAssessment().BL_GetPAYEAssessmentBill(tpid, (int)EnumList.TaxPayerType.Companies);
                    ViewBag.TaxPayerBill = lstTaxPayerBill;

                    IList<usp_GetPAYEPayment_Result> lstTaxPayerPayment = new BLSettlement().BL_GetPAYEPayment(tpid, (int)EnumList.TaxPayerType.Companies);
                    ViewBag.TaxPayerPayment = lstTaxPayerPayment;

                    IList<usp_GetPAYEProfileInformation_Result> lstProfileInformation = bLTaxPayerAsset.BL_GetPAYEProfileInformation((int)EnumList.TaxPayerType.Companies, tpid);
                    ViewBag.ProfileInformation = lstProfileInformation;

                    IList<usp_GetPAYEEmployerStaff_Result> lstStaff = bLTaxPayerAsset.BL_GetPAYEEmployerStaff(aid);
                    ViewBag.StaffList = lstStaff;

                    return View(mObjCompanyData);
                }
                else
                {
                    return RedirectToAction("EmployerPAYELiability", "OperationManager");
                }
            }
            else
            {
                return RedirectToAction("EmployerPAYELiability", "OperationManager");
            }
        }

        #endregion

        #region OM056 - TCC - Monthly Summary

        [HttpGet]
        public ActionResult TCCMonthlySummary()
        {
            UI_FillYearDropDown();
            UI_FillTCCStatus();
            return View();
        }

        [HttpPost]
        public JsonResult TCCMonthlySummaryLoadData(int TaxYear, int StatusID)
        {

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? TrynParse.parseInt(vLength) : 0;
            int IntSkip = vStart != null ? TrynParse.parseInt(vStart) : 0;
            int IntTotalRecords = 0;

            TCCReportSearchParams reportParams = new TCCReportSearchParams
            {
                TaxYear = TaxYear,
                StatusID = StatusID
            };

            IList<usp_RPT_MonthlyTCCSummary_Result> lstTCCSummary = new BLOperationManager().BL_GetMonthlyTCCSummary(reportParams);

            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstTCCSummary = lstTCCSummary.Where(t => t.StartMonthName != null && t.StartMonthName.ToLower().Trim().Contains(vFilter.ToLower().Trim())
                || t.RequestCount.ToString().Trim().Contains(vFilter.Trim())).ToList();
            }

            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstTCCSummary = lstTCCSummary.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstTCCSummary.Count;
            var data = lstTCCSummary.Skip(IntSkip).Take(IntPageSize).ToList();


            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult TCCMonthlySummaryExportToExcel(int TaxYear, int StatusID)
        {
            TCCReportSearchParams reportParams = new TCCReportSearchParams
            {
                TaxYear = TaxYear,
                StatusID = StatusID
            };

            IList<usp_RPT_MonthlyTCCSummary_Result> lstTCCSummary = new BLOperationManager().BL_GetMonthlyTCCSummary(reportParams);

            string[] strColumns = new string[] { "StartMonthName", "RequestCount" };
            string[] strTotalColumns = new string[] { "RequestCount" };
            string method = MethodBase.GetCurrentMethod().Name;


            return ExportToExcel(lstTCCSummary, this.RouteData, strColumns, true, strTotalColumns, method);
        }

        [HttpGet]
        public ActionResult TCCMonthlySummaryDetail(int year, int month, int statid)
        {
            ViewBag.TaxYear = year;
            ViewBag.TaxMonth = month;
            ViewBag.StatusID = statid;
            return View();
        }

        [HttpPost]
        public JsonResult TCCMonthlySummaryDetailLoadData(int TaxYear, int TaxMonth, int StatusID)
        {

            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            var vFilter = Request.Form.GetValues("search[value]").FirstOrDefault();
            int IntPageSize = vLength != null ? TrynParse.parseInt(vLength) : 0;
            int IntSkip = vStart != null ? TrynParse.parseInt(vStart) : 0;
            int IntTotalRecords = 0, IntFilteredRecords = 0;

            TCCReportSearchParams reportParams = new TCCReportSearchParams
            {
                TaxYear = TaxYear,
                TaxMonth = TaxMonth,
                StatusID = StatusID
            };

            IList<usp_RPT_MonthlyTCCSummaryDetail_Result> lstTCCSummaryDetail = new BLOperationManager().BL_GetMonthlyTCCSummaryDetail(reportParams);

            IntTotalRecords = lstTCCSummaryDetail.Count;
            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstTCCSummaryDetail = lstTCCSummaryDetail.Where(t => t.RequestRefNo != null && t.RequestRefNo.Trim().Contains(vFilter.Trim())
                || t.TaxPayerName != null && t.TaxPayerName.Trim().Contains(vFilter.Trim())
                || t.TaxPayerRIN != null && t.TaxPayerRIN.Trim().Contains(vFilter.Trim())
                || t.TaxPayerTIN != null && t.TaxPayerTIN.Trim().Contains(vFilter.Trim())
                || t.MobileNumber != null && t.MobileNumber.Trim().Contains(vFilter.Trim())
                || t.StatusName != null && t.StatusName.Trim().Contains(vFilter.Trim())
                || t.RequestDate.Value.ToString("dd-MMM-yy").Trim().Contains(vFilter.Trim())).ToList();
            }

            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstTCCSummaryDetail = lstTCCSummaryDetail.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntFilteredRecords = lstTCCSummaryDetail.Count;
            var data = lstTCCSummaryDetail.Skip(IntSkip).Take(IntPageSize).ToList();


            return Json(new { draw = vDraw, recordsFiltered = IntFilteredRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult TCCMonthlySummaryDetailExportToExcel(int TaxYear, int TaxMonth, int StatusID)
        {
            TCCReportSearchParams reportParams = new TCCReportSearchParams
            {
                TaxYear = TaxYear,
                TaxMonth = TaxMonth,
                StatusID = StatusID
            };

            IList<usp_RPT_MonthlyTCCSummaryDetail_Result> lstTCCSummaryDetail = new BLOperationManager().BL_GetMonthlyTCCSummaryDetail(reportParams);

            string[] strColumns = new string[] { "RequestDate", "RequestRefNo", "TaxPayerName", "TaxPayerRIN", "TaxPayerTIN", "MobileNumber", "StatusName" };
            string method = MethodBase.GetCurrentMethod().Name;
            return ExportToExcel(lstTCCSummaryDetail, this.RouteData, strColumns, false, null, method);
        }

        #endregion

        #region OM057 - TCC - Monthly Revoked

        [HttpGet]
        public ActionResult TCCMonthlyRevoked()
        {
            UI_FillYearDropDown();
            UI_FillTCCStatus();
            return View();
        }

        [HttpPost]
        public JsonResult TCCMonthlyRevokedLoadData(int TaxYear, int StatusID)
        {

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? TrynParse.parseInt(vLength) : 0;
            int IntSkip = vStart != null ? TrynParse.parseInt(vStart) : 0;
            int IntTotalRecords = 0;

            TCCReportSearchParams reportParams = new TCCReportSearchParams
            {
                TaxYear = TaxYear,
                StatusID = StatusID
            };

            IList<usp_RPT_MonthlyTCCRevokeList_Result> lstTCCRevoke = new BLOperationManager().BL_GetMonthlyTCCRevokeSummary(reportParams);

            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstTCCRevoke = lstTCCRevoke.Where(t => t.StartMonthName != null && t.StartMonthName.ToLower().Trim().Contains(vFilter.ToLower().Trim())
                || t.RequestCount.ToString().Trim().Contains(vFilter.Trim())).ToList();
            }

            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstTCCRevoke = lstTCCRevoke.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstTCCRevoke.Count;
            var data = lstTCCRevoke.Skip(IntSkip).Take(IntPageSize).ToList();


            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult TCCMonthlyRevokedExportToExcel(int TaxYear, int StatusID)
        {
            TCCReportSearchParams reportParams = new TCCReportSearchParams
            {
                TaxYear = TaxYear,
                StatusID = StatusID
            };

            IList<usp_RPT_MonthlyTCCRevokeList_Result> lstTCCRevoke = new BLOperationManager().BL_GetMonthlyTCCRevokeSummary(reportParams);

            string[] strColumns = new string[] { "StartMonthName", "RequestCount" };
            string[] strTotalColumns = new string[] { "RequestCount" };
            string method = MethodBase.GetCurrentMethod().Name;

            return ExportToExcel(lstTCCRevoke, this.RouteData, strColumns, true, strTotalColumns, method);
        }

        [HttpGet]
        public ActionResult TCCMonthlyRevokedDetail(int year, int month, int statid)
        {
            ViewBag.TaxYear = year;
            ViewBag.TaxMonth = month;
            ViewBag.StatusID = statid;
            return View();
        }

        [HttpPost]
        public JsonResult TCCMonthlyRevokedDetailLoadData(int TaxYear, int TaxMonth, int StatusID)
        {

            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            var vFilter = Request.Form.GetValues("search[value]").FirstOrDefault();
            int IntPageSize = vLength != null ? TrynParse.parseInt(vLength) : 0;
            int IntSkip = vStart != null ? TrynParse.parseInt(vStart) : 0;
            int IntTotalRecords = 0, IntFilteredRecords = 0;

            TCCReportSearchParams reportParams = new TCCReportSearchParams
            {
                TaxYear = TaxYear,
                TaxMonth = TaxMonth,
                StatusID = StatusID
            };

            IList<usp_RPT_MonthlyTCCRevokeDetail_Result> lstTCCRevokeDetail = new BLOperationManager().BL_GetMonthlyTCCRevokeDetail(reportParams);

            IntTotalRecords = lstTCCRevokeDetail.Count;
            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstTCCRevokeDetail = lstTCCRevokeDetail.Where(t => t.RequestRefNo != null && t.RequestRefNo.Trim().Contains(vFilter.Trim())
                || t.TaxPayerName != null && t.TaxPayerName.Trim().Contains(vFilter.Trim())
                || t.TaxPayerRIN != null && t.TaxPayerRIN.Trim().Contains(vFilter.Trim())
                || t.TaxPayerTIN != null && t.TaxPayerTIN.Trim().Contains(vFilter.Trim())
                || t.MobileNumber != null && t.MobileNumber.Trim().Contains(vFilter.Trim())
                || t.StatusName != null && t.StatusName.Trim().Contains(vFilter.Trim())
                || t.RequestDate.Value.ToString("dd-MMM-yy").Trim().Contains(vFilter.Trim())).ToList();
            }

            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstTCCRevokeDetail = lstTCCRevokeDetail.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntFilteredRecords = lstTCCRevokeDetail.Count;
            var data = lstTCCRevokeDetail.Skip(IntSkip).Take(IntPageSize).ToList();


            return Json(new { draw = vDraw, recordsFiltered = IntFilteredRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult TCCMonthlyRevokedDetailExportToExcel(int TaxYear, int TaxMonth, int StatusID)
        {
            TCCReportSearchParams reportParams = new TCCReportSearchParams
            {
                TaxYear = TaxYear,
                TaxMonth = TaxMonth,
                StatusID = StatusID
            };

            IList<usp_RPT_MonthlyTCCRevokeDetail_Result> lstTCCRevokeDetail = new BLOperationManager().BL_GetMonthlyTCCRevokeDetail(reportParams);

            string[] strColumns = new string[] { "RequestDate", "RequestRefNo", "TaxPayerName", "TaxPayerRIN", "TaxPayerTIN", "MobileNumber", "StatusName" };

            string method = MethodBase.GetCurrentMethod().Name;
            return ExportToExcel(lstTCCRevokeDetail, this.RouteData, strColumns, false, null, method);
        }

        #endregion

        #region OM058 - TCC - Monthly Commission

        [HttpGet]
        public ActionResult TCCMonthlyCommission()
        {
            UI_FillYearDropDown();
            UI_FillTCCStatus();
            return View();
        }

        [HttpPost]
        public JsonResult TCCMonthlyCommissionLoadData(int TaxYear, int StatusID)
        {

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? TrynParse.parseInt(vLength) : 0;
            int IntSkip = vStart != null ? TrynParse.parseInt(vStart) : 0;
            int IntTotalRecords = 0;

            TCCReportSearchParams reportParams = new TCCReportSearchParams
            {
                TaxYear = TaxYear,
                StatusID = StatusID
            };

            IList<usp_RPT_MonthlyTCCCommissionList_Result> lstTCCCommission = new BLOperationManager().BL_GetMonthlyTCCCommissionSummary(reportParams);

            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstTCCCommission = lstTCCCommission.Where(t => t.StartMonthName != null && t.StartMonthName.ToLower().Trim().Contains(vFilter.ToLower().Trim())
                || t.RequestCount.ToString().Trim().Contains(vFilter.Trim())).ToList();
            }

            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstTCCCommission = lstTCCCommission.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstTCCCommission.Count;
            var data = lstTCCCommission.Skip(IntSkip).Take(IntPageSize).ToList();


            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult TCCMonthlyCommissionExportToExcel(int TaxYear, int StatusID)
        {
            TCCReportSearchParams reportParams = new TCCReportSearchParams
            {
                TaxYear = TaxYear,
                StatusID = StatusID
            };

            IList<usp_RPT_MonthlyTCCCommissionList_Result> lstTCCCommission = new BLOperationManager().BL_GetMonthlyTCCCommissionSummary(reportParams);

            string[] strColumns = new string[] { "StartMonthName", "RequestCount", "CommissionAmount" };
            string[] strTotalColumns = new string[] { "RequestCount", "CommissionAmount" };
            string method = MethodBase.GetCurrentMethod().Name;

            return ExportToExcel(lstTCCCommission, this.RouteData, strColumns, true, strTotalColumns, method);
        }

        #endregion

        #region OM059 - Revise a Bill

        [HttpGet]
        public ActionResult ReviseBillSummary()
        {
            IList<DropDownListResult> lstRevenueStream = new BLRevenueStream().BL_GetRevenueStreamDropDownList(new Revenue_Stream() { intStatus = 1 });
            lstRevenueStream = lstRevenueStream.Where(t => t.id != 8).ToList();
            ViewBag.RevenueStreamList = new SelectList(lstRevenueStream, "id", "text");

            IList<SelectListItem> lstBillType = new List<SelectListItem>
            {
                new SelectListItem(){Value="0",Text="All",Selected=true},
                new SelectListItem(){Value="1",Text="Assessment"},
                new SelectListItem(){Value="2",Text="Service Bills"}
            };

            ViewBag.BillTypeList = lstBillType;

            UI_FillYearDropDown();

            return View();
        }

        [HttpPost]
        public JsonResult ReviseBillSummaryLoadData(int TaxYear, DateTime? FromDate, DateTime? ToDate, int RevenueStreamID, int BillTypeID)
        {

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? TrynParse.parseInt(vLength) : 0;
            int IntSkip = vStart != null ? TrynParse.parseInt(vStart) : 0;
            int IntTotalRecords = 0;

            IList<usp_RPT_ReviseBill_Result> lstReviseBill = new BLOperationManager().BL_GetReviseBillSummary(TaxYear, FromDate, ToDate, BillTypeID, RevenueStreamID);

            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstReviseBill = lstReviseBill.Where(t => t.BillRef != null && t.BillRef.Trim().Contains(vFilter.Trim())
                || t.BillAmount.ToString().Trim().Contains(vFilter.Trim())
                || t.BillDate.Value.ToString("dd-MMM-yy").Trim().Contains(vFilter.Trim())
                || t.BillStatusName.Trim().Contains(vFilter.Trim())
                || t.RevisedDate.Value.ToString("dd-MMM-yy").Trim().Contains(vFilter.Trim())
                || t.RevisedAmount.ToString().Trim().Contains(vFilter.Trim())
                ).ToList();
            }

            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstReviseBill = lstReviseBill.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstReviseBill.Count;
            var data = lstReviseBill.Skip(IntSkip).Take(IntPageSize).ToList();


            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ReviseBillSummaryExportToExcel(int TaxYear, DateTime? FromDate, DateTime? ToDate, int RevenueStreamID, int BillTypeID)
        {
            IList<usp_RPT_ReviseBill_Result> lstReviseBill = new BLOperationManager().BL_GetReviseBillSummary(TaxYear, FromDate, ToDate, BillTypeID, RevenueStreamID);

            string[] strColumns = new string[] { "BillRef", "BillDate", "BillAmount", "RevisedDate", "RevisedAmount", "BillStatusName" };
            string[] strTotalColumns = new string[] { "BillAmount", "RevisedAmount" };
            string method = MethodBase.GetCurrentMethod().Name;

            return ExportToExcel(lstReviseBill, this.RouteData, strColumns, true, strTotalColumns, method);
        }


        #endregion

        #region OM060 - Add Manual PoA

        private void UI_FillManualPoADropDown(ManualPoAViewModel pObjPoAModel = null)
        {
            if (pObjPoAModel == null)
            {
                pObjPoAModel = new ManualPoAViewModel()
                {
                    RevenueStreamID = -1
                };
            }

            UI_FillTaxPayerTypeDropDown();

            UI_FillRevenueStreamDropDown();
            UI_FillRevenueSubStreamDropDown(new Revenue_SubStream() { RevenueStreamID = pObjPoAModel.RevenueStreamID, intStatus = 1 });
            UI_FillAgencyDropDown();
            UI_FillSettlementMethodDropDown();
        }

        [HttpGet]
        public ActionResult ManualPoA()
        {
            UI_FillManualPoADropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult ManualPoA(ManualPoAViewModel pObjPoAModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillManualPoADropDown(pObjPoAModel);
                return View(pObjPoAModel);
            }
            else
            {
                Payment_Account mObjPaymentAccount = new Payment_Account()
                {
                    PaymentAccountID = 0,
                    TaxPayerTypeID = pObjPoAModel.TaxPayerTypeID,
                    TaxPayerID = pObjPoAModel.TaxPayerID,
                    Amount = pObjPoAModel.Amount,
                    SettlementMethodID = pObjPoAModel.SettlementMethodID,
                    SettlementStatusID = (int)EnumList.SettlementStatus.Settled,
                    PaymentDate = pObjPoAModel.PaymentDate,
                    TransactionRefNo = pObjPoAModel.TransactionRefNo,
                    Notes = pObjPoAModel.Notes,
                    RevenueStreamID = pObjPoAModel.RevenueStreamID,
                    RevenueSubStreamID = pObjPoAModel.RevenueSubStreamID,
                    AgencyID = pObjPoAModel.AgencyID,
                    isManualEntry = true,
                    Active = true,
                    CreatedBy = SessionManager.UserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                FuncResponse<Payment_Account> mObjFuncResponse = new BLPaymentAccount().BL_InsertUpdatePaymentAccount(mObjPaymentAccount);

                if (mObjFuncResponse.Success)
                {
                    ModelState.Clear();
                    UI_FillManualPoADropDown();
                    FlashMessage.Info("Payment on Account Added Successfully");
                    return View();
                }
                else
                {
                    ViewBag.Message = mObjFuncResponse.Message;
                    UI_FillManualPoADropDown(pObjPoAModel);
                    return View(pObjPoAModel);
                }
            }
        }

        #endregion

        #region OM061 - Tax Payer Capture Analysis

        [HttpGet]
        public ActionResult TaxPayerCaptureAnalysis()
        {
            UI_FillYearDropDown();
            UI_FillTaxPayerTypeDropDown();
            UI_FillTaxOfficeDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TaxPayerCaptureAnalysis(TaxPayerCaptureAnalysisViewModel pObjExportProfileModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillYearDropDown();
                UI_FillTaxPayerTypeDropDown();
                UI_FillTaxOfficeDropDown();
                return View(pObjExportProfileModel);
            }
            else
            {
                IList<usp_RPT_TaxPayerCaptureAnalysis_Result> lstData = new BLOperationManager().BL_TaxPayerCaptureAnalysis(pObjExportProfileModel.FromDate, pObjExportProfileModel.ToDate, pObjExportProfileModel.TaxPayerTypeID, pObjExportProfileModel.TaxOfficeID);

                string[] strColumns = new string[] { "TaxPayerRIN", "TaxPayerName", "TaxPayerMobileNumber", "TaxPayerAddress", "TaxOfficeName", "CreatedDate", "CreatedBy" };
                string method = MethodBase.GetCurrentMethod().Name;
                return ExportToExcel(lstData, this.RouteData, strColumns, method);
            }
        }

        #endregion

        #region OM062 - PAYE Output Aggregation Report

        [HttpGet]
        public ActionResult PAYEOutputAggregation()
        {
            UI_FillYearDropDown();
            UI_FillTaxOfficeDropDown();
            return View();
        }

        [HttpPost]
        public JsonResult PAYEOutputAggregationLoadData(int TaxYear, int? TaxOfficeID)
        {

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? TrynParse.parseInt(vLength) : 0;
            int IntSkip = vStart != null ? TrynParse.parseInt(vStart) : 0;
            int IntTotalRecords = 0, IntFilteredRecords = 0;


            IList<usp_RPT_PAYEOutputAggregationSummary_Result> lstData = new BLPAYEOutput().BL_PAYEOutputAggregationSummary(TaxYear, TaxOfficeID);

            IntTotalRecords = lstData.Count;

            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstData = lstData.Where(t => t.EmployerName != null && t.EmployerName.ToLower().Trim().Contains(vFilter.ToLower().Trim())
                || t.EmployerRIN != null && t.EmployerRIN.ToLower().Trim().Contains(vFilter.ToLower().Trim())
                || t.EmployeeCount.ToString().Trim().Contains(vFilter.Trim())
                || t.AmountCollected.ToString().Trim().Contains(vFilter.Trim())).ToList();
            }

            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstData = lstData.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntFilteredRecords = lstData.Count;
            var data = lstData.Skip(IntSkip).Take(IntPageSize).ToList();


            return Json(new { draw = vDraw, recordsFiltered = IntFilteredRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PAYEOutputAggregationExportToExcel(int TaxYear, int? TaxOfficeID)
        {
            IList<usp_RPT_PAYEOutputAggregationSummary_Result> lstData = new BLPAYEOutput().BL_PAYEOutputAggregationSummary(TaxYear, TaxOfficeID);

            string[] strColumns = new string[] { "EmployerRIN", "EmployerName", "EmployeeCount", "AmountCollected" };
            string[] strTotalColumns = new string[] { "EmployeeCount", "AmountCollected" };
            string method = MethodBase.GetCurrentMethod().Name;

            return ExportToExcel(lstData, this.RouteData, strColumns, true, strTotalColumns, method);
        }

        [HttpGet]
        public ActionResult PAYEOutputAggregationDetail(string erin, int tyear, int? toffID)
        {
            if (!string.IsNullOrWhiteSpace(erin))
            {
                ViewBag.EmployerRIN = erin;
                ViewBag.TaxOffice = toffID;
                ViewBag.TaxYear = tyear;
            }
            else
            {
                return RedirectToAction("PAYEOutputAggregation", "OperationManager");
            }

            return View();

        }

        [HttpPost]
        public JsonResult PAYEOutputAggregationDetailLoadData(string EmployerRIN, int TaxYear, int? TaxOfficeID)
        {
            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();

            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? TrynParse.parseInt(vLength) : 0;
            int IntSkip = vStart != null ? TrynParse.parseInt(vStart) : 0;
            int IntTotalRecords = 0, IntFilteredRecords = 0;


            IList<usp_RPT_PAYEOutputAggregationList_Result> lstData = new BLPAYEOutput().BL_PAYEOutputAggregationList(EmployerRIN, TaxYear, TaxOfficeID);

            IntTotalRecords = lstData.Count;
            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstData = lstData
                    .Where(t => t.EmployeeName != null && t.EmployeeName.ToLower().Trim().Contains(vFilter.ToLower().Trim())
                || t.EmployeeRIN != null && t.EmployeeRIN.ToLower().Trim().Contains(vFilter.ToLower().Trim())
                || t.AmountContributed.ToString().Trim().Contains(vFilter.Trim())).ToList();
            }


            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstData = lstData.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntFilteredRecords = lstData.Count;
            var data = lstData.Skip(IntSkip).Take(IntPageSize).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntFilteredRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PAYEOutputAggregationDetailExportToExcel(string EmployerRIN, int TaxYear, int? TaxOfficeID)
        {
            IList<usp_RPT_PAYEOutputAggregationList_Result> lstData = new BLPAYEOutput().BL_PAYEOutputAggregationList(EmployerRIN, TaxYear, TaxOfficeID);

            string[] strColumns = new string[] { "EmployeeRIN", "EmployeeName", "AmountContributed" };
            string[] strTotalColumns = new string[] { "AmountContributed" };
            string method = MethodBase.GetCurrentMethod().Name;

            return ExportToExcel(lstData, this.RouteData, strColumns, true, strTotalColumns, method);

        }

        #endregion


        public ActionResult TaxOfficeManagerStatus()
        {
            UI_FillTaxOfficeDropDown();
            UI_FillTaxPayerTypeDropDown();
            UI_FillReviewStatus();
            return View();
        }


        public JsonResult TaxOfficeManagerStatusLoadData(int TaxOfficeID, int TaxPayerTypeID, int ReviewStatusID, int TOManagerID)
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? Convert.ToInt32(vLength) : 0;
            int IntSkip = vStart != null ? Convert.ToInt32(vStart) : 0;
            int IntTotalRecords = 0;


            IList<usp_RPT_TaxOfficeManagerStatus_Result> lstTaxPayer = new BLOperationManager().BL_GetTaxOfficeManagerStatus(TaxOfficeID, TaxPayerTypeID, ReviewStatusID, TOManagerID);
            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstTaxPayer = lstTaxPayer.Where(t =>
                t.TaxPayerName != null && t.TaxPayerName.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TaxPayerRIN != null && t.TaxPayerRIN.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TotalAssessmentAmount != null && t.TotalAssessmentAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TotalPaymentAmount != null && t.TotalPaymentAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.OutstandingAmount != null && t.OutstandingAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.ReviewStatusName != null && t.ReviewStatusName.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TaxOfficerName != null && t.TaxOfficerName.ToLower().Trim().Contains(vFilter.ToLower().Trim())).ToList();
            }


            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstTaxPayer = lstTaxPayer.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            decimal dcAssessmentTotal = lstTaxPayer.Sum(t => t.TotalAssessmentAmount.GetValueOrDefault());
            decimal dcPaymentTotal = lstTaxPayer.Sum(t => t.TotalPaymentAmount.GetValueOrDefault());
            decimal dcOutstandingTotal = lstTaxPayer.Sum(t => t.OutstandingAmount.GetValueOrDefault());

            IntTotalRecords = lstTaxPayer.Count();
            var data = lstTaxPayer.Skip(IntSkip).Take(IntPageSize).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data, AssessmentTotal = dcAssessmentTotal, PaymentTotal = dcPaymentTotal, OutstandingTotal = dcOutstandingTotal }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TaxOfficeManagerStatusExportToExcel(int TaxOfficeID, int TaxPayerTypeID, int ReviewStatusID, int TOManagerID)
        {
            string strTableName = "Tax Office Manager Status";


            IList<usp_RPT_TaxOfficeManagerStatus_Result> lstTaxPayer = new BLOperationManager().BL_GetTaxOfficeManagerStatus(TaxOfficeID, TaxPayerTypeID, ReviewStatusID, TOManagerID);

            DataTable dt = CommUtil.ConvertToDataTable(lstTaxPayer);

            var ObjExcelData = CommUtil.ConvertDataTableToExcel(dt);
            return File(ObjExcelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "TaxOfficeManagerStatus_" + DateTime.Now.ToString("dd_MM_yy") + ".xlsx");

        }


        public ActionResult TaxOfficerSummary()
        {
            UI_FillTaxOfficeDropDown();
            return View();
        }

        public JsonResult TaxOfficerSummaryLoadData(int TaxOfficeID)
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? Convert.ToInt32(vLength) : 0;
            int IntSkip = vStart != null ? Convert.ToInt32(vStart) : 0;
            int IntTotalRecords = 0;


            IList<usp_RPT_TaxOfficerSummary_Result> lstSummary = new BLOperationManager().BL_GetTaxOfficerSummary(TaxOfficeID);
            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstSummary = lstSummary.Where(t =>
                t.TotalAssessmentAmount != null && t.TotalAssessmentAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TotalPaymentAmount != null && t.TotalPaymentAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.OutstandingAmount != null && t.OutstandingAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TotalTaxPayerCount != null && t.TotalTaxPayerCount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TaxOfficerName != null && t.TaxOfficerName.ToLower().Trim().Contains(vFilter.ToLower().Trim())).ToList();
            }


            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstSummary = lstSummary.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstSummary.Count();
            var data = lstSummary.Skip(IntSkip).Take(IntPageSize).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TaxOfficerSummaryExportToExcel(int TaxOfficeID)
        {
            string strTableName = "Tax Officer Summary";


            IList<usp_RPT_TaxOfficerSummary_Result> lstSummary = new BLOperationManager().BL_GetTaxOfficerSummary(TaxOfficeID);
            DataTable dt = CommUtil.ConvertToDataTable(lstSummary);

            var ObjExcelData = CommUtil.ConvertDataTableToExcel(dt);
            return File(ObjExcelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "TaxOfficerSummary_" + DateTime.Now.ToString("dd_MM_yy") + ".xlsx");

        }


        public ActionResult TaxOfficerTaxPayerList(int toid, int tofid)
        {
            ViewBag.TaxOfficeID = toid;
            ViewBag.TaxOfficerID = tofid;
            return View();
        }

        public JsonResult TaxOfficerTaxPayerLoadData(int TaxOfficeID, int TaxOfficerID)
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? Convert.ToInt32(vLength) : 0;
            int IntSkip = vStart != null ? Convert.ToInt32(vStart) : 0;
            int IntTotalRecords = 0;


            IList<usp_GetTaxPayerforTaxOfficer_Result> lstTaxPayer = new BLOperationManager().BL_GetTaxPayerBasedOnTaxOfficer(TaxOfficeID, TaxOfficerID);
            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstTaxPayer = lstTaxPayer.Where(t =>
                t.TaxPayerName != null && t.TaxPayerName.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TaxPayerRIN != null && t.TaxPayerRIN.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TotalAssessmentAmount != null && t.TotalAssessmentAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TotalPaymentAmount != null && t.TotalPaymentAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.OutstandingAmount != null && t.OutstandingAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.ReviewStatusName != null && t.ReviewStatusName.ToLower().Trim().Contains(vFilter.ToLower().Trim())

                ).ToList();
            }


            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstTaxPayer = lstTaxPayer.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstTaxPayer.Count();
            var data = lstTaxPayer.Skip(IntSkip).Take(IntPageSize).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult TaxOfficeManagerSummary()
        {
            UI_FillTaxOfficeDropDown();
            return View();
        }

        public JsonResult TaxOfficeManagerSummaryLoadData(int TaxOfficeID)
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? Convert.ToInt32(vLength) : 0;
            int IntSkip = vStart != null ? Convert.ToInt32(vStart) : 0;
            int IntTotalRecords = 0;


            IList<usp_RPT_TaxOfficeManagerSummary_Result> lstSummary = new BLOperationManager().BL_GetTaxOfficeManagerSummary(TaxOfficeID);
            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstSummary = lstSummary.Where(t =>
                t.TotalAssessmentAmount != null && t.TotalAssessmentAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TotalPaymentAmount != null && t.TotalPaymentAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.OutstandingAmount != null && t.OutstandingAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TaxOfficerCount != null && t.TaxOfficerCount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TotalTaxPayerCount != null && t.TotalTaxPayerCount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TaxOfficeManagerName != null && t.TaxOfficeManagerName.ToLower().Trim().Contains(vFilter.ToLower().Trim())).ToList();
            }


            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstSummary = lstSummary.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstSummary.Count();
            var data = lstSummary.Skip(IntSkip).Take(IntPageSize).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TaxOfficeManagerSummaryExportToExcel(int TaxOfficeID)
        {
            string strTableName = "Tax Office Manager Summary";


            IList<usp_RPT_TaxOfficeManagerSummary_Result> lstSummary = new BLOperationManager().BL_GetTaxOfficeManagerSummary(TaxOfficeID);
            DataTable dt = CommUtil.ConvertToDataTable(lstSummary);

            var ObjExcelData = CommUtil.ConvertDataTableToExcel(dt);
            return File(ObjExcelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "TaxOfficeManagerSummary_" + DateTime.Now.ToString("dd_MM_yy") + ".xlsx");

        }


        public ActionResult TaxOfficeManagerTaxPayerList(int toid, int tomid)
        {
            ViewBag.TaxOfficeID = toid;
            ViewBag.TOManagerID = tomid;
            return View();
        }

        public JsonResult TaxOfficeManagerTaxPayerLoadData(int TaxOfficeID, int TOManagerID)
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? Convert.ToInt32(vLength) : 0;
            int IntSkip = vStart != null ? Convert.ToInt32(vStart) : 0;
            int IntTotalRecords = 0;


            IList<usp_GetTaxPayerforTaxOfficeManager_Result> lstTaxPayer = new BLOperationManager().BL_GetTaxPayerBasedOnTaxOfficeManager(TaxOfficeID, TOManagerID);
            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstTaxPayer = lstTaxPayer.Where(t =>
                t.TaxPayerName != null && t.TaxPayerName.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TaxPayerRIN != null && t.TaxPayerRIN.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TotalAssessmentAmount != null && t.TotalAssessmentAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TotalPaymentAmount != null && t.TotalPaymentAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.OutstandingAmount != null && t.OutstandingAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.ReviewStatusName != null && t.ReviewStatusName.ToLower().Trim().Contains(vFilter.ToLower().Trim())

                ).ToList();
            }


            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstTaxPayer = lstTaxPayer.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstTaxPayer.Count();
            var data = lstTaxPayer.Skip(IntSkip).Take(IntPageSize).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult JTBPullData()
        {
            IList<SelectListItem> lstTaxPayer = new List<SelectListItem>();
            lstTaxPayer.Add(new SelectListItem() { Value = "1", Text = "Individual" });
            lstTaxPayer.Add(new SelectListItem() { Value = "2", Text = "Non Individual" });

            ViewBag.TaxPayerList = lstTaxPayer;

            return View();
        }

        public ActionResult ReviewStatusSummary()
        {
            UI_FillTaxOfficeDropDown();
            UI_FillTaxPayerTypeDropDown();
            UI_FillReviewStatus();
            return View();
        }

        public JsonResult ReviewStatusSummaryLoadData(int TaxOfficeID, int TaxPayerTypeID, int ReviewStatusID)
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? Convert.ToInt32(vLength) : 0;
            int IntSkip = vStart != null ? Convert.ToInt32(vStart) : 0;
            int IntTotalRecords = 0;


            IList<usp_RPT_ReviewStatusSummary_Result> lstSummary = new BLOperationManager().BL_GetReviewStatusSummary(TaxOfficeID, TaxPayerTypeID, ReviewStatusID);
            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstSummary = lstSummary.Where(t =>
                t.ReviewStatusName != null && t.ReviewStatusName.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TotalAssessmentAmount != null && t.TotalAssessmentAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TotalPaymentAmount != null && t.TotalPaymentAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.OutstandingAmount != null && t.OutstandingAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TotalTaxPayerCount != null && t.TotalTaxPayerCount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim())).ToList();
            }


            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstSummary = lstSummary.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstSummary.Count();
            var data = lstSummary.Skip(IntSkip).Take(IntPageSize).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReviewStatusSummaryExportToExcel(int TaxOfficeID, int TaxPayerTypeID, int ReviewStatusID)
        {
            string strTableName = "Review Status Summary Report";

            IList<usp_RPT_ReviewStatusSummary_Result> lstSummary = new BLOperationManager().BL_GetReviewStatusSummary(TaxOfficeID, TaxPayerTypeID, ReviewStatusID);
            DataTable dt = CommUtil.ConvertToDataTable(lstSummary);

            var ObjExcelData = CommUtil.ConvertDataTableToExcel(dt);
            return File(ObjExcelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReviewStatusSummaryReport_" + DateTime.Now.ToString("dd_MM_yy") + ".xlsx");

        }

        public ActionResult ReviewStatusTaxPayer(int toid, int tptid, int rsid)
        {
            ViewBag.TaxOfficeID = toid;
            ViewBag.TaxPayerTypeID = tptid;
            ViewBag.ReviewStatusID = rsid;
            return View();
        }

        public JsonResult ReviewStatusTaxPayerLoadData(int TaxOfficeID, int TaxPayerTypeID, int ReviewStatusID)
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? Convert.ToInt32(vLength) : 0;
            int IntSkip = vStart != null ? Convert.ToInt32(vStart) : 0;
            int IntTotalRecords = 0;


            IList<usp_RPT_TaxOfficeManagerStatus_Result> lstTaxPayer = new BLOperationManager().BL_GetTaxOfficeManagerStatus(TaxOfficeID, TaxPayerTypeID, ReviewStatusID, 0);
            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstTaxPayer = lstTaxPayer.Where(t =>
                t.TaxPayerName != null && t.TaxPayerName.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TaxPayerRIN != null && t.TaxPayerRIN.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TotalAssessmentAmount != null && t.TotalAssessmentAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TotalPaymentAmount != null && t.TotalPaymentAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.OutstandingAmount != null && t.OutstandingAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.ReviewStatusName != null && t.ReviewStatusName.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TaxOfficerName != null && t.TaxOfficerName.ToLower().Trim().Contains(vFilter.ToLower().Trim())).ToList();
            }


            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstTaxPayer = lstTaxPayer.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstTaxPayer.Count();
            var data = lstTaxPayer.Skip(IntSkip).Take(IntPageSize).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TaxOfficerMonthlyPayment()
        {
            UI_FillTaxOfficeDropDown();
            UI_FillYearDropDown();
            return View();
        }

        public JsonResult TaxOfficerMonthlyPaymentLoadData(int TaxOfficeID, int TaxOfficerID, int Year)
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? Convert.ToInt32(vLength) : 0;
            int IntSkip = vStart != null ? Convert.ToInt32(vStart) : 0;
            int IntTotalRecords = 0;


            IList<usp_RPT_TaxOfficerMonthlyPayment_Result> lstSummary = new BLOperationManager().BL_GetTaxOfficerMonthlyPayment(TaxOfficeID, TaxOfficerID, 0, Year);
            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstSummary = lstSummary.Where(t =>
                t.StartMonthName != null && t.StartMonthName.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.Settlement != null && t.Settlement.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.PoA != null && t.PoA.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TotalPayment != null && t.TotalPayment.Value.ToString().Trim().Contains(vFilter.ToLower().Trim())).ToList();
            }


            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstSummary = lstSummary.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstSummary.Count();
            var data = lstSummary.Skip(IntSkip).Take(IntPageSize).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TaxOfficeTarget()
        {
            UI_FillYearDropDown();
            IList<usp_GetTaxOfficeList_Result> lstTaxOffice = new BLTaxOffice().BL_GetTaxOfficeList(new Tax_Offices() { intStatus = 1 });
            return View(lstTaxOffice);
        }

        public ActionResult SetTaxOfficeTarget(int? toid, int? year)
        {
            if (toid.GetValueOrDefault() > 0 && year.GetValueOrDefault() > 0)
            {
                usp_GetTaxOfficeList_Result mObjTaxOfficeData = new BLTaxOffice().BL_GetTaxOfficeDetails(new Tax_Offices() { intStatus = 1, TaxOfficeID = toid.GetValueOrDefault() });

                if (mObjTaxOfficeData != null)
                {
                    TaxOfficeTargetViewModel mObjTargetModel = new TaxOfficeTargetViewModel()
                    {
                        TaxOfficeID = toid.GetValueOrDefault(),
                        TaxOfficeName = mObjTaxOfficeData.TaxOfficeName,
                        TaxYearID = year.GetValueOrDefault(),
                        TaxYearName = year.GetValueOrDefault().ToString()
                    };

                    IList<usp_GetTaxOfficeTargetList_Result> lstTaxOfficeTarget = new BLTaxOffice().BL_GetTaxOfficeTarget(new MAP_TaxOffice_Target() { TaxOfficeID = toid.GetValueOrDefault(), TaxYear = year });
                    ViewBag.TaxOfficeTargetList = lstTaxOfficeTarget;

                    return View(mObjTargetModel);
                }
                else
                {
                    return RedirectToAction("TaxOfficeTarget", "OperationManager");
                }
            }
            else
            {
                return RedirectToAction("TaxOfficeTarget", "OperationManager");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult SetTaxOfficeTarget(TaxOfficeTargetViewModel pObjTargetModel, FormCollection pObjFormCollection)
        {
            //Start Savings
            IList<usp_GetTaxOfficeTargetList_Result> lstTaxOfficeTarget = new BLTaxOffice().BL_GetTaxOfficeTarget(new MAP_TaxOffice_Target() { TaxOfficeID = pObjTargetModel.TaxOfficeID, TaxYear = pObjTargetModel.TaxYearID });
            IList<MAP_TaxOffice_Target> lstTarget = new List<MAP_TaxOffice_Target>();
            MAP_TaxOffice_Target mObjTarget;
            foreach (var item in lstTaxOfficeTarget)
            {
                mObjTarget = new MAP_TaxOffice_Target()
                {
                    TaxOfficeID = pObjTargetModel.TaxOfficeID,
                    TaxYear = pObjTargetModel.TaxYearID,
                    TOTID = TrynParse.parseInt(pObjFormCollection.Get("hdnRSTOTID_" + item.RevenueStreamID)),
                    RevenueStreamID = item.RevenueStreamID,
                    TargetAmount = TrynParse.parseDecimal(pObjFormCollection.Get("txtRSAmount_" + item.RevenueStreamID)),
                    CreatedBy = SessionManager.UserID,
                    CreatedDate = CommUtil.GetCurrentDateTime(),
                    ModifiedBy = SessionManager.UserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime(),
                };

                lstTarget.Add(mObjTarget);

                item.TargetAmount = TrynParse.parseDecimal(pObjFormCollection.Get("txtRSAmount_" + item.RevenueStreamID));
            }

            FuncResponse mObjFuncResponse = new BLTaxOffice().BL_InsertUpdateTaxOfficeTarget(lstTarget);

            if (mObjFuncResponse.Success)
            {
                FlashMessage.Info(mObjFuncResponse.Message);
                return RedirectToAction("TaxOfficeTarget", "OperationManager");
            }
            else
            {
                ViewBag.TaxOfficeTargetList = lstTaxOfficeTarget;
                return View(pObjTargetModel);
            }

        }

        public ActionResult TaxOfficerTarget(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                usp_GetTaxOfficeList_Result mObjTaxOfficeData = new BLTaxOffice().BL_GetTaxOfficeDetails(new Tax_Offices() { intStatus = 1, TaxOfficeID = id.GetValueOrDefault() });

                if (mObjTaxOfficeData != null)
                {
                    UI_FillYearDropDown();
                    ViewBag.TaxOfficeData = mObjTaxOfficeData;

                    IList<usp_GetUserList_Result> lstTaxOfficer = new BLUser().BL_GetUserList(new MST_Users() { TaxOfficeID = id.GetValueOrDefault(), intStatus = 1 });

                    return View(lstTaxOfficer);
                }
                else
                {
                    return RedirectToAction("TaxOfficeTarget", "OperationManager");
                }
            }
            else
            {
                return RedirectToAction("TaxOfficeTarget", "OperationManager");
            }
        }

        public ActionResult SetTaxOfficerTarget(int? toid, int? tofid, int? year)
        {
            if (toid.GetValueOrDefault() > 0 && year.GetValueOrDefault() > 0)
            {
                usp_GetTaxOfficeList_Result mObjTaxOfficeData = new BLTaxOffice().BL_GetTaxOfficeDetails(new Tax_Offices() { intStatus = 1, TaxOfficeID = toid.GetValueOrDefault() });

                if (mObjTaxOfficeData != null)
                {
                    usp_GetUserList_Result mObjUserData = new BLUser().BL_GetUserDetails(new MST_Users() { intStatus = 1, UserID = tofid.GetValueOrDefault() });
                    TaxOfficerTargetViewModel mObjTargetModel = new TaxOfficerTargetViewModel()
                    {
                        TaxOfficeID = toid.GetValueOrDefault(),
                        TaxOfficeName = mObjTaxOfficeData.TaxOfficeName,
                        TaxOfficerID = mObjUserData.UserID.GetValueOrDefault(),
                        TaxOfficerName = mObjUserData.ContactName,
                        TaxYearID = year.GetValueOrDefault(),
                        TaxYearName = year.GetValueOrDefault().ToString()
                    };

                    IList<usp_GetTaxOfficerTargetList_Result> lstTaxOfficerTarget = new BLUser().BL_GetTaxOfficerTarget(new MAP_TaxOfficer_Target() { TaxOfficeID = toid.GetValueOrDefault(), TaxOfficerID = tofid.GetValueOrDefault(), TaxYear = year });
                    ViewBag.TaxOfficerTargetList = lstTaxOfficerTarget;

                    return View(mObjTargetModel);
                }
                else
                {
                    return RedirectToAction("TaxOfficeTarget", "OperationManager");
                }
            }
            else
            {
                return RedirectToAction("TaxOfficeTarget", "OperationManager");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult SetTaxOfficerTarget(TaxOfficerTargetViewModel pObjTargetModel, FormCollection pObjFormCollection)
        {
            //Start Savings
            IList<usp_GetTaxOfficerTargetList_Result> lstTaxOfficerTarget = new BLUser().BL_GetTaxOfficerTarget(new MAP_TaxOfficer_Target() { TaxOfficeID = pObjTargetModel.TaxOfficeID, TaxOfficerID = pObjTargetModel.TaxOfficerID, TaxYear = pObjTargetModel.TaxYearID });
            IList<MAP_TaxOfficer_Target> lstTarget = new List<MAP_TaxOfficer_Target>();
            MAP_TaxOfficer_Target mObjTarget;
            foreach (var item in lstTaxOfficerTarget)
            {
                mObjTarget = new MAP_TaxOfficer_Target()
                {
                    TaxOfficeID = pObjTargetModel.TaxOfficeID,
                    TaxOfficerID = pObjTargetModel.TaxOfficerID,
                    TaxYear = pObjTargetModel.TaxYearID,
                    TOTID = TrynParse.parseInt(pObjFormCollection.Get("hdnRSTOTID_" + item.RevenueStreamID)),
                    RevenueStreamID = item.RevenueStreamID,
                    TargetAmount = TrynParse.parseDecimal(pObjFormCollection.Get("txtRSAmount_" + item.RevenueStreamID)),
                    CreatedBy = SessionManager.UserID,
                    CreatedDate = CommUtil.GetCurrentDateTime(),
                    ModifiedBy = SessionManager.UserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime(),
                };

                lstTarget.Add(mObjTarget);

                item.TargetAmount = TrynParse.parseDecimal(pObjFormCollection.Get("txtRSAmount_" + item.RevenueStreamID));
            }

            FuncResponse mObjFuncResponse = new BLUser().BL_InsertUpdateTaxOfficerTarget(lstTarget);

            if (mObjFuncResponse.Success)
            {
                FlashMessage.Info(mObjFuncResponse.Message);
                return RedirectToAction("TaxOfficerTarget", "OperationManager", new { id = pObjTargetModel.TaxOfficeID, name = pObjTargetModel.TaxOfficeName.ToSeoUrl() });
            }
            else
            {
                ViewBag.TaxOfficerTargetList = lstTaxOfficerTarget;
                return View(pObjTargetModel);
            }

        }

        public ActionResult MonthlyTaxOfficeTarget()
        {
            UI_FillTaxOfficeDropDown();
            UI_FillYearDropDown();
            return View();
        }
        public JsonResult MonthlyTaxOfficeTargetLoadData(int TaxOfficeID, int Year)
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? Convert.ToInt32(vLength) : 0;
            int IntSkip = vStart != null ? Convert.ToInt32(vStart) : 0;
            int IntTotalRecords = 0;


            IList<usp_RPT_MonthlyTaxOfficeTarget_Result> lstSummary = new BLOperationManager().BL_GetMonthlyTaxOfficeTarget(TaxOfficeID, Year);
            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstSummary = lstSummary.Where(t =>
                t.TargetAmount != null && t.TargetAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.AssessedAmount != null && t.AssessedAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.RevenueAmount != null && t.RevenueAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.StartMonthName != null && t.StartMonthName.ToLower().Trim().Contains(vFilter.ToLower().Trim())).ToList();
            }


            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstSummary = lstSummary.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstSummary.Count();
            var data = lstSummary.Skip(IntSkip).Take(IntPageSize).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReviseBill()
        {
            string url = getUrl();
            bool itCan = new UtilityController().CheckAccess(url);
            if (!itCan) { return RedirectToAction("AccessDenied", "Utility"); }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult ReviseBill(ReviseBillViewModel pObjReviseBillModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjReviseBillModel);
            }
            else
            {
                if (pObjReviseBillModel.BillRefNo.StartsWith("AB"))
                {
                    BLAssessment mObjBLAssessment = new BLAssessment();
                    usp_GetAssessmentList_Result mObjAssessmentData = mObjBLAssessment.BL_GetAssessmentDetails(new Assessment() { AssessmentRefNo = pObjReviseBillModel.BillRefNo, IntStatus = 2 });

                    if (mObjAssessmentData != null)
                    {
                        return RedirectToAction("Assessment", "Adjustment", new { id = mObjAssessmentData.AssessmentID, name = mObjAssessmentData.AssessmentRefNo.ToSeoUrl() });
                    }
                    else
                    {
                        ViewBag.Message = "Invalid Bill Ref No";
                        return View(pObjReviseBillModel);
                    }

                }
                else if (pObjReviseBillModel.BillRefNo.StartsWith("SB"))
                {
                    BLServiceBill mObjBLServiceBill = new BLServiceBill();
                    usp_GetServiceBillList_Result mObjServiceBillData = mObjBLServiceBill.BL_GetServiceBillDetails(new ServiceBill() { ServiceBillRefNo = pObjReviseBillModel.BillRefNo, IntStatus = 2 });

                    if (mObjServiceBillData != null)
                    {
                        return RedirectToAction("ServiceBill", "Adjustment", new { id = mObjServiceBillData.ServiceBillID, name = mObjServiceBillData.ServiceBillRefNo.ToSeoUrl() });
                    }
                    else
                    {
                        ViewBag.Message = "Invalid Bill Ref No";
                        return View(pObjReviseBillModel);
                    }
                }
                else
                {
                    ViewBag.Message = "Invalid Bill Ref No";
                    return View(pObjReviseBillModel);
                }
            }
        }
        public ActionResult TCCReports(int taxYear, int status, int txoffId)
        {
            var fullUrl = Request.Url.ToString();
            UI_FillYearDropDown();
            UI_FillTCCStatusDropDown();

            var finalRes = new List<TaxReportReturn>();
            var rec = SessionManager.LstTaxReportModel ?? new List<TaxReportModel>();
            if (rec.Count() <= 1)
            {
                rec = GetReport();
            }
            if (taxYear == 0 && status == 0 && txoffId != 0)
            {
                string body = SessionManager.detHolder.ToString();
                if (!String.IsNullOrEmpty(body))
                {
                    var ye = body.Split('@');
                    var cc = ye[0].Replace('"', ' ');
                    var dd = ye[1].Replace('"', ' ');
                    int yr = cc == "" ? 0 : Convert.ToInt32(cc);
                    int st = dd == "" ? 0 : Convert.ToInt32(dd);
                    if (yr != 0)
                    {
                        rec = rec.Where(o => o.TaxOfficeId == txoffId && o.TaxYear == yr).ToList();
                    }
                    else
                    {
                        rec = rec.Where(o => o.TaxOfficeId == txoffId).ToList();
                    }

                    switch (st)
                    {
                        case 1:
                            rec = rec.Where(o => o.StatusId != 14).ToList();
                            break;
                        case 2:
                            rec = rec.Where(o => o.IsDownload == true).ToList();
                            break;
                        case 3:

                            rec = rec.Where(o => o.StatusId == 14).ToList();
                            break;

                        default:
                            break;
                    }
                }
                else
                {
                    rec = rec.Where(o => o.TaxOfficeId == txoffId).ToList();
                }
                ViewBag.Taxxx = rec;
                ViewBag.Det = 2;
            }
            else
            {
                if (taxYear == 0)
                {
                    if (status != 0)
                    {
                        switch (status)
                        {
                            case 1:
                                rec = rec.Where(o => o.StatusId != 14).ToList();
                                break;
                            case 2:
                                rec = rec.Where(o => o.IsDownload == true).ToList();
                                break;
                            case 3:

                                rec = rec.Where(o => o.StatusId == 14).ToList();
                                break;

                            default:
                                break;
                        }

                    }
                    finalRes = rec.GroupBy(o => o.TaxOfficeId)
                          .Select(g => new TaxReportReturn
                          {
                              TaxOfficeId = g.Key,
                              TaxOffice = g.First().TaxOffice,
                              TotalRequest = g.Count().ToString()
                          }).ToList();

                    ViewBag.Det = 1;
                    SessionManager.detHolder = $"{taxYear}@{status}";
                }
                else
                {
                    rec = SessionManager.LstTaxReportModel.Where(o => o.TaxYear == taxYear).ToList();
                    switch (status)
                    {
                        case 1:
                            rec = rec.Where(o => o.StatusId != 14).ToList();
                            break;
                        case 2:
                            rec = rec.Where(o => o.IsDownload == true).ToList();
                            break;
                        case 3:

                            rec = rec.Where(o => o.StatusId == 14).ToList();
                            break;

                        default:
                            break;
                    }
                    finalRes = rec.GroupBy(o => o.TaxOfficeId)
                     .Select(g => new TaxReportReturn
                     {
                         TaxOfficeId = g.Key,
                         TaxOffice = g.First().TaxOffice,
                         TotalRequest = g.Count().ToString()
                     }).ToList();

                    ViewBag.Det = 1;

                    SessionManager.detHolder = $"{taxYear}@{status}";
                }
            }
            return View(finalRes);
        }

        private List<TaxReportModel> GetReport()
        {
            var rec = (from d in _db.TCC_Request
                       join t in _db.Tax_Offices
                       on d.TaxOfficeId equals t.TaxOfficeID
                       join i in _db.Individuals
                       on d.TaxPayerID equals i.IndividualID
                       join s in _db.MST_TCCRequestStatus
                       on d.StatusID equals s.StatusID
                       select new TaxReportModel
                       {
                           TaxOfficeId = d.TaxOfficeId,
                           TaxOffice = t.TaxOfficeName,
                           Taxpayername = i.FirstName + " " + i.LastName,
                           TaxpayerId = d.TaxPayerID,
                           StatusId = d.StatusID,
                           IsDownload = d.IsDownloaded,
                           TaxYear = d.TaxYear,
                           RequestDate = d.RequestDate,
                           StatusName = s.StatusName,
                           RequestRef = d.RequestRefNo
                       }
                     ).AsNoTracking().ToList();


            SessionManager.LstTaxReportModel = rec;
            return rec;
        }

        public ActionResult ManageLateCharge()
        {
            //string url = getUrl();
            //bool itCan = new UtilityController().CheckAccess(url);
            //if (!itCan) { return RedirectToAction("AccessDenied", "Utility"); }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult ManageLateCharge(ReviseBillViewModel pObjReviseBillModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjReviseBillModel);
            }
            else
            {
                if (pObjReviseBillModel.BillRefNo.StartsWith("AB"))
                {
                    BLAssessment mObjBLAssessment = new BLAssessment();
                    usp_GetAssessmentList_Result mObjAssessmentData = mObjBLAssessment.BL_GetAssessmentDetails(new Assessment() { AssessmentRefNo = pObjReviseBillModel.BillRefNo, IntStatus = 2 });

                    if (mObjAssessmentData != null)
                    {
                        return RedirectToAction("AssessmentLateCharge", "Adjustment", new { id = mObjAssessmentData.AssessmentID, name = mObjAssessmentData.AssessmentRefNo.ToSeoUrl() });
                    }
                    else
                    {
                        ViewBag.Message = "Invalid Bill Ref No";
                        return View(pObjReviseBillModel);
                    }

                }
                else if (pObjReviseBillModel.BillRefNo.StartsWith("SB"))
                {
                    BLServiceBill mObjBLServiceBill = new BLServiceBill();
                    usp_GetServiceBillList_Result mObjServiceBillData = mObjBLServiceBill.BL_GetServiceBillDetails(new ServiceBill() { ServiceBillRefNo = pObjReviseBillModel.BillRefNo, IntStatus = 2 });

                    if (mObjServiceBillData != null)
                    {
                        return RedirectToAction("ServiceBill", "Adjustment", new { id = mObjServiceBillData.ServiceBillID, name = mObjServiceBillData.ServiceBillRefNo.ToSeoUrl() });
                    }
                    else
                    {
                        ViewBag.Message = "Invalid Bill Ref No";
                        return View(pObjReviseBillModel);
                    }
                }
                else
                {
                    ViewBag.Message = "Invalid Bill Ref No";
                    return View(pObjReviseBillModel);
                }
            }
        }

        //
        public ActionResult IndividualLiabilityStatus()
        {
            return View();
        }
        /// <summary>
        /// This Function Are Used For Search Filter to search particular data in that table
        /// </summary>
        /// <param name="p_ObjFormCollection"></param>
        /// <returns></returns>
        public JsonResult IndividualLiabilityStatusLoadData()
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? Convert.ToInt32(vLength) : 0;
            int IntSkip = vStart != null ? Convert.ToInt32(vStart) : 0;
            int IntTotalRecords = 0;


            IList<usp_RPT_InvidualLiabilityStatus_Result> lstIndividualLiabilityStatus = new BLOperationManager().BL_GetIndividualLiabilityStatus();
            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstIndividualLiabilityStatus = lstIndividualLiabilityStatus.Where(t =>
                t.TaxPayerRIN != null && t.TaxPayerRIN.Trim().Contains(vFilter.Trim()) ||
                t.TaxPayerName != null && t.TaxPayerName.Trim().Contains(vFilter.Trim()) ||
                t.MobileNumber != null && t.MobileNumber.Trim().Contains(vFilter.Trim()) ||
                t.Balance != null && t.Balance.Value.ToString().Trim().Contains(vFilter.Trim())).ToList();
            }


            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstIndividualLiabilityStatus = lstIndividualLiabilityStatus.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstIndividualLiabilityStatus.Count();
            var data = lstIndividualLiabilityStatus.Skip(IntSkip).Take(IntPageSize).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult IndividualLiabilityStatusExportToExcel()
        {
            string strTableName = "Income Tax Liability Report";

            IList<usp_RPT_InvidualLiabilityStatus_Result> lstIndividualLiabilityStatus = new BLOperationManager().BL_GetIndividualLiabilityStatus();
            DataTable dt = CommUtil.ConvertToDataTable(lstIndividualLiabilityStatus);

            var ObjExcelData = CommUtil.ConvertDataTableToExcel(dt);
            return File(ObjExcelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "IncomeTaxLiabilityReport_" + DateTime.Now.ToString("dd_MM_yy") + ".xlsx");

        }

        public ActionResult IndividualLiabilityDetails(int? id)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Individual mObjIndividual = new Individual()
                {
                    IndividualID = id.GetValueOrDefault(),
                    intStatus = 1
                };

                usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(mObjIndividual);

                if (mObjIndividualData != null)
                {
                    MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                    {
                        TaxPayerID = id.GetValueOrDefault(),
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual
                    };

                    IList<usp_GetAssessmentRuleInformation_Result> lstAssessmentRuleInformation = new BLTaxPayerAsset().BL_GetTaxPayerAssessmentRuleInformation((int)EnumList.TaxPayerType.Individual, id.GetValueOrDefault());
                    ViewBag.AssessmentRuleInformation = lstAssessmentRuleInformation;

                    IList<usp_GetTaxPayerBill_Result> lstTaxPayerBill = new BLAssessment().BL_GetTaxPayerBill(id.GetValueOrDefault(), (int)EnumList.TaxPayerType.Individual, 0);
                    ViewBag.TaxPayerBill = lstTaxPayerBill;

                    IList<usp_GetTaxPayerPayment_Result> lstTaxPayerPayment = new BLSettlement().BL_GetTaxPayerPayment(id.GetValueOrDefault(), (int)EnumList.TaxPayerType.Individual);
                    ViewBag.TaxPayerPayment = lstTaxPayerPayment;

                    IList<usp_GetProfileInformation_Result> lstProfileInformation = new BLTaxPayerAsset().BL_GetTaxPayerProfileInformation((int)EnumList.TaxPayerType.Individual, id.GetValueOrDefault());
                    ViewBag.ProfileInformation = lstProfileInformation;


                    return View(mObjIndividualData);
                }
                else
                {
                    return RedirectToAction("IndividualLiabilityStatus", "OperationManager");
                }
            }
            else
            {
                return RedirectToAction("IndividualLiabilityStatus", "OperationManager");
            }
        }

        public ActionResult ProcessTCCRequest()
        {
            return View();
        }

        public ActionResult ExportProfileData()
        {
            UI_FillProfileDropDown();
            UI_FillYearDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExportProfileData(ExportProfileDataViewModel pObjExportProfileModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillProfileDropDown();
                UI_FillYearDropDown();
                return View(pObjExportProfileModel);
            }
            else
            {
                string strProfileIds = string.Join(",", pObjExportProfileModel.ProfileID);

                IList<usp_GetTaxPayerProfileForExport_Result> lstData = new BLOperationManager().BL_GetTaxPayerProfileForExport(strProfileIds, pObjExportProfileModel.Year);
                string method = "TaxPayerAssetData_";

                byte[] ObjExcelData = CommUtil.ToExcel(lstData, $"{method}");
                return File(ObjExcelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{method}" + DateTime.Now.ToString("dd_MM_yy") + ".xlsx");

            }
        }

        [HttpGet]
        public ActionResult ExportProfileGroupingData()
        {
            UI_FillProfileTypeDropDown();
            UI_FillYearDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExportProfileGroupingData(ExportProfileGroupDataViewModel pObjExportProfileModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillProfileTypeDropDown();
                UI_FillYearDropDown();
                return View(pObjExportProfileModel);
            }
            else
            {
                IList<usp_GetTaxPayerProfileTypeForExport_Result> lstData = new BLOperationManager().BL_GetTaxPayerProfileTypeForExport(pObjExportProfileModel.ProfileTypeID, pObjExportProfileModel.Year);


                string method = "TaxPayerAssetData_";

                byte[] ObjExcelData = CommUtil.ToExcel(lstData, $"{method}");
                return File(ObjExcelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{method}" + DateTime.Now.ToString("dd_MM_yy") + ".xlsx");


            }
        }

        public ActionResult ReplaceTaxOfficeManager()
        {
            UI_FillTaxOfficeDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReplaceTaxOfficeManager(ReplaceTaxOfficeManagerViewModel pObjReplaceTaxOfficeManagerModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillTaxOfficeDropDown();
                return View(pObjReplaceTaxOfficeManagerModel);
            }
            else
            {
                MST_Users mObjUser = new MST_Users()
                {
                    TOManagerID = pObjReplaceTaxOfficeManagerModel.TaxOfficeManagerID,
                    ReplacementID = pObjReplaceTaxOfficeManagerModel.ReplacementManagerID,
                    ModifiedBy = SessionManager.UserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                FuncResponse mObjFuncResponse = new BLUser().BL_ReplaceTaxOfficeManager(mObjUser);

                if (mObjFuncResponse.Success)
                {
                    FlashMessage.Info("Manager Replaced Successfully");
                    UI_FillTaxOfficeDropDown();
                    ModelState.Clear();
                    return View();
                }
                else
                {
                    ViewBag.Message = "Error Occurred will replacing manager";

                    UI_FillTaxOfficeDropDown();
                    return View(pObjReplaceTaxOfficeManagerModel);
                }
            }
        }

        public ActionResult ReplaceTaxOfficer()
        {
            UI_FillTaxOfficeDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReplaceTaxOfficer(ReplaceTaxOfficerViewModel pObjReplaceTaxOfficerModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillTaxOfficeDropDown();
                return View(pObjReplaceTaxOfficerModel);
            }
            else
            {
                MST_Users mObjUser = new MST_Users()
                {
                    UserID = pObjReplaceTaxOfficerModel.TaxOfficerID,
                    ReplacementID = pObjReplaceTaxOfficerModel.ReplacementID,
                    ModifiedBy = SessionManager.UserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                FuncResponse mObjFuncResponse = new BLUser().BL_ReplaceTaxOfficer(mObjUser);

                if (mObjFuncResponse.Success)
                {
                    FlashMessage.Info("Tax Officer Replaced Successfully");
                    UI_FillTaxOfficeDropDown();
                    ModelState.Clear();
                    return View();
                }
                else
                {
                    ViewBag.Message = "Error Occurred will replacing tax officer";

                    UI_FillTaxOfficeDropDown();
                    return View(pObjReplaceTaxOfficerModel);
                }
            }
        }

        public ActionResult ReallocateTaxPayerToTaxOfficer()
        {
            UI_FillTaxPayerTypeDropDown();
            UI_FillTaxOfficeDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReallocateTaxPayerToTaxOfficer(ReallocateTaxPayerToTaxOfficerViewModel pObjReallocateTaxPayerToTaxOfficerModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillTaxPayerTypeDropDown();
                UI_FillTaxOfficeDropDown();
                return View(pObjReallocateTaxPayerToTaxOfficerModel);
            }
            else
            {
                if (pObjReallocateTaxPayerToTaxOfficerModel.TaxPayerTypeID == (int)EnumList.TaxPayerType.Individual)
                {
                    Individual mObjIndividual = new Individual()
                    {
                        IndividualID = pObjReallocateTaxPayerToTaxOfficerModel.TaxPayerID,
                        TaxOfficerID = pObjReallocateTaxPayerToTaxOfficerModel.TaxOfficerID,
                        ModifiedBy = SessionManager.UserID,
                        ModifiedDate = CommUtil.GetCurrentDateTime()
                    };

                    FuncResponse mObjFuncResponse = new BLIndividual().BL_UpdateTaxOfficer(mObjIndividual);

                    if (mObjFuncResponse.Success)
                    {
                        FlashMessage.Info("Realloacted Tax Payer to Tax Officer successfully");
                        UI_FillTaxPayerTypeDropDown();
                        UI_FillTaxOfficeDropDown();
                        ModelState.Clear();
                        return View();
                    }
                    else
                    {
                        ViewBag.Message = "Error Occurred will updating tax officer";
                        UI_FillTaxPayerTypeDropDown();
                        UI_FillTaxOfficeDropDown();
                        return View(pObjReallocateTaxPayerToTaxOfficerModel);
                    }

                }
                else if (pObjReallocateTaxPayerToTaxOfficerModel.TaxPayerTypeID == (int)EnumList.TaxPayerType.Companies)
                {
                    Company mObjCompany = new Company()
                    {
                        CompanyID = pObjReallocateTaxPayerToTaxOfficerModel.TaxPayerID,
                        TaxOfficerID = pObjReallocateTaxPayerToTaxOfficerModel.TaxOfficerID,
                        ModifiedBy = SessionManager.UserID,
                        ModifiedDate = CommUtil.GetCurrentDateTime()
                    };

                    FuncResponse mObjFuncResponse = new BLCompany().BL_UpdateTaxOfficer(mObjCompany);

                    if (mObjFuncResponse.Success)
                    {
                        FlashMessage.Info("Realloacted Tax Payer to Tax Officer successfully");
                        UI_FillTaxPayerTypeDropDown();
                        UI_FillTaxOfficeDropDown();
                        ModelState.Clear();
                        return View();
                    }
                    else
                    {
                        ViewBag.Message = "Error Occurred will updating tax officer";
                        UI_FillTaxPayerTypeDropDown();
                        UI_FillTaxOfficeDropDown();
                        return View(pObjReallocateTaxPayerToTaxOfficerModel);
                    }
                }
                else if (pObjReallocateTaxPayerToTaxOfficerModel.TaxPayerTypeID == (int)EnumList.TaxPayerType.Government)
                {
                    Government mObjGovernment = new Government()
                    {
                        GovernmentID = pObjReallocateTaxPayerToTaxOfficerModel.TaxPayerID,
                        TaxOfficerID = pObjReallocateTaxPayerToTaxOfficerModel.TaxOfficerID,
                        ModifiedBy = SessionManager.UserID,
                        ModifiedDate = CommUtil.GetCurrentDateTime()
                    };

                    FuncResponse mObjFuncResponse = new BLGovernment().BL_UpdateTaxOfficer(mObjGovernment);

                    if (mObjFuncResponse.Success)
                    {
                        FlashMessage.Info("Realloacted Tax Payer to Tax Officer successfully");
                        UI_FillTaxPayerTypeDropDown();
                        UI_FillTaxOfficeDropDown();
                        ModelState.Clear();
                        return View();
                    }
                    else
                    {
                        ViewBag.Message = "Error Occurred will updating tax officer";
                        UI_FillTaxPayerTypeDropDown();
                        UI_FillTaxOfficeDropDown();
                        return View(pObjReallocateTaxPayerToTaxOfficerModel);
                    }
                }
                else if (pObjReallocateTaxPayerToTaxOfficerModel.TaxPayerTypeID == (int)EnumList.TaxPayerType.Special)
                {
                    Special mObjSpecial = new Special()
                    {
                        SpecialID = pObjReallocateTaxPayerToTaxOfficerModel.TaxPayerID,
                        TaxOfficerID = pObjReallocateTaxPayerToTaxOfficerModel.TaxOfficerID,
                        ModifiedBy = SessionManager.UserID,
                        ModifiedDate = CommUtil.GetCurrentDateTime()
                    };

                    FuncResponse mObjFuncResponse = new BLSpecial().BL_UpdateTaxOfficer(mObjSpecial);

                    if (mObjFuncResponse.Success)
                    {
                        FlashMessage.Info("Realloacted Tax Payer to Tax Officer successfully");
                        UI_FillTaxPayerTypeDropDown();
                        UI_FillTaxOfficeDropDown();
                        ModelState.Clear();
                        return View();
                    }
                    else
                    {
                        ViewBag.Message = "Error Occurred will updating tax officer";
                        UI_FillTaxPayerTypeDropDown();
                        UI_FillTaxOfficeDropDown();
                        return View(pObjReallocateTaxPayerToTaxOfficerModel);
                    }
                }
                else
                {
                    ViewBag.Message = "Invalid Tax Payer Type";
                    UI_FillTaxPayerTypeDropDown();
                    UI_FillTaxOfficeDropDown();
                    return View(pObjReallocateTaxPayerToTaxOfficerModel);
                }
            }
        }

        public ActionResult TaxPayerTypeByTaxOffice()
        {
            IList<usp_RPT_GetTaxPayerTypeByTaxOffice_Result> lstData = new BLOperationManager().BL_GetTaxPayerTypeByTaxOffice();
            return View(lstData);
        }

        public ActionResult TaxPayerTypeByTaxOfficeExportToExcel()
        {
            IList<usp_RPT_GetTaxPayerTypeByTaxOffice_Result> lstData = new BLOperationManager().BL_GetTaxPayerTypeByTaxOffice();

            string[] strColumns = new string[] { "TaxOfficeName", "IndividualCount", "CorporateCount", "GovernmentCount", "SpecialCount", "TotalCount" };
            string[] strTotalColumns = new string[] { "IndividualCount", "CorporateCount", "GovernmentCount", "SpecialCount", "TotalCount" };
            //var vMemberInfoData = typeof(usp_RPT_GetTaxPayerTypeByTaxOffice_Result)
            //        .GetProperties()
            //        .Where(pi => strColumns.Contains(pi.Name))
            //        .Select(pi => (MemberInfo)pi)
            //        .ToArray();

            string method = "TaxPayerTypeByTaxOffice_";

            byte[] ObjExcelData = CommUtil.ToExcel(lstData, $"{method}");
            return File(ObjExcelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{method}" + DateTime.Now.ToString("dd_MM_yy") + ".xlsx");


        }

        public ActionResult TaxPayerTypeByTaxOfficeDetail(int tofid)
        {
            ViewBag.TaxOfficeID = tofid;
            return View();
        }

        public JsonResult TaxPayerTypeByTaxOfficeDetailLoadData(int TaxOfficeID)
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? Convert.ToInt32(vLength) : 0;
            int IntSkip = vStart != null ? Convert.ToInt32(vStart) : 0;
            int IntTotalRecords = 0;


            IList<usp_SearchTaxPayer_Result> lstTaxPayerDetails = new BLTaxPayerAsset().BL_SearchTaxPayer(new SearchTaxPayerFilter()
            {
                TaxOfficeID = TaxOfficeID,
                intSearchType = 7
            });
            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstTaxPayerDetails = lstTaxPayerDetails.Where(t =>
                t.TaxPayerName != null && t.TaxPayerName.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TaxPayerRIN != null && t.TaxPayerRIN.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TaxPayerMobileNumber != null && t.TaxPayerMobileNumber.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TaxPayerAddress != null && t.TaxPayerAddress.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TaxPayerTypeName != null && t.TaxPayerTypeName.ToLower().Trim().Contains(vFilter.ToLower().Trim())).ToList();
            }


            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstTaxPayerDetails = lstTaxPayerDetails.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstTaxPayerDetails.Count();
            var data = lstTaxPayerDetails.Skip(IntSkip).Take(IntPageSize).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TaxPayerTypeByTaxOfficeDetailExportToExcel(int TaxOfficeID)
        {

            IList<usp_GetTaxPayerBasedOnTaxOfficeForExport_Result> lstTaxPayerData = new BLOperationManager().BL_GetTaxPayerBasedOnTaxOfficeForExport(TaxOfficeID);

            string method = "TaxPayerTypeByTaxOfficeDetail_";

            byte[] ObjExcelData = CommUtil.ToExcel(lstTaxPayerData, $"{method}");
            return File(ObjExcelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{method}" + DateTime.Now.ToString("dd_MM_yy") + ".xlsx");
        }

        public ActionResult AssetTypeByTaxOffice()
        {
            IList<usp_RPT_GetAssetTypeByTaxOffice_Result> lstData = new BLOperationManager().BL_GetAssetTypeByTaxOffice();
            return View(lstData);
        }

        public ActionResult AssetTypeByTaxOfficeExportToExcel()
        {
            IList<usp_RPT_GetAssetTypeByTaxOffice_Result> lstData = new BLOperationManager().BL_GetAssetTypeByTaxOffice();

            string[] strColumns = new string[] { "TaxOfficeName", "BusinessCount", "BuildingCount", "LandCount", "VehicleCount", "TotalCount" };
            string[] strTotalColumns = new string[] { "BusinessCount", "BuildingCount", "LandCount", "VehicleCount", "TotalCount" };

            string method = "AssetTypeByTaxOffice_";

            byte[] ObjExcelData = CommUtil.ToExcel(lstData, $"{method}");
            return File(ObjExcelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{method}" + DateTime.Now.ToString("dd_MM_yy") + ".xlsx");

        }

        public ActionResult AssetTypeByTaxOfficeDetail(int tofid)
        {
            ViewBag.TaxOfficeID = tofid;
            return View();
        }

        public JsonResult AssetTypeByTaxOfficeDetailLoadData(int TaxOfficeID)
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? Convert.ToInt32(vLength) : 0;
            int IntSkip = vStart != null ? Convert.ToInt32(vStart) : 0;
            int IntTotalRecords = 0;


            IList<usp_RPT_GetAssetDetailByTaxOffice_Result> lstAssetDetails = new BLOperationManager().BL_GetAssetDetailByTaxOffice(TaxOfficeID);
            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstAssetDetails = lstAssetDetails.Where(t =>
                t.AssetName != null && t.AssetName.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.AssetRIN != null && t.AssetRIN.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.AssetLGA != null && t.AssetLGA.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.AssetTypeName != null && t.AssetTypeName.ToLower().Trim().Contains(vFilter.ToLower().Trim())).ToList();
            }


            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstAssetDetails = lstAssetDetails.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstAssetDetails.Count();
            var data = lstAssetDetails.Skip(IntSkip).Take(IntPageSize).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AssetTypeByTaxOfficeDetailExportToExcel(int TaxOfficeID)
        {

            IList<usp_GetAssetBasedOnTaxOfficeForExport_Result> lstAssetData = new BLOperationManager().BL_GetAssetBasedOnTaxOfficeForExport(TaxOfficeID);

            string method = "AssetTypeByTaxOfficeDetail_";

            byte[] ObjExcelData = CommUtil.ToExcel(lstAssetData, $"{method}");
            return File(ObjExcelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{method}" + DateTime.Now.ToString("dd_MM_yy") + ".xlsx");
        }

        public ActionResult PaymentCharges()
        {
            return View();
        }

        public JsonResult PaymentChargesLoadData()
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? Convert.ToInt32(vLength) : 0;
            int IntSkip = vStart != null ? Convert.ToInt32(vStart) : 0;
            int IntTotalRecords = 0;


            IList<usp_GetPaymentChargeList_Result> lstPaymentCharge = new BLOperationManager().BL_GetPaymentChargeList(0, 0);
            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstPaymentCharge = lstPaymentCharge.Where(t =>
                t.TaxPayerRIN != null && t.TaxPayerRIN.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TaxPayerTypeName != null && t.TaxPayerTypeName.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TaxPayerName != null && t.TaxPayerName.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TaxYear != null && t.TaxYear.GetValueOrDefault().ToString().ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.BillRefNo != null && t.BillRefNo.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.RevenueStreamName != null && t.RevenueStreamName.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.Penalty != null && t.Penalty.GetValueOrDefault().ToString().ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.Interest != null && t.Interest.GetValueOrDefault().ToString().ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TotalCharge != null && t.TotalCharge.GetValueOrDefault().ToString().ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.ChargeDate != null && t.ChargeDate.GetValueOrDefault().ToString("dd-MMM-yyyy").ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.BillStatus != null && t.BillStatus.ToLower().Trim().Contains(vFilter.ToLower().Trim())).ToList();
            }


            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstPaymentCharge = lstPaymentCharge.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstPaymentCharge.Count();
            var data = lstPaymentCharge.Skip(IntSkip).Take(IntPageSize).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PaymentChargesExportToExcel()
        {

            IList<usp_GetPaymentChargeList_Result> lstPaymentCharge = new BLOperationManager().BL_GetPaymentChargeList(0, 0);

            string method = "PaymentCharges_";

            byte[] ObjExcelData = CommUtil.ToExcel(lstPaymentCharge, $"{method}");
            return File(ObjExcelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{method}" + DateTime.Now.ToString("dd_MM_yy") + ".xlsx");
        }

        public ActionResult TaxPayerMonthlyPayment()
        {
            UI_FillRevenueStreamDropDown();
            UI_FillTaxPayerTypeDropDown();
            UI_FillYearDropDown();
            return View();
        }

        public JsonResult TaxPayerMonthlyPaymentLoadData(int TaxPayerTypeID, int TaxPayerID, int RevenueStreamID, int Year)
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? Convert.ToInt32(vLength) : 0;
            int IntSkip = vStart != null ? Convert.ToInt32(vStart) : 0;
            int IntTotalRecords = 0;


            IList<usp_RPT_GetTaxPayerMonthlyPayment_Result> lstSummary = new BLOperationManager().BL_GetTaxPayerMonthlyPayment(TaxPayerTypeID, TaxPayerID, RevenueStreamID, Year);
            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstSummary = lstSummary.Where(t =>
                t.StartMonthName != null && t.StartMonthName.Trim().Contains(vFilter.ToLower().Trim()) ||
                t.SettlementAmount != null && t.SettlementAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.PoAAmount != null && t.PoAAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TotalAmount != null && t.TotalAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim())).ToList();
            }

            IDictionary<string, object> dcFooterTotal = new Dictionary<string, object>();

            dcFooterTotal["PoAAmount"] = lstSummary.Sum(t => t.PoAAmount);
            dcFooterTotal["SettlementAmount"] = lstSummary.Sum(t => t.SettlementAmount);
            dcFooterTotal["TotalAmount"] = lstSummary.Sum(t => t.TotalAmount);

            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstSummary = lstSummary.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstSummary.Count();
            var data = lstSummary.Skip(IntSkip).Take(IntPageSize).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data, FooterTotal = dcFooterTotal }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TaxPayerMonthlyPaymentExportToExcel(int TaxPayerTypeID, int TaxPayerID, int RevenueStreamID, int Year)
        {
            IList<usp_RPT_GetTaxPayerMonthlyPayment_Result> lstSummary = new BLOperationManager().BL_GetTaxPayerMonthlyPayment(TaxPayerTypeID, TaxPayerID, RevenueStreamID, Year);

            string method = "TaxPayerMonthlyPayment_";

            byte[] ObjExcelData = CommUtil.ToExcel(lstSummary, $"{method}");
            return File(ObjExcelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{method}" + DateTime.Now.ToString("dd_MM_yy") + ".xlsx");

        }

        public ActionResult TaxPayerMonthlyPaymentDetail(int tptid, int tpid, int rsid, int year, int month)
        {
            ViewBag.TaxPayerTypeID = tptid;
            ViewBag.TaxPayerID = tpid;
            ViewBag.TaxYear = year;
            ViewBag.TaxMonth = month;
            ViewBag.RevenueStreamID = rsid;

            return View();

        }

        public ActionResult TaxPayerMonthlyPaymentDetailLoadData(int TaxPayerTypeID, int TaxPayerID, int RevenueStreamID, int Year, int Month)
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? Convert.ToInt32(vLength) : 0;
            int IntSkip = vStart != null ? Convert.ToInt32(vStart) : 0;
            int IntTotalRecords = 0;


            IList<usp_RPT_GetTaxPayerMonthlyPaymentDetail_Result> lstSummary = new BLOperationManager().BL_GetTaxPayerMonthlyPaymentDetail(TaxPayerTypeID, TaxPayerID, RevenueStreamID, Year, Month);
            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstSummary = lstSummary.Where(t =>
                t.PaymentDate != null && t.PaymentDate.GetValueOrDefault().ToString("dd-MMM-yyyy").Trim().Contains(vFilter.ToLower().Trim()) ||
                t.Amount != null && t.Amount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.PaymentTypeName != null && t.PaymentTypeName.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.PaymentRefNo != null && t.PaymentRefNo.ToLower().Trim().Contains(vFilter.ToLower().Trim())).ToList();
            }


            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstSummary = lstSummary.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstSummary.Count();
            var data = lstSummary.Skip(IntSkip).Take(IntPageSize).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TaxPayerMonthlyPaymentDetailExportToExcel(int TaxPayerTypeID, int TaxPayerID, int RevenueStreamID, int Year, int Month)
        {
            IList<usp_RPT_GetTaxPayerMonthlyPaymentDetail_Result> lstSummary = new BLOperationManager().BL_GetTaxPayerMonthlyPaymentDetail(TaxPayerTypeID, TaxPayerID, RevenueStreamID, Year, Month);

            string[] strColumns = new string[] { "PaymentDate", "PaymentTypeName", "PaymentRefNo", "Amount" };
            string[] strTotalColumns = new string[] { "Amount" };
            string method = "TaxPayerMonthlyPaymentDetail_";

            byte[] ObjExcelData = CommUtil.ToExcel(lstSummary, $"{method}");
            return File(ObjExcelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{method}" + DateTime.Now.ToString("dd_MM_yy") + ".xlsx");

        }

        public ActionResult TaxOfficeAssessmentSummary()
        {
            UI_FillYearDropDown();
            return View();
        }

        public JsonResult TaxOfficeAssessmentSummaryLoadData(int TaxYear)
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? Convert.ToInt32(vLength) : 0;
            int IntSkip = vStart != null ? Convert.ToInt32(vStart) : 0;
            int IntTotalRecords = 0;


            IList<usp_RPT_TaxOfficeAssessmentSummary_Result> lstSummary = new BLOperationManager().BL_GetTaxOfficeAssessmentSummary(TaxYear);
            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstSummary = lstSummary.Where(t =>
                t.TaxOfficeName != null && t.TaxOfficeName.Trim().Contains(vFilter.ToLower().Trim()) ||
                t.BillAmount != null && t.BillAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.SettlementAmount != null && t.SettlementAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.PoAAmount != null && t.PoAAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.OutstandingAmount != null && t.OutstandingAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim())).ToList();
            }


            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstSummary = lstSummary.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstSummary.Count();
            var data = lstSummary.Skip(IntSkip).Take(IntPageSize).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TaxOfficeAssessmentSummaryExportToExcel(int TaxYear)
        {
            IList<usp_RPT_TaxOfficeAssessmentSummary_Result> lstSummary = new BLOperationManager().BL_GetTaxOfficeAssessmentSummary(TaxYear);

            string method = "TaxOfficeAssessmentSummary_";

            byte[] ObjExcelData = CommUtil.ToExcel(lstSummary, $"{method}");
            return File(ObjExcelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{method}" + DateTime.Now.ToString("dd_MM_yy") + ".xlsx");

        }

        public ActionResult TaxOfficeAssessmentDetail(int tofid, int tyear)
        {
            ViewBag.TaxOfficeID = tofid;
            ViewBag.TaxYear = tyear;
            return View();

        }

        public ActionResult TaxOfficeAssessmentDetailLoadData(int TaxOfficeID, int TaxYear)
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
            var vFilter = Request.Form.GetValues("search[value]")[0];
            int IntPageSize = vLength != null ? Convert.ToInt32(vLength) : 0;
            int IntSkip = vStart != null ? Convert.ToInt32(vStart) : 0;
            int IntTotalRecords = 0;


            IList<usp_RPT_TaxOfficeAssessmentDetail_Result> lstDetails = new BLOperationManager().BL_GetTaxOfficeAssessmentDetail(TaxOfficeID, TaxYear);
            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstDetails = lstDetails.Where(t =>
                t.TaxPayerRIN != null && t.TaxPayerRIN.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TaxPayerTypeName != null && t.TaxPayerTypeName.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TaxPayerName != null && t.TaxPayerName.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.BillAmount != null && t.BillAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.SettlementAmount != null && t.SettlementAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.PoAAmount != null && t.PoAAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.OutstandingAmount != null && t.OutstandingAmount.Value.ToString().Trim().Contains(vFilter.ToLower().Trim())).ToList();
            }


            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstDetails = lstDetails.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstDetails.Count();
            var data = lstDetails.Skip(IntSkip).Take(IntPageSize).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TaxOfficeAssessmentDetailExportToExcel(int TaxOfficeID, int TaxYear)
        {
            IList<usp_RPT_TaxOfficeAssessmentDetail_Result> lstDetails = new BLOperationManager().BL_GetTaxOfficeAssessmentDetail(TaxOfficeID, TaxYear);

            string[] strColumns = new string[] { "TaxPayerRIN","TaxPayerName","TaxPayerTypeName","TaxPayerTIN","ContactNumber","ContactEmailAddress","ContactAddress",
                                                 "BillAmount","SettlementAmount","PoAAmount","OutstandingAmount" };
            string[] strTotalColumns = new string[] { "BillAmount", "SettlementAmount", "PoAAmount", "OutstandingAmount" };
            var vMemberInfoData = typeof(usp_RPT_TaxOfficeAssessmentDetail_Result)
                   .GetProperties()
                   .Where(pi => strColumns.Contains(pi.Name))
                   .Select(pi => (MemberInfo)pi)
                   .ToArray();

            string method = "TaxOfficeAssessmentDetail_";

            byte[] ObjExcelData = CommUtil.ToExcel(lstDetails, $"{method}");
            return File(ObjExcelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{method}" + DateTime.Now.ToString("dd_MM_yy") + ".xlsx");

        }

        public ActionResult GenerateTLRPDF(int tpid)
        {
            if (tpid != 0)
            {
                usp_GetTaxPayerDetails_Result mObjTaxPayerData = new BLTaxPayerAsset().BL_GetTaxPayerDetails(tpid, (int)EnumList.TaxPayerType.Individual);

                IList<usp_GetIndividualLiabilityDetail_Result> lstIndividualLiability = new BLOperationManager().BL_GetIndividualLiabilityDetail(tpid, (int)EnumList.TaxPayerType.Individual);

                string mStrDirectory = GlobalDefaultValues.DocumentLocation + "TaxLiabilityReport/";
                string mStrGeneratedFileName = "TLR" + "_" + DateTime.Now.ToString("_ddMMyyyy_") + mObjTaxPayerData.TaxPayerRIN + ".pdf";
                string mStrGeneratedDocumentPath = Path.Combine(mStrDirectory, mStrGeneratedFileName);

                if (!Directory.Exists(mStrDirectory))
                {
                    Directory.CreateDirectory(mStrDirectory);
                }

                if (System.IO.File.Exists(mStrGeneratedDocumentPath))
                {
                    System.IO.File.Delete(mStrGeneratedDocumentPath);
                }

                //// Generate QR Code
                //string mStrQRCode;
                //string mQRCodeText = "TLR No : TLR2020XX";
                //QRCodeGenerator mobjQRCodeGenerator = new QRCodeGenerator();
                //using (QRCodeData mObjQRCodeData = mobjQRCodeGenerator.CreateQrCode(mQRCodeText, QRCodeGenerator.ECCLevel.H))
                //{
                //    using (Bitmap mObjQRCodeImage = new QRCode(mObjQRCodeData).GetGraphic(16, "#000000", "#FFFFFF"))
                //    {
                //        using (MemoryStream ms = new MemoryStream())
                //        {
                //            mObjQRCodeImage.Save(ms, ImageFormat.Png);
                //            byte[] byteImage = ms.ToArray();
                //            mStrQRCode = Convert.ToBase64String(byteImage); // Get Base64
                //            ms.Dispose();
                //            mObjQRCodeImage.Dispose();
                //            mObjQRCodeData.Dispose();
                //        }
                //    }
                //}

                string sbTableBody = "";
                foreach (var item in lstIndividualLiability)
                {
                    sbTableBody += "<tr>";
                    sbTableBody += $"<td>{CommUtil.GetFormatedDate(item.BillDate)}</td>";
                    sbTableBody += $"<td>{item.BillRefNo}</td>";
                    sbTableBody += $"<td>{CommUtil.GetFormatedCurrency(item.BillAmount)}</td>";
                    sbTableBody += $"<td>{CommUtil.GetFormatedCurrency(item.SettledAmount)}</td>";
                    sbTableBody += $"<td>{CommUtil.GetFormatedCurrency(item.BillAmount.GetValueOrDefault() - item.SettledAmount.GetValueOrDefault())}</td>";
                    sbTableBody += $"<td>{CommUtil.GetFormatedCurrency(item.Penalty)}</td>";
                    sbTableBody += $"<td>{CommUtil.GetFormatedCurrency(item.Interest)}</td>";
                    sbTableBody += $"<td>{CommUtil.GetFormatedCurrency(item.TotalCharge)}</td>";
                    sbTableBody += "</tr>";
                }

                return File(mStrGeneratedDocumentPath, "application/pdf", mStrGeneratedFileName);
            }
            else
            {
                return Content("Invalid Request");
            }
        }

        [HttpGet]
        public ActionResult SFTPDataSubmission()
        {
            return View();
        }

    }
}