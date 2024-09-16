using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EIRS.Common;
using EIRS.BOL;
using Newtonsoft.Json;
using System.Net.Http;
using Elmah;
using EIRS.BLL;
using System.IO;
using Microsoft.Reporting.WinForms;
using EIRS.Models;
using Vereyon.Web;
using System.Transactions;

namespace EIRS.Web.Controllers
{
    public class TaxPayerPanelController : BaseController
    {
        public ActionResult Individual()
        {
            if (SessionManager.TaxpayerTypeID == (int)EnumList.TaxPayerType.Individual)
            {
                IList<usp_GetIndividualList_Result> lstIndividual = new BLIndividual().BL_GetIndividualList(new Individual() { IndividualID = SessionManager.TaxPayerID, intStatus = 1 });
                return View(lstIndividual);
            }
            else
            {
                return RedirectToAction("Dashboard", "TaxPayerPanel");
            }
        }

        public ActionResult IndividualDetails()
        {
            int id = SessionManager.TaxPayerID;
            Individual mObjIndividual = new Individual()
            {
                IndividualID = id,
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
                    DOB = mObjIndividualData.DOB != null ? mObjIndividualData.DOB.Value.ToString("dd/MM/yyyy") : " - ",
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

                MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                {
                    TaxPayerID = id,
                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual
                };

                IList<usp_GetTaxPayerAssetList_Result> lstTaxPayerAsset = new BLTaxPayerAsset().BL_GetTaxPayerAssetList(mObjTaxPayerAsset);
                ViewBag.AssetList = lstTaxPayerAsset;

                IList<usp_GetAssessmentRuleInformation_Result> lstAssessmentRuleInformation = new BLTaxPayerAsset().BL_GetTaxPayerAssessmentRuleInformation((int)EnumList.TaxPayerType.Individual, id);
                ViewBag.AssessmentRuleInformation = lstAssessmentRuleInformation;

                IList<usp_GetTaxPayerBill_Result> lstTaxPayerBill = new BLAssessment().BL_GetTaxPayerBill(id, (int)EnumList.TaxPayerType.Individual, 0);
                ViewBag.TaxPayerBill = lstTaxPayerBill;

                IList<usp_GetTaxPayerPayment_Result> lstTaxPayerPayment = new BLSettlement().BL_GetTaxPayerPayment(id, (int)EnumList.TaxPayerType.Individual);
                ViewBag.TaxPayerPayment = lstTaxPayerPayment;

                IList<usp_GetTaxPayerMDAService_Result> lstMDAService = new BLMDAService().BL_GetTaxPayerMDAService((int)EnumList.TaxPayerType.Individual, id);
                ViewBag.MDAService = lstMDAService;

                return View(mObjIndividualModelView);
            }
            else
            {
                return RedirectToAction("Dashboard", "TaxPayerPanel");
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

        public ActionResult EditIndividual()
        {
            int? id = SessionManager.TaxPayerID;

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
                    return RedirectToAction("Dashboard", "TaxPayerPanel");
                }
            }
            else
            {
                return RedirectToAction("Dashboard", "TaxPayerPanel");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult EditIndividual(IndividualViewModel pObjIndividualModel)
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
                    DOB = TrynParse.parseDatetime(pObjIndividualModel.DOB),
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
                    ModifiedBy = SessionManager.UserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse<Individual> mObjResponse = new BLIndividual().BL_InsertUpdateIndividual(mObjIndividual);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("IndividualDetails", "TaxPayerPanel");
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
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillDropDown(pObjIndividualModel);
                    ViewBag.Message = "Error occurred while saving individual";
                    return View(pObjIndividualModel);
                }
            }
        }

        public ActionResult AssessmentBill()
        {
            IList<usp_GetAssessmentList_Result> lstAssessmentBill = new BLAssessment().BL_GetAssessmentList(new Assessment() { TaxPayerID = SessionManager.TaxPayerID, TaxPayerTypeID = SessionManager.TaxpayerTypeID, IntStatus = 1 });
            return View(lstAssessmentBill);
        }

        public ActionResult MDAServiceBills()
        {
            IList<usp_GetServiceBillList_Result> lstMDAServiceBill = new BLServiceBill().BL_GetServiceBillList(new ServiceBill() { TaxPayerID = SessionManager.TaxPayerID, TaxPayerTypeID = SessionManager.TaxpayerTypeID, IntStatus = 1 });
            return View(lstMDAServiceBill);
        }

        public ActionResult AssessmentRule()
        {
            return View();
        }

        public JsonResult GetData()
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            try
            {
                string mStrSearchFilter = Request.Form.GetValues("search[value]")[0];
                string mStrDraw = Request.Form.GetValues("draw")[0];
                string mstrOrderBy = Request.Form.GetValues("order[0][column]")[0];
                string mStrOrderByDir = Request.Form.GetValues("order[0][dir]")[0];
                int mIntStartRowNumber = TrynParse.parseInt(Request.Form.GetValues("start")[0]);
                int mIntPageNumber = TrynParse.parseInt(Request.Form.GetValues("length")[0]);

                // Loading. 
                IList<vw_AssessmentRule> lstAssessmentRuleData = new BLAssessmentRule().BL_GetAssessmentRuleList();

                // Total record count.   
                int totalRecords = lstAssessmentRuleData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstAssessmentRuleData = lstAssessmentRuleData.Where(p => p.TaxYear.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssessmentRuleName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.RuleRunName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.PaymentFrequencyName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssessmentAmount.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower())).ToList();
                }

                // Sorting.   
                lstAssessmentRuleData = this.SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstAssessmentRuleData);

                // Filter record count.   
                int recFilter = lstAssessmentRuleData.Count;

                // Apply pagination.   
                lstAssessmentRuleData = lstAssessmentRuleData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstAssessmentRuleData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        private IList<vw_AssessmentRule> SortByColumnWithOrder(string order, string orderDir, IList<vw_AssessmentRule> data)
        {
            // Initialization.   
            IList<vw_AssessmentRule> lst = new List<vw_AssessmentRule>();
            try
            {
                // Sorting   
                switch (order)
                {
                    case "0":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TaxYear).ToList() : data.OrderBy(p => p.TaxYear).ToList();
                        break;
                    case "1":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.AssessmentRuleName).ToList() : data.OrderBy(p => p.AssessmentRuleName).ToList();
                        break;
                    case "2":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.RuleRunName).ToList() : data.OrderBy(p => p.RuleRunName).ToList();
                        break;
                    case "3":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PaymentFrequencyName).ToList() : data.OrderBy(p => p.PaymentFrequencyName).ToList();
                        break;
                    default:
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.AssessmentAmount).ToList() : data.OrderBy(p => p.AssessmentAmount).ToList();
                        break;
                }
            }
            catch (Exception ex)
            {
                // info.   
                //Console.Write(ex);
            }
            // info.   
            return lst;
        }

        public ActionResult MDAService()
        {
            //return View();
            IList<usp_GetMDAServiceList_Result> lstMDAService = new BLMDAService().BL_GetMDAServiceList(new MDA_Services() { IntStatus = 1 });
            return View(lstMDAService);
        }

        public ActionResult Businesses()
        {
            IList<usp_GetTaxPayerBusinessList_Result> lstTaxPayerAsset = new BLTaxPayerAsset().BL_GetTaxPayerBusinessList(new MAP_TaxPayer_Asset() { TaxPayerTypeID = SessionManager.TaxpayerTypeID, TaxPayerID = SessionManager.TaxPayerID });
            return View(lstTaxPayerAsset);
        }

        public ActionResult BusinessDetails()
        {
            int id = SessionManager.TaxPayerID;
            Business mObjBusiness = new Business()
            {
                BusinessID = id,
                intStatus = 2
            };

            usp_GetBusinessList_Result mObjBusinessData = new BLBusiness().BL_GetBusinessDetails(mObjBusiness);

            if (mObjBusinessData != null)
            {
                BusinessViewModel mObjBusinessModelView = new BusinessViewModel()
                {
                    BusinessID = mObjBusinessData.BusinessID.GetValueOrDefault(),
                    BusinessRIN = mObjBusinessData.BusinessRIN,
                    AssetTypeName = mObjBusinessData.AssetTypeName,
                    BusinessTypeName = mObjBusinessData.BusinessTypeName,
                    BusinessName = mObjBusinessData.BusinessName,
                    LGAName = mObjBusinessData.LGAName,
                    BusinessCategoryName = mObjBusinessData.BusinessCategoryName,
                    BusinessSectorName = mObjBusinessData.BusinessSectorName,
                    BusinessSubSectorName = mObjBusinessData.BusinessSubSectorName,
                    BusinessStructureName = mObjBusinessData.BusinessStructureName,
                    BusinessOperationName = mObjBusinessData.BusinessOperationName,
                    SizeName = mObjBusinessData.SizeName,
                    ContactName = mObjBusinessData.ContactName,
                    BusinessAddress = mObjBusinessData.BusinessAddress,
                    BusinessNumber = mObjBusinessData.BusinessNumber,
                    ActiveText = mObjBusinessData.ActiveText
                };

                MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                {
                    AssetID = id,
                    AssetTypeID = (int)EnumList.AssetTypes.Business
                };

                IList<usp_GetTaxPayerAssetList_Result> lstTaxPayerAsset = new BLTaxPayerAsset().BL_GetTaxPayerAssetList(mObjTaxPayerAsset);
                ViewBag.AssetList = lstTaxPayerAsset;

                return View(mObjBusinessModelView);
            }
            else
            {
                return RedirectToAction("Dashboard", "TaxPayerPanel");
            }

        }

        public ActionResult Buildings()
        {
            IList<usp_GetTaxPayerBuildingList_Result> lstTaxPayerAsset = new BLTaxPayerAsset().BL_GetTaxPayerBuildingList(new MAP_TaxPayer_Asset() { TaxPayerTypeID = SessionManager.TaxpayerTypeID, TaxPayerID = SessionManager.TaxPayerID });
            return View(lstTaxPayerAsset);
        }

        public ActionResult BuildingDetails()
        {
            int id = SessionManager.TaxPayerID;
            Building mObjBuilding = new Building()
            {
                BuildingID = id,
                intStatus = 2
            };

            usp_GetBuildingList_Result mObjBuildingData = new BLBuilding().BL_GetBuildingDetails(mObjBuilding);

            if (mObjBuildingData != null)
            {
                BuildingViewModel mObjBuildingModelView = new BuildingViewModel()
                {
                    BuildingID = mObjBuildingData.BuildingID.GetValueOrDefault(),
                    BuildingRIN = mObjBuildingData.BuildingRIN,
                    BuildingTAGNumber = mObjBuildingData.BuildingTagNumber,
                    BuildingName = mObjBuildingData.BuildingName,
                    BuildingNumber = mObjBuildingData.BuildingNumber,
                    StreetName = mObjBuildingData.StreetName,
                    OffStreetName = mObjBuildingData.OffStreetName,
                    TownName = mObjBuildingData.TownName,
                    LGAName = mObjBuildingData.LGAName,
                    WardName = mObjBuildingData.WardName,
                    AssetTypeName = mObjBuildingData.AssetTypeName,
                    BuildingTypeName = mObjBuildingData.BuildingTypeName,
                    BuildingCompletionName = mObjBuildingData.BuildingCompletionName,
                    BuildingPurposeName = mObjBuildingData.BuildingPurposeName,
                    NoOfUnits = mObjBuildingData.NoOfUnits.GetValueOrDefault(),
                    BuildingOwnershipName = mObjBuildingData.BuildingOwnershipName,
                    Latitude = mObjBuildingData.Latitude,
                    Longitude = mObjBuildingData.Longitude,
                    ActiveText = mObjBuildingData.ActiveText
                };


                MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                {
                    AssetID = id,
                    AssetTypeID = (int)EnumList.AssetTypes.Building
                };

                IList<usp_GetTaxPayerAssetList_Result> lstTaxPayerAsset = new BLTaxPayerAsset().BL_GetTaxPayerAssetList(mObjTaxPayerAsset);
                ViewBag.AssetList = lstTaxPayerAsset;

                IList<usp_GetBuildingUnitNumberList_Result> lstUnitInformation = new BLBuilding().BL_GetBuildingUnitNumberList(new MAP_Building_BuildingUnit() { BuildingID = id });
                ViewBag.UnitList = lstUnitInformation;

                return View(mObjBuildingModelView);
            }
            else
            {
                return RedirectToAction("Dashboard", "TaxPayerPanel");
            }
        }

        public ActionResult Lands()
        {
            IList<usp_GetTaxPayerLandList_Result> lstTaxPayerAsset = new BLTaxPayerAsset().BL_GetTaxPayerLandList(new MAP_TaxPayer_Asset() { TaxPayerTypeID = SessionManager.TaxpayerTypeID, TaxPayerID = SessionManager.TaxPayerID });
            return View(lstTaxPayerAsset);
        }

        public ActionResult LandDetails()
        {
            int id = SessionManager.TaxPayerID;
            Land mObjLand = new Land()
            {
                LandID = id,
                intStatus = 2
            };

            usp_GetLandList_Result mObjLandData = new BLLand().BL_GetLandDetails(mObjLand);

            if (mObjLandData != null)
            {
                LandViewModel mObjLandModelView = new LandViewModel()
                {
                    LandID = mObjLandData.LandID.GetValueOrDefault(),
                    LandRIN = mObjLandData.LandRIN,
                    PlotNumber = mObjLandData.PlotNumber,
                    StreetName = mObjLandData.StreetName,
                    TownName = mObjLandData.TownName,
                    LGAName = mObjLandData.LGAName,
                    WardName = mObjLandData.WardName,
                    AssetTypeName = mObjLandData.AssetTypeName,
                    LandSize_Length = mObjLandData.LandSize_Length,
                    LandSize_Width = mObjLandData.LandSize_Width,
                    C_OF_O_Ref = mObjLandData.C_OF_O_Ref,
                    LandPurposeName = mObjLandData.LandPurposeName,
                    LandFunctionName = mObjLandData.LandFunctionName,
                    LandOwnershipName = mObjLandData.LandOwnershipName,
                    LandDevelopmentName = mObjLandData.LandDevelopmentName,
                    Latitude = mObjLandData.Latitude,
                    Longitude = mObjLandData.Longitude,
                    ValueOfLand = mObjLandData.ValueOfLand.GetValueOrDefault(),
                    LandStreetConditionName = mObjLandData.LandStreetConditionName,
                    Neighborhood = mObjLandData.Neighborhood,
                    ActiveText = mObjLandData.ActiveText
                };


                MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                {
                    AssetID = id,
                    AssetTypeID = (int)EnumList.AssetTypes.Land
                };

                IList<usp_GetTaxPayerAssetList_Result> lstTaxPayerAsset = new BLTaxPayerAsset().BL_GetTaxPayerAssetList(mObjTaxPayerAsset);
                ViewBag.AssetList = lstTaxPayerAsset;

                return View(mObjLandModelView);
            }
            else
            {
                return RedirectToAction("Dashboard", "TaxPayerPanel");
            }
        }

        public ActionResult Vehicle()
        {
            IList<usp_GetTaxPayerVehicleList_Result> lstTaxPayerVehicle = new BLTaxPayerAsset().BL_GetTaxPayerVehicleList(new MAP_TaxPayer_Asset() { TaxPayerTypeID = SessionManager.TaxpayerTypeID, TaxPayerID = SessionManager.TaxPayerID });
            return View(lstTaxPayerVehicle);
        }

        public ActionResult VehicleDetails()
        {
            int id = SessionManager.TaxPayerID;
            Vehicle mObjVehicle = new Vehicle()
            {
                VehicleID = id,
                intStatus = 2
            };

            usp_GetVehicleList_Result mObjVehicleData = new BLVehicle().BL_GetVehicleDetails(mObjVehicle);

            if (mObjVehicleData != null)
            {
                VehicleViewModel mObjVehicleModelView = new VehicleViewModel()
                {
                    VehicleID = mObjVehicleData.VehicleID.GetValueOrDefault(),
                    VehicleRIN = mObjVehicleData.VehicleRIN,
                    VehicleRegNumber = mObjVehicleData.VehicleRegNumber,
                    VIN = mObjVehicleData.VIN,
                    AssetTypeName = mObjVehicleData.AssetTypeName,
                    VehicleTypeName = mObjVehicleData.VehicleTypeName,
                    VehicleSubTypeName = mObjVehicleData.VehicleSubTypeName,
                    LGAName = mObjVehicleData.LGAName,
                    VehiclePurposeName = mObjVehicleData.VehiclePurposeName,
                    VehicleFunctionName = mObjVehicleData.VehicleFunctionName,
                    VehicleOwnershipName = mObjVehicleData.VehicleOwnershipName,
                    VehicleDescription = mObjVehicleData.VehicleDescription,
                    ActiveText = mObjVehicleData.ActiveText
                };


                MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                {
                    AssetID = id,
                    AssetTypeID = (int)EnumList.AssetTypes.Vehicles
                };

                IList<usp_GetTaxPayerAssetList_Result> lstTaxPayerAsset = new BLTaxPayerAsset().BL_GetTaxPayerAssetList(mObjTaxPayerAsset);
                ViewBag.AssetList = lstTaxPayerAsset;

                return View(mObjVehicleModelView);
            }
            else
            {
                return RedirectToAction("Dashboard", "TaxPayerPanel");
            }
        }

        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult Corporate()
        {
            if (SessionManager.TaxpayerTypeID == (int)EnumList.TaxPayerType.Companies)
            {
                IList<usp_GetCompanyList_Result> lstCompany = new BLCompany().BL_GetCompanyList(new Company() { CompanyID = SessionManager.TaxPayerID, intStatus = 1 });
                return View(lstCompany);
            }
            else
            {
                return RedirectToAction("Dashboard", "TaxPayerPanel");
            }
        }

        public ActionResult CorporateDetails()
        {
            int id = SessionManager.TaxPayerID;
            Company mObjCompany = new Company()
            {
                CompanyID = id,
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

                MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                {
                    TaxPayerID = id,
                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies
                };

                IList<usp_GetTaxPayerAssetList_Result> lstTaxPayerAsset = new BLTaxPayerAsset().BL_GetTaxPayerAssetList(mObjTaxPayerAsset);
                ViewBag.AssetList = lstTaxPayerAsset;

                IList<usp_GetAssessmentRuleInformation_Result> lstAssessmentRuleInformation = new BLTaxPayerAsset().BL_GetTaxPayerAssessmentRuleInformation((int)EnumList.TaxPayerType.Companies, id);
                ViewBag.AssessmentRuleInformation = lstAssessmentRuleInformation;

                IList<usp_GetTaxPayerBill_Result> lstTaxPayerBill = new BLAssessment().BL_GetTaxPayerBill(id, (int)EnumList.TaxPayerType.Companies, 0);
                ViewBag.TaxPayerBill = lstTaxPayerBill;

                IList<usp_GetTaxPayerPayment_Result> lstTaxPayerPayment = new BLSettlement().BL_GetTaxPayerPayment(id, (int)EnumList.TaxPayerType.Companies);
                ViewBag.TaxPayerPayment = lstTaxPayerPayment;

                IList<usp_GetTaxPayerMDAService_Result> lstMDAService = new BLMDAService().BL_GetTaxPayerMDAService((int)EnumList.TaxPayerType.Companies, id);
                ViewBag.MDAService = lstMDAService;

                return View(mObjCompanyModelView);
            }
            else
            {
                return RedirectToAction("Dashboard", "TaxPayerPanel");
            }
        }

        public ActionResult Government()
        {
            if (SessionManager.TaxpayerTypeID == (int)EnumList.TaxPayerType.Government)
            {
                IList<usp_GetGovernmentList_Result> lstGovernment = new BLGovernment().BL_GetGovernmentList(new Government() { GovernmentID = SessionManager.TaxPayerID, intStatus = 1 });
                return View(lstGovernment);
            }
            else
            {
                return RedirectToAction("Dashboard", "TaxPayerPanel");
            }

        }

        public ActionResult GovernmentDetails()
        {
            int id = SessionManager.TaxPayerID;
            Government mObjGovernment = new Government()
            {
                GovernmentID = id,
                intStatus = 1
            };

            usp_GetGovernmentList_Result mObjGovernmentData = new BLGovernment().BL_GetGovernmentDetails(mObjGovernment);

            if (mObjGovernmentData != null)
            {
                GovernmentViewModel mObjGovernmentModelView = new GovernmentViewModel()
                {
                    GovernmentID = mObjGovernmentData.GovernmentID.GetValueOrDefault(),
                    GovernmentRIN = mObjGovernmentData.GovernmentRIN,
                    TIN = mObjGovernmentData.TIN,
                    GovernmentName = mObjGovernmentData.GovernmentName,
                    TaxOfficeName = mObjGovernmentData.TaxOfficeName,
                    TaxPayerTypeName = mObjGovernmentData.TaxPayerTypeName,
                    GovernmentTypeName = mObjGovernmentData.GovernmentTypeName,
                    ContactNumber = mObjGovernmentData.ContactNumber,
                    ContactEmail = mObjGovernmentData.ContactEmail,
                    ContactName = mObjGovernmentData.ContactName,
                    NotificationMethodName = mObjGovernmentData.NotificationMethodName,
                    ContactAddress = mObjGovernmentData.ContactAddress,
                    ActiveText = mObjGovernmentData.ActiveText
                };

                MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                {
                    TaxPayerID = id,
                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Government
                };

                IList<usp_GetTaxPayerAssetList_Result> lstTaxPayerAsset = new BLTaxPayerAsset().BL_GetTaxPayerAssetList(mObjTaxPayerAsset);
                ViewBag.AssetList = lstTaxPayerAsset;

                IList<usp_GetAssessmentRuleInformation_Result> lstAssessmentRuleInformation = new BLTaxPayerAsset().BL_GetTaxPayerAssessmentRuleInformation((int)EnumList.TaxPayerType.Government, id);
                ViewBag.AssessmentRuleInformation = lstAssessmentRuleInformation;

                IList<usp_GetTaxPayerBill_Result> lstTaxPayerBill = new BLAssessment().BL_GetTaxPayerBill(id, (int)EnumList.TaxPayerType.Government, 0);
                ViewBag.TaxPayerBill = lstTaxPayerBill;

                IList<usp_GetTaxPayerPayment_Result> lstTaxPayerPayment = new BLSettlement().BL_GetTaxPayerPayment(id, (int)EnumList.TaxPayerType.Government);
                ViewBag.TaxPayerPayment = lstTaxPayerPayment;

                IList<usp_GetTaxPayerMDAService_Result> lstMDAService = new BLMDAService().BL_GetTaxPayerMDAService((int)EnumList.TaxPayerType.Government, id);
                ViewBag.MDAService = lstMDAService;

                return View(mObjGovernmentModelView);
            }
            else
            {
                return RedirectToAction("Dashboard", "TaxPayerPanel");
            }
        }

        public ActionResult Special()
        {
            if (SessionManager.TaxpayerTypeID == (int)EnumList.TaxPayerType.Special)
            {
                IList<usp_GetSpecialList_Result> lstSpecial = new BLSpecial().BL_GetSpecialList(new Special() { SpecialID = SessionManager.TaxPayerID, intStatus = 1 });
                return View(lstSpecial);
            }
            else
            {
                return RedirectToAction("Dashboard", "TaxPayerPanel");
            }
        }

        public ActionResult SpecialDetails()
        {
            int id = SessionManager.TaxPayerID;
            Special mObjSpecial = new Special()
            {
                SpecialID = id,
                intStatus = 1
            };

            usp_GetSpecialList_Result mObjSpecialData = new BLSpecial().BL_GetSpecialDetails(mObjSpecial);

            if (mObjSpecialData != null)
            {
                SpecialViewModel mObjSpecialModelView = new SpecialViewModel()
                {
                    SpecialID = mObjSpecialData.SpecialID.GetValueOrDefault(),
                    TIN = mObjSpecialData.TIN,
                    SpecialRIN = mObjSpecialData.SpecialRIN,
                    SpecialName = mObjSpecialData.SpecialTaxPayerName,
                    TaxOfficeName = mObjSpecialData.TaxOfficeName,
                    TaxPayerTypeName = mObjSpecialData.TaxPayerTypeName,
                    Description = mObjSpecialData.Description,
                    ContactNumber = mObjSpecialData.ContactNumber,
                    ContactEmail = mObjSpecialData.ContactEmail,
                    ContactName = mObjSpecialData.ContactName,
                    NotificationMethodName = mObjSpecialData.NotificationMethodName,
                    ActiveText = mObjSpecialData.ActiveText
                };

                MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                {
                    TaxPayerID = id,
                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Special
                };

                IList<usp_GetTaxPayerAssetList_Result> lstTaxPayerAsset = new BLTaxPayerAsset().BL_GetTaxPayerAssetList(mObjTaxPayerAsset);
                ViewBag.AssetList = lstTaxPayerAsset;

                IList<usp_GetAssessmentRuleInformation_Result> lstAssessmentRuleInformation = new BLTaxPayerAsset().BL_GetTaxPayerAssessmentRuleInformation((int)EnumList.TaxPayerType.Special, id);
                ViewBag.AssessmentRuleInformation = lstAssessmentRuleInformation;

                IList<usp_GetTaxPayerBill_Result> lstTaxPayerBill = new BLAssessment().BL_GetTaxPayerBill(id, (int)EnumList.TaxPayerType.Special, 0);
                ViewBag.TaxPayerBill = lstTaxPayerBill;

                IList<usp_GetTaxPayerPayment_Result> lstTaxPayerPayment = new BLSettlement().BL_GetTaxPayerPayment(id, (int)EnumList.TaxPayerType.Special);
                ViewBag.TaxPayerPayment = lstTaxPayerPayment;

                IList<usp_GetTaxPayerMDAService_Result> lstMDAService = new BLMDAService().BL_GetTaxPayerMDAService((int)EnumList.TaxPayerType.Special, id);
                ViewBag.MDAService = lstMDAService;

                return View(mObjSpecialModelView);
            }
            else
            {
                return RedirectToAction("Dashboard", "TaxPayerPanel");
            }
        }

        public ActionResult AssessAllBills()
        {
            IList<usp_GetTaxPayerBill_Result> lstBills = new BLAssessment().BL_GetTaxPayerBill(SessionManager.TaxPayerID, SessionManager.TaxpayerTypeID, 1);
            return View(lstBills);
        }

        public ActionResult NotficationList()
        {
            IList<usp_GetNotificationList_Result> lstnotifcation = new BLNotification().BL_GetNotificationList(new Notification() { TaxPayerID = SessionManager.TaxPayerID, TaxPayerTypeID = SessionManager.TaxPayerID, IntStatus = 1 });
            return View(lstnotifcation);
        }

        public ActionResult SettleList()
        {
            IList<usp_GetSettlementList_Result> lstSettlement = new BLSettlement().BL_GetSettlementList(new Settlement() { TaxPayerID = SessionManager.TaxPayerID, TaxPayerTypeID = SessionManager.TaxpayerTypeID });
            return View(lstSettlement);
        }

        public ActionResult SettlePaymentAccount()
        {
            IList<usp_GetPaymentAccountList_Result> lstPayment = new BLPaymentAccount().BL_GetPaymentAccountList(new Payment_Account() { TaxPayerID = SessionManager.TaxPayerID, TaxPayerTypeID = SessionManager.TaxpayerTypeID });
            return View(lstPayment);
        }

        public ActionResult ReportList()
        {
            UI_FillYearDropDown();
            UI_FillMonthDropDown();

            return View();
        }

        [HttpPost]
        public ActionResult ReportList(FormCollection p_ObjFormCollection)
        {
            int year = TrynParse.parseInt(p_ObjFormCollection.Get("TaxYear"));
            int month = TrynParse.parseInt(p_ObjFormCollection.Get("TaxMonth"));

            IList<usp_RPT_GetTaxPayerReport_Result> lstTaxPayerReport = new BLReport().BL_GetTaxPayerReport(year, month, SessionManager.TaxpayerTypeID, SessionManager.TaxPayerID);
            return PartialView("GenerateTaxPayerReportList", lstTaxPayerReport);
        }

        public ActionResult SearchBuilding()
        {
            if (SessionManager.TaxPayerID > 0)
            {
                Individual mObjIndividual = new Individual()
                {
                    IndividualID = SessionManager.TaxPayerID,
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

            IList<usp_GetBusinessList_Result> lstBusiness = new BLBusiness().BL_GetBusinessList(mObjBusiness);
            return PartialView("_BindBusinessTable_SingleSelect", lstBusiness.Take(5).ToList());
        }

        public ActionResult SearchLand(int? id, string name)
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
            UI_FillBusinessCategoryDropDown(new Business_Category() { intStatus = 1, IncludeBusinessCategoryIds = pObjBusinessViewModel.BusinessCategoryID.ToString(), BusinessTypeID = pObjBusinessViewModel.BusinessTypeID });
            UI_FillBusinessSectorDropDown(new Business_Sector() { intStatus = 1, IncludeBusinessSectorIds = pObjBusinessViewModel.BusinessSectorID.ToString(), BusinessTypeID = pObjBusinessViewModel.BusinessTypeID, BusinessCategoryID = pObjBusinessViewModel.BusinessCategoryID });
            UI_FillBusinessSubSectorDropDown(new Business_SubSector() { intStatus = 1, IncludeBusinessSubSectorIds = pObjBusinessViewModel.BusinessSubSectorID.ToString(), BusinessSectorID = pObjBusinessViewModel.BusinessSectorID });
            UI_FillBusinessStructureDropDown(new Business_Structure() { intStatus = 1, IncludeBusinessStructureIds = pObjBusinessViewModel.BusinessStructureID.ToString(), BusinessTypeID = pObjBusinessViewModel.BusinessTypeID });
            UI_FillBusinessOperationDropDown(new Business_Operation() { intStatus = 1, IncludeBusinessOperationIds = pObjBusinessViewModel.BusinessOperationID.ToString(), BusinessTypeID = pObjBusinessViewModel.BusinessTypeID });
            UI_FillSizeDropDown(new Size() { intStatus = 1, IncludeSizeIds = pObjBusinessViewModel.SizeID.ToString() });
        }

        public ActionResult AddBusiness(int? id, string name)
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
                    catch (Exception Ex)
                    {
                        ErrorSignal.FromCurrentContext().Raise(Ex);
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
                    catch (Exception Ex)
                    {
                        ErrorSignal.FromCurrentContext().Raise(Ex);
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

                    UI_FillVehicleDropDown();

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
                    catch (Exception Ex)
                    {
                        ErrorSignal.FromCurrentContext().Raise(Ex);
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

    }

}