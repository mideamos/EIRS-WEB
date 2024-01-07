using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EIRS.Web.Models
{
    public class SFTPDataSubmitterViewModel
    {
        public int DataSubmitterID { get; set; }

        [Display(Name = "Submitter RIN")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Submitter RIN")]
        [MaxLength(250, ErrorMessage = "Only 250 characters allowed.")]
        public string SubmitterRIN { get; set; }

        [Required(ErrorMessage = "Enter User Name")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Enter Password")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Enter Confirm Password")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password & Confirm Password does not match")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Select Data Submission Type")]
        [Display(Name = "Data Submission Type")]
        public int[] DataSubmissionTypeID { get; set; }

        public string DataSubmissionTypeName { get; set; }

        public string DataSubmissionTypeIds { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}