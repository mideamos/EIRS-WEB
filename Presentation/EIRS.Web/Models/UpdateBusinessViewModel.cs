using EIRS.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;


namespace EIRS.Web.Models
{
    public class UpdateBusinessViewModel
    {
        public long BusinessID { get; set; }

        public int TaxPayerTypeID { get; set; }

        public int TaxPayerRoleID { get; set; }

        [Display(Name = "Asset Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Asset Type")]
        public int AssetTypeID { get; set; }

        [Display(Name = "Business Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select the type of Business")]
        public int BusinessTypeID { get; set; }

        [Display(Name = "Business Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Business Name")]
        [MaxLength(150, ErrorMessage = "Only 150 characters allowed.")]
        public string BusinessName { get; set; }

        [Display(Name = "Business LGA")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select business lga")]
        public int LGAID { get; set; }

        [Display(Name = "Business Category")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Business Category")]
        public int BusinessCategoryID { get; set; }

        [Display(Name = "Business Sector")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Business Sector")]
        public int BusinessSectorID { get; set; }

        [Display(Name = "Business Sub Sector")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Business Sub Sector")]
        public int BusinessSubSectorID { get; set; }

        [Display(Name = "Business Structure")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Business Structure")]
        public int BusinessStructureID { get; set; }

        [Display(Name = "Business Operations")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Business Operations")]
        public int BusinessOperationID { get; set; }

        [Display(Name = "Premises Size")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Premises Size")]
        public int SizeID { get; set; }


        [Display(Name = "Company Name")]
        //[CompanyValidator("TaxPayerTypeID",AllowEmptyStrings = false, ErrorMessage = "Enter Company name")]
        [MaxLength(150, ErrorMessage = "Only 150 characters allowed.")]
        public string COMP_CompanyName { get; set; }

        [Display(Name = "TIN")]
        [MaxLength(50, ErrorMessage = "Only 50 characters allowed.")]
        public string COMP_TIN { get; set; }

        [Display(Name = "Mobile No 1")]
        //[CompanyValidator("TaxPayerTypeID", AllowEmptyStrings = false, ErrorMessage = "Enter mobile phone number of Company")]
        [RegularExpression(@"^[1-9][0-9]{9}$", ErrorMessage = "Looks like you entered invalid mobile number")]
        [MaxLength(10, ErrorMessage = "Only 10 numbers allowed.")]
        public string COMP_MobileNumber1 { get; set; }

        [Display(Name = "Mobile No 2")]
        [RegularExpression(@"^[1-9][0-9]{9}$", ErrorMessage = "Looks like you entered invalid mobile number")]
        [MaxLength(10, ErrorMessage = "Only 10 numbers allowed.")]
        public string COMP_MobileNumber2 { get; set; }

        [Display(Name = "Email Address 1")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Enter valid email address")]
        public string COMP_EmailAddress1 { get; set; }

        [Display(Name = "Email Address 2")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Enter valid email address")]
        public string COMP_EmailAddress2 { get; set; }

        [Display(Name = "Tax Office")]
        public int? COMP_TaxOfficeID { get; set; }

        [Display(Name = "Economic Activity")]
        //[CompanyValidator("TaxPayerTypeID", AllowEmptyStrings = false, ErrorMessage = "Select Economic Activity")]
        public int? COMP_EconomicActivitiesID { get; set; }

        [Display(Name = "Preferred Notification")]
        //[CompanyValidator("TaxPayerTypeID", AllowEmptyStrings = false, ErrorMessage = "Select Preferred Notification")]
        public int? COMP_NotificationMethodID { get; set; }

        [Display(Name = "Gender")]
        //[IndividualValidator("TaxPayerTypeID", AllowEmptyStrings = false, ErrorMessage = "Select individual’s gender")]
        public int? IND_GenderID { get; set; }

        [Display(Name = "Title")]
        //[IndividualValidator("TaxPayerTypeID", AllowEmptyStrings = false, ErrorMessage = "Select title")]
        public int? IND_TitleID { get; set; }

        [Display(Name = "First Name")]
        //[IndividualValidator("TaxPayerTypeID", AllowEmptyStrings = false, ErrorMessage = "Enter individuals first name")]
        [MaxLength(25, ErrorMessage = "Only 25 characters allowed.")]
        public string IND_FirstName { get; set; }

        [Display(Name = "Last Name")]
        //[IndividualValidator("TaxPayerTypeID", AllowEmptyStrings = false, ErrorMessage = "Enter individuals surname name")]
        [MaxLength(25, ErrorMessage = "Only 25 characters allowed.")]
        public string IND_LastName { get; set; }


        [Display(Name = "Middle Name")]
        [MaxLength(25, ErrorMessage = "Only 25 characters allowed.")]
        public string IND_MiddleName { get; set; }


        [Display(Name = "Date of Birth")]
        //[IndividualValidator("TaxPayerTypeID", AllowEmptyStrings = false, ErrorMessage = "Enter enter individuals date of birth")]
        public string IND_DOB { get; set; }

        [Display(Name = "TIN")]
        [MaxLength(50, ErrorMessage = "Only 50 characters allowed.")]
        public string IND_TIN { get; set; }

        [Display(Name = "Mobile No 1")]
        //[IndividualValidator("TaxPayerTypeID", AllowEmptyStrings = false, ErrorMessage = "Enter mobile phone number of individual")]
        [RegularExpression(@"^[1-9][0-9]{9}$", ErrorMessage = "Looks like you entered invalid mobile number")]
        [MaxLength(10, ErrorMessage = "Only 10 numbers allowed.")]
        public string IND_MobileNumber1 { get; set; }

        [Display(Name = "Mobile No 2")]
        [RegularExpression(@"^[1-9][0-9]{9}$", ErrorMessage = "Looks like you entered invalid mobile number")]
        [MaxLength(10, ErrorMessage = "Only 10 numbers allowed.")]
        public string IND_MobileNumber2 { get; set; }

        [Display(Name = "Email Address 1")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Enter valid email address")]
        public string IND_EmailAddress1 { get; set; }

        [Display(Name = "Email Address 2")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Enter valid email address")]
        public string IND_EmailAddress2 { get; set; }


        [Display(Name = "Biometric Details")]
        public string IND_BiometricDetails { get; set; }

        [Display(Name = "Tax Office")]
        public int? IND_TaxOfficeID { get; set; }

        [Display(Name = "Marital Status")]
        public int? IND_MaritalStatusID { get; set; }

        [Display(Name = "Nationality")]
        //[IndividualValidator("TaxPayerTypeID", AllowEmptyStrings = false, ErrorMessage = "Select Nationality")]
        public int? IND_NationalityID { get; set; }


        [Display(Name = "Economic Activity")]
        //[IndividualValidator("TaxPayerTypeID", AllowEmptyStrings = false, ErrorMessage = "Select Economic Activity")]
        public int? IND_EconomicActivitiesID { get; set; }

        [Display(Name = "Preferred Notification")]
        //[IndividualValidator("TaxPayerTypeID", AllowEmptyStrings = false, ErrorMessage = "Select Preferred Notification")]
        public int? IND_NotificationMethodID { get; set; }

        [Display(Name = "Special Tax Payer Name")]
        //[SpecialValidator("TaxPayerTypeID", AllowEmptyStrings = false, ErrorMessage = "Enter Special Tax Payer Name")]
        [MaxLength(150, ErrorMessage = "Only 150 characters allowed.")]
        public string SP_SpecialName { get; set; }

        [Display(Name = "Tax Office")]
        public int? SP_TaxOfficeID { get; set; }

        [Display(Name = "Contact Number")]
        //[SpecialValidator("TaxPayerTypeID", AllowEmptyStrings = false, ErrorMessage = "Enter Contact Number")]
        [RegularExpression(@"^[1-9][0-9]{9}$", ErrorMessage = "Looks like you entered invalid contact number")]
        [MaxLength(10, ErrorMessage = "Only 10 numbers allowed.")]
        public string SP_ContactNumber { get; set; }

        [Display(Name = "Contact Email")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Enter valid contact email")]
        public string SP_ContactEmail { get; set; }

        [Display(Name = "Contact Name")]
        //[SpecialValidator("TaxPayerTypeID", AllowEmptyStrings = false, ErrorMessage = "Enter Contact Name")]
        public string SP_ContactName { get; set; }

        [Display(Name = "Description")]
        public string SP_Description { get; set; }

        [Display(Name = "Preferred Notification")]
        //[SpecialValidator("TaxPayerTypeID", AllowEmptyStrings = false, ErrorMessage = "Select Preferred Notification")]
        public int? SP_NotificationMethodID { get; set; }

        [Display(Name = "Government Name")]
       // [GovernmentValidator("TaxPayerTypeID", AllowEmptyStrings = false, ErrorMessage = "Enter Government name")]
        [MaxLength(150, ErrorMessage = "Only 150 characters allowed.")]
        public string GOV_GovernmentName { get; set; }

        [Display(Name = "Government Type")]
       //// [GovernmentValidator("TaxPayerTypeID", AllowEmptyStrings = false, ErrorMessage = "Select Government Type")]
        public int? GOV_GovernmentTypeID { get; set; }

        [Display(Name = "Tax Office")]
        public int? GOV_TaxOfficeID { get; set; }

        [Display(Name = "Contact Number")]
       //// [GovernmentValidator("TaxPayerTypeID", AllowEmptyStrings = false, ErrorMessage = "Enter Contact Number")]
        [RegularExpression(@"^[1-9][0-9]{9}$", ErrorMessage = "Looks like you entered invalid contact number")]
        [MaxLength(10, ErrorMessage = "Only 10 numbers allowed.")]
        public string GOV_ContactNumber { get; set; }

        [Display(Name = "Contact Email")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Enter valid contact email")]
        public string GOV_ContactEmail { get; set; }

        [Display(Name = "Contact Name")]
     //  // [GovernmentValidator("TaxPayerTypeID", AllowEmptyStrings = false, ErrorMessage = "Enter Contact Name")]
        public string GOV_ContactName { get; set; }

        [Display(Name = "Preferred Notification")]
      // // [GovernmentValidator("TaxPayerTypeID", AllowEmptyStrings = false, ErrorMessage = "Select Preferred Notification")]
        public int? GOV_NotificationMethodID { get; set; }
    }


    //[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    //public sealed class CompanyValidator : RequiredAttribute, IClientValidatable
    //{
    //    private string PropertyName { get; set; }

    //    public CompanyValidator(string propertyName)
    //    {
    //        PropertyName = propertyName;
    //    }


    //    //    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    //    //    {
    //    //        Object instance = validationContext.ObjectInstance;
    //    //        Type type = instance.GetType();
    //    //        int TaxPayerTypeID = TrynParse.parseInt(type.GetProperty(PropertyName).GetValue(instance, null));

    //    //        if (type.GetProperty(validationContext.MemberName).PropertyType.Equals(typeof(System.Int32)))
    //    //        {
    //    //            if (TaxPayerTypeID == (int)EnumList.TaxPayerType.Companies && TrynParse.parseInt(value) == 0)
    //    //            {
    //    //                return new ValidationResult("Enter " + validationContext.DisplayName);
    //    //            }
    //    //        }
    //    //        else if (type.GetProperty(validationContext.MemberName).PropertyType.Equals(typeof(System.Nullable<Int32>)))
    //    //        {
    //    //            if (TaxPayerTypeID == (int)EnumList.TaxPayerType.Companies && TrynParse.parseInt(value) == 0)
    //    //            {
    //    //                return new ValidationResult("Enter " + validationContext.DisplayName);
    //    //            }
    //    //        }
    //    //        else
    //    //        {
    //    //            if (TaxPayerTypeID == (int)EnumList.TaxPayerType.Companies && string.IsNullOrEmpty(TrynParse.parseString(value)))
    //    //            {
    //    //                return new ValidationResult("Enter " + validationContext.DisplayName);
    //    //            }
    //    //        }


    //    //        return ValidationResult.Success;
    //    //    }

    //    //    public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
    //    //    {
    //    //        ModelClientValidationRule mvr = new ModelClientValidationRule();
    //    //        mvr.ErrorMessage = "Enter " + metadata.DisplayName;
    //    //        mvr.ValidationType = "companyvalidator";
    //    //        return new[] { mvr };
    //    //    }
    //    //}

    //    //[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    //    //public sealed class IndividualValidator : RequiredAttribute, IClientValidatable
    //    //{
    //    //    private string PropertyName { get; set; }

    //    //    public IndividualValidator(string propertyName)
    //    //    {
    //    //        PropertyName = propertyName;
    //    //    }


    //    //    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    //    //    {
    //    //        Object instance = validationContext.ObjectInstance;
    //    //        Type type = instance.GetType();
    //    //        int TaxPayerTypeID = TrynParse.parseInt(type.GetProperty(PropertyName).GetValue(instance, null));

    //    //        if (type.GetProperty(validationContext.MemberName).PropertyType.Equals(typeof(System.Int32)))
    //    //        {
    //    //            if (TaxPayerTypeID == (int)EnumList.TaxPayerType.Individual && TrynParse.parseInt(value) == 0)
    //    //            {
    //    //                return new ValidationResult("Enter " + validationContext.DisplayName);
    //    //            }
    //    //        }
    //    //        else if (type.GetProperty(validationContext.MemberName).PropertyType.Equals(typeof(System.Nullable<Int32>)))
    //    //        {
    //    //            if (TaxPayerTypeID == (int)EnumList.TaxPayerType.Individual && TrynParse.parseInt(value) == 0)
    //    //            {
    //    //                return new ValidationResult("Enter " + validationContext.DisplayName);
    //    //            }
    //    //        }
    //    //        else
    //    //        {
    //    //            if (TaxPayerTypeID == (int)EnumList.TaxPayerType.Individual && string.IsNullOrEmpty(TrynParse.parseString(value)))
    //    //            {
    //    //                return new ValidationResult("Enter " + validationContext.DisplayName);
    //    //            }
    //    //        }


    //    //        return ValidationResult.Success;
    //    //    }

    //    //    public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
    //    //    {
    //    //        ModelClientValidationRule mvr = new ModelClientValidationRule();
    //    //        mvr.ErrorMessage = "Enter " + metadata.DisplayName;
    //    //        mvr.ValidationType = "individualvalidator";
    //    //        return new[] { mvr };
    //    //    }
    //    //}

    //    //[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    //    //public sealed class SpecialValidator : RequiredAttribute, IClientValidatable
    //    //{
    //    //    private string PropertyName { get; set; }

    //    //    public SpecialValidator(string propertyName)
    //    //    {
    //    //        PropertyName = propertyName;
    //    //    }


    //    //    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    //    //    {
    //    //        Object instance = validationContext.ObjectInstance;
    //    //        Type type = instance.GetType();
    //    //        int TaxPayerTypeID = TrynParse.parseInt(type.GetProperty(PropertyName).GetValue(instance, null));

    //    //        if (type.GetProperty(validationContext.MemberName).PropertyType.Equals(typeof(System.Int32)))
    //    //        {
    //    //            if (TaxPayerTypeID == (int)EnumList.TaxPayerType.Special && TrynParse.parseInt(value) == 0)
    //    //            {
    //    //                return new ValidationResult("Enter " + validationContext.DisplayName);
    //    //            }
    //    //        }
    //    //        else if (type.GetProperty(validationContext.MemberName).PropertyType.Equals(typeof(System.Nullable<Int32>)))
    //    //        {
    //    //            if (TaxPayerTypeID == (int)EnumList.TaxPayerType.Special && TrynParse.parseInt(value) == 0)
    //    //            {
    //    //                return new ValidationResult("Enter " + validationContext.DisplayName);
    //    //            }
    //    //        }
    //    //        else
    //    //        {
    //    //            if (TaxPayerTypeID == (int)EnumList.TaxPayerType.Special && string.IsNullOrEmpty(TrynParse.parseString(value)))
    //    //            {
    //    //                return new ValidationResult("Enter " + validationContext.DisplayName);
    //    //            }
    //    //        }


    //    //        return ValidationResult.Success;
    //    //    }

    //    //    //public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
    //    //    //{
    //    //    //    ModelClientValidationRule mvr = new ModelClientValidationRule();
    //    //    //    mvr.ErrorMessage = "Enter " + metadata.DisplayName;
    //    //    //    mvr.ValidationType = "specialvalidator";
    //    //    //    return new[] { mvr };
    //    //    //}
    //    //}

    //    //[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    //    //public sealed class GovernmentValidator : RequiredAttribute, IClientValidatable
    //    //{
    //    //    private string PropertyName { get; set; }

    //    //    public GovernmentValidator(string propertyName)
    //    //    {
    //    //        PropertyName = propertyName;
    //    //    }


    //    //    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    //    //    {
    //    //        Object instance = validationContext.ObjectInstance;
    //    //        Type type = instance.GetType();
    //    //        int TaxPayerTypeID = TrynParse.parseInt(type.GetProperty(PropertyName).GetValue(instance, null));

    //    //        if (type.GetProperty(validationContext.MemberName).PropertyType.Equals(typeof(System.Int32)))
    //    //        {
    //    //            if (TaxPayerTypeID == (int)EnumList.TaxPayerType.Government && TrynParse.parseInt(value) == 0)
    //    //            {
    //    //                return new ValidationResult("Enter " + validationContext.DisplayName);
    //    //            }
    //    //        }
    //    //        else if (type.GetProperty(validationContext.MemberName).PropertyType.Equals(typeof(System.Nullable<Int32>)))
    //    //        {
    //    //            if (TaxPayerTypeID == (int)EnumList.TaxPayerType.Government && TrynParse.parseInt(value) == 0)
    //    //            {
    //    //                return new ValidationResult("Enter " + validationContext.DisplayName);
    //    //            }
    //    //        }
    //    //        else
    //    //        {
    //    //            if (TaxPayerTypeID == (int)EnumList.TaxPayerType.Government && string.IsNullOrEmpty(TrynParse.parseString(value)))
    //    //            {
    //    //                return new ValidationResult("Enter " + validationContext.DisplayName);
    //    //            }
    //    //        }


    //    //        return ValidationResult.Success;
    //    //    }

    //    //    //public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
    //    //    //{
    //    //    //    ModelClientValidationRule mvr = new ModelClientValidationRule();
    //    //    //    mvr.ErrorMessage = "Enter " + metadata.DisplayName;
    //    //    //    mvr.ValidationType = "governmentvalidator";
    //    //    //    return new[] { mvr };
    //    //    //}
    //    //}
    //}
}