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
    
    public partial class ScratchCard_PurchaseRequest
    {
        public int SCPRequestID { get; set; }
        public string RequestReferenceNo { get; set; }
        public Nullable<System.DateTime> RequestDate { get; set; }
        public Nullable<int> DealerTypeID { get; set; }
        public Nullable<int> ScratchCardDealerID { get; set; }
        public Nullable<int> RequestedQty { get; set; }
        public Nullable<decimal> Commission { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public Nullable<int> PaymentStatusID { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}
