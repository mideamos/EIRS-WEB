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
    
    public partial class TCCDetail
    {
        public long TCCDetailID { get; set; }
        public Nullable<int> TaxPayerTypeID { get; set; }
        public Nullable<int> TaxPayerID { get; set; }
        public Nullable<int> TaxYear { get; set; }
        public Nullable<decimal> AssessableIncome { get; set; }
        public Nullable<decimal> TCCTaxPaid { get; set; }
        public Nullable<decimal> ERASTaxPaid { get; set; }
        public Nullable<decimal> ERASAssessed { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}
