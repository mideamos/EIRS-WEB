using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace EIRS.Admin.Models
{
    public partial class MdaserviceRollover
    {
        public int Id { get; set; }
        public bool Active { get; set; }
        public string TaxYear { get; set; }
        public int? PaymentFrequencyId { get; set; }
        public int? MdaserviceId { get; set; }
        public int? NewMdaserviceId { get; set; }
        public int? RuleRunId { get; set; }
        public decimal ServiceAmount { get; set; }
        public string MdaserviceName { get; set; }
        public string MdaserviceCode { get; set; }
        public int? MdaserviceItemId { get; set; }
        public int? Msmiid { get; set; }
    }
}
