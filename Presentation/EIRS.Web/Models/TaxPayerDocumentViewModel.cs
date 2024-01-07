using EIRS.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EIRS.Web.Models
{
    public class TaxPayerDocumentViewModel
    {
        public int TaxPayerID { get; set; }
        public int TaxPayerTypeID { get; set; }
        public string TaxPayerTypeName { get; set; }
        public string TaxPayerName { get; set; }
        public string TaxPayerTIN { get; set; }
        public string TaxPayerRIN { get; set; }
        public string ContactNumber { get; set; }
        public string ContactAddress { get; set; }

        [Display(Name ="Document Title")]
        [Required(ErrorMessage = "Enter Document Title")]
        public string DocumentTitle { get; set; }

        [Display(Name = "Notes")]
        public string Notes { get; set; }

        [Display(Name = "Document")]
        //[RequiredUploadFileValidator(AllowEmptyStrings = false)]
        public HttpPostedFileBase DocumentFileUpload { get; set; }
    }

    //[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    //public class RequiredUploadFileValidator : RequiredAttribute, IClientValidatable
    //{
    //    string[] FileExtension = { ".pdf", ".PDF" };
        
    //    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    //    {
    //        object instance = validationContext.ObjectInstance;
    //        Type type = instance.GetType();
    //        HttpPostedFileBase postedFile = (HttpPostedFileBase)value;
    //        if ((postedFile == null || postedFile.ContentLength == 0))
    //        {
    //            return new ValidationResult("Upload valid " + validationContext.DisplayName);
    //        }

    //        if (postedFile != null)
    //        {
    //            var vExtention = Path.GetExtension(postedFile.FileName);
    //            if (FileExtension.Contains(vExtention))
    //            {
    //                return ValidationResult.Success;
    //            }
    //            else
    //            {
    //                return new ValidationResult("Upload only PDF Files");
    //            }
    //        }
    //        return ValidationResult.Success;
    //    }

    //    public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext controllerContext)
    //    {
    //        ModelClientValidationRule mvr = new ModelClientValidationRule
    //        {
    //            ErrorMessage = $"Upload valid {metadata.DisplayName}",
    //            ValidationType = "requireduploadfile"
    //        };
    //        return new[] { mvr };
    //    }
    //}
}