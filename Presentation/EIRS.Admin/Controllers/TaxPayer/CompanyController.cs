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
    public class CompanyController : BaseController
    {
        public ActionResult List()
        {
            return View();
        }

        [HttpPost]
        public JsonResult LoadData()
        {
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
                sbWhereCondition.Append(" AND ISNULL(CompanyRIN,'') LIKE @CompanyRIN");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["CompanyName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(CompanyName,'') LIKE @CompanyName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["TIN"]))
            {
                sbWhereCondition.Append(" AND ISNULL(TIN,'') LIKE @TIN");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["MobileNum1"]))
            {
                sbWhereCondition.Append(" AND ISNULL(MobileNumber1,'') LIKE @MobileNumber1");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["MobileNum2"]))
            {
                sbWhereCondition.Append(" AND ISNULL(MobileNumber2,'') LIKE @MobileNumber2");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["EmailAddress1"]))
            {
                sbWhereCondition.Append(" AND ISNULL(EmailAddress1,'') LIKE @EmailAddress1");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["EmailAddress2"]))
            {
                sbWhereCondition.Append(" AND ISNULL(EmailAddress2,'') LIKE @EmailAddress2");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["TaxOffice"]))
            {
                sbWhereCondition.Append(" AND ISNULL(TaxOfficeName,'') LIKE @TaxOfficeName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["TaxPayerType"]))
            {
                sbWhereCondition.Append(" AND ISNULL(TaxPayerTypeName,'') LIKE @TaxPayerTypeName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["EconomicActivities"]))
            {
                sbWhereCondition.Append(" AND ISNULL(EconomicActivitiesName,'') LIKE @EconomicActivitiesName");
            }

            if (!string.IsNullOrWhiteSpace(Request.Form["NotificationMethod"]))
            {
                sbWhereCondition.Append(" AND ISNULL(NotificationMethodName,'') LIKE @NotificationMethodName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["ActiveText"]))
            {
                sbWhereCondition.Append(" AND CASE WHEN ISNULL(comp.Active,0) = 0 THEN 'In Active' ELSE 'Active' END LIKE @ActiveText");
            }


            if (!string.IsNullOrEmpty(vFilter))
            {
                sbWhereCondition.Append(" AND ( ISNULL(CompanyRIN,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(CompanyName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(TIN,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(MobileNumber1,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(MobileNumber2,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(EmailAddress1,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(EmailAddress2,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(TaxOfficeName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(TaxPayerTypeName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(EconomicActivitiesName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(NotificationMethodName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR CASE WHEN ISNULL(comp.Active,0) = 0 THEN 'In Active' ELSE 'Active' END LIKE @MainFilter)");
            }

            Company mObjCompany = new Company()
            {
                WhereCondition = sbWhereCondition.ToString(),
                OrderBy = vSortColumn,
                OrderByDirection = vSortColumnDir,
                PageNumber = (TrynParse.parseInt(vStart) / TrynParse.parseInt(vLength)) + 1,
                PageSize = TrynParse.parseInt(vLength),

                CompanyRIN = !string.IsNullOrWhiteSpace(Request.Form["RIN"]) ? "%" + Request.Form["RIN"].Trim() + "%" : TrynParse.parseString(Request.Form["RIN"]),
                CompanyName = !string.IsNullOrWhiteSpace(Request.Form["companyName"]) ? "%" + Request.Form["companyName"].Trim() + "%" : TrynParse.parseString(Request.Form["companyName"]),
                TIN = !string.IsNullOrWhiteSpace(Request.Form["TIN"]) ? "%" + Request.Form["TIN"].Trim() + "%" : TrynParse.parseString(Request.Form["TIN"]),
                MobileNumber1 = !string.IsNullOrWhiteSpace(Request.Form["MobileNum1"]) ? "%" + Request.Form["MobileNum1"].Trim() + "%" : TrynParse.parseString(Request.Form["MobileNum1"]),
                MobileNumber2 = !string.IsNullOrWhiteSpace(Request.Form["MobileNum2"]) ? "%" + Request.Form["MobileNum2"].Trim() + "%" : TrynParse.parseString(Request.Form["MobileNum2"]),
                EmailAddress1 = !string.IsNullOrWhiteSpace(Request.Form["EmailAddress1"]) ? "%" + Request.Form["EmailAddress1"].Trim() + "%" : TrynParse.parseString(Request.Form["EmailAddress1"]),
                EmailAddress2 = !string.IsNullOrWhiteSpace(Request.Form["EmailAddress2"]) ? "%" + Request.Form["EmailAddress2"].Trim() + "%" : TrynParse.parseString(Request.Form["EmailAddress2"]),
                TaxOfficeName = !string.IsNullOrWhiteSpace(Request.Form["TaxOffice"]) ? "%" + Request.Form["TaxOffice"].Trim() + "%" : TrynParse.parseString(Request.Form["TaxOffice"]),
                TaxPayerTypeName = !string.IsNullOrWhiteSpace(Request.Form["TaxPayerType"]) ? "%" + Request.Form["TaxPayerType"].Trim() + "%" : TrynParse.parseString(Request.Form["TaxPayerType"]),
                EconomicActivitiesName = !string.IsNullOrWhiteSpace(Request.Form["EconomicActivities"]) ? "%" + Request.Form["EconomicActivities"].Trim() + "%" : TrynParse.parseString(Request.Form["EconomicActivities"]),
                NotificationMethodName = !string.IsNullOrWhiteSpace(Request.Form["NotificationMethod"]) ? "%" + Request.Form["NotificationMethod"].Trim() + "%" : TrynParse.parseString(Request.Form["NotificationMethod"]),
                ActiveText = !string.IsNullOrWhiteSpace(Request.Form["ActiveText"]) ? "%" + Request.Form["ActiveText"].Trim() + "%" : TrynParse.parseString(Request.Form["ActiveText"]),
                MainFilter = !string.IsNullOrWhiteSpace(vFilter) ? "%" + vFilter.Trim() + "%" : vFilter
            };

            IDictionary<string, object> dcData = new BLCompany().BL_SearchCompany(mObjCompany);
            IList<usp_SearchCompany_Result> lstCompany = (IList<usp_SearchCompany_Result>)dcData["CompanyList"];

            return Json(new
            {
                draw = vDraw,
                recordsFiltered = dcData["FilteredRecords"],
                recordsTotal = dcData["TotalRecords"],
                data = lstCompany
            }, JsonRequestBehavior.AllowGet);
        }


        public void UI_FillDropDown(CompanyViewModel pObjCompanyViewModel = null)
        {
            if (pObjCompanyViewModel != null)
                pObjCompanyViewModel.TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies;
            else if (pObjCompanyViewModel == null)
                pObjCompanyViewModel = new CompanyViewModel();

            UI_FillTaxOfficeDropDown(new Tax_Offices() { intStatus = 1, IncludeTaxOfficeIds = pObjCompanyViewModel.TaxOfficeID.ToString() });
            UI_FillTaxPayerTypeDropDown(new TaxPayer_Types() { intStatus = 1, IncludeTaxPayerTypeIds = pObjCompanyViewModel.TaxPayerTypeID.ToString() }, (int)EnumList.TaxPayerType.Companies);
            UI_FillEconomicActivitiesDropDown(new Economic_Activities() { intStatus = 1, IncludeEconomicActivitiesIds = pObjCompanyViewModel.EconomicActivitiesID.ToString(), TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies });
            UI_FillNotificationMethodDropDown(new Notification_Method() { intStatus = 1, IncludeNotificationMethodIds = pObjCompanyViewModel.NotificationMethodID.ToString() });
        }

        public ActionResult Add()
        {
            UI_FillDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(CompanyViewModel pObjCompanyModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown(pObjCompanyModel);
                return View(pObjCompanyModel);
            }
            else
            {
                Company mObjCompany = new Company()
                {
                    CompanyID = 0,
                    CompanyName = pObjCompanyModel.CompanyName.Trim(),
                    TIN = pObjCompanyModel.TIN,
                    MobileNumber1 = pObjCompanyModel.MobileNumber1,
                    MobileNumber2 = pObjCompanyModel.MobileNumber2,
                    EmailAddress1 = pObjCompanyModel.EmailAddress1,
                    EmailAddress2 = pObjCompanyModel.EmailAddress2,
                    TaxOfficeID = pObjCompanyModel.TaxOfficeID,
                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies,
                    EconomicActivitiesID = pObjCompanyModel.EconomicActivitiesID,
                    NotificationMethodID = pObjCompanyModel.NotificationMethodID,
                    ContactAddress = pObjCompanyModel.ContactAddress,
                    CACRegistrationNumber = pObjCompanyModel.CACRegistrationNumber,
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse<Company> mObjResponse = new BLCompany().BL_InsertUpdateCompany(mObjCompany);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "Company");
                    }
                    else
                    {
                        UI_FillDropDown(pObjCompanyModel);
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjCompanyModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillDropDown(pObjCompanyModel);
                    ViewBag.Message = "Error occurred while saving company";
                    return View(pObjCompanyModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Company mObjCompany = new Company()
                {
                    CompanyID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(mObjCompany);

                if (mObjCompanyData != null)
                {
                    CompanyViewModel mObjCompanyModelView = new CompanyViewModel()
                    {
                        CompanyID = mObjCompanyData.CompanyID.GetValueOrDefault(),
                        CompanyRIN = mObjCompanyData.CompanyRIN,
                        CompanyName = mObjCompanyData.CompanyName.Trim(),
                        TIN = mObjCompanyData.TIN,
                        MobileNumber1 = mObjCompanyData.MobileNumber1,
                        MobileNumber2 = mObjCompanyData.MobileNumber2,
                        EmailAddress1 = mObjCompanyData.EmailAddress1,
                        EmailAddress2 = mObjCompanyData.EmailAddress2,
                        TaxOfficeID = mObjCompanyData.TaxOfficeID,
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies,
                        EconomicActivitiesID = mObjCompanyData.EconomicActivitiesID.GetValueOrDefault(),
                        NotificationMethodID = mObjCompanyData.NotificationMethodID.GetValueOrDefault(),
                        ContactAddress = mObjCompanyData.ContactAddress,
                        CACRegistrationNumber = mObjCompanyData.CACRegistrationNumber,
                        Active = mObjCompanyData.Active.GetValueOrDefault(),
                    };

                    UI_FillDropDown(mObjCompanyModelView);
                    return View(mObjCompanyModelView);
                }
                else
                {
                    return RedirectToAction("List", "Company");
                }
            }
            else
            {
                return RedirectToAction("List", "Company");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(CompanyViewModel pObjCompanyModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown(pObjCompanyModel);
                return View(pObjCompanyModel);
            }
            else
            {
                Company mObjCompany = new Company()
                {
                    CompanyID = pObjCompanyModel.CompanyID,
                    CompanyName = pObjCompanyModel.CompanyName,
                    TIN = pObjCompanyModel.TIN,
                    MobileNumber1 = pObjCompanyModel.MobileNumber1,
                    MobileNumber2 = pObjCompanyModel.MobileNumber2,
                    EmailAddress1 = pObjCompanyModel.EmailAddress1,
                    EmailAddress2 = pObjCompanyModel.EmailAddress2,
                    TaxOfficeID = pObjCompanyModel.TaxOfficeID,
                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies,
                    EconomicActivitiesID = pObjCompanyModel.EconomicActivitiesID,
                    NotificationMethodID = pObjCompanyModel.NotificationMethodID,
                    ContactAddress = pObjCompanyModel.ContactAddress,
                    CACRegistrationNumber = pObjCompanyModel.CACRegistrationNumber,
                    Active = pObjCompanyModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse<Company> mObjResponse = new BLCompany().BL_InsertUpdateCompany(mObjCompany);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "Company");
                    }
                    else
                    {
                        UI_FillDropDown(pObjCompanyModel);
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjCompanyModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillDropDown(pObjCompanyModel);
                    ViewBag.Message = "Error occurred while saving company";
                    return View(pObjCompanyModel);
                }
            }
        }


        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Company mObjCompany = new Company()
                {
                    CompanyID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(mObjCompany);

                if (mObjCompanyData != null)
                {
                    CompanyViewModel mObjCompanyModelView = new CompanyViewModel()
                    {
                        CompanyID = mObjCompanyData.CompanyID.GetValueOrDefault(),
                        CompanyRIN = mObjCompanyData.CompanyRIN,
                        CompanyName = mObjCompanyData.CompanyName,
                        TIN = mObjCompanyData.TIN,
                        MobileNumber1 = mObjCompanyData.MobileNumber1,
                        MobileNumber2 = mObjCompanyData.MobileNumber2,
                        EmailAddress1 = mObjCompanyData.EmailAddress1,
                        EmailAddress2 = mObjCompanyData.EmailAddress2,
                        TaxOfficeName = mObjCompanyData.TaxOfficeName,
                        TaxPayerTypeName = mObjCompanyData.TaxPayerTypeName,
                        EconomicActivitiesName = mObjCompanyData.EconomicActivitiesName,
                        NotificationMethodName = mObjCompanyData.NotificationMethodName,
                        ContactAddress = mObjCompanyData.ContactAddress,
                        ActiveText = mObjCompanyData.ActiveText
                    };

                    return View(mObjCompanyModelView);
                }
                else
                {
                    return RedirectToAction("List", "Company");
                }
            }
            else
            {
                return RedirectToAction("List", "Company");
            }
        }

        public JsonResult UpdateStatus(Company pObjCompanyData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjCompanyData.CompanyID != 0)
            {
                FuncResponse mObjFuncResponse = new BLCompany().BL_UpdateStatus(pObjCompanyData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                //if (mObjFuncResponse.Success)
                //{
                //    dcResponse["CompanyList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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
                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies
                };

                IList<usp_GetTaxPayerAssetList_Result> lstTaxPayerAsset = new BLTaxPayerAsset().BL_GetTaxPayerAssetList(mObjTaxPayerAsset);
                usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(new Company() { intStatus = 2, CompanyID = id.GetValueOrDefault() });
                if (lstTaxPayerAsset != null)
                {
                    ViewBag.TaxPayerID = id;
                    ViewBag.TaxPayerRIN = name;
                    ViewBag.TaxPayerName = mObjCompanyData.CompanyName;
                    return View(lstTaxPayerAsset);
                }
                else
                {
                    return RedirectToAction("List", "Company");
                }
            }
            else
            {
                return RedirectToAction("List", "Company");
            }
        }

        public void UI_FillDropDown(TaxPayerAssetViewModel pObjTPAModel)
        {
            UI_FillAssetTypeDropDown(new Asset_Types() { intStatus = 1 });
            UI_FillTaxPayerTypeDropDown(new TaxPayer_Types() { intStatus = 1 }, (int)EnumList.TaxPayerType.Companies);
            UI_FillTaxPayerRoleDropDown(new TaxPayer_Roles() { intStatus = 1, TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies });
        }

        public ActionResult AddAsset(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(new Company() { intStatus = 2, CompanyID = id.GetValueOrDefault() });
                TaxPayerAssetViewModel mObjTaxPayerAssetModel = new TaxPayerAssetViewModel()
                {
                    TaxPayerID = id.GetValueOrDefault(),
                    TaxPayerName = mObjCompanyData.CompanyName,
                    TaxPayerRIN = name,
                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies
                };

                UI_FillDropDown(mObjTaxPayerAssetModel);
                return View(mObjTaxPayerAssetModel);
            }
            else
            {
                return RedirectToAction("AssetList", "Company");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddAsset(TaxPayerAssetViewModel pObjAssetModel)
        {
            if (!ModelState.IsValid)
            {
                pObjAssetModel.TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies;
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
                            TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies,
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
                return RedirectToAction("AssetList", "Company", new { id = pObjAssetModel.TaxPayerID, name = pObjAssetModel.TaxPayerName });
            }
        }


        public ActionResult AssessmentList(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                IList<usp_GetAssessmentList_Result> lstAssessment = new BLAssessment().BL_GetAssessmentList(new Assessment() { TaxPayerID = id.GetValueOrDefault(), TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies, IntStatus = 2 });
                usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(new Company() { intStatus = 2, CompanyID = id.GetValueOrDefault() });
                ViewBag.TaxPayerID = id;
                ViewBag.TaxPayerRIN = name;
                ViewBag.TaxPayerName = mObjCompanyData.CompanyName;
                return View(lstAssessment);
            }
            else
            {
                return RedirectToAction("List", "Company");
            }
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


        public ActionResult AddAssessment(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(new Company() { intStatus = 2, CompanyID = id.GetValueOrDefault() });

                AssessmentViewModel mObjAssessmentModel = new AssessmentViewModel()
                {
                    TaxPayerID = id.GetValueOrDefault(),
                    TaxPayerRIN = name,
                    TaxPayerName = mObjCompanyData.CompanyName,
                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies,
                    SettlementDuedate = CommUtil.GetCurrentDateTime()
                };

                SessionManager.lstAssessmentItem = new List<Assessment_AssessmentItem>();
                SessionManager.lstAssessmentRule = new List<Assessment_AssessmentRule>();
                UI_FillAssessmentDropDown();
                return View(mObjAssessmentModel);
            }
            else
            {
                return RedirectToAction("List", "Company");
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
                                return RedirectToAction("AssessmentList", "Company", new { id = pObjAssessmentModel.TaxPayerID, name = pObjAssessmentModel.TaxPayerRIN });

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

        public ActionResult EditAssessment(int? id, string name, int? astid)
        {
            if (id.GetValueOrDefault() > 0 && astid.GetValueOrDefault() > 0)
            {
                BLAssessment mObjBLAssessment = new BLAssessment();
                usp_GetAssessmentList_Result mObjAssessmentData = mObjBLAssessment.BL_GetAssessmentDetails(new Assessment() { AssessmentID = astid.GetValueOrDefault(), IntStatus = 2 });

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
                    return RedirectToAction("List", "Company");
                }
            }
            else
            {
                return RedirectToAction("List", "Company");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult EditAssessment(AssessmentViewModel pObjAssessmentModel)
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
                                return RedirectToAction("AssessmentList", "Company", new { id = pObjAssessmentModel.TaxPayerID, name = pObjAssessmentModel.TaxPayerRIN });

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

        public ActionResult AssessmentDetails(int? id, string name, int? astid)
        {
            if (id.GetValueOrDefault() > 0 && astid.GetValueOrDefault() > 0)
            {
                BLAssessment mObjBLAssessment = new BLAssessment();

                usp_GetAssessmentList_Result mObjAssessmentDetails = mObjBLAssessment.BL_GetAssessmentDetails(new Assessment() { AssessmentID = astid.GetValueOrDefault(), TaxPayerID = id.GetValueOrDefault(), TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies, IntStatus = 1 });

                if (mObjAssessmentDetails != null)
                {
                    IList<usp_GetAssessmentRuleItemList_Result> lstAssessmentItems = mObjBLAssessment.BL_GetAssessmentRuleItem(mObjAssessmentDetails.AssessmentID.GetValueOrDefault());

                    ViewBag.AssessmentItemList = lstAssessmentItems;

                    return View(mObjAssessmentDetails);
                }
                else
                {
                    return RedirectToAction("AssessmentList", "Company", new { id = id, name = name });
                }
            }
            else
            {
                return RedirectToAction("AssessmentList", "Company", new { id = id, name = name });
            }

        }

        public ActionResult ServiceBillList(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ServiceBill mObjServiceBill = new ServiceBill()
                {
                    IntStatus = 2,
                    TaxPayerID = id.GetValueOrDefault(),
                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies
                };

                IList<usp_GetServiceBillList_Result> lstServiceBill = new BLServiceBill().BL_GetServiceBillList(mObjServiceBill);
                usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(new Company() { intStatus = 2, CompanyID = id.GetValueOrDefault() });
                ViewBag.TaxPayerID = id;
                ViewBag.TaxPayerRIN = name;
                ViewBag.TaxPayerName = mObjCompanyData.CompanyName;
                return View(lstServiceBill);
            }
            else
            {
                return RedirectToAction("List", "Company");
            }
        }

        public void UI_FillServiceBillDropDown(ServiceBillViewModel pObjServiceBillModel = null)
        {
            IList<DropDownListResult> lstMDAService = new BLMDAService().BL_GetMDAServiceDropDownList(new MDA_Services() { IntStatus = 1 });
            ViewBag.MDAServiceList = new SelectList(lstMDAService, "id", "text");

            ViewBag.MAPServiceBillServiceList = SessionManager.lstServiceBillService;
            ViewBag.MAPServiceBillItemList = SessionManager.lstServiceBillItem.Where(t => t.MDAService_RowID == 0).ToList();
        }

        public ActionResult AddServiceBill(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(new Company() { intStatus = 2, CompanyID = id.GetValueOrDefault() });

                ServiceBillViewModel mObjServiceBillModel = new ServiceBillViewModel()
                {
                    TaxPayerID = id.GetValueOrDefault(),
                    TaxPayerRIN = name,
                    TaxPayerName = mObjCompanyData.CompanyName,
                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies,
                    SettlementDuedate = CommUtil.GetCurrentDateTime()
                };

                SessionManager.lstServiceBillItem = new List<ServiceBill_MDAServiceItem>();
                SessionManager.lstServiceBillService = new List<ServiceBill_MDAService>();

                UI_FillServiceBillDropDown();
                return View(mObjServiceBillModel);
            }
            else
            {
                return RedirectToAction("List", "Company");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddServiceBill(ServiceBillViewModel pObjServiceBillModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillServiceBillDropDown(pObjServiceBillModel);
                return View(pObjServiceBillModel);
            }
            else
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    IList<ServiceBill_MDAServiceItem> lstServiceBillItems = SessionManager.lstServiceBillItem ?? new List<ServiceBill_MDAServiceItem>();
                    IList<ServiceBill_MDAService> lstServiceBillServices = SessionManager.lstServiceBillService ?? new List<ServiceBill_MDAService>();

                    int IntServiceBillServiceCount = lstServiceBillServices.Where(t => t.intTrack != EnumList.Track.DELETE).Count();

                    if (IntServiceBillServiceCount == 0)
                    {
                        UI_FillServiceBillDropDown(pObjServiceBillModel);
                        ModelState.AddModelError("ServiceBillService-error", "Please Add Atleast One MDA Service");
                        Transaction.Current.Rollback();
                        return View(pObjServiceBillModel);
                    }
                    else
                    {

                        BLServiceBill mObjBLServiceBill = new BLServiceBill();

                        ServiceBill mObjServiceBill = new ServiceBill()
                        {
                            ServiceBillID = 0,
                            TaxPayerID = pObjServiceBillModel.TaxPayerID,
                            TaxPayerTypeID = pObjServiceBillModel.TaxPayerTypeID,
                            ServiceBillAmount = lstServiceBillItems.Count > 0 ? lstServiceBillItems.Where(t => t.intTrack != EnumList.Track.DELETE).Sum(t => t.ServiceAmount) : 0,
                            ServiceBillDate = CommUtil.GetCurrentDateTime(),
                            SettlementDueDate = pObjServiceBillModel.SettlementDuedate,
                            SettlementStatusID = 2,
                            Active = true,
                            CreatedBy = SessionManager.SystemUserID,
                            CreatedDate = CommUtil.GetCurrentDateTime()
                        };

                        try
                        {

                            FuncResponse<ServiceBill> mObjServiceBillResponse = mObjBLServiceBill.BL_InsertUpdateServiceBill(mObjServiceBill);

                            if (mObjServiceBillResponse.Success)
                            {
                                //Adding MDA Service

                                foreach (ServiceBill_MDAService mObjSBS in lstServiceBillServices.Where(t => t.intTrack != EnumList.Track.DELETE))
                                {
                                    MAP_ServiceBill_MDAService mObjMDAService = new MAP_ServiceBill_MDAService()
                                    {
                                        ServiceBillID = mObjServiceBillResponse.AdditionalData.ServiceBillID,
                                        MDAServiceID = mObjSBS.MDAServiceID,
                                        ServiceAmount = mObjSBS.ServiceAmount,
                                        ServiceBillYear = mObjSBS.TaxYear,
                                        CreatedBy = SessionManager.SystemUserID,
                                        CreatedDate = CommUtil.GetCurrentDateTime()
                                    };

                                    FuncResponse<MAP_ServiceBill_MDAService> mObjSBSResponse = mObjBLServiceBill.BL_InsertUpdateMDAService(mObjMDAService);

                                    if (mObjSBSResponse.Success)
                                    {
                                        IList<MAP_ServiceBill_MDAServiceItem> lstInsertServiceBillDetail = new List<MAP_ServiceBill_MDAServiceItem>();

                                        foreach (ServiceBill_MDAServiceItem mObjServiceBillItemDetail in lstServiceBillItems.Where(t => t.MDAService_RowID == mObjSBS.RowID))
                                        {
                                            MAP_ServiceBill_MDAServiceItem mObjSIDetail = new MAP_ServiceBill_MDAServiceItem()
                                            {
                                                SBSID = mObjSBSResponse.AdditionalData.SBSID,
                                                MDAServiceItemID = mObjServiceBillItemDetail.MDAServiceItemID,
                                                ServiceBaseAmount = mObjServiceBillItemDetail.ServiceBaseAmount,
                                                ServiceAmount = mObjServiceBillItemDetail.ServiceAmount,
                                                Percentage = mObjServiceBillItemDetail.Percentage,
                                                PaymentStatusID = (int)EnumList.PaymentStatus.Due,
                                                CreatedBy = SessionManager.SystemUserID,
                                                CreatedDate = CommUtil.GetCurrentDateTime(),
                                            };

                                            lstInsertServiceBillDetail.Add(mObjSIDetail);
                                        }

                                        FuncResponse mObjSDResponse = mObjBLServiceBill.BL_InsertServiceBillItem(lstInsertServiceBillDetail);

                                        if (!mObjSDResponse.Success)
                                        {
                                            throw (mObjSDResponse.Exception);
                                        }
                                    }
                                    else
                                    {
                                        throw (mObjSBSResponse.Exception);
                                    }
                                }

                                scope.Complete();
                                FlashMessage.Info(mObjServiceBillResponse.Message);
                                return RedirectToAction("ServiceBillList", "Company", new { id = pObjServiceBillModel.TaxPayerID, name = pObjServiceBillModel.TaxPayerRIN });
                            }
                            else
                            {
                                UI_FillServiceBillDropDown(pObjServiceBillModel);
                                ViewBag.Message = mObjServiceBillResponse.Message;
                                Transaction.Current.Rollback();
                                return View(pObjServiceBillModel);
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorSignal.FromCurrentContext().Raise(ex);
                            UI_FillServiceBillDropDown(pObjServiceBillModel);
                            ViewBag.Message = "Error occurred while saving service bill";
                            Transaction.Current.Rollback();
                            return View(pObjServiceBillModel);
                        }
                    }
                }
            }
        }

        public ActionResult EditServiceBill(int? id, string name, int? sbillid)
        {
            if (id.GetValueOrDefault() > 0)
            {
                BLServiceBill mObjBLServiceBill = new BLServiceBill();
                usp_GetServiceBillList_Result mObjServiceBillData = mObjBLServiceBill.BL_GetServiceBillDetails(new ServiceBill() { ServiceBillID = sbillid.GetValueOrDefault(), IntStatus = 2 });

                if (mObjServiceBillData != null)
                {
                    ServiceBillViewModel mObjServiceBillModel = new ServiceBillViewModel()
                    {
                        ServiceBillID = mObjServiceBillData.ServiceBillID.GetValueOrDefault(),
                        TaxPayerName = mObjServiceBillData.TaxPayerName,
                        TaxPayerRIN = mObjServiceBillData.TaxPayerRIN,
                        SettlementDuedate = mObjServiceBillData.SettlementDueDate.Value,
                        TaxPayerID = mObjServiceBillData.TaxPayerID.GetValueOrDefault(),
                        TaxPayerTypeID = mObjServiceBillData.TaxPayerTypeID.GetValueOrDefault(),
                    };

                    IList<ServiceBill_MDAService> lstServiceBillService = new List<ServiceBill_MDAService>();
                    IList<ServiceBill_MDAServiceItem> lstServiceBillItem = new List<ServiceBill_MDAServiceItem>();

                    IList<MAP_ServiceBill_MDAService> lstMAPServiceBillServices = mObjBLServiceBill.BL_GetMDAServices(mObjServiceBillData.ServiceBillID.GetValueOrDefault());
                    IList<MAP_ServiceBill_MDAServiceItem> lstMAPServiceBillItems;
                    foreach (var item in lstMAPServiceBillServices)
                    {
                        ServiceBill_MDAService ServiceBill_MDAService = new ServiceBill_MDAService()
                        {
                            RowID = lstServiceBillService.Count + 1,
                            TablePKID = item.SBSID,
                            MDAServiceID = item.MDAServiceID.GetValueOrDefault(),
                            ServiceAmount = item.ServiceAmount.GetValueOrDefault(),
                            MDAServiceName = item.MDA_Services.MDAServiceCode + " - " + item.MDA_Services.MDAServiceName,
                            TaxYear = item.MDA_Services.TaxYear.GetValueOrDefault(),
                            intTrack = EnumList.Track.EXISTING,
                        };

                        lstServiceBillService.Add(ServiceBill_MDAService);

                        lstMAPServiceBillItems = mObjBLServiceBill.BL_GetServiceBillItems(item.SBSID);

                        foreach (var subitem in lstMAPServiceBillItems)
                        {
                            ServiceBill_MDAServiceItem mObjServiceBillItem = new ServiceBill_MDAServiceItem()
                            {
                                RowID = lstServiceBillItem.Count + 1,
                                MDAService_RowID = ServiceBill_MDAService.RowID,
                                TablePKID = subitem.SBSIID,
                                MDAServiceItemID = subitem.MDAServiceItemID.GetValueOrDefault(),
                                MDAServiceItemName = subitem.MDA_Service_Items.MDAServiceItemName,
                                MDAServiceItemReferenceNo = subitem.MDA_Service_Items.MDAServiceItemReferenceNo,
                                ServiceAmount = subitem.ServiceAmount.GetValueOrDefault(),
                                ServiceBaseAmount = subitem.ServiceBaseAmount.GetValueOrDefault(),
                                ComputationID = subitem.MDA_Service_Items.ComputationID,
                                Percentage = subitem.Percentage.GetValueOrDefault(),
                                intTrack = EnumList.Track.EXISTING
                            };

                            lstServiceBillItem.Add(mObjServiceBillItem);
                        }
                    }



                    SessionManager.lstServiceBillItem = lstServiceBillItem;
                    SessionManager.lstServiceBillService = lstServiceBillService;
                    UI_FillServiceBillDropDown();

                    return View(mObjServiceBillModel);
                }
                else
                {
                    return RedirectToAction("List", "Company");
                }
            }
            else
            {
                return RedirectToAction("List", "Company");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult EditServiceBill(ServiceBillViewModel pObjServiceBillModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillServiceBillDropDown(pObjServiceBillModel);
                return View(pObjServiceBillModel);
            }
            else
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    IList<ServiceBill_MDAServiceItem> lstServiceBillItems = SessionManager.lstServiceBillItem ?? new List<ServiceBill_MDAServiceItem>();
                    IList<ServiceBill_MDAService> lstServiceBillServices = SessionManager.lstServiceBillService ?? new List<ServiceBill_MDAService>();

                    int IntServiceBillServiceCount = lstServiceBillServices.Where(t => t.intTrack != EnumList.Track.DELETE).Count();

                    if (IntServiceBillServiceCount == 0)
                    {
                        UI_FillServiceBillDropDown(pObjServiceBillModel);
                        ModelState.AddModelError("ServiceBillService-error", "Please Add Atleast One MDA Service");
                        Transaction.Current.Rollback();
                        return View(pObjServiceBillModel);
                    }
                    else
                    {

                        BLServiceBill mObjBLServiceBill = new BLServiceBill();

                        ServiceBill mObjServiceBill = new ServiceBill()
                        {
                            ServiceBillID = pObjServiceBillModel.ServiceBillID,
                            TaxPayerID = pObjServiceBillModel.TaxPayerID,
                            TaxPayerTypeID = pObjServiceBillModel.TaxPayerTypeID,
                            ServiceBillAmount = lstServiceBillItems.Count > 0 ? lstServiceBillItems.Where(t => t.intTrack != EnumList.Track.DELETE).Sum(t => t.ServiceAmount) : 0,
                            ServiceBillDate = CommUtil.GetCurrentDateTime(),
                            SettlementDueDate = pObjServiceBillModel.SettlementDuedate,
                            SettlementStatusID = 2,
                            Active = true,
                            CreatedBy = SessionManager.SystemUserID,
                            CreatedDate = CommUtil.GetCurrentDateTime()
                        };

                        try
                        {

                            FuncResponse<ServiceBill> mObjServiceBillResponse = mObjBLServiceBill.BL_InsertUpdateServiceBill(mObjServiceBill);

                            if (mObjServiceBillResponse.Success)
                            {
                                //Adding MDA Service

                                foreach (ServiceBill_MDAService mObjSBS in lstServiceBillServices)
                                {
                                    if (mObjSBS.intTrack == EnumList.Track.INSERT)
                                    {
                                        MAP_ServiceBill_MDAService mObjMDAService = new MAP_ServiceBill_MDAService()
                                        {
                                            ServiceBillID = mObjServiceBillResponse.AdditionalData.ServiceBillID,
                                            MDAServiceID = mObjSBS.MDAServiceID,
                                            ServiceAmount = mObjSBS.ServiceAmount,
                                            ServiceBillYear = mObjSBS.TaxYear,
                                            CreatedBy = SessionManager.SystemUserID,
                                            CreatedDate = CommUtil.GetCurrentDateTime()
                                        };

                                        FuncResponse<MAP_ServiceBill_MDAService> mObjSBSResponse = mObjBLServiceBill.BL_InsertUpdateMDAService(mObjMDAService);

                                        if (mObjSBSResponse.Success)
                                        {
                                            IList<MAP_ServiceBill_MDAServiceItem> lstInsertServiceBillDetail = new List<MAP_ServiceBill_MDAServiceItem>();

                                            foreach (ServiceBill_MDAServiceItem mObjServiceBillItemDetail in lstServiceBillItems.Where(t => t.MDAService_RowID == mObjSBS.RowID))
                                            {
                                                MAP_ServiceBill_MDAServiceItem mObjSIDetail = new MAP_ServiceBill_MDAServiceItem()
                                                {
                                                    SBSID = mObjSBSResponse.AdditionalData.SBSID,
                                                    MDAServiceItemID = mObjServiceBillItemDetail.MDAServiceItemID,
                                                    ServiceBaseAmount = mObjServiceBillItemDetail.ServiceBaseAmount,
                                                    ServiceAmount = mObjServiceBillItemDetail.ServiceAmount,
                                                    Percentage = mObjServiceBillItemDetail.Percentage,
                                                    PaymentStatusID = (int)EnumList.PaymentStatus.Due,
                                                    CreatedBy = SessionManager.SystemUserID,
                                                    CreatedDate = CommUtil.GetCurrentDateTime(),
                                                };

                                                lstInsertServiceBillDetail.Add(mObjSIDetail);
                                            }

                                            FuncResponse mObjSDResponse = mObjBLServiceBill.BL_InsertServiceBillItem(lstInsertServiceBillDetail);

                                            if (!mObjSDResponse.Success)
                                            {
                                                throw (mObjSDResponse.Exception);
                                            }
                                        }
                                        else
                                        {
                                            throw (mObjSBSResponse.Exception);
                                        }
                                    }
                                    else if (mObjSBS.intTrack == EnumList.Track.DELETE)
                                    {
                                        FuncResponse mObjMSResponse = mObjBLServiceBill.BL_DeleteMDAService(mObjSBS.TablePKID);

                                        if (!mObjMSResponse.Success)
                                        {
                                            throw (mObjMSResponse.Exception);
                                        }
                                    }
                                }

                                scope.Complete();
                                FlashMessage.Info(mObjServiceBillResponse.Message);
                                return RedirectToAction("ServiceBillList", "Company", new { id = pObjServiceBillModel.TaxPayerID, name = pObjServiceBillModel.TaxPayerRIN });
                            }
                            else
                            {
                                UI_FillServiceBillDropDown(pObjServiceBillModel);
                                ViewBag.Message = mObjServiceBillResponse.Message;
                                Transaction.Current.Rollback();
                                return View(pObjServiceBillModel);
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorSignal.FromCurrentContext().Raise(ex);
                            UI_FillServiceBillDropDown(pObjServiceBillModel);
                            ViewBag.Message = "Error occurred while updating service bill";
                            Transaction.Current.Rollback();
                            return View(pObjServiceBillModel);
                        }
                    }
                }
            }
        }


        public ActionResult ServiceBillDetails(int? id, string name, int? sbillid)
        {
            if (id.GetValueOrDefault() > 0 && sbillid.GetValueOrDefault() > 0)
            {
                BLServiceBill mObjBLServiceBill = new BLServiceBill();

                usp_GetServiceBillList_Result mObjServiceBillDetails = mObjBLServiceBill.BL_GetServiceBillDetails(new ServiceBill() { ServiceBillID = sbillid.GetValueOrDefault(), TaxPayerID = id.GetValueOrDefault(), TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies, IntStatus = 1 });

                if (mObjServiceBillDetails != null)
                {
                    IList<usp_GetServiceBillItemList_Result> lstServiceBillItems = mObjBLServiceBill.BL_GetServiceBillItem(mObjServiceBillDetails.ServiceBillID.GetValueOrDefault());

                    ViewBag.ServiceBillItemList = lstServiceBillItems;

                    return View(mObjServiceBillDetails);
                }
                else
                {
                    return RedirectToAction("ServiceBillList", "Company", new { id = id, name = name });
                }
            }
            else
            {
                return RedirectToAction("ServiceBillList", "Company", new { id = id, name = name });
            }
        }

        public ActionResult SettlementList(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Settlement mObjSettlement = new Settlement()
                {
                    TaxPayerID = id.GetValueOrDefault(),
                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies
                };

                IList<usp_GetSettlementList_Result> lstSettlement = new BLSettlement().BL_GetSettlementList(mObjSettlement);

                usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(new Company() { intStatus = 2, CompanyID = id.GetValueOrDefault() });
                ViewBag.TaxPayerID = id;
                ViewBag.TaxPayerRIN = name;
                ViewBag.TaxPayerName = mObjCompanyData.CompanyName;

                return View(lstSettlement);
            }
            else
            {
                return RedirectToAction("List", "Company");
            }
        }

        public ActionResult GenerateSettlement(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                IList<DropDownListResult> lstSettlementType = new List<DropDownListResult>();
                lstSettlementType.Add(new DropDownListResult() { id = 1, text = "Assessment" });
                lstSettlementType.Add(new DropDownListResult() { id = 2, text = "Service Bill" });

                ViewBag.SettlementTypeList = new SelectList(lstSettlementType, "id", "text");

                usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(new Company() { intStatus = 2, CompanyID = id.GetValueOrDefault() });
                ViewBag.TaxPayerID = id;
                ViewBag.TaxPayerRIN = name;
                ViewBag.TaxPayerName = mObjCompanyData.CompanyName;

                return View();
            }
            else
            {
                return RedirectToAction("List", "Company");
            }
        }

        public ActionResult AddSettlement(int? id, string name, int? stype, int? asid)
        {
            if (id.GetValueOrDefault() > 0)
            {
                usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(new Company() { intStatus = 2, CompanyID = id.GetValueOrDefault() });
                if (stype == 1)
                {
                    BLAssessment mObjBLAssessment = new BLAssessment();

                    usp_GetAssessmentList_Result mObjAssessmentData = mObjBLAssessment.BL_GetAssessmentDetails(new Assessment() { AssessmentID = asid.GetValueOrDefault(), IntStatus = 1 });

                    if (mObjAssessmentData != null)
                    {
                        IList<usp_GetAssessmentRuleItemList_Result> lstAssessmentRuleItem = mObjBLAssessment.BL_GetAssessmentRuleItem(asid.GetValueOrDefault());

                        IList<DropDownListResult> lstSettlementMethod = mObjBLAssessment.BL_GetSettlementMethodAssessmentRuleBased(asid.GetValueOrDefault());
                        ViewBag.SettlementMethodList = new SelectList(lstSettlementMethod, "id", "text");


                        SettlementViewModel mObjSettlementModel = new SettlementViewModel()
                        {
                            AssessmentID = asid.GetValueOrDefault(),
                            SettlementDate = CommUtil.GetCurrentDateTime(),
                            TaxPayerID = id.GetValueOrDefault(),
                            TaxPayerRIN = name,
                            TaxPayerName = mObjCompanyData.CompanyName,
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
                else if (stype == 2)
                {
                    BLServiceBill mObjBLServiceBill = new BLServiceBill();

                    usp_GetServiceBillList_Result mObjServiceBillData = mObjBLServiceBill.BL_GetServiceBillDetails(new ServiceBill() { ServiceBillID = asid.GetValueOrDefault(), IntStatus = 1 });

                    if (mObjServiceBillData != null)
                    {
                        IList<usp_GetServiceBillItemList_Result> lstServiceBillItem = mObjBLServiceBill.BL_GetServiceBillItem(asid.GetValueOrDefault());

                        IList<DropDownListResult> lstSettlementMethod = mObjBLServiceBill.BL_GetSettlementMethodMDAServiceBased(asid.GetValueOrDefault());
                        ViewBag.SettlementMethodList = new SelectList(lstSettlementMethod, "id", "text");


                        SettlementViewModel mObjSettlementModel = new SettlementViewModel()
                        {
                            ServiceBillID = asid.GetValueOrDefault(),
                            SettlementDate = CommUtil.GetCurrentDateTime(),
                            TaxPayerID = id.GetValueOrDefault(),
                            TaxPayerRIN = name,
                            TaxPayerName = mObjCompanyData.CompanyName,
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
                    return RedirectToAction("List", "Company");
                }
            }
            else
            {
                return RedirectToAction("List", "Company");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddSettlement(SettlementViewModel pObjSettlementModel)
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
                else if (pObjSettlementModel.ServiceBillID != 0)
                {
                    BLServiceBill mObjBLServiceBill = new BLServiceBill();
                    usp_GetServiceBillList_Result mObjServiceBillData = mObjBLServiceBill.BL_GetServiceBillDetails(new ServiceBill() { ServiceBillID = pObjSettlementModel.ServiceBillID, IntStatus = 1 });

                    IList<DropDownListResult> lstSettlementMethod = mObjBLServiceBill.BL_GetSettlementMethodMDAServiceBased(pObjSettlementModel.ServiceBillID);
                    ViewBag.SettlementMethodList = new SelectList(lstSettlementMethod, "id", "text");

                    ViewBag.ServiceBillData = mObjServiceBillData;
                }
                else
                {
                    return RedirectToAction("List", "Company");
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

                                        if (pObjSettlementModel.ServiceBillID != 0)
                                        {
                                            mObjSettlementItem.SBSIID = mObjSAI.TBPKID;
                                        }

                                        if (pObjSettlementModel.AssessmentID != 0)
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
                                            else if (pObjSettlementModel.ServiceBillID != 0)
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
                                else if (pObjSettlementModel.ServiceBillID != 0)
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
                                }

                                scope.Complete();
                                FlashMessage.Info(mObjSettlementResponse.Message);
                                return RedirectToAction("SettlementList", "Company", new { id = pObjSettlementModel.TaxPayerID, name = pObjSettlementModel.TaxPayerRIN });

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

        public ActionResult SettlementDetails(int? id, string name, int? smtid)
        {
            if (id.GetValueOrDefault() > 0 && smtid.GetValueOrDefault() > 0)
            {
                BLSettlement mObjBLSettlement = new BLSettlement();
                usp_GetSettlementList_Result mObjSettlementDetails = mObjBLSettlement.BL_GetSettlementDetails(new Settlement() { SettlementID = smtid.GetValueOrDefault(), TaxPayerID = id.GetValueOrDefault(), TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies });

                if (mObjSettlementDetails != null)
                {
                    IList<usp_GetSettlementItemList_Result> lstSettlementItems = mObjBLSettlement.BL_GetSettlementItemList(mObjSettlementDetails.SettlementID.GetValueOrDefault());

                    ViewBag.SettlementItemList = lstSettlementItems;

                    return View(mObjSettlementDetails);
                }
                else
                {
                    return RedirectToAction("List", "Company");
                }
            }
            else
            {
                return RedirectToAction("List", "Company");
            }
        }



        public ActionResult AddressInformationList(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                BLCompany mObjCompany = new BLCompany();

                IList<usp_GetCompanyAddressInformation_Result> lstAddressInformation = mObjCompany.BL_GetAddressInformation(new Company() { CompanyID = id.GetValueOrDefault() });
                usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(new Company() { intStatus = 2, CompanyID = id.GetValueOrDefault() });
                ViewBag.TaxPayerID = id;
                ViewBag.TaxPayerRIN = name;
                ViewBag.TaxPayerName = mObjCompanyData.CompanyName;
                return View(lstAddressInformation);
            }
            else
            {
                return RedirectToAction("List", "Company");
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
                usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(new Company() { intStatus = 2, CompanyID = id.GetValueOrDefault() });
                AddressInformationViewModel mObjAddressInformationModel = new AddressInformationViewModel()
                {
                    TaxPayerID = id.GetValueOrDefault(),
                    TaxPayerRIN = name,
                    TaxPayerName = mObjCompanyData.CompanyName
                };

                UI_FillAddressDropDown();
                return View(mObjAddressInformationModel);
            }
            else
            {
                return RedirectToAction("AddressInformationList", "Company", new { id = id, name = name });
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
                MAP_Company_AddressInformation mObjAddressInformation = new MAP_Company_AddressInformation()
                {
                    AddressTypeID = pObjAddressModel.AddressTypeID,
                    BuildingID = pObjAddressModel.BuildingID,
                    CompanyID = pObjAddressModel.TaxPayerID,
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLCompany().BL_InsertAddressInformation(mObjAddressInformation);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("AddressInformationList", "Company", new { id = pObjAddressModel.TaxPayerID, name = pObjAddressModel.TaxPayerRIN });
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
                pObjAssetData.TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies;
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
                pObjAssetData.TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies;
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
            IList<DropDownListResult> lstAssetType = new BLTaxPayerRole().BL_GetAssetTypeDropDownList(new TaxPayer_Roles() { intStatus = 1, TaxPayerRoleID = TaxPayerRoleID, TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies });
            return Json(lstAssetType, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProfileInformation(int CompanyID)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (CompanyID != 0)
            {
                IList<usp_GetProfileInformation_Result> lstProfileInformation = new BLTaxPayerAsset().BL_GetTaxPayerProfileInformation((int)EnumList.TaxPayerType.Companies, CompanyID);
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

        public JsonResult GetAssessmentRuleInformation(int CompanyID)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (CompanyID != 0)
            {
                IList<usp_GetAssessmentRuleInformation_Result> lstAssessmentRuleInformation = new BLTaxPayerAsset().BL_GetTaxPayerAssessmentRuleInformation((int)EnumList.TaxPayerType.Companies, CompanyID);
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

        public JsonResult RemoveAddressInformation(MAP_Company_AddressInformation pObjAddressInformation)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjAddressInformation.CAIID != 0)
            {
                FuncResponse<IList<usp_GetCompanyAddressInformation_Result>> mObjFuncResponse = new BLCompany().BL_RemoveAddressInformation(pObjAddressInformation);
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

        public JsonResult GetAssessmentList(int TaxPayerID)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            IList<usp_GetAssessmentList_Result> lstAssessment = new BLAssessment().BL_GetAssessmentList(new Assessment() { IntStatus = 1, TaxPayerID = TaxPayerID, TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies });

            dcResponse["success"] = true;
            dcResponse["AssessmentList"] = CommUtil.RenderPartialToString("_BindAssessmentTable_SingleSelect", lstAssessment, this.ControllerContext);

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetServiceBillList(int TaxPayerID)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            IList<usp_GetServiceBillList_Result> lstServiceBill = new BLServiceBill().BL_GetServiceBillList(new ServiceBill() { IntStatus = 1, TaxPayerID = TaxPayerID, TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies });

            dcResponse["success"] = true;
            dcResponse["ServiceBillList"] = CommUtil.RenderPartialToString("_BindServiceTable_SingleSelect", lstServiceBill, this.ControllerContext);

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }
    }
}