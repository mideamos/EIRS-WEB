using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
namespace EIRS.Web.Controllers
{
    public class MDAServiceController : BaseController
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
                sbWhereCondition.Append(" AND ( ISNULL(MDAServiceName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(RuleRunName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(PaymentFrequencyName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(ServiceAmount,0) LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(TaxYear,0) LIKE @MainFilter)");
            }

            MDA_Services mObjMDAService = new MDA_Services()
            {
                WhereCondition = sbWhereCondition.ToString(),
                OrderBy = vSortColumn,
                OrderByDirection = vSortColumnDir,
                PageNumber = (TrynParse.parseInt(vStart) / TrynParse.parseInt(vLength)) + 1,
                PageSize = TrynParse.parseInt(vLength),
                MainFilter = !string.IsNullOrWhiteSpace(vFilter) ? "%" + vFilter.Trim() + "%" : vFilter
            };


            IDictionary<string, object> dcData = new BLMDAService().BL_SearchMDAServiceForSideMenu(mObjMDAService);
            IList<usp_SearchMDAServiceForSideMenu_Result> lstMDAService = (IList<usp_SearchMDAServiceForSideMenu_Result>)dcData["MDAServiceList"];

            return Json(new
            {
                draw = vDraw,
                recordsFiltered = dcData["FilteredRecords"],
                recordsTotal = dcData["TotalRecords"],
                data = lstMDAService
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
                sbWhereCondition.Append(" AND ( ISNULL(MDAServiceName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(RuleRunName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(PaymentFrequencyName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(ServiceAmount,0) LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(TaxYear,0) LIKE @MainFilter)");
            }

            MDA_Services mObjMDAService = new MDA_Services()
            {
                WhereCondition = sbWhereCondition.ToString(),
                OrderBy = vSortColumn,
                OrderByDirection = vSortColumnDir,
                PageNumber = (TrynParse.parseInt(vStart) / TrynParse.parseInt(vLength)) + 1,
                PageSize = TrynParse.parseInt(vLength),
                MainFilter = !string.IsNullOrWhiteSpace(vFilter) ? "%" + vFilter.Trim() + "%" : vFilter
            };


            IDictionary<string, object> dcData = new BLMDAService().BL_SearchMDAServiceForSideMenu(mObjMDAService);
            IList<usp_SearchMDAServiceForSideMenu_Result> lstMDAService = (IList<usp_SearchMDAServiceForSideMenu_Result>)dcData["MDAServiceList"];

            return Json(new
            {
                draw = vDraw,
                recordsFiltered = dcData["FilteredRecords"],
                recordsTotal = dcData["TotalRecords"],
                data = lstMDAService
            }, JsonRequestBehavior.AllowGet);
        }

        
        [HttpGet]
        public ActionResult ExportData()
        {
            IList<usp_GetMDAServiceList_Result> lstMDAServiceData = new BLMDAService().BL_GetMDAServiceList(new MDA_Services() { IntStatus = 2 });
            string[] strColumns = new string[] { "MDAServiceCode",
                                                "MDAServiceName",
                                                "RuleRunName",
                                                "PaymentFrequencyName",
                                                "TaxYear",
                                                "PaymentOptionName",
                                                "SettlementMethodNames",
                                                "MDAServiceItemNames",
                                                "ServiceAmount",
                                                "ActiveText" };
            return ExportToExcel(lstMDAServiceData, this.RouteData, strColumns, "MDAService");
        }
    }
}