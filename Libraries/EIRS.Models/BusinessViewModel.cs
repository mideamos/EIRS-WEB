using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public class BusinessViewModel
    {
        public int BusinessID { get; set; }
        public string BusinessRIN { get; set; }

        [Display(Name = "Asset Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Asset Type")]
        public int AssetTypeID { get; set; }
        public string AssetTypeName { get; set; }


        [Display(Name = "Business Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select the type of Business")]
        public int BusinessTypeID { get; set; }

        public string BusinessTypeName { get; set; }


        [Display(Name = "Business Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Business Name")]
        [MaxLength(250, ErrorMessage = "Only 250 characters allowed.")]
        public string BusinessName { get; set; }

        [Display(Name = "Business LGA")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select business lga")]
        public int LGAID { get; set; }

        public string LGAName { get; set; }
        [Display(Name = "Zone")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Zone")]
        public int ZoneId { get; set; }
        public string ZoneName { get; set; }

        [Display(Name = "TaxOffice")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select TaxOffice")]
        public int TaxOfficeId { get; set; }

        public string TaxOfficeName { get; set; }


        [Display(Name = "Business Category")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Business Category")]
        public int BusinessCategoryID { get; set; }

        public string BusinessCategoryName { get; set; }

        [Display(Name = "Business Sector")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Business Sector")]
        public int BusinessSectorID { get; set; }

        public string BusinessSectorName { get; set; }

        [Display(Name = "Business Sub Sector")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Business Sub Sector")]
        public int BusinessSubSectorID { get; set; }

        public string BusinessSubSectorName { get; set; }

        [Display(Name = "Business Structure")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Business Structure")]
        public int BusinessStructureID { get; set; }

        public string BusinessStructureName { get; set; }

        [Display(Name = "Business Operations")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Business Operations")]
        public int BusinessOperationID { get; set; }

        public string BusinessOperationName { get; set; }

        [Display(Name = "Premises Size")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Premises Size")]
        public int SizeID { get; set; }

        public string SizeName { get; set; }


        [Display(Name = "Contact Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Contact Name")]
        public string ContactName { get; set; }


        [Display(Name = "Business Number")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Business Number")]
        [RegularExpression(@"^[1-9][0-9]{9}$", ErrorMessage = "Looks like you entered invalid business number")]
        public string BusinessNumber { get; set; }


        [Display(Name = "Business Address")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Business Address")]
        public string BusinessAddress { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }

    public sealed class TPBusinessViewModel : BusinessViewModel
    {
        [Display(Name = "Tax Payer Role")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Tax Payer Role")]
        public int TaxPayerRoleID { get; set; }

        public long TPAID { get; set; }

        public int TaxPayerID { get; set; }
        public int TaxPayerTypeID { get; set; }
        public string TaxPayerTypeName { get; set; }
        public string TaxPayerName { get; set; }
        public string TaxPayerTIN { get; set; }
        public string TaxPayerRIN { get; set; }
        public string MobileNumber { get; set; }
        public string ContactAddress { get; set; }
    }
}
