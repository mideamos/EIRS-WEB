using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIRS.Models
{
   public class FAQViewModel
    {
        public int FAQID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter FAQ Question")]
        [Display(Name = "FAQ Question")]
        public string FAQTitle { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Select Awareness Category")]
        [Display(Name = "AwarenessCategory")]
        public int AwarenessCategoryID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter FAQ Answer")]
        [Display(Name = "FAQ Answer")]
        public string FAQText { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }

        public string AwarenessCategoryName { get; set; }
    }
}
