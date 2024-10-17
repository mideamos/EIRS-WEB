using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;
using Elmah;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Vereyon.Web;
using System.Linq;
using System.Transactions;
using EIRS.Web.Models;
using System.IO;
using System.Text;
using Microsoft.Reporting.WebForms;
using System.Collections;
using System.Net.Mail;
using System.Security.Cryptography;
using EIRS.Repository;
using Newtonsoft.Json;
using System.Data.Entity;
using static EIRS.Web.Controllers.Filters;
using System.Configuration;
using System.Web.Configuration;
using SelectPdf;
using EIRS.Web.Utility;
using static EIRS.Common.EnumList;
using Twilio.TwiML.Voice;
using System.Web.Script.Serialization;

namespace EIRS.Web.Controllers
{
    [SessionTimeout]

    public class CaptureIndividualController : BaseController
    {
        private ASCIIEncoding MyASCIIEncoding = new ASCIIEncoding();
        private static TripleDESCryptoServiceProvider cryptDES3 = new TripleDESCryptoServiceProvider();
        private static MD5CryptoServiceProvider cryptMD5Hash = new MD5CryptoServiceProvider();
        EIRSEntities _db = new EIRSEntities();
        IAssessmentRepository _AssessmentRepository;
        IAdjustmentRepository _AdjustmentRepository;

        public CaptureIndividualController()
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
                sbWhereCondition.Append(" AND ( ISNULL(IndividualRIN,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(FirstName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(LastName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(TIN,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(ContactAddress,'') LIKE @MainFilter)");
            }

            Individual mObjIndividual = new Individual()
            {
                WhereCondition = sbWhereCondition.ToString(),
                OrderBy = vSortColumn,
                OrderByDirection = vSortColumnDir,
                PageNumber = (TrynParse.parseInt(vStart) / TrynParse.parseInt(vLength)) + 1,
                PageSize = TrynParse.parseInt(vLength),
                MainFilter = !string.IsNullOrWhiteSpace(vFilter) ? "%" + vFilter.Trim() + "%" : vFilter
            };


            IDictionary<string, object> dcData = new BLIndividual().BL_SearchIndividualForSideMenu(mObjIndividual);
            IList<usp_SearchIndividualForSideMenu_Result> lstIndividual = (IList<usp_SearchIndividualForSideMenu_Result>)dcData["IndivudalList"];

            return Json(new
            {
                draw = vDraw,
                recordsFiltered = dcData["FilteredRecords"],
                recordsTotal = dcData["TotalRecords"],
                data = lstIndividual
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ListWithExport()
        {
            string url = getUrl();
            bool itCan = new UtilityController().CheckAccess(url);
            if (!itCan) { return RedirectToAction("AccessDenied", "Utility"); }
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
                sbWhereCondition.Append(" AND ( ISNULL(IndividualRIN,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(FirstName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(LastName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(TIN,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(ContactAddress,'') LIKE @MainFilter)");
            }

            Individual mObjIndividual = new Individual()
            {
                WhereCondition = sbWhereCondition.ToString(),
                OrderBy = vSortColumn,
                OrderByDirection = vSortColumnDir,
                PageNumber = (TrynParse.parseInt(vStart) / TrynParse.parseInt(vLength)) + 1,
                PageSize = TrynParse.parseInt(vLength),
                MainFilter = !string.IsNullOrWhiteSpace(vFilter) ? "%" + vFilter.Trim() + "%" : vFilter
            };


            IDictionary<string, object> dcData = new BLIndividual().BL_SearchIndividualForSideMenu(mObjIndividual);
            IList<usp_SearchIndividualForSideMenu_Result> lstIndividual = (IList<usp_SearchIndividualForSideMenu_Result>)dcData["IndivudalList"];

            return Json(new
            {
                draw = vDraw,
                recordsFiltered = dcData["FilteredRecords"],
                recordsTotal = dcData["TotalRecords"],
                data = lstIndividual
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ExportData()
        {
            IList<usp_GetIndividualList_Result> lstIndividualData = new BLIndividual().BL_GetIndividualList(new Individual() { intStatus = 2 });
            string[] strColumns = new string[] { "IndividualRIN", "GenderName", "TitleName", "FirstName", "LastName", "MiddleName", "DOB", "TIN", "MobileNumber1", "MobileNumber2", "EmailAddress1", "EmailAddress2", "BiometricDetails", "TaxOfficeName", "MaritalStatusName", "NationalityName", "TaxPayerTypeName", "EconomicActivitiesName", "NotificationMethodName", "ContactAddress", "ActiveText" };
            return ExportToExcel(lstIndividualData, this.RouteData, strColumns, "Individual");
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
            string mStrName = pObjCollection.Get("txtName");
            string mStrMobileNumber = pObjCollection.Get("txtMobileNumber");
            string mStrRIN = pObjCollection.Get("txtRIN");

            Individual mObjIndividual = new Individual()
            {
                IndividualName = mStrName,
                MobileNumber1 = mStrMobileNumber,
                TIN = mStrRIN,
                IndividualRIN = mStrRIN,
                NIN = mStrRIN,
                intStatus = 1
            };

            IList<usp_GetIndividualList_Result> lstIndividual = new BLIndividual().BL_GetIndividualList(mObjIndividual);
            return PartialView("_BindTable", lstIndividual.Take(5).ToList());
        }

        public void UI_FillDropDown(IndividualViewModel pObjIndividualViewModel = null)
        {
            if (pObjIndividualViewModel != null)
                pObjIndividualViewModel.TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual;
            else if (pObjIndividualViewModel == null)
                pObjIndividualViewModel = new IndividualViewModel();
            int LOGINtAXoFFICE = SessionManager.TaxOfficeID;
            UI_FillGender();
            UI_FillTitleDropDown(new Title() { intStatus = 1, IncludeTitleIds = pObjIndividualViewModel.TitleID.ToString(), GenderID = pObjIndividualViewModel.GenderID });
            UI_FillTaxOfficeDropDown(new Tax_Offices() { intStatus = 1, IncludeTaxOfficeIds = pObjIndividualViewModel.TaxOfficeID.ToString() });
            UI_FillTaxOfficeDropDownForStatic(new Tax_Offices() { intStatus = 1, IncludeTaxOfficeIds = pObjIndividualViewModel.TaxOfficeID.ToString() }, false, 0, LOGINtAXoFFICE);
            UI_FillTaxOfficeDropDownForStatic(new Tax_Offices() { intStatus = 1, IncludeTaxOfficeIds = pObjIndividualViewModel.TaxOfficeID.ToString() }, false, pObjIndividualViewModel.TaxOfficeID.GetValueOrDefault(), 0);
            UI_FillMaritalStatus();
            UI_FillNationality();
            UI_FillTaxPayerTypeDropDown(new TaxPayer_Types() { intStatus = 1, IncludeTaxPayerTypeIds = pObjIndividualViewModel.TaxPayerTypeID.ToString() }, (int)EnumList.TaxPayerType.Individual);
            UI_FillEconomicActivitiesDropDown(new Economic_Activities() { intStatus = 1, IncludeEconomicActivitiesIds = pObjIndividualViewModel.EconomicActivitiesID.ToString(), TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual });
            UI_FillNotificationMethodDropDown(new Notification_Method() { intStatus = 1, IncludeNotificationMethodIds = pObjIndividualViewModel.NotificationMethodID.ToString() });
        }
        public ActionResult Add(string nin = "", bool showModal = false)
        {
            string url = getUrl();
            bool itCan = new UtilityController().CheckAccess(url);
            if (!itCan)
            {
                return RedirectToAction("AccessDenied", "Utility");
            }

            IndividualViewModel ivm = new IndividualViewModel()
            {
                TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                NIN = nin
            };

            if (!string.IsNullOrEmpty(nin))
            {
                var individualData = _db.Individuals.Select(i => new IndividualFormModel
                {
                    FirstName = i.FirstName,
                    LastName = i.LastName,
                    MiddleName = i.MiddleName,
                    NIN = i.NIN,
                    NINStatus = i.NINStatus,
                    ContactAddress = i.ContactAddress
                }).FirstOrDefault(i => i.NIN == nin);

                if (individualData != null)
                {
                    ivm.FirstName = individualData.FirstName;
                    ivm.LastName = individualData.LastName;
                    ivm.ContactAddress = individualData.ContactAddress;
                    var IndNIN = _db.Individuals.FirstOrDefault(i => i.NIN == nin);
                    // Populate the NINStatus in the ViewBag for icon rendering in the view
                    ViewBag.NINStatus = IndNIN.NINStatus;
                }
            }

            UI_FillDropDown(ivm);
            ViewBag.ShowModal = showModal;
            return View(ivm);
        }

        public ActionResult CheckActiveNIN(string nin)
        {
            if (string.IsNullOrEmpty(nin))
            {
                return Json(new { success = false, message = "NIN is required." });
            }

            //Check Individual Table for Taxpayer Existence 
            var individual = _db.Individuals.Select(i => new IndividualFormModel
            {
                FirstName = i.FirstName,
                LastName = i.LastName,
                MiddleName = i.MiddleName,
                NIN = i.NIN,
                NINStatus = i.NINStatus,
                ContactAddress = i.ContactAddress
            }).FirstOrDefault(i => i.NIN == nin);

            if (individual != null && !string.IsNullOrEmpty(individual.NIN))
            {
                if(!string.IsNullOrEmpty(individual.NINStatus))
                {
                    var IndNIN = _db.Individuals.FirstOrDefault(i => i.NIN == nin);
                    if (IndNIN.NINStatus == "Valid")
                    {
                        ViewBag.FillIndividual = individual;
                    }
                    else if (IndNIN.NINStatus == "Invalid")
                    {
                        ViewBag.FillIndividual = individual;
                    }
                    else if (IndNIN.NINStatus == "No NIN")
                    {
                        ViewBag.FillIndividual = individual;
                    }
                    else if (IndNIN.NINStatus == "Not verified")
                    {
                        ViewBag.FillIndividual = individual;
                    }
                }
                else
                {
                    var CheckNINDetails = _db.NINDetails.FirstOrDefault(x => x.NIN == nin);
                    if (CheckNINDetails != null && !string.IsNullOrEmpty(CheckNINDetails.NIN))
                    {
                        var existingIndividual = _db.Individuals.FirstOrDefault(i => i.NIN == nin);

                        if (existingIndividual != null && !string.IsNullOrEmpty(existingIndividual.NIN))
                        {
                            if (existingIndividual != null)
                            {
                                // Update the fields with new data
                                existingIndividual.FirstName = CheckNINDetails.FirstName;
                                existingIndividual.LastName = CheckNINDetails.Surname;
                                existingIndividual.MiddleName = CheckNINDetails.MiddleName;
                                existingIndividual.NINStatus = CheckNINDetails.status == "successful" ? "Valid" : "Invalid";
                                existingIndividual.ContactAddress = CheckNINDetails.ResidenceAdressLine1;

                                _db.Entry(existingIndividual).State = EntityState.Modified;
                                _db.SaveChanges();
                            }
                        }

                    }
                }


                return Json(new { success = true, message = "NIN found.", data = individual });

            }
            else
            {
                var CheckNINDetails = _db.NINDetails.FirstOrDefault(x => x.NIN == nin);

                if (CheckNINDetails != null && !string.IsNullOrEmpty(CheckNINDetails.NIN))
                {
                    var existingIndividual = _db.Individuals.FirstOrDefault(i => i.NIN == nin);

                    if (existingIndividual != null && !string.IsNullOrEmpty(existingIndividual.NIN))
                    {
                        if (existingIndividual != null)
                        {
                            // Update the fields with new data
                            existingIndividual.FirstName = CheckNINDetails.FirstName;
                            existingIndividual.LastName = CheckNINDetails.Surname;
                            existingIndividual.MiddleName = CheckNINDetails.MiddleName;
                            existingIndividual.NINStatus = CheckNINDetails.status == "successful" ? "Valid" : "Invalid";
                            existingIndividual.ContactAddress = CheckNINDetails.ResidenceAdressLine1;

                            _db.Entry(existingIndividual).State = EntityState.Modified;
                            _db.SaveChanges();
                        }
                    }
                    else
                    {
                        //find with Rin amd save or Update Api response to db
                    }

                }
                else
                {
                    //NIMC Api will be run here to get the NIN details
                }

                return Json(new { success = false, message = "NIN not found (Need to Look-Up NIMC)" });
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(IndividualViewModel pObjIndividualModel)
        {
            //if (!ModelState.IsValid)
            //{
            //    pObjIndividualModel.TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual;
            //    UI_FillDropDown(pObjIndividualModel);
            //    return View(pObjIndividualModel);
            //}
            //else
            //{
            var checker = _db.Individuals.FirstOrDefault(o => o.NIN == pObjIndividualModel.NIN);
            if (checker != null)
            {
                FlashMessage.Info("NIN Already Exist!!");
                return View(pObjIndividualModel);
            }
            Individual mObjIndividual = new Individual()
            {
                IndividualID = 0,
                GenderID = pObjIndividualModel.GenderID,
                TitleID = pObjIndividualModel.TitleID,
                FirstName = pObjIndividualModel.FirstName,
                LastName = pObjIndividualModel.LastName,
                MiddleName = pObjIndividualModel.MiddleName,
                DOB = DateTime.Parse(pObjIndividualModel.DOB),
                TIN = pObjIndividualModel.TIN,
                NIN = pObjIndividualModel.NIN,
                MobileNumber1 = pObjIndividualModel.MobileNumber1,
                MobileNumber2 = pObjIndividualModel.MobileNumber2,
                EmailAddress1 = pObjIndividualModel.EmailAddress1,
                EmailAddress2 = pObjIndividualModel.EmailAddress2,
                BiometricDetails = pObjIndividualModel.BiometricDetails,
                TaxOfficeID = pObjIndividualModel.TaxOfficeID,
                MaritalStatusID = pObjIndividualModel.MaritalStatusID,
                NationalityID = pObjIndividualModel.NationalityID,
                TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                EconomicActivitiesID = pObjIndividualModel.EconomicActivitiesID,
                NotificationMethodID = pObjIndividualModel.NotificationMethodID,
                ContactAddress = pObjIndividualModel.ContactAddress,
                Active = true,
                CreatedBy = SessionManager.UserID,
                CreatedDate = CommUtil.GetCurrentDateTime()
            };

            try
            {

                FuncResponse<Individual> mObjResponse = new BLIndividual().BL_InsertUpdateIndividual(mObjIndividual);

                if (mObjResponse.Success)
                {
                    if (GlobalDefaultValues.SendNotification)
                    {
                        //Send Notification
                        EmailDetails mObjEmailDetails = new EmailDetails()
                        {
                            TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                            TaxPayerTypeName = "Individual",
                            TaxPayerID = mObjIndividual.IndividualID,
                            TaxPayerName = mObjIndividual.FirstName + " " + mObjIndividual.LastName,
                            TaxPayerRIN = mObjIndividual.IndividualRIN,
                            TaxPayerMobileNumber = mObjIndividual.MobileNumber1,
                            TaxPayerEmail = mObjIndividual.EmailAddress1,
                            ContactAddress = mObjIndividual.ContactAddress,
                            TaxPayerTIN = mObjIndividual.TIN,
                            LoggedInUserID = SessionManager.UserID,
                        };

                        if (!string.IsNullOrWhiteSpace(mObjIndividual.EmailAddress1))
                        {
                            BLEmailHandler.BL_TaxPayerCreated(mObjEmailDetails);
                        }

                        if (!string.IsNullOrWhiteSpace(mObjIndividual.MobileNumber1))
                        {
                            UtilityController.BL_TaxPayerCreated(mObjEmailDetails);
                        }
                    }

                    Audit_Log mObjAuditLog = new Audit_Log()
                    {
                        LogDate = CommUtil.GetCurrentDateTime(),
                        ASLID = (int)EnumList.ALScreen.Capture_Individual_Add,
                        Comment = $"New Individual Added - {mObjResponse.AdditionalData.IndividualRIN}",
                        IPAddress = CommUtil.GetIPAddress(),
                        StaffID = SessionManager.UserID,
                    };

                    new BLAuditLog().BL_InsertAuditLog(mObjAuditLog);

                    FlashMessage.Info(mObjResponse.Message);
                    return RedirectToAction("Details", "CaptureIndividual", new { id = mObjResponse.AdditionalData.IndividualID, name = (mObjResponse.AdditionalData.FirstName + " " + mObjResponse.AdditionalData.LastName).ToSeoUrl() });
                }
                else
                {
                    UI_FillDropDown(pObjIndividualModel);
                    ViewBag.Message = mObjResponse.Message;
                    return View(pObjIndividualModel);
                }
            }
            catch (Exception ex)
            {
                Logger.SendErrorToText(ex);
                ErrorSignal.FromCurrentContext().Raise(ex);
                UI_FillDropDown(pObjIndividualModel);
                ViewBag.Message = "Error occurred while saving Individual";
                return View(pObjIndividualModel);
            }
        }
        public ActionResult AddWithoutNumber()
        {
            string url = getUrl();
            bool itCan = new UtilityController().CheckAccess(url);
            if (!itCan) { return RedirectToAction("AccessDenied", "Utility"); }
            UI_FillDropDown();

            IndividualViewModel mObjIndividualModel = new IndividualViewModel()
            {
                TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                MobileNumber1 = "5000000000"
            };

            return View(mObjIndividualModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddWithoutNumber(IndividualViewModel pObjIndividualModel)
        {
            //if (!ModelState.IsValid)
            //{
            //    UI_FillDropDown(pObjIndividualModel);
            //    return View(pObjIndividualModel);
            //}
            //else
            //{
            Individual mObjIndividual = new Individual()
            {
                IndividualID = 0,
                GenderID = pObjIndividualModel.GenderID,
                TitleID = pObjIndividualModel.TitleID,
                FirstName = pObjIndividualModel.FirstName,
                LastName = pObjIndividualModel.LastName,
                MiddleName = pObjIndividualModel.MiddleName,
                DOB = TrynParse.parseNullableDate(pObjIndividualModel.DOB),
                TIN = pObjIndividualModel.TIN,
                MobileNumber1 = pObjIndividualModel.MobileNumber1,
                MobileNumber2 = pObjIndividualModel.MobileNumber2,
                EmailAddress1 = pObjIndividualModel.EmailAddress1,
                EmailAddress2 = pObjIndividualModel.EmailAddress2,
                BiometricDetails = pObjIndividualModel.BiometricDetails,
                TaxOfficeID = pObjIndividualModel.TaxOfficeID,
                MaritalStatusID = pObjIndividualModel.MaritalStatusID,
                NationalityID = pObjIndividualModel.NationalityID,
                TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                EconomicActivitiesID = pObjIndividualModel.EconomicActivitiesID,
                NotificationMethodID = pObjIndividualModel.NotificationMethodID,
                ContactAddress = pObjIndividualModel.ContactAddress,
                Active = true,
                CreatedBy = SessionManager.UserID,
                CreatedDate = CommUtil.GetCurrentDateTime()
            };

            try
            {

                FuncResponse<Individual> mObjResponse = new BLIndividual().BL_InsertUpdateIndividual(mObjIndividual, true, true);

                if (mObjResponse.Success)
                {
                    if (GlobalDefaultValues.SendNotification)
                    {
                        //Send Notification
                        EmailDetails mObjEmailDetails = new EmailDetails()
                        {
                            TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                            TaxPayerTypeName = "Individual",
                            TaxPayerID = mObjIndividual.IndividualID,
                            TaxPayerName = mObjIndividual.FirstName + " " + mObjIndividual.LastName,
                            TaxPayerRIN = mObjIndividual.IndividualRIN,
                            TaxPayerMobileNumber = mObjIndividual.MobileNumber1,
                            TaxPayerEmail = mObjIndividual.EmailAddress1,
                            ContactAddress = mObjIndividual.ContactAddress,
                            TaxPayerTIN = mObjIndividual.TIN,
                            LoggedInUserID = SessionManager.UserID,
                        };

                        if (!string.IsNullOrWhiteSpace(mObjIndividual.EmailAddress1))
                        {
                            BLEmailHandler.BL_TaxPayerCreated(mObjEmailDetails);
                        }

                        if (!string.IsNullOrWhiteSpace(mObjIndividual.MobileNumber1))
                        {
                            UtilityController.BL_TaxPayerCreated(mObjEmailDetails);
                        }
                    }

                    Audit_Log mObjAuditLog = new Audit_Log()
                    {
                        LogDate = CommUtil.GetCurrentDateTime(),
                        ASLID = (int)EnumList.ALScreen.Capture_Individual_Add,
                        Comment = $"New Individual Added Without Number - {mObjResponse.AdditionalData.IndividualRIN}",
                        IPAddress = CommUtil.GetIPAddress(),
                        StaffID = SessionManager.UserID,
                    };

                    new BLAuditLog().BL_InsertAuditLog(mObjAuditLog);

                    FlashMessage.Info(mObjResponse.Message);
                    return RedirectToAction("Details", "CaptureIndividual", new { id = mObjResponse.AdditionalData.IndividualID, name = (mObjResponse.AdditionalData.FirstName + " " + mObjResponse.AdditionalData.LastName).ToSeoUrl() });
                }
                else
                {
                    UI_FillDropDown(pObjIndividualModel);
                    ViewBag.Message = mObjResponse.Message;
                    return View(pObjIndividualModel);
                }
            }
            catch (Exception ex)
            {
                Logger.SendErrorToText(ex);
                ErrorSignal.FromCurrentContext().Raise(ex);
                UI_FillDropDown(pObjIndividualModel);
                ViewBag.Message = "Error occurred while saving Individual";
                return View(pObjIndividualModel);
            }
            //}
        }
        public ActionResult Edit(int? id, string name)
        {
            string url = getUrl();
            bool itCan = new UtilityController().CheckAccess(url);
            if (!itCan) { return RedirectToAction("AccessDenied", "Utility"); }
            if (id.GetValueOrDefault() > 0)
            {
                Individual mObjIndividual = new Individual()
                {
                    IndividualID = id.GetValueOrDefault(),
                    intStatus = 1
                };

                usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(mObjIndividual);

                if (mObjIndividualData != null)
                {
                    string nin = _db.Individuals.FirstOrDefault(o => o.IndividualID == mObjIndividualData.IndividualID.Value).NIN;
                    IndividualViewModel mObjIndividualModelView = new IndividualViewModel()
                    {
                        IndividualID = mObjIndividualData.IndividualID.GetValueOrDefault(),
                        IndividualRIN = mObjIndividualData.IndividualRIN,
                        GenderID = mObjIndividualData.GenderID.GetValueOrDefault(),
                        TitleID = mObjIndividualData.TitleID.GetValueOrDefault(),
                        FirstName = mObjIndividualData.FirstName,
                        LastName = mObjIndividualData.LastName,
                        MiddleName = mObjIndividualData.MiddleName,
                        DOB = mObjIndividualData.DOB == null ? "" : mObjIndividualData.DOB.Value.ToString("dd/MM/yyyy"),
                        TIN = mObjIndividualData.TIN,
                        NIN = nin,
                        MobileNumber1 = mObjIndividualData.MobileNumber1,
                        MobileNumber2 = mObjIndividualData.MobileNumber2,
                        EmailAddress1 = mObjIndividualData.EmailAddress1,
                        EmailAddress2 = mObjIndividualData.EmailAddress2,
                        BiometricDetails = mObjIndividualData.BiometricDetails,
                        TaxOfficeID = mObjIndividualData.TaxOfficeID,
                        MaritalStatusID = mObjIndividualData.MaritalStatusID,
                        NationalityID = mObjIndividualData.NationalityID.GetValueOrDefault(),
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                        EconomicActivitiesID = mObjIndividualData.EconomicActivitiesID.GetValueOrDefault(),
                        NotificationMethodID = mObjIndividualData.NotificationMethodID.GetValueOrDefault(),
                        ContactAddress = mObjIndividualData.ContactAddress,
                        Active = mObjIndividualData.Active.GetValueOrDefault(),
                    };

                    UI_FillDropDown(mObjIndividualModelView);
                    return View(mObjIndividualModelView);
                }
                else
                {
                    return RedirectToAction("Search", "CaptureIndividual");
                }
            }
            else
            {
                return RedirectToAction("Search", "CaptureIndividual");
            }
        }
        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(IndividualViewModel pObjIndividualModel)
        {
            Individual mObjIndividual = new Individual()
            {
                IndividualID = pObjIndividualModel.IndividualID,
                GenderID = pObjIndividualModel.GenderID,
                TitleID = pObjIndividualModel.TitleID,
                FirstName = pObjIndividualModel.FirstName,
                LastName = pObjIndividualModel.LastName,
                MiddleName = pObjIndividualModel.MiddleName,
                DOB = DateTime.Parse(pObjIndividualModel.DOB),
                TIN = pObjIndividualModel.TIN,
                NIN = pObjIndividualModel.NIN,
                MobileNumber1 = pObjIndividualModel.MobileNumber1,
                MobileNumber2 = pObjIndividualModel.MobileNumber2,
                EmailAddress1 = pObjIndividualModel.EmailAddress1,
                EmailAddress2 = pObjIndividualModel.EmailAddress2,
                BiometricDetails = pObjIndividualModel.BiometricDetails,
                TaxOfficeID = pObjIndividualModel.TaxOfficeID,
                MaritalStatusID = pObjIndividualModel.MaritalStatusID,
                NationalityID = pObjIndividualModel.NationalityID,
                TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                EconomicActivitiesID = pObjIndividualModel.EconomicActivitiesID,
                NotificationMethodID = pObjIndividualModel.NotificationMethodID,
                ContactAddress = pObjIndividualModel.ContactAddress,
                Active = true,
                ModifiedBy = SessionManager.UserID,
                ModifiedDate = CommUtil.GetCurrentDateTime()
            };

            try
            {

                FuncResponse<Individual> mObjResponse = new BLIndividual().BL_InsertUpdateIndividual(mObjIndividual);

                if (mObjResponse.Success)
                {
                    if (GlobalDefaultValues.SendNotification)
                    {
                        //Send Notification
                        EmailDetails mObjEmailDetails = new EmailDetails()
                        {
                            TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                            TaxPayerTypeName = "Individual",
                            TaxPayerID = mObjIndividual.IndividualID,
                            TaxPayerName = mObjIndividual.FirstName + " " + mObjIndividual.LastName,
                            TaxPayerRIN = mObjIndividual.IndividualRIN,
                            TaxPayerMobileNumber = mObjIndividual.MobileNumber1,
                            TaxPayerEmail = mObjIndividual.EmailAddress1,
                            ContactAddress = mObjIndividual.ContactAddress,
                            TaxPayerTIN = mObjIndividual.TIN,
                            LoggedInUserID = SessionManager.UserID,
                        };

                        if (!string.IsNullOrWhiteSpace(mObjIndividual.EmailAddress1))
                        {
                            BLEmailHandler.BL_TaxPayerCreated(mObjEmailDetails);
                        }

                        if (!string.IsNullOrWhiteSpace(mObjIndividual.MobileNumber1))
                        {
                            UtilityController.BL_TaxPayerCreated(mObjEmailDetails);
                        }
                    }

                    FlashMessage.Info(mObjResponse.Message);
                    return RedirectToAction("Details", "CaptureIndividual", new { id = mObjResponse.AdditionalData.IndividualID, name = mObjResponse.AdditionalData.IndividualRIN.ToSeoUrl() });
                }
                else
                {
                    UI_FillDropDown(pObjIndividualModel);
                    ViewBag.Message = mObjResponse.Message;
                    return View(pObjIndividualModel);
                }
            }
            catch (Exception ex)
            {
                Logger.SendErrorToText(ex);
                ErrorSignal.FromCurrentContext().Raise(ex);
                UI_FillDropDown(pObjIndividualModel);
                ViewBag.Message = "Error occurred while saving Individual";
                return View(pObjIndividualModel);
            }
        }
        public ActionResult EditTaxOffice(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Individual mObjIndividual = new Individual()
                {
                    IndividualID = id.GetValueOrDefault(),
                    intStatus = 1
                };

                usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(mObjIndividual);

                if (mObjIndividualData != null)
                {
                    string nin = _db.Individuals.FirstOrDefault(o => o.IndividualID == mObjIndividualData.IndividualID.Value).NIN;
                    IndividualViewModel mObjIndividualModelView = new IndividualViewModel()
                    {
                        IndividualID = mObjIndividualData.IndividualID.GetValueOrDefault(),
                        IndividualRIN = mObjIndividualData.IndividualRIN,
                        GenderID = mObjIndividualData.GenderID.GetValueOrDefault(),
                        TitleID = mObjIndividualData.TitleID.GetValueOrDefault(),
                        FirstName = mObjIndividualData.FirstName,
                        LastName = mObjIndividualData.LastName,
                        MiddleName = mObjIndividualData.MiddleName,
                        DOB = mObjIndividualData.DOB == null ? "" : mObjIndividualData.DOB.Value.ToString("dd/MM/yyyy"),
                        TIN = mObjIndividualData.TIN,
                        NIN = nin,
                        MobileNumber1 = mObjIndividualData.MobileNumber1,
                        MobileNumber2 = mObjIndividualData.MobileNumber2,
                        EmailAddress1 = mObjIndividualData.EmailAddress1,
                        EmailAddress2 = mObjIndividualData.EmailAddress2,
                        BiometricDetails = mObjIndividualData.BiometricDetails,
                        TaxOfficeID = mObjIndividualData.TaxOfficeID,
                        MaritalStatusID = mObjIndividualData.MaritalStatusID,
                        PresentTaxOfficeID = mObjIndividualData.TaxOfficeID.GetValueOrDefault(),
                        NationalityID = mObjIndividualData.NationalityID.GetValueOrDefault(),
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                        EconomicActivitiesID = mObjIndividualData.EconomicActivitiesID.GetValueOrDefault(),
                        NotificationMethodID = mObjIndividualData.NotificationMethodID.GetValueOrDefault(),
                        ContactAddress = mObjIndividualData.ContactAddress,
                        Active = mObjIndividualData.Active.GetValueOrDefault(),
                    };

                    UI_FillDropDown(mObjIndividualModelView);
                    return View(mObjIndividualModelView);
                }
                else
                {
                    return RedirectToAction("Search", "CaptureIndividual");
                }
            }
            else
            {
                return RedirectToAction("Search", "CaptureIndividual");
            }
        }
        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult EditTaxOffice(IndividualViewModel p)
        {
            int ret = 0;
            Individual det = new Individual();
            if (p.NewTaxOfficeID == 0)
            {
                FlashMessage.Info("Select New Tax Office");
                UI_FillDropDown(p);
                return View(p);
            }
            try
            {
                using (var db = new EIRSEntities())
                {
                    det = db.Individuals.FirstOrDefault(o => o.IndividualID == p.IndividualID);
                    det.TaxOfficeID = p.NewTaxOfficeID;
                    ret = db.SaveChanges();
                }
                if (ret > 0)
                {
                    FlashMessage.Info("Tax Office Updated Successfully");
                    return RedirectToAction("Details", "CaptureIndividual", new { id = det.IndividualID, name = det.IndividualRIN.ToSeoUrl() });
                }
                else
                {
                    UI_FillDropDown(p);
                    return View(p);
                }
            }
            catch (Exception ex)
            {
                Logger.SendErrorToText(ex);
                ErrorSignal.FromCurrentContext().Raise(ex);
                UI_FillDropDown(p);
                ViewBag.Message = "Error occurred while saving Individual";
                return View(p);
            }

        }
        public ActionResult Details(int? id, string name)
        {
            string url = getUrl();
            decimal? newAmount = 0;
            decimal? totalnewAmount = 0;
            decimal? totalnewAmountLateCharge = 0;
            bool itCan = new UtilityController().CheckAccess(url);
            if (!itCan) { return RedirectToAction("AccessDenied", "Utility"); }
            if (id.GetValueOrDefault() > 0)
            {
                Individual mObjIndividual = new Individual()
                {
                    IndividualID = id.GetValueOrDefault(),
                    intStatus = 1
                };

                usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(mObjIndividual);



                if (mObjIndividualData != null)
                {
                    IList<usp_GetTaxPayerBill_Result> lstTaxPayerBill = new BLAssessment().BL_GetTaxPayerBill(id.GetValueOrDefault(), (int)EnumList.TaxPayerType.Individual, 0);
                    var lstOfBillId = lstTaxPayerBill.Select(o => Convert.ToInt64(o.BillID.Value)).ToList();
                    var lstAAIID = _AdjustmentRepository.GetListOfItemId(lstOfBillId);
                    var saiid = _AdjustmentRepository.GetListOfServiceItemId(lstOfBillId);
                    //lstAAIID
                    var aaiid = lstAAIID.Select(o => o.AAIID).ToList();
                    var adjustmentResponse = _AdjustmentRepository.GetAdjustmentResponse(aaiid);
                    var lateChargeResponse = _AdjustmentRepository.GetLateChargeResponse(aaiid);
                    var lateServiceChargeResponse = _AdjustmentRepository.GetLateChargeServiceResponse();
                    var serviceResponse = _AdjustmentRepository.GetAdjustmentServiceResponse();
                    var assBill = lstTaxPayerBill.Where(o => o.BillRefNo.StartsWith("AB")).ToList();
                    foreach (var ass in assBill)
                    {
                        var naaiid = lstAAIID.Where(o => o.BillId == ass.BillID.Value).ToList();
                        if (naaiid.Count > 1)
                        {
                            foreach (var aa in naaiid)
                            {
                                var lti = lateChargeResponse.Where(o => o.AAIID == aa.AAIID).Select(x => x.TotalAmount).FirstOrDefault();
                                newAmount = adjustmentResponse.Where(o => o.AAIID == aa.AAIID).Select(x => x.TotalAmount).FirstOrDefault();
                                totalnewAmount = newAmount == null ? totalnewAmount + 0 : totalnewAmount + newAmount;
                                totalnewAmountLateCharge = lti == null ? totalnewAmountLateCharge + 0 : totalnewAmountLateCharge + lti;

                            }
                        }
                        else
                        {
                            var lt = lateChargeResponse.Where(o => o.AAIID == naaiid.FirstOrDefault().AAIID).Select(x => x.TotalAmount).FirstOrDefault();
                            newAmount = adjustmentResponse.Where(o => o.AAIID == naaiid.FirstOrDefault().AAIID).Select(x => x.TotalAmount).FirstOrDefault();
                            totalnewAmount = newAmount == null ? totalnewAmount + 0 : totalnewAmount + newAmount;
                            totalnewAmountLateCharge = lt == null ? totalnewAmountLateCharge + 0 : totalnewAmountLateCharge + lt;
                        }

                        ass.BillAmount = ass.BillAmount + totalnewAmount + totalnewAmountLateCharge;
                        totalnewAmount = 0;
                        totalnewAmountLateCharge = 0;
                    }
                    var serviceBill = lstTaxPayerBill.Where(o => o.BillRefNo.StartsWith("SB")).ToList();
                    foreach (var ass in serviceBill)
                    {
                        var nsaiid = saiid.Where(o => o.ToString() == ass.BillID.Value.ToString()).ToList();
                        if (nsaiid.Count > 1)
                        {
                            foreach (var aa in nsaiid)
                            {
                                newAmount = serviceResponse.Where(o => o.SBIID == aa).Select(x => x.TotalAmount).FirstOrDefault();
                                totalnewAmount = newAmount == null ? totalnewAmount + 0 : totalnewAmount + newAmount;
                            }
                        }
                        else
                        {
                            newAmount = serviceResponse.Where(o => o.SBIID == aaiid.FirstOrDefault()).Select(x => x.TotalAmount).FirstOrDefault();
                            totalnewAmount = newAmount == null ? totalnewAmount + 0 : totalnewAmount + newAmount;
                        }
                        newAmount = newAmount.HasValue ? newAmount : 0;
                        ass.BillAmount = ass.BillAmount + newAmount;
                        totalnewAmount = 0;
                    }
                    ViewBag.TaxPayerBill = lstTaxPayerBill;
                    MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                    {
                        TaxPayerID = id.GetValueOrDefault(),
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual
                    };
                    // var totalTrans = _db.
                    IList<usp_GetTaxPayerAssetList_Result> lstTaxPayerAsset = new BLTaxPayerAsset().BL_GetTaxPayerAssetList(mObjTaxPayerAsset);
                    ViewBag.AssetList = lstTaxPayerAsset;

                    IList<usp_GetTaxPayerDocumentList_Result> lstTaxPayerDocument = new BLTaxPayerDocument().BL_GetTaxPayerDocumentList(new MAP_TaxPayer_Document()
                    {
                        TaxPayerID = id.GetValueOrDefault(),
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual
                    });
                    ViewBag.DocumentList = lstTaxPayerDocument;

                    IList<usp_GetAssessmentRuleInformation_Result> lstAssessmentRuleInformation = new BLTaxPayerAsset().BL_GetTaxPayerAssessmentRuleInformation((int)EnumList.TaxPayerType.Individual, id.GetValueOrDefault());
                    ViewBag.AssessmentRuleInformation = lstAssessmentRuleInformation;

                    IList<usp_GetTaxPayerPayment_Result> lstTaxPayerPayment = new BLSettlement().BL_GetTaxPayerPayment(id.GetValueOrDefault(), (int)EnumList.TaxPayerType.Individual);
                    ViewBag.TaxPayerPayment = lstTaxPayerPayment;

                    IList<usp_GetTaxPayerMDAService_Result> lstMDAService = new BLMDAService().BL_GetTaxPayerMDAService((int)EnumList.TaxPayerType.Individual, id.GetValueOrDefault());
                    ViewBag.MDAService = lstMDAService;

                    decimal dcPoABalance = new BLPaymentAccount().BL_GetWalletBalance((int)EnumList.TaxPayerType.Individual, id.GetValueOrDefault());
                    ViewBag.PoABalance = dcPoABalance;

                    IList<usp_GetProfileInformation_Result> lstProfileInformation = new BLTaxPayerAsset().BL_GetTaxPayerProfileInformation((int)EnumList.TaxPayerType.Individual, id.GetValueOrDefault());
                    ViewBag.ProfileInformation = lstProfileInformation;

                    IList<usp_GetTCCRequestList_Result> lstTCCRequest = new BLTCC().BL_GetTCCRequestList(new TCC_Request() { TaxPayerID = id.GetValueOrDefault() });
                    ViewBag.TCCRequestList = lstTCCRequest;

                    IList<DropDownListResult> lstYear = new List<DropDownListResult>();
                    lstYear.Add(new DropDownListResult() { id = DateTime.Now.Year - 1, text = (DateTime.Now.Year - 1).ToString() });
                    ViewBag.YearList = new SelectList(lstYear, "id", "text");
                    IList<DropDownListResult> lstYearForDropDown = new List<DropDownListResult>();
                    lstYearForDropDown.Add(new DropDownListResult() { id = DateTime.Now.Year - 1, text = (DateTime.Now.Year - 1).ToString() });
                    ViewBag.YearListlstYearForDropDown = new SelectList(lstYearForDropDown, "id", "text");
                    var nimc = _db.Individuals.Where(x => x.IndividualID == mObjIndividual.IndividualID).FirstOrDefault();
                    ViewBag.Nimc = nimc;

                    var nimcdets = _db.NINDetails.Where(x => x.NIN == nimc.NIN).FirstOrDefault();
                    ViewBag.NimcDet = nimcdets;

                    // The Base64 image string from the database
                    string base64Image = nimcdets?.Photo;  
                    if (!string.IsNullOrEmpty(base64Image))
                    {
                        // Construct the full image data URL
                        ViewBag.ImageSrc = $"data:image/png;base64,{base64Image}";
                    }



                    return View(mObjIndividualData);
                }
                else
                {
                    return RedirectToAction("Search", "CaptureIndividual");
                }
            }
            else
            {
                return RedirectToAction("Search", "CaptureIndividual");
            }
        }
        public ActionResult SearchBuilding(int? id, string name)
        {
            string url = getUrl();
            bool itCan = new UtilityController().CheckAccess(url);
            if (!itCan) { return RedirectToAction("AccessDenied", "Utility"); }
            if (id.GetValueOrDefault() > 0)
            {
                Individual mObjIndividual = new Individual()
                {
                    IndividualID = id.GetValueOrDefault(),
                    intStatus = 1
                };

                usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(mObjIndividual);

                if (mObjIndividualData != null)
                {
                    IndividualViewModel mObjIndividualModelView = new IndividualViewModel()
                    {
                        IndividualID = mObjIndividualData.IndividualID.GetValueOrDefault(),
                        IndividualRIN = mObjIndividualData.IndividualRIN,
                        GenderName = mObjIndividualData.GenderName,
                        TitleName = mObjIndividualData.TitleName,
                        FirstName = mObjIndividualData.FirstName,
                        LastName = mObjIndividualData.LastName,
                        MiddleName = mObjIndividualData.MiddleName,
                        DOB = mObjIndividualData.DOB.Value.ToString("dd/MM/yyyy"),
                        TIN = mObjIndividualData.TIN,
                        MobileNumber1 = mObjIndividualData.MobileNumber1,
                        MobileNumber2 = mObjIndividualData.MobileNumber2,
                        EmailAddress1 = mObjIndividualData.EmailAddress1,
                        EmailAddress2 = mObjIndividualData.EmailAddress2,
                        BiometricDetails = mObjIndividualData.BiometricDetails,
                        TaxOfficeName = mObjIndividualData.TaxOfficeName,
                        MaritalStatusName = mObjIndividualData.MaritalStatusName,
                        NationalityName = mObjIndividualData.NationalityName,
                        TaxPayerTypeName = mObjIndividualData.TaxPayerTypeName,
                        EconomicActivitiesName = mObjIndividualData.EconomicActivitiesName,
                        NotificationMethodName = mObjIndividualData.NotificationMethodName,
                        ContactAddress = mObjIndividualData.ContactAddress,
                        ActiveText = mObjIndividualData.ActiveText
                    };

                    return View(mObjIndividualModelView);
                }
                else
                {
                    return RedirectToAction("Search", "CaptureIndividual");
                }
            }
            else
            {
                return RedirectToAction("Search", "CaptureIndividual");
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
                Individual mObjIndividual = new Individual()
                {
                    IndividualID = id.GetValueOrDefault(),
                    intStatus = 1
                };

                usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(mObjIndividual);

                if (mObjIndividualData != null)
                {
                    IndividualViewModel mObjIndividualModelView = new IndividualViewModel()
                    {
                        IndividualID = mObjIndividualData.IndividualID.GetValueOrDefault(),
                        IndividualRIN = mObjIndividualData.IndividualRIN,
                        GenderName = mObjIndividualData.GenderName,
                        TitleName = mObjIndividualData.TitleName,
                        FirstName = mObjIndividualData.FirstName,
                        LastName = mObjIndividualData.LastName,
                        MiddleName = mObjIndividualData.MiddleName,
                        DOB = mObjIndividualData.DOB.Value.ToString("dd/MM/yyyy"),
                        TIN = mObjIndividualData.TIN,
                        MobileNumber1 = mObjIndividualData.MobileNumber1,
                        MobileNumber2 = mObjIndividualData.MobileNumber2,
                        EmailAddress1 = mObjIndividualData.EmailAddress1,
                        EmailAddress2 = mObjIndividualData.EmailAddress2,
                        BiometricDetails = mObjIndividualData.BiometricDetails,
                        TaxOfficeName = mObjIndividualData.TaxOfficeName,
                        MaritalStatusName = mObjIndividualData.MaritalStatusName,
                        NationalityName = mObjIndividualData.NationalityName,
                        TaxPayerTypeName = mObjIndividualData.TaxPayerTypeName,
                        EconomicActivitiesName = mObjIndividualData.EconomicActivitiesName,
                        NotificationMethodName = mObjIndividualData.NotificationMethodName,
                        ContactAddress = mObjIndividualData.ContactAddress,
                        ActiveText = mObjIndividualData.ActiveText
                    };

                    return View(mObjIndividualModelView);
                }
                else
                {
                    return RedirectToAction("Search", "CaptureIndividual");
                }
            }
            else
            {
                return RedirectToAction("Search", "CaptureIndividual");
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
            if (lstBusiness.Count() > 5)
                return PartialView("_BindBusinessTable_SingleSelect", lstBusiness.Take(5).ToList());
            else
                return PartialView("_BindBusinessTable_SingleSelect", lstBusiness.ToList());

        }
        public ActionResult SearchLand(int? id, string name)
        {
            string url = getUrl();
            bool itCan = new UtilityController().CheckAccess(url);
            if (!itCan) { return RedirectToAction("AccessDenied", "Utility"); }
            if (id.GetValueOrDefault() > 0)
            {
                Individual mObjIndividual = new Individual()
                {
                    IndividualID = id.GetValueOrDefault(),
                    intStatus = 1
                };

                usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(mObjIndividual);

                if (mObjIndividualData != null)
                {
                    IndividualViewModel mObjIndividualModelView = new IndividualViewModel()
                    {
                        IndividualID = mObjIndividualData.IndividualID.GetValueOrDefault(),
                        IndividualRIN = mObjIndividualData.IndividualRIN,
                        GenderName = mObjIndividualData.GenderName,
                        TitleName = mObjIndividualData.TitleName,
                        FirstName = mObjIndividualData.FirstName,
                        LastName = mObjIndividualData.LastName,
                        MiddleName = mObjIndividualData.MiddleName,
                        DOB = mObjIndividualData.DOB.Value.ToString("dd/MM/yyyy"),
                        TIN = mObjIndividualData.TIN,
                        MobileNumber1 = mObjIndividualData.MobileNumber1,
                        MobileNumber2 = mObjIndividualData.MobileNumber2,
                        EmailAddress1 = mObjIndividualData.EmailAddress1,
                        EmailAddress2 = mObjIndividualData.EmailAddress2,
                        BiometricDetails = mObjIndividualData.BiometricDetails,
                        TaxOfficeName = mObjIndividualData.TaxOfficeName,
                        MaritalStatusName = mObjIndividualData.MaritalStatusName,
                        NationalityName = mObjIndividualData.NationalityName,
                        TaxPayerTypeName = mObjIndividualData.TaxPayerTypeName,
                        EconomicActivitiesName = mObjIndividualData.EconomicActivitiesName,
                        NotificationMethodName = mObjIndividualData.NotificationMethodName,
                        ContactAddress = mObjIndividualData.ContactAddress,
                        ActiveText = mObjIndividualData.ActiveText
                    };

                    return View(mObjIndividualModelView);
                }
                else
                {
                    return RedirectToAction("Search", "CaptureIndividual");
                }
            }
            else
            {
                return RedirectToAction("Search", "CaptureIndividual");
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
                Individual mObjIndividual = new Individual()
                {
                    IndividualID = id.GetValueOrDefault(),
                    intStatus = 1
                };

                usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(mObjIndividual);

                if (mObjIndividualData != null)
                {
                    IndividualViewModel mObjIndividualModelView = new IndividualViewModel()
                    {
                        IndividualID = mObjIndividualData.IndividualID.GetValueOrDefault(),
                        IndividualRIN = mObjIndividualData.IndividualRIN,
                        GenderName = mObjIndividualData.GenderName,
                        TitleName = mObjIndividualData.TitleName,
                        FirstName = mObjIndividualData.FirstName,
                        LastName = mObjIndividualData.LastName,
                        MiddleName = mObjIndividualData.MiddleName,
                        DOB = mObjIndividualData.DOB.Value.ToString("dd/MM/yyyy"),
                        TIN = mObjIndividualData.TIN,
                        MobileNumber1 = mObjIndividualData.MobileNumber1,
                        MobileNumber2 = mObjIndividualData.MobileNumber2,
                        EmailAddress1 = mObjIndividualData.EmailAddress1,
                        EmailAddress2 = mObjIndividualData.EmailAddress2,
                        BiometricDetails = mObjIndividualData.BiometricDetails,
                        TaxOfficeName = mObjIndividualData.TaxOfficeName,
                        MaritalStatusName = mObjIndividualData.MaritalStatusName,
                        NationalityName = mObjIndividualData.NationalityName,
                        TaxPayerTypeName = mObjIndividualData.TaxPayerTypeName,
                        EconomicActivitiesName = mObjIndividualData.EconomicActivitiesName,
                        NotificationMethodName = mObjIndividualData.NotificationMethodName,
                        ContactAddress = mObjIndividualData.ContactAddress,
                        ActiveText = mObjIndividualData.ActiveText
                    };

                    return View(mObjIndividualModelView);
                }
                else
                {
                    return RedirectToAction("Search", "CaptureIndividual");
                }
            }
            else
            {
                return RedirectToAction("Search", "CaptureIndividual");
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
                pObjBusinessViewModel.AssetTypeID = (int)EnumList.AssetTypes.Business;
            else if (pObjBusinessViewModel == null)
                pObjBusinessViewModel = new TPBusinessViewModel();

            UI_FillTaxPayerRoleDropDown(new TaxPayer_Roles() { TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual, AssetTypeID = (int)EnumList.AssetTypes.Business });
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
                Individual mObjIndividual = new Individual()
                {
                    IndividualID = id.GetValueOrDefault(),
                    intStatus = 1
                };

                usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(mObjIndividual);

                if (mObjIndividualData != null)
                {
                    TPBusinessViewModel mObjBusinessModel = new TPBusinessViewModel()
                    {
                        TaxPayerID = mObjIndividualData.IndividualID.GetValueOrDefault(),
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                        TaxPayerTypeName = mObjIndividualData.TaxPayerTypeName,
                        TaxPayerRIN = mObjIndividualData.IndividualRIN,
                        TaxPayerTIN = mObjIndividualData.TIN,
                        TaxPayerName = mObjIndividualData.FirstName + " " + mObjIndividualData.LastName,
                        MobileNumber = mObjIndividualData.MobileNumber1,
                        ContactAddress = mObjIndividualData.ContactAddress,
                        AssetTypeID = (int)EnumList.AssetTypes.Business,
                        ZoneId = 0,
                        TaxOfficeId = 0
                    };

                    UI_FillBusinessDropDown(mObjBusinessModel);

                    return View(mObjBusinessModel);
                }
                else
                {
                    return RedirectToAction("Search", "CaptureIndividual");
                }
            }
            else
            {
                return RedirectToAction("Search", "CaptureIndividual");
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
                                usp_GetTaxPayerAssetList_Result mObjTaxPayerAssetData = (usp_GetTaxPayerAssetList_Result)mObjTPResponse.AdditionalData;

                                if (mObjTaxPayerAssetData != null && GlobalDefaultValues.SendNotification)
                                {
                                    //Send Notification
                                    EmailDetails mObjEmailDetails = new EmailDetails()
                                    {
                                        TaxPayerTypeID = mObjTaxPayerAssetData.TaxPayerTypeID.GetValueOrDefault(),
                                        TaxPayerTypeName = mObjTaxPayerAssetData.TaxPayerTypeName,
                                        TaxPayerID = mObjTaxPayerAssetData.TaxPayerID.GetValueOrDefault(),
                                        TaxPayerName = mObjTaxPayerAssetData.TaxPayerName,
                                        TaxPayerRIN = mObjTaxPayerAssetData.TaxPayerRINNumber,
                                        TaxPayerRoleName = mObjTaxPayerAssetData.TaxPayerRoleName,
                                        AssetName = mObjTaxPayerAssetData.AssetName,
                                        AssetLGA = mObjTaxPayerAssetData.AssetLGA,
                                        AssetRIN = mObjTaxPayerAssetData.AssetRIN,
                                        AssetTypeName = mObjTaxPayerAssetData.AssetTypeName,
                                        TaxPayerMobileNumber = mObjTaxPayerAssetData.TaxPayerMobileNumber,
                                        TaxPayerEmail = mObjTaxPayerAssetData.TaxPayerEmailAddress,
                                        LoggedInUserID = SessionManager.UserID,
                                    };

                                    if (!string.IsNullOrWhiteSpace(mObjTaxPayerAssetData.TaxPayerEmailAddress))
                                    {
                                        BLEmailHandler.BL_AssetProfileLinked(mObjEmailDetails);
                                    }

                                    if (!string.IsNullOrWhiteSpace(mObjTaxPayerAssetData.TaxPayerMobileNumber))
                                    {
                                        UtilityController.BL_AssetProfileLinked(mObjEmailDetails);
                                    }
                                }
                                mObjScope.Complete();
                                FlashMessage.Info("Business Created Successfully and Linked to Tax Payer");
                                return RedirectToAction("Details", "CaptureIndividual", new { id = pObjBusinessModel.TaxPayerID, name = pObjBusinessModel.TaxPayerRIN });
                            }
                            else
                            {
                                throw new Exception(mObjResponse.Message);
                            }
                        }
                        else
                        {
                            UI_FillBusinessDropDown(pObjBusinessModel);
                            Transaction.Current.Rollback();
                            ViewBag.Message = mObjResponse.Message;
                            return View(pObjBusinessModel);
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

        public ActionResult EditBusiness(int indid, int busid, long tpaid)
        {
            string url = getUrl();
            bool itCan = new UtilityController().CheckAccess(url);
            if (!itCan) { return RedirectToAction("AccessDenied", "Utility"); }
            if (indid > 0)
            {
                Individual mObjIndividual = new Individual()
                {
                    IndividualID = indid,
                    intStatus = 1
                };

                usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(mObjIndividual);

                if (mObjIndividualData != null)
                {
                    Business mObjBusiness = new Business()
                    {
                        BusinessID = busid,
                        intStatus = 2
                    };

                    usp_GetBusinessListNewTy_Result mObjBusinessData = new BLBusiness().BL_GetBusinessDetails(mObjBusiness);

                    if (mObjBusinessData != null)
                    {
                        MAP_TaxPayer_Asset mObjTaxPayerAssetData = new BLTaxPayerAsset().BL_GetTaxPayerAssetDetails(tpaid);
                        TPBusinessViewModel mObjBusinessModel = new TPBusinessViewModel()
                        {
                            TaxPayerID = mObjIndividualData.IndividualID.GetValueOrDefault(),
                            TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                            TaxPayerTypeName = mObjIndividualData.TaxPayerTypeName,
                            TaxPayerRIN = mObjIndividualData.IndividualRIN,
                            TaxPayerTIN = mObjIndividualData.TIN,
                            TaxPayerName = mObjIndividualData.FirstName + " " + mObjIndividualData.LastName,
                            MobileNumber = mObjIndividualData.MobileNumber1,
                            ContactAddress = mObjIndividualData.ContactAddress,
                            AssetTypeID = (int)EnumList.AssetTypes.Business,
                            BusinessID = mObjBusinessData.BusinessID.GetValueOrDefault(),
                            BusinessRIN = mObjBusinessData.BusinessRIN,
                            BusinessTypeID = mObjBusinessData.BusinessTypeID.GetValueOrDefault(),
                            BusinessName = mObjBusinessData.BusinessName,
                            LGAID = mObjBusinessData.LGAID.GetValueOrDefault(),
                            BusinessCategoryID = mObjBusinessData.BusinessCategoryID.GetValueOrDefault(),
                            BusinessSectorID = mObjBusinessData.BusinessSectorID.GetValueOrDefault(),
                            BusinessSubSectorID = mObjBusinessData.BusinessSubSectorID.GetValueOrDefault(),
                            BusinessStructureID = mObjBusinessData.BusinessStructureID.GetValueOrDefault(),
                            BusinessOperationID = mObjBusinessData.BusinessOperationID.GetValueOrDefault(),
                            SizeID = mObjBusinessData.SizeID.GetValueOrDefault(),
                            Active = mObjBusinessData.Active.GetValueOrDefault(),
                            ContactName = mObjBusinessData.ContactName,
                            BusinessAddress = mObjBusinessData.BusinessAddress,
                            BusinessNumber = mObjBusinessData.BusinessNumber,
                            TaxPayerRoleID = mObjTaxPayerAssetData.TaxPayerRoleID.GetValueOrDefault(),
                            TPAID = mObjTaxPayerAssetData.TPAID
                        };

                        UI_FillBusinessDropDown(mObjBusinessModel);

                        return View(mObjBusinessModel);
                    }
                    else
                    {
                        return RedirectToAction("Search", "CaptureIndividual");
                    }
                }
                else
                {
                    return RedirectToAction("Search", "CaptureIndividual");
                }
            }
            else
            {
                return RedirectToAction("Search", "CaptureIndividual");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult EditBusiness(TPBusinessViewModel pObjBusinessModel)
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
                            BusinessID = pObjBusinessModel.BusinessID,
                            AssetTypeID = (int)EnumList.AssetTypes.Business,
                            BusinessTypeID = pObjBusinessModel.BusinessTypeID,
                            BusinessName = pObjBusinessModel.BusinessName,
                            LGAID = pObjBusinessModel.LGAID,
                            BusinessCategoryID = pObjBusinessModel.BusinessCategoryID,
                            BusinessSectorID = pObjBusinessModel.BusinessSectorID,
                            BusinessSubSectorID = pObjBusinessModel.BusinessSubSectorID,
                            BusinessStructureID = pObjBusinessModel.BusinessStructureID,
                            BusinessOperationID = pObjBusinessModel.BusinessOperationID,
                            SizeID = pObjBusinessModel.SizeID,
                            ContactName = pObjBusinessModel.ContactName,
                            BusinessAddress = pObjBusinessModel.BusinessAddress,
                            BusinessNumber = pObjBusinessModel.BusinessNumber,
                            Active = pObjBusinessModel.Active,
                            ModifiedBy = SessionManager.UserID,
                            ModifiedDate = CommUtil.GetCurrentDateTime()
                        };

                        FuncResponse<Business> mObjResponse = new BLBusiness().BL_InsertUpdateBusiness(mObjBusiness);

                        if (mObjResponse.Success)
                        {
                            //Creating mapping between tax payer and business
                            MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                            {
                                TPAID = pObjBusinessModel.TPAID,
                                AssetTypeID = (int)EnumList.AssetTypes.Business,
                                AssetID = mObjResponse.AdditionalData.BusinessID,
                                TaxPayerTypeID = pObjBusinessModel.TaxPayerTypeID,
                                TaxPayerRoleID = pObjBusinessModel.TaxPayerRoleID,
                                TaxPayerID = pObjBusinessModel.TaxPayerID,
                                Active = true,
                                ModifiedBy = SessionManager.UserID,
                                ModifiedDate = CommUtil.GetCurrentDateTime()
                            };

                            FuncResponse mObjTPResponse = new BLTaxPayerAsset().BL_UpdateTaxPayerAsset(mObjTaxPayerAsset);

                            if (mObjTPResponse.Success)
                            {
                                mObjScope.Complete();
                                FlashMessage.Info("Business Updated Successfully");
                                return RedirectToAction("Details", "CaptureIndividual", new { id = pObjBusinessModel.TaxPayerID, name = pObjBusinessModel.TaxPayerRIN });
                            }
                            else
                            {
                                throw new Exception(mObjResponse.Message);
                            }
                        }
                        else
                        {
                            UI_FillBusinessDropDown(pObjBusinessModel);
                            Transaction.Current.Rollback();
                            ViewBag.Message = mObjResponse.Message;
                            return View(pObjBusinessModel);
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
                pObjLandViewModel.AssetTypeID = (int)EnumList.AssetTypes.Land;
            else if (pObjLandViewModel == null)
                pObjLandViewModel = new TPLandViewModel();

            UI_FillTaxPayerRoleDropDown(new TaxPayer_Roles() { TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual, AssetTypeID = (int)EnumList.AssetTypes.Land });
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
                Individual mObjIndividual = new Individual()
                {
                    IndividualID = id.GetValueOrDefault(),
                    intStatus = 1
                };

                usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(mObjIndividual);

                if (mObjIndividualData != null)
                {
                    TPLandViewModel mObjLandModel = new TPLandViewModel()
                    {
                        TaxPayerID = mObjIndividualData.IndividualID.GetValueOrDefault(),
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                        TaxPayerTypeName = mObjIndividualData.TaxPayerTypeName,
                        TaxPayerRIN = mObjIndividualData.IndividualRIN,
                        TaxPayerTIN = mObjIndividualData.TIN,
                        TaxPayerName = mObjIndividualData.FirstName + " " + mObjIndividualData.LastName,
                        MobileNumber = mObjIndividualData.MobileNumber1,
                        ContactAddress = mObjIndividualData.ContactAddress,
                        AssetTypeID = (int)EnumList.AssetTypes.Land,
                    };

                    UI_FillLandDropDown();

                    return View(mObjLandModel);
                }
                else
                {
                    return RedirectToAction("Search", "CaptureIndividual");
                }
            }
            else
            {
                return RedirectToAction("Search", "CaptureIndividual");
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
                                //}
                                mObjScope.Complete();
                                FlashMessage.Info("Land Created Successfully and Linked to Tax Payer");
                                return RedirectToAction("Details", "CaptureIndividual", new { id = pObjLandModel.TaxPayerID, name = pObjLandModel.TaxPayerRIN });
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
                pObjVehicleViewModel.AssetTypeID = (int)EnumList.AssetTypes.Vehicles;
            else if (pObjVehicleViewModel == null)
                pObjVehicleViewModel = new VehicleViewModel();

            UI_FillTaxPayerRoleDropDown(new TaxPayer_Roles() { TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual, AssetTypeID = (int)EnumList.AssetTypes.Vehicles });
            UI_FillLGADropDown(new LGA() { intStatus = 1, IncludeLGAIds = pObjVehicleViewModel.LGAID.ToString() });
            UI_FillAssetTypeDropDown(new Asset_Types() { intStatus = 1, IncludeAssetTypeIds = pObjVehicleViewModel.AssetTypeID.ToString() }, (int)EnumList.AssetTypes.Vehicles);
            UI_FillVehicleTypeDropDown(new Vehicle_Types() { intStatus = 1, IncludeVehicleTypeIds = pObjVehicleViewModel.VehicleTypeID.ToString() });
            UI_FillVehicleSubTypeDropDown(new Vehicle_SubTypes() { intStatus = 1, IncludeVehicleSubTypeIds = pObjVehicleViewModel.VehicleSubTypeID.ToString(), VehicleTypeID = pObjVehicleViewModel.VehicleTypeID });
            UI_FillVehiclePurposeDropDown(new Vehicle_Purpose() { intStatus = 1, IncludeVehiclePurposeIds = pObjVehicleViewModel.VehiclePurposeID.ToString() });
            UI_FillVehicleFunctionDropDown(new Vehicle_Function() { intStatus = 1, IncludeVehicleFunctionIds = pObjVehicleViewModel.VehicleFunctionID.ToString(), VehiclePurposeID = pObjVehicleViewModel.VehiclePurposeID });
            UI_FillVehicleOwnershipDropDown(new Vehicle_Ownership() { intStatus = 1, IncludeVehicleOwnershipIds = pObjVehicleViewModel.VehicleOwnershipID.ToString() });
        }
        public ActionResult AddVehicle(int? id, string name)
        {
            string url = getUrl();
            bool itCan = new UtilityController().CheckAccess(url);
            if (!itCan) { return RedirectToAction("AccessDenied", "Utility"); }
            if (id.GetValueOrDefault() > 0)
            {
                Individual mObjIndividual = new Individual()
                {
                    IndividualID = id.GetValueOrDefault(),
                    intStatus = 1
                };

                usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(mObjIndividual);

                if (mObjIndividualData != null)
                {
                    TPVehicleViewModel mObjVehicleModel = new TPVehicleViewModel()
                    {
                        TaxPayerID = mObjIndividualData.IndividualID.GetValueOrDefault(),
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                        TaxPayerTypeName = mObjIndividualData.TaxPayerTypeName,
                        TaxPayerRIN = mObjIndividualData.IndividualRIN,
                        TaxPayerTIN = mObjIndividualData.TIN,
                        TaxPayerName = mObjIndividualData.FirstName + " " + mObjIndividualData.LastName,
                        MobileNumber = mObjIndividualData.MobileNumber1,
                        ContactAddress = mObjIndividualData.ContactAddress,
                        AssetTypeID = (int)EnumList.AssetTypes.Vehicles,

                    };

                    UI_FillVehicleDropDown(mObjVehicleModel);

                    return View(mObjVehicleModel);
                }
                else
                {
                    return RedirectToAction("Search", "CaptureIndividual");
                }
            }
            else
            {
                return RedirectToAction("Search", "CaptureIndividual");
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
                                return RedirectToAction("Details", "CaptureIndividual", new { id = pObjVehicleModel.TaxPayerID, name = pObjVehicleModel.TaxPayerRIN });
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
                pObjBuildingViewModel.AssetTypeID = (int)EnumList.AssetTypes.Building;
            else if (pObjBuildingViewModel == null)
                pObjBuildingViewModel = new TPBuildingViewModel();

            UI_FillTaxPayerRoleDropDown(new TaxPayer_Roles() { TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual, AssetTypeID = (int)EnumList.AssetTypes.Building });
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
                Individual mObjIndividual = new Individual()
                {
                    IndividualID = id.GetValueOrDefault(),
                    intStatus = 1
                };

                usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(mObjIndividual);

                if (mObjIndividualData != null)
                {
                    TPBuildingViewModel mObjBuildingModel = new TPBuildingViewModel()
                    {
                        TaxPayerID = mObjIndividualData.IndividualID.GetValueOrDefault(),
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                        TaxPayerTypeName = mObjIndividualData.TaxPayerTypeName,
                        TaxPayerRIN = mObjIndividualData.IndividualRIN,
                        TaxPayerTIN = mObjIndividualData.TIN,
                        TaxPayerName = mObjIndividualData.FirstName.Trim() + " " + mObjIndividualData.LastName,
                        MobileNumber = mObjIndividualData.MobileNumber1,
                        ContactAddress = mObjIndividualData.ContactAddress,
                        AssetTypeID = (int)EnumList.AssetTypes.Building,
                    };

                    SessionManager.LstBuildingUnit = new List<Building_BuildingUnit>();

                    UI_FillBuildingDropDown();

                    return View(mObjBuildingModel);
                }
                else
                {
                    return RedirectToAction("Search", "CaptureIndividual");
                }
            }
            else
            {
                return RedirectToAction("Search", "CaptureIndividual");
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
                                    //var vExists = (from tpa in _db.MAP_TaxPayer_Asset
                                    //               join aa in _db.Asset_Types on tpa.AssetTypeID equals aa.AssetTypeID
                                    //               join tp in _db.TaxPayer_Roles on tpa.TaxPayerRoleID equals tp.TaxPayerRoleID
                                    //               join tpx in _db.TaxPayer_Types on tpa.TaxPayerTypeID equals tpx.TaxPayerTypeID
                                    //               join idd in _db.Individuals on tpa.TaxPayerID equals idd.IndividualID
                                    //               where tpa.AssetTypeID == mObjTaxPayerAsset.AssetTypeID && tpa.AssetID == mObjTaxPayerAsset.AssetID
                                    //                  && tpa.TaxPayerTypeID == mObjTaxPayerAsset.TaxPayerTypeID && tpa.TaxPayerID == mObjTaxPayerAsset.TaxPayerID
                                    //                  && tpa.TaxPayerRoleID == mObjTaxPayerAsset.TaxPayerRoleID && tpa.Active == true

                                    //               select new { Post = tpa, Meta = tp, All = tpx, Idd = idd,Aa = aa }).FirstOrDefault();

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
                                    FlashMessage.Info("Building Created Successfully and Linked to Tax Payer");
                                    return RedirectToAction("Details", "CaptureIndividual", new { id = pObjBuildingModel.TaxPayerID, name = pObjBuildingModel.TaxPayerRIN });
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
                        catch (Exception Ex)
                        {
                            ErrorSignal.FromCurrentContext().Raise(Ex);
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
            List<string> keys = new List<string>();

            if (id.GetValueOrDefault() > 0)
            {
                Individual mObjIndividual = new Individual()
                {
                    IndividualID = id.GetValueOrDefault(),
                    intStatus = 1
                };

                usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(mObjIndividual);

                if (mObjIndividualData != null)
                {
                    GenerateAssessmentViewModel mObjGenerateAssessmentModel = new GenerateAssessmentViewModel()
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

                    int doneby = SessionManager.UserID;
                    MAP_Assessment_AssessmentRule neededRule = new MAP_Assessment_AssessmentRule();
                    List<MAP_Assessment_AssessmentRule> neededRules = new List<MAP_Assessment_AssessmentRule>();
                    IList<usp_GetAssessmentRuleForAssessment_Result> lstAssessmentRules = new BLTaxPayerAsset().BL_GetTaxPayerAssessmentRuleForAssessment((int)EnumList.TaxPayerType.Individual, id.GetValueOrDefault());
                    List<usp_GetAssessmentRuleForAssessment_Result> lAR = new List<usp_GetAssessmentRuleForAssessment_Result>();

                    //foreach (var ret in lstAssessmentRules)
                    //{
                    //    using (_db = new EIRSEntities())
                    //    {
                    //        decimal? needAmount = _db.Assessment_Rules.FirstOrDefault(o => o.AssessmentRuleID == ret.AssessmentRuleID).AssessmentAmount;
                    //        neededRule = _db.MAP_Assessment_AssessmentRule.FirstOrDefault(o => o.AssessmentRuleID == ret.AssessmentRuleID && o.CreatedBy == doneby && o.AssessmentAmount == needAmount);
                    //        if (neededRule == null)
                    //        {
                    //            lAR.Add(ret);
                    //        }
                    //        else
                    //        {

                    //        }
                    //    }
                    //}

                    ViewBag.AssessmentRuleInformation = lstAssessmentRules;

                    return View(mObjGenerateAssessmentModel);
                }
                else
                {
                    return RedirectToAction("Search", "CaptureIndividual");
                }
            }
            else
            {
                return RedirectToAction("Search", "CaptureIndividual");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult GenerateAssessment(GenerateAssessmentViewModel pObjGenerateAssessmentModel)
        {
            if (!ModelState.IsValid)
            {
                IList<usp_GetAssessmentRuleForAssessment_Result> lstAssessmentRules = new BLTaxPayerAsset().BL_GetTaxPayerAssessmentRuleForAssessment((int)EnumList.TaxPayerType.Individual, pObjGenerateAssessmentModel.TaxPayerID);
                ViewBag.AssessmentRuleInformation = lstAssessmentRules;

                return View(pObjGenerateAssessmentModel);
            }
            else
            {
                return RedirectToAction("AddAssessment", new { id = pObjGenerateAssessmentModel.TaxPayerID, name = pObjGenerateAssessmentModel.TaxPayerRIN, aruleIds = pObjGenerateAssessmentModel.AssessmentRuleId });
            }
        }

        public ActionResult GenerateServiceBill(int? id, string name)
        {
            string url = getUrl();
            bool itCan = new UtilityController().CheckAccess(url);
            if (!itCan) { return RedirectToAction("AccessDenied", "Utility"); }
            if (id.GetValueOrDefault() > 0)
            {
                Individual mObjIndividual = new Individual()
                {
                    IndividualID = id.GetValueOrDefault(),
                    intStatus = 1
                };

                usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(mObjIndividual);

                if (mObjIndividualData != null)
                {
                    GenerateServiceBillViewModel mObjGenerateServiceBillModel = new GenerateServiceBillViewModel()
                    {
                        TaxPayerID = mObjIndividualData.IndividualID.GetValueOrDefault(),
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                        TaxPayerTypeName = mObjIndividualData.TaxPayerTypeName,
                        TaxPayerRIN = mObjIndividualData.IndividualRIN,
                        TaxPayerTIN = mObjIndividualData.TIN,
                        TaxPayerName = mObjIndividualData.FirstName + " " + mObjIndividualData.LastName,
                        ContactNumber = mObjIndividualData.MobileNumber1,
                        ContactAddress = mObjIndividualData.ContactAddress,
                        //MDAServiceIds = mObjIndividualData.
                    };

                    IList<usp_GetMDAServiceForServiceBill_Result> lstMDAService = new BLMDAService().BL_GetMDAServiceForServiceBill((int)EnumList.TaxPayerType.Individual, id.GetValueOrDefault());
                    ViewBag.MDAServiceInformation = lstMDAService;

                    return View(mObjGenerateServiceBillModel);
                }
                else
                {
                    return RedirectToAction("Search", "CaptureIndividual");
                }
            }
            else
            {
                return RedirectToAction("Search", "CaptureIndividual");
            }
        }

        public JsonResult UpdateAssetStatus(MAP_TaxPayer_Asset pObjAssetData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjAssetData.TPAID != 0)
            {
                pObjAssetData.TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual;
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

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult GenerateServiceBill(GenerateServiceBillViewModel pObjGenerateServiceBillModel)
        {
            if (!ModelState.IsValid)
            {
                IList<usp_GetMDAServiceForServiceBill_Result> lstMDAService = new BLMDAService().BL_GetMDAServiceForServiceBill((int)EnumList.TaxPayerType.Individual, pObjGenerateServiceBillModel.TaxPayerID);
                ViewBag.MDAServiceInformation = lstMDAService;

                return View(pObjGenerateServiceBillModel);
            }
            else
            {
                return RedirectToAction("AddServiceBill", new { id = pObjGenerateServiceBillModel.TaxPayerID, name = pObjGenerateServiceBillModel.TaxPayerRIN, mdsIds = pObjGenerateServiceBillModel.MDAServiceIds });
            }
        }
        //had to add the new model here since i dont have controll over the eirs.models

        public ActionResult AddAssessment(int? id, string name, string aruleIds)
        {
            string url = getUrl();
            bool itCan = new UtilityController().CheckAccess(url);
            if (!itCan) { return RedirectToAction("AccessDenied", "Utility"); }
            if (id.GetValueOrDefault() > 0)
            {
                Individual mObjIndividual = new Individual()
                {
                    IndividualID = id.GetValueOrDefault(),
                    intStatus = 1
                };

                usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(mObjIndividual);

                if (mObjIndividualData != null)
                {
                    NewAssessmentViewModel mObjAssessmentModel = new NewAssessmentViewModel()
                    {
                        TaxPayerID = mObjIndividualData.IndividualID.GetValueOrDefault(),
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                        TaxPayerRIN = mObjIndividualData.IndividualRIN,
                        TaxPayerName = mObjIndividualData.FirstName + " " + mObjIndividualData.LastName,
                        TaxPayerAddress = mObjIndividualData.ContactAddress,
                        SettlementDuedate = DateTime.Now,
                        AssessmentDate = DateTime.Now
                    };

                    IList<Assessment_AssessmentRule> lstAssessmentRules = new List<Assessment_AssessmentRule>();
                    IList<Assessment_AssessmentItem> lstAssessmentItems = new List<Assessment_AssessmentItem>();
                    IList<usp_GetAssessmentRuleForAssessment_Result> newlstAssessmentRules = new BLTaxPayerAsset().BL_GetTaxPayerAssessmentRuleForAssessment((int)EnumList.TaxPayerType.Individual, id.GetValueOrDefault());

                    BLAssessmentRule mObjBLAssessmentRule = new BLAssessmentRule();
                    BLAssessmentItem mObjBLAssessmentItem = new BLAssessmentItem();

                    string strAssessmentRuleIds = Decrypt(aruleIds);

                    List<string> strArrAssessmentRuleIds = new List<string>();
                    List<string> strArrAssestIds = new List<string>();

                    //string[] strArrAssessmentRuleIds = strAssessmentRuleIds.Split(',');
                    if (aruleIds.Contains("{"))
                    {
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        var assBillIds = js.Deserialize<List<newServiceBillIdsRequest>>(aruleIds);
                        //  var assBillIds = (List<newServiceBillIdsRequest>)js.DeserializeObject(aruleIds);
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
                    ViewBag.AssessmentItemList = lstAssessmentItems.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();

                    return View(mObjAssessmentModel);
                }
                else
                {
                    return RedirectToAction("Search", "CaptureIndividual");
                }
            }
            else
            {
                return RedirectToAction("Search", "CaptureIndividual");
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
                // IList<Assessment_AssessmentItem> lstAssessmentItems = new List<Assessment_AssessmentItem>();
                IList<Assessment_AssessmentItem> lstAssessmentItems = SessionManager.lstAssessmentItem ?? new List<Assessment_AssessmentItem>();
                //IList<Assessment_AssessmentRule> lstAssessmentRules = new List<Assessment_AssessmentRule>();
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

                        var lstProfileId = lstAssessmentRules.Select(o => o.ProfileID).ToList();
                        var lstOfProfiles = _db.Profiles.Where(o => lstProfileId.Contains(o.ProfileID)).Select(o => o.ProfileTypeID).ToList();
                        lstOfProfiles = lstOfProfiles.Where(o => o.Value.Equals(5) || o.Value.Equals(7)).ToList();
                        Assessment mObjAssessment = new Assessment()
                        {
                            AssessmentID = 0,
                            TaxPayerID = pObjAssessmentModel.TaxPayerID,
                            TaxPayerTypeID = pObjAssessmentModel.TaxPayerTypeID,
                            AssessmentAmount = lstAssessmentRules.Count > 0 ? lstAssessmentItems.Sum(t => t.TaxBaseAmount) : 0,
                            AssessmentDate = CommUtil.GetCurrentDateTime(),
                            SettlementDueDate = pObjAssessmentModel.SettlementDuedate,
                            SettlementStatusID = lstOfProfiles.Count > 0 ? (int)EnumList.SettlementStatus.PendingApproval : (int)EnumList.SettlementStatus.Assessed,
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
                                        AssessmentAmount = lstAssessmentRules.Count > 0 ? lstAssessmentItems.Where(o => o.AssessmentRule_RowID == mObjAAR.RowID).Sum(t => t.TaxBaseAmount) : 0,
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
                                }



                                //Send Notification
                                if (GlobalDefaultValues.SendNotification)
                                {
                                    usp_GetAssessmentList_Result mObjAssessmentData = mObjBLAssessment.BL_GetAssessmentDetails(new Assessment() { AssessmentID = mObjAssessmentResponse.AdditionalData.AssessmentID, IntStatus = 2 });
                                    usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(new Individual() { IndividualID = pObjAssessmentModel.TaxPayerID, intStatus = 1 });
                                    IList<usp_GetAssessment_AssessmentRuleList_Result> lstMAPAssessmentRules = mObjBLAssessment.BL_GetAssessmentRules(mObjAssessmentResponse.AdditionalData.AssessmentID);

                                    string AssessmentRuleNames = string.Join(",", lstMAPAssessmentRules.Select(t => t.AssessmentRuleName).ToArray());
                                    if (mObjIndividualData != null && mObjAssessmentData != null)
                                    {
                                        EmailDetails mObjEmailDetails = new EmailDetails()
                                        {
                                            TaxPayerTypeID = mObjIndividualData.TaxPayerTypeID.GetValueOrDefault(),
                                            TaxPayerTypeName = mObjIndividualData.TaxPayerTypeName,
                                            TaxPayerID = mObjIndividualData.IndividualID.GetValueOrDefault(),
                                            TaxPayerName = mObjIndividualData.FirstName + " " + mObjIndividualData.LastName,
                                            TaxPayerRIN = mObjIndividualData.IndividualRIN,
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
                                return RedirectToAction("Details", "CaptureIndividual", new { id = pObjAssessmentModel.TaxPayerID, name = pObjAssessmentModel.TaxPayerRIN });

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
        public ActionResult EditAssessment(int? id, string name, int? aid)
        {
            if (id.GetValueOrDefault() > 0 && aid.GetValueOrDefault() > 0)
            {
                Individual mObjIndividual = new Individual()
                {
                    IndividualID = id.GetValueOrDefault(),
                    intStatus = 1
                };

                usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(mObjIndividual);

                BLAssessment mObjBLAssessment = new BLAssessment();
                BLAssessmentItem mObjBLAssessmentItem = new BLAssessmentItem();
                usp_GetAssessmentList_Result mObjAssessmentData = mObjBLAssessment.BL_GetAssessmentDetails(new Assessment() { AssessmentID = aid.GetValueOrDefault(), IntStatus = 2 });

                if (mObjIndividualData != null && mObjAssessmentData != null)
                {
                    NewAssessmentViewModel mObjAssessmentModel = new NewAssessmentViewModel()
                    {
                        TaxPayerID = mObjIndividualData.IndividualID.GetValueOrDefault(),
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                        TaxPayerRIN = mObjIndividualData.IndividualRIN,
                        TaxPayerName = mObjIndividualData.FirstName + " " + mObjIndividualData.LastName,
                        TaxPayerAddress = mObjIndividualData.ContactAddress,
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
                    return RedirectToAction("Search", "CaptureIndividual");
                }
            }
            else
            {
                return RedirectToAction("Search", "CaptureIndividual");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult EditAssessment(NewAssessmentViewModel pObjAssessmentModel)
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

                            FuncResponse<Assessment> mObjAssessmentResponse = _AssessmentRepository.REP_InsertUpdateAssessment(mObjAssessment, 0);

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
                                    usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(new Individual() { IndividualID = pObjAssessmentModel.TaxPayerID, intStatus = 1 });
                                    IList<usp_GetAssessment_AssessmentRuleList_Result> lstMAPAssessmentRules = mObjBLAssessment.BL_GetAssessmentRules(mObjAssessmentResponse.AdditionalData.AssessmentID);

                                    string AssessmentRuleNames = string.Join(",", lstMAPAssessmentRules.Select(t => t.AssessmentRuleName).ToArray());
                                    if (mObjIndividualData != null && mObjAssessmentData != null)
                                    {
                                        EmailDetails mObjEmailDetails = new EmailDetails()
                                        {
                                            TaxPayerTypeID = mObjIndividualData.TaxPayerTypeID.GetValueOrDefault(),
                                            TaxPayerTypeName = mObjIndividualData.TaxPayerTypeName,
                                            TaxPayerID = mObjIndividualData.IndividualID.GetValueOrDefault(),
                                            TaxPayerName = mObjIndividualData.FirstName + " " + mObjIndividualData.LastName,
                                            TaxPayerRIN = mObjIndividualData.IndividualRIN,
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

                                Audit_Log mObjAuditLog = new Audit_Log()
                                {
                                    LogDate = CommUtil.GetCurrentDateTime(),
                                    ASLID = (int)EnumList.ALScreen.Capture_Individual_Edit_Assessment,
                                    Comment = $"Assessment Bill Updated - {mObjAssessmentResponse.AdditionalData.AssessmentRefNo}",
                                    IPAddress = CommUtil.GetIPAddress(),
                                    StaffID = SessionManager.UserID,
                                };

                                new BLAuditLog().BL_InsertAuditLog(mObjAuditLog);

                                scope.Complete();
                                FlashMessage.Info(mObjAssessmentResponse.Message);
                                return RedirectToAction("Details", "CaptureIndividual", new { id = pObjAssessmentModel.TaxPayerID, name = pObjAssessmentModel.TaxPayerRIN });
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
                                ViewBag.Message = "Error occurred while updating assessment";
                            }

                            Transaction.Current.Rollback();
                            return View(pObjAssessmentModel);
                        }

                    }
                }
            }
        }
        public ActionResult AddServiceBill(int? id, string name, string mdsIds = null)
        {
            string url = getUrl();
            bool itCan = new UtilityController().CheckAccess(url);
            if (!itCan) { return RedirectToAction("AccessDenied", "Utility"); }
            if (id.GetValueOrDefault() > 0)
            {
                Individual mObjIndividual = new Individual()
                {
                    IndividualID = id.GetValueOrDefault(),
                    intStatus = 1
                };

                usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(mObjIndividual);

                if (mObjIndividualData != null)
                {
                    ServiceBillViewModel mObjServiceBillModel = new ServiceBillViewModel()
                    {
                        TaxPayerID = mObjIndividualData.IndividualID.GetValueOrDefault(),
                        TaxPayerRIN = mObjIndividualData.IndividualRIN,
                        TaxPayerName = mObjIndividualData.FirstName + " " + mObjIndividualData.LastName,
                        TaxPayerAddress = mObjIndividualData.ContactAddress,
                        SettlementDuedate = CommUtil.GetCurrentDateTime(),
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
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
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        var serviceBillIds = js.Deserialize<List<newServiceBillIdsRequest>>(mdsIds);
                        // var serviceBillIds = (List<newServiceBillIdsRequest>)js.DeserializeObject(mdsIds);
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
                                            ServiceBaseAmount = mObjMDAServiceItemData.ServiceAmount.GetValueOrDefault(),
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
                    return RedirectToAction("Search", "CaptureIndividual");
                }
            }
            else
            {
                return RedirectToAction("Search", "CaptureIndividual");
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
                                    usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(new Individual() { IndividualID = pObjServiceBillModel.TaxPayerID, intStatus = 1 });

                                    IList<usp_GetServiceBill_MDAServiceList_Result> lstMAPServiceBillServices = mObjBLServiceBill.BL_GetMDAServiceList(mObjServiceBillData.ServiceBillID.GetValueOrDefault());

                                    string MDAServicesNames = string.Join(",", lstMAPServiceBillServices.Select(t => t.MDAServiceName).ToArray());
                                    if (mObjIndividualData != null && mObjServiceBillData != null)
                                    {
                                        EmailDetails mObjEmailDetails = new EmailDetails()
                                        {
                                            TaxPayerTypeID = mObjIndividualData.TaxPayerTypeID.GetValueOrDefault(),
                                            TaxPayerTypeName = mObjIndividualData.TaxPayerTypeName,
                                            TaxPayerID = mObjIndividualData.IndividualID.GetValueOrDefault(),
                                            TaxPayerName = mObjIndividualData.FirstName + " " + mObjIndividualData.LastName,
                                            TaxPayerRIN = mObjIndividualData.IndividualRIN,
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
                                return RedirectToAction("Details", "CaptureIndividual", new { id = pObjServiceBillModel.TaxPayerID, name = pObjServiceBillModel.TaxPayerRIN });
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
        public ActionResult EditServiceBill(int? id, string name, int? sbid)
        {
            string url = getUrl();
            bool itCan = new UtilityController().CheckAccess(url);
            if (!itCan) { return RedirectToAction("AccessDenied", "Utility"); }
            if (id.GetValueOrDefault() > 0 && sbid.GetValueOrDefault() > 0)
            {
                Individual mObjIndividual = new Individual()
                {
                    IndividualID = id.GetValueOrDefault(),
                    intStatus = 1
                };

                usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(mObjIndividual);

                BLMDAService mObjBLMDAService = new BLMDAService();
                BLMDAServiceItem mObjBLMDAServiceItem = new BLMDAServiceItem();
                BLServiceBill mObjBLServiceBill = new BLServiceBill();

                usp_GetServiceBillList_Result mObjServiceBillData = mObjBLServiceBill.BL_GetServiceBillDetails(new ServiceBill() { ServiceBillID = sbid.GetValueOrDefault(), IntStatus = 2 });

                if (mObjIndividualData != null && mObjServiceBillData != null)
                {
                    ServiceBillViewModel mObjServiceBillModel = new ServiceBillViewModel()
                    {
                        TaxPayerID = mObjIndividualData.IndividualID.GetValueOrDefault(),
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                        TaxPayerRIN = mObjIndividualData.IndividualRIN,
                        TaxPayerName = mObjIndividualData.FirstName + " " + mObjIndividualData.LastName,
                        TaxPayerAddress = mObjIndividualData.ContactAddress,
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
                    return RedirectToAction("Search", "CaptureIndividual");
                }
            }
            else
            {
                return RedirectToAction("Search", "CaptureIndividual");
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
                            ServiceBillAmount = lstMDAServices.Count > 0 ? lstMDAServices.Sum(t => t.ServiceAmount) : 0,

                            //ServiceBillAmount = lstMDAServiceItems.Count > 0 ? lstMDAServiceItems.Where(t => t.intTrack != EnumList.Track.DELETE).Sum(t => t.ServiceAmount) : 0,
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

                                //Send Notification
                                if (GlobalDefaultValues.SendNotification)
                                {
                                    usp_GetServiceBillList_Result mObjServiceBillData = mObjBLServiceBill.BL_GetServiceBillDetails(new ServiceBill() { ServiceBillID = mObjServiceBillResponse.AdditionalData.ServiceBillID, IntStatus = 2 });
                                    usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(new Individual() { IndividualID = pObjServiceBillModel.TaxPayerID, intStatus = 1 });

                                    IList<usp_GetServiceBill_MDAServiceList_Result> lstMAPServiceBillServices = mObjBLServiceBill.BL_GetMDAServiceList(mObjServiceBillData.ServiceBillID.GetValueOrDefault());

                                    string MDAServicesNames = string.Join(",", lstMAPServiceBillServices.Select(t => t.MDAServiceName).ToArray());

                                    if (mObjIndividualData != null && mObjServiceBillData != null)
                                    {
                                        EmailDetails mObjEmailDetails = new EmailDetails()
                                        {
                                            TaxPayerTypeID = mObjIndividualData.TaxPayerTypeID.GetValueOrDefault(),
                                            TaxPayerTypeName = mObjIndividualData.TaxPayerTypeName,
                                            TaxPayerID = mObjIndividualData.IndividualID.GetValueOrDefault(),
                                            TaxPayerName = mObjIndividualData.FirstName + " " + mObjIndividualData.LastName,
                                            TaxPayerRIN = mObjIndividualData.IndividualRIN,
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
                                return RedirectToAction("Details", "CaptureIndividual", new { id = pObjServiceBillModel.TaxPayerID, name = pObjServiceBillModel.TaxPayerRIN });
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
            string url = getUrl();
            bool itCan = new UtilityController().CheckAccess(url);
            if (!itCan) { return RedirectToAction("AccessDenied", "Utility"); }
            if (id.GetValueOrDefault() > 0 && billid.GetValueOrDefault() > 0)
            {
                Individual mObjIndividual = new Individual()
                {
                    IndividualID = id.GetValueOrDefault(),
                    intStatus = 1
                };
                usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(mObjIndividual);

                if (mObjIndividualData != null)
                {
                    if (billrefno.StartsWith("AB"))
                    {
                        BLAssessment mObjBLAssessment = new BLAssessment();
                        BLAssessmentItem mObjBLAssessmentItem = new BLAssessmentItem();
                        usp_GetAssessmentList_Result mObjAssessmentData = mObjBLAssessment.BL_GetAssessmentDetails(new Assessment() { AssessmentID = billid.GetValueOrDefault(), IntStatus = 2 });

                        if (mObjAssessmentData != null && mObjAssessmentData.TaxPayerID == mObjIndividualData.IndividualID && mObjAssessmentData.TaxPayerTypeID == (int)EnumList.TaxPayerType.Individual)
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

                        if (mObjServiceBillData != null && mObjServiceBillData.TaxPayerID == mObjIndividualData.IndividualID && mObjServiceBillData.TaxPayerTypeID == (int)EnumList.TaxPayerType.Individual)
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
                            return RedirectToAction("Search", "CaptureIndividual");
                        }
                    }
                    else
                    {
                        return RedirectToAction("Search", "CaptureIndividual");
                    }
                }
                else
                {
                    return RedirectToAction("Search", "CaptureIndividual");
                }
            }
            else
            {
                return RedirectToAction("Search", "CaptureIndividual");
            }
        }
        public ActionResult BillDetailFromPending(int? id, long? billid, string billrefno)
        {
            if (id.GetValueOrDefault() > 0 && billid.GetValueOrDefault() > 0)
            {
                Individual mObjIndividual = new Individual()
                {
                    IndividualID = id.GetValueOrDefault(),
                    intStatus = 1
                };
                usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(mObjIndividual);

                if (mObjIndividualData != null)
                {
                    if (billrefno.StartsWith("AB"))
                    {
                        BLAssessment mObjBLAssessment = new BLAssessment();
                        BLAssessmentItem mObjBLAssessmentItem = new BLAssessmentItem();
                        usp_GetAssessmentList_Result mObjAssessmentData = mObjBLAssessment.BL_GetAssessmentDetails(new Assessment() { AssessmentID = billid.GetValueOrDefault(), IntStatus = 2 });

                        if (mObjAssessmentData != null && mObjAssessmentData.TaxPayerID == mObjIndividualData.IndividualID && mObjAssessmentData.TaxPayerTypeID == (int)EnumList.TaxPayerType.Individual)
                        {
                            var disaap = _db.MapAssessmentDisapprove_.Where(o => o.AssessmentID == mObjAssessmentData.AssessmentID).ToList();

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
                            ViewBag.disaap = disaap;
                            return View("AssessmentBillDetailFromPending", mObjAssessmentData);
                        }
                        else
                        {
                            return RedirectToAction("Search", "CaptureIndividual");
                        }

                    }
                    else
                    {
                        return RedirectToAction("Search", "CaptureIndividual");
                    }
                }
                else
                {
                    return RedirectToAction("Search", "CaptureIndividual");
                }
            }
            else
            {
                return RedirectToAction("Search", "CaptureIndividual");
            }
        }
        public ActionResult BillDetailFromDecline(int? id, long? billid, string billrefno)
        {
            if (id.GetValueOrDefault() > 0 && billid.GetValueOrDefault() > 0)
            {
                Individual mObjIndividual = new Individual()
                {
                    IndividualID = id.GetValueOrDefault(),
                    intStatus = 1
                };
                usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(mObjIndividual);

                if (mObjIndividualData != null)
                {
                    if (billrefno.StartsWith("AB"))
                    {
                        BLAssessment mObjBLAssessment = new BLAssessment();
                        BLAssessmentItem mObjBLAssessmentItem = new BLAssessmentItem();
                        usp_GetAssessmentList_Result mObjAssessmentData = mObjBLAssessment.BL_GetAssessmentDetails(new Assessment() { AssessmentID = billid.GetValueOrDefault(), IntStatus = 2 });

                        if (mObjAssessmentData != null && mObjAssessmentData.TaxPayerID == mObjIndividualData.IndividualID && mObjAssessmentData.TaxPayerTypeID == (int)EnumList.TaxPayerType.Individual)
                        {
                            var disaap = _db.MapAssessmentDisapprove_.Where(o => o.AssessmentID == mObjAssessmentData.AssessmentID).ToList();
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
                            ViewBag.disaap = disaap;

                            return View("BillDetailFromDecline", mObjAssessmentData);
                        }
                        else
                        {
                            return RedirectToAction("Search", "CaptureIndividual");
                        }

                    }
                    else
                    {
                        return RedirectToAction("Search", "CaptureIndividual");
                    }
                }
                else
                {
                    return RedirectToAction("Search", "CaptureIndividual");
                }
            }
            else
            {
                return RedirectToAction("Search", "CaptureIndividual");
            }
        }

        public ActionResult BillDetailToBeApproved(string billid, string type, string declineNote)
        {
            long bId = Convert.ToInt64(billid);
            int t = type != null ? Convert.ToInt32(type) : 2;
            BLAssessment mObjBLAssessment = new BLAssessment();
            var ass = _db.Assessments.FirstOrDefault(o => o.AssessmentID == bId);
            var set = _db.Settlements.FirstOrDefault(o => o.AssessmentID == bId);
            var allTax = _db.Tax_Offices.Where(o => o.OfficeManagerID == SessionManager.UserID);
            if (!allTax.Any())
                allTax = _db.Tax_Offices.Where(o => o.IncomeDirector == SessionManager.UserID);
            switch (t)
            {
                case 1:
                    if (ass.SettlementStatusID == 6)
                    {
                        ass.SettlementStatusID = 1;
                    }
                    else if (ass.SettlementStatusID == 8)
                    {
                        IList<usp_GetAssessmentAdjustmentList_Result> lstAssessmentAdjustment = mObjBLAssessment.BL_GetAssessmentAdjustment(ass.AssessmentID);
                        IList<usp_GetAssessmentLateChargeList_Result> lstAssessmentLateCharge = mObjBLAssessment.BL_GetAssessmentLateCharge(ass.AssessmentID);


                        //sum of items
                        var totalAss = ass.AssessmentAmount + lstAssessmentAdjustment.Sum(o => o.Amount) + lstAssessmentLateCharge.Sum(o => o.TotalAmount);
                        if (set.SettlementAmount == 0)
                            ass.SettlementStatusID = 1;
                        else if (totalAss > set.SettlementAmount)
                            ass.SettlementStatusID = 3;
                        else
                            ass.SettlementStatusID = 4;

                    }
                    break;
                case 2:
                    ass.SettlementStatusID = 7;
                    MapAssessmentDisapprove_ map = new MapAssessmentDisapprove_();
                    map.AssessmentID = bId;
                    map.Notes = declineNote;
                    map.DateCreated = DateTime.Now;
                    map.TaxOfficerDesignation = SessionManager.UserID.ToString();
                    map.TaxOfficerId = allTax.FirstOrDefault().TaxOfficeID;
                    _db.MapAssessmentDisapprove_.Add(map);
                    break;
                default:
                    break;
            }
            _db.SaveChanges();
            return RedirectToAction("Pending", "Home", new { id = ass.TaxPayerID, name = ass.TaxPayerRIN });

        }
        public ActionResult GenerateBill(int? id, string name, int? billid, string billrefno)
        {
            string url = getUrl();
            bool itCan = new UtilityController().CheckAccess(url);
            if (!itCan) { return RedirectToAction("AccessDenied", "Utility"); }
            if (id.GetValueOrDefault() > 0 && billid.GetValueOrDefault() > 0)
            {
                Individual mObjIndividual = new Individual()
                {
                    IndividualID = id.GetValueOrDefault(),
                    intStatus = 1
                };

                usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(mObjIndividual);
                string mHtmlDirectory = $"{DocumentHTMLLocation}/assessmentBills.html";
                if (mObjIndividualData != null)
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

                        if (mObjAssessmentData != null && mObjAssessmentData.TaxPayerID == mObjIndividualData.IndividualID && mObjAssessmentData.TaxPayerTypeID == (int)EnumList.TaxPayerType.Individual)
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

                                    sbBillSummary.Append("<li>"); sbBillSummary.Append(item.AssessmentRuleName);

                                    sbBillSummary.Append(" - "); sbBillSummary.Append(CommUtil.GetFormatedCurrency(item.AssessmentRuleAmount)); sbBillSummary.Append("</li>");
                                }
                            }
                            else
                            {
                                foreach (var item in lstMAPAssessmentRules)
                                {
                                    sbBillSummary.Append("<li>"); sbBillSummary.Append(item.AssessmentRuleName); sbBillSummary.Append(" - ");
                                    sbBillSummary.Append(CommUtil.GetFormatedCurrency(item.AssessmentRuleAmount));
                                    sbBillSummary.Append("</li>");
                                }
                            }
                            sbBillSummary.Append("</ul>");


                            marksheet = marksheet.Replace("@@rptTaxYear@@", strTaxYear)
                             .Replace("@@rptBillType@@", "1")
                           .Replace("@@rptReferenceNumber@@", mObjAssessmentData.AssessmentRefNo)
                             .Replace("@@rptTaxPayerRIN@@", mObjIndividualData.IndividualRIN)
                            .Replace("@@rptTaxPayerName@@", mObjIndividualData.FirstName + " " + mObjIndividualData.LastName)
                            .Replace("@@rptTaxPayerNumber@@", mObjIndividualData.MobileNumber1)
                            .Replace("@@rptTaxPayerContactAddress@@", mObjIndividualData.ContactAddress)
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
                            return RedirectToAction("Search", "CaptureIndividual");
                        }

                    }
                    else if (billrefno.StartsWith("SB"))
                    {
                        BLMDAService mObjBLMDAService = new BLMDAService();
                        BLMDAServiceItem mObjBLMDAServiceItem = new BLMDAServiceItem();
                        BLServiceBill mObjBLServiceBill = new BLServiceBill();

                        usp_GetServiceBillList_Result mObjServiceBillData = mObjBLServiceBill.BL_GetServiceBillDetails(new ServiceBill() { ServiceBillID = billid.GetValueOrDefault(), IntStatus = 2 });

                        if (mObjServiceBillData != null && mObjServiceBillData.TaxPayerID == mObjIndividualData.IndividualID && mObjServiceBillData.TaxPayerTypeID == (int)EnumList.TaxPayerType.Individual)
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
                                    sbBillSummary.Append("<li>");
                                    sbBillSummary.Append(item.MDAServiceName);
                                    sbBillSummary.Append(" - ");

                                    sbBillSummary.Append(CommUtil.GetFormatedCurrency(item.ServiceAmount));

                                    sbBillSummary.Append("</li>");
                                }
                            }
                            else
                            {
                                foreach (var item in lstMAPServiceBillServices)
                                {
                                    sbBillSummary.Append("<li>"); sbBillSummary.Append(item.MDAServiceName);
                                    sbBillSummary.Append(" - ");
                                    sbBillSummary.Append(CommUtil.GetFormatedCurrency(mObjServiceBillData.ServiceBillAmount));
                                    sbBillSummary.Append("</li>");
                                }
                            }


                            sbBillSummary.Append("</ul>");


                            marksheet = marksheet.Replace("@@rptTaxYear@@", strTaxYear)
                             .Replace("@@rptBillType@@", "1")
                           .Replace("@@rptReferenceNumber@@", mObjServiceBillData.ServiceBillRefNo)
                             .Replace("@@rptTaxPayerRIN@@", mObjIndividualData.IndividualRIN)
                            .Replace("@@rptTaxPayerName@@", mObjIndividualData.FirstName + " " + mObjIndividualData.LastName)
                            .Replace("@@rptTaxPayerNumber@@", mObjIndividualData.MobileNumber1)
                            .Replace("@@rptTaxPayerContactAddress@@", mObjIndividualData.ContactAddress)
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
                            return RedirectToAction("Search", "CaptureIndividual");
                        }
                    }
                    else
                    {
                        return RedirectToAction("Search", "CaptureIndividual");
                    }
                }
                else
                {
                    return RedirectToAction("Search", "CaptureIndividual");
                }
            }
            else
            {
                return RedirectToAction("Search", "CaptureIndividual");
            }
        }
        private IList<vw_Individual> SortByColumnWithOrder(string order, string orderDir, IList<vw_Individual> data)
        {
            // Initialization.   
            IList<vw_Individual> lst = new List<vw_Individual>();
            try
            {
                // Sorting   
                switch (order)
                {
                    case "0":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.IndividualRIN).ToList() : data.OrderBy(p => p.IndividualRIN).ToList();
                        break;
                    case "1":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.IndividualName).ToList() : data.OrderBy(p => p.IndividualName).ToList();
                        break;
                    case "2":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TIN).ToList() : data.OrderBy(p => p.TIN).ToList();
                        break;
                    case "3":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ContactAddress).ToList() : data.OrderBy(p => p.ContactAddress).ToList();
                        break;
                    default:
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.IndividualRIN).ToList() : data.OrderBy(p => p.IndividualRIN).ToList();
                        break;
                }
            }
            catch (Exception ex)
            {
                // info.   
                Console.Write(ex);
                Logger.SendErrorToText(ex);
            }
            // info.   
            return lst;
        }
        public JsonResult UpdateStatus(Individual pObjIndividualData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjIndividualData.IndividualID != 0)
            {
                FuncResponse mObjFuncResponse = new BLIndividual().BL_UpdateStatus(pObjIndividualData);
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
        public JsonResult AddTCCRequest(int TaxYear, int TaxPayerID)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();
            BLTCC mObjBLTCC = new BLTCC();
            var userDet = _db.Individuals.FirstOrDefault(o => o.IndividualID == TaxPayerID);

            TCC_Request mObjRequest = new TCC_Request()
            {
                TaxOfficeId = userDet != null ? userDet.TaxOfficeID : 0,
                RequestDate = CommUtil.GetCurrentDateTime(),
                TaxPayerID = TaxPayerID,
                TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                TaxYear = TaxYear,
                StatusID = (int)EnumList.TCCRequestStatus.In_Progess,
                CreatedBy = SessionManager.UserID,
                CreatedDate = CommUtil.GetCurrentDateTime(),
            };

            FuncResponse<TCC_Request> mObjICFuncResponse = mObjBLTCC.BL_GetIncompleteRequest(mObjRequest);

            if (mObjICFuncResponse.Success)
            {

                FuncResponse<TCC_Request> mObjReqResponse = mObjBLTCC.BL_InsertTCCRequest(mObjRequest);

                if (mObjReqResponse.Success)
                {
                    mObjRequest = new TCC_Request()
                    {
                        TCCRequestID = mObjReqResponse.AdditionalData.TCCRequestID,
                        ServiceBillID = null,
                        StatusID = (int)EnumList.TCCRequestStatus.In_Progess,
                    };

                    new BLTCC().BL_UpdateServiceBillInRequest(mObjRequest);

                    //string msg = $"Your TCC Application with request Reference number {mObjReqResponse.AdditionalData.RequestRefNo} has been received and under process";
                    //  bool blnSMSSent = UtilityController.SendSMS(userDet.MobileNumber1, msg);

                    //Get List
                    dcResponse["success"] = true;
                    dcResponse["Message"] = "Request added successfully";

                    IList<usp_GetTCCRequestList_Result> lstTCCRequest = new BLTCC().BL_GetTCCRequestList(new TCC_Request() { TaxPayerID = TaxPayerID });
                    dcResponse["RequestList"] = CommUtil.RenderPartialToString("_BindTCCRequest", lstTCCRequest, this.ControllerContext);
                }
                else
                {
                    dcResponse["success"] = false;
                    dcResponse["Message"] = mObjReqResponse.Message;
                }

            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Request already exists for selected tax year and tax payer";
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditTCCRequest(long? reqid, string name)
        {
            string url = getUrl();
            bool itCan = new UtilityController().CheckAccess(url);
            if (!itCan) { return RedirectToAction("AccessDenied", "Utility"); }
            if (reqid.GetValueOrDefault() > 0)
            {
                BLTCC mObjBLTCC = new BLTCC();

                usp_GetTCCRequestDetails_Result mObjRequestData = mObjBLTCC.BL_GetRequestDetails(reqid.GetValueOrDefault());

                if (mObjRequestData != null)
                {
                    IList<usp_GetRequestIncomeStreamList_Result> lstRequestIncomeStream = mObjBLTCC.BL_GetIncomeStreamList(mObjRequestData.TCCRequestID);
                    Request_IncomeStream mObjRequestIncomeStream;
                    IList<Request_IncomeStream> lstIncomeStream = new List<Request_IncomeStream>();

                    foreach (var item in lstRequestIncomeStream)
                    {
                        mObjRequestIncomeStream = new Request_IncomeStream()
                        {
                            RowID = lstRequestIncomeStream.Count + 1,
                            TBKID = item.TRISID,
                            TaxYear = item.TaxYear.GetValueOrDefault(),
                            TotalIncomeEarned = item.TotalIncomeEarned.GetValueOrDefault(),
                            TaxPayerRoleID = item.TaxPayerRoleID.GetValueOrDefault(),
                            TaxPayerRoleName = item.TaxPayerRoleName,
                            BusinessName = item.BusinessName,
                            BusinessTypeID = item.BusinessTypeID.GetValueOrDefault(),
                            BusinessTypeName = item.BusinessTypeName,
                            LGAID = item.LGAID.GetValueOrDefault(),
                            LGAName = item.LGAName,
                            BusinessOperationID = item.BusinessOperationID.GetValueOrDefault(),
                            BusinessOperationName = item.BusinessOperationName,
                            BusinessAddress = item.BusinessAddress,
                            BusinessNumber = item.BusinessNumber,
                            ContactPersonName = item.ContactName,
                            intTrack = EnumList.Track.EXISTING
                        };

                        lstIncomeStream.Add(mObjRequestIncomeStream);
                    }

                    SessionManager.LstIncomeStream = lstIncomeStream;

                    ViewBag.IncomeStreamList = lstIncomeStream.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();

                    UI_FillYearDropDown();
                    UI_FillTaxPayerRoleDropDown(new TaxPayer_Roles() { TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual, AssetTypeID = (int)EnumList.AssetTypes.Business });
                    UI_FillBusinessTypeDropDown(new Business_Types() { intStatus = 1 });
                    UI_FillLGADropDown(new LGA() { intStatus = 1 });


                    return View(mObjRequestData);
                }
                else
                {
                    return RedirectToAction("List", "ProcessTCCRequest");
                }
            }
            else
            {
                return RedirectToAction("List", "ProcessTCCRequest");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult EditTCCRequest(FormCollection pObjFormCollection)
        {
            long mlngRequestID = TrynParse.parseLong(pObjFormCollection.Get("RequestID"));

            if (mlngRequestID > 0)
            {
                BLTCC mObjBLTCC = new BLTCC();
                usp_GetTCCRequestDetails_Result mObjRequestData = mObjBLTCC.BL_GetRequestDetails(mlngRequestID);
                IList<Request_IncomeStream> lstIncomeStream = SessionManager.LstIncomeStream ?? new List<Request_IncomeStream>();

                try
                {
                    using (TransactionScope mObjScope = new TransactionScope())
                    {
                        MAP_TCCRequest_IncomeStream mObjIncomeStream;
                        foreach (var item in lstIncomeStream)
                        {
                            if (item.intTrack == EnumList.Track.INSERT)
                            {
                                mObjIncomeStream = new MAP_TCCRequest_IncomeStream()
                                {
                                    TCCRequestID = mlngRequestID,
                                    TaxYear = item.TaxYear,
                                    TotalIncomeEarned = item.TotalIncomeEarned,
                                    TaxPayerRoleID = item.TaxPayerRoleID,
                                    BusinessName = item.BusinessName,
                                    BusinessTypeID = item.BusinessTypeID,
                                    LGAID = item.LGAID,
                                    BusinessOperationID = item.BusinessOperationID,
                                    BusinessAddress = item.BusinessAddress,
                                    BusinessNumber = item.BusinessNumber,
                                    ContactPersonName = item.ContactPersonName,
                                    CreatedBy = SessionManager.UserID,
                                    CreatedDate = CommUtil.GetCurrentDateTime(),
                                };

                                mObjBLTCC.BL_InsertUpdateIncomeStream(mObjIncomeStream);
                            }
                            else if (item.intTrack == EnumList.Track.DELETE)
                            {
                                if (item.TBKID > 0)
                                {
                                    mObjIncomeStream = new MAP_TCCRequest_IncomeStream()
                                    {
                                        TRISID = item.TBKID
                                    };

                                    mObjBLTCC.BL_RemoveIncomeStream(mObjIncomeStream);
                                }
                            }
                        }

                        SessionManager.LstIncomeStream = null;
                        mObjScope.Complete();
                        return RedirectToAction("Details", "CaptureIndividual", new { id = mObjRequestData.IndividualID, name = mObjRequestData.IndividualRIN });
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.IncomeStreamList = lstIncomeStream.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();

                    UI_FillYearDropDown();
                    UI_FillTaxPayerRoleDropDown(new TaxPayer_Roles() { TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual, AssetTypeID = (int)EnumList.AssetTypes.Business });
                    UI_FillBusinessTypeDropDown(new Business_Types() { intStatus = 1 });
                    UI_FillLGADropDown(new LGA() { intStatus = 1 });
                    Transaction.Current.Rollback();
                    ViewBag.Message = "Error Occurred Will Saving income stream";
                    Logger.SendErrorToText(ex);

                    return View(mObjRequestData);
                }
            }
            else
            {
                return RedirectToAction("Search", "CaptureIndividual");
            }
        }
        public JsonResult DeleteIncomeStream(int RowID)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (RowID > 0)
            {
                IList<Request_IncomeStream> lstIncomeStream = SessionManager.LstIncomeStream ?? new List<Request_IncomeStream>();


                Request_IncomeStream mObjIncomeStreamModel = lstIncomeStream.Where(t => t.RowID == RowID).FirstOrDefault();

                if (mObjIncomeStreamModel != null)
                {
                    mObjIncomeStreamModel.intTrack = EnumList.Track.DELETE;
                }

                dcResponse["success"] = true;
                dcResponse["Message"] = "Income Stream Deleted Successfully";

                dcResponse["IncomeStreamData"] = CommUtil.RenderPartialToString("_BindIncomeStreamTable", lstIncomeStream.Where(t => t.intTrack != EnumList.Track.DELETE).ToList(), this.ControllerContext);
                SessionManager.LstIncomeStream = lstIncomeStream;
            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Request";
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddIncomeStream(IncomeStreamViewModel pObjIncomeStreamModel)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (!ModelState.IsValid)
            {
                var errorList = ModelState.ToDictionary(kvp => kvp.Key,
                kvp => kvp.Value.Errors
                                .Select(e => e.ErrorMessage).ToArray())
                                .Where(m => m.Value.Any());

                dcResponse["success"] = false;
                dcResponse["Message"] = "All Fields are required";
                dcResponse["ErrorList"] = errorList;
            }
            else
            {
                IList<Request_IncomeStream> lstIncomeStream = SessionManager.LstIncomeStream ?? new List<Request_IncomeStream>();

                usp_GetTaxPayerRoleList_Result mObjTaxPayerRoleDetails = new BLTaxPayerRole().BL_GetTaxPayerRoleDetails(new TaxPayer_Roles() { intStatus = 2, TaxPayerRoleID = pObjIncomeStreamModel.TaxPayerRoleID });
                usp_GetLGAList_Result mObjLGADetails = new BLLGA().BL_GetLGADetails(new LGA() { intStatus = 2, LGAID = pObjIncomeStreamModel.LGAID });
                usp_GetBusinessOperationList_Result mObjBusinessOperationData = new BLBusinessOperation().BL_GetBusinessOperationDetails(new Business_Operation() { intStatus = 2, BusinessOperationID = pObjIncomeStreamModel.BusinessOperationID });

                Request_IncomeStream mObjIncomeStream = new Request_IncomeStream()
                {
                    RowID = lstIncomeStream.Count + 1,
                    TBKID = 0,
                    TaxYear = pObjIncomeStreamModel.TaxYear,
                    TotalIncomeEarned = pObjIncomeStreamModel.TotalIncomeEarned,
                    TaxPayerRoleID = pObjIncomeStreamModel.TaxPayerRoleID,
                    TaxPayerRoleName = mObjTaxPayerRoleDetails.TaxPayerRoleName,
                    BusinessName = pObjIncomeStreamModel.BusinessName,
                    BusinessTypeID = pObjIncomeStreamModel.BusinessTypeID,
                    BusinessTypeName = mObjBusinessOperationData.BusinessTypeName,
                    LGAID = pObjIncomeStreamModel.LGAID,
                    LGAName = mObjLGADetails.LGAName,
                    BusinessOperationID = pObjIncomeStreamModel.BusinessOperationID,
                    BusinessOperationName = mObjBusinessOperationData.BusinessOperationName,
                    BusinessAddress = pObjIncomeStreamModel.BusinessAddress,
                    BusinessNumber = pObjIncomeStreamModel.BusinessNumber,
                    ContactPersonName = pObjIncomeStreamModel.ContactName,
                    intTrack = EnumList.Track.INSERT,
                };

                lstIncomeStream.Add(mObjIncomeStream);

                dcResponse["success"] = true;
                dcResponse["Message"] = "Income Stream Added Successfully";

                dcResponse["IncomeStreamData"] = CommUtil.RenderPartialToString("_BindIncomeStreamTable", lstIncomeStream.Where(t => t.intTrack != EnumList.Track.DELETE).ToList(), this.ControllerContext);
                SessionManager.LstIncomeStream = lstIncomeStream;
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
                Individual mObjIndividual = new Individual()
                {
                    IndividualID = id.GetValueOrDefault(),
                    intStatus = 1
                };

                usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(mObjIndividual);

                if (mObjIndividualData != null)
                {
                    TaxPayerDocumentViewModel mObjDocumentViewModel = new TaxPayerDocumentViewModel()
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


                    return View(mObjDocumentViewModel);
                }
                else
                {
                    return RedirectToAction("Search", "CaptureIndividual");
                }
            }
            else
            {
                return RedirectToAction("Search", "CaptureIndividual");
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
                        return RedirectToAction("Details", "CaptureIndividual", new { id = pObjTaxPayerDocument.TaxPayerID, name = pObjTaxPayerDocument.TaxPayerRIN });
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
                    Logger.SendErrorToText(ex);
                }
            }
        }
        public ActionResult DocumentDetails(long? tpdid)
        {
            if (tpdid.GetValueOrDefault() > 0)
            {
                usp_GetTaxPayerDocumentList_Result mObjDocumentData = new BLTaxPayerDocument().BL_GetTaxPayerDocumentDetails(tpdid.GetValueOrDefault());

                if (mObjDocumentData != null)
                {
                    usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(new Individual() { IndividualID = mObjDocumentData.TaxPayerID.GetValueOrDefault(), intStatus = 2 });

                    ViewBag.IndividualData = mObjIndividualData;

                    return View(mObjDocumentData);
                }
                else
                {
                    return RedirectToAction("Search", "CaptureIndividual");
                }
            }
            else
            {
                return RedirectToAction("Search", "CaptureIndividual");
            }
        }
        public static string Decrypt(string pstrValue)
        {
            try
            {
                string decodedPhrase;

                byte[] Buff;

                cryptDES3.Key = cryptMD5Hash.ComputeHash(ASCIIEncoding.ASCII.GetBytes(pstrValue));

                cryptDES3.Mode = CipherMode.ECB;

                ICryptoTransform desdencrypt = cryptDES3.CreateDecryptor();

                Buff = cryptDES3.Key;


                decodedPhrase = ASCIIEncoding.ASCII.GetString(Buff, 0, Buff.Length);

                return decodedPhrase;
            }
            catch (Exception ex)
            {
                //CommUtil.ExceptionHandler(Ex);
                Logger.SendErrorToText(ex);
                return null;
            }
        }



        static string DocumentHTMLLocation = WebConfigurationManager.AppSettings["documentHTMLLocation"] ?? "";
        static string documentLocation = WebConfigurationManager.AppSettings["documentLocation"] ?? "";
    }
}