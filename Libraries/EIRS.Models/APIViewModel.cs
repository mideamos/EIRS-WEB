using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public class APIViewModel
    {
        public int APIID { get; set; }

        [Display(Name = "API Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter API name")]
        [MaxLength(250, ErrorMessage = "Only 250 characters allowed.")]
        public string APIName { get; set; }

        [Display(Name = "API Description")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter API Description")]
        public string APIDescription { get; set; }

        public HttpPostedFileBase APIDocument { get; set; }

        [Display(Name = "API Documentation")]
        public string DocumentPath { get; set; }


        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }

    }
}