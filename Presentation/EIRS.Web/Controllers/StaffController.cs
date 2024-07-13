using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Transactions;
using System.Web.Configuration;
using System.Web.Mvc;
using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Web.Models;
using EIRS.Web.Utility;
using Elmah;
using Newtonsoft.Json;
using PagedList;
using SelectPdf;
using static EIRS.Web.Controllers.Filters;

namespace EIRS.Web.Controllers
{
    [SessionTimeout]
    public class StaffController : BaseController
    {
        // GET: Staff
        EIRSEntities _db = new EIRSEntities();
        public ActionResult TaxPayerList()
        {
            return View();
        }

        public JsonResult TaxPayerLoadData()
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


            IList<usp_GetTaxPayerforTaxOfficer_Result> lstTaxPayer = new BLOperationManager().BL_GetTaxPayerBasedOnTaxOfficer(mObjUserData.TaxOfficeID.GetValueOrDefault(), SessionManager.UserID);
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


            IList<usp_RPT_StaffReviewStatusSummary_Result> lstSummary = new BLOperationManager().BL_GetStaffReviewStatusSummary(mObjUserData.TaxOfficeID.GetValueOrDefault(), SessionManager.UserID, ReviewStatusID);
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


            IList<usp_RPT_TaxOfficerStatus_Result> lstTaxPayer = new BLOperationManager().BL_GetTaxOfficerStatus(mObjUserData.TaxOfficeID.GetValueOrDefault(), 0, ReviewStatusID, SessionManager.UserID);
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


            IList<usp_RPT_TaxOfficerMonthlyPayment_Result> lstSummary = new BLOperationManager().BL_GetTaxOfficerMonthlyPayment(mObjUserData.TaxOfficeID.GetValueOrDefault(), SessionManager.UserID, 0, Year);
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
        public ActionResult SignTCCList()
        {
            long holder;
            int userId = SessionManager.UserID;
            List<NewTccViewModel> listTccVm = new List<NewTccViewModel>();

            var checkAppLevel = _db.Tax_Offices.Where(x =>
                x.Approver1 == userId ||
                x.Approver2 == userId ||
                x.Approver3 == userId ||
                x.PAYE_ApproverID == userId).ToList();

            if (!checkAppLevel.Any())
            {
                ViewBag.Message = "You Are Not An Approving Officer";
                return View(listTccVm);
            }
            if (checkAppLevel.Any(o => o.Approver1 == userId || o.PAYE_ApproverID == userId))
            {
                holder = (long)TCCSigningStage.AwaitingFirstSigner;
            }
            else if (checkAppLevel.Any(o => o.Approver2 == userId))
            {
                holder = (long)TCCSigningStage.AwaitingSecondSigner;
            }
            else if (checkAppLevel.Any(o => o.Approver3 == userId))
            {
                holder = (long)TCCSigningStage.AwaitingThirdSigner;
            }
            else
            {
                ViewBag.Message = "You Are Not An Approving Officer As You Are Not PAYE APPROVAL";
                return View(listTccVm);
            }

            var ee = (from ex in _db.TCC_Request
                      where ex.SEDE_OrderID == holder || ex.SEDE_DocumentID == userId || ex.ServiceBillID == userId || ex.VisibleSignStatusID == userId
                      join ue in _db.Individuals on ex.TaxPayerID equals ue.IndividualID
                      select new
                      {
                          TCCRequestID = ex.TCCRequestID,
                          GeneratedPath = ex.GeneratedPath,
                          RequestRefNo = ex.RequestRefNo,
                          MobileNumber = ue.MobileNumber1,
                          RequestDate = ex.RequestDate,
                          TaxFName = ue.FirstName,
                          TaxYear = ex.TaxYear,
                          TaxLName = ue.LastName,
                          modifiedDate = ex.ModifiedDate,
                          modifiedBy = ex.ModifiedBy,
                          sedeDoc = ex.SEDE_DocumentID,
                          serviceBillid = ex.ServiceBillID,
                          VisibleSignStatusID = ex.VisibleSignStatusID,
                          sedeId = ex.SEDE_OrderID
                      }).OrderByDescending(o => o.modifiedDate).ToList();

            foreach (var item in ee)
            {
                NewTccViewModel tccVm = new NewTccViewModel
                {
                    BillStatus = item.sedeId switch
                    {
                        10000 => "Awaiting First Signer",
                        10001 => "Awaiting Second Signer",
                        10002 => "Awaiting Third Signer",
                        _ => "TCC Signed Successfully"
                    },
                    IsSigned = item.sedeDoc == userId || item.serviceBillid == userId || item.VisibleSignStatusID == userId,
                    TCCRequestID = item.TCCRequestID,
                    GeneratedPath = item.GeneratedPath,
                    RequestRefNo = item.RequestRefNo,
                    MobileNumber = item.MobileNumber,
                    RequestDate = item.modifiedDate,
                    TaxFName = item.TaxFName,
                    TaxLName = item.TaxLName,
                    TaxYear = item.TaxYear,
                    TaxPayerID = ee.IndexOf(item) + 1,
                    ModifiedBy = item.modifiedBy
                };

                listTccVm.Add(tccVm);
            }

            return View(listTccVm);
        }

        //eSign
        //public ActionResult SignTCCList()
        //{
        //    long holder;
        //    int userId = SessionManager.UserID;
        //    List<NewTccViewModel> listTccVm = new List<NewTccViewModel>();

        //    var checkAppLevel = _db.Tax_Offices.Where(x =>
        //    x.Approver1 == userId ||
        //    x.Approver2 == userId ||
        //    x.Approver3 == userId ||
        //    x.PAYE_ApproverID == userId)
        //    .ToList();

        //    if (checkAppLevel.Count == 0)
        //    {
        //        ViewBag.Message = "You Are Not An Approving Officer";
        //        return View(listTccVm);
        //    }
        //    else
        //    {
        //        bool isFirstSigner = false;
        //        bool isSecondSigner = false;
        //        bool isThirdSigner = false;

        //        foreach (var item in checkAppLevel)
        //        {
        //            if (item.Approver1 == userId || item.PAYE_ApproverID == userId)
        //            {
        //                isFirstSigner = true;
        //                isSecondSigner = true;
        //                isThirdSigner = true;
        //                break; // No need to check further, highest priority matched
        //            }

        //            if (item.Approver2 == userId)
        //            {
        //                isSecondSigner = true;
        //                isThirdSigner = true;
        //            }

        //            if (item.Approver3 == userId)
        //            {
        //                isThirdSigner = true;
        //            }
        //        }

        //        if (isFirstSigner)
        //        {
        //            holder = (long)TCCSigningStage.AwaitingFirstSigner;
        //        }
        //        else if (isSecondSigner)
        //        {
        //            holder = (long)TCCSigningStage.AwaitingSecondSigner;
        //        }
        //        else if (isThirdSigner)
        //        {
        //            holder = (long)TCCSigningStage.AwaitingThirdSigner;
        //        }
        //        else
        //        {
        //            ViewBag.Message = "You Are Not An Approving Officer As You Are Not PAYE APPROVAL";
        //            return View(listTccVm);
        //        }

        //        var ee = (from ex in _db.TCC_Request.Where(o => o.SEDE_OrderID == holder || o.SEDE_DocumentID == userId || o.ServiceBillID == userId || o.VisibleSignStatusID == userId)
        //                  join ue in _db.Individuals on
        //                  ex.TaxPayerID equals ue.IndividualID
        //                  select new
        //                  {
        //                      TCCRequestID = ex.TCCRequestID,
        //                      GeneratedPath = ex.GeneratedPath,
        //                      RequestRefNo = ex.RequestRefNo,
        //                      MobileNumber = ue.MobileNumber1,
        //                      RequestDate = ex.RequestDate,
        //                      TaxFName = ue.FirstName,
        //                      TaxYear = ex.TaxYear,
        //                      TaxLName = ue.LastName,
        //                      modifiedDate = ex.ModifiedDate,
        //                      modifiedBy = ex.ModifiedBy,
        //                      sedeDoc = ex.SEDE_DocumentID,
        //                      serviceBillid = ex.ServiceBillID,
        //                      VisibleSignStatusID = ex.VisibleSignStatusID,
        //                      sedeId = ex.SEDE_OrderID
        //                  }).OrderByDescending(o => o.modifiedDate).ToList();

        //        for (int i = 0; i < ee.Count; i++)
        //        {
        //            NewTccViewModel tccVm = new NewTccViewModel();
        //            if (ee[i].sedeId == 10000)
        //                tccVm.BillStatus = "Awaiting First Signer";
        //            else if (ee[i].sedeId == 10001)
        //                tccVm.BillStatus = "Awaiting Second Signer";
        //            else if (ee[i].sedeId == 10002)
        //                tccVm.BillStatus = "Awaiting Third Signer";
        //            else
        //                tccVm.BillStatus = "TCC Signed Successfully";
        //            if (ee[i].sedeDoc == userId || ee[i].serviceBillid == userId || ee[i].VisibleSignStatusID == userId)
        //                tccVm.IsSigned = true;
        //            tccVm.TCCRequestID = ee[i].TCCRequestID;
        //            tccVm.GeneratedPath = ee[i].GeneratedPath;
        //            tccVm.RequestRefNo = ee[i].RequestRefNo;
        //            tccVm.MobileNumber = ee[i].MobileNumber;
        //            tccVm.RequestDate = ee[i].modifiedDate;
        //            tccVm.TaxFName = ee[i].TaxFName;
        //            tccVm.TaxLName = ee[i].TaxLName;
        //            tccVm.TaxYear = ee[i].TaxYear;
        //            tccVm.TaxPayerID = i + 1;
        //            tccVm.ModifiedBy = ee[i].modifiedBy;
        //            listTccVm.Add(tccVm);
        //        }
        //    }
        //    return View(listTccVm);
        //}
        public ActionResult SignTCCListBulk()
        {
            long holder;
            int userId = SessionManager.UserID;
            List<NewTccViewModel> listTccVm = new List<NewTccViewModel>();
            var checkAppLevel = _db.Tax_Offices.Where(x => x.Approver1 == userId || x.Approver2 == userId || x.Approver3 == userId).ToList();
            if (checkAppLevel.Count == 0)
            {
                ViewBag.Message = "You Are Not An Approving Officer";
                return View(listTccVm);
            }

            var retHolder = checkAppLevel.FirstOrDefault();

            if (retHolder.Approver1 == userId)
                holder = (long)TCCSigningStage.AwaitingFirstSigner;
            else if (retHolder.Approver2 == userId)
                holder = (long)TCCSigningStage.AwaitingSecondSigner;
            else
                holder = (long)TCCSigningStage.AwaitingThirdSigner;
            var ee =
                (from ex in _db.TCC_Request.Where(o => o.SEDE_OrderID == holder)
                 join ue in _db.Individuals on
                 ex.TaxPayerID equals ue.IndividualID
                 select new
                 {
                     TCCRequestID = ex.TCCRequestID,
                     GeneratedPath = ex.GeneratedPath,
                     RequestRefNo = ex.RequestRefNo,
                     MobileNumber = ue.MobileNumber1,
                     RequestDate = ex.RequestDate,
                     TaxFName = ue.FirstName,
                     TaxYear = ex.TaxYear,
                     TaxLName = ue.LastName,
                     modifiedDate = ex.ModifiedDate,
                     modifiedBy = ex.ModifiedBy,
                     sedeDoc = ex.SEDE_DocumentID,
                     serviceBillid = ex.ServiceBillID,
                     VisibleSignStatusID = ex.VisibleSignStatusID,
                     sedeId = ex.SEDE_OrderID
                 }).OrderByDescending(o => o.modifiedDate).ToList();

            //ee = ee.Where(o => o.sedeId == holder).ToList();

            for (int i = 0; i < ee.Count; i++)
            {
                NewTccViewModel tccVm = new NewTccViewModel();
                if (ee[i].sedeId == 10000)
                    tccVm.BillStatus = "Awaiting First Signer";
                else if (ee[i].sedeId == 10001)
                    tccVm.BillStatus = "Awaiting Second Signer";
                else if (ee[i].sedeId == 10002)
                    tccVm.BillStatus = "Awaiting Third Signer";
                else
                    tccVm.BillStatus = "TCC Signed Successfully";
                if (ee[i].sedeDoc == userId || ee[i].serviceBillid == userId || ee[i].VisibleSignStatusID == userId)
                    tccVm.IsSigned = true;
                tccVm.TCCRequestID = ee[i].TCCRequestID;
                tccVm.GeneratedPath = ee[i].GeneratedPath;
                tccVm.RequestRefNo = ee[i].RequestRefNo;
                tccVm.MobileNumber = ee[i].MobileNumber;
                tccVm.RequestDate = ee[i].modifiedDate;
                tccVm.TaxFName = ee[i].TaxFName;
                tccVm.TaxLName = ee[i].TaxLName;
                tccVm.TaxYear = ee[i].TaxYear;
                tccVm.TaxPayerID = i + 1;
                tccVm.ModifiedBy = ee[i].modifiedBy;
                listTccVm.Add(tccVm);
            }

            return View(listTccVm);
        }
        [HttpPost]
        public ActionResult SignTCCListBulk(List<string> selectedIds)
        {
            try
            {
                string imgData = "";
                IDictionary<string, object> dcResponse = new Dictionary<string, object>();

                usp_GetUserList_Result mObjUserData = new BLUser().BL_GetUserDetails(new MST_Users() { UserID = SessionManager.UserID, intStatus = 2 });
                string mStrDirectory = "", mStrGeneratedFileName = "", mStrGeneratedDocumentPath = "",
                    mStrGeneratedHtmlPath = "", mHtmlDirectory = "",
                     mStrDirectoryForPrint = "",
                 mStrGeneratedFileNameForPrint = "",
                 mStrGeneratedDocumentPathForPrint = "", mStrGeneratedHtmlPathForPrint = "",
                  mHtmlDirectoryForPrint = "";
                if (mObjUserData != null)
                {
                    string imgScrs = !string.IsNullOrWhiteSpace(mObjUserData.SignaturePath) ? mObjUserData.SignaturePath : "";
                    string fulPath = WebConfigurationManager.AppSettings["documentLocation"] + imgScrs;
                    if (!System.IO.File.Exists(fulPath))
                    {
                        dcResponse["success"] = false;
                        dcResponse["Message"] = $"Signing eTCC Failed  As User Signature {fulPath} is not found";

                        return Json(dcResponse, JsonRequestBehavior.AllowGet);
                    }
                    imgData = ImageConverter(fulPath);
                    imgData = "data:image/png;base64," + imgData;
                    BLTCC mObjBLTCC = new BLTCC();
                    for (int i = 0; i < selectedIds.Count; i++)
                    {
                        long nereqid = Convert.ToInt64(selectedIds[i]);
                        var ret = mObjBLTCC.BL_GetTCCRequestGenerateDetails(nereqid);
                        var retVal = _db.TCC_Request.FirstOrDefault(o => o.TCCRequestID == nereqid);
                        usp_GetTCCRequestDetails_Result mObjRequestData = mObjBLTCC.BL_GetRequestDetails(nereqid);

                        switch (mObjRequestData.SEDE_OrderID)
                        {
                            case (long)TCCSigningStage.AwaitingFirstSigner:
                                mStrDirectory = $"{GlobalDefaultValues.DocumentLocation}/ETCC/{mObjRequestData.IndividualID}/Signed/";
                                mStrGeneratedFileName = $"{mObjRequestData.IndividualID}_{DateTime.Now:ddMMyyyy}_Generated.pdf";//mObjTreasuryData.ReceiptRefNo + "_" + DateTime.Now.ToString("_ddMMyyyy") + "_TR.pdf";
                                mStrGeneratedDocumentPath = Path.Combine(mStrDirectory, mStrGeneratedFileName);
                                mStrGeneratedHtmlPath = Path.Combine(mStrDirectory + "/Temp/Html", mObjRequestData.IndividualID + "_template.html");
                                mHtmlDirectory = $"{GlobalDefaultValues.DocumentLocation}/{mObjRequestData.ValidatedPath}";

                                //ETCC/Print/149726/Temp/Html/149726_template.html
                                mStrDirectoryForPrint = $"{GlobalDefaultValues.DocumentLocation}/ETCC/Print/{mObjRequestData.IndividualID}";
                                mStrGeneratedFileNameForPrint = $"{mObjRequestData.IndividualID}_{DateTime.Now:ddMMyyyy}_Generated.pdf";//mObjTreasuryData.ReceiptRefNo + "_" + DateTime.Now.ToString("_ddMMyyyy") + "_TR.pdf";
                                mStrGeneratedDocumentPathForPrint = Path.Combine(mStrDirectoryForPrint, mStrGeneratedFileNameForPrint);
                                mStrGeneratedHtmlPathForPrint = Path.Combine(mStrDirectoryForPrint + "/Temp/Html", mObjRequestData.IndividualID + "_template.html");
                                mHtmlDirectoryForPrint = $"{GlobalDefaultValues.DocumentLocation}/{retVal.GeneratePathForPrint}";
                                if (!Directory.Exists(mStrDirectory))
                                {
                                    Directory.CreateDirectory(mStrDirectory);
                                }
                                if (!Directory.Exists(mStrDirectory + "/Temp/Html"))
                                {
                                    Directory.CreateDirectory(mStrDirectory + "/Temp/Html");
                                }
                                if (!Directory.Exists(mStrDirectoryForPrint))
                                {
                                    Directory.CreateDirectory(mStrDirectoryForPrint);
                                }
                                if (!Directory.Exists(mStrDirectoryForPrint + "/Temp/Html"))
                                {
                                    Directory.CreateDirectory(mStrDirectoryForPrint + "/Temp/Html");
                                }
                                HtmlToPdf pdf = new HtmlToPdf();
                                // set converter options
                                pdf.Options.PdfPageSize = PdfPageSize.A4;
                                pdf.Options.PdfPageOrientation = PdfPageOrientation.Portrait;

                                pdf.Options.WebPageWidth = 0;
                                pdf.Options.WebPageHeight = 0;
                                pdf.Options.WebPageFixedSize = false;

                                pdf.Options.AutoFitWidth = HtmlToPdfPageFitMode.NoAdjustment;
                                pdf.Options.AutoFitHeight = HtmlToPdfPageFitMode.NoAdjustment;
                                string marksheet = string.Empty;
                                string marksheetForPrint = string.Empty;
                                marksheet = System.IO.File.ReadAllText(mHtmlDirectory);
                                marksheet = marksheet.Replace("@@first@@", imgData);
                                marksheetForPrint = System.IO.File.ReadAllText(mHtmlDirectoryForPrint);
                                marksheetForPrint = marksheetForPrint.Replace("@@first@@", imgData);

                                System.IO.File.WriteAllText(mStrGeneratedHtmlPath, marksheet);
                                PdfDocument doc = pdf.ConvertHtmlString(marksheet);
                                var bytes = doc.Save();
                                System.IO.File.WriteAllText(mStrGeneratedHtmlPathForPrint, marksheetForPrint);
                                PdfDocument docII = pdf.ConvertHtmlString(marksheetForPrint);
                                var bytesII = docII.Save();

                                System.IO.File.WriteAllBytes(mStrGeneratedDocumentPath, bytes);
                                System.IO.File.WriteAllBytes(mStrGeneratedDocumentPathForPrint, bytesII);
                                ViewBag.RequestData = mObjRequestData;
                                ViewBag.pdf = mStrGeneratedDocumentPath;
                                GenerateViewModel mObjGenerateTCCModel = new GenerateViewModel()
                                {
                                    RequestID = mObjRequestData.TCCRequestID,
                                };

                                TCC_Request mObjUpdateStatus = new TCC_Request()
                                {
                                    TCCRequestID = mObjRequestData.TCCRequestID,
                                    StatusID = (int)NewTCCRequestStage.Waiting_For_Second_Signature,
                                    ModifiedBy = SessionManager.UserID,
                                    SEDE_DocumentID = SessionManager.UserID,//to holder first signer id
                                    SEDE_OrderID = (long)TCCSigningStage.AwaitingSecondSigner,
                                    GeneratedPath = "ETCC/" + mObjRequestData.IndividualID + "/Signed/" + mStrGeneratedFileName,
                                    ValidatedPath = "ETCC/" + mObjRequestData.IndividualID + "/Signed/Temp/Html/" + mObjRequestData.IndividualID + "_template.html",
                                    //  RequestDate = CommUtil.GetCurrentDateTime(),
                                    GeneratePathForPrint = "ETCC/Print/" + mObjRequestData.IndividualID + "/Temp/Html/" + mObjRequestData.IndividualID + "_template.html",
                                    ModifiedDate = CommUtil.GetCurrentDateTime()
                                };

                                mObjBLTCC.BL_UpdateRequestStatus(mObjUpdateStatus);

                                SessionManager.Path = mStrGeneratedDocumentPath;
                                SessionManager.Path = mStrGeneratedDocumentPath;
                                ViewBag.path = mStrGeneratedDocumentPath;
                                dcResponse["success"] = true;
                                dcResponse["Message"] = "eTCC Signed Succcessfully";
                                break;
                            case (long)TCCSigningStage.AwaitingSecondSigner:
                                mStrDirectory = $"{GlobalDefaultValues.DocumentLocation}/ETCC/{mObjRequestData.IndividualID}/Signed/";
                                mStrGeneratedFileName = $"{mObjRequestData.IndividualID}_{DateTime.Now:ddMMyyyy}_Generated.pdf";//mObjTreasuryData.ReceiptRefNo + "_" + DateTime.Now.ToString("_ddMMyyyy") + "_TR.pdf";
                                mStrGeneratedDocumentPath = Path.Combine(mStrDirectory, mStrGeneratedFileName);
                                mStrGeneratedHtmlPath = Path.Combine(mStrDirectory + "/Temp/Html", mObjRequestData.IndividualID + "_template.html");
                                mHtmlDirectory = $"{GlobalDefaultValues.DocumentLocation}/{mObjRequestData.ValidatedPath}";
                                mStrDirectoryForPrint = $"{GlobalDefaultValues.DocumentLocation}/ETCC/Print/{mObjRequestData.IndividualID}";
                                mStrGeneratedFileNameForPrint = $"{mObjRequestData.IndividualID}_{DateTime.Now:ddMMyyyy}_Generated.pdf";//mObjTreasuryData.ReceiptRefNo + "_" + DateTime.Now.ToString("_ddMMyyyy") + "_TR.pdf";
                                mStrGeneratedDocumentPathForPrint = Path.Combine(mStrDirectoryForPrint, mStrGeneratedFileNameForPrint);
                                mStrGeneratedHtmlPathForPrint = Path.Combine(mStrDirectoryForPrint + "/Temp/Html", mObjRequestData.IndividualID + "_template.html");
                                mHtmlDirectoryForPrint = $"{GlobalDefaultValues.DocumentLocation}/{retVal.GeneratePathForPrint}";
                                if (!Directory.Exists(mStrDirectory))
                                {
                                    Directory.CreateDirectory(mStrDirectory);
                                }
                                if (!Directory.Exists(mStrDirectory + "/Temp/Html"))
                                {
                                    Directory.CreateDirectory(mStrDirectory + "/Temp/Html");
                                }
                                if (!Directory.Exists(mStrDirectoryForPrint))
                                {
                                    Directory.CreateDirectory(mStrDirectoryForPrint);
                                }
                                if (!Directory.Exists(mStrDirectoryForPrint + "/Temp/Html"))
                                {
                                    Directory.CreateDirectory(mStrDirectoryForPrint + "/Temp/Html");
                                }

                                HtmlToPdf pdf1 = new HtmlToPdf();
                                // set converter options
                                pdf1.Options.PdfPageSize = PdfPageSize.A4;
                                pdf1.Options.PdfPageOrientation = PdfPageOrientation.Portrait;

                                pdf1.Options.WebPageWidth = 0;
                                pdf1.Options.WebPageHeight = 0;
                                pdf1.Options.WebPageFixedSize = false;

                                pdf1.Options.AutoFitWidth = HtmlToPdfPageFitMode.NoAdjustment;
                                pdf1.Options.AutoFitHeight = HtmlToPdfPageFitMode.NoAdjustment;
                                string marksheet1 = string.Empty;
                                string marksheetII = string.Empty;
                                marksheet = System.IO.File.ReadAllText(mHtmlDirectory);
                                marksheetII = System.IO.File.ReadAllText(mHtmlDirectoryForPrint);
                                marksheet = marksheet.Replace("@@second@@", imgData);
                                marksheetII = marksheetII.Replace("@@second@@", imgData);

                                System.IO.File.WriteAllText(mStrGeneratedHtmlPath, marksheet);
                                PdfDocument doc1 = pdf1.ConvertHtmlString(marksheet);
                                var bytes1 = doc1.Save();
                                System.IO.File.WriteAllText(mStrGeneratedHtmlPathForPrint, marksheetII);
                                PdfDocument docII1 = pdf1.ConvertHtmlString(marksheetII);
                                var bytesII1 = docII1.Save();

                                System.IO.File.WriteAllBytes(mStrGeneratedDocumentPath, bytes1);
                                System.IO.File.WriteAllBytes(mStrGeneratedDocumentPathForPrint, bytesII1);
                                ViewBag.RequestData = mObjRequestData;
                                ViewBag.pdf = mStrGeneratedDocumentPath;
                                GenerateViewModel mObjGenerateTCCModel1 = new GenerateViewModel()
                                {
                                    RequestID = mObjRequestData.TCCRequestID,
                                };

                                TCC_Request mObjUpdateStatus1 = new TCC_Request()
                                {
                                    TCCRequestID = mObjRequestData.TCCRequestID,
                                    SEDE_DocumentID = retVal.SEDE_DocumentID,
                                    ServiceBillID = SessionManager.UserID,//to holder second signer id
                                    StatusID = (int)NewTCCRequestStage.Waiting_For_Second_Signature,
                                    ModifiedBy = SessionManager.UserID,
                                    SEDE_OrderID = (long)TCCSigningStage.AwaitingThirdSigner,
                                    //  RequestDate = CommUtil.GetCurrentDateTime(),
                                    GeneratedPath = "ETCC/" + mObjRequestData.IndividualID + "/Signed/" + mStrGeneratedFileName,
                                    ValidatedPath = "ETCC/" + mObjRequestData.IndividualID + "/Signed/Temp/Html/" + mObjRequestData.IndividualID + "_template.html",
                                    GeneratePathForPrint = "ETCC/Print/" + mObjRequestData.IndividualID + "/Temp/Html/" + mObjRequestData.IndividualID + "_template.html",
                                    ModifiedDate = CommUtil.GetCurrentDateTime()
                                };

                                mObjBLTCC.BL_UpdateRequestStatus(mObjUpdateStatus1);
                                var taxDetail = _db.Individuals.FirstOrDefault(o => o.IndividualID == mObjRequestData.IndividualID);

                                string msg = $"Your TCC request is awaiting final approval";
                                bool blnSMSSent = UtilityController.SendSMS(taxDetail.MobileNumber1, msg);

                                SessionManager.Path = mStrGeneratedDocumentPath;
                                ViewBag.path = mStrGeneratedDocumentPath;
                                dcResponse["success"] = true;
                                dcResponse["Message"] = "eTCC Signed Succcessfully";

                                break;
                            case (long)TCCSigningStage.AwaitingThirdSigner:

                                mStrDirectory = $"{GlobalDefaultValues.DocumentLocation}/ETCC/{mObjRequestData.IndividualID}/Signed/";
                                mStrGeneratedFileName = $"{mObjRequestData.IndividualID}_{DateTime.Now:ddMMyyyy}_Generated.pdf";//mObjTreasuryData.ReceiptRefNo + "_" + DateTime.Now.ToString("_ddMMyyyy") + "_TR.pdf";
                                mStrGeneratedDocumentPath = Path.Combine(mStrDirectory, mStrGeneratedFileName);
                                mStrGeneratedHtmlPath = Path.Combine(mStrDirectory + "/Temp/Html", mObjRequestData.IndividualID + "_template.html");
                                mHtmlDirectory = $"{GlobalDefaultValues.DocumentLocation}/{mObjRequestData.ValidatedPath}";

                                mStrDirectoryForPrint = $"{GlobalDefaultValues.DocumentLocation}/ETCC/Print/{mObjRequestData.IndividualID}";
                                mStrGeneratedFileNameForPrint = $"{mObjRequestData.IndividualID}_{DateTime.Now:ddMMyyyy}_Generated.pdf";//mObjTreasuryData.ReceiptRefNo + "_" + DateTime.Now.ToString("_ddMMyyyy") + "_TR.pdf";
                                mStrGeneratedDocumentPathForPrint = Path.Combine(mStrDirectoryForPrint, mStrGeneratedFileNameForPrint);
                                mStrGeneratedHtmlPathForPrint = Path.Combine(mStrDirectoryForPrint + "/Temp/Html", mObjRequestData.IndividualID + "_template.html");
                                mHtmlDirectoryForPrint = $"{GlobalDefaultValues.DocumentLocation}/{retVal.GeneratePathForPrint}";
                                if (!Directory.Exists(mStrDirectory))
                                {
                                    Directory.CreateDirectory(mStrDirectory);
                                }
                                if (!Directory.Exists(mStrDirectory + "/Temp/Html"))
                                {
                                    Directory.CreateDirectory(mStrDirectory + "/Temp/Html");
                                }
                                if (!Directory.Exists(mStrDirectoryForPrint))
                                {
                                    Directory.CreateDirectory(mStrDirectoryForPrint);
                                }
                                if (!Directory.Exists(mStrDirectoryForPrint + "/Temp/Html"))
                                {
                                    Directory.CreateDirectory(mStrDirectoryForPrint + "/Temp/Html");
                                }
                                HtmlToPdf pdf2 = new HtmlToPdf();
                                // set converter options
                                pdf2.Options.PdfPageSize = PdfPageSize.A4;
                                pdf2.Options.PdfPageOrientation = PdfPageOrientation.Portrait;

                                pdf2.Options.WebPageWidth = 0;
                                pdf2.Options.WebPageHeight = 0;
                                pdf2.Options.WebPageFixedSize = false;

                                pdf2.Options.AutoFitWidth = HtmlToPdfPageFitMode.NoAdjustment;
                                pdf2.Options.AutoFitHeight = HtmlToPdfPageFitMode.NoAdjustment;
                                string marksheet2 = string.Empty;
                                string marksheetForPrint2 = string.Empty;
                                marksheet = System.IO.File.ReadAllText(mHtmlDirectory);
                                marksheetForPrint = System.IO.File.ReadAllText(mHtmlDirectoryForPrint);
                                marksheet = marksheet.Replace("@@third@@", imgData);
                                marksheetForPrint = marksheetForPrint.Replace("@@third@@", imgData).Replace("₦", "<span>&#8358;</span>");
                                System.IO.File.WriteAllText(mStrGeneratedHtmlPath, marksheet);
                                System.IO.File.WriteAllText(mStrGeneratedHtmlPathForPrint, marksheetForPrint);
                                PdfDocument doc2 = pdf2.ConvertHtmlString(marksheet);
                                PdfDocument doc22 = pdf2.ConvertHtmlString(marksheetForPrint);
                                var bytes2 = doc2.Save();
                                var bytes22 = doc22.Save();

                                System.IO.File.WriteAllBytes(mStrGeneratedDocumentPath, bytes2);
                                System.IO.File.WriteAllBytes(mStrGeneratedDocumentPathForPrint, bytes22);
                                ViewBag.RequestData = mObjRequestData;
                                ViewBag.pdf = mStrGeneratedDocumentPath;
                                GenerateViewModel mObjGenerateTCCModel2 = new GenerateViewModel()
                                {
                                    RequestID = mObjRequestData.TCCRequestID,
                                };

                                TCC_Request mObjUpdateStatus2 = new TCC_Request()
                                {
                                    TCCRequestID = mObjRequestData.TCCRequestID,
                                    StatusID = (int)TCCRequestStatus.Issued_eTCC,
                                    ModifiedBy = SessionManager.UserID,
                                    SEDE_DocumentID = retVal.SEDE_DocumentID,
                                    ServiceBillID = retVal.ServiceBillID,
                                    VisibleSignStatusID = SessionManager.UserID,//to holder third signer id
                                    SEDE_OrderID = (long)TCCSigningStage.Done,
                                    GeneratedPath = "ETCC/" + mObjRequestData.IndividualID + "/Signed/" + mStrGeneratedFileName,
                                    //ETCC\Print\149726
                                    // GeneratePathForPrint = "ETCC/Print/" + mObjRequestData.IndividualID + $"/{mStrGeneratedFileNameForPrint}",
                                    GeneratePathForPrint = "ETCC/Print/" + mObjRequestData.IndividualID + "/Temp/Html/" + mObjRequestData.IndividualID + "_template.html",

                                    ValidatedPath = "ETCC/" + mObjRequestData.IndividualID + "/Signed/Temp/Html/" + mObjRequestData.IndividualID + "_template.html",
                                    ModifiedDate = CommUtil.GetCurrentDateTime()
                                };

                                var mNewObjFuncResponse = mObjBLTCC.BL_UpdateRequestStatus(mObjUpdateStatus2);
                                if (mNewObjFuncResponse.Success)
                                {

                                    var holder = _db.MAP_TCCRequest_Stages.Where(o => o.RequestID == mObjRequestData.TCCRequestID).ToList();
                                    foreach (var hol in holder)
                                    {
                                        hol.ApprovalDate = DateTime.Now;
                                    }

                                }
                                Byte[] bytesArray = System.IO.File.ReadAllBytes(mStrGeneratedDocumentPath);
                                string file = Convert.ToBase64String(bytesArray);

                                var existingTcc = _db.ValidateTccs.FirstOrDefault(o => o.TccRequestId == nereqid);
                                if (existingTcc != null)
                                {
                                    existingTcc.TCCpdf = file;
                                    existingTcc.DateofTCCissued = DateTime.Now;
                                    existingTcc.DateModified = DateTime.Now;

                                }

                                //Your TCC request has been approved and ready for issuance. Kindly visit closest EIRS office for collection
                                SessionManager.Path = mStrGeneratedDocumentPath;
                                ViewBag.path = mStrGeneratedDocumentPath;
                                dcResponse["success"] = true;
                                dcResponse["Message"] = "eTCC Signed Succcessfully";
                                var taxDetailKK = _db.Individuals.FirstOrDefault(o => o.IndividualID == mObjRequestData.IndividualID);

                                string msgKK = $"Your TCC request has been approved and ready for issuance. Kindly visit closest EIRS office for collection";
                                bool blnSMSSentKK = UtilityController.SendSMS(taxDetailKK.MobileNumber1, msgKK);

                                _db.SaveChanges();
                                break;
                            default:
                                dcResponse["success"] = false;
                                dcResponse["Message"] = " eTCC As been Signed";
                                break;
                        }
                    }

                }

                return RedirectToAction("SignTCCList", "Staff");
            }
            catch (Exception ex)
            {
                Logger.SendErrorToText(ex);
                throw ex;
            }
        }
        public ActionResult SignTCCDetails(long? reqId)
        {
            if (reqId.GetValueOrDefault() > 0)
            {
                BLTCC mObjBLTCC = new BLTCC();
                usp_GetTCCRequestDetails_Result mObjRequestData = mObjBLTCC.BL_GetRequestDetails(reqId.GetValueOrDefault());
                if (mObjRequestData != null)
                {
                    ViewBag.RequestData = mObjRequestData;

                    MAP_TCCRequest_SignVisible mObjSignVisibleData = mObjBLTCC.BL_GetTCCRequestSignVisibleDetails(mObjRequestData.TCCRequestID, SessionManager.UserID, mObjRequestData.VisibleSignStatusID.GetValueOrDefault());

                    SignVisibleViewModel mObjSignVisibleModel = new SignVisibleViewModel()
                    {
                        RequestID = mObjRequestData.TCCRequestID,
                    };

                    if (mObjSignVisibleData != null)
                    {
                        mObjSignVisibleModel.RSVID = mObjSignVisibleData.RSVID;
                        mObjSignVisibleModel.AdditionalSignatureLocation = mObjSignVisibleData.AdditionalSignatureLocation;
                    }

                    usp_GetUserList_Result mObjUserData = new BLUser().BL_GetUserDetails(new MST_Users() { UserID = SessionManager.UserID, intStatus = 2 });
                    ViewBag.path = GlobalDefaultValues.DocumentLocation;
                    if (mObjUserData != null)
                    {
                        mObjSignVisibleModel.SavedSignaturePath = !string.IsNullOrWhiteSpace(mObjUserData.SignaturePath) ? mObjUserData.SignaturePath : "";
                    }

                    return View(mObjSignVisibleModel);
                }
                else
                {
                    return RedirectToAction("List", "SignTCCList");
                }
            }
            else
            {
                return RedirectToAction("List", "SignTCCList");
            }
        }

        public JsonResult SignTCCVisible(SignVisibleViewModel pObjSignVisibleModel)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();
            BLTCC mObjBLTCC = new BLTCC();

            if (!ModelState.IsValid)
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "All Fields are Required";
            }
            else
            {
                usp_GetTCCRequestDetails_Result mObjRequestData = mObjBLTCC.BL_GetRequestDetails(pObjSignVisibleModel.RequestID);

                using (TransactionScope mObjTransctionScope = new TransactionScope())
                {
                    try
                    {

                        IList<AdditionalSignature> lstAdditionalSignatures = new List<AdditionalSignature>();
                        AdditionalSignature additionalSignature = new AdditionalSignature()
                        {
                            imgSrc = pObjSignVisibleModel.ImgSrc,
                            PageNumber = 1,
                        };

                        lstAdditionalSignatures.Add(additionalSignature);

                        MAP_TCCRequest_SignVisible mObjSignVisible = new MAP_TCCRequest_SignVisible()
                        {
                            RSVID = pObjSignVisibleModel.RSVID,
                            RequestID = pObjSignVisibleModel.RequestID,
                            UserID = SessionManager.UserID,
                            SignDate = CommUtil.GetCurrentDateTime(),
                            SignSourceID = pObjSignVisibleModel.SignSourceID,
                            AdditionalSignatureLocation = JsonConvert.SerializeObject(lstAdditionalSignatures),
                            Notes = pObjSignVisibleModel.SignNotes,
                            CreatedBy = SessionManager.UserID,
                            CreatedDate = CommUtil.GetCurrentDateTime(),
                            ModifiedBy = SessionManager.UserID,
                            ModifiedDate = CommUtil.GetCurrentDateTime(),
                            Request_StageID = (int)EnumList.TCCRequestStage.Sign_eTCC_Visble,
                            ApprovalDate = CommUtil.GetCurrentDateTime(),
                            IsAction = false,
                        };

                        FuncResponse<MAP_TCCRequest_SignVisible> mObjSignFuncResponse = mObjBLTCC.BL_InsertUpdateTCCRequestSignVisible(mObjSignVisible);

                        if (mObjSignFuncResponse.Success)
                        {

                            string mStrDirectory = GlobalDefaultValues.DocumentLocation + "TCC/Request/" + mObjRequestData.TCCRequestID + "/SignVisible";
                            string mStrSignVisibleFileName = DateTime.Now.ToString("ddMMyyyy") + "_SignVisible.pdf";
                            string mStrSignVisibleDocumentPath = System.IO.Path.Combine(mStrDirectory, mStrSignVisibleFileName);

                            if (!Directory.Exists(mStrDirectory))
                            {
                                Directory.CreateDirectory(mStrDirectory);
                            }

                            string mStrDocumentPath = GlobalDefaultValues.DocumentLocation + "/" + (string.IsNullOrWhiteSpace(mObjRequestData.SignedVisiblePath) ? mObjRequestData.ValidatedPath : mObjRequestData.SignedVisiblePath);

                            mObjSignVisible.RSVID = mObjSignFuncResponse.AdditionalData.RSVID;
                            mObjSignVisible.IsAction = true;
                            mObjSignVisible.SignedVisiblePath = "TCC/Request/" + mObjRequestData.TCCRequestID + "/SignVisible/" + mStrSignVisibleFileName;

                            mObjSignFuncResponse = mObjBLTCC.BL_InsertUpdateTCCRequestSignVisible(mObjSignVisible);
                            mObjTransctionScope.Complete();
                            //}

                            dcResponse["success"] = true;
                            dcResponse["Message"] = "eTCC Signed Succcessfully";

                            //Delete temp folder
                            if (Directory.Exists(GlobalDefaultValues.DocumentLocation + "TCC/Request/" + mObjRequestData.TCCRequestID + "/SignVisible/temp"))
                            {
                                Directory.Delete(GlobalDefaultValues.DocumentLocation + "TCC/Request/" + mObjRequestData.TCCRequestID + "/SignVisible/temp", true);
                            }
                        }
                        else
                        {
                            Transaction.Current.Rollback();
                            dcResponse["success"] = false;
                            dcResponse["Message"] = mObjSignFuncResponse.Message;
                            if (mObjSignFuncResponse.Exception != null)
                            {
                                //ErrorSignal.FromCurrentContext().Raise(mObjSignFuncResponse.Exception);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.SendErrorToText(ex);
                        dcResponse["success"] = false;
                        dcResponse["Message"] = ex.Message;//"Error occurred while signing etcc";
                        Transaction.Current.Rollback();
                        //ErrorSignal.FromCurrentContext().Raise(Ex);
                    }
                }
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GenerateTCC(string reqid, string imgData, string SignSourceID)
        {
            long? nereqid = Convert.ToInt64(reqid);
            int? neSignSourceID = Convert.ToInt32(SignSourceID);
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();
            try
            {
                if (nereqid.GetValueOrDefault() > 0)
                {

                    usp_GetUserList_Result mObjUserData = new BLUser().BL_GetUserDetails(new MST_Users() { UserID = SessionManager.UserID, intStatus = 2 });

                    if (mObjUserData != null && neSignSourceID == 1)
                    {
                        string imgScrs = !string.IsNullOrWhiteSpace(mObjUserData.SignaturePath) ? mObjUserData.SignaturePath : "";
                        string fulPath = WebConfigurationManager.AppSettings["documentLocation"] + imgScrs;
                        if (!System.IO.File.Exists(fulPath))
                        {
                            dcResponse["success"] = false;
                            dcResponse["Message"] = $"Signing eTCC Failed  As User Signature {fulPath} is not found";

                            return Json(dcResponse, JsonRequestBehavior.AllowGet);
                        }
                        imgData = ImageConverter(fulPath);
                        imgData = "data:image/png;base64," + imgData;
                    }

                    BLTCC mObjBLTCC = new BLTCC();
                    var ret = mObjBLTCC.BL_GetTCCRequestGenerateDetails((long)nereqid);
                    var retVal = _db.TCC_Request.FirstOrDefault(o => o.TCCRequestID == nereqid);
                    usp_GetTCCRequestDetails_Result mObjRequestData = mObjBLTCC.BL_GetRequestDetails(nereqid.GetValueOrDefault());

                    if (mObjRequestData.SEDE_OrderID == (long)TCCSigningStage.AwaitingFirstSigner)
                    {
                        string mStrDirectory = $"{GlobalDefaultValues.DocumentLocation}/ETCC/{mObjRequestData.IndividualID}/Signed/";
                        string mStrGeneratedFileName = $"{mObjRequestData.IndividualID}_{DateTime.Now:ddMMyyyy}_Generated.pdf";//mObjTreasuryData.ReceiptRefNo + "_" + DateTime.Now.ToString("_ddMMyyyy") + "_TR.pdf";
                        string mStrGeneratedDocumentPath = Path.Combine(mStrDirectory, mStrGeneratedFileName);
                        string mStrGeneratedHtmlPath = Path.Combine(mStrDirectory + "/Temp/Html", mObjRequestData.IndividualID + "_template.html");
                        string mHtmlDirectory = $"{GlobalDefaultValues.DocumentLocation}/{mObjRequestData.ValidatedPath}";

                        //ETCC/Print/149726/Temp/Html/149726_template.html
                        string mStrDirectoryForPrint = $"{GlobalDefaultValues.DocumentLocation}/ETCC/Print/{mObjRequestData.IndividualID}";
                        string mStrGeneratedFileNameForPrint = $"{mObjRequestData.IndividualID}_{DateTime.Now:ddMMyyyy}_Generated.pdf";//mObjTreasuryData.ReceiptRefNo + "_" + DateTime.Now.ToString("_ddMMyyyy") + "_TR.pdf";
                        string mStrGeneratedDocumentPathForPrint = Path.Combine(mStrDirectoryForPrint, mStrGeneratedFileNameForPrint);
                        string mStrGeneratedHtmlPathForPrint = Path.Combine(mStrDirectoryForPrint + "/Temp/Html", mObjRequestData.IndividualID + "_template.html");
                        string mHtmlDirectoryForPrint = $"{GlobalDefaultValues.DocumentLocation}/{retVal.GeneratePathForPrint}";
                        if (!Directory.Exists(mStrDirectory))
                        {
                            Directory.CreateDirectory(mStrDirectory);
                        }
                        if (!Directory.Exists(mStrDirectory + "/Temp/Html"))
                        {
                            Directory.CreateDirectory(mStrDirectory + "/Temp/Html");
                        }
                        if (!Directory.Exists(mStrDirectoryForPrint))
                        {
                            Directory.CreateDirectory(mStrDirectoryForPrint);
                        }
                        if (!Directory.Exists(mStrDirectoryForPrint + "/Temp/Html"))
                        {
                            Directory.CreateDirectory(mStrDirectoryForPrint + "/Temp/Html");
                        }
                        HtmlToPdf pdf = new HtmlToPdf();
                        // set converter options
                        pdf.Options.PdfPageSize = PdfPageSize.A4;
                        pdf.Options.PdfPageOrientation = PdfPageOrientation.Portrait;

                        pdf.Options.WebPageWidth = 0;
                        pdf.Options.WebPageHeight = 0;
                        pdf.Options.WebPageFixedSize = false;

                        pdf.Options.AutoFitWidth = HtmlToPdfPageFitMode.NoAdjustment;
                        pdf.Options.AutoFitHeight = HtmlToPdfPageFitMode.NoAdjustment;
                        string marksheet = string.Empty;
                        string marksheetForPrint = string.Empty;
                        marksheet = System.IO.File.ReadAllText(mHtmlDirectory);
                        marksheet = marksheet.Replace("@@first@@", imgData);
                        marksheetForPrint = System.IO.File.ReadAllText(mHtmlDirectoryForPrint);
                        marksheetForPrint = marksheetForPrint.Replace("@@first@@", imgData);

                        System.IO.File.WriteAllText(mStrGeneratedHtmlPath, marksheet);
                        PdfDocument doc = pdf.ConvertHtmlString(marksheet);
                        var bytes = doc.Save();
                        System.IO.File.WriteAllText(mStrGeneratedHtmlPathForPrint, marksheetForPrint);
                        PdfDocument docII = pdf.ConvertHtmlString(marksheetForPrint);
                        var bytesII = docII.Save();

                        System.IO.File.WriteAllBytes(mStrGeneratedDocumentPath, bytes);
                        System.IO.File.WriteAllBytes(mStrGeneratedDocumentPathForPrint, bytesII);
                        ViewBag.RequestData = mObjRequestData;
                        ViewBag.pdf = mStrGeneratedDocumentPath;
                        GenerateViewModel mObjGenerateTCCModel = new GenerateViewModel()
                        {
                            RequestID = mObjRequestData.TCCRequestID,
                        };

                        TCC_Request mObjUpdateStatus = new TCC_Request()
                        {
                            TCCRequestID = mObjRequestData.TCCRequestID,
                            StatusID = (int)NewTCCRequestStage.Waiting_For_Second_Signature,
                            ModifiedBy = SessionManager.UserID,
                            SEDE_DocumentID = SessionManager.UserID,//to holder first signer id
                            SEDE_OrderID = (long)TCCSigningStage.AwaitingSecondSigner,
                            GeneratedPath = "ETCC/" + mObjRequestData.IndividualID + "/Signed/" + mStrGeneratedFileName,
                            ValidatedPath = "ETCC/" + mObjRequestData.IndividualID + "/Signed/Temp/Html/" + mObjRequestData.IndividualID + "_template.html",
                            //  RequestDate = CommUtil.GetCurrentDateTime(),
                            GeneratePathForPrint = "ETCC/Print/" + mObjRequestData.IndividualID + "/Temp/Html/" + mObjRequestData.IndividualID + "_template.html",
                            ModifiedDate = CommUtil.GetCurrentDateTime()
                        };

                        mObjBLTCC.BL_UpdateRequestStatus(mObjUpdateStatus);

                        SessionManager.Path = mStrGeneratedDocumentPath;
                        SessionManager.Path = mStrGeneratedDocumentPath;
                        ViewBag.path = mStrGeneratedDocumentPath;
                        dcResponse["success"] = true;
                        dcResponse["Message"] = "eTCC Signed Succcessfully";
                    }
                    else if (mObjRequestData.SEDE_OrderID == (long)TCCSigningStage.AwaitingSecondSigner)
                    {
                        string mStrDirectory = $"{GlobalDefaultValues.DocumentLocation}/ETCC/{mObjRequestData.IndividualID}/Signed/";
                        string mStrGeneratedFileName = $"{mObjRequestData.IndividualID}_{DateTime.Now:ddMMyyyy}_Generated.pdf";//mObjTreasuryData.ReceiptRefNo + "_" + DateTime.Now.ToString("_ddMMyyyy") + "_TR.pdf";
                        string mStrGeneratedDocumentPath = Path.Combine(mStrDirectory, mStrGeneratedFileName);
                        string mStrGeneratedHtmlPath = Path.Combine(mStrDirectory + "/Temp/Html", mObjRequestData.IndividualID + "_template.html");
                        string mHtmlDirectory = $"{GlobalDefaultValues.DocumentLocation}/{mObjRequestData.ValidatedPath}";


                        string mStrDirectoryForPrint = $"{GlobalDefaultValues.DocumentLocation}/ETCC/Print/{mObjRequestData.IndividualID}";
                        string mStrGeneratedFileNameForPrint = $"{mObjRequestData.IndividualID}_{DateTime.Now:ddMMyyyy}_Generated.pdf";//mObjTreasuryData.ReceiptRefNo + "_" + DateTime.Now.ToString("_ddMMyyyy") + "_TR.pdf";
                        string mStrGeneratedDocumentPathForPrint = Path.Combine(mStrDirectoryForPrint, mStrGeneratedFileNameForPrint);
                        string mStrGeneratedHtmlPathForPrint = Path.Combine(mStrDirectoryForPrint + "/Temp/Html", mObjRequestData.IndividualID + "_template.html");
                        string mHtmlDirectoryForPrint = $"{GlobalDefaultValues.DocumentLocation}/{retVal.GeneratePathForPrint}";
                        if (!Directory.Exists(mStrDirectory))
                        {
                            Directory.CreateDirectory(mStrDirectory);
                        }
                        if (!Directory.Exists(mStrDirectory + "/Temp/Html"))
                        {
                            Directory.CreateDirectory(mStrDirectory + "/Temp/Html");
                        }
                        if (!Directory.Exists(mStrDirectoryForPrint))
                        {
                            Directory.CreateDirectory(mStrDirectoryForPrint);
                        }
                        if (!Directory.Exists(mStrDirectoryForPrint + "/Temp/Html"))
                        {
                            Directory.CreateDirectory(mStrDirectoryForPrint + "/Temp/Html");
                        }
                        //if (!Directory.Exists(mStrDirectory))
                        //{
                        //    Directory.CreateDirectory(mStrDirectory);
                        //}
                        //if (!Directory.Exists(mStrDirectory + "/Temp/Html"))
                        //{
                        //    Directory.CreateDirectory(mStrDirectory + "/Temp/Html");
                        //}
                        HtmlToPdf pdf = new HtmlToPdf();
                        // set converter options
                        pdf.Options.PdfPageSize = PdfPageSize.A4;
                        pdf.Options.PdfPageOrientation = PdfPageOrientation.Portrait;

                        pdf.Options.WebPageWidth = 0;
                        pdf.Options.WebPageHeight = 0;
                        pdf.Options.WebPageFixedSize = false;

                        pdf.Options.AutoFitWidth = HtmlToPdfPageFitMode.NoAdjustment;
                        pdf.Options.AutoFitHeight = HtmlToPdfPageFitMode.NoAdjustment;
                        string marksheet = string.Empty;
                        string marksheetII = string.Empty;
                        marksheet = System.IO.File.ReadAllText(mHtmlDirectory);
                        marksheetII = System.IO.File.ReadAllText(mHtmlDirectoryForPrint);
                        marksheet = marksheet.Replace("@@second@@", imgData);
                        marksheetII = marksheetII.Replace("@@second@@", imgData);

                        System.IO.File.WriteAllText(mStrGeneratedHtmlPath, marksheet);
                        PdfDocument doc = pdf.ConvertHtmlString(marksheet);
                        var bytes = doc.Save();
                        System.IO.File.WriteAllText(mStrGeneratedHtmlPathForPrint, marksheetII);
                        PdfDocument docII = pdf.ConvertHtmlString(marksheetII);
                        var bytesII = docII.Save();

                        System.IO.File.WriteAllBytes(mStrGeneratedDocumentPath, bytes);
                        System.IO.File.WriteAllBytes(mStrGeneratedDocumentPathForPrint, bytesII);
                        ViewBag.RequestData = mObjRequestData;
                        ViewBag.pdf = mStrGeneratedDocumentPath;
                        GenerateViewModel mObjGenerateTCCModel = new GenerateViewModel()
                        {
                            RequestID = mObjRequestData.TCCRequestID,
                        };

                        TCC_Request mObjUpdateStatus = new TCC_Request()
                        {
                            TCCRequestID = mObjRequestData.TCCRequestID,
                            SEDE_DocumentID = retVal.SEDE_DocumentID,
                            ServiceBillID = SessionManager.UserID,//to holder second signer id
                            StatusID = (int)NewTCCRequestStage.Waiting_For_Second_Signature,
                            ModifiedBy = SessionManager.UserID,
                            SEDE_OrderID = (long)TCCSigningStage.AwaitingThirdSigner,
                            //  RequestDate = CommUtil.GetCurrentDateTime(),
                            GeneratedPath = "ETCC/" + mObjRequestData.IndividualID + "/Signed/" + mStrGeneratedFileName,
                            ValidatedPath = "ETCC/" + mObjRequestData.IndividualID + "/Signed/Temp/Html/" + mObjRequestData.IndividualID + "_template.html",
                            GeneratePathForPrint = "ETCC/Print/" + mObjRequestData.IndividualID + "/Temp/Html/" + mObjRequestData.IndividualID + "_template.html",
                            ModifiedDate = CommUtil.GetCurrentDateTime()
                        };

                        mObjBLTCC.BL_UpdateRequestStatus(mObjUpdateStatus);

                        SessionManager.Path = mStrGeneratedDocumentPath;
                        ViewBag.path = mStrGeneratedDocumentPath;
                        dcResponse["success"] = true;
                        dcResponse["Message"] = "eTCC Signed Succcessfully";

                    }
                    else if (mObjRequestData.SEDE_OrderID == (long)TCCSigningStage.AwaitingThirdSigner)
                    {
                        string mStrDirectory = $"{GlobalDefaultValues.DocumentLocation}/ETCC/{mObjRequestData.IndividualID}/Signed/";
                        string mStrGeneratedFileName = $"{mObjRequestData.IndividualID}_{DateTime.Now:ddMMyyyy}_Generated.pdf";//mObjTreasuryData.ReceiptRefNo + "_" + DateTime.Now.ToString("_ddMMyyyy") + "_TR.pdf";
                        string mStrGeneratedDocumentPath = Path.Combine(mStrDirectory, mStrGeneratedFileName);
                        string mStrGeneratedHtmlPath = Path.Combine(mStrDirectory + "/Temp/Html", mObjRequestData.IndividualID + "_template.html");
                        string mHtmlDirectory = $"{GlobalDefaultValues.DocumentLocation}/{mObjRequestData.ValidatedPath}";

                        string mStrDirectoryForPrint = $"{GlobalDefaultValues.DocumentLocation}/ETCC/Print/{mObjRequestData.IndividualID}";
                        string mStrGeneratedFileNameForPrint = $"{mObjRequestData.IndividualID}_{DateTime.Now:ddMMyyyy}_Generated.pdf";//mObjTreasuryData.ReceiptRefNo + "_" + DateTime.Now.ToString("_ddMMyyyy") + "_TR.pdf";
                        string mStrGeneratedDocumentPathForPrint = Path.Combine(mStrDirectoryForPrint, mStrGeneratedFileNameForPrint);
                        string mStrGeneratedHtmlPathForPrint = Path.Combine(mStrDirectoryForPrint + "/Temp/Html", mObjRequestData.IndividualID + "_template.html");
                        string mHtmlDirectoryForPrint = $"{GlobalDefaultValues.DocumentLocation}/{retVal.GeneratePathForPrint}";
                        if (!Directory.Exists(mStrDirectory))
                        {
                            Directory.CreateDirectory(mStrDirectory);
                        }
                        if (!Directory.Exists(mStrDirectory + "/Temp/Html"))
                        {
                            Directory.CreateDirectory(mStrDirectory + "/Temp/Html");
                        }
                        if (!Directory.Exists(mStrDirectoryForPrint))
                        {
                            Directory.CreateDirectory(mStrDirectoryForPrint);
                        }
                        if (!Directory.Exists(mStrDirectoryForPrint + "/Temp/Html"))
                        {
                            Directory.CreateDirectory(mStrDirectoryForPrint + "/Temp/Html");
                        }
                        HtmlToPdf pdf = new HtmlToPdf();
                        // set converter options
                        pdf.Options.PdfPageSize = PdfPageSize.A4;
                        pdf.Options.PdfPageOrientation = PdfPageOrientation.Portrait;

                        pdf.Options.WebPageWidth = 0;
                        pdf.Options.WebPageHeight = 0;
                        pdf.Options.WebPageFixedSize = false;

                        pdf.Options.AutoFitWidth = HtmlToPdfPageFitMode.NoAdjustment;
                        pdf.Options.AutoFitHeight = HtmlToPdfPageFitMode.NoAdjustment;
                        string marksheet = string.Empty;
                        string marksheetForPrint = string.Empty;
                        marksheet = System.IO.File.ReadAllText(mHtmlDirectory);
                        marksheetForPrint = System.IO.File.ReadAllText(mHtmlDirectoryForPrint);
                        marksheet = marksheet.Replace("@@third@@", imgData);
                        marksheetForPrint = marksheetForPrint.Replace("@@third@@", imgData).Replace("₦", "<span>&#8358;</span>");
                        System.IO.File.WriteAllText(mStrGeneratedHtmlPath, marksheet);
                        System.IO.File.WriteAllText(mStrGeneratedHtmlPathForPrint, marksheetForPrint);
                        PdfDocument doc = pdf.ConvertHtmlString(marksheet);
                        PdfDocument doc2 = pdf.ConvertHtmlString(marksheetForPrint);
                        var bytes = doc.Save();
                        var bytes2 = doc2.Save();

                        System.IO.File.WriteAllBytes(mStrGeneratedDocumentPath, bytes);
                        System.IO.File.WriteAllBytes(mStrGeneratedDocumentPathForPrint, bytes2);
                        ViewBag.RequestData = mObjRequestData;
                        ViewBag.pdf = mStrGeneratedDocumentPath;
                        GenerateViewModel mObjGenerateTCCModel = new GenerateViewModel()
                        {
                            RequestID = mObjRequestData.TCCRequestID,
                        };

                        TCC_Request mObjUpdateStatus = new TCC_Request()
                        {
                            TCCRequestID = mObjRequestData.TCCRequestID,
                            StatusID = (int)TCCRequestStatus.Issued_eTCC,
                            ModifiedBy = SessionManager.UserID,
                            SEDE_DocumentID = retVal.SEDE_DocumentID,
                            ServiceBillID = retVal.ServiceBillID,
                            VisibleSignStatusID = SessionManager.UserID,//to holder third signer id
                            SEDE_OrderID = (long)TCCSigningStage.Done,
                            GeneratedPath = "ETCC/" + mObjRequestData.IndividualID + "/Signed/" + mStrGeneratedFileName,
                            //ETCC\Print\149726
                            // GeneratePathForPrint = "ETCC/Print/" + mObjRequestData.IndividualID + $"/{mStrGeneratedFileNameForPrint}",
                            GeneratePathForPrint = "ETCC/Print/" + mObjRequestData.IndividualID + "/Temp/Html/" + mObjRequestData.IndividualID + "_template.html",

                            ValidatedPath = "ETCC/" + mObjRequestData.IndividualID + "/Signed/Temp/Html/" + mObjRequestData.IndividualID + "_template.html",
                            ModifiedDate = CommUtil.GetCurrentDateTime()
                        };

                        var mNewObjFuncResponse = mObjBLTCC.BL_UpdateRequestStatus(mObjUpdateStatus);
                        if (mNewObjFuncResponse.Success)
                        {
                            using (_db = new EIRSEntities())
                            {
                                var holder = _db.MAP_TCCRequest_Stages.Where(o => o.RequestID == mObjRequestData.TCCRequestID).ToList();
                                foreach (var hol in holder)
                                {
                                    hol.ApprovalDate = DateTime.Now;
                                }
                                _db.SaveChanges();
                            }

                        }
                        Byte[] bytesArray = System.IO.File.ReadAllBytes(mStrGeneratedDocumentPath);
                        string file = Convert.ToBase64String(bytesArray);
                        using (_db = new EIRSEntities())
                        {
                            var existingTcc = _db.ValidateTccs.FirstOrDefault(o => o.TccRequestId == nereqid);
                            if (existingTcc != null)
                            {
                                existingTcc.TCCpdf = file;
                                existingTcc.DateofTCCissued = DateTime.Now;
                                existingTcc.DateModified = DateTime.Now;


                                _db.SaveChanges();
                            }
                        }

                        SessionManager.Path = mStrGeneratedDocumentPath;
                        ViewBag.path = mStrGeneratedDocumentPath;
                        dcResponse["success"] = true;
                        dcResponse["Message"] = "eTCC Signed Succcessfully";

                    }
                    else
                    {
                        dcResponse["success"] = false;
                        dcResponse["Message"] = " eTCC As been Signed";

                    }
                    //dcResponse["success"] = true;
                    //dcResponse["Message"] = "eTCC Signed Succcessfully";
                }
                else
                {
                    dcResponse["success"] = false;
                    dcResponse["Message"] = "Signing eTCC Failed";
                }
            }
            catch (Exception ex)
            {
                var errors = ex.StackTrace;
                dcResponse["success"] = false;
                dcResponse["Message"] = $"{errors} AND {ex.Message}";
            }
            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SignCertificateList()
        {
            return View();
        }

        [HttpPost]
        public JsonResult LoadSignCertificateData()
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
            sbWhereCondition.Append(" AND ISNULL(crt.StatusID,0) >= "); sbWhereCondition.Append((int)EnumList.CertificateStatus.Validated); sbWhereCondition.Append(" ");
            sbWhereCondition.Append(" AND ((ISNULL(crt.VisibleSignStatusID,0) = 0 AND toff.Approver1 = "); sbWhereCondition.Append(SessionManager.UserID); sbWhereCondition.Append(" ) ");
            sbWhereCondition.Append(" OR (ISNULL(crt.VisibleSignStatusID,0) = 1 AND toff.Approver2 = "); sbWhereCondition.Append(SessionManager.UserID); sbWhereCondition.Append(" ) ");
            sbWhereCondition.Append(" OR (ISNULL(crt.VisibleSignStatusID,0) = 2 AND toff.Approver3 = "); sbWhereCondition.Append(SessionManager.UserID); sbWhereCondition.Append(" )) ");

            if (!string.IsNullOrEmpty(vFilter))
            {
                sbWhereCondition.Append(" AND ( crt.CertificateNumber LIKE @MainFilter ");
                sbWhereCondition.Append(" OR crtype.CertificateTypeName LIKE @MainFilter ");
                sbWhereCondition.Append(" OR crt.TaxYear LIKE @MainFilter ");
                sbWhereCondition.Append(" OR tptype.TaxPayerTypeName LIKE @MainFilter ");
                sbWhereCondition.Append(" OR dbo.GetTaxPayerName(crt.TaxPayerID,crt.TaxPayerTypeID) LIKE @MainFilter ");
                sbWhereCondition.Append(" OR dbo.GetTaxPayerRIN(crt.TaxPayerID,crt.TaxPayerTypeID) LIKE @MainFilter ");
                sbWhereCondition.Append(" OR crt.StatusName LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(REPLACE(CONVERT(varchar(50),crt.CertificateDate,106),' ','-'),'') LIKE @MainFilter )");

            }

            Certificate mObjCertificate = new Certificate()
            {
                WhereCondition = sbWhereCondition.ToString(),
                OrderBy = vSortColumn,
                OrderByDirection = vSortColumnDir,
                PageNumber = (TrynParse.parseInt(vStart) / TrynParse.parseInt(vLength)) + 1,
                PageSize = TrynParse.parseInt(vLength),
                MainFilter = !string.IsNullOrWhiteSpace(vFilter) ? "%" + vFilter.Trim() + "%" : vFilter,
                StatusIds = ""

            };

            IDictionary<string, object> dcData = new BLCertificate().BL_SearchCertificate(mObjCertificate);
            IList<usp_SearchCertificate_Result> lstCertificate = (IList<usp_SearchCertificate_Result>)dcData["CertificateList"];

            return Json(new
            {
                draw = vDraw,
                recordsFiltered = dcData["FilteredRecords"],
                recordsTotal = dcData["TotalRecords"],
                data = lstCertificate
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SignCertificateDetails(long? certid)
        {
            if (certid.GetValueOrDefault() > 0)
            {
                BLCertificate mObjBLCertificate = new BLCertificate();
                usp_GetCertificateDetails_Result mObjCertificateData = mObjBLCertificate.BL_GetCertificateDetails(certid.GetValueOrDefault());

                if (mObjCertificateData != null)
                {
                    ViewBag.CertificateData = mObjCertificateData;

                    MAP_Certificate_SignVisible mObjSignVisibleData = mObjBLCertificate.BL_GetCertificateSignVisibleDetails(mObjCertificateData.CertificateID, SessionManager.UserID, mObjCertificateData.VisibleSignStatusID.GetValueOrDefault());

                    SignVisibleViewModel mObjSignVisibleModel = new SignVisibleViewModel()
                    {
                        CertificateID = mObjCertificateData.CertificateID,
                    };

                    if (mObjSignVisibleData != null)
                    {
                        mObjSignVisibleModel.CSVID = mObjSignVisibleData.CSVID;
                        mObjSignVisibleModel.AdditionalSignatureLocation = mObjSignVisibleData.AdditionalSignatureLocation;
                    }

                    usp_GetUserList_Result mObjUserData = new BLUser().BL_GetUserDetails(new MST_Users() { UserID = SessionManager.UserID, intStatus = 2 });

                    if (mObjUserData != null)
                    {
                        mObjSignVisibleModel.SavedSignaturePath = !string.IsNullOrWhiteSpace(mObjUserData.SignaturePath) ? "/Document/" + mObjUserData.SignaturePath : "";
                    }

                    return View(mObjSignVisibleModel);
                }
                else
                {
                    return RedirectToAction("List", "SignCertificateList");
                }
            }
            else
            {
                return RedirectToAction("List", "SignCertificateList");
            }
        }

        public JsonResult SignCertificateVisible(SignVisibleViewModel pObjSignVisibleModel)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();
            BLCertificate mObjBLCertificate = new BLCertificate();

            if (!ModelState.IsValid)
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "All Fields are Required";
            }
            else
            {
                using (TransactionScope mObjTransctionScope = new TransactionScope())
                {
                    try
                    {
                        MAP_Certificate_SignVisible mObjSignVisible = new MAP_Certificate_SignVisible()
                        {
                            CSVID = pObjSignVisibleModel.CSVID,
                            CertificateID = pObjSignVisibleModel.CertificateID,
                            UserID = SessionManager.UserID,
                            SignDate = CommUtil.GetCurrentDateTime(),
                            SignSourceID = pObjSignVisibleModel.SignSourceID,
                            AdditionalSignatureLocation = pObjSignVisibleModel.AdditionalSignatureLocation,
                            Notes = pObjSignVisibleModel.SignNotes,
                            DocumentWidth = pObjSignVisibleModel.DocumentWidth.ToString(),
                            CreatedBy = SessionManager.UserID,
                            CreatedDate = CommUtil.GetCurrentDateTime(),
                            ModifiedBy = SessionManager.UserID,
                            ModifiedDate = CommUtil.GetCurrentDateTime(),
                            Request_StageID = (int)EnumList.CertificateStage.Sign_Visible,
                            ApprovalDate = CommUtil.GetCurrentDateTime(),
                            IsAction = false,
                        };


                        FuncResponse<MAP_Certificate_SignVisible> mObjSignFuncResponse = mObjBLCertificate.BL_InsertUpdateCertificateSignVisible(mObjSignVisible);

                        if (mObjSignFuncResponse.Success)
                        {
                            usp_GetCertificateDetails_Result mObjCertificateData = mObjBLCertificate.BL_GetCertificateDetails(pObjSignVisibleModel.CertificateID);

                            //Update PDF With Sign
                            if (!string.IsNullOrWhiteSpace(pObjSignVisibleModel.AdditionalSignatureLocation))
                            {
                                string mStrDirectory = GlobalDefaultValues.DocumentLocation + "Certificate/" + mObjCertificateData.CertificateID + "/SignVisible";
                                string mStrSignVisibleFileName = DateTime.Now.ToString("ddMMyyyy") + "_SignVisible.pdf";
                                string mStrSignVisibleDocumentPath = System.IO.Path.Combine(mStrDirectory, mStrSignVisibleFileName);

                                if (!Directory.Exists(mStrDirectory))
                                {
                                    Directory.CreateDirectory(mStrDirectory);
                                }

                                string mStrDocumentPath = GlobalDefaultValues.DocumentLocation + "/" + (string.IsNullOrWhiteSpace(mObjCertificateData.SignedVisiblePath) ? mObjCertificateData.ValidatedPath : mObjCertificateData.SignedVisiblePath);


                                mObjSignVisible.CSVID = mObjSignFuncResponse.AdditionalData.CSVID;
                                mObjSignVisible.IsAction = true;
                                mObjSignVisible.SignedVisiblePath = "Certificate/" + mObjCertificateData.CertificateID + "/SignVisible/" + mStrSignVisibleFileName;

                                mObjSignFuncResponse = mObjBLCertificate.BL_InsertUpdateCertificateSignVisible(mObjSignVisible);
                                mObjTransctionScope.Complete();
                            }

                            dcResponse["success"] = true;
                            dcResponse["Message"] = "Certificate Signed Succcessfully";
                        }
                        else
                        {
                            Transaction.Current.Rollback();
                            dcResponse["success"] = false;
                            dcResponse["Message"] = mObjSignFuncResponse.Message;
                            if (mObjSignFuncResponse.Exception != null)
                            {
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.SendErrorToText(ex);
                        dcResponse["success"] = false;
                        dcResponse["Message"] = ex.Message;
                        Transaction.Current.Rollback();
                    }
                }
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }
        public string ImageConverter(string path)
        {
            using (System.Drawing.Image image = System.Drawing.Image.FromFile(path))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();

                    // Convert byte[] to Base64 String
                    string base64String = Convert.ToBase64String(imageBytes);
                    return base64String;
                }
            }
        }
        static string DocumentHTMLLocation = WebConfigurationManager.AppSettings["documentHTMLLocation"] ?? "";
    }
}