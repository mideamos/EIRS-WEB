//using Aspose.Pdf.Operators;
using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using EIRS.Web.Models;
using EIRS.Web.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using static EIRS.Web.Controllers.Filters;

namespace EIRS.Web.Controllers
{
    [SessionTimeout]
    public class AdjustmentController : BaseController
    {
        EIRSEntities _db;
        IAssessmentRepository _AssessmentRepository;
        public AdjustmentController()
        {
            _AssessmentRepository = new AssessmentRepository();
        }
        public string getUrl()
        {
            string actionName = ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = ControllerContext.RouteData.Values["controller"].ToString();
            var ret = $"/{controllerName}/{actionName}";
            return ret;
        }
        public void UI_FillDropDown()
        {
            UI_FillRevenueStreamDropDown();
            UI_FillAgencyDropDown();
            UI_FillSettlementMethodDropDown();
        }
        // GET: Adjustment
        public ActionResult Assessment(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                BLAssessment mObjBLAssessment = new BLAssessment();
                BLAssessmentItem mObjBLAssessmentItem = new BLAssessmentItem();
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

                    IList<DropDownListResult<long>> lstAssessmentItem = new List<DropDownListResult<long>>();
                    foreach (var item in lstAssessmentItems)
                    {
                        lstAssessmentItem.Add(new DropDownListResult<long>() { Id = item.AAIID.GetValueOrDefault(), Text = item.AssessmentRuleName + " - " + item.AssessmentItemName });
                    }

                    ViewBag.AssessmentItemList = new SelectList(lstAssessmentItem, "Id", "Text");

                    IList<SelectListItem> lstAdjustmentType = new List<SelectListItem>();
                    lstAdjustmentType.Add(new SelectListItem() { Text = "Increase", Value = "1" });
                    lstAdjustmentType.Add(new SelectListItem() { Text = "Deduction", Value = "2" });

                    ViewBag.AdjustmentTypeList = lstAdjustmentType;
                    UI_FillDropDown();
                    return View(mObjAssessmentData);
                }
                else
                {
                    return RedirectToAction("ReviseBill", "OperationManager");
                }
            }
            else
            {
                return RedirectToAction("ReviseBill", "OperationManager");
            }
        }
        public ActionResult AssessmentLateCharge(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                BLAssessment mObjBLAssessment = new BLAssessment();
                BLAssessmentItem mObjBLAssessmentItem = new BLAssessmentItem();
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

                    IList<DropDownListResult<long>> lstAssessmentItem = new List<DropDownListResult<long>>();
                    foreach (var item in lstAssessmentItems)
                    {
                        lstAssessmentItem.Add(new DropDownListResult<long>() { Id = item.AAIID.GetValueOrDefault(), Text = item.AssessmentRuleName + " - " + item.AssessmentItemName });
                    }

                    ViewBag.AssessmentItemList = new SelectList(lstAssessmentItem, "Id", "Text");

                    IList<SelectListItem> lstAdjustmentType = new List<SelectListItem>();
                    lstAdjustmentType.Add(new SelectListItem() { Text = "Increase", Value = "1" });
                    lstAdjustmentType.Add(new SelectListItem() { Text = "Deduction", Value = "2" });

                    ViewBag.AdjustmentTypeList = lstAdjustmentType;
                    UI_FillDropDown();
                    return View(mObjAssessmentData);
                }
                else
                {
                    return RedirectToAction("ReviseBill", "OperationManager");
                }
            }
            else
            {
                return RedirectToAction("ReviseBill", "OperationManager");
            }
        }

        public JsonResult AddABAdjustment(MAP_Assessment_Adjustment pObjAdjustment, int AssessmentID)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();
            try
            {
                decimal? Amountholder = 0;
                List<int?> lstOfProfiles = new List<int?>();
                MAP_Assessment_AssessmentItem retVal2 = new MAP_Assessment_AssessmentItem();
                Assessment retVal = new Assessment();
                using (_db = new EIRSEntities())
                {
                    retVal = _db.Assessments.FirstOrDefault(o => o.AssessmentID == AssessmentID);
                    Amountholder = retVal.AssessmentAmount;
                    retVal2 = _db.MAP_Assessment_AssessmentItem.FirstOrDefault(o => o.AAIID == pObjAdjustment.AAIID);
                    var lstAssessmentRules = _db.MAP_Assessment_AssessmentRule.Where(o => o.AssessmentID == AssessmentID);
                    var lstProfileId = lstAssessmentRules.Select(o => o.ProfileID).ToList();
                    lstOfProfiles = _db.Profiles.Where(o => lstProfileId.Contains(o.ProfileID)).Select(o => o.ProfileTypeID).ToList();
                    lstOfProfiles = lstOfProfiles.Where(o => o.Value.Equals(5) || o.Value.Equals(7)).ToList();
                }
                BLAssessment mObjBLAssessment = new BLAssessment();
                pObjAdjustment.AdjustmentDate = CommUtil.GetCurrentDateTime();
                pObjAdjustment.CreatedDate = CommUtil.GetCurrentDateTime();
                pObjAdjustment.CreatedBy = SessionManager.UserID;

                FuncResponse mObjResponse = mObjBLAssessment.BL_InsertAdjustment(pObjAdjustment);

                //Check if Assessment Amount is Greater than Paid Amount then mark it parital
                usp_GetAssessmentRuleItemDetails_Result mObjAssessmentRuleItemDetails = mObjBLAssessment.BL_GetAssessmentRuleItemDetails(pObjAdjustment.AAIID.GetValueOrDefault());
                usp_GetAssessmentList_Result mObjAssessmentData = mObjBLAssessment.BL_GetAssessmentDetails(new Assessment() { AssessmentID = AssessmentID, IntStatus = 2 });
                IList<usp_GetAssessmentAdjustmentList_Result> lstAssessmentAdjustment = mObjBLAssessment.BL_GetAssessmentAdjustment(mObjAssessmentData.AssessmentID.GetValueOrDefault());
                //AssessmentRepository assessmentRepository = new AssessmentRepository();
                IList<usp_GetAssessmentRuleItemList_Result> lstAssessmentItems = mObjBLAssessment.BL_GetAssessmentRuleItem(mObjAssessmentData.AssessmentID.GetValueOrDefault());

                MAP_Assessment_AssessmentItem ret = mObjBLAssessment.GetAssessmentItems(pObjAdjustment.AAIID.GetValueOrDefault());
                decimal? newTaxAmount;
                decimal? newTaxBaseAmount;

                decimal? newAmount = 0;
                if (pObjAdjustment.AdjustmentTypeID == 1)
                {
                    // var retVal = lstAssessmentItems.FirstOrDefault(o => o.AAIID == pObjAdjustment.AAIID);
                    newAmount = (lstAssessmentItems.Sum(t => t.TaxAmount) + pObjAdjustment.Amount);
                    ret.TaxAmount = ret.TaxAmount + pObjAdjustment.Amount;
                }
                else
                {

                    // var retVal = lstAssessmentItems.FirstOrDefault(o => o.AAIID == pObjAdjustment.AAIID);
                    newAmount = (lstAssessmentItems.Sum(t => t.TaxAmount) + pObjAdjustment.Amount);
                    ret.TaxAmount = ret.TaxAmount + pObjAdjustment.Amount;

                }
                int statId = 1;
                if (mObjAssessmentData.SettlementAmount >= newAmount)
                    statId = (int)EnumList.SettlementStatus.Settled;
                else if (mObjAssessmentData.SettlementAmount < newAmount && newAmount != 0)
                    statId = (int)EnumList.SettlementStatus.Partial;

                //update item table
                MAP_Assessment_AssessmentItem mObjAAI = new MAP_Assessment_AssessmentItem()
                {
                    AAIID = pObjAdjustment.AAIID.GetValueOrDefault(),
                    ModifiedBy = SessionManager.UserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime(),
                    PaymentStatusID = statId,
                    TaxAmount = retVal2.TaxAmount,
                    TaxBaseAmount = retVal2.TaxBaseAmount
                };
                mObjBLAssessment.BL_UpdateAssessmentItemStatus(mObjAAI);
                Assessment mObjAssessment = new Assessment();
                //update assessment table
                if (lstOfProfiles.Count > 0)
                {
                    mObjAssessment = new Assessment()
                    {
                        AssessmentID = AssessmentID,
                        SettlementStatusID = retVal.SettlementStatusID == 3 ? 8 : 6,
                        ModifiedDate = CommUtil.GetCurrentDateTime(),
                        ModifiedBy = SessionManager.UserID,
                        AssessmentAmount = Amountholder
                    };
                }
                else
                {
                    mObjAssessment = new Assessment()
                    {
                        AssessmentID = AssessmentID,
                        SettlementStatusID = statId,
                        ModifiedDate = CommUtil.GetCurrentDateTime(),
                        ModifiedBy = SessionManager.UserID,
                        AssessmentAmount = Amountholder
                    };
                }

                mObjBLAssessment.BL_UpdateAssessmentSettlementStatus(mObjAssessment);

                dcResponse["success"] = mObjResponse.Success;
                dcResponse["Message"] = mObjResponse.Message;

            }
            catch (Exception ex)
            {
                Logger.SendErrorToText(ex);
            }
            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddABAdjustmentWithPoA(AdjustmentWithPoAModel pObjAdjustmentData)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            try
            {
                MAP_Assessment_Adjustment mObjAdjustment = new MAP_Assessment_Adjustment()
                {
                    AAIID = pObjAdjustmentData.AAIID,
                    AdjustmentLine = pObjAdjustmentData.AdjustmentLine,
                    AdjustmentTypeID = pObjAdjustmentData.AdjustmentTypeID,
                    Amount = pObjAdjustmentData.AdjustmentAmount,
                    AdjustmentDate = CommUtil.GetCurrentDateTime(),
                    CreatedDate = CommUtil.GetCurrentDateTime(),
                    CreatedBy = SessionManager.UserID
                };

                FuncResponse mObjAdjResponse = new BLAssessment().BL_InsertAdjustment(mObjAdjustment);

                if (mObjAdjResponse.Success)
                {
                    usp_GetAssessmentList_Result mObjAssessmentData = new BLAssessment().BL_GetAssessmentDetails(new Assessment() { AssessmentID = pObjAdjustmentData.AssessmentID, IntStatus = 2 });

                    Payment_Account mObjPoA = new Payment_Account()
                    {
                        PaymentAccountID = 0,
                        TaxPayerTypeID = mObjAssessmentData.TaxPayerTypeID,
                        TaxPayerID = mObjAssessmentData.TaxPayerID,
                        Amount = pObjAdjustmentData.PoAAmount,
                        SettlementMethodID = pObjAdjustmentData.PaymentMethodID,
                        SettlementStatusID = (int)EnumList.SettlementStatus.Settled,
                        PaymentDate = CommUtil.GetCurrentDateTime(),
                        TransactionRefNo = pObjAdjustmentData.TransactionRefNo,
                        Notes = pObjAdjustmentData.Notes,
                        RevenueStreamID = pObjAdjustmentData.RevenueStreamID,
                        RevenueSubStreamID = pObjAdjustmentData.RevenueSubStreamID,
                        AgencyID = pObjAdjustmentData.AgencyID,
                        Active = true,
                        CreatedDate = CommUtil.GetCurrentDateTime(),
                        CreatedBy = SessionManager.UserID
                    };

                    FuncResponse<Payment_Account> mObjPoAFuncResponse = new BLPaymentAccount().BL_InsertUpdatePaymentAccount(mObjPoA);

                    if (mObjPoAFuncResponse.Success)
                    {
                        BLSettlement mObjBLSettlement = new BLSettlement();

                        Settlement mObjSettlement = new Settlement()
                        {
                            AssessmentID = pObjAdjustmentData.AssessmentID,
                            SettlementDate = CommUtil.GetCurrentDateTime(),
                            SettlementAmount = 0 - pObjAdjustmentData.PoAAmount,
                            SettlementMethodID = pObjAdjustmentData.PaymentMethodID,
                            TransactionRefNo = "POA-" + mObjPoAFuncResponse.AdditionalData.PaymentAccountID,
                            SettlementNotes = "Deduction due to adjustment",
                            Active = true,
                            CreatedDate = CommUtil.GetCurrentDateTime(),
                            CreatedBy = SessionManager.UserID
                        };

                        FuncResponse<Settlement> mObjSettlementResponse = mObjBLSettlement.BL_InsertUpdateSettlement(mObjSettlement);

                        if (mObjSettlementResponse.Success)
                        {
                            MAP_Settlement_SettlementItem mObjSettlementItem = new MAP_Settlement_SettlementItem()
                            {
                                AAIID = pObjAdjustmentData.AAIID,
                                SettlementID = mObjSettlementResponse.AdditionalData.SettlementID,
                                SettlementAmount = 0 - pObjAdjustmentData.PoAAmount,
                                TaxAmount = 0 - pObjAdjustmentData.PoAAmount,
                                CreatedDate = CommUtil.GetCurrentDateTime(),
                                CreatedBy = SessionManager.UserID
                            };

                            FuncResponse mObjSIResponse = mObjBLSettlement.BL_InsertSettlementItem(mObjSettlementItem);

                            if (mObjSIResponse.Success)
                            {
                                dcResponse["success"] = true;
                                dcResponse["Message"] = mObjAdjResponse.Message;

                            }
                            else
                            {
                                dcResponse["success"] = false;
                                dcResponse["Message"] = mObjSIResponse.Message;
                            }
                        }
                        else
                        {
                            dcResponse["success"] = false;
                            dcResponse["Message"] = mObjSettlementResponse.Message;
                        }

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
                    dcResponse["Message"] = mObjAdjResponse.Message;
                }

            }
            catch (Exception ex)
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Failed to add adjustment";
                Logger.SendErrorToText(ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ServiceBill(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                BLMDAService mObjBLMDAService = new BLMDAService();
                BLMDAServiceItem mObjBLMDAServiceItem = new BLMDAServiceItem();
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

                    IList<DropDownListResult<long>> lstMDAServiceItem = new List<DropDownListResult<long>>();
                    foreach (var item in lstServiceBillItems)
                    {
                        lstMDAServiceItem.Add(new DropDownListResult<long>() { Id = item.SBSIID.GetValueOrDefault(), Text = item.MDAServiceName + " - " + item.MDAServiceItemName });
                    }

                    ViewBag.MDAServiceItemList = new SelectList(lstMDAServiceItem, "Id", "Text");

                    IList<SelectListItem> lstAdjustmentType = new List<SelectListItem>();
                    lstAdjustmentType.Add(new SelectListItem() { Text = "Increase", Value = "1" });
                    lstAdjustmentType.Add(new SelectListItem() { Text = "Deduction", Value = "2" });

                    ViewBag.AdjustmentTypeList = lstAdjustmentType;
                    UI_FillDropDown();
                    return View(mObjServiceBillData);
                }
                else
                {
                    return RedirectToAction("ReviseBill", "OperationManager");
                }
            }
            else
            {
                return RedirectToAction("ReviseBill", "OperationManager");
            }
        }

        public JsonResult AddSBAdjustment(MAP_ServiceBill_Adjustment pObjAdjustment, int ServiceBillID)
        {
            FuncResponse mObjResponse = new FuncResponse();
            int statId = 1; decimal? newAmount = 0;
            BLServiceBill mObjBLServiceBill = new BLServiceBill();
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();
            MAP_ServiceBill_MDAServiceItem mObjSBMSI = new MAP_ServiceBill_MDAServiceItem()
            { };
            pObjAdjustment.AdjustmentDate = CommUtil.GetCurrentDateTime();
            pObjAdjustment.CreatedDate = CommUtil.GetCurrentDateTime();
            pObjAdjustment.CreatedBy = SessionManager.UserID;
            decimal? Amountholder = 0;
            decimal? AmountAssItemholder = 0;
            try
            {

                using (_db = new EIRSEntities())
                {
                    var retVal = _db.ServiceBills.FirstOrDefault(o => o.ServiceBillID == ServiceBillID);
                    Amountholder = retVal.ServiceBillAmount;
                    AmountAssItemholder = _db.MAP_ServiceBill_MDAServiceItem.FirstOrDefault(o => o.SBSIID == pObjAdjustment.SBSIID).ServiceAmount;
                }

                MAP_ServiceBill_MDAService mObj = new MAP_ServiceBill_MDAService()
                {

                };

                ServiceBill mObjServiceBill = new ServiceBill()
                {
                };
                usp_GetServiceBillList_Result mObjServiceBillData = mObjBLServiceBill.BL_GetServiceBillDetails(new ServiceBill() { ServiceBillID = ServiceBillID, IntStatus = 2 });

                MAP_ServiceBill_MDAServiceItem ret = mObjBLServiceBill.GetServiceBillItems(pObjAdjustment.SBSIID.GetValueOrDefault());
                //Check if Assessment Amount is Greater than Paid Amount then mark it parital
                usp_GetServiceBillItemDetails_Result mObjServiceBillItemDetails = mObjBLServiceBill.BL_GetServiceBillItemDetails(pObjAdjustment.SBSIID.GetValueOrDefault());

                IList<usp_GetServiceBillItemList_Result> lstServiceBillItems = mObjBLServiceBill.BL_GetServiceBillItem(ServiceBillID);
                IList<usp_GetServiceBillAdjustmentList_Result> lstAssessmentAdjustment = mObjBLServiceBill.BL_GetServiceBillAdjustment(ServiceBillID);
                ServiceBillRepository assessmentRepository = new ServiceBillRepository();
                var oldData = assessmentRepository.REP_GetMDAServices(ServiceBillID);
                if (mObjServiceBillData.SettlementStatusID == 4 && pObjAdjustment.AdjustmentTypeID != 1)
                {
                    dcResponse["success"] = false;
                    dcResponse["Message"] = "You Cant Revise DownWard A Settled Bill";

                    return Json(dcResponse, JsonRequestBehavior.AllowGet);
                }
                if (ret.PaymentStatusID == (int)EnumList.SettlementStatus.Partial)
                {
                    if (pObjAdjustment.AdjustmentTypeID == 1)
                    {
                        newAmount = (lstServiceBillItems.Sum(t => t.ServiceAmount) + pObjAdjustment.Amount);
                        //ret.ServiceAmount = ret.ServiceAmount + pObjAdjustment.Amount;

                        statId = (int)EnumList.SettlementStatus.Partial;


                        mObjSBMSI.SBSIID = pObjAdjustment.SBSIID.GetValueOrDefault();
                        mObjSBMSI.ModifiedBy = SessionManager.UserID;
                        mObjSBMSI.ModifiedDate = CommUtil.GetCurrentDateTime();
                        mObjSBMSI.ServiceAmount = AmountAssItemholder;
                        mObjSBMSI.PaymentStatusID = statId;


                        mObjBLServiceBill.BL_UpdateMDAServiceItemStatus(mObjSBMSI);

                        //mObj.SBSID = ret.SBSID.GetValueOrDefault();
                        //mObj.ServiceAmount = newAmount;
                        //mObj.ModifiedDate = CommUtil.GetCurrentDateTime();
                        //mObj.ModifiedBy = SessionManager.UserID;
                        //mObjBLServiceBill.BL_InsertUpdateMDAService(mObj);

                        mObjServiceBill.ServiceBillID = ServiceBillID;
                        mObjServiceBill.SettlementStatusID = statId;
                        mObjServiceBill.ModifiedDate = CommUtil.GetCurrentDateTime();
                        mObjServiceBill.ModifiedBy = SessionManager.UserID;
                        mObjServiceBill.ServiceBillAmount = newAmount;

                        mObjBLServiceBill.BL_UpdateServiceBillSettlementStatus(mObjServiceBill);

                        mObjResponse = new BLServiceBill().BL_InsertAdjustment(pObjAdjustment);

                        dcResponse["success"] = mObjResponse.Success;
                        dcResponse["Message"] = mObjResponse.Message;

                        return Json(dcResponse, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {

                        newAmount = (lstServiceBillItems.Sum(t => t.ServiceAmount) + pObjAdjustment.Amount);
                        // ret.ServiceAmount = ret.ServiceAmount + pObjAdjustment.Amount;

                        if (mObjServiceBillData.SettlementAmount >= newAmount)
                            statId = (int)EnumList.SettlementStatus.Settled;
                        else if (mObjServiceBillData.SettlementAmount < newAmount)
                            statId = (int)EnumList.SettlementStatus.Partial;


                        mObjSBMSI.SBSIID = pObjAdjustment.SBSIID.GetValueOrDefault();
                        mObjSBMSI.ModifiedBy = SessionManager.UserID;
                        mObjSBMSI.ModifiedDate = CommUtil.GetCurrentDateTime();
                        mObjSBMSI.ServiceAmount = ret.ServiceAmount;
                        mObjSBMSI.PaymentStatusID = statId;


                        mObjBLServiceBill.BL_UpdateMDAServiceItemStatus(mObjSBMSI);

                        //mObj.SBSID = ret.SBSID.GetValueOrDefault();
                        //mObj.ServiceAmount = newAmount;
                        //mObj.ModifiedDate = CommUtil.GetCurrentDateTime();
                        //mObj.ModifiedBy = SessionManager.UserID;
                        //mObjBLServiceBill.BL_InsertUpdateMDAService(mObj);

                        mObjServiceBill.ServiceBillID = ServiceBillID;
                        mObjServiceBill.SettlementStatusID = statId;
                        mObjServiceBill.ModifiedDate = CommUtil.GetCurrentDateTime();
                        mObjServiceBill.ModifiedBy = SessionManager.UserID;
                        mObjServiceBill.ServiceBillAmount = newAmount;

                        mObjBLServiceBill.BL_UpdateServiceBillSettlementStatus(mObjServiceBill);

                        mObjResponse = new BLServiceBill().BL_InsertAdjustment(pObjAdjustment);
                        dcResponse["success"] = mObjResponse.Success;
                        dcResponse["Message"] = mObjResponse.Message;
                        return Json(dcResponse, JsonRequestBehavior.AllowGet);
                    }

                    //if (mObjServiceBillData.SettlementAmount >= newAmount)
                    //    statId = (int)EnumList.SettlementStatus.Settled;
                    //else if (mObjServiceBillData.SettlementAmount < newAmount)
                    statId = (int)EnumList.SettlementStatus.Partial;


                    mObjSBMSI.SBSIID = pObjAdjustment.SBSIID.GetValueOrDefault();
                    mObjSBMSI.ModifiedBy = SessionManager.UserID;
                    mObjSBMSI.ModifiedDate = CommUtil.GetCurrentDateTime();
                    mObjSBMSI.ServiceAmount = ret.ServiceAmount;
                    mObjSBMSI.PaymentStatusID = statId;


                    mObjBLServiceBill.BL_UpdateMDAServiceItemStatus(mObjSBMSI);

                    //mObj.SBSID = ret.SBSID.GetValueOrDefault();
                    //mObj.ServiceAmount = newAmount;
                    //mObj.ModifiedDate = CommUtil.GetCurrentDateTime();
                    //mObj.ModifiedBy = SessionManager.UserID;
                    //mObjBLServiceBill.BL_InsertUpdateMDAService(mObj);

                    mObjServiceBill.ServiceBillID = ServiceBillID;
                    mObjServiceBill.SettlementStatusID = statId;
                    mObjServiceBill.ModifiedDate = CommUtil.GetCurrentDateTime();
                    mObjServiceBill.ModifiedBy = SessionManager.UserID;
                    mObjServiceBill.ServiceBillAmount = newAmount;

                    mObjBLServiceBill.BL_UpdateServiceBillSettlementStatus(mObjServiceBill);

                    mObjResponse = new BLServiceBill().BL_InsertAdjustment(pObjAdjustment);

                    dcResponse["success"] = true;
                    dcResponse["Message"] = "";

                    return Json(dcResponse, JsonRequestBehavior.AllowGet);
                }



                mObjResponse = new BLServiceBill().BL_InsertAdjustment(pObjAdjustment);


                if (pObjAdjustment.AdjustmentTypeID == 1)
                {
                    newAmount = (lstServiceBillItems.Sum(t => t.ServiceAmount) + pObjAdjustment.Amount);
                    ret.ServiceAmount = ret.ServiceAmount + pObjAdjustment.Amount;
                }
                else
                {
                    newAmount = (lstServiceBillItems.Sum(t => t.ServiceAmount) + pObjAdjustment.Amount);
                    ret.ServiceAmount = ret.ServiceAmount + pObjAdjustment.Amount;

                }

                if (mObjServiceBillData.SettlementAmount >= newAmount)
                    statId = (int)EnumList.SettlementStatus.Settled;
                else if (mObjServiceBillData.SettlementAmount < newAmount)
                    statId = (int)EnumList.SettlementStatus.Partial;

                mObjSBMSI.SBSIID = pObjAdjustment.SBSIID.GetValueOrDefault();
                mObjSBMSI.ModifiedBy = SessionManager.UserID;
                mObjSBMSI.ModifiedDate = CommUtil.GetCurrentDateTime();
                mObjSBMSI.ServiceAmount = ret.ServiceAmount;
                mObjSBMSI.PaymentStatusID = statId;

                mObjBLServiceBill.BL_UpdateMDAServiceItemStatus(mObjSBMSI);


                //mObj.SBSID = ret.SBSID.GetValueOrDefault();
                //mObj.ServiceAmount = newAmount;
                //mObj.ModifiedDate = CommUtil.GetCurrentDateTime();
                //mObj.ModifiedBy = SessionManager.UserID;
                //mObjBLServiceBill.BL_InsertUpdateMDAService(mObj);

                mObjServiceBill.ServiceBillID = ServiceBillID;
                mObjServiceBill.SettlementStatusID = statId;
                mObjServiceBill.ModifiedDate = CommUtil.GetCurrentDateTime();
                mObjServiceBill.ModifiedBy = SessionManager.UserID;
                mObjServiceBill.ServiceBillAmount = newAmount;

                mObjBLServiceBill.BL_UpdateServiceBillSettlementStatus(mObjServiceBill);



                dcResponse["success"] = mObjResponse.Success;
                dcResponse["Message"] = mObjResponse.Message;

            }
            catch (Exception ex)
            {
                Logger.SendErrorToText(ex);
            }
            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddSBAdjustmentWithPoA(AdjustmentWithPoAModel pObjAdjustmentData)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            try
            {
                MAP_ServiceBill_Adjustment mObjAdjustment = new MAP_ServiceBill_Adjustment()
                {
                    SBSIID = pObjAdjustmentData.SBSIID,
                    AdjustmentLine = pObjAdjustmentData.AdjustmentLine,
                    AdjustmentTypeID = pObjAdjustmentData.AdjustmentTypeID,
                    Amount = pObjAdjustmentData.AdjustmentAmount,
                    AdjustmentDate = CommUtil.GetCurrentDateTime(),
                    CreatedDate = CommUtil.GetCurrentDateTime(),
                    CreatedBy = SessionManager.UserID
                };

                FuncResponse mObjAdjResponse = new BLServiceBill().BL_InsertAdjustment(mObjAdjustment);

                if (mObjAdjResponse.Success)
                {
                    usp_GetServiceBillList_Result mObjServiceBillData = new BLServiceBill().BL_GetServiceBillDetails(new ServiceBill() { ServiceBillID = pObjAdjustmentData.ServiceBillID, IntStatus = 2 });

                    Payment_Account mObjPoA = new Payment_Account()
                    {
                        PaymentAccountID = 0,
                        TaxPayerTypeID = mObjServiceBillData.TaxPayerTypeID,
                        TaxPayerID = mObjServiceBillData.TaxPayerID,
                        Amount = pObjAdjustmentData.PoAAmount,
                        SettlementMethodID = pObjAdjustmentData.PaymentMethodID,
                        SettlementStatusID = (int)EnumList.SettlementStatus.Settled,
                        PaymentDate = CommUtil.GetCurrentDateTime(),
                        TransactionRefNo = pObjAdjustmentData.TransactionRefNo,
                        Notes = pObjAdjustmentData.Notes,
                        RevenueStreamID = pObjAdjustmentData.RevenueStreamID,
                        RevenueSubStreamID = pObjAdjustmentData.RevenueSubStreamID,
                        AgencyID = pObjAdjustmentData.AgencyID,
                        Active = true,
                        CreatedDate = CommUtil.GetCurrentDateTime(),
                        CreatedBy = SessionManager.UserID
                    };

                    FuncResponse<Payment_Account> mObjPoAFuncResponse = new BLPaymentAccount().BL_InsertUpdatePaymentAccount(mObjPoA);

                    if (mObjPoAFuncResponse.Success)
                    {
                        BLSettlement mObjBLSettlement = new BLSettlement();

                        Settlement mObjSettlement = new Settlement()
                        {
                            ServiceBillID = pObjAdjustmentData.ServiceBillID,
                            SettlementDate = CommUtil.GetCurrentDateTime(),
                            SettlementAmount = 0 - pObjAdjustmentData.PoAAmount,
                            SettlementMethodID = pObjAdjustmentData.PaymentMethodID,
                            TransactionRefNo = "POA-" + mObjPoAFuncResponse.AdditionalData.PaymentAccountID,
                            SettlementNotes = "Deduction due to adjustment",
                            Active = true,
                            CreatedDate = CommUtil.GetCurrentDateTime(),
                            CreatedBy = SessionManager.UserID
                        };

                        FuncResponse<Settlement> mObjSettlementResponse = mObjBLSettlement.BL_InsertUpdateSettlement(mObjSettlement);

                        if (mObjSettlementResponse.Success)
                        {
                            MAP_Settlement_SettlementItem mObjSettlementItem = new MAP_Settlement_SettlementItem()
                            {
                                SBSIID = pObjAdjustmentData.SBSIID,
                                SettlementID = mObjSettlementResponse.AdditionalData.SettlementID,
                                SettlementAmount = 0 - pObjAdjustmentData.PoAAmount,
                                TaxAmount = 0 - pObjAdjustmentData.PoAAmount,
                                CreatedDate = CommUtil.GetCurrentDateTime(),
                                CreatedBy = SessionManager.UserID
                            };

                            FuncResponse mObjSIResponse = mObjBLSettlement.BL_InsertSettlementItem(mObjSettlementItem);

                            if (mObjSIResponse.Success)
                            {
                                dcResponse["success"] = true;
                                dcResponse["Message"] = mObjAdjResponse.Message;

                            }
                            else
                            {
                                dcResponse["success"] = false;
                                dcResponse["Message"] = mObjSIResponse.Message;
                            }
                        }
                        else
                        {
                            dcResponse["success"] = false;
                            dcResponse["Message"] = mObjSettlementResponse.Message;
                        }

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
                    dcResponse["Message"] = mObjAdjResponse.Message;
                }

            }
            catch (Exception Ex)
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Failed to add adjustment";
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAssessmentItemDetails(long AAIID)
        {
            usp_GetAssessmentRuleItemDetails_Result mObjAssessmentRuleItemDetails = new BLAssessment().BL_GetAssessmentRuleItemDetails(AAIID);
            mObjAssessmentRuleItemDetails.TotalAmount = mObjAssessmentRuleItemDetails.AdjustmentAmount + mObjAssessmentRuleItemDetails.TaxAmount;
            return Json(mObjAssessmentRuleItemDetails, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetServiceBillItemDetails(long SBSIID)
        {
            usp_GetServiceBillItemDetails_Result mObjServiceBillItemDetails = new BLServiceBill().BL_GetServiceBillItemDetails(SBSIID);
            mObjServiceBillItemDetails.TotalAmount = mObjServiceBillItemDetails.AdjustmentAmount + mObjServiceBillItemDetails.ServiceAmount;
            return Json(mObjServiceBillItemDetails, JsonRequestBehavior.AllowGet);
        }
    }
}