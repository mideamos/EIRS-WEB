using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIRS.Models
{
   public class DAOutputViewModel
    {

        public int DAOutputID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Transaction Date")]
        [Display(Name = "Transcation Date")]
        public DateTime transaction_date { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Taxpayer RIN")]
        [Display(Name = "Taxpayer_RIN")]
        public string Taxpayer_RIN { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Taxpayer TIN ")]
        [Display(Name = "Taxpayer_TIN")]
        public string Taxpayer_TIN { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Business RIN")]
        [Display(Name = "Business_RIN")]
        public string Business_RIN { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Business TIN ")]
        [Display(Name = "Business_TIN")]
        public string Business_TIN { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Pension Contribution Declared ")]
        [Display(Name = "Pension_Contribution_Declared")]
        public string Pension_Contribution_Declared { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter NHF Declared ")]
        [Display(Name = "NHF__Declared")]
        public string NHF_Declared { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Employer RIN")]
        [Display(Name = "Employer_RIN")]
        public string NHIS_Declared { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Total Income ")]
        [Display(Name = "Total_Income")]
        public string Total_Income { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Life Assurance ")]
        [Display(Name = "Life_Assurance")]
        public string Life_Assurance { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter PAYE Pension ")]
        [Display(Name = "PAYE_Pension")]
        public string PAYE_Pension { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter PAYE NHF ")]
        [Display(Name = "PAYE_NHF")]
        public string PAYE_NHF { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter PAYE NHIS ")]
        [Display(Name = "PAYE_NHIS")]
        public string PAYE_NHIS { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter PAYE Income ")]
        [Display(Name = "PAYE_Income")]
        public string PAYE_Income { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Assessment Year ")]
        [Display(Name = "Assessment_Year")]
        public string Assessment_Year { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter tax office ")]
        [Display(Name = "tax_office")]
        public string tax_office { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Employee PAYE Contribution  ")]
        [Display(Name = "Employee PAYE Contribution")]
        public string Employee_PAYE_Contribution { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Share of Assessment  ")]
        [Display(Name = "Share of Assessment")]
        public string Share_of_Assessment { get; set; }
        
    }
}
