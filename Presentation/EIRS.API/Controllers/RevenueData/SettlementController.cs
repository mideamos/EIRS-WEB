
using EIRS.API.Models;
using EIRS.API.Utility;
using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using Elmah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Http;

namespace EIRS.API.Controllers.RevenueData
{
    //
    [RoutePrefix("RevenueData/Settlement")]
    public class SettlementController : BaseController
    {
        [HttpPost]
        [Route("Add")]
        public IHttpActionResult Add(SettlementModel pObjSettlementModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();
            String token = Request.Headers.Authorization.Parameter;
            int? userId = Utilities.GetUserId(token);

            if (!ModelState.IsValid)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            }
            else
            {
                var totalAmount = pObjSettlementModel.lstSettlementItems.Sum(O => O.TaxAmount);

                if (totalAmount == 0)
                {
                    mObjAPIResponse.Success = false;
                    mObjAPIResponse.Message = "Sum of Tax Amount cannot be 0";
                    return Ok(mObjAPIResponse);
                }

                //if (pObjSettlementModel.SettlementMethod == (int) Enums.SettlementMethods.InternetWebPay)
                //{
                //    mObjAPIResponse.Success = false;
                //    mObjAPIResponse.Message = "Settlement for InternetWebPay method has been suspended";
                //    return Ok(mObjAPIResponse);
                //}

                using (TransactionScope scope = new TransactionScope())
                {
                    IList<Settlement_ASBItem> lstSettlementItems = pObjSettlementModel.lstSettlementItems;

                    BLSettlement mObjBLSettlement = new BLSettlement();

                    //Check if its reversal
                    if (lstSettlementItems.Sum(t => t.ToSettleAmount) > 0)
                    {
                        var settlementStatus = 0;
                        FuncResponse assessmentUpdateRes = new FuncResponse();
                        FuncResponse serviceBillUpdateRes = new FuncResponse();
                        SettlementRepository settlementmentRepository = new SettlementRepository();
                        AssessmentRepository repository = new AssessmentRepository();

                        Settlement mObjSettlement = new Settlement()
                        {
                            SettlementDate = pObjSettlementModel.SettlementDate,
                            SettlementAmount = lstSettlementItems.Sum(t => t.ToSettleAmount),
                            SettlementMethodID = pObjSettlementModel.SettlementMethod,
                            TransactionRefNo = pObjSettlementModel.TransactionRefNo,
                            SettlementNotes = pObjSettlementModel.Notes,

                            ServiceBillID = pObjSettlementModel.ServiceBillID,
                            AssessmentID = pObjSettlementModel.AssessmentID,

                            Active = true,
                            CreatedBy = userId.HasValue ? userId : 22,
                            CreatedDate = CommUtil.GetCurrentDateTime()
                        };

                        //Check if settlement for assessment or service bill is completed or pending
                        if (pObjSettlementModel.AssessmentID != 0 && pObjSettlementModel.AssessmentID != null)
                        {
                            AssessmentRepository assessmentRepository = new AssessmentRepository();
                            var assessmentData = assessmentRepository.REP_GetAssessmentDetailsById(pObjSettlementModel.AssessmentID.GetValueOrDefault());

                            usp_GetAssessmentList_Result mObjAssessmentData = new BLAssessment().BL_GetAssessmentDetails(new Assessment() { AssessmentID = pObjSettlementModel.AssessmentID.Value, IntStatus = 1 });

                            if (mObjAssessmentData == null)
                            {
                                mObjAPIResponse.Success = false;
                                mObjAPIResponse.Message = "Settlement for invalid assessment";
                                return Ok(mObjAPIResponse);
                            }

                            //var totalSettledAmount = mObjBLSettlement.BL_GetSettlementList(mObjSettlement).Sum(O => O.SettlementAmount) + mObjSettlement.SettlementAmount;
                            var totalAdjustmentAmount = settlementmentRepository.REP_GetAdjustmentListById(pObjSettlementModel.AssessmentID).Sum(O => O.Amount);
                            var totalLateChargeAmount = settlementmentRepository.REP_GetLateChargeListById(pObjSettlementModel.AssessmentID);
                            var totalSettledAmount = settlementmentRepository.REP_GetSettlementListById(null, pObjSettlementModel.AssessmentID).Sum(O => O.SettlementAmount) + mObjSettlement.SettlementAmount + totalAdjustmentAmount + totalLateChargeAmount;
                            settlementStatus = assessmentData.AssessmentAmount > totalSettledAmount ? (int)EnumList.SettlementStatus.Partial : (int)EnumList.SettlementStatus.Settled;

                            // Update assessment status
                            Assessment mObjAssessment = new Assessment()
                            {
                                AssessmentID = pObjSettlementModel.AssessmentID.GetValueOrDefault(),
                                SettlementDate = pObjSettlementModel.SettlementDate,
                                ModifiedDate = CommUtil.GetCurrentDateTime(),
                                ModifiedBy = userId.HasValue ? userId : 22,
                                //ModifiedBy = SessionManager.SystemUserID,
                                AssessmentAmount = assessmentData.AssessmentAmount,
                                SettlementStatusID = settlementStatus
                            };

                            BLAssessment mObjBLAssessment = new BLAssessment();
                            assessmentUpdateRes = mObjBLAssessment.BL_UpdateAssessmentSettlementStatus(mObjAssessment);
                        }

                        if (pObjSettlementModel.ServiceBillID != 0 && pObjSettlementModel.ServiceBillID != null)
                        {
                            ServiceBillRepository serviceBillRepository = new ServiceBillRepository();
                            var serviceBillData = serviceBillRepository.REP_GetServiceBillDetailsById(pObjSettlementModel.ServiceBillID.GetValueOrDefault());

                            usp_GetServiceBillList_Result mObjServiceBillData = new BLServiceBill().BL_GetServiceBillDetails(new ServiceBill() { ServiceBillID = pObjSettlementModel.ServiceBillID.Value, IntStatus = 1 });

                            if (mObjServiceBillData == null)
                            {
                                mObjAPIResponse.Success = false;
                                mObjAPIResponse.Message = "Settlement for invalid service bill";
                                return Ok(mObjAPIResponse);
                            }

                            //var totalSettledAmount = mObjBLSettlement.BL_GetSettlementList(mObjSettlement).Sum(O => O.SettlementAmount) + mObjSettlement.SettlementAmount;
                            var totalAdjustmentAmount = settlementmentRepository.REP_GetAdjustmentServiceBillListById(pObjSettlementModel.ServiceBillID).Sum(O => O.Amount);
                            var totalLateChargeAmount = settlementmentRepository.REP_GetLateChargeServiceBillListById(pObjSettlementModel.ServiceBillID);
                            var totalSettledAmount = settlementmentRepository.REP_GetSettlementListById(pObjSettlementModel.ServiceBillID, null).Sum(O => O.SettlementAmount) + mObjSettlement.SettlementAmount + totalAdjustmentAmount + totalLateChargeAmount;
                            settlementStatus = serviceBillData.ServiceBillAmount > totalSettledAmount ? (int)EnumList.SettlementStatus.Partial : (int)EnumList.SettlementStatus.Settled;

                            //Update service bill status
                            ServiceBill mObjServiceBill = new ServiceBill()
                            {
                                ServiceBillID = pObjSettlementModel.ServiceBillID.GetValueOrDefault(),
                                SettlementDate = pObjSettlementModel.SettlementDate,
                                ModifiedDate = CommUtil.GetCurrentDateTime(),
                                ModifiedBy = userId.HasValue ? userId : 22,
                                ServiceBillAmount = serviceBillData.ServiceBillAmount,
                                //ModifiedBy = SessionManager.SystemUserID,
                                SettlementStatusID = settlementStatus
                            };

                            BLServiceBill mObjBLServiceBill = new BLServiceBill();
                            serviceBillUpdateRes = mObjBLServiceBill.BL_UpdateServiceBillSettlementStatus(mObjServiceBill);

                            //If Service Bill Status is Settled. Check for Request and Mark Paid
                            //if (settlementStatus == (int)EnumList.SettlementStatus.Settled)
                            //{
                            //    //Search for Request with Service Bill
                            //    BLTCC mObjBLTCC = new BLTCC();
                            //    TCC_Request mObjTCCRequest = mObjBLTCC.BL_GetRequestBasedOnServiceBill(pObjSettlementModel.ServiceBillID.GetValueOrDefault());

                            //    if (mObjTCCRequest != null)
                            //    {
                            //        TCC_Request mObjUpdateStatus = new TCC_Request()
                            //        {
                            //            TCCRequestID = mObjTCCRequest.TCCRequestID,
                            //            StatusID = (int)EnumList.TCCRequestStatus.Paid,
                            //            //ModifiedBy = SessionManager.SystemUserID,
                            //            ModifiedDate = CommUtil.GetCurrentDateTime()
                            //        };

                            //        mObjBLTCC.BL_UpdateRequestStatus(mObjUpdateStatus);
                            //    }
                            //}
                        }

                        try
                        {
                            FuncResponse<Settlement> mObjSettlementResponse = mObjBLSettlement.BL_InsertUpdateSettlement(mObjSettlement);
                            var settlementResponseData = mObjSettlementResponse.AdditionalData;

                            if (mObjSettlementResponse.Success && assessmentUpdateRes.Success || mObjSettlementResponse.Success && serviceBillUpdateRes.Success)
                            {
                                BLAssessment mObjBLAssessment = new BLAssessment();
                                BLServiceBill mObjBLServiceBill = new BLServiceBill();
                                var loggedOnUser = userId.HasValue ? userId : 22;

                                foreach (Settlement_ASBItem mObjSAI in lstSettlementItems.Where(o=>o.ToSettleAmount>0))
                                {
                                    if (mObjSAI.PaymentStatusID != (int)EnumList.PaymentStatus.Paid && (mObjSAI.ToSettleAmount > 0 || mObjSAI.TaxAmount == 0))
                                    {
                                        MAP_Settlement_SettlementItem mObjSettlementItem = new MAP_Settlement_SettlementItem()
                                        {
                                            SettlementID = mObjSettlementResponse.AdditionalData.SettlementID,
                                            SettlementAmount = mObjSAI.ToSettleAmount,
                                            TaxAmount = mObjSAI.TaxAmount,
                                            CreatedBy = loggedOnUser,
                                            CreatedDate = CommUtil.GetCurrentDateTime()
                                        };

                                        if (settlementResponseData.ServiceBillID > 0)
                                        {
                                            mObjSettlementItem.SBSIID = mObjSAI.TBPKID;
                                        }

                                        if (settlementResponseData.AssessmentID > 0)
                                        {
                                            mObjSettlementItem.AAIID = mObjSAI.TBPKID;
                                        }

                                        FuncResponse mObjSIResponse = mObjBLSettlement.BL_InsertSettlementItem(mObjSettlementItem);

                                        if (mObjSIResponse.Success)
                                        {
                                            if (settlementResponseData.AssessmentID > 0)
                                            {//
                                             // var newRet = _db.MAP_Settlement_SettlementItem.Where(o => o.AAIID == mObjAIDetail.AAIID);


                                                // MAP_Assessment_AssessmentItem ret = mObjBLAssessment.GetAssessmentItems(mObjAIDetail.AAIID.Value);
                                                MAP_Assessment_AssessmentItem mObjAAI = new MAP_Assessment_AssessmentItem()
                                                {
                                                    AAIID = mObjSAI.TBPKID,
                                                    ModifiedBy = loggedOnUser,
                                                    ModifiedDate = CommUtil.GetCurrentDateTime()
                                                };

                                                //Update Assessment item Status
                                                if (mObjSAI.TaxAmount == (mObjSAI.ToSettleAmount + mObjSAI.SettlementAmount))
                                                {
                                                    mObjAAI.PaymentStatusID = (int)EnumList.PaymentStatus.Paid;
                                                }
                                                else if ((mObjSAI.ToSettleAmount + mObjSAI.SettlementAmount) < mObjSAI.TaxAmount)
                                                {
                                                    mObjAAI.PaymentStatusID = (int)EnumList.PaymentStatus.Partial;
                                                }

                                                if (mObjAAI.PaymentStatusID != null)
                                                    //FuncResponse rettt = repository.REP_UpdateAssessmentItemStatus2(mObjAAI);
                                                    mObjBLAssessment.BL_UpdateAssessmentItemStatus2(mObjAAI);
                                            }
                                            else if (settlementResponseData.ServiceBillID > 0)
                                            {
                                                MAP_ServiceBill_MDAServiceItem mObjSBMSI = new MAP_ServiceBill_MDAServiceItem()
                                                {
                                                    SBSIID = mObjSAI.TBPKID,
                                                    ModifiedBy = loggedOnUser,
                                                    ModifiedDate = CommUtil.GetCurrentDateTime()
                                                };

                                                //Update Assessment item Status
                                                if (mObjSAI.TaxAmount == (mObjSAI.ToSettleAmount + mObjSAI.SettlementAmount))
                                                {
                                                    mObjSBMSI.PaymentStatusID = (int)EnumList.PaymentStatus.Paid;
                                                }
                                                else if ((mObjSAI.ToSettleAmount + mObjSAI.SettlementAmount) < mObjSAI.TaxAmount)
                                                {
                                                    mObjSBMSI.PaymentStatusID = (int)EnumList.PaymentStatus.Partial;
                                                }

                                                if (mObjSBMSI.PaymentStatusID != null)
                                                    mObjBLServiceBill.BL_UpdateMDAServiceItemStatus(mObjSBMSI);
                                            }
                                        }
                                        else
                                        {
                                            mObjAPIResponse.Success = false;
                                            mObjAPIResponse.Message = mObjSettlementResponse.Message;
                                            Transaction.Current.Rollback();

                                            //throw (mObjSIResponse.Exception);
                                        }
                                    }
                                }

                                scope.Complete();
                                mObjAPIResponse.Success = true;
                                mObjAPIResponse.Message = mObjSettlementResponse.Message;
                                mObjAPIResponse.Result = mObjSettlementResponse.AdditionalData.SettlementRefNo;
                            }
                            else
                            {
                                mObjAPIResponse.Success = false;
                                mObjAPIResponse.Message = mObjSettlementResponse.Message;
                                Transaction.Current.Rollback();
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorSignal.FromCurrentContext().Raise(ex);
                            mObjAPIResponse.Success = false;
                            mObjAPIResponse.Message = "Error occurred while saving settlement";
                            Transaction.Current.Rollback();
                        }
                    }
                    else
                    {
                        Settlement mObjSettlement = new Settlement()
                        {
                            SettlementDate = pObjSettlementModel.SettlementDate,
                            SettlementAmount = lstSettlementItems.Sum(t => t.ToSettleAmount),
                            SettlementMethodID = pObjSettlementModel.SettlementMethod,
                            TransactionRefNo = pObjSettlementModel.TransactionRefNo,
                            SettlementNotes = pObjSettlementModel.Notes,

                            ServiceBillID = pObjSettlementModel.ServiceBillID,
                            AssessmentID = pObjSettlementModel.AssessmentID,

                            Active = true,
                            CreatedBy = userId.HasValue ? userId : 22,
                            CreatedDate = CommUtil.GetCurrentDateTime()
                        };


                        try
                        {
                            FuncResponse<Settlement> mObjSettlementResponse = mObjBLSettlement.BL_InsertUpdateSettlement(mObjSettlement);

                            if (mObjSettlementResponse.Success)
                            {
                                scope.Complete();
                                mObjAPIResponse.Success = true;
                                mObjAPIResponse.Message = mObjSettlementResponse.Message;
                                mObjAPIResponse.Result = mObjSettlementResponse.AdditionalData.SettlementRefNo;
                            }
                            else
                            {
                                mObjAPIResponse.Success = false;
                                mObjAPIResponse.Message = mObjSettlementResponse.Message;
                                Transaction.Current.Rollback();
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorSignal.FromCurrentContext().Raise(ex);

                            mObjAPIResponse.Success = false;
                            mObjAPIResponse.Message = "Error occurred while saving settlement";
                            Transaction.Current.Rollback();
                        }
                    }

                }
            }

            return Ok(mObjAPIResponse);
        }
    }
}

