using EIRS.Admin.Models;
using EIRS.BOL;
using EIRS.Common;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Web;


namespace EIRS.Admin
{
    public class SessionManager
    {
        public static int SystemUserID
        {
            get { return TrynParse.parseInt(HttpContext.Current.Session["SystemUserID"]); }
            set { HttpContext.Current.Session["SystemUserID"] = value; }
        }

        public static int SystemRoleID
        {
            get { return TrynParse.parseInt(HttpContext.Current.Session["SystemRoleID"]); }
            set { HttpContext.Current.Session["SystemRoleID"] = value; }
        }

        public static string UserName
        {
            get { return TrynParse.parseString(HttpContext.Current.Session["UserName"]); }
            set { HttpContext.Current.Session["UserName"] = value; }
        }


        public static string UserEmail
        {
            get { return TrynParse.parseString(HttpContext.Current.Session["UserEmail"]); }
            set { HttpContext.Current.Session["UserEmail"] = value; }
        }

        public static string UserContact
        {
            get { return TrynParse.parseString(HttpContext.Current.Session["UserContact"]); }
            set { HttpContext.Current.Session["UserContact"] = value; }
        }

        public static IList<TaxOfficeAddress> lstTaxOfficeAddress
        {
            get { return HttpContext.Current.Session["lstTaxOfficeAddress"] != null ? JsonConvert.DeserializeObject<IList<TaxOfficeAddress>>(HttpContext.Current.Session["lstTaxOfficeAddress"].ToString()) : null; }
            set { HttpContext.Current.Session["lstTaxOfficeAddress"] = JsonConvert.SerializeObject(value); }
        }
        public static IList<TempAssHolder> lstTempAssHolder
        {
            get { return HttpContext.Current.Session["lstTempAssHolder"] != null ? JsonConvert.DeserializeObject<IList<TempAssHolder>>(HttpContext.Current.Session["lstTempAssHolder"].ToString()) : null; }
            set { HttpContext.Current.Session["lstTempAssHolder"] = JsonConvert.SerializeObject(value);}
        }

        public static IList<AssessmentRule_Profile> lstAssessmentRuleProfile
        {
            get { return HttpContext.Current.Session["lstAssessmentRuleProfile"] != null ? JsonConvert.DeserializeObject<IList<AssessmentRule_Profile>>(HttpContext.Current.Session["lstAssessmentRuleProfile"].ToString()) : null; }
            set { HttpContext.Current.Session["lstAssessmentRuleProfile"] = JsonConvert.SerializeObject(value); ; }
        } 
        public static IList<AssessmentAndItemRollOver> lstAssessmentAndItemRollOver
        {
            get { return HttpContext.Current.Session["lstAssessmentAndItemRollOver"] != null ? JsonConvert.DeserializeObject<IList<AssessmentAndItemRollOver>>(HttpContext.Current.Session["lstAssessmentRuleProfile"].ToString()) : null; }
            set { HttpContext.Current.Session["lstAssessmentAndItemRollOver"] = JsonConvert.SerializeObject(value); ; }
        }

        public static IList<AssessmentRule_AssessmentItem> lstAssessmentRuleItem
        {
            get { return HttpContext.Current.Session["lstAssessmentRuleItem"] != null ? JsonConvert.DeserializeObject<IList<AssessmentRule_AssessmentItem>>(HttpContext.Current.Session["lstAssessmentRuleItem"].ToString()) : null; }
            set { HttpContext.Current.Session["lstAssessmentRuleItem"] = JsonConvert.SerializeObject(value); ; }
        }

        public static IList<Assessment_AssessmentRule> lstAssessmentRule
        {
            get { return HttpContext.Current.Session["lstAssessmentRule"] != null ? JsonConvert.DeserializeObject<IList<Assessment_AssessmentRule>>(HttpContext.Current.Session["lstAssessmentRule"].ToString()) : null; }
            set { HttpContext.Current.Session["lstAssessmentRule"] = JsonConvert.SerializeObject(value); ; }
        } 
        public static IList<Assessment_Rules> lstAssesRule
        {
            get { return HttpContext.Current.Session["lstAssesRule"] != null ? JsonConvert.DeserializeObject<IList<Assessment_Rules>>(HttpContext.Current.Session["lstAssesRule"].ToString()) : null; }
            set { HttpContext.Current.Session["lstAssesRule"] = JsonConvert.SerializeObject(value); ; }
        }

        public static IList<Assessment_AssessmentItem> lstAssessmentItem
        {
            get { return HttpContext.Current.Session["lstAssessmentItem"] != null ? JsonConvert.DeserializeObject<IList<Assessment_AssessmentItem>>(HttpContext.Current.Session["lstAssessmentItem"].ToString()) : null; }
            set { HttpContext.Current.Session["lstAssessmentItem"] = JsonConvert.SerializeObject(value); ; }
        }

        public static IList<Settlement_ASBItem> lstSettlementItem
        {
            get { return HttpContext.Current.Session["lstSettlementItem"] != null ? JsonConvert.DeserializeObject<IList<Settlement_ASBItem>>(HttpContext.Current.Session["lstSettlementItem"].ToString()) : null; }
            set { HttpContext.Current.Session["lstSettlementItem"] = JsonConvert.SerializeObject(value); ; }
        }

        public static IList<MDAService_MDAServiceItem> lstMDAServiceItem
        {
            get { return HttpContext.Current.Session["lstMDAServiceItem"] != null ? JsonConvert.DeserializeObject<IList<MDAService_MDAServiceItem>>(HttpContext.Current.Session["lstMDAServiceItem"].ToString()) : null; }
            set { HttpContext.Current.Session["lstMDAServiceItem"] = JsonConvert.SerializeObject(value); ; }
        }

        public static IList<ServiceBill_MDAService> lstServiceBillService
        {
            get { return HttpContext.Current.Session["lstServiceBillService"] != null ? JsonConvert.DeserializeObject<IList<ServiceBill_MDAService>>(HttpContext.Current.Session["lstServiceBillService"].ToString()) : null; }
            set { HttpContext.Current.Session["lstServiceBillService"] = JsonConvert.SerializeObject(value); ; }
        }

        public static IList<ServiceBill_MDAServiceItem> lstServiceBillItem
        {
            get { return HttpContext.Current.Session["lstServiceBillItem"] != null ? JsonConvert.DeserializeObject<IList<ServiceBill_MDAServiceItem>>(HttpContext.Current.Session["lstServiceBillItem"].ToString()) : null; }
            set { HttpContext.Current.Session["lstServiceBillItem"] = JsonConvert.SerializeObject(value); ; }
        }

    }


    public class TaxOfficeAddress
    {

        public int RowID { get; set; }

        public int AddressTypeID { get; set; }

        public string AddressTypeName { get; set; }

        public int BuildingID { get; set; }

        public string BuildingRIN { get; set; }

        public string BuildingName { get; set; }

        public EnumList.Track intTrack { get; set; }
    }

    public class AssessmentRule_Profile
    {
        public int RowID { get; set; }

        public int ProfileID { get; set; }

        public string ProfileReferenceNo { get; set; }

        public string AssetTypeName { get; set; }

        public EnumList.Track intTrack { get; set; }
    }

    public class AssessmentRule_AssessmentItem
    {
        public int RowID { get; set; }

        public int TablePKID { get; set; }

        public int AssessmentItemID { get; set; }

        public string AssessmentItemReferenceNo { get; set; }

        public string AssessmentItemName { get; set; }

        public decimal TaxAmount { get; set; }

        public EnumList.Track intTrack { get; set; }
    }

    public class Assessment_AssessmentRule
    {
        public int RowID { get; set; }

        public long TablePKID { get; set; }

        public int AssetTypeID { get; set; }

        public string AssetTypeName { get; set; }

        public int AssetID { get; set; }

        public string AssetRIN { get; set; }

        public int ProfileID { get; set; }

        public string ProfileDescription { get; set; }

        public int AssessmentRuleID { get; set; }

        public string AssessmentRuleName { get; set; }

        public int TaxYear { get; set; }

        public decimal AssessmentRuleAmount { get; set; }

        public EnumList.Track intTrack { get; set; }

    }

    public class Assessment_AssessmentItem
    {
        public int RowID { get; set; }

        public long TablePKID { get; set; }

        public int AssessmentRule_RowID { get; set; }

        public int AssessmentItemID { get; set; }

        public string AssessmentItemReferenceNo { get; set; }

        public string AssessmentItemName { get; set; }

        public decimal TaxBaseAmount { get; set; }

        public decimal ComputationID { get; set; }

        public decimal TaxAmount { get; set; }

        public decimal Percentage { get; set; }

        public EnumList.Track intTrack { get; set; }
    }

    public class Settlement_ASBItem
    {
        public int RowID { get; set; }

        public long TBPKID { get; set; }

        public string ASBName { get; set; }

        public string ItemName { get; set; }

        public int PaymentStatusID { get; set; }

        public string PaymentStatusName { get; set; }

        public decimal TaxAmount { get; set; }
        public decimal AdjustmentAmount { get; set; }
        public decimal LateChargeAmount { get; set; }
        public decimal TotalAmount { get; set; }

        public decimal SettlementAmount { get; set; }

        public decimal UnSettledAmount { get; set; }

        public decimal ToSettleAmount { get; set; }
    }

    public class MDAService_MDAServiceItem
    {
        public int RowID { get; set; }

        public int TablePKID { get; set; }

        public int MDAServiceItemID { get; set; }

        public string MDAServiceItemReferenceNo { get; set; }

        public string MDAServiceItemName { get; set; }

        public decimal ServiceAmount { get; set; }

        public EnumList.Track intTrack { get; set; }
    }


    public class ServiceBill_MDAService
    {
        public int RowID { get; set; }

        public long TablePKID { get; set; }

        public int MDAServiceID { get; set; }

        public string MDAServiceName { get; set; }

        public int TaxYear { get; set; }

        public decimal ServiceAmount { get; set; }

        public EnumList.Track intTrack { get; set; }

    }

    public class ServiceBill_MDAServiceItem
    {
        public int RowID { get; set; }

        public long TablePKID { get; set; }

        public int MDAService_RowID { get; set; }

        public int MDAServiceItemID { get; set; }

        public string MDAServiceItemReferenceNo { get; set; }

        public string MDAServiceItemName { get; set; }

        public decimal ServiceBaseAmount { get; set; }

        public decimal ComputationID { get; set; }

        public decimal ServiceAmount { get; set; }

        public decimal Percentage { get; set; }

        public EnumList.Track intTrack { get; set; }
    }
}