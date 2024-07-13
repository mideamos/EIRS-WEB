using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIRS.Admin.Models.ViewModel
{
    public class AssessmentAndItemRollOver
    {
        public int Id { get; set; }
        public bool Active { get; set; }
        public string TaxYear { get; set; }
        public string TaxMonth { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal Percentage { get; set; }
        public decimal TaxBaseAmount { get; set; }
        public string AssessmentItemName { get; set; }
        public string AssessmentRuleCode { get; set; }
        public string AssessmentRuleName { get; set; }
        public decimal AssessmentAmount { get; set; }
    }
}