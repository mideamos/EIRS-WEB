using EIRS.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace EIRS.Admin.Models
{
    public sealed class AssessmentItemViewModel
    {
        public int AssessmentItemID { get; set; }

        public string AssessmentRefNo { get; set; }

        [Display(Name = "Asset Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Asset Type")]
        public int AssetTypeID { get; set; }

        public string AssetTypeName { get; set; }

        [Display(Name = "Assessment Group")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Assessment Group")]
        public int AssessmentGroupID { get; set; }

        public string AssessmentGroupName { get; set; }

        [Display(Name = "Assessment Sub Group")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Assessment Sub Group")]
        public int AssessmentSubGroupID { get; set; }

        public string AssessmentSubGroupName { get; set; }

        [Display(Name = "Revenue Stream")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Revenue Stream")]
        public int RevenueStreamID { get; set; }

        public string RevenueStreamName { get; set; }

        [Display(Name = "Revenue Sub Stream")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Revenue Sub Stream")]
        public int RevenueSubStreamID { get; set; }

        public string RevenueSubStreamName { get; set; }

        [Display(Name = "Assessment Item Category")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Assessment Item Category")]
        public int AssessmentItemCategoryID { get; set; }

        public string AssessmentItemCategoryName { get; set; }

        [Display(Name = "Assessment Item Sub Category")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Assessment Item Sub Category")]
        public int AssessmentItemSubCategoryID { get; set; }

        public string AssessmentItemSubCategoryName { get; set; }

        [Display(Name = "Revenue Agency")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Revenue Agency")]
        public int AgencyID { get; set; }

        public string AgencyName { get; set; }

        [Display(Name = "Assessment Item Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Assessment Item Name")]
        [MaxLength(100, ErrorMessage = "Only 100 characters allowed.")]
        public string AssessmentItemName { get; set; }

        [Display(Name = "Computation")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Computation")]
        public int ComputationID { get; set; }

        public string ComputationName { get; set; }

        [Display(Name = "Tax Base Amount")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Tax Base Amount")]
        public decimal TaxBaseAmount { get; set; }

        [ComputationValidator("ComputationID", ErrorMessage = "Please Enter Percentage")]
        [Display(Name = "Percentage (%)")]
        public decimal? Percentage { get; set; }


        [Display(Name = "Tax Amount")]
        public decimal? TaxAmount { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }

    public sealed class ComputationValidator : RequiredAttribute, IClientValidatable
    {
        private string PropertyName { get; set; }

        public ComputationValidator(string propertyName)
        {
            PropertyName = propertyName;
        }


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Object instance = validationContext.ObjectInstance;
            Type type = instance.GetType();
            int ComputationID = TrynParse.parseInt(type.GetProperty(PropertyName).GetValue(instance, null));

            if (ComputationID == 2 && (TrynParse.parseDecimal(value) <= 0 || TrynParse.parseDecimal(value) >= 100))
            {
                return new ValidationResult("Enter Correct Value");
            }

            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule mvr = new ModelClientValidationRule();
            mvr.ErrorMessage = "Enter Correct Value";
            mvr.ValidationType = "computationvalidator";
            return new[] { mvr };
        }
    }
}
