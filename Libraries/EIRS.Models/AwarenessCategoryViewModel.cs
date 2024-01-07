using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public class AwarenessCategoryViewModel
    {
        public int AwarenessCategoryID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter Awareness Category Name")]
        [Display(Name = "Awareness Category Name")]
        public string AwarenessCategoryName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter Section Description")]
        [Display(Name = "Section Description")]
        public string SectionDescription { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

       
        public string ActiveText { get; set; }
    }
}
