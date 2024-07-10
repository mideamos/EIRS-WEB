using Aspose.BarCode.Generation;
using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;
using EIRS.Web.Models;
using Elmah;
using Newtonsoft.Json;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using static EIRS.Web.Controllers.Filters;
using EIRS.Web.Utility;
using Title = EIRS.BOL.Title;
using Vereyon.Web;

namespace EIRS.Web.Controllers
{
    public class ProcessTCCRequestController : BaseController
    {
        EIRSEntities _db = new EIRSEntities();
        // GET: ProcessTCCRequest
        [HttpGet]
        public ActionResult List()
        {
            return View();
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

            if (!string.IsNullOrEmpty(vFilter))
            {
                sbWhereCondition.Append(" AND ( treq.RequestRefNo LIKE @MainFilter ");
                sbWhereCondition.Append(" OR dbo.GetTaxPayerName(treq.TaxPayerID,treq.TaxPayerTypeID) LIKE @MainFilter ");
                sbWhereCondition.Append(" OR dbo.GetTaxPayerRIN(treq.TaxPayerID,treq.TaxPayerTypeID) LIKE @MainFilter ");
                sbWhereCondition.Append(" OR dbo.GetTaxPayerMobile(treq.TaxPayerID,treq.TaxPayerTypeID) LIKE @MainFilter ");
                sbWhereCondition.Append(" OR treq.TaxYear LIKE @MainFilter ");
                //sbWhereCondition.Append(" OR sb.ServiceBillRefNo LIKE @MainFilter ");
                sbWhereCondition.Append(" OR req_stat.StatusName LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(REPLACE(CONVERT(varchar(50),treq.RequestDate,106),' ','-'),'') LIKE @MainFilter )");

            }

            TCC_Request mObjTCCRequest = new TCC_Request()
            {
                WhereCondition = sbWhereCondition.ToString(),
                OrderBy = vSortColumn,
                OrderByDirection = vSortColumnDir,
                PageNumber = (TrynParse.parseInt(vStart) / TrynParse.parseInt(vLength)) + 1,
                PageSize = TrynParse.parseInt(vLength),
                MainFilter = !string.IsNullOrWhiteSpace(vFilter) ? "%" + vFilter.Trim() + "%" : vFilter,

            };

            IDictionary<string, object> dcData = new BLTCC().BL_SearchTCCRequest(mObjTCCRequest);
            IList<usp_SearchTCCRequest_Result> lstRequest = (IList<usp_SearchTCCRequest_Result>)dcData["RequestList"];

            return Json(new
            {
                draw = vDraw,
                recordsFiltered = dcData["FilteredRecords"],
                recordsTotal = dcData["TotalRecords"],
                data = lstRequest
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Add()
        {
            IList<DropDownListResult> lstYear = new List<DropDownListResult>();
            lstYear.Add(new DropDownListResult() { id = 2020, text = "2020" });
            ViewBag.YearList = new SelectList(lstYear, "id", "text");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(TCCRequestViewModel pObjRequestModel)
        {
            if (!ModelState.IsValid)
            {
                IList<DropDownListResult> lstYear = new List<DropDownListResult>();
                lstYear.Add(new DropDownListResult() { id = 2020, text = "2020" });
                ViewBag.YearList = new SelectList(lstYear, "id", "text");
                return View(pObjRequestModel);
            }
            else
            {
                BLTCC mObjBLTCC = new BLTCC();

                var userDet = _db.Individuals.FirstOrDefault(o => o.IndividualID == pObjRequestModel.TaxPayerID);
                TCC_Request mObjRequest = new TCC_Request()
                {
                    TaxOfficeId = userDet != null ? userDet.TaxOfficeID : 0,
                    RequestDate = CommUtil.GetCurrentDateTime(),
                    TaxPayerID = pObjRequestModel.TaxPayerID,
                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                    TaxYear = pObjRequestModel.TaxYear,
                    StatusID = (int)TCCRequestStatus.In_Progess,
                    CreatedBy = SessionManager.UserID,
                    CreatedDate = CommUtil.GetCurrentDateTime(),
                    IsDownloaded = false,
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
                            StatusID = (int)TCCRequestStatus.In_Progess,
                        };

                        new BLTCC().BL_UpdateServiceBillInRequest(mObjRequest);


                        return RedirectToAction("List", "ProcessTCCRequest");
                    }
                    else
                    {
                        IList<DropDownListResult> lstYear = new List<DropDownListResult>();
                        lstYear.Add(new DropDownListResult() { id = 2020, text = "2020" });
                        ViewBag.YearList = new SelectList(lstYear, "id", "text");
                        ViewBag.Message = mObjReqResponse.Message;
                        return View(pObjRequestModel);
                    }

                }
                else
                {
                    IList<DropDownListResult> lstYear = new List<DropDownListResult>();
                    lstYear.Add(new DropDownListResult() { id = 2020, text = "2020" });
                    ViewBag.YearList = new SelectList(lstYear, "id", "text");
                    ViewBag.Message = "Request already exists for selected tax year and tax payer";
                    return View(pObjRequestModel);

                }
            }
        }
        [HttpGet]
        public ActionResult Details(long? reqid, string name)
        {
            if (reqid.GetValueOrDefault() > 0)
            {
                BLTCC mObjBLTCCC = new BLTCC();

                usp_GetTCCRequestDetails_Result mObjRequestData = mObjBLTCCC.BL_GetRequestDetails(reqid.GetValueOrDefault());

                if (mObjRequestData != null)
                {
                    //Get Stage List
                    IList<usp_GetAdminRequestStageList_Result> lstRequestStage = mObjBLTCCC.BL_GetAdminRequestStageList(mObjRequestData.TCCRequestID);
                    //var itemToremove = lstRequestStage.Where(o => o.StageID == 3).FirstOrDefault();
                    // lstRequestStage.Remove(itemToremove);
                    foreach (var ret in lstRequestStage)
                    {
                        var ch = lstRequestStage.FirstOrDefault(o => o.StageID == 2);
                        if (ch.StatusName == "Completed")
                        {
                            if (ret.StageID == 5)
                            {
                                ret.ShowButton = true;
                            }
                        }

                        if (mObjRequestData.SEDE_OrderID == 10003)
                        {
                            if (ret.StageID > 6)
                            {
                                ret.StatusName = "Completed";
                            }
                        }
                        else if (mObjRequestData.SEDE_OrderID == 10000)
                        {
                            if (ret.StageID == 5)
                            {
                                ret.StatusName = "Completed";
                            }
                        }
                        else if (mObjRequestData.SEDE_OrderID == 10001)
                        {
                            if (ret.StageID < 7)
                            {
                                ret.StatusName = "Completed";
                            }
                        }
                        else if (mObjRequestData.SEDE_OrderID == 10002)
                        {
                            if (ret.StageID <= 12)
                            {
                                ret.StatusName = "Completed";
                            }
                        }
                    }
                    ViewBag.RequestStageList = lstRequestStage;

                    IList<usp_GetRequestNotesList_Result> lstNotes = mObjBLTCCC.BL_GetRequestNotesList(new MAP_TCCRequest_Notes() { RequestID = mObjRequestData.TCCRequestID });
                    ViewBag.RequestNotesList = lstNotes;


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

        private void UI_FillDropDown(ValidateTaxPayerInformationViewModel pObjIndividualViewModel = null)
        {
            if (pObjIndividualViewModel != null)
            {
                pObjIndividualViewModel.TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual;
            }
            else if (pObjIndividualViewModel == null)
            {
                pObjIndividualViewModel = new ValidateTaxPayerInformationViewModel();
            }

            UI_FillGender();
            UI_FillTitleDropDown(new Title() { intStatus = 1, IncludeTitleIds = pObjIndividualViewModel.TitleID.ToString(), GenderID = pObjIndividualViewModel.GenderID });
            UI_FillTaxOfficeDropDown(new Tax_Offices() { intStatus = 1, IncludeTaxOfficeIds = pObjIndividualViewModel.TaxOfficeID.ToString() });
            UI_FillMaritalStatus();
            UI_FillNationality();
            UI_FillTaxPayerTypeDropDown(new TaxPayer_Types() { intStatus = 1, IncludeTaxPayerTypeIds = pObjIndividualViewModel.TaxPayerTypeID.ToString() }, (int)EnumList.TaxPayerType.Individual);
            UI_FillEconomicActivitiesDropDown(new Economic_Activities() { intStatus = 1, IncludeEconomicActivitiesIds = pObjIndividualViewModel.EconomicActivitiesID.ToString(), TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual });
            UI_FillNotificationMethodDropDown(new Notification_Method() { intStatus = 1, IncludeNotificationMethodIds = pObjIndividualViewModel.NotificationMethodID.ToString() });
        }

        [HttpGet]
        public ActionResult ValidateTaxPayerInformation(long? reqid)
        {
            if (reqid.GetValueOrDefault() > 0)
            {
                BLTCC mObjBLTCC = new BLTCC();
                usp_GetTCCRequestDetails_Result mObjRequestData = mObjBLTCC.BL_GetRequestDetails(reqid.GetValueOrDefault());

                if (mObjRequestData != null)
                {
                    var tinRet = _db.Individuals.FirstOrDefault(o => o.IndividualID == mObjRequestData.IndividualID);
                    usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(new Individual() { intStatus = 1, IndividualID = mObjRequestData.IndividualID });

                    if (mObjIndividualData != null)
                    {

                        ValidateTaxPayerInformationViewModel mObjValidateTaxPayerInformationModel = new ValidateTaxPayerInformationViewModel()
                        {
                            RequestID = reqid.GetValueOrDefault(),
                            IndividualID = mObjIndividualData.IndividualID.GetValueOrDefault(),
                            IndividualRIN = mObjIndividualData.IndividualRIN,
                            GenderID = mObjIndividualData.GenderID.GetValueOrDefault(),
                            TitleID = mObjIndividualData.TitleID.GetValueOrDefault(),
                            FirstName = mObjIndividualData.FirstName,
                            LastName = mObjIndividualData.LastName,
                            MiddleName = mObjIndividualData.MiddleName,
                            DOB = mObjIndividualData.DOB.Value.ToString("dd/MM/yyyy"),
                            TIN = tinRet.TIN ?? "",
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

                        ViewBag.RequestData = mObjRequestData;

                        MAP_TCCRequest_ValidateTaxPayerInformation mObjValidateTPInformation = mObjBLTCC.BL_GetValidateInformationDetails(mObjRequestData.TCCRequestID);

                        if (mObjValidateTPInformation != null)
                        {
                            mObjValidateTaxPayerInformationModel.VTPInformationID = mObjValidateTPInformation.VTPInformationID;
                            mObjValidateTaxPayerInformationModel.Notes = mObjValidateTPInformation.Notes;
                        }

                        UI_FillDropDown(mObjValidateTaxPayerInformationModel);
                        return View(mObjValidateTaxPayerInformationModel);
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
            else
            {
                return RedirectToAction("List", "ProcessTCCRequest");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult ValidateTaxPayerInformation(ValidateTaxPayerInformationViewModel pobjValidateTaxPayerInformationModel)
        {
            BLTCC mObjBLTCCC = new BLTCC();
            usp_GetTCCRequestDetails_Result mObjRequestData = mObjBLTCCC.BL_GetRequestDetails(pobjValidateTaxPayerInformationModel.RequestID);

            if (pobjValidateTaxPayerInformationModel.TIN == "" || string.IsNullOrEmpty(pobjValidateTaxPayerInformationModel.TIN) || string.IsNullOrWhiteSpace(pobjValidateTaxPayerInformationModel.TIN))
            {
                FlashMessage.Danger("You Have No TIN");
                ViewBag.RequestData = mObjRequestData;
                UI_FillDropDown(pobjValidateTaxPayerInformationModel);
                return View(pobjValidateTaxPayerInformationModel);
            }

            Individual mObjIndividual = new Individual()
            {
                IndividualID = pobjValidateTaxPayerInformationModel.IndividualID,
                GenderID = pobjValidateTaxPayerInformationModel.GenderID,
                TitleID = pobjValidateTaxPayerInformationModel.TitleID,
                FirstName = pobjValidateTaxPayerInformationModel.FirstName,
                LastName = pobjValidateTaxPayerInformationModel.LastName,
                MiddleName = pobjValidateTaxPayerInformationModel.MiddleName,
                DOB = TrynParse.parseDatetime(pobjValidateTaxPayerInformationModel.DOB),
                TIN = mObjRequestData.TIN,
                MobileNumber1 = pobjValidateTaxPayerInformationModel.MobileNumber1,
                MobileNumber2 = pobjValidateTaxPayerInformationModel.MobileNumber2,
                EmailAddress1 = pobjValidateTaxPayerInformationModel.EmailAddress1,
                EmailAddress2 = pobjValidateTaxPayerInformationModel.EmailAddress2,
                BiometricDetails = pobjValidateTaxPayerInformationModel.BiometricDetails,
                TaxOfficeID = pobjValidateTaxPayerInformationModel.TaxOfficeID,
                MaritalStatusID = pobjValidateTaxPayerInformationModel.MaritalStatusID,
                NationalityID = pobjValidateTaxPayerInformationModel.NationalityID,
                TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                EconomicActivitiesID = pobjValidateTaxPayerInformationModel.EconomicActivitiesID,
                NotificationMethodID = pobjValidateTaxPayerInformationModel.NotificationMethodID,
                ContactAddress = pobjValidateTaxPayerInformationModel.ContactAddress,
                Active = true,
                CreatedBy = SessionManager.UserID,
                CreatedDate = CommUtil.GetCurrentDateTime(),
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

                    //Update 
                    MAP_TCCRequest_ValidateTaxPayerInformation mObjValidateInformation = new MAP_TCCRequest_ValidateTaxPayerInformation()
                    {
                        VTPInformationID = pobjValidateTaxPayerInformationModel.VTPInformationID,
                        RequestID = pobjValidateTaxPayerInformationModel.RequestID,
                        Notes = pobjValidateTaxPayerInformationModel.Notes,
                        CreatedBy = SessionManager.UserID,
                        CreatedDate = CommUtil.GetCurrentDateTime()
                    };

                    FuncResponse mObjFuncResponse = mObjBLTCCC.BL_InsertUpdateValidateInformation(mObjValidateInformation);

                    if (mObjFuncResponse.Success)
                    {
                        //Update Stage Status
                        MAP_TCCRequest_Stages mObjRequestStage = new MAP_TCCRequest_Stages()
                        {
                            ApprovalDate = CommUtil.GetCurrentDateTime(),
                            StageID = (int)NewTCCRequestStage.Validate_Tax_Payer_Information,
                            StatusID = (int)TCCRequestStatus.Validated_Information,
                            RequestID = pobjValidateTaxPayerInformationModel.RequestID
                        };

                        mObjBLTCCC.BL_UpdateRequestStage(mObjRequestStage);
                        string msg = $"Your TCC application with request reference number {mObjRequestData.RequestRefNo} has been received and under process, click on http://eras.eirs.vip/Default/VerifyTccRequestStatus to verify your status ";
                        bool blnSMSSent = UtilityController.SendSMS(mObjIndividual.MobileNumber1, msg);

                        return RedirectToAction("Details", "ProcessTCCRequest", new { reqid = pobjValidateTaxPayerInformationModel.RequestID });

                    }
                    else
                    {
                        ViewBag.RequestData = mObjRequestData;
                        UI_FillDropDown(pobjValidateTaxPayerInformationModel);
                        ViewBag.Message = mObjFuncResponse.Message;
                        return View(pobjValidateTaxPayerInformationModel);
                    }
                }
                else
                {
                    ViewBag.RequestData = mObjRequestData;
                    UI_FillDropDown(pobjValidateTaxPayerInformationModel);
                    ViewBag.Message = mObjResponse.Message;
                    return View(pobjValidateTaxPayerInformationModel);
                }
            }
            catch (Exception ex)
            {
                Logger.SendErrorToText(ex);
                ViewBag.RequestData = mObjRequestData;
                ErrorSignal.FromCurrentContext().Raise(ex);
                UI_FillDropDown(pobjValidateTaxPayerInformationModel);
                ViewBag.Message = "Error occurred while saving individual";
                return View(pobjValidateTaxPayerInformationModel);
            }

        }

        [HttpGet]
        public ActionResult ViewValidateTaxPayerInformation(long? reqid)
        {
            if (reqid.GetValueOrDefault() > 0)
            {
                BLTCC mObjBLTCCC = new BLTCC();
                usp_GetTCCRequestDetails_Result mObjRequestData = mObjBLTCCC.BL_GetRequestDetails(reqid.GetValueOrDefault());

                if (mObjRequestData != null)
                {
                    usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(new Individual() { intStatus = 1, IndividualID = mObjRequestData.IndividualID });

                    if (mObjIndividualData != null)
                    {

                        ValidateTaxPayerInformationViewModel mObjValidateTaxPayerInformationModel = new ValidateTaxPayerInformationViewModel()
                        {
                            RequestID = reqid.GetValueOrDefault(),
                            IndividualID = mObjIndividualData.IndividualID.GetValueOrDefault(),
                            IndividualRIN = mObjIndividualData.IndividualRIN,
                            GenderID = mObjIndividualData.GenderID.GetValueOrDefault(),
                            TitleID = mObjIndividualData.TitleID.GetValueOrDefault(),
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
                            TaxOfficeID = mObjIndividualData.TaxOfficeID,
                            MaritalStatusID = mObjIndividualData.MaritalStatusID,
                            NationalityID = mObjIndividualData.NationalityID.GetValueOrDefault(),
                            TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                            EconomicActivitiesID = mObjIndividualData.EconomicActivitiesID.GetValueOrDefault(),
                            NotificationMethodID = mObjIndividualData.NotificationMethodID.GetValueOrDefault(),
                            ContactAddress = mObjIndividualData.ContactAddress,
                            Active = mObjIndividualData.Active.GetValueOrDefault(),

                        };

                        ViewBag.RequestData = mObjRequestData;

                        UI_FillDropDown(mObjValidateTaxPayerInformationModel);
                        return View(mObjValidateTaxPayerInformationModel);
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
            else
            {
                return RedirectToAction("List", "ProcessTCCRequest");
            }
        }


        [HttpGet]
        public ActionResult ValidateTaxPayerIncome(long? reqid)
        {
            if (reqid.GetValueOrDefault() > 0)
            {
                UI_FillNatureOfBusinessDropDown();
                //string tickRefNo = "";
                int currentYear = DateTime.Now.Year;
                BLTCC mObjBLTCC = new BLTCC();
                usp_GetTCCRequestDetails_Result mObjRequestData = mObjBLTCC.BL_GetRequestDetails(reqid.GetValueOrDefault());
                List<PayeTccHolder> payeTccHolder = new List<PayeTccHolder>();
                List<NewERASTccHolder> NewERASTccHolder = new List<NewERASTccHolder>();
                List<NewTCCDetailsHold> NewTCCDetailsHold = new List<NewTCCDetailsHold>();
                IList<PayeApiResponse> newpai = new List<PayeApiResponse>();
                Request_IncomeStream mObjRequestIncomeStream;
                NewERASTccHolder empt1, empt2, empt3;
                PayeApiResponse payeApiResponse;
                IList<Request_IncomeStream> lstIncomeStream = new List<Request_IncomeStream>();
                if (mObjRequestData != null)
                {
                    using (var ddb = new EIRSEntities())
                    {
                        var assDetails = ddb.Assessments.Where(o => o.AssessmentRefNo == mObjRequestData.ServiceBillRefNo).FirstOrDefault();
                        payeTccHolder = ddb.PayeTccHolders.Where(o => o.IndividualRIN == mObjRequestData.IndividualRIN).ToList();
                        NewERASTccHolder = ddb.NewERASTccHolders.Where(o => o.RIN == mObjRequestData.IndividualRIN).ToList();
                        NewTCCDetailsHold = ddb.NewTCCDetailsHolds.Where(o => o.IndividualRIN == mObjRequestData.IndividualRIN).ToList();
                    }
                    //check for paye record first if doesnt exist in the table then go to the api
                    if (payeTccHolder.Count < 1)
                    {
                        var pai = GetLogRecord(mObjRequestData.IndividualRIN).Result;
                        pai = pai.Where(t => t.AssessmentYear != currentYear.ToString()).ToList();

                        var lastYearRec = pai.Where(t => t.AssessmentYear == (currentYear - 1).ToString()).FirstOrDefault();
                        if (lastYearRec == null)
                        {
                            lastYearRec = new PayeApiResponse()
                            {
                                AnnualGross = 0.0,
                                Cra = 0.0,
                                ValidatedPension = 0.0,
                                ValidatedNhf = 0.0,
                                ValidatedNhis = 0.0,
                                TaxFreePay = 0.0,
                                ChargeableIncome = 0.0,
                                AnnualTax = 0.0,
                                AnnualTaxII = 0.0,
                                MonthlyTax = 0.0
                            };
                            lastYearRec.RowID = 3;
                            lastYearRec.AssessmentYear = (currentYear - 1).ToString();
                        }
                        newpai.Add(lastYearRec);
                        var last2YearRec = pai.Where(t => t.AssessmentYear == (currentYear - 2).ToString()).FirstOrDefault();
                        if (last2YearRec == null)
                        {
                            last2YearRec = new PayeApiResponse()
                            {
                                AnnualGross = 0.0,
                                Cra = 0.0,
                                ValidatedPension = 0.0,
                                ValidatedNhf = 0.0,
                                ValidatedNhis = 0.0,
                                TaxFreePay = 0.0,
                                ChargeableIncome = 0.0,
                                AnnualTax = 0.0,
                                AnnualTaxII = 0.0,
                                MonthlyTax = 0.0
                            };
                            last2YearRec.RowID = 2;
                            last2YearRec.AssessmentYear = (currentYear - 2).ToString();
                        }
                        newpai.Add(last2YearRec);
                        var last3YearRec = pai.Where(t => t.AssessmentYear == (currentYear - 3).ToString()).FirstOrDefault();
                        if (last3YearRec == null)
                        {
                            last3YearRec = new PayeApiResponse()
                            {
                                AnnualGross = 0.0,
                                Cra = 0.0,
                                ValidatedPension = 0.0,
                                ValidatedNhf = 0.0,
                                ValidatedNhis = 0.0,
                                TaxFreePay = 0.0,
                                ChargeableIncome = 0.0,
                                AnnualTax = 0.0,
                                AnnualTaxII = 0.0,
                                MonthlyTax = 0.0
                            };
                            last3YearRec.RowID = 1;
                            last3YearRec.AssessmentYear = (currentYear - 3).ToString();
                        }
                        newpai.Add(last3YearRec);
                    }
                    else
                    {
                        var lastYearRec = payeTccHolder.Where(t => t.AssessmentYear == (currentYear - 1).ToString()).FirstOrDefault();
                        if (lastYearRec == null)
                        {
                            var emptyVals = new PayeApiResponse()
                            {
                                AnnualGross = 0.0,
                                Cra = 0.0,
                                ValidatedPension = 0.0,
                                ValidatedNhf = 0.0,
                                ValidatedNhis = 0.0,
                                TaxFreePay = 0.0,
                                ChargeableIncome = 0.0,
                                AnnualTax = 0.0,
                                AnnualTaxII = 0.0,
                                MonthlyTax = 0.0
                            };
                            emptyVals.RowID = 3;
                            emptyVals.AssessmentYear = (currentYear - 1).ToString();
                            newpai.Add(emptyVals);
                        }
                        else
                            newpai.Add(new PayeApiResponse
                            {
                                AnnualGross = lastYearRec.AnnualGross.Value,
                                Cra = lastYearRec.Cra.Value,
                                ValidatedPension = lastYearRec.ValidatedPension.Value,
                                ValidatedNhf = lastYearRec.ValidatedNhf.Value,
                                ValidatedNhis = lastYearRec.ValidatedNhis.Value,
                                TaxFreePay = lastYearRec.TaxFreePay.Value,
                                ChargeableIncome = lastYearRec.ChargeableIncome.Value,
                                AnnualTax = lastYearRec.AnnualTax.Value,
                                AnnualTaxII = lastYearRec.AnnualTaxII.Value,
                                MonthlyTax = lastYearRec.MonthlyTax.Value,
                                RowID = 3,
                                AssessmentYear = (currentYear - 1).ToString()
                            });
                        var last2YearRec = payeTccHolder.Where(t => t.AssessmentYear == (currentYear - 2).ToString()).FirstOrDefault();
                        if (last2YearRec == null)
                        {
                            var empties2 = new PayeApiResponse()
                            {
                                AnnualGross = 0.0,
                                Cra = 0.0,
                                ValidatedPension = 0.0,
                                ValidatedNhf = 0.0,
                                ValidatedNhis = 0.0,
                                TaxFreePay = 0.0,
                                ChargeableIncome = 0.0,
                                AnnualTax = 0.0,
                                AnnualTaxII = 0.0,
                                MonthlyTax = 0.0
                            };
                            empties2.RowID = 2;
                            empties2.AssessmentYear = (currentYear - 2).ToString();
                            newpai.Add(empties2);
                        }
                        else
                            newpai.Add(new PayeApiResponse
                            {
                                AnnualGross = last2YearRec.AnnualGross.Value,
                                Cra = last2YearRec.Cra.Value,
                                ValidatedPension = last2YearRec.ValidatedPension.Value,
                                ValidatedNhf = last2YearRec.ValidatedNhf.Value,
                                ValidatedNhis = last2YearRec.ValidatedNhis.Value,
                                TaxFreePay = last2YearRec.TaxFreePay.Value,
                                ChargeableIncome = last2YearRec.ChargeableIncome.Value,
                                AnnualTax = last2YearRec.AnnualTax.Value,
                                AnnualTaxII = last2YearRec.AnnualTaxII.Value,
                                MonthlyTax = last2YearRec.MonthlyTax.Value,
                                RowID = 2,
                                AssessmentYear = (currentYear - 2).ToString()
                            });
                        var last3YearRec = payeTccHolder.Where(t => t.AssessmentYear == (currentYear - 3).ToString()).FirstOrDefault();
                        if (last3YearRec == null)
                        {
                            var empties3 = new PayeApiResponse()
                            {
                                AnnualGross = 0.0,
                                Cra = 0.0,
                                ValidatedPension = 0.0,
                                ValidatedNhf = 0.0,
                                ValidatedNhis = 0.0,
                                TaxFreePay = 0.0,
                                ChargeableIncome = 0.0,
                                AnnualTax = 0.0,
                                AnnualTaxII = 0.0,
                                MonthlyTax = 0.0
                            };
                            empties3.RowID = 1;
                            empties3.AssessmentYear = (currentYear - 3).ToString();
                            newpai.Add(empties3);
                        }
                        else
                            newpai.Add(new PayeApiResponse
                            {
                                AnnualGross = last3YearRec.AnnualGross.Value,
                                Cra = last3YearRec.Cra.Value,
                                ValidatedPension = last3YearRec.ValidatedPension.Value,
                                ValidatedNhf = last3YearRec.ValidatedNhf.Value,
                                ValidatedNhis = last3YearRec.ValidatedNhis.Value,
                                TaxFreePay = last3YearRec.TaxFreePay.Value,
                                ChargeableIncome = last3YearRec.ChargeableIncome.Value,
                                AnnualTax = last3YearRec.AnnualTax.Value,
                                AnnualTaxII = last3YearRec.AnnualTaxII.Value,
                                MonthlyTax = last3YearRec.MonthlyTax.Value,
                                RowID = 1,
                                AssessmentYear = (currentYear - 3).ToString()
                            });
                    }
                    var finalnewpai = newpai.OrderBy(x => x.AssessmentYear).ToList();
                    finalnewpai = finalnewpai.Select(entry =>
                    {
                        if ((int)entry.RowID != 0)
                        {
                            entry.RowID = 0;
                        }
                        return entry;
                    }).ToList();
                    for (int i = 0; i < finalnewpai.Count; i++)
                    {
                        var e = finalnewpai[i];
                        e.RowID = i + 1;
                    }


                    ViewBag.PAYEIncomeStreamList = finalnewpai;
                    SessionManager.LstPayeApiResponse = finalnewpai;
                    //check for Eras record first if doesnt exist in the table then go to the api
                    if (NewERASTccHolder.Count < 1)
                    {
                        IList<usp_GetRequestIncomeStreamList_Result> lstRequestIncomeStream = mObjBLTCC.BL_GetIncomeStreamList(mObjRequestData.TCCRequestID);
                        if (lstRequestIncomeStream.Count > 0)
                        {
                            foreach (usp_GetRequestIncomeStreamList_Result item in lstRequestIncomeStream)
                            {
                                mObjRequestIncomeStream = new Request_IncomeStream()
                                {
                                    RowID = lstRequestIncomeStream.Count + 1,
                                    TBKID = item.TRISID,
                                    TaxYear = item.TaxYear.GetValueOrDefault(),
                                    TotalIncomeEarned = item.TotalIncomeEarned.GetValueOrDefault(),
                                    TaxPayerRoleID = item.TaxPayerRoleID.GetValueOrDefault(),
                                    TaxPayerRoleName = item.TaxPayerRoleName,
                                    BusinessID = item.BusinessID,
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
                                    Notes = item.Notes,
                                    intTrack = EnumList.Track.EXISTING
                                };
                                lstIncomeStream.Add(mObjRequestIncomeStream);
                            }
                        }
                        else
                        {
                            empt1 = new NewERASTccHolder()
                            {
                                AssessmentYear = currentYear - 1,

                                TotalIncomeEarned = 0
                            };
                            empt2 = new NewERASTccHolder()
                            {
                                AssessmentYear = currentYear - 2,
                                TotalIncomeEarned = 0
                            };
                            empt3 = new NewERASTccHolder()
                            {
                                AssessmentYear = currentYear - 3,
                                TotalIncomeEarned = 0
                            };
                            lstIncomeStream.Add(new Request_IncomeStream { TotalIncomeEarned = empt1.TotalIncomeEarned.Value, TaxYear = empt1.AssessmentYear.Value, RowID = 1 });
                            lstIncomeStream.Add(new Request_IncomeStream { TotalIncomeEarned = empt2.TotalIncomeEarned.Value, TaxYear = empt2.AssessmentYear.Value, RowID = 2 });
                            lstIncomeStream.Add(new Request_IncomeStream { TotalIncomeEarned = empt3.TotalIncomeEarned.Value, TaxYear = empt3.AssessmentYear.Value, RowID = 3 });
                        }
                    }
                    else
                    {
                        var lastYREras = NewERASTccHolder.Where(t => t.AssessmentYear == (currentYear - 1)).FirstOrDefault();
                        if (lastYREras == null)
                        {
                            lastYREras = new NewERASTccHolder()
                            {
                                AssessmentYear = currentYear - 1,

                                TotalIncomeEarned = 0
                            };

                        }
                        var last2YREras = NewERASTccHolder.Where(t => t.AssessmentYear == (currentYear - 2)).FirstOrDefault();
                        if (last2YREras == null)
                        {
                            last2YREras = new NewERASTccHolder()
                            {
                                AssessmentYear = currentYear - 2,
                                TotalIncomeEarned = 0
                            };

                        }
                        var last3YREras = NewERASTccHolder.Where(t => t.AssessmentYear == (currentYear - 3)).FirstOrDefault();
                        if (last3YREras == null)
                        {
                            last3YREras = new NewERASTccHolder()
                            {
                                AssessmentYear = currentYear - 3,
                                TotalIncomeEarned = 0
                            };
                        }
                        lstIncomeStream.Add(new Request_IncomeStream { BusinessName = lastYREras.BusinessName, LGAName = lastYREras.LGA, TaxPayerRoleName = lastYREras.Role, TotalIncomeEarned = lastYREras.TotalIncomeEarned.Value, TaxYear = lastYREras.AssessmentYear.Value, RowID = 1 });
                        lstIncomeStream.Add(new Request_IncomeStream { BusinessName = last2YREras.BusinessName, LGAName = last2YREras.LGA, TaxPayerRoleName = last2YREras.Role, TotalIncomeEarned = last2YREras.TotalIncomeEarned.Value, TaxYear = last2YREras.AssessmentYear.Value, RowID = 2 });
                        lstIncomeStream.Add(new Request_IncomeStream { BusinessName = last3YREras.BusinessName, LGAName = last3YREras.LGA, TaxPayerRoleName = last3YREras.Role, TotalIncomeEarned = last3YREras.TotalIncomeEarned.Value, TaxYear = last3YREras.AssessmentYear.Value, RowID = 3 });

                    }
                    lstIncomeStream = lstIncomeStream.Where(t => t.TaxYear != currentYear).ToList();

                    //mObjRequestData.ServiceBillRefNo
                    List<usp_GetTaxPayerPaymentForTCCNEW_Result> tickRefNo = new List<usp_GetTaxPayerPaymentForTCCNEW_Result>();
                    ValidateTaxPayerIncomeViewModel mObjValidateTaxPayerIncomeModel = new ValidateTaxPayerIncomeViewModel()
                    {
                        RequestID = mObjRequestData.TCCRequestID,
                        TaxPayerID = mObjRequestData.IndividualID,
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                        TaxPayerTypeName = "Individual",
                        TaxPayerName = mObjRequestData.FirstName + " " + mObjRequestData.LastName,
                        RequestRefNo = mObjRequestData.RequestRefNo,
                        IndividualRin = mObjRequestData.IndividualRIN,
                        TaxYear = mObjRequestData.TaxYear.GetValueOrDefault(),
                        SerialNumber = mObjRequestData.TCCRequestID.ToString(),
                        // SourceOfIncome = mObjRequestData.EconomicActivitiesName,
                    };
                    IList<usp_GetTaxPayerPaymentForTCCNEW_Result> lstTaxPayerPayment = mObjBLTCC.BL_GetTaxPayerPaymentListNEW(mObjValidateTaxPayerIncomeModel.TaxPayerID, mObjValidateTaxPayerIncomeModel.TaxPayerTypeID);
                    ViewBag.TaxPayerPayment = lstTaxPayerPayment;
                    SessionManager.LstTCCTaxPayerPayment = lstTaxPayerPayment;
                    var lstErasHolder = new List<NewERASTccHolder>();
                    SessionManager.LstIncomeStream = lstIncomeStream;
                    // SessionManager.LstErasTccHolder = ;
                    ViewBag.IncomeStreamList = SessionManager.LstIncomeStream.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
                    List<TicketRef> ticketRefs = new List<TicketRef>();

                    IList<usp_GetTCCDetail_Result> lstRequestTCCDetail = mObjBLTCC.BL_GetTCCDetail(mObjRequestData.IndividualID, (int)EnumList.TaxPayerType.Individual, mObjRequestData.TaxYear.GetValueOrDefault());
                    Request_TCCDetail mObjRequest1TCCDetail, mObjRequest2TCCDetail, mObjRequest3TCCDetail;
                    SessionManager.LstTCCDetailNew = lstRequestTCCDetail;
                    IList<Request_TCCDetail> lstTCCDetail = new List<Request_TCCDetail>();

                    var tccDetailPaye = newpai.Where(o => o.AssessmentYear == (currentYear - 1).ToString()).FirstOrDefault();
                    var tccDetailPay2e = newpai.Where(o => o.AssessmentYear == (currentYear - 2).ToString()).FirstOrDefault();
                    var tccDetailPay3e = newpai.Where(o => o.AssessmentYear == (currentYear - 3).ToString()).FirstOrDefault();
                    var tccDetailEras = lstIncomeStream.Where(o => o.TaxYear == (currentYear - 1)).FirstOrDefault();
                    var newlstTaxPayerPayment = lstTaxPayerPayment.Where(o => o.AssessmentYear == (currentYear - 1)).ToList();
                    string ref1 = "";
                    string ref2 = "";
                    string ref3 = "";
                    if (lstTaxPayerPayment.Count > 0)
                    {
                        tickRefNo = lstTaxPayerPayment.OrderByDescending(o => o.PaymentID).ToList();
                        if (tickRefNo.Count > 0)
                        {
                            var ret1Paye = tccDetailPaye.ReceiptRef;
                            var ret2Paye = tccDetailPay2e.ReceiptRef;
                            var ret3Paye = tccDetailPay3e.ReceiptRef;
                            var ret1 = tickRefNo.Where(o => o.AssessmentYear == (currentYear - 1)).FirstOrDefault();
                            if (string.IsNullOrEmpty(ret1Paye))
                            {
                                if (ret1 != null)
                                    ref1 = ret1.ReceiptRefNo;
                            }
                            else
                            {
                                ref1 = ret1Paye;
                            }

                            var ret2 = tickRefNo.Where(o => o.AssessmentYear == (currentYear - 2)).FirstOrDefault();
                            if (string.IsNullOrEmpty(ret2Paye))
                            {
                                if (ret2 != null)
                                    ref2 = ret2.ReceiptRefNo;
                            }
                            else
                            {
                                ref2 = ret2Paye;
                            }
                            var ret3 = tickRefNo.Where(o => o.AssessmentYear == (currentYear - 3)).FirstOrDefault();
                            if (string.IsNullOrEmpty(ret3Paye))
                            {
                                if (ret3 != null)
                                    ref3 = ret3.ReceiptRefNo;
                            }
                            else
                            {
                                ref3 = ret3Paye;
                            }
                        }

                        //newlstTaxPayerPayment.OrderByDescending(o => o.PaymentID).FirstOrDefault().PaymentDate.Value.ToString("dd-MMMM-yyyy")
                        string PD = newlstTaxPayerPayment.Count() > 0 ? newlstTaxPayerPayment.OrderByDescending(o => o.PaymentID).FirstOrDefault().PaymentDate.Value.ToString("dd-MMMM-yyyy") : null;
                        ticketRefs.Add(new TicketRef { TaxYear = (currentYear - 1).ToString(), TickRefNo = ref1, PaymentDate = PD });
                        ticketRefs.Add(new TicketRef { TaxYear = (currentYear - 2).ToString(), TickRefNo = ref2, PaymentDate = PD });
                        ticketRefs.Add(new TicketRef { TaxYear = (currentYear - 3).ToString(), TickRefNo = ref3, PaymentDate = PD });
                    }
                    string tckRef1 = "";
                    string tckRef2 = "";
                    string tckRef3 = "";
                    if (ticketRefs.Count > 0)
                    {
                        var ret1 = tickRefNo.Where(o => o.AssessmentYear == (currentYear - 1)).FirstOrDefault();
                        if (ret1 != null)
                            tckRef1 = ret1.ReceiptRefNo;
                        var ret2 = tickRefNo.Where(o => o.AssessmentYear == (currentYear - 2)).FirstOrDefault();
                        if (ret2 != null)
                            tckRef2 = ret2.ReceiptRefNo;
                        var ret3 = tickRefNo.Where(o => o.AssessmentYear == (currentYear - 3)).FirstOrDefault();
                        if (ret3 != null)
                            tckRef3 = ret3.ReceiptRefNo;
                    }
                    if (NewTCCDetailsHold.Any())
                    {
                        foreach (var rec in NewTCCDetailsHold)
                        {
                            mObjRequest1TCCDetail = new Request_TCCDetail()
                            {
                                RowID = rec.RowID.GetValueOrDefault(),
                                TBKID = rec.TBKID.Value,
                                TaxYear = rec.TaxYear.GetValueOrDefault(),
                                AssessableIncome = rec.AssessableIncome.Value,
                                TCCTaxPaid = rec.TCCTaxPaid.Value,
                                ERASAssessed = rec.ERASAssessed.Value,
                                ERASTaxPaid = rec.ERASTaxPaid.Value,
                                Tax_receipt = rec.Tax_receipt,
                                intTrack = EnumList.Track.INSERT,
                                RevenueType =rec.RevenueType
                            };
                            lstTCCDetail.Add(mObjRequest1TCCDetail);
                        }
                    }
                    else
                    {

                        var tccDetailEras2 = lstIncomeStream.Where(o => o.TaxYear == (currentYear - 2)).FirstOrDefault();

                        var tccDetailEra3 = lstIncomeStream.Where(o => o.TaxYear == (currentYear - 3)).FirstOrDefault();

                        decimal chargeableIncome = Convert.ToDecimal(tccDetailPaye.ChargeableIncome);
                        decimal totalIncomeEarned = Convert.ToDecimal(tccDetailEras.TotalIncomeEarned);
                        decimal chargeableIncome2 = Convert.ToDecimal(tccDetailPay2e.ChargeableIncome);
                        decimal totalIncomeEarned2 = Convert.ToDecimal(tccDetailEras2.TotalIncomeEarned);
                        decimal chargeableIncome3 = Convert.ToDecimal(tccDetailPay3e.ChargeableIncome);
                        decimal totalIncomeEarned3 = Convert.ToDecimal(tccDetailEra3.TotalIncomeEarned);

                        // Calculate the sum
                        decimal totalIncome = chargeableIncome + totalIncomeEarned;
                        decimal totalIncome2 = chargeableIncome2 + totalIncomeEarned2;
                        decimal totalIncome3 = chargeableIncome3 + totalIncomeEarned3;

                        // Determine the revenue type based on the values of chargeableIncome and totalIncomeEarned
                        string revenueType = string.Empty;

                        if (chargeableIncome == 0 && totalIncomeEarned == 0)
                        {
                            revenueType = "";
                        }
                        else if (chargeableIncome == 0)
                        {
                            revenueType = "DA";
                        }
                        else if (totalIncomeEarned == 0)
                        {
                            revenueType = "PAYE";
                        }


                        mObjRequest1TCCDetail = new Request_TCCDetail()
                        {
                            RowID = 1,
                            TBKID = 1,
                            TaxYear = (currentYear - 1),
                            //income amount from paye + TotalIncomeEarned
                            AssessableIncome = totalIncome,
                            TCCTaxPaid = Convert.ToDecimal(tccDetailPaye.AnnualTax),
                            ERASAssessed = Convert.ToDecimal(tccDetailPaye.AnnualTax),
                            ERASTaxPaid = Convert.ToDecimal(tccDetailPaye.AnnualTaxII),
                            Tax_receipt = tckRef1,
                            RevenueType = revenueType,
                            intTrack = EnumList.Track.INSERT
                        };
                        lstTCCDetail.Add(mObjRequest1TCCDetail);
                        revenueType = string.Empty;

                        if (chargeableIncome2 == 0 && totalIncomeEarned2 == 0)
                        {
                            revenueType = "";
                        }
                        else if (chargeableIncome2 == 0)
                        {
                            revenueType = "DA";
                        }
                        else if (totalIncomeEarned2 == 0)
                        {
                            revenueType = "PAYE";
                        }

                        var newlstTaxPayerPayment2 = lstTaxPayerPayment.Where(o => o.AssessmentYear == (currentYear - 2)).ToList();
                        mObjRequest2TCCDetail = new Request_TCCDetail()
                        {
                            RowID = 2,
                            TBKID = 2,
                            TaxYear = (currentYear - 2),
                            AssessableIncome = totalIncome2,
                            TCCTaxPaid = Convert.ToDecimal(tccDetailPay2e.AnnualGross),
                            ERASAssessed = Convert.ToDecimal(tccDetailPay2e.AnnualTax),
                            ERASTaxPaid = Convert.ToDecimal(tccDetailPay2e.AnnualTaxII),
                            Tax_receipt = tckRef2,
                            RevenueType = revenueType,
                            intTrack = EnumList.Track.INSERT
                        };
                        lstTCCDetail.Add(mObjRequest2TCCDetail);
                        revenueType = string.Empty;

                        if (chargeableIncome3 == 0 && totalIncomeEarned3 == 0)
                        {
                            revenueType = "";
                        }
                        else if (chargeableIncome3 == 0)
                        {
                            revenueType = "DA";
                        }
                        else if (totalIncomeEarned3 == 0)
                        {
                            revenueType = "PAYE";
                        }
                        var newlstTaxPayerPayment3 = lstTaxPayerPayment.Where(o => o.AssessmentYear == (currentYear - 3)).ToList();
                        mObjRequest3TCCDetail = new Request_TCCDetail()
                        {
                            RowID = 3,
                            TBKID = 3,
                            TaxYear = (currentYear - 3),
                            AssessableIncome = totalIncome3,
                            TCCTaxPaid = Convert.ToDecimal(tccDetailPay3e.AnnualGross),
                            ERASAssessed = Convert.ToDecimal(tccDetailPay3e.AnnualTax),
                            ERASTaxPaid = Convert.ToDecimal(tccDetailPay3e.AnnualTaxII),
                            Tax_receipt = tckRef3,
                            RevenueType = revenueType,
                            intTrack = EnumList.Track.INSERT
                        };
                        lstTCCDetail.Add(mObjRequest3TCCDetail);
                    }
                    SessionManager.LstTCCDetail = lstTCCDetail;
                    ViewBag.TCCDetailList = SessionManager.LstTCCDetail.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();

                    ViewBag.RequestData = mObjRequestData;
                    SessionManager.LstTicketRef = ticketRefs;

                    MAP_TCCRequest_ValidateTaxPayerIncome mObjValidateTPIncome = mObjBLTCC.BL_GetValidateIncomeDetails(mObjRequestData.TCCRequestID);

                    usp_GetTaxClearanceCertificateDetails_Result mObjTCCData = mObjBLTCC.BL_GetTaxClearanceCertificateDetail(new TaxClearanceCertificate()
                    {
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                        TaxPayerID = mObjRequestData.IndividualID,
                        TaxYear = mObjRequestData.TaxYear.GetValueOrDefault(),
                        RequestRefNo = mObjRequestData.RequestRefNo
                    });


                    // SessionManager.LstPayeApiResponse = newpai;
                    if (mObjValidateTPIncome != null)
                    {
                        mObjValidateTaxPayerIncomeModel.VTPIncomeID = mObjValidateTPIncome.VTPIncomeID;
                        mObjValidateTaxPayerIncomeModel.Notes = mObjValidateTPIncome.Notes;
                    }

                    if (mObjTCCData != null)
                    {
                        mObjValidateTaxPayerIncomeModel.TCCID = mObjTCCData.TCCID;
                        mObjValidateTaxPayerIncomeModel.CertificateNotes = mObjTCCData.TaxPayerDetails;
                    }
                    MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                    {
                        TaxPayerID = mObjValidateTaxPayerIncomeModel.TaxPayerID,
                        TaxPayerTypeID = mObjValidateTaxPayerIncomeModel.TaxPayerTypeID,
                        AssetTypeID = 3
                    };
                    //usp_GetTaxPayerAssetForTCC_Result
                    IList<usp_GetTaxPayerAssetList_Result> lstTaxPayerAsset = new BLTaxPayerAsset().BL_GetTaxPayerAssetList(mObjTaxPayerAsset);
                    ViewBag.AssetList = lstTaxPayerAsset;

                    var pObjAssetType = new Asset_Types();

                    pObjAssetType.intStatus = 1;

                    IList<DropDownListResult> lstAssetType = new BLAssetType().BL_GetAssetTypeDropDownList(pObjAssetType);
                    ViewBag.AssetTypeList = new SelectList(lstAssetType, "id", "text", 0);

                    IList<usp_GetProfileInformation_Result> lstProfileInformation = new BLTaxPayerAsset().BL_GetTaxPayerProfileInformation((int)EnumList.TaxPayerType.Individual, mObjValidateTaxPayerIncomeModel.TaxPayerID);
                    ViewBag.ProfileInformation = lstProfileInformation;
                    IList<usp_GetAssessmentRuleInformationForTCC_Result> lstAssessmentRuleInformation = mObjBLTCC.BL_GetTaxPayerAssessmentRuleList(mObjValidateTaxPayerIncomeModel.TaxPayerID, mObjValidateTaxPayerIncomeModel.TaxPayerTypeID);
                    ViewBag.AssessmentRuleInformation = lstAssessmentRuleInformation;
                    IList<usp_GetTaxPayerBillForTCC_Result> lstTaxPayerBill = mObjBLTCC.BL_GetTaxPayerBillList(mObjValidateTaxPayerIncomeModel.TaxPayerID, mObjValidateTaxPayerIncomeModel.TaxPayerTypeID);
                    ViewBag.TaxPayerBill = lstTaxPayerBill;

                    IList<usp_GetTaxPayerLiabilityForTCC_Result> lstTaxPayerLiability = mObjBLTCC.BL_GetTaxPayerLiabilityForTCC(mObjValidateTaxPayerIncomeModel.TaxPayerID, mObjValidateTaxPayerIncomeModel.TaxPayerTypeID, mObjRequestData.TaxYear.GetValueOrDefault());
                    SessionManager.lstLaibility = lstTaxPayerLiability;
                    ViewBag.TaxPayerLiability = lstTaxPayerLiability;

                    UI_FillTaxYearDropDown(mObjRequestData.TaxYear.GetValueOrDefault());
                    ViewBag.TaxBusiness = new SelectList(lstTaxPayerAsset.Where(o => o.AssetTypeID == 3 && (o.TaxPayerRoleID == 4 || o.TaxPayerRoleID == 5 || o.TaxPayerRoleID == 37 || o.TaxPayerRoleID == 39)).Select(t => new { id = t.AssetID, text = t.AssetName }).Distinct(), "id", "text");
                    ViewBag.TaxPayerRoleList = new SelectList(lstTaxPayerAsset.Select(t => new { id = t.TaxPayerRoleID, text = t.TaxPayerRoleName }).Distinct(), "id", "text");

                    return View(mObjValidateTaxPayerIncomeModel);
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
        public ActionResult ValidateTaxPayerIncome(ValidateTaxPayerIncomeViewModel pobjValidateTaxPayerIncomeModel)
        {
            List<DA_And_PAYE_TCC_Details> lsdap = new List<DA_And_PAYE_TCC_Details>();
            MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
            {
                TaxPayerID = pobjValidateTaxPayerIncomeModel.TaxPayerID,
                TaxPayerTypeID = pobjValidateTaxPayerIncomeModel.TaxPayerTypeID
            };

            var pObjAssetType = new Asset_Types();

            pObjAssetType.intStatus = 1;
            IList<PayeApiResponse> lstPayeDetail = SessionManager.LstPayeApiResponse ?? new List<PayeApiResponse>();
            ViewBag.PAYEIncomeStreamList = lstPayeDetail;
            IList<Request_IncomeStream> lstErasDetail = SessionManager.LstIncomeStream ?? new List<Request_IncomeStream>();
            ViewBag.IncomeStreamList = lstErasDetail;
            IList<Request_TCCDetail> lstTCCDetail = SessionManager.LstTCCDetail ?? new List<Request_TCCDetail>();
            List<TicketRef> ticketRefs = new List<TicketRef>();
            string strAction = Request["btnAction"];
            BLTCC mObjBLTCC = new BLTCC();
            usp_GetTCCRequestDetails_Result mObjRequestData = mObjBLTCC.BL_GetRequestDetails(pobjValidateTaxPayerIncomeModel.RequestID);
            //save to NewErasTccHolder
            IList<usp_GetTaxPayerAssetList_Result> lstTaxPayerAsset = new BLTaxPayerAsset().BL_GetTaxPayerAssetList(mObjTaxPayerAsset);
            ViewBag.AssetList = lstTaxPayerAsset;
            IList<usp_GetTaxPayerAssetForTCC_Result> newlstTaxPayerAsset = mObjBLTCC.BL_GetTaxPayerAssetList(mObjRequestData.IndividualID, (int)EnumList.TaxPayerType.Individual);
            IList<usp_GetProfileInformation_Result> lstProfileInformation = new BLTaxPayerAsset().BL_GetTaxPayerProfileInformation((int)EnumList.TaxPayerType.Individual, mObjRequestData.IndividualID);
            IList<usp_GetAssessmentRuleInformationForTCC_Result> lstAssessmentRuleInformation = mObjBLTCC.BL_GetTaxPayerAssessmentRuleList(mObjRequestData.IndividualID, (int)EnumList.TaxPayerType.Individual);
            IList<usp_GetTaxPayerBillForTCC_Result> lstTaxPayerBill = mObjBLTCC.BL_GetTaxPayerBillList(mObjRequestData.IndividualID, (int)EnumList.TaxPayerType.Individual);
            IList<usp_GetTaxPayerPaymentForTCCNEW_Result> lstTaxPayerPayment = mObjBLTCC.BL_GetTaxPayerPaymentListNEW(mObjRequestData.IndividualID, (int)EnumList.TaxPayerType.Individual);
            IList<usp_GetTaxPayerLiabilityForTCC_Result> lstTaxPayerLiability = mObjBLTCC.BL_GetTaxPayerLiabilityForTCC(mObjRequestData.IndividualID, (int)EnumList.TaxPayerType.Individual, mObjRequestData.TaxYear.GetValueOrDefault());
            IList<GetEmployerLiability_Result> lstEmployerLiability = mObjBLTCC.BL_GetEmployerLiability(mObjRequestData.IndividualID);

            if (!ModelState.IsValid)
            {
                ViewBag.RequestData = mObjRequestData;
                return View(pobjValidateTaxPayerIncomeModel);
            }
            else
            {
                if (strAction != "Save")
                {
                    if (pobjValidateTaxPayerIncomeModel.SourceOfIncome == "---Select One----")
                    {
                        FlashMessage.Danger("Select New Source Of Income");
                        IList<DropDownListResult> lstAssetType = new BLAssetType().BL_GetAssetTypeDropDownList(pObjAssetType);
                        ViewBag.AssetTypeList = new SelectList(lstAssetType, "id", "text", 0);
                        ViewBag.ProfileInformation = lstProfileInformation;
                        ViewBag.AssessmentRuleInformation = lstAssessmentRuleInformation;
                        ViewBag.TaxPayerBill = lstTaxPayerBill;
                        ViewBag.TaxPayerLiability = lstTaxPayerLiability;

                        UI_FillTaxYearDropDown(mObjRequestData.TaxYear.GetValueOrDefault());
                        ViewBag.TaxBusiness = new SelectList(lstTaxPayerAsset.Where(o => o.AssetTypeID == 3 && (o.TaxPayerRoleID == 4 || o.TaxPayerRoleID == 5 || o.TaxPayerRoleID == 37 || o.TaxPayerRoleID == 39)).Select(t => new { id = t.AssetID, text = t.AssetName }).Distinct(), "id", "text");
                        ViewBag.TaxPayerRoleList = new SelectList(lstTaxPayerAsset.Select(t => new { id = t.TaxPayerRoleID, text = t.TaxPayerRoleName }).Distinct(), "id", "text");
                        UI_FillNatureOfBusinessDropDown();
                        ViewBag.RequestData = mObjRequestData;
                        ViewBag.IncomeStreamList = SessionManager.LstIncomeStream.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
                        ViewBag.TCCDetailList = SessionManager.LstTCCDetail.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
                        ViewBag.RequestData = mObjRequestData;
                        ViewBag.NewAssetList = newlstTaxPayerAsset;
                        ViewBag.ProfileInformation = lstProfileInformation;
                        ViewBag.AssessmentRuleInformation = lstAssessmentRuleInformation;
                        ViewBag.TaxPayerBill = lstTaxPayerBill;
                        ViewBag.TaxPayerPayment = lstTaxPayerPayment;
                        ViewBag.TaxPayerLiability = lstTaxPayerLiability;
                        UI_FillTaxYearDropDown(mObjRequestData.TaxYear.GetValueOrDefault());
                        ViewBag.TaxPayerRoleList = new SelectList(newlstTaxPayerAsset.Select(t => new { id = t.TaxPayerRoleID, text = t.TaxPayerRoleName }).Distinct(), "id", "text");

                        return View(pobjValidateTaxPayerIncomeModel);
                    }
                }
                using (TransactionScope mObjTransactionScope = new TransactionScope())
                {
                    try
                    {
                        FuncResponse mObjFuncResponse;
                        foreach (var item in lstPayeDetail)
                        {
                            var removeFormal = _db.PayeTccHolders.Where(o => o.AssessmentYear == item.AssessmentYear && o.IndividualRIN == mObjRequestData.IndividualRIN).ToList();
                            if (removeFormal != null)
                                _db.PayeTccHolders.RemoveRange(removeFormal);
                            _db.PayeTccHolders.Add(new PayeTccHolder
                            {
                                AnnualGross = item.AnnualGross,
                                Cra = item.Cra,
                                ValidatedPension = item.ValidatedPension,
                                ValidatedNhf = item.ValidatedNhf,
                                ValidatedNhis = item.ValidatedNhis,
                                TaxFreePay = item.TaxFreePay,
                                MonthlyTax = item.MonthlyTax,
                                IndividualRIN = mObjRequestData.IndividualRIN,
                                ChargeableIncome = item.ChargeableIncome,
                                AnnualTax = item.AnnualTax,
                                AnnualTaxII = item.AnnualTaxII,
                                AssessmentYear = item.AssessmentYear,
                                BusinessName = item.EmployerName,
                                ReceiptDetail = item.ReceiptDetail
                            });
                        }
                        List<BalanceHolder> lstb = new List<BalanceHolder>();
                        //  IList<BusinessNameHolder> bnLst = SessionManager.businessNameHolderList ?? new List<BusinessNameHolder>();

                        var bnLst = new List<BusinessName>();

                        var bnLstII = _db.BusinessNames.Where(o => o.TccRequestId == pobjValidateTaxPayerIncomeModel.RequestID);
                        if (bnLstII != null)
                            _db.BusinessNames.RemoveRange(bnLstII);
                        MAP_TCCRequest_IncomeStream mObjIncomeStream;
                        foreach (var item in lstErasDetail)
                        {
                            var removeFormal = _db.NewERASTccHolders.Where(o => o.AssessmentYear == item.TaxYear && o.RIN == mObjRequestData.IndividualRIN).ToList();
                            if (removeFormal != null)
                                _db.NewERASTccHolders.RemoveRange(removeFormal);
                            _db.NewERASTccHolders.Add(new NewERASTccHolder
                            {
                                RIN = mObjRequestData.IndividualRIN,
                                AssessmentYear = item.TaxYear,
                                Role = item.TaxPayerRoleName,
                                BusinessName = item.BusinessName,
                                TotalIncomeEarned = item.TotalIncomeEarned,
                                LGA = item.LGAName
                            });
                            BusinessName bn = new BusinessName();
                            bn.BusinessName1 = item.BusinessName;
                            bn.TccRequestId = pobjValidateTaxPayerIncomeModel.RequestID;
                            bnLst.Add(bn);
                            if (item.intTrack == EnumList.Track.INSERT)
                            {
                                mObjIncomeStream = new MAP_TCCRequest_IncomeStream()
                                {
                                    TCCRequestID = pobjValidateTaxPayerIncomeModel.RequestID,
                                    TaxYear = item.TaxYear,
                                    TotalIncomeEarned = item.TotalIncomeEarned,
                                    TaxPayerRoleID = item.TaxPayerRoleID,
                                    BusinessID = item.BusinessID,
                                    Notes = item.Notes,
                                    CreatedBy = SessionManager.UserID,
                                    CreatedDate = CommUtil.GetCurrentDateTime(),
                                };

                                mObjFuncResponse = mObjBLTCC.BL_InsertUpdateIncomeStream(mObjIncomeStream);
                                if (!mObjFuncResponse.Success)
                                {
                                    throw (new Exception(mObjFuncResponse.Message));
                                }
                            }
                            else if (item.intTrack == EnumList.Track.DELETE)
                            {
                                if (item.TBKID > 0)
                                {
                                    mObjIncomeStream = new MAP_TCCRequest_IncomeStream()
                                    {
                                        TRISID = item.TBKID
                                    };

                                    mObjFuncResponse = mObjBLTCC.BL_RemoveIncomeStream(mObjIncomeStream);
                                    if (!mObjFuncResponse.Success)
                                    {
                                        throw (new Exception(mObjFuncResponse.Message));
                                    }
                                }
                            }
                            else if (item.intTrack == EnumList.Track.UPDATE)
                            {
                                mObjIncomeStream = new MAP_TCCRequest_IncomeStream()
                                {
                                    TRISID = item.TBKID,
                                    TCCRequestID = pobjValidateTaxPayerIncomeModel.RequestID,
                                    TaxYear = item.TaxYear,
                                    TotalIncomeEarned = item.TotalIncomeEarned,
                                    TaxPayerRoleID = item.TaxPayerRoleID,
                                    BusinessID = item.BusinessID,
                                    Notes = item.Notes,
                                    ModifiedBy = SessionManager.UserID,
                                    ModifiedDate = CommUtil.GetCurrentDateTime(),
                                };

                                mObjFuncResponse = mObjBLTCC.BL_InsertUpdateIncomeStream(mObjIncomeStream);
                                if (!mObjFuncResponse.Success)
                                {
                                    throw (new Exception(mObjFuncResponse.Message));
                                }
                            }
                        }
                        TCCDetail mObjTCCDetail;
                        foreach (Request_TCCDetail item in lstTCCDetail)
                        {
                            BalanceHolder b = new BalanceHolder();
                            NewTCCDetailsHold removeFormal = _db.NewTCCDetailsHolds.FirstOrDefault(o => o.TaxYear == item.TaxYear && o.IndividualRIN == mObjRequestData.IndividualRIN);
                            if (removeFormal != null)
                                _db.NewTCCDetailsHolds.Remove(removeFormal);
                          //  NewTCCDetailsHold neth = new NewTCCDetailsHold();
                            string refDate = "";
                            var newlstTaxPayerPayment = lstTaxPayerPayment.Where(o => o.AssessmentYear == item.TaxYear).ToList();
                            if (newlstTaxPayerPayment.Count > 0)
                                refDate = newlstTaxPayerPayment.OrderByDescending(o => o.PaymentID).FirstOrDefault().PaymentDate.Value.ToString("dd-MMMM-yyyy") ?? "";

                            var allToBeDeleted = _db.TccRefHolders.Where(o => o.TaxYear == item.TaxYear && o.ReqId == pobjValidateTaxPayerIncomeModel.RequestID.ToString()).ToList();
                            _db.TccRefHolders.RemoveRange(allToBeDeleted);
                            //_db.TccRefHolders.Add(new TccRefHolder { ReciptRef = item.Tax_receipt, TaxYear = item.TaxYear, ReqId = pobjValidateTaxPayerIncomeModel.RequestID.ToString() });
                            _db.TccRefHolders.Add(new TccRefHolder { ReciptRef = $"{item.Tax_receipt}" + "--" + $"{refDate}", TaxYear = item.TaxYear, ReqId = pobjValidateTaxPayerIncomeModel.RequestID.ToString() });
                            if (item.intTrack == EnumList.Track.INSERT)
                            {
                                mObjTCCDetail = new TCCDetail()
                                {
                                    TCCDetailID = item.TBKID,
                                    TaxYear = item.TaxYear,
                                    TaxPayerID = pobjValidateTaxPayerIncomeModel.TaxPayerID,
                                    TaxPayerTypeID = pobjValidateTaxPayerIncomeModel.TaxPayerTypeID,
                                    AssessableIncome = item.AssessableIncome,
                                    TCCTaxPaid = item.TCCTaxPaid,
                                    ERASAssessed = item.ERASAssessed,
                                    ERASTaxPaid = item.ERASTaxPaid,
                                    CreatedBy = SessionManager.UserID,
                                    CreatedDate = CommUtil.GetCurrentDateTime(),
                                };
                                b.Balance = item.ERASAssessed - item.ERASTaxPaid;
                                _db.NewTCCDetailsHolds.Add(new NewTCCDetailsHold
                                {
                                    RowID = item.RowID,
                                    TCCRequestID = mObjRequestData.TCCRequestID,
                                    TaxYear = item.TaxYear,
                                    TBKID = item.TBKID,
                                    AssessableIncome = item.AssessableIncome,
                                    TCCTaxPaid = item.TCCTaxPaid,
                                    ERASAssessed = item.ERASAssessed,
                                    ERASTaxPaid = item.ERASTaxPaid,
                                    Tax_receipt = item.Tax_receipt,
                                    RevenueType = item.RevenueType,
                                    IndividualRIN = mObjRequestData.IndividualRIN
                                });

                                mObjFuncResponse = mObjBLTCC.BL_InsertUpdateTCCDetail(mObjTCCDetail);
                                if (!mObjFuncResponse.Success)
                                {
                                    throw (new Exception(mObjFuncResponse.Message));
                                }
                            }
                            else if (item.intTrack == EnumList.Track.DELETE)
                            {
                                if (item.TBKID > 0)
                                {
                                    mObjTCCDetail = new TCCDetail()
                                    {
                                        TCCDetailID = item.TBKID
                                    };

                                    mObjFuncResponse = mObjBLTCC.BL_RemoveTCCDetail(mObjTCCDetail);
                                    if (!mObjFuncResponse.Success)
                                    {
                                        throw (new Exception(mObjFuncResponse.Message));
                                    }
                                }
                            }
                            else if (item.intTrack == EnumList.Track.UPDATE)
                            {
                                mObjTCCDetail = new TCCDetail()
                                {
                                    TCCDetailID = item.TBKID,
                                    TaxYear = item.TaxYear,
                                    TaxPayerID = pobjValidateTaxPayerIncomeModel.TaxPayerID,
                                    TaxPayerTypeID = pobjValidateTaxPayerIncomeModel.TaxPayerTypeID,
                                    AssessableIncome = item.AssessableIncome,
                                    TCCTaxPaid = item.TCCTaxPaid,
                                    ERASAssessed = item.ERASAssessed,
                                    ERASTaxPaid = item.ERASTaxPaid,
                                    ModifiedBy = SessionManager.UserID,
                                    ModifiedDate = CommUtil.GetCurrentDateTime(),
                                };

                                b.Balance = item.ERASAssessed - item.ERASTaxPaid;
                                _db.NewTCCDetailsHolds.Add(new NewTCCDetailsHold
                                {
                                    RowID = item.RowID,
                                    TaxYear = item.TaxYear,
                                    TBKID = item.TBKID,
                                    AssessableIncome = item.AssessableIncome,
                                    TCCTaxPaid = item.TCCTaxPaid,
                                    ERASAssessed = item.ERASAssessed,
                                    ERASTaxPaid = item.ERASTaxPaid,
                                    Tax_receipt = item.Tax_receipt,
                                    IndividualRIN = mObjRequestData.IndividualRIN
                                });


                                mObjFuncResponse = mObjBLTCC.BL_InsertUpdateTCCDetail(mObjTCCDetail);
                                if (!mObjFuncResponse.Success)
                                {
                                    throw (new Exception(mObjFuncResponse.Message));
                                }
                            }
                            lstb.Add(b);
                        }
                        if (strAction != "Save")
                        {
                            if (lstb.Any(o => o.Balance > 0))
                            {
                                FlashMessage.Danger("Balance Should Not Be Greater Than 0");
                                IList<DropDownListResult> lstAssetType = new BLAssetType().BL_GetAssetTypeDropDownList(pObjAssetType);
                                ViewBag.AssetTypeList = new SelectList(lstAssetType, "id", "text", 0);
                                ViewBag.ProfileInformation = lstProfileInformation;
                                ViewBag.AssessmentRuleInformation = lstAssessmentRuleInformation;
                                ViewBag.TaxPayerBill = lstTaxPayerBill;
                                ViewBag.TaxPayerLiability = lstTaxPayerLiability;

                                UI_FillTaxYearDropDown(mObjRequestData.TaxYear.GetValueOrDefault());
                                ViewBag.TaxBusiness = new SelectList(lstTaxPayerAsset.Where(o => o.AssetTypeID == 3 && (o.TaxPayerRoleID == 4 || o.TaxPayerRoleID == 5 || o.TaxPayerRoleID == 37 || o.TaxPayerRoleID == 39)).Select(t => new { id = t.AssetID, text = t.AssetName }).Distinct(), "id", "text");
                                ViewBag.TaxPayerRoleList = new SelectList(lstTaxPayerAsset.Select(t => new { id = t.TaxPayerRoleID, text = t.TaxPayerRoleName }).Distinct(), "id", "text");
                                UI_FillNatureOfBusinessDropDown();
                                ViewBag.RequestData = mObjRequestData;
                                ViewBag.IncomeStreamList = SessionManager.LstIncomeStream.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
                                ViewBag.TCCDetailList = SessionManager.LstTCCDetail.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
                                ViewBag.RequestData = mObjRequestData;
                                ViewBag.NewAssetList = newlstTaxPayerAsset;
                                ViewBag.ProfileInformation = lstProfileInformation;
                                ViewBag.AssessmentRuleInformation = lstAssessmentRuleInformation;
                                ViewBag.TaxPayerBill = lstTaxPayerBill;
                                ViewBag.TaxPayerPayment = lstTaxPayerPayment;
                                ViewBag.TaxPayerLiability = lstTaxPayerLiability;
                                UI_FillTaxYearDropDown(mObjRequestData.TaxYear.GetValueOrDefault());
                                ViewBag.TaxPayerRoleList = new SelectList(newlstTaxPayerAsset.Select(t => new { id = t.TaxPayerRoleID, text = t.TaxPayerRoleName }).Distinct(), "id", "text");

                                return View(pobjValidateTaxPayerIncomeModel);
                            }
                        }
                        SessionManager.LstTCCDetail = lstTCCDetail;

                        //Update Certificate
                        TaxClearanceCertificate mObjTCC = new TaxClearanceCertificate()
                        {
                            TCCID = pobjValidateTaxPayerIncomeModel.TCCID,
                            TaxPayerID = mObjRequestData.IndividualID,
                            TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                            TaxYear = mObjRequestData.TaxYear,
                            RequestRefNo = mObjRequestData.RequestRefNo,
                            IncomeSource = pobjValidateTaxPayerIncomeModel.SourceOfIncome,
                            SerialNumber = TrynParse.parseString(mObjRequestData.TCCRequestID),
                            TaxPayerDetails = pobjValidateTaxPayerIncomeModel.CertificateNotes,
                            StatusID = 1,
                            TCCDate = CommUtil.GetCurrentDateTime(),
                            CreatedBy = SessionManager.UserID,
                            CreatedDate = CommUtil.GetCurrentDateTime()
                        };

                        mObjFuncResponse = mObjBLTCC.BL_InsertUpdateTaxClearanceCertificate(mObjTCC);
                        if (!mObjFuncResponse.Success)
                        {
                            throw (new Exception(mObjFuncResponse.Message));
                        }

                        //Update Stage
                        MAP_TCCRequest_ValidateTaxPayerIncome mObjValidateIncome = new MAP_TCCRequest_ValidateTaxPayerIncome()
                        {
                            VTPIncomeID = pobjValidateTaxPayerIncomeModel.VTPIncomeID,
                            RequestID = pobjValidateTaxPayerIncomeModel.RequestID,
                            Notes = pobjValidateTaxPayerIncomeModel.Notes,
                            CreatedBy = SessionManager.UserID,
                            CreatedDate = CommUtil.GetCurrentDateTime()
                        };

                        mObjFuncResponse = mObjBLTCC.BL_InsertUpdateValidateIncome(mObjValidateIncome);

                        if (mObjFuncResponse.Success)
                        {
                            if (strAction != "Save")
                            {
                                MAP_TCCRequest_Stages mObjRequestStage1 = new MAP_TCCRequest_Stages()
                                {
                                    ApprovalDate = CommUtil.GetCurrentDateTime(),
                                    StageID = (int)NewTCCRequestStage.Validate_Tax_Payer_Income,
                                    StatusID = (int)TCCRequestStatus.Validated_Income,
                                    RequestID = mObjValidateIncome.RequestID
                                };

                                mObjBLTCC.BL_UpdateRequestStage(mObjRequestStage1);
                                MAP_TCCRequest_Stages mObjRequestStage = new MAP_TCCRequest_Stages()
                                {
                                    ApprovalDate = CommUtil.GetCurrentDateTime(),
                                    StageID = (int)NewTCCRequestStage.Prepare_TCC_Draft,
                                    StatusID = (int)TCCRequestStatus.Prepared_TCC_Draft,
                                    RequestID = mObjValidateIncome.RequestID
                                };

                                mObjBLTCC.BL_UpdateRequestStage(mObjRequestStage);

                                int year = DateTime.Now.Year;
                                DateTime lastDay = new DateTime(year, 12, 31);
                                MAP_TCCRequest_Generate mObjGenerate = new MAP_TCCRequest_Generate()
                                {
                                    RGID = 0,
                                    RequestID = pobjValidateTaxPayerIncomeModel.RequestID,
                                    Notes = pobjValidateTaxPayerIncomeModel.Notes,
                                    PDFTemplateID = GlobalDefaultValues.TCC_PDFTemplateID,
                                    IsExpirable = true,
                                    ExpiryDate = lastDay,
                                    Reason = "",
                                    Location = "",
                                    StageID = (int)NewTCCRequestStage.Generate_eTCC,
                                    ApprovalDate = CommUtil.GetCurrentDateTime(),
                                    IsAction = false,
                                    CreatedBy = SessionManager.UserID,
                                    CreatedDate = CommUtil.GetCurrentDateTime()
                                };

                                //mObjFuncResponse = mObjBLTCC.BL_InsertUpdateTCCRequestGenerate(mObjGenerate);
                                FuncResponse<MAP_TCCRequest_Generate> mNewObjFuncResponse = mObjBLTCC.BL_InsertUpdateTCCRequestGenerate(mObjGenerate);
                                if (pobjValidateTaxPayerIncomeModel.needBusinessName == true)
                                {
                                    bnLst.RemoveAll(item => item.BusinessName1 == null);

                                    if (bnLst.Any())
                                    {
                                        List<BusinessName> nameLst = new List<BusinessName>();

                                        foreach (var b in bnLst)
                                        {
                                            BusinessName name = new BusinessName();
                                            name.BusinessName1 = b.BusinessName1;
                                            name.TccRequestId = pobjValidateTaxPayerIncomeModel.RequestID;
                                            nameLst.Add(name);
                                        }
                                        _db.BusinessNames.AddRange(nameLst);
                                        //SessionManager.businessNameHolderList = bnLst;
                                    }
                                }
                            }
                            SessionManager.LstTicketRef = ticketRefs;
                            _db.SaveChanges();
                            mObjTransactionScope.Complete();
                            return RedirectToAction("Details", "ProcessTCCRequest", new { reqid = pobjValidateTaxPayerIncomeModel.RequestID });

                        }
                        else
                        {
                            ViewBag.Message = mObjFuncResponse.Message;
                            ViewBag.IncomeStreamList = SessionManager.LstIncomeStream.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
                            ViewBag.TCCDetailList = SessionManager.LstTCCDetail.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
                            ViewBag.RequestData = mObjRequestData;
                            ViewBag.NewAssetList = newlstTaxPayerAsset;
                            ViewBag.ProfileInformation = lstProfileInformation;
                            ViewBag.AssessmentRuleInformation = lstAssessmentRuleInformation;
                            ViewBag.TaxPayerBill = lstTaxPayerBill;
                            ViewBag.TaxPayerPayment = lstTaxPayerPayment;
                            ViewBag.TaxPayerLiability = lstTaxPayerLiability;
                            UI_FillTaxYearDropDown(mObjRequestData.TaxYear.GetValueOrDefault());
                            ViewBag.TaxPayerRoleList = new SelectList(newlstTaxPayerAsset.Select(t => new { id = t.TaxPayerRoleID, text = t.TaxPayerRoleName }).Distinct(), "id", "text");
                            Transaction.Current.Rollback();
                            return View(pobjValidateTaxPayerIncomeModel);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.SendErrorToText(ex);
                        Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                        UI_FillTaxYearDropDown(mObjRequestData.TaxYear.GetValueOrDefault());
                        ViewBag.TaxBusiness = new SelectList(lstTaxPayerAsset.Where(o => o.AssetTypeID == 3 && (o.TaxPayerRoleID == 4 || o.TaxPayerRoleID == 5 || o.TaxPayerRoleID == 37 || o.TaxPayerRoleID == 39)).Select(t => new { id = t.AssetID, text = t.AssetName }).Distinct(), "id", "text");
                        ViewBag.TaxPayerRoleList = new SelectList(lstTaxPayerAsset.Select(t => new { id = t.TaxPayerRoleID, text = t.TaxPayerRoleName }).Distinct(), "id", "text");
                        UI_FillNatureOfBusinessDropDown();
                        ViewBag.RequestData = mObjRequestData;
                        ViewBag.IncomeStreamList = SessionManager.LstIncomeStream.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
                        ViewBag.TCCDetailList = SessionManager.LstTCCDetail.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
                        ViewBag.RequestData = mObjRequestData;
                        ViewBag.NewAssetList = newlstTaxPayerAsset;
                        ViewBag.ProfileInformation = lstProfileInformation;
                        ViewBag.AssessmentRuleInformation = lstAssessmentRuleInformation;
                        ViewBag.TaxPayerBill = lstTaxPayerBill;
                        ViewBag.TaxPayerPayment = lstTaxPayerPayment;
                        ViewBag.TaxPayerLiability = lstTaxPayerLiability;
                        UI_FillTaxYearDropDown(mObjRequestData.TaxYear.GetValueOrDefault());
                        ViewBag.TaxPayerRoleList = new SelectList(newlstTaxPayerAsset.Select(t => new { id = t.TaxPayerRoleID, text = t.TaxPayerRoleName }).Distinct(), "id", "text");

                        return View(pobjValidateTaxPayerIncomeModel);
                    }
                }
            }
        }

        private void UI_FillTaxYearDropDown(int EndTaxYear)
        {
            IList<DropDownListResult> lstYear = new List<DropDownListResult>();
            int mIntCurrentYear = EndTaxYear - 2;//CommUtil.GetCurrentDateTime().AddYears(-1).Year;
            for (int i = mIntCurrentYear; i <= EndTaxYear; i++)
            {
                lstYear.Add(new DropDownListResult() { id = i, text = i.ToString() });
            }

            ViewBag.YearList = new SelectList(lstYear, "id", "text");
        }
        private void UI_FillNatureOfBusinessDropDown()
        {
            IList<SelectListItem> lstres = new List<SelectListItem>();
            using (var _db = new EIRSEntities())
            {
                var res = _db.NatureOfBusinesses.ToList();
                res.Add(new NatureOfBusiness { Id = 0, NatureOfBusinessName = "---Select One----" });

                res = res.OrderBy(x => x.Id).ToList();
                foreach (var i in res)
                {
                    lstres.Add(new SelectListItem() { Text = i.NatureOfBusinessName, Value = i.NatureOfBusinessName });
                }
            }
            ViewBag.lstresList = new SelectList(lstres, "Text", "Value");
        }

        [HttpGet]
        public ActionResult GenerateTCCDetail(long? reqid)
        {
            if (reqid.GetValueOrDefault() > 0)
            {
                BLTCC mObjBLTCC = new BLTCC();
                usp_GetTCCRequestDetails_Result mObjRequestData = mObjBLTCC.BL_GetRequestDetails(reqid.GetValueOrDefault());

                if (mObjRequestData != null)
                {
                    ViewBag.RequestData = mObjRequestData;

                    MAP_TCCRequest_GenerateTCCDetail mObjGenerateTCCDetail = mObjBLTCC.BL_GetGenerateTCCDetailDetails(mObjRequestData.TCCRequestID);

                    GenerateTCCDetailViewModel mObjGenerateTCCModel = new GenerateTCCDetailViewModel()
                    {
                        RequestID = mObjRequestData.TCCRequestID
                    };

                    if (mObjGenerateTCCDetail != null)
                    {
                        mObjGenerateTCCModel.GTCCDetailID = mObjGenerateTCCDetail.GTCCDetailID;
                        mObjGenerateTCCModel.Notes = mObjGenerateTCCDetail.Notes;
                    }

                    IList<usp_GetTCCDetail_Result> lstTCCDetail = mObjBLTCC.BL_GetTCCDetail(mObjRequestData.IndividualID, (int)EnumList.TaxPayerType.Individual, mObjRequestData.TaxYear.GetValueOrDefault());
                    ViewBag.TCCDetailList = lstTCCDetail;

                    return View(mObjGenerateTCCModel);
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
        public ActionResult GenerateTCCDetail(GenerateTCCDetailViewModel pobjGenerateTCCModel)
        {
            BLTCC mObjBLTCC = new BLTCC();
            usp_GetTCCRequestDetails_Result mObjRequestData = mObjBLTCC.BL_GetRequestDetails(pobjGenerateTCCModel.RequestID);

            IList<usp_GetTCCDetail_Result> lstTCCDetail = mObjBLTCC.BL_GetTCCDetail(mObjRequestData.IndividualID, (int)EnumList.TaxPayerType.Individual, mObjRequestData.TaxYear.GetValueOrDefault());
            ViewBag.TCCDetailList = lstTCCDetail;

            if (!ModelState.IsValid)
            {
                ViewBag.RequestData = mObjRequestData;
                return View(pobjGenerateTCCModel);
            }
            else
            {
                try
                {
                    //Update 
                    MAP_TCCRequest_GenerateTCCDetail mObjGenerateTCCDetail = new MAP_TCCRequest_GenerateTCCDetail()
                    {
                        GTCCDetailID = pobjGenerateTCCModel.GTCCDetailID,
                        RequestID = pobjGenerateTCCModel.RequestID,
                        Notes = pobjGenerateTCCModel.Notes,
                        CreatedBy = SessionManager.UserID,
                        CreatedDate = CommUtil.GetCurrentDateTime()
                    };

                    FuncResponse mObjFuncResponse = mObjBLTCC.BL_InsertUpdateGenerateTCCDetail(mObjGenerateTCCDetail);

                    if (mObjFuncResponse.Success)
                    {
                        //Update Stage Status
                        MAP_TCCRequest_Stages mObjRequestStage = new MAP_TCCRequest_Stages()
                        {
                            ApprovalDate = CommUtil.GetCurrentDateTime(),
                            StageID = (int)NewTCCRequestStage.Generate_TCC_Detail,
                            StatusID = (int)TCCRequestStatus.Generated_TCC_Details,
                            RequestID = pobjGenerateTCCModel.RequestID
                        };

                        mObjBLTCC.BL_UpdateRequestStage(mObjRequestStage);

                        return RedirectToAction("Details", "ProcessTCCRequest", new { reqid = pobjGenerateTCCModel.RequestID });

                    }
                    else
                    {
                        ViewBag.RequestData = mObjRequestData;
                        ViewBag.Message = mObjFuncResponse.Message;
                        return View(pobjGenerateTCCModel);
                    }
                }
                catch (Exception ex)
                {
                    Logger.SendErrorToText(ex);
                    ViewBag.RequestData = mObjRequestData;
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving generate tcc details";
                    return View(pobjGenerateTCCModel);
                }
            }
        }

        [HttpGet]
        public ActionResult PrepareTCCDraft(long? reqid)
        {
            if (reqid.GetValueOrDefault() > 0)
            {
                BLTCC mObjBLTCC = new BLTCC();
                usp_GetTCCRequestDetails_Result mObjRequestData = mObjBLTCC.BL_GetRequestDetails(reqid.GetValueOrDefault());

                if (mObjRequestData != null)
                {
                    ViewBag.RequestData = mObjRequestData;

                    MAP_TCCRequest_PrepareTCCDraft mObjPrepareTCCDraft = mObjBLTCC.BL_GetPrepareTCCDraftDetails(mObjRequestData.TCCRequestID);

                    PrepareTCCDraftViewModel mObjPrepareDraftModel = new PrepareTCCDraftViewModel()
                    {
                        RequestID = mObjRequestData.TCCRequestID
                    };

                    if (mObjPrepareTCCDraft != null)
                    {
                        mObjPrepareDraftModel.PTCCDraftID = mObjPrepareTCCDraft.PTCCDraftID;
                        mObjPrepareDraftModel.Notes = mObjPrepareTCCDraft.Notes;
                    }

                    IList<usp_GetTaxClearanceCertificateDetails_Result> lstTCC = mObjBLTCC.BL_GetTaxClearanceCertificateList(new TaxClearanceCertificate() { TaxPayerID = mObjRequestData.IndividualID, TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual });
                    ViewBag.TaxClearanceCertificateList = lstTCC.Where(t => t.TaxYear == mObjRequestData.TaxYear).ToList();

                    return View(mObjPrepareDraftModel);
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
        public ActionResult PrepareTCCDraft(PrepareTCCDraftViewModel pobjPrepareDraftModel)
        {
            BLTCC mObjBLTCC = new BLTCC();
            usp_GetTCCRequestDetails_Result mObjRequestData = mObjBLTCC.BL_GetRequestDetails(pobjPrepareDraftModel.RequestID);

            IList<usp_GetTaxClearanceCertificateDetails_Result> lstTCC = mObjBLTCC.BL_GetTaxClearanceCertificateList(new TaxClearanceCertificate() { TaxPayerID = mObjRequestData.IndividualID, TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual });
            ViewBag.TaxClearanceCertificateList = lstTCC;

            if (!ModelState.IsValid)
            {
                ViewBag.RequestData = mObjRequestData;
                return View(pobjPrepareDraftModel);
            }
            else
            {
                try
                {
                    //Update 
                    MAP_TCCRequest_PrepareTCCDraft mObjPrepareTCCDraft = new MAP_TCCRequest_PrepareTCCDraft()
                    {
                        PTCCDraftID = pobjPrepareDraftModel.PTCCDraftID,
                        RequestID = pobjPrepareDraftModel.RequestID,
                        Notes = pobjPrepareDraftModel.Notes,
                        CreatedBy = SessionManager.UserID,
                        CreatedDate = CommUtil.GetCurrentDateTime()
                    };

                    FuncResponse mObjFuncResponse = mObjBLTCC.BL_InsertUpdatePrepareTCCDraft(mObjPrepareTCCDraft);

                    if (mObjFuncResponse.Success)
                    {
                        int year = DateTime.Now.Year;
                        DateTime lastDay = new DateTime(year, 12, 31);
                        //Update 
                        MAP_TCCRequest_Generate mObjGenerate = new MAP_TCCRequest_Generate()
                        {
                            RGID = 0,
                            RequestID = pobjPrepareDraftModel.RequestID,
                            Notes = pobjPrepareDraftModel.Notes,
                            PDFTemplateID = GlobalDefaultValues.TCC_PDFTemplateID,
                            IsExpirable = true,
                            ExpiryDate = lastDay,
                            Reason = pobjPrepareDraftModel.Reason,
                            Location = pobjPrepareDraftModel.Location,
                            StageID = (int)NewTCCRequestStage.Generate_eTCC,
                            ApprovalDate = CommUtil.GetCurrentDateTime(),
                            IsAction = false,
                            CreatedBy = SessionManager.UserID,
                            CreatedDate = CommUtil.GetCurrentDateTime()
                        };

                        //mObjFuncResponse = mObjBLTCC.BL_InsertUpdateTCCRequestGenerate(mObjGenerate);
                        FuncResponse<MAP_TCCRequest_Generate> mNewObjFuncResponse = mObjBLTCC.BL_InsertUpdateTCCRequestGenerate(mObjGenerate);
                        if (mNewObjFuncResponse.Success)
                        {//Update Stage Status
                            MAP_TCCRequest_Stages mObjRequestStage = new MAP_TCCRequest_Stages()
                            {
                                ApprovalDate = CommUtil.GetCurrentDateTime(),
                                StageID = (int)NewTCCRequestStage.Prepare_TCC_Draft,
                                StatusID = (int)TCCRequestStatus.Prepared_TCC_Draft,
                                RequestID = pobjPrepareDraftModel.RequestID
                            };

                            mObjBLTCC.BL_UpdateRequestStage(mObjRequestStage);

                        }
                        return RedirectToAction("Details", "ProcessTCCRequest", new { reqid = pobjPrepareDraftModel.RequestID });
                    }
                    else
                    {
                        ViewBag.RequestData = mObjRequestData;
                        ViewBag.Message = mObjFuncResponse.Message;
                        return View(pobjPrepareDraftModel);
                    }
                }
                catch (Exception ex)
                {
                    Logger.SendErrorToText(ex);
                    ViewBag.RequestData = mObjRequestData;
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving generate tcc details";
                    return View(pobjPrepareDraftModel);
                }
            }
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
        [HttpGet]
        public ActionResult PreviewDoc(GenerateViewModel pobjGenerateModel)
        {
            string name = SessionManager.Path;
            string pdfPath = Server.MapPath(name);
            WebClient client = new WebClient();
            Byte[] buffer = client.DownloadData(pdfPath);
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-length", buffer.Length.ToString());
            Response.BinaryWrite(buffer);

            return View();
        }
        [HttpGet]
        public FileResult GetTCC()
        {
            string name = SessionManager.Path;
            string ReportURL = name;
            byte[] FileBytes = System.IO.File.ReadAllBytes(ReportURL);

            return File(FileBytes, "application/pdf");
        }

        [HttpGet]
        public async Task<ActionResult> GenerateTCC(long? reqid)
        {
            if (reqid.GetValueOrDefault() > 0)
            {
                var url = tccAPI;
                var lastyear = new Request_TCCDetail();
                var last2year = new Request_TCCDetail();
                var last3year = new Request_TCCDetail();
                var lastyear1 = new usp_GetTCCDetail_Result();
                var last2year1 = new usp_GetTCCDetail_Result();
                var last3year1 = new usp_GetTCCDetail_Result();
                var txxx = new TaxClearanceCertificate();

                var bnLst = new List<BusinessName>();
                using (var ddd = new EIRSEntities())
                {
                    bnLst = ddd.BusinessNames.Where(o => o.TccRequestId == reqid).ToList();
                    txxx = ddd.TaxClearanceCertificates.FirstOrDefault(o => o.SerialNumber == reqid.ToString());
                }
                string busiName = "";

                if (bnLst.Any())
                {
                    var distinctStrings = bnLst.GroupBy(v => v.BusinessName1).ToList();
                    if (distinctStrings.Count > 1)
                    {
                        foreach (var item in distinctStrings)
                        {
                            busiName += $"{item.Key},";
                        }
                        busiName = busiName.Remove(busiName.Length - 1);
                        busiName = busiName.TrimStart(',');
                    }
                    else
                    {
                        busiName = distinctStrings.FirstOrDefault().Key;
                    }
                }
                BLTCC mObjBLTCC = new BLTCC();
                var ret = mObjBLTCC.BL_GetTCCRequestGenerateDetails((long)reqid);
                usp_GetTCCRequestDetails_Result mObjRequestData = mObjBLTCC.BL_GetRequestDetails(reqid.GetValueOrDefault());
                if (mObjRequestData != null)
                {
                    var eget = "1";
                    var curYear = DateTime.Now.Year;
                    if (mObjRequestData.TaxYear == curYear)
                        mObjRequestData.TaxYear = curYear - 1;
                    usp_GetTCCRequestDetails_Result mObjRequestDatanew = mObjBLTCC.BL_GetRequestDetails(reqid.GetValueOrDefault());
                    IList<usp_GetTCCDetail_Result> lstTCCDetail = mObjBLTCC.BL_GetTCCDetail(mObjRequestData.IndividualID, (int)EnumList.TaxPayerType.Individual, mObjRequestData.TaxYear.GetValueOrDefault());

                    IList<Request_TCCDetail> tccDetails = SessionManager.LstTCCDetail;

                    IList<usp_GetTaxClearanceCertificateDetails_Result> lstTCC = mObjBLTCC.BL_GetTaxClearanceCertificateList(new TaxClearanceCertificate() { TaxPayerID = mObjRequestData.IndividualID, TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual });
                    var certNumber = lstTCC.Where(t => t.TaxYear == mObjRequestData.TaxYear).ToList();
                    if (tccDetails != null)
                    {
                        eget = "2";
                        lastyear = tccDetails.Where(t => t.TaxYear == (mObjRequestData.TaxYear)).FirstOrDefault();
                        last2year = tccDetails.Where(t => t.TaxYear == (mObjRequestData.TaxYear - 1)).FirstOrDefault();
                        last3year = tccDetails.Where(t => t.TaxYear == (mObjRequestData.TaxYear - 2)).FirstOrDefault();
                    }
                    else
                    {
                        if (lstTCCDetail != null)
                        {
                            eget = "3";
                            lastyear1 = lstTCCDetail.Where(t => t.TaxYear == (mObjRequestData.TaxYear)).FirstOrDefault();
                            last2year1 = lstTCCDetail.Where(t => t.TaxYear == (mObjRequestData.TaxYear - 1)).FirstOrDefault();
                            last3year1 = lstTCCDetail.Where(t => t.TaxYear == (mObjRequestData.TaxYear - 2)).FirstOrDefault();
                        }

                    }
                    //get receipt
                    IList<TicketRef> trf = SessionManager.LstTicketRef ?? new List<TicketRef>();
                    usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(new Individual() { intStatus = 1, IndividualID = mObjRequestData.IndividualID });
                    string mStrDirectory = $"{GlobalDefaultValues.DocumentLocation}/ETCC/{mObjRequestData.IndividualID}/";
                    string mStrGeneratedFileName = $"{mObjRequestData.IndividualID}_{DateTime.Now:ddMMyyyy}_Generated.pdf";//mObjTreasuryData.ReceiptRefNo + "_" + DateTime.Now.ToString("_ddMMyyyy") + "_TR.pdf";
                    string mStrGeneratedDocumentPath = Path.Combine(mStrDirectory, mStrGeneratedFileName);
                    string mStrGeneratedBarCodePath = Path.Combine(mStrDirectory + "/Temp/Barcode/");
                    string mStrGeneratedHtmlPath = Path.Combine(mStrDirectory + "/Temp/Html", mObjRequestData.IndividualID + "_template.html");
                    string mStrDirectoryForPrint = $"{GlobalDefaultValues.DocumentLocation}/ETCC/Print/{mObjRequestData.IndividualID}/";
                    string mStrGeneratedFileNameForPrint = $"{mObjRequestData.IndividualID}_{DateTime.Now:ddMMyyyy}_Generated.pdf";//mObjTreasuryData.ReceiptRefNo + "_" + DateTime.Now.ToString("_ddMMyyyy") + "_TR.pdf";
                    string mStrGeneratedDocumentPathForPrint = Path.Combine(mStrDirectoryForPrint, mStrGeneratedFileNameForPrint);
                    string mStrGeneratedBarCodePathForPrint = Path.Combine(mStrDirectoryForPrint + "/Temp/Barcode/");
                    string mStrGeneratedHtmlPathForPrint = Path.Combine(mStrDirectoryForPrint + "/Temp/Html", mObjRequestData.IndividualID + "_template.html");

                    string mHtmlDirectory = $"{DocumentHTMLLocation}/Personal-eTCCForAllYears.html";
                    string mHtmlDirectoryForPrint = $"{DocumentHTMLLocation}/EtccForPrintCase.html";
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
                    string tin = "", firstName = "", lastName = "", streciptanddate = "", ndreciptanddate = "", rdreciptanddate = "", rin = "", station = "", title = "", address = "", certificateNumber = "", serialNumber = "", incomeSource = "";
                    TicketRef streciptand = new TicketRef();
                    TicketRef ndreciptand = new TicketRef();
                    TicketRef rdreciptand = new TicketRef();
                    //if (trf.Count > 0)
                    //{
                    //    streciptand = trf.Where(o => o.TaxYear == mObjRequestData.TaxYear.ToString()).FirstOrDefault();
                    //    if (streciptand != null)
                    //        if ((streciptand.TickRefNo != null) && (streciptand.PaymentDate != null))
                    //            streciptanddate = $"{streciptand.TickRefNo} || {streciptand.PaymentDate}";
                    //        else
                    //            streciptanddate = tccDetails.FirstOrDefault(o => o.TaxYear == mObjRequestData.TaxYear).Tax_receipt;
                    //    else
                    //        streciptanddate = tccDetails.FirstOrDefault(o => o.TaxYear == mObjRequestData.TaxYear).Tax_receipt;
                    //    ndreciptand = trf.Where(o => o.TaxYear == (mObjRequestData.TaxYear - 1).ToString()).FirstOrDefault();
                    //    if (ndreciptand != null)
                    //        if ((ndreciptand.TickRefNo != null) && (ndreciptand.PaymentDate != null))
                    //            ndreciptanddate = $"{ndreciptand.TickRefNo} || {ndreciptand.PaymentDate}";
                    //        else
                    //            ndreciptanddate = tccDetails.FirstOrDefault(o => o.TaxYear == mObjRequestData.TaxYear - 1).Tax_receipt;
                    //    else
                    //        ndreciptanddate = tccDetails.FirstOrDefault(o => o.TaxYear == mObjRequestData.TaxYear - 1).Tax_receipt;
                    //    rdreciptand = trf.Where(o => o.TaxYear == (mObjRequestData.TaxYear - 2).ToString()).FirstOrDefault();
                    //    if (rdreciptand != null)
                    //        if ((rdreciptand.TickRefNo != null) && (rdreciptand.PaymentDate != null))
                    //            rdreciptanddate = $"{rdreciptand.TickRefNo} || {rdreciptand.PaymentDate}";
                    //        else
                    //            rdreciptanddate = tccDetails.FirstOrDefault(o => o.TaxYear == mObjRequestData.TaxYear - 2).Tax_receipt;
                    //    else
                    //        rdreciptanddate = tccDetails.FirstOrDefault(o => o.TaxYear == mObjRequestData.TaxYear - 2).Tax_receipt;
                    //}
                    //else
                    //{
                    var allRef = _db.TccRefHolders.Where(o => o.ReqId == reqid.ToString()).ToList();

                    // var newlstTaxPayerPayment = lstTaxPayerPayment.Where(o => o.AssessmentYear == (currentYear - 1)).ToList();
                    streciptanddate = allRef.FirstOrDefault(o => o.TaxYear == mObjRequestData.TaxYear) != null ? allRef.FirstOrDefault(o => o.TaxYear == mObjRequestData.TaxYear).ReciptRef : "";
                    ndreciptanddate = allRef.FirstOrDefault(o => o.TaxYear == mObjRequestData.TaxYear - 1) != null ? allRef.FirstOrDefault(o => o.TaxYear == mObjRequestData.TaxYear - 1).ReciptRef : "";
                    rdreciptanddate = allRef.FirstOrDefault(o => o.TaxYear == mObjRequestData.TaxYear - 2) != null ? allRef.FirstOrDefault(o => o.TaxYear == mObjRequestData.TaxYear - 2).ReciptRef : "";
                    //}
                    string money1 = "", money2 = "", money3 = "";
                    string money1a = "", money2a = "", money3a = "";
                    string money1b = "", money2b = "", money3b = "", senty = "";
                    decimal x = 0, y = 0, z = 0, l = 0;
                    switch (eget)
                    {
                        case "2":
                            x = lastyear != null ? lastyear.ERASTaxPaid : 0;
                            y = lastyear != null ? last2year.ERASTaxPaid : 0;
                            z = lastyear != null ? last3year.ERASTaxPaid : 0;
                            l = x + y + z;
                            senty = CommUtil.GetFormatedCurrency(l);
                            money1 = lastyear != null ? CommUtil.GetFormatedCurrency(lastyear.ERASTaxPaid) : CommUtil.GetFormatedCurrency(0);
                            money2 = last2year != null ? CommUtil.GetFormatedCurrency(last2year.ERASTaxPaid) : CommUtil.GetFormatedCurrency(0);
                            money3 = last3year != null ? CommUtil.GetFormatedCurrency(last3year.ERASTaxPaid) : CommUtil.GetFormatedCurrency(0);
                            money1a = lastyear != null ? CommUtil.GetFormatedCurrency(lastyear.AssessableIncome) : CommUtil.GetFormatedCurrency(0);
                            money2a = last2year != null ? CommUtil.GetFormatedCurrency(last2year.AssessableIncome) : CommUtil.GetFormatedCurrency(0);
                            money3a = last3year != null ? CommUtil.GetFormatedCurrency(last3year.AssessableIncome) : CommUtil.GetFormatedCurrency(0);
                            money1b = lastyear != null ? CommUtil.GetFormatedCurrency(lastyear.ERASAssessed) : CommUtil.GetFormatedCurrency(0);
                            money2b = last2year != null ? CommUtil.GetFormatedCurrency(last2year.ERASAssessed) : CommUtil.GetFormatedCurrency(0);
                            money3b = last3year != null ? CommUtil.GetFormatedCurrency(last3year.ERASAssessed) : CommUtil.GetFormatedCurrency(0);
                            break;
                        case "3":
                            x = lastyear != null ? lastyear.ERASTaxPaid : 0;
                            y = lastyear != null ? last2year.ERASTaxPaid : 0;
                            z = lastyear != null ? last3year.ERASTaxPaid : 0;
                            l = x + y + z;
                            senty = CommUtil.GetFormatedCurrency(l);
                            money1 = lastyear1 != null ? CommUtil.GetFormatedCurrency(lastyear.ERASTaxPaid) : CommUtil.GetFormatedCurrency(0);
                            money2 = last2year1 != null ? CommUtil.GetFormatedCurrency(last2year.ERASTaxPaid) : CommUtil.GetFormatedCurrency(0);
                            money3 = last3year1 != null ? CommUtil.GetFormatedCurrency(last3year.ERASTaxPaid) : CommUtil.GetFormatedCurrency(0);
                            money1a = lastyear1 != null ? CommUtil.GetFormatedCurrency(lastyear.AssessableIncome) : CommUtil.GetFormatedCurrency(0);
                            money2a = last2year1 != null ? CommUtil.GetFormatedCurrency(last2year.AssessableIncome) : CommUtil.GetFormatedCurrency(0);
                            money3a = last3year1 != null ? CommUtil.GetFormatedCurrency(last3year.AssessableIncome) : CommUtil.GetFormatedCurrency(0);
                            money1b = lastyear1 != null ? CommUtil.GetFormatedCurrency(lastyear.ERASAssessed) : CommUtil.GetFormatedCurrency(0);
                            money2b = last2year1 != null ? CommUtil.GetFormatedCurrency(last2year.ERASAssessed) : CommUtil.GetFormatedCurrency(0);
                            money3b = last3year1 != null ? CommUtil.GetFormatedCurrency(last3year.ERASAssessed) : CommUtil.GetFormatedCurrency(0);

                            break;
                        default:
                            break;
                    }
                    serialNumber = certNumber.Select(o => o.SerialNumber).FirstOrDefault();
                    certificateNumber = certNumber.Select(o => o.TCCNumber).FirstOrDefault();
                    tin = mObjRequestData.TIN;
                    lastName = mObjRequestData.LastName;
                    firstName = mObjRequestData.FirstName;
                    rin = mObjRequestData.IndividualRIN;
                    title = mObjIndividualData.TitleName;
                    address = mObjIndividualData.ContactAddress;
                    station = mObjIndividualData.TaxOfficeName;
                    incomeSource = txxx != null ? txxx.IncomeSource : "";
                    string fullName = $"{firstName} {lastName}";
                    var SigBase = BrCode(fullName, rin, certificateNumber);
                    var SigBase64 = $"data:image/png;base64,{SigBase}";
                    string marksheet = System.IO.File.ReadAllText(mHtmlDirectory);
                    string marksheetForPrint = System.IO.File.ReadAllText(mHtmlDirectoryForPrint);

                    using (HttpClient client = new HttpClient())
                    {
                        try
                        {
                            RequestPayload t = new RequestPayload();
                            t.money1 = money1;
                            t.senty = senty;
                            t.money1a = money1a;
                            t.money2 = money2;
                            t.money2a = money2a;
                            t.money3 = money3;
                            t.money3a = money3a;
                            t.money1b = money1b;
                            t.money2b = money2b;
                            t.money3b = money3b;
                            t.marksheet = marksheet;
                            t.marksheetForPrint = marksheetForPrint;
                            t.rin = rin;
                            t.station = station;
                            t.busiName = busiName;
                            t.tin = tin;
                            t.title = title;
                            t.firstName = firstName;
                            t.lastName = lastName;
                            t.certificateNumber = certificateNumber;
                            t.serialNumber = serialNumber;
                            t.incomeSource = incomeSource;
                            t.rdreciptanddate = rdreciptanddate;
                            t.ndreciptanddate = ndreciptanddate;
                            t.streciptanddate = streciptanddate;
                            t.address = address;
                            t.SigBase64 = SigBase64;
                            t.mStrGeneratedHtmlPath = mStrGeneratedHtmlPath;
                            t.mStrGeneratedHtmlPathForPrint = mStrGeneratedHtmlPathForPrint;
                            t.mStrGeneratedDocumentPath = mStrGeneratedDocumentPath;
                            t.mStrGeneratedDocumentPathForPrint = mStrGeneratedDocumentPathForPrint;
                            // Convert the data to JSON
                            string jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(t);

                            // Create a StringContent with the JSON data
                            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                            // Make a POST request
                            HttpResponseMessage response = await client.PostAsync(url, content);

                            // Check if the request was successful
                            //if (response.IsSuccessStatusCode)
                            //{
                            //    // Read and display the response content
                            //    var result = await response.Content.ReadAsStringAsync();
                            //    if (Convert.ToBoolean(result) == false)
                            //    {
                            //        return RedirectToAction("List", "ProcessTCCRequest");
                            //    }
                            //}
                            //else
                            //{
                            //    return RedirectToAction("List", "ProcessTCCRequest");
                            //}
                        }
                        catch (Exception ex)
                        {
                            // Handle any exceptions that may occur during the request
                            ViewBag.ApiResponse = $"Exception: {ex.Message}";
                        }
                    }
                    ViewBag.RequestData = mObjRequestData;
                    ViewBag.pdf = mStrGeneratedDocumentPath;
                    GenerateViewModel mObjGenerateTCCModel = new GenerateViewModel()
                    {
                        RequestID = mObjRequestData.TCCRequestID,
                    };
                    mObjGenerateTCCModel.RGID = ret.RGID;
                    mObjGenerateTCCModel.GenerateNotes = ret.Notes;
                    mObjGenerateTCCModel.ExpiryDate = ret.ExpiryDate;
                    mObjGenerateTCCModel.IsExpirable = true;
                    mObjGenerateTCCModel.Reason = ret.Reason;
                    mObjGenerateTCCModel.Location = ret.Location;
                    mObjGenerateTCCModel.PDFTemplateID = mObjRequestData.PDFTemplateID.GetValueOrDefault();
                    mObjGenerateTCCModel.SEDE_DocumentID = mObjRequestData.SEDE_DocumentID.GetValueOrDefault();

                    // lstTemplateField = SEDEFunction.PDFTemplateFieldList(GlobalDefaultValues.TCC_PDFTemplateID, mObjGenerateTCCModel.SEDE_DocumentID);

                    TCC_Request mObjUpdateStatus = new TCC_Request()
                    {
                        TCCRequestID = mObjRequestData.TCCRequestID,
                        StatusID = (int)NewTCCRequestStage.Waiting_For_First_Signature,
                        ModifiedBy = SessionManager.UserID,
                        SEDE_OrderID = (long)TCCSigningStage.AwaitingFirstSigner,
                        ValidatedPath = "ETCC/" + mObjRequestData.IndividualID + "/Temp/Html/" + mObjRequestData.IndividualID + "_template.html",
                        GeneratePathForPrint = "ETCC/Print/" + mObjRequestData.IndividualID + "/Temp/Html/" + mObjRequestData.IndividualID + "_template.html",
                        GeneratedPath = "ETCC/" + mObjRequestData.IndividualID + "/" + mStrGeneratedFileName,
                        ModifiedDate = CommUtil.GetCurrentDateTime(),
                        RequestDate = CommUtil.GetCurrentDateTime()
                    };
                    mObjBLTCC.BL_UpdateRequestStatus(mObjUpdateStatus);
                    //Byte[] bytesArray = System.IO.File.ReadAllBytes(mStrGeneratedDocumentPath);
                    //string file = Convert.ToBase64String(bytesArray);
                    ValidateTcc tcc = new ValidateTcc()
                    {
                        DateCreated = DateTime.Now,
                        Fullname = fullName,
                        Taxyear = mObjRequestData.TaxYear.ToString(),
                        TaxpayerRIN = rin,
                        TaxpayerTIN = tin,
                        TCCCertificateNumber = certificateNumber,
                        DateofTCCissued = DateTime.Now,
                        TCCpdf = "file",
                        TccRequestId = mObjRequestData.TCCRequestID
                    };

                    using (_db = new EIRSEntities())
                    {
                        _db.ValidateTccs.Add(tcc);
                        _db.SaveChanges();
                    }

                    //send mail to the signer
                    //    int yr = DateTime.Now.Year;
                    //   string[] separatingStrings = { "||" };
                    //  string[] words = first_Signer.Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);
                    //   var strlist = first_Signer.Split("||".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToArray();
                    //EmailDetails mObjEmailDetails = new EmailDetails()
                    //{
                    //    FirstSignerEmail = words[0],
                    //    FirstSignerName = words[1],
                    //    TaxPayerRIN = rin,
                    //    CertificateID = certificateNumber,
                    //    ExpiryDate = $"12/31/{yr}",
                    //    TaxPayerName = fullName,
                    //    TaxYearCovered = $"{yr - 1},{yr - 2},{yr - 3}"
                    //};

                    //if (!string.IsNullOrWhiteSpace(mObjEmailDetails.FirstSignerEmail))
                    //{
                    //    BLEmailHandler.BL_TccSignerAsync(mObjEmailDetails);
                    //}
                    SessionManager.Path = mStrGeneratedDocumentPath;
                    ViewBag.path = GlobalDefaultValues.DocumentLocation;
                    return View(mObjGenerateTCCModel);
                }

                return RedirectToAction("List", "ProcessTCCRequest");

            }
            else
            {
                return RedirectToAction("List", "ProcessTCCRequest");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult GenerateTCC(GenerateViewModel pobjGenerateModel, FormCollection pObjFormCollection)
        {
            BLTCC mObjBLTCC = new BLTCC();
            usp_GetTCCRequestDetails_Result mObjRequestData = mObjBLTCC.BL_GetRequestDetails(pobjGenerateModel.RequestID);

            IList<PDFTemplateFieldList> lstTemplateField = SEDEFunction.PDFTemplateFieldList(GlobalDefaultValues.TCC_PDFTemplateID, pobjGenerateModel.SEDE_DocumentID);

            foreach (var item in lstTemplateField)
            {
                if (item.FieldTypeID == (int)EnumList.FieldType.Combo)
                {
                    item.FieldValue = pObjFormCollection.Get("cbo_" + item.FieldID + "_" + item.FieldName.ToSeoUrl());
                }
                else
                {
                    item.FieldValue = pObjFormCollection.Get("txt_" + item.FieldID + "_" + item.FieldName.ToSeoUrl());
                }
            }

            if (!ModelState.IsValid)
            {
                ViewBag.RequestData = mObjRequestData;
                ViewBag.TemplateFieldList = lstTemplateField;
                return View(pobjGenerateModel);
            }
            else
            {
                using (TransactionScope mObjTransctionScope = new TransactionScope())
                {
                    try
                    {
                        //Update 
                        MAP_TCCRequest_Generate mObjGenerate = new MAP_TCCRequest_Generate()
                        {
                            RGID = pobjGenerateModel.RGID,
                            RequestID = pobjGenerateModel.RequestID,
                            Notes = pobjGenerateModel.GenerateNotes,
                            PDFTemplateID = GlobalDefaultValues.TCC_PDFTemplateID,
                            IsExpirable = pobjGenerateModel.IsExpirable,
                            ExpiryDate = pobjGenerateModel.ExpiryDate,
                            Reason = pobjGenerateModel.Reason,
                            Location = pobjGenerateModel.Location,
                            StageID = (int)NewTCCRequestStage.Waiting_For_First_Signature,
                            ApprovalDate = CommUtil.GetCurrentDateTime(),
                            IsAction = false,
                            CreatedBy = SessionManager.UserID,
                            CreatedDate = CommUtil.GetCurrentDateTime()
                        };

                        FuncResponse<MAP_TCCRequest_Generate> mObjFuncResponse = mObjBLTCC.BL_InsertUpdateTCCRequestGenerate(mObjGenerate);

                        if (mObjFuncResponse.Success)
                        {
                            MAP_TCCRequest_Generate_Field mObjGenerateField;
                            foreach (var item in lstTemplateField)
                            {
                                string strFieldValue = "";
                                if (item.FieldTypeID == (int)EnumList.FieldType.Combo)
                                {
                                    strFieldValue = pObjFormCollection.Get("cbo_" + item.FieldID + "_" + item.FieldName.ToSeoUrl());
                                }
                                else if (item.FieldTypeID == (int)EnumList.FieldType.FileUpload)
                                {

                                    HttpPostedFileBase mObjPostedFile = Request.Files["fu_" + item.FieldID + "_" + item.FieldName.ToSeoUrl()];

                                    if (mObjPostedFile != null && mObjPostedFile.ContentLength > 0)
                                    {
                                        string strDirectory = GlobalDefaultValues.DocumentLocation + "TCC/Request/" + pobjGenerateModel.RequestID + "/Generate/";
                                        string mstrFileName = "CF_" + item.FieldID + "_" + Path.GetFileName(mObjPostedFile.FileName);

                                        if (!Directory.Exists(strDirectory))
                                        {
                                            Directory.CreateDirectory(strDirectory);
                                        }

                                        string mStrDocumentPath = Path.Combine(strDirectory, mstrFileName);
                                        mObjPostedFile.SaveAs(mStrDocumentPath);

                                        strFieldValue = "TCC/Request/" + pobjGenerateModel.RequestID + "/Generate/" + mstrFileName;
                                    }
                                    else
                                    {
                                        strFieldValue = item.FieldValue;
                                    }

                                }
                                else
                                {
                                    strFieldValue = pObjFormCollection.Get("txt_" + item.FieldID + "_" + item.FieldName.ToSeoUrl());
                                }

                                mObjGenerateField = new MAP_TCCRequest_Generate_Field()
                                {
                                    RGFID = 0,
                                    RGID = mObjFuncResponse.AdditionalData.RGID,
                                    FieldID = item.FieldID,
                                    PFID = item.PFID,
                                    FieldValue = strFieldValue,
                                    Active = true,
                                    CreatedBy = SessionManager.UserID,
                                    CreatedDate = CommUtil.GetCurrentDateTime()

                                };

                                item.FieldValue = strFieldValue;

                                mObjBLTCC.BL_InsertUpdateGenerateField(mObjGenerateField);
                            }

                            //Pass Document to Sede
                            IList<DocumentFieldModel> lstDocumentField = new List<DocumentFieldModel>();
                            DocumentFieldModel mObjDocumentFieldModel;
                            IDictionary<string, string> dcFileData = new Dictionary<string, string>();


                            foreach (var item in lstTemplateField)
                            {
                                if (item.FieldTypeID == (int)EnumList.FieldType.FileUpload)
                                {
                                    if (!string.IsNullOrWhiteSpace(item.FieldValue))
                                    {
                                        dcFileData["fu_" + item.FieldID + "_" + item.FieldName.ToSeoUrl()] = GlobalDefaultValues.DocumentLocation + item.FieldValue;
                                    }
                                }
                                else
                                {
                                    mObjDocumentFieldModel = new DocumentFieldModel()
                                    {
                                        FieldID = item.FieldID,
                                        FieldValue = item.FieldValue,
                                    };

                                    lstDocumentField.Add(mObjDocumentFieldModel);
                                }
                            }


                            DocumentViewModel mObjSEDEDocumentModel = new DocumentViewModel()
                            {
                                DocumentID = pobjGenerateModel.SEDE_DocumentID,
                                OrganizationID = GlobalDefaultValues.TCC_SEDEOrganizationID,
                                PDFTemplateID = GlobalDefaultValues.TCC_PDFTemplateID,
                                IsExpirable = pobjGenerateModel.IsExpirable,
                                ExpiryDate = pobjGenerateModel.ExpiryDate,
                                Reason = pobjGenerateModel.Reason,
                                Location = pobjGenerateModel.Location,
                                FieldList = lstDocumentField
                            };


                            IDictionary<string, object> dcSEDEResponse;
                            if (mObjRequestData.SEDE_DocumentID.GetValueOrDefault() > 0)
                            {
                                dcSEDEResponse = APICall.PostData(GlobalDefaultValues.SEDE_API_UpdateDocument, mObjSEDEDocumentModel, dcFileData);
                            }
                            else
                            {
                                dcSEDEResponse = APICall.PostData(GlobalDefaultValues.SEDE_API_AddDocument, mObjSEDEDocumentModel, dcFileData);
                            }

                            if (TrynParse.parseBool(dcSEDEResponse["success"]))
                            {
                                int mIntSEDEDocumentID = TrynParse.parseInt(dcSEDEResponse["documentId"]);

                                //Generate PDF and Update DB
                                PDFTemplateModel mObjPDFTemplateData = SEDEFunction.PDFTemplateDetail(GlobalDefaultValues.TCC_PDFTemplateID);

                                string mStrDirectory = GlobalDefaultValues.DocumentLocation + "TCC/Request/" + mObjRequestData.TCCRequestID + "/Generate";
                                string mStrGeneratedFileName = DateTime.Now.ToString("ddMMyyyy") + "_Generated.pdf";
                                string mStrGeneratedDocumentPath = Path.Combine(mStrDirectory, mStrGeneratedFileName);

                                if (!Directory.Exists(mStrDirectory))
                                {
                                    Directory.CreateDirectory(mStrDirectory);
                                }

                                mObjGenerate.GeneratedPath = "TCC/Request/" + mObjRequestData.TCCRequestID + "/Generate/" + mStrGeneratedFileName;
                                mObjGenerate.SEDE_DocumentID = mIntSEDEDocumentID;
                                mObjGenerate.StageID = (int)NewTCCRequestStage.Generate_eTCC;
                                mObjGenerate.IsAction = true;
                                mObjGenerate.RGID = mObjFuncResponse.AdditionalData.RGID;


                                mObjFuncResponse = mObjBLTCC.BL_InsertUpdateTCCRequestGenerate(mObjGenerate);

                                if (mObjFuncResponse.Success)
                                {
                                    mObjTransctionScope.Complete();
                                    return RedirectToAction("Details", "ProcessTCCRequest", new { reqid = pobjGenerateModel.RequestID });
                                }
                                else
                                {
                                    Transaction.Current.Rollback();
                                    ViewBag.Message = "Error Occurred while Generating";
                                    ViewBag.RequestData = mObjRequestData;
                                    ViewBag.TemplateFieldList = lstTemplateField;

                                    return View(pobjGenerateModel);
                                }
                            }
                            else
                            {
                                Transaction.Current.Rollback();

                                ViewBag.Message = dcSEDEResponse["Message"];
                                ViewBag.RequestData = mObjRequestData;
                                ViewBag.TemplateFieldList = lstTemplateField;
                                return View(pobjGenerateModel);
                            }



                        }
                        else
                        {
                            ViewBag.RequestData = mObjRequestData;
                            ViewBag.TemplateFieldList = lstTemplateField;
                            ViewBag.Message = mObjFuncResponse.Message;
                            return View(pobjGenerateModel);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.SendErrorToText(ex);
                        ViewBag.RequestData = mObjRequestData;
                        ViewBag.TemplateFieldList = lstTemplateField;
                        ErrorSignal.FromCurrentContext().Raise(ex);
                        ViewBag.Message = "Error occurred while saving generate tcc details";
                        return View(pobjGenerateModel);
                    }
                }
            }
        }

        [HttpGet]
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

        [HttpGet]
        public ActionResult SignTCCVisible(long? reqid)
        {
            if (reqid.GetValueOrDefault() > 0)
            {
                BLTCC mObjBLTCC = new BLTCC();
                usp_GetTCCRequestDetails_Result mObjRequestData = mObjBLTCC.BL_GetRequestDetails(reqid.GetValueOrDefault());

                if (mObjRequestData != null)
                {
                    ViewBag.RequestData = mObjRequestData;

                    MAP_TCCRequest_Validate mObjValidateData = mObjBLTCC.BL_GetTCCRequestValidateDetails(mObjRequestData.TCCRequestID);

                    ValidateViewModel mObjValidateTCCModel = new ValidateViewModel()
                    {
                        RequestID = mObjRequestData.TCCRequestID,
                    };

                    if (mObjValidateData != null)
                    {
                        mObjValidateTCCModel.RVID = mObjValidateData.RVID;
                        mObjValidateTCCModel.ValidateNotes = mObjValidateData.Notes;
                    }

                    return View(mObjValidateTCCModel);
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


        [HttpGet]
        public ActionResult SealTCC(long? reqid)
        {
            if (reqid.GetValueOrDefault() > 0)
            {
                BLTCC mObjBLTCC = new BLTCC();
                usp_GetTCCRequestDetails_Result mObjRequestData = mObjBLTCC.BL_GetRequestDetails(reqid.GetValueOrDefault());

                if (mObjRequestData != null)
                {
                    ViewBag.RequestData = mObjRequestData;

                    MAP_TCCRequest_Seal mObjSealData = mObjBLTCC.BL_GetTCCRequestSealDetails(mObjRequestData.TCCRequestID);

                    SealViewModel mObjSealTCCModel = new SealViewModel()
                    {
                        RequestID = mObjRequestData.TCCRequestID,
                    };

                    if (mObjSealData != null)
                    {
                        mObjSealTCCModel.RSID = mObjSealData.RSID;
                        mObjSealTCCModel.SealNotes = mObjSealData.Notes;
                    }

                    return View(mObjSealTCCModel);
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
        [HttpGet]
        public ActionResult IssueTCC(long? reqid)
        {
            if (reqid.GetValueOrDefault() > 0)
            {
                BLTCC mObjBLTCC = new BLTCC();
                usp_GetTCCRequestDetails_Result mObjRequestData = mObjBLTCC.BL_GetRequestDetails(reqid.GetValueOrDefault());

                if (mObjRequestData != null)
                {
                    ViewBag.RequestData = mObjRequestData;

                    MAP_TCCRequest_Issue mObjIssueData = mObjBLTCC.BL_GetTCCRequestIssueDetails(mObjRequestData.TCCRequestID);

                    IssueViewModel mObjIssueTCCModel = new IssueViewModel()
                    {
                        RequestID = mObjRequestData.TCCRequestID,
                    };

                    if (mObjIssueData != null)
                    {
                        mObjIssueTCCModel.RIID = mObjIssueData.RIID;
                        mObjIssueTCCModel.IssueNotes = mObjIssueData.Notes;
                    }

                    return View(mObjIssueTCCModel);
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
        public ActionResult IssueTCC(IssueViewModel pobjIssueModel)
        {
            BLTCC mObjBLTCC = new BLTCC();
            usp_GetTCCRequestDetails_Result mObjRequestData = mObjBLTCC.BL_GetRequestDetails(pobjIssueModel.RequestID);

            if (!ModelState.IsValid)
            {
                ViewBag.RequestData = mObjRequestData;
                return View(pobjIssueModel);
            }
            else
            {
                using (TransactionScope mObjTransctionScope = new TransactionScope())
                {
                    try
                    {
                        //Update 
                        MAP_TCCRequest_Issue mObjIssue = new MAP_TCCRequest_Issue()
                        {
                            RIID = pobjIssueModel.RIID,
                            RequestID = pobjIssueModel.RequestID,
                            Notes = pobjIssueModel.IssueNotes,
                            StageID = (int)NewTCCRequestStage.Issue_eTCC,
                            ApprovalDate = CommUtil.GetCurrentDateTime(),
                            IsAction = true,
                            CreatedBy = SessionManager.UserID,
                            CreatedDate = CommUtil.GetCurrentDateTime()
                        };

                        FuncResponse<MAP_TCCRequest_Issue> mObjFuncResponse = mObjBLTCC.BL_InsertUpdateTCCRequestIssue(mObjIssue);

                        if (mObjFuncResponse.Success)
                        {
                            mObjTransctionScope.Complete();
                            return RedirectToAction("Details", "ProcessTCCRequest", new { reqid = pobjIssueModel.RequestID });
                        }
                        else
                        {
                            Transaction.Current.Rollback();
                            ViewBag.RequestData = mObjRequestData;
                            ViewBag.Message = mObjFuncResponse.Message;
                            return View(pobjIssueModel);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.SendErrorToText(ex);
                        Transaction.Current.Rollback();
                        ViewBag.RequestData = mObjRequestData;
                        ErrorSignal.FromCurrentContext().Raise(ex);
                        ViewBag.Message = "Error occurred while saving validate tcc details";
                        return View(pobjIssueModel);
                    }
                }
            }
        }

        [HttpGet]
        public ActionResult Download(long? reqid)
        {
            if (reqid.GetValueOrDefault() > 0)
            {
                BLTCC mObjBLTCC = new BLTCC();
                usp_GetTCCRequestDetails_Result mObjRequestData = mObjBLTCC.BL_GetRequestDetails(reqid.GetValueOrDefault());

                if (mObjRequestData != null)
                {
                    return File(GlobalDefaultValues.DocumentLocation + mObjRequestData.GeneratedPath, "application/force-download", mObjRequestData.RequestRefNo.Trim() + ".pdf");
                }
                else
                {
                    return Content("Document Not Found");
                }
            }
            else
            {
                return Content("Document Not Found");
            }
        }

        [HttpGet]
        public ActionResult Print(long? reqid)
        {
            BLTCC mObjBLTCC = new BLTCC();
            usp_GetTCCRequestDetails_Result mObjRequestData = mObjBLTCC.BL_GetRequestDetails(reqid.GetValueOrDefault());
            if (mObjRequestData != null)
            {
                var db = _db.TCC_Request.FirstOrDefault(o => o.TCCRequestID == mObjRequestData.TCCRequestID);
                var fname = GlobalDefaultValues.DocumentLocation + db.GeneratePathForPrint.Trim();

                var result = new FilePathResult($"{fname}", "text/html");
                return result;
            }
            return null;
        }
        [HttpPost]
        public JsonResult DeleteIncomeStream(int RowID)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (RowID > 0)
            {
                IList<Request_IncomeStream> lstIncomeStream = SessionManager.LstIncomeStream ?? new List<Request_IncomeStream>();
                IList<PayeApiResponse> lstPayeApiResponse = SessionManager.LstPayeApiResponse ?? new List<PayeApiResponse>();

                Request_IncomeStream mObjIncomeStreamModel = lstIncomeStream.Where(t => t.RowID == RowID).FirstOrDefault();

                if (mObjIncomeStreamModel != null)
                {
                    mObjIncomeStreamModel.TotalIncomeEarned = 0;
                    mObjIncomeStreamModel.BusinessID = 0;
                    mObjIncomeStreamModel.BusinessName = "";
                    mObjIncomeStreamModel.LGAName = "";
                    mObjIncomeStreamModel.LGAID = 0;
                    mObjIncomeStreamModel.TaxPayerRoleID = 0;
                    mObjIncomeStreamModel.TaxPayerRoleName = "";
                    mObjIncomeStreamModel.intTrack = EnumList.Track.UPDATE;
                }

                //Update TCC
                IList<Request_TCCDetail> lstTCCDetails = SessionManager.LstTCCDetail ?? new List<Request_TCCDetail>();
                //Search if Row for Tax Year Exists
                Request_TCCDetail mObjTCCDetail;
                var pp = lstPayeApiResponse.FirstOrDefault(o => o.AssessmentYear == mObjIncomeStreamModel.TaxYear.ToString());
                mObjTCCDetail = lstTCCDetails.Where(t => t.TaxYear == mObjIncomeStreamModel.TaxYear).FirstOrDefault();
                lstTCCDetails.Remove(mObjTCCDetail);
                if (mObjTCCDetail != null)
                {
                    mObjTCCDetail.TaxYear = mObjIncomeStreamModel.TaxYear;
                    mObjTCCDetail.TCCTaxPaid = Convert.ToDecimal(pp.AnnualTaxII);
                    mObjTCCDetail.ERASTaxPaid = Convert.ToDecimal(pp.AnnualTaxII);
                    mObjTCCDetail.ERASAssessed = Convert.ToDecimal(pp.AnnualTax);
                    mObjTCCDetail.Tax_receipt = "";
                    mObjTCCDetail.AssessableIncome = Convert.ToDecimal(pp.ChargeableIncome);
                    mObjTCCDetail.intTrack = lstIncomeStream.Where(t => t.TaxYear == mObjIncomeStreamModel.TaxYear && t.intTrack != EnumList.Track.DELETE).Any() ? EnumList.Track.UPDATE : EnumList.Track.DELETE;
                }
                lstTCCDetails.Add(mObjTCCDetail);

                dcResponse["success"] = true;
                dcResponse["Message"] = "Income Stream Deleted Successfully";

                dcResponse["IncomeStreamData"] = CommUtil.RenderPartialToString("_BindIncomeStreamTable", lstIncomeStream.Where(t => t.intTrack != EnumList.Track.DELETE).ToList(), this.ControllerContext);
                SessionManager.LstIncomeStream = lstIncomeStream;

                dcResponse["TCCDetailData"] = CommUtil.RenderPartialToString("_BindTCCDetailTable", lstTCCDetails.Where(t => t.intTrack != EnumList.Track.DELETE).ToList(), this.ControllerContext);
                SessionManager.LstTCCDetail = lstTCCDetails;
            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Request";
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetIncomeStreamDetails(int RowID, int tpid, int tptid)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (RowID > 0)
            {
                IList<Request_IncomeStream> lstIncomeStream = SessionManager.LstIncomeStream ?? new List<Request_IncomeStream>();

                Request_IncomeStream mObjIncomeStreamModel = lstIncomeStream.Where(t => t.RowID == RowID).FirstOrDefault();

                if (mObjIncomeStreamModel != null)
                {
                    IList<usp_GetTaxPayerAssetForTCC_Result> lstTaxPayerAsset = new BLTCC().BL_GetTaxPayerAssetList(tpid, tptid);
                    if (mObjIncomeStreamModel.TaxPayerRoleID != 0)
                    {
                        lstTaxPayerAsset = lstTaxPayerAsset.Where(t => t.TaxPayerRoleID == mObjIncomeStreamModel.TaxPayerRoleID).ToList();
                    }
                    dcResponse["success"] = true;
                    dcResponse["AssetList"] = lstTaxPayerAsset.Select(t => new { id = t.AssetID, text = t.AssetName }).Distinct();
                    dcResponse["IncomeStreamData"] = mObjIncomeStreamModel;
                }
                else
                {
                    dcResponse["success"] = false;
                    dcResponse["Message"] = "Invalid Request";
                }
            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Request";
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetPAYEIncomeStreamDetails(int RowID)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (RowID > 0)
            {
                IList<PayeApiResponse> lstIncomeStream = SessionManager.LstPayeApiResponse ?? new List<PayeApiResponse>();

                PayeApiResponse mObjIncomeStreamModel = lstIncomeStream.Where(t => t.RowID == RowID).FirstOrDefault();
                mObjIncomeStreamModel.TaxYear = Convert.ToInt32(mObjIncomeStreamModel.AssessmentYear);

                if (mObjIncomeStreamModel != null)
                {
                    dcResponse["success"] = true;
                    dcResponse["IncomeStreamData"] = mObjIncomeStreamModel;
                }
                else
                {
                    dcResponse["success"] = false;
                    dcResponse["Message"] = "Invalid Request";
                }
            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Request";
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddUpdateIncomeStream(RequestIncomeStreamViewModel pObjIncomeStreamModel)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            IList<PayeApiResponse> lstPayeApiResponse = SessionManager.LstPayeApiResponse ?? new List<PayeApiResponse>();

            int mIntOldTaxYear = 0;
            decimal fomalTax = 0, formalAssessedIncome = 0;

            IList<Request_IncomeStream> lstIncomeStream = SessionManager.LstIncomeStream ?? new List<Request_IncomeStream>();
            Request_IncomeStream mObjIncomeStream;
            if (pObjIncomeStreamModel.RowID == 0)
            {
                mObjIncomeStream = new Request_IncomeStream()
                {
                    RowID = lstIncomeStream.Count + 1,
                    TBKID = 0,
                    intTrack = EnumList.Track.INSERT,
                };

            }
            else
            {
                mObjIncomeStream = lstIncomeStream.Where(t => t.RowID == pObjIncomeStreamModel.RowID).FirstOrDefault();
                if (mObjIncomeStream == null)
                {
                    dcResponse["success"] = false;
                    dcResponse["Message"] = "Row Not Found";
                }

                mObjIncomeStream.intTrack = EnumList.Track.UPDATE;
                mIntOldTaxYear = mObjIncomeStream.TaxYear;
            }
            usp_GetTaxPayerRoleList_Result mObjTaxPayerRoleDetails = new BLTaxPayerRole().BL_GetTaxPayerRoleDetails(new TaxPayer_Roles() { intStatus = 2, TaxPayerRoleID = pObjIncomeStreamModel.TaxPayerRoleID });
            usp_GetBusinessListNewTy_Result mObjBusinessData = new BLBusiness().BL_GetBusinessDetails(new Business() { BusinessID = pObjIncomeStreamModel.BusinessID, intStatus = 2 });

            mObjIncomeStream.TaxYear = pObjIncomeStreamModel.TaxYear;
            mObjIncomeStream.TotalIncomeEarned = pObjIncomeStreamModel.TotalIncomeEarned;
            mObjIncomeStream.TaxPayerRoleID = pObjIncomeStreamModel.TaxPayerRoleID;
            mObjIncomeStream.TaxPayerRoleName = mObjTaxPayerRoleDetails.TaxPayerRoleName;
            mObjIncomeStream.BusinessID = pObjIncomeStreamModel.BusinessID;
            mObjIncomeStream.BusinessName = mObjBusinessData != null ? mObjBusinessData.BusinessName : "";
            mObjIncomeStream.BusinessTypeName = mObjBusinessData != null ? mObjBusinessData.BusinessTypeName : "";
            mObjIncomeStream.LGAName = mObjBusinessData != null ? mObjBusinessData.LGAName : "";
            mObjIncomeStream.BusinessOperationName = mObjBusinessData != null ? mObjBusinessData.BusinessOperationName : "";
            mObjIncomeStream.BusinessAddress = mObjBusinessData != null ? mObjBusinessData.BusinessAddress : "";
            mObjIncomeStream.BusinessNumber = mObjBusinessData != null ? mObjBusinessData.BusinessNumber : "";
            mObjIncomeStream.ContactPersonName = mObjBusinessData != null ? mObjBusinessData.ContactName : "";
            mObjIncomeStream.Notes = pObjIncomeStreamModel.Notes;

            if (pObjIncomeStreamModel.RowID == 0)
            {
                var mObjIncomeStreamOld = lstIncomeStream.Where(t => t.TaxYear == pObjIncomeStreamModel.TaxYear).FirstOrDefault();
                lstIncomeStream.Remove(mObjIncomeStreamOld);
                mObjIncomeStream.RowID = mObjIncomeStreamOld.RowID;

                lstIncomeStream.Add(mObjIncomeStream);
            }
            usp_GetTaxPayerLiabilityByTaxYear_Result mObjLiabilityData = new BLReport().BL_GetTaxPayerLiabilityByTaxYear(pObjIncomeStreamModel.TaxPayerTypeID, pObjIncomeStreamModel.TaxPayerID, mIntOldTaxYear);

            //Update or Add TCC Details
            IList<Request_TCCDetail> lstTCCDetails = SessionManager.LstTCCDetail ?? new List<Request_TCCDetail>();
            IList<usp_GetTaxPayerLiabilityForTCC_Result> lstTCCDetailsliability = SessionManager.lstLaibility ?? new List<usp_GetTaxPayerLiabilityForTCC_Result>();
            IList<usp_GetTCCDetail_Result> lstTCCDetailsNew = SessionManager.LstTCCDetailNew ?? new List<usp_GetTCCDetail_Result>();

            // BLTCC mObjBLTCC = new BLTCC();
            // IList<usp_GetTCCDetail_Result> lstTCCDetailsNew = mObjBLTCC.BL_GetTCCDetail(mObjBusinessData.IndividualID, (int)EnumList.TaxPayerType.Individual, mIntOldTaxYear);

            //Search if Row for Tax Year Exists
            Request_TCCDetail mObjOldTCCDetail, mObjTCCDetail;
            IList<usp_GetTaxPayerPaymentForTCCNEW_Result> lstTaxPayerPayment = SessionManager.LstTCCTaxPayerPayment ?? new List<usp_GetTaxPayerPaymentForTCCNEW_Result>();
            decimal value = 0;
            decimal valuePaye = 0;
            mIntOldTaxYear = pObjIncomeStreamModel.TaxYear;
            string tickRefNo = "";
            lstTaxPayerPayment = lstTaxPayerPayment.Where(o => o.PaymentDate.Value.Year == pObjIncomeStreamModel.TaxYear).ToList();

            if (mIntOldTaxYear != 0)
            {
                var newmObjOldTCCDetail = lstTCCDetailsNew.Where(t => t.TaxYear == mIntOldTaxYear).FirstOrDefault();
                var formalTccRecord = lstIncomeStream.Where(t => t.TaxYear == mIntOldTaxYear).FirstOrDefault();
                if (formalTccRecord != null)
                {
                    SessionManager.LstIncomeStream = lstIncomeStream;

                }
                if (newmObjOldTCCDetail != null)
                {
                    fomalTax = newmObjOldTCCDetail.ERASAssessed.Value;
                    formalAssessedIncome = newmObjOldTCCDetail.ERASTaxPaid.Value;
                    mObjOldTCCDetail = new Request_TCCDetail()
                    {
                        RowID = lstTCCDetails.Count + 1,
                        TBKID = 0,
                        TaxYear = mIntOldTaxYear,
                        AssessableIncome = formalTccRecord.TotalIncomeEarned,
                        TCCTaxPaid = newmObjOldTCCDetail == null ? 0 : newmObjOldTCCDetail.TCCTaxPaid.GetValueOrDefault(),
                        ERASAssessed = newmObjOldTCCDetail == null ? 0 : newmObjOldTCCDetail.ERASAssessed.GetValueOrDefault(),
                        ERASTaxPaid = newmObjOldTCCDetail == null ? 0 : newmObjOldTCCDetail.ERASTaxPaid.GetValueOrDefault(),
                        Tax_receipt = tickRefNo,
                        intTrack = EnumList.Track.INSERT,
                    };
                    lstTCCDetails.Add(mObjOldTCCDetail);
                }
                else
                {
                    mObjOldTCCDetail = new Request_TCCDetail()
                    {
                        RowID = lstTCCDetails.Count + 1,
                        TBKID = 0,
                        TaxYear = mIntOldTaxYear,
                        AssessableIncome = lstIncomeStream.Where(t => t.TaxYear == mIntOldTaxYear && t.intTrack != EnumList.Track.DELETE).Sum(t => t.TotalIncomeEarned) + valuePaye,
                        TCCTaxPaid = newmObjOldTCCDetail == null ? 0 : newmObjOldTCCDetail.TCCTaxPaid.GetValueOrDefault(),
                        ERASAssessed = newmObjOldTCCDetail == null ? 0 : newmObjOldTCCDetail.ERASAssessed.GetValueOrDefault(),
                        ERASTaxPaid = newmObjOldTCCDetail == null ? 0 : newmObjOldTCCDetail.ERASTaxPaid.GetValueOrDefault(),
                        Tax_receipt = tickRefNo,
                        intTrack = EnumList.Track.INSERT,
                    };
                    lstTCCDetails.Add(mObjOldTCCDetail);
                }
                var mObjTCCDetailtobeDeleted = lstTCCDetails.Where(t => t.TaxYear == pObjIncomeStreamModel.TaxYear).FirstOrDefault();
                mObjTCCDetail = lstTCCDetails.Where(t => t.TaxYear == pObjIncomeStreamModel.TaxYear).FirstOrDefault();
                //lstTCCDetails.Remove(mObjTCCDetail);
                if (mObjTCCDetail != null)
                {
                    decimal liaby = 0;
                    decimal liabz = 0;
                    lstTCCDetails.Remove(mObjTCCDetailtobeDeleted);
                    decimal x, y, z = 0;
                    var newPayeRec = lstPayeApiResponse.FirstOrDefault(o => o.AssessmentYear == pObjIncomeStreamModel.TaxYear.ToString());
                    string refref;
                    if (string.IsNullOrEmpty(mObjTCCDetail.Tax_receipt))
                        refref = newPayeRec.ReceiptDetail;
                    else
                        refref = mObjTCCDetail.Tax_receipt;

                    var liab = lstTCCDetailsliability.FirstOrDefault(o => o.TaxYear == pObjIncomeStreamModel.TaxYear);
                    if (liab != null)
                    {
                        liaby = liab.AssessmentAmount.Value;
                        liabz = liab.PaymentAmount.Value;
                    }
                    x = ((pObjIncomeStreamModel.TotalIncomeEarned) + (Convert.ToDecimal(newPayeRec.ChargeableIncome)));
                    y = (liaby + (Convert.ToDecimal(newPayeRec.AnnualTax)));
                    z = (liabz + (Convert.ToDecimal(newPayeRec.AnnualTaxII)));
                    mObjTCCDetail.AssessableIncome = x;
                    mObjTCCDetail.ERASAssessed = y;
                    mObjTCCDetail.ERASTaxPaid = z;
                    mObjTCCDetail.Tax_receipt = refref;
                    lstTCCDetails.Add(mObjTCCDetail);
                }
                lstTCCDetails = lstTCCDetails.Where(o => o.TBKID != 0).ToList();
            }

            dcResponse["success"] = true;
            dcResponse["Message"] = "Income Stream Added Successfully";

            dcResponse["IncomeStreamData"] = CommUtil.RenderPartialToString("_BindIncomeStreamTable", lstIncomeStream.Where(t => t.intTrack != EnumList.Track.DELETE).ToList(), this.ControllerContext);
            SessionManager.LstIncomeStream = lstIncomeStream;

            dcResponse["TCCDetailData"] = CommUtil.RenderPartialToString("_BindTCCDetailTable", lstTCCDetails.Where(t => t.intTrack != EnumList.Track.DELETE).ToList(), this.ControllerContext);
            SessionManager.LstTCCDetail = lstTCCDetails;


            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AddUpdatePayeIncomeStream(RequestPayeIncomeStreamViewModel pObjIncomeStreamModel)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (!ModelState.IsValid)
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "All Fields are required";
            }
            else
            {
                int mIntOldTaxYear = 0;
                double fomalTax = 0, formalAssessedIncome = 0;
                IList<Request_IncomeStream> lstIncomeStream = SessionManager.LstIncomeStream ?? new List<Request_IncomeStream>();
                IList<PayeApiResponse> lstPayeApiResponse = SessionManager.LstPayeApiResponse ?? new List<PayeApiResponse>();
                Request_IncomeStream mObjIncomeStream;
                PayeApiResponse paye = new PayeApiResponse();
                if (pObjIncomeStreamModel.RowID == 0)
                {
                    mObjIncomeStream = new Request_IncomeStream()
                    {
                        RowID = lstIncomeStream.Count + 1,
                        TBKID = 0,
                        intTrack = EnumList.Track.INSERT,
                    };
                }
                else
                {
                    paye = lstPayeApiResponse.Where(t => t.RowID == pObjIncomeStreamModel.RowID).FirstOrDefault();
                    if (paye == null)
                    {
                        dcResponse["success"] = false;
                        dcResponse["Message"] = "Row Not Found";
                    }
                    fomalTax = paye.AnnualTax;
                    formalAssessedIncome = paye.ChargeableIncome;

                    lstPayeApiResponse.Remove(paye);
                    paye.intTrack = EnumList.Track.UPDATE;
                    mIntOldTaxYear = Convert.ToInt32(paye.AssessmentYear);
                    paye.ChargeableIncome = Convert.ToDouble(pObjIncomeStreamModel.TotalIncomeEarned);
                    paye.AnnualTaxII = Convert.ToDouble(pObjIncomeStreamModel.payeTaxPaid);
                    paye.AnnualTax = Convert.ToDouble(pObjIncomeStreamModel.payeAssessedIncome);
                    paye.ReceiptRef = pObjIncomeStreamModel.ReceiptReference;
                    paye.ReceiptDate = pObjIncomeStreamModel.ReceiptDate;
                    paye.ReceiptDetail = $"{pObjIncomeStreamModel.ReceiptReference}" + "--" + $"{pObjIncomeStreamModel.ReceiptDate}";
                    lstPayeApiResponse.Add(paye);
                }

                //Update or Add TCC Details
                IList<Request_TCCDetail> lstTCCDetails = SessionManager.LstTCCDetail ?? new List<Request_TCCDetail>();
                //Search if Row for Tax Year Exists
                Request_TCCDetail mObjOldTCCDetail, mObjTCCDetail;


                mObjTCCDetail = lstTCCDetails.FirstOrDefault(t => t.TaxYear == mIntOldTaxYear);

                if (mObjTCCDetail != null)
                {
                    string refref;
                    //if (string.IsNullOrEmpty(paye.ReceiptDetail))
                    refref = paye.ReceiptDetail;
                    //else
                    //    refref = mObjTCCDetail.Tax_receipt;
                    mObjTCCDetail.AssessableIncome = (mObjTCCDetail.AssessableIncome - Convert.ToDecimal(formalAssessedIncome)) + pObjIncomeStreamModel.TotalIncomeEarned;
                    mObjTCCDetail.ERASAssessed = (mObjTCCDetail.ERASAssessed - Convert.ToDecimal(fomalTax)) + pObjIncomeStreamModel.payeAssessedIncome;
                    mObjTCCDetail.ERASTaxPaid = (mObjTCCDetail.ERASTaxPaid - Convert.ToDecimal(fomalTax)) + pObjIncomeStreamModel.payeTaxPaid;
                    mObjTCCDetail.Tax_receipt = refref;
                }


                dcResponse["success"] = true;
                dcResponse["Message"] = "Paye Income Stream Added Successfully";

                dcResponse["PayeIncomeStreamData"] = CommUtil.RenderPartialToString("_BindPayeIncomeStreamTable", lstPayeApiResponse.Where(t => t.intTrack != EnumList.Track.DELETE).OrderBy(x => x.AssessmentYear).ToList(), this.ControllerContext);

                SessionManager.LstPayeApiResponse = lstPayeApiResponse.OrderBy(o => o.AssessmentYear).ToList();

                dcResponse["TCCDetailData"] = CommUtil.RenderPartialToString("_BindTCCDetailTable", lstTCCDetails.Where(t => t.intTrack != EnumList.Track.DELETE).ToList(), this.ControllerContext);
                SessionManager.LstTCCDetail = lstTCCDetails;
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeletePayeIncomeStream(int RowID)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (!ModelState.IsValid)
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "All Fields are required";
            }
            else
            {
                int mIntOldTaxYear = 0;
                double fomalTax = 0, formalAssessedIncome = 0;
                IList<Request_IncomeStream> lstIncomeStream = SessionManager.LstIncomeStream ?? new List<Request_IncomeStream>();
                IList<PayeApiResponse> lstPayeApiResponse = SessionManager.LstPayeApiResponse ?? new List<PayeApiResponse>();
                Request_IncomeStream mObjIncomeStream;
                PayeApiResponse paye = new PayeApiResponse();
                paye = lstPayeApiResponse.Where(t => t.RowID == RowID).FirstOrDefault();
                if (paye == null)
                {
                    dcResponse["success"] = false;
                    dcResponse["Message"] = "Row Not Found";
                }


                IList<Request_TCCDetail> lstTCCDetails = SessionManager.LstTCCDetail ?? new List<Request_TCCDetail>();
                //Search if Row for Tax Year Exists
                Request_TCCDetail mObjOldTCCDetail, mObjTCCDetail;

                mObjTCCDetail = lstTCCDetails.FirstOrDefault(t => t.TaxYear == Convert.ToInt32(paye.AssessmentYear));

                if (mObjTCCDetail != null)
                {
                    mObjTCCDetail.AssessableIncome = (mObjTCCDetail.AssessableIncome - Convert.ToDecimal(paye.ChargeableIncome));
                    mObjTCCDetail.ERASAssessed = (mObjTCCDetail.ERASAssessed - Convert.ToDecimal(paye.AnnualTax));
                    mObjTCCDetail.ERASTaxPaid = (mObjTCCDetail.ERASTaxPaid - Convert.ToDecimal(paye.AnnualTaxII));
                    mObjTCCDetail.Tax_receipt = "";
                }

                lstPayeApiResponse.Remove(paye);
                paye.intTrack = EnumList.Track.UPDATE;
                mIntOldTaxYear = Convert.ToInt32(paye.AssessmentYear);
                paye.ChargeableIncome = Convert.ToDouble(0);
                paye.AnnualTaxII = Convert.ToDouble(0);
                paye.AnnualTax = Convert.ToDouble(0);
                paye.ReceiptRef = string.Empty;
                paye.ReceiptDate = string.Empty;
                paye.EmployerName = string.Empty;
                paye.ReceiptDetail = string.Empty;
                lstPayeApiResponse.Add(paye);

                dcResponse["success"] = true;
                dcResponse["Message"] = "Paye Income Stream Deleted Successfully";

                dcResponse["PayeIncomeStreamData"] = CommUtil.RenderPartialToString("_BindPayeIncomeStreamTable", lstPayeApiResponse.Where(t => t.intTrack != EnumList.Track.DELETE).OrderBy(x => x.AssessmentYear).ToList(), this.ControllerContext);

                SessionManager.LstPayeApiResponse = lstPayeApiResponse.OrderBy(o => o.AssessmentYear).ToList();

                dcResponse["TCCDetailData"] = CommUtil.RenderPartialToString("_BindTCCDetailTable", lstTCCDetails.Where(t => t.intTrack != EnumList.Track.DELETE).ToList(), this.ControllerContext);
                SessionManager.LstTCCDetail = lstTCCDetails;
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }
        public IDictionary<decimal, decimal> getTaxPaidAssessed(int year, int individualid, byte TaxPayerTypeID)
        {
            var respValue = new Dictionary<decimal, decimal>();
            MyClassII myClassII = new MyClassII();
            MyClassIII myClassIII = new MyClassIII();
            List<MyClassII> myClassIILst = new List<MyClassII>();
            List<MyClassIII> myClassIIILst = new List<MyClassIII>();
            List<int> profileId = new List<int>();
            List<long> asseId = new List<long>();
            List<long> AARID = new List<long>();
            List<long> aaiid = new List<long>();
            using (var db = new EIRSEntities())
            {
                Individual individual = db.Individuals.FirstOrDefault(o => o.IndividualID == individualid);
                List<Assessment> ass = (List<Assessment>)db.Assessments.Where(o => o.TaxPayerID == individualid && o.TaxPayerTypeID == TaxPayerTypeID);
                profileId.Add(1278); profileId.Add(1279); profileId.Add(1860);
                foreach (var item in ass)
                    asseId.Add(item.AssessmentID);
                var maa = from f in db.MAP_Assessment_AssessmentRule
                          where asseId.Any(p => p == f.AssessmentID) && profileId.Any(k => k == f.ProfileID)
                          && f.AssessmentYear.Value == year
                          select f;
                List<MAP_Assessment_AssessmentRule> maaList = maa.ToList();
                foreach (var item in maaList)
                    AARID.Add(item.AARID);

                var mat = from k in db.MAP_Assessment_AssessmentItem
                          where AARID.Any(p => p == k.AARID)
                          select k;
                List<MAP_Assessment_AssessmentItem> matList = mat.ToList();
                var matRet = matList.GroupBy(o => o.AAIID).Select(g => new MyClass
                {
                    AAIID = g.Key,
                    Assessed_amount = g.Sum(s => s.TaxAmount.Value),
                }).ToList();

                foreach (var item in matRet)
                    aaiid.Add(item.AAIID);

                var mpaa = from k in db.MAP_Assessment_Adjustment
                           where aaiid.Any(p => p == k.AAIID)
                           select k;
                List<MAP_Assessment_Adjustment> mpaaList = mpaa.ToList();
                var mpaaRet = mpaaList.GroupBy(o => o.AAIID).Select(g => new MyClass
                {
                    AAIID = g.Key.Value,
                    SettlementAmount = g.Sum(s => s.Amount.Value),
                }).ToList();
                foreach (var item in matRet)
                {
                    decimal AA = mpaaRet.FirstOrDefault(p => p.Equals(item.AAIID)).Assessed_amount;
                    myClassII.TotalAssessed = AA + item.Assessed_amount;

                    myClassIILst.Add(myClassII);
                }
                //decimal totalAss = 
                //--Total Assessed = Assessed_amount + Adjustement_amount
                var mpas = from k in db.MAP_Settlement_SettlementItem
                           where aaiid.Any(p => p == k.AAIID)
                           select k;
                List<MAP_Settlement_SettlementItem> mpasList = mpas.ToList();
                var mpasRet = mpasList.GroupBy(o => o.AAIID).Select(g => new MyClass
                {
                    AAIID = g.Key.Value,
                    SettlementAmount = g.Sum(s => s.SettlementAmount.Value),
                }).ToList();
                foreach (var item in mpasRet)
                {
                    myClassIII.settled_amount = item.SettlementAmount;

                    myClassIIILst.Add(myClassIII);
                }

                decimal balance = myClassIILst.Sum(o => o.TotalAssessed) - myClassIIILst.Sum(o => o.settled_amount);
                //-- Balance = total_assessed - settled_amount
                respValue.Add(myClassIILst.Sum(o => o.TotalAssessed), balance);
                return respValue;
            }
        }
        [HttpPost]
        public JsonResult GetAssetDropDownList(int tpid, int tptid, int tprolid)
        {
            IList<usp_GetTaxPayerAssetForTCC_Result> lstTaxPayerAsset = new BLTCC().BL_GetTaxPayerAssetList(tpid, tptid);
            //IList<usp_GetProfileInformation_Result> lstTaxPayerAsset = new BLTaxPayerAsset().BL_GetTaxPayerProfileInformation((int)EnumList.TaxPayerType.Individual, tpid);
            if (lstTaxPayerAsset.Count() > 1)
                lstTaxPayerAsset = lstTaxPayerAsset.Where(t => t.AssetID == tprolid).ToList();
            return Json(lstTaxPayerAsset.Select(t => new { id = t.TaxPayerRoleID, text = t.TaxPayerRoleName }).Distinct(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetTCCDetail(int RowID)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (RowID > 0)
            {
                IList<Request_TCCDetail> lstTCCDetail = SessionManager.LstTCCDetail ?? new List<Request_TCCDetail>();

                Request_TCCDetail mObjTCCDetailModel = lstTCCDetail.Where(t => t.RowID == RowID).FirstOrDefault();

                if (mObjTCCDetailModel != null)
                {
                    dcResponse["success"] = true;
                    dcResponse["TCCDetailData"] = mObjTCCDetailModel;
                }
                else
                {
                    dcResponse["success"] = false;
                    dcResponse["Message"] = "Invalid Request";
                }
            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Request";
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateTCCDetail(RequestTCCViewModel pObjTCCDetailModel)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (!ModelState.IsValid)
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "All Fields are required";
            }
            else
            {

                IList<Request_TCCDetail> lstTCCDetail = SessionManager.LstTCCDetail ?? new List<Request_TCCDetail>();
                Request_TCCDetail mObjTCCDetail;

                mObjTCCDetail = lstTCCDetail.Where(t => t.RowID == pObjTCCDetailModel.RowID).FirstOrDefault();
                if (mObjTCCDetail == null)
                {
                    dcResponse["success"] = false;
                    dcResponse["Message"] = "Row Not Found";
                }

                mObjTCCDetail.intTrack = EnumList.Track.UPDATE;
                mObjTCCDetail.TCCTaxPaid = pObjTCCDetailModel.TCCTaxPaid;
                mObjTCCDetail.ERASTaxPaid = pObjTCCDetailModel.ERASTaxPaid;

                dcResponse["success"] = true;
                dcResponse["Message"] = "TCC Detail Updated";

                dcResponse["TCCDetailData"] = CommUtil.RenderPartialToString("_BindTCCDetailTable", lstTCCDetail.Where(t => t.intTrack != EnumList.Track.DELETE).ToList(), this.ControllerContext);
                SessionManager.LstTCCDetail = lstTCCDetail;
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRequestNotesAttachment(long rnid)
        {
            IList<MAP_TCCRequest_Notes_Document> lstDocument = new BLTCC().BL_GetNotesDocumentList(rnid);
            return PartialView(lstDocument);
        }

        public JsonResult SendNotes(long RequestID, int StageID, string Notes)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (RequestID == 0)
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "<div class='alert alert-danger'> This request doesn't exist.</div>";
            }
            else if (string.IsNullOrEmpty(Notes.Trim()))
            {

                dcResponse["success"] = false;
                dcResponse["Message"] = "<div class='alert alert-danger'> Your notes must not be empty. </div>";
            }
            else
            {
                BLTCC mObjBLTCC = new BLTCC();

                MAP_TCCRequest_Notes mObjRequestNotes = new MAP_TCCRequest_Notes()
                {
                    RNID = 0,
                    RequestID = RequestID,
                    StaffID = SessionManager.UserID,
                    StageID = StageID,
                    Notes = Notes,
                    NotesDate = CommUtil.GetCurrentDateTime(),
                    CreatedBy = SessionManager.UserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                UI_AddNotesDocument(mObjRequestNotes);
                FuncResponse<MAP_TCCRequest_Notes> mObjFuncResponse = mObjBLTCC.BL_InsertRequestNotes(mObjRequestNotes);

                if (mObjFuncResponse.Success)
                {
                    SessionManager.LstTCCNotesAttachment = null;

                    dcResponse["success"] = true;
                    dcResponse["Message"] = "<div class='alert alert-success'> Your notes added successfully. </div>";
                }
                else
                {
                    dcResponse["success"] = false;
                    dcResponse["Message"] = "<div class='alert alert-danger'> Error Occurred will sending message. </div>";
                }
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public MAP_TCCRequest_Notes UI_AddNotesDocument(MAP_TCCRequest_Notes pObjRequestNotes)
        {
            IList<FileUploadData> LstTCCNotesAttachment;
            if (SessionManager.LstTCCNotesAttachment != null)
            {
                LstTCCNotesAttachment = SessionManager.LstTCCNotesAttachment;
            }
            else
            {
                LstTCCNotesAttachment = new List<FileUploadData>();
            }

            foreach (FileUploadData doc in LstTCCNotesAttachment)
            {
                if (doc.IntTrack == EnumList.Track.INSERT)
                {
                    MAP_TCCRequest_Notes_Document mObjDocument = new MAP_TCCRequest_Notes_Document()
                    {
                        DocumentName = doc.DocumentName,
                        DocumentPath = doc.DocumentPath,
                        CreatedBy = SessionManager.UserID,
                        CreatedDate = CommUtil.GetCurrentDateTime(),
                        RNDID = doc.TablePKID
                    };

                    pObjRequestNotes.MAP_TCCRequest_Notes_Document.Add(mObjDocument);
                }
            }

            return pObjRequestNotes;
        }

        [HttpPost]
        public JsonResult UploadNotesAttachment()
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();
            IList<FileUploadData> LstTCCNotesAttachment;
            if (SessionManager.LstTCCNotesAttachment != null)
            {
                LstTCCNotesAttachment = SessionManager.LstTCCNotesAttachment;
            }
            else
            {
                LstTCCNotesAttachment = new List<FileUploadData>();
            }

            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase postedFile = Request.Files[0];
                if (postedFile != null && postedFile.ContentLength > 0)
                {
                    var vFilePath = GlobalDefaultValues.DocumentLocation + "Temp/";

                    try
                    {

                        if (!Directory.Exists(vFilePath))
                        {
                            Directory.CreateDirectory(vFilePath);
                        }

                        var FileName = postedFile.FileName;

                        var vFileName = $"{DateTime.Now:dd_MM_yyyy_hh_mm_ss_}{FileName}";
                        vFilePath = vFilePath + vFileName;
                        postedFile.SaveAs(vFilePath);

                        FileUploadData fuData = new FileUploadData()
                        {
                            FileID = LstTCCNotesAttachment.Count + 1,
                            DocumentName = FileName,
                            IntTrack = EnumList.Track.INSERT,
                            DocumentPath = vFilePath,
                            TablePKID = 0,
                            DocumentLink = GlobalDefaultValues.DocumentLink + "Temp/" + vFileName
                        };

                        LstTCCNotesAttachment.Add(fuData);
                        SessionManager.LstTCCNotesAttachment = LstTCCNotesAttachment;
                        dcResponse["Status"] = "Ok";
                        dcResponse["Data"] = CommUtil.RenderPartialToString("_BindDocument", LstTCCNotesAttachment.Where(t => t.IntTrack != EnumList.Track.DELETE).ToList(), this.ControllerContext);
                        return Json(dcResponse, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception ex)
                    {
                        Logger.SendErrorToText(ex);
                        dcResponse["Status"] = "Error";
                        dcResponse["ErrorMessage"] = ex.Message;
                        return Json(dcResponse, JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                    dcResponse["Status"] = "Invalid";
                    dcResponse["Message"] = "Invalid File";
                    return Json(dcResponse, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                dcResponse["Status"] = "Invalid";
                dcResponse["Message"] = "No File Posted";
                return Json(dcResponse, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult RemoveNotesAttachment(int id)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            IList<FileUploadData> LstTCCNotesAttachment;
            if (SessionManager.LstTCCNotesAttachment != null)
            {
                LstTCCNotesAttachment = SessionManager.LstTCCNotesAttachment;
            }
            else
            {
                LstTCCNotesAttachment = new List<FileUploadData>();
            }

            FileUploadData m_Remove = LstTCCNotesAttachment.Where(t => t.FileID == id).Take(1).Single();
            if (m_Remove != null)
            {
                m_Remove.IntTrack = EnumList.Track.DELETE;
            }

            SessionManager.LstTCCNotesAttachment = LstTCCNotesAttachment;

            dcResponse["success"] = true;
            dcResponse["DocumentList"] = CommUtil.RenderPartialToString("_BindDocument", LstTCCNotesAttachment.Where(t => t.IntTrack != EnumList.Track.DELETE).ToList(), this.ControllerContext);

            return Json(dcResponse, JsonRequestBehavior.AllowGet);

        }

        public JsonResult Revoke(MAP_TCCRequest_Revoke pObjRevoke)
        {
            string gp = "", gpd = "";
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            BLTCC mObjBLTCCC = new BLTCC();

            usp_GetTCCRequestDetails_Result mObjRequestData = mObjBLTCCC.BL_GetRequestDetails(pObjRevoke.RequestID.GetValueOrDefault());

            if (mObjRequestData != null)
            {
                using (_db = new EIRSEntities())
                {
                    var tccPath = _db.TCC_Request.FirstOrDefault(o => o.TCCRequestID == pObjRevoke.RequestID);
                    gp = tccPath.GeneratedPath;

                    gpd = tccPath.GeneratePathForPrint;
                    if (System.IO.File.Exists(gp))
                    {
                        System.IO.File.Delete(gp);
                    }
                    if (System.IO.File.Exists(gpd))
                    {
                        System.IO.File.Delete(gpd);
                    }
                    var refHolderToDelete = _db.TccRefHolders.Where(o => o.ReqId == pObjRevoke.RequestID.ToString()).ToList();
                    var recToDelete = _db.NewTCCDetailsHolds.Where(o => o.TCCRequestID == pObjRevoke.RequestID).ToList();
                    var detailsToRemove = _db.ValidateTccs.FirstOrDefault(o => o.TccRequestId == pObjRevoke.RequestID);
                    if (detailsToRemove != null)
                    {
                        var deleteFromValidateTccTable = _db.ValidateTccs.Remove(detailsToRemove);
                    }
                    if (recToDelete.Any())
                        _db.NewTCCDetailsHolds.RemoveRange(recToDelete);
                    if (refHolderToDelete.Any())
                        _ = _db.TccRefHolders.RemoveRange(refHolderToDelete);
                    _db.SaveChanges();

                }
                pObjRevoke.CreatedDate = CommUtil.GetCurrentDateTime();
                pObjRevoke.CreatedBy = SessionManager.UserID;

                FuncResponse mObjResponse = new BLTCC().BL_RevokeTCC(pObjRevoke);

                dcResponse["success"] = mObjResponse.Success;
                dcResponse["Message"] = mObjResponse.Message;
            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Request";
            }


            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public async Task<List<PayeApiResponse>> GetLogRecord(string req)
        {
            string api = payeAPI;
            api = api + req;
            var respObj = new PayeApiFullResponse();

            var respObjList = new List<PayeApiResponse>();
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(api).ConfigureAwait(false);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = response.Content.ReadAsStringAsync();
                    string res = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    respObj = JsonConvert.DeserializeObject<PayeApiFullResponse>(res);
                    int currentYear = DateTime.Now.Year;
                    if (respObj.Result.Count() > 0)
                    {
                        var lastYr = respObj.Result.Where(o => o.AssessmentYear == (currentYear - 1).ToString()).ToList();
                        var last2Yr = respObj.Result.Where(o => o.AssessmentYear == (currentYear - 2).ToString()).ToList();
                        var last3Yr = respObj.Result.Where(o => o.AssessmentYear == (currentYear - 3).ToString()).ToList();

                        if (lastYr.Count > 0)
                        {
                            var item = new PayeApiResponse();
                            item.RowID = 1;
                            item.EmployerName = lastYr.FirstOrDefault().EmployerName;
                            item.AnnualTaxII = lastYr.Sum(o => o.AnnualTax);
                            item.ChargeableIncome = lastYr.Sum(o => o.AnnualGross);
                            item.AnnualTax = lastYr.Sum(o => o.AnnualTax);
                            //anual gros ==> income amount
                            //anual tax ==> assed amount
                            //anual tax ==> tax paid
                            item.AssessmentYear = lastYr.FirstOrDefault().AssessmentYear;
                            respObjList.Add(item);
                        }
                        if (last2Yr.Count > 0)
                        {
                            var item = new PayeApiResponse();
                            item.RowID = 2;
                            item.EmployerName = last2Yr.FirstOrDefault().EmployerName;
                            item.ChargeableIncome = last2Yr.Sum(o => o.AnnualGross);
                            item.AnnualTax = last2Yr.Sum(o => o.AnnualTax);
                            item.AssessmentYear = last2Yr.FirstOrDefault().AssessmentYear;
                            item.AnnualTaxII = last2Yr.Sum(o => o.AnnualTax);
                            respObjList.Add(item);
                        }
                        if (last3Yr.Count > 0)
                        {
                            var item = new PayeApiResponse();
                            item.RowID = 3;
                            item.EmployerName = last3Yr.FirstOrDefault().EmployerName;
                            item.AnnualTax = last3Yr.Sum(o => o.AnnualTax);
                            item.ChargeableIncome = last3Yr.Sum(o => o.AnnualGross);
                            item.AssessmentYear = last3Yr.FirstOrDefault().AssessmentYear;
                            item.AnnualTaxII = last3Yr.Sum(o => o.AnnualTax);
                            respObjList.Add(item);
                        }
                    }
                    return respObjList;
                }

            }
            return respObjList;
        }
        private string BrCode(string param2txpName, string param2txprin, string param2txprefNo)
        {
            string currectYear = DateTime.Now.AddYears(-1).Year.ToString();
            string SigBase64;

            StringBuilder sbBillSummary = new StringBuilder();
            sbBillSummary.Append("EIRS : Tax Clearance Certificate ");
            sbBillSummary.Append("\n");
            sbBillSummary.Append($"For Year : {currectYear}");
            sbBillSummary.Append("\n");
            sbBillSummary.Append("Tax Revenue RIN");
            sbBillSummary.Append(" : ");
            sbBillSummary.Append(param2txprin);
            sbBillSummary.Append("\n");
            sbBillSummary.Append("Taxpayer Name");
            sbBillSummary.Append(" : ");
            sbBillSummary.Append(param2txpName);
            sbBillSummary.Append("\n");
            sbBillSummary.Append("TCC Certificate Number");
            sbBillSummary.Append(" : ");
            sbBillSummary.Append(param2txprefNo);

            string dataDir = "directoryPath";
            string mbarcodeHtmlDirectory = $"{DocumentLocation}/BarCodes/{param2txprin}";
            string mStrGeneratedFileName = "output.Jpeg";
            string mStrGeneratedDocumentPath = Path.Combine(mbarcodeHtmlDirectory, mStrGeneratedFileName);

            BarcodeGenerator generator = new BarcodeGenerator(EncodeTypes.QR, sbBillSummary.ToString());
            generator.Parameters.Barcode.XDimension.Millimeters = 1f;

            if (!Directory.Exists(mbarcodeHtmlDirectory))
            {
                Directory.CreateDirectory(mbarcodeHtmlDirectory);
                generator.Save(mStrGeneratedDocumentPath, BarCodeImageFormat.Jpeg);
            }
            using (System.Drawing.Image image = System.Drawing.Image.FromFile(mStrGeneratedDocumentPath))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();
                    SigBase64 = Convert.ToBase64String(imageBytes);
                }
            }
            return SigBase64;
        }
        static string DocumentLocation = WebConfigurationManager.AppSettings["documentLocation"] ?? "";
        static string DocumentHTMLLocation = WebConfigurationManager.AppSettings["documentHTMLLocation"] ?? "";
        static string payeAPI = WebConfigurationManager.AppSettings["PayeApiLink"] ?? "";
        static string tccAPI = WebConfigurationManager.AppSettings["TCCApiLink"] ?? "";
        static string first_Signer = WebConfigurationManager.AppSettings["signer1st"] ?? "";
    }
}
