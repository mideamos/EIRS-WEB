//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EIRS.BOL
{
    using System;
    
    public partial class usp_SearchGovernmentForRDMLoad_Result
    {
        public int GovernmentID { get; set; }
        public string GovernmentRIN { get; set; }
        public string GovernmentName { get; set; }
        public Nullable<int> GovernmentTypeID { get; set; }
        public string GovernmentTypeName { get; set; }
        public string TIN { get; set; }
        public Nullable<int> TaxOfficeID { get; set; }
        public string TaxOfficeName { get; set; }
        public Nullable<int> TaxPayerTypeID { get; set; }
        public string TaxPayerTypeName { get; set; }
        public string ContactNumber { get; set; }
        public string ContactEmail { get; set; }
        public string ContactName { get; set; }
        public Nullable<int> NotificationMethodID { get; set; }
        public string NotificationMethodName { get; set; }
        public string ContactAddress { get; set; }
        public Nullable<int> RegisterationStatusID { get; set; }
        public string RegisterationStatusName { get; set; }
        public Nullable<System.DateTime> RegisterationDate { get; set; }
        public Nullable<bool> Active { get; set; }
        public string ActiveText { get; set; }
    }
}
