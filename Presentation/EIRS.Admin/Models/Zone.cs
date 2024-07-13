using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace EIRS.Admin.Models
{
    public partial class Zone
    {
        public int ZoneId { get; set; }
        public string ZoneName { get; set; }
        public string ZoneCode { get; set; }
        [NotMapped]
        public string ActiveText { get ; set; } = "InActive";
        [NotMapped]
        public List<ZoneLga> LgaNames { get; set; }
        [NotMapped]
        public int LgaId { get; set; }
        [NotMapped]
        public int[] TaxOfficeId { get; set; }
        [NotMapped]
        public List<TaxOffice> TaxOffices { get; set; }
        public bool? Active { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
