using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public class IndividualViewModelForTCCSTATUS
    {

        [Display(Name = "Mobile Number")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter mobile phone number of individual")]
        [RegularExpression(@"^[1-9][0-9]{9}$", ErrorMessage = "Looks like you entered invalid mobile number")]
        [MaxLength(10, ErrorMessage = "Only 10 numbers allowed.")]
        public string phonenumber { get; set; }
    }
    public class IndividualViewModel
    {
        public int IndividualID { get; set; }
        public string IndividualRIN { get; set; }


        [Display(Name = "Gender")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select individual’s gender")]
        public int GenderID { get; set; }

        public string GenderName { get; set; }

        [Display(Name = "Title")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select title")]
        public int TitleID { get; set; }

        public string TitleName { get; set; }

        [Display(Name = "First Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter individuals first name")]
        [MaxLength(250, ErrorMessage = "Only 250 characters allowed.")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter individuals surname name")]
        [MaxLength(250, ErrorMessage = "Only 250 characters allowed.")]
        public string LastName { get; set; }


        [Display(Name = "Middle Name")]
        [MaxLength(250, ErrorMessage = "Only 250 characters allowed.")]
        public string MiddleName { get; set; }


        [Display(Name = "Date of Birth")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Enter enter individuals date of birth")]
        public string DOB { get; set; }

        [Display(Name = "TIN")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Enter individuals TIN")]
        //[RegularExpression(@"^[1-9][0-9]{9}$", ErrorMessage = "Looks like you entered invalid number")]
        //[MaxLength(10, ErrorMessage = "Only 10 characters allowed.")]
        public string TIN { get; set; }
        [Display(Name = "NIN")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Enter individuals NIN")]
        //[RegularExpression(@"^[1-9][0-9]{10}$", ErrorMessage = "Looks like you entered invalid number")]
        //[MaxLength(11, ErrorMessage = "Only 11 characters allowed.")]
        public string NIN { get; set; }

        [Display(Name = "Mobile No 1")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter mobile phone number of individual")]
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
        //[EmailAddress(ErrorMessage = "Enter valid email address")]
        public string EmailAddress2 { get; set; }


        [Display(Name = "Biometric Details")]
        public string BiometricDetails { get; set; }

        [Display(Name = "Tax Office")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Tax Office")]
        public int? TaxOfficeID { get; set; }
        [Display(Name = "Present Tax Office")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Tax Office")]
        public int? PresentTaxOfficeID { get; set; }

        [Display(Name = "New Tax Office")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Tax Office")]
        public int? NewTaxOfficeID { get; set; }

        public string TaxOfficeName { get; set; }


        [Display(Name = "Marital Status")]
        public int? MaritalStatusID { get; set; }

        public string MaritalStatusName { get; set; }

        [Display(Name = "Nationality")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Nationality")]
        public int NationalityID { get; set; }

        public string NationalityName { get; set; }

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

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }

    public sealed class TPIndividualViewModel : IndividualViewModel
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
