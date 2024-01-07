using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EIRS.Web.GISModels
{
    public partial class GISFileAssessment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long FileId { get; set; }
        public string FileNumber { get; set; }
        public string PageNo { get; set; }
        public long AssessmentID { get; set; }
        public string AssessmentYear { get; set; }
        public DateTime DateSaved { get; set; }
    }
}