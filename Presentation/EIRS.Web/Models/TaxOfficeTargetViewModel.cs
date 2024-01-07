using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIRS.Web.Models
{
    public class TaxOfficeTargetViewModel
    {
        public int TaxOfficeID { get; set; }
        public string TaxOfficeName { get; set; }

        public int TaxYearID { get; set; }
        public string TaxYearName { get; set; }
    }

    public class TaxOfficerTargetViewModel
    {
        public int TaxOfficeID { get; set; }
        public string TaxOfficeName { get; set; }

        public int TaxOfficerID { get; set; }
        public string TaxOfficerName { get; set; }

        public int TaxYearID { get; set; }
        public string TaxYearName { get; set; }
    }
}