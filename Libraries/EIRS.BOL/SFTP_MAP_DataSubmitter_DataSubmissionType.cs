
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
    
public partial class SFTP_MAP_DataSubmitter_DataSubmissionType
{

    public long DSTDSID { get; set; }

    public Nullable<int> DataSubmissionTypeID { get; set; }

    public Nullable<int> DataSubmitterID { get; set; }

    public Nullable<bool> Active { get; set; }

    public Nullable<int> CreatedBy { get; set; }

    public Nullable<System.DateTime> CreatedDate { get; set; }



    public virtual SFTP_DataSubmissionType SFTP_DataSubmissionType { get; set; }

    public virtual SFTP_DataSubmitter SFTP_DataSubmitter { get; set; }

}

}
