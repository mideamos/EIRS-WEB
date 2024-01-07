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
    
    public partial class MDA_Service_Items
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MDA_Service_Items()
        {
            this.MAP_MDAService_MDAServiceItem = new HashSet<MAP_MDAService_MDAServiceItem>();
            this.MAP_ServiceBill_MDAServiceItem = new HashSet<MAP_ServiceBill_MDAServiceItem>();
            this.MAP_ServiceBill_MDAServiceItem1 = new HashSet<MAP_ServiceBill_MDAServiceItem>();
        }
    
        public int MDAServiceItemID { get; set; }
        public string MDAServiceItemReferenceNo { get; set; }
        public Nullable<int> RevenueStreamID { get; set; }
        public Nullable<int> RevenueSubStreamID { get; set; }
        public Nullable<int> AssessmentItemCategoryID { get; set; }
        public Nullable<int> AssessmentItemSubCategoryID { get; set; }
        public Nullable<int> AgencyID { get; set; }
        public string MDAServiceItemName { get; set; }
        public int ComputationID { get; set; }
        public Nullable<decimal> ServiceBaseAmount { get; set; }
        public Nullable<decimal> Percentage { get; set; }
        public Nullable<decimal> ServiceAmount { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    
        public virtual Agency Agency { get; set; }
        public virtual Assessment_Item_Category Assessment_Item_Category { get; set; }
        public virtual Assessment_Item_SubCategory Assessment_Item_SubCategory { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MAP_MDAService_MDAServiceItem> MAP_MDAService_MDAServiceItem { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MAP_ServiceBill_MDAServiceItem> MAP_ServiceBill_MDAServiceItem { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MAP_ServiceBill_MDAServiceItem> MAP_ServiceBill_MDAServiceItem1 { get; set; }
        public virtual MST_Computation MST_Computation { get; set; }
        public virtual Revenue_Stream Revenue_Stream { get; set; }
        public virtual Revenue_SubStream Revenue_SubStream { get; set; }
    }
}
