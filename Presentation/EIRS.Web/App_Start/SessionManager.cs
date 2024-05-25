using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;
using EIRS.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web;

namespace EIRS.Web
{
    public class SessionManager
    {
        public static int TaxOfficeID
        {
            get { return TrynParse.parseInt(HttpContext.Current.Session["TaxOfficeID"]); }
            set { HttpContext.Current.Session["TaxOfficeID"] = value; }
        }
       public static int UserID
        {
            get { return TrynParse.parseInt(HttpContext.Current.Session["UserID"]); }
            set { HttpContext.Current.Session["UserID"] = value; }
        }
     
        public static bool CanSeeApproval
        {
            get { return TrynParse.parseBool(HttpContext.Current.Session["CanSeeApproval"]); }
            set { HttpContext.Current.Session["CanSeeApproval"] = value; }
        }
        

        public static int DataSubmitterID
        {
            get { return TrynParse.parseInt(HttpContext.Current.Session["DataSubmitterID"]); }
            set { HttpContext.Current.Session["DataSubmitterID"] = value; }
        }

        public static int TaxPayerID
        {
            get { return TrynParse.parseInt(HttpContext.Current.Session["TaxPayerID"]); }
            set { HttpContext.Current.Session["TaxPayerID"] = value; }
        }
        public static int TaxPayerIDForPoa
        {
            get { return TrynParse.parseInt(HttpContext.Current.Session["TaxPayerIDForPoa"]); }
            set { HttpContext.Current.Session["TaxPayerIDForPoa"] = value; }
        }

        public static int UserTypeID
        {
            get { return TrynParse.parseInt(HttpContext.Current.Session["UserTypeID"]); }
            set { HttpContext.Current.Session["UserTypeID"] = value; }
        }
               public static long GetAAIID
        {
            get { return TrynParse.parseInt(HttpContext.Current.Session["aaiid"]); }
            set { HttpContext.Current.Session["aaiid"] = value; }
        }

        public static int TaxPayerTypeID
        {
            get { return TrynParse.parseInt(HttpContext.Current.Session["TaxPayerTypeID"]); }
            set { HttpContext.Current.Session["TaxPayerTypeID"] = value; }
        }

        public static string UserName
        {
            get { return TrynParse.parseString(HttpContext.Current.Session["UserName"]); }
            set { HttpContext.Current.Session["UserName"] = value; }
        }

        public static string ContactName
        {
            get { return TrynParse.parseString(HttpContext.Current.Session["ContactName"]); }
            set { HttpContext.Current.Session["ContactName"] = value; }
        }
        public static string RIN
        {
            get { return TrynParse.parseString(HttpContext.Current.Session["RIN"]); }
            set { HttpContext.Current.Session["RIN"] = value; }
        }
        public static string EmailAddress
        {
            get { return TrynParse.parseString(HttpContext.Current.Session["EmailAddress"]); }
            set { HttpContext.Current.Session["EmailAddress"] = value; }
        }

        public static string ContactNumber
        {
            get { return TrynParse.parseString(HttpContext.Current.Session["ContactNumber"]); }
            set { HttpContext.Current.Session["ContactNumber"] = value; }
        }
        public static string Path
        {
            get;
            set;
        }

        public static IList<Building_BuildingUnit> LstBuildingUnit
        {
            get { return HttpContext.Current.Session["lstBuildingUnit"] != null ? JsonConvert.DeserializeObject<IList<Building_BuildingUnit>>(HttpContext.Current.Session["lstBuildingUnit"].ToString()) : null; }
            set { HttpContext.Current.Session["lstBuildingUnit"] = JsonConvert.SerializeObject(value); ; }
        }   
        public static string detHolder
        {
            get { return HttpContext.Current.Session["detHolder"] != null ? HttpContext.Current.Session["detHolder"].ToString() : ""; }
            set { HttpContext.Current.Session["detHolder"] = JsonConvert.SerializeObject(value); ; }
        }    
        public static IList<TaxReportModel> LstTaxReportModel
        {
            get { return HttpContext.Current.Session["lstTaxReportModel"] != null ? JsonConvert.DeserializeObject<IList<TaxReportModel>>(HttpContext.Current.Session["lstTaxReportModel"].ToString()) : null; }
            set { HttpContext.Current.Session["lstTaxReportModel"] = JsonConvert.SerializeObject(value); ; }
        }
        public static List<LateChargeResponse> LateChargeResponse
        {
            get { return HttpContext.Current.Session["LateChargeResponse"] != null ? JsonConvert.DeserializeObject<List<LateChargeResponse>>(HttpContext.Current.Session["LateChargeResponse"].ToString()) : null; }
            set { HttpContext.Current.Session["LateChargeResponse"] = JsonConvert.SerializeObject(value); ; }
        }
        public static IList<NewPoASettlementViewModel> LstPOS
        {
            get { return HttpContext.Current.Session["lstPOS"] != null ? JsonConvert.DeserializeObject<IList<NewPoASettlementViewModel>>(HttpContext.Current.Session["lstPOS"].ToString()) : null; }
            set { HttpContext.Current.Session["lstPOS"] = JsonConvert.SerializeObject(value); ; }
        }

       
        public static IList<MAP_Assessment_LateCharge> LstLa
        {
            get { return HttpContext.Current.Session["LstLa"] != null ? JsonConvert.DeserializeObject<IList<MAP_Assessment_LateCharge>>(HttpContext.Current.Session["LstLa"].ToString()) : null; }
            set { HttpContext.Current.Session["LstLa"] = JsonConvert.SerializeObject(value); }
        }

        public static IList<Assessment_AssessmentRule> lstAssessmentRule
        {
            get { return HttpContext.Current.Session["lstAssessmentRule"] != null ? JsonConvert.DeserializeObject<IList<Assessment_AssessmentRule>>(HttpContext.Current.Session["lstAssessmentRule"].ToString()) : null; }
            set { HttpContext.Current.Session["lstAssessmentRule"] = JsonConvert.SerializeObject(value); ; }
        }
        public static IList<Assessment> lstAssessment
        {
            get { return HttpContext.Current.Session["lstAssessment"] != null ? JsonConvert.DeserializeObject<IList<Assessment>>(HttpContext.Current.Session["lstAssessment"].ToString()) : null; }
            set { HttpContext.Current.Session["lstAssessment"] = JsonConvert.SerializeObject(value); ; }
        }

        public static IList<Assessment_AssessmentItem> lstAssessmentItem
        {
            get { return HttpContext.Current.Session["lstAssessmentItem"] != null ? JsonConvert.DeserializeObject<IList<Assessment_AssessmentItem>>(HttpContext.Current.Session["lstAssessmentItem"].ToString()) : null; }
            set { HttpContext.Current.Session["lstAssessmentItem"] = JsonConvert.SerializeObject(value); ; }
        }
        public static IList<usp_GetAssessment_AssessmentRuleList_Result> lstAssessmentRules
        {
            get { return HttpContext.Current.Session["lstAssessmentRules"] != null ? JsonConvert.DeserializeObject<IList<usp_GetAssessment_AssessmentRuleList_Result>>(HttpContext.Current.Session["lstAssessmentRules"].ToString()) : null; }
            set { HttpContext.Current.Session["lstAssessmentRules"] = JsonConvert.SerializeObject(value); ; }
        }

        public static IList<usp_GetAssessmentRuleItemList_Result> lstAssessmentItems
        {
            get { return HttpContext.Current.Session["lstAssessmentItems"] != null ? JsonConvert.DeserializeObject<IList<usp_GetAssessmentRuleItemList_Result>>(HttpContext.Current.Session["lstAssessmentItems"].ToString()) : null; }
            set { HttpContext.Current.Session["lstAssessmentItems"] = JsonConvert.SerializeObject(value); ; }
        }

        public static IList<ServiceBill_MDAService> lstMDAService
        {
            get { return HttpContext.Current.Session["lstMDAService"] != null ? JsonConvert.DeserializeObject<IList<ServiceBill_MDAService>>(HttpContext.Current.Session["lstMDAService"].ToString()) : null; }
            set { HttpContext.Current.Session["lstMDAService"] = JsonConvert.SerializeObject(value); ; }
        }

        public static IList<ServiceBill_MDAServiceItem> lstMDAServiceItem
        {
            get { return HttpContext.Current.Session["lstMDAServiceItem"] != null ? JsonConvert.DeserializeObject<IList<ServiceBill_MDAServiceItem>>(HttpContext.Current.Session["lstMDAServiceItem"].ToString()) : null; }
            set { HttpContext.Current.Session["lstMDAServiceItem"] = JsonConvert.SerializeObject(value); ; }
        }

        public static IList<AssetDetails> lstAssetDetails
        {
            get { return HttpContext.Current.Session["lstAssetDetails"] != null ? JsonConvert.DeserializeObject<IList<AssetDetails>>(HttpContext.Current.Session["lstAssetDetails"].ToString()) : null; }
            set { HttpContext.Current.Session["lstAssetDetails"] = JsonConvert.SerializeObject(value); ; }
        }

        public static IList<usp_GetAssessmentRuleBasedSettlement_Result> lstAssessmentRuleSettlement
        {
            get { return HttpContext.Current.Session["lstAssessmentRuleSettlement"] != null ? JsonConvert.DeserializeObject<IList<usp_GetAssessmentRuleBasedSettlement_Result>>(HttpContext.Current.Session["lstAssessmentRuleSettlement"].ToString()) : null; }
            set { HttpContext.Current.Session["lstAssessmentRuleSettlement"] = JsonConvert.SerializeObject(value); ; }
        }

        public static IList<usp_GetMDAServiceBasedSettlement_Result> lstMDAServiceSettlement
        {
            get { return HttpContext.Current.Session["lstMDAServiceSettlement"] != null ? JsonConvert.DeserializeObject<IList<usp_GetMDAServiceBasedSettlement_Result>>(HttpContext.Current.Session["lstMDAServiceSettlement"].ToString()) : null; }
            set { HttpContext.Current.Session["lstMDAServiceSettlement"] = JsonConvert.SerializeObject(value); ; }
        }

        public static IList<Request_IncomeStream> LstIncomeStream
        {
            get { return HttpContext.Current.Session["lstIncomeStream"] != null ? JsonConvert.DeserializeObject<IList<Request_IncomeStream>>(HttpContext.Current.Session["lstIncomeStream"].ToString()) : null; }
            set { HttpContext.Current.Session["lstIncomeStream"] = JsonConvert.SerializeObject(value); }
        }
        public static IList<NewERASTccHolder> LstErasTccHolder
        {
            get { return HttpContext.Current.Session["lstErasTccHolder"] != null ? JsonConvert.DeserializeObject<IList<NewERASTccHolder>>(HttpContext.Current.Session["lstErasTccHolder"].ToString()) : null; }
            set { HttpContext.Current.Session["lstErasTccHolder"] = JsonConvert.SerializeObject(value); }
        }

        public static IList<Request_TCCDetail> LstTCCDetail
        {
            get { return HttpContext.Current.Session["LstTCCDetail"] != null ? JsonConvert.DeserializeObject<IList<Request_TCCDetail>>(HttpContext.Current.Session["LstTCCDetail"].ToString()) : null; }
            set { HttpContext.Current.Session["LstTCCDetail"] = JsonConvert.SerializeObject(value); }
        }
        public static IList<usp_GetTaxPayerLiabilityForTCC_Result> lstLaibility
        {
            get { return HttpContext.Current.Session["lstLaibility"] != null ? JsonConvert.DeserializeObject<IList<usp_GetTaxPayerLiabilityForTCC_Result>>(HttpContext.Current.Session["lstLaibility"].ToString()) : null; }
            set { HttpContext.Current.Session["lstLaibility"] = JsonConvert.SerializeObject(value); }
        }
        public static IList<usp_GetTCCDetail_Result> LstTCCDetailNew
        {
            get { return HttpContext.Current.Session["LstTCCDetailNew"] != null ? JsonConvert.DeserializeObject<IList<usp_GetTCCDetail_Result>>(HttpContext.Current.Session["LstTCCDetailNew"].ToString()) : null; }
            set { HttpContext.Current.Session["LstTCCDetailNew"] = JsonConvert.SerializeObject(value); }
        }
        public static IList<usp_GetTaxPayerPaymentForTCCNEW_Result> LstTCCTaxPayerPayment
        {
            get { return HttpContext.Current.Session["LstTCCTaxPayerPayment"] != null ? JsonConvert.DeserializeObject<IList<usp_GetTaxPayerPaymentForTCCNEW_Result>>(HttpContext.Current.Session["LstTCCTaxPayerPayment"].ToString()) : null; }
            set { HttpContext.Current.Session["LstTCCTaxPayerPayment"] = JsonConvert.SerializeObject(value); }
        }
        public static IList<PayeApiResponse> LstPayeApiResponse
        {
            get { return HttpContext.Current.Session["LstPayeApiResponse"] != null ? JsonConvert.DeserializeObject<IList<PayeApiResponse>>(HttpContext.Current.Session["LstPayeApiResponse"].ToString()) : null; }
            set { HttpContext.Current.Session["LstPayeApiResponse"] = JsonConvert.SerializeObject(value); }
        }

        public static IList<CertificateTypeItemViewModel> LstCertificateTypeItem
        {
            get { return HttpContext.Current.Session["LstCertificateTypeItem"] != null ? JsonConvert.DeserializeObject<IList<CertificateTypeItemViewModel>>(HttpContext.Current.Session["LstCertificateTypeItem"].ToString()) : null; }
            set { HttpContext.Current.Session["LstCertificateTypeItem"] = JsonConvert.SerializeObject(value); }
        }
        public static List<usp_GetScreenUserList_Result> LstScreensToSee
        {
            get { return HttpContext.Current.Session["LstScreensToSee"] != null ? JsonConvert.DeserializeObject<List<usp_GetScreenUserList_Result>>(HttpContext.Current.Session["LstScreensToSee"].ToString()) : null; }
            set { HttpContext.Current.Session["LstScreensToSee"] = JsonConvert.SerializeObject(value); }
        }
        public static List<map_assessmet_rule_response> LstmAAR
        {
            get { return HttpContext.Current.Session["LstmAAR"] != null ? JsonConvert.DeserializeObject<List<map_assessmet_rule_response>>(HttpContext.Current.Session["LstmAAR"].ToString()) : null; }
            set { HttpContext.Current.Session["LstmAAR"] = JsonConvert.SerializeObject(value); }
        }
        public static List<usp_GetLateChargeList_Result> Lstlcr
        {
            get { return HttpContext.Current.Session["Lstlcr"] != null ? JsonConvert.DeserializeObject<List<usp_GetLateChargeList_Result>>(HttpContext.Current.Session["Lstlcr"].ToString()) : null; }
            set { HttpContext.Current.Session["Lstlcr"] = JsonConvert.SerializeObject(value); }
        }
        public static List<usp_GetAssessmentRuleItemListForLateCharges_Result> LstRil
        {
            get { return HttpContext.Current.Session["LstRil"] != null ? JsonConvert.DeserializeObject<List<usp_GetAssessmentRuleItemListForLateCharges_Result>>(HttpContext.Current.Session["LstRil"].ToString()) : null; }
            set { HttpContext.Current.Session["LstRil"] = JsonConvert.SerializeObject(value); }
        }
        public static List<MAP_PaymentAccount_Operation> LstPaO
        {
            get { return HttpContext.Current.Session["LstPaO"] != null ? JsonConvert.DeserializeObject<List<MAP_PaymentAccount_Operation>>(HttpContext.Current.Session["LstPaO"].ToString()) : null; }
            set { HttpContext.Current.Session["LstPaO"] = JsonConvert.SerializeObject(value); }
        }
        public static Late_Charges lCharge
        {
            get { return HttpContext.Current.Session["lCharge"] != null ? JsonConvert.DeserializeObject<Late_Charges>(HttpContext.Current.Session["lCharge"].ToString()) : null; }
            set { HttpContext.Current.Session["lCharge"] = JsonConvert.SerializeObject(value); }
        }
        public static PoaMyClass poaMyClass
        {
            get { return HttpContext.Current.Session["poaMyClass"] != null ? JsonConvert.DeserializeObject<PoaMyClass>(HttpContext.Current.Session["poaMyClass"].ToString()) : null; }
            set { HttpContext.Current.Session["poaMyClass"] = JsonConvert.SerializeObject(value); }
        }
        public static IList<BusinessNameHolder> businessNameHolderList
        {
            get { return HttpContext.Current.Session["businessNameHolder"] != null ? JsonConvert.DeserializeObject<IList<BusinessNameHolder>>(HttpContext.Current.Session["businessNameHolder"].ToString()) : null; }
            set { HttpContext.Current.Session["businessNameHolder"] = JsonConvert.SerializeObject(value); }
        }
        public static string revenueName
        {
            get { return HttpContext.Current.Session["revenueName"] != null ? JsonConvert.DeserializeObject<string>(HttpContext.Current.Session["revenueName"].ToString()) : null; }
            set { HttpContext.Current.Session["revenueName"] = JsonConvert.SerializeObject(value); }
        }
        public static string revenueTaxYear
        {
            get { return HttpContext.Current.Session["revenueTaxYear"] != null ? JsonConvert.DeserializeObject<string>(HttpContext.Current.Session["revenueTaxYear"].ToString()) : null; }
            set { HttpContext.Current.Session["revenueTaxYear"] = JsonConvert.SerializeObject(value); }
        }
        public static IList<CertificateTypeFieldViewModel> LstCertificateTypeField
        {
            get { return HttpContext.Current.Session["LstCertificateTypeField"] != null ? JsonConvert.DeserializeObject<IList<CertificateTypeFieldViewModel>>(HttpContext.Current.Session["LstCertificateTypeField"].ToString()) : null; }
            set { HttpContext.Current.Session["LstCertificateTypeField"] = JsonConvert.SerializeObject(value); }
        }

        public static IList<FileUploadData> LstMessageAttachment
        {
            get { return HttpContext.Current.Session["LstMessageAttachment"] != null ? JsonConvert.DeserializeObject<IList<FileUploadData>>(HttpContext.Current.Session["LstMessageAttachment"].ToString()) : null; }
            set { HttpContext.Current.Session["LstMessageAttachment"] = JsonConvert.SerializeObject(value); }
        }
        public static IList<TicketRef> LstTicketRef
        {
            get { return HttpContext.Current.Session["LstTicketRef"] != null ? JsonConvert.DeserializeObject<IList<TicketRef>>(HttpContext.Current.Session["LstTicketRef"].ToString()) : null; }
            set { HttpContext.Current.Session["LstTicketRef"] = JsonConvert.SerializeObject(value); }
        }

        public static IList<FileUploadData> LstTCCNotesAttachment
        {
            get { return HttpContext.Current.Session["LstTCCNotesAttachment"] != null ? JsonConvert.DeserializeObject<IList<FileUploadData>>(HttpContext.Current.Session["LstTCCNotesAttachment"].ToString()) : null; }
            set { HttpContext.Current.Session["LstTCCNotesAttachment"] = JsonConvert.SerializeObject(value); }
        }
    }
    public class newServiceBillIdsRequest
    {
        public string AssID { get; set; }
        public string ServiceId { get; set; }
    }

    public class TicketRef
    {
        public string TickRefNo { get; set; }
        public string TaxYear { get; set; }
        public string PaymentDate { get; set; }
    }
    public class BusinessNameHolder
    {
        public string BusinessName { get; set; }
    }
    public class map_assessmet_rule_response
    {
        public long AARID { get; set; }
        public Nullable<long> AssessmentID { get; set; }
        public Nullable<int> AssetTypeID { get; set; }
        public Nullable<int> AssetID { get; set; }
        public Nullable<int> ProfileID { get; set; }
        public Nullable<int> AssessmentRuleID { get; set; }
        public Nullable<int> AssessmentYear { get; set; }
        public Nullable<decimal> AssessmentAmount { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }

    }
    public class Building_BuildingUnit
    {
        public int RowID { get; set; }

        public int BuildingUnitID { get; set; }

        public string UnitNumber { get; set; }

        public int UnitPurposeID { get; set; }
        public string UnitPurposeName { get; set; }

        public int UnitFunctionID { get; set; }
        public string UnitFunctionName { get; set; }

        public int UnitOccupancyID { get; set; }
        public string UnitOccupancyName { get; set; }

        public int UnitSizeID { get; set; }
        public string UnitSizeName { get; set; }

        public EnumList.Track intTrack { get; set; }
    }
    public class PoaMyClass
    {
        public long PaymentAccountID { get; set; }
        public decimal RA { get; set; }
        public decimal SA { get; set; }
        public decimal BA { get; set; }
        public string TransactionRefNo { get; set; }

    }
    public class NewPoASettlementViewModel
    {
        public long AssessmentID { get; set; }

        public long ServiceBillID { get; set; }

        public int TaxPayerID { get; set; }

        public string TaxPayerRIN { get; set; }

        public string TaxPayerName { get; set; }

        public int? TaxPayerTypeID { get; set; }
        public string TaxPayerTypeName { get; set; }
        public string Notes { get; set; }

        public DateTime? BillDate { get; set; }

        public DateTime? DueDate { get; set; }

        public string BillRefNo { get; set; }

        public string StatusName { get; set; }

        public string BillNotes { get; set; }

        public decimal? BillAmount { get; set; }
        public decimal? AmountToPay { get; set; }

        public decimal? TotalPaid { get; set; }
        public int? HolderId { get; set; }

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

        public decimal UnSettledAmount { get; set; }

        public decimal? SettledAmount { get; set; }

        public decimal? ToSettleAmount { get; set; }

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

        public int ComputationID { get; set; }

        public string ComputationName { get; set; }

        public decimal TaxAmount { get; set; }
        public decimal AdjustmentAmount { get; set; }
        public decimal LateChargeAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Percentage { get; set; }
        public decimal SettlementAmount { get; set; }
        public decimal UnSettledAmount { get; set; }
        public decimal ToSettleAmount { get; set; }

        public EnumList.Track intTrack { get; set; }

        public int PaymentStatusID { get; set; }
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

        public decimal UnSettledAmount { get; set; }

        public decimal? SettledAmount { get; set; }

        public decimal? ToSettleAmount { get; set; }

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
        public string ComputationName { get; set; }
        public decimal ServiceAmount { get; set; }
        public decimal AdjustmentAmount { get; set; }
        public decimal LateChargeAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Percentage { get; set; }
        public EnumList.Track intTrack { get; set; }
        public decimal SettlementAmount { get; set; }
        public decimal UnSettledAmount { get; set; }
        public decimal ToSettleAmount { get; set; }
        public int PaymentStatusID { get; set; }
    }
    public class PayeApiFullResponse
    {
        [JsonProperty("status")]
        public bool Status { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("result")]
        public PayeApiResponse[] Result { get; set; }

    }
    public class PayeApiResponse
    {

        public EnumList.Track intTrack { get; set; }
        public int RowID { get; set; }

        [JsonProperty("employerName")]
        public string EmployerName { get; set; } = null;

        [JsonProperty("employerAddress")]
        public string EmployerAddress { get; set; } = null;

        [JsonProperty("employerRin")] public string EmployerRin { get; set; } = null;

        [JsonProperty("startMonth")] public string StartMonth { get; set; } = null;

        [JsonProperty("nationality")] public string Nationality { get; set; } = null;

        [JsonProperty("title")] public string Title { get; set; } = null;

        [JsonProperty("firstName")] public string FirstName { get; set; } = null;

        [JsonProperty("middleName")] public string MiddleName { get; set; } = null;

        [JsonProperty("surname")] public string Surname { get; set; } = null;

        [JsonProperty("employeeRin")] public string EmployeeRin { get; set; } = null;

        [JsonProperty("employeeTin")]
        public object EmployeeTin { get; set; } = null;

        [JsonProperty("annualGross")]
        public double AnnualGross { get; set; } = 0.0;
        [JsonProperty("cra")]
        public double Cra { get; set; } = 0.0;
        [JsonProperty("validatedPension")]
        public double ValidatedPension { get; set; } = 0.0;
        [JsonProperty("validatedNhf")]
        public double ValidatedNhf { get; set; } = 0.0;
        [JsonProperty("validatedNhis")]
        public double ValidatedNhis { get; set; } = 0.0;
        [JsonProperty("taxFreePay")]
        public double TaxFreePay { get; set; } = 0.0;
        [JsonProperty("chargeableIncome")]
        public double ChargeableIncome { get; set; } = 0.0;
        [JsonProperty("annualTax")]
        public double AnnualTax { get; set; } = 0.0;
        public double AnnualTaxII { get; set; } = 0.0;
        [JsonProperty("monthlyTax")]
        public double MonthlyTax { get; set; } = 0.0;
        [JsonProperty("assessmentYear")]
        public string AssessmentYear { get; set; } = null;
        public int TaxYear { get; set; } = 0;
        [JsonProperty("status")]
        public int Status { get; set; } = 0;
        [JsonProperty("endMonth")]
        public string EndMonth { get; set; } = null;
        public string ReceiptRef { get; set; }
        public string ReceiptDate { get; set; }
        public string ReceiptDetail { get; set; }
        //[JsonProperty("id")]
        //public int Id { get; set; } = 0;
        //[JsonProperty("apiId")]
        //public int ApiId { get; set; } = 0;
        //public string? AssetRin { get; set; }
        //[JsonProperty("annualRent")]
        //public double AnnualRent { get; set; } = 0.0;

        //[JsonProperty("annualTransport")]
        //public long AnnualTransport { get; set; } = 0;

        //[JsonProperty("annualUtility")]
        //public long AnnualUtility { get; set; } = 0;

        //[JsonProperty("annualMeal")] public double AnnualMeal { get; set; } = 0.0;

        //[JsonProperty("otherAllowancesAnnual")] public long OtherAllowancesAnnual { get; set; } = 0;

        //[JsonProperty("leaveTransportAnnual")] public long LeaveTransportAnnual { get; set; } = 0;

        //[JsonProperty("annualGross")] public double AnnualGross { get; set; } = 0.0;

        //[JsonProperty("pension")] public long Pension { get; set; } = 0;

        //[JsonProperty("nhf")] public long Nhf { get; set; } = 0;

        //[JsonProperty("nhis")] public long Nhis { get; set; } = 0;

        //[JsonProperty("assessmentYear")] public long AssessmentYear { get; set; } = 0;

        //[JsonProperty("endMonth")]
        //public string EndMonth { get; set; } = null;   

        ////[JsonProperty("dateCreated")]
        ////public DateTimeOffset? DateCreated { get; set; }

        //[JsonProperty("id")] public long Id { get; set; } = 0;

        //[JsonProperty("apiId")]
        //public object ApiId { get; set; }

        //[JsonProperty("statusId")] public long StatusId { get; set; } = 0;

        //[JsonProperty("assetRin")]
        //public string AssetRin { get; set; } = null;
    }


    //public class PayeOuputFile
    //{
    //    public string? EmployerName { get; set; }
    //    public string? EmployerAddress { get; set; }
    //    public string? EmployerRin { get; set; }
    //    public string? StartMonth { get; set; }
    //    public string? Nationality { get; set; }
    //    public string? Title { get; set; }
    //    public string? FirstName { get; set; }
    //    public string? MiddleName { get; set; }
    //    public string? Surname { get; set; }
    //    public string? EmployeeRin { get; set; }
    //    public string? EmployeeTin { get; set; }
    //    public double? AnnualGross { get; set; }
    //    public double? Cra { get; set; }
    //    public double? ValidatedPension { get; set; }
    //    public double? ValidatedNhf { get; set; }
    //    public double? ValidatedNhis { get; set; }
    //    public double? TaxFreePay { get; set; }
    //    public double? ChargeableIncome { get; set; }
    //    public double? AnnualTax { get; set; }
    //    public double? MonthlyTax { get; set; }
    //    public string? AssessmentYear { get; set; }
    //    public int? Status { get; set; }
    //    public string? EndMonth { get; set; }
    //    public DateTime? DateCreated { get; set; }
    //    public int? Id { get; set; }
    //    public int? ApiId { get; set; }
    //    public string? AssetRin { get; set; }
    //}
    public class AssetDetails
    {
        public int RowID { get; set; }

        public long TPAID { get; set; }

        public int AssetTypeID { get; set; }

        public string AssetTypeName { get; set; }

        public int TaxPayerRoleID { get; set; }

        public string TaxPayerRoleName { get; set; }

        public int AssetID { get; set; }

        public string AssetLGA { get; set; }

        public string AssetRIN { get; set; }

        public string AssetName { get; set; }

        public int BuildingUnitID { get; set; }

        public string UnitNumber { get; set; }

        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }

    public class Request_IncomeStream
    {
        public int RowID { get; set; }

        public long TBKID { get; set; }

        public int TaxYear { get; set; }

        public decimal TotalIncomeEarned { get; set; }

        public int TaxPayerRoleID { get; set; }

        public string TaxPayerRoleName { get; set; }

        public int BusinessID { get; set; }

        public string BusinessName { get; set; }

        public int BusinessTypeID { get; set; }

        public string BusinessTypeName { get; set; }

        public int LGAID { get; set; }

        public string LGAName { get; set; }

        public int BusinessOperationID { get; set; }

        public string BusinessOperationName { get; set; }

        public string BusinessAddress { get; set; }

        public string BusinessNumber { get; set; }

        public string ContactPersonName { get; set; }

        public string Notes { get; set; }

        public EnumList.Track intTrack { get; set; }
    }

    public class Request_TCCDetail
    {
        public int RowID { get; set; }

        public long TBKID { get; set; }

        public int TaxYear { get; set; }
        public decimal AssessableIncome { get; set; }
        public decimal TCCTaxPaid { get; set; }
        public decimal ERASTaxPaid { get; set; }
        public decimal ERASAssessed { get; set; }
        public string Tax_receipt { get; set; }
        public EnumList.Track intTrack { get; set; }
    }

    public sealed partial class CertificateTypeItemViewModel
    {
        public int CTIID { get; set; }

        public int RowID { get; set; }

        public int ItemTypeID { get; set; }

        public string ItemTypeName { get; set; }

        public int ItemID { get; set; }

        public string ItemName { get; set; }

        public EnumList.Track IntTrack { get; set; }

    }

    public sealed partial class CertificateTypeFieldViewModel
    {
        public int CTFID { get; set; }

        public int RowID { get; set; }

        public string FieldName { get; set; }

        public int FieldTypeID { get; set; }

        public string FieldTypeName { get; set; }

        public string FieldComboValue { get; set; }

        public bool IsRequired { get; set; }

        public string RequiredText { get; set; }

        public EnumList.Track IntTrack { get; set; }

    }
}