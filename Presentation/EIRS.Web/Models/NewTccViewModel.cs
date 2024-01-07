using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static EIRS.Web.Controllers.Filters;

namespace EIRS.Web.Models
{
    public class NewTccViewModel
    {
        public long TCCRequestID { get; set; }
        public string RequestRefNo { get; set; }
        public Nullable<System.DateTime> RequestDate { get; set; }
        public Nullable<long> ServiceBillID { get; set; }
        public Nullable<int> TaxPayerID { get; set; }
        public Nullable<int> TaxPayerTypeID { get; set; }
        public Nullable<int> TaxYear { get; set; }
        public Nullable<int> StatusID { get; set; }
        public Nullable<int> VisibleSignStatusID { get; set; }
        public Nullable<int> PDFTemplateID { get; set; }
        public string GeneratedPath { get; set; }
        public string ValidatedPath { get; set; }
        public string SignedVisiblePath { get; set; }
        public bool IsSigned { get; set; }
        public string SignedDigitalPath { get; set; }
        public string SealedPath { get; set; }
        public Nullable<int> SEDE_DocumentID { get; set; }
        public Nullable<long> SEDE_OrderID { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }

        //////Joined Field 
        public string TaxFName { get; set; }
        public string TaxLName { get; set; }
        public string MobileNumber { get; set; }
        public string BillStatus { get; set; }
    }
}