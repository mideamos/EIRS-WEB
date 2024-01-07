using System;
using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
   public class PAYEInputViewModel
    {
        public int PAYEInputID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Transaction Date")]
        [Display(Name = "Transcation Date")]
        public DateTime TranscationDate { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Employer RIN")]
        [Display(Name = "Employer_RIN")]
        public string Employer_RIN { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Employee RIN")]
        [Display(Name = "Employee_RIN")]
        public string Employee_RIN { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Assessment Year")]
        [Display(Name = "Assessment_Year")]
        public int Assessment_Year { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Start Month")]
        [Display(Name = "Start_Month")]
        public int Start_Month { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter End Month")]
        [Display(Name = "End_Month")]
        public int End_Month { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Annual Basic")]
        [Display(Name = "Annual_Basic")]
        public decimal Annual_Basic { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Annual Rent")]
        [Display(Name = "Annual_Rent")]
        public decimal Annual_Rent { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Annual Transport")]
        [Display(Name = "Annual_Transport")]
        public decimal Annual_Transport { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Annual Utility")]
        [Display(Name = "Annual_Utility")]
        public decimal Annual_Utility { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Annual Meal")]
        [Display(Name = "Annual_Meal")]
        public decimal Annual_Meal { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Other Allowances Annual")]
        [Display(Name = "Other_Allowances_Annual")]
        public decimal Other_Allowances_Annual { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Leave Transport Grant Annual")]
        [Display(Name = "Leave_Transport_Grant_Annual")]
        public decimal Leave_Transport_Grant_Annual { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter pension contribution declared")]
        [Display(Name = "pension_contribution_declared")]
        public decimal pension_contribution_declared { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter nhf contribution declared")]
        [Display(Name = "nhf_contribution_declared")]
        public decimal nhf_contribution_declared { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter nhis contribution declared")]
        [Display(Name = "nhis_contribution_declared")]
        public decimal nhis_contribution_declared { get; set; }

        [Required(ErrorMessage = "Tax Office is Required")]
        public string Tax_Office { get; set; }
    }
}
