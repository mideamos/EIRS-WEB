using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;
using Elmah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Transactions;
using System.Web.Mvc;
using Vereyon.Web;

namespace EIRS.Admin.Controllers
{
    public class IndividualController : BaseController
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
                sbWhereCondition.Append(" AND ISNULL(IndividualRIN,'') LIKE @IndividualRIN");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["GenderName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(GenderName,'') LIKE @GenderName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["TitleName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(TitleName,'') LIKE @TitleName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["FirstName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(FirstName,'') LIKE @FirstName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["MiddleName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(MiddleName,'') LIKE @MiddleName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["LastName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(LastName,'') LIKE @LastName");
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
            if (!string.IsNullOrWhiteSpace(Request.Form["BiometricDetails"]))
            {
                sbWhereCondition.Append(" AND ISNULL(BiometricDetails,'') LIKE @BiometricDetails");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["TaxOfficeName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(TaxOfficeName,'') LIKE @TaxOfficeName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["MaritalStatusName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(MaritalStatusName,'') LIKE @MaritalStatusName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["NationalityName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(NationalityName,'') LIKE @NationalityName");
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
            if (!string.IsNullOrWhiteSpace(Request.Form["DOB"]))
            {
                sbWhereCondition.Append(" AND ISNULL(REPLACE(CONVERT(varchar(50),DOB,106),' ','-'),'') LIKE @DOB");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["ActiveText"]))
            {
                sbWhereCondition.Append(" AND CASE WHEN ISNULL(ind.Active,0) = 0 THEN 'In Active' ELSE 'Active' END LIKE @ActiveText");
            }

            if (!string.IsNullOrEmpty(vFilter))
            {
                sbWhereCondition.Append(" AND ( ISNULL(IndividualRIN,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(GenderName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(TitleName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(FirstName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(MiddleName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(LastName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(TIN,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(MobileNumber1,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(MobileNumber2,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(EmailAddress1,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(EmailAddress2,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(BiometricDetails,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(TaxOfficeName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(MaritalStatusName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(NationalityName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(TaxPayerTypeName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(EconomicActivitiesName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(NotificationMethodName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(REPLACE(CONVERT(varchar(50),DOB,106),' ','-'),'') LIKE @MainFilter");
                sbWhereCondition.Append(" OR CASE WHEN ISNULL(ind.Active,0) = 0 THEN 'In Active' ELSE 'Active' END LIKE @MainFilter)");
            }

            Individual mObjIndividual = new Individual()
            {
                WhereCondition = sbWhereCondition.ToString(),
                OrderBy = vSortColumn,
                OrderByDirection = vSortColumnDir,
                PageNumber = (TrynParse.parseInt(vStart) / TrynParse.parseInt(vLength)) + 1,
                PageSize = TrynParse.parseInt(vLength),
                IndividualRIN = !string.IsNullOrWhiteSpace(Request.Form["RIN"]) ? "%" + Request.Form["RIN"].Trim() + "%" : TrynParse.parseString(Request.Form["RIN"]),
                GenderName = !string.IsNullOrWhiteSpace(Request.Form["GenderName"]) ? "%" + Request.Form["GenderName"].Trim() + "%" : TrynParse.parseString(Request.Form["GenderName"]),
                TitleName = !string.IsNullOrWhiteSpace(Request.Form["TitleName"]) ? "%" + Request.Form["TitleName"].Trim() + "%" : TrynParse.parseString(Request.Form["TitleName"]),
                FirstName = !string.IsNullOrWhiteSpace(Request.Form["FirstName"]) ? "%" + Request.Form["FirstName"].Trim() + "%" : TrynParse.parseString(Request.Form["FirstName"]),
                MiddleName = !string.IsNullOrWhiteSpace(Request.Form["MiddleName"]) ? "%" + Request.Form["MiddleName"].Trim() + "%" : TrynParse.parseString(Request.Form["MiddleName"]),
                LastName = !string.IsNullOrWhiteSpace(Request.Form["LastName"]) ? "%" + Request.Form["LastName"].Trim() + "%" : TrynParse.parseString(Request.Form["LastName"]),
                StrDOB = !string.IsNullOrWhiteSpace(Request.Form["DOB"]) ? "%" + Request.Form["DOB"].Trim() + "%" : TrynParse.parseString(Request.Form["DOB"]),
                TIN = !string.IsNullOrWhiteSpace(Request.Form["TIN"]) ? "%" + Request.Form["TIN"].Trim() + "%" : TrynParse.parseString(Request.Form["TIN"]),
                MobileNumber1 = !string.IsNullOrWhiteSpace(Request.Form["MobileNum1"]) ? "%" + Request.Form["MobileNum1"].Trim() + "%" : TrynParse.parseString(Request.Form["MobileNum1"]),
                MobileNumber2 = !string.IsNullOrWhiteSpace(Request.Form["MobileNum2"]) ? "%" + Request.Form["MobileNum2"].Trim() + "%" : TrynParse.parseString(Request.Form["MobileNum2"]),
                EmailAddress1 = !string.IsNullOrWhiteSpace(Request.Form["EmailAddress1"]) ? "%" + Request.Form["EmailAddress1"].Trim() + "%" : TrynParse.parseString(Request.Form["EmailAddress1"]),
                EmailAddress2 = !string.IsNullOrWhiteSpace(Request.Form["EmailAddress2"]) ? "%" + Request.Form["EmailAddress2"].Trim() + "%" : TrynParse.parseString(Request.Form["EmailAddress2"]),
                BiometricDetails = !string.IsNullOrWhiteSpace(Request.Form["BiometricDetails"]) ? "%" + Request.Form["BiometricDetails"].Trim() + "%" : TrynParse.parseString(Request.Form["BiometricDetails"]),
                TaxOfficeName = !string.IsNullOrWhiteSpace(Request.Form["TaxOfficeName"]) ? "%" + Request.Form["TaxOfficeName"].Trim() + "%" : TrynParse.parseString(Request.Form["TaxOfficeName"]),
                MaritalStatusName = !string.IsNullOrWhiteSpace(Request.Form["MaritalStatusName"]) ? "%" + Request.Form["MaritalStatusName"].Trim() + "%" : TrynParse.parseString(Request.Form["MaritalStatusName"]),
                NationalityName = !string.IsNullOrWhiteSpace(Request.Form["NationalityName"]) ? "%" + Request.Form["NationalityName"].Trim() + "%" : TrynParse.parseString(Request.Form["NationalityName"]),
                TaxPayerTypeName = !string.IsNullOrWhiteSpace(Request.Form["TaxPayerType"]) ? "%" + Request.Form["TaxPayerType"].Trim() + "%" : TrynParse.parseString(Request.Form["TaxPayerType"]),
                EconomicActivitiesName = !string.IsNullOrWhiteSpace(Request.Form["EconomicActivities"]) ? "%" + Request.Form["EconomicActivities"].Trim() + "%" : TrynParse.parseString(Request.Form["EconomicActivities"]),
                NotificationMethodName = !string.IsNullOrWhiteSpace(Request.Form["NotificationMethod"]) ? "%" + Request.Form["NotificationMethod"].Trim() + "%" : TrynParse.parseString(Request.Form["NotificationMethod"]),
                ActiveText = !string.IsNullOrWhiteSpace(Request.Form["ActiveText"]) ? "%" + Request.Form["ActiveText"].Trim() + "%" : TrynParse.parseString(Request.Form["ActiveText"]),
                MainFilter = !string.IsNullOrWhiteSpace(vFilter) ? "%" + vFilter.Trim() + "%" : vFilter
            };



            IDictionary<string, object> dcData = new BLIndividual().BL_SearchIndividual(mObjIndividual);
            IList<usp_SearchIndividual_Result> lstIndividual = (IList<usp_SearchIndividual_Result>)dcData["IndivudalList"];

            return Json(new
            {
                draw = vDraw,
                recordsFiltered = dcData["FilteredRecords"],
                recordsTotal = dcData["TotalRecords"],
                data = lstIndividual
            }, JsonRequestBehavior.AllowGet);
        }

        public void UI_FillDropDown(IndividualViewModel pObjIndividualViewModel = null)
        {
            if (pObjIndividualViewModel != null)
            {
                pObjIndividualViewModel.TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual;
            }
            else if (pObjIndividualViewModel == null)
            {
                pObjIndividualViewModel = new IndividualViewModel();
            }

            UI_FillGender();
            UI_FillTitleDropDown(new Title() { intStatus = 1, IncludeTitleIds = pObjIndividualViewModel.TitleID.ToString(), GenderID = pObjIndividualViewModel.GenderID });
            UI_FillTaxOfficeDropDown(new Tax_Offices() { intStatus = 1, IncludeTaxOfficeIds = pObjIndividualViewModel.TaxOfficeID.ToString() });
            UI_FillMaritalStatus();
            UI_FillNationality();
            UI_FillTaxPayerTypeDropDown(new TaxPayer_Types() { intStatus = 1, IncludeTaxPayerTypeIds = pObjIndividualViewModel.TaxPayerTypeID.ToString() }, (int)EnumList.TaxPayerType.Individual);
            UI_FillEconomicActivitiesDropDown(new Economic_Activities() { intStatus = 1, IncludeEconomicActivitiesIds = pObjIndividualViewModel.EconomicActivitiesID.ToString(), TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual });
            UI_FillNotificationMethodDropDown(new Notification_Method() { intStatus = 1, IncludeNotificationMethodIds = pObjIndividualViewModel.NotificationMethodID.ToString() });
        }

        public ActionResult Add()
        {
            UI_FillDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(IndividualViewModel pObjIndividualModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown(pObjIndividualModel);
                return View(pObjIndividualModel);
            }
            else
            {
                Individual mObjIndividual = new Individual()
                {
                    IndividualID = 0,
                    GenderID = pObjIndividualModel.GenderID,
                    TitleID = pObjIndividualModel.TitleID,
                    FirstName = pObjIndividualModel.FirstName.Trim(),
                    LastName = pObjIndividualModel.LastName.Trim(),
                    MiddleName = pObjIndividualModel.MiddleName,
                    DOB = TrynParse.parseDatetime(pObjIndividualModel.DOB),
                    TIN = pObjIndividualModel.TIN,
                    MobileNumber1 = pObjIndividualModel.MobileNumber1,
                    MobileNumber2 = pObjIndividualModel.MobileNumber2,
                    EmailAddress1 = pObjIndividualModel.EmailAddress1,
                    EmailAddress2 = pObjIndividualModel.EmailAddress2,
                    BiometricDetails = pObjIndividualModel.BiometricDetails,
                    TaxOfficeID = pObjIndividualModel.TaxOfficeID,
                    MaritalStatusID = pObjIndividualModel.MaritalStatusID,
                    NationalityID = pObjIndividualModel.NationalityID,
                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                    EconomicActivitiesID = pObjIndividualModel.EconomicActivitiesID,
                    NotificationMethodID = pObjIndividualModel.NotificationMethodID,
                    //ContactAddress = pObjIndividualModel.ContactAddress,
                    ContactAddress = "My House",
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse<Individual> mObjResponse = new BLIndividual().BL_InsertUpdateIndividual(mObjIndividual);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "Individual");
                    }
                    else
                    {
                        UI_FillDropDown(pObjIndividualModel);
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjIndividualModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillDropDown(pObjIndividualModel);
                    ViewBag.Message = "Error occurred while saving individual";
                    return View(pObjIndividualModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Individual mObjIndividual = new Individual()
                {
                    IndividualID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(mObjIndividual);

                if (mObjIndividualData != null)
                {
                    IndividualViewModel mObjIndividualModelView = new IndividualViewModel()
                    {
                        IndividualID = mObjIndividualData.IndividualID.GetValueOrDefault(),
                        IndividualRIN = mObjIndividualData.IndividualRIN,
                        GenderID = mObjIndividualData.GenderID.GetValueOrDefault(),
                        TitleID = mObjIndividualData.TitleID.GetValueOrDefault(),
                        FirstName = mObjIndividualData.FirstName.Trim(),
                        LastName = mObjIndividualData.LastName.Trim(),
                        MiddleName = mObjIndividualData.MiddleName,
                        DOB = mObjIndividualData.DOB.Value.ToString("dd/MM/yyyy"),
                        TIN = mObjIndividualData.TIN,
                        MobileNumber1 = mObjIndividualData.MobileNumber1,
                        MobileNumber2 = mObjIndividualData.MobileNumber2,
                        EmailAddress1 = mObjIndividualData.EmailAddress1,
                        EmailAddress2 = mObjIndividualData.EmailAddress2,
                        BiometricDetails = mObjIndividualData.BiometricDetails,
                        TaxOfficeID = mObjIndividualData.TaxOfficeID,
                        MaritalStatusID = mObjIndividualData.MaritalStatusID,
                        NationalityID = mObjIndividualData.NationalityID.GetValueOrDefault(),
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                        EconomicActivitiesID = mObjIndividualData.EconomicActivitiesID.GetValueOrDefault(),
                        NotificationMethodID = mObjIndividualData.NotificationMethodID.GetValueOrDefault(),
                        Active = mObjIndividualData.Active.GetValueOrDefault(),
                        ContactAddress = mObjIndividualData.ContactAddress,
                    };

                    UI_FillDropDown(mObjIndividualModelView);
                    return View(mObjIndividualModelView);
                }
                else
                {
                    return RedirectToAction("List", "Individual");
                }
            }
            else
            {
                return RedirectToAction("List", "Individual");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(IndividualViewModel pObjIndividualModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown(pObjIndividualModel);
                return View(pObjIndividualModel);
            }
            else
            {
                Individual mObjIndividual = new Individual()
                {
                    IndividualID = pObjIndividualModel.IndividualID,
                    GenderID = pObjIndividualModel.GenderID,
                    TitleID = pObjIndividualModel.TitleID,
                    FirstName = pObjIndividualModel.FirstName.Trim(),
                    LastName = pObjIndividualModel.LastName.Trim(),
                    MiddleName = pObjIndividualModel.MiddleName,
                    DOB = TrynParse.parseDatetime(pObjIndividualModel.DOB),
                    TIN = pObjIndividualModel.TIN,
                    MobileNumber1 = pObjIndividualModel.MobileNumber1,
                    MobileNumber2 = pObjIndividualModel.MobileNumber2,
                    EmailAddress1 = pObjIndividualModel.EmailAddress1,
                    EmailAddress2 = pObjIndividualModel.EmailAddress2,
                    BiometricDetails = pObjIndividualModel.BiometricDetails,
                    TaxOfficeID = pObjIndividualModel.TaxOfficeID,
                    MaritalStatusID = pObjIndividualModel.MaritalStatusID,
                    NationalityID = pObjIndividualModel.NationalityID,
                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                    EconomicActivitiesID = pObjIndividualModel.EconomicActivitiesID,
                    NotificationMethodID = pObjIndividualModel.NotificationMethodID,
                    ContactAddress = pObjIndividualModel.ContactAddress,
                    Active = pObjIndividualModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse<Individual> mObjResponse = new BLIndividual().BL_InsertUpdateIndividual(mObjIndividual);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "Individual");
                    }
                    else
                    {
                        UI_FillDropDown(pObjIndividualModel);
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjIndividualModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillDropDown(pObjIndividualModel);
                    ViewBag.Message = "Error occurred while saving individual";
                    return View(pObjIndividualModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Individual mObjIndividual = new Individual()
                {
                    IndividualID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(mObjIndividual);

                if (mObjIndividualData != null)
                {
                    IndividualViewModel mObjIndividualModelView = new IndividualViewModel()
                    {
                        IndividualID = mObjIndividualData.IndividualID.GetValueOrDefault(),
                        IndividualRIN = mObjIndividualData.IndividualRIN,
                        GenderName = mObjIndividualData.GenderName,
                        TitleName = mObjIndividualData.TitleName,
                        FirstName = mObjIndividualData.FirstName.Trim(),
                        LastName = mObjIndividualData.LastName.Trim(),
                        MiddleName = mObjIndividualData.MiddleName,
                        DOB = mObjIndividualData.DOB.Value.ToString("dd/MM/yyyy"),
                        TIN = mObjIndividualData.TIN,
                        MobileNumber1 = mObjIndividualData.MobileNumber1,
                        MobileNumber2 = mObjIndividualData.MobileNumber2,
                        EmailAddress1 = mObjIndividualData.EmailAddress1,
                        EmailAddress2 = mObjIndividualData.EmailAddress2,
                        BiometricDetails = mObjIndividualData.BiometricDetails,
                        TaxOfficeName = mObjIndividualData.TaxOfficeName,
                        MaritalStatusName = mObjIndividualData.MaritalStatusName,
                        NationalityName = mObjIndividualData.NationalityName,
                        TaxPayerTypeName = mObjIndividualData.TaxPayerTypeName,
                        EconomicActivitiesName = mObjIndividualData.EconomicActivitiesName,
                        NotificationMethodName = mObjIndividualData.NotificationMethodName,
                        ActiveText = mObjIndividualData.ActiveText,
                        ContactAddress = mObjIndividualData.ContactAddress,
                    };

                    return View(mObjIndividualModelView);
                }
                else
                {
                    return RedirectToAction("List", "Individual");
                }
            }
            else
            {
                return RedirectToAction("List", "Individual");
            }
        }

        public JsonResult UpdateStatus(Individual pObjIndividualData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjIndividualData.IndividualID != 0)
            {
                FuncResponse mObjFuncResponse = new BLIndividual().BL_UpdateStatus(pObjIndividualData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                //if (mObjFuncResponse.Success)
                //{
                //    dcResponse["IndividualList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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
                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual
                };

                IList<usp_GetTaxPayerAssetList_Result> lstTaxPayerAsset = new BLTaxPayerAsset().BL_GetTaxPayerAssetList(mObjTaxPayerAsset);
                usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(new Individual() { intStatus = 2, IndividualID = id.GetValueOrDefault() });
                if (lstTaxPayerAsset != null)
                {
                    ViewBag.TaxPayerID = id;
                    ViewBag.TaxPayerRIN = name;
                    ViewBag.TaxPayerName = mObjIndividualData.FirstName + " " + mObjIndividualData.LastName;
                    return View(lstTaxPayerAsset);
                }
                else
                {
                    return RedirectToAction("List", "Individual");
                }
            }
            else
            {
                return RedirectToAction("List", "Individual");
            }
        }

        public void UI_FillDropDown(TaxPayerAssetViewModel pObjTPAModel)
        {
            UI_FillAssetTypeDropDown(new Asset_Types() { intStatus = 1 });
            UI_FillTaxPayerTypeDropDown(new TaxPayer_Types() { intStatus = 1 }, (int)EnumList.TaxPayerType.Individual);
            UI_FillTaxPayerRoleDropDown(new TaxPayer_Roles() { intStatus = 1, TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual });
        }

        public ActionResult AddAsset(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(new Individual() { intStatus = 2, IndividualID = id.GetValueOrDefault() });
                TaxPayerAssetViewModel mObjTaxPayerAssetModel = new TaxPayerAssetViewModel()
                {
                    TaxPayerID = id.GetValueOrDefault(),
                    TaxPayerRIN = name,
                    TaxPayerName = mObjIndividualData.FirstName + " " + mObjIndividualData.LastName,
                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual
                };

                UI_FillDropDown(mObjTaxPayerAssetModel);
                return View(mObjTaxPayerAssetModel);
            }
            else
            {
                return RedirectToAction("AssetList", "Individual", new { id = id, name = name });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddAsset(TaxPayerAssetViewModel pObjAssetModel)
        {
            if (!ModelState.IsValid)
            {
                pObjAssetModel.TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual;
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
                            TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                            TaxPayerRoleID = pObjAssetModel.TaxPayerRoleID,
                            TaxPayerID = pObjAssetModel.TaxPayerID,
                            AssetID = TrynParse.parseInt(vAssetID),
                            BuildingUnitID = pObjAssetModel.BuildingUnitID,
                            Active = true,
                            CreatedBy = SessionManager.SystemUserID,
                            CreatedDate = CommUtil.GetCurrentDateTime()
                        };

                        FuncResponse mObjResponse = mobjBLTaxPayerAsset.BL_InsertTaxPayerAsset(mObjTaxPayerAsset);
                    }
                }

                FlashMessage.Info("Asset Linked Successfully");
                return RedirectToAction("AssetList", "Individual", new { id = pObjAssetModel.TaxPayerID, name = pObjAssetModel.TaxPayerRIN });
            }
        }

        public ActionResult AssessmentList(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                IList<usp_GetAssessmentList_Result> lstAssessment = new BLAssessment().BL_GetAssessmentList(new Assessment() { TaxPayerID = id.GetValueOrDefault(), TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual, IntStatus = 2 });
                usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(new Individual() { intStatus = 2, IndividualID = id.GetValueOrDefault() });
                ViewBag.TaxPayerID = id;
                ViewBag.TaxPayerRIN = name;
                ViewBag.TaxPayerName = mObjIndividualData.FirstName + " " + mObjIndividualData.LastName;
                return View(lstAssessment);
            }
            else
            {
                return RedirectToAction("List", "Individual");
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
                usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(new Individual() { intStatus = 2, IndividualID = id.GetValueOrDefault() });
                AssessmentViewModel mObjAssessmentModel = new AssessmentViewModel()
                {
                    TaxPayerID = id.GetValueOrDefault(),
                    TaxPayerRIN = name,
                    TaxPayerName = mObjIndividualData.FirstName + " " + mObjIndividualData.LastName,
                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                    SettlementDuedate = CommUtil.GetCurrentDateTime()
                };

                SessionManager.lstAssessmentItem = new List<Assessment_AssessmentItem>();
                SessionManager.lstAssessmentRule = new List<Assessment_AssessmentRule>();
                UI_FillAssessmentDropDown();
                return View(mObjAssessmentModel);
            }
            else
            {
                return RedirectToAction("List", "Individual");
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
                                return RedirectToAction("AssessmentList", "Individual", new { id = pObjAssessmentModel.TaxPayerID, name = pObjAssessmentModel.TaxPayerRIN });

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

                usp_GetAssessmentList_Result mObjAssessmentDetails = mObjBLAssessment.BL_GetAssessmentDetails(new Assessment() { AssessmentID = astid.GetValueOrDefault(), TaxPayerID = id.GetValueOrDefault(), TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual, IntStatus = 1 });

                if (mObjAssessmentDetails != null)
                {
                    IList<usp_GetAssessmentRuleItemList_Result> lstAssessmentItems = mObjBLAssessment.BL_GetAssessmentRuleItem(mObjAssessmentDetails.AssessmentID.GetValueOrDefault());
                    ViewBag.AssessmentItemList = lstAssessmentItems;

                    return View(mObjAssessmentDetails);
                }
                else
                {
                    return RedirectToAction("AssessmentList", "Individual", new { id = id, name = name });
                }
            }
            else
            {
                return RedirectToAction("AssessmentList", "Individual", new { id = id, name = name });
            }

        }

        public ActionResult AddressInformationList(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                BLIndividual mObjIndividual = new BLIndividual();

                IList<usp_GetIndividualAddressInformation_Result> lstAddressInformation = mObjIndividual.BL_GetAddressInformation(new Individual() { IndividualID = id.GetValueOrDefault() });
                usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(new Individual() { intStatus = 2, IndividualID = id.GetValueOrDefault() });
                ViewBag.TaxPayerID = id;
                ViewBag.TaxPayerRIN = name;
                ViewBag.TaxPayerName = mObjIndividualData.FirstName + " " + mObjIndividualData.LastName;
                return View(lstAddressInformation);
            }
            else
            {
                return RedirectToAction("List", "Individual");
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
                usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(new Individual() { intStatus = 2, IndividualID = id.GetValueOrDefault() });
                AddressInformationViewModel mObjAddressInformationModel = new AddressInformationViewModel()
                {
                    TaxPayerID = id.GetValueOrDefault(),
                    TaxPayerRIN = name,
                    TaxPayerName = mObjIndividualData.FirstName + " " + mObjIndividualData.LastName
                };

                UI_FillAddressDropDown();
                return View(mObjAddressInformationModel);
            }
            else
            {
                return RedirectToAction("AddressInformationList", "Individual", new { id = id, name = name });
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
                MAP_Individual_AddressInformation mObjAddressInformation = new MAP_Individual_AddressInformation()
                {
                    AddressTypeID = pObjAddressModel.AddressTypeID,
                    BuildingID = pObjAddressModel.BuildingID,
                    IndividualID = pObjAddressModel.TaxPayerID,
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLIndividual().BL_InsertAddressInformation(mObjAddressInformation);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("AddressInformationList", "Individual", new { id = pObjAddressModel.TaxPayerID, name = pObjAddressModel.TaxPayerRIN });
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

        public JsonResult GetBuildingUnitList(int BuildingID)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            MAP_Building_BuildingUnit mObjBuildingUnit = new MAP_Building_BuildingUnit()
            {
                BuildingID = BuildingID
            };

            IList<usp_GetBuildingUnitNumberList_Result> lstBuildingUnitNumberList = new BLBuilding().BL_GetBuildingUnitNumberList(mObjBuildingUnit);
            dcResponse["success"] = true;
            dcResponse["BuildingUnitList"] = CommUtil.RenderPartialToString("_BindBuildingUnitTable_SingleSelect", lstBuildingUnitNumberList, this.ControllerContext);
            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateAssetStatus(MAP_TaxPayer_Asset pObjAssetData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjAssetData.TPAID != 0)
            {
                pObjAssetData.TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual;
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
                pObjAssetData.TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual;
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
            IList<DropDownListResult> lstAssetType = new BLTaxPayerRole().BL_GetAssetTypeDropDownList(new TaxPayer_Roles() { intStatus = 1, TaxPayerRoleID = TaxPayerRoleID, TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual });
            return Json(lstAssetType, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProfileInformation(int IndividualID)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (IndividualID != 0)
            {
                IList<usp_GetProfileInformation_Result> lstProfileInformation = new BLTaxPayerAsset().BL_GetTaxPayerProfileInformation((int)EnumList.TaxPayerType.Individual, IndividualID);
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

        public JsonResult GetAssessmentRuleInformation(int IndividualID)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (IndividualID != 0)
            {
                IList<usp_GetAssessmentRuleInformation_Result> lstAssessmentRuleInformation = new BLTaxPayerAsset().BL_GetTaxPayerAssessmentRuleInformation((int)EnumList.TaxPayerType.Individual, IndividualID);
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

        public JsonResult RemoveAddressInformation(MAP_Individual_AddressInformation pObjAddressInformation)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjAddressInformation.IAIID != 0)
            {
                FuncResponse<IList<usp_GetIndividualAddressInformation_Result>> mObjFuncResponse = new BLIndividual().BL_RemoveAddressInformation(pObjAddressInformation);
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