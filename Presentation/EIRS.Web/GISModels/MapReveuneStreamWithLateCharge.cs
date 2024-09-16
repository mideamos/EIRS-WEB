using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIRS.Web.GISModels
{
    public class MapReveuneStreamWithLateCharge
    {
        public int Id { get; set; }
        public int TaxYear { get; set; }
        public decimal Penalty { get; set; }
        public decimal Interest { get; set; }
        public string RevenueStreamName { get; set; }
    }
}