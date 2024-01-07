using EIRS.Common;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace EIRS.Web.Models
{
    public sealed class CertificateTypeViewModel
    {
        public int CertificateTypeID { get; set; }

        [Display(Name = "Certificate Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Certificate Type")]
        [MaxLength(500,ErrorMessage = "Only 500 characters allowed.")]
        public string CertificateTypeName { get; set; }

        [Display(Name = "Tax Year")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Tax Year")]
        public int TaxYear { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Certificate Number Prefix")]
        [Required(AllowEmptyStrings = false,ErrorMessage = "Enter Certificate Number Prefix")]
        public string CNPrefix { get; set; }

        [Display(Name = "Agency")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Agency")]
        public int AgencyID { get; set; }

        [Display(Name = "Template")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Template")]
        public int TemplateID { get; set; }

        [Display(Name = "Template")]
        public string CertificateTemplatePath { get; set; }

        [Display(Name = "Template")]
        public HttpPostedFileBase CertificateTemplateFile { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }
    }

    
}
