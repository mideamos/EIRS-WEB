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
    
    public partial class usp_GetPaymentByRevenueStreamDetail_Result
    {
        public Nullable<int> PaymentTypeID { get; set; }
        public Nullable<int> TaxPayerID { get; set; }
        public string TaxPayerName { get; set; }
        public Nullable<int> TaxPayerTypeID { get; set; }
        public string TaxPayerTypeName { get; set; }
        public string TaxPayerRIN { get; set; }
        public string PaymentRef { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<int> PaymentMethodID { get; set; }
        public string PaymentMethodName { get; set; }
    }
}
