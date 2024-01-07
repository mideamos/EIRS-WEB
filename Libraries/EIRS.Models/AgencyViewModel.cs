using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class AgencyViewModel
    {
        public int AgencyID { get; set; }

        [Display(Name = "Agency")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Agency Name")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string AgencyName { get; set; }

        [Display(Name = "Agency Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Agency Type")]
        public int AgencyTypeID { get; set; }

        public string AgencyTypeName { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
