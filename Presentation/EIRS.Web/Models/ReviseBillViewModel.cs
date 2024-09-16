using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EIRS.Web.Models
{
    public class ReviseBillViewModel
    {
        [Display(Name = "Bill Ref No")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Bill Ref No")]
        public string BillRefNo { get; set; }
    }
}