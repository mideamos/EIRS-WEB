
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
    
public partial class MAP_TaxPayer_Asset_Profile
{

    public long TPAPID { get; set; }

    public Nullable<long> TPAID { get; set; }

    public Nullable<int> ProfileID { get; set; }

    public Nullable<int> CreatedBy { get; set; }

    public Nullable<System.DateTime> CreatedDate { get; set; }

    public Nullable<int> ModifiedBy { get; set; }

    public Nullable<System.DateTime> ModifiedDate { get; set; }



    public virtual MAP_TaxPayer_Asset MAP_TaxPayer_Asset { get; set; }

    public virtual Profile Profile { get; set; }

}

}
