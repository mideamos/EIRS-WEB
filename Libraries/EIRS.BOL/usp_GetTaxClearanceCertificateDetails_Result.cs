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
    
    public partial class usp_GetTaxClearanceCertificateDetails_Result
    {
        public long TCCID { get; set; }
        public string TCCNumber { get; set; }
        public Nullable<System.DateTime> TCCDate { get; set; }
        public Nullable<int> TaxYear { get; set; }
        public Nullable<int> TaxPayerID { get; set; }
        public Nullable<int> TaxPayerTypeID { get; set; }
        public string RequestRefNo { get; set; }
        public string SerialNumber { get; set; }
        public string TaxPayerDetails { get; set; }
        public string IncomeSource { get; set; }
        public Nullable<int> StatusID { get; set; }
        public string CancelNotes { get; set; }
        public string StatusName { get; set; }
        public string TaxPayerName { get; set; }
        public string TaxPayerRIN { get; set; }
        public string TaxPayerTIN { get; set; }
    }
}
