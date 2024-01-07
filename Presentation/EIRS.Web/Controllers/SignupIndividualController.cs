using EIRS.BLL;
using EIRS.BOL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EIRS.Common;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using EIRS.Web.Models;
using EIRS.Models;
using Elmah;
using System.Transactions;
using Vereyon.Web;
using EIRS.Web.Utility;

namespace EIRS.Web.Controllers
{
    public class SignupIndividualController : BaseController
    {
        EIRSEntities _db;
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
                    return RedirectToAction("SignUp", "Default");
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
                return RedirectToAction("SignUp", "Default");
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
                if(pObjTaxPayerModel.TaxPayerID > 0)
                {
                    Individual mObjIndividual = new Individual()
                    {
                        IndividualID = pObjTaxPayerModel.TaxPayerID,
                        VerificationOTP = pObjTaxPayerModel.OTP
                    };

                    FuncResponse mObjFuncResponse = new BLIndividual().REP_CheckOTPCode(mObjIndividual);

                    if(mObjFuncResponse.Success)
                    {
                        Session["TaxPayerID"] = pObjTaxPayerModel.TaxPayerID;
                        Session["TaxPayerMobileNumber"] = pObjTaxPayerModel.MobileNumber;
                        return RedirectToAction("UpdateDetail", "SignupIndividual");
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

                    if(mIntOTP == pObjTaxPayerModel.OTP)
                    {
                        Session["TaxPayerID"] = pObjTaxPayerModel.TaxPayerID;
                        Session["TaxPayerMobileNumber"] = pObjTaxPayerModel.MobileNumber;
                        return RedirectToAction("UpdateDetail", "SignupIndividual");
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
                pObjIndividualViewModel.TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual;
            else if (pObjIndividualViewModel == null)
                pObjIndividualViewModel = new IndividualViewModel();

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
            if (mIntIndividualID > 0)
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
                    return RedirectToAction("SignUp", "Default");
                }
            }
            else if (!string.IsNullOrWhiteSpace(mStrMobileNumber))
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
                return RedirectToAction("SignUp", "Default");
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
                    RegisterationStatusID = (int)EnumList.RegisterationStatus.Partial,
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

                        Session["TaxPayerID"] = mObjResponse.AdditionalData.IndividualID;
                        return RedirectToAction("AssetList", "SignupIndividual");
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

        public ActionResult AssetList()
        {
            int mIntTaxPayerID = TrynParse.parseInt(Session["TaxPayerID"]);

            if (mIntTaxPayerID > 0)
            {
                usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(new Individual() { intStatus = 1, IndividualID = mIntTaxPayerID });

                if (mObjIndividualData != null)
                {
                    return View(mObjIndividualData);
                }
                else
                {
                    return RedirectToAction("SignUp", "Default");
                }


            }
            else
            {
                return RedirectToAction("SignUp", "Default");
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
            return PartialView("_BindBuildingTable", lstBuilding.Take(5).ToList());
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
            return PartialView("_BindBusinessTable", lstBusiness.Take(5).ToList());
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
            return PartialView("_BindLandTable", lstLand.Take(5).ToList());
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
            return PartialView("_BindVehicleTable", lstVehicle.Take(5).ToList());
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
            UI_FillBusinessCategoryDropDown(new Business_Category() { intStatus = 1, IncludeBusinessCategoryIds = pObjBusinessViewModel.BusinessCategoryID.ToString(), BusinessTypeID = pObjBusinessViewModel.BusinessTypeID });
            UI_FillBusinessSectorDropDown(new Business_Sector() { intStatus = 1, IncludeBusinessSectorIds = pObjBusinessViewModel.BusinessSectorID.ToString(), BusinessTypeID = pObjBusinessViewModel.BusinessTypeID, BusinessCategoryID = pObjBusinessViewModel.BusinessCategoryID });
            UI_FillBusinessSubSectorDropDown(new Business_SubSector() { intStatus = 1, IncludeBusinessSubSectorIds = pObjBusinessViewModel.BusinessSubSectorID.ToString(), BusinessSectorID = pObjBusinessViewModel.BusinessSectorID });
            UI_FillBusinessStructureDropDown(new Business_Structure() { intStatus = 1, IncludeBusinessStructureIds = pObjBusinessViewModel.BusinessStructureID.ToString(), BusinessTypeID = pObjBusinessViewModel.BusinessTypeID });
            UI_FillBusinessOperationDropDown(new Business_Operation() { intStatus = 1, IncludeBusinessOperationIds = pObjBusinessViewModel.BusinessOperationID.ToString(), BusinessTypeID = pObjBusinessViewModel.BusinessTypeID });
            UI_FillSizeDropDown(new Size() { intStatus = 1, IncludeSizeIds = pObjBusinessViewModel.SizeID.ToString() });
        }

        public ActionResult AddBusiness()
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
                    };

                    UI_FillBusinessDropDown();

                    return View(mObjBusinessModel);
                }
                else
                {
                    return RedirectToAction("SignUp", "Default");
                }
            }
            else
            {
                return RedirectToAction("SignUp", "Default");
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
                                var vExists = (from tpa in _db.MAP_TaxPayer_Asset
                                               join aa in _db.Asset_Types on tpa.AssetTypeID equals aa.AssetTypeID
                                               join tp in _db.TaxPayer_Roles on tpa.TaxPayerRoleID equals tp.TaxPayerRoleID
                                               join tpx in _db.TaxPayer_Types on tpa.TaxPayerTypeID equals tpx.TaxPayerTypeID
                                               join idd in _db.Individuals on tpa.TaxPayerID equals idd.IndividualID
                                               where tpa.AssetTypeID == mObjTaxPayerAsset.AssetTypeID && tpa.AssetID == mObjTaxPayerAsset.AssetID
                                                  && tpa.TaxPayerTypeID == mObjTaxPayerAsset.TaxPayerTypeID && tpa.TaxPayerID == mObjTaxPayerAsset.TaxPayerID
                                                  && tpa.TaxPayerRoleID == mObjTaxPayerAsset.TaxPayerRoleID && tpa.Active == true

                                               select new { Post = tpa, Meta = tp, All = tpx, Idd = idd, Aa = aa }).FirstOrDefault();

                                if (GlobalDefaultValues.SendNotification)
                                {
                                    //Send Notification
                                    EmailDetails mObjEmailDetails = new EmailDetails()
                                    {
                                        TaxPayerTypeID = vExists.Post.TaxPayerTypeID.GetValueOrDefault(),
                                        TaxPayerTypeName = vExists.All.TaxPayerTypeName,
                                        TaxPayerID = vExists.Idd.IndividualID,
                                        TaxPayerName = vExists.Idd.FirstName + " " + vExists.Idd.LastName,
                                        TaxPayerRIN = vExists.Idd.IndividualRIN,
                                        TaxPayerRoleName = vExists.Meta.TaxPayerRoleName,
                                        AssetName = vExists.Aa.AssetTypeName,
                                        LoggedInUserID = SessionManager.UserID,
                                    };

                                    if (!string.IsNullOrWhiteSpace(vExists.Idd.EmailAddress1))
                                    {
                                        BLEmailHandler.BL_AssetProfileLinked(mObjEmailDetails);
                                    }

                                    if (!string.IsNullOrWhiteSpace(vExists.Idd.MobileNumber1))
                                    {
                                        UtilityController.BL_AssetProfileLinked(mObjEmailDetails);
                                    }
                                }
                                Session["TPAID"] = mObjTaxPayerAssetData.TPAID;

                                mObjScope.Complete();
                                FlashMessage.Info("Business Created Successfully and Linked to Tax Payer");
                                return RedirectToAction("GeneratePassword", "SignupIndividual");
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

        public ActionResult AddLand()
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
                    return RedirectToAction("SignUp", "Default");
                }
            }
            else
            {
                return RedirectToAction("SignUp", "Default");
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
                                usp_GetTaxPayerAssetList_Result mObjTaxPayerAssetData = (usp_GetTaxPayerAssetList_Result)mObjTPResponse.AdditionalData;
                                var vExists = (from tpa in _db.MAP_TaxPayer_Asset
                                               join aa in _db.Asset_Types on tpa.AssetTypeID equals aa.AssetTypeID
                                               join tp in _db.TaxPayer_Roles on tpa.TaxPayerRoleID equals tp.TaxPayerRoleID
                                               join tpx in _db.TaxPayer_Types on tpa.TaxPayerTypeID equals tpx.TaxPayerTypeID
                                               join idd in _db.Individuals on tpa.TaxPayerID equals idd.IndividualID
                                               where tpa.AssetTypeID == mObjTaxPayerAsset.AssetTypeID && tpa.AssetID == mObjTaxPayerAsset.AssetID
                                                  && tpa.TaxPayerTypeID == mObjTaxPayerAsset.TaxPayerTypeID && tpa.TaxPayerID == mObjTaxPayerAsset.TaxPayerID
                                                  && tpa.TaxPayerRoleID == mObjTaxPayerAsset.TaxPayerRoleID && tpa.Active == true

                                               select new { Post = tpa, Meta = tp, All = tpx, Idd = idd, Aa = aa }).FirstOrDefault();

                                if (GlobalDefaultValues.SendNotification)
                                {
                                    //Send Notification
                                    EmailDetails mObjEmailDetails = new EmailDetails()
                                    {
                                        TaxPayerTypeID = vExists.Post.TaxPayerTypeID.GetValueOrDefault(),
                                        TaxPayerTypeName = vExists.All.TaxPayerTypeName,
                                        TaxPayerID = vExists.Idd.IndividualID,
                                        TaxPayerName = vExists.Idd.FirstName + " " + vExists.Idd.LastName,
                                        TaxPayerRIN = vExists.Idd.IndividualRIN,
                                        TaxPayerRoleName = vExists.Meta.TaxPayerRoleName,
                                        AssetName = vExists.Aa.AssetTypeName,
                                        LoggedInUserID = SessionManager.UserID,
                                    };

                                    if (!string.IsNullOrWhiteSpace(vExists.Idd.EmailAddress1))
                                    {
                                        BLEmailHandler.BL_AssetProfileLinked(mObjEmailDetails);
                                    }

                                    if (!string.IsNullOrWhiteSpace(vExists.Idd.MobileNumber1))
                                    {
                                        UtilityController.BL_AssetProfileLinked(mObjEmailDetails);
                                    }
                                }
                                Session["TPAID"] = mObjTaxPayerAssetData.TPAID;

                                mObjScope.Complete();
                                FlashMessage.Info("Land Created Successfully and Linked to Tax Payer");
                                return RedirectToAction("GeneratePassword", "SignupIndividual");
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


        public ActionResult AddVehicle()
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

                    UI_FillVehicleDropDown();

                    return View(mObjVehicleModel);
                }
                else
                {
                    return RedirectToAction("SignUp", "Default");
                }
            }
            else
            {
                return RedirectToAction("SignUp", "Default");
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
                                usp_GetTaxPayerAssetList_Result mObjTaxPayerAssetData = (usp_GetTaxPayerAssetList_Result)mObjTPResponse.AdditionalData;
                                var vExists = (from tpa in _db.MAP_TaxPayer_Asset
                                               join aa in _db.Asset_Types on tpa.AssetTypeID equals aa.AssetTypeID
                                               join tp in _db.TaxPayer_Roles on tpa.TaxPayerRoleID equals tp.TaxPayerRoleID
                                               join tpx in _db.TaxPayer_Types on tpa.TaxPayerTypeID equals tpx.TaxPayerTypeID
                                               join idd in _db.Individuals on tpa.TaxPayerID equals idd.IndividualID
                                               where tpa.AssetTypeID == mObjTaxPayerAsset.AssetTypeID && tpa.AssetID == mObjTaxPayerAsset.AssetID
                                                  && tpa.TaxPayerTypeID == mObjTaxPayerAsset.TaxPayerTypeID && tpa.TaxPayerID == mObjTaxPayerAsset.TaxPayerID
                                                  && tpa.TaxPayerRoleID == mObjTaxPayerAsset.TaxPayerRoleID && tpa.Active == true

                                               select new { Post = tpa, Meta = tp, All = tpx, Idd = idd, Aa = aa }).FirstOrDefault();

                                if (GlobalDefaultValues.SendNotification)
                                {
                                    //Send Notification
                                    EmailDetails mObjEmailDetails = new EmailDetails()
                                    {
                                        TaxPayerTypeID = vExists.Post.TaxPayerTypeID.GetValueOrDefault(),
                                        TaxPayerTypeName = vExists.All.TaxPayerTypeName,
                                        TaxPayerID = vExists.Idd.IndividualID,
                                        TaxPayerName = vExists.Idd.FirstName + " " + vExists.Idd.LastName,
                                        TaxPayerRIN = vExists.Idd.IndividualRIN,
                                        TaxPayerRoleName = vExists.Meta.TaxPayerRoleName,
                                        AssetName = vExists.Aa.AssetTypeName,
                                        LoggedInUserID = SessionManager.UserID,
                                    };

                                    if (!string.IsNullOrWhiteSpace(vExists.Idd.EmailAddress1))
                                    {
                                        BLEmailHandler.BL_AssetProfileLinked(mObjEmailDetails);
                                    }

                                    if (!string.IsNullOrWhiteSpace(vExists.Idd.MobileNumber1))
                                    {
                                        UtilityController.BL_AssetProfileLinked(mObjEmailDetails);
                                    }
                                }
                                Session["TPAID"] = mObjTaxPayerAssetData.TPAID;

                                mObjScope.Complete();
                                FlashMessage.Info("Vehicle Created Successfully and Linked to Tax Payer");
                                return RedirectToAction("GeneratePassword", "SignupIndividual");
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

        public ActionResult AddBuilding()
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
                    return RedirectToAction("SignUp", "Default");
                }
            }
            else
            {
                return RedirectToAction("SignUp", "Default");
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
                                    usp_GetTaxPayerAssetList_Result mObjTaxPayerAssetData = (usp_GetTaxPayerAssetList_Result)mObjTPResponse.AdditionalData;
                                    var vExists = (from tpa in _db.MAP_TaxPayer_Asset
                                                   join aa in _db.Asset_Types on tpa.AssetTypeID equals aa.AssetTypeID
                                                   join tp in _db.TaxPayer_Roles on tpa.TaxPayerRoleID equals tp.TaxPayerRoleID
                                                   join tpx in _db.TaxPayer_Types on tpa.TaxPayerTypeID equals tpx.TaxPayerTypeID
                                                   join idd in _db.Individuals on tpa.TaxPayerID equals idd.IndividualID
                                                   where tpa.AssetTypeID == mObjTaxPayerAsset.AssetTypeID && tpa.AssetID == mObjTaxPayerAsset.AssetID
                                                      && tpa.TaxPayerTypeID == mObjTaxPayerAsset.TaxPayerTypeID && tpa.TaxPayerID == mObjTaxPayerAsset.TaxPayerID
                                                      && tpa.TaxPayerRoleID == mObjTaxPayerAsset.TaxPayerRoleID && tpa.Active == true

                                                   select new { Post = tpa, Meta = tp, All = tpx, Idd = idd, Aa = aa }).FirstOrDefault();

                                    if (GlobalDefaultValues.SendNotification)
                                    {
                                        //Send Notification
                                        EmailDetails mObjEmailDetails = new EmailDetails()
                                        {
                                            TaxPayerTypeID = vExists.Post.TaxPayerTypeID.GetValueOrDefault(),
                                            TaxPayerTypeName = vExists.All.TaxPayerTypeName,
                                            TaxPayerID = vExists.Idd.IndividualID,
                                            TaxPayerName = vExists.Idd.FirstName + " " + vExists.Idd.LastName,
                                            TaxPayerRIN = vExists.Idd.IndividualRIN,
                                            TaxPayerRoleName = vExists.Meta.TaxPayerRoleName,
                                            AssetName = vExists.Aa.AssetTypeName,
                                            LoggedInUserID = SessionManager.UserID,
                                        };

                                        if (!string.IsNullOrWhiteSpace(vExists.Idd.EmailAddress1))
                                        {
                                            BLEmailHandler.BL_AssetProfileLinked(mObjEmailDetails);
                                        }

                                        if (!string.IsNullOrWhiteSpace(vExists.Idd.MobileNumber1))
                                        {
                                            UtilityController.BL_AssetProfileLinked(mObjEmailDetails);
                                        }
                                    }
                                    Session["TPAID"] = mObjTaxPayerAssetData.TPAID;

                                    mObjScope.Complete();
                                    FlashMessage.Info("Building Created Successfully and Linked to Tax Payer");
                                    return RedirectToAction("GeneratePassword", "SignupIndividual");
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

        public ActionResult GeneratePassword()
        {
            int mIntTaxPayerID = TrynParse.parseInt(Session["TaxPayerID"]);
            int mIntTPAID = TrynParse.parseInt(Session["TPAID"]);
            if (mIntTaxPayerID > 0 && mIntTPAID > 0)
            {
                Individual mObjIndividual = new Individual()
                {
                    IndividualID = mIntTaxPayerID,
                    intStatus = 1
                };

                usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(mObjIndividual);

                if (mObjIndividualData != null)
                {

                    usp_GetTaxPayerAssetList_Result mObjTaxPayerAsset = new BLTaxPayerAsset().BL_GetTaxPayerAssetList(new MAP_TaxPayer_Asset() { TPAID = mIntTPAID }).FirstOrDefault();

                    if (mObjTaxPayerAsset != null)
                    {

                        TaxPayerPasswordViewModel mObjPasswordModel = new TaxPayerPasswordViewModel()
                        {
                            TaxPayerID = mObjIndividualData.IndividualID.GetValueOrDefault(),
                            TaxPayerTypeName = mObjIndividualData.TaxPayerTypeName,
                            TaxPayerRIN = mObjIndividualData.IndividualRIN,
                            TaxPayerTIN = mObjIndividualData.TIN,
                            TaxPayerName = mObjIndividualData.FirstName + " " + mObjIndividualData.LastName,
                            MobileNumber = mObjIndividualData.MobileNumber1,
                            ContactAddress = mObjIndividualData.ContactAddress,
                            AssetName = mObjTaxPayerAsset.AssetName,
                            AssetRIN = mObjTaxPayerAsset.AssetRIN,
                            AssetTypeName = mObjTaxPayerAsset.AssetTypeName,
                            TaxPayerRoleName = mObjTaxPayerAsset.TaxPayerRoleName
                        };

                        return View(mObjPasswordModel);
                    }
                    else
                    {
                        return RedirectToAction("SignUp", "Default");
                    }
                }
                else
                {
                    return RedirectToAction("SignUp", "Default");
                }
            }
            else
            {
                return RedirectToAction("SignUp", "Default");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult GeneratePassword(TaxPayerPasswordViewModel pObjTaxPayerPasswordModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjTaxPayerPasswordModel);
            }
            else
            {
                Individual mObjIndividual = new Individual()
                {
                    IndividualID = TrynParse.parseInt(Session["TaxPayerID"]),
                    Password = EncryptDecrypt.Encrypt(pObjTaxPayerPasswordModel.NewPassword),
                    RegisterationStatusID = (int)EnumList.RegisterationStatus.Completed,
                    RegisterationDate = CommUtil.GetCurrentDateTime()
                };

                FuncResponse mObjFuncResponse = new BLIndividual().BL_UpdatePassword(mObjIndividual);

                if (mObjFuncResponse.Success)
                {
                    return RedirectToAction("Individual", "Login");
                }
                else
                {
                    ViewBag.Message = mObjFuncResponse.Message;
                    return View(pObjTaxPayerPasswordModel);
                }
            }
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

                if (mObjIndividualData.RegisterationStatusID == (int)EnumList.RegisterationStatus.Completed)
                {
                    dcResponse["RedirectUrl"] = Url.Action("Individual", "Login");
                }
                else
                {
                    dcResponse["RedirectUrl"] = Url.Action("GenerateOTP", "SignupIndividual");
                    Session["TaxPayerID"] = IndividualID;
                    Session["TaxPayerTypeID"] = (int)EnumList.TaxPayerType.Individual;
                }
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

        public JsonResult GetTaxPayerRoleAssetBased(int AssetTypeID)
        {
            IList<DropDownListResult> lstTaxPayerRole = new BLTaxPayerRole().BL_GetTaxPayerRoleDropDownList(new TaxPayer_Roles() { intStatus = 1, AssetTypeID = AssetTypeID, TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual });
            return Json(lstTaxPayerRole, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LinkAsset(MAP_TaxPayer_Asset pObjTaxPayerAsset)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();
            int mIntTaxPayerID = TrynParse.parseInt(Session["TaxPayerID"]);

            if (pObjTaxPayerAsset.TaxPayerRoleID > 0 && pObjTaxPayerAsset.AssetTypeID > 0 && pObjTaxPayerAsset.AssetID > 0)
            {
                MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                {
                    AssetTypeID = pObjTaxPayerAsset.AssetTypeID,
                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                    TaxPayerRoleID = pObjTaxPayerAsset.TaxPayerRoleID,
                    TaxPayerID = mIntTaxPayerID,
                    AssetID = pObjTaxPayerAsset.AssetID,
                    BuildingUnitID = pObjTaxPayerAsset.BuildingUnitID,
                    Active = true,
                    CreatedBy = mIntTaxPayerID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                FuncResponse mObjResponse = new BLTaxPayerAsset().BL_InsertTaxPayerAsset(mObjTaxPayerAsset);
                if (mObjResponse.Success)
                {
                    usp_GetTaxPayerAssetList_Result mObjTaxPayerAssetData = (usp_GetTaxPayerAssetList_Result)mObjResponse.AdditionalData;
                    var vExists = (from tpa in _db.MAP_TaxPayer_Asset
                                   join aa in _db.Asset_Types on tpa.AssetTypeID equals aa.AssetTypeID
                                   join tp in _db.TaxPayer_Roles on tpa.TaxPayerRoleID equals tp.TaxPayerRoleID
                                   join tpx in _db.TaxPayer_Types on tpa.TaxPayerTypeID equals tpx.TaxPayerTypeID
                                   join idd in _db.Individuals on tpa.TaxPayerID equals idd.IndividualID
                                   where tpa.AssetTypeID == mObjTaxPayerAsset.AssetTypeID && tpa.AssetID == mObjTaxPayerAsset.AssetID
                                      && tpa.TaxPayerTypeID == mObjTaxPayerAsset.TaxPayerTypeID && tpa.TaxPayerID == mObjTaxPayerAsset.TaxPayerID
                                      && tpa.TaxPayerRoleID == mObjTaxPayerAsset.TaxPayerRoleID && tpa.Active == true

                                   select new { Post = tpa, Meta = tp, All = tpx, Idd = idd, Aa = aa }).FirstOrDefault();

                    if (GlobalDefaultValues.SendNotification)
                    {
                        //Send Notification
                        EmailDetails mObjEmailDetails = new EmailDetails()
                        {
                            TaxPayerTypeID = vExists.Post.TaxPayerTypeID.GetValueOrDefault(),
                            TaxPayerTypeName = vExists.All.TaxPayerTypeName,
                            TaxPayerID = vExists.Idd.IndividualID,
                            TaxPayerName = vExists.Idd.FirstName + " " + vExists.Idd.LastName,
                            TaxPayerRIN = vExists.Idd.IndividualRIN,
                            TaxPayerRoleName = vExists.Meta.TaxPayerRoleName,
                            AssetName = vExists.Aa.AssetTypeName,
                            LoggedInUserID = SessionManager.UserID,
                        };

                        if (!string.IsNullOrWhiteSpace(vExists.Idd.EmailAddress1))
                        {
                            BLEmailHandler.BL_AssetProfileLinked(mObjEmailDetails);
                        }

                        if (!string.IsNullOrWhiteSpace(vExists.Idd.MobileNumber1))
                        {
                            UtilityController.BL_AssetProfileLinked(mObjEmailDetails);
                        }
                    }
                    Session["TPAID"] = mObjTaxPayerAssetData.TPAID;
                }

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

        public JsonResult CheckMobileNumber(string MobileNumber)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();
            Individual mObjIndividual = new Individual()
            {
                MobileNumber1 = MobileNumber,
                intStatus = 1
            };

            IList<usp_GetIndividualList_Result> lstIndividual = new BLIndividual().BL_GetIndividualList(mObjIndividual);

            if (lstIndividual.Count > 0)
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Mobile Number already exists. Please Search with Mobile Number and proceed futher";
            }
            else
            {
                Session["TaxPayerMobileNumber"] = MobileNumber;
                dcResponse["success"] = true;
                dcResponse["RedirectUrl"] = Url.Action("GenerateOTP", "SignupIndividual");
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }
    }
}