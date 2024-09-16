using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class DirectorateViewModel
    {
        public int DirectorateID { get; set; }

        [Display(Name = "Directorate")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Directorate")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string DirectorateName { get; set; }

        [Display(Name = "Revenue Stream")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Revenue Stream")]
        public int[] RevenueStreamIds { get; set; }

        public string RevenueStreamNames { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
