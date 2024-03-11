using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Web.Utility;
using iTextSharp.text.pdf;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Reflection;
using System.Text;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace EIRS.Web.Controllers
{

    public class SettleController : BaseController
    {
        
        public ActionResult List()
        {
            return View();
        }

        
        public JsonResult GetSettleList()
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            try
            {
                string mStrSearchFilter = Request.Form.GetValues("search[value]")[0];
                string mStrDraw = Request.Form.GetValues("draw")[0];
                string mstrOrderBy = Request.Form.GetValues("order[0][column]")[0];
                string mStrOrderByDir = Request.Form.GetValues("order[0][dir]")[0];
                int mIntStartRowNumber = TrynParse.parseInt(Request.Form.GetValues("start")[0]);
                int mIntPageNumber = TrynParse.parseInt(Request.Form.GetValues("length")[0]);

                // Loading. 
                Settlement mObjSettlement = new Settlement()
                {

                };

                IList<usp_GetSettlementList_Result> lstSettlement = new BLSettlement().BL_GetSettlementList(mObjSettlement);

                // Total record count.   
                int totalRecords = lstSettlement.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstSettlement = lstSettlement.Where(p => p.TaxPayerName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.ASRefNo.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.SettlementDate.GetValueOrDefault().ToString("dd-MMM-yyyy").ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.SettlementAmount.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.SettlementMethodName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.TransactionReferenceNo != null && p.TransactionReferenceNo.ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                //Purpose Sorting Data 
                if (!(string.IsNullOrEmpty(mstrOrderBy) && string.IsNullOrEmpty(mStrOrderByDir)))
                {
                    lstSettlement = lstSettlement.OrderBy(mstrOrderBy + " " + mStrOrderByDir).ToList();
                }

                // Filter record count.   
                int recFilter = lstSettlement.Count;

                // Apply pagination.   
                lstSettlement = lstSettlement.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstSettlement;

            }
            catch (Exception ex)
            {
                Logger.SendErrorToText(ex);
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult Individual()
        {
            Settlement mObjSettlement = new Settlement()
            {
                TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual
            };

            IList<usp_GetSettlementList_Result> lstSettlement = new BLSettlement().BL_GetSettlementList(mObjSettlement);
            return View(lstSettlement);
        }

        
        public ActionResult Corporate()
        {
            Settlement mObjSettlement = new Settlement()
            {
                TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual
            };

            IList<usp_GetSettlementList_Result> lstSettlement = new BLSettlement().BL_GetSettlementList(mObjSettlement);
            return View(lstSettlement);
        }

        
        public ActionResult PaymentAccount()
        {
            return View();
        }

        public JsonResult GetPoAData()
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            try
            {
                string mStrSearchFilter = Request.Form.GetValues("search[value]")[0];
                string mStrDraw = Request.Form.GetValues("draw")[0];
                string mstrOrderBy = Request.Form.GetValues("order[0][column]")[0];
                string mStrOrderByDir = Request.Form.GetValues("order[0][dir]")[0];
                int mIntStartRowNumber = TrynParse.parseInt(Request.Form.GetValues("start")[0]);
                int mIntPageNumber = TrynParse.parseInt(Request.Form.GetValues("length")[0]);

                // Loading. 
                IList<vw_PaymentAccount> lstPoAData = new BLPaymentAccount().BL_PaymentAccountList();

                // Total record count.   
                int totalRecords = lstPoAData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstPoAData = lstPoAData.Where(p => p.PaymentRefNo.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.SettlementMethodName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.Amount != null && p.Amount.ToString().ToLower().Contains(mStrSearchFilter.ToLower())) ||
                        (p.PaymentDate != null && p.PaymentDate.ToString().ToLower().Contains(mStrSearchFilter.ToLower())) ||
                        (p.TransactionRefNo != null && p.TransactionRefNo.ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstPoAData = this.SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstPoAData);

                // Filter record count.   
                int recFilter = lstPoAData.Count;

                // Apply pagination.   
                lstPoAData = lstPoAData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstPoAData;

            }
            catch (Exception ex)
            {
                Logger.SendErrorToText(ex);
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        private IList<vw_PaymentAccount> SortByColumnWithOrder(string order, string orderDir, IList<vw_PaymentAccount> data)
        {
            // Initialization.   
            IList<vw_PaymentAccount> lst = new List<vw_PaymentAccount>();
            try
            {
                // Sorting   
                switch (order)
                {
                    case "0":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PaymentRefNo).ToList() : data.OrderBy(p => p.PaymentRefNo).ToList();
                        break;
                    case "1":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TaxPayerName).ToList() : data.OrderBy(p => p.TaxPayerName).ToList();
                        break;
                    case "2":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PaymentDate).ToList() : data.OrderBy(p => p.PaymentDate).ToList();
                        break;
                    case "3":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Amount).ToList() : data.OrderBy(p => p.Amount).ToList();
                        break;
                    default:
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.SettlementMethodName).ToList() : data.OrderBy(p => p.SettlementMethodName).ToList();
                        break;
                }
            }
            catch (Exception ex)
            {
                // info.   
                Console.Write(ex);
            }
            // info.   
            return lst;
        }

        //VP_T-ERAS-9_ListAllTransaction

        /// <summary>
        /// Function Are Used to Gave List of Data In Settle Transcation
        /// </summary>
        /// <returns></returns>
        public ActionResult SettleTransactionList()
        {
            IList<SelectListItem> lstTransactionType = new List<SelectListItem>
            {
                new SelectListItem(){Value="1",Text="Settlement"},
                new SelectListItem(){Value="2",Text="Payment On Account"}
            };
            ViewBag.TransactionTypeList = lstTransactionType;
            return View();
        }

        /// <summary>
        /// vLength:using this page are maximum 10 rows shows
        /// vDraw:When you perform an action such as changing the sorting, filtering or paging characteristics of the table you will want DataTables to update the display to reflect these changes. This function is provided for that purpose
        /// vStart:Paging Start Index Of Bind Table
        /// vSortColumn:ByDefault Sorting Column
        /// vSortColumnDir:Sorting Purpose of particular Column
        /// vFilter:Purpose Of Searching
        /// IntPageSize:Minimum Size Of Data Are Stored Depending on this
        /// IntSkip:The Skip operator bypasses a specified number of contiguous rows from a sequence/table and returns the remaining table
        /// IntTotalRecords:Show Total records In Table
        /// </summary>
        [HttpPost]
        public JsonResult SettleTransactionLoadData()
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
            int IntPageSize = vLength != null ? Convert.ToInt32(vLength) : 0;
            int IntSkip = vStart != null ? Convert.ToInt32(vStart) : 0;
            int IntTotalRecords = 0;

            Settlement mObjSettlement = new Settlement()
            {
                TransactionTypeID = TrynParse.parseInt(Request.Form["TransactionTypeID"]),
                FromDate = TrynParse.parseNullableDate(Request.Form["FromDate"]),
                ToDate = TrynParse.parseNullableDate(Request.Form["ToDate"])
            };

            IList<usp_GetSettleTransactionList_Result> lstSettleTransaction = new BLSettlement().BL_GetSettleTransactionList(mObjSettlement);

            if (!string.IsNullOrEmpty(vFilter))
            {
                lstSettleTransaction = lstSettleTransaction.Where(t => t.RefNo != null && t.RefNo.ToLower().Trim().Contains(vFilter.ToLower().Trim())
                || t.TransactionDate.Value.ToString("dd-MMM-yy").ToLower().Trim().Contains(vFilter.ToLower().Trim())
                || t.SettlementMethodName != null && t.SettlementMethodName.ToLower().Trim().Contains(vFilter.ToLower().Trim())
                || t.Amount.ToString().Trim().Contains(vFilter.Trim())
                || t.TaxPayerName != null && t.TaxPayerName.ToLower().Trim().Contains(vFilter.ToLower().Trim())
                || t.TransactionReferenceNo != null && t.TransactionReferenceNo.ToLower().Trim().Contains(vFilter.ToLower().Trim())
                || t.TransactionTypeName.ToLower().Trim().Contains(vFilter.ToLower().Trim())).ToList();
            }


            //>>>Purpose Sorting Data 
            if (!(string.IsNullOrEmpty(vSortColumn) && string.IsNullOrEmpty(vSortColumnDir)))
            {
                lstSettleTransaction = lstSettleTransaction.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstSettleTransaction.Count();
            var data = lstSettleTransaction.Skip(IntSkip).Take(IntPageSize).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Receipt()
        {
            return View();
        }

        [HttpPost]
        public JsonResult LoadReceiptData()
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

            StringBuilder sbWhereCondition = new StringBuilder();

            if (!string.IsNullOrEmpty(vFilter))
            {
                sbWhereCondition.Append(" AND ( ISNULL(ReceiptRefNo,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(Convert(varchar(50),rcpt.ReceiptDate,106),'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR (ISNULL(ast.AssessmentRefNo,'') LIKE @MainFilter OR ISNULL(sb.ServiceBillRefNo,'') LIKE @MainFilter) ");
                sbWhereCondition.Append(" OR ISNULL(Convert(varchar(50),ReceiptAmount,106),'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(rs.ReceiptStatusName,'') LIKE @MainFilter )");

            }

            Treasury_Receipt mObjReceipt = new Treasury_Receipt()
            {
                WhereCondition = sbWhereCondition.ToString(),
                OrderBy = vSortColumn,
                OrderByDirection = vSortColumnDir,
                PageNumber = (TrynParse.parseInt(vStart) / TrynParse.parseInt(vLength)) + 1,
                PageSize = TrynParse.parseInt(vLength),
                MainFilter = !string.IsNullOrWhiteSpace(vFilter) ? "%" + vFilter.Trim() + "%" : vFilter

            };

            IDictionary<string, object> dcData = new BLTreasuryReceipt().BL_SearchTreasuryReceipt(mObjReceipt);
            IList<usp_SearchTreasuryReceipt_Result> lstReceipt = (IList<usp_SearchTreasuryReceipt_Result>)dcData["TreasuryReceiptList"];

            return Json(new
            {
                draw = vDraw,
                recordsFiltered = dcData["FilteredRecords"],
                recordsTotal = dcData["TotalRecords"],
                data = lstReceipt
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CancelReceipt(Treasury_Receipt pObjReceipt)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            pObjReceipt.CancelledBy = SessionManager.UserID;
            pObjReceipt.StatusID = 2;
            pObjReceipt.ModifiedDate = CommUtil.GetCurrentDateTime();
            pObjReceipt.ModifiedBy = SessionManager.UserID;

            FuncResponse mObjResponse = new BLTreasuryReceipt().BL_CancelTreasuryReceipt(pObjReceipt);

            dcResponse["success"] = mObjResponse.Success;
            dcResponse["Message"] = mObjResponse.Message;

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetReceiptNotes(int ReceiptID)
        {
            usp_GetTreasuryReceiptList_Result mObjTreasuryReceipt = new BLTreasuryReceipt().BL_GetTreasuryReceiptDetails(new Treasury_Receipt() { ReceiptID = ReceiptID });
            return Json(mObjTreasuryReceipt, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult UpdateReceiptNotes(Treasury_Receipt pObjReceipt)
        //{
        //    IDictionary<string, object> dcResponse = new Dictionary<string, object>();

        //    pObjReceipt.ModifiedDate = CommUtil.GetCurrentDateTime();
        //    pObjReceipt.ModifiedBy = SessionManager.UserID;

        //    FuncResponse mObjResponse = new BLTreasuryReceipt().BL_UpdateTreasuryReceiptNotes(pObjReceipt);

        //    dcResponse["success"] = mObjResponse.Success;
        //    dcResponse["Message"] = mObjResponse.Message;

        //    return Json(dcResponse, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult DemandNotice()
        {
            return View();
        }

        public JsonResult DemandNoticeLoadData()
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


            IList<usp_GetDemandNoticeList_Result> lstDemandNotice = new BLOperationManager().BL_GetDemandNoticeList();
            //Filtering/Searching data 
            if (!string.IsNullOrEmpty(vFilter))
            {
                lstDemandNotice = lstDemandNotice.Where(t =>
                t.TaxPayerRIN != null && t.TaxPayerRIN.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TaxPayerTypeName != null && t.TaxPayerTypeName.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TaxPayerName != null && t.TaxPayerName.ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TotalAssessed != null && t.TotalAssessed.GetValueOrDefault().ToString().ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TotalPenalty != null && t.TotalPenalty.GetValueOrDefault().ToString().ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TotalInterest != null && t.TotalInterest.GetValueOrDefault().ToString().ToLower().Trim().Contains(vFilter.ToLower().Trim()) ||
                t.TotalCharge != null && t.TotalCharge.GetValueOrDefault().ToString().ToLower().Trim().Contains(vFilter.ToLower().Trim())).ToList();
            }


            //Purpose Sorting Data 
            if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
            {
                lstDemandNotice = lstDemandNotice.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstDemandNotice.Count();
            var data = lstDemandNotice.Skip(IntSkip).Take(IntPageSize).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DemandNoticeExportToExcel()
        {

            IList<usp_GetDemandNoticeList_Result> lstDemandNotice = new BLOperationManager().BL_GetDemandNoticeList();

            //string[] strColumns = new string[] { "TaxPayerRIN", "TaxPayerTypeName", "TaxPayerName", "TotalAssessed", "TotalPenalty", "TotalInterest", "TotalCharge" };
            //string[] strTotalColumns = new string[] { "TotalAssessed", "TotalPenalty", "TotalInterest", "TotalCharge" };
            //var vMemberInfoData = typeof(usp_GetDemandNoticeList_Result)
            //        .GetProperties()
            //        .Where(pi => strColumns.Contains(pi.Name))
            //        .Select(pi => (MemberInfo)pi)
            //        .ToArray();


            DataTable dt = CommUtil.ConvertToDataTable(lstDemandNotice);
            var ObjExcelData = CommUtil.ConvertDataTableToExcel(dt);
            return File(ObjExcelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DemandNoticeCharge_" + DateTime.Now.ToString("dd_MM_yy") + ".xlsx");
        }

        public ActionResult DemandNoticeCharges(int tptid, int tpid)
        {
            ViewBag.TaxPayerTypeID = tptid;
            ViewBag.TaxPayerID = tpid;
            return View();
        }

        public JsonResult DemandNoticeChargesLoadData(int TaxPayerTypeID, int TaxPayerID)
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


            IList<usp_GetPaymentChargeList_Result> lstPaymentCharge = new BLOperationManager().BL_GetPaymentChargeList(TaxPayerID, TaxPayerTypeID);
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

        public ActionResult DemandNoticeChargesExportToExcel(int TaxPayerTypeID, int TaxPayerID)
        {

            IList<usp_GetPaymentChargeList_Result> lstPaymentCharge = new BLOperationManager().BL_GetPaymentChargeList(TaxPayerID, TaxPayerTypeID);

            //string[] strColumns = new string[] { "TaxPayerRIN", "TaxPayerTypeName", "TaxPayerName", "TaxYear", "BillRefNo", "BillDate", "RevenueStreamName", "Penalty", "Interest", "TotalCharge", "ChargeDate", "BillStatus" };
            //string[] strTotalColumns = new string[] { "Penalty", "Interest", "TotalCharge" };
            //var vMemberInfoData = typeof(usp_GetPaymentChargeList_Result)
            //        .GetProperties()
            //        .Where(pi => strColumns.Contains(pi.Name))
            //        .Select(pi => (MemberInfo)pi)
            //        .ToArray();
            DataTable dt = CommUtil.ConvertToDataTable(lstPaymentCharge);
            var ObjExcelData = CommUtil.ConvertDataTableToExcel(dt);
            return File(ObjExcelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DemandNoticeCharge_" + DateTime.Now.ToString("dd_MM_yy") + ".xlsx");
            //byte[] ObjExcelData = CommUtil.ExportToExcel2(lstPaymentCharge, vMemberInfoData, true, strTotalColumns);
           // return File(ObjExcelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DemandNoticeCharge_" + DateTime.Now.ToString("dd_MM_yy") + ".xlsx");
        }

        public ActionResult GenerateDemandNoticePDF(int tptid, int tpid)
        {
            if (tptid != 0 && tpid != 0)
            {
                usp_GetTaxPayerDetails_Result mObjTaxPayerData = new BLTaxPayerAsset().BL_GetTaxPayerDetails(tpid, tptid);

                IList<usp_GetPaymentChargeList_Result> lstPaymentCharge = new BLOperationManager().BL_GetPaymentChargeList(tpid, tptid);

                string mStrDirectory = GlobalDefaultValues.DocumentLocation + "DemandNotice/";
                string mStrGeneratedFileName = "DN" + "_" + DateTime.Now.ToString("_ddMMyyyy_") + mObjTaxPayerData.TaxPayerRIN + ".pdf";
                string mStrGeneratedDocumentPath = Path.Combine(mStrDirectory, mStrGeneratedFileName);

                if (!Directory.Exists(mStrDirectory))
                {
                    Directory.CreateDirectory(mStrDirectory);
                }

                if (System.IO.File.Exists(mStrGeneratedDocumentPath))
                {
                    System.IO.File.Delete(mStrGeneratedDocumentPath);
                }
                string mHtmlDirectory = $"{DocumentHTMLLocation}/Demand-Notice.html";

                HtmlToPdf pdf = new HtmlToPdf();
                // set converter options
                pdf.Options.PdfPageSize = PdfPageSize.A4;
                pdf.Options.PdfPageOrientation = PdfPageOrientation.Portrait;

                pdf.Options.WebPageWidth = 1024;
                pdf.Options.WebPageHeight = 0;
                pdf.Options.WebPageFixedSize = false;

                pdf.Options.AutoFitWidth = HtmlToPdfPageFitMode.ShrinkOnly;
                pdf.Options.AutoFitHeight = HtmlToPdfPageFitMode.NoAdjustment;
                string marksheet = string.Empty;
                marksheet = System.IO.File.ReadAllText(mHtmlDirectory);
                string sbTableBody = "";
                foreach(var item in lstPaymentCharge)
                {
                    sbTableBody += "<tr>";
                    sbTableBody += $"<td>{item.TaxYear}</td>";
                    sbTableBody += $"<td>{item.BillRefNo}</td>";
                    sbTableBody += $"<td>{item.RevenueStreamName}</td>";
                    sbTableBody += $"<td>{CommUtil.GetFormatedCurrency(item.BillAmount)}</td>";
                    sbTableBody += $"<td>{CommUtil.GetFormatedCurrency(item.SettledAmount)}</td>";
                    sbTableBody += $"<td>{CommUtil.GetFormatedCurrency(item.BillAmount.GetValueOrDefault() - item.SettledAmount.GetValueOrDefault())}</td>";
                    sbTableBody += $"<td>{CommUtil.GetFormatedCurrency(item.Penalty)}</td>";
                    sbTableBody += $"<td>{CommUtil.GetFormatedCurrency(item.Interest)}</td>";
                    sbTableBody += $"<td>{CommUtil.GetFormatedCurrency(item.TotalCharge.GetValueOrDefault() + item.BillAmount.GetValueOrDefault() - item.SettledAmount.GetValueOrDefault())}</td>";
                    sbTableBody += "</tr>";
                }
                decimal? totalSettledAmount = lstPaymentCharge.Sum(o=>o.SettledAmount);
                decimal? totalBilledAmount = lstPaymentCharge.Sum(o=>o.BillAmount);
                decimal? totalOutstanding = lstPaymentCharge.Sum(o=>o.OutstandingAmount);
                decimal? totalPenalties = lstPaymentCharge.Sum(o=>o.Penalty);
                decimal? totalInterest = lstPaymentCharge.Sum(o=>o.Interest);
                decimal? totalTotal = lstPaymentCharge.Sum(o=>o.TotalCharge);
                decimal? totalAll = totalOutstanding + totalTotal;
                marksheet = marksheet
                    .Replace("@@DemandNoticeBody@@", sbTableBody)
                    .Replace("@@TaxPayerRIN@@", mObjTaxPayerData.TaxPayerRIN)
                    .Replace("@@taxyear@@", lstPaymentCharge.FirstOrDefault().TaxYear.ToString())
                    .Replace("@@TaxPayerName@@", mObjTaxPayerData.TaxPayerName)
                    .Replace("@@ContactAddress@@", mObjTaxPayerData.TaxPayerAddress)
                    .Replace("@@ContactNumber@@", mObjTaxPayerData.TaxPayerMobileNumber)
                    .Replace("@@DemandNoticeAmount@@", CommUtil.GetFormatedCurrency(totalAll))
                    .Replace("@@DemandNoticeDate@@", lstPaymentCharge.FirstOrDefault().ChargeDate.ToString())
                    .Replace("@@BilledAmount@@", CommUtil.GetFormatedCurrency(totalBilledAmount))
                    .Replace("@@SettledAmount@@", CommUtil.GetFormatedCurrency(totalSettledAmount))
                    .Replace("@@Outstanding@@", CommUtil.GetFormatedCurrency(totalOutstanding))
                    .Replace("@@Penalties@@", CommUtil.GetFormatedCurrency(totalPenalties))
                    .Replace("@@Interest@@", CommUtil.GetFormatedCurrency(totalInterest))
                    .Replace("@@Total@@", CommUtil.GetFormatedCurrency(totalAll));
               
                SelectPdf.PdfDocument doc = pdf.ConvertHtmlString(marksheet);
                var bytes = doc.Save();
                System.IO.File.WriteAllBytes(mStrGeneratedDocumentPath, bytes);
                return File(mStrGeneratedDocumentPath, "application/pdf", mStrGeneratedFileName);
            }
            else
            {
                return Content("Invalid Request");
            }
        }

        static string DocumentHTMLLocation = WebConfigurationManager.AppSettings["documentHTMLLocation"] ?? "";

    }
}