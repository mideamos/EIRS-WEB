using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public class SpecialViewModel
    {
        public int SpecialID { get; set; }
        public string SpecialRIN { get; set; }

        [Display(Name = "Special Tax Payer Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Special Tax Payer Name")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string SpecialName { get; set; }

        [Display(Name = "TIN")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string TIN { get; set; }

        [Display(Name = "Tax Office")]
        public int? TaxOfficeID { get; set; }

        public string TaxOfficeName { get; set; }

        [Display(Name = "Tax Payer Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Tax Payer Type")]
        public int TaxPayerTypeID { get; set; }

        public string TaxPayerTypeName { get; set; }

        [Display(Name = "Contact Number")]
        [Required(AllowEmptyStrings = false,ErrorMessage = "Enter Contact Number")]
        [RegularExpression(@"^[1-9][0-9]{9}$", ErrorMessage = "Looks like you entered invalid contact number")]
        [MaxLength(10, ErrorMessage = "Only 10 numbers allowed.")]
        public string ContactNumber { get; set; }

        [Display(Name = "Contact Email")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Enter valid contact email")]
        public string ContactEmail { get; set; }

        [Display(Name = "Contact Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Contact Name")]
        public string ContactName { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Preferred Notification")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Preferred Notification")]
        public int NotificationMethodID { get; set; }

        public string NotificationMethodName { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }

    public sealed class TPSpecialViewModel : SpecialViewModel
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
