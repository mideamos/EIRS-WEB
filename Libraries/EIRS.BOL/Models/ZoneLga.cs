using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace EIRS.BOL.Models
{
    public partial class ZoneLga
    {
        public int Id { get; set; }
        public string LgaName { get; set; }
        public string ZoneCode { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
