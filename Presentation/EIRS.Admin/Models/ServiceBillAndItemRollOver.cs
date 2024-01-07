using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIRS.Admin.Models
{
    public class ServiceBillAndItemRollOver
    {
        public int Id { get; set; }
        public bool Active { get; set; }
        public string TaxYear { get; set; }
        public decimal Percentage { get; set; }
        public decimal ServiceBaseAmount { get; set; }
        public string MDAServiceCode { get; set; }
        public string MDAServiceName { get; set; }
        public decimal ServiceAmount { get; set; }
    }
}