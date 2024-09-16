using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIRS.Web.Models
{
    public class GenerateServiceBillViewModel
    {
        public int TaxPayerID { get; set; }
        public int TaxPayerTypeID { get; set; }
        public string TaxPayerTypeName { get; set; }
        public string TaxPayerName { get; set; }
        public string TaxPayerTIN { get; set; }
        public string TaxPayerRIN { get; set; }
        public string ContactNumber { get; set; }
        public string ContactAddress { get; set; }

        public string MDAServiceIds { get; set; }
    }
}