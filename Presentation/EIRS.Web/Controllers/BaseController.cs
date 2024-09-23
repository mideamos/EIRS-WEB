using DocumentFormat.OpenXml.Office2010.Excel;
using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;
using EIRS.Web.Models;
using EIRS.Web.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Reflection;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Vereyon.Web;
using static EIRS.Web.Controllers.Filters;
using Zone = EIRS.BOL.Zone;

namespace EIRS.Web.Controllers
{
    [SessionTimeout]
    public class BaseController : Controller
    {
        EIRSEntities _db = new EIRSEntities();
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

        public void UI_FillTaxOfficeDropDown(Tax_Offices pObjTaxOffice = null, bool pblnAddAll = false)
        {
            if (pObjTaxOffice == null)
                pObjTaxOffice = new Tax_Offices();

            pObjTaxOffice.intStatus = 1;

            IList<DropDownListResult> lstTaxOffice = new BLTaxOffice().BL_GetTaxOfficeDropDownList(pObjTaxOffice);

            if (pblnAddAll)
            {
                lstTaxOffice.Add(new DropDownListResult() { id = 0, text = "All Tax Offices" });
                ViewBag.TaxOfficeList = new SelectList(lstTaxOffice, "id", "text", 0);
            }
            else
            {
                ViewBag.TaxOfficeList = new SelectList(lstTaxOffice, "id", "text");
            }

        }
        public void UI_FillTaxOfficeDropDownForStatic(Tax_Offices pObjTaxOffice = null, bool pblnAddAll = false, int userTaxOffice = 0, int loginTaxOffice = 0)
        {
            if (pObjTaxOffice == null)
                pObjTaxOffice = new Tax_Offices();

            pObjTaxOffice.intStatus = 1;

            IList<DropDownListResult> lstTaxOffice = new BLTaxOffice().BL_GetTaxOfficeDropDownList(pObjTaxOffice);
            if (userTaxOffice != 0)
            {
                lstTaxOffice = lstTaxOffice.Where(o => o.id == userTaxOffice).ToList();
                ViewBag.UserTaxOffice = new SelectList(lstTaxOffice, "id", "text");
            }
            else
            {
                lstTaxOffice = lstTaxOffice.Where(o => o.id == loginTaxOffice).ToList();
                ViewBag.LoginTaxOffice = new SelectList(lstTaxOffice, "id", "text");
            }
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

        public void UI_FillDirectorateDropDown(Directorate pObjDirectorate = null)
        {
            if (pObjDirectorate == null)
                pObjDirectorate = new Directorate();

            pObjDirectorate.intStatus = 1;

            IList<DropDownListResult> lstDirectorate = new BLDirectorate().BL_GetDirectorateDropDownList(pObjDirectorate);
            ViewBag.DirectorateList = new SelectList(lstDirectorate, "id", "text");
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

        public void UI_FillSettlementStatusDropDown(Settlement_Status pObjSettlementStatus = null)
        {
            if (pObjSettlementStatus == null)
                pObjSettlementStatus = new Settlement_Status();

            pObjSettlementStatus.intStatus = 1;

            IList<DropDownListResult> lstSettlementStatus = new BLSettlementStatus().BL_GetSettlementStatusDropDownList(pObjSettlementStatus);
            ViewBag.SettlementStatusList = new SelectList(lstSettlementStatus, "id", "text");
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
            int mIntCurrentYear = 2019;//CommUtil.GetCurrentDateTime().AddYears(-1).Year;
            for (int i = mIntCurrentYear; i <= DateTime.Now.AddYears(1).Year; i++)
            {
                lstYear.Add(new DropDownListResult() { id = i, text = i.ToString() });
            }

            ViewBag.YearList = new SelectList(lstYear, "id", "text");

        }
        public void UI_FillTCCStatusDropDown()
        {
            IList<DropDownListResult> lstYear = new List<DropDownListResult>()
        {
            new DropDownListResult { id = 1, text = "Pending TCC" },
            new DropDownListResult { id = 2, text = "Downloaded" },
            new DropDownListResult { id = 3, text = "Issued" }
        };
            ViewBag.TCCStatusList = new SelectList(lstYear, "id", "text");

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

        public void UI_FillNotificationTypeDropDown(Notification_Type pObjNotificationType = null)
        {
            if (pObjNotificationType == null)
                pObjNotificationType = new Notification_Type();

            pObjNotificationType.intStatus = 1;

            IList<DropDownListResult> lstNotificationMethod = new BLNotificationType().BL_GetNotificationTypeDropDownList(pObjNotificationType);
            ViewBag.NotificationTypeList = new SelectList(lstNotificationMethod, "id", "text");
        }

        public void UI_FillReviewStatus()
        {
            IList<DropDownListResult> lstReviewStatus = new BLCommon().BL_GetReviewStatusDropDownList();
            ViewBag.ReviewStatusList = new SelectList(lstReviewStatus, "id", "text");
        }

        public void UI_FillProfileDropDown(Profile pObjProfile = null)
        {
            if (pObjProfile == null)
                pObjProfile = new Profile();

            pObjProfile.IntStatus = 1;

            IList<DropDownListResult> lstProfile = new BLProfile().BL_GetProfileDropDownList(pObjProfile);
            ViewBag.ProfileList = new SelectList(lstProfile, "id", "text");
        }

        public void UI_FillProfileTypeDropDown()
        {
            IList<DropDownListResult> lstProfileType = new BLCommon().BL_GetProfileTypeDropDownList();
            ViewBag.ProfileTypeList = new SelectList(lstProfileType, "id", "text");
        }

        public void UI_FillFieldTypeDropDown()
        {
            IList<DropDownListResult> lstFieldType = new BLCommon().BL_GetFieldTypeDropDownList();
            ViewBag.FieldTypeList = new SelectList(lstFieldType, "id", "text");
        }

        public void UI_FillCertificateTypeDropDown(Certificate_Types pObjCertificateType = null)
        {
            if (pObjCertificateType == null)
                pObjCertificateType = new Certificate_Types();

            pObjCertificateType.IntStatus = 1;

            IList<DropDownListResult> lstCertificateType = new BLCertificateType().BL_GetCertificateTypeDropDown(pObjCertificateType);
            ViewBag.CertificateTypeList = new SelectList(lstCertificateType, "id", "text");
        }

        public void UI_FillTaxPayerProfileDropDown(int TaxPayerTypeID, int TaxPayerID)
        {
            IList<DropDownListResult> lstProfile = new BLTaxPayerAsset().BL_GetTaxPayerProfileDropDown(TaxPayerID, TaxPayerTypeID);
            ViewBag.ProfileList = new SelectList(lstProfile, "id", "text");
        }

        public void UI_FillPDFTemplateDropDown(int pIntOrganizationID, string pStrIncludePDFTemplate = null)
        {
            IDictionary<string, object> dcParameter = new Dictionary<string, object>
            {
                ["OrganizationID"] = pIntOrganizationID,
                ["ProductID"] = 0
            };

            if (pStrIncludePDFTemplate != null)
            {
                dcParameter["includePDFTemplate"] = pStrIncludePDFTemplate;
            }

            IDictionary<string, object> dcAPIResponse = APICall.GetData(GlobalDefaultValues.SEDE_API_PDFTemplateUrl, dcParameter);

            if (TrynParse.parseBool(dcAPIResponse["success"]))
            {
                IList<DropDownListResult> lstPDFTemplate = JsonConvert.DeserializeObject<IList<DropDownListResult>>(TrynParse.parseString(dcAPIResponse["result"]));
                ViewBag.PDFTemplateList = new SelectList(lstPDFTemplate, "id", "text");
            }
        }

        public void UI_FillTCCStatus()
        {
            IList<DropDownListResult> lstTCCStatus = new BLCommon().BL_GetTCCStatusDropDownList();
            ViewBag.TCCStatusList = new SelectList(lstTCCStatus, "id", "text");
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
        //public JsonResult GetZone(int? zoneId)
        //{
        //    List<Zone> zones = new List<Zone>();
        //    IList<DropDownListResult> lstZone = new List<DropDownListResult>();
        //    if  (zoneId == 0)
        //    {
        //        zones = _db.Zones.ToList();
        //    }
        //    else
        //    {
        //        zones = _db.Zones.Where(o=>o.ZoneId == Convert.ToInt32(zoneId)).ToList();
        //    }

        //    foreach(var item in zones)
        //    {
        //        lstZone.Add(new DropDownListResult() { id = item.ZoneId, text = item.ZoneName.ToString() });
        //    }
        //    ViewBag.ZoneList = lstZone;
        //    return Json(lstZone, JsonRequestBehavior.AllowGet);
        //}

        public void UI_FillZoneDropDown(int? zoneId)
        {
            List<Zone> zones = new List<Zone>();
            IList<DropDownListResult> lstZone = new List<DropDownListResult>();
            if (zoneId == 0)
            {
                zones = _db.Zones.ToList();
            }
            else
            {
                zones = _db.Zones.Where(o => o.ZoneId == Convert.ToInt32(zoneId)).ToList();
            }
            foreach (var item in zones)
            {
                lstZone.Add(new DropDownListResult() { id = item.ZoneId, text = item.ZoneName.ToString() });
            }
            ViewBag.ZoneList = new SelectList(lstZone, "id", "text");
            // ViewBag.LGAList = new SelectList(lstLGA, "id", "text");
        }
        public void UI_FillTaxOfficeDropDown(int? zoneId)
        {
            List<Tax_Offices> zones = new List<Tax_Offices>();
            IList<DropDownListResult> lstZone = new List<DropDownListResult>();
            if (zoneId == 0)
            {
                zones = _db.Tax_Offices.ToList();
            }
            else
            {
                zones = _db.Tax_Offices.Where(o => o.TaxOfficeID == Convert.ToInt32(zoneId)).ToList();
            }
            foreach (var item in zones)
            {
                lstZone.Add(new DropDownListResult() { id = item.TaxOfficeID, text = item.TaxOfficeName.ToString() });
            }
            ViewBag.TaxOfficeList = new SelectList(lstZone, "id", "text");
            // ViewBag.LGAList = new SelectList(lstLGA, "id", "text");
        }

        //public void UI_FillLGADropDown(LGA pObjLGA = null)
        //{
        //    if (pObjLGA == null)
        //        pObjLGA = new LGA();

        //    pObjLGA.intStatus = 1;

        //    IList<DropDownListResult> lstLGA = new BLLGA().BL_GetLGADropDownList(pObjLGA);
        //    ViewBag.LGAList = new SelectList(lstLGA, "id", "text");
        //}
        //public JsonResult GetTaxofficee(int? zoneId)
        //{
        //    List<Tax_Offices> zones = new List<Tax_Offices>();

        //    IList<DropDownListResult> lstZone = new List<DropDownListResult>();
        //    if (zoneId == 0)
        //    {
        //        zones = _db.Tax_Offices.ToList();
        //    }
        //    else
        //    {
        //        zones = _db.Tax_Offices.Where(o => o.TaxOfficeID == Convert.ToInt32(zoneId)).ToList();
        //    }

        //    foreach (var item in zones)
        //    {
        //        lstZone.Add(new DropDownListResult() { id = item.TaxOfficeID, text = item.TaxOfficeName.ToString() });
        //    }
        //    //TaxOfficeId

        //    ViewBag.TaxOfficeList = lstZone;
        //    return Json(lstZone, JsonRequestBehavior.AllowGet);
        //}

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

        public class TaxPayerAsset
        {
            public int TaxPayerTypeID { get; set; }
            public int TaxPayerID { get; set; }
            public int AssetTypeID { get; set; }
            public int TaxPayerRoleID { get; set; }
            public int AssetID { get; set; }
            public int? BuildingUnitID { get; set; }
        }

        public bool LinkTaxPayerToAsset(TaxPayerAsset tpa)
        {
            if (!ModelState.IsValid)
            {
                return false;
            }
            MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
            {
                AssetTypeID = tpa.AssetTypeID,
                AssetID = tpa.AssetID,
                TaxPayerTypeID = tpa.TaxPayerTypeID,
                TaxPayerRoleID = tpa.TaxPayerRoleID,
                TaxPayerID = tpa.TaxPayerID,
                BuildingUnitID = tpa.BuildingUnitID,
                Active = true,
                CreatedBy = SessionManager.UserID,
                CreatedDate = CommUtil.GetCurrentDateTime()
            };

            FuncResponse mObjTPResponse = new BLTaxPayerAsset().BL_InsertTaxPayerAsset(mObjTaxPayerAsset);
            if (mObjTPResponse.Success)
            {
                var vExists = (from k in _db.MAP_TaxPayer_Asset
                               join aa in _db.Asset_Types on k.AssetTypeID equals aa.AssetTypeID
                               join tp in _db.TaxPayer_Roles on k.TaxPayerRoleID equals tp.TaxPayerRoleID
                               join tpx in _db.TaxPayer_Types on k.TaxPayerTypeID equals tpx.TaxPayerTypeID
                               join idd in _db.Individuals on k.TaxPayerID equals idd.IndividualID
                               where k.AssetTypeID == mObjTaxPayerAsset.AssetTypeID && k.AssetID == mObjTaxPayerAsset.AssetID
                                  && k.TaxPayerTypeID == mObjTaxPayerAsset.TaxPayerTypeID && k.TaxPayerID == mObjTaxPayerAsset.TaxPayerID
                                  && k.TaxPayerRoleID == mObjTaxPayerAsset.TaxPayerRoleID && k.Active == true

                               select new { Post = k, Meta = tp, All = tpx, Idd = idd, Aa = aa }).FirstOrDefault();

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
                FlashMessage.Info("Linked to Tax Payer");
                return true;
            }
            else
            {
                // var msg = mObjTPResponse.Exception.Message;
                FlashMessage.Info("Selected asset is already mapped to this TaxPayer");
                return false;
            }
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

        public JsonResult BusinessTypeChange(int BusinessTypeID)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            IList<DropDownListResult> lstBusinessCategory = new BLBusinessCategory().BL_GetBusinessCategoryDropDownList(new Business_Category() { intStatus = 1, BusinessTypeID = BusinessTypeID });
            dcResponse["BusinessCategory"] = lstBusinessCategory;

            IList<DropDownListResult> lstBusinessStructure = new BLBusinessStructure().BL_GetBusinessStructureDropDownList(new Business_Structure() { intStatus = 1, BusinessTypeID = BusinessTypeID });
            dcResponse["BusinessStructure"] = lstBusinessStructure;

            IList<DropDownListResult> lstBusinessOperation = new BLBusinessOperation().BL_GetBusinessOperationDropDownList(new Business_Operation() { intStatus = 1, BusinessTypeID = BusinessTypeID });
            dcResponse["BusinessOperation"] = lstBusinessOperation;

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBuildingUnitList(int BuildingID)
        {
            MAP_Building_BuildingUnit mObjBuildingUnit = new MAP_Building_BuildingUnit()
            {
                BuildingID = BuildingID
            };

            IList<usp_GetBuildingUnitNumberList_Result> lstBuildingUnitNumberList = new BLBuilding().BL_GetBuildingUnitNumberList(mObjBuildingUnit);
            return Json(lstBuildingUnitNumberList, JsonRequestBehavior.AllowGet);
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

        public JsonResult GetProfileDetails(int ProfileID)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            usp_GetProfileList_Result mObjProfileDetails = new BLProfile().BL_GetProfileDetails(new Profile() { IntStatus = 2, ProfileID = ProfileID });

            if (mObjProfileDetails != null)
            {
                dcResponse["success"] = true;
                dcResponse["ProfileDetails"] = mObjProfileDetails;
            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Request";
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetIndividualDetails(int IndividualID)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(new Individual() { intStatus = 1, IndividualID = IndividualID });

            if (mObjIndividualData != null)
            {
                dcResponse["success"] = true;
                dcResponse["TaxPayerDetails"] = mObjIndividualData;
            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "No Record Found";
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCompanyDetails(int CompanyID)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(new Company() { intStatus = 1, CompanyID = CompanyID });

            if (mObjCompanyData != null)
            {
                dcResponse["success"] = true;
                dcResponse["TaxPayerDetails"] = mObjCompanyData;
            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "No Record Found";
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGovernmentDetails(int GovernmentID)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            usp_GetGovernmentList_Result mObjGovernmentData = new BLGovernment().BL_GetGovernmentDetails(new Government() { intStatus = 1, GovernmentID = GovernmentID });

            if (mObjGovernmentData != null)
            {
                dcResponse["success"] = true;
                dcResponse["TaxPayerDetails"] = mObjGovernmentData;
            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "No Record Found";
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSpecialDetails(int SpecialID)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            usp_GetSpecialList_Result mObjSpecialData = new BLSpecial().BL_GetSpecialDetails(new Special() { intStatus = 1, SpecialID = SpecialID });

            if (mObjSpecialData != null)
            {
                dcResponse["success"] = true;
                dcResponse["TaxPayerDetails"] = mObjSpecialData;
            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "No Record Found";
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBuildingDetails(int BuildingID)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            usp_GetBuildingList_Result mObjBuildingData = new BLBuilding().BL_GetBuildingDetails(new Building() { intStatus = 1, BuildingID = BuildingID });

            if (mObjBuildingData != null)
            {
                dcResponse["success"] = true;
                dcResponse["AssetDetails"] = mObjBuildingData;
            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "No Record Found";
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBusinessDetails(int BusinessID)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            usp_GetBusinessListNewTy_Result mObjBusinessData = new BLBusiness().BL_GetBusinessDetails(new Business() { intStatus = 1, BusinessID = BusinessID });

            if (mObjBusinessData != null)
            {
                dcResponse["success"] = true;
                dcResponse["AssetDetails"] = mObjBusinessData;
            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "No Record Found";
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLandDetails(int LandID)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            usp_GetLandList_Result mObjLandData = new BLLand().BL_GetLandDetails(new Land() { intStatus = 1, LandID = LandID });

            if (mObjLandData != null)
            {
                dcResponse["success"] = true;
                dcResponse["AssetDetails"] = mObjLandData;
            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "No Record Found";
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetVehicleDetails(int VehicleID)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            usp_GetVehicleList_Result mObjVehicleData = new BLVehicle().BL_GetVehicleDetails(new Vehicle() { intStatus = 1, VehicleID = VehicleID });

            if (mObjVehicleData != null)
            {
                dcResponse["success"] = true;
                dcResponse["AssetDetails"] = mObjVehicleData;
            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "No Record Found";
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetNotificationDetails(int NotificationID)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            usp_GetNotificationList_Result mObjNotificationData = new BLNotification().BL_GetNotificationDetails(new Notification() { NotificationID = NotificationID, IntStatus = 2 });

            if (mObjNotificationData != null)
            {
                dcResponse["NotificationContent"] = mObjNotificationData.NotificationContent;
                dcResponse["success"] = true;

            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Response";
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddBuildingUnit(Building_BuildingUnit pObjBuildingUnit)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();
            if (pObjBuildingUnit == null)
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Request";
            }
            else
            {
                IList<Building_BuildingUnit> lstBuildingUnit = SessionManager.LstBuildingUnit ?? new List<Building_BuildingUnit>();

                pObjBuildingUnit.RowID = lstBuildingUnit.Count + 1;
                pObjBuildingUnit.intTrack = EnumList.Track.INSERT;

                lstBuildingUnit.Add(pObjBuildingUnit);


                SessionManager.LstBuildingUnit = lstBuildingUnit;

                dcResponse["success"] = true;
                dcResponse["BuildingUnitList"] = CommUtil.RenderPartialToString("_BindBuildingUnitTable_SingleSelect", lstBuildingUnit.Where(t => t.intTrack != EnumList.Track.DELETE).ToList(), this.ControllerContext);
                dcResponse["Message"] = "Building Unit Added";

            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddUnitInformation(Building_BuildingUnit pObjBuildingUnit)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();
            if (pObjBuildingUnit == null)
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Request";
            }
            else
            {
                IList<Building_BuildingUnit> lstBuildingUnit = SessionManager.LstBuildingUnit ?? new List<Building_BuildingUnit>();

                pObjBuildingUnit.RowID = lstBuildingUnit.Count + 1;
                pObjBuildingUnit.intTrack = EnumList.Track.INSERT;

                lstBuildingUnit.Add(pObjBuildingUnit);


                SessionManager.LstBuildingUnit = lstBuildingUnit;

                dcResponse["success"] = true;
                dcResponse["BuildingUnitList"] = CommUtil.RenderPartialToString("_BindBuildingUnitTable", lstBuildingUnit.Where(t => t.intTrack != EnumList.Track.DELETE).ToList(), this.ControllerContext);
                dcResponse["Message"] = "Building Unit Added";

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
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            IList<MAP_MDAService_MDAServiceItem> lstMDAServiceItem = new BLMDAService().BL_GetMDAServiceItem(MDAServiceID);
            if (lstMDAServiceItem != null && lstMDAServiceItem.Count > 0)
            {
                IList<usp_GetMDAServiceItemList_Result> lstMDAServiceItems = new List<usp_GetMDAServiceItemList_Result>();

                BLMDAServiceItem mObjBLMDAServiceItem = new BLMDAServiceItem();

                foreach (var vMDAServiceItem in lstMDAServiceItem)
                {
                    if (vMDAServiceItem.MDAServiceItemID.GetValueOrDefault() > 0)
                    {
                        usp_GetMDAServiceItemList_Result mObjMDAServiceItem = mObjBLMDAServiceItem.BL_GetMDAServiceItemDetails(new MDA_Service_Items() { intStatus = 2, MDAServiceItemID = vMDAServiceItem.MDAServiceItemID.GetValueOrDefault() });
                        lstMDAServiceItems.Add(mObjMDAServiceItem);
                    }
                }

                dcResponse["success"] = true;
                dcResponse["MDAServiceItemList"] = CommUtil.RenderPartialToString("_BindMDAServiceItem", lstMDAServiceItems, this.ControllerContext);
            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Request";
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAssessmentDetails(int AssessmentID)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            usp_GetAssessmentList_Result mObjAssessmentData = new BLAssessment().BL_GetAssessmentDetails(new Assessment() { IntStatus = 2, AssessmentID = AssessmentID });

            if (mObjAssessmentData != null)
            {
                dcResponse["success"] = true;
                dcResponse["AssessmentDetails"] = mObjAssessmentData;
            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Request";
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetServiceBillDetails(int ServiceBillID)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            usp_GetServiceBillList_Result mObjServiceBillData = new BLServiceBill().BL_GetServiceBillDetails(new ServiceBill() { IntStatus = 2, ServiceBillID = ServiceBillID });

            if (mObjServiceBillData != null)
            {
                dcResponse["success"] = true;
                dcResponse["ServiceBillDetails"] = mObjServiceBillData;
            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Request";
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBillPaymentList(int BillID, int BillTypeID)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            Settlement mObjSettlement;

            if (BillTypeID == 1)
            {
                mObjSettlement = new Settlement()
                {
                    ServiceBillID = -1,
                    AssessmentID = BillID
                };
            }
            else if (BillTypeID == 2)
            {
                mObjSettlement = new Settlement()
                {
                    ServiceBillID = BillID,
                    AssessmentID = -1
                };
            }
            else
            {
                mObjSettlement = new Settlement();
            }

            IList<usp_GetSettlementList_Result> lstSettlement = new BLSettlement().BL_GetSettlementList(mObjSettlement);

            dcResponse["success"] = true;
            dcResponse["BillPaymentList"] = CommUtil.RenderPartialToString("_BindBillPaymentTable", lstSettlement, this.ControllerContext);


            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateMDAServiceItem(string rowdata)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            IList<ServiceBill_MDAServiceItem> lstMDAServiceItems = SessionManager.lstMDAServiceItem ?? new List<ServiceBill_MDAServiceItem>();
            IList<ServiceBill_MDAService> lstMDAServices = SessionManager.lstMDAService ?? new List<ServiceBill_MDAService>();

            if (!string.IsNullOrWhiteSpace(rowdata))
            {
                string[] strRowData = rowdata.Split(new string[] { "~~" }, StringSplitOptions.RemoveEmptyEntries);

                if (strRowData.Length > 0)
                {
                    foreach (var vRowData in strRowData)
                    {
                        string[] strServiceItemData = vRowData.Split(new string[] { "@@" }, StringSplitOptions.RemoveEmptyEntries);

                        if (strServiceItemData.Length == 2)
                        {
                            ServiceBill_MDAServiceItem mObjUpdateMDAServiceItem = lstMDAServiceItems.Where(t => t.RowID == TrynParse.parseInt(strServiceItemData[0])).FirstOrDefault();


                            if (mObjUpdateMDAServiceItem != null)
                            {
                                decimal ServiceBaseAmount = TrynParse.parseDecimal(strServiceItemData[1]);

                                mObjUpdateMDAServiceItem.ServiceBaseAmount = ServiceBaseAmount;

                                if (mObjUpdateMDAServiceItem.ComputationID == 2)
                                {
                                    mObjUpdateMDAServiceItem.ServiceAmount = ServiceBaseAmount * (mObjUpdateMDAServiceItem.Percentage / 100);
                                    mObjUpdateMDAServiceItem.ServiceBaseAmount = ServiceBaseAmount;
                                }
                                else if (mObjUpdateMDAServiceItem.ComputationID == 1 || mObjUpdateMDAServiceItem.ComputationID == 3)
                                    mObjUpdateMDAServiceItem.ServiceAmount = ServiceBaseAmount;

                                mObjUpdateMDAServiceItem.intTrack = mObjUpdateMDAServiceItem.TablePKID > 0 ? EnumList.Track.UPDATE : EnumList.Track.INSERT;


                                ServiceBill_MDAService mObjUpdateMDAService = lstMDAServices.Where(t => t.RowID == mObjUpdateMDAServiceItem.MDAService_RowID).FirstOrDefault();

                                mObjUpdateMDAService.ServiceAmount = lstMDAServiceItems.Where(t => t.MDAService_RowID == mObjUpdateMDAService.RowID).Sum(t => t.ServiceAmount);
                                mObjUpdateMDAService.ToSettleAmount = mObjUpdateMDAServiceItem.ServiceAmount;
                                mObjUpdateMDAService.intTrack = mObjUpdateMDAService.TablePKID > 0 ? EnumList.Track.UPDATE : EnumList.Track.INSERT;
                            }
                        }
                    }

                    ViewBag.MDAServiceList = lstMDAServices.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();

                    dcResponse["success"] = true;
                    dcResponse["MDAServiceDetails"] = CommUtil.RenderPartialToString("_BindMDAServiceForSerivceBill", null, this.ControllerContext, ViewData);

                    SessionManager.lstMDAService = lstMDAServices;
                    SessionManager.lstMDAServiceItem = lstMDAServiceItems;

                }
                else
                {
                    ViewBag.MDAServiceList = SessionManager.lstMDAService.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();

                    dcResponse["success"] = true;
                    dcResponse["MDAServiceDetails"] = CommUtil.RenderPartialToString("_BindMDAServiceForSerivceBill", null, this.ControllerContext, ViewData);
                }
            }
            else
            {
                ViewBag.MDAServiceList = SessionManager.lstMDAService.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();

                dcResponse["success"] = true;
                dcResponse["MDAServiceDetails"] = CommUtil.RenderPartialToString("_BindMDAServiceForSerivceBill", null, this.ControllerContext, ViewData);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateAssessmentItem(string rowdata)
        {
            int holder = SessionManager.DataSubmitterID;
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            IList<Assessment_AssessmentItem> lstMDAServiceItems = SessionManager.lstAssessmentItem ?? new List<Assessment_AssessmentItem>();
            IList<Assessment_AssessmentRule> lstMDAServices = SessionManager.lstAssessmentRule ?? new List<Assessment_AssessmentRule>();
            Assessment_AssessmentRule mObjUpdateMDAService = new Assessment_AssessmentRule();
            Assessment_AssessmentItem mObjUpdateMDAServiceItem = new Assessment_AssessmentItem();
            if (!string.IsNullOrWhiteSpace(rowdata))
            {
                string[] strRowData = rowdata.Split(new string[] { "~~" }, StringSplitOptions.RemoveEmptyEntries);

                if (strRowData.Length > 0)
                {
                    foreach (var vRowData in strRowData)
                    {
                        string[] strServiceItemData = vRowData.Split(new string[] { "@@" }, StringSplitOptions.RemoveEmptyEntries);
                        if (strServiceItemData.Length == 2)
                        {
                            mObjUpdateMDAServiceItem = lstMDAServiceItems.Where(t => t.RowID == TrynParse.parseInt(strServiceItemData[0])).FirstOrDefault();

                            if (mObjUpdateMDAServiceItem != null)
                            {
                                decimal ServiceBaseAmount = TrynParse.parseDecimal(strServiceItemData[1]);

                                mObjUpdateMDAServiceItem.TaxBaseAmount = ServiceBaseAmount;
                                mObjUpdateMDAServiceItem.TaxAmount = ServiceBaseAmount;
                                mObjUpdateMDAServiceItem.ToSettleAmount = ServiceBaseAmount;

                                mObjUpdateMDAServiceItem.intTrack = mObjUpdateMDAServiceItem.TablePKID > 0 ? EnumList.Track.UPDATE : EnumList.Track.INSERT;

                                //Assessment_Items
                                mObjUpdateMDAService = lstMDAServices.Where(t => t.RowID == mObjUpdateMDAServiceItem.RowID).FirstOrDefault();
                                mObjUpdateMDAService.ToSettleAmount = ServiceBaseAmount;
                                mObjUpdateMDAService.AssessmentRuleAmount = lstMDAServiceItems.Where(t => t.AssessmentRule_RowID == mObjUpdateMDAService.RowID).Sum(t => t.TaxBaseAmount);
                                mObjUpdateMDAService.intTrack = mObjUpdateMDAService.TablePKID > 0 ? EnumList.Track.UPDATE : EnumList.Track.INSERT;
                            }
                        }
                    }

                    ViewBag.AssessmentRuleList = lstMDAServices.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
                    SessionManager.lstAssessmentRule = lstMDAServices;
                    SessionManager.lstAssessmentItem = lstMDAServiceItems;
                    dcResponse["success"] = true;
                    dcResponse["determinate"] = true;
                    // dcResponse["AssessmentRuleDetails"] = CommUtil.RenderPartialToString("_BindAssessmentRuleForSettlement", null, this.ControllerContext, ViewData);
                    dcResponse["AssessmentRuleDetails"] = CommUtil.RenderPartialToString("_BindAssessmentRuleForAssessment", null, this.ControllerContext, ViewData);



                }
                else
                {
                    ViewBag.AssessmentRuleList = SessionManager.lstMDAService.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();

                    dcResponse["success"] = true;
                    dcResponse["MDAServiceDetails"] = CommUtil.RenderPartialToString("_BindMDAServiceForSerivceBill", null, this.ControllerContext, ViewData);
                }
            }
            else
            {
                ViewBag.AssessmentRuleList = SessionManager.lstMDAService.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();

                dcResponse["success"] = true;
                dcResponse["MDAServiceDetails"] = CommUtil.RenderPartialToString("_BindMDAServiceForSerivceBill", null, this.ControllerContext, ViewData);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateAssessmentSettleAmount(string rowdata)
        {
            // int holder = SessionManager.DataSubmitterID;
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            IList<Assessment_AssessmentItem> lstMDAServiceItems = SessionManager.lstAssessmentItem ?? new List<Assessment_AssessmentItem>();
            IList<Assessment_AssessmentRule> lstMDAServices = SessionManager.lstAssessmentRule ?? new List<Assessment_AssessmentRule>();
            Assessment_AssessmentRule mObjUpdateMDAService = new Assessment_AssessmentRule();
            Assessment_AssessmentItem mObjUpdateMDAServiceItem = new Assessment_AssessmentItem();
            if (!string.IsNullOrWhiteSpace(rowdata))
            {
                string[] strRowData = rowdata.Split(new string[] { "~~" }, StringSplitOptions.RemoveEmptyEntries);

                if (strRowData.Length > 0 && strRowData.Length < 2)
                {
                    foreach (var vRowData in strRowData)
                    {
                        string[] strServiceItemData = vRowData.Split(new string[] { "@@" }, StringSplitOptions.RemoveEmptyEntries);
                        if (strServiceItemData.Length == 2)
                        {
                            mObjUpdateMDAServiceItem = lstMDAServiceItems.Where(t => t.RowID == TrynParse.parseInt(strServiceItemData[0])).FirstOrDefault();
                            // mObjUpdateMDAServiceItem = lstMDAServices.Where(t => t.RowID == TrynParse.parseInt(strServiceItemData[0])).FirstOrDefault();

                            if (mObjUpdateMDAServiceItem != null)
                            {
                                decimal ServiceBaseAmount = TrynParse.parseDecimal(strServiceItemData[1]);

                                mObjUpdateMDAServiceItem.ToSettleAmount = ServiceBaseAmount;

                                mObjUpdateMDAServiceItem.intTrack = mObjUpdateMDAServiceItem.TablePKID > 0 ? EnumList.Track.UPDATE : EnumList.Track.INSERT;

                                //Assessment_Items
                                mObjUpdateMDAService = lstMDAServices.Where(t => t.RowID == mObjUpdateMDAServiceItem.AssessmentRule_RowID).FirstOrDefault();

                                mObjUpdateMDAService.AssessmentRuleAmount = lstMDAServiceItems.Where(t => t.AssessmentRule_RowID == mObjUpdateMDAService.RowID).Sum(t => t.TaxAmount);
                                mObjUpdateMDAService.ToSettleAmount = ServiceBaseAmount;
                                mObjUpdateMDAService.intTrack = mObjUpdateMDAService.TablePKID > 0 ? EnumList.Track.UPDATE : EnumList.Track.INSERT;

                                ViewBag.Determinate = false;
                                ViewBag.ndnewToSettle = ServiceBaseAmount;
                            }
                        }
                    }
                    var ret = lstMDAServices.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
                    ViewBag.AssessmentRuleList = ret;
                    //one to many
                    if (lstMDAServiceItems.Count > lstMDAServices.Count)
                    {
                        ViewBag.ndnewToSettle = lstMDAServiceItems.Sum(x => x.ToSettleAmount);
                    }
                    //one to one
                    else
                        ViewBag.ndnewToSettle = ret.Sum(o => o.ToSettleAmount);
                    //ViewBag.AssessmentRuleList = lstMDAServices.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
                    SessionManager.lstAssessmentRule = lstMDAServices;
                    SessionManager.lstAssessmentItem = lstMDAServiceItems;
                    dcResponse["success"] = true;
                    dcResponse["NewHolder"] = ViewBag.ndnewToSettle;
                    // dcResponse["AssessmentRuleDetails"] = CommUtil.RenderPartialToString("_BindAssessmentRuleForSettlement", null, this.ControllerContext, ViewData);
                    dcResponse["AssessmentRuleDetails"] = CommUtil.RenderPartialToString("_BindAssessmentRuleForAssessment", null, this.ControllerContext, ViewData);

                }
                //else if (lstMDAServices.Count > 1 && strRowData.Length > 1)
                else if (strRowData.Length > 1)
                {
                    decimal newServiceBaseAmount = 0;
                    // int holderServiceBaseAmount = 0;
                    decimal ServiceBaseAmount = 0;
                    foreach (var k in strRowData)
                    {
                        List<string> strServiceItemData = k.Split(new string[] { "@@" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        decimal kkk = Convert.ToDecimal(strServiceItemData[1]);
                        newServiceBaseAmount += kkk;
                    }
                    foreach (var vRowData in strRowData)
                    {
                        string[] strServiceItemData = vRowData.Split(new string[] { "@@" }, StringSplitOptions.RemoveEmptyEntries);
                        if (strServiceItemData.Length == 2)
                        {
                            var mObjUpdateMDAServiceItemAll = lstMDAServiceItems.Where(t => t.RowID == TrynParse.parseInt(strServiceItemData[0])).ToList();
                            ServiceBaseAmount = mObjUpdateMDAServiceItemAll.Sum(o => o.ToSettleAmount);
                            mObjUpdateMDAServiceItem = lstMDAServiceItems.Where(t => t.RowID == TrynParse.parseInt(strServiceItemData[0])).FirstOrDefault();
                            // mObjUpdateMDAServiceItem = lstMDAServices.Where(t => t.RowID == TrynParse.parseInt(strServiceItemData[0])).FirstOrDefault();

                            if (mObjUpdateMDAServiceItem != null)
                            {
                                ServiceBaseAmount = TrynParse.parseDecimal(strServiceItemData[1]);

                                mObjUpdateMDAServiceItem.ToSettleAmount = ServiceBaseAmount;

                                mObjUpdateMDAServiceItem.intTrack = mObjUpdateMDAServiceItem.TablePKID > 0 ? EnumList.Track.UPDATE : EnumList.Track.INSERT;

                                //Assessment_Items
                                mObjUpdateMDAService = lstMDAServices.Where(t => t.RowID == mObjUpdateMDAServiceItem.AssessmentRule_RowID).FirstOrDefault();

                                mObjUpdateMDAService.AssessmentRuleAmount = lstMDAServiceItems.Where(t => t.AssessmentRule_RowID == mObjUpdateMDAService.RowID).Sum(t => t.TaxAmount);
                                mObjUpdateMDAService.ToSettleAmount = newServiceBaseAmount;
                                mObjUpdateMDAService.intTrack = mObjUpdateMDAService.TablePKID > 0 ? EnumList.Track.UPDATE : EnumList.Track.INSERT;

                                ViewBag.Determinate = false;
                                //ViewBag.ndnewToSettle = ServiceBaseAmount + newServiceBaseAmount;
                            }
                        }
                    }
                    var ret = lstMDAServices.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
                    ViewBag.AssessmentRuleList = ret;

                    ViewBag.ndnewToSettle = lstMDAServiceItems.Sum(x => x.ToSettleAmount);
                    mObjUpdateMDAService.ToSettleAmount = newServiceBaseAmount;


                    //ViewBag.AssessmentRuleList = lstMDAServices.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
                    SessionManager.lstAssessmentRule = lstMDAServices;
                    SessionManager.lstAssessmentItem = lstMDAServiceItems;
                    dcResponse["success"] = true;
                    dcResponse["NewHolder"] = ViewBag.ndnewToSettle;
                    // dcResponse["AssessmentRuleDetails"] = CommUtil.RenderPartialToString("_BindAssessmentRuleForSettlement", null, this.ControllerContext, ViewData);
                    dcResponse["AssessmentRuleDetails"] = CommUtil.RenderPartialToString("_BindAssessmentRuleForAssessment", null, this.ControllerContext, ViewData);

                }
                else
                {
                    ViewBag.AssessmentRuleList = SessionManager.lstAssessmentRule.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();

                    dcResponse["success"] = true;
                    dcResponse["MDAServiceDetails"] = CommUtil.RenderPartialToString("_BindAssessmentRuleForAssessment", null, this.ControllerContext, ViewData);
                }
            }
            else
            {
                ViewBag.AssessmentRuleList = SessionManager.lstAssessmentRule.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();

                dcResponse["success"] = true;
                dcResponse["MDAServiceDetails"] = CommUtil.RenderPartialToString("_BindMDAServiceForSerivceBill", null, this.ControllerContext, ViewData);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateAssessmentSettleAmount2(string rowdata)
        {
            // int holder = SessionManager.DataSubmitterID;
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            IList<usp_GetAssessment_AssessmentRuleList_Result> lstMDAServices = SessionManager.lstAssessmentRules ?? new List<usp_GetAssessment_AssessmentRuleList_Result>();
            IList<usp_GetAssessmentRuleItemList_Result> lstMDAServiceItems = SessionManager.lstAssessmentItems ?? new List<usp_GetAssessmentRuleItemList_Result>();


            //IList<Assessment_AssessmentItem> lstMDAServiceItems = SessionManager.lstAssessmentItem ?? new List<Assessment_AssessmentItem>();
            //IList<Assessment_AssessmentRule> lstMDAServices = SessionManager.lstAssessmentRule ?? new List<Assessment_AssessmentRule>();
            usp_GetAssessment_AssessmentRuleList_Result mObjUpdateMDAService = new usp_GetAssessment_AssessmentRuleList_Result();
            usp_GetAssessmentRuleItemList_Result mObjUpdateMDAServiceItem = new usp_GetAssessmentRuleItemList_Result();
            if (!string.IsNullOrWhiteSpace(rowdata))
            {
                string[] strRowData = rowdata.Split(new string[] { "~~" }, StringSplitOptions.RemoveEmptyEntries);

                if (strRowData.Length > 0 && strRowData.Length < 2)
                {
                    foreach (var vRowData in strRowData)
                    {
                        string[] strServiceItemData = vRowData.Split(new string[] { "@@" }, StringSplitOptions.RemoveEmptyEntries);
                        if (strServiceItemData.Length == 2)
                        {
                            mObjUpdateMDAServiceItem = lstMDAServiceItems.Where(t => t.AAIID == TrynParse.parseInt(strServiceItemData[0])).FirstOrDefault();
                            // mObjUpdateMDAServiceItem = lstMDAServices.Where(t => t.RowID == TrynParse.parseInt(strServiceItemData[0])).FirstOrDefault();

                            if (mObjUpdateMDAServiceItem != null)
                            {
                                decimal ServiceBaseAmount = TrynParse.parseDecimal(strServiceItemData[1]);

                                mObjUpdateMDAServiceItem.PendingAmount = ServiceBaseAmount;

                                //mObjUpdateMDAServiceItem.intTrack = mObjUpdateMDAServiceItem.TablePKID > 0 ? EnumList.Track.UPDATE : EnumList.Track.INSERT;

                                //Assessment_Items
                                mObjUpdateMDAService = lstMDAServices.Where(t => t.AssessmentRuleID == mObjUpdateMDAServiceItem.AssessmentRuleID).FirstOrDefault();

                                mObjUpdateMDAService.AssessmentRuleAmount = lstMDAServiceItems.Where(t => t.AARID == mObjUpdateMDAService.AARID).Sum(t => t.TotalAmount);
                                // mObjUpdateMDAService.ToSettleAmount = ServiceBaseAmount;
                                //mObjUpdateMDAService.intTrack = mObjUpdateMDAService.TablePKID > 0 ? EnumList.Track.UPDATE : EnumList.Track.INSERT;

                                ViewBag.Determinate = false;
                                ViewBag.ndnewToSettle = ServiceBaseAmount;
                            }
                        }
                    }
                    var ret = lstMDAServices.ToList();
                    ViewBag.AssessmentRuleList = ret;
                    //one to many
                    if (lstMDAServiceItems.Count > lstMDAServices.Count)
                    {
                        ViewBag.ndnewToSettle = lstMDAServiceItems.Sum(x => x.PendingAmount);
                    }
                    //one to one
                    else
                        ViewBag.ndnewToSettle = lstMDAServiceItems.Sum(x => x.PendingAmount);
                    //ViewBag.AssessmentRuleList = lstMDAServices.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
                    SessionManager.lstAssessmentRules = lstMDAServices;
                    SessionManager.lstAssessmentItems = lstMDAServiceItems;
                    dcResponse["success"] = true;
                    dcResponse["NewHolder"] = ViewBag.ndnewToSettle;
                    // dcResponse["AssessmentRuleDetails"] = CommUtil.RenderPartialToString("_BindAssessmentRuleForSettlement", null, this.ControllerContext, ViewData);
                    dcResponse["AssessmentRuleDetails"] = CommUtil.RenderPartialToString("_BindAssessmentRuleForAssessment2", null, this.ControllerContext, ViewData);

                }
                //else if (lstMDAServices.Count > 1 && strRowData.Length > 1)
                else if (strRowData.Length > 1)
                {
                    decimal newServiceBaseAmount = 0;
                    // int holderServiceBaseAmount = 0;
                    decimal? ServiceBaseAmount = 0;
                    foreach (var k in strRowData)
                    {
                        List<string> strServiceItemData = k.Split(new string[] { "@@" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        decimal kkk = Convert.ToDecimal(strServiceItemData[1]);
                        newServiceBaseAmount += kkk;
                    }
                    foreach (var vRowData in strRowData)
                    {
                        string[] strServiceItemData = vRowData.Split(new string[] { "@@" }, StringSplitOptions.RemoveEmptyEntries);
                        if (strServiceItemData.Length == 2)
                        {
                            var mObjUpdateMDAServiceItemAll = lstMDAServiceItems.Where(t => t.AssessmentItemID == TrynParse.parseInt(strServiceItemData[0])).ToList();
                            ServiceBaseAmount = mObjUpdateMDAServiceItemAll.Sum(o => o.PendingAmount);
                            mObjUpdateMDAServiceItem = lstMDAServiceItems.Where(t => t.AAIID == TrynParse.parseInt(strServiceItemData[0])).FirstOrDefault();
                            // mObjUpdateMDAServiceItem = lstMDAServices.Where(t => t.RowID == TrynParse.parseInt(strServiceItemData[0])).FirstOrDefault();

                            if (mObjUpdateMDAServiceItem != null)
                            {
                                ServiceBaseAmount = TrynParse.parseDecimal(strServiceItemData[1]);

                                mObjUpdateMDAServiceItem.PendingAmount = ServiceBaseAmount;

                                //mObjUpdateMDAServiceItem.intTrack = mObjUpdateMDAServiceItem.TablePKID > 0 ? EnumList.Track.UPDATE : EnumList.Track.INSERT;

                                //Assessment_Items
                                mObjUpdateMDAService = lstMDAServices.Where(t => t.AssessmentRuleID == mObjUpdateMDAServiceItem.AssessmentRuleID).FirstOrDefault();

                                mObjUpdateMDAService.AssessmentRuleAmount = lstMDAServiceItems.Where(t => t.AARID == mObjUpdateMDAService.AssessmentRuleID).Sum(t => t.TotalAmount);
                                //mObjUpdateMDAService.ToSettleAmount = newServiceBaseAmount;
                                //mObjUpdateMDAService.intTrack = mObjUpdateMDAService.TablePKID > 0 ? EnumList.Track.UPDATE : EnumList.Track.INSERT;

                                ViewBag.Determinate = false;
                                //ViewBag.ndnewToSettle = ServiceBaseAmount + newServiceBaseAmount;
                            }
                        }
                    }
                    var ret = lstMDAServices.ToList();
                    ViewBag.AssessmentRuleList = ret;
                    ViewBag.AssessmentItemList = lstMDAServiceItems;
                    ViewBag.ndnewToSettle = lstMDAServiceItems.Sum(x => x.PendingAmount);
                    // mObjUpdateMDAService.ToSettleAmount = newServiceBaseAmount;


                    //ViewBag.AssessmentRuleList = lstMDAServices.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
                    SessionManager.lstAssessmentRules = lstMDAServices;
                    SessionManager.lstAssessmentItems = lstMDAServiceItems;
                    dcResponse["success"] = true;
                    dcResponse["NewHolder"] = ViewBag.ndnewToSettle;
                    // dcResponse["AssessmentRuleDetails"] = CommUtil.RenderPartialToString("_BindAssessmentRuleForSettlement", null, this.ControllerContext, ViewData);
                    dcResponse["AssessmentRuleDetails"] = CommUtil.RenderPartialToString("_BindAssessmentRuleForAssessment2", null, this.ControllerContext, ViewData);

                }
                else
                {
                    ViewBag.AssessmentRuleList = SessionManager.lstAssessmentRule.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();

                    dcResponse["success"] = true;
                    dcResponse["MDAServiceDetails"] = CommUtil.RenderPartialToString("_BindAssessmentRuleForAssessment2", null, this.ControllerContext, ViewData);
                }
            }
            else
            {
                ViewBag.AssessmentRuleList = SessionManager.lstAssessmentRule.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();

                dcResponse["success"] = true;
                dcResponse["MDAServiceDetails"] = CommUtil.RenderPartialToString("_BindMDAServiceForSerivceBill", null, this.ControllerContext, ViewData);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateServiceBillSettleAmount(string rowdata)
        {
            int holder = SessionManager.DataSubmitterID;
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            IList<ServiceBill_MDAServiceItem> lstMDAServiceItems = SessionManager.lstMDAServiceItem ?? new List<ServiceBill_MDAServiceItem>();
            IList<ServiceBill_MDAService> lstMDAServices = SessionManager.lstMDAService ?? new List<ServiceBill_MDAService>();
            ServiceBill_MDAService mObjUpdateMDAService = new ServiceBill_MDAService();
            ServiceBill_MDAServiceItem mObjUpdateMDAServiceItem = new ServiceBill_MDAServiceItem();
            if (!string.IsNullOrWhiteSpace(rowdata))
            {
                string[] strRowData = rowdata.Split(new string[] { "~~" }, StringSplitOptions.RemoveEmptyEntries);

                if (strRowData.Length > 0 && lstMDAServices.Count < 2)
                {
                    foreach (var vRowData in strRowData)
                    {
                        string[] strServiceItemData = vRowData.Split(new string[] { "@@" }, StringSplitOptions.RemoveEmptyEntries);
                        if (strServiceItemData.Length == 2)
                        {
                            mObjUpdateMDAServiceItem = lstMDAServiceItems.Where(t => t.RowID == TrynParse.parseInt(strServiceItemData[0])).FirstOrDefault();
                            // mObjUpdateMDAServiceItem = lstMDAServices.Where(t => t.RowID == TrynParse.parseInt(strServiceItemData[0])).FirstOrDefault();

                            if (mObjUpdateMDAServiceItem != null)
                            {
                                decimal ServiceBaseAmount = TrynParse.parseDecimal(strServiceItemData[1]);

                                mObjUpdateMDAServiceItem.ToSettleAmount = ServiceBaseAmount;

                                mObjUpdateMDAServiceItem.intTrack = mObjUpdateMDAServiceItem.TablePKID > 0 ? EnumList.Track.UPDATE : EnumList.Track.INSERT;

                                //Assessment_Items
                                mObjUpdateMDAService = lstMDAServices.Where(t => t.RowID == mObjUpdateMDAServiceItem.RowID).FirstOrDefault();

                                mObjUpdateMDAService.ServiceAmount = lstMDAServiceItems.Where(t => t.RowID == mObjUpdateMDAService.RowID).Sum(t => t.ServiceAmount);
                                mObjUpdateMDAService.ToSettleAmount = ServiceBaseAmount;
                                //mObjUpdateMDAService.ServiceAmount = ServiceBaseAmount;
                                mObjUpdateMDAService.intTrack = mObjUpdateMDAService.TablePKID > 0 ? EnumList.Track.UPDATE : EnumList.Track.INSERT;

                                ViewBag.Determinate = false;
                            }
                        }
                    }

                    var ret = lstMDAServices.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
                    ViewBag.MDAServiceList = ret;

                    ViewBag.ndnewToSettle = ret.Sum(o => o.ToSettleAmount);
                    SessionManager.lstMDAService = lstMDAServices;

                    SessionManager.lstMDAServiceItem = lstMDAServiceItems;
                    dcResponse["success"] = true;
                    dcResponse["NewHolder"] = ViewBag.ndnewToSettle;
                    // dcResponse["AssessmentRuleDetails"] = CommUtil.RenderPartialToString("_BindAssessmentRuleForSettlement", null, this.ControllerContext, ViewData);
                    dcResponse["MDAServiceDetails"] = CommUtil.RenderPartialToString("_BindMDAServiceForSerivceBill", null, this.ControllerContext, ViewData);

                }
                else if (lstMDAServices.Count > 1)
                {
                    decimal newServiceBaseAmount = 0;
                    // int holderServiceBaseAmount = 0;
                    decimal ServiceBaseAmount = 0;
                    foreach (var k in strRowData)
                    {
                        List<string> strServiceItemData = k.Split(new string[] { "@@" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        decimal kkk = Convert.ToDecimal(strServiceItemData[1]);
                        newServiceBaseAmount += kkk;
                    }
                    foreach (var vRowData in strRowData)
                    {
                        string[] strServiceItemData = vRowData.Split(new string[] { "@@" }, StringSplitOptions.RemoveEmptyEntries);
                        if (strServiceItemData.Length == 2)
                        {
                            var mObjUpdateMDAServiceItemAll = lstMDAServiceItems.Where(t => t.RowID == TrynParse.parseInt(strServiceItemData[0])).ToList();
                            ServiceBaseAmount = mObjUpdateMDAServiceItemAll.Sum(o => o.ToSettleAmount);
                            mObjUpdateMDAServiceItem = lstMDAServiceItems.Where(t => t.RowID == TrynParse.parseInt(strServiceItemData[0])).FirstOrDefault();
                            // mObjUpdateMDAServiceItem = lstMDAServices.Where(t => t.RowID == TrynParse.parseInt(strServiceItemData[0])).FirstOrDefault();

                            if (mObjUpdateMDAServiceItem != null)
                            {
                                ServiceBaseAmount = TrynParse.parseDecimal(strServiceItemData[1]);

                                mObjUpdateMDAServiceItem.ToSettleAmount = ServiceBaseAmount;

                                mObjUpdateMDAServiceItem.intTrack = mObjUpdateMDAServiceItem.TablePKID > 0 ? EnumList.Track.UPDATE : EnumList.Track.INSERT;

                                //Assessment_Items
                                mObjUpdateMDAService = lstMDAServices.Where(t => t.RowID == mObjUpdateMDAServiceItem.MDAService_RowID).FirstOrDefault();

                                mObjUpdateMDAService.ServiceAmount = lstMDAServiceItems.Where(t => t.MDAService_RowID == mObjUpdateMDAService.RowID).Sum(t => t.ServiceAmount);
                                mObjUpdateMDAService.ToSettleAmount = newServiceBaseAmount;
                                mObjUpdateMDAService.intTrack = mObjUpdateMDAService.TablePKID > 0 ? EnumList.Track.UPDATE : EnumList.Track.INSERT;

                                ViewBag.Determinate = false;
                                //ViewBag.ndnewToSettle = ServiceBaseAmount + newServiceBaseAmount;
                            }
                        }
                    }
                    var ret = lstMDAServices.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
                    ViewBag.MDAServiceList = ret;

                    ViewBag.ndnewToSettle = lstMDAServiceItems.Sum(x => x.ToSettleAmount);
                    mObjUpdateMDAService.ToSettleAmount = newServiceBaseAmount;


                    //ViewBag.AssessmentRuleList = lstMDAServices.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
                    SessionManager.lstMDAService = lstMDAServices;
                    SessionManager.lstMDAServiceItem = lstMDAServiceItems;
                    dcResponse["success"] = true;
                    dcResponse["NewHolder"] = ViewBag.ndnewToSettle;
                    // dcResponse["AssessmentRuleDetails"] = CommUtil.RenderPartialToString("_BindAssessmentRuleForSettlement", null, this.ControllerContext, ViewData);
                    dcResponse["MDAServiceDetails"] = CommUtil.RenderPartialToString("_BindMDAServiceForSerivceBill", null, this.ControllerContext, ViewData);

                }
                else
                {
                    ViewBag.AssessmentRuleList = SessionManager.lstMDAService.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();

                    dcResponse["success"] = true;
                    dcResponse["MDAServiceDetails"] = CommUtil.RenderPartialToString("_BindMDAServiceForSerivceBill", null, this.ControllerContext, ViewData);
                }
            }
            else
            {
                ViewBag.AssessmentRuleList = SessionManager.lstMDAService.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();

                dcResponse["success"] = true;
                dcResponse["MDAServiceDetails"] = CommUtil.RenderPartialToString("_BindMDAServiceForSerivceBill", null, this.ControllerContext, ViewData);
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
                ViewBag.AssessmentRuleList = lstAssessmentRules.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
                dcResponse["success"] = true;
                dcResponse["AssessmentRuleList"] = CommUtil.RenderPartialToString("_BindAssessmentRuleForAssessment", null, this.ControllerContext, ViewData);
                // dcResponse["MDAServiceDetails"] = CommUtil.RenderPartialToString("_BindMDAServiceForSerivceBill", null, this.ControllerContext, ViewData);
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

        //public JsonResult UpdateAssessmentNewItem(string rowdata)
        //{
        //    int holder = SessionManager.DataSubmitterID;
        //    IDictionary<string, object> dcResponse = new Dictionary<string, object>();

        //    IList<Assessment_AssessmentItem> lstMDAServiceItems = SessionManager.lstAssessmentItem ?? new List<Assessment_AssessmentItem>();
        //    IList<Assessment_AssessmentRule> lstMDAServices = SessionManager.lstAssessmentRule ?? new List<Assessment_AssessmentRule>();
        //    Assessment_AssessmentRule mObjUpdateMDAService = new Assessment_AssessmentRule();
        //    Assessment_AssessmentItem mObjUpdateMDAServiceItem = new Assessment_AssessmentItem();
        //    if (!string.IsNullOrWhiteSpace(rowdata))
        //    {
        //        string[] strRowData = rowdata.Split(new string[] { "~~" }, StringSplitOptions.RemoveEmptyEntries);

        //        if (strRowData.Length > 0)
        //        {
        //            foreach (var vRowData in strRowData)
        //            {
        //                string[] strServiceItemData = vRowData.Split(new string[] { "@@" }, StringSplitOptions.RemoveEmptyEntries);
        //                if (strServiceItemData.Length == 2)
        //                {
        //                    mObjUpdateMDAServiceItem = lstMDAServiceItems.Where(t => t.RowID == TrynParse.parseInt(strServiceItemData[0])).FirstOrDefault();

        //                    if (mObjUpdateMDAServiceItem != null)
        //                    {
        //                        decimal ServiceBaseAmount = TrynParse.parseDecimal(strServiceItemData[1]);

        //                        mObjUpdateMDAServiceItem.TaxBaseAmount = ServiceBaseAmount;

        //                        mObjUpdateMDAServiceItem.intTrack = mObjUpdateMDAServiceItem.TablePKID > 0 ? EnumList.Track.UPDATE : EnumList.Track.INSERT;

        //                        //Assessment_Items
        //                        mObjUpdateMDAService = lstMDAServices.Where(t => t.RowID == mObjUpdateMDAServiceItem.RowID).FirstOrDefault();

        //                        mObjUpdateMDAService.AssessmentRuleAmount = lstMDAServiceItems.Where(t => t.RowID == mObjUpdateMDAService.RowID).Sum(t => t.TaxBaseAmount);
        //                        mObjUpdateMDAService.intTrack = mObjUpdateMDAService.TablePKID > 0 ? EnumList.Track.UPDATE : EnumList.Track.INSERT;
        //                    }
        //                }
        //            }

        //            ViewBag.AssessmentRuleList = lstMDAServices.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();
        //            SessionManager.lstAssessmentRule = lstMDAServices;
        //            SessionManager.lstAssessmentItem = lstMDAServiceItems;
        //            dcResponse["success"] = true;
        //            // dcResponse["AssessmentRuleDetails"] = CommUtil.RenderPartialToString("_BindAssessmentRuleForSettlement", null, this.ControllerContext, ViewData);
        //            dcResponse["AssessmentRuleDetails"] = CommUtil.RenderPartialToString("_BindAssessmentRuleForAssessment", null, this.ControllerContext, ViewData);



        //        }
        //        else
        //        {
        //            ViewBag.AssessmentRuleList = SessionManager.lstMDAService.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();

        //            dcResponse["success"] = true;
        //            dcResponse["MDAServiceDetails"] = CommUtil.RenderPartialToString("_BindMDAServiceForSerivceBill", null, this.ControllerContext, ViewData);
        //        }
        //    }
        //    else
        //    {
        //        ViewBag.AssessmentRuleList = SessionManager.lstMDAService.Where(t => t.intTrack != EnumList.Track.DELETE).ToList();

        //        dcResponse["success"] = true;
        //        dcResponse["MDAServiceDetails"] = CommUtil.RenderPartialToString("_BindMDAServiceForSerivceBill", null, this.ControllerContext, ViewData);
        //    }

        //    return Json(dcResponse, JsonRequestBehavior.AllowGet);
        //}

        public void UI_FillMonthDropDown()
        {
            IList<DropDownListResult> lstMonth = new List<DropDownListResult>
            {
                new DropDownListResult() { id = 1, text = "January" },
                new DropDownListResult() { id = 2, text = "February" },
                new DropDownListResult() { id = 3, text = "March" },
                new DropDownListResult() { id = 4, text = "April" },
                new DropDownListResult() { id = 5, text = "May" },
                new DropDownListResult() { id = 6, text = "June" },
                new DropDownListResult() { id = 7, text = "July" },
                new DropDownListResult() { id = 8, text = "August" },
                new DropDownListResult() { id = 9, text = "September" },
                new DropDownListResult() { id = 10, text = "October" },
                new DropDownListResult() { id = 11, text = "November" },
                new DropDownListResult() { id = 12, text = "December" }
            };

            ViewBag.MonthList = new SelectList(lstMonth, "id", "text");

        }

        public JsonResult GetTaxPayerList(int TaxPayerTypeID)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (TaxPayerTypeID == (int)EnumList.TaxPayerType.Individual)
            {
                IList<DropDownListResult> lstIndividual = new BLIndividual().BL_GetIndividualDropDownList(new Individual() { intStatus = 1 });

                dcResponse["success"] = true;
                dcResponse["TaxPayerList"] = lstIndividual;
            }
            else if (TaxPayerTypeID == (int)EnumList.TaxPayerType.Companies)
            {
                IList<DropDownListResult> lstCompany = new BLCompany().BL_GetCompanyDropDownList(new Company() { intStatus = 1 });

                dcResponse["success"] = true;
                dcResponse["TaxPayerList"] = lstCompany;
            }
            else if (TaxPayerTypeID == (int)EnumList.TaxPayerType.Government)
            {
                IList<DropDownListResult> lstGovernment = new BLGovernment().BL_GetGovernmentDropDownList(new Government() { intStatus = 1 });

                dcResponse["success"] = true;
                dcResponse["TaxPayerList"] = lstGovernment;
            }
            else if (TaxPayerTypeID == (int)EnumList.TaxPayerType.Special)
            {
                IList<DropDownListResult> lstSpecial = new BLSpecial().BL_GetSpecialDropDownList(new Special() { intStatus = 1 });

                dcResponse["success"] = true;
                dcResponse["TaxPayerList"] = lstSpecial;
            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Request";
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSettlementDetails(int SettlementID)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            usp_GetSettlementList_Result mObjSettlementData = new BLSettlement().BL_GetSettlementDetails(new Settlement() { SettlementID = SettlementID });

            if (mObjSettlementData != null)
            {
                dcResponse["success"] = true;
                dcResponse["SettlementDetails"] = mObjSettlementData;
            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Request";
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPoADetails(int PaymentAccountID)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            usp_GetPaymentAccountList_Result mObjPaymentData = new BLPaymentAccount().BL_GetPaymentAccountDetails(new Payment_Account() { PaymentAccountID = PaymentAccountID });

            if (mObjPaymentData != null)
            {
                dcResponse["success"] = true;
                dcResponse["PaymentAccountDetails"] = mObjPaymentData;
            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Request";
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPoATransferDetails(int POATID)
        {

            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            // Start query the Map_PoA_Transfer_Operation
            var poaTransferOperation = _db.Map_PoA_Transfer_Operation.FirstOrDefault(o => o.POATID == POATID);
            if (poaTransferOperation == null)
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "PoA Transfer Operation not found.";
                return Json(dcResponse, JsonRequestBehavior.AllowGet);
            }

            int PaymentAccountID = poaTransferOperation.To_Taxpayer_POAID ?? 0;
            // End query the Map_PoA_Transfer_Operation
            if (PaymentAccountID == 0)
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Payment Account ID.";
                return Json(dcResponse, JsonRequestBehavior.AllowGet);
            }

            usp_GetPaymentAccountList_Result mObjPaymentData = new BLPaymentAccount().BL_GetPaymentAccountDetails(new Payment_Account() { PaymentAccountID = PaymentAccountID });

            if (mObjPaymentData != null)
            {
                dcResponse["success"] = true;
                dcResponse["PaymentAccountDetails"] = mObjPaymentData;
            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Request";
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAssetDetails(int TPAID)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            MAP_TaxPayer_Asset mObjAsset = new BLTaxPayerAsset().BL_GetTaxPayerAssetDetails(TPAID);

            if (mObjAsset != null)
            {
                dcResponse["AssetTypeID"] = mObjAsset.AssetTypeID;
                dcResponse["success"] = true;
                if (mObjAsset.AssetTypeID == (int)EnumList.AssetTypes.Building)
                {
                    dcResponse["AssetDetails"] = new BLBuilding().BL_GetBuildingDetails(new Building() { intStatus = 2, BuildingID = mObjAsset.AssetID.GetValueOrDefault() });
                }
                else if (mObjAsset.AssetTypeID == (int)EnumList.AssetTypes.Vehicles)
                {
                    dcResponse["AssetDetails"] = new BLVehicle().BL_GetVehicleDetails(new Vehicle() { intStatus = 2, VehicleID = mObjAsset.AssetID.GetValueOrDefault() });
                }
                else if (mObjAsset.AssetTypeID == (int)EnumList.AssetTypes.Business)
                {
                    dcResponse["AssetDetails"] = new BLBusiness().BL_GetBusinessDetails(new Business() { intStatus = 2, BusinessID = mObjAsset.AssetID.GetValueOrDefault() });
                }
                else if (mObjAsset.AssetTypeID == (int)EnumList.AssetTypes.Land)
                {
                    dcResponse["AssetDetails"] = new BLLand().BL_GetLandDetails(new Land() { intStatus = 2, LandID = mObjAsset.AssetID.GetValueOrDefault() });
                }
            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Response";
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAssessmentRuleItemView(int AssessmentRuleRowID)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            IList<Assessment_AssessmentItem> lstAssessmentItems = SessionManager.lstAssessmentItem ?? new List<Assessment_AssessmentItem>();
            IList<Assessment_AssessmentRule> lstAssessmentRules = SessionManager.lstAssessmentRule ?? new List<Assessment_AssessmentRule>();

            Assessment_AssessmentRule mObjUpdateAssessmentRule = lstAssessmentRules.Where(t => t.RowID == AssessmentRuleRowID).FirstOrDefault();

            if (mObjUpdateAssessmentRule != null && lstAssessmentItems.Where(t => t.AssessmentRule_RowID == AssessmentRuleRowID).Count() > 0)
            {
                ViewBag.Mode = "View";
                dcResponse["success"] = true;
                dcResponse["AssessmentRuleItemList"] = CommUtil.RenderPartialToString("_BindAssessmentRuleItemList", lstAssessmentItems.Where(t => t.AssessmentRule_RowID == AssessmentRuleRowID).ToList(), this.ControllerContext, ViewData);
                dcResponse["AssessmentRuleName"] = mObjUpdateAssessmentRule.AssessmentRuleName;
            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Request";
            }


            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAssessmentRuleItemEdit(int AssessmentRuleRowID)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            IList<Assessment_AssessmentItem> lstAssessmentItems = SessionManager.lstAssessmentItem ?? new List<Assessment_AssessmentItem>();
            IList<Assessment_AssessmentRule> lstAssessmentRules = SessionManager.lstAssessmentRule ?? new List<Assessment_AssessmentRule>();

            Assessment_AssessmentRule mObjUpdateAssessmentRule = lstAssessmentRules.Where(t => t.RowID == AssessmentRuleRowID).FirstOrDefault();

            if (mObjUpdateAssessmentRule != null)
            {
                ViewBag.Mode = "Edit";
                dcResponse["success"] = true;
                dcResponse["AssessmentRuleItemList"] = CommUtil.RenderPartialToString("_BindAssessmentRuleItemList", lstAssessmentItems.Where(t => t.AssessmentRule_RowID == AssessmentRuleRowID).ToList(), this.ControllerContext, ViewData);
                dcResponse["AssessmentRuleName"] = mObjUpdateAssessmentRule.AssessmentRuleName;
            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Request";
            }


            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAssessmentRuleItemForSettlement(int AssessmentRuleRowID)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            IList<Assessment_AssessmentItem> lstAssessmentItems = SessionManager.lstAssessmentItem ?? new List<Assessment_AssessmentItem>();
            IList<Assessment_AssessmentRule> lstAssessmentRules = SessionManager.lstAssessmentRule ?? new List<Assessment_AssessmentRule>();

            Assessment_AssessmentRule mObjUpdateAssessmentRule = lstAssessmentRules.Where(t => t.RowID == AssessmentRuleRowID).FirstOrDefault();

            if (mObjUpdateAssessmentRule != null && lstAssessmentItems.Where(t => t.AssessmentRule_RowID == AssessmentRuleRowID).Count() > 0)
            {
                ViewBag.Mode = "EditForSettlement";
                dcResponse["success"] = true;
                dcResponse["AssessmentRuleItemList"] = CommUtil.RenderPartialToString("_BindAssessmentRuleItemList", lstAssessmentItems.Where(t => t.AssessmentRule_RowID == AssessmentRuleRowID).ToList(), this.ControllerContext, ViewData);
                dcResponse["AssessmentRuleName"] = mObjUpdateAssessmentRule.AssessmentRuleName;
            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Request";
            }


            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAssessmentRuleItemForSettlement2(int AssessmentRuleRowID)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            //IList<Assessment_AssessmentItem> lstAssessmentItems = SessionManager.lstAssessmentItem ?? new List<Assessment_AssessmentItem>();
            //IList<Assessment_AssessmentRule> lstAssessmentRules = SessionManager.lstAssessmentRule ?? new List<Assessment_AssessmentRule>();
            IList<usp_GetAssessment_AssessmentRuleList_Result> lstAssessmentRule = SessionManager.lstAssessmentRules ?? new List<usp_GetAssessment_AssessmentRuleList_Result>();
            IList<usp_GetAssessmentRuleItemList_Result> lstAssessmentItem = SessionManager.lstAssessmentItems ?? new List<usp_GetAssessmentRuleItemList_Result>();

            usp_GetAssessment_AssessmentRuleList_Result mObjUpdateAssessmentRule = lstAssessmentRule.Where(t => t.AARID == AssessmentRuleRowID).FirstOrDefault();

            if (mObjUpdateAssessmentRule != null && lstAssessmentItem.Where(t => t.AARID == AssessmentRuleRowID).Count() > 0)
            {
                ViewBag.Mode = "EditForSettlement";
                dcResponse["success"] = true;
                dcResponse["AssessmentRuleItemList"] = CommUtil.RenderPartialToString("_BindAssessmentRuleItemList2", lstAssessmentItem.Where(t => t.AARID == AssessmentRuleRowID).ToList(), this.ControllerContext, ViewData);
                dcResponse["AssessmentRuleName"] = mObjUpdateAssessmentRule.AssessmentRuleName;
            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Request";
            }


            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAssessmentRulePaymentView(int AssessmentRuleRowID)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            IList<usp_GetAssessmentRuleBasedSettlement_Result> lstAssessmentRuleSettlement = SessionManager.lstAssessmentRuleSettlement ?? new List<usp_GetAssessmentRuleBasedSettlement_Result>();

            IList<usp_GetAssessment_AssessmentRuleList_Result> lstAssessmentRules = SessionManager.lstAssessmentRules ?? new List<usp_GetAssessment_AssessmentRuleList_Result>();

            usp_GetAssessment_AssessmentRuleList_Result mObjUpdateAssessmentRule = lstAssessmentRules.Where(t => t.AARID == AssessmentRuleRowID).FirstOrDefault();

            dcResponse["success"] = true;
            dcResponse["AssessmentRulePaymentList"] = CommUtil.RenderPartialToString("_BindAssessmentRulePaymentList", lstAssessmentRuleSettlement.Where(t => t.AARID == mObjUpdateAssessmentRule.AARID).ToList(), this.ControllerContext, ViewData);
            dcResponse["AssessmentRuleName"] = mObjUpdateAssessmentRule.AssessmentRuleName;

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMDAServiceItemView(int MDAServiceRowID)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            IList<ServiceBill_MDAServiceItem> lstMDAServiceItems = SessionManager.lstMDAServiceItem ?? new List<ServiceBill_MDAServiceItem>();
            IList<ServiceBill_MDAService> lstMDAServices = SessionManager.lstMDAService ?? new List<ServiceBill_MDAService>();

            ServiceBill_MDAService mObjUpdateMDAService = lstMDAServices.Where(t => t.RowID == MDAServiceRowID).FirstOrDefault();

            if (mObjUpdateMDAService != null && lstMDAServiceItems.Where(t => t.MDAService_RowID == MDAServiceRowID).Count() > 0)
            {
                ViewBag.Mode = "View";
                dcResponse["success"] = true;
                dcResponse["MDAServiceItemList"] = CommUtil.RenderPartialToString("_BindMDAServiceItemList", lstMDAServiceItems.Where(t => t.MDAService_RowID == MDAServiceRowID).ToList(), this.ControllerContext, ViewData);
                dcResponse["MDAServiceName"] = mObjUpdateMDAService.MDAServiceName;
            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Request";
            }


            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMDAServiceItemEdit(int MDAServiceRowID)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            IList<ServiceBill_MDAServiceItem> lstMDAServiceItems = SessionManager.lstMDAServiceItem ?? new List<ServiceBill_MDAServiceItem>();
            IList<ServiceBill_MDAService> lstMDAServices = SessionManager.lstMDAService ?? new List<ServiceBill_MDAService>();

            ServiceBill_MDAService mObjUpdateMDAService = lstMDAServices.Where(t => t.RowID == MDAServiceRowID).FirstOrDefault();

            if (mObjUpdateMDAService != null && lstMDAServiceItems.Where(t => t.MDAService_RowID == MDAServiceRowID).Count() > 0)
            {
                ViewBag.Mode = "Edit";
                dcResponse["success"] = true;
                dcResponse["MDAServiceItemList"] = CommUtil.RenderPartialToString("_BindMDAServiceItemList", lstMDAServiceItems.Where(t => t.MDAService_RowID == MDAServiceRowID).ToList(), this.ControllerContext, ViewData);
                dcResponse["MDAServiceName"] = mObjUpdateMDAService.MDAServiceName;
            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Request";
            }


            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMDAServiceItemEditForSettlement(int MDAServiceRowID)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            IList<ServiceBill_MDAServiceItem> lstMDAServiceItems = SessionManager.lstMDAServiceItem ?? new List<ServiceBill_MDAServiceItem>();
            IList<ServiceBill_MDAService> lstMDAServices = SessionManager.lstMDAService ?? new List<ServiceBill_MDAService>();

            ServiceBill_MDAService mObjUpdateMDAService = lstMDAServices.Where(t => t.RowID == MDAServiceRowID).FirstOrDefault();

            if (mObjUpdateMDAService != null && lstMDAServiceItems.Where(t => t.MDAService_RowID == MDAServiceRowID).Count() > 0)
            {
                ViewBag.Mode = "EditForSettlement";
                dcResponse["success"] = true;
                dcResponse["MDAServiceItemList"] = CommUtil.RenderPartialToString("_BindMDAServiceItemList", lstMDAServiceItems.Where(t => t.MDAService_RowID == MDAServiceRowID).ToList(), this.ControllerContext, ViewData);
                dcResponse["MDAServiceName"] = mObjUpdateMDAService.MDAServiceName;
            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Request";
            }


            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMDAServicePaymentView(int MDAServiceRowID)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            IList<usp_GetMDAServiceBasedSettlement_Result> lstMDAServiceSettlement = SessionManager.lstMDAServiceSettlement ?? new List<usp_GetMDAServiceBasedSettlement_Result>();

            IList<ServiceBill_MDAService> lstMDAServices = SessionManager.lstMDAService ?? new List<ServiceBill_MDAService>();

            ServiceBill_MDAService mObjUpdateMDAService = lstMDAServices.Where(t => t.RowID == MDAServiceRowID).FirstOrDefault();

            dcResponse["success"] = true;
            dcResponse["MDAServicePaymentList"] = CommUtil.RenderPartialToString("_BindMDAServicePaymentList", lstMDAServiceSettlement.Where(t => t.SBSID == mObjUpdateMDAService.TablePKID).ToList(), this.ControllerContext, ViewData);
            dcResponse["MDAServiceName"] = mObjUpdateMDAService.MDAServiceName;

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTaxPayerAssessmentRules(int TaxPayerTypeID, int TaxPayerID)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            IList<Assessment_AssessmentRule> lstSessionAssessmentRule = SessionManager.lstAssessmentRule ?? new List<Assessment_AssessmentRule>();

            IList<usp_GetAssessmentRuleForAssessment_Result> lstAssessmentRules = new BLTaxPayerAsset().BL_GetTaxPayerAssessmentRuleForAssessment(TaxPayerTypeID, TaxPayerID);

            foreach (var item in lstSessionAssessmentRule)
            {
                if (item.intTrack != EnumList.Track.DELETE)
                {
                    var vAssessmentRule = lstAssessmentRules.Where(t => t.AssessmentRuleID == item.AssessmentRuleID
                                                                                        && t.AssetID == item.AssetID
                                                                                        && t.AssetTypeID == item.AssetTypeID
                                                                                        && t.ProfileID == item.ProfileID).SingleOrDefault();
                    lstAssessmentRules.Remove(vAssessmentRule);
                }
                //else if(item.intTrack == EnumList.Track.DELETE)
                //{
                //    usp_GetAssessmentRuleForAssessment_Result mObjAssessmentRule = new usp_GetAssessmentRuleForAssessment_Result()
                //    {
                //        AssetTypeName = item.AssetTypeName,
                //        AssetTypeID = item.AssetTypeID,
                //        AssetID = item.AssetID,
                //        AssetRIN = item.AssetRIN,
                //        TaxYear = item.TaxYear,
                //        AssessmentRuleName = item.AssessmentRuleName,
                //        AssessmentRuleID = item.AssessmentRuleID,
                //        AssessmentAmount = item.AssessmentRuleAmount,
                //        ProfileID = item.ProfileID,
                //        ProfileDescription = item.ProfileDescription,
                //    };

                //    lstAssessmentRules.Add(mObjAssessmentRule);
                //}
            }

            dcResponse["AssessmentRuleList"] = CommUtil.RenderPartialToString("_BindAssessmentRuleTableForAssessment", lstAssessmentRules, this.ControllerContext);
            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSettlementList(int? AssessmentID, int? ServiceID)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();
            IList<usp_GetSettlementList_Result> lstSettlement = new BLSettlement().BL_GetSettlementList(new Settlement() { AssessmentID = AssessmentID, ServiceBillID = ServiceID });
            dcResponse["success"] = true;
            dcResponse["SettlementList"] = CommUtil.RenderPartialToString("_BindSettlementList", lstSettlement, this.ControllerContext, ViewData);
            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMDAServiceData()
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
                IList<vw_MDAServices> lstMDAServiceData = new BLMDAService().BL_GetMDAServiceList();

                // Total record count.   
                int totalRecords = lstMDAServiceData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstMDAServiceData = lstMDAServiceData.Where(p => p.TaxYear.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.MDAServiceName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.RuleRunName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.PaymentFrequencyName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.ServiceAmount.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower())).ToList();
                }

                // Sorting.   
                lstMDAServiceData = this.SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstMDAServiceData);

                // Filter record count.   
                int recFilter = lstMDAServiceData.Count;

                // Apply pagination.   
                lstMDAServiceData = lstMDAServiceData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstMDAServiceData;

            }
            catch (Exception ex)
            {
                Logger.SendErrorToText(ex);
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        private IList<vw_MDAServices> SortByColumnWithOrder(string order, string orderDir, IList<vw_MDAServices> data)
        {
            // Initialization.   
            IList<vw_MDAServices> lst = new List<vw_MDAServices>();
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
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.MDAServiceName).ToList() : data.OrderBy(p => p.MDAServiceName).ToList();
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
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ServiceAmount).ToList() : data.OrderBy(p => p.ServiceAmount).ToList();
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.SendErrorToText(ex);
                // info.   
                //Console.Write(ex);
            }
            // info.   
            return lst;
        }

        public JsonResult GetAssessmentRuleData()
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
            catch (Exception ex)
            {
                Logger.SendErrorToText(ex);
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
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
                Logger.SendErrorToText(ex);
                // info.   
                //Console.Write(ex);
            }
            // info.   
            return lst;
        }

        public JsonResult GetTaxPayer(int TaxPayerTypeID, string query)
        {
            IList<DropDownListResult> lstTaxPayer;
            if (TaxPayerTypeID == (int)EnumList.TaxPayerType.Individual)
            {
                lstTaxPayer = new BLIndividual().BL_GetIndividualDropDownList(query);
            }
            else if (TaxPayerTypeID == (int)EnumList.TaxPayerType.Companies)
            {
                lstTaxPayer = new BLCompany().BL_GetCompanyDropDownList(query);
            }
            else if (TaxPayerTypeID == (int)EnumList.TaxPayerType.Government)
            {
                lstTaxPayer = new BLGovernment().BL_GetGovernmentDropDownList(query);
            }
            else if (TaxPayerTypeID == (int)EnumList.TaxPayerType.Special)
            {
                lstTaxPayer = new BLSpecial().BL_GetSpecialDropDownList(query);
            }
            else
            {
                lstTaxPayer = null;
            }
            return Json(lstTaxPayer.Take(10), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTaxOfficeBasedTaxPayer(int TaxOfficeID, int TaxPayerTypeID, string query)
        {
            IList<DropDownListResult> lstTaxPayer;
            if (TaxPayerTypeID == (int)EnumList.TaxPayerType.Individual)
            {
                lstTaxPayer = new BLIndividual().BL_GetIndividualDropDownList(query, TaxOfficeID);
            }
            else if (TaxPayerTypeID == (int)EnumList.TaxPayerType.Companies)
            {
                lstTaxPayer = new BLCompany().BL_GetCompanyDropDownList(query, TaxOfficeID);
            }
            else if (TaxPayerTypeID == (int)EnumList.TaxPayerType.Government)
            {
                lstTaxPayer = new BLGovernment().BL_GetGovernmentDropDownList(query, TaxOfficeID);
            }
            else if (TaxPayerTypeID == (int)EnumList.TaxPayerType.Special)
            {
                lstTaxPayer = new BLSpecial().BL_GetSpecialDropDownList(query, TaxOfficeID);
            }
            else
            {
                lstTaxPayer = null;
            }
            return Json(lstTaxPayer.Take(10), JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchRIN(string RIN)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (SessionManager.TaxPayerTypeID == 0)
            {
                SearchTaxPayerFilter mObjTaxPayerFilter = new SearchTaxPayerFilter()
                {
                    TaxPayerRIN = RIN,
                    intSearchType = 1
                };

                IList<usp_SearchTaxPayer_Result> lstTaxPayer = new BLTaxPayerAsset().BL_SearchTaxPayer(mObjTaxPayerFilter);

                if (lstTaxPayer.Count() > 0)
                {
                    dcResponse["success"] = true;
                    if (lstTaxPayer.FirstOrDefault().TaxPayerTypeID == (int)EnumList.TaxPayerType.Individual)
                    {
                        dcResponse["RedirectUrl"] = Url.Action("Details", "CaptureIndividual", new { id = lstTaxPayer.FirstOrDefault().TaxPayerID, name = lstTaxPayer.FirstOrDefault().TaxPayerName.ToSeoUrl() });
                    }
                    else if (lstTaxPayer.FirstOrDefault().TaxPayerTypeID == (int)EnumList.TaxPayerType.Companies)
                    {
                        dcResponse["RedirectUrl"] = Url.Action("Details", "CaptureCorporate", new { id = lstTaxPayer.FirstOrDefault().TaxPayerID, name = lstTaxPayer.FirstOrDefault().TaxPayerName.ToSeoUrl() });
                    }
                    else if (lstTaxPayer.FirstOrDefault().TaxPayerTypeID == (int)EnumList.TaxPayerType.Government)
                    {
                        dcResponse["RedirectUrl"] = Url.Action("Details", "CaptureGovernment", new { id = lstTaxPayer.FirstOrDefault().TaxPayerID, name = lstTaxPayer.FirstOrDefault().TaxPayerName.ToSeoUrl() });
                    }
                    else if (lstTaxPayer.FirstOrDefault().TaxPayerTypeID == (int)EnumList.TaxPayerType.Special)
                    {
                        dcResponse["RedirectUrl"] = Url.Action("Details", "CaptureSpecial", new { id = lstTaxPayer.FirstOrDefault().TaxPayerID, name = lstTaxPayer.FirstOrDefault().TaxPayerName.ToSeoUrl() });
                    }
                }
                else
                {
                    dcResponse["success"] = false;
                    dcResponse["Message"] = "Invalid Tax Payer RIN";
                }
            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Request";
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTaxOfficeManger(int TaxOfficeID, bool showmanager)
        {
            IList<DropDownListResult> lstTaxOfficeManager = new BLCommon().BL_GetTaxOfficeManagerList(TaxOfficeID, showmanager);
            return Json(lstTaxOfficeManager, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PostReview(ReviewViewModel pObjReviewModel)
        {
            if (ModelState.IsValid)
            {
                MAP_TaxPayer_Review mObjReview = new MAP_TaxPayer_Review()
                {
                    ReviewDate = CommUtil.GetCurrentDateTime(),
                    Notes = pObjReviewModel.Notes,
                    TaxPayerID = pObjReviewModel.TaxPayerID,
                    TaxPayerTypeID = pObjReviewModel.TaxPayerTypeID,
                    ReviewStatusID = pObjReviewModel.ReviewStatusID,
                    UserID = SessionManager.UserID,
                    CreatedBy = SessionManager.UserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                FuncResponse mObjFuncResponse = new BLReview().BL_InsertTaxPayerReview(mObjReview);
                if (mObjFuncResponse.Success)
                {
                    return Content("<div class='alert alert-success'>" + mObjFuncResponse.Message + "</div>");
                }
                else
                {
                    return Content("<div class='alert alert-danger'>" + mObjFuncResponse.Message + "</div>");
                }
            }
            else
            {
                return Content("<div class='alert alert-danger'>All Fields Required</div>");
            }
        }

        public JsonResult GetTaxPayerReview(int TaxPayerID, int TaxPayerTypeID)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();
            IList<usp_GetTaxPayerReviewNotes_Result> lstReivewNotes = new BLReview().BL_GetReviewNotes(new MAP_TaxPayer_Review() { TaxPayerID = TaxPayerID, TaxPayerTypeID = TaxPayerTypeID });
            dcResponse["ReviewList"] = CommUtil.RenderPartialToString("_BindReviewNotes", lstReivewNotes, this.ControllerContext);
            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public void UI_FillEMCategoryDropDown(EM_Category pObjCategory = null)
        {
            if (pObjCategory == null)
                pObjCategory = new EM_Category();

            pObjCategory.intStatus = 1;

            IList<DropDownListResult> lstCategory = new BLEMCategory().BL_GetCategoryDropDownList(pObjCategory);
            ViewBag.CategoryList = new SelectList(lstCategory, "id", "text");
        }

        public void UI_FillEMRevenueHeadDropDown(EM_RevenueHead pObjRevenueHead = null)
        {
            if (pObjRevenueHead == null)
                pObjRevenueHead = new EM_RevenueHead();

            pObjRevenueHead.intStatus = 1;

            IList<DropDownListResult> lstRevenueHead = new BLEMRevenueHead().BL_GetRevenueHeadDropDownList(pObjRevenueHead);
            ViewBag.RevenueHeadList = new SelectList(lstRevenueHead, "id", "text");
        }

        public JsonResult GetEMRevenueHead(int CategoryID)
        {
            IList<DropDownListResult> lstRevenueHead = new BLEMRevenueHead().BL_GetRevenueHeadDropDownList(new EM_RevenueHead() { intStatus = 1, CategoryID = CategoryID });
            return Json(lstRevenueHead, JsonRequestBehavior.AllowGet);
        }

        public void UI_FillSFTPDataSubmissionTypeDropDown(SFTP_DataSubmissionType pObjDataSubmissionType = null)
        {
            if (pObjDataSubmissionType == null)
                pObjDataSubmissionType = new SFTP_DataSubmissionType();

            pObjDataSubmissionType.intStatus = 1;

            IList<DropDownListResult> lstDataSubmissionType = new BLSFTPDataSubmissionType().BL_GetDataSubmissionTypeDropDownList(pObjDataSubmissionType);
            ViewBag.DataSubmissionTypeList = new SelectList(lstDataSubmissionType, "id", "text");
        }

        public void UI_FillSFTPDataSubmitterDropDown(SFTP_DataSubmitter pObjDataSubmitter = null)
        {
            if (pObjDataSubmitter == null)
                pObjDataSubmitter = new SFTP_DataSubmitter();

            pObjDataSubmitter.intStatus = 1;

            IList<DropDownListResult> lstDataSubmitter = new BLSFTPDataSubmitter().BL_GetDataSubmitterDropDownList(pObjDataSubmitter);
            ViewBag.DataSubmitterList = new SelectList(lstDataSubmitter, "id", "text");
        }

        public void UI_FillSFTPDSDSTDropDown(SFTP_DataSubmitter pObjDataSubmitter = null)
        {
            if (pObjDataSubmitter == null)
                pObjDataSubmitter = new SFTP_DataSubmitter();

            pObjDataSubmitter.intStatus = 1;

            IList<DropDownListResult> lstDataSubmissionType = new BLSFTPDataSubmitter().BL_GetDataSubmissionTypeDropDownList(pObjDataSubmitter);
            ViewBag.DataSubmissionTypeList = new SelectList(lstDataSubmissionType, "id", "text");
        }

        public JsonResult GetDataSubmissionType(int DataSubmitterID)
        {
            IList<DropDownListResult> lstDataSubmissionType = new BLSFTPDataSubmitter().BL_GetDataSubmissionTypeDropDownList(new SFTP_DataSubmitter() { DataSubmitterID = DataSubmitterID });
            return Json(lstDataSubmissionType, JsonRequestBehavior.AllowGet);
        }

        public FileResult ExportToExcel<T>(IList<T> lstData, RouteData routeData, string[] lstColumns, bool blnShowTotal, string[] strTotalColumns = null, string AppendExcelName = "")
        {
            var vMemberInfoData = typeof(T)
                    .GetProperties()
                    .Join(lstColumns.Select((value, index) => new { value, index }), a => a.Name, b => b.value, (c, d) => new { memberInfo = c, sort = d.index })
                    .OrderBy(t => t.sort)
                    .Select(pi => (MemberInfo)pi.memberInfo)
                    .ToArray();

            byte[] ObjExcelData = CommUtil.ToExcel(lstData, AppendExcelName);
            return File(ObjExcelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", routeData.ToExcelName(AppendExcelName));
        }

        public FileResult ExportToExcel<T>(IList<T> lstData, RouteData routeData, string[] lstColumns, string AppendExcelName)
        {
            var vMemberInfoData = typeof(T)
                    .GetProperties()
                    .Join(lstColumns.Select((value, index) => new { value, index }), a => a.Name, b => b.value, (c, d) => new { memberInfo = c, sort = d.index })
                    .OrderBy(t => t.sort)
                    .Select(pi => (MemberInfo)pi.memberInfo)
                    .ToArray();

            byte[] ObjExcelData = CommUtil.ToExcel(lstData, AppendExcelName);
            return File(ObjExcelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", routeData.ToExcelName(AppendExcelName));
        }


        public ActionResult GetTaxPayerMessage(int TaxPayerID, int TaxPayerTypeID)
        {
            IList<usp_GetTaxPayerMessageList_Result> lstMessage = new BLTaxPayerMessage().BL_GetMessageList(new MAP_TaxPayer_Message() { TaxPayerID = TaxPayerID, TaxPayerTypeID = TaxPayerTypeID });
            return PartialView("_BindMessageTab", lstMessage);
        }

        public ActionResult GetMessageAttachment(long tpmid)
        {
            IList<MAP_TaxPayer_Message_Document> lstDocument = new BLTaxPayerMessage().BL_GetMessageDocumentList(tpmid);
            return PartialView("_BindMessageAttachment", lstDocument);
        }

    }
}