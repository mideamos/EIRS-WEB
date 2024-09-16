using EIRS.BOL;
using EIRS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EIRS.BLL
{
    public class BLOperationManager
    {
        IOperationManagerRepository _OperationManagerRepository;
        /// <summary>
        /// BLOperationManager Constructor
        /// </summary>
        public BLOperationManager()
        {
            _OperationManagerRepository = new OperationManagerRepository();
        }

        public IList<usp_GetPaymentByRevenueStream_Result> BL_GetPaymentByRevenueStream(int? pIntPaymentTypeID, int pIntTaxYear, DateTime? pdtFromDate, DateTime? pdtToDate)
        {
            return _OperationManagerRepository.REP_GetPaymentByRevenueStream(pIntPaymentTypeID, pIntTaxYear, pdtFromDate, pdtToDate);
        }
        public IList<usp_GetPaymentByRevenueStreamDetail_Result> BL_GetPaymentByRevenueStreamDetails(int pIntTaxYear, DateTime? pdtFromDate, DateTime? pdtToDate, int? pIntPaymentTypeID, int? pIntRevenueStreamID)
        {
            return _OperationManagerRepository.REP_GetPaymentByRevenueStreamDetails(pIntTaxYear, pdtFromDate, pdtToDate, pIntPaymentTypeID, pIntRevenueStreamID);
        }

        public IList<usp_GetBillByRevenueStream_Result> BL_GetBillByRevenueStream(int pIntTaxYear, DateTime? FromDate, DateTime? ToDate, int pIntBillType, int pIntBillStatusID)
        {
            return _OperationManagerRepository.REP_GetBillByRevenueStream(pIntTaxYear, FromDate, ToDate, pIntBillType, pIntBillStatusID);
        }
        public IList<usp_GetBillByRevenueStreamDetail_Result> BL_GetBillByRevenueStreamDetails(int pIntTaxYear, DateTime? FromDate, DateTime? ToDate, int pIntBillType, int pIntBillStatusID, int? pIntRevenueStreamID)
        {
            return _OperationManagerRepository.REP_GetBillByRevenueStreamDetails(pIntTaxYear, FromDate, ToDate, pIntBillType, pIntBillStatusID, pIntRevenueStreamID);
        }

        public IList<usp_GetBillAgingByRevenueStream_Result> BL_GetBillAgingByRevenueStream(int? BillType, int? StageID)
        {
            return _OperationManagerRepository.REP_GetBillAgingByRevenueStream(BillType, StageID).ToList();
        }
        public IList<usp_GetBillAgingDetailByRevenueStream_Result> BL_GetBillAgingDetailByRevenueStream(int? BillType, int? StageID, int? RevenueStreamID)
        {
            return _OperationManagerRepository.REP_GetBillAgingDetailByRevenueStream(BillType, StageID, RevenueStreamID).ToList();
        }

        public IList<usp_GetPOATaxPayerWithoutAsset_Result> BL_GetPOATaxPayerWithoutAsset(int pIntTaxYear, DateTime? pdtFromDate, DateTime? pdtToDate, int pIntTaxPayerTypeID, int pIntTaxOfficeID)
        {
            return _OperationManagerRepository.REP_GetPOATaxPayerWithoutAsset(pIntTaxYear, pdtFromDate, pdtToDate, pIntTaxPayerTypeID, pIntTaxOfficeID);
        }

        public IList<usp_GetPaymentByTaxOffice_Result> BL_GetPaymentByTaxOffice(int? pIntPaymentTypeID, int pIntTaxYear, DateTime? pdtFromDate, DateTime? pdtToDate)
        {
            return _OperationManagerRepository.REP_GetPaymentByTaxOffice(pIntPaymentTypeID, pIntTaxYear, pdtFromDate, pdtToDate);
        }
        public IList<usp_GetPaymentByTaxOfficeDetail_Result> BL_GetPaymentByTaxOfficeDetails(int pIntTaxYear, DateTime? pdtFromDate, DateTime? pdtToDate, int? pIntPaymentTypeID, int? pIntTaxOfficeID)
        {
            return _OperationManagerRepository.REP_GetPaymentByTaxOfficeDetails(pIntTaxYear, pdtFromDate, pdtToDate, pIntPaymentTypeID, pIntTaxOfficeID);
        }

        public IList<usp_GetBillByTaxOffice_Result> BL_GetBillByTaxOffice(int pIntTaxYear, DateTime? FromDate, DateTime? ToDate, int pIntBillType, int pIntBillStatusID)
        {
            return _OperationManagerRepository.REP_GetBillByTaxOffice(pIntTaxYear, FromDate, ToDate, pIntBillType, pIntBillStatusID);
        }
        public IList<usp_GetBillByTaxOfficeDetail_Result> BL_GetBillByTaxOfficeDetails(int pIntTaxYear, DateTime? FromDate, DateTime? ToDate, int pIntBillType, int pIntBillStatusID, int? pIntTaxOfficeID)
        {
            return _OperationManagerRepository.REP_GetBillByTaxOfficeDetails(pIntTaxYear, FromDate, ToDate, pIntBillType, pIntBillStatusID, pIntTaxOfficeID);
        }

        public IList<usp_GetBillAgingByTaxOffice_Result> BL_GetBillAgingByTaxOffice(int? BillType, int? StageID)
        {
            return _OperationManagerRepository.REP_GetBillAgingByTaxOffice(BillType, StageID).ToList();
        }
        public IList<usp_GetBillAgingDetailByTaxOffice_Result> BL_GetBillAgingDetailByTaxOffice(int? BillType, int? StageID, int? TaxOfficeID)
        {
            return _OperationManagerRepository.REP_GetBillAgingDetailByTaxOffice(BillType, StageID, TaxOfficeID).ToList();
        }



        public IList<usp_RPT_TaxPayerLiabilityStatus_Result> BL_GetTaxPayerLiabilityStatus(string TaxPayerRIN, int TaxYear, DateTime? FromDate, DateTime? ToDate)
        {
            return _OperationManagerRepository.REP_GetTaxPayerLiabilityStatus(TaxPayerRIN, TaxYear, FromDate, ToDate).ToList();
        }

        public IList<usp_RPT_TaxPayerLiabilityStatus_Bills_Result> BL_GetTaxPayerLiabilityStatusDetails(int? intTaxPayerID, int? intTaxPayerTypeID, int TaxYear, DateTime? FromDate, DateTime? ToDate)
        {
            return _OperationManagerRepository.REP_GetTaxPayerLiabilityStatusDetails(intTaxPayerID, intTaxPayerTypeID, TaxYear, FromDate, ToDate).ToList();
        }

        public IList<usp_RPT_TaxPayerLiabilityStatus_Payment_Result> BL_GetTaxPayerLiabilityStatusPaymentDetails(int? intTaxPayerID, int? intTaxPayerTypeID, int TaxYear, DateTime? FromDate, DateTime? ToDate)
        {
            return _OperationManagerRepository.REP_GetTaxPayerLiabilityStatusPaymentDetails(intTaxPayerID, intTaxPayerTypeID, TaxYear, FromDate, ToDate).ToList();
        }

        public IList<usp_GetRevenueStreamBillByTaxOffice_Result> BL_GetRevenueStreamAssessmentsbyTaxOffice(int TaxYear, DateTime? FromDate, DateTime? ToDate, int? RevenueStreamID)
        {
            return _OperationManagerRepository.REP_GetRevenueStreamAssessmentsbyTaxOffice(TaxYear, FromDate, ToDate, RevenueStreamID).ToList();

        }
        public IList<usp_GetRevenueStreamBillDetailByTaxOffice_Result> BL_GetRevenueStreamBillDetailByTaxOffice(int TaxYear, DateTime? FromDate, DateTime? ToDate, int? RevenueStreamID, int? TaxOfficeID)
        {
            return _OperationManagerRepository.REP_GetRevenueStreamBillDetailByTaxOffice(TaxYear, FromDate, ToDate, RevenueStreamID, TaxOfficeID).ToList();

        }
        public IList<usp_GetPAYERevenueStreamBillByTaxOffice_Result> BL_GetPayeRevenueStreamAssessmentsbyTaxOffice(int TaxYear, DateTime? FromDate, DateTime? ToDate)
        {
            return _OperationManagerRepository.REP_GetPayeRevenueStreamAssessmentsbyTaxOffice(TaxYear, FromDate, ToDate).ToList();

        }
        public IList<usp_GetPAYERevenueStreamBillDetailByTaxOffice_Result> BL_GetPayeRevenueStreamBillDetailbyTaxOffice(int TaxYear, DateTime? FromDate, DateTime? ToDate, int? TaxOfficeID)
        {
            return _OperationManagerRepository.REP_GetPayeRevenueStreamBillDetailbyTaxOffice(TaxYear, FromDate, ToDate, TaxOfficeID).ToList();
        }


        public IList<usp_RPT_GetUnallocatedTaxPayerList_Result> BL_GetUnAllocatedTaxPayerList(int TaxOfficeID, int TaxPayerTypeID)
        {
            return _OperationManagerRepository.REP_GetUnAllocatedTaxPayerList(TaxOfficeID, TaxPayerTypeID);
        }

        public IList<usp_RPT_GetAllocatedTaxPayerList_Result> BL_GetAllocatedTaxPayerList(int TaxOfficeID, int TaxOfficerID)
        {
            return _OperationManagerRepository.REP_GetAllocatedTaxPayerList(TaxOfficeID, TaxOfficerID);
        }

        public IList<usp_RPT_GetTaxPayerStatusManager_Result> BL_GetTaxPayerStatusManagerList()
        {
            return _OperationManagerRepository.REP_GetTaxPayerStatusManagerList();
        }

        public IList<usp_RPT_GetTaxPayerStatusManager_TaxPayerList_Result> BL_GetTaxPayerStatusManager_TaxPayerList(int TaxOfficeID)
        {
            return _OperationManagerRepository.REP_GetTaxPayerStatusManager_TaxPayerList(TaxOfficeID);
        }

        public IList<usp_GetTaxPayerforTaxOfficer_Result> BL_GetTaxPayerBasedOnTaxOfficer(int TaxOfficeID, int TaxOfficerID)
        {
            return _OperationManagerRepository.REP_GetTaxPayerBasedOnTaxOfficer(TaxOfficeID, TaxOfficerID);
        }

        public IList<usp_RPT_TaxOfficerStatus_Result> BL_GetTaxOfficerStatus(int TaxOfficeID, int TaxPayerTypeID, int ReviewStatusID, int TaxOfficerID)
        {
            return _OperationManagerRepository.REP_GetTaxOfficerStatus(TaxOfficeID, TaxPayerTypeID, ReviewStatusID, TaxOfficerID);
        }

        public IList<usp_RPT_TaxOfficeManagerStatus_Result> BL_GetTaxOfficeManagerStatus(int TaxOfficeID, int TaxPayerTypeID, int ReviewStatusID, int TOManagerID)
        {
            return _OperationManagerRepository.REP_GetTaxOfficeManagerStatus(TaxOfficeID, TaxPayerTypeID, ReviewStatusID, TOManagerID);
        }

        public IList<usp_RPT_TaxOfficerSummary_Result> BL_GetTaxOfficerSummary(int TaxOfficeID)
        {
            return _OperationManagerRepository.REP_GetTaxOfficerSummary(TaxOfficeID);
        }

        public IList<usp_RPT_TaxOfficeManagerSummary_Result> BL_GetTaxOfficeManagerSummary(int TaxOfficeID)
        {
            return _OperationManagerRepository.REP_GetTaxOfficeManagerSummary(TaxOfficeID);
        }

        public IList<usp_GetTaxPayerforTaxOfficeManager_Result> BL_GetTaxPayerBasedOnTaxOfficeManager(int TaxOfficeID, int TOManagerID)
        {
            return _OperationManagerRepository.REP_GetTaxPayerBasedOnTaxOfficeManager(TaxOfficeID, TOManagerID);
        }

        public IList<usp_RPT_ReviewStatusSummary_Result> BL_GetReviewStatusSummary(int TaxOfficeID, int TaxPayerTypeID, int ReviewStatusID)
        {
            return _OperationManagerRepository.REP_GetReviewStatusSummary(TaxOfficeID, TaxPayerTypeID, ReviewStatusID);
        }

        public IList<usp_RPT_ManagerReviewStatusSummary_Result> BL_GetManagerReviewStatusSummary(int TaxOfficeID, int TOManagerID, int ReviewStatusID)
        {
            return _OperationManagerRepository.REP_GetManagerReviewStatusSummary(TaxOfficeID, TOManagerID, ReviewStatusID);
        }

        public IList<usp_RPT_StaffReviewStatusSummary_Result> BL_GetStaffReviewStatusSummary(int TaxOfficeID, int TaxOfficerID, int ReviewStatusID)
        {
            return _OperationManagerRepository.REP_GetStaffReviewStatusSummary(TaxOfficeID, TaxOfficerID, ReviewStatusID);
        }

        public IList<usp_RPT_TaxOfficerMonthlyPayment_Result> BL_GetTaxOfficerMonthlyPayment(int TaxOfficeID, int TaxOfficerID, int TOManagerID, int Year)
        {
            return _OperationManagerRepository.REP_GetTaxOfficerMonthlyPayment(TaxOfficeID, TaxOfficerID, TOManagerID, Year);
        }

        public IList<usp_RPT_MonthlyTaxOfficeTarget_Result> BL_GetMonthlyTaxOfficeTarget(int TaxOfficeID, int Year)
        {
            return _OperationManagerRepository.REP_GetMonthlyTaxOfficeTarget(TaxOfficeID, Year);
        }

        public IList<usp_RPT_TaxOfficeByRevenueStreamTarget_Result> BL_GetTaxOfficeByRevenueStreamTarget(int TaxOfficeID, int Year, int Month)
        {
            return _OperationManagerRepository.REP_GetTaxOfficeByRevenueStreamTarget(TaxOfficeID, Year, Month);
        }

        public IList<usp_RPT_RevenueStreamByTaxOfficeTarget_Result> BL_GetRevenueStreamByTaxOfficeTarget(int RevenueStreamID, int Year, int Month,int taxofficeId)
        {
            return _OperationManagerRepository.REP_GetRevenueStreamByTaxOfficeTarget(RevenueStreamID, Year, Month,taxofficeId);
        }

        public IList<usp_RPT_GetRevenueStreamByPaymentChannel_Result> BL_GetRevenueStreamByPaymentChannel(int SettlementMethodID, int Year, DateTime? FromDate, DateTime? ToDate)
        {
            return _OperationManagerRepository.REP_GetRevenueStreamByPaymentChannel(SettlementMethodID, Year, FromDate, ToDate);
        }

        public IList<usp_RPT_GetPaymentChannelByRevenueStream_Result> BL_GetPaymentChannelByRevenueStream(int RevenueStreamID, int Year, DateTime? FromDate, DateTime? ToDate)
        {
            return _OperationManagerRepository.REP_GetPaymentChannelByRevenueStream(RevenueStreamID, Year, FromDate, ToDate);
        }

        public IList<usp_RPT_GetPaymentDetail_Result> BL_GetPaymentChannelDetail(int SettlementMethodID, int RevenueStreamID, int Year, DateTime? FromDate, DateTime? ToDate)
        {
            return _OperationManagerRepository.REP_GetPaymentChannelDetail(SettlementMethodID, RevenueStreamID, Year, FromDate, ToDate);
        }

        public IList<usp_RPT_TaxOfficerByRevenueStreamTarget_Result> BL_GetTaxOfficerByRevenueStreamTarget(int TaxOfficerID, int Year, int Month)
        {
            return _OperationManagerRepository.REP_GetTaxOfficerByRevenueStreamTarget(TaxOfficerID, Year, Month);
        }

        public IList<usp_RPT_RevenueStreamByTaxOfficerTarget_Result> BL_GetRevenueStreamByTaxOfficerTarget(int TaxOfficeID, int RevenueStreamID, int Year, int Month)
        {
            return _OperationManagerRepository.REP_GetRevenueStreamByTaxOfficerTarget(TaxOfficeID, RevenueStreamID, Year, Month);
        }

        public IList<usp_RPT_GetRevenueStreamByTaxOfficerTargetDetail_Result> BL_GetRevenueStreamByTaxOfficerTargetDetail(int TaxOfficerID, int RevenueStreamID, int Year, int Month)
        {
            return _OperationManagerRepository.REP_GetRevenueStreamByTaxOfficerTargetDetail(TaxOfficerID, RevenueStreamID, Year, Month);
        }

        public IList<usp_RPT_InvidualLiabilityStatus_Result> BL_GetIndividualLiabilityStatus()
        {
            return _OperationManagerRepository.REP_GetIndividualLiabilityStatus();
        }

        public IList<usp_GetTaxPayerProfileForExport_Result> BL_GetTaxPayerProfileForExport(string pstrProfileID, int pIntYear)
        {
            return _OperationManagerRepository.REP_GetTaxPayerProfileForExport(pstrProfileID, pIntYear);
        }

        public IList<usp_GetTaxPayerProfileTypeForExport_Result> BL_GetTaxPayerProfileTypeForExport(int ProfileTypeID, int pIntYear)
        {
            return _OperationManagerRepository.REP_GetTaxPayerProfileTypeForExport(ProfileTypeID, pIntYear);
        }

        public IList<usp_RPT_GetTaxPayerTypeByTaxOffice_Result> BL_GetTaxPayerTypeByTaxOffice()
        {
            return _OperationManagerRepository.REP_GetTaxPayerTypeByTaxOffice();
        }

        public IList<usp_GetTaxPayerBasedOnTaxOfficeForExport_Result> BL_GetTaxPayerBasedOnTaxOfficeForExport(int pIntTaxOfficeID)
        {
            return _OperationManagerRepository.REP_GetTaxPayerBasedOnTaxOfficeForExport(pIntTaxOfficeID);
        }

        public IList<usp_RPT_GetAssetTypeByTaxOffice_Result> BL_GetAssetTypeByTaxOffice()
        {
            return _OperationManagerRepository.REP_GetAssetTypeByTaxOffice();
        }

        public IList<usp_RPT_GetAssetDetailByTaxOffice_Result> BL_GetAssetDetailByTaxOffice(int pIntTaxOfficeID)
        {
            return _OperationManagerRepository.REP_GetAssetDetailByTaxOffice(pIntTaxOfficeID);
        }

        public IList<usp_GetAssetBasedOnTaxOfficeForExport_Result> BL_GetAssetBasedOnTaxOfficeForExport(int pIntTaxOfficeID)
        {
            return _OperationManagerRepository.REP_GetAssetBasedOnTaxOfficeForExport(pIntTaxOfficeID);
        }

        public IList<usp_GetIndividualLiabilityDetail_Result> BL_GetIndividualLiabilityDetail(int pIntTaxPayerID, int pIntTaxPayerTypeID)
        {
            return _OperationManagerRepository.REP_GetIndividualLiabilityDetail(pIntTaxPayerID, pIntTaxPayerTypeID);
        }

        public IList<usp_GetPaymentChargeList_Result> BL_GetPaymentChargeList(int pIntTaxPayerID, int pIntTaxPayerTypeID)
        {
            return _OperationManagerRepository.REP_GetPaymentChargeList(pIntTaxPayerID, pIntTaxPayerTypeID);
        }

        public IList<usp_GetDemandNoticeList_Result> BL_GetDemandNoticeList()
        {
            return _OperationManagerRepository.REP_GetDemandNoticeList();
        }

        public IList<usp_RPT_GetTaxPayerMonthlyPayment_Result> BL_GetTaxPayerMonthlyPayment(int TaxPayerTypeID, int TaxPayerID, int RevenueStreamID, int Year)
        {
            return _OperationManagerRepository.REP_GetTaxPayerMonthlyPayment(TaxPayerTypeID, TaxPayerID, RevenueStreamID, Year);
        }

        public IList<usp_RPT_GetTaxPayerMonthlyPaymentDetail_Result> BL_GetTaxPayerMonthlyPaymentDetail(int TaxPayerTypeID, int TaxPayerID, int RevenueStreamID, int Year, int Month)
        {
            return _OperationManagerRepository.REP_GetTaxPayerMonthlyPaymentDetail(TaxPayerTypeID, TaxPayerID, RevenueStreamID, Year, Month);
        }

        public IList<usp_RPT_TaxOfficeAssessmentSummary_Result> BL_GetTaxOfficeAssessmentSummary(int TaxYear)
        {
            return _OperationManagerRepository.REP_GetTaxOfficeAssessmentSummary(TaxYear);
        }

        public IList<usp_RPT_TaxOfficeAssessmentDetail_Result> BL_GetTaxOfficeAssessmentDetail(int TaxOfficeID, int TaxYear)
        {
            return _OperationManagerRepository.REP_GetTaxOfficeAssessmentDetail(TaxOfficeID, TaxYear);
        }

        public IList<usp_RPT_BusinessSectorAssessmentSummary_Result> BL_GetBusinessSectorAssessmentSummary(int TaxYear, int BusinessTypeID, int BusinessCategoryID)
        {
            return _OperationManagerRepository.REP_GetBusinessSectorAssessmentSummary(TaxYear, BusinessTypeID, BusinessCategoryID);
        }

        public IList<usp_RPT_BusinessSectorAssessmentDetail_Result> BL_GetBusinessSectorAssessmentDetail(int TaxYear, int BusinessSectorID)
        {
            return _OperationManagerRepository.REP_GetBusinessSectorAssessmentDetail(TaxYear, BusinessSectorID);
        }

        public IList<usp_GetTreasuryReceiptByRevenueStream_Result> BL_GetTreasuryReceiptByRevenueStream(int pIntTaxYear, DateTime? pdtFromDate, DateTime? pdtToDate)
        {
            return _OperationManagerRepository.REP_GetTreasuryReceiptByRevenueStream(pIntTaxYear, pdtFromDate, pdtToDate);
        }
        public IList<usp_GetTreasuryReceiptByRevenueStreamDetail_Result> BL_GetTreasuryReceiptByRevenueStreamDetails(int pIntTaxYear, DateTime? pdtFromDate, DateTime? pdtToDate, int? pIntRevenueStreamID)
        {
            return _OperationManagerRepository.REP_GetTreasuryReceiptByRevenueStreamDetails(pIntTaxYear, pdtFromDate, pdtToDate, pIntRevenueStreamID);
        }

        public IList<usp_GetTreasuryReceiptByTaxOffice_Result> BL_GetTreasuryReceiptByTaxOffice(int pIntTaxYear, DateTime? pdtFromDate, DateTime? pdtToDate)
        {
            return _OperationManagerRepository.REP_GetTreasuryReceiptByTaxOffice(pIntTaxYear, pdtFromDate, pdtToDate);
        }
        public IList<usp_GetTreasuryReceiptByTaxOfficeDetail_Result> BL_GetTreasuryReceiptByTaxOfficeDetails(int pIntTaxYear, DateTime? pdtFromDate, DateTime? pdtToDate, int? pIntTaxOfficeID)
        {
            return _OperationManagerRepository.REP_GetTreasuryReceiptByTaxOfficeDetails(pIntTaxYear, pdtFromDate, pdtToDate, pIntTaxOfficeID);
        }

        public IList<usp_RPT_EmployerLiability_Result> BL_GetEmployerLiability()
        {
            return _OperationManagerRepository.REP_GetEmployerLiability();
        }

        public IList<usp_RPT_EmployerPAYELiability_Result> BL_GetEmployerPAYELiability()
        {
            return _OperationManagerRepository.REP_GetEmployerPAYELiability();
        }

        public IList<usp_RPT_MonthlyTCCSummary_Result> BL_GetMonthlyTCCSummary(TCCReportSearchParams pObjSearchParams)
        {
            return _OperationManagerRepository.REP_GetMonthlyTCCSummary(pObjSearchParams);
        }

        public IList<usp_RPT_MonthlyTCCSummaryDetail_Result> BL_GetMonthlyTCCSummaryDetail(TCCReportSearchParams pObjSearchParams)
        {
            return _OperationManagerRepository.REP_GetMonthlyTCCSummaryDetail(pObjSearchParams);
        }

        public IList<usp_RPT_MonthlyTCCCommissionList_Result> BL_GetMonthlyTCCCommissionSummary(TCCReportSearchParams pObjSearchParams)
        {
            return _OperationManagerRepository.REP_GetMonthlyTCCCommissionSummary(pObjSearchParams);
        }

        public IList<usp_RPT_MonthlyTCCRevokeList_Result> BL_GetMonthlyTCCRevokeSummary(TCCReportSearchParams pObjSearchParams)
        {
            return _OperationManagerRepository.REP_GetMonthlyTCCRevokeSummary(pObjSearchParams);
        }

        public IList<usp_RPT_MonthlyTCCRevokeDetail_Result> BL_GetMonthlyTCCRevokeDetail(TCCReportSearchParams pObjSearchParams)
        {
            return _OperationManagerRepository.REP_GetMonthlyTCCRevokeDetail(pObjSearchParams);
        }

        public IList<usp_RPT_ReviseBill_Result> BL_GetReviseBillSummary(int TaxYear, DateTime? FromDate, DateTime? ToDate, int BillTypeID, int RevenueStreamID)
        {
            return _OperationManagerRepository.REP_GetReviseBillSummary(TaxYear, FromDate, ToDate, BillTypeID, RevenueStreamID);
        }

        public IList<usp_RPT_TaxPayerCaptureAnalysis_Result> BL_TaxPayerCaptureAnalysis(DateTime? FromDate, DateTime? ToDate, int TaxPayerTypeID, int? TaxOfficeID)
        {
            return _OperationManagerRepository.REP_TaxPayerCaptureAnalysis(FromDate, ToDate, TaxPayerTypeID, TaxOfficeID);
        }
    }
}
