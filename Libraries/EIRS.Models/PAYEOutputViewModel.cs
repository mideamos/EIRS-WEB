using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIRS.Models
{
  public  class PAYEOutputViewModel
    {
        public int PAYEOutputID { get; set; }

        [Required(ErrorMessage = "Transaction Date is Required")]
        public DateTime Transaction_Date { get; set; }
        [Required(ErrorMessage = "Employee Rin  is Required")]
        public string Employee_Rin { get; set; }
        [Required(ErrorMessage = "Employer Rin is Required")]
        public string Employer_Rin { get; set; }
        [Required(ErrorMessage = "Assessment Year is Required")]
        public int AssessmentYear { get; set; }
        [Required(ErrorMessage = "Assessment Month is Required")]
        public int Assessment_Month { get; set; }
        [Required(ErrorMessage = "Monthly CRA is Required")]
        public decimal Monthly_CRA { get; set; }
        [Required(ErrorMessage = "Monthly Gross is Required")]
        public decimal Monthly_Gross { get; set; }
        [Required(ErrorMessage = "Monthly Validated NHF is Required")]
        public decimal Monthly_ValidatedNHF { get; set; }
        [Required(ErrorMessage = "Monthly Validated NHIS is Required")]
        public decimal Monthly_ValidatedNHIS { get; set; }
        [Required(ErrorMessage = "Monthly Validated Pension is Required")]
        public decimal Monthly_ValidatedPension { get; set; }
        [Required(ErrorMessage = "Monthly TaxFreePay is Required")]
        public decimal Monthly_TaxFreePay { get; set; }
        [Required(ErrorMessage = "Monthly Chargeable Income is Required")]
        public decimal Monthly_ChargeableIncome { get; set; }
        [Required(ErrorMessage = "Monthly Tax is Required")]
        public decimal Monthly_Tax { get; set; }

        [Required(ErrorMessage = "Tax Office is Required")]
        public string Tax_Office { get; set; }




    }
}
