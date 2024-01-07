using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class RevenueSubStreamViewModel
    {
        public int RevenueSubStreamID { get; set; }

        [Display(Name = "Revenue Sub Stream")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Revenue Sub Stream")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string RevenueSubStreamName { get; set; }


        [Display(Name = "Revenue Stream")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Revenue Stream")]
        public int RevenueStreamID { get; set; }

        public string RevenueStreamName { get; set; }

        //[Display(Name = "Asset Type")]
        //public int? AssetTypeID { get; set; }

        //public string AssetTypeName { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
