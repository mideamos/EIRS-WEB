using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Transactions;
using System.Web.Mvc;
using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;
using EIRS.Web.Models;
using Elmah;
using Vereyon.Web;

namespace EIRS.Web.Controllers
{
    public class ManagerController : BaseController
    {
        // GET: Manager
        
        public ActionResult TaxOfficerList()
        {
            IList<usp_GetUserList_Result> lstTaxOfficer = new BLUser().BL_GetUserList(new MST_Users() { intStatus = 1, TOManagerID = SessionManager.UserID });
            return View(lstTaxOfficer);
        }

        
        public ActionResult TaxPayerList(int? tofid)
        {
            ViewBag.TaxOfficerID = tofid;
            return View();
        }

       
        public JsonResult TaxPayerLoadData(int TaxOfficerID)
        {
            //Get parameters
            usp_GetUserList_Result mObjUserData = new BLUser().BL_GetUserDetails(new MST_Users() { intStatus = 2, UserID = SessionManager.UserID });
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


            IList<usp_GetTaxPayerforTaxOfficer_Result> lstTaxPayer = new BLOperationManager().BL_GetTaxPayerBasedOnTaxOfficer(mObjUserData.TaxOfficeID.GetValueOrDefault(), TaxOfficerID);
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

            decimal dcAssessmentTotal = lstTaxPayer.Sum(t => t.TotalAssessmentAmount.GetValueOrDefault());
            decimal dcPaymentTotal = lstTaxPayer.Sum(t => t.TotalPaymentAmount.GetValueOrDefault());
            decimal dcOutstandingTotal = lstTaxPayer.Sum(t => t.OutstandingAmount.GetValueOrDefault());

            IntTotalRecords = lstTaxPayer.Count();
            var data = lstTaxPayer.Skip(IntSkip).Take(IntPageSize).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data, AssessmentTotal = dcAssessmentTotal, PaymentTotal = dcPaymentTotal, OutstandingTotal = dcOutstandingTotal }, JsonRequestBehavior.AllowGet);
        }

        
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
                        return RedirectToAction("TaxPayerList", "Staff");
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
                        return RedirectToAction("TaxPayerList", "Staff");
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
                        return RedirectToAction("TaxPayerList", "Staff");
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
                        return RedirectToAction("TaxPayerList", "Staff");
                    }
                }
                else
                {
                    return RedirectToAction("TaxPayerList", "Staff");
                }
            }
            else
            {
                return RedirectToAction("TaxPayerList", "Staff");
            }
        }

        
        public ActionResult ReviewStatusSummary()
        {
            UI_FillReviewStatus();
            return View();
        }

        
        public JsonResult ReviewStatusSummaryLoadData(int ReviewStatusID)
        {
            //Get parameters
            usp_GetUserList_Result mObjUserData = new BLUser().BL_GetUserDetails(new MST_Users() { intStatus = 2, UserID = SessionManager.UserID });
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


            IList<usp_RPT_ManagerReviewStatusSummary_Result> lstSummary = new BLOperationManager().BL_GetManagerReviewStatusSummary(mObjUserData.TaxOfficeID.GetValueOrDefault(), SessionManager.UserID, ReviewStatusID);
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

        
        public ActionResult ReviewStatusTaxPayer(int rsid)
        {
            ViewBag.ReviewStatusID = rsid;
            return View();
        }

        
        public JsonResult ReviewStatusTaxPayerLoadData(int ReviewStatusID)
        {
            //Get parameters
            usp_GetUserList_Result mObjUserData = new BLUser().BL_GetUserDetails(new MST_Users() { intStatus = 2, UserID = SessionManager.UserID });
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


            IList<usp_RPT_TaxOfficeManagerStatus_Result> lstTaxPayer = new BLOperationManager().BL_GetTaxOfficeManagerStatus(mObjUserData.TaxOfficeID.GetValueOrDefault(), 0, ReviewStatusID, SessionManager.UserID);
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
            UI_FillYearDropDown();
            return View();
        }

        
        public JsonResult TaxOfficerMonthlyPaymentLoadData(int Year)
        {
            //Get parameters
            usp_GetUserList_Result mObjUserData = new BLUser().BL_GetUserDetails(new MST_Users() { intStatus = 2, UserID = SessionManager.UserID });
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


            IList<usp_RPT_TaxOfficerMonthlyPayment_Result> lstSummary = new BLOperationManager().BL_GetTaxOfficerMonthlyPayment(mObjUserData.TaxOfficeID.GetValueOrDefault(), 0, SessionManager.UserID, Year);
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
    }
}