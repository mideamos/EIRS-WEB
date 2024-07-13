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
using System.Linq.Dynamic;
using System.Text;

namespace EIRS.Admin.Controllers
{
    public class AssessmentRuleController : BaseController
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

            if (!string.IsNullOrWhiteSpace(Request.Form["AssessmentRuleCode"]))
            {
                sbWhereCondition.Append(" AND ISNULL(AssessmentRuleCode,'') LIKE @AssessmentRuleCode");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["ProfileReferenceNo"]))
            {
                sbWhereCondition.Append(" AND ISNULL(ProfileReferenceNo,'') LIKE @ProfileReferenceNo");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["AssessmentRuleName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(AssessmentRuleName,'') LIKE @AssessmentRuleName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["RuleRunName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(RuleRunName,'') LIKE @RuleRunName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["PaymentFrequencyName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(PaymentFrequencyName,'') LIKE @PaymentFrequencyName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["TaxYear"]))
            {
                sbWhereCondition.Append(" AND ISNULL(Convert(varchar(50),TaxYear,106),'') LIKE @TaxYear");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["PaymentOptionName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(PaymentOptionName,'') LIKE @PaymentOptionName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["SettlementMethodName"]))
            {
                sbWhereCondition.Append(" AND EXISTS(SELECT * FROM MAP_AssessmentRule_SettlementMethod arsmthd ");
                sbWhereCondition.Append(" INNER JOIN Settlement_Method smthd ON arsmthd.SettlementMethodID = smthd.SettlementMethodID ");
                sbWhereCondition.Append(" WHERE arsmthd.AssessmentRuleID = arule.AssessmentRuleID ");
                sbWhereCondition.Append(" AND ISNULL(smthd.SettlementMethodName, '') LIKE @SettlementMethodNames) ");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["AssessmentItemName"]))
            {
                sbWhereCondition.Append(" AND EXISTS (SELECT * FROM MAP_AssessmentRule_AssessmentItem araitem ");
                sbWhereCondition.Append(" INNER JOIN Assessment_Items aitem ON araitem.AssessmentItemID= aitem.AssessmentItemID ");
                sbWhereCondition.Append(" WHERE araitem.AssessmentRuleID = arule.AssessmentRuleID ");
                sbWhereCondition.Append(" AND ISNULL(aitem.AssessmentItemName, '') LIKE @AssessmentItemNames) ");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["AssessmentAmount"]))
            {
                sbWhereCondition.Append(" AND ISNULL(Convert(varchar(50),AssessmentAmount,106),'') LIKE @AssessmentAmount");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["ActiveText"]))
            {
                sbWhereCondition.Append(" AND CASE WHEN ISNULL(arule.Active,0) = 0 THEN 'In Active' ELSE 'Active' END LIKE @ActiveText");
            }

            if (!string.IsNullOrEmpty(vFilter))
            {
                sbWhereCondition.Append(" AND ( ISNULL(AssessmentRuleCode,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(ProfileReferenceNo,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(AssessmentRuleName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(RuleRunName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(PaymentFrequencyName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(Convert(varchar(50),TaxYear,106),'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(PaymentOptionName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR EXISTS (SELECT * FROM MAP_AssessmentRule_AssessmentItem araitem ");
                sbWhereCondition.Append(" INNER JOIN Assessment_Items aitem ON araitem.AssessmentItemID= aitem.AssessmentItemID ");
                sbWhereCondition.Append(" WHERE araitem.AssessmentRuleID = arule.AssessmentRuleID ");
                sbWhereCondition.Append(" AND ISNULL(aitem.AssessmentItemName, '') LIKE @MainFilter)  ");
                sbWhereCondition.Append(" OR EXISTS(SELECT * FROM MAP_AssessmentRule_SettlementMethod arsmthd ");
                sbWhereCondition.Append(" INNER JOIN Settlement_Method smthd ON arsmthd.SettlementMethodID = smthd.SettlementMethodID ");
                sbWhereCondition.Append(" WHERE arsmthd.AssessmentRuleID = arule.AssessmentRuleID ");
                sbWhereCondition.Append(" AND ISNULL(smthd.SettlementMethodName, '') LIKE @MainFilter) ");
                sbWhereCondition.Append(" OR ISNULL(Convert(varchar(50),AssessmentAmount,106),'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR CASE WHEN ISNULL(arule.Active,0) = 0 THEN 'In Active' ELSE 'Active' END LIKE @MainFilter)");
            }

            Assessment_Rules mObjAssessmentRule = new Assessment_Rules()
            {
                WhereCondition = sbWhereCondition.ToString(),
                OrderBy = vSortColumn,
                OrderByDirection = vSortColumnDir,
                PageNumber = (TrynParse.parseInt(vStart) / TrynParse.parseInt(vLength)) + 1,
                PageSize = TrynParse.parseInt(vLength),

                AssessmentRuleCode = !string.IsNullOrWhiteSpace(Request.Form["AssessmentRuleCode"]) ? "%" + Request.Form["AssessmentRuleCode"].Trim() + "%" : TrynParse.parseString(Request.Form["AssessmentRuleCode"]),
                ProfileReferenceNo = !string.IsNullOrWhiteSpace(Request.Form["ProfileReferenceNo"]) ? "%" + Request.Form["ProfileReferenceNo"].Trim() + "%" : TrynParse.parseString(Request.Form["ProfileReferenceNo"]),
                AssessmentRuleName = !string.IsNullOrWhiteSpace(Request.Form["AssessmentRuleName"]) ? "%" + Request.Form["AssessmentRuleName"].Trim() + "%" : TrynParse.parseString(Request.Form["AssessmentRuleName"]),
                RuleRunName= !string.IsNullOrWhiteSpace(Request.Form["RuleRunName"]) ? "%" + Request.Form["RuleRunName"].Trim() + "%" : TrynParse.parseString(Request.Form["RuleRunName"]),
                PaymentFrequencyName = !string.IsNullOrWhiteSpace(Request.Form["PaymentFrequencyName"]) ? "%" + Request.Form["PaymentFrequencyName"].Trim() + "%" : TrynParse.parseString(Request.Form["PaymentFrequencyName"]),
                AssessmentItemName = !string.IsNullOrWhiteSpace(Request.Form["AssessmentItemName"]) ? "%" + Request.Form["AssessmentItemName"].Trim() + "%" : TrynParse.parseString(Request.Form["AssessmentItemName"]),
                StrAssessmentAmount= !string.IsNullOrWhiteSpace(Request.Form["AssessmentAmount"]) ? "%" + Request.Form["AssessmentAmount"].Trim() + "%" : TrynParse.parseString(Request.Form["AssessmentAmount"]),
                StrTaxYear = !string.IsNullOrWhiteSpace(Request.Form["TaxYear"]) ? "%" + Request.Form["TaxYear"].Trim() + "%" : TrynParse.parseString(Request.Form["TaxYear"]),
                SettlementMethodName = !string.IsNullOrWhiteSpace(Request.Form["SettlementMethodName"]) ? "%" + Request.Form["SettlementMethodName"].Trim() + "%" : TrynParse.parseString(Request.Form["SettlementMethodName"]),
                PaymentOptionName = !string.IsNullOrWhiteSpace(Request.Form["PaymentOptionName"]) ? "%" + Request.Form["PaymentOptionName"].Trim() + "%" : TrynParse.parseString(Request.Form["PaymentOptionName"]),
                ActiveText = !string.IsNullOrWhiteSpace(Request.Form["ActiveText"]) ? "%" + Request.Form["ActiveText"].Trim() + "%" : TrynParse.parseString(Request.Form["ActiveText"]),
                MainFilter = !string.IsNullOrWhiteSpace(vFilter) ? "%" + vFilter.Trim() + "%" : vFilter
            };

            IDictionary<string, object> dcData = new BLAssessmentRule().BL_SearchAssessmentRule(mObjAssessmentRule);
            IList<usp_SearchAssessmentRules_Result> lstAssessmentRule = (IList<usp_SearchAssessmentRules_Result>)dcData["AssessmentRuleList"];

            return Json(new
            {
                draw = vDraw,
                recordsFiltered = dcData["FilteredRecords"],
                recordsTotal = dcData["TotalRecords"],
                data = lstAssessmentRule
            }, JsonRequestBehavior.AllowGet);
        }

        private void UI_FillDropDown(AssessmentRuleViewModel pObjAssessmentRuleModel = null)
        {
            if (pObjAssessmentRuleModel == null)
                pObjAssessmentRuleModel = new AssessmentRuleViewModel();

            UI_FillRuleRun();
            UI_FillPaymentFrequencyDropDown(new Payment_Frequency() { intStatus = 1, IncludePaymentFrequencyIds = pObjAssessmentRuleModel.PaymentFrequencyID.ToString() });
            UI_FillPaymentOptionDropDown(new Payment_Options() { intStatus = 1, IncludePaymentOptionIds = pObjAssessmentRuleModel.PaymentOptionID.ToString() });
            UI_FillSettlementMethodDropDown(new Settlement_Method() { intStatus = 1 });
            UI_FillYearDropDown();


            ViewBag.AssessmentRuleItemList = SessionManager.lstAssessmentRuleItem;
            ViewBag.AssessmentRuleProfileList = SessionManager.lstAssessmentRuleProfile;

            //Profile mObjProfile = new Profile()
            //{
            //    IntStatus = 1
            //};

            //IList<usp_GetProfileList_Result> lstProfile = new BLProfile().BL_GetProfileList(mObjProfile);
            //ViewBag.ProfileList = lstProfile;

            //Assessment_Items mObjAssessmentItem = new Assessment_Items()
            //{
            //    intStatus = 1
            //};

            //IList<usp_GetAssessmentItemList_Result> lstAssessmentItem = new BLAssessmentItem().BL_GetAssessmentItemList(mObjAssessmentItem);
            //ViewBag.AssessmentItemList = lstAssessmentItem;

        }

        public ActionResult Add()
        {
            SessionManager.lstAssessmentRuleProfile = new List<AssessmentRule_Profile>();
            SessionManager.lstAssessmentRuleItem = new List<AssessmentRule_AssessmentItem>();
            UI_FillDropDown();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(AssessmentRuleViewModel pObjAssessmentRuleModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown(pObjAssessmentRuleModel);
                return View(pObjAssessmentRuleModel);
            }
            else
            {
                AssessmentRule_Profile mObjAssessmentRuleProfile = SessionManager.lstAssessmentRuleProfile != null ? SessionManager.lstAssessmentRuleProfile.Where(t => t.intTrack != EnumList.Track.DELETE).FirstOrDefault() : new AssessmentRule_Profile();
                IList<AssessmentRule_AssessmentItem> lstAssessmentRuleItems = SessionManager.lstAssessmentRuleItem ?? new List<AssessmentRule_AssessmentItem>();

                int IntAssessmentRuleItemCount = lstAssessmentRuleItems.Where(t => t.intTrack != EnumList.Track.DELETE).Count();
                int IntAssessmentRuleProfileCount = mObjAssessmentRuleProfile != null && mObjAssessmentRuleProfile.ProfileID > 0 ? 1 : 0;

                if (IntAssessmentRuleProfileCount == 0 && IntAssessmentRuleItemCount == 0)
                {
                    UI_FillDropDown(pObjAssessmentRuleModel);
                    ModelState.AddModelError("AssessmentRuleProfile-error", "Please Link Assessment Rule Profile");
                    ModelState.AddModelError("AssessmentRuleItem-error", "Please Link Assessment Rule Item");
                    return View(pObjAssessmentRuleModel);
                }
                else if (IntAssessmentRuleProfileCount == 0)
                {
                    UI_FillDropDown(pObjAssessmentRuleModel);
                    ModelState.AddModelError("AssessmentRuleProfile-error", "Please Link Assessment Rule Profile");
                    return View(pObjAssessmentRuleModel);
                }
                else if (IntAssessmentRuleItemCount == 0)
                {
                    UI_FillDropDown(pObjAssessmentRuleModel);
                    ModelState.AddModelError("AssessmentRuleItem-error", "Please Link Assessment Rule Item");
                    return View(pObjAssessmentRuleModel);
                }
                else
                {
                    using (TransactionScope mObjTransactionScope = new TransactionScope())
                    {
                        BLAssessmentRule mObjBLAssessmentRule = new BLAssessmentRule();

                        Assessment_Rules mObjAssessmentRule = new Assessment_Rules()
                        {
                            AssessmentRuleID = 0,
                            AssessmentRuleName = pObjAssessmentRuleModel.AssessmentRuleName.Trim(),
                            RuleRunID = pObjAssessmentRuleModel.RuleRunID,
                            PaymentFrequencyID = pObjAssessmentRuleModel.PaymentFrequencyID,
                            TaxYear = pObjAssessmentRuleModel.TaxYear,
                            PaymentOptionID = pObjAssessmentRuleModel.PaymentOptionID,
                            ProfileID = mObjAssessmentRuleProfile != null ? mObjAssessmentRuleProfile.ProfileID : 0,
                            AssessmentAmount = lstAssessmentRuleItems != null ? lstAssessmentRuleItems.Where(t => t.intTrack != EnumList.Track.DELETE).Sum(t => t.TaxAmount) : 0,
                            Active = true,
                            CreatedBy = SessionManager.SystemUserID,
                            CreatedDate = CommUtil.GetCurrentDateTime()
                        };

                        try
                        {

                            FuncResponse<Assessment_Rules> mObjResponse = mObjBLAssessmentRule.BL_InsertUpdateAssessmentRule(mObjAssessmentRule);

                            if (mObjResponse.Success)
                            {
                                if (pObjAssessmentRuleModel.SettlementMethodIds != null && pObjAssessmentRuleModel.SettlementMethodIds.Length > 0)
                                {
                                    foreach (int intSettlementMethodID in pObjAssessmentRuleModel.SettlementMethodIds)
                                    {
                                        MAP_AssessmentRule_SettlementMethod mObjSettlementMethod = new MAP_AssessmentRule_SettlementMethod()
                                        {
                                            SettlementMethodID = intSettlementMethodID,
                                            AssessmentRuleID = mObjResponse.AdditionalData.AssessmentRuleID,
                                            CreatedBy = SessionManager.SystemUserID,
                                            CreatedDate = DateTime.Now
                                        };

                                        mObjBLAssessmentRule.BL_InsertSettlementMethod(mObjSettlementMethod);
                                    }
                                }

                                if (lstAssessmentRuleItems.Where(t => t.intTrack != EnumList.Track.DELETE).Count() > 0)
                                {
                                    foreach (var assessmentitem in lstAssessmentRuleItems)
                                    {
                                        if (assessmentitem.intTrack != EnumList.Track.DELETE)
                                        {
                                            MAP_AssessmentRule_AssessmentItem mObjAssessmenstItem = new MAP_AssessmentRule_AssessmentItem()
                                            {
                                                AssessmentItemID = assessmentitem.AssessmentItemID,
                                                AssessmentRuleID = mObjResponse.AdditionalData.AssessmentRuleID,
                                                CreatedBy = SessionManager.SystemUserID,
                                                CreatedDate = DateTime.Now
                                            };

                                            mObjBLAssessmentRule.BL_InsertAssessmentItem(mObjAssessmenstItem);
                                        }
                                    }
                                }

                                mObjTransactionScope.Complete();
                                FlashMessage.Info(mObjResponse.Message);
                                return RedirectToAction("List", "AssessmentRule");
                            }
                            else
                            {
                                UI_FillDropDown(pObjAssessmentRuleModel);
                                Transaction.Current.Rollback();
                                ViewBag.Message = mObjResponse.Message;
                                return View(pObjAssessmentRuleModel);
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorSignal.FromCurrentContext().Raise(ex);
                            UI_FillDropDown(pObjAssessmentRuleModel);
                            Transaction.Current.Rollback();
                            ViewBag.Message = "Error occurred while saving assessment rule";
                            return View(pObjAssessmentRuleModel);
                        }
                    }
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Assessment_Rules mObjAssessmentRule = new Assessment_Rules()
                {
                    AssessmentRuleID = id.GetValueOrDefault(),
                    IntStatus = 2
                };

                usp_GetAssessmentRuleList_Result mObjAssessmentRuleData = new BLAssessmentRule().BL_GetAssessmentRuleDetails(mObjAssessmentRule);

                if (mObjAssessmentRuleData != null)
                {
                    AssessmentRuleViewModel mObjAssessmentRuleModelView = new AssessmentRuleViewModel()
                    {
                        AssessmentRuleID = mObjAssessmentRuleData.AssessmentRuleID.GetValueOrDefault(),
                        AssessmentRuleCode = mObjAssessmentRuleData.AssessmentRuleCode,
                        AssessmentRuleName = mObjAssessmentRuleData.AssessmentRuleName,
                        RuleRunID = mObjAssessmentRuleData.RuleRunID.GetValueOrDefault(),
                        PaymentFrequencyID = mObjAssessmentRuleData.PaymentFrequencyID.GetValueOrDefault(),
                        TaxYear = mObjAssessmentRuleData.TaxYear.GetValueOrDefault(),
                        SettlementMethodIds = TrynParse.parseIntArray(mObjAssessmentRuleData.SettlementMethodIds),
                        PaymentOptionID = mObjAssessmentRuleData.PaymentOptionID.GetValueOrDefault(),
                        Active = mObjAssessmentRuleData.Active.GetValueOrDefault(),
                    };

                    SessionManager.lstAssessmentRuleProfile = new List<AssessmentRule_Profile>();
                    SessionManager.lstAssessmentRuleItem = new List<AssessmentRule_AssessmentItem>();

                    if (mObjAssessmentRuleData.ProfileID.GetValueOrDefault() > 0)
                    {
                        AssessmentRule_Profile mObjAssessmentRuleProfile = new AssessmentRule_Profile()
                        {
                            RowID = 1,
                            ProfileID = mObjAssessmentRuleData.ProfileID.GetValueOrDefault(),
                            ProfileReferenceNo = mObjAssessmentRuleData.ProfileReferenceNo,
                            AssetTypeName = mObjAssessmentRuleData.ProfileAssetTypeName,
                            intTrack = EnumList.Track.EXISTING
                        };

                        IList<AssessmentRule_Profile> lstAssessmentRuleProfile = new List<AssessmentRule_Profile>();
                        lstAssessmentRuleProfile.Add(mObjAssessmentRuleProfile);

                        SessionManager.lstAssessmentRuleProfile = lstAssessmentRuleProfile;
                    }


                    IList<MAP_AssessmentRule_AssessmentItem> lstAssessmentRule_AItem = new BLAssessmentRule().BL_GetAssessmentItem(mObjAssessmentRuleData.AssessmentRuleID.GetValueOrDefault());
                    if (lstAssessmentRule_AItem != null && lstAssessmentRule_AItem.Count > 0)
                    {
                        IList<AssessmentRule_AssessmentItem> lstAssessmentRuleItems = new List<AssessmentRule_AssessmentItem>();
                        string[] strArrAssessmentItemIds = mObjAssessmentRuleData.AssessmentItemIds.Split(',');

                        BLAssessmentItem mObjBLAssessmentItem = new BLAssessmentItem();

                        foreach (var vAssessmentItem in lstAssessmentRule_AItem)
                        {
                            if (vAssessmentItem.AssessmentItemID.GetValueOrDefault() > 0)
                            {
                                usp_GetAssessmentItemList_Result mObjAssessmentItem = mObjBLAssessmentItem.BL_GetAssessmentItemDetails(new Assessment_Items() { intStatus = 2, AssessmentItemID = vAssessmentItem.AssessmentItemID.GetValueOrDefault() });

                                AssessmentRule_AssessmentItem mObjAssessmentRuleItem = new AssessmentRule_AssessmentItem()
                                {
                                    RowID = lstAssessmentRuleItems.Count + 1
                                };

                                mObjAssessmentRuleItem.TablePKID = vAssessmentItem.ARAIID;
                                mObjAssessmentRuleItem.AssessmentItemID = mObjAssessmentItem.AssessmentItemID.GetValueOrDefault();
                                mObjAssessmentRuleItem.AssessmentItemReferenceNo = mObjAssessmentItem.AssessmentItemReferenceNo;
                                mObjAssessmentRuleItem.AssessmentItemName = mObjAssessmentItem.AssessmentItemName;
                                mObjAssessmentRuleItem.TaxAmount = mObjAssessmentItem.TaxAmount.GetValueOrDefault();
                                mObjAssessmentRuleItem.intTrack = EnumList.Track.EXISTING;

                                lstAssessmentRuleItems.Add(mObjAssessmentRuleItem);
                            }
                        }

                        SessionManager.lstAssessmentRuleItem = lstAssessmentRuleItems;

                    }


                    UI_FillDropDown(mObjAssessmentRuleModelView);
                    return View(mObjAssessmentRuleModelView);
                }
                else
                {
                    return RedirectToAction("List", "AssessmentRule");
                }
            }
            else
            {
                return RedirectToAction("List", "AssessmentRule");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(AssessmentRuleViewModel pObjAssessmentRuleModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown(pObjAssessmentRuleModel);
                return View(pObjAssessmentRuleModel);
            }
            else
            {
                AssessmentRule_Profile mObjAssessmentRuleProfile = SessionManager.lstAssessmentRuleProfile != null ? SessionManager.lstAssessmentRuleProfile.Where(t => t.intTrack != EnumList.Track.DELETE).FirstOrDefault() : new AssessmentRule_Profile();
                IList<AssessmentRule_AssessmentItem> lstAssessmentRuleItems = SessionManager.lstAssessmentRuleItem ?? new List<AssessmentRule_AssessmentItem>();

                int IntAssessmentRuleItemCount = lstAssessmentRuleItems.Where(t => t.intTrack != EnumList.Track.DELETE).Count();
                int IntAssessmentRuleProfileCount = mObjAssessmentRuleProfile != null && mObjAssessmentRuleProfile.ProfileID > 0 ? 1 : 0;

                if (IntAssessmentRuleProfileCount == 0 && IntAssessmentRuleItemCount == 0)
                {
                    UI_FillDropDown(pObjAssessmentRuleModel);
                    ModelState.AddModelError("AssessmentRuleProfile-error", "Please Link Assessment Rule Profile");
                    ModelState.AddModelError("AssessmentRuleItem-error", "Please Link Assessment Rule Item");
                    return View(pObjAssessmentRuleModel);
                }
                else if (IntAssessmentRuleProfileCount == 0)
                {
                    UI_FillDropDown(pObjAssessmentRuleModel);
                    ModelState.AddModelError("AssessmentRuleProfile-error", "Please Link Assessment Rule Profile");
                    return View(pObjAssessmentRuleModel);
                }
                else if (IntAssessmentRuleItemCount == 0)
                {
                    UI_FillDropDown(pObjAssessmentRuleModel);
                    ModelState.AddModelError("AssessmentRuleItem-error", "Please Link Assessment Rule Item");
                    return View(pObjAssessmentRuleModel);
                }
                else
                {
                    using (TransactionScope mObjTransactionScope = new TransactionScope())
                    {
                        BLAssessmentRule mObjBLAssessmentRule = new BLAssessmentRule();

                        Assessment_Rules mObjAssessmentRule = new Assessment_Rules()
                        {
                            AssessmentRuleID = pObjAssessmentRuleModel.AssessmentRuleID,
                            AssessmentRuleName = pObjAssessmentRuleModel.AssessmentRuleName.Trim(),
                            RuleRunID = pObjAssessmentRuleModel.RuleRunID,
                            PaymentFrequencyID = pObjAssessmentRuleModel.PaymentFrequencyID,
                            TaxYear = pObjAssessmentRuleModel.TaxYear,
                            PaymentOptionID = pObjAssessmentRuleModel.PaymentOptionID,
                            ProfileID = mObjAssessmentRuleProfile != null ? mObjAssessmentRuleProfile.ProfileID : 0,
                            AssessmentAmount = lstAssessmentRuleItems != null ? lstAssessmentRuleItems.Where(t => t.intTrack != EnumList.Track.DELETE).Sum(t => t.TaxAmount) : 0,
                            Active = pObjAssessmentRuleModel.Active,
                            ModifiedBy = SessionManager.SystemUserID,
                            ModifiedDate = CommUtil.GetCurrentDateTime()
                        };

                        try
                        {

                            FuncResponse<Assessment_Rules> mObjResponse = mObjBLAssessmentRule.BL_InsertUpdateAssessmentRule(mObjAssessmentRule);

                            if (mObjResponse.Success)
                            {

                                IList<MAP_AssessmentRule_SettlementMethod> lstOldSettlementMethod = mObjBLAssessmentRule.BL_GetSettlementMethod(pObjAssessmentRuleModel.AssessmentRuleID);

                                int[] intRemovedSettlementMethod = new int[] { };
                                int[] intAddedSettlementMethod = new int[] { };

                                if (pObjAssessmentRuleModel.SettlementMethodIds == null)
                                {
                                    intRemovedSettlementMethod = lstOldSettlementMethod.Select(t => t.ARSMID).ToArray();
                                }
                                else
                                {
                                    intRemovedSettlementMethod = lstOldSettlementMethod.Where(t => !pObjAssessmentRuleModel.SettlementMethodIds.Contains(t.SettlementMethodID.GetValueOrDefault())).Select(t => t.ARSMID).ToArray();

                                    if (lstOldSettlementMethod == null || lstOldSettlementMethod.Count() == 0)
                                    {
                                        intAddedSettlementMethod = pObjAssessmentRuleModel.SettlementMethodIds;
                                    }
                                    else
                                    {
                                        int[] intSettlementMethodID = lstOldSettlementMethod.Select(t => t.SettlementMethodID.GetValueOrDefault()).ToArray();
                                        intAddedSettlementMethod = pObjAssessmentRuleModel.SettlementMethodIds.Except(intSettlementMethodID).ToArray();
                                    }
                                }

                                foreach (int intARSMID in intRemovedSettlementMethod)
                                {
                                    MAP_AssessmentRule_SettlementMethod mObjSettlementMethod = new MAP_AssessmentRule_SettlementMethod()
                                    {
                                        ARSMID = intARSMID
                                    };

                                    mObjBLAssessmentRule.BL_RemoveSettlementMethod(mObjSettlementMethod);
                                }

                                foreach (int intSettlementMethodID in intAddedSettlementMethod)
                                {
                                    MAP_AssessmentRule_SettlementMethod mObjSettlementMethod = new MAP_AssessmentRule_SettlementMethod()
                                    {
                                        AssessmentRuleID = pObjAssessmentRuleModel.AssessmentRuleID,
                                        SettlementMethodID = intSettlementMethodID,
                                        CreatedBy = SessionManager.SystemUserID,
                                        CreatedDate = DateTime.Now
                                    };

                                    mObjBLAssessmentRule.BL_InsertSettlementMethod(mObjSettlementMethod);
                                }


                                foreach (var assessmentitem in lstAssessmentRuleItems)
                                {
                                    if (assessmentitem.intTrack == EnumList.Track.INSERT)
                                    {
                                        MAP_AssessmentRule_AssessmentItem mObjAssessmenstItem = new MAP_AssessmentRule_AssessmentItem()
                                        {
                                            AssessmentItemID = assessmentitem.AssessmentItemID,
                                            AssessmentRuleID = mObjResponse.AdditionalData.AssessmentRuleID,
                                            CreatedBy = SessionManager.SystemUserID,
                                            CreatedDate = DateTime.Now
                                        };

                                        mObjBLAssessmentRule.BL_InsertAssessmentItem(mObjAssessmenstItem);
                                    }
                                    else if (assessmentitem.intTrack == EnumList.Track.DELETE)
                                    {
                                        MAP_AssessmentRule_AssessmentItem mObjAssessmenstItem = new MAP_AssessmentRule_AssessmentItem()
                                        {
                                            ARAIID = assessmentitem.TablePKID
                                        };

                                        mObjBLAssessmentRule.BL_RemoveAssessmentItem(mObjAssessmenstItem);
                                    }
                                }


                                mObjTransactionScope.Complete();
                                FlashMessage.Info(mObjResponse.Message);
                                return RedirectToAction("List", "AssessmentRule");
                            }
                            else
                            {
                                UI_FillDropDown(pObjAssessmentRuleModel);
                                Transaction.Current.Rollback();
                                ViewBag.Message = mObjResponse.Message;
                                return View(pObjAssessmentRuleModel);
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorSignal.FromCurrentContext().Raise(ex);
                            UI_FillDropDown(pObjAssessmentRuleModel);
                            Transaction.Current.Rollback();
                            ViewBag.Message = "Error occurred while updating assessment rule";
                            return View(pObjAssessmentRuleModel);
                        }
                    }
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Assessment_Rules mObjAssessmentRule = new Assessment_Rules()
                {
                    AssessmentRuleID = id.GetValueOrDefault(),
                    IntStatus = 2
                };

                usp_GetAssessmentRuleList_Result mObjAssessmentRuleData = new BLAssessmentRule().BL_GetAssessmentRuleDetails(mObjAssessmentRule);

                if (mObjAssessmentRuleData != null)
                {
                    AssessmentRuleViewModel mObjAssessmentRuleModelView = new AssessmentRuleViewModel()
                    {
                        AssessmentRuleID = mObjAssessmentRuleData.AssessmentRuleID.GetValueOrDefault(),
                        AssessmentRuleCode = mObjAssessmentRuleData.AssessmentRuleCode,
                        AssessmentRuleName = mObjAssessmentRuleData.AssessmentRuleName,
                        RuleRunName = mObjAssessmentRuleData.RuleRunName,
                        PaymentFrequencyName = mObjAssessmentRuleData.PaymentFrequencyName,
                        TaxYear = mObjAssessmentRuleData.TaxYear.GetValueOrDefault(),
                        SettlementMethodNames = mObjAssessmentRuleData.SettlementMethodNames,
                        PaymentOptionName = mObjAssessmentRuleData.PaymentOptionName,
                        ActiveText = mObjAssessmentRuleData.ActiveText,
                    };

                    SessionManager.lstAssessmentRuleProfile = new List<AssessmentRule_Profile>();
                    SessionManager.lstAssessmentRuleItem = new List<AssessmentRule_AssessmentItem>();

                    if (mObjAssessmentRuleData.ProfileID.GetValueOrDefault() > 0)
                    {
                        AssessmentRule_Profile mObjAssessmentRuleProfile = new AssessmentRule_Profile()
                        {
                            RowID = 1,
                            ProfileID = mObjAssessmentRuleData.ProfileID.GetValueOrDefault(),
                            ProfileReferenceNo = mObjAssessmentRuleData.ProfileReferenceNo,
                            AssetTypeName = mObjAssessmentRuleData.ProfileAssetTypeName,
                            intTrack = EnumList.Track.EXISTING
                        };

                        IList<AssessmentRule_Profile> lstAssessmentRuleProfile = new List<AssessmentRule_Profile>();
                        lstAssessmentRuleProfile.Add(mObjAssessmentRuleProfile);

                        SessionManager.lstAssessmentRuleProfile = lstAssessmentRuleProfile;
                    }


                    IList<MAP_AssessmentRule_AssessmentItem> lstAssessmentRule_AItem = new BLAssessmentRule().BL_GetAssessmentItem(mObjAssessmentRuleData.AssessmentRuleID.GetValueOrDefault());
                    if (lstAssessmentRule_AItem != null && lstAssessmentRule_AItem.Count > 0)
                    {
                        IList<AssessmentRule_AssessmentItem> lstAssessmentRuleItems = new List<AssessmentRule_AssessmentItem>();

                        BLAssessmentItem mObjBLAssessmentItem = new BLAssessmentItem();

                        foreach (var vAssessmentItem in lstAssessmentRule_AItem)
                        {
                            if (vAssessmentItem.AssessmentItemID.GetValueOrDefault() > 0)
                            {
                                usp_GetAssessmentItemList_Result mObjAssessmentItem = mObjBLAssessmentItem.BL_GetAssessmentItemDetails(new Assessment_Items() { intStatus = 2, AssessmentItemID = vAssessmentItem.AssessmentItemID.GetValueOrDefault() });

                                AssessmentRule_AssessmentItem mObjAssessmentRuleItem = new AssessmentRule_AssessmentItem()
                                {
                                    RowID = lstAssessmentRuleItems.Count + 1
                                };

                                mObjAssessmentRuleItem.TablePKID = vAssessmentItem.ARAIID;
                                mObjAssessmentRuleItem.AssessmentItemID = mObjAssessmentItem.AssessmentItemID.GetValueOrDefault();
                                mObjAssessmentRuleItem.AssessmentItemReferenceNo = mObjAssessmentItem.AssessmentItemReferenceNo;
                                mObjAssessmentRuleItem.AssessmentItemName = mObjAssessmentItem.AssessmentItemName;
                                mObjAssessmentRuleItem.TaxAmount = mObjAssessmentItem.TaxAmount.GetValueOrDefault();
                                mObjAssessmentRuleItem.intTrack = EnumList.Track.EXISTING;

                                lstAssessmentRuleItems.Add(mObjAssessmentRuleItem);
                            }
                        }

                        SessionManager.lstAssessmentRuleItem = lstAssessmentRuleItems;

                    }

                    ViewBag.AssessmentRuleItemList = SessionManager.lstAssessmentRuleItem;
                    ViewBag.AssessmentRuleProfileList = SessionManager.lstAssessmentRuleProfile;

                    return View(mObjAssessmentRuleModelView);
                }
                else
                {
                    return RedirectToAction("List", "AssessmentRule");
                }
            }
            else
            {
                return RedirectToAction("List", "AssessmentRule");
            }
        }

        public ActionResult ProfileDetails(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Assessment_Rules mObjAssessmentRule = new Assessment_Rules()
                {
                    AssessmentRuleID = id.GetValueOrDefault(),
                    IntStatus = 2
                };

                usp_GetAssessmentRuleList_Result mObjAssessmentRuleData = new BLAssessmentRule().BL_GetAssessmentRuleDetails(mObjAssessmentRule);

                if (mObjAssessmentRuleData != null)
                {
                    IList<AssessmentRule_Profile> lstAssessmentRuleProfile = new List<AssessmentRule_Profile>();
                    if (mObjAssessmentRuleData.ProfileID.GetValueOrDefault() > 0)
                    {
                        AssessmentRule_Profile mObjAssessmentRuleProfile = new AssessmentRule_Profile()
                        {
                            RowID = 1,
                            ProfileID = mObjAssessmentRuleData.ProfileID.GetValueOrDefault(),
                            ProfileReferenceNo = mObjAssessmentRuleData.ProfileReferenceNo,
                            AssetTypeName = mObjAssessmentRuleData.ProfileAssetTypeName,
                            intTrack = EnumList.Track.EXISTING
                        };


                        lstAssessmentRuleProfile.Add(mObjAssessmentRuleProfile);

                    }

                    return View(lstAssessmentRuleProfile);
                }
                else
                {
                    return RedirectToAction("List", "AssessmentRule");
                }
            }
            else
            {
                return RedirectToAction("List", "AssessmentRule");
            }
        }

        public ActionResult AssessmentItemDetails(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                IList<MAP_AssessmentRule_AssessmentItem> lstAssessmentRule_AItem = new BLAssessmentRule().BL_GetAssessmentItem(id.GetValueOrDefault());
                if (lstAssessmentRule_AItem != null && lstAssessmentRule_AItem.Count > 0)
                {
                    IList<AssessmentRule_AssessmentItem> lstAssessmentRuleItems = new List<AssessmentRule_AssessmentItem>();

                    BLAssessmentItem mObjBLAssessmentItem = new BLAssessmentItem();

                    foreach (var vAssessmentItem in lstAssessmentRule_AItem)
                    {
                        if (vAssessmentItem.AssessmentItemID.GetValueOrDefault() > 0)
                        {
                            usp_GetAssessmentItemList_Result mObjAssessmentItem = mObjBLAssessmentItem.BL_GetAssessmentItemDetails(new Assessment_Items() { intStatus = 2, AssessmentItemID = vAssessmentItem.AssessmentItemID.GetValueOrDefault() });

                            AssessmentRule_AssessmentItem mObjAssessmentRuleItem = new AssessmentRule_AssessmentItem()
                            {
                                RowID = lstAssessmentRuleItems.Count + 1
                            };

                            mObjAssessmentRuleItem.TablePKID = vAssessmentItem.ARAIID;
                            mObjAssessmentRuleItem.AssessmentItemID = mObjAssessmentItem.AssessmentItemID.GetValueOrDefault();
                            mObjAssessmentRuleItem.AssessmentItemReferenceNo = mObjAssessmentItem.AssessmentItemReferenceNo;
                            mObjAssessmentRuleItem.AssessmentItemName = mObjAssessmentItem.AssessmentItemName;
                            mObjAssessmentRuleItem.TaxAmount = mObjAssessmentItem.TaxAmount.GetValueOrDefault();
                            mObjAssessmentRuleItem.intTrack = EnumList.Track.EXISTING;

                            lstAssessmentRuleItems.Add(mObjAssessmentRuleItem);
                        }
                    }

                    return View(lstAssessmentRuleItems);

                }
                else
                {
                    return RedirectToAction("List", "AssessmentRule");
                }
            }
            else
            {
                return RedirectToAction("List", "AssessmentRule");
            }
        }

        public JsonResult UpdateStatus(Assessment_Rules pObjAssessmentRuleData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjAssessmentRuleData.AssessmentRuleID != 0)
            {
                FuncResponse mObjFuncResponse = new BLAssessmentRule().BL_UpdateStatus(pObjAssessmentRuleData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["AssessmentRuleList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
                }

            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Action";
            }



            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddAssessmentRuleProfile(AssessmentRule_Profile pObjAssessmentRuleProfile)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();
            IList<AssessmentRule_Profile> lstAssessmentRuleProfile = SessionManager.lstAssessmentRuleProfile ?? new List<AssessmentRule_Profile>();
            AssessmentRule_Profile mObjAssessmentRuleProfile = lstAssessmentRuleProfile.Where(t => t.intTrack != EnumList.Track.DELETE).Count() > 0 ? lstAssessmentRuleProfile.Where(t => t.intTrack != EnumList.Track.DELETE).FirstOrDefault() : new AssessmentRule_Profile();

            if (lstAssessmentRuleProfile.Where(t => t.intTrack != EnumList.Track.DELETE).Count() == 0)
                mObjAssessmentRuleProfile.RowID = lstAssessmentRuleProfile.Count + 1;

            mObjAssessmentRuleProfile.ProfileID = pObjAssessmentRuleProfile.ProfileID;
            mObjAssessmentRuleProfile.ProfileReferenceNo = pObjAssessmentRuleProfile.ProfileReferenceNo;
            mObjAssessmentRuleProfile.AssetTypeName = pObjAssessmentRuleProfile.AssetTypeName;
            mObjAssessmentRuleProfile.intTrack = EnumList.Track.UPDATE;

            if (lstAssessmentRuleProfile.Where(t => t.intTrack != EnumList.Track.DELETE).Count() == 0)
                lstAssessmentRuleProfile.Add(mObjAssessmentRuleProfile);

            SessionManager.lstAssessmentRuleProfile = lstAssessmentRuleProfile;

            dcResponse["success"] = true;
            dcResponse["AssessmentRuleProfileList"] = CommUtil.RenderPartialToString("_BindAssessmentRuleProfile", lstAssessmentRuleProfile.Where(t => t.intTrack != EnumList.Track.DELETE).ToList(), this.ControllerContext);
            dcResponse["AssessmentRuleProfileCount"] = lstAssessmentRuleProfile.Where(t => t.intTrack != EnumList.Track.DELETE).Count();
            dcResponse["Message"] = "Profile Added";
            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RemoveAssessmentRuleProfile(int RowID)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();
            IList<AssessmentRule_Profile> lstAssessmentRuleProfile = SessionManager.lstAssessmentRuleProfile ?? new List<AssessmentRule_Profile>();
            AssessmentRule_Profile mObjAssessmentRuleProfile = lstAssessmentRuleProfile.Where(t => t.RowID == RowID).FirstOrDefault();

            if (mObjAssessmentRuleProfile != null)
            {
                mObjAssessmentRuleProfile.intTrack = EnumList.Track.DELETE;
                SessionManager.lstAssessmentRuleProfile = lstAssessmentRuleProfile;

                dcResponse["success"] = true;
                dcResponse["AssessmentRuleProfileList"] = CommUtil.RenderPartialToString("_BindAssessmentRuleProfile", lstAssessmentRuleProfile.Where(t => t.intTrack != EnumList.Track.DELETE).ToList(), this.ControllerContext);
                dcResponse["AssessmentRuleProfileCount"] = lstAssessmentRuleProfile.Where(t => t.intTrack != EnumList.Track.DELETE).Count();
                dcResponse["Message"] = "Profile Removed";

            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Request";
            }


            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddAssessmentRuleItem(string AssessmentItemIds)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();
            IList<AssessmentRule_AssessmentItem> lstAssessmentRuleItem = SessionManager.lstAssessmentRuleItem ?? new List<AssessmentRule_AssessmentItem>();

            string[] strArrAssessmentItemIds = AssessmentItemIds.Split(',');

            if (strArrAssessmentItemIds.Length > 0)
            {
                BLAssessmentItem mObjBLAssessmentItem = new BLAssessmentItem();
                foreach (string strAssessmentItem in strArrAssessmentItemIds)
                {
                    if (!string.IsNullOrEmpty(strAssessmentItem))
                    {
                        usp_GetAssessmentItemList_Result mObjAssessmentItem = mObjBLAssessmentItem.BL_GetAssessmentItemDetails(new Assessment_Items() { intStatus = 2, AssessmentItemID = TrynParse.parseInt(strAssessmentItem) });

                        AssessmentRule_AssessmentItem mObjAssessmentRuleItem;

                        if (lstAssessmentRuleItem.Where(t => t.intTrack != EnumList.Track.DELETE && t.AssessmentItemID == mObjAssessmentItem.AssessmentItemID).Count() == 0)
                        {
                            mObjAssessmentRuleItem = new AssessmentRule_AssessmentItem()
                            {
                                RowID = lstAssessmentRuleItem.Count + 1,
                                intTrack = EnumList.Track.INSERT
                            };
                        }
                        else
                        {
                            mObjAssessmentRuleItem = lstAssessmentRuleItem.Where(t => t.intTrack != EnumList.Track.DELETE && t.AssessmentItemID == mObjAssessmentItem.AssessmentItemID).FirstOrDefault();
                            mObjAssessmentRuleItem.intTrack = EnumList.Track.UPDATE;
                        }


                        mObjAssessmentRuleItem.AssessmentItemID = mObjAssessmentItem.AssessmentItemID.GetValueOrDefault();
                        mObjAssessmentRuleItem.AssessmentItemReferenceNo = mObjAssessmentItem.AssessmentItemReferenceNo;
                        mObjAssessmentRuleItem.AssessmentItemName = mObjAssessmentItem.AssessmentItemName;
                        mObjAssessmentRuleItem.TaxAmount = mObjAssessmentItem.TaxAmount.GetValueOrDefault();

                        if (lstAssessmentRuleItem.Where(t => t.intTrack != EnumList.Track.DELETE && t.AssessmentItemID == mObjAssessmentItem.AssessmentItemID).Count() == 0)
                            lstAssessmentRuleItem.Add(mObjAssessmentRuleItem);

                    }
                }
            }


            SessionManager.lstAssessmentRuleItem = lstAssessmentRuleItem;

            dcResponse["success"] = true;
            dcResponse["AssessmentRuleItemList"] = CommUtil.RenderPartialToString("_BindAssessmentRuleItem", lstAssessmentRuleItem.Where(t => t.intTrack != EnumList.Track.DELETE).ToList(), this.ControllerContext);
            dcResponse["AssessmentRuleItemCount"] = lstAssessmentRuleItem.Where(t => t.intTrack != EnumList.Track.DELETE).Count();
            dcResponse["Message"] = "Assessment Item Added";
            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RemoveAssessmentRuleItem(int RowID)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();
            IList<AssessmentRule_AssessmentItem> lstAssessmentRuleItem = SessionManager.lstAssessmentRuleItem ?? new List<AssessmentRule_AssessmentItem>();
            AssessmentRule_AssessmentItem mObjAssessmentRuleItem = lstAssessmentRuleItem.Where(t => t.RowID == RowID).FirstOrDefault();

            if (mObjAssessmentRuleItem != null)
            {
                mObjAssessmentRuleItem.intTrack = EnumList.Track.DELETE;
                SessionManager.lstAssessmentRuleItem = lstAssessmentRuleItem;

                dcResponse["success"] = true;
                dcResponse["AssessmentRuleItemList"] = CommUtil.RenderPartialToString("_BindAssessmentRuleItem", lstAssessmentRuleItem.Where(t => t.intTrack != EnumList.Track.DELETE).ToList(), this.ControllerContext);
                dcResponse["AssessmentRuleItemCount"] = lstAssessmentRuleItem.Where(t => t.intTrack != EnumList.Track.DELETE).Count();
                dcResponse["Message"] = "Profile Removed";

            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Request";
            }


            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProfileDetails(int ProfileID)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            usp_GetProfileList_Result mObjProfileDetails = new BLProfile().BL_GetProfileDetails(new Profile() { IntStatus = 2, ProfileID = ProfileID });

            if (mObjProfileDetails != null)
            {
                dcResponse["success"] = true;
                dcResponse["ProfileDetails"] = mObjProfileDetails;
            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Request";
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

    }
}