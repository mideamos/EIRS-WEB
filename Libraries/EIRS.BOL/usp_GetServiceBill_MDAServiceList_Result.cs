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
    
    public partial class usp_GetServiceBill_MDAServiceList_Result
    {
        public Nullable<long> SBSID { get; set; }
        public Nullable<int> MDAServiceID { get; set; }
        public string MDAServiceName { get; set; }
        public Nullable<int> TaxYear { get; set; }
        public Nullable<decimal> ServiceAmount { get; set; }
        public Nullable<decimal> SettledAmount { get; set; }
    }
}
