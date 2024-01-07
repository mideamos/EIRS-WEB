using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EIRS.Common;
using CompareAttribute = System.ComponentModel.DataAnnotations.CompareAttribute;

namespace EIRS.Web.Models
{
    public class UserViewModel
    {
        public int UserID { get; set; }

        [Required(ErrorMessage = "Please User Type")]
        [Display(Name = "User Type")]
        public int UserTypeID { get; set; }

        public string UserTypeName { get; set; }

        [Required(ErrorMessage = "Enter User Name")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Enter Contact Name")]
        [Display(Name = "Contact Name")]
        public string ContactName { get; set; }

        [Required(ErrorMessage = "Enter Email Address")]
        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Enter valid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter Contact Number")]
        [Display(Name = "Contact Number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$", ErrorMessage = "Please enter valid contact number.")]
        public string ContactNumber { get; set; }

        [Required(ErrorMessage = "Enter Password")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Enter Confirm Password")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password & Confirm Password does not match")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Is Manager")]
        public bool IsTOManager { get; set; }

        public string IsTOManagerText { get; set; }

        [Display(Name = "Tax Office")]
      //  [TaxOfficeValidator("UserTypeID", AllowEmptyStrings = true, ErrorMessage = "Select Tax Office")]
        public int? TaxOfficeID { get; set; }

        public string TaxOfficeName { get; set; }

        [Display(Name = "Manager")]
     //   [TaxOfficerValidator("UserTypeID", "IsTOManager", AllowEmptyStrings = true, ErrorMessage = "Select Tax Office Manager")]
        public int? TOManagerID { get; set; }

        public string TOManagerName { get; set; }

        [Display(Name = "Signature")]
      //  [SignatureFileValidator(AllowEmptyStrings = false)]
        public HttpPostedFileBase SignatureFileUpload { get; set; }

        [Display(Name = "Signature")]
        public string SignatureFilePath { get; set; }

        [Display(Name = "Active")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }

        [Display(Name = "Director")]
        public bool IsDirector { get; set; }

        public string IsDirectorText { get; set; }


    }

    //[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    //public sealed class TaxOfficeValidator : RequiredAttribute, IClientValidatable
    //{
    //    private string PropertyName { get; set; }

    //    public TaxOfficeValidator(string propertyName)
    //    {
    //        PropertyName = propertyName;
    //    }


    //    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    //    {
    //        Object instance = validationContext.ObjectInstance;
    //        Type type = instance.GetType();
    //        int mIntUserTypeID = TrynParse.parseInt(type.GetProperty(PropertyName).GetValue(instance, null));

    //        if (mIntUserTypeID == (int)EnumList.UserType.Staff && TrynParse.parseInt(value) == 0)
    //        {
    //            return new ValidationResult("Enter " + validationContext.DisplayName);
    //        }

    //        return ValidationResult.Success;
    //    }

    //    public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
    //    {
    //        ModelClientValidationRule mvr = new ModelClientValidationRule();
    //        mvr.ErrorMessage = "Enter " + metadata.DisplayName;
    //        mvr.ValidationType = "taxofficevalidator";
    //        return new[] { mvr };
    //    }
    //}

    //[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    //public sealed class TaxOfficerValidator : RequiredAttribute, IClientValidatable
    //{
    //    private string UserTypeID { get; set; }
    //    private string IsTOManager { get; set; }

    //    public TaxOfficerValidator(string UserTypeID, string IsTOManager)
    //    {
    //        this.UserTypeID = UserTypeID;
    //        this.IsTOManager = IsTOManager;
    //    }


    //    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    //    {
    //        Object instance = validationContext.ObjectInstance;
    //        Type type = instance.GetType();
    //        int mIntUserTypeID = TrynParse.parseInt(type.GetProperty(UserTypeID).GetValue(instance, null));
    //        bool mblnIsTOManager = TrynParse.parseBool(type.GetProperty(IsTOManager).GetValue(instance, null));

    //        if (mIntUserTypeID == (int)EnumList.UserType.Staff && mblnIsTOManager == false && TrynParse.parseInt(value) == 0)
    //        {
    //            return new ValidationResult("Enter " + validationContext.DisplayName);
    //        }

    //        return ValidationResult.Success;
    //    }

    //    public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
    //    {
    //        ModelClientValidationRule mvr = new ModelClientValidationRule();
    //        mvr.ErrorMessage = "Enter " + metadata.DisplayName;
    //        mvr.ValidationType = "taxofficervalidator";
    //        return new[] { mvr };
    //    }
    //}

    //[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    //public sealed class SignatureFileValidator : RequiredAttribute, IClientValidatable
    //{
    //    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    //    {
    //        Object instance = validationContext.ObjectInstance;
    //        Type type = instance.GetType();
    //        string[] FileExtension = { ".png", ".jpg", ".jpeg", ".bmp", ".BMP", ".gif" };
    //        HttpPostedFileBase postedFile = (HttpPostedFileBase)value;

    //        if (postedFile != null)
    //        {
    //            var vExtention = Path.GetExtension(postedFile.FileName);
    //            if (FileExtension.Contains(vExtention))
    //            {
    //                return ValidationResult.Success;
    //            }
    //            else
    //            {
    //                return new ValidationResult("Upload only signature image");
    //            }
    //        }
    //        return ValidationResult.Success;
    //    }

    //    //public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext controllerContext)
    //    //{
    //    //    ModelClientValidationRule mvr = new ModelClientValidationRule
    //    //    {
    //    //        ErrorMessage = "Upload " + metadata.DisplayName + " Image",
    //    //        ValidationType = "signaturefilevalidator"
    //    //    };
    //    //    return new[] { mvr };
    //    //}
    //}
}