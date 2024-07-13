using EIRS.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace EIRS.Admin.Models
{
    public sealed class MDAServiceItemViewModel
    {
        public int MDAServiceItemID { get; set; }

        public string MDAServiceRefNo { get; set; }

        [Display(Name = "Revenue Stream")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Revenue Stream")]
        public int RevenueStreamID { get; set; }

        public string RevenueStreamName { get; set; }

        [Display(Name = "Revenue Sub Stream")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Revenue Sub Stream")]
        public int RevenueSubStreamID { get; set; }

        public string RevenueSubStreamName { get; set; }

        [Display(Name = "Item Category")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Item Category")]
        public int AssessmentItemCategoryID { get; set; }

        public string AssessmentItemCategoryName { get; set; }

        [Display(Name = "Item Sub Category")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Item Sub Category")]
        public int AssessmentItemSubCategoryID { get; set; }

        public string AssessmentItemSubCategoryName { get; set; }

        [Display(Name = "Revenue Agency")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Revenue Agency")]
        public int AgencyID { get; set; }

        public string AgencyName { get; set; }

        [Display(Name = "MDA Service Item Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter MDA Service Item Name")]
        [MaxLength(1000, ErrorMessage = "Only 1000 characters allowed.")]
        public string MDAServiceItemName { get; set; }

        [Display(Name = "Computation")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Computation")]
        public int ComputationID { get; set; }

        public string ComputationName { get; set; }

        [Display(Name = "Service Base Amount")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Service Base Amount")]
        public decimal ServiceBaseAmount { get; set; }

        [ComputationValidator("ComputationID", ErrorMessage = "Please Enter Percentage")]
        [Display(Name = "Percentage (%)")]
        public decimal? Percentage { get; set; }


        [Display(Name = "Service Amount")]
        public decimal? ServiceAmount { get; set; }

       
        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
