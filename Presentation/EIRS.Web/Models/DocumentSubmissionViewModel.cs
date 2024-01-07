using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EIRS.Web.Models
{
    public class DocumentSubmissionViewModel
    {
        [Display(Name = "Data Submitter")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Data Submitter")]
        public int DataSubmitterID { get; set; }

        [Display(Name = "Tax Year")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Tax Year")]
        public int TaxYear { get; set; }

        [Display(Name = "Data Submission Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Data Submission Type")]
        public int DataSubmissionTypeID { get; set; }

        [Display(Name = "Upload Document")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Upload Document")]
        public HttpPostedFileBase DocumentFile { get; set; }
    }

    public class DS_DocumentSubmissionViewModel
    {

        [Display(Name = "Tax Year")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Tax Year")]
        public int TaxYear { get; set; }

        [Display(Name = "Data Submission Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Data Submission Type")]
        public int DataSubmissionTypeID { get; set; }

        [Display(Name = "Upload Document")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Upload Document")]
        public HttpPostedFileBase DocumentFile { get; set; }
    }
}