
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
    
public partial class usp_SearchNotification_Result
{

    public long NotificationID { get; set; }

    public string NotificationRefNo { get; set; }

    public Nullable<System.DateTime> NotificationDate { get; set; }

    public Nullable<int> NotificationMethodID { get; set; }

    public string NotificationMethodName { get; set; }

    public Nullable<int> NotificationTypeID { get; set; }

    public string NotificationTypeName { get; set; }

    public string EventRefNo { get; set; }

    public Nullable<int> TaxPayerTypeID { get; set; }

    public string TaxPayerTypeName { get; set; }

    public Nullable<int> TaxPayerID { get; set; }

    public string TaxPayerName { get; set; }

    public Nullable<bool> NotificationStatus { get; set; }

    public string NotificationStatusText { get; set; }

    public Nullable<int> NotificationModeID { get; set; }

    public string NotificationModeName { get; set; }

    public string NotificationContent { get; set; }

    public string TaxPayerRIN { get; set; }

}

}
