using EIRS.BOL;
using System;
using System.Collections.Generic;

namespace EIRS.Repository
{
    public interface IOperationManagerRepository
    {
        IList<usp_GetPaymentByRevenueStream_Result> REP_GetPaymentByRevenueStream(int? pIntPaymentTypeID, int pIntTaxYear, DateTime? pdtFromDate, DateTime? pdtToDate);
        IList<usp_GetPaymentByRevenueStreamDetail_Result> REP_GetPaymentByRevenueStreamDetails(int pIntTaxYear, DateTime? pdtFromDate, DateTime? pdtToDate, int? pIntPaymentTypeID, int? pIntRevenueStreamID);
        IList<usp_GetBillByRevenueStream_Result> REP_GetBillByRevenueStream(int pIntTaxYear, DateTime? FromDate, DateTime? ToDate, int pIntBillType, int pIntBillStatusID);
        IList<usp_GetBillByRevenueStreamDetail_Result> REP_GetBillByRevenueStreamDetails(int pIntTaxYear, DateTime? FromDate, DateTime? ToDate, int pIntBillType, int pIntBillStatusID, int? pIntRevenueStreamID);
        IList<usp_GetBillAgingByRevenueStream_Result> REP_GetBillAgingByRevenueStream(int? BillType, int? StageID);
        IList<usp_GetBillAgingDetailByRevenueStream_Result> REP_GetBillAgingDetailByRevenueStream(int? BillType, int? StageID, int? RevenueStreamID);
        
        IList<usp_GetPOATaxPayerWithoutAsset_Result> REP_GetPOATaxPayerWithoutAsset(int pIntTaxYear, DateTime? pdtFromDate, DateTime? pdtToDate, int pIntTaxPayerTypeID, int pIntTaxOfficeID);

        IList<usp_GetPaymentByTaxOffice_Result> REP_GetPaymentByTaxOffice(int? pIntPaymentTypeID, int pIntTaxYear, DateTime? pdtFromDate, DateTime? pdtToDate);
        IList<usp_GetPaymentByTaxOfficeDetail_Result> REP_GetPaymentByTaxOfficeDetails(int pIntTaxYear, DateTime? pdtFromDate, DateTime? pdtToDate, int? pIntPaymentTypeID, int? pIntTaxOfficeID);
        IList<usp_GetBillByTaxOffice_Result> REP_GetBillByTaxOffice(int pIntTaxYear, DateTime? FromDate, DateTime? ToDate, int pIntBillType, int pIntBillStatusID);
        IList<usp_GetBillByTaxOfficeDetail_Result> REP_GetBillByTaxOfficeDetails(int pIntTaxYear, DateTime? FromDate, DateTime? ToDate, int pIntBillType, int pIntBillStatusID, int? pIntTaxOfficeID);
        IList<usp_GetBillAgingByTaxOffice_Result> REP_GetBillAgingByTaxOffice(int? BillType, int? StageID);
        IList<usp_GetBillAgingDetailByTaxOffice_Result> REP_GetBillAgingDetailByTaxOffice(int? BillType, int? StageID, int? TaxOfficeID);


        ////QC_PD_T-ERAS_GetTaxPayerLiabilityStatus_12
        IList<usp_RPT_TaxPayerLiabilityStatus_Result> REP_GetTaxPayerLiabilityStatus(string TaxPayerRIN, int TaxYear, DateTime? FromDate, DateTime? ToDate);
        IList<usp_RPT_TaxPayerLiabilityStatus_Bills_Result> REP_GetTaxPayerLiabilityStatusDetails(int? intTaxPayerID, int? intTaxPayerTypeID, int TaxYear, DateTime? FromDate, DateTime? ToDate);
        IList<usp_RPT_TaxPayerLiabilityStatus_Payment_Result> REP_GetTaxPayerLiabilityStatusPaymentDetails(int? intTaxPayerID, int? intTaxPayerTypeID, int TaxYear, DateTime? FromDate, DateTime? ToDate);

        ////QC_PD_T-ERAS_GetRevenueStreamAssessmentsbyTaxOffice_13
        IList<usp_GetRevenueStreamBillByTaxOffice_Result> REP_GetRevenueStreamAssessmentsbyTaxOffice(int TaxYear, DateTime? FromDate, DateTime? ToDate, int? RevenueStreamID);
        IList<usp_GetRevenueStreamBillDetailByTaxOffice_Result> REP_GetRevenueStreamBillDetailByTaxOffice(int TaxYear, DateTime? FromDate, DateTime? ToDate, int? RevenueStreamID, int? TaxOfficeID);

        ////QC_PD_T-ERAS_GetPayeRevenueStreamAssessmentsbyTaxOffice_14
        IList<usp_GetPAYERevenueStreamBillByTaxOffice_Result> REP_GetPayeRevenueStreamAssessmentsbyTaxOffice(int TaxYear, DateTime? FromDate, DateTime? ToDate);
        IList<usp_GetPAYERevenueStreamBillDetailByTaxOffice_Result> REP_GetPayeRevenueStreamBillDetailbyTaxOffice(int TaxYear, DateTime? FromDate, DateTime? ToDate, int? TaxOfficeID);


        IList<usp_RPT_GetUnallocatedTaxPayerList_Result> REP_GetUnAllocatedTaxPayerList(int TaxOfficeID, int TaxPayerTypeID);
        IList<usp_RPT_GetAllocatedTaxPayerList_Result> REP_GetAllocatedTaxPayerList(int TaxOfficeID, int TaxOfficerID);
        IList<usp_RPT_GetTaxPayerStatusManager_Result> REP_GetTaxPayerStatusManagerList();
        IList<usp_RPT_GetTaxPayerStatusManager_TaxPayerList_Result> REP_GetTaxPayerStatusManager_TaxPayerList(int TaxOfficeID);

        IList<usp_GetTaxPayerforTaxOfficer_Result> REP_GetTaxPayerBasedOnTaxOfficer(int TaxOfficeID, int TaxOfficerID);
        IList<usp_RPT_TaxOfficerStatus_Result> REP_GetTaxOfficerStatus(int TaxOfficeID, int TaxPayerTypeID, int ReviewStatusID, int TaxOfficerID);
        IList<usp_RPT_TaxOfficeManagerStatus_Result> REP_GetTaxOfficeManagerStatus(int TaxOfficeID, int TaxPayerTypeID, int ReviewStatusID, int TOManagerID);

        IList<usp_RPT_TaxOfficerSummary_Result> REP_GetTaxOfficerSummary(int TaxOfficeID);
        IList<usp_RPT_TaxOfficeManagerSummary_Result> REP_GetTaxOfficeManagerSummary(int TaxOfficeID);

        IList<usp_GetTaxPayerforTaxOfficeManager_Result> REP_GetTaxPayerBasedOnTaxOfficeManager(int TaxOfficeID, int TOManagerID);

        IList<usp_RPT_ReviewStatusSummary_Result> REP_GetReviewStatusSummary(int TaxOfficeID, int TaxPayerTypeID, int ReviewStatusID);
        IList<usp_RPT_ManagerReviewStatusSummary_Result> REP_GetManagerReviewStatusSummary(int TaxOfficeID, int TOManagerID, int ReviewStatusID);
        IList<usp_RPT_StaffReviewStatusSummary_Result> REP_GetStaffReviewStatusSummary(int TaxOfficeID, int TaxOfficerID, int ReviewStatusID);
        IList<usp_RPT_TaxOfficerMonthlyPayment_Result> REP_GetTaxOfficerMonthlyPayment(int TaxOfficeID, int TaxOfficerID, int TOManagerID, int Year);

        IList<usp_RPT_MonthlyTaxOfficeTarget_Result> REP_GetMonthlyTaxOfficeTarget(int TaxOfficeID, int Year);
        IList<usp_RPT_TaxOfficeByRevenueStreamTarget_Result> REP_GetTaxOfficeByRevenueStreamTarget(int TaxOfficeID, int Year, int Month);
        IList<usp_RPT_RevenueStreamByTaxOfficeTarget_Result> REP_GetRevenueStreamByTaxOfficeTarget(int RevenueStreamID, int Year, int Month,int taxofficeId);

        IList<usp_RPT_GetRevenueStreamByPaymentChannel_Result> REP_GetRevenueStreamByPaymentChannel(int SettlementMethodID, int Year, DateTime? FromDate, DateTime? ToDate);
        IList<usp_RPT_GetPaymentChannelByRevenueStream_Result> REP_GetPaymentChannelByRevenueStream(int RevenueStreamID, int Year, DateTime? FromDate, DateTime? ToDate);

        IList<usp_RPT_GetPaymentDetail_Result> REP_GetPaymentChannelDetail(int SettlementMethodID, int RevenueStreamID, int Year, DateTime? FromDate, DateTime? ToDate);

        IList<usp_RPT_TaxOfficerByRevenueStreamTarget_Result> REP_GetTaxOfficerByRevenueStreamTarget(int TaxOfficerID, int Year, int Month);
        IList<usp_RPT_RevenueStreamByTaxOfficerTarget_Result> REP_GetRevenueStreamByTaxOfficerTarget(int TaxOfficeID, int RevenueStreamID, int Year, int Month);
        IList<usp_RPT_GetRevenueStreamByTaxOfficerTargetDetail_Result> REP_GetRevenueStreamByTaxOfficerTargetDetail(int TaxOfficerID, int RevenueStreamID, int Year, int Month);
        IList<usp_RPT_InvidualLiabilityStatus_Result> REP_GetIndividualLiabilityStatus();

        IList<usp_GetTaxPayerProfileTypeForExport_Result> REP_GetTaxPayerProfileTypeForExport(int ProfileTypeID, int pIntYear);
        IList<usp_GetTaxPayerProfileForExport_Result> REP_GetTaxPayerProfileForExport(string pstrProfileID, int pIntYear);

        IList<usp_RPT_GetTaxPayerTypeByTaxOffice_Result> REP_GetTaxPayerTypeByTaxOffice();

        IList<usp_GetTaxPayerBasedOnTaxOfficeForExport_Result> REP_GetTaxPayerBasedOnTaxOfficeForExport(int pIntTaxOfficeID);

        IList<usp_RPT_GetAssetTypeByTaxOffice_Result> REP_GetAssetTypeByTaxOffice();
        IList<usp_RPT_GetAssetDetailByTaxOffice_Result> REP_GetAssetDetailByTaxOffice(int pIntTaxOfficeID);
        IList<usp_GetAssetBasedOnTaxOfficeForExport_Result> REP_GetAssetBasedOnTaxOfficeForExport(int pIntTaxOfficeID);

        IList<usp_GetIndividualLiabilityDetail_Result> REP_GetIndividualLiabilityDetail(int pIntTaxPayerID, int pIntTaxPayerTypeID);
        IList<usp_GetPaymentChargeList_Result> REP_GetPaymentChargeList(int pIntTaxPayerID, int pIntTaxPayerTypeID);
        IList<usp_GetDemandNoticeList_Result> REP_GetDemandNoticeList();

        IList<usp_RPT_GetTaxPayerMonthlyPayment_Result> REP_GetTaxPayerMonthlyPayment(int TaxPayerTypeID, int TaxPayerID, int RevenueStreamID, int Year);
        IList<usp_RPT_GetTaxPayerMonthlyPaymentDetail_Result> REP_GetTaxPayerMonthlyPaymentDetail(int TaxPayerTypeID, int TaxPayerID, int RevenueStreamID, int Year, int Month);

        IList<usp_RPT_TaxOfficeAssessmentSummary_Result> REP_GetTaxOfficeAssessmentSummary(int TaxYear);
        IList<usp_RPT_TaxOfficeAssessmentDetail_Result> REP_GetTaxOfficeAssessmentDetail(int TaxOfficeID,int TaxYear);

        IList<usp_RPT_BusinessSectorAssessmentSummary_Result> REP_GetBusinessSectorAssessmentSummary(int TaxYear, int BusinessTypeID, int BusinessCategoryID);
        IList<usp_RPT_BusinessSectorAssessmentDetail_Result> REP_GetBusinessSectorAssessmentDetail(int TaxYear, int BusinessSectorID);

        IList<usp_GetTreasuryReceiptByRevenueStream_Result> REP_GetTreasuryReceiptByRevenueStream(int pIntTaxYear, DateTime? pdtFromDate, DateTime? pdtToDate);
        IList<usp_GetTreasuryReceiptByRevenueStreamDetail_Result> REP_GetTreasuryReceiptByRevenueStreamDetails(int pIntTaxYear, DateTime? pdtFromDate, DateTime? pdtToDate, int? pIntRevenueStreamID);

        IList<usp_GetTreasuryReceiptByTaxOffice_Result> REP_GetTreasuryReceiptByTaxOffice(int pIntTaxYear, DateTime? pdtFromDate, DateTime? pdtToDate);
        IList<usp_GetTreasuryReceiptByTaxOfficeDetail_Result> REP_GetTreasuryReceiptByTaxOfficeDetails(int pIntTaxYear, DateTime? pdtFromDate, DateTime? pdtToDate, int? pIntTaxOfficeID);

        IList<usp_RPT_EmployerLiability_Result> REP_GetEmployerLiability();
        IList<usp_RPT_EmployerPAYELiability_Result> REP_GetEmployerPAYELiability();

        IList<usp_RPT_MonthlyTCCSummary_Result> REP_GetMonthlyTCCSummary(TCCReportSearchParams pObjSearchParams);
        IList<usp_RPT_MonthlyTCCSummaryDetail_Result> REP_GetMonthlyTCCSummaryDetail(TCCReportSearchParams pObjSearchParams);
        IList<usp_RPT_MonthlyTCCCommissionList_Result> REP_GetMonthlyTCCCommissionSummary(TCCReportSearchParams pObjSearchParams);
        IList<usp_RPT_MonthlyTCCRevokeList_Result> REP_GetMonthlyTCCRevokeSummary(TCCReportSearchParams pObjSearchParams);
        IList<usp_RPT_MonthlyTCCRevokeDetail_Result> REP_GetMonthlyTCCRevokeDetail(TCCReportSearchParams pObjSearchParams);
        IList<usp_RPT_ReviseBill_Result> REP_GetReviseBillSummary(int TaxYear, DateTime? FromDate, DateTime? ToDate, int BillTypeID, int RevenueStreamID);

        IList<usp_RPT_TaxPayerCaptureAnalysis_Result> REP_TaxPayerCaptureAnalysis(DateTime? FromDate, DateTime? ToDate, int TaxPayerTypeID, int? TaxOfficeID);
        

    }
}
