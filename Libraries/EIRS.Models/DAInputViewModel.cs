using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIRS.Models
{
    public class DAInputViewModel
    {
        public int DAInputID { get; set; }

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

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Salaries")]
        [Display(Name = "Salaries")]
        public string Salaries { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Wages ")]
        [Display(Name = "Wages")]
        public string Wages { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Trade ")]
        [Display(Name = "Trade")]
        public string Trade { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Share of Partnership Profit ")]
        [Display(Name = "Share_of_Partnership_Profit")]
        public string Share_of_Partnership_Profit { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Commissions Recieved ")]
        [Display(Name = "Commissions_Recieved")]
        public string Commissions_Recieved { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Bonuses ")]
        [Display(Name = "Bonuses")]
        public string Bonuses { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Gratuities ")]
        [Display(Name = "Gratuities")]
        public string Gratuities { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Annuity")]
        [Display(Name = "Annuity")]
        public string Annuity { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Rent ")]
        [Display(Name = "Rent")]
        public string Rent { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Interest on Discount ")]
        [Display(Name = "Interest_on_Discount")]
        public string Interest_on_Discount { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Other Incomes Not Included ")]
        [Display(Name = "Other_Incomes_Not_Included")]
        public string Other_Incomes_Not_Included { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Total Income ")]
        [Display(Name = "Total_Income")]
        public string Total_Income { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Pension Contribution Declared ")]
        [Display(Name = "Pension_Contribution_Declared")]
        public string Pension_Contribution_Declared { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter NHF Declared ")]
        [Display(Name = "NHF_Declared")]
        public string NHF_Declared { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Employer RIN")]
        [Display(Name = "Employer_RIN")]
        public string NHIS_Declared { get; set; }

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

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Employee PAYE Contribution ")]
        [Display(Name = "Employee_PAYE_Contribution")]
        public string Employee_PAYE_Contribution { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter PAYE Income ")]
        [Display(Name = "PAYE_Income")]
        public string PAYE_Income { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Assessment Year ")]
        [Display(Name = "Assessment_Year")]
        public string Assessment_Year { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Start Month ")]
        [Display(Name = "Start_Month")]
        public string Start_Month { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter tax office ")]
        [Display(Name = "tax_office")]
        public string tax_office { get; set; }
        
    }
}
