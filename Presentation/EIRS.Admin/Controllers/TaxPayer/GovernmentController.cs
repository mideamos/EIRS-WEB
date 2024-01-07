using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using Vereyon.Web;
using Elmah;
using System.Linq.Dynamic;
using System.Text;

namespace EIRS.Admin.Controllers
{
    public class GovernmentController : BaseController
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

            if (!string.IsNullOrWhiteSpace(Request.Form["RIN"]))
            {
                sbWhereCondition.Append(" AND ISNULL(GovernmentRIN,'') LIKE @GovernmentRIN");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["TIN"]))
            {
                sbWhereCondition.Append(" AND ISNULL(TIN,'') LIKE @TIN");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["GovernmentName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(GovernmentName,'') LIKE @GovernmentName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["GovernmentTypeName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(GovernmentTypeName,'') LIKE @GovernmentTypeName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["TaxOffice"]))
            {
                sbWhereCondition.Append(" AND ISNULL(TaxOfficeName,'') LIKE @TaxOfficeName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["TaxPayerType"]))
            {
                sbWhereCondition.Append(" AND ISNULL(TaxPayerTypeName,'') LIKE @TaxPayerTypeName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["ContactName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(ContactName,'') LIKE @ContactName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["ContactEmail"]))
            {
                sbWhereCondition.Append(" AND ISNULL(ContactEmail,'') LIKE @ContactEmail");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["ContactNumber"]))
            {
                sbWhereCondition.Append(" AND ISNULL(ContactNumber,'') LIKE @ContactNumber");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["NotificationMethod"]))
            {
                sbWhereCondition.Append(" AND ISNULL(NotificationMethodName,'') LIKE @NotificationMethodName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["ActiveText"]))
            {
                sbWhereCondition.Append(" AND CASE WHEN ISNULL(gov.Active,0) = 0 THEN 'In Active' ELSE 'Active' END LIKE @ActiveText");
            }


            if (!string.IsNullOrEmpty(vFilter))
            {
                sbWhereCondition.Append(" AND ( ISNULL(GovernmentRIN,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(TIN,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(GovernmentName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(GovernmentTypeName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(TaxOfficeName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(TaxPayerTypeName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(ContactName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(ContactEmail,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(NotificationMethodName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR CASE WHEN ISNULL(gov.Active,0) = 0 THEN 'In Active' ELSE 'Active' END LIKE @MainFilter)");
            }

            Government mObjGovernment = new Government()
            {
                WhereCondition = sbWhereCondition.ToString(),
                OrderBy = vSortColumn,
                OrderByDirection = vSortColumnDir,
                PageNumber = (TrynParse.parseInt(vStart) / TrynParse.parseInt(vLength)) + 1,
                PageSize = TrynParse.parseInt(vLength),

                GovernmentRIN = !string.IsNullOrWhiteSpace(Request.Form["RIN"]) ? "%" + Request.Form["RIN"].Trim() + "%" : TrynParse.parseString(Request.Form["RIN"]),
                GovernmentName = !string.IsNullOrWhiteSpace(Request.Form["GovernmentName"]) ? "%" + Request.Form["GovernmentName"].Trim() + "%" : TrynParse.parseString(Request.Form["GovernmentName"]),
                TIN = !string.IsNullOrWhiteSpace(Request.Form["TIN"]) ? "%" + Request.Form["TIN"].Trim() + "%" : TrynParse.parseString(Request.Form["TIN"]),
                GovernmentTypeName = !string.IsNullOrWhiteSpace(Request.Form["GovernmentTypeName"]) ? "%" + Request.Form["GovernmentTypeName"].Trim() + "%" : TrynParse.parseString(Request.Form["GovernmentTypeName"]),
                TaxOfficeName = !string.IsNullOrWhiteSpace(Request.Form["TaxOffice"]) ? "%" + Request.Form["TaxOffice"].Trim() + "%" : TrynParse.parseString(Request.Form["TaxOffice"]),
                TaxPayerTypeName = !string.IsNullOrWhiteSpace(Request.Form["TaxPayerType"]) ? "%" + Request.Form["TaxPayerType"].Trim() + "%" : TrynParse.parseString(Request.Form["TaxPayerType"]),
                ContactName = !string.IsNullOrWhiteSpace(Request.Form["ContactName"]) ? "%" + Request.Form["ContactName"].Trim() + "%" : TrynParse.parseString(Request.Form["ContactName"]),
                ContactEmail = !string.IsNullOrWhiteSpace(Request.Form["ContactEmail"]) ? "%" + Request.Form["ContactEmail"].Trim() + "%" : TrynParse.parseString(Request.Form["ContactEmail"]),
                ContactNumber = !string.IsNullOrWhiteSpace(Request.Form["ContactNumber"]) ? "%" + Request.Form["ContactNumber"].Trim() + "%" : TrynParse.parseString(Request.Form["ContactNumber"]),
                NotificationMethodName = !string.IsNullOrWhiteSpace(Request.Form["NotificationMethod"]) ? "%" + Request.Form["NotificationMethod"].Trim() + "%" : TrynParse.parseString(Request.Form["NotificationMethod"]),
                ActiveText = !string.IsNullOrWhiteSpace(Request.Form["ActiveText"]) ? "%" + Request.Form["ActiveText"].Trim() + "%" : TrynParse.parseString(Request.Form["ActiveText"]),
                MainFilter = !string.IsNullOrWhiteSpace(vFilter) ? "%" + vFilter.Trim() + "%" : vFilter
            };

            IDictionary<string, object> dcData = new BLGovernment().BL_SearchGovernment(mObjGovernment);
            IList<usp_SearchGovernment_Result> lstGovernment = (IList<usp_SearchGovernment_Result>)dcData["GovernmentList"];

            return Json(new
            {
                draw = vDraw,
                recordsFiltered = dcData["FilteredRecords"],
                recordsTotal = dcData["TotalRecords"],
                data = lstGovernment
            }, JsonRequestBehavior.AllowGet);
        }

        public void UI_FillDropDown(GovernmentViewModel pObjGovernmentViewModel = null)
        {
            if (pObjGovernmentViewModel != null)
                pObjGovernmentViewModel.TaxPayerTypeID = (int)EnumList.TaxPayerType.Government;
            else if (pObjGovernmentViewModel == null)
                pObjGovernmentViewModel = new GovernmentViewModel();

            UI_FillTaxOfficeDropDown(new Tax_Offices() { intStatus = 1, IncludeTaxOfficeIds = pObjGovernmentViewModel.TaxOfficeID.ToString() });
            UI_FillTaxPayerTypeDropDown(new TaxPayer_Types() { intStatus = 1, IncludeTaxPayerTypeIds = pObjGovernmentViewModel.TaxPayerTypeID.ToString() }, (int)EnumList.TaxPayerType.Government);
            UI_FillGovernmentTypeDropDown(new Government_Types() { intStatus = 1, IncludeGovernmentTypeIds = pObjGovernmentViewModel.GovernmentTypeID.ToString() });
            UI_FillNotificationMethodDropDown(new Notification_Method() { intStatus = 1, IncludeNotificationMethodIds = pObjGovernmentViewModel.NotificationMethodID.ToString() });
        }

        public ActionResult Add()
        {
            UI_FillDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(GovernmentViewModel pObjGovernmentModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown(pObjGovernmentModel);
                return View(pObjGovernmentModel);
            }
            else
            {
                Government mObjGovernment = new Government()
                {
                    GovernmentID = 0,
                    GovernmentName = pObjGovernmentModel.GovernmentName.Trim(),
                    GovernmentTypeID = pObjGovernmentModel.GovernmentTypeID,
                    TIN = pObjGovernmentModel.TIN,
                    TaxOfficeID = pObjGovernmentModel.TaxOfficeID,
                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Government,
                    ContactNumber = pObjGovernmentModel.ContactNumber,
                    ContactEmail = pObjGovernmentModel.ContactEmail,
                    ContactName = pObjGovernmentModel.ContactName,
                    NotificationMethodID = pObjGovernmentModel.NotificationMethodID,
                    ContactAddress = pObjGovernmentModel.ContactAddress,
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse<Government> mObjResponse = new BLGovernment().BL_InsertUpdateGovernment(mObjGovernment);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "Government");
                    }
                    else
                    {
                        UI_FillDropDown(pObjGovernmentModel);
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjGovernmentModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillDropDown(pObjGovernmentModel);
                    ViewBag.Message = "Error occurred while saving government";
                    return View(pObjGovernmentModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Government mObjGovernment = new Government()
                {
                    GovernmentID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetGovernmentList_Result mObjGovernmentData = new BLGovernment().BL_GetGovernmentDetails(mObjGovernment);

                if (mObjGovernmentData != null)
                {
                    GovernmentViewModel mObjGovernmentModelView = new GovernmentViewModel()
                    {
                        GovernmentID = mObjGovernmentData.GovernmentID.GetValueOrDefault(),
                        GovernmentRIN = mObjGovernmentData.GovernmentRIN,
                        TIN = mObjGovernmentData.TIN,
                        GovernmentName = mObjGovernmentData.GovernmentName.Trim(),
                        GovernmentTypeID = mObjGovernmentData.GovernmentTypeID.GetValueOrDefault(),
                        TaxOfficeID = mObjGovernmentData.TaxOfficeID,
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Government,
                        ContactNumber = mObjGovernmentData.ContactNumber,
                        ContactEmail = mObjGovernmentData.ContactEmail,
                        ContactName = mObjGovernmentData.ContactName,
                        NotificationMethodID = mObjGovernmentData.NotificationMethodID.GetValueOrDefault(),
                        ContactAddress = mObjGovernmentData.ContactAddress,
                        Active = mObjGovernmentData.Active.GetValueOrDefault(),
                    };

                    UI_FillDropDown(mObjGovernmentModelView);
                    return View(mObjGovernmentModelView);
                }
                else
                {
                    return RedirectToAction("List", "Government");
                }
            }
            else
            {
                return RedirectToAction("List", "Government");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(GovernmentViewModel pObjGovernmentModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown(pObjGovernmentModel);
                return View(pObjGovernmentModel);
            }
            else
            {
                Government mObjGovernment = new Government()
                {
                    GovernmentID = pObjGovernmentModel.GovernmentID,
                    GovernmentName = pObjGovernmentModel.GovernmentName.Trim(),
                    GovernmentTypeID = pObjGovernmentModel.GovernmentTypeID,
                    TIN = pObjGovernmentModel.TIN,
                    TaxOfficeID = pObjGovernmentModel.TaxOfficeID,
                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Government,
                    ContactNumber = pObjGovernmentModel.ContactNumber,
                    ContactEmail = pObjGovernmentModel.ContactEmail,
                    ContactName = pObjGovernmentModel.ContactName,
                    NotificationMethodID = pObjGovernmentModel.NotificationMethodID,
                    Active = pObjGovernmentModel.Active,
                    ContactAddress = pObjGovernmentModel.ContactAddress,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse<Government> mObjResponse = new BLGovernment().BL_InsertUpdateGovernment(mObjGovernment);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "Government");
                    }
                    else
                    {
                        UI_FillDropDown(pObjGovernmentModel);
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjGovernmentModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillDropDown(pObjGovernmentModel);
                    ViewBag.Message = "Error occurred while saving government";
                    return View(pObjGovernmentModel);
                }
            }
        }


        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Government mObjGovernment = new Government()
                {
                    GovernmentID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetGovernmentList_Result mObjGovernmentData = new BLGovernment().BL_GetGovernmentDetails(mObjGovernment);

                if (mObjGovernmentData != null)
                {
                    GovernmentViewModel mObjGovernmentModelView = new GovernmentViewModel()
                    {
                        GovernmentID = mObjGovernmentData.GovernmentID.GetValueOrDefault(),
                        GovernmentRIN = mObjGovernmentData.GovernmentRIN,
                        TIN = mObjGovernmentData.TIN,
                        GovernmentName = mObjGovernmentData.GovernmentName,
                        TaxOfficeName = mObjGovernmentData.TaxOfficeName,
                        TaxPayerTypeName = mObjGovernmentData.TaxPayerTypeName,
                        GovernmentTypeName = mObjGovernmentData.GovernmentTypeName,
                        ContactNumber = mObjGovernmentData.ContactNumber,
                        ContactEmail = mObjGovernmentData.ContactEmail,
                        ContactName = mObjGovernmentData.ContactName,
                        NotificationMethodName = mObjGovernmentData.NotificationMethodName,
                        ContactAddress = mObjGovernmentData.ContactAddress,
                        ActiveText = mObjGovernmentData.ActiveText
                    };

                    return View(mObjGovernmentModelView);
                }
                else
                {
                    return RedirectToAction("List", "Government");
                }
            }
            else
            {
                return RedirectToAction("List", "Government");
            }
        }

        public JsonResult UpdateStatus(Government pObjGovernmentData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjGovernmentData.GovernmentID != 0)
            {
                FuncResponse mObjFuncResponse = new BLGovernment().BL_UpdateStatus(pObjGovernmentData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                //if (mObjFuncResponse.Success)
                //{
                //    dcResponse["GovernmentList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
                //}

            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Action";
            }



            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }


        public ActionResult AssetList(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                {
                    TaxPayerID = id.GetValueOrDefault(),
                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Government
                };

                IList<usp_GetTaxPayerAssetList_Result> lstTaxPayerAsset = new BLTaxPayerAsset().BL_GetTaxPayerAssetList(mObjTaxPayerAsset);
                usp_GetGovernmentList_Result mObjGovernmentData = new BLGovernment().BL_GetGovernmentDetails(new Government() { intStatus = 2, GovernmentID = id.GetValueOrDefault() });
                if (lstTaxPayerAsset != null)
                {
                    ViewBag.TaxPayerID = id;
                    ViewBag.TaxPayerRIN = name;
                    ViewBag.TaxPayerName = mObjGovernmentData.GovernmentName;
                    return View(lstTaxPayerAsset);
                }
                else
                {
                    return RedirectToAction("List", "Government");
                }
            }
            else
            {
                return RedirectToAction("List", "Government");
            }
        }

        public void UI_FillDropDown(TaxPayerAssetViewModel pObjTPAModel)
        {
            UI_FillAssetTypeDropDown(new Asset_Types() { intStatus = 1 });
            UI_FillTaxPayerTypeDropDown(new TaxPayer_Types() { intStatus = 1 }, (int)EnumList.TaxPayerType.Government);
            UI_FillTaxPayerRoleDropDown(new TaxPayer_Roles() { intStatus = 1, TaxPayerTypeID = (int)EnumList.TaxPayerType.Government });
        }

        public ActionResult AddAsset(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                usp_GetGovernmentList_Result mObjGovernmentData = new BLGovernment().BL_GetGovernmentDetails(new Government() { intStatus = 2, GovernmentID = id.GetValueOrDefault() });
                TaxPayerAssetViewModel mObjTaxPayerAssetModel = new TaxPayerAssetViewModel()
                {
                    TaxPayerID = id.GetValueOrDefault(),
                    TaxPayerName = mObjGovernmentData.GovernmentName,
                    TaxPayerRIN = name,
                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Government
                };

                UI_FillDropDown(mObjTaxPayerAssetModel);
                return View(mObjTaxPayerAssetModel);
            }
            else
            {
                return RedirectToAction("AssetList", "Government");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddAsset(TaxPayerAssetViewModel pObjAssetModel)
        {
            if (!ModelState.IsValid)
            {
                pObjAssetModel.TaxPayerTypeID = (int)EnumList.TaxPayerType.Government;
                UI_FillDropDown(pObjAssetModel);
                return View(pObjAssetModel);
            }
            else
            {
                BLTaxPayerAsset mobjBLTaxPayerAsset = new BLTaxPayerAsset();
                string[] strAssetIds = pObjAssetModel.AssetIds.Split(',');

                foreach (var vAssetID in strAssetIds)
                {
                    if (!string.IsNullOrWhiteSpace(vAssetID))
                    {
                        MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                        {
                            AssetTypeID = pObjAssetModel.AssetTypeID,
                            TaxPayerTypeID = (int)EnumList.TaxPayerType.Government,
                            TaxPayerRoleID = pObjAssetModel.TaxPayerRoleID,
                            TaxPayerID = pObjAssetModel.TaxPayerID,
                            AssetID = TrynParse.parseInt(vAssetID),
                            Active = true,
                            CreatedBy = SessionManager.SystemUserID,
                            CreatedDate = CommUtil.GetCurrentDateTime()
                        };

                        FuncResponse mObjResponse = mobjBLTaxPayerAsset.BL_InsertTaxPayerAsset(mObjTaxPayerAsset);
                    }
                }

                FlashMessage.Info("Asset Linked Successfully");
                return RedirectToAction("AssetList", "Government", new { id = pObjAssetModel.TaxPayerID, name = pObjAssetModel.TaxPayerName });
            }
        }


        public ActionResult AssessmentList(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                IList<usp_GetAssessmentList_Result> lstAssessment = new BLAssessment().BL_GetAssessmentList(new Assessment() { TaxPayerID = id.GetValueOrDefault(), TaxPayerTypeID = (int)EnumList.TaxPayerType.Government, IntStatus = 2 });
                usp_GetGovernmentList_Result mObjGovernmentData = new BLGovernment().BL_GetGovernmentDetails(new Government() { intStatus = 2, GovernmentID = id.GetValueOrDefault() });
                ViewBag.TaxPayerID = id;
                ViewBag.TaxPayerRIN = name;
                ViewBag.TaxPayerName = mObjGovernmentData.GovernmentName;
                return View(lstAssessment);
            }
            else
            {
                return RedirectToAction("List", "Government");
            }
        }


        public void UI_FillAssessmentDropDown(AssessmentViewModel pObjAssessmentModel = null)
        {
            UI_FillAssetTypeDropDown();
            IList<DropDownListResult> lstDropDowns = new List<DropDownListResult>();
            ViewBag.AssetList = new SelectList(lstDropDowns, "id", "text");
            ViewBag.ProfileList = new SelectList(lstDropDowns, "id", "text");
            ViewBag.AssessmentRuleList = new SelectList(lstDropDowns, "id", "text");

            ViewBag.SAssessmentRuleList = SessionManager.lstAssessmentRule;
            ViewBag.AssessmentItemList = SessionManager.lstAssessmentItem;
        }


        public ActionResult AddAssessment(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                usp_GetGovernmentList_Result mObjGovernmentData = new BLGovernment().BL_GetGovernmentDetails(new Government() { intStatus = 2, GovernmentID = id.GetValueOrDefault() });
                AssessmentViewModel mObjAssessmentModel = new AssessmentViewModel()
                {
                    TaxPayerID = id.GetValueOrDefault(),
                    TaxPayerRIN = name,
                    TaxPayerName = mObjGovernmentData.GovernmentName,
                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Government,
                    SettlementDuedate = CommUtil.GetCurrentDateTime()
                };

                SessionManager.lstAssessmentItem = new List<Assessment_AssessmentItem>();
                SessionManager.lstAssessmentRule = new List<Assessment_AssessmentRule>();

                UI_FillAssessmentDropDown();
                return View(mObjAssessmentModel);
            }
            else
            {
                return RedirectToAction("List", "Government");
            }


        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddAssessment(AssessmentViewModel pObjAssessmentModel)
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
                            AssessmentID = 0,
                            TaxPayerID = pObjAssessmentModel.TaxPayerID,
                            TaxPayerTypeID = pObjAssessmentModel.TaxPayerTypeID,
                            AssessmentAmount = lstAssessmentItems.Count > 0 ? lstAssessmentItems.Where(t => t.intTrack != EnumList.Track.DELETE).Sum(t => t.TaxAmount) : 0,
                            AssessmentDate = CommUtil.GetCurrentDateTime(),
                            SettlementDueDate = pObjAssessmentModel.SettlementDuedate,
                            SettlementStatusID = 2,
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

                                foreach (Assessment_AssessmentRule mObjAAR in lstAssessmentRules)
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
                                return RedirectToAction("AssessmentList", "Government", new { id = pObjAssessmentModel.TaxPayerID, name = pObjAssessmentModel.TaxPayerRIN });

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

        public ActionResult AssessmentDetails(int? id, string name, int? astid)
        {
            if (id.GetValueOrDefault() > 0 && astid.GetValueOrDefault() > 0)
            {
                BLAssessment mObjBLAssessment = new BLAssessment();

                usp_GetAssessmentList_Result mObjAssessmentDetails = mObjBLAssessment.BL_GetAssessmentDetails(new Assessment() { AssessmentID = astid.GetValueOrDefault(), TaxPayerID = id.GetValueOrDefault(), TaxPayerTypeID = (int)EnumList.TaxPayerType.Government, IntStatus = 1 });

                if (mObjAssessmentDetails != null)
                {
                    IList<usp_GetAssessmentRuleItemList_Result> lstAssessmentItems = mObjBLAssessment.BL_GetAssessmentRuleItem(mObjAssessmentDetails.AssessmentID.GetValueOrDefault());

                    ViewBag.AssessmentItemList = lstAssessmentItems;

                    return View(mObjAssessmentDetails);
                }
                else
                {
                    return RedirectToAction("AssessmentList", "Government", new { id = id, name = name });
                }
            }
            else
            {
                return RedirectToAction("AssessmentList", "Government", new { id = id, name = name });
            }

        }


        public ActionResult AddressInformationList(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                BLGovernment mObjGovernment = new BLGovernment();

                IList<usp_GetGovernmentAddressInformation_Result> lstAddressInformation = mObjGovernment.BL_GetAddressInformation(new Government() { GovernmentID = id.GetValueOrDefault() });
                usp_GetGovernmentList_Result mObjGovernmentData = new BLGovernment().BL_GetGovernmentDetails(new Government() { intStatus = 2, GovernmentID = id.GetValueOrDefault() });
                ViewBag.TaxPayerID = id;
                ViewBag.TaxPayerRIN = name;
                ViewBag.TaxPayerName = mObjGovernmentData.GovernmentName;
                return View(lstAddressInformation);
            }
            else
            {
                return RedirectToAction("List", "Government");
            }
        }

        public void UI_FillAddressDropDown()
        {
            UI_FillAddressTypeDropDown();

            IList<usp_GetBuildingList_Result> lstBuilding = new BLBuilding().BL_GetBuildingList(new Building() { intStatus = 1 });
            ViewBag.BuildingList = lstBuilding;
        }

        public ActionResult AddAddressInformation(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                usp_GetGovernmentList_Result mObjGovernmentData = new BLGovernment().BL_GetGovernmentDetails(new Government() { intStatus = 2, GovernmentID = id.GetValueOrDefault() });
                AddressInformationViewModel mObjAddressInformationModel = new AddressInformationViewModel()
                {
                    TaxPayerID = id.GetValueOrDefault(),
                    TaxPayerRIN = name,
                    TaxPayerName = mObjGovernmentData.GovernmentName
                };

                UI_FillAddressDropDown();
                return View(mObjAddressInformationModel);
            }
            else
            {
                return RedirectToAction("AddressInformationList", "Government", new { id = id, name = name });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddAddressInformation(AddressInformationViewModel pObjAddressModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillAddressDropDown();
                return View(pObjAddressModel);
            }
            else
            {
                MAP_Government_AddressInformation mObjAddressInformation = new MAP_Government_AddressInformation()
                {
                    AddressTypeID = pObjAddressModel.AddressTypeID,
                    BuildingID = pObjAddressModel.BuildingID,
                    GovernmentID = pObjAddressModel.TaxPayerID,
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLGovernment().BL_InsertAddressInformation(mObjAddressInformation);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("AddressInformationList", "Government", new { id = pObjAddressModel.TaxPayerID, name = pObjAddressModel.TaxPayerRIN });
                    }
                    else
                    {
                        UI_FillAddressDropDown();
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjAddressModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillAddressDropDown();
                    ViewBag.Message = "Error occurred while saving address information";
                    return View();
                }
            }
        }

        public JsonResult GetAssetList(int AssetTypeID)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (AssetTypeID == (int)EnumList.AssetTypes.Building)
            {
                Building mObjBuilding = new Building()
                {
                    intStatus = 2
                };

                IList<usp_GetBuildingList_Result> lstBuilding = new BLBuilding().BL_GetBuildingList(mObjBuilding);
                dcResponse["success"] = true;
                dcResponse["AssetList"] = CommUtil.RenderPartialToString("_BindBuildingTable_SingleSelect", lstBuilding, this.ControllerContext);
            }
            else if (AssetTypeID == (int)EnumList.AssetTypes.Vehicles)
            {
                Vehicle mObjVehicle = new Vehicle()
                {
                    intStatus = 2
                };

                IList<usp_GetVehicleList_Result> lstVehicle = new BLVehicle().BL_GetVehicleList(mObjVehicle);
                dcResponse["success"] = true;
                dcResponse["AssetList"] = CommUtil.RenderPartialToString("_BindVehicleTable", lstVehicle, this.ControllerContext);
            }
            else if (AssetTypeID == (int)EnumList.AssetTypes.Business)
            {
                Business mObjBusiness = new Business()
                {
                    intStatus = 2
                };

                IList<usp_GetBusinessListNewTy_Result> lstBusiness = new BLBusiness().BL_GetBusinessList(mObjBusiness);
                dcResponse["success"] = true;
                dcResponse["AssetList"] = CommUtil.RenderPartialToString("_BindBusinessTable", lstBusiness, this.ControllerContext);
            }
            else if (AssetTypeID == (int)EnumList.AssetTypes.Land)
            {
                Land mObjLand = new Land()
                {
                    intStatus = 2
                };

                IList<usp_GetLandList_Result> lstLand = new BLLand().BL_GetLandList(mObjLand);
                dcResponse["success"] = true;
                dcResponse["AssetList"] = CommUtil.RenderPartialToString("_BindLandTable", lstLand, this.ControllerContext);
            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Request";
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateAssetStatus(MAP_TaxPayer_Asset pObjAssetData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjAssetData.TPAID != 0)
            {
                pObjAssetData.TaxPayerTypeID = (int)EnumList.TaxPayerType.Government;
                pObjAssetData.IntStatus = 2;
                FuncResponse<IList<usp_GetTaxPayerAssetList_Result>> mObjFuncResponse = new BLTaxPayerAsset().BL_UpdateTaxPayerAssetStatus(pObjAssetData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["AssetList"] = CommUtil.RenderPartialToString("_BindAssetTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
                }

            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Action";
            }



            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RemoveAsset(MAP_TaxPayer_Asset pObjAssetData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjAssetData.TPAID != 0)
            {
                pObjAssetData.TaxPayerTypeID = (int)EnumList.TaxPayerType.Government;
                FuncResponse<IList<usp_GetTaxPayerAssetList_Result>> mObjFuncResponse = new BLTaxPayerAsset().BL_RemoveTaxPayerAsset(pObjAssetData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["AssetList"] = CommUtil.RenderPartialToString("_BindAssetTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
                }

            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Action";
            }



            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAssetDetails(int TPAID)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            MAP_TaxPayer_Asset mObjAsset = new BLTaxPayerAsset().BL_GetTaxPayerAssetDetails(TPAID);

            if (mObjAsset != null)
            {
                dcResponse["AssetTypeID"] = mObjAsset.AssetTypeID;
                dcResponse["success"] = true;
                if (mObjAsset.AssetTypeID == (int)EnumList.AssetTypes.Building)
                {
                    dcResponse["AssetDetails"] = new BLBuilding().BL_GetBuildingDetails(new Building() { intStatus = 2, BuildingID = mObjAsset.AssetID.GetValueOrDefault() });
                }
                else if (mObjAsset.AssetTypeID == (int)EnumList.AssetTypes.Vehicles)
                {
                    dcResponse["AssetDetails"] = new BLVehicle().BL_GetVehicleDetails(new Vehicle() { intStatus = 2, VehicleID = mObjAsset.AssetID.GetValueOrDefault() });
                }
                else if (mObjAsset.AssetTypeID == (int)EnumList.AssetTypes.Business)
                {
                    dcResponse["AssetDetails"] = new BLBusiness().BL_GetBusinessDetails(new Business() { intStatus = 2, BusinessID = mObjAsset.AssetID.GetValueOrDefault() });
                }
                else if (mObjAsset.AssetTypeID == (int)EnumList.AssetTypes.Land)
                {
                    dcResponse["AssetDetails"] = new BLLand().BL_GetLandDetails(new Land() { intStatus = 2, LandID = mObjAsset.AssetID.GetValueOrDefault() });
                }
            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Response";
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAssetTypeList(int TaxPayerRoleID)
        {
            IList<DropDownListResult> lstAssetType = new BLTaxPayerRole().BL_GetAssetTypeDropDownList(new TaxPayer_Roles() { intStatus = 1, TaxPayerRoleID = TaxPayerRoleID, TaxPayerTypeID = (int)EnumList.TaxPayerType.Government });
            return Json(lstAssetType, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProfileInformation(int GovernmentID)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (GovernmentID != 0)
            {
                IList<usp_GetProfileInformation_Result> lstProfileInformation = new BLTaxPayerAsset().BL_GetTaxPayerProfileInformation((int)EnumList.TaxPayerType.Government, GovernmentID);
                dcResponse["ProfileInformationList"] = CommUtil.RenderPartialToString("_BindProfileInformationTable", lstProfileInformation, this.ControllerContext);
                dcResponse["success"] = true;
            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Action";
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAssessmentRuleInformation(int GovernmentID)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (GovernmentID != 0)
            {
                IList<usp_GetAssessmentRuleInformation_Result> lstAssessmentRuleInformation = new BLTaxPayerAsset().BL_GetTaxPayerAssessmentRuleInformation((int)EnumList.TaxPayerType.Government, GovernmentID);
                dcResponse["AssessmentRuleInformationList"] = CommUtil.RenderPartialToString("_BindAssessmentRuleInformationTable", lstAssessmentRuleInformation, this.ControllerContext);
                dcResponse["success"] = true;
            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Action";
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RemoveAddressInformation(MAP_Government_AddressInformation pObjAddressInformation)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjAddressInformation.GAIID != 0)
            {
                FuncResponse<IList<usp_GetGovernmentAddressInformation_Result>> mObjFuncResponse = new BLGovernment().BL_RemoveAddressInformation(pObjAddressInformation);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["AddressInformationList"] = CommUtil.RenderPartialToString("_BindAddressTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
                }

            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Action";
            }



            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAddressInformationDetails(int BuildingID)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            usp_GetBuildingList_Result mObjBuildingDetails = new BLBuilding().BL_GetBuildingDetails(new Building() { intStatus = 2, BuildingID = BuildingID });

            if (mObjBuildingDetails != null)
            {
                dcResponse["success"] = true;
                dcResponse["BuildingDetails"] = mObjBuildingDetails;
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