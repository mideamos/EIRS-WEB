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
    
    public partial class MAP_TCCRequest_Generate_Field
    {
        public long RGFID { get; set; }
        public Nullable<int> PFID { get; set; }
        public Nullable<long> RGID { get; set; }
        public Nullable<int> FieldID { get; set; }
        public string FieldValue { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    
        public virtual MAP_TCCRequest_Generate MAP_TCCRequest_Generate { get; set; }
    }
}
