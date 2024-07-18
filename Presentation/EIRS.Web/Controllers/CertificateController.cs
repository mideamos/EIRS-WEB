using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using static EIRS.Web.Controllers.Filters;

namespace EIRS.Web.Controllers
{
    [SessionTimeout]
    public class CertificateController : BaseController
    {
        [HttpGet]
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
            sbWhereCondition.Append(" AND ISNULL(crt.StatusID,0) IN (Select id From dbo.SplitString(@StatusID,',')) ");

            if (!string.IsNullOrEmpty(vFilter))
            {
                sbWhereCondition.Append(" AND ( crt.CertificateNumber LIKE @MainFilter ");
                sbWhereCondition.Append(" OR crtype.CertificateTypeName LIKE @MainFilter ");
                sbWhereCondition.Append(" OR tptype.TaxPayerTypeName LIKE @MainFilter ");
                sbWhereCondition.Append(" OR dbo.GetTaxPayerName(crt.TaxPayerID,crt.TaxPayerTypeID) LIKE @MainFilter ");
                sbWhereCondition.Append(" OR dbo.GetTaxPayerRIN(crt.TaxPayerID,crt.TaxPayerTypeID) LIKE @MainFilter ");
                sbWhereCondition.Append(" OR cstat.CertificateStatusName LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(REPLACE(CONVERT(varchar(50),crt.CertificateDate,106),' ','-'),'') LIKE @MainFilter )");

            }

            Certificate mObjCertificate = new Certificate()
            {
                WhereCondition = sbWhereCondition.ToString(),
                OrderBy = vSortColumn,
                OrderByDirection = vSortColumnDir,
                PageNumber = (TrynParse.parseInt(vStart) / TrynParse.parseInt(vLength)) + 1,
                PageSize = TrynParse.parseInt(vLength),
                MainFilter = !string.IsNullOrWhiteSpace(vFilter) ? "%" + vFilter.Trim() + "%" : vFilter,
                StatusIds = "1,2"

            };

            IDictionary<string, object> dcData = new BLCertificate().BL_SearchCertificate(mObjCertificate);
            IList<usp_SearchCertificate_Result> lstCetificates = (IList<usp_SearchCertificate_Result>)dcData["CertificateList"];

            return Json(new
            {
                draw = vDraw,
                recordsFiltered = dcData["FilteredRecords"],
                recordsTotal = dcData["TotalRecords"],
                data = lstCetificates
            }, JsonRequestBehavior.AllowGet);
        }

        public void UI_FillDropDown(GenerateCertificateViewModel pobjCertificateModel = null)
        {
            if (pobjCertificateModel == null)
            {
                ViewBag.ProfileList = new SelectList(Enumerable.Empty<SelectListItem>());
                ViewBag.AssetList = new SelectList(Enumerable.Empty<SelectListItem>());
            }
            else
            {
                IList<DropDownListResult> lstProfile = new BLTaxPayerAsset().BL_GetTaxPayerProfileDropDownForCertificate(pobjCertificateModel.TaxPayerID, pobjCertificateModel.TaxPayerTypeID, pobjCertificateModel.CertificateTypeID);
                ViewBag.ProfileList = new SelectList(lstProfile, "id", "text");

                IList<DropDownListResult> lstAsset = new BLTaxPayerAsset().BL_GetTaxPayerAssetDropDownList(new MAP_TaxPayer_Asset() { TaxPayerID = pobjCertificateModel.TaxPayerID, TaxPayerTypeID = pobjCertificateModel.TaxPayerTypeID, ProfileID = pobjCertificateModel.ProfileID });
                ViewBag.AssetList = new SelectList(lstAsset, "id", "text");
            }

            UI_FillCertificateTypeDropDown();
            UI_FillTaxPayerTypeDropDown();
        }

        [HttpGet]
        public ActionResult Generate()
        {
            UI_FillDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Generate(GenerateCertificateViewModel pObjCertificateModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown(pObjCertificateModel);
                return View(pObjCertificateModel);
            }
            else
            {
                usp_GetCertificateTypeList_Result mObjCertificateTypeData = new BLCertificateType().BL_GetCertificateTypeDetails(new Certificate_Types() { CertificateTypeID = pObjCertificateModel.CertificateTypeID, IntStatus = 2 });

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = pObjCertificateModel.ProfileID, IntStatus = 2 });

                Certificate mObjCertificate = new Certificate()
                {
                    CertificateID = 0,
                    CertificateTypeID = pObjCertificateModel.CertificateTypeID,
                    TaxPayerTypeID = pObjCertificateModel.TaxPayerTypeID,
                    TaxPayerID = pObjCertificateModel.TaxPayerID,
                    AssetID = pObjCertificateModel.AssetID,
                    AssetTypeID = mObjProfileData.AssetTypeID,
                    ProfileID = pObjCertificateModel.ProfileID,
                    CertificateDate = CommUtil.GetCurrentDateTime(),
                    ExpiryDate = new DateTime(mObjCertificateTypeData.TaxYear.GetValueOrDefault(), 12, 31),
                    StatusID = (int)EnumList.CertificateStatus.Started,
                    CreatedBy = SessionManager.UserID,
                    CreatedDate = CommUtil.GetCurrentDateTime(),
                };

                FuncResponse<Certificate> mObjFuncResponse = new BLCertificate().BL_InsertCertificate(mObjCertificate);

                if (mObjFuncResponse.Success)
                {
                    return RedirectToAction("Update", "Certificate", new { id = mObjFuncResponse.AdditionalData.CertificateID, name = mObjFuncResponse.AdditionalData.CertificateNumber.ToSeoUrl() });
                }
                else
                {
                    ViewBag.Message = mObjFuncResponse.Message;
                    UI_FillDropDown(pObjCertificateModel);
                    return View(pObjCertificateModel);
                }
            }
        }

        [HttpGet]
        public ActionResult Update(long? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                BLCertificate mObjBLCertificate = new BLCertificate();

                usp_GetCertificateDetails_Result mObjCertificateDetails = mObjBLCertificate.BL_GetCertificateDetails(id.GetValueOrDefault());

                if (mObjCertificateDetails != null)
                {
                    UpdateCertificateViewModel mObjUpdateCertificateModel = new UpdateCertificateViewModel()
                    {
                        CertificateID = mObjCertificateDetails.CertificateID,
                        Notes = mObjCertificateDetails.Notes,
                        OtherInformation = mObjCertificateDetails.OtherInformation,
                        QRCodeID = mObjCertificateDetails.QRCodeID,
                        SignerID = mObjCertificateDetails.SignerID.GetValueOrDefault(),
                        SignerRoleID = mObjCertificateDetails.SignerRoleID
                    };

                    ViewBag.CertificateDetails = mObjCertificateDetails;

                    IList<usp_GetCertificateFieldList_Result> lstCertificateField = mObjBLCertificate.BL_GetCertificateField(new Certificate() { CertificateID = mObjCertificateDetails.CertificateID, CertificateTypeID = mObjCertificateDetails.CertificateTypeID });
                    ViewBag.CertificateFieldList = lstCertificateField;

                    IList<usp_GetCertificateItemList_Result> lstCertificateItem = mObjBLCertificate.BL_GetCertificateItem(new Certificate() { CertificateID = mObjCertificateDetails.CertificateID, CertificateTypeID = mObjCertificateDetails.CertificateTypeID });
                    ViewBag.CertificateItemList = lstCertificateItem;

                    IList<usp_GetAssessmentRuleInformationForCertificate_Result> lstAssessmentRuleInformation = mObjBLCertificate.BL_GetAssessmentRuleInformationForCertificate(new Certificate() { CertificateID = mObjCertificateDetails.CertificateID, CertificateTypeID = mObjCertificateDetails.CertificateTypeID });
                    ViewBag.AssessmentRuleInformationList = lstAssessmentRuleInformation;

                    return View(mObjUpdateCertificateModel);
                }
                else
                {
                    return RedirectToAction("List", "Certificate");
                }
            }
            else
            {
                return RedirectToAction("List", "Certificate");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Update(UpdateCertificateViewModel pObjCertificateModel, FormCollection pObjFormCollection)
        {
            string strAction = Request["btnAction"];
            BLCertificate mObjBLCertificate = new BLCertificate();
            usp_GetCertificateDetails_Result mObjCertificateDetails = mObjBLCertificate.BL_GetCertificateDetails(pObjCertificateModel.CertificateID);
            IList<usp_GetCertificateFieldList_Result> lstCertificateField = mObjBLCertificate.BL_GetCertificateField(new Certificate() { CertificateID = mObjCertificateDetails.CertificateID, CertificateTypeID = mObjCertificateDetails.CertificateTypeID });
            IList<usp_GetCertificateItemList_Result> lstCertificateItem = mObjBLCertificate.BL_GetCertificateItem(new Certificate() { CertificateID = mObjCertificateDetails.CertificateID, CertificateTypeID = mObjCertificateDetails.CertificateTypeID });
            IList<usp_GetAssessmentRuleInformationForCertificate_Result> lstAssessmentRuleInformation = mObjBLCertificate.BL_GetAssessmentRuleInformationForCertificate(new Certificate() { CertificateID = mObjCertificateDetails.CertificateID, CertificateTypeID = mObjCertificateDetails.CertificateTypeID });

            foreach (var item in lstCertificateField)
            {
                if (item.FieldTypeID == (int)EnumList.FieldType.Combo)
                {
                    item.FieldValue = pObjFormCollection.Get("cbo_" + item.CTFID + "_" + item.FieldName.ToSeoUrl());
                }
                else
                {
                    item.FieldValue = pObjFormCollection.Get("txt_" + item.CTFID + "_" + item.FieldName.ToSeoUrl());
                }
            }


            if (!ModelState.IsValid)
            {
                ViewBag.CertificateDetails = mObjCertificateDetails;
                ViewBag.CertificateFieldList = lstCertificateField;
                ViewBag.AssessmentRuleInformationList = lstAssessmentRuleInformation;
                ViewBag.CertificateItemList = lstCertificateItem;
                return View(pObjCertificateModel);
            }
            else
            {
                if (strAction == "Submit")
                {
                    //Validation
                    if (!lstCertificateItem.Where(t => t.BilledAmount.GetValueOrDefault() > 0).Any())
                    {
                        ViewBag.Message = "No Bill Found for Tax Payer";
                        ViewBag.CertificateDetails = mObjCertificateDetails;
                        ViewBag.CertificateFieldList = lstCertificateField;
                        ViewBag.AssessmentRuleInformationList = lstAssessmentRuleInformation;
                        ViewBag.CertificateItemList = lstCertificateItem;
                        return View(pObjCertificateModel);
                    }

                    if (lstCertificateItem.Where(t => t.OustandingAmount.GetValueOrDefault() > 0).Any())
                    {
                        ViewBag.Message = "There are outstanding payment on tax payer";
                        ViewBag.CertificateDetails = mObjCertificateDetails;
                        ViewBag.CertificateFieldList = lstCertificateField;
                        ViewBag.AssessmentRuleInformationList = lstAssessmentRuleInformation;
                        ViewBag.CertificateItemList = lstCertificateItem;
                        return View(pObjCertificateModel);
                    }

                    //Check if tax payer is in liability report and has tcc

                }

                Certificate mObjCertificate = new Certificate()
                {
                    CertificateID = pObjCertificateModel.CertificateID,
                    Notes = pObjCertificateModel.Notes,
                    OtherInformation = pObjCertificateModel.OtherInformation,
                    StatusID = (int)EnumList.CertificateStatus.Started,
                    ModifiedBy = SessionManager.UserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime(),
                };

                FuncResponse mObjFuncResponse = mObjBLCertificate.BL_UpdateCertificate(mObjCertificate);

                if (mObjFuncResponse.Success)
                {
                    MAP_Certificate_CustomField mObjCertificateCustomField;
                    foreach (var item in lstCertificateField)
                    {
                        string strFieldValue = "";
                        if (item.FieldTypeID == (int)EnumList.FieldType.Combo)
                        {
                            strFieldValue = pObjFormCollection.Get("cbo_" + item.CTFID + "_" + item.FieldName.ToSeoUrl());
                        }
                        else if (item.FieldTypeID == (int)EnumList.FieldType.FileUpload)
                        {

                            HttpPostedFileBase mObjPostedFile = Request.Files["fu_" + item.CTFID + "_" + item.FieldName.ToSeoUrl()];

                            if (mObjPostedFile != null && mObjPostedFile.ContentLength > 0)
                            {
                                string strDirectory = GlobalDefaultValues.DocumentLocation + "Certificate/" + pObjCertificateModel.CertificateID + "/CustomField/";
                                string mstrFileName = "CF_" + item.CTFID + "_" + Path.GetFileName(mObjPostedFile.FileName);

                                if (!Directory.Exists(strDirectory))
                                {
                                    Directory.CreateDirectory(strDirectory);
                                }

                                string mStrDocumentPath = Path.Combine(strDirectory, mstrFileName);
                                mObjPostedFile.SaveAs(mStrDocumentPath);

                                strFieldValue = "Certificate/" + pObjCertificateModel.CertificateID + "/CustomField/" + mstrFileName;
                            }
                            else
                            {
                                strFieldValue = item.FieldValue;
                            }

                        }
                        else
                        {
                            strFieldValue = pObjFormCollection.Get("txt_" + item.CTFID + "_" + item.FieldName.ToSeoUrl());
                        }

                        mObjCertificateCustomField = new MAP_Certificate_CustomField()
                        {
                            CCFID = 0,
                            CertificateID = pObjCertificateModel.CertificateID,
                            CTFID = item.CTFID,
                            FieldValue = strFieldValue,
                            Active = true,
                            CreatedBy = SessionManager.UserID,
                            CreatedDate = CommUtil.GetCurrentDateTime(),
                            ModifiedBy = SessionManager.UserID,
                            ModifiedDate = CommUtil.GetCurrentDateTime()
                        };

                        mObjBLCertificate.BL_InsertUpdateCertificateField(mObjCertificateCustomField);
                    }

                    if (strAction == "Submit")
                    {

                        //Generate Certificate
                        // 1 -- BPP, 2 -- BPC
                        string mStrExportDocumentPath = GlobalDefaultValues.DocumentLocation + "Certificate/" + mObjCertificateDetails.CertificateTypeName.ToSeoUrl() + "/" + mObjCertificateDetails.CertificateNumber + ".pdf";
                        if (!Directory.Exists(GlobalDefaultValues.DocumentLocation + "Certificate/" + mObjCertificateDetails.CertificateTypeName.ToSeoUrl()))
                        {
                            Directory.CreateDirectory(GlobalDefaultValues.DocumentLocation + "Certificate/" + mObjCertificateDetails.CertificateTypeName.ToSeoUrl());
                        }

                        if (System.IO.File.Exists(mStrExportDocumentPath))
                        {
                            System.IO.File.Delete(mStrExportDocumentPath);
                        }

                        string strHtmlContent = System.IO.File.ReadAllText(GlobalDefaultValues.DocumentLocation + mObjCertificateDetails.CertificateTemplatePath);

                        //Certificate Details
                        string mStrCertificateDetails = "";
                        if (lstCertificateItem.Any())
                        {
                            foreach (var item in lstCertificateItem)
                            {
                                mStrCertificateDetails += $"<li>{item.CertificateItemName}</li>";
                            }
                        }
                        else
                        {
                            mStrCertificateDetails += "<li>None</li>";
                        }

                        //Custom Fields Details
                        string mStrCustomField = "";
                        if (lstCertificateField.Any())
                        {
                            foreach (var item in lstCertificateField)
                            {
                                mStrCustomField += $"<li>{item.FieldName} : {item.FieldValue}</li>";
                            }
                        }
                        else
                        {
                            mStrCustomField += "<li>None</li>";
                        }

                        IList<usp_GetTaxPayerAssetList_Result> lstTaxPayerAsset = new BLTaxPayerAsset().BL_GetTaxPayerAssetList(new MAP_TaxPayer_Asset()
                        {
                            TaxPayerID = mObjCertificateDetails.TaxPayerID,
                            TaxPayerTypeID = mObjCertificateDetails.TaxPayerTypeID,
                            AssetID = mObjCertificateDetails.AssetID,
                            AssetTypeID = mObjCertificateDetails.AssetTypeID
                        });

                        var vTaxPayerRole = lstTaxPayerAsset.FirstOrDefault().TaxPayerRoleName;


                        mObjCertificate = new Certificate()
                        {
                            CertificateID = pObjCertificateModel.CertificateID,
                            CertificatePath = "Certificate/" + mObjCertificateDetails.CertificateTypeName.ToSeoUrl() + "/" + mObjCertificateDetails.CertificateNumber + ".pdf",
                            StatusID = (int)EnumList.CertificateStatus.Created,
                            ModifiedBy = SessionManager.UserID,
                            ModifiedDate = CommUtil.GetCurrentDateTime(),
                        };

                        mObjBLCertificate.BL_UpdateCertificatePath(mObjCertificate);
                    }

                    return RedirectToAction("List", "Certificate");
                }
                else
                {
                    ViewBag.Message = mObjFuncResponse.Message;
                    ViewBag.CertificateDetails = mObjCertificateDetails;
                    ViewBag.CertificateFieldList = lstCertificateField;
                    ViewBag.AssessmentRuleInformationList = lstAssessmentRuleInformation;
                    ViewBag.CertificateItemList = lstCertificateItem;
                    return View(pObjCertificateModel);
                }
            }
        }


        [HttpGet]
        public ActionResult GeneratePDF(long? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                BLCertificate mObjBLCertificate = new BLCertificate();

                usp_GetCertificateDetails_Result mObjCertificateDetails = mObjBLCertificate.BL_GetCertificateDetails(id.GetValueOrDefault());

                if (mObjCertificateDetails != null)
                {
                    return File(GlobalDefaultValues.DocumentLocation + mObjCertificateDetails.CertificatePath, "application/force-download", mObjCertificateDetails.CertificateNumber.Trim() + "_Unsecured.pdf");
                }
                else
                {
                    return Content("Invalid Request");
                }
            }
            else
            {
                return Content("Invalid Request");
            }
        }

        public JsonResult GetProfileDropDown(int TaxPayerID, int TaxPayerTypeID, int CertificateTypeID)
        {
            IList<DropDownListResult> lstProfile = new BLTaxPayerAsset().BL_GetTaxPayerProfileDropDownForCertificate(TaxPayerID, TaxPayerTypeID, CertificateTypeID);
            return Json(lstProfile, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAssetDropDown(int TaxPayerID, int TaxPayerTypeID, int ProfileID)
        {
            IList<DropDownListResult> lstTaxPayerAsset = new BLTaxPayerAsset().BL_GetTaxPayerAssetDropDownList(new MAP_TaxPayer_Asset() { TaxPayerID = TaxPayerID, TaxPayerTypeID = TaxPayerTypeID, ProfileID = ProfileID });
            return Json(lstTaxPayerAsset, JsonRequestBehavior.AllowGet);
        }
    }
}