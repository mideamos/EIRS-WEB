using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Transactions;
using System.Web.Mvc;
using Elmah;
using EIRS.Models;
using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using Vereyon.Web;
using EIRS.Web.Models;
using System.IO;
using EIRS.Web.Utility;

namespace EIRS.Web.Controllers
{
    public class CertificateTypeController : BaseController
    {
        // GET: CertificateType
        [HttpGet]
        public ActionResult List()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoadData()
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
            int IntPageSize = vLength != null ? Convert.ToInt32(vLength) : 0;
            int IntSkip = vStart != null ? Convert.ToInt32(vStart) : 0;
            int IntTotalRecords = 0;

            IList<usp_GetCertificateTypeList_Result> lstCertificateTypes = new BLCertificateType().BL_GetCertificateTypeList(new Certificate_Types() { IntStatus = 2 });

            if (!string.IsNullOrEmpty(vFilter))
            {
                lstCertificateTypes = lstCertificateTypes.Where(t => t.CertificateTypeName != null && t.CertificateTypeName.ToLower().Trim().Contains(vFilter.ToLower().Trim())
                || t.TaxYear != null && t.TaxYear.ToString().Contains(vFilter.ToLower().Trim())
                || t.ActiveText.ToLower().Trim().Contains(vFilter.ToLower().Trim())).ToList();
            }


            //>>>Purpose Sorting Data 
            if (!(string.IsNullOrEmpty(vSortColumn) && string.IsNullOrEmpty(vSortColumnDir)))
            {
                lstCertificateTypes = lstCertificateTypes.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstCertificateTypes.Count();
            var data = lstCertificateTypes.Skip(IntSkip).Take(IntPageSize).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }

        public void UI_FillDropDown()
        {
            UI_FillYearDropDown();
            UI_FillFieldTypeDropDown();
            UI_FillAgencyDropDown();
            UI_FillPDFTemplateDropDown(GlobalDefaultValues.TCC_SEDEOrganizationID);

            IList<SelectListItem> lstCertificateItemType = new List<SelectListItem>();
            lstCertificateItemType.Add(new SelectListItem() { Text = "Assessment Rule", Value = "1" });
            lstCertificateItemType.Add(new SelectListItem() { Text = "MDA Service", Value = "2" });

            ViewBag.CertificateItemType = lstCertificateItemType;
            ViewBag.FieldList = SessionManager.LstCertificateTypeField.Where(t => t.IntTrack != EnumList.Track.DELETE).ToList();
        }

        [HttpGet]
        public ActionResult Add()
        {
            SessionManager.LstCertificateTypeItem = new List<CertificateTypeItemViewModel>();
            SessionManager.LstCertificateTypeField = new List<CertificateTypeFieldViewModel>();
            UI_FillDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(CertificateTypeViewModel pObjCertificateTypeModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown();
                return View(pObjCertificateTypeModel);
            }
            else
            {
                IList<CertificateTypeFieldViewModel> lstCustomField = SessionManager.LstCertificateTypeField ?? new List<CertificateTypeFieldViewModel>();
                IList<CertificateTypeItemViewModel> lstCertificateItem = SessionManager.LstCertificateTypeItem ?? new List<CertificateTypeItemViewModel>();

                bool mblnCertificateItem = lstCertificateItem.Where(t => t.IntTrack != EnumList.Track.DELETE).Any();

                if (!mblnCertificateItem)
                {
                    ModelState.AddModelError("CertificateItem-error", "Please Add Atleast One Certificate Item");
                    UI_FillDropDown();
                    return View(pObjCertificateTypeModel);
                }

                using (TransactionScope mObjTransctionScope = new TransactionScope())
                {
                    string strCertificateTemplateFilePath = "";

                    if (pObjCertificateTypeModel.CertificateTemplateFile != null && pObjCertificateTypeModel.CertificateTemplateFile.ContentLength > 0)
                    {

                        string strDirectory = GlobalDefaultValues.DocumentLocation + "CertificateType/";
                        string mstrFileName = DateTime.Now.ToString("ddMMyyyyhhmmss_") + Path.GetFileName(pObjCertificateTypeModel.CertificateTemplateFile.FileName);
                        if (!Directory.Exists(strDirectory))
                        {
                            Directory.CreateDirectory(strDirectory);
                        }

                        string mStrCertificateTemplateFilePath = Path.Combine(strDirectory, mstrFileName);
                        pObjCertificateTypeModel.CertificateTemplateFile.SaveAs(mStrCertificateTemplateFilePath);

                        strCertificateTemplateFilePath = "CertificateType/" + mstrFileName;
                    }

                    BLCertificateType mObjBLCertificateType = new BLCertificateType();

                    Certificate_Types mObjCertificateType = new Certificate_Types()
                    {
                        CertificateTypeID = 0,
                        CertificateTypeName = pObjCertificateTypeModel.CertificateTypeName,
                        TaxYear = pObjCertificateTypeModel.TaxYear,
                        Description = pObjCertificateTypeModel.Description,
                        AgencyID = pObjCertificateTypeModel.AgencyID,
                        SEDE_PDFTemplateID = pObjCertificateTypeModel.TemplateID,
                        CNPrefix = pObjCertificateTypeModel.CNPrefix,
                        CertificateTemplatePath = strCertificateTemplateFilePath,
                        Active = true,
                        CreatedBy = SessionManager.UserID,
                        CreatedDate = CommUtil.GetCurrentDateTime()
                    };

                    try
                    {
                        FuncResponse<Certificate_Types> mObjFuncResponse = mObjBLCertificateType.BL_InsertUpdateCertificateType(mObjCertificateType);

                        if (mObjFuncResponse.Success)
                        {
                            MAP_CertificateType_Field mObjCustomField;
                            foreach (var item in lstCustomField.Where(t => t.IntTrack != EnumList.Track.DELETE))
                            {
                                mObjCustomField = new MAP_CertificateType_Field()
                                {
                                    FieldName = item.FieldName,
                                    FieldTypeID = item.FieldTypeID,
                                    CertificateTypeID = mObjFuncResponse.AdditionalData.CertificateTypeID,
                                    FieldComboValue = item.FieldComboValue,
                                    IsRequired = item.IsRequired,
                                    Active = true,
                                    CreatedBy = SessionManager.UserID,
                                    CreatedDate = DateTime.Now
                                };

                                mObjBLCertificateType.BL_InsertCertificateTypeField(mObjCustomField);
                            }

                            MAP_CertificateType_Items mObjCertificateItem;
                            foreach (var item in lstCertificateItem.Where(t => t.IntTrack != EnumList.Track.DELETE))
                            {
                                mObjCertificateItem = new MAP_CertificateType_Items()
                                {
                                    CertificateTypeID = mObjFuncResponse.AdditionalData.CertificateTypeID,
                                    ItemID = item.ItemID,
                                    ItemTypeID = item.ItemTypeID,
                                    CreatedBy = SessionManager.UserID,
                                    CreatedDate = DateTime.Now
                                };

                                mObjBLCertificateType.BL_InsertCertificateTypeItem(mObjCertificateItem);
                            }

                            mObjTransctionScope.Complete();
                            FlashMessage.Info(mObjFuncResponse.Message);
                            return RedirectToAction("List", "CertificateType");
                        }
                        else
                        {
                            UI_FillDropDown();
                            Transaction.Current.Rollback();

                            if (mObjFuncResponse.Exception != null)
                            {
                                ErrorSignal.FromCurrentContext().Raise(mObjFuncResponse.Exception);
                            }

                            FlashMessage.Danger(mObjFuncResponse.Message);
                            return View(pObjCertificateTypeModel);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.SendErrorToText(ex);
                        UI_FillDropDown();
                        ViewBag.Message = "Error occurred while saving certificate type";
                        ErrorSignal.FromCurrentContext().Raise(ex);
                        return View(pObjCertificateTypeModel);
                    }
                }
            }
        }

        [HttpGet]
        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Certificate_Types mObjCertificateType = new Certificate_Types()
                {
                    CertificateTypeID = id.GetValueOrDefault(),
                    IntStatus = 2
                };

                BLCertificateType mObjBLCertificateType = new BLCertificateType();

                usp_GetCertificateTypeList_Result mObjCertificateTypeData = mObjBLCertificateType.BL_GetCertificateTypeDetails(mObjCertificateType);

                if (mObjCertificateTypeData != null)
                {
                    IList<usp_GetCertificateTypeFieldList_Result> lstCertificateTypeField = mObjBLCertificateType.BL_GetCertificateTypeFieldList(mObjCertificateType);
                    IList<usp_GetCertificateTypeItemList_Result> lstCertificateTypeItems = mObjBLCertificateType.BL_GetCertificateTypeItemList(mObjCertificateType);

                    CertificateTypeViewModel mObjCertificateTypeModel = new CertificateTypeViewModel()
                    {
                        CertificateTypeID = mObjCertificateTypeData.CertificateTypeID.GetValueOrDefault(),
                        CertificateTypeName = mObjCertificateTypeData.CertificateTypeName,
                        CNPrefix = mObjCertificateTypeData.CNPrefix,
                        Description = mObjCertificateTypeData.Description,
                        TaxYear = mObjCertificateTypeData.TaxYear.GetValueOrDefault(),
                        AgencyID = mObjCertificateTypeData.AgencyID.GetValueOrDefault(),
                        TemplateID = mObjCertificateTypeData.SEDE_PDFTemplateID.GetValueOrDefault(),
                        CertificateTemplatePath = mObjCertificateTypeData.CertificateTemplatePath,
                        Active = mObjCertificateTypeData.Active.GetValueOrDefault(),
                    };

                    IList<CertificateTypeItemViewModel> lstCertificateItem = new List<CertificateTypeItemViewModel>();
                    IList<CertificateTypeFieldViewModel> lstCustomField = new List<CertificateTypeFieldViewModel>();

                    CertificateTypeItemViewModel mObjCertificateTypeItemViewModel;
                    foreach (var item in lstCertificateTypeItems)
                    {
                        mObjCertificateTypeItemViewModel = new CertificateTypeItemViewModel()
                        {
                            CTIID = item.CTIID.GetValueOrDefault(),
                            ItemID = item.ItemID.GetValueOrDefault(),
                            ItemName = item.ItemName,
                            ItemTypeID = item.ItemTypeID.GetValueOrDefault(),
                            ItemTypeName = item.ItemTypeName,
                            IntTrack = EnumList.Track.EXISTING,
                            RowID = lstCertificateItem.Count + 1
                        };

                        lstCertificateItem.Add(mObjCertificateTypeItemViewModel);
                    }

                    SessionManager.LstCertificateTypeItem = lstCertificateItem;

                    CertificateTypeFieldViewModel mObjCertificateTypeFieldViewModel;
                    foreach (var item in lstCertificateTypeField)
                    {
                        mObjCertificateTypeFieldViewModel = new CertificateTypeFieldViewModel()
                        {
                            CTFID = item.CTFID.GetValueOrDefault(),
                            FieldTypeID = item.FieldTypeID.GetValueOrDefault(),
                            FieldName = item.FieldName,
                            FieldComboValue = item.FieldComboValue,
                            FieldTypeName = item.FieldTypeName,
                            IsRequired = item.IsRequired.GetValueOrDefault(),
                            RequiredText = item.RequiredText,
                            IntTrack = EnumList.Track.EXISTING,
                            RowID = lstCertificateItem.Count + 1
                        };

                        lstCustomField.Add(mObjCertificateTypeFieldViewModel);
                    }

                    SessionManager.LstCertificateTypeField = lstCustomField;

                    UI_FillDropDown();
                    return View(mObjCertificateTypeModel);
                }
                else
                {
                    return RedirectToAction("List", "CertificateType");
                }
            }
            else
            {
                return RedirectToAction("List", "CertificateType");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(CertificateTypeViewModel pObjCertificateTypeModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown();
                return View(pObjCertificateTypeModel);
            }
            else
            {
                IList<CertificateTypeFieldViewModel> lstCustomField = SessionManager.LstCertificateTypeField ?? new List<CertificateTypeFieldViewModel>();
                IList<CertificateTypeItemViewModel> lstCertificateItem = SessionManager.LstCertificateTypeItem ?? new List<CertificateTypeItemViewModel>();

                bool mblnCertificateItem = lstCertificateItem.Where(t => t.IntTrack != EnumList.Track.DELETE).Any();

                if (!mblnCertificateItem)
                {
                    ModelState.AddModelError("CertificateItem-error", "Please Add Atleast One Certificate Item");
                    UI_FillDropDown();
                    return View(pObjCertificateTypeModel);
                }

                using (TransactionScope mObjTransctionScope = new TransactionScope())
                {
                    string strCertificateTemplateFilePath = pObjCertificateTypeModel.CertificateTemplatePath;

                    if (pObjCertificateTypeModel.CertificateTemplateFile != null && pObjCertificateTypeModel.CertificateTemplateFile.ContentLength > 0)
                    {

                        string strDirectory = GlobalDefaultValues.DocumentLocation + "CertificateType/";
                        string mstrFileName = DateTime.Now.ToString("ddMMyyyyhhmmss_") + Path.GetFileName(pObjCertificateTypeModel.CertificateTemplateFile.FileName);
                        if (!Directory.Exists(strDirectory))
                        {
                            Directory.CreateDirectory(strDirectory);
                        }

                        string mStrCertificateTemplateFilePath = Path.Combine(strDirectory, mstrFileName);
                        pObjCertificateTypeModel.CertificateTemplateFile.SaveAs(mStrCertificateTemplateFilePath);

                        strCertificateTemplateFilePath = "CertificateType/" + mstrFileName;
                    }

                    BLCertificateType mObjBLCertificateType = new BLCertificateType();

                    Certificate_Types mObjCertificateType = new Certificate_Types()
                    {
                        CertificateTypeID = pObjCertificateTypeModel.CertificateTypeID,
                        CertificateTypeName = pObjCertificateTypeModel.CertificateTypeName,
                        TaxYear = pObjCertificateTypeModel.TaxYear,
                        Description = pObjCertificateTypeModel.Description,
                        AgencyID = pObjCertificateTypeModel.AgencyID,
                        SEDE_PDFTemplateID = pObjCertificateTypeModel.TemplateID,
                        CNPrefix = pObjCertificateTypeModel.CNPrefix,
                        CertificateTemplatePath = strCertificateTemplateFilePath,
                        Active = pObjCertificateTypeModel.Active,
                        ModifiedBy = SessionManager.UserID,
                        ModifiedDate = CommUtil.GetCurrentDateTime()
                    };

                    try
                    {
                        FuncResponse<Certificate_Types> mObjFuncResponse = mObjBLCertificateType.BL_InsertUpdateCertificateType(mObjCertificateType);

                        if (mObjFuncResponse.Success)
                        {
                            MAP_CertificateType_Field mObjCustomField;
                            foreach (var item in lstCustomField)
                            {
                                if (item.IntTrack == EnumList.Track.INSERT)
                                {
                                    mObjCustomField = new MAP_CertificateType_Field()
                                    {
                                        FieldName = item.FieldName,
                                        FieldTypeID = item.FieldTypeID,
                                        CertificateTypeID = mObjFuncResponse.AdditionalData.CertificateTypeID,
                                        FieldComboValue = item.FieldComboValue,
                                        IsRequired = item.IsRequired,
                                        Active = true,
                                        CreatedBy = SessionManager.UserID,
                                        CreatedDate = DateTime.Now
                                    };

                                    mObjBLCertificateType.BL_InsertCertificateTypeField(mObjCustomField);
                                }
                                else if (item.IntTrack == EnumList.Track.DELETE)
                                {
                                    mObjCustomField = new MAP_CertificateType_Field()
                                    {
                                        CTFID = item.CTFID
                                    };

                                    mObjBLCertificateType.BL_RemoveCertificateTypeField(mObjCustomField);
                                }
                            }

                            MAP_CertificateType_Items mObjCertificateItem;
                            foreach (var item in lstCertificateItem)
                            {
                                if (item.IntTrack == EnumList.Track.INSERT)
                                {
                                    mObjCertificateItem = new MAP_CertificateType_Items()
                                    {
                                        CertificateTypeID = mObjFuncResponse.AdditionalData.CertificateTypeID,
                                        ItemID = item.ItemID,
                                        ItemTypeID = item.ItemTypeID,
                                        CreatedBy = SessionManager.UserID,
                                        CreatedDate = DateTime.Now
                                    };

                                    mObjBLCertificateType.BL_InsertCertificateTypeItem(mObjCertificateItem);
                                }
                                else if (item.IntTrack == EnumList.Track.DELETE)
                                {
                                    if (item.CTIID > 0)
                                    {
                                        mObjCertificateItem = new MAP_CertificateType_Items()
                                        {
                                            CTIID = item.CTIID
                                        };

                                        mObjBLCertificateType.BL_RemoveCertificateTypeItem(mObjCertificateItem);
                                    }
                                }
                            }

                            mObjTransctionScope.Complete();
                            FlashMessage.Info(mObjFuncResponse.Message);
                            return RedirectToAction("List", "CertificateType");
                        }
                        else
                        {
                            UI_FillDropDown();
                            Transaction.Current.Rollback();

                            if (mObjFuncResponse.Exception != null)
                            {
                                ErrorSignal.FromCurrentContext().Raise(mObjFuncResponse.Exception);
                            }

                            FlashMessage.Danger(mObjFuncResponse.Message);
                            return View(pObjCertificateTypeModel);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.SendErrorToText(ex);
                        UI_FillDropDown();
                        ViewBag.Message = "Error occurred while saving certificate type";
                        ErrorSignal.FromCurrentContext().Raise(ex);
                        return View(pObjCertificateTypeModel);
                    }



                }
            }
        }

        [HttpGet]
        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Certificate_Types mObjCertificateType = new Certificate_Types()
                {
                    CertificateTypeID = id.GetValueOrDefault(),
                    IntStatus = 2
                };

                BLCertificateType mObjBLCertificateType = new BLCertificateType();

                usp_GetCertificateTypeList_Result mObjCertificateTypeData = mObjBLCertificateType.BL_GetCertificateTypeDetails(mObjCertificateType);

                if (mObjCertificateTypeData != null)
                {
                    IList<usp_GetCertificateTypeFieldList_Result> lstCertificateTypeField = mObjBLCertificateType.BL_GetCertificateTypeFieldList(mObjCertificateType);
                    IList<usp_GetCertificateTypeItemList_Result> lstCertificateTypeItems = mObjBLCertificateType.BL_GetCertificateTypeItemList(mObjCertificateType);

                    ViewBag.CustomFieldList = lstCertificateTypeField;
                    ViewBag.CertificateItemList = lstCertificateTypeItems;

                    return View(mObjCertificateTypeData);
                }
                else
                {
                    return RedirectToAction("List", "CertificateType");
                }
            }
            else
            {
                return RedirectToAction("List", "CertificateType");
            }
        }

        public JsonResult AddCustomField(CertificateTypeFieldViewModel pObjCustomField)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            IList<CertificateTypeFieldViewModel> lstCustomField = SessionManager.LstCertificateTypeField ?? new List<CertificateTypeFieldViewModel>();

            if (!lstCustomField.Where(t => t.FieldName == pObjCustomField.FieldName && t.IntTrack != EnumList.Track.DELETE).Any())
            {
                pObjCustomField.IntTrack = EnumList.Track.INSERT;
                pObjCustomField.RowID = lstCustomField.Count + 1;

                lstCustomField.Add(pObjCustomField);
                SessionManager.LstCertificateTypeField = lstCustomField;
                dcResponse["Success"] = true;
                dcResponse["CustomFieldTable"] = CommUtil.RenderPartialToString("_BindCustomFieldTable", lstCustomField.Where(t => t.IntTrack != EnumList.Track.DELETE).ToList(), this.ControllerContext);
            }
            else
            {
                dcResponse["Success"] = false;
                dcResponse["Message"] = "Field with same name already exists";
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }
        public JsonResult RemoveCustomField(int? RowID)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();
            try
            {
                IList<CertificateTypeFieldViewModel> lstCustomField = SessionManager.LstCertificateTypeField ?? new List<CertificateTypeFieldViewModel>();
                CertificateTypeFieldViewModel mObjCustomField = lstCustomField.Where(x => x.RowID == RowID.GetValueOrDefault()).FirstOrDefault();

                if (mObjCustomField != null)
                {
                    mObjCustomField.IntTrack = EnumList.Track.DELETE;
                }

                SessionManager.LstCertificateTypeField = lstCustomField;
                dcResponse["Success"] = true;
                dcResponse["CustomFieldTable"] = CommUtil.RenderPartialToString("_BindCustomFieldTable", lstCustomField.Where(t => t.IntTrack != EnumList.Track.DELETE).ToList(), this.ControllerContext);
            }
            catch (Exception ex)
            {
                Logger.SendErrorToText(ex);
                dcResponse["success"] = false;
                dcResponse["Message"] = "Error Occurred. Try Again Later or Contact Administrator";
            }
            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddCertificateItem(string tyear, int itemtypeid, string itemname)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            IList<CertificateTypeItemViewModel> lstCertificateItem = SessionManager.LstCertificateTypeItem ?? new List<CertificateTypeItemViewModel>();
            CertificateTypeItemViewModel mObjCertificateItemModel;
            if (itemtypeid == 1)
            {
                //Get Assessment Rule List
                IList<usp_SearchAssessmentRulesForRDMLoad_Result> lstAssessmentRule = new BLAssessmentRule().BL_SearchAssessmentRuleDetails(new Assessment_Rules() { AssessmentRuleName = itemname, StrTaxYear = tyear });
                foreach (var item in lstAssessmentRule)
                {
                    if (!lstCertificateItem.Where(t => t.ItemID == item.AssessmentRuleID && t.ItemTypeID == 1 && t.IntTrack != EnumList.Track.DELETE).Any())
                    {
                        mObjCertificateItemModel = new CertificateTypeItemViewModel()
                        {
                            CTIID = 0,
                            RowID = lstCertificateItem.Count + 1,
                            ItemID = item.AssessmentRuleID,
                            ItemName = item.AssessmentRuleName,
                            ItemTypeID = 1,
                            ItemTypeName = "Assessment Rule",
                            IntTrack = EnumList.Track.INSERT
                        };

                        lstCertificateItem.Add(mObjCertificateItemModel);
                    }
                }

            }
            else if (itemtypeid == 2)
            {
                IList<usp_SearchMDAServiceForRDMLoad_Result> lstMDAService = new BLMDAService().BL_SearchMDAServiceDetails(new MDA_Services() { MDAServiceItemName = itemname, StrTaxYear = tyear });
                foreach (var item in lstMDAService)
                {
                    if (!lstCertificateItem.Where(t => t.ItemID == item.MDAServiceID && t.ItemTypeID == 2 && t.IntTrack != EnumList.Track.DELETE).Any())
                    {
                        mObjCertificateItemModel = new CertificateTypeItemViewModel()
                        {
                            CTIID = 0,
                            RowID = lstCertificateItem.Count + 1,
                            ItemID = item.MDAServiceID,
                            ItemName = item.MDAServiceName,
                            ItemTypeID = 1,
                            ItemTypeName = "MDA Service",
                            IntTrack = EnumList.Track.INSERT
                        };

                        lstCertificateItem.Add(mObjCertificateItemModel);
                    }
                }
            }


            SessionManager.LstCertificateTypeItem = lstCertificateItem;
            dcResponse["Success"] = true;
            //dcResponse["CertificateItemTable"] = CommUtil.RenderPartialToString("_BindCertificateItemTable", lstCertificateItem.Where(t => t.IntTrack != EnumList.Track.DELETE).ToList(), this.ControllerContext);

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RemoveCertificateItem(int? RowID)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();
            try
            {
                IList<CertificateTypeItemViewModel> lstCertificateItem = SessionManager.LstCertificateTypeItem ?? new List<CertificateTypeItemViewModel>();
                CertificateTypeItemViewModel mObjCustomItem = lstCertificateItem.Where(x => x.RowID == RowID.GetValueOrDefault()).FirstOrDefault();

                if (mObjCustomItem != null)
                {
                    mObjCustomItem.IntTrack = EnumList.Track.DELETE;
                }

                SessionManager.LstCertificateTypeItem = lstCertificateItem;
                dcResponse["Success"] = true;
                //dcResponse["CertificateItemTable"] = CommUtil.RenderPartialToString("_BindCertificateItemTable", lstCertificateItem.Where(t => t.IntTrack != EnumList.Track.DELETE).ToList(), this.ControllerContext);
            }
            catch (Exception ex)
            {
                Logger.SendErrorToText(ex);
                dcResponse["success"] = false;
                dcResponse["Message"] = "Error Occurred. Try Again Later or Contact Administrator";
            }
            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult LoadCertificateItem()
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
            int IntPageSize = vLength != null ? Convert.ToInt32(vLength) : 0;
            int IntSkip = vStart != null ? Convert.ToInt32(vStart) : 0;
            int IntTotalRecords = 0;

            IList<CertificateTypeItemViewModel> lstCertificateItem = SessionManager.LstCertificateTypeItem ?? new List<CertificateTypeItemViewModel>();

            if (!string.IsNullOrEmpty(vFilter))
            {
                lstCertificateItem = lstCertificateItem.Where(t => t.ItemName != null && t.ItemName.ToLower().Trim().Contains(vFilter.ToLower().Trim())
                || t.ItemTypeName != null && t.ItemTypeName.ToString().Contains(vFilter.ToLower().Trim())).ToList();
            }

            lstCertificateItem = lstCertificateItem.Where(t => t.IntTrack != EnumList.Track.DELETE).ToList();


            //>>>Purpose Sorting Data 
            if (!(string.IsNullOrEmpty(vSortColumn) && string.IsNullOrEmpty(vSortColumnDir)))
            {
                lstCertificateItem = lstCertificateItem.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstCertificateItem.Count;
            var data = lstCertificateItem.Skip(IntSkip).Take(IntPageSize).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }
    }
}