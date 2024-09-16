using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIRS.Web.Models
{
    public class TaxReportModel
    {
        public string TaxOffice { get; set; }
        public string StatusName { get; set; }
        public string Taxpayername { get; set; }
        public int? TaxpayerId { get; set; }
        public int? TaxOfficeId { get; set; }
        public int? StatusId { get; set; }
        public bool? IsDownload { get; set; }
        public int? TaxYear { get; set; }
        public string RequestRef { get; set; }
        public DateTime? RequestDate { get; set; }
    }    
    public class TaxReportReturn
    {
        public int? TaxOfficeId { get; set; }
        public string TaxOffice { get; set; }
        public string TotalRequest { get; set; }
    }
}