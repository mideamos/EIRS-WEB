
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
    
public partial class MAP_TCCRequest_Notes
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public MAP_TCCRequest_Notes()
    {

        this.MAP_TCCRequest_Notes_Document = new HashSet<MAP_TCCRequest_Notes_Document>();

    }


    public long RNID { get; set; }

    public Nullable<long> RequestID { get; set; }

    public Nullable<int> StageID { get; set; }

    public Nullable<int> StaffID { get; set; }

    public string Notes { get; set; }

    public Nullable<System.DateTime> NotesDate { get; set; }

    public Nullable<int> CreatedBy { get; set; }

    public Nullable<System.DateTime> CreatedDate { get; set; }

    public Nullable<int> ModifiedBy { get; set; }

    public Nullable<System.DateTime> ModifiedDate { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<MAP_TCCRequest_Notes_Document> MAP_TCCRequest_Notes_Document { get; set; }

}

}
