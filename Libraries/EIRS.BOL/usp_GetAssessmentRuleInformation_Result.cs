
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
    
public partial class usp_GetAssessmentRuleInformation_Result
{

    public Nullable<int> AssessmentRuleID { get; set; }

    public Nullable<int> TaxYear { get; set; }

    public string AssessmentRuleCode { get; set; }

    public string AssessmentRuleName { get; set; }

    public Nullable<decimal> AssessmentAmount { get; set; }

    public Nullable<decimal> BilledAmount { get; set; }

    public Nullable<int> ProfileID { get; set; }

    public string ProfileRefNo { get; set; }

    public string ProfileDescription { get; set; }

    public Nullable<int> AssetTypeID { get; set; }

    public string AssetTypeName { get; set; }

    public Nullable<int> AssetID { get; set; }

    public string AssetRIN { get; set; }

    public string AssetName { get; set; }

    public Nullable<int> TaxPayerRoleID { get; set; }

    public string TaxPayerRoleName { get; set; }

    public Nullable<decimal> SettledAmount { get; set; }

}

}
