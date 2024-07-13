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
    public class AssessmentController : BaseController
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
                sbWhereCondition.Append(" OR ISNULL(AssessmentAmount,0) LIKE @MainFilter ");
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

        public ActionResult Generate()
        {
            UI_FillTaxPayerTypeDropDown();
            return View();
        }

        public void UI_FillAssessmentDropDown(AssessmentViewModel pObjAssessmentModel = null)
        {
            UI_FillAssetTypeDropDown();

            IList<DropDownListResult> lstDropDowns = new List<DropDownListResult>();
            ViewBag.AssetList = new SelectList(lstDropDowns, "id", "text");
            ViewBag.ProfileList = new SelectList(lstDropDowns, "id", "text");
            ViewBag.AssessmentRuleList = new SelectList(lstDropDowns, "id", "text");


            ViewBag.MAPAssessmentRuleList = SessionManager.lstAssessmentRule;
            ViewBag.MAPAssessmentItemList = SessionManager.lstAssessmentItem.Where(t => t.AssessmentRule_RowID == 0).ToList();
        }

        public ActionResult Add(int? id, string name, int? tptype)
        {
            if (id.GetValueOrDefault() > 0 && tptype.GetValueOrDefault() > 0)
            {

                AssessmentViewModel mObjAssessmentModel = new AssessmentViewModel()
                {
                    TaxPayerID = id.GetValueOrDefault(),
                    TaxPayerRIN = name,
                    TaxPayerTypeID = tptype.GetValueOrDefault(),
                    SettlementDuedate = CommUtil.GetCurrentDateTime()
                };

                if (tptype.GetValueOrDefault() == (int)EnumList.TaxPayerType.Individual)
                {
                    usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(new Individual() { intStatus = 2, IndividualID = id.GetValueOrDefault() });
                    mObjAssessmentModel.TaxPayerName = mObjIndividualData.FirstName + " " + mObjIndividualData.LastName;
                }
                else if (tptype.GetValueOrDefault() == (int)EnumList.TaxPayerType.Companies)
                {
                    usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(new Company() { intStatus = 2, CompanyID = id.GetValueOrDefault() });
                    mObjAssessmentModel.TaxPayerName = mObjCompanyData.CompanyName;
                }
                else if (tptype.GetValueOrDefault() == (int)EnumList.TaxPayerType.Special)
                {
                    usp_GetSpecialList_Result mObjSpecialData = new BLSpecial().BL_GetSpecialDetails(new Special() { intStatus = 2, SpecialID = id.GetValueOrDefault() });
                    mObjAssessmentModel.TaxPayerName = mObjSpecialData.SpecialTaxPayerName;
                }
                else if (tptype.GetValueOrDefault() == (int)EnumList.TaxPayerType.Government)
                {
                    usp_GetGovernmentList_Result mObjGovernmentData = new BLGovernment().BL_GetGovernmentDetails(new Government() { intStatus = 2, GovernmentID = id.GetValueOrDefault() });
                    mObjAssessmentModel.TaxPayerName = mObjGovernmentData.GovernmentName;
                }

                SessionManager.lstAssessmentItem = new List<Assessment_AssessmentItem>();
                SessionManager.lstAssessmentRule = new List<Assessment_AssessmentRule>();
                UI_FillAssessmentDropDown();
                return View(mObjAssessmentModel);
            }
            else
            {
                return RedirectToAction("List", "Assessment");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(AssessmentViewModel pObjAssessmentModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillAssessmentDropDown(pObjAssessmentModel);
                return View(pObjAssessmentModel);
            }
            else
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    IList<Assessment_AssessmentItem> lstAssessmentItems = SessionManager.lstAssessmentItem ?? new List<Assessment_AssessmentItem>();
                    IList<Assessment_AssessmentRule> lstAssessmentRules = SessionManager.lstAssessmentRule ?? new List<Assessment_AssessmentRule>();

                    int IntAssessmentRuleCount = lstAssessmentRules.Where(t => t.intTrack != EnumList.Track.DELETE).Count();

                    if (IntAssessmentRuleCount == 0)
                    {
                        UI_FillAssessmentDropDown(pObjAssessmentModel);
                        ModelState.AddModelError("AssessmentRule-error", "Please Add Atleast One Assessment Rule");
                        Transaction.Current.Rollback();
                        return View(pObjAssessmentModel);
                    }
                    else
                    {
                        BLAssessment mObjBLAssessment = new BLAssessment();

                        Assessment mObjAssessment = new Assessment()
                        {
                            AssessmentID = 0,
                            TaxPayerID = pObjAssessmentModel.TaxPayerID,
                            TaxPayerTypeID = pObjAssessmentModel.TaxPayerTypeID,
                            AssessmentAmount = lstAssessmentItems.Count > 0 ? lstAssessmentItems.Where(t => t.intTrack != EnumList.Track.DELETE).Sum(t => t.TaxAmount) : 0,
                            AssessmentDate = CommUtil.GetCurrentDateTime(),
                            SettlementDueDate = pObjAssessmentModel.SettlementDuedate,
                            SettlementStatusID = (int)EnumList.SettlementStatus.Assessed,
                            AssessmentNotes = pObjAssessmentModel.Notes,
                            Active = true,
                            CreatedBy = SessionManager.SystemUserID,
                            CreatedDate = CommUtil.GetCurrentDateTime()
                        };

                        try
                        {

                            FuncResponse<Assessment> mObjAssessmentResponse = mObjBLAssessment.BL_InsertUpdateAssessment(mObjAssessment);

                            if (mObjAssessmentResponse.Success)
                            {
                                //Adding Asssessment Rules

                                foreach (Assessment_AssessmentRule mObjAAR in lstAssessmentRules.Where(t => t.intTrack != EnumList.Track.DELETE))
                                {
                                    MAP_Assessment_AssessmentRule mObjAssessmentRule = new MAP_Assessment_AssessmentRule()
                                    {
                                        AARID = 0,
                                        AssessmentID = mObjAssessmentResponse.AdditionalData.AssessmentID,
                                        AssetTypeID = mObjAAR.AssetTypeID,
                                        AssetID = mObjAAR.AssetID,
                                        ProfileID = mObjAAR.ProfileID,
                                        AssessmentRuleID = mObjAAR.AssessmentRuleID,
                                        AssessmentAmount = mObjAAR.AssessmentRuleAmount,
                                        AssessmentYear = mObjAAR.TaxYear,
                                        CreatedBy = SessionManager.SystemUserID,
                                        CreatedDate = CommUtil.GetCurrentDateTime()
                                    };

                                    FuncResponse<MAP_Assessment_AssessmentRule> mObjARResponse = mObjBLAssessment.BL_InsertUpdateAssessmentRule(mObjAssessmentRule);

                                    if (mObjARResponse.Success)
                                    {
                                        IList<MAP_Assessment_AssessmentItem> lstInsertAssessmentDetail = new List<MAP_Assessment_AssessmentItem>();

                                        foreach (Assessment_AssessmentItem mObjAssessmentItemDetail in lstAssessmentItems.Where(t => t.AssessmentRule_RowID == mObjAAR.RowID))
                                        {
                                            MAP_Assessment_AssessmentItem mObjAIDetail = new MAP_Assessment_AssessmentItem()
                                            {
                                                AARID = mObjARResponse.AdditionalData.AARID,
                                                AAIID = 0,
                                                AssessmentItemID = mObjAssessmentItemDetail.AssessmentItemID,
                                                TaxBaseAmount = mObjAssessmentItemDetail.TaxBaseAmount,
                                                TaxAmount = mObjAssessmentItemDetail.TaxAmount,
                                                Percentage = mObjAssessmentItemDetail.Percentage,
                                                PaymentStatusID = (int)EnumList.PaymentStatus.Due,
                                                CreatedBy = SessionManager.SystemUserID,
                                                CreatedDate = CommUtil.GetCurrentDateTime(),
                                            };

                                            lstInsertAssessmentDetail.Add(mObjAIDetail);
                                        }

                                        FuncResponse mObjADResponse = mObjBLAssessment.BL_InsertAssessmentItem(lstInsertAssessmentDetail);

                                        if (!mObjADResponse.Success)
                                        {
                                            throw (mObjADResponse.Exception);
                                        }
                                    }
                                    else
                                    {
                                        throw (mObjARResponse.Exception);
                                    }
                                }

                                scope.Complete();
                                FlashMessage.Info(mObjAssessmentResponse.Message);
                                return RedirectToAction("List", "Assessment");

                            }
                            else
                            {
                                UI_FillAssessmentDropDown(pObjAssessmentModel);
                                ViewBag.Message = mObjAssessmentResponse.Message;
                                Transaction.Current.Rollback();
                                return View(pObjAssessmentModel);
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorSignal.FromCurrentContext().Raise(ex);
                            UI_FillAssessmentDropDown(pObjAssessmentModel);
                            ViewBag.Message = "Error occurred while saving assessment";
                            Transaction.Current.Rollback();
                            return View(pObjAssessmentModel);
                        }
                    }
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                BLAssessment mObjBLAssessment = new BLAssessment();
                usp_GetAssessmentList_Result mObjAssessmentData = mObjBLAssessment.BL_GetAssessmentDetails(new Assessment() { AssessmentID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjAssessmentData != null)
                {
                    AssessmentViewModel mObjAssessmentModel = new AssessmentViewModel()
                    {
                        AssessmentID = mObjAssessmentData.AssessmentID.GetValueOrDefault(),
                        TaxPayerName = mObjAssessmentData.TaxPayerName,
                        TaxPayerRIN = mObjAssessmentData.TaxPayerRIN,
                        Notes = mObjAssessmentData.AssessmentNotes,
                        SettlementDuedate = mObjAssessmentData.SettlementDueDate.Value,
                        TaxPayerID = mObjAssessmentData.TaxPayerID.GetValueOrDefault(),
                        TaxPayerTypeID = mObjAssessmentData.TaxPayerTypeID.GetValueOrDefault()
                    };

                    IList<Assessment_AssessmentRule> lstAssessmentRule = new List<Assessment_AssessmentRule>();
                    IList<Assessment_AssessmentItem> lstAssessmentItem = new List<Assessment_AssessmentItem>();

                    IList<usp_GetAssessment_AssessmentRuleList_Result> lstMAPAssessmentRules = mObjBLAssessment.BL_GetAssessmentRules(mObjAssessmentData.AssessmentID.GetValueOrDefault());
                    IList<MAP_Assessment_AssessmentItem> lstMAPAssessmentItems;
                    foreach (var item in lstMAPAssessmentRules)
                    {
                        Assessment_AssessmentRule assessment_AssessmentRule = new Assessment_AssessmentRule()
                        {
                            RowID = lstAssessmentRule.Count + 1,
                            TablePKID = item.AARID.GetValueOrDefault(),
                            AssetTypeID = item.AssetTypeID.GetValueOrDefault(),
                            AssetTypeName = item.AssetTypeName,
                            AssetID = item.AssetID.GetValueOrDefault(),
                            AssetRIN = item.AssetRIN,
                            ProfileID = item.ProfileID.GetValueOrDefault(),
                            ProfileDescription = item.ProfileDescription,
                            AssessmentRuleID = item.AssessmentRuleID.GetValueOrDefault(),
                            AssessmentRuleName = item.AssessmentRuleName,
                            AssessmentRuleAmount = item.AssessmentRuleAmount.GetValueOrDefault(),
                            TaxYear = item.TaxYear.GetValueOrDefault(),
                            intTrack = EnumList.Track.EXISTING
                        };

                        lstAssessmentRule.Add(assessment_AssessmentRule);

                        lstMAPAssessmentItems = mObjBLAssessment.BL_GetAssessmentItems(item.AARID.GetValueOrDefault());

                        foreach (var subitem in lstMAPAssessmentItems)
                        {
                            Assessment_AssessmentItem mObjAssessmentItem = new Assessment_AssessmentItem()
                            {
                                RowID = lstAssessmentItem.Count + 1,
                                AssessmentRule_RowID = assessment_AssessmentRule.RowID,
                                TablePKID = subitem.AAIID,
                                AssessmentItemID = subitem.AssessmentItemID.GetValueOrDefault(),
                                AssessmentItemName = subitem.Assessment_Items.AssessmentItemName,
                                AssessmentItemReferenceNo = subitem.Assessment_Items.AssessmentItemReferenceNo,
                                ComputationID = subitem.Assessment_Items.ComputationID.GetValueOrDefault(),
                                TaxBaseAmount = subitem.TaxBaseAmount.GetValueOrDefault(),
                                TaxAmount = subitem.TaxAmount.GetValueOrDefault(),
                                Percentage = subitem.Percentage.GetValueOrDefault(),
                                intTrack = EnumList.Track.EXISTING
                            };

                            lstAssessmentItem.Add(mObjAssessmentItem);
                        }
                    }



                    SessionManager.lstAssessmentItem = lstAssessmentItem;
                    SessionManager.lstAssessmentRule = lstAssessmentRule;
                    UI_FillAssessmentDropDown();

                    return View(mObjAssessmentModel);
                }
                else
                {
                    return RedirectToAction("List", "Assessment");
                }
            }
            else
            {
                return RedirectToAction("List", "Assessment");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(AssessmentViewModel pObjAssessmentModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillAssessmentDropDown(pObjAssessmentModel);
                return View(pObjAssessmentModel);
            }
            else
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    IList<Assessment_AssessmentItem> lstAssessmentItems = SessionManager.lstAssessmentItem ?? new List<Assessment_AssessmentItem>();
                    IList<Assessment_AssessmentRule> lstAssessmentRules = SessionManager.lstAssessmentRule ?? new List<Assessment_AssessmentRule>();

                    int IntAssessmentRuleCount = lstAssessmentRules.Where(t => t.intTrack != EnumList.Track.DELETE).Count();

                    if (IntAssessmentRuleCount == 0)
                    {
                        UI_FillAssessmentDropDown(pObjAssessmentModel);
                        ModelState.AddModelError("AssessmentRule-error", "Please Add Atleast One Assessment Rule");
                        return View(pObjAssessmentModel);
                    }
                    else
                    {
                        BLAssessment mObjBLAssessment = new BLAssessment();

                        Assessment mObjAssessment = new Assessment()
                        {
                            AssessmentID = pObjAssessmentModel.AssessmentID,
                            TaxPayerID = pObjAssessmentModel.TaxPayerID,
                            TaxPayerTypeID = pObjAssessmentModel.TaxPayerTypeID,
                            AssessmentAmount = lstAssessmentItems.Count > 0 ? lstAssessmentItems.Where(t => t.intTrack != EnumList.Track.DELETE).Sum(t => t.TaxAmount) : 0,
                            AssessmentDate = CommUtil.GetCurrentDateTime(),
                            SettlementDueDate = pObjAssessmentModel.SettlementDuedate,
                            SettlementStatusID = (int)EnumList.SettlementStatus.Assessed,
                            AssessmentNotes = pObjAssessmentModel.Notes,
                            Active = true,
                            ModifiedBy= SessionManager.SystemUserID,
                            ModifiedDate = CommUtil.GetCurrentDateTime()
                        };

                        try
                        {

                            FuncResponse<Assessment> mObjAssessmentResponse = mObjBLAssessment.BL_InsertUpdateAssessment(mObjAssessment);

                            if (mObjAssessmentResponse.Success)
                            {
                                //Adding Asssessment Rules

                                foreach (Assessment_AssessmentRule mObjAAR in lstAssessmentRules)
                                {
                                    if (mObjAAR.intTrack == EnumList.Track.INSERT)
                                    {
                                        MAP_Assessment_AssessmentRule mObjAssessmentRule = new MAP_Assessment_AssessmentRule()
                                        {
                                            AARID = 0,
                                            AssessmentID = mObjAssessmentResponse.AdditionalData.AssessmentID,
                                            AssetTypeID = mObjAAR.AssetTypeID,
                                            AssetID = mObjAAR.AssetID,
                                            ProfileID = mObjAAR.ProfileID,
                                            AssessmentRuleID = mObjAAR.AssessmentRuleID,
                                            AssessmentAmount = mObjAAR.AssessmentRuleAmount,
                                            AssessmentYear = mObjAAR.TaxYear,
                                            CreatedBy = SessionManager.SystemUserID,
                                            CreatedDate = CommUtil.GetCurrentDateTime()
                                        };

                                        FuncResponse<MAP_Assessment_AssessmentRule> mObjARResponse = mObjBLAssessment.BL_InsertUpdateAssessmentRule(mObjAssessmentRule);

                                        if (mObjARResponse.Success)
                                        {
                                            IList<MAP_Assessment_AssessmentItem> lstInsertAssessmentDetail = new List<MAP_Assessment_AssessmentItem>();

                                            foreach (Assessment_AssessmentItem mObjAssessmentItemDetail in lstAssessmentItems.Where(t => t.AssessmentRule_RowID == mObjAAR.RowID))
                                            {
                                                MAP_Assessment_AssessmentItem mObjAIDetail = new MAP_Assessment_AssessmentItem()
                                                {
                                                    AARID = mObjARResponse.AdditionalData.AARID,
                                                    AAIID = 0,
                                                    AssessmentItemID = mObjAssessmentItemDetail.AssessmentItemID,
                                                    TaxBaseAmount = mObjAssessmentItemDetail.TaxBaseAmount,
                                                    TaxAmount = mObjAssessmentItemDetail.TaxAmount,
                                                    Percentage = mObjAssessmentItemDetail.Percentage,
                                                    PaymentStatusID = (int)EnumList.PaymentStatus.Due,
                                                    CreatedBy = SessionManager.SystemUserID,
                                                    CreatedDate = CommUtil.GetCurrentDateTime(),
                                                };

                                                lstInsertAssessmentDetail.Add(mObjAIDetail);
                                            }

                                            FuncResponse mObjADResponse = mObjBLAssessment.BL_InsertAssessmentItem(lstInsertAssessmentDetail);

                                            if (!mObjADResponse.Success)
                                            {
                                                throw (mObjADResponse.Exception);
                                            }
                                        }
                                        else
                                        {
                                            throw (mObjARResponse.Exception);
                                        }
                                    }
                                    else if (mObjAAR.intTrack == EnumList.Track.DELETE)
                                    {
                                        FuncResponse mObjARResponse = mObjBLAssessment.BL_DeleteAssessmentRule(mObjAAR.TablePKID);

                                        if (!mObjARResponse.Success)
                                        {
                                            throw (mObjARResponse.Exception);
                                        }
                                    }
                                }

                                scope.Complete();
                                FlashMessage.Info(mObjAssessmentResponse.Message);
                                return RedirectToAction("List", "Assessment");

                            }
                            else
                            {
                                UI_FillAssessmentDropDown(pObjAssessmentModel);
                                ViewBag.Message = mObjAssessmentResponse.Message;
                                Transaction.Current.Rollback();
                                return View(pObjAssessmentModel);
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorSignal.FromCurrentContext().Raise(ex);
                            UI_FillAssessmentDropDown(pObjAssessmentModel);
                            ViewBag.Message = "Error occurred while updating assessment";
                            Transaction.Current.Rollback();
                            return View(pObjAssessmentModel);
                        }
                    }
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                BLAssessment mObjBLAssessment = new BLAssessment();
                usp_GetAssessmentList_Result mObjAssessmentDetails = mObjBLAssessment.BL_GetAssessmentDetails(new Assessment() { AssessmentID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjAssessmentDetails != null)
                {
                    IList<usp_GetAssessmentRuleItemList_Result> lstAssessmentItems = mObjBLAssessment.BL_GetAssessmentRuleItem(mObjAssessmentDetails.AssessmentID.GetValueOrDefault());

                    ViewBag.AssessmentItemList = lstAssessmentItems;

                    return View(mObjAssessmentDetails);
                }
                else
                {
                    return RedirectToAction("List", "Assessment");
                }
            }
            else
            {
                return RedirectToAction("List", "Assessment");
            }
        }

        public JsonResult UpdateStatus(Assessment pObjAssessmentData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjAssessmentData.AssessmentID != 0)
            {
                FuncResponse mObjFuncResponse = new BLAssessment().BL_UpdateStatus(pObjAssessmentData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["AssessmentList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
                }

            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Action";
            }



            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTaxPayerList(int TaxPayerTypeID)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (TaxPayerTypeID == (int)EnumList.TaxPayerType.Individual)
            {
                Individual mObjIndividual = new Individual()
                {
                    intStatus = 2
                };

                IList<usp_GetIndividualList_Result> lstIndividual = new BLIndividual().BL_GetIndividualList(mObjIndividual);
                dcResponse["success"] = true;

                dcResponse["TaxPayerList"] = CommUtil.RenderPartialToString("_BindIndividualTable_SingleSelect", lstIndividual, this.ControllerContext);

            }
            else if (TaxPayerTypeID == (int)EnumList.TaxPayerType.Companies)
            {
                Company mObjCompany = new Company()
                {
                    intStatus = 2
                };

                IList<usp_GetCompanyList_Result> lstCompany = new BLCompany().BL_GetCompanyList(mObjCompany);
                dcResponse["success"] = true;

                dcResponse["TaxPayerList"] = CommUtil.RenderPartialToString("_BindCompanyTable_SingleSelect", lstCompany, this.ControllerContext);
            }
            else if (TaxPayerTypeID == (int)EnumList.TaxPayerType.Government)
            {
                Government mObjGovernment = new Government()
                {
                    intStatus = 2
                };

                IList<usp_GetGovernmentList_Result> lstGovernment = new BLGovernment().BL_GetGovernmentList(mObjGovernment);
                dcResponse["success"] = true;

                dcResponse["TaxPayerList"] = CommUtil.RenderPartialToString("_BindGovernmentTable_SingleSelect", lstGovernment, this.ControllerContext);

            }
            else if (TaxPayerTypeID == (int)EnumList.TaxPayerType.Special)
            {
                Special mObjSpecial = new Special()
                {
                    intStatus = 2
                };

                IList<usp_GetSpecialList_Result> lstSpecial = new BLSpecial().BL_GetSpecialList(mObjSpecial);
                dcResponse["success"] = true;

                dcResponse["TaxPayerList"] = CommUtil.RenderPartialToString("_BindSpecialTable_SingleSelect", lstSpecial, this.ControllerContext);
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