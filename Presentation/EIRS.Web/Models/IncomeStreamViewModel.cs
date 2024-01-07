using System.ComponentModel.DataAnnotations;

namespace EIRS.Web.Models
{
    public class IncomeStreamViewModel
    {
        public int TaxPayerID { get; set; }
        public int TaxPayerTypeID { get; set; }
        public string TaxPayerTypeName { get; set; }
        public string TaxPayerName { get; set; }
        public string TaxPayerTIN { get; set; }
        public string TaxPayerRIN { get; set; }
        public string MobileNumber { get; set; }
        public string ContactAddress { get; set; }

        [Display(Name = "Tax Year")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Tax Year")]
        public int TaxYear { get; set; }

        [Display(Name = "Total Income Earned")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Total Income Earned")]
        public decimal TotalIncomeEarned { get; set; }

        [Display(Name = "Tax Payer Role")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Tax Payer Role")]
        public int TaxPayerRoleID { get; set; }

        [Display(Name = "Business Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Business Name")]
        [MaxLength(250, ErrorMessage = "Only 250 characters allowed.")]
        public string BusinessName { get; set; }

        [Display(Name = "Business Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select the type of Business")]
        public int BusinessTypeID { get; set; }

        [Display(Name = "Business LGA")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select business lga")]
        public int LGAID { get; set; }

        [Display(Name = "Business Operations")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Business Operations")]
        public int BusinessOperationID { get; set; }

        [Display(Name = "Business Number")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Business Number")]
        [RegularExpression(@"^[1-9][0-9]{9}$", ErrorMessage = "Looks like you entered invalid business number")]
        public string BusinessNumber { get; set; }


        [Display(Name = "Business Address")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Business Address")]
        public string BusinessAddress { get; set; }

        [Display(Name = "Contact Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Contact Name")]
        public string ContactName { get; set; }
    }

    public class RequestIncomeStreamViewModel
    {
        public int TaxPayerID { get; set; }
        public int TaxPayerTypeID { get; set; }

        public int RowID { get; set; }

        [Display(Name = "Tax Year")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Tax Year")]
        public int TaxYear { get; set; }

        [Display(Name = "Total Income Earned")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Total Income Earned")]
        public decimal TotalIncomeEarned { get; set; }

        [Display(Name = "Tax Payer Role")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Tax Payer Role")]
        public int TaxPayerRoleID { get; set; }

        [Display(Name = "Asset")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Asset")]
        public int BusinessID { get; set; }

        public string Notes { get; set; }
    }  
    public class RequestPayeIncomeStreamViewModel
    {
        public int RowID { get; set; }
        public int TaxYear { get; set; }
        public decimal TotalIncomeEarned { get; set; }
        public decimal payeTaxPaid { get; set; }
        public decimal payeAssessedIncome { get; set; }
        public string ReceiptReference{ get; set; }
        public string ReceiptDate { get; set; }
    }

    public class RequestTCCViewModel
    {
        public int RowID { get; set; }

        [Display(Name = "TCC Tax Paid")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter TCC Tax Paid")]
        public decimal TCCTaxPaid { get; set; }
        
        [Display(Name = "ERAS Tax Paid")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter ERAS Tax Paid")]
        public decimal ERASTaxPaid { get; set; }
    }
}