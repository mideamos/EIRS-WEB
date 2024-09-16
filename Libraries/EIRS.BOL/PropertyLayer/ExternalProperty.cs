using System;

namespace EIRS.BOL
{
    partial class MAP_TaxPayer_Asset
    {
        public int ProfileID { get; set; }
        public int IntStatus { get; set; }
    }

    partial class Certificate_Types
    {
        public int IntStatus { get; set; }
    }

    partial class Scratch_Card_Dealers
    {
        public int intStatus { get; set; }
        public string ScratchCardDealerIds { get; set; }
        public string IncludeScratchCardDealerIds { get; set; }
        public string ExcludeScratchCardDealerIds { get; set; }

    }
    partial class Scratch_Card_Printer
    {
        public int intStatus { get; set; }
        public string ScratchCardPrinterIds { get; set; }
        public string IncludeScratchCardPrinterIds { get; set; }
        public string ExcludeScratchCardPrinterIds { get; set; }

    }

    partial class Exception_Type
    {
        public int intStatus { get; set; }
        public string ExceptionTypeIds { get; set; }
        public string IncludeExceptionTypeIds { get; set; }
        public string ExcludeExceptionTypeIds { get; set; }

    }
    partial class Settlement_Status
    {
        public int intStatus { get; set; }
        public string SettlementStatusIds { get; set; }
        public string IncludeSettlementStatusIds { get; set; }
        public string ExcludeSettlementStatusIds { get; set; }

    }
    partial class Notification_Type
    {
        public int intStatus { get; set; }
        public string NotificationTypeIds { get; set; }
        public string IncludeNotificationTypeIds { get; set; }
        public string ExcludeNotificationTypeIds { get; set; }

    }
    partial class Notification_Method
    {
        public int intStatus { get; set; }

        public string NotificationMethodIds { get; set; }
        public string IncludeNotificationMethodIds { get; set; }
        public string ExcludeNotificationMethodIds { get; set; }
    }
    partial class SystemUser
    {
        public int intStatus { get; set; }

        public string SystemUserIds { get; set; }

        public string IncludeSystemUserIds { get; set; }

        public string ExcludeSystemUserIds { get; set; }

        public string OldPassword { get; set; }
    }

    partial class Asset_Types
    {
        public int intStatus { get; set; }

        public string AssetTypeIds { get; set; }

        public string IncludeAssetTypeIds { get; set; }

        public string ExcludeAssetTypeIds { get; set; }

    }

    partial class Government_Types
    {
        public int intStatus { get; set; }

        public string GovernmentTypeIds { get; set; }

        public string IncludeGovernmentTypeIds { get; set; }

        public string ExcludeGovernmentTypeIds { get; set; }

    }

    partial class Address_Types
    {
        public int intStatus { get; set; }

        public string AddressTypeIds { get; set; }

        public string IncludeAddressTypeIds { get; set; }

        public string ExcludeAddressTypeIds { get; set; }

    }

    partial class Tax_Offices
    {
        public int intStatus { get; set; }

        public string TaxOfficeIds { get; set; }

        public string IncludeTaxOfficeIds { get; set; }

        public string ExcludeTaxOfficeIds { get; set; }

    }

    partial class Ward
    {
        public int intStatus { get; set; }

        public string WardIds { get; set; }

        public string IncludeWardIds { get; set; }

        public string ExcludeWardIds { get; set; }

    }

    partial class LGA
    {
        public int intStatus { get; set; }

        public string LGAIds { get; set; }

        public string IncludeLGAIds { get; set; }

        public string ExcludeLGAIds { get; set; }

    }

    partial class Town
    {
        public int intStatus { get; set; }

        public string TownIds { get; set; }

        public string IncludeTownIds { get; set; }

        public string ExcludeTownIds { get; set; }

    }

    partial class Building_Types
    {
        public int intStatus { get; set; }

        public string BuildingTypeIds { get; set; }

        public string IncludeBuildingTypeIds { get; set; }

        public string ExcludeBuildingTypeIds { get; set; }

    }

    partial class Building_Unit
    {
        public int intStatus { get; set; }

        public string BuildingUnitIds { get; set; }

        public string IncludeBuildingUnitIds { get; set; }

        public string ExcludeBuildingUnitIds { get; set; }

        public string UnitPurposeName { get; set; }
        public string UnitFunctionName { get; set; }
        public string UnitOccupancyName { get; set; }
        public string SizeName { get; set; }
        public string ActiveText { get; set; }

        public string OrderBy { get; set; }
        public string OrderByDirection { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string WhereCondition { get; set; }
        public string MainFilter { get; set; }

    }

    partial class Building_Completion
    {
        public int intStatus { get; set; }

        public string BuildingCompletionIds { get; set; }

        public string IncludeBuildingCompletionIds { get; set; }

        public string ExcludeBuildingCompletionIds { get; set; }

    }

    partial class Unit_Occupancy
    {
        public int intStatus { get; set; }

        public string UnitOccupancyIds { get; set; }

        public string IncludeUnitOccupancyIds { get; set; }

        public string ExcludeUnitOccupancyIds { get; set; }

    }

    partial class Building_Purpose
    {
        public int intStatus { get; set; }

        public string BuildingPurposeIds { get; set; }

        public string IncludeBuildingPurposeIds { get; set; }

        public string ExcludeBuildingPurposeIds { get; set; }

    }

    partial class Unit_Purpose
    {
        public int intStatus { get; set; }

        public string UnitPurposeIds { get; set; }

        public string IncludeUnitPurposeIds { get; set; }

        public string ExcludeUnitPurposeIds { get; set; }

    }

    partial class Building_Ownership
    {
        public int intStatus { get; set; }

        public string BuildingOwnershipIds { get; set; }

        public string IncludeBuildingOwnershipIds { get; set; }

        public string ExcludeBuildingOwnershipIds { get; set; }

    }

    partial class Unit_Function
    {
        public int intStatus { get; set; }

        public string UnitFunctionIds { get; set; }

        public string IncludeUnitFunctionIds { get; set; }

        public string ExcludeUnitFunctionIds { get; set; }

    }


    partial class Building
    {
        public int intStatus { get; set; }

        public string TownName { get; set; }
        public string LGAName { get; set; }
        public string WardName { get; set; }
        public string BuildingTypeName { get; set; }
        public string BuildingCompletionName { get; set; }
        public string BuildingPurposeName { get; set; }
        public string BuildingOwnershipName { get; set; }
        public string strNoOfUnits { get; set; }
        public string strBuildingSize_Length { get; set; }
        public string strBuildingSize_Width { get; set; }
        public string ActiveText { get; set; }

        public string OrderBy { get; set; }
        public string OrderByDirection { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string WhereCondition { get; set; }
        public string MainFilter { get; set; }
    }

    partial class Individual
    {
        public string IndividualName { get; set; }
        public int intStatus { get; set; }
        public string OldPassword { get; set; }
        public string TaxOfficeName { get; set; }
        public string TaxPayerTypeName { get; set; }
        public string EconomicActivitiesName { get; set; }
        public string NotificationMethodName { get; set; }
        public string ActiveText { get; set; }
        public string GenderName { get; set; }
        public string TitleName { get; set; }
        public string MaritalStatusName { get; set; }
        public string NationalityName { get; set; }
        public string StrDOB { get; set; }
        public string OrderBy { get; set; }
        public string OrderByDirection { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string WhereCondition { get; set; }
        public string MainFilter { get; set; }

    }

    partial class Company
    {
        public int intStatus { get; set; }
        public string OldPassword { get; set; }
        public string TaxOfficeName { get; set; }
        public string TaxPayerTypeName { get; set; }
        public string EconomicActivitiesName { get; set; }
        public string NotificationMethodName { get; set; }
        public string ActiveText { get; set; }
        public string OrderBy { get; set; }
        public string OrderByDirection { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string WhereCondition { get; set; }
        public string MainFilter { get; set; }
    }

    partial class Government
    {
        public int intStatus { get; set; }
        public string OldPassword { get; set; }
        public string TaxOfficeName { get; set; }
        public string TaxPayerTypeName { get; set; }
        public string GovernmentTypeName { get; set; }
        public string NotificationMethodName { get; set; }
        public string ActiveText { get; set; }
        public string OrderBy { get; set; }
        public string OrderByDirection { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string WhereCondition { get; set; }
        public string MainFilter { get; set; }
    }

    partial class Special
    {
        public int intStatus { get; set; }
        public string TaxOfficeName { get; set; }
        public string TaxPayerTypeName { get; set; }
        public string NotificationMethodName { get; set; }
        public string ActiveText { get; set; }
        public string OrderBy { get; set; }
        public string OrderByDirection { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string WhereCondition { get; set; }
        public string MainFilter { get; set; }
    }

    partial class Vehicle
    {
        public int intStatus { get; set; }

        public string VehicleTypeName { get; set; }
        public string VehicleSubTypeName { get; set; }
        public string LGAName { get; set; }
        public string VehiclePurposeName { get; set; }
        public string VehicleFunctionName { get; set; }
        public string VehicleOwnershipName { get; set; }
        public string ActiveText { get; set; }

        public string OrderBy { get; set; }
        public string OrderByDirection { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string WhereCondition { get; set; }
        public string MainFilter { get; set; }
    }

    partial class Business
    {
        public int intStatus { get; set; }

        public string BusinessTypeName { get; set; }
        public string LGAName { get; set; }
        public string BusinessCategoryName { get; set; }
        public string BusinessSectorName { get; set; }
        public string BusinessSubSectorName { get; set; }
        public string BusinessStructureName { get; set; }
        public string BusinessOperationName { get; set; }
        public string SizeName { get; set; }
        public string ActiveText { get; set; }

        public string OrderBy { get; set; }
        public string OrderByDirection { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string WhereCondition { get; set; }
        public string MainFilter { get; set; }
    }

    partial class TaxPayer_Types
    {
        public int intStatus { get; set; }

        public string TaxPayerTypeIds { get; set; }

        public string IncludeTaxPayerTypeIds { get; set; }

        public string ExcludeTaxPayerTypeIds { get; set; }

    }

    partial class Land_Ownership
    {
        public int intStatus { get; set; }

        public string LandOwnershipIds { get; set; }

        public string IncludeLandOwnershipIds { get; set; }

        public string ExcludeLandOwnershipIds { get; set; }

    }

    partial class Land_Purpose
    {
        public int intStatus { get; set; }

        public string LandPurposeIds { get; set; }

        public string IncludeLandPurposeIds { get; set; }

        public string ExcludeLandPurposeIds { get; set; }

    }

    partial class Agency_Types
    {
        public int intStatus { get; set; }

        public string AgencyTypeIds { get; set; }

        public string IncludeAgencyTypeIds { get; set; }

        public string ExcludeAgencyTypeIds { get; set; }

    }

    partial class Payment_Options
    {
        public int intStatus { get; set; }

        public string PaymentOptionIds { get; set; }

        public string IncludePaymentOptionIds { get; set; }

        public string ExcludePaymentOptionIds { get; set; }

    }

    partial class Payment_Frequency
    {
        public int intStatus { get; set; }

        public string PaymentFrequencyIds { get; set; }

        public string IncludePaymentFrequencyIds { get; set; }

        public string ExcludePaymentFrequencyIds { get; set; }

    }

    partial class Land
    {
        public int intStatus { get; set; }

        public int TaxPayerID { get; set; }

        public int TaxPayerTypeID { get; set; }

        public string TownName { get; set; }
        public string LGAName { get; set; }
        public string WardName { get; set; }
        public string strLandSize_Length { get; set; }
        public string strLandSize_Width { get; set; }
        public string LandPurposeName { get; set; }
        public string LandFunctionName { get; set; }
        public string LandOwnershipName { get; set; }
        public string LandDevelopmentName { get; set; }
        public string LandStreetConditionName { get; set; }
        public string strValueOfLand { get; set; }
        public string ActiveText { get; set; }

        public string OrderBy { get; set; }
        public string OrderByDirection { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string WhereCondition { get; set; }
        public string MainFilter { get; set; }
    }

    partial class MAP_Building_Land
    {
        public int TaxPayerID { get; set; }

        public int TaxPayerTypeID { get; set; }
    }

    partial class Assessment_Items
    {
        public int intStatus { get; set; }
        public string AssetTypeName { get; set; }
        public string AssessmentGroupName { get; set; }
        public string AssessmentSubGroupName { get; set; }

        public string RevenueStreamName { get; set; }
        public string RevenueSubStreamName { get; set; }
        public string AssessmentItemCategoryName { get; set; }
        public string AssessmentItemSubCategoryName { get; set; }
        public string AgencyName { get; set; }
        public string ComputationName { get; set; }
        public string ActiveText { get; set; }
        public string StrTaxAmount { get; set; }
        public string StrPercentage { get; set; }
        public string StrTaxBaseAmount { get; set; }

        public string OrderBy { get; set; }
        public string OrderByDirection { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string WhereCondition { get; set; }
        public string MainFilter { get; set; }
    }

    partial class Profile
    {
        public int IntStatus { get; set; }

        public int IntSearchType { get; set; }

        public int TaxPayerTypeID { get; set; }

        public int TaxPayerID { get; set; }

        public int VehiclePurposeID { get; set; }

        public string BusinessSector { get; set; }

        public string BusinessCategory { get; set; }

        public string TaxPayerName { get; set; }


        public string ActiveText { get; set; }
        public string AssetTypeName { get; set; }
        public string ProfileSectorName { get; set; }
        public string ProfileSubSectorName { get; set; }
        public string ProfileSubGroupName { get; set; }
        public string ProfileGroupName { get; set; }
        public string ProfileSectorElementName { get; set; }
        public string ProfileSectorSubElementName { get; set; }
        public string ProfileAttributeName { get; set; }
        public string ProfileSubAttributeName { get; set; }
        public string TaxPayerRoleName { get; set; }
        public string TaxPayerTypeName { get; set; }

        public string OrderBy { get; set; }
        public string OrderByDirection { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string WhereCondition { get; set; }
        public string MainFilter { get; set; }

    }

    partial class Assessment_Rules
    {
        public int IntStatus { get; set; }
        public string ActiveText { get; set; }
        public string ProfileReferenceNo { get; set; }
        public string RuleRunName { get; set; }
        public string PaymentFrequencyName { get; set; }
        public string AssessmentItemName { get; set; }
        public string SettlementMethodName { get; set; }
        public string PaymentOptionName { get; set; }
        public string StrAssessmentAmount { get; set; }
        public string StrTaxYear { get; set; }

        public string OrderBy { get; set; }
        public string OrderByDirection { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string WhereCondition { get; set; }
        public string MainFilter { get; set; }
    }

    partial class Assessment
    {
        public int IntStatus { get; set; }

        public string OrderBy { get; set; }
        public string OrderByDirection { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string WhereCondition { get; set; }
        public string MainFilter { get; set; }

        public string TaxPayerTypeName { get; set; }
        public string TaxPayerRIN { get; set; }
        public string SettlementStatusName { get; set; }
        public string ActiveText { get; set; }
        public string strAssessmentDate { get; set; }
        public string strAssessmentAmount { get; set; }
        public string strSettlementDueDate { get; set; }
        public string TaxPayerName { get; set; }
        public string strSettlementDate { get; set; }
    }

    partial class MDA_Service_Items
    {
        public int intStatus { get; set; }

        public string RevenueStreamName { get; set; }
        public string RevenueSubStreamName { get; set; }
        public string AssessmentItemCategoryName { get; set; }
        public string AssessmentItemSubCategoryName { get; set; }
        public string AgencyName { get; set; }
        public string ComputationName { get; set; }
        public string ActiveText { get; set; }
        public string StrServiceAmount { get; set; }
        public string StrPercentage { get; set; }
        public string StrServiceBaseAmount { get; set; }

        public string OrderBy { get; set; }
        public string OrderByDirection { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string WhereCondition { get; set; }
        public string MainFilter { get; set; }
    }

    partial class MDA_Services
    {
        public int IntStatus { get; set; }
        public string ActiveText { get; set; }
        public string RuleRunName { get; set; }
        public string PaymentFrequencyName { get; set; }
        public string MDAServiceItemName { get; set; }
        public string SettlementMethodName { get; set; }
        public string PaymentOptionName { get; set; }
        public string StrServiceAmount { get; set; }
        public string StrTaxYear { get; set; }

        public string OrderBy { get; set; }
        public string OrderByDirection { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string WhereCondition { get; set; }
        public string MainFilter { get; set; }
    }

    partial class ServiceBill
    {
        public int IntStatus { get; set; }

        public string OrderBy { get; set; }
        public string OrderByDirection { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string WhereCondition { get; set; }
        public string MainFilter { get; set; }

        public string TaxPayerTypeName { get; set; }
        public string TaxPayerRIN { get; set; }
        public string SettlementStatusName { get; set; }
        public string ActiveText { get; set; }
        public string strServiceBillDate { get; set; }
        public string strServiceBillAmount { get; set; }
        public string strSettlementDueDate { get; set; }
        public string TaxPayerName { get; set; }
        public string strSettlementDate { get; set; }
    }

    partial class Size
    {
        public int intStatus { get; set; }

        public string SizeIds { get; set; }

        public string IncludeSizeIds { get; set; }

        public string ExcludeSizeIds { get; set; }

    }

    partial class Vehicle_Insurance
    {
        public int IntStatus { get; set; }
    }

    partial class Vehicle_Licenses
    {
        public int IntStatus { get; set; }
    }

    partial class Notification
    {
        public int IntStatus { get; set; }

        public string OrderBy { get; set; }
        public string OrderByDirection { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string WhereCondition { get; set; }
        public string MainFilter { get; set; }
        public string strNotificationDate { get; set; }
        public string strNotificationStatus { get; set; }
        public string NotificationMethodName { get; set; }
        public string NotificationTypeName { get; set; }
        public string TaxPayerTypeName { get; set; }
        public string TaxPayerName { get; set; }
        public string StatusName { get; set; }
    }

    partial class Land_Development
    {
        public int intStatus { get; set; }

        public string LandDevelopmentIds { get; set; }

        public string IncludeLandDevelopmentIds { get; set; }

        public string ExcludeLandDevelopmentIds { get; set; }

    }

    partial class Land_Function
    {
        public int intStatus { get; set; }

        public string LandFunctionIds { get; set; }

        public string IncludeLandFunctionIds { get; set; }

        public string ExcludeLandFunctionIds { get; set; }

    }

    partial class Land_StreetCondition
    {
        public int intStatus { get; set; }

        public string LandStreetConditionIds { get; set; }

        public string IncludeLandStreetConditionIds { get; set; }

        public string ExcludeLandStreetConditionIds { get; set; }

    }

    partial class Settlement
    {
        public int TaxPayerID { get; set; }
        public int TaxPayerTypeID { get; set; }

        public int? TransactionTypeID { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public string OrderBy { get; set; }
        public string OrderByDirection { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string WhereCondition { get; set; }
        public string MainFilter { get; set; }

        public string strSettlementDate { get; set; }
        public string BillRefNo { get; set; }
        public string BillAmount { get; set; }
        public string strSettlementAmount { get; set; }
        public string SettlementMethodName { get; set; }
        public string SettlementStatusName { get; set; }

        public bool ValidateDuplicateCheck { get; set; } = true;

    }

    partial class MST_Menu
    {
        public int intStatus { get; set; }
    }

    partial class MST_CentralMenu
    {
        public int intStatus { get; set; }
    }

    partial class MST_Screen
    {
        public int intStatus { get; set; }

        public int UserID { get; set; }

        public int CentralMenuID { get; set; }
    }

    partial class MST_Business
    {
        public int intClaimed { get; set; }

        public int intStatus { get; set; }
    }

    partial class MST_AwarenessCategory
    {
        public int intStatus { get; set; }

        public string AwarenessCategoryIds { get; set; }

        public string IncludeAwarenessCategoryIds { get; set; }

        public string ExcludeAwarenessCategoryIds { get; set; }
    }

    partial class MST_FAQ
    {
        public int intStatus { get; set; }

        public string FAQIds { get; set; }

        public string AwarenessCategoryIds { get; set; }

        public string IncludeFAQIds { get; set; }

        public string ExcludeFAQIds { get; set; }
    }

    partial class MST_Users
    {
        public int intStatus { get; set; }

        public string OldPassword { get; set; }


        public int ReplacementID { get; set; }

    }

    partial class MST_API
    {
        public int intStatus { get; set; }

        public int UserID { get; set; }
    }

    partial class MAP_Assessment_AssessmentRule
    {
        public int TaxPayerTypeID { get; set; }

        public int TaxPayerID { get; set; }
    }

    public partial class SearchTaxPayerFilter
    {
        public string TaxPayerRIN { get; set; }
        public string TaxPayerTIN { get; set; }
        public string MobileNumber { get; set; }
        public string AssetName { get; set; }
        public int intSearchType { get; set; }
        public string TaxPayerName { get; set; }
        public int TaxPayerTypeID { get; set; }
        public int TaxOfficeID { get; set; }
    }

    partial class Treasury_Receipt
    {
        public int TaxPayerID { get; set; }
        public int TaxPayerTypeID { get; set; }

        public int? TransactionTypeID { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public string OrderBy { get; set; }
        public string OrderByDirection { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string WhereCondition { get; set; }
        public string MainFilter { get; set; }

        public string StrReceiptDate { get; set; }
        public string BillRefNo { get; set; }
        public string ReceiptStatusName { get; set; }
        public string StrReceiptAmount { get; set; }

        public string SettlementIds { get; set; }

    }

    partial class TCCDetail
    {
        public string OrderBy { get; set; }
        public string OrderByDirection { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string WhereCondition { get; set; }
        public string MainFilter { get; set; }
        public string TaxPayerName { get; set; }
        public string TaxPayerRIN { get; set; }
        public string TIN { get; set; }
        public string strTaxYear { get; set; }
        public string strTCCTaxPaid { get; set; }
    }

    partial class TaxClearanceCertificate
    {
        public string OrderBy { get; set; }
        public string OrderByDirection { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string WhereCondition { get; set; }
        public string MainFilter { get; set; }
    }

    partial class MAP_TCCRequest_Generate
    {
        public int SEDE_DocumentID { get; set; }
        public int StageID { get; set; }
        public DateTime ApprovalDate { get; set; }
        public bool IsAction { get; set; }
        public int? PDFTemplateID { get; set; }

        public string GeneratedPath { get; set; }
    }

    partial class MAP_TCCRequest_Validate
    {
        public long SEDE_OrderID { get; set; }
        public int StageID { get; set; }
        public DateTime ApprovalDate { get; set; }
        public bool IsAction { get; set; }

        public string ValidatedPath { get; set; }

    }

    partial class MAP_TCCRequest_SignVisible
    {
        public int Request_StageID { get; set; }
        public int StatusID { get; set; }
        public DateTime ApprovalDate { get; set; }
        public bool IsAction { get; set; }
        public string SignedVisiblePath { get; set; }
    }

    partial class MAP_TCCRequest_SignDigital
    {
        public int StageID { get; set; }
        public DateTime ApprovalDate { get; set; }
        public bool IsAction { get; set; }

        public string SignedDigitalPath { get; set; }
    }

    //partial class MAP_TCCRequest_
    //{
    //    public int StageID { get; set; }
    //    public DateTime ApprovalDate { get; set; }
    //    public bool IsAction { get; set; }
    //}

    partial class MAP_TCCRequest_Seal
    {
        public int StageID { get; set; }
        public DateTime ApprovalDate { get; set; }
        public bool IsAction { get; set; }

        public string SealedPath { get; set; }
    }

    partial class MAP_TCCRequest_Issue
    {
        public int StageID { get; set; }
        public DateTime ApprovalDate { get; set; }
        public bool IsAction { get; set; }
    }

    partial class TCC_Request
    {
        public string OrderBy { get; set; }
        public string OrderByDirection { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string WhereCondition { get; set; }
        public string MainFilter { get; set; }
        public int TaxOfficeID { get; set; }

    }

    partial class MAP_TCCRequest_Stages
    {
        public int StatusID { get; set; }
    }

    partial class Certificate
    {
        public string OrderBy { get; set; }
        public string OrderByDirection { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string WhereCondition { get; set; }
        public string MainFilter { get; set; }
        public string StatusIds { get; set; }
    }

    partial class MAP_Certificate_Generate
    {
        public int SEDE_DocumentID { get; set; }
        public int StageID { get; set; }
        public DateTime ApprovalDate { get; set; }
        public bool IsAction { get; set; }
        public int? PDFTemplateID { get; set; }

        public string GeneratedPath { get; set; }
    }

    partial class MAP_Certificate_Validate
    {
        public long SEDE_OrderID { get; set; }
        public int StageID { get; set; }
        public DateTime ApprovalDate { get; set; }
        public bool IsAction { get; set; }

        public string ValidatedPath { get; set; }

    }

    partial class MAP_Certificate_SignVisible
    {
        public int Request_StageID { get; set; }
        public int StatusID { get; set; }
        public DateTime ApprovalDate { get; set; }
        public bool IsAction { get; set; }
        public string SignedVisiblePath { get; set; }
    }

    partial class MAP_Certificate_SignDigital
    {
        public int StageID { get; set; }
        public DateTime ApprovalDate { get; set; }
        public bool IsAction { get; set; }

        public string SignedDigitalPath { get; set; }
    }

    partial class MAP_Certificate_Seal
    {
        public int StageID { get; set; }
        public DateTime ApprovalDate { get; set; }
        public bool IsAction { get; set; }

        public string SealedPath { get; set; }
    }

    partial class MAP_Certificate_Issue
    {
        public int StageID { get; set; }
        public DateTime ApprovalDate { get; set; }
        public bool IsAction { get; set; }
    }

    partial class Audit_Log
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    partial class MAP_TaxPayer_Message
    {
        public int TaxPayerTypeID { get; set; }
        public int TaxPayerID { get; set; }
    }

    public partial class TCCReportSearchParams
    {
        public int TaxYear { get; set; }
        public int TaxMonth { get; set; }
        public int StageID { get; set; }
        public int StatusID { get; set; }

        public string SortBy { get; set; }
        public string SortDirection { get; set; }

        public int PageSize { get; set; }
        public int PageNumber { get; set; }

    }
}
