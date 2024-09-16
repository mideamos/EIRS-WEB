using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using static EIRS.Web.Controllers.Filters;
using EIRS.Web.Utility;

namespace EIRS.Web.Controllers
{
    [SessionTimeout]
    public class AssessController : BaseController
    {
        public string getUrl()
        {
            string actionName = ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = ControllerContext.RouteData.Values["controller"].ToString();
            var ret = $"/{controllerName}/{actionName}";
            return ret;
        }
        public ActionResult List()
        {
            string url = getUrl();
            bool itCan = new UtilityController().CheckAccess(url);
            if (!itCan) { return RedirectToAction("AccessDenied", "Utility"); }
            return View();
        }


        public JsonResult GetAllBills()
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            try
            {
                string mStrSearchFilter = Request.Form.GetValues("search[value]")[0];
                string mStrDraw = Request.Form.GetValues("draw")[0];
                string mstrOrderBy = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                string mStrOrderByDir = Request.Form.GetValues("order[0][dir]")[0];
                int mIntStartRowNumber = TrynParse.parseInt(Request.Form.GetValues("start")[0]);
                int mIntPageNumber = TrynParse.parseInt(Request.Form.GetValues("length")[0]);

                // Loading. 
                IList<usp_GetTaxPayerBill_Result> lstBills = new BLAssessment().BL_GetTaxPayerBill(0, 0, 0);

                // Total record count.   
                int totalRecords = lstBills.Count;

                //// Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstBills = lstBills.Where(p =>
                        p.BillDate.GetValueOrDefault().ToString("dd-MMM-yyyy").ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.BillRefNo.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.BillAmount.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.SettlementStatusName.ToLower().Contains(mStrSearchFilter.ToLower())).ToList();
                }

                // Sorting.   
                //Purpose Sorting Data 
                if (!string.IsNullOrEmpty(mstrOrderBy) && !string.IsNullOrEmpty(mStrOrderByDir))
                {
                    lstBills = lstBills.OrderBy(mstrOrderBy + " " + mStrOrderByDir).ToList();
                }

                // Filter record count.   
                int recFilter = lstBills.Count;

                // Apply pagination.   
                lstBills = lstBills.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstBills;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Unsettled()
        {
            string url = getUrl();
            bool itCan = new UtilityController().CheckAccess(url);
            if (!itCan) { return RedirectToAction("AccessDenied", "Utility"); }
            return View();
        }

        public JsonResult GetUnsettled()
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            try
            {
                string mStrSearchFilter = Request.Form.GetValues("search[value]")[0];
                string mStrDraw = Request.Form.GetValues("draw")[0];
                string mstrOrderBy = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                string mStrOrderByDir = Request.Form.GetValues("order[0][dir]")[0];
                int mIntStartRowNumber = TrynParse.parseInt(Request.Form.GetValues("start")[0]);
                int mIntPageNumber = TrynParse.parseInt(Request.Form.GetValues("length")[0]);

                // Loading. 
                IList<usp_GetTaxPayerBill_Result> lstBills = new BLAssessment().BL_GetTaxPayerBill(0, 0, (int)EnumList.SettlementStatus.Unsettled);

                // Total record count.   
                int totalRecords = lstBills.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstBills = lstBills.Where(p => p.BillDate.GetValueOrDefault().ToString("dd-MMM-yyyy").ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.BillRefNo.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.BillAmount.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.SettlementStatusName.ToLower().Contains(mStrSearchFilter.ToLower())).ToList();
                }

                // Sorting.   
                //Purpose Sorting Data 
                if (!(string.IsNullOrEmpty(mstrOrderBy) && string.IsNullOrEmpty(mStrOrderByDir)))
                {
                    lstBills = lstBills.OrderBy(mstrOrderBy + " " + mStrOrderByDir).ToList();
                }

                // Filter record count.   
                int recFilter = lstBills.Count;

                // Apply pagination.   
                lstBills = lstBills.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstBills;

            }
            catch (Exception ex)
            {
                Logger.SendErrorToText(ex);
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Settled()
        {
            return View();
        }


        public JsonResult GetSettled()
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            try
            {
                string mStrSearchFilter = Request.Form.GetValues("search[value]")[0];
                string mStrDraw = Request.Form.GetValues("draw")[0];
                string mstrOrderBy = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                string mStrOrderByDir = Request.Form.GetValues("order[0][dir]")[0];
                int mIntStartRowNumber = TrynParse.parseInt(Request.Form.GetValues("start")[0]);
                int mIntPageNumber = TrynParse.parseInt(Request.Form.GetValues("length")[0]);

                // Loading. 
                IList<usp_GetTaxPayerBill_Result> lstBills = new BLAssessment().BL_GetTaxPayerBill(0, 0, (int)EnumList.SettlementStatus.Settled);

                // Total record count.   
                int totalRecords = lstBills.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstBills = lstBills.Where(p => p.BillDate.GetValueOrDefault().ToString("dd-MMM-yyyy").ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.BillRefNo.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.BillAmount.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.SettlementStatusName.ToLower().Contains(mStrSearchFilter.ToLower())).ToList();
                }

                // Sorting.   
                //Purpose Sorting Data 
                if (!(string.IsNullOrEmpty(mstrOrderBy) && string.IsNullOrEmpty(mStrOrderByDir)))
                {
                    lstBills = lstBills.OrderBy(mstrOrderBy + " " + mStrOrderByDir).ToList();
                }

                // Filter record count.   
                int recFilter = lstBills.Count;

                // Apply pagination.   
                lstBills = lstBills.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstBills;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }


        public ActionResult PartialSettled()
        {
            return View();
        }


        public JsonResult GetPartialSettled()
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            try
            {
                string mStrSearchFilter = Request.Form.GetValues("search[value]")[0];
                string mStrDraw = Request.Form.GetValues("draw")[0];
                string mstrOrderBy = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                string mStrOrderByDir = Request.Form.GetValues("order[0][dir]")[0];
                int mIntStartRowNumber = TrynParse.parseInt(Request.Form.GetValues("start")[0]);
                int mIntPageNumber = TrynParse.parseInt(Request.Form.GetValues("length")[0]);

                // Loading. 
                IList<usp_GetTaxPayerBill_Result> lstBills = new BLAssessment().BL_GetTaxPayerBill(0, 0, (int)EnumList.SettlementStatus.Partial);

                // Total record count.   
                int totalRecords = lstBills.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstBills = lstBills.Where(p => p.BillDate.GetValueOrDefault().ToString("dd-MMM-yyyy").ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.BillRefNo.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.BillAmount.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.SettlementStatusName.ToLower().Contains(mStrSearchFilter.ToLower())).ToList();
                }

                // Sorting.   
                //Purpose Sorting Data 
                if (!(string.IsNullOrEmpty(mstrOrderBy) && string.IsNullOrEmpty(mStrOrderByDir)))
                {
                    lstBills = lstBills.OrderBy(mstrOrderBy + " " + mStrOrderByDir).ToList();
                }

                // Filter record count.   
                int recFilter = lstBills.Count;

                // Apply pagination.   
                lstBills = lstBills.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstBills;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }
    }
}