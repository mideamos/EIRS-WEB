//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EIRS.BOL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tax_Offices
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tax_Offices()
        {
            this.Governments = new HashSet<Government>();
            this.Individuals = new HashSet<Individual>();
            this.MAP_TaxOffice_Target = new HashSet<MAP_TaxOffice_Target>();
            this.MAP_TaxOfficer_Target = new HashSet<MAP_TaxOfficer_Target>();
            this.Specials = new HashSet<Special>();
            this.Companies = new HashSet<Company>();
        }
    
        public int TaxOfficeID { get; set; }
        public string TaxOfficeName { get; set; }
        public Nullable<int> AddressTypeID { get; set; }
        public Nullable<int> BuildingID { get; set; }
        public Nullable<int> Approver1 { get; set; }
        public Nullable<int> Approver2 { get; set; }
        public Nullable<int> Approver3 { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ZoneId { get; set; }
        public Nullable<int> OfficeManagerID { get; set; }
        public Nullable<int> IncomeDirector { get; set; }
    
        public virtual Address_Types Address_Types { get; set; }
        public virtual Building Building { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Government> Governments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Individual> Individuals { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MAP_TaxOffice_Target> MAP_TaxOffice_Target { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MAP_TaxOfficer_Target> MAP_TaxOfficer_Target { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Special> Specials { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Company> Companies { get; set; }
    }
}
