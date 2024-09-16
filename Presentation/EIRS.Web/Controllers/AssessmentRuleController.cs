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
using static EIRS.Web.Controllers.Filters;

namespace EIRS.Web.Controllers
{
    [SessionTimeout]
    public class AssessmentRuleController : BaseController
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
                sbWhereCondition.Append(" AND ( ISNULL(AssessmentRuleName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(RuleRunName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(PaymentFrequencyName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(AssessmentAmount,0) LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(TaxYear,0) LIKE @MainFilter)");
            }

            Assessment_Rules mObjAssessmentRule = new Assessment_Rules()
            {
                WhereCondition = sbWhereCondition.ToString(),
                OrderBy = vSortColumn,
                OrderByDirection = vSortColumnDir,
                PageNumber = (TrynParse.parseInt(vStart) / TrynParse.parseInt(vLength)) + 1,
                PageSize = TrynParse.parseInt(vLength),
                MainFilter = !string.IsNullOrWhiteSpace(vFilter) ? "%" + vFilter.Trim() + "%" : vFilter
            };


            IDictionary<string, object> dcData = new BLAssessmentRule().BL_SearchAssessmentRuleForSideMenu(mObjAssessmentRule);
            IList<usp_SearchAssessmentRuleForSideMenu_Result> lstAssessmentRule = (IList<usp_SearchAssessmentRuleForSideMenu_Result>)dcData["AssessmentRuleList"];

            return Json(new
            {
                draw = vDraw,
                recordsFiltered = dcData["FilteredRecords"],
                recordsTotal = dcData["TotalRecords"],
                data = lstAssessmentRule
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
                sbWhereCondition.Append(" AND ( ISNULL(AssessmentRuleName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(RuleRunName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(PaymentFrequencyName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(AssessmentAmount,0) LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(TaxYear,0) LIKE @MainFilter)");
            }

            Assessment_Rules mObjAssessmentRule = new Assessment_Rules()
            {
                WhereCondition = sbWhereCondition.ToString(),
                OrderBy = vSortColumn,
                OrderByDirection = vSortColumnDir,
                PageNumber = (TrynParse.parseInt(vStart) / TrynParse.parseInt(vLength)) + 1,
                PageSize = TrynParse.parseInt(vLength),
                MainFilter = !string.IsNullOrWhiteSpace(vFilter) ? "%" + vFilter.Trim() + "%" : vFilter
            };


            IDictionary<string, object> dcData = new BLAssessmentRule().BL_SearchAssessmentRuleForSideMenu(mObjAssessmentRule);
            IList<usp_SearchAssessmentRuleForSideMenu_Result> lstAssessmentRule = (IList<usp_SearchAssessmentRuleForSideMenu_Result>)dcData["AssessmentRuleList"];

            return Json(new
            {
                draw = vDraw,
                recordsFiltered = dcData["FilteredRecords"],
                recordsTotal = dcData["TotalRecords"],
                data = lstAssessmentRule
            }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult ExportData()
        {
            IList<usp_GetAssessmentRuleList_Result> lstAssessmentRuleData = new BLAssessmentRule().BL_GetAssessmentRuleList(new Assessment_Rules() { IntStatus = 2 });
            string[] strColumns = new string[] { "AssessmentRuleCode",
                                                "ProfileReferenceNo",
                                                "AssessmentRuleName",
                                                "RuleRunName",
                                                "PaymentFrequencyName",
                                                "AssessmentAmount",
                                                "TaxYear",
                                                "PaymentOptionName",
                                                "SettlementMethodNames",
                                                "AssessmentItemNames",
                                                "ProfileAssetTypeName",
                                                "ActiveText",
                                                 };
            return ExportToExcel(lstAssessmentRuleData, this.RouteData, strColumns, "AssessmentRule");
        }
    }
}