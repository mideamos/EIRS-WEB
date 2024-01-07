using System;
using System.Collections.Generic;

namespace EIRS.Admin.Models
{
    public partial class TaxOffice
    {
        //public TaxOffice()
        //{
        //    Companies = new HashSet<Company>();
        //    Governments = new HashSet<Government>();
        //    Individuals = new HashSet<Individual>();
        //    MapTaxOfficeTargets = new HashSet<MapTaxOfficeTarget>();
        //    MapTaxOfficerTargets = new HashSet<MapTaxOfficerTarget>();
        //    Specials = new HashSet<Special>();
        //}

        public int TaxOfficeId { get; set; }
        public string TaxOfficeName { get; set; }
        public int AddressTypeId { get; set; }
        public int BuildingId { get; set; }
        public int Approver1 { get; set; }
        public int Approver2 { get; set; }
        public int Approver3 { get; set; }
        public int ZoneId { get; set; }
        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }

      
    }
}
