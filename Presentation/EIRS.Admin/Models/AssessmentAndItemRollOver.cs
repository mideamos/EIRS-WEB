using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace EIRS.Admin.Models
{
    public partial class AssessmentAndItemRollOver
    {
        public int Id { get; set; }
        public bool Active { get; set; }
        public string TaxYear { get; set; }
        public string TaxMonth { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal Percentage { get; set; }
        public int? AssessmentRuleId { get; set; }
        public int? NewAssessmentRuleId { get; set; }
        public decimal TaxBaseAmount { get; set; }
        public string AssessmentItemName { get; set; }
        public string AssessmentRuleCode { get; set; }
        public string AssessmentRuleName { get; set; }
        public decimal AssessmentAmount { get; set; }
    }
}
