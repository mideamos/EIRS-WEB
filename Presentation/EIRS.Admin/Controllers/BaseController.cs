using System.Web.Mvc;
using System.Web.Security;
using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;
using System.Collections.Generic;
using System;
using System.Linq;
using Elmah;

namespace EIRS.Admin.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (SessionManager.SystemUserID != 0)
            {
                base.OnActionExecuting(filterContext);
            }
            else if (SessionManager.SystemUserID == 0 && Request.IsAjaxRequest())
            {
                FormsAuthentication.SignOut();
                Session.Clear();
                filterContext.HttpContext.Response.StatusCode = 401;
                filterContext.HttpContext.Response.StatusDescription = "UserLoggedOut";
                filterContext.HttpContext.Response.End();
            }
            else
            {
                FormsAuthentication.RedirectToLoginPage(filterContext.HttpContext.Request.Url.PathAndQuery);
                FormsAuthentication.SignOut();

                Session.Clear();
                filterContext.Result = new RedirectResult("/Login.aspx?returnUrl=" + filterContext.HttpContext.Request.Url.PathAndQuery);
            }
        }

        public void UI_FillCompanyDropDown(Company pObjCompany = null)
        {
            if (pObjCompany == null)
                pObjCompany = new Company();

            pObjCompany.intStatus = 1;

            IList<DropDownListResult> lstCompany = new BLCompany().BL_GetCompanyDropDownList(pObjCompany);
            ViewBag.CompanyList = new SelectList(lstCompany, "id", "text");
        }

        public void UI_FillVehicleDropDown(Vehicle pObjVehicle = null)
        {
            if (pObjVehicle == null)
                pObjVehicle = new Vehicle();

            pObjVehicle.intStatus = 1;

            IList<DropDownListResult> lstVehicle = new BLVehicle().BL_GetVehicleDropDownList(pObjVehicle);
            ViewBag.VehicleList = new SelectList(lstVehicle, "id", "text");
        }

        public void UI_FillDealerType()
        {
            IList<DropDownListResult> lstDealerType = new BLCommon().BL_GetDealerTypeDropDownList();
            ViewBag.DealerTypeList = new SelectList(lstDealerType, "id", "text");
        }
        public void UI_FillLGAClass()
        {
            IList<DropDownListResult> lstLGAClass = new BLLGA().BL_GetLGAClassDropDownList();
            ViewBag.LGAClassList = new SelectList(lstLGAClass, "id", "text");
        }
        public usp_GetLGAList_Result getLgaName(int lgaId)
        {
            LGA gA = new LGA
            { 
                LGAID = lgaId
            };

           return new BLLGA().BL_GetLGADetails(gA);
        }

        public void UI_FillGender()
        {
            IList<DropDownListResult> lstGender = new BLCommon().BL_GetGenderDropDownList();
            ViewBag.GenderList = new SelectList(lstGender, "id", "text");
        }

        public void UI_FillMaritalStatus()
        {
            IList<DropDownListResult> lstMaritalStatus = new BLCommon().BL_GetMaritalStatusDropDownList();
            ViewBag.MaritalStatusList = new SelectList(lstMaritalStatus, "id", "text");
        }

        public void UI_FillNationality()
        {
            IList<DropDownListResult> lstNationality = new BLCommon().BL_GetNationalityDropDownList();
            ViewBag.NationalityList = new SelectList(lstNationality, "id", "text");
        }

        public void UI_FillComputation()
        {
            IList<DropDownListResult> lstComputation = new BLCommon().BL_GetComputationDropDownList();
            ViewBag.ComputationList = new SelectList(lstComputation, "id", "text");
        }

        public void UI_FillRuleRun()
        {
            IList<DropDownListResult> lstRuleRun = new BLCommon().BL_GetRuleRunDropDownList();
            ViewBag.RuleRunList = new SelectList(lstRuleRun, "id", "text");
        }

        public void UI_FillReviewStatus()
        {
            IList<DropDownListResult> lstReviewStatus = new BLCommon().BL_GetReviewStatusDropDownList();
            ViewBag.ReviewStatusList = new SelectList(lstReviewStatus, "id", "text");
        }

        public void UI_FillLGADropDown(LGA pObjLGA = null)
        {
            if (pObjLGA == null)
                pObjLGA = new LGA();

            pObjLGA.intStatus = 1;

            IList<DropDownListResult> lstLGA = new BLLGA().BL_GetLGADropDownList(pObjLGA);
            ViewBag.LGAList = new SelectList(lstLGA, "id", "text");
        }

        public void UI_FillTownDropDown(Town pObjTown = null)
        {
            if (pObjTown == null)
                pObjTown = new Town();

            pObjTown.intStatus = 1;

            IList<DropDownListResult> lstTown = new BLTown().BL_GetTownDropDownList(pObjTown);
            ViewBag.TownList = new SelectList(lstTown, "id", "text");
        }

        public void UI_FillWardDropDown(Ward pObjWard = null)
        {
            if (pObjWard == null)
                pObjWard = new Ward();

            pObjWard.intStatus = 1;

            IList<DropDownListResult> lstWard = new BLWard().BL_GetWardDropDownList(pObjWard);
            ViewBag.WardList = new SelectList(lstWard, "id", "text");
        }

        public void UI_FillAssetTypeDropDown(Asset_Types pObjAssetType = null, int pIntSelectedAssetTypeID = 0)
        {
            if (pObjAssetType == null)
                pObjAssetType = new Asset_Types();

            pObjAssetType.intStatus = 1;

            IList<DropDownListResult> lstAssetType = new BLAssetType().BL_GetAssetTypeDropDownList(pObjAssetType);
            ViewBag.AssetTypeList = new SelectList(lstAssetType, "id", "text", pIntSelectedAssetTypeID);
        }

        public void UI_FillAddressTypeDropDown(Address_Types pObjAddressType = null)
        {
            if (pObjAddressType == null)
                pObjAddressType = new Address_Types();

            pObjAddressType.intStatus = 1;

            IList<DropDownListResult> lstAddressType = new BLAddressType().BL_GetAddressTypeDropDownList(pObjAddressType);
            ViewBag.AddressTypeList = new SelectList(lstAddressType, "id", "text");
        }

        public void UI_FillTaxOfficeDropDown(Tax_Offices pObjTaxOffice = null)
        {
            if (pObjTaxOffice == null)
                pObjTaxOffice = new Tax_Offices();

            pObjTaxOffice.intStatus = 1;

            IList<DropDownListResult> lstTaxOffice = new BLTaxOffice().BL_GetTaxOfficeDropDownList(pObjTaxOffice);
            ViewBag.TaxOfficeList = new SelectList(lstTaxOffice, "id", "text");
        }

        public void UI_FillBuildingTypeDropDown(Building_Types pObjBuildingType = null)
        {
            if (pObjBuildingType == null)
                pObjBuildingType = new Building_Types();

            pObjBuildingType.intStatus = 1;

            IList<DropDownListResult> lstBuildingType = new BLBuildingType().BL_GetBuildingTypeDropDownList(pObjBuildingType);
            ViewBag.BuildingTypeList = new SelectList(lstBuildingType, "id", "text");
        }

        public void UI_FillBuildingCompletionDropDown(Building_Completion pObjBuildingCompletion = null)
        {
            if (pObjBuildingCompletion == null)
                pObjBuildingCompletion = new Building_Completion();

            pObjBuildingCompletion.intStatus = 1;

            IList<DropDownListResult> lstBuildingCompletion = new BLBuildingCompletion().BL_GetBuildingCompletionDropDownList(pObjBuildingCompletion);
            ViewBag.BuildingCompletionList = new SelectList(lstBuildingCompletion, "id", "text");
        }

        public void UI_FillBuildingPurposeDropDown(Building_Purpose pObjBuildingPurpose = null)
        {
            if (pObjBuildingPurpose == null)
                pObjBuildingPurpose = new Building_Purpose();

            pObjBuildingPurpose.intStatus = 1;

            IList<DropDownListResult> lstBuildingPurpose = new BLBuildingPurpose().BL_GetBuildingPurposeDropDownList(pObjBuildingPurpose);
            ViewBag.BuildingPurposeList = new SelectList(lstBuildingPurpose, "id", "text");
        }

        public void UI_FillUnitPurposeDropDown(Unit_Purpose pObjUnitPurpose = null)
        {
            if (pObjUnitPurpose == null)
                pObjUnitPurpose = new Unit_Purpose();

            pObjUnitPurpose.intStatus = 1;

            IList<DropDownListResult> lstUnitPurpose = new BLUnitPurpose().BL_GetUnitPurposeDropDownList(pObjUnitPurpose);
            ViewBag.UnitPurposeList = new SelectList(lstUnitPurpose, "id", "text");
        }

        public void UI_FillUnitFunctionDropDown(Unit_Function pObjUnitFunction = null)
        {
            if (pObjUnitFunction == null)
                pObjUnitFunction = new Unit_Function();

            pObjUnitFunction.intStatus = 1;

            IList<DropDownListResult> lstUnitFunction = new BLUnitFunction().BL_GetUnitFunctionDropDownList(pObjUnitFunction);
            ViewBag.UnitFunctionList = new SelectList(lstUnitFunction, "id", "text");
        }

        public void UI_FillBuildingOwnershipDropDown(Building_Ownership pObjBuildingOwnership = null)
        {
            if (pObjBuildingOwnership == null)
                pObjBuildingOwnership = new Building_Ownership();

            pObjBuildingOwnership.intStatus = 1;

            IList<DropDownListResult> lstBuildingOwnership = new BLBuildingOwnership().BL_GetBuildingOwnershipDropDownList(pObjBuildingOwnership);
            ViewBag.BuildingOwnershipList = new SelectList(lstBuildingOwnership, "id", "text");
        }

        public void UI_FillUnitOccupancyDropDown(Unit_Occupancy pObjUnitOccupancy = null)
        {
            if (pObjUnitOccupancy == null)
                pObjUnitOccupancy = new Unit_Occupancy();

            pObjUnitOccupancy.intStatus = 1;

            IList<DropDownListResult> lstUnitOccupancy = new BLUnitOccupancy().BL_GetUnitOccupancyDropDownList(pObjUnitOccupancy);
            ViewBag.UnitOccupancyList = new SelectList(lstUnitOccupancy, "id", "text");
        }

        public void UI_FillTaxPayerTypeDropDown(TaxPayer_Types pObjTaxPayerType = null, int pIntSelectedTaxPayerTypeID = 0)
        {
            if (pObjTaxPayerType == null)
                pObjTaxPayerType = new TaxPayer_Types();

            pObjTaxPayerType.intStatus = 1;

            IList<DropDownListResult> lstTaxPayerType = new BLTaxPayerType().BL_GetTaxPayerTypeDropDownList(pObjTaxPayerType);
            ViewBag.TaxPayerTypeList = new SelectList(lstTaxPayerType, "id", "text", pIntSelectedTaxPayerTypeID);
        }

        public void UI_FillTaxPayerRoleDropDown(TaxPayer_Roles pObjTaxPayerRole = null, int pIntSelectedTaxPayerRoleID = 0)
        {
            if (pObjTaxPayerRole == null)
                pObjTaxPayerRole = new TaxPayer_Roles();

            pObjTaxPayerRole.intStatus = 1;

            IList<DropDownListResult> lstTaxPayerRole = new BLTaxPayerRole().BL_GetTaxPayerRoleDropDownList(pObjTaxPayerRole);
            ViewBag.TaxPayerRoleList = new SelectList(lstTaxPayerRole, "id", "text", pIntSelectedTaxPayerRoleID);
        }

        public void UI_FillTitleDropDown(Title pObjTitle = null)
        {
            if (pObjTitle == null)
                pObjTitle = new Title();

            pObjTitle.intStatus = 1;

            IList<DropDownListResult> lstTitle = new BLTitle().BL_GetTitleDropDownList(pObjTitle);
            ViewBag.TitleList = new SelectList(lstTitle, "id", "text");
        }

        public void UI_FillEconomicActivitiesDropDown(Economic_Activities pObjEconomicActivities = null)
        {
            if (pObjEconomicActivities == null)
                pObjEconomicActivities = new Economic_Activities();

            pObjEconomicActivities.intStatus = 1;

            IList<DropDownListResult> lstEconomicActivities = new BLEconomicActivities().BL_GetEconomicActivitiesDropDownList(pObjEconomicActivities);
            ViewBag.EconomicActivitiesList = new SelectList(lstEconomicActivities, "id", "text");
        }

        public void UI_FillVehiclePurposeDropDown(Vehicle_Purpose pObjVehiclePurpose = null)
        {
            if (pObjVehiclePurpose == null)
                pObjVehiclePurpose = new Vehicle_Purpose();

            pObjVehiclePurpose.intStatus = 1;

            IList<DropDownListResult> lstVehiclePurpose = new BLVehiclePurpose().BL_GetVehiclePurposeDropDownList(pObjVehiclePurpose);
            ViewBag.VehiclePurposeList = new SelectList(lstVehiclePurpose, "id", "text");
        }

        public void UI_FillVehicleFunctionDropDown(Vehicle_Function pObjVehicleFunction = null)
        {
            if (pObjVehicleFunction == null)
                pObjVehicleFunction = new Vehicle_Function();

            pObjVehicleFunction.intStatus = 1;

            IList<DropDownListResult> lstVehicleFunction = new BLVehicleFunction().BL_GetVehicleFunctionDropDownList(pObjVehicleFunction);
            ViewBag.VehicleFunctionList = new SelectList(lstVehicleFunction, "id", "text");
        }

        public void UI_FillVehicleOwnershipDropDown(Vehicle_Ownership pObjVehicleOwnership = null)
        {
            if (pObjVehicleOwnership == null)
                pObjVehicleOwnership = new Vehicle_Ownership();

            pObjVehicleOwnership.intStatus = 1;

            IList<DropDownListResult> lstVehicleOwnership = new BLVehicleOwnership().BL_GetVehicleOwnershipDropDownList(pObjVehicleOwnership);
            ViewBag.VehicleOwnershipList = new SelectList(lstVehicleOwnership, "id", "text");
        }

        public void UI_FillVehicleInsuranceDropDown(Vehicle_Insurance pObjVehicleInsurance = null)
        {
            if (pObjVehicleInsurance == null)
                pObjVehicleInsurance = new Vehicle_Insurance();

            pObjVehicleInsurance.IntStatus = 1;

            IList<DropDownListResult> lstVehicleInsurance = new BLVehicleInsurance().BL_GetVehicleInsuranceDropDownList(pObjVehicleInsurance);
            ViewBag.VehicleInsuranceList = new SelectList(lstVehicleInsurance, "id", "text");
        }

        public void UI_FillVehicleTypeDropDown(Vehicle_Types pObjVehicleType = null)
        {
            if (pObjVehicleType == null)
                pObjVehicleType = new Vehicle_Types();

            pObjVehicleType.intStatus = 1;

            IList<DropDownListResult> lstVehicleType = new BLVehicleType().BL_GetVehicleTypeDropDownList(pObjVehicleType);
            ViewBag.VehicleTypeList = new SelectList(lstVehicleType, "id", "text");
        }

        public void UI_FillVehicleSubTypeDropDown(Vehicle_SubTypes pObjVehicleSubType = null)
        {
            if (pObjVehicleSubType == null)
                pObjVehicleSubType = new Vehicle_SubTypes();

            pObjVehicleSubType.intStatus = 1;

            IList<DropDownListResult> lstVehicleSubType = new BLVehicleSubType().BL_GetVehicleSubTypeDropDownList(pObjVehicleSubType);
            ViewBag.VehicleSubTypeList = new SelectList(lstVehicleSubType, "id", "text");
        }

        public void UI_FillBusinessTypeDropDown(Business_Types pObjBusinessType = null)
        {
            if (pObjBusinessType == null)
                pObjBusinessType = new Business_Types();

            pObjBusinessType.intStatus = 1;

            IList<DropDownListResult> lstBusinessType = new BLBusinessType().BL_GetBusinessTypeDropDownList(pObjBusinessType);
            ViewBag.BusinessTypeList = new SelectList(lstBusinessType, "id", "text");
        }

        public void UI_FillBusinessCategoryDropDown(Business_Category pObjBusinessCategory = null)
        {
            if (pObjBusinessCategory == null)
                pObjBusinessCategory = new Business_Category();

            pObjBusinessCategory.intStatus = 1;

            IList<DropDownListResult> lstBusinessCategory = new BLBusinessCategory().BL_GetBusinessCategoryDropDownList(pObjBusinessCategory);
            ViewBag.BusinessCategoryList = new SelectList(lstBusinessCategory, "id", "text");
        }

        public void UI_FillBusinessSectorDropDown(Business_Sector pObjBusinessSector = null)
        {
            if (pObjBusinessSector == null)
                pObjBusinessSector = new Business_Sector();

            pObjBusinessSector.intStatus = 1;

            IList<DropDownListResult> lstBusinessSector = new BLBusinessSector().BL_GetBusinessSectorDropDownList(pObjBusinessSector);
            ViewBag.BusinessSectorList = new SelectList(lstBusinessSector, "id", "text");
        }

        public void UI_FillBusinessSubSectorDropDown(Business_SubSector pObjBusinessSubSector = null)
        {
            if (pObjBusinessSubSector == null)
                pObjBusinessSubSector = new Business_SubSector();

            pObjBusinessSubSector.intStatus = 1;

            IList<DropDownListResult> lstBusinessSubSector = new BLBusinessSubSector().BL_GetBusinessSubSectorDropDownList(pObjBusinessSubSector);
            ViewBag.BusinessSubSectorList = new SelectList(lstBusinessSubSector, "id", "text");
        }

        public void UI_FillBusinessStructureDropDown(Business_Structure pObjBusinessStructure = null)
        {
            if (pObjBusinessStructure == null)
                pObjBusinessStructure = new Business_Structure();

            pObjBusinessStructure.intStatus = 1;

            IList<DropDownListResult> lstBusinessStructure = new BLBusinessStructure().BL_GetBusinessStructureDropDownList(pObjBusinessStructure);
            ViewBag.BusinessStructureList = new SelectList(lstBusinessStructure, "id", "text");
        }

        public void UI_FillBusinessOperationDropDown(Business_Operation pObjBusinessOperation = null)
        {
            if (pObjBusinessOperation == null)
                pObjBusinessOperation = new Business_Operation();

            pObjBusinessOperation.intStatus = 1;

            IList<DropDownListResult> lstBusinessOperation = new BLBusinessOperation().BL_GetBusinessOperationDropDownList(pObjBusinessOperation);
            ViewBag.BusinessOperationList = new SelectList(lstBusinessOperation, "id", "text");
        }

        public void UI_FillAgencyTypeDropDown(Agency_Types pObjAgencyType = null)
        {
            if (pObjAgencyType == null)
                pObjAgencyType = new Agency_Types();

            pObjAgencyType.intStatus = 1;

            IList<DropDownListResult> lstAgencyType = new BLAgencyType().BL_GetAgencyTypeDropDownList(pObjAgencyType);
            ViewBag.AgencyTypeList = new SelectList(lstAgencyType, "id", "text");
        }

        public void UI_FillAgencyDropDown(Agency pObjAgency = null)
        {
            if (pObjAgency == null)
                pObjAgency = new Agency();

            pObjAgency.intStatus = 1;

            IList<DropDownListResult> lstAgency = new BLAgency().BL_GetAgencyDropDownList(pObjAgency);
            ViewBag.AgencyList = new SelectList(lstAgency, "id", "text");
        }

        public void UI_FillRevenueStreamDropDown(Revenue_Stream pObjRevenueStream = null)
        {
            if (pObjRevenueStream == null)
                pObjRevenueStream = new Revenue_Stream();

            pObjRevenueStream.intStatus = 1;

            IList<DropDownListResult> lstRevenueStream = new BLRevenueStream().BL_GetRevenueStreamDropDownList(pObjRevenueStream);
            ViewBag.RevenueStreamList = new SelectList(lstRevenueStream, "id", "text");
        }

        public void UI_FillRevenueSubStreamDropDown(Revenue_SubStream pObjRevenueSubStream = null)
        {
            if (pObjRevenueSubStream == null)
                pObjRevenueSubStream = new Revenue_SubStream();

            pObjRevenueSubStream.intStatus = 1;

            IList<DropDownListResult> lstRevenueSubStream = new BLRevenueSubStream().BL_GetRevenueSubStreamDropDownList(pObjRevenueSubStream);
            ViewBag.RevenueSubStreamList = new SelectList(lstRevenueSubStream, "id", "text");
        }

        public void UI_FillAssessmentGroupDropDown(Assessment_Group pObjAssessmentGroup = null)
        {
            if (pObjAssessmentGroup == null)
                pObjAssessmentGroup = new Assessment_Group();

            pObjAssessmentGroup.intStatus = 1;

            IList<DropDownListResult> lstAssessmentGroup = new BLAssessmentGroup().BL_GetAssessmentGroupDropDownList(pObjAssessmentGroup);
            ViewBag.AssessmentGroupList = new SelectList(lstAssessmentGroup, "id", "text");
        }

        public void UI_FillAssessmentSubGroupDropDown(Assessment_SubGroup pObjAssessmentSubGroup = null)
        {
            if (pObjAssessmentSubGroup == null)
                pObjAssessmentSubGroup = new Assessment_SubGroup();

            pObjAssessmentSubGroup.intStatus = 1;

            IList<DropDownListResult> lstAssessmentSubGroup = new BLAssessmentSubGroup().BL_GetAssessmentSubGroupDropDownList(pObjAssessmentSubGroup);
            ViewBag.AssessmentSubGroupList = new SelectList(lstAssessmentSubGroup, "id", "text");
        }

        public void UI_FillAssessmentItemCategoryDropDown(Assessment_Item_Category pObjAssessmentItemCategory = null)
        {
            if (pObjAssessmentItemCategory == null)
                pObjAssessmentItemCategory = new Assessment_Item_Category();

            pObjAssessmentItemCategory.intStatus = 1;

            IList<DropDownListResult> lstAssessmentItem_Category = new BLAssessmentItemCategory().BL_GetAssessmentItemCategoryDropDownList(pObjAssessmentItemCategory);
            ViewBag.AssessmentItemCategoryList = new SelectList(lstAssessmentItem_Category, "id", "text");
        }

        public void UI_FillAssessmentItemSubCategoryDropDown(Assessment_Item_SubCategory pObjAssessmentItemSubCategory = null)
        {
            if (pObjAssessmentItemSubCategory == null)
                pObjAssessmentItemSubCategory = new Assessment_Item_SubCategory();

            pObjAssessmentItemSubCategory.intStatus = 1;

            IList<DropDownListResult> lstAssessmentItem_SubCategory = new BLAssessmentItemSubCategory().BL_GetAssessmentItemSubCategoryDropDownList(pObjAssessmentItemSubCategory);
            ViewBag.AssessmentItemSubCategoryList = new SelectList(lstAssessmentItem_SubCategory, "id", "text");
        }

        public void UI_FillLandPurposeDropDown(Land_Purpose pObjLandPurpose = null)
        {
            if (pObjLandPurpose == null)
                pObjLandPurpose = new Land_Purpose();

            pObjLandPurpose.intStatus = 1;

            IList<DropDownListResult> lstLandPurpose = new BLLandPurpose().BL_GetLandPurposeDropDownList(pObjLandPurpose);
            ViewBag.LandPurposeList = new SelectList(lstLandPurpose, "id", "text");
        }

        public void UI_FillLandFunctionDropDown(Land_Function pObjLandFunction = null)
        {
            if (pObjLandFunction == null)
                pObjLandFunction = new Land_Function();

            pObjLandFunction.intStatus = 1;

            IList<DropDownListResult> lstLandFunction = new BLLandFunction().BL_GetLandFunctionDropDownList(pObjLandFunction);
            ViewBag.LandFunctionList = new SelectList(lstLandFunction, "id", "text");
        }

        public void UI_FillLandDevelopmentDropDown(Land_Development pObjLandDevelopment = null)
        {
            if (pObjLandDevelopment == null)
                pObjLandDevelopment = new Land_Development();

            pObjLandDevelopment.intStatus = 1;

            IList<DropDownListResult> lstLandDevelopment = new BLLandDevelopment().BL_GetLandDevelopmentDropDownList(pObjLandDevelopment);
            ViewBag.LandDevelopmentList = new SelectList(lstLandDevelopment, "id", "text");
        }

        public void UI_FillLandStreetConditionDropDown(Land_StreetCondition pObjLandStreetCondition = null)
        {
            if (pObjLandStreetCondition == null)
                pObjLandStreetCondition = new Land_StreetCondition();

            pObjLandStreetCondition.intStatus = 1;

            IList<DropDownListResult> lstLandStreetCondition = new BLLandStreetCondition().BL_GetLandStreetConditionDropDownList(pObjLandStreetCondition);
            ViewBag.LandStreetConditionList = new SelectList(lstLandStreetCondition, "id", "text");
        }

        public void UI_FillLandOwnershipDropDown(Land_Ownership pObjLandOwnership = null)
        {
            if (pObjLandOwnership == null)
                pObjLandOwnership = new Land_Ownership();

            pObjLandOwnership.intStatus = 1;

            IList<DropDownListResult> lstLandOwnership = new BLLandOwnership().BL_GetLandOwnershipDropDownList(pObjLandOwnership);
            ViewBag.LandOwnershipList = new SelectList(lstLandOwnership, "id", "text");
        }

        public void UI_FillPaymentFrequencyDropDown(Payment_Frequency pObjPaymentFrequency = null)
        {
            if (pObjPaymentFrequency == null)
                pObjPaymentFrequency = new Payment_Frequency();

            pObjPaymentFrequency.intStatus = 1;

            IList<DropDownListResult> lstPaymentFrequency = new BLPaymentFrequency().BL_GetPaymentFrequencyDropDownList(pObjPaymentFrequency);
            ViewBag.PaymentFrequencyList = new SelectList(lstPaymentFrequency, "id", "text");
        }

        public void UI_FillPaymentOptionDropDown(Payment_Options pObjPaymentOption = null)
        {
            if (pObjPaymentOption == null)
                pObjPaymentOption = new Payment_Options();

            pObjPaymentOption.intStatus = 1;

            IList<DropDownListResult> lstPaymentOption = new BLPaymentOption().BL_GetPaymentOptionDropDownList(pObjPaymentOption);
            ViewBag.PaymentOptionList = new SelectList(lstPaymentOption, "id", "text");
        }

        public void UI_FillSettlementMethodDropDown(Settlement_Method pObjSettlementMethod = null)
        {
            if (pObjSettlementMethod == null)
                pObjSettlementMethod = new Settlement_Method();

            pObjSettlementMethod.intStatus = 1;

            IList<DropDownListResult> lstSettlementMethod = new BLSettlementMethod().BL_GetSettlementMethodDropDownList(pObjSettlementMethod);
            ViewBag.SettlementMethodList = new SelectList(lstSettlementMethod, "id", "text");
        }

        public void UI_FillYearDropDown()
        {
            IList<DropDownListResult> lstYear = new List<DropDownListResult>();
            int mIntCurrentYear = 2017;//CommUtil.GetCurrentDateTime().AddYears(-1).Year;
            for (int i = mIntCurrentYear; i <= DateTime.Now.AddYears(1).Year; i++)
            {
                lstYear.Add(new DropDownListResult() { id = i, text = i.ToString() });
            }

            ViewBag.YearList = new SelectList(lstYear, "id", "text");

        }

        public void UI_FillCoverTypeDropDown()
        {
            IList<DropDownListResult> lstCoverType = new List<DropDownListResult>
            {
                new DropDownListResult() { id = 1, text = "Third Party" },
                new DropDownListResult() { id = 2, text = "Comprehensive" }
            };

            ViewBag.CoverTypeList = new SelectList(lstCoverType, "id", "text");
        }

        public void UI_FillInsuranceStatusDropDown()
        {
            IList<DropDownListResult> lstInsuranceStatus = new List<DropDownListResult>
            {
                new DropDownListResult() { id = 1, text = "Active" },
                new DropDownListResult() { id = 2, text = "Expired" }
            };

            ViewBag.InsuranceStatusList = new SelectList(lstInsuranceStatus, "id", "text");
        }

        public void UI_FillSizeDropDown(Size pObjSize = null)
        {
            if (pObjSize == null)
                pObjSize = new Size();

            pObjSize.intStatus = 1;

            IList<DropDownListResult> lstSize = new BLSize().BL_GetSizeDropDownList(pObjSize);
            ViewBag.SizeList = new SelectList(lstSize, "id", "text");
        }

        public void UI_FillGovernmentTypeDropDown(Government_Types pObjGovernmentType = null)
        {
            if (pObjGovernmentType == null)
                pObjGovernmentType = new Government_Types();

            pObjGovernmentType.intStatus = 1;

            IList<DropDownListResult> lstGovernmentType = new BLGovernmentType().BL_GetGovernmentTypeDropDownList(pObjGovernmentType);
            ViewBag.GovernmentTypeList = new SelectList(lstGovernmentType, "id", "text");
        }

        public void UI_FillNotificationMethodDropDown(Notification_Method pObjNotificationMethod = null)
        {
            if (pObjNotificationMethod == null)
                pObjNotificationMethod = new Notification_Method();

            pObjNotificationMethod.intStatus = 1;

            IList<DropDownListResult> lstNotificationMethod = new BLNotificationMethod().BL_GetNotificationMethodDropDownList(pObjNotificationMethod);
            ViewBag.NotificationMethodList = new SelectList(lstNotificationMethod, "id", "text");
        }

        public void UI_FillApproverDropDown(MST_Users pObjUsers = null)
        {
            if (pObjUsers == null)
                pObjUsers = new MST_Users();

            pObjUsers.intStatus = 1;

            IList<DropDownListResult> lstUsers = new BLUser().BL_GetApproverList(pObjUsers);
            ViewBag.UserList = new SelectList(lstUsers, "id", "text");
        }

        public JsonResult GetWard(int LGAID)
        {
            IList<DropDownListResult> lstWard = new BLWard().BL_GetWardDropDownList(new Ward() { intStatus = 1, LGAID = LGAID });
            return Json(lstWard, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUnitFunction(int UnitPurposeID)
        {
            IList<DropDownListResult> lstUnitFunction = new BLUnitFunction().BL_GetUnitFunctionDropDownList(new Unit_Function() { intStatus = 1, UnitPurposeID = UnitPurposeID });
            return Json(lstUnitFunction, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetTitle(int GenderID)
        {
            IList<DropDownListResult> lstTitle = new BLTitle().BL_GetTitleDropDownList(new Title() { intStatus = 1, GenderID = GenderID });
            return Json(lstTitle, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTownDetails(int TownID)
        {
            usp_GetTownList_Result mObjTownDetails = new BLTown().BL_GetTownDetails(new Town() { intStatus = 1, TownID = TownID });
            return Json(mObjTownDetails, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetVehicleSubType(int VehicleTypeID)
        {
            IList<DropDownListResult> lstVehicleSubType = new BLVehicleSubType().BL_GetVehicleSubTypeDropDownList(new Vehicle_SubTypes() { intStatus = 1, VehicleTypeID = VehicleTypeID });
            return Json(lstVehicleSubType, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetVehicleFunction(int VehiclePurposeID)
        {
            IList<DropDownListResult> lstVehicleFunction = new BLVehicleFunction().BL_GetVehicleFunctionDropDownList(new Vehicle_Function() { intStatus = 1, VehiclePurposeID = VehiclePurposeID });
            return Json(lstVehicleFunction, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLandFunction(int LandPurposeID)
        {
            IList<DropDownListResult> lstLandFunction = new BLLandFunction().BL_GetLandFunctionDropDownList(new Land_Function() { intStatus = 1, LandPurposeID = LandPurposeID });
            return Json(lstLandFunction, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBusinessCategory(int BusinessTypeID)
        {
            IList<DropDownListResult> lstBusinessCategory = new BLBusinessCategory().BL_GetBusinessCategoryDropDownList(new Business_Category() { intStatus = 1, BusinessTypeID = BusinessTypeID });
            return Json(lstBusinessCategory, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBusinessSector(int BusinessCategoryID)
        {
            IList<DropDownListResult> lstBusinessSector = new BLBusinessSector().BL_GetBusinessSectorDropDownList(new Business_Sector() { intStatus = 1, BusinessCategoryID = BusinessCategoryID });
            return Json(lstBusinessSector, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBusinessSubSector(int BusinessSectorID)
        {
            IList<DropDownListResult> lstBusinessSubSector = new BLBusinessSubSector().BL_GetBusinessSubSectorDropDownList(new Business_SubSector() { intStatus = 1, BusinessSectorID = BusinessSectorID });
            return Json(lstBusinessSubSector, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult GetRevenueStream(int AssetTypeID)
        //{
        //    IList<DropDownListResult> lstRevenueStream = new BLRevenueStream().BL_GetRevenueStreamDropDownList(new Revenue_Stream() { intStatus = 1, AssetTypeID = AssetTypeID });
        //    return Json(lstRevenueStream, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult GetRevenueSubStream(int RevenueStreamID)
        {
            IList<DropDownListResult> lstRevenueSubStream = new BLRevenueSubStream().BL_GetRevenueSubStreamDropDownList(new Revenue_SubStream() { intStatus = 1, RevenueStreamID = RevenueStreamID });
            return Json(lstRevenueSubStream, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAssessmentGroup(int AssetTypeID)
        {
            IList<DropDownListResult> lstAssessmentGroup = new BLAssessmentGroup().BL_GetAssessmentGroupDropDownList(new Assessment_Group() { intStatus = 1, AssetTypeID = AssetTypeID });
            return Json(lstAssessmentGroup, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAssessmentSubGroup(int AssessmentGroupID)
        {
            IList<DropDownListResult> lstAssessmentSubGroup = new BLAssessmentSubGroup().BL_GetAssessmentSubGroupDropDownList(new Assessment_SubGroup() { intStatus = 1, AssessmentGroupID = AssessmentGroupID });
            return Json(lstAssessmentSubGroup, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAssessmentItemSubCategory(int AssessmentItemCategoryID)
        {
            IList<DropDownListResult> lstAssessmentItemSubCategory = new BLAssessmentItemSubCategory().BL_GetAssessmentItemSubCategoryDropDownList(new Assessment_Item_SubCategory() { intStatus = 1, AssessmentItemCategoryID = AssessmentItemCategoryID });
            return Json(lstAssessmentItemSubCategory, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTaxPayerRole(int TaxPayerTypeID)
        {
            IList<DropDownListResult> lstTaxPayerRole = new BLTaxPayerRole().BL_GetTaxPayerRoleDropDownList(new TaxPayer_Roles() { intStatus = 1, TaxPayerTypeID = TaxPayerTypeID });
            return Json(lstTaxPayerRole, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTaxPayerRole_ATTP(int AssetTypeID, int TaxPayerTypeID)
        {
            IList<DropDownListResult> lstTaxPayerRole = new BLTaxPayerRole().BL_GetTaxPayerRoleDropDownList(new TaxPayer_Roles() { intStatus = 1, AssetTypeID = AssetTypeID, TaxPayerTypeID = TaxPayerTypeID });
            return Json(lstTaxPayerRole, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAssessmentItemDetails(int AssessmentItemID)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            usp_GetAssessmentItemList_Result mObjAssessmentItemDetails = new BLAssessmentItem().BL_GetAssessmentItemDetails(new Assessment_Items() { intStatus = 2, AssessmentItemID = AssessmentItemID });

            if (mObjAssessmentItemDetails != null)
            {
                dcResponse["success"] = true;
                dcResponse["AssessmentItemDetails"] = mObjAssessmentItemDetails;
            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Request";
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMDAServiceItemDetails(int MDAServiceItemID)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            usp_GetMDAServiceItemList_Result mObjMDAServiceItemDetails = new BLMDAServiceItem().BL_GetMDAServiceItemDetails(new MDA_Service_Items() { intStatus = 2, MDAServiceItemID = MDAServiceItemID });

            if (mObjMDAServiceItemDetails != null)
            {
                dcResponse["success"] = true;
                dcResponse["MDAServiceItemDetails"] = mObjMDAServiceItemDetails;
            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Request";
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTaxPayerAssetList(int TaxPayerID, int AssetTypeID, int TaxPayerTypeID)
        {

            IList<DropDownListResult> lstTaxPayerAsset = new BLTaxPayerAsset().BL_GetTaxPayerAssetDropDownList(new MAP_TaxPayer_Asset() { AssetTypeID = AssetTypeID, TaxPayerID = TaxPayerID, TaxPayerTypeID = TaxPayerTypeID });
            return Json(lstTaxPayerAsset, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTaxPayerAssetList(int TaxPayerID, int TaxPayerTypeID, int ProfileID, int AssetTypeID)
        {
            IList<DropDownListResult> lstTaxPayerAsset = new BLTaxPayerAsset().BL_GetTaxPayerAssetDropDownList(new MAP_TaxPayer_Asset() { AssetTypeID = AssetTypeID, TaxPayerID = TaxPayerID, TaxPayerTypeID = TaxPayerTypeID, ProfileID = ProfileID });
            return Json(lstTaxPayerAsset, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProfileList(int TaxPayerID, int AssetID, int AssetTypeID, int TaxPayerTypeID)
        {

            IList<DropDownListResult> lstProfiles = new BLTaxPayerAsset().BL_GetTaxPayerProfileDropDownList(TaxPayerTypeID, TaxPayerID, AssetID, AssetTypeID);
            return Json(lstProfiles, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAssessmentRuleList(int TaxPayerID, int ProfileID, int AssetID, int AssetTypeID, int TaxPayerTypeID)
        {
            IList<DropDownListResult> lstAssessmentRule = new BLTaxPayerAsset().BL_GetTaxPayerAssessmentRuleDropDownList(TaxPayerTypeID, TaxPayerID, ProfileID, AssetID, AssetTypeID);
            return Json(lstAssessmentRule, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddAssessmentRule(Assessment_AssessmentRule pobjAssessmentRule)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();
            if (pobjAssessmentRule == null)
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Request";
            }
            else
            {
                IList<Assessment_AssessmentRule> lstAssessmentRules = SessionManager.lstAssessmentRule ?? new List<Assessment_AssessmentRule>();
                IList<Assessment_AssessmentItem> lstAssessmentItems = SessionManager.lstAssessmentItem ?? new List<Assessment_AssessmentItem>();
                if (lstAssessmentItems.Where(t => t.AssessmentRule_RowID == 0 && t.intTrack != EnumList.Track.DELETE && t.TaxAmount == 0 && (t.ComputationID == 2 || t.ComputationID == 3)).Count() > 0)
                {
                    dcResponse["success"] = false;
                    dcResponse["Message"] = "Invalid Tax Amount for Assessment Items";
                }
                else if (lstAssessmentRules.Where(t => t.AssetTypeID == pobjAssessmentRule.AssetTypeID && t.AssetID == pobjAssessmentRule.AssetID && t.ProfileID == pobjAssessmentRule.ProfileID && t.AssessmentRuleID == pobjAssessmentRule.AssessmentRuleID && t.intTrack != EnumList.Track.DELETE).Count() == 0)
                {
                    Assessment_AssessmentRule mObjAssessmentRule = new Assessment_AssessmentRule()
                    {
                        RowID = lstAssessmentRules.Count + 1,
                        AssetTypeID = pobjAssessmentRule.AssetTypeID,
                        AssetTypeName = pobjAssessmentRule.AssetTypeName,
                        AssetID = pobjAssessmentRule.AssetID,
                        AssetRIN = pobjAssessmentRule.AssetRIN,
                        ProfileID = pobjAssessmentRule.ProfileID,
                        ProfileDescription = pobjAssessmentRule.ProfileDescription,
                        AssessmentRuleID = pobjAssessmentRule.AssessmentRuleID,
                        AssessmentRuleName = pobjAssessmentRule.AssessmentRuleName,
                        AssessmentRuleAmount = lstAssessmentItems.Where(t => t.AssessmentRule_RowID == 0 && t.intTrack != EnumList.Track.DELETE).Sum(t => t.TaxAmount),
                        intTrack = EnumList.Track.INSERT,
                        TaxYear = pobjAssessmentRule.TaxYear,
                    };

                    lstAssessmentRules.Add(mObjAssessmentRule);

                    foreach (var item in lstAssessmentItems.Where(t => t.AssessmentRule_RowID == 0 && t.intTrack != EnumList.Track.DELETE))
                    {
                        item.AssessmentRule_RowID = mObjAssessmentRule.RowID;
                    }

                    SessionManager.lstAssessmentRule = lstAssessmentRules;
                    SessionManager.lstAssessmentItem = lstAssessmentItems;

                    dcResponse["success"] = true;
                    dcResponse["AssessmentRuleList"] = CommUtil.RenderPartialToString("_BindAssessmentRule", lstAssessmentRules.Where(t => t.intTrack != EnumList.Track.DELETE).ToList(), this.ControllerContext);
                    dcResponse["AssessmentRuleCount"] = lstAssessmentRules.Where(t => t.intTrack != EnumList.Track.DELETE).Count();
                    dcResponse["Message"] = "Assessment Rule Added";
                    dcResponse["AssessmentItemList"] = CommUtil.RenderPartialToString("_BindAssessmentItem", lstAssessmentItems.Where(t => t.AssessmentRule_RowID == 0).ToList(), this.ControllerContext);
                }
                else
                {
                    dcResponse["success"] = false;
                    dcResponse["Message"] = "Assessment Rule Already Exists";
                }
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RemoveAssessmentRule(int RowID)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            IList<Assessment_AssessmentRule> lstAssessmentRules = SessionManager.lstAssessmentRule ?? new List<Assessment_AssessmentRule>();
            IList<Assessment_AssessmentItem> lstAssessmentItems = SessionManager.lstAssessmentItem ?? new List<Assessment_AssessmentItem>();

            Assessment_AssessmentRule mObjAssessmentRule = lstAssessmentRules.Where(t => t.RowID == RowID).FirstOrDefault();

            if (mObjAssessmentRule != null)
            {
                mObjAssessmentRule.intTrack = EnumList.Track.DELETE;

                foreach (var item in lstAssessmentItems.Where(t => t.AssessmentRule_RowID == RowID))
                {
                    item.intTrack = EnumList.Track.DELETE;
                }

                SessionManager.lstAssessmentRule = lstAssessmentRules;
                SessionManager.lstAssessmentItem = lstAssessmentItems;

                dcResponse["success"] = true;
                dcResponse["AssessmentRuleList"] = CommUtil.RenderPartialToString("_BindAssessmentRule", lstAssessmentRules.Where(t => t.intTrack != EnumList.Track.DELETE).ToList(), this.ControllerContext);
                dcResponse["AssessmentRuleCount"] = lstAssessmentRules.Where(t => t.intTrack != EnumList.Track.DELETE).Count();
                dcResponse["Message"] = "Assessment Rule Removed";

            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Request";
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAssessmentRuleDetails(int AssessmentRuleID)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            usp_GetAssessmentRuleList_Result mObjAssessmentRuleData = new BLAssessmentRule().BL_GetAssessmentRuleDetails(new Assessment_Rules() { IntStatus = 2, AssessmentRuleID = AssessmentRuleID });

            if (mObjAssessmentRuleData != null)
            {
                dcResponse["success"] = true;
                dcResponse["AssessmentRuleDetails"] = mObjAssessmentRuleData;
            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Request";
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAssessmentRuleItemList(int AssessmentRuleID)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (AssessmentRuleID > 0)
            {
                usp_GetAssessmentRuleList_Result mObjAssessmentRuleDetails = new BLAssessmentRule().BL_GetAssessmentRuleDetails(new Assessment_Rules() { AssessmentRuleID = AssessmentRuleID, IntStatus = 2 });
                if (mObjAssessmentRuleDetails != null)
                {
                    dcResponse["success"] = true;
                    dcResponse["TaxYear"] = mObjAssessmentRuleDetails.TaxYear;

                    IList<Assessment_AssessmentItem> lstAssessmentItems = SessionManager.lstAssessmentItem ?? new List<Assessment_AssessmentItem>();

                    //Check if AssessmentRule_RowID = 0 Contains

                    var lstDeleteAssessmentItems = lstAssessmentItems.Where(t => t.AssessmentRule_RowID == 0 && t.intTrack != EnumList.Track.DELETE).ToList();
                    foreach (var item in lstDeleteAssessmentItems)
                    {
                        lstAssessmentItems.Remove(item);
                    }

                    string[] strArrAssessmentItemIds = mObjAssessmentRuleDetails.AssessmentItemIds.Split(',');

                    BLAssessmentItem mObjBLAssessmentItem = new BLAssessmentItem();

                    foreach (string strAssessmentItemID in strArrAssessmentItemIds)
                    {
                        if (TrynParse.parseInt(strAssessmentItemID) > 0)
                        {
                            usp_GetAssessmentItemList_Result mObjAssessmentItem = mObjBLAssessmentItem.BL_GetAssessmentItemDetails(new Assessment_Items() { intStatus = 2, AssessmentItemID = TrynParse.parseInt(strAssessmentItemID) });

                            Assessment_AssessmentItem mObjAssessmentRuleItem = new Assessment_AssessmentItem()
                            {
                                RowID = lstAssessmentItems.Count + 1
                            };

                            mObjAssessmentRuleItem.AssessmentRule_RowID = 0;
                            mObjAssessmentRuleItem.AssessmentItemID = mObjAssessmentItem.AssessmentItemID.GetValueOrDefault();
                            mObjAssessmentRuleItem.AssessmentItemReferenceNo = mObjAssessmentItem.AssessmentItemReferenceNo;
                            mObjAssessmentRuleItem.AssessmentItemName = mObjAssessmentItem.AssessmentItemName;
                            mObjAssessmentRuleItem.TaxAmount = mObjAssessmentItem.TaxAmount.GetValueOrDefault();
                            mObjAssessmentRuleItem.TaxBaseAmount = mObjAssessmentItem.TaxBaseAmount.GetValueOrDefault();
                            mObjAssessmentRuleItem.ComputationID = mObjAssessmentItem.ComputationID.GetValueOrDefault();
                            mObjAssessmentRuleItem.Percentage = mObjAssessmentItem.Percentage.GetValueOrDefault();

                            lstAssessmentItems.Add(mObjAssessmentRuleItem);
                        }
                    }

                    dcResponse["AssessmentItemList"] = CommUtil.RenderPartialToString("_BindAssessmentItem", lstAssessmentItems.Where(t => t.AssessmentRule_RowID == 0).ToList(), this.ControllerContext);

                    SessionManager.lstAssessmentItem = lstAssessmentItems;
                }
                else
                {
                    dcResponse["success"] = false;
                    dcResponse["Message"] = "Invalid Assessment Rule";
                }
            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Request";
            }



            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateAssessmentItem(int AssessmentItemRowID, decimal TaxBaseAmount)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            IList<Assessment_AssessmentItem> lstAssessmentItems = SessionManager.lstAssessmentItem ?? new List<Assessment_AssessmentItem>();

            Assessment_AssessmentItem mObjUpdateAssessmentItem = lstAssessmentItems.Where(t => t.RowID == AssessmentItemRowID).FirstOrDefault();

            if (mObjUpdateAssessmentItem != null)
            {
                mObjUpdateAssessmentItem.TaxBaseAmount = TaxBaseAmount;

                if (mObjUpdateAssessmentItem.ComputationID == 2)
                    mObjUpdateAssessmentItem.TaxAmount = TaxBaseAmount * (mObjUpdateAssessmentItem.Percentage / 100);
                else if (mObjUpdateAssessmentItem.ComputationID == 1 || mObjUpdateAssessmentItem.ComputationID == 3)
                    mObjUpdateAssessmentItem.TaxAmount = TaxBaseAmount;
            }


            dcResponse["AssessmentItemList"] = CommUtil.RenderPartialToString("_BindAssessmentItem", lstAssessmentItems.Where(t => t.AssessmentRule_RowID == 0 && t.intTrack != EnumList.Track.DELETE).ToList(), this.ControllerContext);

            SessionManager.lstAssessmentItem = lstAssessmentItems;

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }


        public JsonResult AddMDAService(ServiceBill_MDAService pobjMDAService)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();
            if (pobjMDAService == null)
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Request";
            }
            else
            {
                IList<ServiceBill_MDAService> lstServiceBillServices = SessionManager.lstServiceBillService ?? new List<ServiceBill_MDAService>();
                IList<ServiceBill_MDAServiceItem> lstServiceBillItems = SessionManager.lstServiceBillItem ?? new List<ServiceBill_MDAServiceItem>();
                if (lstServiceBillItems.Where(t => t.MDAService_RowID == 0 && t.intTrack != EnumList.Track.DELETE && t.ServiceAmount == 0 && (t.ComputationID == 2 || t.ComputationID == 3)).Count() > 0)
                {
                    dcResponse["success"] = false;
                    dcResponse["Message"] = "Invalid Service Amount for Service Bill Items";
                }
                else if (lstServiceBillServices.Where(t => t.MDAServiceID == pobjMDAService.MDAServiceID && t.intTrack != EnumList.Track.DELETE).Count() == 0)
                {
                    ServiceBill_MDAService mObjMDAService = new ServiceBill_MDAService()
                    {
                        RowID = lstServiceBillServices.Count + 1,
                        MDAServiceID = pobjMDAService.MDAServiceID,
                        MDAServiceName = pobjMDAService.MDAServiceName,
                        ServiceAmount = lstServiceBillItems.Where(t => t.MDAService_RowID == 0 && t.intTrack != EnumList.Track.DELETE).Sum(t => t.ServiceAmount),
                        intTrack = EnumList.Track.INSERT,
                        TaxYear = pobjMDAService.TaxYear,
                    };

                    lstServiceBillServices.Add(mObjMDAService);

                    foreach (var item in lstServiceBillItems.Where(t => t.MDAService_RowID == 0 && t.intTrack != EnumList.Track.DELETE))
                    {
                        item.MDAService_RowID = mObjMDAService.RowID;
                    }

                    SessionManager.lstServiceBillService = lstServiceBillServices;
                    SessionManager.lstServiceBillItem = lstServiceBillItems;

                    dcResponse["success"] = true;
                    dcResponse["MDAServiceList"] = CommUtil.RenderPartialToString("_BindServiceBillServices", lstServiceBillServices.Where(t => t.intTrack != EnumList.Track.DELETE).ToList(), this.ControllerContext);
                    dcResponse["MDAServiceCount"] = lstServiceBillServices.Where(t => t.intTrack != EnumList.Track.DELETE).Count();
                    dcResponse["Message"] = "MDA Service Added";
                    dcResponse["ServiceBillItemList"] = CommUtil.RenderPartialToString("_BindServiceBillItem", lstServiceBillItems.Where(t => t.MDAService_RowID == 0).ToList(), this.ControllerContext);
                }
                else
                {
                    dcResponse["success"] = false;
                    dcResponse["Message"] = "MDA Service Already Exists";
                }
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RemoveMDAService(int RowID)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            IList<ServiceBill_MDAService> lstServiceBillServices = SessionManager.lstServiceBillService ?? new List<ServiceBill_MDAService>();
            IList<ServiceBill_MDAServiceItem> lstServiceBillItems = SessionManager.lstServiceBillItem ?? new List<ServiceBill_MDAServiceItem>();

            ServiceBill_MDAService mObjAssessmentRule = lstServiceBillServices.Where(t => t.RowID == RowID).FirstOrDefault();

            if (mObjAssessmentRule != null)
            {
                mObjAssessmentRule.intTrack = EnumList.Track.DELETE;

                foreach (var item in lstServiceBillItems.Where(t => t.MDAService_RowID == RowID))
                {
                    item.intTrack = EnumList.Track.DELETE;
                }

                SessionManager.lstServiceBillService = lstServiceBillServices;
                SessionManager.lstServiceBillItem = lstServiceBillItems;

                dcResponse["success"] = true;
                dcResponse["MDAServiceList"] = CommUtil.RenderPartialToString("_BindServiceBillServices", lstServiceBillServices.Where(t => t.intTrack != EnumList.Track.DELETE).ToList(), this.ControllerContext);
                dcResponse["MDAServiceCount"] = lstServiceBillServices.Where(t => t.intTrack != EnumList.Track.DELETE).Count();
                dcResponse["Message"] = "Assessment Rule Removed";

            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Request";
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMDAServiceDetails(int MDAServiceID)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            usp_GetMDAServiceList_Result mObjMDAServiceData = new BLMDAService().BL_GetMDAServiceDetails(new MDA_Services() { IntStatus = 2, MDAServiceID = MDAServiceID });

            if (mObjMDAServiceData != null)
            {
                dcResponse["success"] = true;
                dcResponse["MDAServiceDetails"] = mObjMDAServiceData;
            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Request";
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMDAServiceItemList(int MDAServiceID)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (MDAServiceID > 0)
            {
                usp_GetMDAServiceList_Result mObjMDAServiceData = new BLMDAService().BL_GetMDAServiceDetails(new MDA_Services() { IntStatus = 2, MDAServiceID = MDAServiceID });
                if (mObjMDAServiceData != null)
                {
                    dcResponse["success"] = true;
                    dcResponse["TaxYear"] = mObjMDAServiceData.TaxYear;

                    IList<ServiceBill_MDAServiceItem> lstMDAServiceItems = SessionManager.lstServiceBillItem ?? new List<ServiceBill_MDAServiceItem>();

                    //Check if MDAService_RowID = 0 Contains

                    var lstDeleteMDAServiceItems = lstMDAServiceItems.Where(t => t.MDAService_RowID == 0 && t.intTrack != EnumList.Track.DELETE).ToList();
                    foreach (var item in lstDeleteMDAServiceItems)
                    {
                        lstMDAServiceItems.Remove(item);
                    }

                    string[] strArrMDAServiceItemIds = mObjMDAServiceData.MDAServiceItemIds.Split(',');

                    BLMDAServiceItem mObjBLMDAServiceItem = new BLMDAServiceItem();

                    foreach (string strMDAServiceItemID in strArrMDAServiceItemIds)
                    {
                        if (TrynParse.parseInt(strMDAServiceItemID) > 0)
                        {
                            usp_GetMDAServiceItemList_Result mObjMDAServiceItem = mObjBLMDAServiceItem.BL_GetMDAServiceItemDetails(new MDA_Service_Items() { intStatus = 2, MDAServiceItemID = TrynParse.parseInt(strMDAServiceItemID) });

                            ServiceBill_MDAServiceItem mObjServiceBillItem = new ServiceBill_MDAServiceItem()
                            {
                                RowID = lstMDAServiceItems.Count + 1
                            };

                            mObjServiceBillItem.MDAService_RowID = 0;
                            mObjServiceBillItem.MDAServiceItemID = mObjMDAServiceItem.MDAServiceItemID.GetValueOrDefault();
                            mObjServiceBillItem.MDAServiceItemReferenceNo = mObjMDAServiceItem.MDAServiceItemReferenceNo;
                            mObjServiceBillItem.MDAServiceItemName = mObjMDAServiceItem.MDAServiceItemName;
                            mObjServiceBillItem.ServiceAmount = mObjMDAServiceItem.ServiceAmount.GetValueOrDefault();
                            mObjServiceBillItem.ServiceBaseAmount = mObjMDAServiceItem.ServiceBaseAmount.GetValueOrDefault();
                            mObjServiceBillItem.ComputationID = mObjMDAServiceItem.ComputationID.GetValueOrDefault();
                            mObjServiceBillItem.Percentage = mObjMDAServiceItem.Percentage.GetValueOrDefault();

                            lstMDAServiceItems.Add(mObjServiceBillItem);
                        }
                    }

                    dcResponse["ServiceBillItemList"] = CommUtil.RenderPartialToString("_BindServiceBillItem", lstMDAServiceItems.Where(t => t.MDAService_RowID == 0).ToList(), this.ControllerContext);

                    SessionManager.lstServiceBillItem = lstMDAServiceItems;
                }
                else
                {
                    dcResponse["success"] = false;
                    dcResponse["Message"] = "Invalid Assessment Rule";
                }
            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Request";
            }



            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }



        public JsonResult UpdateMDAServiceItem(int ServiceBillItemRowID, decimal ServiceBaseAmount)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            IList<ServiceBill_MDAServiceItem> lstServiceBillItems = SessionManager.lstServiceBillItem ?? new List<ServiceBill_MDAServiceItem>();

            ServiceBill_MDAServiceItem mObjUpdateServiceBillItem = lstServiceBillItems.Where(t => t.RowID == ServiceBillItemRowID).FirstOrDefault();

            if (mObjUpdateServiceBillItem != null)
            {
                mObjUpdateServiceBillItem.ServiceBaseAmount = ServiceBaseAmount;

                if (mObjUpdateServiceBillItem.ComputationID == 2)
                    mObjUpdateServiceBillItem.ServiceAmount = ServiceBaseAmount * (mObjUpdateServiceBillItem.Percentage / 100);
                else if (mObjUpdateServiceBillItem.ComputationID == 1)
                    mObjUpdateServiceBillItem.ServiceAmount = ServiceBaseAmount;
            }


            dcResponse["ServiceBillItemList"] = CommUtil.RenderPartialToString("_BindServiceBillItem", lstServiceBillItems.Where(t => t.MDAService_RowID == 0 && t.intTrack != EnumList.Track.DELETE).ToList(), this.ControllerContext);

            SessionManager.lstServiceBillItem = lstServiceBillItems;

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetVehicleInsurance(int VehicleID)
        {
            IList<DropDownListResult> lstVehicleInsurance = new BLVehicleInsurance().BL_GetVehicleInsuranceDropDownList(new Vehicle_Insurance() { IntStatus = 1, VehicleID = VehicleID });
            return Json(lstVehicleInsurance, JsonRequestBehavior.AllowGet);
        }
    }
}