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
    public class AssessmentController : BaseController
    {
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
                sbWhereCondition.Append(" AND ( ISNULL(AssessmentRefNo,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(Convert(varchar(50),AssessmentDate,106),'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR dbo.GetTaxPayerName(ast.TaxPayerID,ast.TaxPayerTypeID) LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(AssessmentAmount,0) LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(SettlementStatusName,'') LIKE @MainFilter)");
            }

            Assessment mObjAssessment = new Assessment()
            {
                WhereCondition = sbWhereCondition.ToString(),
                OrderBy = vSortColumn,
                OrderByDirection = vSortColumnDir,
                PageNumber = (TrynParse.parseInt(vStart) / TrynParse.parseInt(vLength)) + 1,
                PageSize = TrynParse.parseInt(vLength),
                MainFilter = !string.IsNullOrWhiteSpace(vFilter) ? "%" + vFilter.Trim() + "%" : vFilter
            };


            IDictionary<string, object> dcData = new BLAssessment().BL_SearchAssessmentForSideMenu(mObjAssessment);
            IList<usp_SearchAssessmentForSideMenu_Result> lstAssessment = (IList<usp_SearchAssessmentForSideMenu_Result>)dcData["AssessmentList"];

            return Json(new
            {
                draw = vDraw,
                recordsFiltered = dcData["FilteredRecords"],
                recordsTotal = dcData["TotalRecords"],
                data = lstAssessment
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
                sbWhereCondition.Append(" AND ( ISNULL(AssessmentRefNo,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(Convert(varchar(50),AssessmentDate,106),'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR dbo.GetTaxPayerName(ast.TaxPayerID,ast.TaxPayerTypeID) LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(AssessmentAmount,0) LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(SettlementStatusName,'') LIKE @MainFilter)");
            }

            Assessment mObjAssessment = new Assessment()
            {
                WhereCondition = sbWhereCondition.ToString(),
                OrderBy = vSortColumn,
                OrderByDirection = vSortColumnDir,
                PageNumber = (TrynParse.parseInt(vStart) / TrynParse.parseInt(vLength)) + 1,
                PageSize = TrynParse.parseInt(vLength),
                MainFilter = !string.IsNullOrWhiteSpace(vFilter) ? "%" + vFilter.Trim() + "%" : vFilter
            };


            IDictionary<string, object> dcData = new BLAssessment().BL_SearchAssessmentForSideMenu(mObjAssessment);
            IList<usp_SearchAssessmentForSideMenu_Result> lstAssessment = (IList<usp_SearchAssessmentForSideMenu_Result>)dcData["AssessmentList"];

            return Json(new
            {
                draw = vDraw,
                recordsFiltered = dcData["FilteredRecords"],
                recordsTotal = dcData["TotalRecords"],
                data = lstAssessment
            }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult ExportData()
        {
            IList<usp_GetAssessmentList_Result> lstAssessmentData = new BLAssessment().BL_GetAssessmentList(new Assessment() { IntStatus = 2 });
            string[] strColumns = new string[] { "AssessmentRefNo",
                                                    "AssessmentDate",
                                                    "TaxPayerTypeName",
                                                    "TaxPayerName",
                                                    "TaxPayerRIN",
                                                    "AssessmentAmount",
                                                    "SettlementDueDate",
                                                    "SettlementStatusName",
                                                    "AssessmentNotes",
                                                     };
            return ExportToExcel(lstAssessmentData, this.RouteData, strColumns, "Assessment");
        }
    }
}