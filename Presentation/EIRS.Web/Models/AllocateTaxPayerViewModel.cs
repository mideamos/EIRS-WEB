using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIRS.Web.Models
{
    public class AllocateTaxPayerViewModel
    {
        public int TaxPayerTypeID { get; set; }
        public int TaxOfficeID { get; set; }
        public int TaxOfficerID { get; set; }
        
        public string TaxPayerIds { get; set; }
    }
}