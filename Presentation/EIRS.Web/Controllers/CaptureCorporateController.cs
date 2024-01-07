using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;
using EIRS.Repository;
using EIRS.Web.Models;
using EIRS.Web.Utility;
using Elmah;
using Microsoft.Reporting.WebForms;
using Newtonsoft.Json;
using SelectPdf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Transactions;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Services.Description;
using Vereyon.Web;
using static EIRS.Web.Controllers.Filters;

namespace EIRS.Web.Controllers
{
    [SessionTimeout]
    public class CaptureCorporateController : BaseController
    {
        IAssessmentRepository _AssessmentRepository;
        IAdjustmentRepository _AdjustmentRepository;
        EIRSEntities _db = new EIRSEntities();
        public CaptureCorporateController()
        {
            _AssessmentRepository = new AssessmentRepository();
            _AdjustmentRepository = new AdjustmentRepository();
        }
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
                sbWhereCondition.Append(" AND ( ISNULL(CompanyRIN,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(CompanyName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(TIN,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(ContactAddress,'') LIKE @MainFilter)");
            }

            Company mObjCompany = new Company()
            {
                WhereCondition = sbWhereCondition.ToString(),
                OrderBy = vSortColumn,
                OrderByDirection = vSortColumnDir,
                PageNumber = (TrynParse.parseInt(vStart) / TrynParse.parseInt(vLength)) + 1,
                PageSize = TrynParse.parseInt(vLength),
                MainFilter = !string.IsNullOrWhiteSpace(vFilter) ? "%" + vFilter.Trim() + "%" : vFilter
            };


            IDictionary<string, object> dcData = new BLCompany().BL_SearchCompanyForSideMenu(mObjCompany);
            IList<usp_SearchCompanyForSideMenu_Result> lstCompany = (IList<usp_SearchCompanyForSideMenu_Result>)dcData["CompanyList"];

            return Json(new
            {
                draw = vDraw,
                recordsFiltered = dcData["FilteredRecords"],
                recordsTotal = dcData["TotalRecords"],
                data = lstCompany
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
                sbWhereCondition.Append(" AND ( ISNULL(CompanyRIN,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(CompanyName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(TIN,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(ContactAddress,'') LIKE @MainFilter)");
            }

            Company mObjCompany = new Company()
            {
                WhereCondition = sbWhereCondition.ToString(),
                OrderBy = vSortColumn,
                OrderByDirection = vSortColumnDir,
                PageNumber = (TrynParse.parseInt(vStart) / TrynParse.parseInt(vLength)) + 1,
                PageSize = TrynParse.parseInt(vLength),
                MainFilter = !string.IsNullOrWhiteSpace(vFilter) ? "%" + vFilter.Trim() + "%" : vFilter
            };


            IDictionary<string, object> dcData = new BLCompany().BL_SearchCompanyForSideMenu(mObjCompany);
            IList<usp_SearchCompanyForSideMenu_Result> lstCompany = (IList<usp_SearchCompanyForSideMenu_Result>)dcData["CompanyList"];

            return Json(new
            {
                draw = vDraw,
                recordsFiltered = dcData["FilteredRecords"],
                recordsTotal = dcData["TotalRecords"],
                data = lstCompany
            }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult ExportData()
        {
            IList<usp_GetCompanyList_Result> lstCompanyData = new BLCompany().BL_GetCompanyList(new Company() { intStatus = 2 });
            string[] strColumns = new string[] { "CompanyRIN", "CompanyName", "TIN", "MobileNumber1", "MobileNumber2", "EmailAddress1", "EmailAddress2", "TaxOfficeName", "TaxPayerTypeName", "EconomicActivitiesName", "NotificationMethodName", "ContactAddress", "CACRegistrationNumber", "ActiveText" };
            return ExportToExcel(lstCompanyData, this.RouteData, strColumns, "Corporate");
        }


        public ActionResult Search()
        {
            string url = getUrl();
            bool itCan = new UtilityController().CheckAccess(url);
            if (!itCan) { return RedirectToAction("AccessDenied", "Utility"); }
            return View();
        }


        [HttpPost]
        public ActionResult Search(FormCollection pObjCollection)
        {
            string mStrCompanyName = pObjCollection.Get("txtCompanyName");
            string mStrMobileNumber = pObjCollection.Get("txtMobileNumber");
            string mStrRIN = pObjCollection.Get("txtRIN");
            Company mObjCompany = new Company()
            {
                CompanyName = mStrCompanyName,
                MobileNumber1 = mStrMobileNumber,
                TIN = mStrRIN,
                CompanyRIN = mStrRIN,
                CACRegistrationNumber = mStrRIN,
                intStatus = 1
            };

            IList<usp_GetCompanyList_Result> lstCompany = new BLCompany().BL_GetCompanyList(mObjCompany);
            return PartialView("_BindTable", lstCompany.Take(5).ToList());
        }

        public void UI_FillDropDown(CompanyViewModel pObjCompanyViewModel = null)
        {
            if (pObjCompanyViewModel != null)
            {
                pObjCompanyViewModel.TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies;
            }
            else if (pObjCompanyViewModel == null)
            {
                pObjCompanyViewModel = new CompanyViewModel();
            }

            UI_FillTaxOfficeDropDown(new Tax_Offices() { intStatus = 1, IncludeTaxOfficeIds = pObjCompanyViewModel.TaxOfficeID.ToString() });
            UI_FillTaxPayerTypeDropDown(new TaxPayer_Types() { intStatus = 1, IncludeTaxPayerTypeIds = pObjCompanyViewModel.TaxPayerTypeID.ToString() }, (int)EnumList.TaxPayerType.Companies);
            UI_FillEconomicActivitiesDropDown(new Economic_Activities() { intStatus = 1, IncludeEconomicActivitiesIds = pObjCompanyViewModel.EconomicActivitiesID.ToString(), TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies });
            UI_FillNotificationMethodDropDown(new Notification_Method() { intStatus = 1, IncludeNotificationMethodIds = pObjCompanyViewModel.NotificationMethodID.ToString() });
        }


        public ActionResult Add()
        {
            string url = getUrl();
            bool itCan = new UtilityController().CheckAccess(url);
            if (!itCan) { return RedirectToAction("AccessDenied", "Utility"); }
            CompanyViewModel ivm = new CompanyViewModel()
            {
                TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies
            };
            UI_FillDropDown(ivm);
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(CompanyViewModel pObjCompanyModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown(pObjCompanyModel);
                return View(pObjCompanyModel);
            }
            else
            {
                var checker = _db.Companies.FirstOrDefault(o => o.CACRegistrationNumber == pObjCompanyModel.CACRegistrationNumber);
                if (checker != null)
                {
                    FlashMessage.Info("CAC Already Exist!!");
                }
                Company mObjCompany = new Company()
                {
                    CompanyID = 0,
                    CompanyName = pObjCompanyModel.CompanyName,
                    TIN = pObjCompanyModel.TIN,
                    MobileNumber1 = pObjCompanyModel.MobileNumber1,
                    MobileNumber2 = pObjCompanyModel.MobileNumber2,
                    EmailAddress1 = pObjCompanyModel.EmailAddress1,
                    EmailAddress2 = pObjCompanyModel.EmailAddress2,
                    TaxOfficeID = pObjCompanyModel.TaxOfficeID,
                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies,
                    CompanyRIN = pObjCompanyModel.CompanyRIN,
                    EconomicActivitiesID = pObjCompanyModel.EconomicActivitiesID,
                    NotificationMethodID = pObjCompanyModel.NotificationMethodID,
                    ContactAddress = pObjCompanyModel.ContactAddress,
                    CACRegistrationNumber = pObjCompanyModel.CACRegistrationNumber,
                    Active = true,
                    CreatedBy = SessionManager.UserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {
                    FuncResponse<Company> mObjResponse = new BLCompany().BL_InsertUpdateCompany(mObjCompany);

                    if (mObjResponse.Success)
                    {
                        if (GlobalDefaultValues.SendNotification)
                        {
                            //Send Notification
                            EmailDetails mObjEmailDetails = new EmailDetails()
                            {
                                TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies,
                                TaxPayerTypeName = "Company",
                                TaxPayerID = pObjCompanyModel.CompanyID,
                                TaxPayerName = pObjCompanyModel.CompanyName,
                                TaxPayerRIN = pObjCompanyModel.CompanyRIN,
                                TaxPayerMobileNumber = pObjCompanyModel.MobileNumber1,
                                TaxPayerEmail = pObjCompanyModel.EmailAddress1,
                                ContactAddress = pObjCompanyModel.ContactAddress,
                                TaxPayerTIN = pObjCompanyModel.TIN,
                                LoggedInUserID = SessionManager.UserID,
                            };

                            if (!string.IsNullOrWhiteSpace(pObjCompanyModel.EmailAddress1))
                            {
                                BLEmailHandler.BL_TaxPayerCreated(mObjEmailDetails);
                            }

                            if (!string.IsNullOrWhiteSpace(pObjCompanyModel.MobileNumber1))
                            {
                                UtilityController.BL_TaxPayerCreated(mObjEmailDetails);
                            }
                        }

                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("Details", "CaptureCorporate", new { id = mObjResponse.AdditionalData.CompanyID, name = mObjResponse.AdditionalData.CompanyName.ToSeoUrl() });
                    }
                    else
                    {
                        UI_FillDropDown(pObjCompanyModel);
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjCompanyModel);
                    }
                }
                catch (Exception ex)
                {
                    Logger.SendErrorToText(ex);
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillDropDown(pObjCompanyModel);
                    ViewBag.Message = "Error occurred while saving corporate";
                    return View(pObjCompanyModel);
                }
            }
        }

        //
        public ActionResult AddWithoutNumber()
        {
            string url = getUrl();
            bool itCan = new UtilityController().CheckAccess(url);
            if (!itCan) { return RedirectToAction("AccessDenied", "Utility"); }
            UI_FillDropDown();
            CompanyViewModel mObjCompanyModel = new CompanyViewModel()
            {
                TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies,
                MobileNumber1 = "4000000000"
            };

            return View(mObjCompanyModel);
        }

        //
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddWithoutNumber(CompanyViewModel pObjCompanyModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown(pObjCompanyModel);
                return View(pObjCompanyModel);
            }
            else
            {
                Company mObjCompany = new Company()
                {
                    CompanyID = 0,
                    CompanyName = pObjCompanyModel.CompanyName,
                    TIN = pObjCompanyModel.TIN,
                    MobileNumber1 = pObjCompanyModel.MobileNumber1,
                    MobileNumber2 = pObjCompanyModel.MobileNumber2,
                    EmailAddress1 = pObjCompanyModel.EmailAddress1,
                    EmailAddress2 = pObjCompanyModel.EmailAddress2,
                    TaxOfficeID = pObjCompanyModel.TaxOfficeID,
                    CompanyRIN = pObjCompanyModel.CompanyRIN,
                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies,
                    EconomicActivitiesID = pObjCompanyModel.EconomicActivitiesID,
                    NotificationMethodID = pObjCompanyModel.NotificationMethodID,
                    ContactAddress = pObjCompanyModel.ContactAddress,
                    CACRegistrationNumber = pObjCompanyModel.CACRegistrationNumber,
                    Active = true,
                    CreatedBy = SessionManager.UserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse<Company> mObjResponse = new BLCompany().BL_InsertUpdateCompany(mObjCompany, true, true);

                    if (mObjResponse.Success)
                    {
                        if (GlobalDefaultValues.SendNotification)
                        {
                            //Send Notification
                            EmailDetails mObjEmailDetails = new EmailDetails()
                            {
                                TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies,
                                TaxPayerTypeName = "Company",
                                TaxPayerID = pObjCompanyModel.CompanyID,
                                TaxPayerName = pObjCompanyModel.CompanyName,
                                TaxPayerRIN = pObjCompanyModel.CompanyRIN,
                                TaxPayerMobileNumber = pObjCompanyModel.MobileNumber1,
                                TaxPayerEmail = pObjCompanyModel.EmailAddress1,
                                ContactAddress = pObjCompanyModel.ContactAddress,
                                TaxPayerTIN = pObjCompanyModel.TIN,
                                LoggedInUserID = SessionManager.UserID,
                            };

                            if (!string.IsNullOrWhiteSpace(pObjCompanyModel.EmailAddress1))
                            {
                                BLEmailHandler.BL_TaxPayerCreated(mObjEmailDetails);
                            }

                            if (!string.IsNullOrWhiteSpace(pObjCompanyModel.MobileNumber1))
                            {
                                UtilityController.BL_TaxPayerCreated(mObjEmailDetails);
                            }
                        }
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("Details", "CaptureCorporate", new { id = mObjResponse.AdditionalData.CompanyID, name = mObjResponse.AdditionalData.CompanyName.ToSeoUrl() });
                    }
                    else
                    {
                        UI_FillDropDown(pObjCompanyModel);
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjCompanyModel);
                    }
                }
                catch (Exception ex)
                {
                    Logger.SendErrorToText(ex);
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillDropDown(pObjCompanyModel);
                    ViewBag.Message = "Error occurred while saving corporate";
                    return View(pObjCompanyModel);
                }
            }
        }


        public ActionResult Edit(int? id, string name)
        {
            string url = getUrl();
            bool itCan = new UtilityController().CheckAccess(url);
            if (!itCan) { return RedirectToAction("AccessDenied", "Utility"); }
            if (id.GetValueOrDefault() > 0)
            {
                Company mObjCompany = new Company()
                {
                    CompanyID = id.GetValueOrDefault(),
                    intStatus = 1
                };

                usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(mObjCompany);

                if (mObjCompanyData != null)
                {
                    CompanyViewModel mObjCompanyModelView = new CompanyViewModel()
                    {
                        CompanyID = mObjCompanyData.CompanyID.GetValueOrDefault(),
                        CompanyRIN = mObjCompanyData.CompanyRIN,
                        CompanyName = mObjCompanyData.CompanyName,
                        TIN = mObjCompanyData.TIN,
                        MobileNumber1 = mObjCompanyData.MobileNumber1,
                        MobileNumber2 = mObjCompanyData.MobileNumber2,
                        EmailAddress1 = mObjCompanyData.EmailAddress1,
                        EmailAddress2 = mObjCompanyData.EmailAddress2,
                        TaxOfficeID = mObjCompanyData.TaxOfficeID,
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies,
                        EconomicActivitiesID = mObjCompanyData.EconomicActivitiesID.GetValueOrDefault(),
                        NotificationMethodID = mObjCompanyData.NotificationMethodID.GetValueOrDefault(),
                        ContactAddress = mObjCompanyData.ContactAddress,
                        CACRegistrationNumber = mObjCompanyData.CACRegistrationNumber,
                        Active = mObjCompanyData.Active.GetValueOrDefault(),
                    };

                    UI_FillDropDown(mObjCompanyModelView);
                    return View(mObjCompanyModelView);
                }
                else
                {
                    return RedirectToAction("Search", "CaptureCorporate");
                }
            }
            else
            {
                return RedirectToAction("Search", "CaptureCorporate");
            }
        }

        [HttpPost()]

        [ValidateAntiForgeryToken()]
        public ActionResult Edit(CompanyViewModel pObjCompanyModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown(pObjCompanyModel);
                return View(pObjCompanyModel);
            }
            else
            {
                Company mObjCompany = new Company()
                {
                    CompanyID = pObjCompanyModel.CompanyID,
                    CompanyName = pObjCompanyModel.CompanyName,
                    TIN = pObjCompanyModel.TIN,
                    MobileNumber1 = pObjCompanyModel.MobileNumber1,
                    MobileNumber2 = pObjCompanyModel.MobileNumber2,
                    EmailAddress1 = pObjCompanyModel.EmailAddress1,
                    EmailAddress2 = pObjCompanyModel.EmailAddress2,
                    TaxOfficeID = pObjCompanyModel.TaxOfficeID,
                    CompanyRIN = pObjCompanyModel.CompanyRIN,
                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies,
                    EconomicActivitiesID = pObjCompanyModel.EconomicActivitiesID,
                    NotificationMethodID = pObjCompanyModel.NotificationMethodID,
                    ContactAddress = pObjCompanyModel.ContactAddress,
                    CACRegistrationNumber = pObjCompanyModel.CACRegistrationNumber,
                    Active = true,
                    ModifiedBy = SessionManager.UserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse<Company> mObjResponse = new BLCompany().BL_InsertUpdateCompany(mObjCompany);

                    if (mObjResponse.Success)
                    {
                        if (GlobalDefaultValues.SendNotification)
                        {
                            //Send Notification
                            EmailDetails mObjEmailDetails = new EmailDetails()
                            {
                                TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies,
                                TaxPayerTypeName = "Company",
                                TaxPayerID = pObjCompanyModel.CompanyID,
                                TaxPayerName = pObjCompanyModel.CompanyName,
                                TaxPayerRIN = pObjCompanyModel.CompanyRIN,
                                TaxPayerMobileNumber = pObjCompanyModel.MobileNumber1,
                                TaxPayerEmail = pObjCompanyModel.EmailAddress1,
                                ContactAddress = pObjCompanyModel.ContactAddress,
                                TaxPayerTIN = pObjCompanyModel.TIN,
                                LoggedInUserID = SessionManager.UserID,
                            };

                            if (!string.IsNullOrWhiteSpace(pObjCompanyModel.EmailAddress1))
                            {
                                BLEmailHandler.BL_TaxPayerCreated(mObjEmailDetails);
                            }

                            if (!string.IsNullOrWhiteSpace(pObjCompanyModel.MobileNumber1))
                            {
                                UtilityController.BL_TaxPayerCreated(mObjEmailDetails);
                            }
                        }
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("Details", "CaptureCorporate", new { id = mObjResponse.AdditionalData.CompanyID, name = mObjResponse.AdditionalData.CompanyRIN.ToSeoUrl() });
                    }
                    else
                    {
                        UI_FillDropDown(pObjCompanyModel);
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjCompanyModel);
                    }
                }
                catch (Exception ex)
                {
                    Logger.SendErrorToText(ex);
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillDropDown(pObjCompanyModel);
                    ViewBag.Message = "Error occurred while saving company";
                    return View(pObjCompanyModel);
                }
            }
        }


        public ActionResult Details(int? id, string name)
        {
            string url = getUrl();
            decimal? newAmount = 0;
            bool itCan = new UtilityController().CheckAccess(url);
            if (!itCan) { return RedirectToAction("AccessDenied", "Utility"); }
            if (id.GetValueOrDefault() > 0)
            {
                Company mObjCompany = new Company()
                {
                    CompanyID = id.GetValueOrDefault(),
                    intStatus = 1
                };

                usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(mObjCompany);

                if (mObjCompanyData != null)
                {
                    //CompanyViewModel mObjCompanyModelView = new CompanyViewModel()
                    //{
                    //    CompanyID = mObjCompanyData.CompanyID.GetValueOrDefault(),
                    //    CompanyRIN = mObjCompanyData.CompanyRIN,
                    //    CompanyName = mObjCompanyData.CompanyName,
                    //    TIN = mObjCompanyData.TIN,
                    //    MobileNumber1 = mObjCompanyData.MobileNumber1,
                    //    MobileNumber2 = mObjCompanyData.MobileNumber2,
                    //    EmailAddress1 = mObjCompanyData.EmailAddress1,
                    //    EmailAddress2 = mObjCompanyData.EmailAddress2,
                    //    TaxOfficeName = mObjCompanyData.TaxOfficeName,
                    //    TaxPayerTypeName = mObjCompanyData.TaxPayerTypeName,
                    //    EconomicActivitiesName = mObjCompanyData.EconomicActivitiesName,
                    //    NotificationMethodName = mObjCompanyData.NotificationMethodName,
                    //    ContactAddress = mObjCompanyData.ContactAddress,
                    //    Active = mObjCompanyData.Active.GetValueOrDefault(),
                    //    ActiveText = mObjCompanyData.ActiveText
                    //};
                    List<AdjustmentResponse> adjustmentResponse = new List<AdjustmentResponse>();
                    List<AdjustmentResponse> serviceResponse = new List<AdjustmentResponse>();
                    adjustmentResponse = _AdjustmentRepository.GetAdjustmentResponse();
                    serviceResponse = _AdjustmentRepository.GetAdjustmentServiceResponse();
                    IList<usp_GetTaxPayerBill_Result> lstTaxPayerBill = new BLAssessment().BL_GetTaxPayerBill(id.GetValueOrDefault(), (int)EnumList.TaxPayerType.Companies, 0);

                    var assBill = lstTaxPayerBill.Where(o => o.BillRefNo.StartsWith("AB")).ToList();
                    foreach (var ass in assBill)
                    {
                        var aaiid = _AdjustmentRepository.GetListOfItemId(ass.BillID.Value);
                        if (aaiid.Count > 1)
                        {
                            foreach (var aa in aaiid)
                            {
                                newAmount = adjustmentResponse.Where(o => o.AAIID == aa).Select(x => x.TotalAmount).FirstOrDefault();
                            }
                        }
                        else
                        {
                            newAmount = adjustmentResponse.Where(o => o.AAIID == aaiid.FirstOrDefault()).Select(x => x.TotalAmount).FirstOrDefault();
                        }
                        newAmount = newAmount.HasValue ? newAmount : 0;
                        ass.BillAmount = ass.BillAmount + newAmount;
                    }
                    var serviceBill = lstTaxPayerBill.Where(o => o.BillRefNo.StartsWith("SB")).ToList();
                    foreach (var ass in serviceBill)
                    {
                        var aaiid = _AdjustmentRepository.GetListOfServiceItemId(ass.BillID.Value);
                        if (aaiid.Count > 1)
                        {
                            foreach (var aa in aaiid)
                            {
                                newAmount = serviceResponse.Where(o => o.SBIID == aa).Select(x => x.TotalAmount).FirstOrDefault();
                            }
                        }
                        else
                        {
                            newAmount = serviceResponse.Where(o => o.SBIID == aaiid.FirstOrDefault()).Select(x => x.TotalAmount).FirstOrDefault();
                        }
                        newAmount = newAmount.HasValue ? newAmount : 0;
                        ass.BillAmount = ass.BillAmount + newAmount;
                    }
                    ViewBag.TaxPayerBill = lstTaxPayerBill;


                    MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                    {
                        TaxPayerID = id.GetValueOrDefault(),
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies
                    };

                    IList<usp_GetTaxPayerAssetList_Result> lstTaxPayerAsset = new BLTaxPayerAsset().BL_GetTaxPayerAssetList(mObjTaxPayerAsset);
                    ViewBag.AssetList = lstTaxPayerAsset;

                    IList<usp_GetTaxPayerDocumentList_Result> lstTaxPayerDocument = new BLTaxPayerDocument().BL_GetTaxPayerDocumentList(new MAP_TaxPayer_Document()
                    {
                        TaxPayerID = id.GetValueOrDefault(),
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies
                    });
                    ViewBag.DocumentList = lstTaxPayerDocument;

                    IList<usp_GetAssessmentRuleInformation_Result> lstAssessmentRuleInformation = new BLTaxPayerAsset().BL_GetTaxPayerAssessmentRuleInformation((int)EnumList.TaxPayerType.Companies, id.GetValueOrDefault());
                    ViewBag.AssessmentRuleInformation = lstAssessmentRuleInformation;


                    IList<usp_GetTaxPayerPayment_Result> lstTaxPayerPayment = new BLSettlement().BL_GetTaxPayerPayment(id.GetValueOrDefault(), (int)EnumList.TaxPayerType.Companies);
                    ViewBag.TaxPayerPayment = lstTaxPayerPayment;

                    IList<usp_GetTaxPayerMDAService_Result> lstMDAService = new BLMDAService().BL_GetTaxPayerMDAService((int)EnumList.TaxPayerType.Companies, id.GetValueOrDefault());
                    ViewBag.MDAService = lstMDAService;

                    decimal dcPoABalance = new BLPaymentAccount().BL_GetWalletBalance((int)EnumList.TaxPayerType.Companies, id.GetValueOrDefault());
                    ViewBag.PoABalance = dcPoABalance;

                    IList<usp_GetProfileInformation_Result> lstProfileInformation = new BLTaxPayerAsset().BL_GetTaxPayerProfileInformation((int)EnumList.TaxPayerType.Companies, id.GetValueOrDefault());
                    ViewBag.ProfileInformation = lstProfileInformation;

                    return View(mObjCompanyData);
                }
                else
                {
                    return RedirectToAction("Search", "CaptureCorporate");
                }
            }
            else
            {
                return RedirectToAction("Search", "CaptureCorporate");
            }
        }


        public ActionResult SearchBuilding(int? id, string name)
        {
            string url = getUrl();
            bool itCan = new UtilityController().CheckAccess(url);
            if (!itCan) { return RedirectToAction("AccessDenied", "Utility"); }
            if (id.GetValueOrDefault() > 0)
            {
                Company mObjCompany = new Company()
                {
                    CompanyID = id.GetValueOrDefault(),
                    intStatus = 1
                };

                usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(mObjCompany);

                if (mObjCompanyData != null)
                {
                    CompanyViewModel mObjCompanyModelView = new CompanyViewModel()
                    {
                        CompanyID = mObjCompanyData.CompanyID.GetValueOrDefault(),
                        CompanyRIN = mObjCompanyData.CompanyRIN,
                        CompanyName = mObjCompanyData.CompanyName,
                        TIN = mObjCompanyData.TIN,
                        MobileNumber1 = mObjCompanyData.MobileNumber1,
                        MobileNumber2 = mObjCompanyData.MobileNumber2,
                        EmailAddress1 = mObjCompanyData.EmailAddress1,
                        EmailAddress2 = mObjCompanyData.EmailAddress2,
                        TaxOfficeName = mObjCompanyData.TaxOfficeName,
                        TaxPayerTypeName = mObjCompanyData.TaxPayerTypeName,
                        EconomicActivitiesName = mObjCompanyData.EconomicActivitiesName,
                        NotificationMethodName = mObjCompanyData.NotificationMethodName,
                        ContactAddress = mObjCompanyData.ContactAddress,
                        ActiveText = mObjCompanyData.ActiveText
                    };

                    return View(mObjCompanyModelView);
                }
                else
                {
                    return RedirectToAction("Search", "CaptureCorporate");
                }
            }
            else
            {
                return RedirectToAction("Search", "CaptureCorporate");
            }
        }

        [HttpPost]

        public ActionResult SearchBuilding(FormCollection pObjFormCollection)
        {
            string mStrBuildingName = pObjFormCollection.Get("txtBuildingName");
            string mStrStreetName = pObjFormCollection.Get("txtStreetName");
            string mStrRIN = pObjFormCollection.Get("txtRIN");

            Building mObjBuilding = new Building()
            {
                BuildingRIN = mStrRIN,
                BuildingName = mStrBuildingName,
                StreetName = mStrStreetName,
                intStatus = 1
            };

            IList<usp_GetBuildingList_Result> lstBuilding = new BLBuilding().BL_GetBuildingList(mObjBuilding);
            return PartialView("_BindBuildingTable_SingleSelect", lstBuilding.Take(5).ToList());
        }


        public ActionResult SearchBusiness(int? id, string name)
        {
            string url = getUrl();
            bool itCan = new UtilityController().CheckAccess(url);
            if (!itCan) { return RedirectToAction("AccessDenied", "Utility"); }
            if (id.GetValueOrDefault() > 0)
            {
                Company mObjCompany = new Company()
                {
                    CompanyID = id.GetValueOrDefault(),
                    intStatus = 1
                };

                usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(mObjCompany);

                if (mObjCompanyData != null)
                {
                    CompanyViewModel mObjCompanyModelView = new CompanyViewModel()
                    {
                        CompanyID = mObjCompanyData.CompanyID.GetValueOrDefault(),
                        CompanyRIN = mObjCompanyData.CompanyRIN,
                        CompanyName = mObjCompanyData.CompanyName,
                        TIN = mObjCompanyData.TIN,
                        MobileNumber1 = mObjCompanyData.MobileNumber1,
                        MobileNumber2 = mObjCompanyData.MobileNumber2,
                        EmailAddress1 = mObjCompanyData.EmailAddress1,
                        EmailAddress2 = mObjCompanyData.EmailAddress2,
                        TaxOfficeName = mObjCompanyData.TaxOfficeName,
                        TaxPayerTypeName = mObjCompanyData.TaxPayerTypeName,
                        EconomicActivitiesName = mObjCompanyData.EconomicActivitiesName,
                        NotificationMethodName = mObjCompanyData.NotificationMethodName,
                        ContactAddress = mObjCompanyData.ContactAddress,
                        ActiveText = mObjCompanyData.ActiveText
                    };

                    return View(mObjCompanyModelView);
                }
                else
                {
                    return RedirectToAction("Search", "CaptureCorporate");
                }
            }
            else
            {
                return RedirectToAction("Search", "CaptureCorporate");
            }
        }

        [HttpPost]

        public ActionResult SearchBusiness(FormCollection pObjFormCollection)
        {
            string mStrBusinessName = pObjFormCollection.Get("txtBusinessName");
            string mStrBusinessAddress = pObjFormCollection.Get("txtBusinessAddress");
            string mStrRIN = pObjFormCollection.Get("txtRIN");

            Business mObjBusiness = new Business()
            {
                BusinessRIN = mStrRIN,
                BusinessName = mStrBusinessName,
                BusinessAddress = mStrBusinessAddress,
                intStatus = 1
            };

            IList<usp_GetBusinessListNewTy_Result> lstBusiness = new BLBusiness().BL_GetBusinessList(mObjBusiness);
            return PartialView("_BindBusinessTable_SingleSelect", lstBusiness.Take(5).ToList());
        }


        public ActionResult SearchLand(int? id, string name)
        {
            string url = getUrl();
            bool itCan = new UtilityController().CheckAccess(url);
            if (!itCan) { return RedirectToAction("AccessDenied", "Utility"); }
            if (id.GetValueOrDefault() > 0)
            {
                Company mObjCompany = new Company()
                {
                    CompanyID = id.GetValueOrDefault(),
                    intStatus = 1
                };

                usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(mObjCompany);

                if (mObjCompanyData != null)
                {
                    CompanyViewModel mObjCompanyModelView = new CompanyViewModel()
                    {
                        CompanyID = mObjCompanyData.CompanyID.GetValueOrDefault(),
                        CompanyRIN = mObjCompanyData.CompanyRIN,
                        CompanyName = mObjCompanyData.CompanyName,
                        TIN = mObjCompanyData.TIN,
                        MobileNumber1 = mObjCompanyData.MobileNumber1,
                        MobileNumber2 = mObjCompanyData.MobileNumber2,
                        EmailAddress1 = mObjCompanyData.EmailAddress1,
                        EmailAddress2 = mObjCompanyData.EmailAddress2,
                        TaxOfficeName = mObjCompanyData.TaxOfficeName,
                        TaxPayerTypeName = mObjCompanyData.TaxPayerTypeName,
                        EconomicActivitiesName = mObjCompanyData.EconomicActivitiesName,
                        NotificationMethodName = mObjCompanyData.NotificationMethodName,
                        ContactAddress = mObjCompanyData.ContactAddress,
                        ActiveText = mObjCompanyData.ActiveText
                    };

                    return View(mObjCompanyModelView);
                }
                else
                {
                    return RedirectToAction("Search", "CaptureCorporate");
                }
            }
            else
            {
                return RedirectToAction("Search", "CaptureCorporate");
            }
        }

        [HttpPost]

        public ActionResult SearchLand(FormCollection pObjFormCollection)
        {
            string mStrPlotNumber = pObjFormCollection.Get("txtPlotNumber");
            string mStrOccupierName = pObjFormCollection.Get("txtLandOccupier");
            string mStrRIN = pObjFormCollection.Get("txtRIN");

            Land mObjLand = new Land()
            {
                LandRIN = mStrRIN,
                PlotNumber = mStrPlotNumber,
                LandOccupier = mStrOccupierName,
                intStatus = 1
            };

            IList<usp_GetLandList_Result> lstLand = new BLLand().BL_GetLandList(mObjLand);
            return PartialView("_BindLandTable_SingleSelect", lstLand.Take(5).ToList());
        }


        public ActionResult SearchVehicle(int? id, string name)
        {
            string url = getUrl();
            bool itCan = new UtilityController().CheckAccess(url);
            if (!itCan) { return RedirectToAction("AccessDenied", "Utility"); }
            if (id.GetValueOrDefault() > 0)
            {
                Company mObjCompany = new Company()
                {
                    CompanyID = id.GetValueOrDefault(),
                    intStatus = 1
                };

                usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(mObjCompany);

                if (mObjCompanyData != null)
                {
                    CompanyViewModel mObjCompanyModelView = new CompanyViewModel()
                    {
                        CompanyID = mObjCompanyData.CompanyID.GetValueOrDefault(),
                        CompanyRIN = mObjCompanyData.CompanyRIN,
                        CompanyName = mObjCompanyData.CompanyName,
                        TIN = mObjCompanyData.TIN,
                        MobileNumber1 = mObjCompanyData.MobileNumber1,
                        MobileNumber2 = mObjCompanyData.MobileNumber2,
                        EmailAddress1 = mObjCompanyData.EmailAddress1,
                        EmailAddress2 = mObjCompanyData.EmailAddress2,
                        TaxOfficeName = mObjCompanyData.TaxOfficeName,
                        TaxPayerTypeName = mObjCompanyData.TaxPayerTypeName,
                        EconomicActivitiesName = mObjCompanyData.EconomicActivitiesName,
                        NotificationMethodName = mObjCompanyData.NotificationMethodName,
                        ContactAddress = mObjCompanyData.ContactAddress,
                        ActiveText = mObjCompanyData.ActiveText
                    };

                    return View(mObjCompanyModelView);
                }
                else
                {
                    return RedirectToAction("Search", "CaptureCorporate");
                }
            }
            else
            {
                return RedirectToAction("Search", "CaptureCorporate");
            }
        }

        [HttpPost]

        public ActionResult SearchVehicle(FormCollection pObjFormCollection)
        {
            string mStrRegNumber = pObjFormCollection.Get("txtRegNumber");
            string mStrVehicleDescription = pObjFormCollection.Get("txtVehicleDescription");
            string mStrRIN = pObjFormCollection.Get("txtRIN");

            Vehicle mObjVehicle = new Vehicle()
            {
                VehicleRIN = mStrRIN,
                VehicleRegNumber = mStrRegNumber,
                VehicleDescription = mStrVehicleDescription,
                intStatus = 1
            };

            IList<usp_GetVehicleList_Result> lstVehicle = new BLVehicle().BL_GetVehicleList(mObjVehicle);
            return PartialView("_BindVehicleTable_SingleSelect", lstVehicle.Take(5).ToList());
        }

        public void UI_FillBusinessDropDown(TPBusinessViewModel pObjBusinessViewModel = null)
        {
            if (pObjBusinessViewModel != null)
            {
                pObjBusinessViewModel.AssetTypeID = (int)EnumList.AssetTypes.Business;
            }
            else if (pObjBusinessViewModel == null)
            {
                pObjBusinessViewModel = new TPBusinessViewModel();
            }

            UI_FillTaxPayerRoleDropDown(new TaxPayer_Roles() { TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies, AssetTypeID = (int)EnumList.AssetTypes.Business });
            UI_FillAssetTypeDropDown(new Asset_Types() { intStatus = 1, IncludeAssetTypeIds = pObjBusinessViewModel.AssetTypeID.ToString() }, (int)EnumList.AssetTypes.Business);
            UI_FillBusinessTypeDropDown(new Business_Types() { intStatus = 1, IncludeBusinessTypeIds = pObjBusinessViewModel.BusinessTypeID.ToString() });
            UI_FillLGADropDown(new LGA() { intStatus = 1, IncludeLGAIds = pObjBusinessViewModel.LGAID.ToString() }); 
            UI_FillZoneDropDown(pObjBusinessViewModel.ZoneId);
            UI_FillTaxOfficeDropDown(pObjBusinessViewModel.TaxOfficeId);
            UI_FillBusinessCategoryDropDown(new Business_Category() { intStatus = 1, IncludeBusinessCategoryIds = pObjBusinessViewModel.BusinessCategoryID.ToString(), BusinessTypeID = pObjBusinessViewModel.BusinessTypeID });
            UI_FillBusinessSectorDropDown(new Business_Sector() { intStatus = 1, IncludeBusinessSectorIds = pObjBusinessViewModel.BusinessSectorID.ToString(), BusinessTypeID = pObjBusinessViewModel.BusinessTypeID, BusinessCategoryID = pObjBusinessViewModel.BusinessCategoryID });
            UI_FillBusinessSubSectorDropDown(new Business_SubSector() { intStatus = 1, IncludeBusinessSubSectorIds = pObjBusinessViewModel.BusinessSubSectorID.ToString(), BusinessSectorID = pObjBusinessViewModel.BusinessSectorID });
            UI_FillBusinessStructureDropDown(new Business_Structure() { intStatus = 1, IncludeBusinessStructureIds = pObjBusinessViewModel.BusinessStructureID.ToString(), BusinessTypeID = pObjBusinessViewModel.BusinessTypeID });
            UI_FillBusinessOperationDropDown(new Business_Operation() { intStatus = 1, IncludeBusinessOperationIds = pObjBusinessViewModel.BusinessOperationID.ToString(), BusinessTypeID = pObjBusinessViewModel.BusinessTypeID });
            UI_FillSizeDropDown(new Size() { intStatus = 1, IncludeSizeIds = pObjBusinessViewModel.SizeID.ToString() });
        }


        public ActionResult AddBusiness(int? id, string name)
        {
            string url = getUrl();
            bool itCan = new UtilityController().CheckAccess(url);
            if (!itCan) { return RedirectToAction("AccessDenied", "Utility"); }
            if (id.GetValueOrDefault() > 0)
            {
                Company mObjCompany = new Company()
                {
                    CompanyID = id.GetValueOrDefault(),
                    intStatus = 1
                };

                usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(mObjCompany);

                if (mObjCompanyData != null)
                {
                    TPBusinessViewModel mObjBusinessModel = new TPBusinessViewModel()
                    {
                        TaxPayerID = mObjCompanyData.CompanyID.GetValueOrDefault(),
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies,
                        TaxPayerTypeName = mObjCompanyData.TaxPayerTypeName,
                        TaxPayerRIN = mObjCompanyData.CompanyRIN,
                        TaxPayerTIN = mObjCompanyData.TIN,
                        TaxPayerName = mObjCompanyData.CompanyName,
                       // ZoneId = mObjCompanyData.zo
                        MobileNumber = mObjCompanyData.MobileNumber1,
                        ContactAddress = mObjCompanyData.ContactAddress,
                        AssetTypeID = (int)EnumList.AssetTypes.Business,
                    };

                    UI_FillBusinessDropDown(mObjBusinessModel);

                    return View(mObjBusinessModel);
                }
                else
                {
                    return RedirectToAction("Search", "CaptureCorporate");
                }
            }
            else
            {
                return RedirectToAction("Search", "CaptureCorporate");
            }
        }

        [HttpPost]

        [ValidateAntiForgeryToken()]
        public ActionResult AddBusiness(TPBusinessViewModel pObjBusinessModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillBusinessDropDown(pObjBusinessModel);
                return View(pObjBusinessModel);
            }
            else
            {
                using (TransactionScope mObjScope = new TransactionScope())
                {
                    try
                    {
                        Business mObjBusiness = new Business()
                        {
                            BusinessID = 0,
                            AssetTypeID = (int)EnumList.AssetTypes.Business,
                            BusinessTypeID = pObjBusinessModel.BusinessTypeID,
                            BusinessName = pObjBusinessModel.BusinessName,
                            LGAID = pObjBusinessModel.LGAID,
                            ZoneId = pObjBusinessModel.ZoneId,
                            TaxOfficeID = pObjBusinessModel.TaxOfficeId,
                            BusinessCategoryID = pObjBusinessModel.BusinessCategoryID,
                            BusinessSectorID = pObjBusinessModel.BusinessSectorID,
                            BusinessSubSectorID = pObjBusinessModel.BusinessSubSectorID,
                            BusinessStructureID = pObjBusinessModel.BusinessStructureID,
                            BusinessOperationID = pObjBusinessModel.BusinessOperationID,
                            SizeID = pObjBusinessModel.SizeID,
                            ContactName = pObjBusinessModel.ContactName,
                            BusinessAddress = pObjBusinessModel.BusinessAddress,
                            BusinessNumber = pObjBusinessModel.BusinessNumber,
                            Active = true,
                            CreatedBy = SessionManager.UserID,
                            CreatedDate = CommUtil.GetCurrentDateTime()
                        };

                        FuncResponse<Business> mObjResponse = new BLBusiness().BL_InsertUpdateBusiness(mObjBusiness);

                        if (mObjResponse.Success)
                        {
                            //Creating mapping between tax payer and business
                            MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                            {
                                AssetTypeID = (int)EnumList.AssetTypes.Business,
                                AssetID = mObjResponse.AdditionalData.BusinessID,
                                TaxPayerTypeID = pObjBusinessModel.TaxPayerTypeID,
                                TaxPayerRoleID = pObjBusinessModel.TaxPayerRoleID,
                                TaxPayerID = pObjBusinessModel.TaxPayerID,
                                Active = true,
                                CreatedBy = SessionManager.UserID,
                                CreatedDate = CommUtil.GetCurrentDateTime()
                            };

                            FuncResponse mObjTPResponse = new BLTaxPayerAsset().BL_InsertTaxPayerAsset(mObjTaxPayerAsset);

                            if (mObjTPResponse.Success)
                            {
                                //    var vExists = (from k in _db.MAP_TaxPayer_Asset
                                //                   join aa in _db.Asset_Types on k.AssetTypeID equals aa.AssetTypeID
                                //                   join tp in _db.TaxPayer_Roles on k.TaxPayerRoleID equals tp.TaxPayerRoleID
                                //                   join tpx in _db.TaxPayer_Types on k.TaxPayerTypeID equals tpx.TaxPayerTypeID
                                //                   join idd in _db.Individuals on k.TaxPayerID equals idd.IndividualID
                                //                   where k.AssetTypeID == mObjTaxPayerAsset.AssetTypeID && k.AssetID == mObjTaxPayerAsset.AssetID
                                //                      && k.TaxPayerTypeID == mObjTaxPayerAsset.TaxPayerTypeID && k.TaxPayerID == mObjTaxPayerAsset.TaxPayerID
                                //                      && k.TaxPayerRoleID == mObjTaxPayerAsset.TaxPayerRoleID && k.Active == true

                                //                   select new { Post = k, Meta = tp, All = tpx, Idd = idd, Aa = aa }).FirstOrDefault();

                                //    if (GlobalDefaultValues.SendNotification)
                                //    {
                                //        //Send Notification
                                //        EmailDetails mObjEmailDetails = new EmailDetails()
                                //        {
                                //            TaxPayerTypeID = vExists.Post.TaxPayerTypeID.GetValueOrDefault(),
                                //            TaxPayerTypeName = vExists.All.TaxPayerTypeName,
                                //            TaxPayerID = vExists.Idd.IndividualID,
                                //            TaxPayerName = vExists.Idd.FirstName + " " + vExists.Idd.LastName,
                                //            TaxPayerRIN = vExists.Idd.IndividualRIN,
                                //            TaxPayerRoleName = vExists.Meta.TaxPayerRoleName,
                                //            AssetName = vExists.Aa.AssetTypeName,
                                //            LoggedInUserID = SessionManager.UserID,
                                //        };

                                //        if (!string.IsNullOrWhiteSpace(vExists.Idd.EmailAddress1))
                                //        {
                                //            BLEmailHandler.BL_AssetProfileLinked(mObjEmailDetails);
                                //        }

                                //        if (!string.IsNullOrWhiteSpace(vExists.Idd.MobileNumber1))
                                //        {
                                //            UtilityController.BL_AssetProfileLinked(mObjEmailDetails);
                                //        }
                            }
                            mObjScope.Complete();
                            FlashMessage.Info("Business Created Successfully and Linked to Tax Payer");
                            return RedirectToAction("Details", "CaptureCorporate", new { id = pObjBusinessModel.TaxPayerID, name = pObjBusinessModel.TaxPayerRIN });
                        }
                        else
                        {
                            throw new Exception(mObjResponse.Message);
                        }



                    }
                    catch (Exception ex)
                    {
                        Logger.SendErrorToText(ex);
                        ErrorSignal.FromCurrentContext().Raise(ex);
                        UI_FillBusinessDropDown(pObjBusinessModel);
                        Transaction.Current.Rollback();
                        ViewBag.Message = "Error occurred while saving business";
                        return View(pObjBusinessModel);
                    }
                }
            }
        }

        public void UI_FillLandDropDown(TPLandViewModel pObjLandViewModel = null)
        {
            if (pObjLandViewModel != null)
            {
                pObjLandViewModel.AssetTypeID = (int)EnumList.AssetTypes.Land;
            }
            else if (pObjLandViewModel == null)
            {
                pObjLandViewModel = new TPLandViewModel();
            }

            UI_FillTaxPayerRoleDropDown(new TaxPayer_Roles() { TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies, AssetTypeID = (int)EnumList.AssetTypes.Land });
            UI_FillTownDropDown(new Town() { intStatus = 1, IncludeTownIds = pObjLandViewModel.TownID.ToString() });
            UI_FillLGADropDown(new LGA() { intStatus = 1, IncludeLGAIds = pObjLandViewModel.LGAID.ToString() });
            UI_FillWardDropDown(new Ward() { intStatus = 1, LGAID = pObjLandViewModel.LGAID, IncludeWardIds = pObjLandViewModel.WardID.ToString() });
            UI_FillAssetTypeDropDown(new Asset_Types() { intStatus = 1, IncludeAssetTypeIds = pObjLandViewModel.AssetTypeID.ToString() }, (int)EnumList.AssetTypes.Land);
            UI_FillLandPurposeDropDown(new Land_Purpose() { intStatus = 1, IncludeLandPurposeIds = pObjLandViewModel.LandPurposeID.ToString() });
            UI_FillLandFunctionDropDown(new Land_Function() { intStatus = 1, IncludeLandFunctionIds = pObjLandViewModel.LandFunctionID.ToString(), LandPurposeID = pObjLandViewModel.LandPurposeID });
            UI_FillLandDevelopmentDropDown(new Land_Development() { intStatus = 1, IncludeLandDevelopmentIds = pObjLandViewModel.LandDevelopmentID.ToString() });
            UI_FillLandOwnershipDropDown(new Land_Ownership() { intStatus = 1, IncludeLandOwnershipIds = pObjLandViewModel.LandOwnershipID.ToString() });
            UI_FillLandStreetConditionDropDown(new Land_StreetCondition() { intStatus = 1, IncludeLandStreetConditionIds = pObjLandViewModel.LandStreetConditionID.ToString() });
        }


        public ActionResult AddLand(int? id, string name)
        {
            string url = getUrl();
            bool itCan = new UtilityController().CheckAccess(url);
            if (!itCan) { return RedirectToAction("AccessDenied", "Utility"); }
            if (id.GetValueOrDefault() > 0)
            {
                Company mObjCompany = new Company()
                {
                    CompanyID = id.GetValueOrDefault(),
                    intStatus = 1
                };

                usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(mObjCompany);

                if (mObjCompanyData != null)
                {
                    TPLandViewModel mObjLandModel = new TPLandViewModel()
                    {
                        TaxPayerID = mObjCompanyData.CompanyID.GetValueOrDefault(),
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies,
                        TaxPayerTypeName = mObjCompanyData.TaxPayerTypeName,
                        TaxPayerRIN = mObjCompanyData.CompanyRIN,
                        TaxPayerTIN = mObjCompanyData.TIN,
                        TaxPayerName = mObjCompanyData.CompanyName,
                        MobileNumber = mObjCompanyData.MobileNumber1,
                        ContactAddress = mObjCompanyData.ContactAddress,
                        AssetTypeID = (int)EnumList.AssetTypes.Land,
                    };

                    UI_FillLandDropDown();

                    return View(mObjLandModel);
                }
                else
                {
                    return RedirectToAction("Search", "CaptureCorporate");
                }
            }
            else
            {
                return RedirectToAction("Search", "CaptureCorporate");
            }
        }

        [HttpPost]

        [ValidateAntiForgeryToken()]
        public ActionResult AddLand(TPLandViewModel pObjLandModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillLandDropDown(pObjLandModel);
                return View(pObjLandModel);
            }
            else
            {
                using (TransactionScope mObjScope = new TransactionScope())
                {
                    try
                    {
                        Land mObjLand = new Land()
                        {
                            LandID = 0,
                            PlotNumber = pObjLandModel.PlotNumber,
                            StreetName = pObjLandModel.StreetName,
                            TownID = pObjLandModel.TownID,
                            LGAID = pObjLandModel.LGAID,
                            WardID = pObjLandModel.WardID,
                            AssetTypeID = (int)EnumList.AssetTypes.Land,
                            LandSize_Length = pObjLandModel.LandSize_Length,
                            LandSize_Width = pObjLandModel.LandSize_Width,
                            C_OF_O_Ref = pObjLandModel.C_OF_O_Ref,
                            LandPurposeID = pObjLandModel.LandPurposeID,
                            LandFunctionID = pObjLandModel.LandFunctionID,
                            LandOwnershipID = pObjLandModel.LandOwnershipID,
                            LandDevelopmentID = pObjLandModel.LandDevelopmentID,
                            Latitude = pObjLandModel.Latitude,
                            Longitude = pObjLandModel.Longitude,
                            ValueOfLand = pObjLandModel.ValueOfLand,
                            LandStreetConditionID = pObjLandModel.LandStreetConditionID,
                            Neighborhood = pObjLandModel.Neighborhood,
                            Active = true,
                            CreatedBy = SessionManager.UserID,
                            CreatedDate = CommUtil.GetCurrentDateTime()
                        };

                        FuncResponse<Land> mObjResponse = new BLLand().BL_InsertUpdateLand(mObjLand);

                        if (mObjResponse.Success)
                        {
                            //Creating mapping between tax payer and land
                            MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                            {
                                AssetTypeID = (int)EnumList.AssetTypes.Land,
                                AssetID = mObjResponse.AdditionalData.LandID,
                                TaxPayerTypeID = pObjLandModel.TaxPayerTypeID,
                                TaxPayerRoleID = pObjLandModel.TaxPayerRoleID,
                                TaxPayerID = pObjLandModel.TaxPayerID,
                                Active = true,
                                CreatedBy = SessionManager.UserID,
                                CreatedDate = CommUtil.GetCurrentDateTime()
                            };

                            FuncResponse mObjTPResponse = new BLTaxPayerAsset().BL_InsertTaxPayerAsset(mObjTaxPayerAsset);

                            if (mObjTPResponse.Success)
                            {
                                //var vExists = (from tpa in _db.MAP_TaxPayer_Asset
                                //               join aa in _db.Asset_Types on tpa.AssetTypeID equals aa.AssetTypeID
                                //               join tp in _db.TaxPayer_Roles on tpa.TaxPayerRoleID equals tp.TaxPayerRoleID
                                //               join tpx in _db.TaxPayer_Types on tpa.TaxPayerTypeID equals tpx.TaxPayerTypeID
                                //               join idd in _db.Individuals on tpa.TaxPayerID equals idd.IndividualID
                                //               where tpa.AssetTypeID == mObjTaxPayerAsset.AssetTypeID && tpa.AssetID == mObjTaxPayerAsset.AssetID
                                //                  && tpa.TaxPayerTypeID == mObjTaxPayerAsset.TaxPayerTypeID && tpa.TaxPayerID == mObjTaxPayerAsset.TaxPayerID
                                //                  && tpa.TaxPayerRoleID == mObjTaxPayerAsset.TaxPayerRoleID && tpa.Active == true

                                //               select new { Post = tpa, Meta = tp, All = tpx, Idd = idd, Aa = aa }).FirstOrDefault();

                                //if (GlobalDefaultValues.SendNotification)
                                //{
                                //    //Send Notification
                                //    EmailDetails mObjEmailDetails = new EmailDetails()
                                //    {
                                //        TaxPayerTypeID = vExists.Post.TaxPayerTypeID.GetValueOrDefault(),
                                //        TaxPayerTypeName = vExists.All.TaxPayerTypeName,
                                //        TaxPayerID = vExists.Idd.IndividualID,
                                //        TaxPayerName = vExists.Idd.FirstName + " " + vExists.Idd.LastName,
                                //        TaxPayerRIN = vExists.Idd.IndividualRIN,
                                //        TaxPayerRoleName = vExists.Meta.TaxPayerRoleName,
                                //        AssetName = vExists.Aa.AssetTypeName,
                                //        LoggedInUserID = SessionManager.UserID,
                                //    };

                                //    if (!string.IsNullOrWhiteSpace(vExists.Idd.EmailAddress1))
                                //    {
                                //        BLEmailHandler.BL_AssetProfileLinked(mObjEmailDetails);
                                //    }

                                //    if (!string.IsNullOrWhiteSpace(vExists.Idd.MobileNumber1))
                                //    {
                                //        UtilityController.BL_AssetProfileLinked(mObjEmailDetails);
                                //    }
                                // }
                                mObjScope.Complete();
                                FlashMessage.Info("Land Created Successfully and Linked to Tax Payer");
                                return RedirectToAction("Details", "CaptureCorporate", new { id = pObjLandModel.TaxPayerID, name = pObjLandModel.TaxPayerRIN });
                            }
                            else
                            {
                                throw new Exception(mObjResponse.Message);
                            }
                        }
                        else
                        {
                            UI_FillLandDropDown(pObjLandModel);
                            Transaction.Current.Rollback();
                            ViewBag.Message = mObjResponse.Message;
                            return View(pObjLandModel);
                        }

                    }
                    catch (Exception ex)
                    {
                        Logger.SendErrorToText(ex);
                        ErrorSignal.FromCurrentContext().Raise(ex);
                        UI_FillLandDropDown(pObjLandModel);
                        Transaction.Current.Rollback();
                        ViewBag.Message = "Error occurred while saving land";
                        return View(pObjLandModel);
                    }
                }
            }
        }

        public void UI_FillVehicleDropDown(VehicleViewModel pObjVehicleViewModel = null)
        {
            if (pObjVehicleViewModel != null)
            {
                pObjVehicleViewModel.AssetTypeID = (int)EnumList.AssetTypes.Vehicles;
            }
            else if (pObjVehicleViewModel == null)
            {
                pObjVehicleViewModel = new VehicleViewModel();
            }

            UI_FillTaxPayerRoleDropDown(new TaxPayer_Roles() { TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies, AssetTypeID = (int)EnumList.AssetTypes.Vehicles });
            UI_FillLGADropDown(new LGA() { intStatus = 1, IncludeLGAIds = pObjVehicleViewModel.LGAID.ToString() });
            UI_FillAssetTypeDropDown(new Asset_Types() { intStatus = 1, IncludeAssetTypeIds = pObjVehicleViewModel.AssetTypeID.ToString() }, (int)EnumList.AssetTypes.Vehicles);
            UI_FillVehicleTypeDropDown(new Vehicle_Types() { intStatus = 1, IncludeVehicleTypeIds = pObjVehicleViewModel.VehicleTypeID.ToString() });
            UI_FillVehicleSubTypeDropDown(new Vehicle_SubTypes() { intStatus = 1, IncludeVehicleSubTypeIds = pObjVehicleViewModel.VehicleSubTypeID.ToString(), VehicleTypeID = pObjVehicleViewModel.VehicleTypeID });
            UI_FillVehiclePurposeDropDown(new Vehicle_Purpose() { intStatus = 1, IncludeVehiclePurposeIds = pObjVehicleViewModel.VehiclePurposeID.ToString() });
            UI_FillVehicleFunctionDropDown(new Vehicle_Function() { intStatus = 1, IncludeVehicleFunctionIds = pObjVehicleViewModel.VehicleFunctionID.ToString(), VehiclePurposeID = pObjVehicleViewModel.VehiclePurposeID });
            UI_FillVehicleOwnershipDropDown(new Vehicle_Ownership() { intStatus = 1, IncludeVehicleOwnershipIds = pObjVehicleViewModel.VehicleOwnershipID.ToString() });
        }
        public ActionResult AddVehicle2(TaxPayerAsset tpa)
        {
            MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
            {
                AssetTypeID = (int)EnumList.AssetTypes.Vehicles,
                AssetID = tpa.AssetID,
                TaxPayerTypeID = tpa.TaxPayerTypeID,
                TaxPayerRoleID = tpa.TaxPayerRoleID,
                TaxPayerID = tpa.TaxPayerID,
                Active = true,
                CreatedBy = SessionManager.UserID,
                CreatedDate = CommUtil.GetCurrentDateTime()
            };

            FuncResponse mObjTPResponse = new BLTaxPayerAsset().BL_InsertTaxPayerAsset(mObjTaxPayerAsset);

            if (mObjTPResponse.Success)
            {
                FlashMessage.Info("Vehicle Created Successfully and Linked to Tax Payer");
                return RedirectToAction("Details", "CaptureCorporate", new { id = tpa.TaxPayerID, name = "" });
            }
            else
            {
                return RedirectToAction("Details", "CaptureCorporate", new { id = tpa.TaxPayerID, name = "" });
            }

        }

        public ActionResult AddVehicle(int? id, string name)
        {
            string url = getUrl();
            bool itCan = new UtilityController().CheckAccess(url);
            if (!itCan) { return RedirectToAction("AccessDenied", "Utility"); }
            if (id.GetValueOrDefault() > 0)
            {
                Company mObjCompany = new Company()
                {
                    CompanyID = id.GetValueOrDefault(),
                    intStatus = 1
                };

                usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(mObjCompany);

                if (mObjCompanyData != null)
                {
                    TPVehicleViewModel mObjVehicleModel = new TPVehicleViewModel()
                    {
                        TaxPayerID = mObjCompanyData.CompanyID.GetValueOrDefault(),
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies,
                        TaxPayerTypeName = mObjCompanyData.TaxPayerTypeName,
                        TaxPayerRIN = mObjCompanyData.CompanyRIN,
                        TaxPayerTIN = mObjCompanyData.TIN,
                        TaxPayerName = mObjCompanyData.CompanyName,
                        MobileNumber = mObjCompanyData.MobileNumber1,
                        ContactAddress = mObjCompanyData.ContactAddress,
                        AssetTypeID = (int)EnumList.AssetTypes.Vehicles,
                    };

                    UI_FillVehicleDropDown(mObjVehicleModel);

                    return View(mObjVehicleModel);
                }
                else
                {
                    return RedirectToAction("Search", "CaptureCorporate");
                }
            }
            else
            {
                return RedirectToAction("Search", "CaptureCorporate");
            }
        }

        [HttpPost]

        [ValidateAntiForgeryToken()]
        public ActionResult AddVehicle(TPVehicleViewModel pObjVehicleModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillVehicleDropDown(pObjVehicleModel);
                return View(pObjVehicleModel);
            }
            else
            {
                using (TransactionScope mObjScope = new TransactionScope())
                {
                    try
                    {
                        Vehicle mObjVehicle = new Vehicle()
                        {
                            VehicleID = 0,
                            VehicleRegNumber = pObjVehicleModel.VehicleRegNumber,
                            VIN = pObjVehicleModel.VIN != null ? pObjVehicleModel.VIN.Trim() : pObjVehicleModel.VIN,
                            AssetTypeID = (int)EnumList.AssetTypes.Vehicles,
                            VehicleTypeID = pObjVehicleModel.VehicleTypeID,
                            VehicleSubTypeID = pObjVehicleModel.VehicleSubTypeID,
                            LGAID = pObjVehicleModel.LGAID,
                            VehiclePurposeID = pObjVehicleModel.VehiclePurposeID,
                            VehicleFunctionID = pObjVehicleModel.VehicleFunctionID,
                            VehicleOwnershipID = pObjVehicleModel.VehicleOwnershipID,
                            VehicleDescription = pObjVehicleModel.VehicleDescription,
                            Active = true,
                            CreatedBy = SessionManager.UserID,
                            CreatedDate = CommUtil.GetCurrentDateTime()
                        };

                        FuncResponse<Vehicle> mObjResponse = new BLVehicle().BL_InsertUpdateVehicle(mObjVehicle);

                        if (mObjResponse.Success)
                        {
                            //Creating mapping between tax payer and land
                            MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                            {
                                AssetTypeID = (int)EnumList.AssetTypes.Vehicles,
                                AssetID = mObjResponse.AdditionalData.VehicleID,
                                TaxPayerTypeID = pObjVehicleModel.TaxPayerTypeID,
                                TaxPayerRoleID = pObjVehicleModel.TaxPayerRoleID,
                                TaxPayerID = pObjVehicleModel.TaxPayerID,
                                Active = true,
                                CreatedBy = SessionManager.UserID,
                                CreatedDate = CommUtil.GetCurrentDateTime()
                            };

                            FuncResponse mObjTPResponse = new BLTaxPayerAsset().BL_InsertTaxPayerAsset(mObjTaxPayerAsset);

                            if (mObjTPResponse.Success)
                            {
                                //var vExists = (from tpa in _db.MAP_TaxPayer_Asset
                                //               join aa in _db.Asset_Types on tpa.AssetTypeID equals aa.AssetTypeID
                                //               join tp in _db.TaxPayer_Roles on tpa.TaxPayerRoleID equals tp.TaxPayerRoleID
                                //               join tpx in _db.TaxPayer_Types on tpa.TaxPayerTypeID equals tpx.TaxPayerTypeID
                                //               join idd in _db.Individuals on tpa.TaxPayerID equals idd.IndividualID
                                //               where tpa.AssetTypeID == mObjTaxPayerAsset.AssetTypeID && tpa.AssetID == mObjTaxPayerAsset.AssetID
                                //                  && tpa.TaxPayerTypeID == mObjTaxPayerAsset.TaxPayerTypeID && tpa.TaxPayerID == mObjTaxPayerAsset.TaxPayerID
                                //                  && tpa.TaxPayerRoleID == mObjTaxPayerAsset.TaxPayerRoleID && tpa.Active == true

                                //               select new { Post = tpa, Meta = tp, All = tpx, Idd = idd, Aa = aa }).FirstOrDefault();

                                //if (GlobalDefaultValues.SendNotification)
                                //{
                                //    //Send Notification
                                //    EmailDetails mObjEmailDetails = new EmailDetails()
                                //    {
                                //        TaxPayerTypeID = vExists.Post.TaxPayerTypeID.GetValueOrDefault(),
                                //        TaxPayerTypeName = vExists.All.TaxPayerTypeName,
                                //        TaxPayerID = vExists.Idd.IndividualID,
                                //        TaxPayerName = vExists.Idd.FirstName + " " + vExists.Idd.LastName,
                                //        TaxPayerRIN = vExists.Idd.IndividualRIN,
                                //        TaxPayerRoleName = vExists.Meta.TaxPayerRoleName,
                                //        AssetName = vExists.Aa.AssetTypeName,
                                //        LoggedInUserID = SessionManager.UserID,
                                //    };

                                //    if (!string.IsNullOrWhiteSpace(vExists.Idd.EmailAddress1))
                                //    {
                                //        BLEmailHandler.BL_AssetProfileLinked(mObjEmailDetails);
                                //    }

                                //    if (!string.IsNullOrWhiteSpace(vExists.Idd.MobileNumber1))
                                //    {
                                //        UtilityController.BL_AssetProfileLinked(mObjEmailDetails);
                                //    }
                                //}
                                mObjScope.Complete();
                                FlashMessage.Info("Vehicle Created Successfully and Linked to Tax Payer");
                                return RedirectToAction("Details", "CaptureCorporate", new { id = pObjVehicleModel.TaxPayerID, name = pObjVehicleModel.TaxPayerRIN });
                            }
                            else
                            {
                                throw new Exception(mObjResponse.Message);
                            }
                        }
                        else
                        {
                            UI_FillVehicleDropDown(pObjVehicleModel);
                            ViewBag.Message = mObjResponse.Message;
                            Transaction.Current.Rollback();
                            return View(pObjVehicleModel);
                        }

                    }
                    catch (Exception ex)
                    {
                        Logger.SendErrorToText(ex);
                        ErrorSignal.FromCurrentContext().Raise(ex);
                        UI_FillVehicleDropDown(pObjVehicleModel);
                        ViewBag.Message = "Error occurred while saving vehicle";
                        Transaction.Current.Rollback();
                        return View(pObjVehicleModel);
                    }
                }
            }
        }

        public void UI_FillBuildingDropDown(TPBuildingViewModel pObjBuildingViewModel = null)
        {
            if (pObjBuildingViewModel != null)
            {
                pObjBuildingViewModel.AssetTypeID = (int)EnumList.AssetTypes.Building;
            }
            else if (pObjBuildingViewModel == null)
            {
                pObjBuildingViewModel = new TPBuildingViewModel();
            }

            UI_FillTaxPayerRoleDropDown(new TaxPayer_Roles() { TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies, AssetTypeID = (int)EnumList.AssetTypes.Building });
            UI_FillTownDropDown(new Town() { intStatus = 1, IncludeTownIds = pObjBuildingViewModel.TownID.ToString() });
            UI_FillLGADropDown(new LGA() { intStatus = 1, IncludeLGAIds = pObjBuildingViewModel.LGAID.ToString() });
            UI_FillWardDropDown(new Ward() { intStatus = 1, LGAID = pObjBuildingViewModel.LGAID, IncludeWardIds = pObjBuildingViewModel.WardID.ToString() });
            UI_FillAssetTypeDropDown(new Asset_Types() { intStatus = 1, IncludeAssetTypeIds = pObjBuildingViewModel.AssetTypeID.ToString() }, (int)EnumList.AssetTypes.Building);
            UI_FillBuildingTypeDropDown(new Building_Types() { intStatus = 1, IncludeBuildingTypeIds = pObjBuildingViewModel.BuildingTypeID.ToString() });
            UI_FillBuildingCompletionDropDown(new Building_Completion() { intStatus = 1, IncludeBuildingCompletionIds = pObjBuildingViewModel.BuildingCompletionID.ToString() });
            UI_FillBuildingPurposeDropDown(new Building_Purpose() { intStatus = 1, IncludeBuildingPurposeIds = pObjBuildingViewModel.BuildingPurposeID.ToString() });
            UI_FillBuildingOwnershipDropDown(new Building_Ownership() { intStatus = 1, IncludeBuildingOwnershipIds = pObjBuildingViewModel.BuildingOwnershipID.ToString() });

            UI_FillUnitPurposeDropDown(new Unit_Purpose() { intStatus = 1, });
            UI_FillUnitFunctionDropDown(new Unit_Function() { intStatus = 1 });
            UI_FillUnitOccupancyDropDown(new Unit_Occupancy() { intStatus = 1 });
            UI_FillSizeDropDown(new Size() { intStatus = 1 });

            ViewBag.BuildingUnitList = SessionManager.LstBuildingUnit;
        }


        public ActionResult AddBuilding(int? id, string name)
        {
            string url = getUrl();
            bool itCan = new UtilityController().CheckAccess(url);
            if (!itCan) { return RedirectToAction("AccessDenied", "Utility"); }
            if (id.GetValueOrDefault() > 0)
            {
                Company mObjCompany = new Company()
                {
                    CompanyID = id.GetValueOrDefault(),
                    intStatus = 1
                };

                usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(mObjCompany);

                if (mObjCompanyData != null)
                {
                    TPBuildingViewModel mObjBuildingModel = new TPBuildingViewModel()
                    {
                        TaxPayerID = mObjCompanyData.CompanyID.GetValueOrDefault(),
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies,
                        TaxPayerTypeName = mObjCompanyData.TaxPayerTypeName,
                        TaxPayerRIN = mObjCompanyData.CompanyRIN,
                        TaxPayerTIN = mObjCompanyData.TIN,
                        TaxPayerName = mObjCompanyData.CompanyName,
                        MobileNumber = mObjCompanyData.MobileNumber1,
                        ContactAddress = mObjCompanyData.ContactAddress,
                        AssetTypeID = (int)EnumList.AssetTypes.Building,
                    };

                    SessionManager.LstBuildingUnit = new List<Building_BuildingUnit>();

                    UI_FillBuildingDropDown();

                    return View(mObjBuildingModel);
                }
                else
                {
                    return RedirectToAction("Search", "CaptureCorporate");
                }
            }
            else
            {
                return RedirectToAction("Search", "CaptureCorporate");
            }
        }

        [HttpPost]

        [ValidateAntiForgeryToken()]
        public ActionResult AddBuilding(TPBuildingViewModel pObjBuildingModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillBuildingDropDown(pObjBuildingModel);
                return View(pObjBuildingModel);
            }
            else
            {
                IList<Building_BuildingUnit> lstBuildingUnit = SessionManager.LstBuildingUnit;
                if (lstBuildingUnit.Count == 0)
                {
                    UI_FillBuildingDropDown(pObjBuildingModel);
                    ViewBag.Message = "Add atleast one building unit";
                    return View(pObjBuildingModel);
                }
                else
                {
                    using (TransactionScope mObjScope = new TransactionScope())
                    {
                        try
                        {
                            BLBuilding mObjBLBuilding = new BLBuilding();
                            Building mObjBuilding = new Building()
                            {
                                BuildingID = 0,
                                BuildingName = pObjBuildingModel.BuildingName != null ? pObjBuildingModel.BuildingName.Trim() : pObjBuildingModel.BuildingName,
                                BuildingNumber = pObjBuildingModel.BuildingNumber,
                                StreetName = pObjBuildingModel.StreetName,
                                OffStreetName = pObjBuildingModel.OffStreetName != null ? pObjBuildingModel.OffStreetName.Trim() : pObjBuildingModel.OffStreetName,
                                TownID = pObjBuildingModel.TownID,
                                LGAID = pObjBuildingModel.LGAID,
                                WardID = pObjBuildingModel.WardID,
                                AssetTypeID = (int)EnumList.AssetTypes.Building,
                                BuildingTypeID = pObjBuildingModel.BuildingTypeID,
                                BuildingCompletionID = pObjBuildingModel.BuildingCompletionID,
                                BuildingPurposeID = pObjBuildingModel.BuildingPurposeID,
                                BuildingOwnershipID = pObjBuildingModel.BuildingOwnershipID,
                                NoOfUnits = pObjBuildingModel.NoOfUnits,
                                BuildingSize_Length = pObjBuildingModel.BuildingSize_Length,
                                BuildingSize_Width = pObjBuildingModel.BuildingSize_Width,
                                Latitude = pObjBuildingModel.Latitude,
                                Longitude = pObjBuildingModel.Longitude,
                                Active = true,
                                CreatedBy = SessionManager.UserID,
                                CreatedDate = CommUtil.GetCurrentDateTime()
                            };

                            FuncResponse<Building> mObjResponse = mObjBLBuilding.BL_InsertUpdateBuilding(mObjBuilding);

                            if (mObjResponse.Success)
                            {
                                BLBuildingUnit mObjBLBuildingUnit = new BLBuildingUnit();

                                //Adding Building Unit
                                foreach (var item in lstBuildingUnit)
                                {
                                    Building_Unit mObjBuildingUnit = new Building_Unit()
                                    {
                                        BuildingUnitID = 0,
                                        UnitNumber = item.UnitNumber,
                                        UnitPurposeID = item.UnitPurposeID,
                                        UnitFunctionID = item.UnitFunctionID,
                                        UnitOccupancyID = item.UnitOccupancyID,
                                        SizeID = item.UnitSizeID,
                                        Active = true,
                                        CreatedBy = SessionManager.UserID,
                                        CreatedDate = CommUtil.GetCurrentDateTime()
                                    };

                                    FuncResponse<Building_Unit> mObjBUResponse = mObjBLBuildingUnit.BL_InsertUpdateBuildingUnit(mObjBuildingUnit);

                                    if (mObjBUResponse.Success)
                                    {
                                        item.BuildingUnitID = mObjBUResponse.AdditionalData.BuildingUnitID;

                                        //Creating Mapping With Building
                                        MAP_Building_BuildingUnit mObjUnit = new MAP_Building_BuildingUnit()
                                        {
                                            BuildingUnitID = mObjBUResponse.AdditionalData.BuildingUnitID,
                                            BuildingID = mObjResponse.AdditionalData.BuildingID,
                                            CreatedBy = SessionManager.UserID,
                                            CreatedDate = CommUtil.GetCurrentDateTime()
                                        };

                                        FuncResponse mObjBBUResponse = mObjBLBuilding.BL_InsertBuildingUnitNumber(mObjUnit);

                                        if (!mObjBBUResponse.Success)
                                        {
                                            throw new Exception(mObjBBUResponse.Message);
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception(mObjBUResponse.Message);
                                    }
                                }

                                int mIntBuildingUnitID = lstBuildingUnit.Where(t => t.RowID == pObjBuildingModel.BuildingUnitID).FirstOrDefault().BuildingUnitID;


                                //Creating mapping between tax payer and business
                                MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                                {
                                    AssetTypeID = (int)EnumList.AssetTypes.Building,
                                    AssetID = mObjResponse.AdditionalData.BuildingID,
                                    TaxPayerTypeID = pObjBuildingModel.TaxPayerTypeID,
                                    TaxPayerRoleID = pObjBuildingModel.TaxPayerRoleID,
                                    TaxPayerID = pObjBuildingModel.TaxPayerID,
                                    BuildingUnitID = mIntBuildingUnitID,
                                    Active = true,
                                    CreatedBy = SessionManager.UserID,
                                    CreatedDate = CommUtil.GetCurrentDateTime()
                                };

                                FuncResponse mObjTPResponse = new BLTaxPayerAsset().BL_InsertTaxPayerAsset(mObjTaxPayerAsset);

                                if (mObjTPResponse.Success)
                                {
                                    //{
                                    //    var vExists = (from tpa in _db.MAP_TaxPayer_Asset
                                    //                   join aa in _db.Asset_Types on tpa.AssetTypeID equals aa.AssetTypeID
                                    //                   join tp in _db.TaxPayer_Roles on tpa.TaxPayerRoleID equals tp.TaxPayerRoleID
                                    //                   join tpx in _db.TaxPayer_Types on tpa.TaxPayerTypeID equals tpx.TaxPayerTypeID
                                    //                   join idd in _db.Individuals on tpa.TaxPayerID equals idd.IndividualID
                                    //                   where tpa.AssetTypeID == mObjTaxPayerAsset.AssetTypeID && tpa.AssetID == mObjTaxPayerAsset.AssetID
                                    //                      && tpa.TaxPayerTypeID == mObjTaxPayerAsset.TaxPayerTypeID && tpa.TaxPayerID == mObjTaxPayerAsset.TaxPayerID
                                    //                      && tpa.TaxPayerRoleID == mObjTaxPayerAsset.TaxPayerRoleID && tpa.Active == true

                                    //                   select new { Post = tpa, Meta = tp, All = tpx, Idd = idd, Aa = aa }).FirstOrDefault();

                                    //    if (GlobalDefaultValues.SendNotification)
                                    //    {
                                    //        //Send Notification
                                    //        EmailDetails mObjEmailDetails = new EmailDetails()
                                    //        {
                                    //            TaxPayerTypeID = vExists.Post.TaxPayerTypeID.GetValueOrDefault(),
                                    //            TaxPayerTypeName = vExists.All.TaxPayerTypeName,
                                    //            TaxPayerID = vExists.Idd.IndividualID,
                                    //            TaxPayerName = vExists.Idd.FirstName + " " + vExists.Idd.LastName,
                                    //            TaxPayerRIN = vExists.Idd.IndividualRIN,
                                    //            TaxPayerRoleName = vExists.Meta.TaxPayerRoleName,
                                    //            AssetName = vExists.Aa.AssetTypeName,
                                    //            LoggedInUserID = SessionManager.UserID,
                                    //        };

                                    //        if (!string.IsNullOrWhiteSpace(vExists.Idd.EmailAddress1))
                                    //        {
                                    //            BLEmailHandler.BL_AssetProfileLinked(mObjEmailDetails);
                                    //        }

                                    //        if (!string.IsNullOrWhiteSpace(vExists.Idd.MobileNumber1))
                                    //        {
                                    //            UtilityController.BL_AssetProfileLinked(mObjEmailDetails);
                                    //        }
                                    //    }
                                    mObjScope.Complete();
                                    FlashMessage.Info("Building Created Successfully and Linked to Tax Payer");
                                    return RedirectToAction("Details", "CaptureCorporate", new { id = pObjBuildingModel.TaxPayerID, name = pObjBuildingModel.TaxPayerRIN });
                                }
                                else
                                {
                                    throw new Exception(mObjTPResponse.Message);
                                }
                            }
                            else
                            {
                                UI_FillBuildingDropDown(pObjBuildingModel);
                                Transaction.Current.Rollback();
                                ViewBag.Message = mObjResponse.Message;
                                return View(pObjBuildingModel);
                            }

                        }
                        catch (Exception ex)
                        {
                            Logger.SendErrorToText(ex);
                            ErrorSignal.FromCurrentContext().Raise(ex);
                            UI_FillBuildingDropDown(pObjBuildingModel);
                            Transaction.Current.Rollback();
                            ViewBag.Message = "Error occurred while saving building";
                            return View(pObjBuildingModel);
                        }
                    }
                }
            }
        }


        public ActionResult GenerateAssessment(int? id, string name)
        {
            string url = getUrl();
            bool itCan = new UtilityController().CheckAccess(url);
            if (!itCan) { return RedirectToAction("AccessDenied", "Utility"); }
            if (id.GetValueOrDefault() > 0)
            {
                Company mObjCompany = new Company()
                {
                    CompanyID = id.GetValueOrDefault(),
                    intStatus = 1
                };

                usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(mObjCompany);

                if (mObjCompanyData != null)
                {
                    GenerateAssessmentViewModel mObjGenerateAssessmentModel = new GenerateAssessmentViewModel()
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

                    IList<usp_GetAssessmentRuleForAssessment_Result> lstAssessmentRules = new BLTaxPayerAsset().BL_GetTaxPayerAssessmentRuleForAssessment((int)EnumList.TaxPayerType.Companies, id.GetValueOrDefault());
                    ViewBag.AssessmentRuleInformation = lstAssessmentRules;

                    return View(mObjGenerateAssessmentModel);
                }
                else
                {
                    return RedirectToAction("Search", "CaptureCorporate");
                }
            }
            else
            {
                return RedirectToAction("Search", "CaptureCorporate");
            }
        }

        [HttpPost]

        [ValidateAntiForgeryToken()]
        public ActionResult GenerateAssessment(GenerateAssessmentViewModel pObjGenerateAssessmentModel)
        {
            if (!ModelState.IsValid)
            {
                IList<usp_GetAssessmentRuleForAssessment_Result> lstAssessmentRules = new BLTaxPayerAsset().BL_GetTaxPayerAssessmentRuleForAssessment((int)EnumList.TaxPayerType.Companies, pObjGenerateAssessmentModel.TaxPayerID);
                ViewBag.AssessmentRuleInformation = lstAssessmentRules;

                return View(pObjGenerateAssessmentModel);
            }
            else
            {
                return RedirectToAction("AddAssessment", new { id = pObjGenerateAssessmentModel.TaxPayerID, name = pObjGenerateAssessmentModel.TaxPayerRIN, aruleIds = pObjGenerateAssessmentModel.AssessmentRuleId });
            }
        }
        public class newServiceBillIdsRequest
        {
            public string AssID { get; set; }
            public string ServiceId { get; set; }
        }
        public ActionResult AddAssessment(int? id, string name, string aruleIds)
        {
            string url = getUrl();
            bool itCan = new UtilityController().CheckAccess(url);
            if (!itCan) { return RedirectToAction("AccessDenied", "Utility"); }
            if (id.GetValueOrDefault() > 0)
            {
                Company mObjIndividual = new Company()
                {
                    CompanyID = id.GetValueOrDefault(),
                    intStatus = 1
                };

                usp_GetCompanyList_Result mObjIndividualData = new BLCompany().BL_GetCompanyDetails(mObjIndividual);

                if (mObjIndividualData != null)
                {
                    NewAssessmentViewModel mObjAssessmentModel = new NewAssessmentViewModel()
                    {
                        TaxPayerID = mObjIndividualData.CompanyID.GetValueOrDefault(),
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies,
                        TaxPayerRIN = mObjIndividualData.CompanyRIN,
                        TaxPayerName = mObjIndividualData.CompanyName,
                        TaxPayerAddress = mObjIndividualData.ContactAddress,
                        SettlementDuedate = DateTime.Now,
                        AssessmentDate = DateTime.Now
                    };

                    IList<Assessment_AssessmentRule> lstAssessmentRules = new List<Assessment_AssessmentRule>();
                    IList<Assessment_AssessmentItem> lstAssessmentItems = new List<Assessment_AssessmentItem>();
                    IList<usp_GetAssessmentRuleForAssessment_Result> newlstAssessmentRules = new BLTaxPayerAsset().BL_GetTaxPayerAssessmentRuleForAssessment((int)EnumList.TaxPayerType.Companies, id.GetValueOrDefault());

                    BLAssessmentRule mObjBLAssessmentRule = new BLAssessmentRule();
                    BLAssessmentItem mObjBLAssessmentItem = new BLAssessmentItem();

                    string strAssessmentRuleIds = EncryptDecrypt.Decrypt(aruleIds);

                    List<string> strArrAssessmentRuleIds = new List<string>();
                    List<string> strArrAssestIds = new List<string>();

                    //string[] strArrAssessmentRuleIds = strAssessmentRuleIds.Split(',');
                    if (aruleIds.Contains("{"))
                    {
                        var assBillIds = JsonConvert.DeserializeObject<List<newServiceBillIdsRequest>>(aruleIds);

                        foreach (var item in assBillIds)
                        {
                            strArrAssessmentRuleIds.Add(item.ServiceId);
                            strArrAssestIds.Add(item.AssID);
                        }
                    }
                    else
                    {
                        strArrAssessmentRuleIds = aruleIds.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    }
                    foreach (string strARuleData in strArrAssessmentRuleIds)
                    {
                        string newAssId = "";
                        int checkerNumber = 0;
                        if (!string.IsNullOrWhiteSpace(strARuleData))
                        {
                            var checker = newlstAssessmentRules.Where(t => t.AssessmentRuleID == TrynParse.parseInt(strARuleData));
                            checkerNumber = checker.Count();
                            if (checkerNumber > 1)
                            {
                                int index = strArrAssessmentRuleIds.FindIndex(str => str.Contains(strARuleData));
                                newAssId = strArrAssestIds.ElementAt(index);

                                checker = newlstAssessmentRules.Where(t => t.AssessmentRuleID == TrynParse.parseInt(strARuleData) && t.AssetID == TrynParse.parseInt(newAssId));
                                checkerNumber = checker.Count();
                            }
                            string[] strRuleData = strARuleData.Split(new string[] { "@@" }, StringSplitOptions.RemoveEmptyEntries);

                            usp_GetAssessmentRuleList_Result mObjAssessmentRuleData = mObjBLAssessmentRule.BL_GetAssessmentRuleDetails(new Assessment_Rules() { AssessmentRuleID = TrynParse.parseInt(strARuleData), IntStatus = 2, ProfileID = 0 });

                            if (mObjAssessmentRuleData != null)
                            {
                                foreach (var mObjAAR in checker)
                                {

                                    Assessment_AssessmentRule mObjAssessmentRule = new Assessment_AssessmentRule()
                                    {
                                        RowID = lstAssessmentRules.Count + 1,

                                        AssetTypeID = mObjAAR.AssetTypeID.GetValueOrDefault(),
                                        AssetID = mObjAAR.AssetID.GetValueOrDefault(),
                                        ProfileID = mObjAssessmentRuleData.ProfileID.GetValueOrDefault(),
                                        AssessmentRuleAmount = mObjAssessmentRuleData.AssessmentAmount.GetValueOrDefault(),
                                        AssessmentRuleID = mObjAssessmentRuleData.AssessmentRuleID.GetValueOrDefault(),
                                        AssessmentRuleName = mObjAssessmentRuleData.AssessmentRuleCode + " - " + mObjAssessmentRuleData.AssessmentRuleName,
                                        intTrack = EnumList.Track.INSERT,
                                        TaxYear = mObjAssessmentRuleData.TaxYear.GetValueOrDefault(),
                                    };
                                    lstAssessmentRules.Add(mObjAssessmentRule);



                                    if (!string.IsNullOrWhiteSpace(mObjAssessmentRuleData.AssessmentItemIds))
                                    {
                                        string[] strArrAssessmentItemIds = mObjAssessmentRuleData.AssessmentItemIds.Split(',');

                                        foreach (string strAssessmentItemID in strArrAssessmentItemIds)
                                        {
                                            if (TrynParse.parseInt(strAssessmentItemID) > 0)
                                            {
                                                usp_GetAssessmentItemList_Result mObjAssessmentItem = mObjBLAssessmentItem.BL_GetAssessmentItemDetails(new Assessment_Items() { intStatus = 2, AssessmentItemID = TrynParse.parseInt(strAssessmentItemID) });

                                                Assessment_AssessmentItem mObjAssessmentRuleItem = new Assessment_AssessmentItem()
                                                {
                                                    RowID = lstAssessmentItems.Count + 1
                                                };

                                                mObjAssessmentRuleItem.AssessmentRule_RowID = mObjAssessmentRule.RowID;
                                                mObjAssessmentRuleItem.AssessmentItemID = mObjAssessmentItem.AssessmentItemID.GetValueOrDefault();
                                                mObjAssessmentRuleItem.AssessmentItemReferenceNo = mObjAssessmentItem.AssessmentItemReferenceNo;
                                                mObjAssessmentRuleItem.AssessmentItemName = mObjAssessmentItem.AssessmentItemName;
                                                mObjAssessmentRuleItem.ComputationID = mObjAssessmentItem.ComputationID.GetValueOrDefault();
                                                mObjAssessmentRuleItem.ComputationName = mObjAssessmentItem.ComputationName;
                                                mObjAssessmentRuleItem.Percentage = mObjAssessmentItem.Percentage.GetValueOrDefault();
                                                mObjAssessmentRuleItem.TaxAmount = mObjAssessmentItem.TaxAmount.GetValueOrDefault();
                                                mObjAssessmentRuleItem.TaxBaseAmount = mObjAssessmentItem.TaxBaseAmount.GetValueOrDefault();

                                                lstAssessmentItems.Add(mObjAssessmentRuleItem);
                                            }
                                        }
                                    }
                                }
                            }

                        }
                    }

                    SessionManager.lstAssessmentRule = lstAssessmentRules;
                    SessionManager.lstAssessmentItem = lstAssessmentItems;

                    ViewBag.AssessmentRuleList = lstAssessmentRules.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();

                    return View(mObjAssessmentModel);
                }
                else
                {
                    return RedirectToAction("Search", "CaptureCorporate");
                }
            }
            else
            {
                return RedirectToAction("Search", "CaptureCorporate");
            }
        }

        [HttpPost]

        [ValidateAntiForgeryToken()]
        public ActionResult AddAssessment(NewAssessmentViewModel pObjAssessmentModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.AssessmentRuleList = SessionManager.lstAssessmentRule.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
                return View(pObjAssessmentModel);
            }
            else
            {

                IList<Assessment_AssessmentItem> lstAssessmentItems = SessionManager.lstAssessmentItem ?? new List<Assessment_AssessmentItem>();

                IList<Assessment_AssessmentRule> lstAssessmentRules = SessionManager.lstAssessmentRule ?? new List<Assessment_AssessmentRule>();
                int IntAssessmentRuleCount = lstAssessmentRules.Where(t => t.intTrack != EnumList.Track.DELETE).Count();
                var rule = lstAssessmentRules.FirstOrDefault();

                if (IntAssessmentRuleCount == 0)
                {
                    ViewBag.Message = "Please Add Atleast One Assessment Rule";
                    ViewBag.AssessmentRuleList = SessionManager.lstAssessmentRule.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
                    return View(pObjAssessmentModel);
                }
                else
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        BLAssessment mObjBLAssessment = new BLAssessment();

                        Assessment mObjAssessment = new Assessment()
                        {
                            AssessmentID = 0,
                            TaxPayerID = pObjAssessmentModel.TaxPayerID,
                            TaxPayerTypeID = pObjAssessmentModel.TaxPayerTypeID,
                            AssessmentAmount = lstAssessmentRules.Count > 0 ? lstAssessmentRules.Sum(t => t.AssessmentRuleAmount) : 0,
                            AssessmentDate = CommUtil.GetCurrentDateTime(),
                            SettlementDueDate = pObjAssessmentModel.SettlementDuedate,
                            SettlementStatusID = (int)EnumList.SettlementStatus.Assessed,
                            AssessmentNotes = pObjAssessmentModel.Notes,
                            Active = true,
                            CreatedBy = SessionManager.UserID,
                            CreatedDate = CommUtil.GetCurrentDateTime()
                        };

                        try
                        {
                            FuncResponse<Assessment> mObjAssessmentResponse = _AssessmentRepository.REP_InsertUpdateAssessment(mObjAssessment, rule.AssessmentRuleID, rule.AssetID);

                            if (mObjAssessmentResponse.Success)
                            {
                                //Adding Asssessment Rules

                                foreach (Assessment_AssessmentRule mObjAAR in lstAssessmentRules.Where(t => t.intTrack != EnumList.Track.DELETE))
                                {
                                    MAP_Assessment_AssessmentRule mObjAssessmentRule = new MAP_Assessment_AssessmentRule()
                                    {
                                        AARID = 0,
                                        TaxPayerID = pObjAssessmentModel.TaxPayerID,
                                        TaxPayerTypeID = pObjAssessmentModel.TaxPayerTypeID,
                                        AssessmentID = mObjAssessmentResponse.AdditionalData.AssessmentID,
                                        AssetTypeID = mObjAAR.AssetTypeID,
                                        AssetID = mObjAAR.AssetID,
                                        ProfileID = mObjAAR.ProfileID,
                                        AssessmentRuleID = mObjAAR.AssessmentRuleID,
                                        AssessmentAmount = lstAssessmentRules.Count > 0 ? lstAssessmentItems.Sum(t => t.TaxBaseAmount) : 0,
                                        AssessmentYear = mObjAAR.TaxYear,
                                        CreatedBy = SessionManager.UserID,
                                        CreatedDate = CommUtil.GetCurrentDateTime()
                                    };

                                    FuncResponse<MAP_Assessment_AssessmentRule> mObjARResponse = mObjBLAssessment.BL_InsertUpdateAssessmentRule(mObjAssessmentRule);
                                    if (mObjARResponse.Success)
                                    {
                                        IList<MAP_Assessment_AssessmentItem> lstInsertAssessmentDetail = new List<MAP_Assessment_AssessmentItem>();
                                        if (lstAssessmentRules.Count > 1)
                                        {
                                            foreach (Assessment_AssessmentItem mObjAssessmentItemDetail in lstAssessmentItems.Where(t => t.AssessmentRule_RowID == mObjAAR.RowID))
                                            {
                                                MAP_Assessment_AssessmentItem mObjAIDetail = new MAP_Assessment_AssessmentItem()
                                                {
                                                    AARID = mObjARResponse.AdditionalData.AARID,
                                                    AAIID = 0,
                                                    AssessmentItemID = mObjAssessmentItemDetail.AssessmentItemID,
                                                    TaxBaseAmount = mObjAssessmentItemDetail.TaxBaseAmount,
                                                    TaxAmount = mObjAssessmentItemDetail.TaxBaseAmount,
                                                    Percentage = mObjAssessmentItemDetail.Percentage,
                                                    PaymentStatusID = (int)EnumList.PaymentStatus.Due,
                                                    CreatedBy = SessionManager.UserID,
                                                    CreatedDate = CommUtil.GetCurrentDateTime(),
                                                };

                                                lstInsertAssessmentDetail.Add(mObjAIDetail);
                                            }
                                        }
                                        else
                                        {
                                            foreach (Assessment_AssessmentItem mObjAssessmentItemDetail in lstAssessmentItems)
                                            {
                                                MAP_Assessment_AssessmentItem mObjAIDetail = new MAP_Assessment_AssessmentItem()
                                                {
                                                    AARID = mObjARResponse.AdditionalData.AARID,
                                                    AAIID = 0,
                                                    AssessmentItemID = mObjAssessmentItemDetail.AssessmentItemID,
                                                    TaxBaseAmount = mObjAssessmentItemDetail.TaxBaseAmount,
                                                    TaxAmount = mObjAssessmentItemDetail.TaxBaseAmount,
                                                    Percentage = mObjAssessmentItemDetail.Percentage,
                                                    PaymentStatusID = (int)EnumList.PaymentStatus.Due,
                                                    CreatedBy = SessionManager.UserID,
                                                    CreatedDate = CommUtil.GetCurrentDateTime(),
                                                };

                                                lstInsertAssessmentDetail.Add(mObjAIDetail);
                                            }
                                        }
                                        FuncResponse mObjADResponse = mObjBLAssessment.BL_InsertAssessmentItem(lstInsertAssessmentDetail);

                                        if (!mObjADResponse.Success)
                                        {
                                            throw (mObjADResponse.Exception);
                                        }
                                    }
                                    else
                                    {
                                        throw (mObjARResponse.Exception);
                                    }
                                    //if (mObjARResponse.Success)
                                    //{
                                    //IList<MAP_Assessment_AssessmentItem> lstInsertAssessmentDetail = new List<MAP_Assessment_AssessmentItem>();

                                    //foreach (Assessment_AssessmentItem mObjAssessmentItemDetail in lstAssessmentItems.Where(t => t.RowID == mObjAAR.RowID))
                                    //{
                                    //    MAP_Assessment_AssessmentItem mObjAIDetail = new MAP_Assessment_AssessmentItem()
                                    //    {
                                    //        AARID = mObjARResponse.AdditionalData.AARID,
                                    //        AAIID = 0,
                                    //        AssessmentItemID = mObjAssessmentItemDetail.AssessmentItemID,
                                    //        TaxBaseAmount = mObjAssessmentItemDetail.TaxBaseAmount,
                                    //        TaxAmount = mObjAssessmentItemDetail.TaxAmount,
                                    //        Percentage = mObjAssessmentItemDetail.Percentage,
                                    //        PaymentStatusID = (int)EnumList.PaymentStatus.Due,
                                    //        CreatedBy = SessionManager.UserID,
                                    //        CreatedDate = CommUtil.GetCurrentDateTime(),
                                    //    };

                                    //    lstInsertAssessmentDetail.Add(mObjAIDetail);
                                    //}

                                    //FuncResponse mObjADResponse = mObjBLAssessment.BL_InsertAssessmentItem(lstInsertAssessmentDetail);

                                    //if (!mObjADResponse.Success)
                                    //{
                                    //    throw (mObjADResponse.Exception);
                                    //}
                                    //}
                                    //else
                                    //{
                                    //    throw (mObjARResponse.Exception);
                                    //}
                                }



                                //Send Notification
                                if (GlobalDefaultValues.SendNotification)
                                {
                                    usp_GetAssessmentList_Result mObjAssessmentData = mObjBLAssessment.BL_GetAssessmentDetails(new Assessment() { AssessmentID = mObjAssessmentResponse.AdditionalData.AssessmentID, IntStatus = 2 });
                                    usp_GetCompanyList_Result mObjIndividualData = new BLCompany().BL_GetCompanyDetails(new Company() { CompanyID = pObjAssessmentModel.TaxPayerID, intStatus = 1 });
                                    IList<usp_GetAssessment_AssessmentRuleList_Result> lstMAPAssessmentRules = mObjBLAssessment.BL_GetAssessmentRules(mObjAssessmentResponse.AdditionalData.AssessmentID);

                                    string AssessmentRuleNames = string.Join(",", lstMAPAssessmentRules.Select(t => t.AssessmentRuleName).ToArray());
                                    if (mObjIndividualData != null && mObjAssessmentData != null)
                                    {
                                        EmailDetails mObjEmailDetails = new EmailDetails()
                                        {
                                            TaxPayerTypeID = mObjIndividualData.TaxPayerTypeID.GetValueOrDefault(),
                                            TaxPayerTypeName = mObjIndividualData.TaxPayerTypeName,
                                            TaxPayerID = mObjIndividualData.CompanyID.GetValueOrDefault(),
                                            TaxPayerName = mObjIndividualData.CompanyName,
                                            TaxPayerRIN = mObjIndividualData.CompanyRIN,
                                            TaxPayerMobileNumber = mObjIndividualData.MobileNumber1,
                                            TaxPayerEmail = mObjIndividualData.EmailAddress1,
                                            BillRefNo = mObjAssessmentData.AssessmentRefNo,
                                            BillAmount = CommUtil.GetFormatedCurrency(mObjAssessmentData.AssessmentAmount),
                                            BillTypeName = "Assessment Bill",
                                            LoggedInUserID = SessionManager.UserID,
                                            RuleNames = AssessmentRuleNames
                                        };

                                        if (!string.IsNullOrWhiteSpace(mObjIndividualData.EmailAddress1))
                                        {
                                            BLEmailHandler.BL_BillGenerated(mObjEmailDetails);
                                        }

                                        if (!string.IsNullOrWhiteSpace(mObjIndividualData.MobileNumber1))
                                        {
                                            UtilityController.BL_BillGenerated(mObjEmailDetails);
                                        }
                                    }
                                }
                                scope.Complete();
                                FlashMessage.Info(mObjAssessmentResponse.Message);
                                return RedirectToAction("Details", "CaptureCorporate", new { id = pObjAssessmentModel.TaxPayerID, name = pObjAssessmentModel.TaxPayerRIN });

                            }
                            else
                            {
                                ViewBag.AssessmentRuleList = SessionManager.lstAssessmentRule.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
                                ViewBag.Message = mObjAssessmentResponse.Message;
                                Transaction.Current.Rollback();

                                if (mObjAssessmentResponse.Exception != null)
                                {
                                    throw mObjAssessmentResponse.Exception;
                                }

                                return View(pObjAssessmentModel);
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.SendErrorToText(ex);
                            ErrorSignal.FromCurrentContext().Raise(ex);
                            ViewBag.AssessmentRuleList = SessionManager.lstAssessmentRule.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
                            if (ex.Message == "ARALREADY")
                            {
                                ViewBag.Message = "Assessment rules added multiple times and not valid.";
                            }
                            else if (ex.Message == "ARNOTFOUND")
                            {
                                ViewBag.Message = "Assessment rules not found in assessment.";
                            }
                            else if (ex.Message == "AINOTFOUND")
                            {
                                ViewBag.Message = "Assessment items not found in assessment.";
                            }
                            else
                            {
                                ViewBag.Message = ex.Message; //"Error occurred while saving assessment";
                            }
                            Transaction.Current.Rollback();
                            return View(pObjAssessmentModel);
                        }

                    }
                }
            }
        }


        //public ActionResult AddAssessment(int? id, string name, string aruleIds = null)
        //{
        //    if (id.GetValueOrDefault() > 0)
        //    {
        //        Company mObjCompany = new Company()
        //        {
        //            CompanyID = id.GetValueOrDefault(),
        //            intStatus = 1
        //        };

        //        usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(mObjCompany);

        //        if (mObjCompanyData != null)
        //        {
        //            AssessmentViewModel mObjAssessmentModel = new AssessmentViewModel()
        //            {
        //                TaxPayerID = mObjCompanyData.CompanyID.GetValueOrDefault(),
        //                TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies,
        //                TaxPayerRIN = mObjCompanyData.CompanyRIN,
        //                TaxPayerName = mObjCompanyData.CompanyName,
        //                TaxPayerAddress = mObjCompanyData.ContactAddress,
        //                SettlementDuedate = DateTime.Now,
        //                AssessmentDate = DateTime.Now
        //            };

        //            IList<Assessment_AssessmentRule> lstAssessmentRules = new List<Assessment_AssessmentRule>();
        //            IList<Assessment_AssessmentItem> lstAssessmentItems = new List<Assessment_AssessmentItem>();

        //            BLAssessmentRule mObjBLAssessmentRule = new BLAssessmentRule();
        //            BLAssessmentItem mObjBLAssessmentItem = new BLAssessmentItem();

        //            string strAssessmentRuleIds = EncryptDecrypt.Decrypt(aruleIds);

        //            string[] strArrAssessmentRuleIds = strAssessmentRuleIds?.Split(',');

        //            foreach (string strARuleData in strArrAssessmentRuleIds)
        //            {
        //                if (!string.IsNullOrWhiteSpace(strARuleData))
        //                {
        //                    string[] strRuleData = strARuleData.Split(new string[] { "@@" }, StringSplitOptions.RemoveEmptyEntries);

        //                    if (strRuleData.Length == 4)
        //                    {
        //                        usp_GetAssessmentRuleList_Result mObjAssessmentRuleData = mObjBLAssessmentRule.BL_GetAssessmentRuleDetails(new Assessment_Rules() { AssessmentRuleID = TrynParse.parseInt(strRuleData[0]), IntStatus = 2 });

        //                        if (mObjAssessmentRuleData != null)
        //                        {

        //                            Assessment_AssessmentRule mObjAssessmentRule = new Assessment_AssessmentRule()
        //                            {
        //                                RowID = lstAssessmentRules.Count + 1,
        //                                AssetID = TrynParse.parseInt(strRuleData[3]),
        //                                AssetTypeID = TrynParse.parseInt(strRuleData[2]),
        //                                ProfileID = TrynParse.parseInt(strRuleData[1]),
        //                                AssessmentRuleID = mObjAssessmentRuleData.AssessmentRuleID.GetValueOrDefault(),
        //                                AssessmentRuleName = mObjAssessmentRuleData.AssessmentRuleCode + " - " + mObjAssessmentRuleData.AssessmentRuleName,
        //                                intTrack = EnumList.Track.INSERT,
        //                                TaxYear = mObjAssessmentRuleData.TaxYear.GetValueOrDefault(),
        //                            };

        //                            lstAssessmentRules.Add(mObjAssessmentRule);

        //                            if (!string.IsNullOrWhiteSpace(mObjAssessmentRuleData.AssessmentItemIds))
        //                            {
        //                                string[] strArrAssessmentItemIds = mObjAssessmentRuleData.AssessmentItemIds.Split(',');

        //                                foreach (string strAssessmentItemID in strArrAssessmentItemIds)
        //                                {
        //                                    if (TrynParse.parseInt(strAssessmentItemID) > 0)
        //                                    {
        //                                        usp_GetAssessmentItemList_Result mObjAssessmentItem = mObjBLAssessmentItem.BL_GetAssessmentItemDetails(new Assessment_Items() { intStatus = 2, AssessmentItemID = TrynParse.parseInt(strAssessmentItemID) });

        //                                        Assessment_AssessmentItem mObjAssessmentRuleItem = new Assessment_AssessmentItem()
        //                                        {
        //                                            RowID = lstAssessmentItems.Count + 1
        //                                        };

        //                                        mObjAssessmentRuleItem.AssessmentRule_RowID = mObjAssessmentRule.RowID;
        //                                        mObjAssessmentRuleItem.AssessmentItemID = mObjAssessmentItem.AssessmentItemID.GetValueOrDefault();
        //                                        mObjAssessmentRuleItem.AssessmentItemReferenceNo = mObjAssessmentItem.AssessmentItemReferenceNo;
        //                                        mObjAssessmentRuleItem.AssessmentItemName = mObjAssessmentItem.AssessmentItemName;
        //                                        mObjAssessmentRuleItem.ComputationID = mObjAssessmentItem.ComputationID.GetValueOrDefault();
        //                                        mObjAssessmentRuleItem.ComputationName = mObjAssessmentItem.ComputationName;
        //                                        mObjAssessmentRuleItem.Percentage = mObjAssessmentItem.Percentage.GetValueOrDefault();

        //                                        lstAssessmentItems.Add(mObjAssessmentRuleItem);
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }

        //            SessionManager.lstAssessmentRule = lstAssessmentRules;
        //            SessionManager.lstAssessmentItem = lstAssessmentItems;

        //            ViewBag.AssessmentRuleList = lstAssessmentRules.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();

        //            return View(mObjAssessmentModel);
        //        }
        //        else
        //        {
        //            return RedirectToAction("Search", "CaptureCorporate");
        //        }
        //    }
        //    else
        //    {
        //        return RedirectToAction("Search", "CaptureCorporate");
        //    }
        //}

        //[HttpPost]

        //[ValidateAntiForgeryToken()]
        //public ActionResult AddAssessment(AssessmentViewModel pObjAssessmentModel)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        ViewBag.AssessmentRuleList = SessionManager.lstAssessmentRule.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();

        //        return View(pObjAssessmentModel);
        //    }
        //    else
        //    {
        //        IList<Assessment_AssessmentItem> lstAssessmentItems = SessionManager.lstAssessmentItem ?? new List<Assessment_AssessmentItem>();
        //        IList<Assessment_AssessmentRule> lstAssessmentRules = SessionManager.lstAssessmentRule ?? new List<Assessment_AssessmentRule>();
        //        int IntAssessmentRuleCount = lstAssessmentRules.Where(t => t.intTrack != EnumList.Track.DELETE).Count();

        //        if (IntAssessmentRuleCount == 0)
        //        {
        //            ViewBag.Message = "Please Add Atleast One Assessment Rule";
        //            ViewBag.AssessmentRuleList = SessionManager.lstAssessmentRule.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
        //            return View(pObjAssessmentModel);
        //        }
        //        else
        //        {
        //            using (TransactionScope scope = new TransactionScope())
        //            {
        //                BLAssessment mObjBLAssessment = new BLAssessment();

        //                Assessment mObjAssessment = new Assessment()
        //                {
        //                    AssessmentID = 0,
        //                    TaxPayerID = pObjAssessmentModel.TaxPayerID,
        //                    TaxPayerTypeID = pObjAssessmentModel.TaxPayerTypeID,
        //                    AssessmentAmount = lstAssessmentItems.Count > 0 ? lstAssessmentItems.Where(t => t.intTrack != EnumList.Track.DELETE).Sum(t => t.TaxAmount) : 0,
        //                    AssessmentDate = CommUtil.GetCurrentDateTime(),
        //                    SettlementDueDate = pObjAssessmentModel.SettlementDuedate,
        //                    SettlementStatusID = (int)EnumList.SettlementStatus.Assessed,
        //                    AssessmentNotes = pObjAssessmentModel.Notes,
        //                    Active = true,
        //                    CreatedBy = SessionManager.UserID,
        //                    CreatedDate = CommUtil.GetCurrentDateTime()
        //                };

        //                try
        //                {

        //                    FuncResponse<Assessment> mObjAssessmentResponse = _AssessmentRepository.REP_InsertUpdateAssessment(mObjAssessment, 0);

        //                    if (mObjAssessmentResponse.Success)
        //                    {
        //                        //Adding Asssessment Rules

        //                        foreach (Assessment_AssessmentRule mObjAAR in lstAssessmentRules.Where(t => t.intTrack != EnumList.Track.DELETE))
        //                        {
        //                            MAP_Assessment_AssessmentRule mObjAssessmentRule = new MAP_Assessment_AssessmentRule()
        //                            {
        //                                AARID = 0,
        //                                TaxPayerID = pObjAssessmentModel.TaxPayerID,
        //                                TaxPayerTypeID = pObjAssessmentModel.TaxPayerTypeID,
        //                                AssessmentID = mObjAssessmentResponse.AdditionalData.AssessmentID,
        //                                AssetTypeID = mObjAAR.AssetTypeID,
        //                                AssetID = mObjAAR.AssetID,
        //                                ProfileID = mObjAAR.ProfileID,
        //                                AssessmentRuleID = mObjAAR.AssessmentRuleID,
        //                                AssessmentAmount = mObjAAR.AssessmentRuleAmount,
        //                                AssessmentYear = mObjAAR.TaxYear,
        //                                CreatedBy = SessionManager.UserID,
        //                                CreatedDate = CommUtil.GetCurrentDateTime()
        //                            };

        //                            FuncResponse<MAP_Assessment_AssessmentRule> mObjARResponse = mObjBLAssessment.BL_InsertUpdateAssessmentRule(mObjAssessmentRule);

        //                            if (mObjARResponse.Success)
        //                            {
        //                                IList<MAP_Assessment_AssessmentItem> lstInsertAssessmentDetail = new List<MAP_Assessment_AssessmentItem>();

        //                                foreach (Assessment_AssessmentItem mObjAssessmentItemDetail in lstAssessmentItems.Where(t => t.AssessmentRule_RowID == mObjAAR.RowID))
        //                                {
        //                                    MAP_Assessment_AssessmentItem mObjAIDetail = new MAP_Assessment_AssessmentItem()
        //                                    {
        //                                        AARID = mObjARResponse.AdditionalData.AARID,
        //                                        AAIID = 0,
        //                                        AssessmentItemID = mObjAssessmentItemDetail.AssessmentItemID,
        //                                        TaxBaseAmount = mObjAssessmentItemDetail.TaxBaseAmount,
        //                                        TaxAmount = mObjAssessmentItemDetail.TaxAmount,
        //                                        Percentage = mObjAssessmentItemDetail.Percentage,
        //                                        PaymentStatusID = (int)EnumList.PaymentStatus.Due,
        //                                        CreatedBy = SessionManager.UserID,
        //                                        CreatedDate = CommUtil.GetCurrentDateTime(),
        //                                    };

        //                                    lstInsertAssessmentDetail.Add(mObjAIDetail);
        //                                }

        //                                FuncResponse mObjADResponse = mObjBLAssessment.BL_InsertAssessmentItem(lstInsertAssessmentDetail);

        //                                if (!mObjADResponse.Success)
        //                                {
        //                                    throw (mObjADResponse.Exception);
        //                                }
        //                            }
        //                            else
        //                            {
        //                                throw (mObjARResponse.Exception);
        //                            }
        //                        }



        //                        //Send Notification
        //                        if (GlobalDefaultValues.SendNotification)
        //                        {
        //                            usp_GetAssessmentList_Result mObjAssessmentData = mObjBLAssessment.BL_GetAssessmentDetails(new Assessment() { AssessmentID = mObjAssessmentResponse.AdditionalData.AssessmentID, IntStatus = 2 });
        //                            usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(new Company() { CompanyID = pObjAssessmentModel.TaxPayerID, intStatus = 1 });
        //                            IList<usp_GetAssessment_AssessmentRuleList_Result> lstMAPAssessmentRules = mObjBLAssessment.BL_GetAssessmentRules(mObjAssessmentResponse.AdditionalData.AssessmentID);
        //                            string AssessmentRuleNames = string.Join(",", lstMAPAssessmentRules.Select(t => t.AssessmentRuleName).ToArray());

        //                            if (mObjCompanyData != null && mObjAssessmentData != null)
        //                            {
        //                                EmailDetails mObjEmailDetails = new EmailDetails()
        //                                {
        //                                    TaxPayerTypeID = mObjCompanyData.TaxPayerTypeID.GetValueOrDefault(),
        //                                    TaxPayerTypeName = mObjCompanyData.TaxPayerTypeName,
        //                                    TaxPayerID = mObjCompanyData.CompanyID.GetValueOrDefault(),
        //                                    TaxPayerName = mObjCompanyData.CompanyName,
        //                                    TaxPayerRIN = mObjCompanyData.CompanyRIN,
        //                                    TaxPayerMobileNumber = mObjCompanyData.MobileNumber1,
        //                                    TaxPayerEmail = mObjCompanyData.EmailAddress1,
        //                                    BillRefNo = mObjAssessmentData.AssessmentRefNo,
        //                                    BillAmount = CommUtil.GetFormatedCurrency(mObjAssessmentData.AssessmentAmount),
        //                                    BillTypeName = "Assessment Bill",
        //                                    LoggedInUserID = SessionManager.UserID,
        //                                    RuleNames = AssessmentRuleNames
        //                                };

        //                                if (!string.IsNullOrWhiteSpace(mObjCompanyData.EmailAddress1))
        //                                {
        //                                    BLEmailHandler.BL_BillGenerated(mObjEmailDetails);
        //                                }

        //                                if (!string.IsNullOrWhiteSpace(mObjCompanyData.MobileNumber1))
        //                                {
        //                                    UtilityController.BL_BillGenerated(mObjEmailDetails);
        //                                }
        //                            }
        //                        }

        //                        scope.Complete();
        //                        FlashMessage.Info(mObjAssessmentResponse.Message);
        //                        return RedirectToAction("Details", "CaptureCorporate", new { id = pObjAssessmentModel.TaxPayerID, name = pObjAssessmentModel.TaxPayerRIN });

        //                    }
        //                    else
        //                    {
        //                        ViewBag.AssessmentRuleList = SessionManager.lstAssessmentRule.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
        //                        ViewBag.Message = mObjAssessmentResponse.Message;
        //                        Transaction.Current.Rollback();
        //                        return View(pObjAssessmentModel);
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    ErrorSignal.FromCurrentContext().Raise(ex);
        //                    ViewBag.AssessmentRuleList = SessionManager.lstAssessmentRule.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
        //                    if (ex.Message == "ARALREADY")
        //                    {
        //                        ViewBag.Message = "Assessment rules added multiple times and not valid.";
        //                    }
        //                    else if (ex.Message == "ARNOTFOUND")
        //                    {
        //                        ViewBag.Message = "Assessment rules not found in assessment.";
        //                    }
        //                    else if (ex.Message == "AINOTFOUND")
        //                    {
        //                        ViewBag.Message = "Assessment items not found in assessment.";
        //                    }
        //                    else
        //                    {
        //                        ViewBag.Message = "Error occurred while saving assessment";
        //                    }
        //                    Transaction.Current.Rollback();
        //                    return View(pObjAssessmentModel);
        //                }

        //            }
        //        }
        //    }
        //}


        public ActionResult EditAssessment(int? id, string name, int? aid)
        {
            string url = getUrl();
            bool itCan = new UtilityController().CheckAccess(url);
            if (!itCan) { return RedirectToAction("AccessDenied", "Utility"); }
            if (id.GetValueOrDefault() > 0 && aid.GetValueOrDefault() > 0)
            {
                Company mObjCompany = new Company()
                {
                    CompanyID = id.GetValueOrDefault(),
                    intStatus = 1
                };

                usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(mObjCompany);

                BLAssessment mObjBLAssessment = new BLAssessment();
                BLAssessmentItem mObjBLAssessmentItem = new BLAssessmentItem();
                usp_GetAssessmentList_Result mObjAssessmentData = mObjBLAssessment.BL_GetAssessmentDetails(new Assessment() { AssessmentID = aid.GetValueOrDefault(), IntStatus = 2 });

                if (mObjCompanyData != null && mObjAssessmentData != null)
                {
                    AssessmentViewModel mObjAssessmentModel = new AssessmentViewModel()
                    {
                        TaxPayerID = mObjCompanyData.CompanyID.GetValueOrDefault(),
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies,
                        TaxPayerRIN = mObjCompanyData.CompanyRIN,
                        TaxPayerName = mObjCompanyData.CompanyName,
                        TaxPayerAddress = mObjCompanyData.ContactAddress,
                        AssessmentID = aid.GetValueOrDefault(),
                        Notes = mObjAssessmentData.AssessmentNotes,
                        SettlementDuedate = mObjAssessmentData.SettlementDueDate.GetValueOrDefault(),
                        AssessmentDate = mObjAssessmentData.AssessmentDate,
                    };

                    IList<Assessment_AssessmentRule> lstAssessmentRule = new List<Assessment_AssessmentRule>();
                    IList<Assessment_AssessmentItem> lstAssessmentItem = new List<Assessment_AssessmentItem>();

                    IList<usp_GetAssessment_AssessmentRuleList_Result> lstMAPAssessmentRules = mObjBLAssessment.BL_GetAssessmentRules(mObjAssessmentData.AssessmentID.GetValueOrDefault());
                    IList<MAP_Assessment_AssessmentItem> lstMAPAssessmentItems;

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
                            intTrack = EnumList.Track.EXISTING
                        };

                        lstAssessmentRule.Add(assessment_AssessmentRule);

                        lstMAPAssessmentItems = mObjBLAssessment.BL_GetAssessmentItems(item.AARID.GetValueOrDefault());

                        foreach (var subitem in lstMAPAssessmentItems)
                        {
                            usp_GetAssessmentItemList_Result mObjAssessmentItemData = mObjBLAssessmentItem.BL_GetAssessmentItemDetails(new Assessment_Items() { intStatus = 2, AssessmentItemID = subitem.AssessmentItemID.GetValueOrDefault() });

                            Assessment_AssessmentItem mObjAssessmentItem = new Assessment_AssessmentItem()
                            {
                                RowID = lstAssessmentItem.Count + 1,
                                AssessmentRule_RowID = assessment_AssessmentRule.RowID,
                                TablePKID = subitem.AAIID,
                                AssessmentItemID = subitem.AssessmentItemID.GetValueOrDefault(),
                                AssessmentItemName = mObjAssessmentItemData.AssessmentItemName,
                                AssessmentItemReferenceNo = mObjAssessmentItemData.AssessmentItemReferenceNo,
                                ComputationID = mObjAssessmentItemData.ComputationID.GetValueOrDefault(),
                                ComputationName = mObjAssessmentItemData.ComputationName,
                                TaxBaseAmount = subitem.TaxBaseAmount.GetValueOrDefault(),
                                TaxAmount = subitem.TaxAmount.GetValueOrDefault(),
                                Percentage = subitem.Percentage.GetValueOrDefault(),
                                intTrack = EnumList.Track.EXISTING
                            };

                            lstAssessmentItem.Add(mObjAssessmentItem);
                        }
                    }

                    SessionManager.lstAssessmentRule = lstAssessmentRule;
                    SessionManager.lstAssessmentItem = lstAssessmentItem;

                    ViewBag.AssessmentRuleList = SessionManager.lstAssessmentRule.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();

                    return View(mObjAssessmentModel);
                }
                else
                {
                    return RedirectToAction("Search", "CaptureCorporate");
                }
            }
            else
            {
                return RedirectToAction("Search", "CaptureCorporate");
            }
        }

        [HttpPost]

        [ValidateAntiForgeryToken()]
        public ActionResult EditAssessment(AssessmentViewModel pObjAssessmentModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.AssessmentRuleList = SessionManager.lstAssessmentRule.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
                return View(pObjAssessmentModel);
            }
            else
            {
                IList<Assessment_AssessmentItem> lstAssessmentItems = SessionManager.lstAssessmentItem ?? new List<Assessment_AssessmentItem>();
                IList<Assessment_AssessmentRule> lstAssessmentRules = SessionManager.lstAssessmentRule ?? new List<Assessment_AssessmentRule>();
                int IntAssessmentRuleCount = lstAssessmentRules.Where(t => t.intTrack != EnumList.Track.DELETE).Count();

                if (IntAssessmentRuleCount == 0)
                {
                    ViewBag.Message = "Please Add Atleast One Assessment Rule";
                    ViewBag.AssessmentRuleList = SessionManager.lstAssessmentRule.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
                    return View(pObjAssessmentModel);
                }
                else
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        BLAssessment mObjBLAssessment = new BLAssessment();

                        Assessment mObjAssessment = new Assessment()
                        {
                            AssessmentID = pObjAssessmentModel.AssessmentID,
                            AssessmentAmount = lstAssessmentItems.Count > 0 ? lstAssessmentItems.Where(t => t.intTrack != EnumList.Track.DELETE).Sum(t => t.TaxAmount) : 0,
                            SettlementDueDate = pObjAssessmentModel.SettlementDuedate,
                            AssessmentNotes = pObjAssessmentModel.Notes,
                            ModifiedBy = SessionManager.UserID,
                            ModifiedDate = CommUtil.GetCurrentDateTime()
                        };

                        try
                        {

                            FuncResponse<Assessment> mObjAssessmentResponse = mObjBLAssessment.BL_InsertUpdateAssessment(mObjAssessment);

                            if (mObjAssessmentResponse.Success)
                            {
                                //Adding Asssessment Rules

                                foreach (Assessment_AssessmentRule mObjAAR in lstAssessmentRules)
                                {
                                    if (mObjAAR.intTrack == EnumList.Track.INSERT)
                                    {
                                        MAP_Assessment_AssessmentRule mObjAssessmentRule = new MAP_Assessment_AssessmentRule()
                                        {
                                            AARID = 0,
                                            TaxPayerID = pObjAssessmentModel.TaxPayerID,
                                            TaxPayerTypeID = pObjAssessmentModel.TaxPayerTypeID,
                                            AssessmentID = mObjAssessmentResponse.AdditionalData.AssessmentID,
                                            AssetTypeID = mObjAAR.AssetTypeID,
                                            AssetID = mObjAAR.AssetID,
                                            ProfileID = mObjAAR.ProfileID,
                                            AssessmentRuleID = mObjAAR.AssessmentRuleID,
                                            AssessmentAmount = mObjAAR.AssessmentRuleAmount,
                                            AssessmentYear = mObjAAR.TaxYear,
                                            CreatedBy = SessionManager.UserID,
                                            CreatedDate = CommUtil.GetCurrentDateTime()
                                        };

                                        FuncResponse<MAP_Assessment_AssessmentRule> mObjARResponse = mObjBLAssessment.BL_InsertUpdateAssessmentRule(mObjAssessmentRule);

                                        if (mObjARResponse.Success)
                                        {
                                            IList<MAP_Assessment_AssessmentItem> lstInsertAssessmentDetail = new List<MAP_Assessment_AssessmentItem>();

                                            foreach (Assessment_AssessmentItem mObjAssessmentItemDetail in lstAssessmentItems.Where(t => t.AssessmentRule_RowID == mObjAAR.RowID))
                                            {
                                                MAP_Assessment_AssessmentItem mObjAIDetail = new MAP_Assessment_AssessmentItem()
                                                {
                                                    AARID = mObjARResponse.AdditionalData.AARID,
                                                    AAIID = 0,
                                                    AssessmentItemID = mObjAssessmentItemDetail.AssessmentItemID,
                                                    TaxBaseAmount = mObjAssessmentItemDetail.TaxBaseAmount,
                                                    TaxAmount = mObjAssessmentItemDetail.TaxAmount,
                                                    Percentage = mObjAssessmentItemDetail.Percentage,
                                                    PaymentStatusID = (int)EnumList.PaymentStatus.Due,
                                                    CreatedBy = SessionManager.UserID,
                                                    CreatedDate = CommUtil.GetCurrentDateTime(),
                                                };

                                                lstInsertAssessmentDetail.Add(mObjAIDetail);
                                            }

                                            FuncResponse mObjADResponse = mObjBLAssessment.BL_InsertAssessmentItem(lstInsertAssessmentDetail);

                                            if (!mObjADResponse.Success)
                                            {
                                                throw (mObjADResponse.Exception);
                                            }
                                        }
                                        else
                                        {
                                            throw (mObjARResponse.Exception);
                                        }
                                    }
                                    else if (mObjAAR.intTrack == EnumList.Track.DELETE)
                                    {
                                        FuncResponse mObjARResponse = mObjBLAssessment.BL_DeleteAssessmentRule(mObjAAR.TablePKID);

                                        if (!mObjARResponse.Success)
                                        {
                                            throw (mObjARResponse.Exception);
                                        }
                                    }
                                    else if (mObjAAR.intTrack == EnumList.Track.UPDATE)
                                    {
                                        MAP_Assessment_AssessmentRule mObjAssessmentRule = new MAP_Assessment_AssessmentRule()
                                        {
                                            AARID = mObjAAR.TablePKID,
                                            AssessmentAmount = mObjAAR.AssessmentRuleAmount,
                                            ModifiedBy = SessionManager.UserID,
                                            ModifiedDate = CommUtil.GetCurrentDateTime()
                                        };

                                        FuncResponse<MAP_Assessment_AssessmentRule> mObjARResponse = mObjBLAssessment.BL_InsertUpdateAssessmentRule(mObjAssessmentRule);

                                        if (mObjARResponse.Success)
                                        {
                                            IList<MAP_Assessment_AssessmentItem> lstInsertAssessmentDetail = new List<MAP_Assessment_AssessmentItem>();

                                            foreach (Assessment_AssessmentItem mObjAssessmentItemDetail in lstAssessmentItems.Where(t => t.AssessmentRule_RowID == mObjAAR.RowID))
                                            {
                                                MAP_Assessment_AssessmentItem mObjAIDetail = new MAP_Assessment_AssessmentItem()
                                                {
                                                    AARID = mObjARResponse.AdditionalData.AARID,
                                                    AAIID = mObjAssessmentItemDetail.TablePKID,
                                                    AssessmentItemID = mObjAssessmentItemDetail.AssessmentItemID,
                                                    TaxBaseAmount = mObjAssessmentItemDetail.TaxBaseAmount,
                                                    TaxAmount = mObjAssessmentItemDetail.TaxAmount,
                                                    Percentage = mObjAssessmentItemDetail.Percentage,
                                                    PaymentStatusID = (int)EnumList.PaymentStatus.Due,
                                                    ModifiedBy = SessionManager.UserID,
                                                    ModifiedDate = CommUtil.GetCurrentDateTime(),
                                                };

                                                lstInsertAssessmentDetail.Add(mObjAIDetail);
                                            }

                                            FuncResponse mObjADResponse = mObjBLAssessment.BL_InsertAssessmentItem(lstInsertAssessmentDetail);

                                            if (!mObjADResponse.Success)
                                            {
                                                throw (mObjADResponse.Exception);
                                            }
                                        }
                                    }
                                }

                                //Send Notification
                                if (GlobalDefaultValues.SendNotification)
                                {
                                    usp_GetAssessmentList_Result mObjAssessmentData = mObjBLAssessment.BL_GetAssessmentDetails(new Assessment() { AssessmentID = mObjAssessmentResponse.AdditionalData.AssessmentID, IntStatus = 2 });
                                    usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(new Company() { CompanyID = pObjAssessmentModel.TaxPayerID, intStatus = 1 });
                                    IList<usp_GetAssessment_AssessmentRuleList_Result> lstMAPAssessmentRules = mObjBLAssessment.BL_GetAssessmentRules(mObjAssessmentResponse.AdditionalData.AssessmentID);

                                    string AssessmentRuleNames = string.Join(",", lstMAPAssessmentRules.Select(t => t.AssessmentRuleName).ToArray());

                                    if (mObjCompanyData != null && mObjAssessmentData != null)
                                    {
                                        EmailDetails mObjEmailDetails = new EmailDetails()
                                        {
                                            TaxPayerTypeID = mObjCompanyData.TaxPayerTypeID.GetValueOrDefault(),
                                            TaxPayerTypeName = mObjCompanyData.TaxPayerTypeName,
                                            TaxPayerID = mObjCompanyData.CompanyID.GetValueOrDefault(),
                                            TaxPayerName = mObjCompanyData.CompanyName,
                                            TaxPayerRIN = mObjCompanyData.CompanyRIN,
                                            TaxPayerMobileNumber = mObjCompanyData.MobileNumber1,
                                            TaxPayerEmail = mObjCompanyData.EmailAddress1,
                                            BillRefNo = mObjAssessmentData.AssessmentRefNo,
                                            BillAmount = CommUtil.GetFormatedCurrency(mObjAssessmentData.AssessmentAmount),
                                            BillTypeName = "Assessment Bill",
                                            LoggedInUserID = SessionManager.UserID,
                                            RuleNames = AssessmentRuleNames
                                        };

                                        if (!string.IsNullOrWhiteSpace(mObjCompanyData.EmailAddress1))
                                        {
                                            BLEmailHandler.BL_BillGenerated(mObjEmailDetails);
                                        }

                                        if (!string.IsNullOrWhiteSpace(mObjCompanyData.MobileNumber1))
                                        {
                                            UtilityController.BL_BillGenerated(mObjEmailDetails);
                                        }
                                    }
                                }

                                Audit_Log mObjAuditLog = new Audit_Log()
                                {
                                    LogDate = CommUtil.GetCurrentDateTime(),
                                    ASLID = (int)EnumList.ALScreen.Capture_Corporate_Edit_Assessment,
                                    Comment = $"Assessment Bill Updated - {mObjAssessmentResponse.AdditionalData.AssessmentRefNo}",
                                    IPAddress = CommUtil.GetIPAddress(),
                                    StaffID = SessionManager.UserID,
                                };

                                new BLAuditLog().BL_InsertAuditLog(mObjAuditLog);

                                scope.Complete();
                                FlashMessage.Info(mObjAssessmentResponse.Message);
                                return RedirectToAction("Details", "CaptureCorporate", new { id = pObjAssessmentModel.TaxPayerID, name = pObjAssessmentModel.TaxPayerRIN });
                            }
                            else
                            {
                                ViewBag.AssessmentRuleList = SessionManager.lstAssessmentRule.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
                                ViewBag.Message = mObjAssessmentResponse.Message;
                                Transaction.Current.Rollback();
                                return View(pObjAssessmentModel);
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorSignal.FromCurrentContext().Raise(ex);
                            ViewBag.AssessmentRuleList = SessionManager.lstAssessmentRule.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
                            if (ex.Message == "ARALREADY")
                            {
                                ViewBag.Message = "Assessment rules added multiple times and not valid.";
                            }
                            else if (ex.Message == "ARNOTFOUND")
                            {
                                ViewBag.Message = "Assessment rules not found in assessment.";
                            }
                            else if (ex.Message == "AINOTFOUND")
                            {
                                ViewBag.Message = "Assessment items not found in assessment.";
                            }
                            else
                            {
                                ViewBag.Message = "Error occurred while updating assessment";
                            }

                            Transaction.Current.Rollback();
                            return View(pObjAssessmentModel);
                        }

                    }
                }
            }
        }


        public ActionResult GenerateServiceBill(int? id, string name)
        {
            string url = getUrl();
            bool itCan = new UtilityController().CheckAccess(url);
            if (!itCan) { return RedirectToAction("AccessDenied", "Utility"); }
            if (id.GetValueOrDefault() > 0)
            {
                Company mObjCompany = new Company()
                {
                    CompanyID = id.GetValueOrDefault(),
                    intStatus = 1
                };

                usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(mObjCompany);

                if (mObjCompanyData != null)
                {
                    GenerateServiceBillViewModel mObjGenerateServiceBillModel = new GenerateServiceBillViewModel()
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

                    IList<usp_GetMDAServiceForServiceBill_Result> lstMDAService = new BLMDAService().BL_GetMDAServiceForServiceBill((int)EnumList.TaxPayerType.Companies, id.GetValueOrDefault());
                    ViewBag.MDAServiceInformation = lstMDAService;

                    return View(mObjGenerateServiceBillModel);
                }
                else
                {
                    return RedirectToAction("Search", "CaptureCorporate");
                }
            }
            else
            {
                return RedirectToAction("Search", "CaptureCorporate");
            }
        }

        [HttpPost]

        [ValidateAntiForgeryToken()]
        public ActionResult GenerateServiceBill(GenerateServiceBillViewModel pObjGenerateServiceBillModel)
        {
            if (!ModelState.IsValid)
            {
                IList<usp_GetMDAServiceForServiceBill_Result> lstMDAService = new BLMDAService().BL_GetMDAServiceForServiceBill((int)EnumList.TaxPayerType.Companies, pObjGenerateServiceBillModel.TaxPayerID);
                ViewBag.MDAServiceInformation = lstMDAService;

                return View(pObjGenerateServiceBillModel);
            }
            else
            {
                return RedirectToAction("AddServiceBill", new { id = pObjGenerateServiceBillModel.TaxPayerID, name = pObjGenerateServiceBillModel.TaxPayerRIN, mdsIds = pObjGenerateServiceBillModel.MDAServiceIds });
            }
        }

        public ActionResult AddServiceBill(int? id, string name, string mdsIds = null)
        {
            string url = getUrl();
            bool itCan = new UtilityController().CheckAccess(url);
            if (!itCan) { return RedirectToAction("AccessDenied", "Utility"); }
            if (id.GetValueOrDefault() > 0)
            {
                Company mObjCompany = new Company()
                {
                    CompanyID = id.GetValueOrDefault(),
                    intStatus = 1
                };

                usp_GetCompanyList_Result mObjIndividualData = new BLCompany().BL_GetCompanyDetails(mObjCompany);

                if (mObjIndividualData != null)
                {
                    ServiceBillViewModel mObjServiceBillModel = new ServiceBillViewModel()
                    {
                        TaxPayerID = mObjIndividualData.CompanyID.GetValueOrDefault(),
                        TaxPayerRIN = mObjIndividualData.CompanyRIN,
                        TaxPayerName = mObjIndividualData.CompanyName,
                        TaxPayerAddress = mObjIndividualData.ContactAddress,
                        SettlementDuedate = CommUtil.GetCurrentDateTime(),
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies,
                    };

                    IList<ServiceBill_MDAService> lstMDAServices = new List<ServiceBill_MDAService>();
                    IList<ServiceBill_MDAServiceItem> lstMDAServiceItem = new List<ServiceBill_MDAServiceItem>();

                    BLMDAService mObjBLMDAService = new BLMDAService();
                    BLMDAServiceItem mObjBLMDAServiceItem = new BLMDAServiceItem();

                    //string strMDAServiceIds = Decrypt(mdsIds);
                    //string[] strArrMDAServiceIds = strMDAServiceIds.Split(',');

                    //string strMDAServiceIds = EncryptDecrypt.Decrypt(mdsIds);
                    //string[] strArrMDAServiceIds = strMDAServiceIds.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                    List<string> strArrMDAServiceIds = new List<string>();

                    if (mdsIds.Contains("{"))
                    {
                        var serviceBillIds = JsonConvert.DeserializeObject<List<ServiceBillIdsRequest>>(mdsIds);

                        foreach (var item in serviceBillIds)
                        {
                            strArrMDAServiceIds.Add(item.ServiceId);
                        }
                    }
                    else
                    {
                        strArrMDAServiceIds = mdsIds.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    }

                    foreach (var strMDAServiceId in strArrMDAServiceIds)
                    {
                        if (!string.IsNullOrWhiteSpace(strMDAServiceId))
                        {
                            usp_GetMDAServiceList_Result mObjMDAServiceData = mObjBLMDAService.BL_GetMDAServiceDetails(new MDA_Services() { MDAServiceID = TrynParse.parseInt(strMDAServiceId), IntStatus = 2 });

                            if (mObjMDAServiceData != null)
                            {
                                ServiceBill_MDAService mObjMDAService = new ServiceBill_MDAService()
                                {
                                    RowID = lstMDAServices.Count + 1,
                                    MDAServiceID = mObjMDAServiceData.MDAServiceID.GetValueOrDefault(),
                                    MDAServiceName = mObjMDAServiceData.MDAServiceCode + " - " + mObjMDAServiceData.MDAServiceName,
                                    intTrack = EnumList.Track.INSERT,
                                    TaxYear = mObjMDAServiceData.TaxYear.GetValueOrDefault(),
                                    ServiceAmount = mObjMDAServiceData.ServiceAmount.GetValueOrDefault()
                                };

                                lstMDAServices.Add(mObjMDAService);

                                string[] strArrMDAServiceItemIds = mObjMDAServiceData.MDAServiceItemIds.Split(',');

                                foreach (string strMDAServiceItemID in strArrMDAServiceItemIds)
                                {
                                    if (TrynParse.parseInt(strMDAServiceItemID) > 0)
                                    {
                                        usp_GetMDAServiceItemList_Result mObjMDAServiceItemData = mObjBLMDAServiceItem.BL_GetMDAServiceItemDetails(new MDA_Service_Items() { intStatus = 2, MDAServiceItemID = TrynParse.parseInt(strMDAServiceItemID) });

                                        ServiceBill_MDAServiceItem mObjServiceBillItem = new ServiceBill_MDAServiceItem()
                                        {
                                            RowID = lstMDAServiceItem.Count + 1,
                                            ComputationID = mObjMDAServiceItemData.ComputationID.GetValueOrDefault(),
                                            ComputationName = mObjMDAServiceItemData.ComputationName,
                                            MDAServiceItemID = mObjMDAServiceItemData.MDAServiceItemID.GetValueOrDefault(),
                                            MDAServiceItemName = mObjMDAServiceItemData.MDAServiceItemName,
                                            MDAServiceItemReferenceNo = mObjMDAServiceItemData.MDAServiceItemReferenceNo,
                                            MDAService_RowID = mObjMDAService.RowID,
                                            Percentage = mObjMDAServiceItemData.Percentage.GetValueOrDefault(),
                                            ToSettleAmount = mObjMDAServiceItemData.ServiceBaseAmount.GetValueOrDefault(),
                                            ServiceAmount = mObjMDAServiceItemData.ServiceAmount.GetValueOrDefault(),
                                            intTrack = EnumList.Track.INSERT
                                        };

                                        lstMDAServiceItem.Add(mObjServiceBillItem);
                                    }
                                }
                            }
                        }
                    }

                    SessionManager.lstMDAService = lstMDAServices;
                    SessionManager.lstMDAServiceItem = lstMDAServiceItem;


                    ViewBag.MDAServiceList = SessionManager.lstMDAService.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();

                    return View(mObjServiceBillModel);
                }
                else
                {
                    return RedirectToAction("Search", "CaptureCorporate");
                }
            }
            else
            {
                return RedirectToAction("Search", "CaptureCorporate");
            }
        }

        [HttpPost]

        [ValidateAntiForgeryToken()]
        public ActionResult AddServiceBill(ServiceBillViewModel pObjServiceBillModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.MDAServiceList = SessionManager.lstMDAService.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
                return View(pObjServiceBillModel);
            }
            else
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    IList<ServiceBill_MDAServiceItem> lstMDAServiceItems = SessionManager.lstMDAServiceItem ?? new List<ServiceBill_MDAServiceItem>();
                    IList<ServiceBill_MDAService> lstMDAServices = SessionManager.lstMDAService ?? new List<ServiceBill_MDAService>();

                    int IntServiceBillServiceCount = lstMDAServices.Where(t => t.intTrack != EnumList.Track.DELETE).Count();

                    if (IntServiceBillServiceCount == 0)
                    {
                        ViewBag.Message = "Please Add Atleast One MDA Service";
                        ViewBag.MDAServiceList = SessionManager.lstMDAService.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
                        return View(pObjServiceBillModel);
                    }
                    else
                    {
                        BLServiceBill mObjBLServiceBill = new BLServiceBill();

                        ServiceBill mObjServiceBill = new ServiceBill()
                        {
                            ServiceBillID = 0,
                            TaxPayerID = pObjServiceBillModel.TaxPayerID,
                            TaxPayerTypeID = pObjServiceBillModel.TaxPayerTypeID,
                            ServiceBillDate = CommUtil.GetCurrentDateTime(),
                            Notes = pObjServiceBillModel.Notes,
                            SettlementDueDate = pObjServiceBillModel.SettlementDuedate,
                            SettlementStatusID = (int)EnumList.SettlementStatus.Assessed,
                            ServiceBillAmount = lstMDAServices.Count > 0 ? lstMDAServices.Sum(t => t.ServiceAmount) : 0,

                            Active = true,
                            CreatedBy = SessionManager.UserID,
                            CreatedDate = CommUtil.GetCurrentDateTime()
                        };

                        try
                        {
                            FuncResponse<ServiceBill> mObjServiceBillResponse = mObjBLServiceBill.BL_InsertUpdateServiceBill(mObjServiceBill);

                            if (mObjServiceBillResponse.Success)
                            {
                                //Adding MDA Service
                                foreach (ServiceBill_MDAService mObjSBS in lstMDAServices.Where(t => t.intTrack != EnumList.Track.DELETE))
                                {
                                    MAP_ServiceBill_MDAService mObjMDAService = new MAP_ServiceBill_MDAService()
                                    {
                                        ServiceBillID = mObjServiceBillResponse.AdditionalData.ServiceBillID,
                                        MDAServiceID = mObjSBS.MDAServiceID,
                                        ServiceAmount = mObjSBS.ServiceAmount,
                                        ServiceBillYear = mObjSBS.TaxYear,
                                        CreatedBy = SessionManager.UserID,
                                        CreatedDate = CommUtil.GetCurrentDateTime()
                                    };

                                    FuncResponse<MAP_ServiceBill_MDAService> mObjSBSResponse = mObjBLServiceBill.BL_InsertUpdateMDAService(mObjMDAService);

                                    if (mObjSBSResponse.Success)
                                    {
                                        IList<MAP_ServiceBill_MDAServiceItem> lstInsertServiceBillDetail = new List<MAP_ServiceBill_MDAServiceItem>();

                                        foreach (ServiceBill_MDAServiceItem mObjServiceBillItemDetail in lstMDAServiceItems.Where(t => t.MDAService_RowID == mObjSBS.RowID))
                                        {
                                            MAP_ServiceBill_MDAServiceItem mObjSIDetail = new MAP_ServiceBill_MDAServiceItem()
                                            {
                                                SBSID = mObjSBSResponse.AdditionalData.SBSID,
                                                MDAServiceItemID = mObjServiceBillItemDetail.MDAServiceItemID,
                                                ServiceBaseAmount = mObjServiceBillItemDetail.ServiceBaseAmount,
                                                ServiceAmount = mObjServiceBillItemDetail.ServiceAmount,
                                                Percentage = mObjServiceBillItemDetail.Percentage,
                                                PaymentStatusID = (int)EnumList.PaymentStatus.Due,
                                                CreatedBy = SessionManager.UserID,
                                                CreatedDate = CommUtil.GetCurrentDateTime(),
                                            };

                                            lstInsertServiceBillDetail.Add(mObjSIDetail);
                                        }

                                        FuncResponse mObjSDResponse = mObjBLServiceBill.BL_InsertServiceBillItem(lstInsertServiceBillDetail);

                                        if (!mObjSDResponse.Success)
                                        {
                                            throw (mObjSDResponse.Exception);
                                        }
                                    }
                                    else
                                    {
                                        throw (mObjSBSResponse.Exception);
                                    }
                                }

                                //Send Notification
                                if (GlobalDefaultValues.SendNotification)
                                {
                                    usp_GetServiceBillList_Result mObjServiceBillData = mObjBLServiceBill.BL_GetServiceBillDetails(new ServiceBill() { ServiceBillID = mObjServiceBillResponse.AdditionalData.ServiceBillID, IntStatus = 2 });
                                    usp_GetCompanyList_Result mObjIndividualData = new BLCompany().BL_GetCompanyDetails(new Company() { CompanyID = pObjServiceBillModel.TaxPayerID, intStatus = 1 });

                                    IList<usp_GetServiceBill_MDAServiceList_Result> lstMAPServiceBillServices = mObjBLServiceBill.BL_GetMDAServiceList(mObjServiceBillData.ServiceBillID.GetValueOrDefault());

                                    string MDAServicesNames = string.Join(",", lstMAPServiceBillServices.Select(t => t.MDAServiceName).ToArray());
                                    if (mObjIndividualData != null && mObjServiceBillData != null)
                                    {
                                        EmailDetails mObjEmailDetails = new EmailDetails()
                                        {
                                            TaxPayerTypeID = mObjIndividualData.TaxPayerTypeID.GetValueOrDefault(),
                                            TaxPayerTypeName = mObjIndividualData.TaxPayerTypeName,
                                            TaxPayerID = mObjIndividualData.CompanyID.GetValueOrDefault(),
                                            TaxPayerName = mObjIndividualData.CompanyName,
                                            TaxPayerRIN = mObjIndividualData.CompanyRIN,
                                            TaxPayerMobileNumber = mObjIndividualData.MobileNumber1,
                                            TaxPayerEmail = mObjIndividualData.EmailAddress1,
                                            BillRefNo = mObjServiceBillData.ServiceBillRefNo,
                                            BillAmount = CommUtil.GetFormatedCurrency(mObjServiceBillData.ServiceBillAmount),
                                            BillTypeName = "Service Bill",
                                            LoggedInUserID = SessionManager.UserID,
                                            RuleNames = MDAServicesNames
                                        };

                                        if (!string.IsNullOrWhiteSpace(mObjIndividualData.EmailAddress1))
                                        {
                                            BLEmailHandler.BL_BillGenerated(mObjEmailDetails);
                                        }

                                        if (!string.IsNullOrWhiteSpace(mObjIndividualData.MobileNumber1))
                                        {
                                            UtilityController.BL_BillGenerated(mObjEmailDetails);
                                        }
                                    }
                                }

                                scope.Complete();
                                FlashMessage.Info(mObjServiceBillResponse.Message);
                                return RedirectToAction("Details", "CaptureCorporate", new { id = pObjServiceBillModel.TaxPayerID, name = pObjServiceBillModel.TaxPayerRIN });
                            }
                            else
                            {
                                ViewBag.MDAServiceList = SessionManager.lstMDAService.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();

                                ViewBag.Message = mObjServiceBillResponse.Message;
                                Transaction.Current.Rollback();
                                return View(pObjServiceBillModel);
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.SendErrorToText(ex);
                            ErrorSignal.FromCurrentContext().Raise(ex);
                            ViewBag.MDAServiceList = SessionManager.lstMDAService.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();

                            ViewBag.Message = "Error occurred while saving service bill";
                            Transaction.Current.Rollback();
                            return View(pObjServiceBillModel);
                        }
                    }
                }
            }
        }


        //public ActionResult AddServiceBill(int? id, string name, string mdsIds = null)
        //{
        //    if (id.GetValueOrDefault() > 0)
        //    {
        //        Company mObjCompany = new Company()
        //        {
        //            CompanyID = id.GetValueOrDefault(),
        //            intStatus = 1
        //        };

        //        usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(mObjCompany);

        //        if (mObjCompanyData != null)
        //        {

        //            ServiceBillViewModel mObjServiceBillModel = new ServiceBillViewModel()
        //            {
        //                TaxPayerID = mObjCompanyData.CompanyID.GetValueOrDefault(),
        //                TaxPayerRIN = mObjCompanyData.CompanyRIN,
        //                TaxPayerName = mObjCompanyData.CompanyName,
        //                TaxPayerAddress = mObjCompanyData.ContactAddress,
        //                SettlementDuedate = CommUtil.GetCurrentDateTime(),
        //                TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies,
        //            };

        //            IList<ServiceBill_MDAService> lstMDAServices = new List<ServiceBill_MDAService>();
        //            IList<ServiceBill_MDAServiceItem> lstMDAServiceItem = new List<ServiceBill_MDAServiceItem>();

        //            BLMDAService mObjBLMDAService = new BLMDAService();
        //            BLMDAServiceItem mObjBLMDAServiceItem = new BLMDAServiceItem();

        //            string strMDAServiceIds = EncryptDecrypt.Decrypt(mdsIds);

        //            string[] strArrMDAServiceIds = strMDAServiceIds.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

        //            foreach (string strMDAServiceId in strArrMDAServiceIds)
        //            {
        //                if (!string.IsNullOrWhiteSpace(strMDAServiceId))
        //                {

        //                    usp_GetMDAServiceList_Result mObjMDAServiceData = mObjBLMDAService.BL_GetMDAServiceDetails(new MDA_Services() { MDAServiceID = TrynParse.parseInt(strMDAServiceId), IntStatus = 2 });

        //                    if (mObjMDAServiceData != null)
        //                    {

        //                        ServiceBill_MDAService mObjMDAService = new ServiceBill_MDAService()
        //                        {
        //                            RowID = lstMDAServices.Count + 1,
        //                            MDAServiceID = mObjMDAServiceData.MDAServiceID.GetValueOrDefault(),
        //                            MDAServiceName = mObjMDAServiceData.MDAServiceCode + " - " + mObjMDAServiceData.MDAServiceName,
        //                            intTrack = EnumList.Track.INSERT,
        //                            TaxYear = mObjMDAServiceData.TaxYear.GetValueOrDefault(),
        //                        };

        //                        lstMDAServices.Add(mObjMDAService);

        //                        string[] strArrMDAServiceItemIds = mObjMDAServiceData.MDAServiceItemIds.Split(',');

        //                        foreach (string strMDAServiceItemID in strArrMDAServiceItemIds)
        //                        {
        //                            if (TrynParse.parseInt(strMDAServiceItemID) > 0)
        //                            {
        //                                usp_GetMDAServiceItemList_Result mObjMDAServiceItemData = mObjBLMDAServiceItem.BL_GetMDAServiceItemDetails(new MDA_Service_Items() { intStatus = 2, MDAServiceItemID = TrynParse.parseInt(strMDAServiceItemID) });

        //                                ServiceBill_MDAServiceItem mObjServiceBillItem = new ServiceBill_MDAServiceItem()
        //                                {
        //                                    RowID = lstMDAServiceItem.Count + 1,
        //                                    ComputationID = mObjMDAServiceItemData.ComputationID.GetValueOrDefault(),
        //                                    ComputationName = mObjMDAServiceItemData.ComputationName,
        //                                    MDAServiceItemID = mObjMDAServiceItemData.MDAServiceItemID.GetValueOrDefault(),
        //                                    MDAServiceItemName = mObjMDAServiceItemData.MDAServiceItemName,
        //                                    MDAServiceItemReferenceNo = mObjMDAServiceItemData.MDAServiceItemReferenceNo,
        //                                    MDAService_RowID = mObjMDAService.RowID,
        //                                    Percentage = mObjMDAServiceItemData.Percentage.GetValueOrDefault(),
        //                                    intTrack = EnumList.Track.INSERT
        //                                };

        //                                lstMDAServiceItem.Add(mObjServiceBillItem);
        //                            }
        //                        }
        //                    }
        //                }
        //            }

        //            SessionManager.lstMDAService = lstMDAServices;
        //            SessionManager.lstMDAServiceItem = lstMDAServiceItem;


        //            ViewBag.MDAServiceList = SessionManager.lstMDAService.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();

        //            return View(mObjServiceBillModel);
        //        }
        //        else
        //        {
        //            return RedirectToAction("Search", "CaptureCorporate");
        //        }
        //    }
        //    else
        //    {
        //        return RedirectToAction("Search", "CaptureCorporate");
        //    }
        //}

        //[HttpPost]

        //[ValidateAntiForgeryToken()]
        //public ActionResult AddServiceBill(ServiceBillViewModel pObjServiceBillModel)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        ViewBag.MDAServiceList = SessionManager.lstMDAService.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
        //        return View(pObjServiceBillModel);
        //    }
        //    else
        //    {
        //        using (TransactionScope scope = new TransactionScope())
        //        {
        //            IList<ServiceBill_MDAServiceItem> lstMDAServiceItems = SessionManager.lstMDAServiceItem ?? new List<ServiceBill_MDAServiceItem>();
        //            IList<ServiceBill_MDAService> lstMDAServices = SessionManager.lstMDAService ?? new List<ServiceBill_MDAService>();

        //            int IntServiceBillServiceCount = lstMDAServices.Where(t => t.intTrack != EnumList.Track.DELETE).Count();

        //            if (IntServiceBillServiceCount == 0)
        //            {
        //                ViewBag.Message = "Please Add Atleast One MDA Service";
        //                ViewBag.MDAServiceList = SessionManager.lstMDAService.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
        //                return View(pObjServiceBillModel);
        //            }
        //            else
        //            {

        //                BLServiceBill mObjBLServiceBill = new BLServiceBill();

        //                ServiceBill mObjServiceBill = new ServiceBill()
        //                {
        //                    ServiceBillID = 0,
        //                    TaxPayerID = pObjServiceBillModel.TaxPayerID,
        //                    TaxPayerTypeID = pObjServiceBillModel.TaxPayerTypeID,
        //                    ServiceBillAmount = lstMDAServiceItems.Count > 0 ? lstMDAServiceItems.Where(t => t.intTrack != EnumList.Track.DELETE).Sum(t => t.ServiceAmount) : 0,
        //                    ServiceBillDate = CommUtil.GetCurrentDateTime(),
        //                    Notes = pObjServiceBillModel.Notes,
        //                    SettlementDueDate = pObjServiceBillModel.SettlementDuedate,
        //                    SettlementStatusID = (int)EnumList.SettlementStatus.Assessed,
        //                    Active = true,
        //                    CreatedBy = SessionManager.UserID,
        //                    CreatedDate = CommUtil.GetCurrentDateTime()
        //                };

        //                try
        //                {

        //                    FuncResponse<ServiceBill> mObjServiceBillResponse = mObjBLServiceBill.BL_InsertUpdateServiceBill(mObjServiceBill);

        //                    if (mObjServiceBillResponse.Success)
        //                    {
        //                        //Adding MDA Service

        //                        foreach (ServiceBill_MDAService mObjSBS in lstMDAServices.Where(t => t.intTrack != EnumList.Track.DELETE))
        //                        {
        //                            MAP_ServiceBill_MDAService mObjMDAService = new MAP_ServiceBill_MDAService()
        //                            {
        //                                ServiceBillID = mObjServiceBillResponse.AdditionalData.ServiceBillID,
        //                                MDAServiceID = mObjSBS.MDAServiceID,
        //                                ServiceAmount = mObjSBS.ServiceAmount,
        //                                ServiceBillYear = mObjSBS.TaxYear,
        //                                CreatedBy = SessionManager.UserID,
        //                                CreatedDate = CommUtil.GetCurrentDateTime()
        //                            };

        //                            FuncResponse<MAP_ServiceBill_MDAService> mObjSBSResponse = mObjBLServiceBill.BL_InsertUpdateMDAService(mObjMDAService);

        //                            if (mObjSBSResponse.Success)
        //                            {
        //                                IList<MAP_ServiceBill_MDAServiceItem> lstInsertServiceBillDetail = new List<MAP_ServiceBill_MDAServiceItem>();

        //                                foreach (ServiceBill_MDAServiceItem mObjServiceBillItemDetail in lstMDAServiceItems.Where(t => t.MDAService_RowID == mObjSBS.RowID))
        //                                {
        //                                    MAP_ServiceBill_MDAServiceItem mObjSIDetail = new MAP_ServiceBill_MDAServiceItem()
        //                                    {
        //                                        SBSID = mObjSBSResponse.AdditionalData.SBSID,
        //                                        MDAServiceItemID = mObjServiceBillItemDetail.MDAServiceItemID,
        //                                        ServiceBaseAmount = mObjServiceBillItemDetail.ServiceBaseAmount,
        //                                        ServiceAmount = mObjServiceBillItemDetail.ServiceAmount,
        //                                        Percentage = mObjServiceBillItemDetail.Percentage,
        //                                        PaymentStatusID = (int)EnumList.PaymentStatus.Due,
        //                                        CreatedBy = SessionManager.UserID,
        //                                        CreatedDate = CommUtil.GetCurrentDateTime(),
        //                                    };

        //                                    lstInsertServiceBillDetail.Add(mObjSIDetail);
        //                                }

        //                                FuncResponse mObjSDResponse = mObjBLServiceBill.BL_InsertServiceBillItem(lstInsertServiceBillDetail);

        //                                if (!mObjSDResponse.Success)
        //                                {
        //                                    throw (mObjSDResponse.Exception);
        //                                }
        //                            }
        //                            else
        //                            {
        //                                throw (mObjSBSResponse.Exception);
        //                            }
        //                        }

        //                        //Send Notification
        //                        if (GlobalDefaultValues.SendNotification)
        //                        {
        //                            usp_GetServiceBillList_Result mObjServiceBillData = mObjBLServiceBill.BL_GetServiceBillDetails(new ServiceBill() { ServiceBillID = mObjServiceBillResponse.AdditionalData.ServiceBillID, IntStatus = 2 });
        //                            usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(new Company() { CompanyID = pObjServiceBillModel.TaxPayerID, intStatus = 1 });
        //                            IList<usp_GetServiceBill_MDAServiceList_Result> lstMAPServiceBillServices = mObjBLServiceBill.BL_GetMDAServiceList(mObjServiceBillData.ServiceBillID.GetValueOrDefault());

        //                            string MDAServicesNames = string.Join(",", lstMAPServiceBillServices.Select(t => t.MDAServiceName).ToArray());

        //                            if (mObjCompanyData != null && mObjServiceBillData != null)
        //                            {
        //                                EmailDetails mObjEmailDetails = new EmailDetails()
        //                                {
        //                                    TaxPayerTypeID = mObjCompanyData.TaxPayerTypeID.GetValueOrDefault(),
        //                                    TaxPayerTypeName = mObjCompanyData.TaxPayerTypeName,
        //                                    TaxPayerID = mObjCompanyData.CompanyID.GetValueOrDefault(),
        //                                    TaxPayerName = mObjCompanyData.CompanyName,
        //                                    TaxPayerRIN = mObjCompanyData.CompanyRIN,
        //                                    TaxPayerMobileNumber = mObjCompanyData.MobileNumber1,
        //                                    TaxPayerEmail = mObjCompanyData.EmailAddress1,
        //                                    BillRefNo = mObjServiceBillData.ServiceBillRefNo,
        //                                    BillAmount = CommUtil.GetFormatedCurrency(mObjServiceBillData.ServiceBillAmount),
        //                                    BillTypeName = "Service Bill",
        //                                    LoggedInUserID = SessionManager.UserID,
        //                                    RuleNames = MDAServicesNames
        //                                };

        //                                if (!string.IsNullOrWhiteSpace(mObjCompanyData.EmailAddress1))
        //                                {
        //                                    BLEmailHandler.BL_BillGenerated(mObjEmailDetails);
        //                                }

        //                                if (!string.IsNullOrWhiteSpace(mObjCompanyData.MobileNumber1))
        //                                {
        //                                    UtilityController.BL_BillGenerated(mObjEmailDetails);
        //                                }
        //                            }
        //                        }

        //                        scope.Complete();
        //                        FlashMessage.Info(mObjServiceBillResponse.Message);
        //                        return RedirectToAction("Details", "CaptureCorporate", new { id = pObjServiceBillModel.TaxPayerID, name = pObjServiceBillModel.TaxPayerRIN });
        //                    }
        //                    else
        //                    {
        //                        ViewBag.MDAServiceList = SessionManager.lstMDAService.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();

        //                        ViewBag.Message = mObjServiceBillResponse.Message;
        //                        Transaction.Current.Rollback();
        //                        return View(pObjServiceBillModel);
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    ErrorSignal.FromCurrentContext().Raise(ex);
        //                    ViewBag.MDAServiceList = SessionManager.lstMDAService.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();

        //                    ViewBag.Message = "Error occurred while saving service bill";
        //                    Transaction.Current.Rollback();
        //                    return View(pObjServiceBillModel);
        //                }
        //            }
        //        }
        //    }
        //}


        public ActionResult EditServiceBill(int? id, string name, int? sbid)
        {
            string url = getUrl();
            bool itCan = new UtilityController().CheckAccess(url);
            if (!itCan) { return RedirectToAction("AccessDenied", "Utility"); }
            if (id.GetValueOrDefault() > 0 && sbid.GetValueOrDefault() > 0)
            {
                Company mObjCompany = new Company()
                {
                    CompanyID = id.GetValueOrDefault(),
                    intStatus = 1
                };

                usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(mObjCompany);

                BLMDAService mObjBLMDAService = new BLMDAService();
                BLMDAServiceItem mObjBLMDAServiceItem = new BLMDAServiceItem();
                BLServiceBill mObjBLServiceBill = new BLServiceBill();

                usp_GetServiceBillList_Result mObjServiceBillData = mObjBLServiceBill.BL_GetServiceBillDetails(new ServiceBill() { ServiceBillID = sbid.GetValueOrDefault(), IntStatus = 2 });

                if (mObjCompanyData != null && mObjServiceBillData != null)
                {
                    ServiceBillViewModel mObjServiceBillModel = new ServiceBillViewModel()
                    {
                        TaxPayerID = mObjCompanyData.CompanyID.GetValueOrDefault(),
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies,
                        TaxPayerRIN = mObjCompanyData.CompanyRIN,
                        TaxPayerName = mObjCompanyData.CompanyName,
                        TaxPayerAddress = mObjCompanyData.ContactAddress,
                        Notes = mObjServiceBillData.Notes,
                        ServiceBillID = sbid.GetValueOrDefault(),
                        SettlementDuedate = mObjServiceBillData.SettlementDueDate.GetValueOrDefault(),
                        ServiceBillDate = mObjServiceBillData.ServiceBillDate.GetValueOrDefault(),
                    };

                    IList<ServiceBill_MDAService> lstMDAServices = new List<ServiceBill_MDAService>();
                    IList<ServiceBill_MDAServiceItem> lstMDAServiceItem = new List<ServiceBill_MDAServiceItem>();

                    IList<MAP_ServiceBill_MDAService> lstMAPServiceBillServices = mObjBLServiceBill.BL_GetMDAServices(mObjServiceBillData.ServiceBillID.GetValueOrDefault());
                    IList<MAP_ServiceBill_MDAServiceItem> lstMAPServiceBillItems;

                    foreach (var item in lstMAPServiceBillServices)
                    {
                        ServiceBill_MDAService ServiceBill_MDAService = new ServiceBill_MDAService()
                        {
                            RowID = lstMDAServices.Count + 1,
                            TablePKID = item.SBSID,
                            MDAServiceID = item.MDAServiceID.GetValueOrDefault(),
                            ServiceAmount = item.ServiceAmount.GetValueOrDefault(),
                            MDAServiceName = item.MDA_Services.MDAServiceCode + " - " + item.MDA_Services.MDAServiceName,
                            TaxYear = item.MDA_Services.TaxYear.GetValueOrDefault(),
                            intTrack = EnumList.Track.EXISTING,
                        };

                        lstMDAServices.Add(ServiceBill_MDAService);

                        lstMAPServiceBillItems = mObjBLServiceBill.BL_GetServiceBillItems(item.SBSID);

                        foreach (var subitem in lstMAPServiceBillItems)
                        {
                            usp_GetMDAServiceItemList_Result mObjMDAServiceItemData = mObjBLMDAServiceItem.BL_GetMDAServiceItemDetails(new MDA_Service_Items() { intStatus = 2, MDAServiceItemID = subitem.MDAServiceItemID.GetValueOrDefault() });

                            ServiceBill_MDAServiceItem mObjServiceBillItem = new ServiceBill_MDAServiceItem()
                            {
                                RowID = lstMDAServiceItem.Count + 1,
                                MDAService_RowID = ServiceBill_MDAService.RowID,
                                TablePKID = subitem.SBSIID,
                                MDAServiceItemID = subitem.MDAServiceItemID.GetValueOrDefault(),
                                MDAServiceItemName = mObjMDAServiceItemData.MDAServiceItemName,
                                MDAServiceItemReferenceNo = mObjMDAServiceItemData.MDAServiceItemReferenceNo,
                                ServiceAmount = subitem.ServiceAmount.GetValueOrDefault(),
                                ServiceBaseAmount = subitem.ServiceBaseAmount.GetValueOrDefault(),
                                ComputationID = mObjMDAServiceItemData.ComputationID.GetValueOrDefault(),
                                ComputationName = mObjMDAServiceItemData.ComputationName,
                                Percentage = subitem.Percentage.GetValueOrDefault(),
                                intTrack = EnumList.Track.EXISTING
                            };

                            lstMDAServiceItem.Add(mObjServiceBillItem);
                        }
                    }

                    SessionManager.lstMDAService = lstMDAServices;
                    SessionManager.lstMDAServiceItem = lstMDAServiceItem;


                    ViewBag.MDAServiceList = SessionManager.lstMDAService.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();

                    return View(mObjServiceBillModel);
                }
                else
                {
                    return RedirectToAction("Search", "CaptureCorporate");
                }
            }
            else
            {
                return RedirectToAction("Search", "CaptureCorporate");
            }
        }

        [HttpPost]

        [ValidateAntiForgeryToken()]
        public ActionResult EditServiceBill(ServiceBillViewModel pObjServiceBillModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.MDAServiceList = SessionManager.lstMDAService.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
                return View(pObjServiceBillModel);
            }
            else
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    IList<ServiceBill_MDAServiceItem> lstMDAServiceItems = SessionManager.lstMDAServiceItem ?? new List<ServiceBill_MDAServiceItem>();
                    IList<ServiceBill_MDAService> lstMDAServices = SessionManager.lstMDAService ?? new List<ServiceBill_MDAService>();

                    int IntServiceBillServiceCount = lstMDAServices.Where(t => t.intTrack != EnumList.Track.DELETE).Count();

                    if (IntServiceBillServiceCount == 0)
                    {
                        ViewBag.Message = "Please Add Atleast One MDA Service";
                        ViewBag.MDAServiceList = SessionManager.lstMDAService.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
                        return View(pObjServiceBillModel);
                    }
                    else
                    {

                        BLServiceBill mObjBLServiceBill = new BLServiceBill();

                        ServiceBill mObjServiceBill = new ServiceBill()
                        {
                            ServiceBillID = pObjServiceBillModel.ServiceBillID,
                            ServiceBillAmount = lstMDAServiceItems.Count > 0 ? lstMDAServiceItems.Where(t => t.intTrack != EnumList.Track.DELETE).Sum(t => t.ServiceAmount) : 0,
                            Notes = pObjServiceBillModel.Notes,
                            SettlementDueDate = pObjServiceBillModel.SettlementDuedate,
                            ModifiedBy = SessionManager.UserID,
                            ModifiedDate = CommUtil.GetCurrentDateTime()
                        };

                        try
                        {

                            FuncResponse<ServiceBill> mObjServiceBillResponse = mObjBLServiceBill.BL_InsertUpdateServiceBill(mObjServiceBill);

                            if (mObjServiceBillResponse.Success)
                            {
                                foreach (ServiceBill_MDAService mObjSBS in lstMDAServices)
                                {
                                    if (mObjSBS.intTrack == EnumList.Track.INSERT)
                                    {
                                        MAP_ServiceBill_MDAService mObjMDAService = new MAP_ServiceBill_MDAService()
                                        {
                                            ServiceBillID = mObjServiceBillResponse.AdditionalData.ServiceBillID,
                                            MDAServiceID = mObjSBS.MDAServiceID,
                                            ServiceAmount = mObjSBS.ServiceAmount,
                                            ServiceBillYear = mObjSBS.TaxYear,
                                            CreatedBy = SessionManager.UserID,
                                            CreatedDate = CommUtil.GetCurrentDateTime()
                                        };

                                        FuncResponse<MAP_ServiceBill_MDAService> mObjSBSResponse = mObjBLServiceBill.BL_InsertUpdateMDAService(mObjMDAService);

                                        if (mObjSBSResponse.Success)
                                        {
                                            IList<MAP_ServiceBill_MDAServiceItem> lstInsertServiceBillDetail = new List<MAP_ServiceBill_MDAServiceItem>();

                                            foreach (ServiceBill_MDAServiceItem mObjServiceBillItemDetail in lstMDAServiceItems.Where(t => t.MDAService_RowID == mObjSBS.RowID))
                                            {
                                                MAP_ServiceBill_MDAServiceItem mObjSIDetail = new MAP_ServiceBill_MDAServiceItem()
                                                {
                                                    SBSID = mObjSBSResponse.AdditionalData.SBSID,
                                                    MDAServiceItemID = mObjServiceBillItemDetail.MDAServiceItemID,
                                                    ServiceBaseAmount = mObjServiceBillItemDetail.ServiceBaseAmount,
                                                    ServiceAmount = mObjServiceBillItemDetail.ServiceAmount,
                                                    Percentage = mObjServiceBillItemDetail.Percentage,
                                                    PaymentStatusID = (int)EnumList.PaymentStatus.Due,
                                                    CreatedBy = SessionManager.UserID,
                                                    CreatedDate = CommUtil.GetCurrentDateTime(),
                                                };

                                                lstInsertServiceBillDetail.Add(mObjSIDetail);
                                            }

                                            FuncResponse mObjSDResponse = mObjBLServiceBill.BL_InsertServiceBillItem(lstInsertServiceBillDetail);

                                            if (!mObjSDResponse.Success)
                                            {
                                                throw (mObjSDResponse.Exception);
                                            }
                                        }
                                        else
                                        {
                                            throw (mObjSBSResponse.Exception);
                                        }
                                    }
                                    else if (mObjSBS.intTrack == EnumList.Track.DELETE)
                                    {
                                        FuncResponse mObjMSResponse = mObjBLServiceBill.BL_DeleteMDAService(mObjSBS.TablePKID);

                                        if (!mObjMSResponse.Success)
                                        {
                                            throw (mObjMSResponse.Exception);
                                        }
                                    }
                                    else if (mObjSBS.intTrack == EnumList.Track.UPDATE)
                                    {
                                        MAP_ServiceBill_MDAService mObjMDAService = new MAP_ServiceBill_MDAService()
                                        {

                                            SBSID = mObjSBS.TablePKID,
                                            ServiceBillID = mObjServiceBillResponse.AdditionalData.ServiceBillID,
                                            MDAServiceID = mObjSBS.MDAServiceID,
                                            ServiceAmount = mObjSBS.ServiceAmount,
                                            ServiceBillYear = mObjSBS.TaxYear,
                                            ModifiedBy = SessionManager.UserID,
                                            ModifiedDate = CommUtil.GetCurrentDateTime()
                                        };



                                        FuncResponse<MAP_ServiceBill_MDAService> mObjSBSResponse = mObjBLServiceBill.BL_InsertUpdateMDAService(mObjMDAService);

                                        if (mObjSBSResponse.Success)
                                        {
                                            IList<MAP_ServiceBill_MDAServiceItem> lstInsertServiceBillDetail = new List<MAP_ServiceBill_MDAServiceItem>();

                                            foreach (ServiceBill_MDAServiceItem mObjServiceBillItemDetail in lstMDAServiceItems.Where(t => t.MDAService_RowID == mObjSBS.RowID))
                                            {
                                                MAP_ServiceBill_MDAServiceItem mObjSIDetail = new MAP_ServiceBill_MDAServiceItem()
                                                {
                                                    SBSIID = mObjServiceBillItemDetail.TablePKID,
                                                    SBSID = mObjSBSResponse.AdditionalData.SBSID,
                                                    MDAServiceItemID = mObjServiceBillItemDetail.MDAServiceItemID,
                                                    ServiceBaseAmount = mObjServiceBillItemDetail.ServiceBaseAmount,
                                                    ServiceAmount = mObjServiceBillItemDetail.ServiceAmount,
                                                    Percentage = mObjServiceBillItemDetail.Percentage,
                                                    PaymentStatusID = (int)EnumList.PaymentStatus.Due,
                                                    ModifiedBy = SessionManager.UserID,
                                                    ModifiedDate = CommUtil.GetCurrentDateTime(),
                                                };

                                                lstInsertServiceBillDetail.Add(mObjSIDetail);
                                            }

                                            FuncResponse mObjSDResponse = mObjBLServiceBill.BL_InsertServiceBillItem(lstInsertServiceBillDetail);

                                            if (!mObjSDResponse.Success)
                                            {
                                                throw (mObjSDResponse.Exception);
                                            }
                                        }
                                        else
                                        {
                                            throw (mObjSBSResponse.Exception);
                                        }
                                    }
                                }

                                if (GlobalDefaultValues.SendNotification)
                                {
                                    //Send Notification
                                    usp_GetServiceBillList_Result mObjServiceBillData = mObjBLServiceBill.BL_GetServiceBillDetails(new ServiceBill() { ServiceBillID = mObjServiceBillResponse.AdditionalData.ServiceBillID, IntStatus = 2 });
                                    usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(new Company() { CompanyID = pObjServiceBillModel.TaxPayerID, intStatus = 1 });
                                    IList<usp_GetServiceBill_MDAServiceList_Result> lstMAPServiceBillServices = mObjBLServiceBill.BL_GetMDAServiceList(mObjServiceBillData.ServiceBillID.GetValueOrDefault());
                                    string MDAServicesNames = string.Join(",", lstMAPServiceBillServices.Select(t => t.MDAServiceName).ToArray());

                                    if (mObjCompanyData != null && mObjServiceBillData != null)
                                    {
                                        EmailDetails mObjEmailDetails = new EmailDetails()
                                        {
                                            TaxPayerTypeID = mObjCompanyData.TaxPayerTypeID.GetValueOrDefault(),
                                            TaxPayerTypeName = mObjCompanyData.TaxPayerTypeName,
                                            TaxPayerID = mObjCompanyData.CompanyID.GetValueOrDefault(),
                                            TaxPayerName = mObjCompanyData.CompanyName,
                                            TaxPayerRIN = mObjCompanyData.CompanyRIN,
                                            TaxPayerMobileNumber = mObjCompanyData.MobileNumber1,
                                            TaxPayerEmail = mObjCompanyData.EmailAddress1,
                                            BillRefNo = mObjServiceBillData.ServiceBillRefNo,
                                            BillAmount = CommUtil.GetFormatedCurrency(mObjServiceBillData.ServiceBillAmount),
                                            BillTypeName = "Service Bill",
                                            LoggedInUserID = SessionManager.UserID,
                                            RuleNames = MDAServicesNames
                                        };

                                        if (!string.IsNullOrWhiteSpace(mObjCompanyData.EmailAddress1))
                                        {
                                            BLEmailHandler.BL_BillGenerated(mObjEmailDetails);
                                        }

                                        if (!string.IsNullOrWhiteSpace(mObjCompanyData.MobileNumber1))
                                        {
                                            UtilityController.BL_BillGenerated(mObjEmailDetails);
                                        }
                                    }
                                }

                                scope.Complete();
                                FlashMessage.Info(mObjServiceBillResponse.Message);
                                return RedirectToAction("Details", "CaptureCorporate", new { id = pObjServiceBillModel.TaxPayerID, name = pObjServiceBillModel.TaxPayerRIN });
                            }
                            else
                            {
                                ViewBag.MDAServiceList = SessionManager.lstMDAService.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();

                                ViewBag.Message = mObjServiceBillResponse.Message;
                                Transaction.Current.Rollback();
                                return View(pObjServiceBillModel);
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.SendErrorToText(ex);
                            ErrorSignal.FromCurrentContext().Raise(ex);
                            ViewBag.MDAServiceList = SessionManager.lstMDAService.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();

                            ViewBag.Message = "Error occurred while saving service bill";
                            Transaction.Current.Rollback();
                            return View(pObjServiceBillModel);
                        }
                    }
                }
            }
        }


        public ActionResult BillDetail(int? id, string name, int? billid, string billrefno)
        {
            //    string url = getUrl();
            //    bool itCan = new UtilityController().CheckAccess(url);
            //    if (!itCan) { return RedirectToAction("AccessDenied", "Utility"); }
            //    if (id.GetValueOrDefault() > 0 && billid.GetValueOrDefault() > 0)
            //    {
            //        Company mObjCompany = new Company()
            //        {
            //            CompanyID = id.GetValueOrDefault(),
            //            intStatus = 1
            //        };

            //        usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(mObjCompany);

            //        if (mObjCompanyData != null)
            //        {
            //            if (billrefno.StartsWith("AB"))
            //            {
            //                BLAssessment mObjBLAssessment = new BLAssessment();
            //                BLAssessmentItem mObjBLAssessmentItem = new BLAssessmentItem();
            //                usp_GetAssessmentList_Result mObjAssessmentData = mObjBLAssessment.BL_GetAssessmentDetails(new Assessment() { AssessmentID = billid.GetValueOrDefault(), IntStatus = 2 });

            //                if (mObjAssessmentData != null && mObjAssessmentData.TaxPayerID == mObjCompanyData.CompanyID && mObjAssessmentData.TaxPayerTypeID == (int)EnumList.TaxPayerType.Companies)
            //                {
            //                    IList<usp_GetAssessment_AssessmentRuleList_Result> lstMAPAssessmentRules = mObjBLAssessment.BL_GetAssessmentRules(mObjAssessmentData.AssessmentID.GetValueOrDefault());
            //                    IList<usp_GetAssessmentRuleItemList_Result> lstAssessmentItems = mObjBLAssessment.BL_GetAssessmentRuleItem(mObjAssessmentData.AssessmentID.GetValueOrDefault());
            //                    IList<usp_GetAssessmentRuleBasedSettlement_Result> lstAssessmentRuleSettlement = mObjBLAssessment.BL_GetAssessmentRuleBasedSettlement(mObjAssessmentData.AssessmentID.GetValueOrDefault());
            //                    IList<usp_GetAssessmentAdjustmentList_Result> lstAssessmentAdjustment = mObjBLAssessment.BL_GetAssessmentAdjustment(mObjAssessmentData.AssessmentID.GetValueOrDefault());
            //                    IList<usp_GetAssessmentLateChargeList_Result> lstAssessmentLateCharge = mObjBLAssessment.BL_GetAssessmentLateCharge(mObjAssessmentData.AssessmentID.GetValueOrDefault());
            //                    IList<DropDownListResult> lstSettlementMethod = mObjBLAssessment.BL_GetSettlementMethodAssessmentRuleBased(mObjAssessmentData.AssessmentID.GetValueOrDefault());

            //                    IList<usp_GetSettlementList_Result> lstSettlement = new BLSettlement().BL_GetSettlementList(new Settlement() { ServiceBillID = -1, AssessmentID = mObjAssessmentData.AssessmentID.GetValueOrDefault() });

            //                    ViewBag.SettlementMethodList = new SelectList(lstSettlementMethod, "id", "text");
            //                    ViewBag.MAPAssessmentRules = lstMAPAssessmentRules;
            //                    ViewBag.AssessmentItems = lstAssessmentItems;
            //                    ViewBag.AssessmentRuleSettlement = lstAssessmentRuleSettlement;
            //                    ViewBag.SettlementList = lstSettlement;
            //                    ViewBag.AdjustmentList = lstAssessmentAdjustment;
            //                    ViewBag.LateChargeList = lstAssessmentLateCharge;

            //                    return View("AssessmentBillDetail", mObjAssessmentData);
            //                }
            //                else
            //                {
            //                    return RedirectToAction("Search", "CaptureCorporate");
            //                }

            //            }
            //            else if (billrefno.StartsWith("SB"))
            //            {
            //                BLMDAService mObjBLMDAService = new BLMDAService();
            //                BLMDAServiceItem mObjBLMDAServiceItem = new BLMDAServiceItem();
            //                BLServiceBill mObjBLServiceBill = new BLServiceBill();

            //                usp_GetServiceBillList_Result mObjServiceBillData = mObjBLServiceBill.BL_GetServiceBillDetails(new ServiceBill() { ServiceBillID = billid.GetValueOrDefault(), IntStatus = 2 });

            //                if (mObjServiceBillData != null && mObjServiceBillData.TaxPayerID == mObjCompanyData.CompanyID && mObjServiceBillData.TaxPayerTypeID == (int)EnumList.TaxPayerType.Companies)
            //                {
            //                    IList<usp_GetServiceBill_MDAServiceList_Result> lstMAPServiceBillServices = mObjBLServiceBill.BL_GetMDAServiceList(mObjServiceBillData.ServiceBillID.GetValueOrDefault());
            //                    IList<usp_GetServiceBillItemList_Result> lstServiceBillItems = mObjBLServiceBill.BL_GetServiceBillItem(mObjServiceBillData.ServiceBillID.GetValueOrDefault());
            //                    IList<usp_GetMDAServiceBasedSettlement_Result> lstMDAServiceSettlement = mObjBLServiceBill.BL_GetMDAServiceBasedSettlement(mObjServiceBillData.ServiceBillID.GetValueOrDefault());
            //                    IList<usp_GetServiceBillAdjustmentList_Result> lstServiceBillAdjustment = mObjBLServiceBill.BL_GetServiceBillAdjustment(mObjServiceBillData.ServiceBillID.GetValueOrDefault());
            //                    IList<usp_GetServiceBillLateChargeList_Result> lstServiceBillLateCharge = mObjBLServiceBill.BL_GetServiceBillLateCharge(mObjServiceBillData.ServiceBillID.GetValueOrDefault());
            //                    IList<DropDownListResult> lstSettlementMethod = mObjBLServiceBill.BL_GetSettlementMethodMDAServiceBased(mObjServiceBillData.ServiceBillID.GetValueOrDefault());

            //                    IList<usp_GetSettlementList_Result> lstSettlement = new BLSettlement().BL_GetSettlementList(new Settlement() { AssessmentID = -1, ServiceBillID = mObjServiceBillData.ServiceBillID.GetValueOrDefault() });

            //                    ViewBag.SettlementMethodList = new SelectList(lstSettlementMethod, "id", "text");
            //                    ViewBag.MAPServiceBillRules = lstMAPServiceBillServices;
            //                    ViewBag.ServiceBillItems = lstServiceBillItems;
            //                    ViewBag.ServiceBillRuleSettlement = lstMDAServiceSettlement;
            //                    ViewBag.SettlementList = lstSettlement;
            //                    ViewBag.AdjustmentList = lstServiceBillAdjustment;
            //                    ViewBag.LateChargeList = lstServiceBillLateCharge;

            //                    return View("ServiceBillDetail", mObjServiceBillData);

            //                }
            //                else
            //                {
            //                    return RedirectToAction("Search", "CaptureCorporate");
            //                }
            //            }
            //            else
            //            {
            //                return RedirectToAction("Search", "CaptureCorporate");
            //            }
            //        }
            //        else
            //        {
            //            return RedirectToAction("Search", "CaptureCorporate");
            //        }
            //    }
            //    else
            //    {
            //        return RedirectToAction("Search", "CaptureCorporate");
            //    }
            //}
            string url = getUrl();
            bool itCan = new UtilityController().CheckAccess(url);
            if (!itCan) { return RedirectToAction("AccessDenied", "Utility"); }
            if (id.GetValueOrDefault() > 0 && billid.GetValueOrDefault() > 0)
            {

                Company mObjCompany = new Company()
                {
                    CompanyID = id.GetValueOrDefault(),
                    intStatus = 1
                };
                usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(mObjCompany);
                //  usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetCompanyDetails(mObjCompany);

                if (mObjCompanyData != null)
                {
                    if (billrefno.StartsWith("AB"))
                    {
                        BLAssessment mObjBLAssessment = new BLAssessment();
                        BLAssessmentItem mObjBLAssessmentItem = new BLAssessmentItem();
                        usp_GetAssessmentList_Result mObjAssessmentData = mObjBLAssessment.BL_GetAssessmentDetails(new Assessment() { AssessmentID = billid.GetValueOrDefault(), IntStatus = 2 });

                        if (mObjAssessmentData != null && mObjAssessmentData.TaxPayerID == mObjCompanyData.CompanyID && mObjAssessmentData.TaxPayerTypeID == (int)EnumList.TaxPayerType.Companies)
                        {
                            IList<usp_GetAssessment_AssessmentRuleList_Result> lstMAPAssessmentRules = mObjBLAssessment.BL_GetAssessmentRules(mObjAssessmentData.AssessmentID.GetValueOrDefault());
                            IList<usp_GetAssessmentRuleItemList_Result> lstAssessmentItems = mObjBLAssessment.BL_GetAssessmentRuleItem(mObjAssessmentData.AssessmentID.GetValueOrDefault());
                            IList<usp_GetAssessmentRuleBasedSettlement_Result> lstAssessmentRuleSettlement = mObjBLAssessment.BL_GetAssessmentRuleBasedSettlement(mObjAssessmentData.AssessmentID.GetValueOrDefault());
                            IList<usp_GetAssessmentAdjustmentList_Result> lstAssessmentAdjustment = mObjBLAssessment.BL_GetAssessmentAdjustment(mObjAssessmentData.AssessmentID.GetValueOrDefault());
                            IList<usp_GetAssessmentLateChargeList_Result> lstAssessmentLateCharge = mObjBLAssessment.BL_GetAssessmentLateCharge(mObjAssessmentData.AssessmentID.GetValueOrDefault());

                            IList<DropDownListResult> lstSettlementMethod = mObjBLAssessment.BL_GetSettlementMethodAssessmentRuleBased(mObjAssessmentData.AssessmentID.GetValueOrDefault());

                            IList<usp_GetSettlementList_Result> lstSettlement = new BLSettlement().BL_GetSettlementList(new Settlement() { ServiceBillID = -1, AssessmentID = mObjAssessmentData.AssessmentID.GetValueOrDefault() });

                            ViewBag.SettlementMethodList = new SelectList(lstSettlementMethod, "id", "text");
                            ViewBag.MAPAssessmentRules = lstMAPAssessmentRules;
                            ViewBag.AssessmentItems = lstAssessmentItems;
                            ViewBag.AssessmentRuleSettlement = lstAssessmentRuleSettlement;
                            ViewBag.SettlementList = lstSettlement;
                            ViewBag.AdjustmentList = lstAssessmentAdjustment;
                            ViewBag.LateChargeList = lstAssessmentLateCharge;

                            return View("AssessmentBillDetail", mObjAssessmentData);
                        }
                        else
                        {
                            return RedirectToAction("Search", "CaptureIndividual");
                        }

                    }
                    else if (billrefno.StartsWith("SB"))
                    {
                        BLMDAService mObjBLMDAService = new BLMDAService();
                        BLMDAServiceItem mObjBLMDAServiceItem = new BLMDAServiceItem();
                        BLServiceBill mObjBLServiceBill = new BLServiceBill();

                        usp_GetServiceBillList_Result mObjServiceBillData = mObjBLServiceBill.BL_GetServiceBillDetails(new ServiceBill() { ServiceBillID = billid.GetValueOrDefault(), IntStatus = 2 });

                        if (mObjServiceBillData != null && mObjServiceBillData.TaxPayerID == mObjCompanyData.CompanyID && mObjServiceBillData.TaxPayerTypeID == (int)EnumList.TaxPayerType.Companies)
                        {
                            IList<usp_GetServiceBill_MDAServiceList_Result> lstMAPServiceBillServices = mObjBLServiceBill.BL_GetMDAServiceList(mObjServiceBillData.ServiceBillID.GetValueOrDefault());
                            IList<usp_GetServiceBillItemList_Result> lstServiceBillItems = mObjBLServiceBill.BL_GetServiceBillItem(mObjServiceBillData.ServiceBillID.GetValueOrDefault());
                            IList<usp_GetMDAServiceBasedSettlement_Result> lstMDAServiceSettlement = mObjBLServiceBill.BL_GetMDAServiceBasedSettlement(mObjServiceBillData.ServiceBillID.GetValueOrDefault());
                            IList<usp_GetServiceBillAdjustmentList_Result> lstServiceBillAdjustment = mObjBLServiceBill.BL_GetServiceBillAdjustment(mObjServiceBillData.ServiceBillID.GetValueOrDefault());
                            IList<usp_GetServiceBillLateChargeList_Result> lstServiceBillLateCharge = mObjBLServiceBill.BL_GetServiceBillLateCharge(mObjServiceBillData.ServiceBillID.GetValueOrDefault());
                            IList<DropDownListResult> lstSettlementMethod = mObjBLServiceBill.BL_GetSettlementMethodMDAServiceBased(mObjServiceBillData.ServiceBillID.GetValueOrDefault());

                            IList<usp_GetSettlementList_Result> lstSettlement = new BLSettlement().BL_GetSettlementList(new Settlement() { AssessmentID = -1, ServiceBillID = mObjServiceBillData.ServiceBillID.GetValueOrDefault() });

                            ViewBag.SettlementMethodList = new SelectList(lstSettlementMethod, "id", "text");
                            ViewBag.MAPServiceBillRules = lstMAPServiceBillServices;
                            ViewBag.ServiceBillItems = lstServiceBillItems;
                            ViewBag.ServiceBillRuleSettlement = lstMDAServiceSettlement;
                            ViewBag.SettlementList = lstSettlement;
                            ViewBag.AdjustmentList = lstServiceBillAdjustment;
                            ViewBag.LateChargeList = lstServiceBillLateCharge;

                            return View("ServiceBillDetail", mObjServiceBillData);
                        }
                        else
                        {
                            return RedirectToAction("Search", "CaptureCorporate");
                        }
                    }
                    else
                    {
                        return RedirectToAction("Search", "CaptureCorporate");
                    }
                }
                else
                {
                    return RedirectToAction("Search", "CaptureCorporate");
                }
            }
            else
            {
                return RedirectToAction("Search", "CaptureCorporate");
            }
        }
        public JsonResult UpdateAssetStatus(MAP_TaxPayer_Asset pObjAssetData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjAssetData.TPAID != 0)
            {
                pObjAssetData.TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies;
                pObjAssetData.IntStatus = 2;
                FuncResponse<IList<usp_GetTaxPayerAssetList_Result>> mObjFuncResponse = new BLTaxPayerAsset().BL_UpdateTaxPayerAssetStatus(pObjAssetData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["AssetList"] = CommUtil.RenderPartialToString("_BindAssetTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
                }

            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Action";
            }



            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }



        public ActionResult GenerateBill(int? id, string name, int? billid, string billrefno)
        {
            if (id.GetValueOrDefault() > 0 && billid.GetValueOrDefault() > 0)
            {
                Company mObjCompany = new Company()
                {
                    CompanyID = id.GetValueOrDefault(),
                    intStatus = 1
                };

                usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(mObjCompany);
                string mHtmlDirectory = $"{DocumentHTMLLocation}/assessmentBills.html";
                if (mObjCompanyData != null)
                {
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
                    if (billrefno.StartsWith("AB"))
                    {
                        BLAssessment mObjBLAssessment = new BLAssessment();

                        usp_GetAssessmentList_Result mObjAssessmentData = mObjBLAssessment.BL_GetAssessmentDetails(new Assessment() { AssessmentID = billid.GetValueOrDefault(), IntStatus = 2 });

                        if (mObjAssessmentData != null && mObjAssessmentData.TaxPayerID == mObjCompanyData.CompanyID && mObjAssessmentData.TaxPayerTypeID == (int)EnumList.TaxPayerType.Companies)
                        {
                            IList<usp_GetAssessment_AssessmentRuleList_Result> lstMAPAssessmentRules = mObjBLAssessment.BL_GetAssessmentRules(mObjAssessmentData.AssessmentID.GetValueOrDefault());
                            IList<usp_GetAssessmentAdjustmentList_Result> lstAssessmentAdjustment = mObjBLAssessment.BL_GetAssessmentAdjustment(mObjAssessmentData.AssessmentID.GetValueOrDefault());
                            IList<usp_GetAssessmentLateChargeList_Result> lstAssessmentLateCharge = mObjBLAssessment.BL_GetAssessmentLateCharge(mObjAssessmentData.AssessmentID.GetValueOrDefault());
                            string strTaxYear = DateTime.Now.Year.ToString();
                            StringBuilder sbBillSummary = new StringBuilder();
                            sbBillSummary.Append("<ul>");

                            if (lstMAPAssessmentRules.Count > 1)
                            {
                                foreach (var item in lstMAPAssessmentRules)
                                {
                                    sbBillSummary.Append("<li>"); sbBillSummary.Append(item.AssessmentRuleName); sbBillSummary.Append(" - "); sbBillSummary.Append(CommUtil.GetFormatedCurrency(item.AssessmentRuleAmount)); sbBillSummary.Append("</li>");
                                }
                            }
                            else
                            {
                                foreach (var item in lstMAPAssessmentRules)
                                {
                                    sbBillSummary.Append("<li>"); sbBillSummary.Append(item.AssessmentRuleName); sbBillSummary.Append(" - "); sbBillSummary.Append(CommUtil.GetFormatedCurrency(item.AssessmentRuleAmount + lstAssessmentAdjustment.Sum(o => o.Amount) + lstAssessmentLateCharge.Sum(o => o.TotalAmount))); sbBillSummary.Append("</li>");
                                }
                            }

                            sbBillSummary.Append("</ul>");
                            marksheet = marksheet.Replace("@@rptTaxYear@@", strTaxYear)
                          .Replace("@@rptBillType@@", "1")
                        .Replace("@@rptReferenceNumber@@", mObjAssessmentData.AssessmentRefNo)
                          .Replace("@@rptTaxPayerRIN@@", mObjCompanyData.CompanyRIN)
                         .Replace("@@rptTaxPayerName@@", mObjCompanyData.CompanyName)
                         .Replace("@@rptTaxPayerNumber@@", mObjCompanyData.MobileNumber1)
                         .Replace("@@rptTaxPayerContactAddress@@", mObjCompanyData.ContactAddress)
                         .Replace("@@rptBillSummary@@", sbBillSummary.ToString())
                        .Replace("@@rptBillNotes@@", mObjAssessmentData.AssessmentNotes)
                         .Replace("@@rptBillDate@@", CommUtil.GetFormatedFullDate(mObjAssessmentData.AssessmentDate))
                          .Replace("@@rptBillStatus@@", mObjAssessmentData.SettlementStatusName)
                         .Replace("@@rptBillAmount@@", CommUtil.GetFormatedCurrency(mObjAssessmentData.AssessmentAmount + lstAssessmentAdjustment.Sum(o => o.Amount) + lstAssessmentLateCharge.Sum(o => o.TotalAmount)))
                          .Replace("@@rptBillPaidAmount@@", CommUtil.GetFormatedCurrency(mObjAssessmentData.SettlementAmount))
                          .Replace("@@rptBillAmountDue@@", CommUtil.GetFormatedCurrency(mObjAssessmentData.AssessmentAmount + lstAssessmentAdjustment.Sum(o => o.Amount) + lstAssessmentLateCharge.Sum(o => o.TotalAmount) - mObjAssessmentData.SettlementAmount.GetValueOrDefault()))
                         .Replace("@@rptBillDueDate@@", CommUtil.GetFormatedFullDate(mObjAssessmentData.SettlementDueDate))
                         .Replace("@@rptAgencyName@@", mObjAssessmentData.ReportingAgencyName);

                            PdfDocument doc = pdf.ConvertHtmlString(marksheet);
                            var bytes = doc.Save();
                            string strDirectory = "/Bills/";
                            string strfilename = DateTime.Now.ToString("ddMMyyyymmss_") + mObjAssessmentData.AssessmentRefNo + ".pdf";

                            string strExportFilePath = GlobalDefaultValues.DocumentLocation + strDirectory + strfilename;

                            if (!Directory.Exists(GlobalDefaultValues.DocumentLocation + strDirectory))
                            {
                                Directory.CreateDirectory(GlobalDefaultValues.DocumentLocation + strDirectory);
                            }

                            if (System.IO.File.Exists(strExportFilePath))
                                System.IO.File.Delete(strExportFilePath);

                            System.IO.File.WriteAllBytes(strExportFilePath, bytes);
                            //Send email to assessment@eras.eirs.gov.ng
                            string strSubject = $"Assessment Bill Generated : {mObjAssessmentData.AssessmentRefNo}";
                            ArrayList mObjAttachment = new ArrayList {
                                new Attachment(strExportFilePath)
                            };

                            EmailHandler.SendEmail(GlobalDefaultValues.BILL_NOTIFICATION_EMAIL, GlobalDefaultValues.BILL_NOTIFICATION_EMAIL, strSubject, false, "", mObjAttachment);

                            return File(strExportFilePath, "application/pdf");

                        }
                        else
                        {
                            return RedirectToAction("Search", "CaptureCorporate");
                        }

                    }
                    else if (billrefno.StartsWith("SB"))
                    {
                        BLMDAService mObjBLMDAService = new BLMDAService();
                        BLMDAServiceItem mObjBLMDAServiceItem = new BLMDAServiceItem();
                        BLServiceBill mObjBLServiceBill = new BLServiceBill();

                        usp_GetServiceBillList_Result mObjServiceBillData = mObjBLServiceBill.BL_GetServiceBillDetails(new ServiceBill() { ServiceBillID = billid.GetValueOrDefault(), IntStatus = 2 });

                        if (mObjServiceBillData != null && mObjServiceBillData.TaxPayerID == mObjCompanyData.CompanyID && mObjServiceBillData.TaxPayerTypeID == (int)EnumList.TaxPayerType.Companies)
                        {
                            IList<usp_GetServiceBill_MDAServiceList_Result> lstMAPServiceBillServices = mObjBLServiceBill.BL_GetMDAServiceList(mObjServiceBillData.ServiceBillID.GetValueOrDefault());
                            IList<usp_GetServiceBillAdjustmentList_Result> lstServiceBillAdjustment = mObjBLServiceBill.BL_GetServiceBillAdjustment(mObjServiceBillData.ServiceBillID.GetValueOrDefault());
                            IList<usp_GetServiceBillLateChargeList_Result> lstServiceBillLateCharge = mObjBLServiceBill.BL_GetServiceBillLateCharge(mObjServiceBillData.ServiceBillID.GetValueOrDefault());
                            string strTaxYear = DateTime.Now.Year.ToString();
                            StringBuilder sbBillSummary = new StringBuilder();
                            sbBillSummary.Append("<ul>");

                            if (lstMAPServiceBillServices.Count > 1)
                            {
                                foreach (var item in lstMAPServiceBillServices)
                                {
                                    sbBillSummary.Append("<li>"); sbBillSummary.Append(item.MDAServiceName);
                                    sbBillSummary.Append(" - "); sbBillSummary.Append(CommUtil.GetFormatedCurrency(item.ServiceAmount)); sbBillSummary.Append("</li>");
                                }
                            }
                            else
                            {
                                foreach (var item in lstMAPServiceBillServices)
                                {
                                    sbBillSummary.Append("<li>"); sbBillSummary.Append(item.MDAServiceName); sbBillSummary.Append(" - "); sbBillSummary.Append(CommUtil.GetFormatedCurrency(mObjServiceBillData.ServiceBillAmount + lstServiceBillAdjustment.Sum(o => o.Amount) + lstServiceBillLateCharge.Sum(o => o.TotalAmount))); sbBillSummary.Append("</li>");
                                }
                            }

                            sbBillSummary.Append("</ul>");

                            //Generating Invoice
                            LocalReport localReport = new LocalReport
                            {
                                EnableExternalImages = true,
                                EnableHyperlinks = true,
                                ReportPath = Server.MapPath("~\\RDLC\\Bill.rdlc"),
                            };


                            //localReport.Refresh();
                            //localReport.DataSources.Clear();

                            //ReportParameter[] Rpt = new ReportParameter[16];
                            //Rpt[0] = new ReportParameter("rptTaxYear", strTaxYear);
                            //Rpt[1] = new ReportParameter("rptBillType", "2");
                            //Rpt[2] = new ReportParameter("rptReferenceNumber", mObjServiceBillData.ServiceBillRefNo);
                            //Rpt[3] = new ReportParameter("rptTaxPayerRIN", mObjCompanyData.CompanyRIN);
                            //Rpt[4] = new ReportParameter("rptTaxPayerName", mObjCompanyData.CompanyName);
                            //Rpt[5] = new ReportParameter("rptTaxPayerNumber", mObjCompanyData.MobileNumber1);
                            //Rpt[6] = new ReportParameter("rptTaxPayerContactAddress", mObjCompanyData.ContactAddress);
                            //Rpt[7] = new ReportParameter("rptBillSummary", sbBillSummary.ToString());
                            //Rpt[8] = new ReportParameter("rptBillNotes", mObjServiceBillData.Notes);
                            //Rpt[9] = new ReportParameter("rptBillDate", CommUtil.GetFormatedFullDate(mObjServiceBillData.ServiceBillDate));
                            //Rpt[10] = new ReportParameter("rptBillStatus", mObjServiceBillData.SettlementStatusName);
                            //Rpt[11] = new ReportParameter("rptBillAmount", CommUtil.GetFormatedCurrency(mObjServiceBillData.ServiceBillAmount + lstServiceBillAdjustment.Sum(o => o.Amount) + lstServiceBillLateCharge.Sum(o => o.TotalAmount)));
                            //Rpt[12] = new ReportParameter("rptBillPaidAmount", CommUtil.GetFormatedCurrency(mObjServiceBillData.SettlementAmount));

                            //Rpt[13] = new ReportParameter("rptBillAmountDue", CommUtil.GetFormatedCurrency(mObjServiceBillData.ServiceBillAmount + lstServiceBillAdjustment.Sum(o => o.Amount) +lstServiceBillLateCharge.Sum(o=>o.TotalAmount) - mObjServiceBillData.SettlementAmount.GetValueOrDefault()));

                            //Rpt[14] = new ReportParameter("rptBillDueDate", CommUtil.GetFormatedFullDate(mObjServiceBillData.SettlementDueDate));
                            //Rpt[15] = new ReportParameter("rptAgencyName", mObjServiceBillData.ReportingAgencyName);


                            //localReport.SetParameters(Rpt);
                            //localReport.Refresh();

                            //ReportDataSource mObjRDSAdjustment = new ReportDataSource("dsAdjustment", lstServiceBillAdjustment);
                            //ReportDataSource mObjRDSLateCharge = new ReportDataSource("dsLateCharge", lstServiceBillLateCharge);

                            //localReport.DataSources.Add(mObjRDSAdjustment);
                            //localReport.DataSources.Add(mObjRDSLateCharge);

                            //localReport.Refresh();

                            marksheet = marksheet.Replace("@@rptTaxYear@@", strTaxYear)
                         .Replace("@@rptBillType@@", "1")
                       .Replace("@@rptReferenceNumber@@", mObjServiceBillData.ServiceBillRefNo)
                         .Replace("@@rptTaxPayerRIN@@", mObjCompanyData.CompanyRIN)
                        .Replace("@@rptTaxPayerName@@", mObjCompanyData.CompanyName)
                        .Replace("@@rptTaxPayerNumber@@", mObjCompanyData.MobileNumber1)
                        .Replace("@@rptTaxPayerContactAddress@@", mObjCompanyData.ContactAddress)
                        .Replace("@@rptBillSummary@@", sbBillSummary.ToString())
                       .Replace("@@rptBillNotes@@", mObjServiceBillData.Notes)
                        .Replace("@@rptBillDate@@", CommUtil.GetFormatedFullDate(mObjServiceBillData.ServiceBillDate))
                         .Replace("@@rptBillStatus@@", mObjServiceBillData.SettlementStatusName)
                        .Replace("@@rptBillAmount@@", CommUtil.GetFormatedCurrency(mObjServiceBillData.ServiceBillAmount + +lstServiceBillAdjustment.Sum(o => o.Amount) + lstServiceBillLateCharge.Sum(o => o.TotalAmount)))
                         .Replace("@@rptBillPaidAmount@@", CommUtil.GetFormatedCurrency(mObjServiceBillData.SettlementAmount))
                         .Replace("@@rptBillAmountDue@@", CommUtil.GetFormatedCurrency(mObjServiceBillData.ServiceBillAmount + lstServiceBillAdjustment.Sum(o => o.Amount) + lstServiceBillLateCharge.Sum(o => o.TotalAmount) - mObjServiceBillData.SettlementAmount.GetValueOrDefault()))
                        .Replace("@@rptBillDueDate@@", CommUtil.GetFormatedFullDate(mObjServiceBillData.SettlementDueDate))
                        .Replace("@@rptAgencyName@@", mObjServiceBillData.ReportingAgencyName);

                            PdfDocument doc = pdf.ConvertHtmlString(marksheet);
                            var bytes = doc.Save();
                            string strDirectory = "/Bills/";
                            string strfilename = DateTime.Now.ToString("ddMMyyyymmss_") + mObjServiceBillData.ServiceBillRefNo + ".pdf";

                            string strExportFilePath = GlobalDefaultValues.DocumentLocation + strDirectory + strfilename;

                            if (!Directory.Exists(GlobalDefaultValues.DocumentLocation + strDirectory))
                            {
                                Directory.CreateDirectory(GlobalDefaultValues.DocumentLocation + strDirectory);
                            }

                            if (System.IO.File.Exists(strExportFilePath))
                                System.IO.File.Delete(strExportFilePath);

                            //    //CommUtil.RenderReportNStoreInFile(strExportFilePath, localReport, "PDF");

                            System.IO.File.WriteAllBytes(strExportFilePath, bytes);
                            ////CommUtil.RenderReportNStoreInFile(strExportFilePath, localReport, "PDF");

                            //Send email to assessment@eras.eirs.gov.ng
                            string strSubject = $"Service Bill Generated : {mObjServiceBillData.ServiceBillRefNo}";
                            ArrayList mObjAttachment = new ArrayList {
                                new Attachment(strExportFilePath)
                            };

                            EmailHandler.SendEmail(GlobalDefaultValues.BILL_NOTIFICATION_EMAIL, GlobalDefaultValues.BILL_NOTIFICATION_EMAIL, strSubject, false, "", mObjAttachment);

                            return File(strExportFilePath, "application/pdf");

                        }
                        else
                        {
                            return RedirectToAction("Search", "CaptureCorporate");
                        }
                    }
                    else
                    {
                        return RedirectToAction("Search", "CaptureCorporate");
                    }
                }
                else
                {
                    return RedirectToAction("Search", "CaptureCorporate");
                }
            }
            else
            {
                return RedirectToAction("Search", "CaptureCorporate");
            }
        }

        public JsonResult UpdateStatus(Company pObjCompanyData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjCompanyData.CompanyID != 0)
            {
                FuncResponse mObjFuncResponse = new BLCompany().BL_UpdateStatus(pObjCompanyData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;
            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Action";
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddDocument(int? id, string name)
        {
            string url = getUrl();
            bool itCan = new UtilityController().CheckAccess(url);
            if (!itCan) { return RedirectToAction("AccessDenied", "Utility"); }
            if (id.GetValueOrDefault() > 0)
            {
                Company mObjCompany = new Company()
                {
                    CompanyID = id.GetValueOrDefault(),
                    intStatus = 1
                };

                usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(mObjCompany);

                if (mObjCompanyData != null)
                {
                    TaxPayerDocumentViewModel mObjDocumentViewModel = new TaxPayerDocumentViewModel()
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


                    return View(mObjDocumentViewModel);
                }
                else
                {
                    return RedirectToAction("Search", "CaptureCorporate");
                }
            }
            else
            {
                return RedirectToAction("Search", "CaptureCorporate");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddDocument(TaxPayerDocumentViewModel pObjTaxPayerDocument)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjTaxPayerDocument);
            }
            else
            {
                try
                {
                    string strDocumentPath = "";

                    if (pObjTaxPayerDocument.DocumentFileUpload != null && pObjTaxPayerDocument.DocumentFileUpload.ContentLength > 0)
                    {
                        string strDirectory = $"{GlobalDefaultValues.DocumentLocation}TaxPayerDocs/{pObjTaxPayerDocument.TaxPayerTypeID}_{pObjTaxPayerDocument.TaxPayerID}/";
                        string mstrFileName = $"{DateTime.Now:ddMMyyyyhhmmss_}{Path.GetFileName(pObjTaxPayerDocument.DocumentFileUpload.FileName)}";
                        if (!Directory.Exists(strDirectory))
                            Directory.CreateDirectory(strDirectory);
                        string mStrSignaturePath = Path.Combine(strDirectory, mstrFileName);
                        pObjTaxPayerDocument.DocumentFileUpload.SaveAs(mStrSignaturePath);

                        strDocumentPath = $"TaxPayerDocs/{pObjTaxPayerDocument.TaxPayerTypeID}_{pObjTaxPayerDocument.TaxPayerID}/{mstrFileName}";
                    }

                    MAP_TaxPayer_Document mObjTaxPayerDocument = new MAP_TaxPayer_Document()
                    {
                        TaxPayerID = pObjTaxPayerDocument.TaxPayerID,
                        TaxPayerTypeID = pObjTaxPayerDocument.TaxPayerTypeID,
                        DocumentDate = CommUtil.GetCurrentDateTime(),
                        DocumentPath = strDocumentPath,
                        DocumentTitle = pObjTaxPayerDocument.DocumentTitle,
                        Notes = pObjTaxPayerDocument.Notes,
                        StaffID = SessionManager.UserID,
                        TPDID = 0,
                        CreatedBy = SessionManager.UserID,
                        CreatedDate = CommUtil.GetCurrentDateTime()
                    };

                    FuncResponse mObjFuncResponse = new BLTaxPayerDocument().BL_InsertTaxPayerDocument(mObjTaxPayerDocument);

                    if (mObjFuncResponse.Success)
                    {
                        FlashMessage.Info("Document Added Successfully");
                        return RedirectToAction("Details", "CaptureCorporate", new { id = pObjTaxPayerDocument.TaxPayerID, name = pObjTaxPayerDocument.TaxPayerRIN });
                    }
                    else
                    {
                        ViewBag.Message = mObjFuncResponse.Message;
                        return View(pObjTaxPayerDocument);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving document";
                    return View(pObjTaxPayerDocument);
                }
            }
        }

        public ActionResult DocumentDetails(long? tpdid)
        {
            string url = getUrl();
            bool itCan = new UtilityController().CheckAccess(url);
            if (!itCan) { return RedirectToAction("AccessDenied", "Utility"); }
            if (tpdid.GetValueOrDefault() > 0)
            {
                usp_GetTaxPayerDocumentList_Result mObjDocumentData = new BLTaxPayerDocument().BL_GetTaxPayerDocumentDetails(tpdid.GetValueOrDefault());

                if (mObjDocumentData != null)
                {
                    usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(new Company() { CompanyID = mObjDocumentData.TaxPayerID.GetValueOrDefault(), intStatus = 2 });

                    ViewBag.CompanyData = mObjCompanyData;

                    return View(mObjDocumentData);
                }
                else
                {
                    return RedirectToAction("Search", "CaptureCorporate");
                }
            }
            else
            {
                return RedirectToAction("Search", "CaptureCorporate");
            }
        }

        static string DocumentHTMLLocation = WebConfigurationManager.AppSettings["documentHTMLLocation"] ?? "";
        static string documentLocation = WebConfigurationManager.AppSettings["documentLocation"] ?? "";
    }
}