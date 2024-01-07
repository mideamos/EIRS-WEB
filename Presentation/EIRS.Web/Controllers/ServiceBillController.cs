using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;

namespace EIRS.Web.Controllers
{
    public class ServiceBillController : BaseController
    {

        
        [HttpGet]
        public ActionResult List()
        {
            return View();
        }

        
        [HttpPost]
        public JsonResult LoadData()
        {
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
                sbWhereCondition.Append(" AND ( ISNULL(ServiceBillRefNo,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(Convert(varchar(50),ServiceBillDate,106),'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR dbo.GetTaxPayerName(sb.TaxPayerID,sb.TaxPayerTypeID) LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(ServiceBillAmount,0) LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(SettlementStatusName,'') LIKE @MainFilter)");
            }

            ServiceBill mObjServiceBill = new ServiceBill()
            {
                WhereCondition = sbWhereCondition.ToString(),
                OrderBy = vSortColumn,
                OrderByDirection = vSortColumnDir,
                PageNumber = (TrynParse.parseInt(vStart) / TrynParse.parseInt(vLength)) + 1,
                PageSize = TrynParse.parseInt(vLength),
                MainFilter = !string.IsNullOrWhiteSpace(vFilter) ? "%" + vFilter.Trim() + "%" : vFilter
            };


            IDictionary<string, object> dcData = new BLServiceBill().BL_SearchServiceBillForSideMenu(mObjServiceBill);
            IList<usp_SearchServiceBillForSideMenu_Result> lstServiceBill = (IList<usp_SearchServiceBillForSideMenu_Result>)dcData["ServiceBillList"];

            return Json(new
            {
                draw = vDraw,
                recordsFiltered = dcData["FilteredRecords"],
                recordsTotal = dcData["TotalRecords"],
                data = lstServiceBill
            }, JsonRequestBehavior.AllowGet);
        }

        
        [HttpGet]
        public ActionResult ListWithExport()
        {
            return View();
        }

        
        [HttpPost]
        public JsonResult LoadExportData()
        {
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
                sbWhereCondition.Append(" AND ( ISNULL(ServiceBillRefNo,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(Convert(varchar(50),ServiceBillDate,106),'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR dbo.GetTaxPayerName(sb.TaxPayerID,sb.TaxPayerTypeID) LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(ServiceBillAmount,0) LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(SettlementStatusName,'') LIKE @MainFilter)");
            }

            ServiceBill mObjServiceBill = new ServiceBill()
            {
                WhereCondition = sbWhereCondition.ToString(),
                OrderBy = vSortColumn,
                OrderByDirection = vSortColumnDir,
                PageNumber = (TrynParse.parseInt(vStart) / TrynParse.parseInt(vLength)) + 1,
                PageSize = TrynParse.parseInt(vLength),
                MainFilter = !string.IsNullOrWhiteSpace(vFilter) ? "%" + vFilter.Trim() + "%" : vFilter
            };


            IDictionary<string, object> dcData = new BLServiceBill().BL_SearchServiceBillForSideMenu(mObjServiceBill);
            IList<usp_SearchServiceBillForSideMenu_Result> lstServiceBill = (IList<usp_SearchServiceBillForSideMenu_Result>)dcData["ServiceBillList"];

            return Json(new
            {
                draw = vDraw,
                recordsFiltered = dcData["FilteredRecords"],
                recordsTotal = dcData["TotalRecords"],
                data = lstServiceBill
            }, JsonRequestBehavior.AllowGet);
        }

        
        [HttpGet]
        public ActionResult ExportData()
        {
            IList<usp_GetServiceBillList_Result> lstServiceBillData = new BLServiceBill().BL_GetServiceBillList(new ServiceBill() { IntStatus = 2 });
            string[] strColumns = new string[] { "ServiceBillRefNo",
                                                "ServiceBillDate",
                                                "TaxPayerTypeName",
                                                "TaxPayerName",
                                                "TaxPayerRIN",
                                                "ServiceBillAmount",
                                                "SettlementDueDate",
                                                "SettlementStatusName",
                                                "ServiceBillNotes"  };
            return ExportToExcel(lstServiceBillData, this.RouteData, strColumns, "ServiceBill");
        }
    }
}