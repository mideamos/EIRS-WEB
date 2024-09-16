using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIRS.API.Models
{
    public class GetTaxpayerViewModel
    {
        public int? TaxPayerID { get; set; }
        public int? TaxPayerTypeID { get; set; }
        public string TaxPayerTypeName { get; set; }
        public string TaxPayerName { get; set; }
        public string TaxPayerRIN { get; set; }
        public string MobileNumber { get; set; }
        public string ContactAddress { get; set; }
        public string EmailAddress { get; set; }
        public string TIN { get; set; }
        public string TaxOffice { get; set; }

        public int ApiId { get; set; }
    }

    public class GetProfileViewModel
    {
        public int ApiId { get; set; }
        public int? TaxPayerID { get; set; }
        public int? TaxPayerTypeID { get; set; }
        public string TaxPayerTypeName { get; set; }
        public string TaxPayerRIN { get; set; }
        public int? AssetID { get; set; }
        public int? AssetTypeID { get; set; }
        public string AssetTypeName { get; set; }
        public string AssetRIN { get; set; }
        public int ProfileID { get; set; }
        public string ProfileReferenceNo { get; set; }
        public string ProfileDescription { get; set; }
        public int TaxPayerRoleID { get; set; }
        public string TaxPayerRoleName { get; set; }
    }

    public class GetBusinessViewModel
    {
        public int? BusinessSubSectorID { get; set; }
        public string BusinessSubSectorName { get; set; }
        public int? BusinessStructureID { get; set; }
        public string BusinessStructureName { get; set; }
        public int? BusinessOperationID { get; set; }
        public string BusinessOperationName { get; set; }
        public int? SizeID { get; set; }
        public string SizeName { get; set; }
        public string ContactName { get; set; }
        public string BusinessNumber { get; set; }
        public string BusinessAddress { get; set; }
        public int? TaxPayerID { get; set; }
        public int? TaxPayerTypeID { get; set; }
        public string TaxPayerTypeName { get; set; }
        public string TaxPayerName { get; set; }
        public string TaxPayerRIN { get; set; }
        public int BusinessID { get; set; }
        public int? AssetTypeID { get; set; }
        public string AssetTypeName { get; set; }
        public int? BusinessTypeID { get; set; }
        public string BusinessTypeName { get; set; }
        public string BusinessRIN { get; set; }
        public string BusinessName { get; set; }
        public int? LGAID { get; set; }
        public string LGAName { get; set; }
        public int? BusinessCategoryID { get; set; }
        public string BusinessCategoryName { get; set; }
        public int? BusinessSectorID { get; set; }
        public string BusinessSectorName { get; set; }


        public int ApiId { get; set; }
    }

    public class GetAssestItemViewModel
    {
        public int ApiId { get; set; }
        public int? TaxPayerID { get; set; }
        public int? TaxPayerTypeID { get; set; }
        public string TaxPayerTypeName { get; set; }
        public string TaxPayerRIN { get; set; }
        public int? AssetID { get; set; }
        public int? AssetTypeID { get; set; }
        public string AssetTypeName { get; set; }
        public string AssetRIN { get; set; }
        public int ProfileID { get; set; }
        public string ProfileReferenceNo { get; set; }
        public string ProfileDescription { get; set; }
        public int AssessmentRuleID { get; set; }
        public string AssessmentRuleCode { get; set; }
        public string AssessmentRuleName { get; set; }
        public int AssessmentItemID { get; set; }
        public string AssessmentItemReferenceNo { get; set; }
        public int? AssessmentGroupID { get; set; }
        public string AssessmentGroupName { get; set; }
        public int? AssessmentSubGroupID { get; set; }
        public string AssessmentSubGroupName { get; set; }
        public int? RevenueStreamID { get; set; }
        public string RevenueStreamName { get; set; }
        public int? RevenueSubStreamID { get; set; }
        public string RevenueSubStreamName { get; set; }
        public int? AssessmentItemCategoryID { get; set; }
        public string AssessmentItemCategoryName { get; set; }
        public int? AssessmentItemSubCategoryID { get; set; }
        public string AssessmentItemSubCategoryName { get; set; }
        public int? AgencyID { get; set; }
        public string AgencyName { get; set; }
        public string AssessmentItemName { get; set; }
        public int? ComputationID { get; set; }
        public string ComputationName { get; set; }
        public decimal? TaxBaseAmount { get; set; }
        public decimal? Percentage { get; set; }
        public decimal? TaxAmount { get; set; }

    }

    public class GetAssessmentRuleViewModel
    {
        public int ApiId { get; set; }
        public int? TaxPayerID { get; set; }
        public int? TaxPayerTypeID { get; set; }
        public string TaxPayerTypeName { get; set; }
        public string TaxPayerRIN { get; set; }
        public int? AssetID { get; set; }
        public int? AssetTypeID { get; set; }
        public string AssetTypeName { get; set; }
        public string AssetRIN { get; set; }
        public int ProfileID { get; set; }
        public string ProfileReferenceNo { get; set; }
        public string ProfileDescription { get; set; }
        public int AssessmentRuleID { get; set; }
        public string AssessmentRuleCode { get; set; }
        public string AssessmentRuleName { get; set; }
        public int? RuleRunID { get; set; }
        public string RuleRunName { get; set; }
        public int? PaymentFrequencyID { get; set; }
        public string PaymentFrequencyName { get; set; }
        public decimal? AssessmentAmount { get; set; }
        public int? TaxYear { get; set; }
        public int? PaymentOptionID { get; set; }
        public string PaymentOptionName { get; set; }
        public int? TaxMonth { get; set; }

    }
}