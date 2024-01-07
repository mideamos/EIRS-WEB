using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public class CompanyViewModel
    {
        public int CompanyID { get; set; }
        public string CompanyRIN { get; set; }

        [Display(Name = "Company Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Company name")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string CompanyName { get; set; }

        [Display(Name = "TIN")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Enter TIN")]
        //[RegularExpression(@"^[1-9][0-9]{10}$", ErrorMessage = "Looks like you entered invalid number")]
        //[MaxLength(10, ErrorMessage = "Only 10 characters allowed.")]

        public string TIN { get; set; }

        [Display(Name = "Mobile No 1")]
        [Required(AllowEmptyStrings = false,ErrorMessage = "Enter mobile phone number of Company")]
        [RegularExpression(@"^[1-9][0-9]{9}$", ErrorMessage = "Looks like you entered invalid mobile number")]
        [MaxLength(10, ErrorMessage = "Only 10 numbers allowed.")]
        public string MobileNumber1 { get; set; }

        [Display(Name = "Mobile No 2")]
        [RegularExpression(@"^[1-9][0-9]{9}$", ErrorMessage = "Looks like you entered invalid mobile number")]
        [MaxLength(10, ErrorMessage = "Only 10 numbers allowed.")]
        public string MobileNumber2 { get; set; }

        [Display(Name = "Email Address 1")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Enter valid email address")]
        public string EmailAddress1 { get; set; }

        [Display(Name = "Email Address 2")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Enter valid email address")]
        public string EmailAddress2 { get; set; }

        [Display(Name = "Tax Office")]
        public int? TaxOfficeID { get; set; }

        public string TaxOfficeName { get; set; }

        [Display(Name = "Tax Payer Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Tax Payer Type")]
        public int TaxPayerTypeID { get; set; }

        public string TaxPayerTypeName { get; set; }

        [Display(Name = "Economic Activity")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Economic Activity")]
        public int EconomicActivitiesID { get; set; }

        public string EconomicActivitiesName { get; set; }

        [Display(Name = "Preferred Notification")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Preferred Notification")]
        public int NotificationMethodID { get; set; }

        public string NotificationMethodName { get; set; }

        [Display(Name = "Contact Address")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Contact Address")]
        public string ContactAddress { get; set; }

        //[Display(Name = "CAC Registration Number")]
        //[RegularExpression(@"^[1-9][0-9]{14}$", ErrorMessage = "Looks like you entered invalid number")]
        //[MaxLength(14, ErrorMessage = "Only 11 characters allowed.")]
        public string CACRegistrationNumber { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }

    public sealed class TPCompanyViewModel : CompanyViewModel
    {
        [Display(Name = "Tax Payer Role")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Tax Payer Role")]
        public int TaxPayerRoleID { get; set; }

        public int BuildingUnitID { get; set; }

        public int AssetID { get; set; }
        public int AssetTypeID { get; set; }
        public string AssetTypeName { get; set; }
        public string AssetName { get; set; }
        public string AssetRIN { get; set; }
        public string AssetLGAName { get; set; }
    }
}
