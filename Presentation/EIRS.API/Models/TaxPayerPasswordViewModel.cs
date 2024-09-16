using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EIRS.API.Models
{
    public class TaxPayerPasswordViewModel
    {
        public int TaxPayerID { get; set; }

        [Required(ErrorMessage = "Please Enter New Password")]
        [Display(Name = "Enter New Password")]
        public string Password { get; set; }
    }
}