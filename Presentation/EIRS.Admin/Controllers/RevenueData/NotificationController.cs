using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;
using System;
using System.Collections.Generic;
using System.Transactions;
using System.Web.Mvc;
using Vereyon.Web;
using System.Linq;
using Elmah;
using System.Text;

namespace EIRS.Admin.Controllers
{
    public class NotificationController : BaseController
    {
        public ActionResult List()
        {
            return View();
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Notification mObjNotification = new Notification()
                {
                    NotificationID = id.GetValueOrDefault(),
                    IntStatus = 2
                };

                usp_GetNotificationList_Result mObjNotificationData = new BLNotification().BL_GetNotificationDetails(mObjNotification);

                if (mObjNotificationData != null)
                {
                    return View(mObjNotificationData);
                }
                else
                {
                    return RedirectToAction("List", "Notification");
                }
            }
            else
            {
                return RedirectToAction("List", "Notification");
            }
        }


        [HttpPost]
        public JsonResult LoadData()
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
            if (!string.IsNullOrWhiteSpace(Request.Form["NotificationRefNo"]))
            {
                sbWhereCondition.Append(" AND ISNULL(NotificationRefNo,'') LIKE @NotificationRefNo");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["NotificationMethodName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(NotificationMethodName,'') LIKE @NotificationMethodName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["NotificationTypeName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(NotificationTypeName,'') LIKE @NotificationTypeName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["EventRefNo"]))
            {
                sbWhereCondition.Append(" AND ISNULL(EventRefNo,'') LIKE @EventRefNo");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["TaxPayerTypeName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(TaxPayerTypeName,'') LIKE @TaxPayerTypeName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["TaxPayerName"]))
            {
                sbWhereCondition.Append(" AND dbo.GetTaxPayerName(notf.TaxPayerID,notf.TaxPayerTypeID) LIKE @TaxPayerName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["NotificationDate"]))
            {
                sbWhereCondition.Append(" AND ISNULL(REPLACE(CONVERT(varchar(50),NotificationDate,106),' ','-'),'') LIKE @NotificationDate");
            }
            //if (!string.IsNullOrWhiteSpace(Request.Form["NotificationStatus"]))
            //{
            //    sbWhereCondition.Append(" AND CASE WHEN ISNULL(notf.NotificationStatus,0) = 0 THEN 'Failed' ELSE 'Successful' END LIKE @NotificationStatus");
            //}
            if (!string.IsNullOrWhiteSpace(Request.Form["StatusName"]))
            {
                sbWhereCondition.Append(" AND CASE WHEN ISNULL(notf.NotificationStatus,0) = 0  THEN 'Failed' ELSE 'Successful' END LIKE @StatusName");
            }
            
            
            if (!string.IsNullOrEmpty(vFilter))
            {
                sbWhereCondition.Append(" AND ( ISNULL(NotificationRefNo,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(NotificationMethodName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(NotificationTypeName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(EventRefNo,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(TaxPayerTypeName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR dbo.GetTaxPayerName(notf.TaxPayerID,notf.TaxPayerTypeID) LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(NotificationStatus,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(REPLACE(CONVERT(varchar(50),NotificationDate,106),' ','-'),'') LIKE @MainFilter");
             //   sbWhereCondition.Append(" OR CASE WHEN ISNULL(notf.NotificationStatus,0) = 0 THEN 'Failed' ELSE 'Successful' END LIKE @MainFilter)");
                sbWhereCondition.Append(" OR CASE WHEN ISNULL(notf.NotificationStatus,0) = 0 THEN 'Failed' ELSE 'Successful' END LIKE @MainFilter)");
            }

            Notification mObjNotification = new Notification()
            {
                WhereCondition = sbWhereCondition.ToString(),
                OrderBy = vSortColumn,
                OrderByDirection = vSortColumnDir,
                PageNumber = (TrynParse.parseInt(vStart) / TrynParse.parseInt(vLength)) + 1,
                PageSize = TrynParse.parseInt(vLength),
                NotificationRefNo = !string.IsNullOrWhiteSpace(Request.Form["NotificationRefNo"]) ? "%" + Request.Form["NotificationRefNo"].Trim() + "%" : TrynParse.parseString(Request.Form["NotificationRefNo"]),
                NotificationMethodName = !string.IsNullOrWhiteSpace(Request.Form["NotificationMethodName"]) ? "%" + Request.Form["NotificationMethodName"].Trim() + "%" : TrynParse.parseString(Request.Form["NotificationMethodName"]),
                NotificationTypeName = !string.IsNullOrWhiteSpace(Request.Form["NotificationTypeName"]) ? "%" + Request.Form["NotificationTypeName"].Trim() + "%" : TrynParse.parseString(Request.Form["NotificationTypeName"]),
                EventRefNo = !string.IsNullOrWhiteSpace(Request.Form["EventRefNo"]) ? "%" + Request.Form["EventRefNo"].Trim() + "%" : TrynParse.parseString(Request.Form["EventRefNo"]),
                TaxPayerTypeName = !string.IsNullOrWhiteSpace(Request.Form["TaxPayerTypeName"]) ? "%" + Request.Form["TaxPayerTypeName"].Trim() + "%" : TrynParse.parseString(Request.Form["TaxPayerTypeName"]),
                TaxPayerName = !string.IsNullOrWhiteSpace(Request.Form["TaxPayerName"]) ? "%" + Request.Form["TaxPayerName"].Trim() + "%" : TrynParse.parseString(Request.Form["TaxPayerName"]),
                strNotificationDate = !string.IsNullOrWhiteSpace(Request.Form["NotificationDate"]) ? "%" + Request.Form["NotificationDate"].Trim() + "%" : TrynParse.parseString(Request.Form["NotificationDate"]),
                // strNotificationStatus = !string.IsNullOrWhiteSpace(Request.Form["NotificationStatus"]) ? "%" + Request.Form["NotificationStatus"].Trim() + "%" : TrynParse.parseString(Request.Form["NotificationStatus"]),
                StatusName = !string.IsNullOrWhiteSpace(Request.Form["StatusName"]) ? "%" + Request.Form["StatusName"].Trim() + "%" : TrynParse.parseString(Request.Form["StatusName"]),
                MainFilter = !string.IsNullOrWhiteSpace(vFilter) ? "%" + vFilter.Trim() + "%" : vFilter
            };



            IDictionary<string, object> dcData = new BLNotification().BL_SearchNotification(mObjNotification);
          IList<usp_SearchNotification_Result> lstNotification = (IList<usp_SearchNotification_Result>)dcData["NotificationList"];

            return Json(new
            {
                draw = vDraw,
                recordsFiltered = dcData["FilteredRecords"],
                recordsTotal = dcData["TotalRecords"],
                data = lstNotification
            }, JsonRequestBehavior.AllowGet);
             }
    }
}