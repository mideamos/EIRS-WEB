using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class ZoneViewModel
    {
        public int ZoneID { get; set; }

        [Display(Name = "Zone Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Zone Name")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string ZoneName { get; set; }

        [Display(Name = "Local Government Area Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Local Government Area Name")]
        public int LGAID { get; set; }

        public string LGAName { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }   
    
    public sealed class WardViewModel
    {
        public int WardID { get; set; }

        [Display(Name = "Ward Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Ward Name")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string WardName { get; set; }

        [Display(Name = "Local Government Area Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Local Government Area Name")]
        public int LGAID { get; set; }

        public string LGAName { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
