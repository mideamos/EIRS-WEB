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
    
    public partial class usp_RPT_TaxPayerLiabilityStatus_Payment_Result
    {
        public string TaxPayerName { get; set; }
        public Nullable<int> PaymentID { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
        public Nullable<int> PaymentTypeID { get; set; }
        public string PaymentTypeName { get; set; }
        public string PaymentRefNo { get; set; }
        public Nullable<decimal> Amount { get; set; }
    }
}
