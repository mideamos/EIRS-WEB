using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;
using EIRS.Web.Models;
using EIRS.Web.Utility;
using Elmah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using Vereyon.Web;

namespace EIRS.Web.Controllers
{
    public class RequestPersonalTCCController : BaseController
    {
        // GET: RequestPersonalTCC
        public ActionResult Search()
        {
            Session["TaxPayerID"] = 0;
            Session["TaxPayerMobileNumber"] = "";
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
                IndividualRIN = mStrRIN,
                intStatus = 1
            };

            IList<usp_GetIndividualList_Result> lstIndividual = new BLIndividual().BL_GetIndividualList(mObjIndividual);
            return PartialView("_BindTable", lstIndividual.Take(5).ToList());
        }

        public ActionResult GenerateOTP()
        {
            int mIntIndividualID = TrynParse.parseInt(Session["TaxPayerID"]);
            string mStrMobileNumber = TrynParse.parseString(Session["TaxPayerMobileNumber"]);
            if (mIntIndividualID > 0)
            {
                usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(new Individual() { intStatus = 1, IndividualID = mIntIndividualID });

                if (mObjIndividualData != null)
                {
                    //Generate OTP and Sent to Individual

                    int mIntOTP = CommUtil.GenerateUniqueNumber();
                    string mStrSMSText = "Your ERAS Secure One Time Password is " + mIntOTP + ". Please use this to access details the details we hold on your EIRS Tax Account. ";

                    CommUtil.SendSMS(mObjIndividualData.MobileNumber1, mStrSMSText);

                    Individual mObjIndividual = new Individual()
                    {
                        IndividualID = mIntIndividualID,
                        VerificationOTP = mIntOTP
                    };

                    new BLIndividual().REP_UpdateOTPCode(mObjIndividual);

                    TaxPayerOTPViewModel mObjTaxPayerOTPModel = new TaxPayerOTPViewModel()
                    {
                        TaxPayerID = mIntIndividualID,
                        MobileNumber = mObjIndividualData.MobileNumber1,
                        TaxPayerName = mObjIndividualData.FirstName + " " + mObjIndividualData.LastName
                    };

                    return View(mObjTaxPayerOTPModel);

                }
                else
                {
                    return RedirectToAction("Search", "RequestPersonalTCC");
                }
            }
            else if (!string.IsNullOrWhiteSpace(mStrMobileNumber))
            {
                int mIntOTP = CommUtil.GenerateUniqueNumber();
                string mStrSMSText = "Your ERAS Secure One Time Password is " + mIntOTP + ". Please use this to access details the details we hold on your EIRS Tax Account. ";

                CommUtil.SendSMS(mStrMobileNumber, mStrSMSText);
                Session["VerificationOTP"] = mIntOTP;

                TaxPayerOTPViewModel mObjTaxPayerOTPModel = new TaxPayerOTPViewModel()
                {
                    TaxPayerID = 0,
                    MobileNumber = mStrMobileNumber,
                };

                return View(mObjTaxPayerOTPModel);
            }
            else
            {
                return RedirectToAction("Search", "RequestPersonalTCC");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult GenerateOTP(TaxPayerOTPViewModel pObjTaxPayerModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjTaxPayerModel);
            }
            else
            {
                if (pObjTaxPayerModel.TaxPayerID > 0)
                {
                    Individual mObjIndividual = new Individual()
                    {
                        IndividualID = pObjTaxPayerModel.TaxPayerID,
                        VerificationOTP = pObjTaxPayerModel.OTP
                    };

                    FuncResponse mObjFuncResponse = new BLIndividual().REP_CheckOTPCode(mObjIndividual);

                    if (mObjFuncResponse.Success)
                    {
                        Session["IsValid"] = true;
                        Session["TaxPayerID"] = pObjTaxPayerModel.TaxPayerID;
                        Session["TaxPayerMobileNumber"] = pObjTaxPayerModel.MobileNumber;
                        return RedirectToAction("UpdateDetail", "RequestPersonalTCC");
                    }
                    else
                    {
                        ViewBag.OTPMessage = "Invalid OTP";
                        return View(pObjTaxPayerModel);
                    }
                }
                else
                {
                    int mIntOTP = TrynParse.parseInt(Session["VerificationOTP"]);

                    if (mIntOTP == pObjTaxPayerModel.OTP)
                    {
                        Session["IsValid"] = true;
                        Session["TaxPayerID"] = pObjTaxPayerModel.TaxPayerID;
                        Session["TaxPayerMobileNumber"] = pObjTaxPayerModel.MobileNumber;
                        return RedirectToAction("UpdateDetail", "RequestPersonalTCC");
                    }
                    else
                    {
                        ViewBag.OTPMessage = "Invalid OTP";
                        return View(pObjTaxPayerModel);
                    }
                }
            }
        }

        public void UI_FillDropDown(IndividualViewModel pObjIndividualViewModel = null)
        {
            if (pObjIndividualViewModel != null)
            {
                pObjIndividualViewModel.TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual;
            }
            else if (pObjIndividualViewModel == null)
            {
                pObjIndividualViewModel = new IndividualViewModel();
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

        public ActionResult UpdateDetail()
        {
            int mIntIndividualID = TrynParse.parseInt(Session["TaxPayerID"]);
            string mStrMobileNumber = TrynParse.parseString(Session["TaxPayerMobileNumber"]);
            bool IsValid = TrynParse.parseBool(Session["IsValid"]);
            if (mIntIndividualID > 0 && IsValid)
            {
                usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(new Individual() { intStatus = 1, IndividualID = mIntIndividualID });

                if (mObjIndividualData != null)
                {
                    IndividualViewModel mObjIndividualModelView = new IndividualViewModel()
                    {
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

                    UI_FillDropDown(mObjIndividualModelView);
                    return View(mObjIndividualModelView);
                }
                else
                {
                    return RedirectToAction("Search", "RequestPersonalTCC");
                }
            }
            else if (!string.IsNullOrWhiteSpace(mStrMobileNumber) && IsValid)
            {
                IndividualViewModel mObjIndividualModelView = new IndividualViewModel()
                {
                    IndividualID = 0,
                    MobileNumber1 = mStrMobileNumber,
                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                };

                UI_FillDropDown(mObjIndividualModelView);
                return View(mObjIndividualModelView);
            }
            else
            {
                return RedirectToAction("Search", "RequestPersonalTCC");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult UpdateDetail(IndividualViewModel pObjIndividualModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown(pObjIndividualModel);
                return View(pObjIndividualModel);
            }
            else
            {
                Individual mObjIndividual = new Individual()
                {
                    IndividualID = pObjIndividualModel.IndividualID,
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
                    CreatedBy = -1,
                    CreatedDate = CommUtil.GetCurrentDateTime(),
                    ModifiedBy = -1,
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

                        //Check if Request Already Exists for Current Year

                        TCC_Request mObjRequest = new TCC_Request()
                        {
                            RequestDate = CommUtil.GetCurrentDateTime(),
                            TaxPayerID = mObjResponse.AdditionalData.IndividualID,
                            TaxPayerTypeID = mObjResponse.AdditionalData.TaxPayerTypeID,
                            TaxYear = CommUtil.GetCurrentDateTime().Year,
                            StatusID = (int)EnumList.TCCRequestStatus.In_Progess,
                            CreatedBy = mObjResponse.AdditionalData.IndividualID,
                            CreatedDate = CommUtil.GetCurrentDateTime(),
                        };

                        FuncResponse<TCC_Request> mObjICFuncResponse = new BLTCC().BL_GetIncompleteRequest(mObjRequest);

                        if (mObjICFuncResponse.Success)
                        {

                            FuncResponse<TCC_Request> mObjReqResponse = new BLTCC().BL_InsertTCCRequest(mObjRequest);

                            if (mObjReqResponse.Success)
                            {
                                Session["TaxPayerID"] = mObjResponse.AdditionalData.IndividualID;
                                Session["RequestID"] = mObjReqResponse.AdditionalData.TCCRequestID;
                                return RedirectToAction("IncomeStream", "RequestPersonalTCC");
                            }
                            else
                            {
                                UI_FillDropDown(pObjIndividualModel);
                                ViewBag.Message = mObjReqResponse.Message;
                                return View(pObjIndividualModel);
                            }

                        }
                        else
                        {
                            if(mObjICFuncResponse.AdditionalData.StatusID < (int)EnumList.TCCRequestStatus.Awaiting_for_Payment)
                            {
                                Session["TaxPayerID"] = mObjICFuncResponse.AdditionalData.TaxPayerID;
                                Session["RequestID"] = mObjICFuncResponse.AdditionalData.TCCRequestID;
                                return RedirectToAction("IncomeStream", "RequestPersonalTCC");
                            }
                            else
                            {
                                UI_FillDropDown(pObjIndividualModel);
                                ViewBag.Message = "Your Request is alredy in Progess";
                                return View(pObjIndividualModel);
                            }
                        }

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
                    ViewBag.Message = "Error occurred while saving individual";
                    return View(pObjIndividualModel);
                }
            }
        }

        public ActionResult IncomeStream()
        {
            int mIntTaxPayerID = TrynParse.parseInt(Session["TaxPayerID"]);
            long mlngRequestID = TrynParse.parseLong(Session["RequestID"]);
            if (mIntTaxPayerID > 0 && mlngRequestID > 0)
            {
                usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(new Individual() { intStatus = 1, IndividualID = mIntTaxPayerID });

                if (mObjIndividualData != null)
                {
                    if (SessionManager.LstIncomeStream == null)
                    {
                        IList<usp_GetRequestIncomeStreamList_Result> lstRequestIncomeStream = new BLTCC().BL_GetIncomeStreamList(mlngRequestID);
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
                        ViewBag.IndividualData = mObjIndividualData;
                        return View(lstIncomeStream.Where(t => t.intTrack != EnumList.Track.DELETE).ToList());
                    }
                    else
                    {
                        IList<Request_IncomeStream> lstIncomeStream = SessionManager.LstIncomeStream ?? new List<Request_IncomeStream>();

                        ViewBag.IndividualData = mObjIndividualData;
                        return View(lstIncomeStream.Where(t => t.intTrack != EnumList.Track.DELETE).ToList());
                    }
                }
                else
                {
                    return RedirectToAction("Search", "RequestPersonalTCC");
                }
            }
            else
            {
                return RedirectToAction("Search", "RequestPersonalTCC");
            }
        }

        public void UI_FillAISDropDown(IncomeStreamViewModel pObjIncomeStreamModel = null)
        {
            if (pObjIncomeStreamModel == null)
            {
                pObjIncomeStreamModel = new IncomeStreamViewModel();
            }

            UI_FillYearDropDown();
            UI_FillTaxPayerRoleDropDown(new TaxPayer_Roles() { TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual, AssetTypeID = (int)EnumList.AssetTypes.Business });
            UI_FillBusinessTypeDropDown(new Business_Types() { intStatus = 1, IncludeBusinessTypeIds = pObjIncomeStreamModel.BusinessTypeID.ToString() });
            UI_FillLGADropDown(new LGA() { intStatus = 1, IncludeLGAIds = pObjIncomeStreamModel.LGAID.ToString() });
            UI_FillBusinessOperationDropDown(new Business_Operation() { intStatus = 1, IncludeBusinessOperationIds = pObjIncomeStreamModel.BusinessOperationID.ToString(), BusinessTypeID = pObjIncomeStreamModel.BusinessTypeID });
        }

        public ActionResult AddNewIncomeStream()
        {
            int mIntTaxPayerID = TrynParse.parseInt(Session["TaxPayerID"]);

            if (mIntTaxPayerID > 0)
            {
                Individual mObjIndividual = new Individual()
                {
                    IndividualID = mIntTaxPayerID,
                    intStatus = 1
                };

                usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(mObjIndividual);

                if (mObjIndividualData != null)
                {
                    IncomeStreamViewModel mObjIncomeStreamModel = new IncomeStreamViewModel()
                    {
                        TaxPayerID = mObjIndividualData.IndividualID.GetValueOrDefault(),
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                        TaxPayerTypeName = mObjIndividualData.TaxPayerTypeName,
                        TaxPayerRIN = mObjIndividualData.IndividualRIN,
                        TaxPayerTIN = mObjIndividualData.TIN,
                        TaxPayerName = mObjIndividualData.FirstName + " " + mObjIndividualData.LastName,
                        MobileNumber = mObjIndividualData.MobileNumber1,
                        ContactAddress = mObjIndividualData.ContactAddress,
                    };

                    UI_FillAISDropDown();

                    return View(mObjIncomeStreamModel);
                }
                else
                {
                    return RedirectToAction("Search", "RequestPersonalTCC");
                }
            }
            else
            {
                return RedirectToAction("Search", "RequestPersonalTCC");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddNewIncomeStream(IncomeStreamViewModel pObjIncomeStreamModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillAISDropDown(pObjIncomeStreamModel);
                return View(pObjIncomeStreamModel);
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

                SessionManager.LstIncomeStream = lstIncomeStream;
                return RedirectToAction("IncomeStream", "RequestPersonalTCC");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult SaveIncomeStream()
        {
            int mIntTaxPayerID = TrynParse.parseInt(Session["TaxPayerID"]);
            long mlngRequestID = TrynParse.parseLong(Session["RequestID"]);

            if (mIntTaxPayerID > 0 && mlngRequestID > 0)
            {
                usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(new Individual() { intStatus = 1, IndividualID = mIntTaxPayerID });

                BLTCC mObjBLTCC = new BLTCC();
                IList<Request_IncomeStream> lstIncomeStream = SessionManager.LstIncomeStream ?? new List<Request_IncomeStream>();

                if (lstIncomeStream.Where(t => t.intTrack != EnumList.Track.DELETE).Count() > 0)
                {
                    int intYearRecordCount = lstIncomeStream.Where(t => t.intTrack != EnumList.Track.DELETE).Select(t => t.TaxYear).Distinct().Count();

                    if (intYearRecordCount >= 3)
                    {
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
                                            CreatedBy = mIntTaxPayerID,
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
                                return RedirectToAction("ProcessingFees", "RequestPersonalTCC");
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.SendErrorToText(ex);
                            Transaction.Current.Rollback();
                            ViewBag.Message = "Error Occurred Will Saving income stream";
                            ViewBag.IndividualData = mObjIndividualData;
                            return View("IncomeStream", lstIncomeStream.Where(t => t.intTrack != EnumList.Track.DELETE).ToList());
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Last three years stream required";
                        ViewBag.IndividualData = mObjIndividualData;
                        return View("IncomeStream", lstIncomeStream.Where(t => t.intTrack != EnumList.Track.DELETE).ToList());
                    }
                }
                else
                {
                    ViewBag.Message = "Last three years stream required";
                    ViewBag.IndividualData = mObjIndividualData;
                    return View("IncomeStream", lstIncomeStream.Where(t => t.intTrack != EnumList.Track.DELETE).ToList());
                }
            }
            else
            {
                return RedirectToAction("Search", "RequestPersonalTCC");
            }
        }

        public ActionResult ProcessingFees()
        {
            int mIntTaxPayerID = TrynParse.parseInt(Session["TaxPayerID"]);
            long mlngRequestID = TrynParse.parseLong(Session["RequestID"]);
            if (mIntTaxPayerID > 0 && mlngRequestID > 0)
            {
                usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(new Individual() { intStatus = 1, IndividualID = mIntTaxPayerID });

                if (mObjIndividualData != null)
                {
                    ServiceBillViewModel mObjServiceBillModel = new ServiceBillViewModel()
                    {
                        TaxPayerID = mObjIndividualData.IndividualID.GetValueOrDefault(),
                        TaxPayerRIN = mObjIndividualData.IndividualRIN,
                        TaxPayerTIN = mObjIndividualData.TIN,
                        MobileNumber1 = mObjIndividualData.MobileNumber1,
                        TaxPayerName = mObjIndividualData.FirstName + " " + mObjIndividualData.LastName,
                        TaxPayerAddress = mObjIndividualData.ContactAddress,
                        SettlementDuedate = CommUtil.GetCurrentDateTime(),
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                    };

                    IList<ServiceBill_MDAService> lstMDAServices = new List<ServiceBill_MDAService>();
                    IList<ServiceBill_MDAServiceItem> lstMDAServiceItem = new List<ServiceBill_MDAServiceItem>();

                    BLMDAService mObjBLMDAService = new BLMDAService();
                    BLMDAServiceItem mObjBLMDAServiceItem = new BLMDAServiceItem();

                    usp_GetMDAServiceList_Result mObjMDAServiceData = mObjBLMDAService.BL_GetMDAServiceDetails(new MDA_Services() { MDAServiceID = GlobalDefaultValues.TCC_MDAServiceID, IntStatus = 2 });

                    if (mObjMDAServiceData != null)
                    {
                        ServiceBill_MDAService mObjMDAService = new ServiceBill_MDAService()
                        {
                            RowID = lstMDAServices.Count + 1,
                            MDAServiceID = mObjMDAServiceData.MDAServiceID.GetValueOrDefault(),
                            MDAServiceName = mObjMDAServiceData.MDAServiceCode + " - " + mObjMDAServiceData.MDAServiceName,
                            ServiceAmount = mObjMDAServiceData.ServiceAmount.GetValueOrDefault(),
                            intTrack = EnumList.Track.INSERT,
                            TaxYear = mObjMDAServiceData.TaxYear.GetValueOrDefault(),
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
                                    ServiceAmount = mObjMDAServiceItemData.ServiceAmount.GetValueOrDefault(),
                                    ServiceBaseAmount = mObjMDAServiceItemData.ServiceBaseAmount.GetValueOrDefault(),
                                    intTrack = EnumList.Track.INSERT
                                };

                                lstMDAServiceItem.Add(mObjServiceBillItem);
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
                    return RedirectToAction("Search", "RequestPersonalTCC");
                }
            }
            else
            {
                return RedirectToAction("Search", "RequestPersonalTCC");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult ProcessingFees(ServiceBillViewModel pObjServiceBillModel)
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
                            ServiceBillAmount = lstMDAServiceItems.Count > 0 ? lstMDAServiceItems.Where(t => t.intTrack != EnumList.Track.DELETE).Sum(t => t.ServiceAmount) : 0,
                            ServiceBillDate = CommUtil.GetCurrentDateTime(),
                            Notes = "Service Bill For Requesting Personal TCC",
                            SettlementDueDate = CommUtil.GetCurrentDateTime().AddMonths(1),
                            SettlementStatusID = (int)EnumList.SettlementStatus.Assessed,
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

                                //Update Request with service
                                TCC_Request mObjRequest = new TCC_Request()
                                {
                                    TCCRequestID = TrynParse.parseLong(Session["RequestID"]),
                                    ServiceBillID = mObjServiceBillResponse.AdditionalData.ServiceBillID,
                                    StatusID = (int)EnumList.TCCRequestStatus.Awaiting_for_Payment,
                                };

                                new BLTCC().BL_UpdateServiceBillInRequest(mObjRequest);

                                scope.Complete();
                                FlashMessage.Info(mObjServiceBillResponse.Message);
                                return RedirectToAction("ThankYou", "RequestPersonalTCC");
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

        public ActionResult ThankYou()
        {
            return View();
        }

        public JsonResult SetSelectedIndividual(int IndividualID)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            Individual mObjIndividual = new Individual()
            {
                IndividualID = IndividualID,
                intStatus = 1
            };

            usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(mObjIndividual);

            if (mObjIndividualData != null)
            {
                dcResponse["success"] = true;

                dcResponse["RedirectUrl"] = Url.Action("GenerateOTP", "RequestPersonalTCC");
                Session["TaxPayerID"] = IndividualID;
                Session["TaxPayerTypeID"] = (int)EnumList.TaxPayerType.Individual;

            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Request";
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ResendOTP()
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            int mIntIndividualID = TrynParse.parseInt(Session["TaxPayerID"]);
            string mStrMobileNumber = TrynParse.parseString(Session["TaxPayerMobileNumber"]);

            if (mIntIndividualID > 0)
            {
                usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(new Individual() { intStatus = 1, IndividualID = mIntIndividualID });

                if (mObjIndividualData != null)
                {
                    int mIntOTP = CommUtil.GenerateUniqueNumber();
                    string mStrSMSText = "Your ERAS Secure One Time Password is " + mIntOTP + ". Please use this to access details the details we hold on your EIRS Tax Account. ";

                    CommUtil.SendSMS(mObjIndividualData.MobileNumber1, mStrSMSText);

                    Individual mObjIndividual = new Individual()
                    {
                        IndividualID = mIntIndividualID,
                        VerificationOTP = mIntOTP
                    };

                    new BLIndividual().REP_UpdateOTPCode(mObjIndividual);

                    dcResponse["success"] = true;
                }
                else
                {
                    dcResponse["success"] = false;
                }
            }
            else if (!string.IsNullOrWhiteSpace(mStrMobileNumber))
            {
                int mIntOTP = CommUtil.GenerateUniqueNumber();
                string mStrSMSText = "Your ERAS Secure One Time Password is " + mIntOTP + ". Please use this to access details the details we hold on your EIRS Tax Account. ";

                CommUtil.SendSMS(mStrMobileNumber, mStrSMSText);
                Session["VerificationOTP"] = mIntOTP;

                dcResponse["success"] = true;
            }
            else
            {
                dcResponse["success"] = false;
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
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
    }
}