using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EIRS.Web.Models
{
    public class ClaimBusinessViewModel
    {
        public long BusinessID { get; set; }

        [Display(Name="Business Name")]
        public string BusinessName { get; set; }

        [Display(Name ="Tax Payer Number")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter phone number ")]
        [RegularExpression(@"^[1-9][0-9]{9}$", ErrorMessage = "Looks like you entered invalid phone number")]
        [MaxLength(10, ErrorMessage = "Only 10 numbers allowed.")]
        public string MobileNumber { get; set; }

        [Display(Name = "Contact Person Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Contact Person Name")]
        public string ContactPersonName { get; set; }
    }
}