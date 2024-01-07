using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EIRS.Web.Models
{
    public class NewAssessmentViewModel
    {
        public long AssessmentID { get; set; }

        public int TaxPayerTypeID { get; set; }

        public int TaxPayerID { get; set; }
        public decimal TaxAmount { get; set; }

        public string TaxPayerRIN { get; set; }

        public string TaxPayerName { get; set; }

        public string TaxPayerAddress { get; set; }

        public DateTime? AssessmentDate { get; set; }

        [Display(Name = "Settlement Due Date")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Settlement Due Date")]
        public DateTime SettlementDuedate { get; set; }

        [Display(Name = "Assessment Notes")]
        public string Notes { get; set; }

    }
}