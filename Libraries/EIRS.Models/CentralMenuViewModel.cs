using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIRS.Models
{
   public class CentralMenuViewModel
    {
        public int CentralMenuID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter Central Menu Name")]
        [Display(Name = "Central Menu Name")]
        public string CentralMenuName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Select Parent Central Menu")]
        [Display(Name = "Parent Central Menu")]
        public int ParentCentralMenuID { get; set; }

        [Display(Name = "Parent Central Menu Name")]
        public string ParentCentralMenuName { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter Sort Order")]
        [Display(Name = "Sort Order")]
        public decimal SortOrder { get; set; }
      
    }
}
