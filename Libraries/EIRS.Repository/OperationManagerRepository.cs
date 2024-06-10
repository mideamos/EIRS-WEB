using EIRS.BOL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EIRS.Repository
{

    public class OperationManagerRepository : IOperationManagerRepository
    {
        EIRSEntities _db;

        public IList<usp_GetPaymentByRevenueStream_Result> REP_GetPaymentByRevenueStream(int? pIntPaymentTypeID, int pIntTaxYear, DateTime? pdtFromDate, DateTime? pdtToDate)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetPaymentByRevenueStream(pIntTaxYear, pdtFromDate, pdtToDate, pIntPaymentTypeID).ToList();
            }
        }
        public IList<usp_GetPaymentByRevenueStreamDetail_Result> REP_GetPaymentByRevenueStreamDetails(int pIntTaxYear, DateTime? pdtFromDate, DateTime? pdtToDate, int? pIntPaymentTypeID, int? pIntRevenueStreamID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetPaymentByRevenueStreamDetail(pIntTaxYear, pdtFromDate, pdtToDate, pIntPaymentTypeID, pIntRevenueStreamID).ToList();
            }
        }
        public IList<usp_GetBillAgingByRevenueStream_Result> REP_GetBillAgingByRevenueStream(int? BillType, int? StageID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetBillAgingByRevenueStream(BillType, StageID).ToList();

            }
        }
        public IList<usp_GetBillAgingDetailByRevenueStream_Result> REP_GetBillAgingDetailByRevenueStream(int? BillType, int? StageID, int? RevenueStreamID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetBillAgingDetailByRevenueStream(BillType, StageID, RevenueStreamID).ToList();

            }
        }
        public IList<usp_GetBillByRevenueStream_Result> REP_GetBillByRevenueStream(int pIntTaxYear, DateTime? FromDate, DateTime? ToDate, int pIntBillType, int pIntBillStatusID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetBillByRevenueStream(pIntTaxYear, FromDate, ToDate, pIntBillType, pIntBillStatusID).ToList();
            }
        }
        public IList<usp_GetBillByRevenueStreamDetail_Result> REP_GetBillByRevenueStreamDetails(int pIntTaxYear, DateTime? FromDate, DateTime? ToDate, int pIntBillType, int pIntBillStatusID, int? pIntRevenueStreamID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetBillByRevenueStreamDetail(pIntTaxYear, FromDate, ToDate, pIntBillType, pIntBillStatusID, pIntRevenueStreamID).ToList();
            }
        }
        
        public IList<usp_GetPOATaxPayerWithoutAsset_Result> REP_GetPOATaxPayerWithoutAsset(int pIntTaxYear, DateTime? pdtFromDate, DateTime? pdtToDate, int pIntTaxPayerTypeID, int pIntTaxOfficeID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetPOATaxPayerWithoutAsset(pIntTaxYear, pdtFromDate, pdtToDate, pIntTaxPayerTypeID, pIntTaxOfficeID).ToList();
            }
        }

        public IList<usp_GetPaymentByTaxOffice_Result> REP_GetPaymentByTaxOffice(int? pIntPaymentTypeID, int pIntTaxYear, DateTime? pdtFromDate, DateTime? pdtToDate)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetPaymentByTaxOffice(pIntTaxYear, pdtFromDate, pdtToDate, pIntPaymentTypeID).ToList();
            }
        }
        public IList<usp_GetPaymentByTaxOfficeDetail_Result> REP_GetPaymentByTaxOfficeDetails(int pIntTaxYear, DateTime? pdtFromDate, DateTime? pdtToDate, int? pIntPaymentTypeID, int? pIntTaxOfficeID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetPaymentByTaxOfficeDetail(pIntTaxYear, pdtFromDate, pdtToDate, pIntPaymentTypeID, pIntTaxOfficeID).ToList();
            }
        }
        public IList<usp_GetBillAgingByTaxOffice_Result> REP_GetBillAgingByTaxOffice(int? BillType, int? StageID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetBillAgingByTaxOffice(BillType, StageID).ToList();

            }
        }
        public IList<usp_GetBillAgingDetailByTaxOffice_Result> REP_GetBillAgingDetailByTaxOffice(int? BillType, int? StageID, int? TaxOfficeID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetBillAgingDetailByTaxOffice(BillType, StageID, TaxOfficeID).ToList();

            }
        }
        public IList<usp_GetBillByTaxOffice_Result> REP_GetBillByTaxOffice(int pIntTaxYear, DateTime? FromDate, DateTime? ToDate, int pIntBillType, int pIntBillStatusID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetBillByTaxOffice(pIntTaxYear, FromDate, ToDate, pIntBillType, pIntBillStatusID).ToList();
            }
        }
        public IList<usp_GetBillByTaxOfficeDetail_Result> REP_GetBillByTaxOfficeDetails(int pIntTaxYear, DateTime? FromDate, DateTime? ToDate, int pIntBillType, int pIntBillStatusID, int? pIntTaxOfficeID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetBillByTaxOfficeDetail(pIntTaxYear, FromDate, ToDate, pIntBillType, pIntBillStatusID, pIntTaxOfficeID).ToList();
            }
        }


        ////QC_PD_T-ERAS-TaxPayerLiabilityStatus_12
        /// <summary>
        /// Function Are Used To Gave List of data using usp_RPT_TaxPayerLiabilityStatus_Result this SP
        /// </summary>
        /// <param name="TaxPayerRIN"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <returns></returns>
        public IList<usp_RPT_TaxPayerLiabilityStatus_Result> REP_GetTaxPayerLiabilityStatus(string TaxPayerRIN, int TaxYear, DateTime? FromDate, DateTime? ToDate)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_RPT_TaxPayerLiabilityStatus(TaxPayerRIN, TaxYear, FromDate, ToDate).ToList();
            }
        }

        /// <summary>
        /// Function Are Used To gave List of data in usp_RPT_TaxPayerLiabilityStatus_Bills_Result SP
        /// </summary>
        /// <param name="TaxPayerRIN"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <returns></returns>
        public IList<usp_RPT_TaxPayerLiabilityStatus_Bills_Result> REP_GetTaxPayerLiabilityStatusDetails(int? intTaxPayerID, int? intTaxPayerTypeID,int TaxYear, DateTime? FromDate, DateTime? ToDate)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_RPT_TaxPayerLiabilityStatus_Bills(intTaxPayerID, intTaxPayerTypeID, TaxYear, FromDate, ToDate).ToList();
            }
        }

        /// <summary>
        /// Function Are Used To gave list of data to export to excel purpose
        /// </summary>
        /// <param name="TaxPayerRIN"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <returns></returns>
        public IList<usp_RPT_TaxPayerLiabilityStatus_Payment_Result> REP_GetTaxPayerLiabilityStatusPaymentDetails(int? intTaxPayerID, int? intTaxPayerTypeID, int TaxYear, DateTime? FromDate, DateTime? ToDate)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_RPT_TaxPayerLiabilityStatus_Payment(intTaxPayerID, intTaxPayerTypeID, TaxYear, FromDate, ToDate).ToList();
            }
        }

        ////QC_PD_T-ERAS-Revenue Stream Assessments by Tax Office_13
        /// <summary>
        /// Function Are Used To Gave List of data using usp_RPT_usp_GetRevenueStreamBillByTaxOffice_Result this SP
        /// </summary>
        /// <param name="TaxPayerRIN"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <returns></returns>
        public IList<usp_GetRevenueStreamBillByTaxOffice_Result> REP_GetRevenueStreamAssessmentsbyTaxOffice(int TaxYear,DateTime? FromDate, DateTime? ToDate, int? RevenueStreamID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetRevenueStreamBillByTaxOffice(TaxYear,FromDate, ToDate, RevenueStreamID).ToList();
            }
        }

        /// <summary>
        /// Function Are Used To gave list of data to export to excel purpose
        /// </summary>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        ///  <param name="RevenueStreamID"></param>
        ///   <param name="TaxOfficeID"></param>
        /// <returns></returns>
        public IList<usp_GetRevenueStreamBillDetailByTaxOffice_Result> REP_GetRevenueStreamBillDetailByTaxOffice(int TaxYear,DateTime? FromDate, DateTime? ToDate, int? RevenueStreamID, int? TaxOfficeID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetRevenueStreamBillDetailByTaxOffice(TaxYear,FromDate, ToDate, RevenueStreamID, TaxOfficeID).ToList();
            }
        }

        ////QC_PD_T-ERAS-Paye Revenue Stream Assessments by Tax Office_13
        public IList<usp_GetPAYERevenueStreamBillByTaxOffice_Result> REP_GetPayeRevenueStreamAssessmentsbyTaxOffice(int TaxYear,DateTime? FromDate, DateTime? ToDate)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetPAYERevenueStreamBillByTaxOffice(TaxYear,FromDate, ToDate).ToList();
            }
        }
        /// <summary>
        /// Function Are Used To gave list of data to export to excel purpose
        /// </summary>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        ///   <param name="TaxOfficeID"></param>
        /// <returns></returns>
        public IList<usp_GetPAYERevenueStreamBillDetailByTaxOffice_Result> REP_GetPayeRevenueStreamBillDetailbyTaxOffice(int TaxYear,DateTime? FromDate, DateTime? ToDate, int? TaxOfficeID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetPAYERevenueStreamBillDetailByTaxOffice(TaxYear,FromDate, ToDate, TaxOfficeID).ToList();
            }
        }

        public IList<usp_RPT_GetUnallocatedTaxPayerList_Result> REP_GetUnAllocatedTaxPayerList(int TaxOfficeID, int TaxPayerTypeID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_RPT_GetUnallocatedTaxPayerList(TaxOfficeID, TaxPayerTypeID).ToList();
            }
        }

        public IList<usp_RPT_GetAllocatedTaxPayerList_Result> REP_GetAllocatedTaxPayerList(int TaxOfficeID, int TaxOfficerID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_RPT_GetAllocatedTaxPayerList(TaxOfficeID, TaxOfficerID).ToList();
            }
        }


        public IList<usp_RPT_GetTaxPayerStatusManager_Result> REP_GetTaxPayerStatusManagerList()
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_RPT_GetTaxPayerStatusManager().ToList();
            }
        }

        public IList<usp_RPT_GetTaxPayerStatusManager_TaxPayerList_Result> REP_GetTaxPayerStatusManager_TaxPayerList(int TaxOfficeID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_RPT_GetTaxPayerStatusManager_TaxPayerList(TaxOfficeID).ToList();
            }
        }

        public IList<usp_GetTaxPayerforTaxOfficer_Result> REP_GetTaxPayerBasedOnTaxOfficer(int TaxOfficeID, int TaxOfficerID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetTaxPayerforTaxOfficer(TaxOfficeID, TaxOfficerID).ToList();
            }
        }

        public IList<usp_RPT_TaxOfficerStatus_Result> REP_GetTaxOfficerStatus(int TaxOfficeID, int TaxPayerTypeID, int ReviewStatusID, int TaxOfficerID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_RPT_TaxOfficerStatus(TaxOfficeID, TaxPayerTypeID, ReviewStatusID, TaxOfficerID).ToList();
            }
        }

        public IList<usp_RPT_TaxOfficeManagerStatus_Result> REP_GetTaxOfficeManagerStatus(int TaxOfficeID, int TaxPayerTypeID, int ReviewStatusID, int TOManagerID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_RPT_TaxOfficeManagerStatus(TaxOfficeID, TaxPayerTypeID, ReviewStatusID, TOManagerID).ToList();
            }
        }

        public IList<usp_RPT_TaxOfficerSummary_Result> REP_GetTaxOfficerSummary(int TaxOfficeID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_RPT_TaxOfficerSummary(TaxOfficeID).ToList();
            }
        }

        public IList<usp_RPT_TaxOfficeManagerSummary_Result> REP_GetTaxOfficeManagerSummary(int TaxOfficeID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_RPT_TaxOfficeManagerSummary(TaxOfficeID).ToList();
            }
        }

        public IList<usp_GetTaxPayerforTaxOfficeManager_Result> REP_GetTaxPayerBasedOnTaxOfficeManager(int TaxOfficeID, int TOManagerID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetTaxPayerforTaxOfficeManager(TaxOfficeID, TOManagerID).ToList();
            }
        }

        public IList<usp_RPT_ReviewStatusSummary_Result> REP_GetReviewStatusSummary(int TaxOfficeID, int TaxPayerTypeID, int ReviewStatusID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_RPT_ReviewStatusSummary(TaxOfficeID, TaxPayerTypeID, ReviewStatusID).ToList();
            }
        }

        public IList<usp_RPT_ManagerReviewStatusSummary_Result> REP_GetManagerReviewStatusSummary(int TaxOfficeID, int TOManagerID, int ReviewStatusID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_RPT_ManagerReviewStatusSummary(TaxOfficeID, TOManagerID, ReviewStatusID).ToList();
            }
        }

        public IList<usp_RPT_StaffReviewStatusSummary_Result> REP_GetStaffReviewStatusSummary(int TaxOfficeID, int TaxOfficerID, int ReviewStatusID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_RPT_StaffReviewStatusSummary(TaxOfficeID, TaxOfficerID, ReviewStatusID).ToList();
            }
        }

        public IList<usp_RPT_TaxOfficerMonthlyPayment_Result> REP_GetTaxOfficerMonthlyPayment(int TaxOfficeID, int TaxOfficerID, int TOManagerID, int Year)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_RPT_TaxOfficerMonthlyPayment(TaxOfficeID, TaxOfficerID, TOManagerID, Year).ToList();
            }
        }

        public IList<usp_RPT_MonthlyTaxOfficeTarget_Result> REP_GetMonthlyTaxOfficeTarget(int TaxOfficeID, int Year)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_RPT_MonthlyTaxOfficeTarget(TaxOfficeID, Year).ToList();
            }
        }

        public IList<usp_RPT_TaxOfficeByRevenueStreamTarget_Result> REP_GetTaxOfficeByRevenueStreamTarget(int TaxOfficeID, int Year, int Month)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_RPT_TaxOfficeByRevenueStreamTarget(TaxOfficeID, Year, Month).ToList();
            }
        }

        public IList<usp_RPT_RevenueStreamByTaxOfficeTarget_Result> REP_GetRevenueStreamByTaxOfficeTarget(int RevenueStreamID, int Year, int Month,int taxofficeId)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_RPT_RevenueStreamByTaxOfficeTarget(RevenueStreamID, Year, Month, taxofficeId).ToList();
            }
        }

        public IList<usp_RPT_GetRevenueStreamByPaymentChannel_Result> REP_GetRevenueStreamByPaymentChannel(int SettlementMethodID, int Year, DateTime? FromDate, DateTime? ToDate)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_RPT_GetRevenueStreamByPaymentChannel(Year, FromDate, ToDate, SettlementMethodID).ToList();
            }
        }

        public IList<usp_RPT_GetPaymentChannelByRevenueStream_Result> REP_GetPaymentChannelByRevenueStream(int RevenueStreamID, int Year, DateTime? FromDate, DateTime? ToDate)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_RPT_GetPaymentChannelByRevenueStream(Year, FromDate,ToDate, RevenueStreamID).ToList();
            }
        }

        public IList<usp_RPT_GetPaymentDetail_Result> REP_GetPaymentChannelDetail(int SettlementMethodID, int RevenueStreamID, int Year, DateTime? FromDate, DateTime? ToDate)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_RPT_GetPaymentDetail(Year, SettlementMethodID, RevenueStreamID, FromDate, ToDate).ToList();
            }
        }

        public IList<usp_RPT_TaxOfficerByRevenueStreamTarget_Result> REP_GetTaxOfficerByRevenueStreamTarget(int TaxOfficerID, int Year, int Month)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_RPT_TaxOfficerByRevenueStreamTarget(TaxOfficerID, Year, Month).ToList();
            }
        }

        public IList<usp_RPT_RevenueStreamByTaxOfficerTarget_Result> REP_GetRevenueStreamByTaxOfficerTarget(int TaxOfficeID, int RevenueStreamID, int Year, int Month)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_RPT_RevenueStreamByTaxOfficerTarget(TaxOfficeID, RevenueStreamID, Year, Month).ToList();
            }
        }

        public IList<usp_RPT_GetRevenueStreamByTaxOfficerTargetDetail_Result> REP_GetRevenueStreamByTaxOfficerTargetDetail(int TaxOfficerID, int RevenueStreamID, int Year, int Month)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_RPT_GetRevenueStreamByTaxOfficerTargetDetail(Year, Month, TaxOfficerID, RevenueStreamID).ToList();
            }
        }

        public IList<usp_RPT_InvidualLiabilityStatus_Result> REP_GetIndividualLiabilityStatus()
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_RPT_InvidualLiabilityStatus().ToList();
            }
        }

        public IList<usp_GetTaxPayerProfileForExport_Result> REP_GetTaxPayerProfileForExport(string pstrProfileID, int pIntYear)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetTaxPayerProfileForExport(pstrProfileID, pIntYear).ToList();
            }
        }


        public IList<usp_GetTaxPayerProfileTypeForExport_Result> REP_GetTaxPayerProfileTypeForExport(int ProfileTypeID, int pIntYear)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetTaxPayerProfileTypeForExport(ProfileTypeID, pIntYear).ToList();
            }
        }
        public IList<usp_RPT_GetTaxPayerTypeByTaxOffice_Result> REP_GetTaxPayerTypeByTaxOffice()
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_RPT_GetTaxPayerTypeByTaxOffice().ToList();
            }
        }

        public IList<usp_GetTaxPayerBasedOnTaxOfficeForExport_Result> REP_GetTaxPayerBasedOnTaxOfficeForExport(int pIntTaxOfficeID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetTaxPayerBasedOnTaxOfficeForExport(pIntTaxOfficeID).ToList();
            }
        }

        public IList<usp_RPT_GetAssetTypeByTaxOffice_Result> REP_GetAssetTypeByTaxOffice()
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_RPT_GetAssetTypeByTaxOffice().ToList();
            }
        }


        public IList<usp_RPT_GetAssetDetailByTaxOffice_Result> REP_GetAssetDetailByTaxOffice(int pIntTaxOfficeID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_RPT_GetAssetDetailByTaxOffice(pIntTaxOfficeID).ToList();
            }
        }

        public IList<usp_GetAssetBasedOnTaxOfficeForExport_Result> REP_GetAssetBasedOnTaxOfficeForExport(int pIntTaxOfficeID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetAssetBasedOnTaxOfficeForExport(pIntTaxOfficeID).ToList();
            }
        }

        public IList<usp_GetPaymentChargeList_Result> REP_GetPaymentChargeList(int pIntTaxPayerID, int pIntTaxPayerTypeID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetPaymentChargeList(pIntTaxPayerTypeID, pIntTaxPayerID).ToList();
            }
        }

        public IList<usp_GetIndividualLiabilityDetail_Result> REP_GetIndividualLiabilityDetail(int pIntTaxPayerID, int pIntTaxPayerTypeID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetIndividualLiabilityDetail(pIntTaxPayerTypeID, pIntTaxPayerID).ToList();
            }
        }

        public IList<usp_GetDemandNoticeList_Result> REP_GetDemandNoticeList()
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetDemandNoticeList().ToList();
            }
        }

        public IList<usp_RPT_GetTaxPayerMonthlyPayment_Result> REP_GetTaxPayerMonthlyPayment(int TaxPayerTypeID, int TaxPayerID, int RevenueStreamID, int Year)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_RPT_GetTaxPayerMonthlyPayment(TaxPayerTypeID, TaxPayerID, Year, RevenueStreamID).ToList();
            }
        }

        public IList<usp_RPT_GetTaxPayerMonthlyPaymentDetail_Result> REP_GetTaxPayerMonthlyPaymentDetail(int TaxPayerTypeID, int TaxPayerID, int RevenueStreamID, int Year, int Month)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_RPT_GetTaxPayerMonthlyPaymentDetail(TaxPayerTypeID, TaxPayerID, Year, Month, RevenueStreamID).ToList();
            }
        }


        public IList<usp_RPT_TaxOfficeAssessmentSummary_Result> REP_GetTaxOfficeAssessmentSummary(int TaxYear)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_RPT_TaxOfficeAssessmentSummary(TaxYear).ToList();
            }
        }

        public IList<usp_RPT_TaxOfficeAssessmentDetail_Result> REP_GetTaxOfficeAssessmentDetail(int TaxOfficeID, int TaxYear)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_RPT_TaxOfficeAssessmentDetail(TaxOfficeID, TaxYear).ToList();
            }
        }


        public IList<usp_RPT_BusinessSectorAssessmentSummary_Result> REP_GetBusinessSectorAssessmentSummary(int TaxYear, int BusinessTypeID, int BusinessCategoryID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_RPT_BusinessSectorAssessmentSummary(TaxYear, BusinessTypeID, BusinessCategoryID).ToList();
            }
        }

        public IList<usp_RPT_BusinessSectorAssessmentDetail_Result> REP_GetBusinessSectorAssessmentDetail(int TaxYear, int BusinessSectorID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_RPT_BusinessSectorAssessmentDetail(TaxYear, BusinessSectorID).ToList();
            }
        }

        public IList<usp_GetTreasuryReceiptByRevenueStream_Result> REP_GetTreasuryReceiptByRevenueStream(int pIntTaxYear, DateTime? pdtFromDate, DateTime? pdtToDate)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetTreasuryReceiptByRevenueStream(pIntTaxYear, pdtFromDate, pdtToDate).ToList();
            }
        }
        public IList<usp_GetTreasuryReceiptByRevenueStreamDetail_Result> REP_GetTreasuryReceiptByRevenueStreamDetails(int pIntTaxYear, DateTime? pdtFromDate, DateTime? pdtToDate, int? pIntRevenueStreamID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetTreasuryReceiptByRevenueStreamDetail(pIntTaxYear, pdtFromDate, pdtToDate, pIntRevenueStreamID).ToList();
            }
        }

        public IList<usp_GetTreasuryReceiptByTaxOffice_Result> REP_GetTreasuryReceiptByTaxOffice(int pIntTaxYear, DateTime? pdtFromDate, DateTime? pdtToDate)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetTreasuryReceiptByTaxOffice(pIntTaxYear, pdtFromDate, pdtToDate).ToList();
            }
        }
        public IList<usp_GetTreasuryReceiptByTaxOfficeDetail_Result> REP_GetTreasuryReceiptByTaxOfficeDetails(int pIntTaxYear, DateTime? pdtFromDate, DateTime? pdtToDate, int? pIntTaxOfficeID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetTreasuryReceiptByTaxOfficeDetail(pIntTaxYear, pdtFromDate, pdtToDate, pIntTaxOfficeID).ToList();
            }
        }

        public IList<usp_RPT_EmployerLiability_Result> REP_GetEmployerLiability()
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_RPT_EmployerLiability().ToList();
            }
        }

        public IList<usp_RPT_EmployerPAYELiability_Result> REP_GetEmployerPAYELiability()
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_RPT_EmployerPAYELiability().ToList();
            }
        }

        public IList<usp_RPT_MonthlyTCCSummary_Result> REP_GetMonthlyTCCSummary(TCCReportSearchParams pObjSearchParams)
        {
            using(_db = new EIRSEntities())
            {
                return _db.usp_RPT_MonthlyTCCSummary(pObjSearchParams.TaxYear, pObjSearchParams.StageID, pObjSearchParams.StatusID).ToList();
            }
        }

        public IList<usp_RPT_MonthlyTCCSummaryDetail_Result> REP_GetMonthlyTCCSummaryDetail(TCCReportSearchParams pObjSearchParams)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_RPT_MonthlyTCCSummaryDetail(pObjSearchParams.TaxYear, pObjSearchParams.TaxMonth, pObjSearchParams.StageID, pObjSearchParams.StatusID).ToList();
            }
        }

        public IList<usp_RPT_MonthlyTCCCommissionList_Result> REP_GetMonthlyTCCCommissionSummary(TCCReportSearchParams pObjSearchParams)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_RPT_MonthlyTCCCommissionList(pObjSearchParams.TaxYear, pObjSearchParams.StageID, pObjSearchParams.StatusID).ToList();
            }
        }

        public IList<usp_RPT_MonthlyTCCRevokeList_Result> REP_GetMonthlyTCCRevokeSummary(TCCReportSearchParams pObjSearchParams)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_RPT_MonthlyTCCRevokeList(pObjSearchParams.TaxYear, pObjSearchParams.StageID, pObjSearchParams.StatusID).ToList();
            }
        }

        public IList<usp_RPT_MonthlyTCCRevokeDetail_Result> REP_GetMonthlyTCCRevokeDetail(TCCReportSearchParams pObjSearchParams)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_RPT_MonthlyTCCRevokeDetail(pObjSearchParams.TaxYear, pObjSearchParams.TaxMonth, pObjSearchParams.StageID, pObjSearchParams.StatusID).ToList();
            }
        }

        public IList<usp_RPT_ReviseBill_Result> REP_GetReviseBillSummary(int TaxYear, DateTime? FromDate, DateTime? ToDate, int BillTypeID, int RevenueStreamID)
        {
            using(_db = new EIRSEntities())
            {
                return _db.usp_RPT_ReviseBill(TaxYear, FromDate, ToDate, RevenueStreamID, BillTypeID).ToList();
            }
        }

        public IList<usp_RPT_TaxPayerCaptureAnalysis_Result> REP_TaxPayerCaptureAnalysis(DateTime? FromDate, DateTime? ToDate, int TaxPayerTypeID, int? TaxOfficeID)
        {
            using(_db = new EIRSEntities())
            {
                return _db.usp_RPT_TaxPayerCaptureAnalysis(FromDate, ToDate, TaxPayerTypeID, TaxOfficeID).ToList();
            }
        }
    }
}
