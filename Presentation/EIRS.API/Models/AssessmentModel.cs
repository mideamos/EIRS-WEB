using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EIRS.API.Models
{
    public class AssessmentModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Tax Payer Type is Required")]
        public int TaxPayerTypeID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Tax Payer is Required")]
        public int TaxPayerID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Notes is Required")]
        public string Notes { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Asset Type is Required")]
        public int AssetTypeID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Asset is Required")]
        public int AssetID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Profile is Required")]
        public int ProfileID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Assessment Rule is Required")]
        public int AssessmentRuleID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Tax Year is Required")]
        public int TaxYear { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Assessment Items Required")]
        public IList<AssessmentItemModel> LstAssessmentItem { get; set; }
    }

    public class AssessmentItemModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Assessment Item is Required")]
        public int AssessmentItemID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Tax Base Amount is Required")]
        public decimal? TaxBaseAmount { get; set; }
    }

    public class AssessmentRuleModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Asset Type is Required")]
        public int AssetTypeID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Asset is Required")]
        public int AssetID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Profile is Required")]
        public int ProfileID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Assessment Rule is Required")]
        public int AssessmentRuleID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Tax Year is Required")]
        public int TaxYear { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Assessment Items Required")]
        public IList<AssessmentItemModel> LstAssessmentItem { get; set; }
    }

    public class AssessmentWithMultipleRuleModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Tax Payer Type is Required")]
        public int TaxPayerTypeID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Tax Payer is Required")]
        public int TaxPayerID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Notes is Required")]
        public string Notes { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Assessment Rules Required")]
        public IList<AssessmentRuleModel> LstAssessmentRule { get; set; }

    }
}