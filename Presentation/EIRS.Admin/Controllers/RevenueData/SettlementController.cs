using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Vereyon.Web;
using System.Linq;
using System.Transactions;
using Elmah;
using System.Text;

namespace EIRS.Admin.Controllers
{
    public class SettlementController : BaseController
    {
        public ActionResult List()
        {
            return View();
        }

        [HttpPost]
        public JsonResult LoadData()
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            var vFilter = Request.Form.GetValues("search[value]").FirstOrDefault();

            StringBuilder sbWhereCondition = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(Request.Form["SettlementRefNo"]))
            {
                sbWhereCondition.Append(" AND ISNULL(SettlementRefNo,'') LIKE @SettlementRefNo");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["SettlementDate"]))
            {
                sbWhereCondition.Append(" AND ISNULL(REPLACE(CONVERT(varchar(50),stmt.SettlementDate,106),' ','-'),'') LIKE @SettlementDate");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["BillRefNo"]))
            {
                sbWhereCondition.Append(" AND (ISNULL(ast.AssessmentRefNo,'') LIKE @BillRefNo OR ISNULL(sb.ServiceBillRefNo,'') LIKE @BillRefNo) ");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["BillAmount"]))
            {
                sbWhereCondition.Append(" AND (ISNULL(CAST(ast.AssessmentAmount as varchar(50)),'') LIKE @BillAmount OR ISNULL(CAST(sb.ServiceBillAmount as varchar(50)),'') LIKE @BillAmount)");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["SettlementAmount"]))
            {
                sbWhereCondition.Append(" AND ISNULL(Convert(varchar(50),SettlementAmount,106),'') LIKE @SettlementAmount");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["SettlementMethodName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(SettlementMethodName,'') LIKE @SettlementMethodName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["SettlementStatus"]))
            {
                sbWhereCondition.Append(" AND (ISNULL(ast_stat.SettlementStatusName,'') LIKE @SettlementStatus OR ISNULL(sb_stat.SettlementStatusName,'') LIKE @SettlementStatus)");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["SettlementNotes"]))
            {
                sbWhereCondition.Append(" AND ISNULL(SettlementNotes,'') LIKE @SettlementNotes");
            }

            if (!string.IsNullOrEmpty(vFilter))
            {
                sbWhereCondition.Append(" AND ( ISNULL(SettlementRefNo,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(Convert(varchar(50),stmt.SettlementDate,106),'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR (ISNULL(ast.AssessmentRefNo,'') LIKE @MainFilter OR ISNULL(sb.ServiceBillRefNo,'') LIKE @MainFilter) ");
                sbWhereCondition.Append(" OR (ISNULL(CAST(ast.AssessmentAmount as varchar(50)),'') LIKE @MainFilter OR ISNULL(CAST(sb.ServiceBillAmount as varchar(50)),'') LIKE @MainFilter)");
                sbWhereCondition.Append(" OR ISNULL(Convert(varchar(50),SettlementAmount,106),'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(SettlementMethodName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR (ISNULL(ast_stat.SettlementStatusName,'') LIKE @MainFilter OR ISNULL(sb_stat.SettlementStatusName,'') LIKE @MainFilter)");
                sbWhereCondition.Append(" OR ISNULL(SettlementNotes,'') LIKE @MainFilter ");

            }

            Settlement mObjSettlement = new Settlement()
            {
                WhereCondition = sbWhereCondition.ToString(),
                OrderBy = vSortColumn,
                OrderByDirection = vSortColumnDir,
                PageNumber = (TrynParse.parseInt(vStart) / TrynParse.parseInt(vLength)) + 1,
                PageSize = TrynParse.parseInt(vLength),

                SettlementRefNo= !string.IsNullOrWhiteSpace(Request.Form["SettlementRefNo"]) ? "%" + Request.Form["SettlementRefNo"].Trim() + "%" : TrynParse.parseString(Request.Form["SettlementRefNo"]),
                strSettlementDate = !string.IsNullOrWhiteSpace(Request.Form["SettlementDate"]) ? "%" + Request.Form["SettlementDate"].Trim() + "%" : TrynParse.parseString(Request.Form["SettlementDate"]),
                BillRefNo = !string.IsNullOrWhiteSpace(Request.Form["BillRefNo"]) ? "%" + Request.Form["BillRefNo"].Trim() + "%" : TrynParse.parseString(Request.Form["BillRefNo"]),
                BillAmount = !string.IsNullOrWhiteSpace(Request.Form["BillAmount"]) ? "%" + Request.Form["BillAmount"].Trim() + "%" : TrynParse.parseString(Request.Form["BillAmount"]),
                strSettlementAmount = !string.IsNullOrWhiteSpace(Request.Form["SettlementAmount"]) ? "%" + Request.Form["SettlementAmount"].Trim() + "%" : TrynParse.parseString(Request.Form["SettlementAmount"]),
                SettlementMethodName = !string.IsNullOrWhiteSpace(Request.Form["SettlementMethodName"]) ? "%" + Request.Form["SettlementMethodName"].Trim() + "%" : TrynParse.parseString(Request.Form["SettlementMethodName"]),
                SettlementStatusName = !string.IsNullOrWhiteSpace(Request.Form["SettlementStatus"]) ? "%" + Request.Form["SettlementStatus"].Trim() + "%" : TrynParse.parseString(Request.Form["SettlementStatus"]),
                SettlementNotes = !string.IsNullOrWhiteSpace(Request.Form["SettlementNotes"]) ? "%" + Request.Form["SettlementNotes"].Trim() + "%" : TrynParse.parseString(Request.Form["SettlementNotes"]),
                MainFilter = !string.IsNullOrWhiteSpace(vFilter) ? "%" + vFilter.Trim() + "%" : vFilter

            };

            IDictionary<string, object> dcData = new BLSettlement().BL_SearchSettlement(mObjSettlement);
            IList<usp_SearchSettlement_Result> lstSettlement = (IList<usp_SearchSettlement_Result>)dcData["SettlementList"];

            return Json(new
            {
                draw = vDraw,
                recordsFiltered = dcData["FilteredRecords"],
                recordsTotal = dcData["TotalRecords"],
                data = lstSettlement
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Generate()
        {
            IList<DropDownListResult> lstSettlementType = new List<DropDownListResult>();
            lstSettlementType.Add(new DropDownListResult() { id = 1, text = "Assessment" });
            lstSettlementType.Add(new DropDownListResult() { id = 2, text = "Service Bill" });

            ViewBag.SettlementTypeList = new SelectList(lstSettlementType, "id", "text");

            return View();
        }

        public ActionResult Add(int? id, string name, int? stype)
        {
            if (id.GetValueOrDefault() > 0)
            {
                if (stype == 1)
                {
                    BLAssessment mObjBLAssessment = new BLAssessment();

                    usp_GetAssessmentList_Result mObjAssessmentData = mObjBLAssessment.BL_GetAssessmentDetails(new Assessment() { AssessmentID = id.GetValueOrDefault(), IntStatus = 1 });

                    if (mObjAssessmentData != null)
                    {
                        IList<usp_GetAssessmentRuleItemList_Result> lstAssessmentRuleItem = mObjBLAssessment.BL_GetAssessmentRuleItem(id.GetValueOrDefault());

                        IList<DropDownListResult> lstSettlementMethod = mObjBLAssessment.BL_GetSettlementMethodAssessmentRuleBased(id.GetValueOrDefault());
                        ViewBag.SettlementMethodList = new SelectList(lstSettlementMethod, "id", "text");


                        SettlementViewModel mObjSettlementModel = new SettlementViewModel()
                        {
                            AssessmentID = id.GetValueOrDefault(),
                            SettlementDate = CommUtil.GetCurrentDateTime(),
                        };

                        ViewBag.AssessmentData = mObjAssessmentData;

                        IList<Settlement_ASBItem> lstSettlementItems = new List<Settlement_ASBItem>();

                        foreach (var item in lstAssessmentRuleItem)
                        {
                            Settlement_ASBItem mObjSettlementItem = new Settlement_ASBItem()
                            {
                                RowID = lstSettlementItems.Count + 1,
                                TBPKID = item.AAIID.GetValueOrDefault(),
                                ASBName = item.AssessmentRuleName,
                                ItemName = item.AssessmentItemName,
                                PaymentStatusID = item.PaymentStatusID.GetValueOrDefault(),
                                PaymentStatusName = item.PaymentStatusName,
                                TaxAmount = item.TaxAmount.GetValueOrDefault(),
                                SettlementAmount = item.SettlementAmount.GetValueOrDefault(),
                                UnSettledAmount = item.PendingAmount.GetValueOrDefault(),
                                ToSettleAmount = item.PendingAmount.GetValueOrDefault(),
                            };

                            lstSettlementItems.Add(mObjSettlementItem);
                        }

                        SessionManager.lstSettlementItem = lstSettlementItems;
                        ViewBag.SettlementItemList = lstSettlementItems;

                        return View(mObjSettlementModel);
                    }
                    else
                    {
                        return RedirectToAction("List", "Settlement");
                    }
                }
                else if(stype == 2)
                {
                    BLServiceBill mObjBLServiceBill = new BLServiceBill();

                    usp_GetServiceBillList_Result mObjServiceBillData = mObjBLServiceBill.BL_GetServiceBillDetails(new ServiceBill() { ServiceBillID = id.GetValueOrDefault(), IntStatus = 1 });

                    if (mObjServiceBillData != null)
                    {
                        IList<usp_GetServiceBillItemList_Result> lstServiceBillItem = mObjBLServiceBill.BL_GetServiceBillItem(id.GetValueOrDefault());

                        IList<DropDownListResult> lstSettlementMethod = mObjBLServiceBill.BL_GetSettlementMethodMDAServiceBased(id.GetValueOrDefault());
                        ViewBag.SettlementMethodList = new SelectList(lstSettlementMethod, "id", "text");


                        SettlementViewModel mObjSettlementModel = new SettlementViewModel()
                        {
                            ServiceBillID = id.GetValueOrDefault(),
                            SettlementDate = CommUtil.GetCurrentDateTime(),
                        };

                        ViewBag.ServiceBillData = mObjServiceBillData;

                        IList<Settlement_ASBItem> lstSettlementItems = new List<Settlement_ASBItem>();

                        foreach (var item in lstServiceBillItem)
                        {
                            Settlement_ASBItem mObjSettlementItem = new Settlement_ASBItem()
                            {
                                RowID = lstSettlementItems.Count + 1,
                                TBPKID = item.SBSIID.GetValueOrDefault(),
                                ASBName = item.MDAServiceName,
                                ItemName = item.MDAServiceItemName,
                                PaymentStatusID = item.PaymentStatusID.GetValueOrDefault(),
                                PaymentStatusName = item.PaymentStatusName,
                                TaxAmount = item.ServiceAmount.GetValueOrDefault(),
                                AdjustmentAmount = item.AdjustmentAmount.GetValueOrDefault(),
                                LateChargeAmount = item.LateChargeAmount.GetValueOrDefault(),
                                TotalAmount = item.TotalAmount.GetValueOrDefault(),
                                SettlementAmount = item.SettlementAmount.GetValueOrDefault(),
                                UnSettledAmount = item.PendingAmount.GetValueOrDefault(),
                                ToSettleAmount = item.PendingAmount.GetValueOrDefault(),
                            };

                            lstSettlementItems.Add(mObjSettlementItem);
                        }

                        SessionManager.lstSettlementItem = lstSettlementItems;
                        ViewBag.SettlementItemList = lstSettlementItems;

                        return View(mObjSettlementModel);
                    }
                    else
                    {
                        return RedirectToAction("List", "Settlement");
                    }
                }
                else
                {
                    return RedirectToAction("List", "Settlement");
                }
            }
            else
            {
                return RedirectToAction("List", "Settlement");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(SettlementViewModel pObjSettlementModel)
        {
            if (!ModelState.IsValid)
            {
                if (pObjSettlementModel.AssessmentID != 0)
                {
                    BLAssessment mObjBLAssessment = new BLAssessment();
                    usp_GetAssessmentList_Result mObjAssessmentData = mObjBLAssessment.BL_GetAssessmentDetails(new Assessment() { AssessmentID = pObjSettlementModel.AssessmentID, IntStatus = 1 });

                    IList<DropDownListResult> lstSettlementMethod = mObjBLAssessment.BL_GetSettlementMethodAssessmentRuleBased(pObjSettlementModel.AssessmentID);
                    ViewBag.SettlementMethodList = new SelectList(lstSettlementMethod, "id", "text");

                    ViewBag.AssessmentData = mObjAssessmentData;
                }
                else if(pObjSettlementModel.ServiceBillID != 0)
                {
                    BLServiceBill mObjBLServiceBill = new BLServiceBill();
                    usp_GetServiceBillList_Result mObjServiceBillData = mObjBLServiceBill.BL_GetServiceBillDetails(new ServiceBill() { ServiceBillID = pObjSettlementModel.ServiceBillID, IntStatus = 1 });

                    IList<DropDownListResult> lstSettlementMethod = mObjBLServiceBill.BL_GetSettlementMethodMDAServiceBased(pObjSettlementModel.ServiceBillID);
                    ViewBag.SettlementMethodList = new SelectList(lstSettlementMethod, "id", "text");

                    ViewBag.ServiceBillData = mObjServiceBillData;
                }
                else
                {
                    return RedirectToAction("List", "Settlement");
                }

                ViewBag.SettlementItemList = SessionManager.lstSettlementItem;

                return View(pObjSettlementModel);
            }
            else
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    IList<Settlement_ASBItem> lstSettlementItems = SessionManager.lstSettlementItem ?? new List<Settlement_ASBItem>();

                    if (lstSettlementItems.Sum(t => t.ToSettleAmount) == 0)
                    {
                        if (pObjSettlementModel.AssessmentID != 0)
                        {
                            BLAssessment mObjBLAssessment = new BLAssessment();
                            usp_GetAssessmentList_Result mObjAssessmentData = mObjBLAssessment.BL_GetAssessmentDetails(new Assessment() { AssessmentID = pObjSettlementModel.AssessmentID, IntStatus = 1 });

                            IList<DropDownListResult> lstSettlementMethod = mObjBLAssessment.BL_GetSettlementMethodAssessmentRuleBased(pObjSettlementModel.AssessmentID);
                            ViewBag.SettlementMethodList = new SelectList(lstSettlementMethod, "id", "text");

                            ViewBag.AssessmentData = mObjAssessmentData;
                        }
                        else if (pObjSettlementModel.ServiceBillID != 0)
                        {
                            BLServiceBill mObjBLServiceBill = new BLServiceBill();
                            usp_GetServiceBillList_Result mObjServiceBillData = mObjBLServiceBill.BL_GetServiceBillDetails(new ServiceBill() { ServiceBillID = pObjSettlementModel.ServiceBillID, IntStatus = 1 });

                            IList<DropDownListResult> lstSettlementMethod = mObjBLServiceBill.BL_GetSettlementMethodMDAServiceBased(pObjSettlementModel.ServiceBillID);
                            ViewBag.SettlementMethodList = new SelectList(lstSettlementMethod, "id", "text");

                            ViewBag.ServiceBillData = mObjServiceBillData;
                        }

                        ViewBag.SettlementItemList = SessionManager.lstSettlementItem;

                        ModelState.AddModelError("SettlementAmount-error", "Settlement Amount Cannot be zero");
                        return View(pObjSettlementModel);
                    }
                    else
                    {
                        BLSettlement mObjBLSettlement = new BLSettlement();

                        Settlement mObjSettlement = new Settlement()
                        {
                            SettlementDate = pObjSettlementModel.SettlementDate,
                            SettlementAmount = lstSettlementItems.Sum(t => t.ToSettleAmount),
                            SettlementMethodID = pObjSettlementModel.SettlementMethod,
                            TransactionRefNo = pObjSettlementModel.TransactionRefNo,
                            SettlementNotes = pObjSettlementModel.Notes,
                            Active = true,
                            CreatedBy = SessionManager.SystemUserID,
                            CreatedDate = CommUtil.GetCurrentDateTime()
                        };

                        if (pObjSettlementModel.AssessmentID != 0)
                            mObjSettlement.AssessmentID = pObjSettlementModel.AssessmentID;
                        if (pObjSettlementModel.ServiceBillID != 0)
                            mObjSettlement.ServiceBillID = pObjSettlementModel.ServiceBillID;

                        try
                        {

                            FuncResponse<Settlement> mObjSettlementResponse = mObjBLSettlement.BL_InsertUpdateSettlement(mObjSettlement);

                            if (mObjSettlementResponse.Success)
                            {
                                BLAssessment mObjBLAssessment = new BLAssessment();
                                BLServiceBill mObjBLServiceBill = new BLServiceBill();
                                foreach (Settlement_ASBItem mObjSAI in lstSettlementItems)
                                {
                                    if (mObjSAI.PaymentStatusID != (int)EnumList.PaymentStatus.Paid && (mObjSAI.ToSettleAmount > 0 || mObjSAI.TaxAmount == 0))
                                    {
                                        MAP_Settlement_SettlementItem mObjSettlementItem = new MAP_Settlement_SettlementItem()
                                        {
                                            SettlementID = mObjSettlementResponse.AdditionalData.SettlementID,
                                            SettlementAmount = mObjSAI.ToSettleAmount,
                                            TaxAmount = mObjSAI.TotalAmount,
                                            CreatedBy = SessionManager.SystemUserID,
                                            CreatedDate = CommUtil.GetCurrentDateTime()
                                        };

                                        if(pObjSettlementModel.ServiceBillID != 0)
                                        {
                                            mObjSettlementItem.SBSIID = mObjSAI.TBPKID;
                                        }

                                        if(pObjSettlementModel.AssessmentID != 0)
                                        {
                                            mObjSettlementItem.AAIID = mObjSAI.TBPKID;
                                        }

                                        FuncResponse mObjSIResponse = mObjBLSettlement.BL_InsertSettlementItem(mObjSettlementItem);

                                        if (mObjSIResponse.Success)
                                        {
                                            if (pObjSettlementModel.AssessmentID != 0)
                                            {
                                                MAP_Assessment_AssessmentItem mObjAAI = new MAP_Assessment_AssessmentItem()
                                                {
                                                    AAIID = mObjSAI.TBPKID,
                                                    ModifiedBy = SessionManager.SystemUserID,
                                                    ModifiedDate = CommUtil.GetCurrentDateTime()
                                                };

                                                //Update Assessment item Status
                                                if (mObjSAI.TotalAmount == (mObjSAI.ToSettleAmount + mObjSAI.SettlementAmount))
                                                {
                                                    mObjAAI.PaymentStatusID = (int)EnumList.PaymentStatus.Paid;
                                                }
                                                else if ((mObjSAI.ToSettleAmount + mObjSAI.SettlementAmount) < mObjSAI.TotalAmount)
                                                {
                                                    mObjAAI.PaymentStatusID = (int)EnumList.PaymentStatus.Partial;
                                                }

                                                if (mObjAAI.PaymentStatusID != null)
                                                    mObjBLAssessment.BL_UpdateAssessmentItemStatus(mObjAAI);
                                            }
                                            else if(pObjSettlementModel.ServiceBillID != 0)
                                            {
                                                MAP_ServiceBill_MDAServiceItem mObjSBMSI = new MAP_ServiceBill_MDAServiceItem()
                                                {
                                                    SBSIID = mObjSAI.TBPKID,
                                                    ModifiedBy = SessionManager.SystemUserID,
                                                    ModifiedDate = CommUtil.GetCurrentDateTime()
                                                };

                                                //Update Assessment item Status
                                                if (mObjSAI.TotalAmount == (mObjSAI.ToSettleAmount + mObjSAI.SettlementAmount))
                                                {
                                                    mObjSBMSI.PaymentStatusID = (int)EnumList.PaymentStatus.Paid;
                                                }
                                                else if ((mObjSAI.ToSettleAmount + mObjSAI.SettlementAmount) < mObjSAI.TotalAmount)
                                                {
                                                    mObjSBMSI.PaymentStatusID = (int)EnumList.PaymentStatus.Partial;
                                                }

                                                if (mObjSBMSI.PaymentStatusID != null)
                                                    mObjBLServiceBill.BL_UpdateMDAServiceItemStatus(mObjSBMSI);
                                            }
                                        }
                                        else
                                        {
                                            throw (mObjSIResponse.Exception);
                                        }
                                    }
                                }

                                if (pObjSettlementModel.AssessmentID != 0)
                                {
                                    //Update Assessment Status
                                    Assessment mObjAssessment = new Assessment()
                                    {
                                        AssessmentID = pObjSettlementModel.AssessmentID,
                                        SettlementDate = pObjSettlementModel.SettlementDate,
                                        ModifiedDate = CommUtil.GetCurrentDateTime(),
                                        ModifiedBy = SessionManager.SystemUserID,
                                    };

                                    if (lstSettlementItems.Sum(t => t.TotalAmount) == lstSettlementItems.Sum(t => t.ToSettleAmount + t.SettlementAmount))
                                    {
                                        mObjAssessment.SettlementStatusID = (int)EnumList.SettlementStatus.Settled;
                                    }
                                    else if (lstSettlementItems.Sum(t => t.ToSettleAmount + t.SettlementAmount) < lstSettlementItems.Sum(t => t.TotalAmount))
                                    {
                                        mObjAssessment.SettlementStatusID = (int)EnumList.SettlementStatus.Partial;
                                    }

                                    if (mObjAssessment.SettlementStatusID != null)
                                        mObjBLAssessment.BL_UpdateAssessmentSettlementStatus(mObjAssessment);
                                }
                                else if(pObjSettlementModel.ServiceBillID != 0)
                                {
                                    //Update Service Bill Status
                                    ServiceBill mObjServiceBill = new ServiceBill()
                                    {
                                        ServiceBillID = pObjSettlementModel.ServiceBillID,
                                        SettlementDate = pObjSettlementModel.SettlementDate,
                                        ModifiedDate = CommUtil.GetCurrentDateTime(),
                                        ModifiedBy = SessionManager.SystemUserID,
                                    };

                                    if (lstSettlementItems.Sum(t => t.TotalAmount) == lstSettlementItems.Sum(t => t.ToSettleAmount + t.SettlementAmount))
                                    {
                                        mObjServiceBill.SettlementStatusID = (int)EnumList.SettlementStatus.Settled;
                                    }
                                    else if (lstSettlementItems.Sum(t => t.ToSettleAmount + t.SettlementAmount) < lstSettlementItems.Sum(t => t.TotalAmount))
                                    {
                                        mObjServiceBill.SettlementStatusID = (int)EnumList.SettlementStatus.Partial;
                                    }

                                    if (mObjServiceBill.SettlementStatusID != null)
                                        mObjBLServiceBill.BL_UpdateServiceBillSettlementStatus(mObjServiceBill);

                                    //If Service Bill Status is Settled. Check for Request and Mark Paid
                                    if(mObjServiceBill.SettlementStatusID == (int)EnumList.SettlementStatus.Settled)
                                    {
                                        //Search for Request with Service Bill
                                        BLTCC mObjBLTCC = new BLTCC();
                                        TCC_Request mObjTCCRequest = mObjBLTCC.BL_GetRequestBasedOnServiceBill(pObjSettlementModel.ServiceBillID);

                                        if(mObjTCCRequest  != null)
                                        {
                                            TCC_Request mObjUpdateStatus = new TCC_Request()
                                            {
                                                TCCRequestID = mObjTCCRequest.TCCRequestID,
                                                StatusID = (int)EnumList.TCCRequestStatus.Paid,
                                                ModifiedBy = SessionManager.SystemUserID,
                                                ModifiedDate = CommUtil.GetCurrentDateTime()
                                            };

                                            mObjBLTCC.BL_UpdateRequestStatus(mObjUpdateStatus);
                                        }
                                    }
                                }

                                scope.Complete();
                                FlashMessage.Info(mObjSettlementResponse.Message);
                                return RedirectToAction("List", "Settlement");

                            }
                            else
                            {
                                if (pObjSettlementModel.AssessmentID != 0)
                                {
                                    BLAssessment mObjBLAssessment = new BLAssessment();
                                    usp_GetAssessmentList_Result mObjAssessmentData = mObjBLAssessment.BL_GetAssessmentDetails(new Assessment() { AssessmentID = pObjSettlementModel.AssessmentID, IntStatus = 1 });

                                    IList<DropDownListResult> lstSettlementMethod = mObjBLAssessment.BL_GetSettlementMethodAssessmentRuleBased(pObjSettlementModel.AssessmentID);
                                    ViewBag.SettlementMethodList = new SelectList(lstSettlementMethod, "id", "text");

                                    ViewBag.AssessmentData = mObjAssessmentData;
                                }
                                else if (pObjSettlementModel.ServiceBillID != 0)
                                {
                                    BLServiceBill mObjBLServiceBill = new BLServiceBill();
                                    usp_GetServiceBillList_Result mObjServiceBillData = mObjBLServiceBill.BL_GetServiceBillDetails(new ServiceBill() { ServiceBillID = pObjSettlementModel.ServiceBillID, IntStatus = 1 });

                                    IList<DropDownListResult> lstSettlementMethod = mObjBLServiceBill.BL_GetSettlementMethodMDAServiceBased(pObjSettlementModel.ServiceBillID);
                                    ViewBag.SettlementMethodList = new SelectList(lstSettlementMethod, "id", "text");

                                    ViewBag.ServiceBillData = mObjServiceBillData;
                                }

                                ViewBag.SettlementItemList = SessionManager.lstSettlementItem;

                                ViewBag.Message = mObjSettlementResponse.Message;
                                Transaction.Current.Rollback();
                                return View(pObjSettlementModel);
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorSignal.FromCurrentContext().Raise(ex);
                            if (pObjSettlementModel.AssessmentID != 0)
                            {
                                BLAssessment mObjBLAssessment = new BLAssessment();
                                usp_GetAssessmentList_Result mObjAssessmentData = mObjBLAssessment.BL_GetAssessmentDetails(new Assessment() { AssessmentID = pObjSettlementModel.AssessmentID, IntStatus = 1 });

                                IList<DropDownListResult> lstSettlementMethod = mObjBLAssessment.BL_GetSettlementMethodAssessmentRuleBased(pObjSettlementModel.AssessmentID);
                                ViewBag.SettlementMethodList = new SelectList(lstSettlementMethod, "id", "text");

                                ViewBag.AssessmentData = mObjAssessmentData;
                            }
                            else if (pObjSettlementModel.ServiceBillID != 0)
                            {
                                BLServiceBill mObjBLServiceBill = new BLServiceBill();
                                usp_GetServiceBillList_Result mObjServiceBillData = mObjBLServiceBill.BL_GetServiceBillDetails(new ServiceBill() { ServiceBillID = pObjSettlementModel.ServiceBillID, IntStatus = 1 });

                                IList<DropDownListResult> lstSettlementMethod = mObjBLServiceBill.BL_GetSettlementMethodMDAServiceBased(pObjSettlementModel.ServiceBillID);
                                ViewBag.SettlementMethodList = new SelectList(lstSettlementMethod, "id", "text");

                                ViewBag.ServiceBillData = mObjServiceBillData;
                            }

                            ViewBag.SettlementItemList = SessionManager.lstSettlementItem;

                            ViewBag.Message = "Error occurred while saving settlement";
                            Transaction.Current.Rollback();
                            return View(pObjSettlementModel);
                        }
                    }
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                BLSettlement mObjBLSettlement = new BLSettlement();
                usp_GetSettlementList_Result mObjSettlementDetails = mObjBLSettlement.BL_GetSettlementDetails(new Settlement() { SettlementID = id.GetValueOrDefault() });

                if (mObjSettlementDetails != null)
                {
                    IList<usp_GetSettlementItemList_Result> lstSettlementItems = mObjBLSettlement.BL_GetSettlementItemList(mObjSettlementDetails.SettlementID.GetValueOrDefault());

                    ViewBag.SettlementItemList = lstSettlementItems;

                    return View(mObjSettlementDetails);
                }
                else
                {
                    return RedirectToAction("List", "Settlement");
                }
            }
            else
            {
                return RedirectToAction("List", "Settlement");
            }
        }

        public JsonResult UpdateSettlementItem(int SettlementItemRowID, decimal SettlementAmount)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            IList<Settlement_ASBItem> lstSettlementItems = SessionManager.lstSettlementItem ?? new List<Settlement_ASBItem>();

            Settlement_ASBItem mObjUpdateSettlementItem = lstSettlementItems.Where(t => t.RowID == SettlementItemRowID).FirstOrDefault();

            if (mObjUpdateSettlementItem != null)
            {
                mObjUpdateSettlementItem.ToSettleAmount = SettlementAmount;
            }

            dcResponse["SettlementItemList"] = CommUtil.RenderPartialToString("_BindSettlementItem", lstSettlementItems, this.ControllerContext);

            SessionManager.lstSettlementItem = lstSettlementItems;

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetAssessmentList()
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            var vFilter = Request.Form.GetValues("search[value]").FirstOrDefault();

            StringBuilder sbWhereCondition = new StringBuilder();
            sbWhereCondition.Append(" AND ast.SettlementStatusID IN (1,2,3,5) ");

            if (!string.IsNullOrWhiteSpace(Request.Form["AssessmentRefNo"]))
            {
                sbWhereCondition.Append(" AND ISNULL(AssessmentRefNo,'') LIKE @AssessmentRefNo");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["AssessmentDate"]))
            {
                sbWhereCondition.Append(" AND ISNULL(REPLACE(CONVERT(varchar(50),AssessmentDate,106),' ','-'),'') LIKE @AssessmentDate");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["TaxPayerName"]))
            {
                sbWhereCondition.Append(" AND dbo.GetTaxPayerName(ast.TaxPayerID,ast.TaxPayerTypeID) LIKE @TaxPayerName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["TaxPayerTypeName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(TaxPayerTypeName,'') LIKE @TaxPayerTypeName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["TaxPayerRIN"]))
            {
                sbWhereCondition.Append(" AND dbo.GetTaxPayerRIN(ast.TaxPayerID,ast.TaxPayerTypeID) LIKE @TaxPayerRIN");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["AssessmentAmount"]))
            {
                sbWhereCondition.Append(" AND ISNULL(Convert(varchar(50),AssessmentAmount,106),'') LIKE @AssessmentAmount");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["SettlementDueDate"]))
            {
                sbWhereCondition.Append(" AND ISNULL(Convert(varchar(50),SettlementDueDate,106),'') LIKE @SettlementDueDate");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["SettlementStatusName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(SettlementStatusName,'') LIKE @SettlementStatus");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["SettlementDate"]))
            {
                sbWhereCondition.Append(" AND ISNULL(Convert(varchar(50),SettlementDate,106),'') LIKE @SettlementDate");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["AssessmentNotes"]))
            {
                sbWhereCondition.Append(" AND ISNULL(AssessmentNotes,'') LIKE @AssessmentNotes");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["ActiveText"]))
            {
                sbWhereCondition.Append(" AND CASE WHEN ISNULL(ast.Active,0) = 0 THEN 'In Active' ELSE 'Active' END LIKE @ActiveText");
            }

            if (!string.IsNullOrEmpty(vFilter))
            {
                sbWhereCondition.Append(" AND ( ISNULL(AssessmentRefNo,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(Convert(varchar(50),AssessmentDate,106),'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(TaxPayerTypeName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR dbo.GetTaxPayerName(ast.TaxPayerID,ast.TaxPayerTypeID) LIKE @MainFilter ");
                sbWhereCondition.Append(" OR dbo.GetTaxPayerRIN(ast.TaxPayerID,ast.TaxPayerTypeID) LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(Convert(varchar(50),AssessmentAmount,106),'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(Convert(varchar(50),SettlementDueDate,106),'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(SettlementStatusName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(Convert(varchar(50),SettlementDate,106),'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(AssessmentNotes,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR CASE WHEN ISNULL(ast.Active,0) = 0 THEN 'In Active' ELSE 'Active' END LIKE @MainFilter)");

            }

            Assessment mObjAssessment = new Assessment()
            {
                WhereCondition = sbWhereCondition.ToString(),
                OrderBy = vSortColumn,
                OrderByDirection = vSortColumnDir,
                PageNumber = (TrynParse.parseInt(vStart) / TrynParse.parseInt(vLength)) + 1,
                PageSize = TrynParse.parseInt(vLength),

                AssessmentRefNo = !string.IsNullOrWhiteSpace(Request.Form["AssessmentRefNo"]) ? "%" + Request.Form["AssessmentRefNo"].Trim() + "%" : TrynParse.parseString(Request.Form["AssessmentRefNo"]),
                strAssessmentDate = !string.IsNullOrWhiteSpace(Request.Form["AssessmentDate"]) ? "%" + Request.Form["AssessmentDate"].Trim() + "%" : TrynParse.parseString(Request.Form["AssessmentDate"]),
                TaxPayerTypeName = !string.IsNullOrWhiteSpace(Request.Form["TaxPayerTypeName"]) ? "%" + Request.Form["TaxPayerTypeName"].Trim() + "%" : TrynParse.parseString(Request.Form["TaxPayerTypeName"]),
                TaxPayerName = !string.IsNullOrWhiteSpace(Request.Form["TaxPayerName"]) ? "%" + Request.Form["TaxPayerName"].Trim() + "%" : TrynParse.parseString(Request.Form["TaxPayerName"]),
                TaxPayerRIN = !string.IsNullOrWhiteSpace(Request.Form["TaxPayerRIN"]) ? "%" + Request.Form["TaxPayerRIN"].Trim() + "%" : TrynParse.parseString(Request.Form["TaxPayerRIN"]),
                strAssessmentAmount = !string.IsNullOrWhiteSpace(Request.Form["AssessmentAmount"]) ? "%" + Request.Form["AssessmentAmount"].Trim() + "%" : TrynParse.parseString(Request.Form["AssessmentAmount"]),
                strSettlementDueDate = !string.IsNullOrWhiteSpace(Request.Form["SettlementDueDate"]) ? "%" + Request.Form["SettlementDueDate"].Trim() + "%" : TrynParse.parseString(Request.Form["SettlementDueDate"]),
                SettlementStatusName = !string.IsNullOrWhiteSpace(Request.Form["SettlementStatusName"]) ? "%" + Request.Form["SettlementStatusName"].Trim() + "%" : TrynParse.parseString(Request.Form["SettlementStatusName"]),
                strSettlementDate = !string.IsNullOrWhiteSpace(Request.Form["SettlementDate"]) ? "%" + Request.Form["SettlementDate"].Trim() + "%" : TrynParse.parseString(Request.Form["SettlementDate"]),
                AssessmentNotes = !string.IsNullOrWhiteSpace(Request.Form["AssessmentNotes"]) ? "%" + Request.Form["AssessmentNotes"].Trim() + "%" : TrynParse.parseString(Request.Form["AssessmentNotes"]),
                ActiveText = !string.IsNullOrWhiteSpace(Request.Form["ActiveText"]) ? "%" + Request.Form["ActiveText"].Trim() + "%" : TrynParse.parseString(Request.Form["ActiveText"]),
                MainFilter = !string.IsNullOrWhiteSpace(vFilter) ? "%" + vFilter.Trim() + "%" : vFilter
            };

            IDictionary<string, object> dcData = new BLAssessment().BL_SearchAssessment(mObjAssessment);
            IList<usp_SearchAssessment_Result> lstAssessment = (IList<usp_SearchAssessment_Result>)dcData["AssessmentList"];

            return Json(new
            {
                draw = vDraw,
                recordsFiltered = dcData["FilteredRecords"],
                recordsTotal = dcData["TotalRecords"],
                data = lstAssessment
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetServiceBillList()
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            var vFilter = Request.Form.GetValues("search[value]").FirstOrDefault();

            StringBuilder sbWhereCondition = new StringBuilder();
            sbWhereCondition.Append(" AND sb.SettlementStatusID IN (1,2,3,5) ");

            if (!string.IsNullOrWhiteSpace(Request.Form["ServiceBillRefNo"]))
            {
                sbWhereCondition.Append(" AND ISNULL(ServiceBillRefNo,'') LIKE @ServiceBillRefNo");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["ServiceBillDate"]))
            {
                sbWhereCondition.Append(" AND ISNULL(REPLACE(CONVERT(varchar(50),ServiceBillDate,106),' ','-'),'') LIKE @ServiceBillDate");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["TaxPayerTypeName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(TaxPayerTypeName,'') LIKE @TaxPayerTypeName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["TaxPayerName"]))
            {
                sbWhereCondition.Append(" AND dbo.GetTaxPayerName(sb.TaxPayerID,sb.TaxPayerTypeID) LIKE @TaxPayerName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["TaxPayerRIN"]))
            {
                sbWhereCondition.Append(" AND dbo.GetTaxPayerRIN(sb.TaxPayerID,sb.TaxPayerTypeID) LIKE @TaxPayerRIN");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["ServiceBillAmount"]))
            {
                sbWhereCondition.Append(" AND ISNULL(Convert(varchar(50),ServiceBillAmount,106),'') LIKE @ServiceBillAmount");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["SettlementDueDate"]))
            {
                sbWhereCondition.Append(" AND ISNULL(Convert(varchar(50),SettlementDueDate,106),'') LIKE @SettlementDueDate");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["SettlementStatusName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(SettlementStatusName,'') LIKE @SettlementStatus");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["SettlementDate"]))
            {
                sbWhereCondition.Append(" AND ISNULL(Convert(varchar(50),SettlementDate,106),'') LIKE @SettlementDate");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["ServiceBillNotes"]))
            {
                sbWhereCondition.Append(" AND ISNULL(Notes,'') LIKE @ServiceBillNotes");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["ActiveText"]))
            {
                sbWhereCondition.Append(" AND CASE WHEN ISNULL(sb.Active,0) = 0 THEN 'In Active' ELSE 'Active' END LIKE @ActiveText");
            }

            if (!string.IsNullOrEmpty(vFilter))
            {
                sbWhereCondition.Append(" AND ( ISNULL(ServiceBillRefNo,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(Convert(varchar(50),ServiceBillDate,106),'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(TaxPayerTypeName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR dbo.GetTaxPayerName(sb.TaxPayerID,sb.TaxPayerTypeID) LIKE @MainFilter ");
                sbWhereCondition.Append(" OR dbo.GetTaxPayerRIN(sb.TaxPayerID,sb.TaxPayerTypeID) LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(Convert(varchar(50),ServiceBillAmount,106),'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(Convert(varchar(50),SettlementDueDate,106),'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(SettlementStatusName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(Convert(varchar(50),SettlementDate,106),'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(Notes,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR CASE WHEN ISNULL(sb.Active,0) = 0 THEN 'In Active' ELSE 'Active' END LIKE @MainFilter)");

            }

            ServiceBill mObjServiceBill = new ServiceBill()
            {
                WhereCondition = sbWhereCondition.ToString(),
                OrderBy = vSortColumn,
                OrderByDirection = vSortColumnDir,
                PageNumber = (TrynParse.parseInt(vStart) / TrynParse.parseInt(vLength)) + 1,
                PageSize = TrynParse.parseInt(vLength),

                ServiceBillRefNo = !string.IsNullOrWhiteSpace(Request.Form["ServiceBillRefNo"]) ? "%" + Request.Form["ServiceBillRefNo"].Trim() + "%" : TrynParse.parseString(Request.Form["ServiceBillRefNo"]),
                strServiceBillDate = !string.IsNullOrWhiteSpace(Request.Form["ServiceBillDate"]) ? "%" + Request.Form["ServiceBillDate"].Trim() + "%" : TrynParse.parseString(Request.Form["ServiceBillDate"]),
                TaxPayerTypeName = !string.IsNullOrWhiteSpace(Request.Form["TaxPayerTypeName"]) ? "%" + Request.Form["TaxPayerTypeName"].Trim() + "%" : TrynParse.parseString(Request.Form["TaxPayerTypeName"]),
                TaxPayerName = !string.IsNullOrWhiteSpace(Request.Form["TaxPayerName"]) ? "%" + Request.Form["TaxPayerName"].Trim() + "%" : TrynParse.parseString(Request.Form["TaxPayerName"]),
                TaxPayerRIN = !string.IsNullOrWhiteSpace(Request.Form["TaxPayerRIN"]) ? "%" + Request.Form["TaxPayerRIN"].Trim() + "%" : TrynParse.parseString(Request.Form["TaxPayerRIN"]),
                strServiceBillAmount = !string.IsNullOrWhiteSpace(Request.Form["ServiceBillAmount"]) ? "%" + Request.Form["ServiceBillAmount"].Trim() + "%" : TrynParse.parseString(Request.Form["ServiceBillAmount"]),
                strSettlementDueDate = !string.IsNullOrWhiteSpace(Request.Form["SettlementDueDate"]) ? "%" + Request.Form["SettlementDueDate"].Trim() + "%" : TrynParse.parseString(Request.Form["SettlementDueDate"]),
                SettlementStatusName = !string.IsNullOrWhiteSpace(Request.Form["SettlementStatusName"]) ? "%" + Request.Form["SettlementStatusName"].Trim() + "%" : TrynParse.parseString(Request.Form["SettlementStatusName"]),
                strSettlementDate = !string.IsNullOrWhiteSpace(Request.Form["SettlementDate"]) ? "%" + Request.Form["SettlementDate"].Trim() + "%" : TrynParse.parseString(Request.Form["SettlementDate"]),
                Notes = !string.IsNullOrWhiteSpace(Request.Form["ServiceBillNotes"]) ? "%" + Request.Form["ServiceBillNotes"].Trim() + "%" : TrynParse.parseString(Request.Form["ServiceBillNotes"]),
                ActiveText = !string.IsNullOrWhiteSpace(Request.Form["ActiveText"]) ? "%" + Request.Form["ActiveText"].Trim() + "%" : TrynParse.parseString(Request.Form["ActiveText"]),
                MainFilter = !string.IsNullOrWhiteSpace(vFilter) ? "%" + vFilter.Trim() + "%" : vFilter
            };

            IDictionary<string, object> dcData = new BLServiceBill().BL_SearchServiceBill(mObjServiceBill);
            IList<usp_SearchServiceBill_Result> lstServiceBill = (IList<usp_SearchServiceBill_Result>)dcData["ServiceBillList"];

            return Json(new
            {
                draw = vDraw,
                recordsFiltered = dcData["FilteredRecords"],
                recordsTotal = dcData["TotalRecords"],
                data = lstServiceBill
            }, JsonRequestBehavior.AllowGet);
        }
    }
}