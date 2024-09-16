using Foolproof;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EIRS.Web.Models
{
    public class ReplaceTaxOfficeManagerViewModel
    {
        [Display(Name = "Tax Office")]
        [Required(ErrorMessage = "Select Tax Office")]
        public int TaxOfficeID { get; set; }

        [Display(Name = "Tax Office Manager")]
        [Required(ErrorMessage = "Select Tax Office Manager")]
        public int TaxOfficeManagerID { get; set; }


        [Display(Name = "Replacement Manager")]
        [Required(ErrorMessage = "Select Replacement Manager")]
        [NotEqualTo("TaxOfficeManagerID", ErrorMessage = "Tax Office Manager & Replacement are same")]
        public int ReplacementManagerID { get; set; }
    }

    public class ReplaceTaxOfficerViewModel
    {
        [Display(Name = "Tax Office")]
        [Required(ErrorMessage = "Select Tax Office")]
        public int TaxOfficeID { get; set; }

        [Display(Name = "Tax Officer")]
        [Required(ErrorMessage = "Select Tax Officer")]
        public int TaxOfficerID { get; set; }


        [Display(Name = "Replacement")]
        [Required(ErrorMessage = "Select Replacement")]
        [NotEqualTo("TaxOfficerID", ErrorMessage = "Tax Officer & Replacement are same")]
        public int ReplacementID { get; set; }
    }

    public class ReallocateTaxPayerToTaxOfficerViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Tax Payer Type")]
        [Display(Name = "Tax Payer Type")]
        public int TaxPayerTypeID { get; set; }
        public string TaxPayerTypeName { get; set; }

        [Display(Name = "Tax Office")]
        [Required(ErrorMessage = "Select Tax Office")]
        public int TaxOfficeID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Tax Payer")]
        [Display(Name = "Tax Payer")]
        public int TaxPayerID { get; set; }
        public string TaxPayerName { get; set; }

        public string TaxOfficerName { get; set; }

        [Display(Name = "Tax Officer")]
        [Required(ErrorMessage = "Select Tax Officer")]
        public int TaxOfficerID { get; set; }

    }
}