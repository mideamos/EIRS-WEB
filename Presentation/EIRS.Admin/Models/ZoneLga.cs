using System;
using System.Collections.Generic;

namespace EIRS.Admin.Models
{
    public partial class ZoneLga
    {
        public int Id { get; set; }
        public string LgaName { get; set; }
        public string ZoneCode { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
