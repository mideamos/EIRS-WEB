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
    
    public partial class usp_GetMDAServiceList_Result
    {
        public Nullable<int> MDAServiceID { get; set; }
        public string MDAServiceCode { get; set; }
        public string MDAServiceName { get; set; }
        public Nullable<int> RuleRunID { get; set; }
        public string RuleRunName { get; set; }
        public Nullable<int> PaymentFrequencyID { get; set; }
        public string PaymentFrequencyName { get; set; }
        public string MDAServiceItemIds { get; set; }
        public string MDAServiceItemNames { get; set; }
        public Nullable<decimal> ServiceAmount { get; set; }
        public Nullable<int> TaxYear { get; set; }
        public string SettlementMethodIds { get; set; }
        public string SettlementMethodNames { get; set; }
        public Nullable<int> PaymentOptionID { get; set; }
        public string PaymentOptionName { get; set; }
        public Nullable<bool> Active { get; set; }
        public string ActiveText { get; set; }
    }
}
