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
    using System.Collections.Generic;
    
    public partial class vw_TCCRequestList
    {
        public long TCCRequestID { get; set; }
        public string RequestRefNo { get; set; }
        public string TaxPayerName { get; set; }
        public string MobileNumber1 { get; set; }
        public Nullable<System.DateTime> RequestDate { get; set; }
        public string ServiceBillRefNo { get; set; }
        public string SettlementStatusName { get; set; }
        public Nullable<int> StatusID { get; set; }
        public string StatusName { get; set; }
    }
}
