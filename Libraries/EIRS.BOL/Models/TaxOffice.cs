using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace EIRS.BOL.Models
{
    public partial class TaxOffice
    {
        public int TaxOfficeId { get; set; }
        public string TaxOfficeName { get; set; }
        public int? AddressTypeId { get; set; }
        public string ZoneCode { get; set; }
    }
}
