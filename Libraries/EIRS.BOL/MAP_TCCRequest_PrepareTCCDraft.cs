
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
    
public partial class MAP_TCCRequest_PrepareTCCDraft
{

    public long PTCCDraftID { get; set; }

    public Nullable<long> RequestID { get; set; }

    public string Notes { get; set; }

    public Nullable<int> CreatedBy { get; set; }

    public Nullable<System.DateTime> CreatedDate { get; set; }

    public Nullable<int> ModifiedBy { get; set; }

    public Nullable<System.DateTime> ModifiedDate { get; set; }



    public virtual TCC_Request TCC_Request { get; set; }

}

}
