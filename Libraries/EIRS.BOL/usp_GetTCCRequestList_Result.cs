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
    
    public partial class usp_GetTCCRequestList_Result
    {
        public long TCCRequestID { get; set; }
        public string RequestRefNo { get; set; }
        public string TaxPayerName { get; set; }
        public string TaxPayerRIN { get; set; }
        public string TaxPayerTIN { get; set; }
        public string MobileNumber { get; set; }
        public Nullable<int> TaxOfficeID { get; set; }
        public Nullable<int> TaxYear { get; set; }
        public Nullable<System.DateTime> RequestDate { get; set; }
        public string ServiceBillRefNo { get; set; }
        public string SettlementStatusName { get; set; }
        public Nullable<int> StatusID { get; set; }
        public string StatusName { get; set; }
    }
}
