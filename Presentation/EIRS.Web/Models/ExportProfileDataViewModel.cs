using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EIRS.Web.Models
{
    public class ExportProfileDataViewModel
    {
        [Display(Name="Profile")]
        [Required(ErrorMessage ="Select Profile")]
        public int[] ProfileID { get; set; }

        [Display(Name = "Year")]
        [Required(ErrorMessage = "Select Year")]
        public int Year { get; set; }

    }

    public class ExportProfileGroupDataViewModel
    {
        [Display(Name = "Profile")]
        [Required(ErrorMessage = "Select Profile")]
        public int ProfileTypeID { get; set; }

        [Display(Name = "Year")]
        [Required(ErrorMessage = "Select Year")]
        public int Year { get; set; }

    }

    public class TaxPayerCaptureAnalysisViewModel
    {
        [Display(Name = "From Date")]
        [Required(ErrorMessage = "Enter From Date")]
        public DateTime FromDate { get; set; }

        [Display(Name = "To Date")]
        [Required(ErrorMessage = "Enter To Date")]
        public DateTime ToDate { get; set; }

        [Display(Name = "Tax Payer Type")]
        [Required(ErrorMessage = "Select Tax Payer Type")]
        public int TaxPayerTypeID { get; set; }

        [Display(Name = "Tax Office")]
        public int? TaxOfficeID { get; set; }
    }


}