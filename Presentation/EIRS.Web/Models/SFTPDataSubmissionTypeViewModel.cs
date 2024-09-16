using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EIRS.Web.Models
{
    public class SFTPDataSubmissionTypeViewModel
    {
        public int DataSubmissionTypeID { get; set; }

        [Display(Name = "Data Submission Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Data Submission Type")]
        [MaxLength(250, ErrorMessage = "Only 250 characters allowed.")]
        public string DataSubmissionTypeName { get; set; }

        [Display(Name = "Upload Template")]
        public string TemplatePath { get; set; }

        [Display(Name = "Upload Template")]
        public HttpPostedFileBase TemplateFile { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}