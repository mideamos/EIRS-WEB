using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class ProfileViewModel
    {
        public int ProfileID { get; set; }

        [Display(Name = "Profile Reference No")]
        public string ProfileReferenceNo { get; set; }

        [Display(Name ="Profile Description")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Profile Description")]
        [MaxLength(250, ErrorMessage = "Only 250 characters allowed.")]
        public string ProfileDescription { get; set; }

        [Display(Name="Asset Type")]
        [Required(AllowEmptyStrings =false, ErrorMessage = "Select Asset Type")]
        public int AssetTypeID { get; set; }

        public string  AssetTypeName { get; set; }

        [Display(Name = "Tax Payer Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Tax Payer Type")]
        public int[] TaxPayerTypeIds { get; set; }

        public string TaxPayerTypeNames { get; set; }

        [Display(Name = "Tax Payer Role")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Tax Payer Role")]
        public int[] TaxPayerRoleIds { get; set; }

        public string TaxPayerRoleNames { get; set; }


        [Display(Name = "Profile Sector")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Profile Sector")]
        public int[] ProfileSectorIds { get; set; }

        public string ProfileSectorNames { get; set; }

        [Display(Name = "Profile Sub Sector")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Profile Sub Sector")]
        public int[] ProfileSubSectorIds { get; set; }

        public string ProfileSubSectorNames { get; set; }

        [Display(Name = "Profile Group")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Profile Group")]
        public int[] ProfileGroupIds { get; set; }

        public string ProfileGroupNames { get; set; }

        [Display(Name = "Profile Sub Group")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Profile Sub Group")]
        public int[] ProfileSubGroupIds { get; set; }

        public string ProfileSubGroupNames { get; set; }


        [Display(Name = "Sector Element")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Sector Element")]
        public int[] ProfileSectorElementIds { get; set; }

        public string ProfileSectorElementNames { get; set; }

        [Display(Name = "Sector Sub Element")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Sector Sub Element")]
        public int[] ProfileSectorSubElementIds { get; set; }

        public string ProfileSectorSubElementNames { get; set; }


        [Display(Name = "Profile Attribute")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Profile Attribute")]
        public int[] ProfileAttributeIds { get; set; }

        public string ProfileAttributeNames { get; set; }

        [Display(Name = "Profile Sub Attribute")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Profile Sub Attribute")]
        public int[] ProfileSubAttributeIds { get; set; }

        public string ProfileSubAttributeNames { get; set; }


        [Display(Name = "Asset Status")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Asset Status")]
        public int AssetTypeStatus { get; set; }

        public string AssetTypeStatusName { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
