using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Web.Models;
using Elmah;
using System;
using System.Collections.Generic;
using System.Transactions;
using System.Web.Mvc;
using System.Linq;
using static EIRS.Web.Controllers.Filters;
using EIRS.Web.Utility;

namespace EIRS.Web.Controllers
{
    [SessionTimeout]
    public class SettlementRevocationController : BaseController
    {
        EIRSEntities _db;
        public void UI_FillDropDown(IList<usp_GetSettlementList_Result> lstSettlement)
        {
            ViewBag.SettlementDropDownList = new SelectList(lstSettlement.Where(t => t.SettlementAmount > 0), "SettlementID", "SettlementRefNo");

            UI_FillRevenueStreamDropDown();
            UI_FillAgencyDropDown();

        }

        [HttpGet]
        public ActionResult Assessment(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                BLAssessment mObjBLAssessment = new BLAssessment();
                usp_GetAssessmentList_Result mObjAssessmentData = mObjBLAssessment.BL_GetAssessmentDetails(new Assessment() { AssessmentID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjAssessmentData != null)
                {
                    IList<usp_GetAssessment_AssessmentRuleList_Result> lstMAPAssessmentRules = mObjBLAssessment.BL_GetAssessmentRules(mObjAssessmentData.AssessmentID.GetValueOrDefault());
                    IList<usp_GetAssessmentRuleItemList_Result> lstAssessmentItems = mObjBLAssessment.BL_GetAssessmentRuleItem(mObjAssessmentData.AssessmentID.GetValueOrDefault());
                    IList<usp_GetAssessmentRuleBasedSettlement_Result> lstAssessmentRuleSettlement = mObjBLAssessment.BL_GetAssessmentRuleBasedSettlement(mObjAssessmentData.AssessmentID.GetValueOrDefault());
                    IList<usp_GetAssessmentAdjustmentList_Result> lstAssessmentAdjustment = mObjBLAssessment.BL_GetAssessmentAdjustment(mObjAssessmentData.AssessmentID.GetValueOrDefault());
                    IList<usp_GetAssessmentLateChargeList_Result> lstAssessmentLateCharge = mObjBLAssessment.BL_GetAssessmentLateCharge(mObjAssessmentData.AssessmentID.GetValueOrDefault());
                    IList<usp_GetSettlementList_Result> lstSettlement = new BLSettlement().BL_GetSettlementList(new Settlement() { ServiceBillID = -1, AssessmentID = mObjAssessmentData.AssessmentID.GetValueOrDefault() });

                    ViewBag.MAPAssessmentRules = lstMAPAssessmentRules;
                    ViewBag.AssessmentItems = lstAssessmentItems;
                    ViewBag.AssessmentRuleSettlement = lstAssessmentRuleSettlement;
                    ViewBag.SettlementList = lstSettlement;
                    ViewBag.AdjustmentList = lstAssessmentAdjustment;
                    ViewBag.LateChargeList = lstAssessmentLateCharge;

                    UI_FillDropDown(lstSettlement);
                    return View(mObjAssessmentData);
                }
                else
                {
                    return RedirectToAction("SettlementRevocation", "OperationManager");
                }
            }
            else
            {
                return RedirectToAction("SettlementRevocation", "OperationManager");
            }
        }

        [HttpGet]
        public ActionResult ServiceBill(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                BLServiceBill mObjBLServiceBill = new BLServiceBill();

                usp_GetServiceBillList_Result mObjServiceBillData = mObjBLServiceBill.BL_GetServiceBillDetails(new ServiceBill() { ServiceBillID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjServiceBillData != null)
                {
                    IList<usp_GetServiceBill_MDAServiceList_Result> lstMAPServiceBillServices = mObjBLServiceBill.BL_GetMDAServiceList(mObjServiceBillData.ServiceBillID.GetValueOrDefault());
                    IList<usp_GetServiceBillItemList_Result> lstServiceBillItems = mObjBLServiceBill.BL_GetServiceBillItem(mObjServiceBillData.ServiceBillID.GetValueOrDefault());
                    IList<usp_GetMDAServiceBasedSettlement_Result> lstMDAServiceSettlement = mObjBLServiceBill.BL_GetMDAServiceBasedSettlement(mObjServiceBillData.ServiceBillID.GetValueOrDefault());
                    IList<usp_GetServiceBillAdjustmentList_Result> lstServiceBillAdjustment = mObjBLServiceBill.BL_GetServiceBillAdjustment(mObjServiceBillData.ServiceBillID.GetValueOrDefault());
                    IList<usp_GetServiceBillLateChargeList_Result> lstServiceBillLateCharge = mObjBLServiceBill.BL_GetServiceBillLateCharge(mObjServiceBillData.ServiceBillID.GetValueOrDefault());
                    IList<usp_GetSettlementList_Result> lstSettlement = new BLSettlement().BL_GetSettlementList(new Settlement() { AssessmentID = -1, ServiceBillID = mObjServiceBillData.ServiceBillID.GetValueOrDefault() });

                    ViewBag.MAPServiceBillRules = lstMAPServiceBillServices;
                    ViewBag.ServiceBillItems = lstServiceBillItems;
                    ViewBag.ServiceBillRuleSettlement = lstMDAServiceSettlement;
                    ViewBag.SettlementList = lstSettlement;
                    ViewBag.AdjustmentList = lstServiceBillAdjustment;
                    ViewBag.LateChargeList = lstServiceBillLateCharge;

                    UI_FillDropDown(lstSettlement);
                    return View(mObjServiceBillData);
                }
                else
                {
                    return RedirectToAction("SettlementRevocation", "OperationManager");
                }
            }
            else
            {
                return RedirectToAction("SettlementRevocation", "OperationManager");
            }
        }

        public JsonResult AddABSettlementRevocation(SettlementRevocationViewModel pObjSettlementRevocation)
        {
            BLAssessment mObjBLAssessment = new BLAssessment();
            BLSettlement mObjBLSettlement = new BLSettlement();
            BLPaymentAccount mObjBLPaymentAccount = new BLPaymentAccount();

            IDictionary<string, object> dcResponse = new Dictionary<string, object>();
            List<Settlement> mObjSettlementList = new List<Settlement>();
            using (_db = new EIRSEntities())
            {
                mObjSettlementList = _db.Settlements.Where(o => o.AssessmentID == pObjSettlementRevocation.AssessmentID).ToList();
            }
            var totalSettledSum = mObjSettlementList.Sum(o => o.SettlementAmount);
            if ((totalSettledSum - pObjSettlementRevocation.ReversalAmount) >= 0)
            {
                usp_GetSettlementList_Result mObjSettlementDetails = mObjBLSettlement.BL_GetSettlementDetails(new Settlement() { SettlementID = pObjSettlementRevocation.SettlementID });

                using (TransactionScope scope = new TransactionScope())
                {
                    //Add Settlement
                    Settlement mObjSettlement = new Settlement()
                    {
                        AssessmentID = pObjSettlementRevocation.AssessmentID,
                        SettlementDate = CommUtil.GetCurrentDateTime(),
                        SettlementAmount = 0 - pObjSettlementRevocation.ReversalAmount,
                        SettlementMethodID = mObjSettlementDetails.SettlementMethodID,
                        TransactionRefNo = $"RVSL-{mObjSettlementDetails.TransactionReferenceNo}",
                        SettlementNotes = "Settlement Reversed",
                        Active = true,
                        CreatedBy = SessionManager.UserID,
                        CreatedDate = CommUtil.GetCurrentDateTime(),
                        ValidateDuplicateCheck = false
                    };

                    try
                    {
                        FuncResponse<Settlement> mObjSettlementResponse = mObjBLSettlement.BL_InsertUpdateSettlement(mObjSettlement);
                        if (mObjSettlementResponse.Success)
                        {
                            usp_GetSettlementItemDetails_Result mObjSettlementItemDetails = new BLSettlement().BL_GetSettlementItemDetails(pObjSettlementRevocation.SIID);

                            MAP_Settlement_SettlementItem mObjSettlementItem = new MAP_Settlement_SettlementItem()
                            {
                                AAIID = mObjSettlementItemDetails.ItemID,
                                SettlementID = mObjSettlementResponse.AdditionalData.SettlementID,
                                SettlementAmount = 0 - pObjSettlementRevocation.ReversalAmount,
                                TaxAmount = mObjSettlementItemDetails.TotalAmount,
                                CreatedBy = SessionManager.UserID,
                                CreatedDate = CommUtil.GetCurrentDateTime()
                            };

                            FuncResponse mObjSIResponse = mObjBLSettlement.BL_InsertSettlementItem(mObjSettlementItem);

                            if (mObjSIResponse.Success)
                            {
                                decimal? mssiAmount = 0;
                                MAP_Assessment_AssessmentItem ret = mObjBLAssessment.GetAssessmentItems(mObjSettlementItemDetails.ItemID.GetValueOrDefault());
                                MAP_Assessment_AssessmentItem mObjAAI = new MAP_Assessment_AssessmentItem()
                                {
                                    AAIID = mObjSettlementItemDetails.ItemID.GetValueOrDefault(),
                                    ModifiedBy = SessionManager.UserID,
                                    ModifiedDate = CommUtil.GetCurrentDateTime(),
                                    TaxAmount = ret.TaxAmount,
                                    TaxBaseAmount = ret.TaxBaseAmount
                                };
                                using (_db = new EIRSEntities())
                                {
                                    mssiAmount = _db.MAP_Settlement_SettlementItem.Where(o => o.AAIID == mObjAAI.AAIID).Sum(o => o.SettlementAmount);
                                }

                                if (mssiAmount == 0)
                                {
                                    mObjAAI.PaymentStatusID = (int)EnumList.PaymentStatus.Due;
                                }
                                else
                                {
                                    mObjAAI.PaymentStatusID = (int)EnumList.PaymentStatus.Partial;
                                }

                                if (mObjAAI.PaymentStatusID != null)
                                {
                                    mObjBLAssessment.BL_UpdateAssessmentItemStatus(mObjAAI);
                                }
                            }
                            else
                            {
                                throw (mObjSIResponse.Exception);
                            }
                            var kkk = totalSettledSum + mObjSettlementResponse.AdditionalData.SettlementAmount;
                            // var kkk = mObjBLSettlement.BL_GetSettlementList(new Settlement() { AssessmentID = pObjSettlementRevocation.AssessmentID });

                            //NOT NEEDED AGAIN IN THE NEW PRD
                            //Revert Servicebill status
                            Assessment mObjAssessment = new Assessment()
                            {
                                AssessmentID = pObjSettlementRevocation.AssessmentID,
                                SettlementDate = CommUtil.GetCurrentDateTime(),
                                ModifiedDate = CommUtil.GetCurrentDateTime(),
                                ModifiedBy = SessionManager.UserID
                            };
                            decimal? Amountholder = 0;
                            int Statusholder = 0;
                            using (_db = new EIRSEntities())
                            {
                                var retVal = _db.Assessments.FirstOrDefault(o => o.AssessmentID == mObjAssessment.AssessmentID);
                                Amountholder = retVal.AssessmentAmount;
                            }


                            if (Amountholder == kkk)
                                mObjAssessment.SettlementStatusID = (int)EnumList.SettlementStatus.Settled;
                         
                            else if (kkk == 0)
                                mObjAssessment.SettlementStatusID = (int)EnumList.SettlementStatus.Assessed;
                            else if (Amountholder > kkk)
                                mObjAssessment.SettlementStatusID = (int)EnumList.SettlementStatus.Partial;
                            else
                                mObjAssessment.SettlementStatusID = (int)EnumList.SettlementStatus.Assessed;


                            mObjAssessment.AssessmentAmount = Amountholder;
                            mObjBLAssessment.BL_UpdateAssessmentSettlementStatus(mObjAssessment);

                            //Add PoA
                            Payment_Account mObjPoA = new Payment_Account()
                            {
                                PaymentAccountID = 0,
                                TaxPayerTypeID = mObjSettlementDetails.TaxPayerTypeID,
                                TaxPayerID = mObjSettlementDetails.TaxPayerID,
                                Amount = pObjSettlementRevocation.ReversalAmount,
                                SettlementMethodID = 7,
                                SettlementStatusID = (int)EnumList.SettlementStatus.Settled,
                                PaymentDate = CommUtil.GetCurrentDateTime(),
                                TransactionRefNo = $"SRVSL{mObjSettlementDetails.SettlementRefNo}",
                                Notes = pObjSettlementRevocation.PoANotes,
                                RevenueStreamID = pObjSettlementRevocation.RevenueStreamID,
                                RevenueSubStreamID = pObjSettlementRevocation.RevenueSubStreamID,
                                AgencyID = pObjSettlementRevocation.AgencyID,
                                Active = true,
                                CreatedDate = CommUtil.GetCurrentDateTime(),
                                CreatedBy = SessionManager.UserID,
                                ValidateDuplicateCheck = false
                            };

                            FuncResponse<Payment_Account> mObjPoAFuncResponse = mObjBLPaymentAccount.BL_InsertUpdatePaymentAccount(mObjPoA);

                            if (mObjPoAFuncResponse.Success)
                            {
                                scope.Complete();
                                dcResponse["success"] = true;
                                dcResponse["Message"] = "Settlement Reveresed successfully";
                            }
                            else
                            {
                                dcResponse["success"] = false;
                                dcResponse["Message"] = mObjPoAFuncResponse.Message;
                            }
                        }
                        else
                        {
                            dcResponse["success"] = false;
                            dcResponse["Message"] = mObjSettlementResponse.Message;
                            Transaction.Current.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.SendErrorToText(ex);
                        ErrorSignal.FromCurrentContext().Raise(ex);

                        dcResponse["success"] = false;
                        dcResponse["Message"] = "Error occurred while saving settlement";

                        Transaction.Current.Rollback();
                    }
                }
                return Json(dcResponse, JsonRequestBehavior.AllowGet);
            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "This Assessment cant be Revoked as the revoked amount will be greater than the sum of the settlemnt amount";


            }
            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddSBSettlementRevocation(SettlementRevocationViewModel pObjSettlementRevocation)
        {
            BLServiceBill mObjBLServiceBill = new BLServiceBill();
            BLSettlement mObjBLSettlement = new BLSettlement();
            BLPaymentAccount mObjBLPaymentAccount = new BLPaymentAccount();

            IDictionary<string, object> dcResponse = new Dictionary<string, object>();
            List<Settlement> mObjSettlementList = new List<Settlement>();
            using (_db = new EIRSEntities())
            {
                mObjSettlementList = _db.Settlements.Where(o => o.ServiceBillID == pObjSettlementRevocation.ServiceBillID).ToList();
            }
            var totalSettledSum = mObjSettlementList.Sum(o => o.SettlementAmount);
            if ((totalSettledSum - pObjSettlementRevocation.ReversalAmount) >= 0)
            {

                usp_GetSettlementList_Result mObjSettlementDetails = mObjBLSettlement.BL_GetSettlementDetails(new Settlement() { SettlementID = pObjSettlementRevocation.SettlementID });

                using (TransactionScope scope = new TransactionScope())
                {
                    //Add Settlement
                    Settlement mObjSettlement = new Settlement()
                    {
                        ServiceBillID = pObjSettlementRevocation.ServiceBillID,
                        SettlementDate = CommUtil.GetCurrentDateTime(),
                        SettlementAmount = 0 - pObjSettlementRevocation.ReversalAmount,
                        SettlementMethodID = mObjSettlementDetails.SettlementMethodID,
                        TransactionRefNo = $"RVSL-{mObjSettlementDetails.TransactionReferenceNo}",
                        SettlementNotes = "Settlement Reversed",
                        Active = true,
                        CreatedBy = SessionManager.UserID,
                        CreatedDate = CommUtil.GetCurrentDateTime(),
                        ValidateDuplicateCheck = false
                    };

                    try
                    {
                        FuncResponse<Settlement> mObjSettlementResponse = mObjBLSettlement.BL_InsertUpdateSettlement(mObjSettlement);
                        if (mObjSettlementResponse.Success)
                        {
                            usp_GetSettlementItemDetails_Result mObjSettlementItemDetails = new BLSettlement().BL_GetSettlementItemDetails(pObjSettlementRevocation.SIID);

                            //MAP_Assessment_AssessmentItem ret = mObjBLSettlement.getse(mObjSettlementItemDetails.ItemID.GetValueOrDefault());
                            MAP_Settlement_SettlementItem mObjSettlementItem = new MAP_Settlement_SettlementItem()
                            {
                                SBSIID = mObjSettlementItemDetails.ItemID,
                                SettlementID = mObjSettlementResponse.AdditionalData.SettlementID,
                                SettlementAmount = 0 - pObjSettlementRevocation.ReversalAmount,
                                TaxAmount = mObjSettlementItemDetails.TaxAmount,
                                CreatedBy = SessionManager.UserID,
                                CreatedDate = CommUtil.GetCurrentDateTime()
                            };

                            FuncResponse mObjSIResponse = mObjBLSettlement.BL_InsertSettlementItem(mObjSettlementItem);

                            if (mObjSIResponse.Success)
                            {
                                decimal? mssiAmount = 0;
                                MAP_ServiceBill_MDAServiceItem mObjSBMSI = new MAP_ServiceBill_MDAServiceItem()
                                {
                                    SBSIID = mObjSettlementItemDetails.ItemID.GetValueOrDefault(),
                                    ModifiedBy = SessionManager.UserID,
                                    ModifiedDate = CommUtil.GetCurrentDateTime()
                                };

                                using (_db = new EIRSEntities())
                                {
                                    var ret = _db.MAP_ServiceBill_MDAServiceItem.FirstOrDefault(o=>o.SBSIID == mObjSBMSI.SBSIID);
                                    mObjSBMSI.ServiceAmount = ret.ServiceAmount;
                                    mObjSBMSI.ServiceBaseAmount = ret.ServiceBaseAmount; 
                                    
                                    mssiAmount = _db.MAP_Settlement_SettlementItem.Where(o => o.SBSIID == mObjSBMSI.SBSIID).Sum(o => o.SettlementAmount);
                                }
                                

                                if (mssiAmount == 0)
                                {
                                    mObjSBMSI.PaymentStatusID = (int)EnumList.PaymentStatus.Due;
                                }
                                else
                                {
                                    mObjSBMSI.PaymentStatusID = (int)EnumList.PaymentStatus.Partial;
                                }


                                if (mObjSBMSI.PaymentStatusID != null)
                                {
                                    mObjBLServiceBill.BL_UpdateMDAServiceItemStatus(mObjSBMSI);
                                }
                            }
                            else
                            {
                                throw (mObjSIResponse.Exception);
                            }
                            var kkk = totalSettledSum + mObjSettlementResponse.AdditionalData.SettlementAmount;
                            //Revert Servicebill status
                            ServiceBill mObjServiceBill = new ServiceBill()
                            {
                                ServiceBillID = pObjSettlementRevocation.ServiceBillID,
                                SettlementDate = CommUtil.GetCurrentDateTime(),
                               // SettlementStatusID = (int)EnumList.SettlementStatus.Partial,
                                ModifiedDate = CommUtil.GetCurrentDateTime(),
                                ModifiedBy = SessionManager.UserID
                            };
                            decimal? Amountholder = 0;
                            int Statusholder = 0;
                            using (_db = new EIRSEntities())
                            {
                                var retVal = _db.ServiceBills.FirstOrDefault(o => o.ServiceBillID == pObjSettlementRevocation.ServiceBillID);
                                Amountholder = retVal.ServiceBillAmount;
                                mObjServiceBill.ServiceBillAmount = retVal.ServiceBillAmount;
                            }
                            if (Amountholder == kkk)
                                mObjServiceBill.SettlementStatusID = (int)EnumList.SettlementStatus.Settled;
                            else if (kkk == 0)
                                mObjServiceBill.SettlementStatusID = (int)EnumList.SettlementStatus.Assessed;
                            else if (Amountholder > kkk)
                                mObjServiceBill.SettlementStatusID = (int)EnumList.SettlementStatus.Partial;
                            else
                                mObjServiceBill.SettlementStatusID = (int)EnumList.SettlementStatus.Assessed;


                            mObjBLServiceBill.BL_UpdateServiceBillSettlementStatus(mObjServiceBill);

                            //Add PoA
                            Payment_Account mObjPoA = new Payment_Account()
                            {
                                PaymentAccountID = 0,
                                TaxPayerTypeID = mObjSettlementDetails.TaxPayerTypeID,
                                TaxPayerID = mObjSettlementDetails.TaxPayerID,
                                Amount = pObjSettlementRevocation.ReversalAmount,
                                SettlementMethodID = 7,
                                SettlementStatusID = (int)EnumList.SettlementStatus.Settled,
                                PaymentDate = CommUtil.GetCurrentDateTime(),
                                TransactionRefNo = $"SRVSL{mObjSettlementDetails.SettlementRefNo}",
                                Notes = pObjSettlementRevocation.PoANotes,
                                RevenueStreamID = pObjSettlementRevocation.RevenueStreamID,
                                RevenueSubStreamID = pObjSettlementRevocation.RevenueSubStreamID,
                                AgencyID = pObjSettlementRevocation.AgencyID,
                                Active = true,
                                CreatedDate = CommUtil.GetCurrentDateTime(),
                                CreatedBy = SessionManager.UserID,
                                ValidateDuplicateCheck = false,
                            };

                            FuncResponse<Payment_Account> mObjPoAFuncResponse = mObjBLPaymentAccount.BL_InsertUpdatePaymentAccount(mObjPoA);

                            if (mObjPoAFuncResponse.Success)
                            {
                                scope.Complete();
                                dcResponse["success"] = true;
                                dcResponse["Message"] = "Settlement Reveresed successfully";
                            }
                            else
                            {
                                dcResponse["success"] = false;
                                dcResponse["Message"] = mObjPoAFuncResponse.Message;
                            }
                        }
                        else
                        {
                            dcResponse["success"] = false;
                            dcResponse["Message"] = mObjSettlementResponse.Message;
                            Transaction.Current.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorSignal.FromCurrentContext().Raise(ex);
                        Logger.SendErrorToText(ex);
                        dcResponse["success"] = false;
                        dcResponse["Message"] = "Error occurred while saving settlement";

                        Transaction.Current.Rollback();
                    }
                }


                return Json(dcResponse, JsonRequestBehavior.AllowGet);
            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "This Assessment cant be Revoked as the revoked amount will be greater than the sum of the settlemnt amount";


            }
            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSettlementItemList(int SettlementID)
        {
            IList<usp_GetSettlementItemList_Result> lstSettlementItem = new BLSettlement().BL_GetSettlementItemList(SettlementID);
            return Json(lstSettlementItem, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSettlementItemDetails(long SIID)
        {
            usp_GetSettlementItemDetails_Result mObjSettlementItemDetails = new BLSettlement().BL_GetSettlementItemDetails(SIID);
            return Json(mObjSettlementItemDetails, JsonRequestBehavior.AllowGet);
        }
    }
}