using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EIRS.Models
{
    public class TaxPayerPanelLoginViewModel
    {
        public string returnUrl { get; set; }

        [Required(ErrorMessage = "Please enter RIN")]
        public string RIN { get; set; }

        [Required(ErrorMessage = "Enter your password")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}