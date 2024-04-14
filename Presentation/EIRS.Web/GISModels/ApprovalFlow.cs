using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIRS.Web.GISModels
{
    public class ApprovalFlow
    {
        public string Rin { get; set; }
        public string AssessmentRefNo { get; set; }
        public string TaxPayerName { get; set; }
        public double Amount { get; set; }
        public string Status { get; set; }
        public long AssessmentId { get; set; }
        public long? Id { get; set; }
    }
}