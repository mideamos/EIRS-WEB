using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace EIRS.Admin.Models
{
    public partial class TempAssHolder
    {
        public int Id { get; set; }
        public string AssessmentRuleCode { get; set; }
        public int? AssessmentRuleId { get; set; }
    }
}
