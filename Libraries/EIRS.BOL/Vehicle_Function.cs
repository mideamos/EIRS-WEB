
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
    
public partial class Vehicle_Function
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Vehicle_Function()
    {

        this.Vehicles = new HashSet<Vehicle>();

    }


    public int VehicleFunctionID { get; set; }

    public string VehicleFunctionName { get; set; }

    public Nullable<int> VehiclePurposeID { get; set; }

    public Nullable<bool> Active { get; set; }

    public Nullable<int> CreatedBy { get; set; }

    public Nullable<System.DateTime> CreatedDate { get; set; }

    public Nullable<int> ModifiedBy { get; set; }

    public Nullable<System.DateTime> ModifiedDate { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Vehicle> Vehicles { get; set; }

    public virtual Vehicle_Purpose Vehicle_Purpose { get; set; }

}

}
