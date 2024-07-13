using EIRS.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace EIRS.Admin.Models
{
    public class PaymentAccountViewModel
    {
        public long PaymentAccountID { get; set; }

        [Display(Name = "Revenue Stream")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Revenue Stream")]
        public int RevenueStreamID { get; set; }

        [Display(Name = "Revenue Sub Stream")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Revenue Sub Stream")]
        public int RevenueSubStreamID { get; set; }

        
        [Display(Name = "Revenue Agency")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Revenue Agency")]
        public int AgencyID { get; set; }
    }
}