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
    
    public partial class Certificate
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Certificate()
        {
            this.MAP_Certificate_CustomField = new HashSet<MAP_Certificate_CustomField>();
            this.MAP_Certificate_Generate = new HashSet<MAP_Certificate_Generate>();
            this.MAP_Certificate_Seal = new HashSet<MAP_Certificate_Seal>();
            this.MAP_Certificate_Validate = new HashSet<MAP_Certificate_Validate>();
            this.MAP_Certificate_Issue = new HashSet<MAP_Certificate_Issue>();
            this.MAP_Certificate_SignDigital = new HashSet<MAP_Certificate_SignDigital>();
            this.MAP_Certificate_Stages = new HashSet<MAP_Certificate_Stages>();
            this.MAP_Certificate_Revoke = new HashSet<MAP_Certificate_Revoke>();
        }
    
        public long CertificateID { get; set; }
        public string CertificateNumber { get; set; }
        public Nullable<System.DateTime> CertificateDate { get; set; }
        public Nullable<int> CertificateTypeID { get; set; }
        public Nullable<int> TaxPayerTypeID { get; set; }
        public Nullable<int> TaxPayerID { get; set; }
        public Nullable<int> ProfileID { get; set; }
        public Nullable<int> AssetTypeID { get; set; }
        public Nullable<int> AssetID { get; set; }
        public Nullable<int> StatusID { get; set; }
        public string OtherInformation { get; set; }
        public string Notes { get; set; }
        public Nullable<int> SignerID { get; set; }
        public Nullable<int> SignerRoleID { get; set; }
        public Nullable<int> QRCodeID { get; set; }
        public string QRImagePath { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public Nullable<int> VisibleSignStatusID { get; set; }
        public Nullable<int> PDFTemplateID { get; set; }
        public string GeneratedPath { get; set; }
        public string ValidatedPath { get; set; }
        public string SignedVisiblePath { get; set; }
        public string SignedDigitalPath { get; set; }
        public string SealedPath { get; set; }
        public Nullable<int> SEDE_DocumentID { get; set; }
        public Nullable<long> SEDE_OrderID { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string CertificatePath { get; set; }
        public Nullable<int> DisplayTypeID { get; set; }
    
        public virtual Certificate_Types Certificate_Types { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MAP_Certificate_CustomField> MAP_Certificate_CustomField { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MAP_Certificate_Generate> MAP_Certificate_Generate { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MAP_Certificate_Seal> MAP_Certificate_Seal { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MAP_Certificate_Validate> MAP_Certificate_Validate { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MAP_Certificate_Issue> MAP_Certificate_Issue { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MAP_Certificate_SignDigital> MAP_Certificate_SignDigital { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MAP_Certificate_Stages> MAP_Certificate_Stages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MAP_Certificate_Revoke> MAP_Certificate_Revoke { get; set; }
    }
}
