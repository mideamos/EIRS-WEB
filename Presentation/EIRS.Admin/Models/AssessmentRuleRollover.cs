using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIRS.Admin.Models
{
    public class AssessmentRuleRollover
    {
        public int Id { get; set; }
        public int AssessmentRuleID { get; set; }
        public int NewAssessmentRuleID { get; set; }
        public string AssessmentRuleCode { get; set; }
        public int Profileid { get; set; }
        public string AssessmentRuleName { get; set; }
        public decimal AssessmentAmount { get; set; }
        public int Taxyear { get; set; }
        public int RuleRunId { get; set; }
        public int Paymentfrequencyid { get; set; }
        public string Active { get; set; }
        public int Createdby { get; set; }
        public int ARAIID { get; set; }
        public int AssessmentItemID { get; set; }
        public DateTime Createddate { get; set; }
    }
}