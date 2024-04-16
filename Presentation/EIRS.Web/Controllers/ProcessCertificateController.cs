using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Web.Models;
using EIRS.Web.Utility;
using Elmah;
using Newtonsoft.Json;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace EIRS.Web.Controllers
{
    public class ProcessCertificateController : BaseController
    {
        // GET: ProcessCertificate

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
                sbWhereCondition.Append(" OR crt.TaxYear LIKE @MainFilter ");
                sbWhereCondition.Append(" OR tptype.TaxPayerTypeName LIKE @MainFilter ");
                sbWhereCondition.Append(" OR dbo.GetTaxPayerName(crt.TaxPayerID,crt.TaxPayerTypeID) LIKE @MainFilter ");
                sbWhereCondition.Append(" OR dbo.GetTaxPayerRIN(crt.TaxPayerID,crt.TaxPayerTypeID) LIKE @MainFilter ");
                sbWhereCondition.Append(" OR crt.StatusName LIKE @MainFilter ");
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
                StatusIds = "2,3,4,5,6,7,8"

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

        [HttpGet]
        public ActionResult Details(long? certid)
        {
            if (certid.GetValueOrDefault() > 0)
            {
                BLCertificate mObjBLCertificate = new BLCertificate();

                usp_GetCertificateDetails_Result mObjCertificateDetails = mObjBLCertificate.BL_GetCertificateDetails(certid.GetValueOrDefault());

                if (mObjCertificateDetails != null)
                {
                    //Get Stage List
                    IList<usp_GetAdminCertificateStageList_Result> lstCertificateStage = mObjBLCertificate.BL_GetCertificateStageList(mObjCertificateDetails.CertificateID);
                    ViewBag.CertificateStageList = lstCertificateStage;

                    return View(mObjCertificateDetails);
                }
                else
                {
                    return RedirectToAction("List", "ProcessCertificate");
                }
            }
            else
            {
                return RedirectToAction("List", "ProcessCertificate");
            }

        }

        [HttpGet]
        public ActionResult Generate(long? certid)
        {
            if (certid.GetValueOrDefault() > 0)
            {
                BLCertificate mObjBLCertificate = new BLCertificate();
                usp_GetCertificateDetails_Result mObjCertificateData = mObjBLCertificate.BL_GetCertificateDetails(certid.GetValueOrDefault());

                if (mObjCertificateData != null)
                {
                    ViewBag.CertificateData = mObjCertificateData;

                    MAP_Certificate_Generate mObjGenerateData = mObjBLCertificate.BL_GetCertificateGenerateDetails(mObjCertificateData.CertificateID);

                    PDFTemplateModel mObjPDFTemplateData = SEDEFunction.PDFTemplateDetail(mObjCertificateData.SEDE_PDFTemplateID.GetValueOrDefault());

                    IList<PDFTemplateFieldList> lstTemplateField;

                    GenerateViewModel mObjGenerateModel = new GenerateViewModel()
                    {
                        CertificateID = mObjCertificateData.CertificateID,
                    };

                    if (mObjGenerateData != null)
                    {
                        mObjGenerateModel.CGID = mObjGenerateData.CGID;
                        mObjGenerateModel.GenerateNotes = mObjGenerateData.Notes;
                        mObjGenerateModel.ExpiryDate = mObjGenerateData.ExpiryDate;
                        mObjGenerateModel.IsExpirable = mObjGenerateData.IsExpirable.GetValueOrDefault();
                        mObjGenerateModel.Reason = mObjGenerateData.Reason;
                        mObjGenerateModel.Location = mObjGenerateData.Location;
                        mObjGenerateModel.PDFTemplateID = mObjCertificateData.SEDE_PDFTemplateID.GetValueOrDefault();
                        mObjGenerateModel.SEDE_DocumentID = mObjCertificateData.SEDE_DocumentID.GetValueOrDefault();

                        lstTemplateField = SEDEFunction.PDFTemplateFieldList(mObjCertificateData.SEDE_PDFTemplateID.GetValueOrDefault(), mObjGenerateModel.SEDE_DocumentID);
                    }
                    else
                    {
                        lstTemplateField = SEDEFunction.PDFTemplateFieldList(mObjCertificateData.SEDE_PDFTemplateID.GetValueOrDefault(), 0);
                        //Bind Data to Template Field
                        usp_GetCertificateDetailForGenerate_Result mObjCertificateDetailForGenerateProcessData = mObjBLCertificate.BL_GetCertificateDetailForGenerateProcess(mObjCertificateData.CertificateID);

                        if (mObjCertificateDetailForGenerateProcessData != null)
                        {
                            Type mObjRequestType = mObjCertificateDetailForGenerateProcessData.GetType();
                            foreach (var item in lstTemplateField)
                            {
                                if (!string.IsNullOrWhiteSpace(item.ETX_OrderFieldName))
                                {
                                    if (mObjRequestType.GetProperty(item.ETX_OrderFieldName) != null)
                                    {
                                        if (item.FieldTypeID == (int)EnumList.FieldType.Date)
                                        {
                                            DateTime? dtFieldDate = TrynParse.parseDatetime(mObjRequestType.GetProperty(item.ETX_OrderFieldName).GetValue(mObjCertificateDetailForGenerateProcessData, null));
                                            item.FieldValue = dtFieldDate != null ? dtFieldDate.GetValueOrDefault().ToString("dd-MM-yyyy") : "";
                                        }
                                        else
                                        {
                                            item.FieldValue = TrynParse.parseString(mObjRequestType.GetProperty(item.ETX_OrderFieldName).GetValue(mObjCertificateDetailForGenerateProcessData, null));
                                        }
                                    }
                                }
                            }
                        }
                    }

                    ViewBag.TemplateFieldList = lstTemplateField;
                    return View(mObjGenerateModel);
                }
                else
                {
                    return RedirectToAction("List", "ProcessCertificate");
                }
            }
            else
            {
                return RedirectToAction("List", "ProcessCertificate");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Generate(GenerateViewModel pobjGenerateModel, FormCollection pObjFormCollection)
        {
            BLCertificate mObjBLCertificate = new BLCertificate();
            usp_GetCertificateDetails_Result mObjCertificateData = mObjBLCertificate.BL_GetCertificateDetails(pobjGenerateModel.CertificateID);

            IList<PDFTemplateFieldList> lstTemplateField = SEDEFunction.PDFTemplateFieldList(mObjCertificateData.SEDE_PDFTemplateID.GetValueOrDefault(), pobjGenerateModel.SEDE_DocumentID);

            foreach (var item in lstTemplateField)
            {
                if (item.FieldTypeID == (int)EnumList.FieldType.Combo)
                {
                    item.FieldValue = pObjFormCollection.Get("cbo_" + item.FieldID + "_" + item.FieldName.ToSeoUrl());
                }
                else
                {
                    item.FieldValue = pObjFormCollection.Get("txt_" + item.FieldID + "_" + item.FieldName.ToSeoUrl());
                }
            }

            if (!ModelState.IsValid)
            {
                ViewBag.CertificateData = mObjCertificateData;
                ViewBag.TemplateFieldList = lstTemplateField;
                return View(pobjGenerateModel);
            }
            else
            {
                using (TransactionScope mObjTransctionScope = new TransactionScope())
                {
                    try
                    {
                        //Update 
                        MAP_Certificate_Generate mObjGenerate = new MAP_Certificate_Generate()
                        {
                            CGID = pobjGenerateModel.CGID,
                            CertificateID = pobjGenerateModel.CertificateID,
                            Notes = pobjGenerateModel.GenerateNotes,
                            PDFTemplateID = mObjCertificateData.SEDE_PDFTemplateID,
                            IsExpirable = pobjGenerateModel.IsExpirable,
                            ExpiryDate = pobjGenerateModel.ExpiryDate,
                            Reason = pobjGenerateModel.Reason,
                            Location = pobjGenerateModel.Location,
                            StageID = (int)EnumList.CertificateStage.Generate,
                            ApprovalDate = CommUtil.GetCurrentDateTime(),
                            IsAction = false,
                            CreatedBy = SessionManager.UserID,
                            CreatedDate = CommUtil.GetCurrentDateTime()
                        };

                        FuncResponse<MAP_Certificate_Generate> mObjFuncResponse = mObjBLCertificate.BL_InsertUpdateCertificateGenerate(mObjGenerate);

                        if (mObjFuncResponse.Success)
                        {
                            MAP_Certificate_Generate_Field mObjGenerateField;
                            foreach (var item in lstTemplateField)
                            {
                                string strFieldValue = "";
                                if (item.FieldTypeID == (int)EnumList.FieldType.Combo)
                                {
                                    strFieldValue = pObjFormCollection.Get("cbo_" + item.FieldID + "_" + item.FieldName.ToSeoUrl());
                                }
                                else if (item.FieldTypeID == (int)EnumList.FieldType.FileUpload)
                                {

                                    HttpPostedFileBase mObjPostedFile = Request.Files["fu_" + item.FieldID + "_" + item.FieldName.ToSeoUrl()];

                                    if (mObjPostedFile != null && mObjPostedFile.ContentLength > 0)
                                    {
                                        string strDirectory = GlobalDefaultValues.DocumentLocation + "Certificate/" + pobjGenerateModel.CertificateID + "/CustomFiles/";
                                        string mstrFileName = "CF_" + item.FieldID + "_" + Path.GetFileName(mObjPostedFile.FileName);

                                        if (!Directory.Exists(strDirectory))
                                        {
                                            Directory.CreateDirectory(strDirectory);
                                        }

                                        string mStrDocumentPath = Path.Combine(strDirectory, mstrFileName);
                                        mObjPostedFile.SaveAs(mStrDocumentPath);

                                        strFieldValue = "Certificate/" + pobjGenerateModel.CertificateID + "/CustomFiles/" + mstrFileName;
                                    }
                                    else
                                    {
                                        strFieldValue = item.FieldValue;
                                    }

                                }
                                else
                                {
                                    strFieldValue = pObjFormCollection.Get("txt_" + item.FieldID + "_" + item.FieldName.ToSeoUrl());
                                }

                                mObjGenerateField = new MAP_Certificate_Generate_Field()
                                {
                                    CGFID = 0,
                                    CGID = mObjFuncResponse.AdditionalData.CGID,
                                    FieldID = item.FieldID,
                                    PFID = item.PFID,
                                    FieldValue = strFieldValue,
                                    Active = true,
                                    CreatedBy = SessionManager.UserID,
                                    CreatedDate = CommUtil.GetCurrentDateTime()

                                };

                                item.FieldValue = strFieldValue;

                                mObjBLCertificate.BL_InsertUpdateGenerateField(mObjGenerateField);
                            }

                            //Pass Document to Sede
                            IList<DocumentFieldModel> lstDocumentField = new List<DocumentFieldModel>();
                            DocumentFieldModel mObjDocumentFieldModel;
                            IDictionary<string, string> dcFileData = new Dictionary<string, string>();

                            dcFileData["DocumentPath"] = GlobalDefaultValues.DocumentLocation + mObjCertificateData.CertificatePath;

                            foreach (var item in lstTemplateField)
                            {
                                if (item.FieldTypeID == (int)EnumList.FieldType.FileUpload)
                                {
                                    if (!string.IsNullOrWhiteSpace(item.FieldValue))
                                    {
                                        dcFileData["fu_" + item.FieldID + "_" + item.FieldName.ToSeoUrl()] = GlobalDefaultValues.DocumentLocation + item.FieldValue;
                                    }
                                }
                                else
                                {
                                    mObjDocumentFieldModel = new DocumentFieldModel()
                                    {
                                        FieldID = item.FieldID,
                                        FieldValue = item.FieldValue,
                                    };

                                    lstDocumentField.Add(mObjDocumentFieldModel);
                                }
                            }


                            DocumentViewModel mObjSEDEDocumentModel = new DocumentViewModel()
                            {
                                DocumentID = pobjGenerateModel.SEDE_DocumentID,
                                OrganizationID = GlobalDefaultValues.TCC_SEDEOrganizationID,
                                PDFTemplateID = mObjCertificateData.SEDE_PDFTemplateID.GetValueOrDefault(),
                                IsExpirable = pobjGenerateModel.IsExpirable,
                                ExpiryDate = pobjGenerateModel.ExpiryDate,
                                Reason = pobjGenerateModel.Reason,
                                Location = pobjGenerateModel.Location,
                                FieldList = lstDocumentField
                            };


                            IDictionary<string, object> dcSEDEResponse;
                            if (mObjCertificateData.SEDE_DocumentID.GetValueOrDefault() > 0)
                            {
                                dcSEDEResponse = APICall.PostData(GlobalDefaultValues.SEDE_API_UpdateDocument, mObjSEDEDocumentModel, dcFileData);
                            }
                            else
                            {
                                dcSEDEResponse = APICall.PostData(GlobalDefaultValues.SEDE_API_AddDocument, mObjSEDEDocumentModel, dcFileData);
                            }

                            if (TrynParse.parseBool(dcSEDEResponse["success"]))
                            {
                                int mIntSEDEDocumentID = TrynParse.parseInt(dcSEDEResponse["documentId"]);

                                //Generate PDF and Update DB
                                PDFTemplateModel mObjPDFTemplateData = SEDEFunction.PDFTemplateDetail(mObjCertificateData.SEDE_PDFTemplateID.GetValueOrDefault());

                                string mStrDirectory = GlobalDefaultValues.DocumentLocation + "Certificate/" + mObjCertificateData.CertificateID + "/Generate";
                                string mStrGeneratedFileName = DateTime.Now.ToString("ddMMyyyy") + "_Generated.pdf";
                                string mStrExportFileName = DateTime.Now.ToString("ddMMyyyy") + "_Exported.pdf";
                                string mStrExportDocumentPath = Path.Combine(mStrDirectory, mStrExportFileName);
                                string mStrGeneratedDocumentPath = Path.Combine(mStrDirectory, mStrGeneratedFileName);

                                if (!Directory.Exists(mStrDirectory))
                                {
                                    Directory.CreateDirectory(mStrDirectory);
                                }

                                mObjGenerate.GeneratedPath = "Certificate/" + mObjCertificateData.CertificateID + "/Generate/" + mStrGeneratedFileName;
                                mObjGenerate.SEDE_DocumentID = mIntSEDEDocumentID;
                                mObjGenerate.StageID = (int)EnumList.CertificateStage.Generate;
                                mObjGenerate.IsAction = true;
                                mObjGenerate.CGID = mObjFuncResponse.AdditionalData.CGID;


                                mObjFuncResponse = mObjBLCertificate.BL_InsertUpdateCertificateGenerate(mObjGenerate);

                                if (mObjFuncResponse.Success)
                                {
                                    mObjTransctionScope.Complete();
                                    return RedirectToAction("Details", "ProcessCertificate", new { certid = pobjGenerateModel.CertificateID });
                                }
                                else
                                {
                                    Transaction.Current.Rollback();
                                    ViewBag.Message = "Error Occurred while Generating";
                                    ViewBag.CertificateData = mObjCertificateData;
                                    ViewBag.TemplateFieldList = lstTemplateField;

                                    return View(pobjGenerateModel);
                                }
                            }
                            else
                            {
                                Transaction.Current.Rollback();

                                ViewBag.Message = dcSEDEResponse["Message"];
                                ViewBag.CertificateData = mObjCertificateData;
                                ViewBag.TemplateFieldList = lstTemplateField;
                                return View(pobjGenerateModel);
                            }



                        }
                        else
                        {
                            ViewBag.CertificateData = mObjCertificateData;
                            ViewBag.TemplateFieldList = lstTemplateField;
                            ViewBag.Message = mObjFuncResponse.Message;
                            return View(pobjGenerateModel);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.SendErrorToText(ex);
                        ViewBag.CertificateData = mObjCertificateData;
                        ViewBag.TemplateFieldList = lstTemplateField;
                        ErrorSignal.FromCurrentContext().Raise(ex);
                        ViewBag.Message = "Error occurred while saving generate certificate details";
                        return View(pobjGenerateModel);
                    }
                }
            }
        }

        [HttpGet]
        public ActionResult Validate(long? certid)
        {
            if (certid.GetValueOrDefault() > 0)
            {
                BLCertificate mObjBLCertificate = new BLCertificate();
                usp_GetCertificateDetails_Result mObjCertificateData = mObjBLCertificate.BL_GetCertificateDetails(certid.GetValueOrDefault());

                if (mObjCertificateData != null)
                {
                    ViewBag.CertificateData = mObjCertificateData;

                    MAP_Certificate_Validate mObjValidateData = mObjBLCertificate.BL_GetCertificateValidateDetails(mObjCertificateData.CertificateID);

                    ValidateViewModel mObjValidateModel = new ValidateViewModel()
                    {
                        CertificateID = mObjCertificateData.CertificateID,
                    };

                    if (mObjValidateData != null)
                    {
                        mObjValidateModel.CVID = mObjValidateData.CVID;
                        mObjValidateModel.ValidateNotes = mObjValidateData.Notes;
                    }

                    return View(mObjValidateModel);
                }
                else
                {
                    return RedirectToAction("List", "ProcessCertificate");
                }
            }
            else
            {
                return RedirectToAction("List", "ProcessCertificate");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Validate(ValidateViewModel pobjValidateModel)
        {
            BLCertificate mObjBLCertificate = new BLCertificate();
            usp_GetCertificateDetails_Result mObjCertificateData = mObjBLCertificate.BL_GetCertificateDetails(pobjValidateModel.CertificateID);

            if (!ModelState.IsValid)
            {
                ViewBag.CertificateData = mObjCertificateData;
                return View(pobjValidateModel);
            }
            else
            {
                using (TransactionScope mObjTransctionScope = new TransactionScope())
                {
                    try
                    {
                        //Update 
                        MAP_Certificate_Validate mObjValidate = new MAP_Certificate_Validate()
                        {
                            CVID = pobjValidateModel.CVID,
                            CertificateID = pobjValidateModel.CertificateID,
                            Notes = pobjValidateModel.ValidateNotes,
                            StageID = (int)EnumList.CertificateStage.Validate,
                            ApprovalDate = CommUtil.GetCurrentDateTime(),
                            IsAction = false,
                            CreatedBy = SessionManager.UserID,
                            CreatedDate = CommUtil.GetCurrentDateTime()
                        };

                        FuncResponse<MAP_Certificate_Validate> mObjFuncResponse = mObjBLCertificate.BL_InsertUpdateCertificateValidate(mObjValidate);

                        if (mObjFuncResponse.Success)
                        {
                            string mStrDirectory = GlobalDefaultValues.DocumentLocation + "Certificate/" + mObjCertificateData.CertificateID + "/Validate/";
                            string mStrValidatedFileName = DateTime.Now.ToString("ddMMyyyy") + "_Validated.pdf";
                            if (!Directory.Exists(mStrDirectory))
                            {
                                Directory.CreateDirectory(mStrDirectory);
                            }

                            IDictionary<string, object> dcSEDEResponse = APICall.GetData(GlobalDefaultValues.SEDE_API_ProcessDocument, new Dictionary<string, object> { ["DocumentID"] = mObjCertificateData.SEDE_DocumentID.GetValueOrDefault() });

                            if (TrynParse.parseBool(dcSEDEResponse["success"]))
                            {
                                mObjValidate.CVID = mObjFuncResponse.AdditionalData.CVID;
                                mObjValidate.ValidatedPath = "Certificate/" + mObjCertificateData.CertificateID + "/Validate/" + mStrValidatedFileName;
                                mObjValidate.SEDE_OrderID = TrynParse.parseLong(dcSEDEResponse["orderId"]);
                                mObjValidate.StageID = (int)EnumList.CertificateStage.Validate;
                                mObjValidate.IsAction = true;

                                mObjFuncResponse = mObjBLCertificate.BL_InsertUpdateCertificateValidate(mObjValidate);

                                if (mObjFuncResponse.Success)
                                {
                                    mObjTransctionScope.Complete();
                                    return RedirectToAction("Details", "ProcessCertificate", new { certid = pobjValidateModel.CertificateID });
                                }
                                else
                                {
                                    Transaction.Current.Rollback();
                                    ViewBag.Message = "Error Occurred while validating";
                                    ViewBag.CertificateData = mObjCertificateData;

                                    return View(pobjValidateModel);
                                }
                            }
                            else
                            {
                                Transaction.Current.Rollback();

                                ViewBag.Message = dcSEDEResponse["Message"];
                                ViewBag.CertificateData = mObjCertificateData;
                                return View(pobjValidateModel);
                            }



                        }
                        else
                        {
                            ViewBag.CertificateData = mObjCertificateData;
                            ViewBag.Message = mObjFuncResponse.Message;
                            return View(pobjValidateModel);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.SendErrorToText(ex);
                        ViewBag.CertificateData = mObjCertificateData;
                        ErrorSignal.FromCurrentContext().Raise(ex);
                        ViewBag.Message = "Error occurred while saving validate certificate details";
                        return View(pobjValidateModel);
                    }
                }
            }
        }

        [HttpGet]
        public ActionResult SignVisible(long? certid)
        {
            if (certid.GetValueOrDefault() > 0)
            {
                BLCertificate mObjBLCertificate = new BLCertificate();
                usp_GetCertificateDetails_Result mObjCertificateData = mObjBLCertificate.BL_GetCertificateDetails(certid.GetValueOrDefault());

                if (mObjCertificateData != null)
                {
                    ViewBag.CertificateData = mObjCertificateData;

                    MAP_Certificate_Validate mObjValidateData = mObjBLCertificate.BL_GetCertificateValidateDetails(mObjCertificateData.CertificateID);

                    ValidateViewModel mObjValidateModel = new ValidateViewModel()
                    {
                        CertificateID = mObjCertificateData.CertificateID,
                    };

                    if (mObjValidateData != null)
                    {
                        mObjValidateModel.CVID = mObjValidateData.CVID;
                        mObjValidateModel.ValidateNotes = mObjValidateData.Notes;
                    }

                    return View(mObjValidateModel);
                }
                else
                {
                    return RedirectToAction("List", "ProcessCertificate");
                }
            }
            else
            {
                return RedirectToAction("List", "ProcessCertificate");
            }
        }

        [HttpGet]
        public ActionResult SignDigital(long? certid)
        {
            if (certid.GetValueOrDefault() > 0)
            {
                BLCertificate mObjBLCertificate = new BLCertificate();
                usp_GetCertificateDetails_Result mObjCertificateData = mObjBLCertificate.BL_GetCertificateDetails(certid.GetValueOrDefault());

                if (mObjCertificateData != null)
                {
                    ViewBag.CertificateData = mObjCertificateData;

                    MAP_Certificate_SignDigital mObjSignDigitalData = mObjBLCertificate.BL_GetCertificateSignDigitalDetails(mObjCertificateData.CertificateID);

                    SignDigitalViewModel mObjSignDigitalModel = new SignDigitalViewModel()
                    {
                        CertificateID = mObjCertificateData.CertificateID,
                    };

                    if (mObjSignDigitalData != null)
                    {
                        mObjSignDigitalModel.CSDID = mObjSignDigitalData.CSDID;
                        mObjSignDigitalModel.SignNotes = mObjSignDigitalData.Notes;
                    }

                    return View(mObjSignDigitalModel);
                }
                else
                {
                    return RedirectToAction("List", "ProcessCertificate");
                }
            }
            else
            {
                return RedirectToAction("List", "ProcessCertificate");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult SignDigital(SignDigitalViewModel pobjSignDigitalModel)
        {
            BLCertificate mObjBLCertificate = new BLCertificate();
            usp_GetCertificateDetails_Result mObjCertificateData = mObjBLCertificate.BL_GetCertificateDetails(pobjSignDigitalModel.CertificateID);

            if (!ModelState.IsValid)
            {
                ViewBag.CertificateData = mObjCertificateData;
                return View(pobjSignDigitalModel);
            }
            else
            {
                using (TransactionScope mObjTransctionScope = new TransactionScope())
                {
                    try
                    {
                        //Update 
                        MAP_Certificate_SignDigital mObjSignDigital = new MAP_Certificate_SignDigital()
                        {
                            CSDID = pobjSignDigitalModel.CSDID,
                            CertificateID = pobjSignDigitalModel.CertificateID,
                            Notes = pobjSignDigitalModel.SignNotes,
                            StageID = (int)EnumList.CertificateStage.Sign_Digital,
                            ApprovalDate = CommUtil.GetCurrentDateTime(),
                            IsAction = false,
                            CreatedBy = SessionManager.UserID,
                            CreatedDate = CommUtil.GetCurrentDateTime()
                        };

                        FuncResponse<MAP_Certificate_SignDigital> mObjFuncResponse = mObjBLCertificate.BL_InsertUpdateCertificateSignDigital(mObjSignDigital);

                        if (mObjFuncResponse.Success)
                        {
                            string mStrDirectory = GlobalDefaultValues.DocumentLocation + "Certificate/" + mObjCertificateData.CertificateID + "/SignDigital/";
                            string mStrSignDigitalFileName = DateTime.Now.ToString("ddMMyyyy") + "_SignDigital.pdf";
                            if (!Directory.Exists(mStrDirectory))
                            {
                                Directory.CreateDirectory(mStrDirectory);
                            }

                            //Fetch Additional Signature Location
                            IList<MAP_Certificate_SignVisible> lstVisibleSignature = mObjBLCertificate.BL_GetCertificateSignVisibleList(mObjCertificateData.CertificateID);
                            IList<AdditionalSignature> lstAdditionalSignature = new List<AdditionalSignature>();

                            foreach (var item in lstVisibleSignature)
                            {
                                var vAdditionalSignature = JsonConvert.DeserializeObject<IList<AdditionalSignature>>(item.AdditionalSignatureLocation);
                                foreach (var ias in vAdditionalSignature)
                                {
                                    lstAdditionalSignature.Add(ias);
                                }
                            }

                            IDictionary<string, object> dcData = new Dictionary<string, object>();
                            dcData["AdditionalSignatureLocation"] = JsonConvert.SerializeObject(lstAdditionalSignature);
                            dcData["DocumentWidth"] = lstVisibleSignature.FirstOrDefault().DocumentWidth;

                            IDictionary<string, object> dcSEDEResponse = APICall.PostData(GlobalDefaultValues.SEDE_API_VisibleSignDocument + "?OrderID=" + mObjCertificateData.SEDE_OrderID.GetValueOrDefault(), dcData);

                            if (TrynParse.parseBool(dcSEDEResponse["success"]))
                            {

                                mObjSignDigital.CSDID = mObjFuncResponse.AdditionalData.CSDID;
                                mObjSignDigital.SignedDigitalPath = "Certificate/" + mObjCertificateData.CertificateID + "/SignDigital/" + mStrSignDigitalFileName;
                                mObjSignDigital.StageID = (int)EnumList.CertificateStage.Sign_Digital;
                                mObjSignDigital.IsAction = true;

                                mObjFuncResponse = mObjBLCertificate.BL_InsertUpdateCertificateSignDigital(mObjSignDigital);

                                if (mObjFuncResponse.Success)
                                {
                                    mObjTransctionScope.Complete();
                                    return RedirectToAction("Details", "ProcessCertificate", new { certid = pobjSignDigitalModel.CertificateID });
                                }
                                else
                                {
                                    Transaction.Current.Rollback();
                                    ViewBag.Message = "Error Occurred while signing";
                                    ViewBag.CertificateData = mObjCertificateData;

                                    return View(pobjSignDigitalModel);
                                }
                            }
                            else
                            {
                                Transaction.Current.Rollback();

                                ViewBag.Message = JsonConvert.SerializeObject(lstAdditionalSignature); //dcSEDEResponse["Message"];
                                ViewBag.CertificateData = mObjCertificateData;
                                return View(pobjSignDigitalModel);
                            }



                        }
                        else
                        {
                            Transaction.Current.Rollback();
                            ViewBag.CertificateData = mObjCertificateData;
                            ViewBag.Message = mObjFuncResponse.Message;
                            return View(pobjSignDigitalModel);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.SendErrorToText(ex);
                        Transaction.Current.Rollback();
                        ViewBag.CertificateData = mObjCertificateData;
                        ErrorSignal.FromCurrentContext().Raise(ex);
                        ViewBag.Message = "Error occurred while saving signing digital certificate details";
                        return View(pobjSignDigitalModel);
                    }
                }
            }
        }

        [HttpGet]
        public ActionResult Seal(long? certid)
        {
            if (certid.GetValueOrDefault() > 0)
            {
                BLCertificate mObjBLCertificate = new BLCertificate();
                usp_GetCertificateDetails_Result mObjCertificateData = mObjBLCertificate.BL_GetCertificateDetails(certid.GetValueOrDefault());

                if (mObjCertificateData != null)
                {
                    ViewBag.CertificateData = mObjCertificateData;

                    MAP_Certificate_Seal mObjSealData = mObjBLCertificate.BL_GetCertificateSealDetails(mObjCertificateData.CertificateID);

                    SealViewModel mObjSealModel = new SealViewModel()
                    {
                        CertificateID = mObjCertificateData.CertificateID,
                    };

                    if (mObjSealData != null)
                    {
                        mObjSealModel.CSID = mObjSealData.CSID;
                        mObjSealModel.SealNotes = mObjSealData.Notes;
                    }

                    return View(mObjSealModel);
                }
                else
                {
                    return RedirectToAction("List", "ProcessCertificate");
                }
            }
            else
            {
                return RedirectToAction("List", "ProcessCertificate");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Seal(SealViewModel pobjSealModel)
        {
            BLCertificate mObjBLCertificate = new BLCertificate();
            usp_GetCertificateDetails_Result mObjCertificateData = mObjBLCertificate.BL_GetCertificateDetails(pobjSealModel.CertificateID);

            if (!ModelState.IsValid)
            {
                ViewBag.CertificateData = mObjCertificateData;
                return View(pobjSealModel);
            }
            else
            {
                using (TransactionScope mObjTransctionScope = new TransactionScope())
                {
                    try
                    {
                        //Update 
                        MAP_Certificate_Seal mObjSeal = new MAP_Certificate_Seal()
                        {
                            CSID = pobjSealModel.CSID,
                            CertificateID = pobjSealModel.CertificateID,
                            Notes = pobjSealModel.SealNotes,
                            StageID = (int)EnumList.CertificateStage.Seal,
                            ApprovalDate = CommUtil.GetCurrentDateTime(),
                            IsAction = false,
                            CreatedBy = SessionManager.UserID,
                            CreatedDate = CommUtil.GetCurrentDateTime()
                        };

                        FuncResponse<MAP_Certificate_Seal> mObjFuncResponse = mObjBLCertificate.BL_InsertUpdateCertificateSeal(mObjSeal);

                        if (mObjFuncResponse.Success)
                        {
                            string mStrDirectory = GlobalDefaultValues.DocumentLocation + "Certificate/" + mObjCertificateData.CertificateID + "/Seal/";
                            string mStrSealFileName = DateTime.Now.ToString("ddMMyyyy") + "_Seal.pdf";
                            if (!Directory.Exists(mStrDirectory))
                            {
                                Directory.CreateDirectory(mStrDirectory);
                            }

                            IDictionary<string, object> dcSEDEResponse = APICall.GetData(GlobalDefaultValues.SEDE_API_SealDocument, new Dictionary<string, object> { ["OrderID"] = mObjCertificateData.SEDE_OrderID.GetValueOrDefault() });

                            if (TrynParse.parseBool(dcSEDEResponse["success"]))
                            {

                                mObjSeal.CSID = mObjFuncResponse.AdditionalData.CSID;
                                mObjSeal.SealedPath = "Certificate/" + mObjCertificateData.CertificateID + "/Seal/" + mStrSealFileName;
                                mObjSeal.StageID = (int)EnumList.CertificateStage.Seal;
                                mObjSeal.IsAction = true;

                                mObjFuncResponse = mObjBLCertificate.BL_InsertUpdateCertificateSeal(mObjSeal);

                                if (mObjFuncResponse.Success)
                                {
                                    mObjTransctionScope.Complete();
                                    return RedirectToAction("Details", "ProcessCertificate", new { certid = pobjSealModel.CertificateID });
                                }
                                else
                                {
                                    Transaction.Current.Rollback();
                                    ViewBag.Message = "Error Occurred while sealing";
                                    ViewBag.CertificateData = mObjCertificateData;

                                    return View(pobjSealModel);
                                }
                            }
                            else
                            {
                                Transaction.Current.Rollback();

                                ViewBag.Message = dcSEDEResponse["Message"];
                                ViewBag.CertificateData = mObjCertificateData;
                                return View(pobjSealModel);
                            }



                        }
                        else
                        {
                            Transaction.Current.Rollback();
                            ViewBag.CertificateData = mObjCertificateData;
                            ViewBag.Message = mObjFuncResponse.Message;
                            return View(pobjSealModel);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.SendErrorToText(ex);
                        Transaction.Current.Rollback();
                        ViewBag.CertificateData = mObjCertificateData;
                        ErrorSignal.FromCurrentContext().Raise(ex);
                        ViewBag.Message = "Error occurred while saving validate certificate details";
                        return View(pobjSealModel);
                    }
                }
            }
        }

        [HttpGet]
        public ActionResult Issue(long? certid)
        {
            if (certid.GetValueOrDefault() > 0)
            {
                BLCertificate mObjBLCertificate = new BLCertificate();
                usp_GetCertificateDetails_Result mObjCertificateData = mObjBLCertificate.BL_GetCertificateDetails(certid.GetValueOrDefault());

                if (mObjCertificateData != null)
                {
                    ViewBag.CertificateData = mObjCertificateData;

                    MAP_Certificate_Issue mObjIssueData = mObjBLCertificate.BL_GetCertificateIssueDetails(mObjCertificateData.CertificateID);

                    IssueViewModel mObjIssueModel = new IssueViewModel()
                    {
                        CertificateID = mObjCertificateData.CertificateID,
                    };

                    if (mObjIssueData != null)
                    {
                        mObjIssueModel.CIID = mObjIssueData.CIID;
                        mObjIssueModel.IssueNotes = mObjIssueData.Notes;
                    }

                    return View(mObjIssueModel);
                }
                else
                {
                    return RedirectToAction("List", "ProcessCertificate");
                }
            }
            else
            {
                return RedirectToAction("List", "ProcessCertificate");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Issue(IssueViewModel pobjIssueModel)
        {
            BLCertificate mObjBLCertificate = new BLCertificate();
            usp_GetCertificateDetails_Result mObjCertificateData = mObjBLCertificate.BL_GetCertificateDetails(pobjIssueModel.CertificateID);

            if (!ModelState.IsValid)
            {
                ViewBag.CertificateData = mObjCertificateData;
                return View(pobjIssueModel);
            }
            else
            {
                using (TransactionScope mObjTransctionScope = new TransactionScope())
                {
                    try
                    {
                        //Update 
                        MAP_Certificate_Issue mObjIssue = new MAP_Certificate_Issue()
                        {
                            CIID = pobjIssueModel.CIID,
                            CertificateID = pobjIssueModel.CertificateID,
                            Notes = pobjIssueModel.IssueNotes,
                            StageID = (int)EnumList.CertificateStage.Issue,
                            ApprovalDate = CommUtil.GetCurrentDateTime(),
                            IsAction = true,
                            CreatedBy = SessionManager.UserID,
                            CreatedDate = CommUtil.GetCurrentDateTime()
                        };

                        FuncResponse<MAP_Certificate_Issue> mObjFuncResponse = mObjBLCertificate.BL_InsertUpdateCertificateIssue(mObjIssue);

                        if (mObjFuncResponse.Success)
                        {
                            mObjTransctionScope.Complete();
                            return RedirectToAction("Details", "ProcessCertificate", new { certid = pobjIssueModel.CertificateID });
                        }
                        else
                        {
                            Transaction.Current.Rollback();
                            ViewBag.CertificateData = mObjCertificateData;
                            ViewBag.Message = mObjFuncResponse.Message;
                            return View(pobjIssueModel);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.SendErrorToText(ex);
                        Transaction.Current.Rollback();
                        ViewBag.CertificateData = mObjCertificateData;
                        ErrorSignal.FromCurrentContext().Raise(ex);
                        ViewBag.Message = "Error occurred while saving validate certificate details";
                        return View(pobjIssueModel);
                    }
                }
            }
        }

        [HttpGet]
        public ActionResult Download(long? certid)
        {
            if (certid.GetValueOrDefault() > 0)
            {
                BLCertificate mObjBLCertificate = new BLCertificate();
                usp_GetCertificateDetails_Result mObjCertificateData = mObjBLCertificate.BL_GetCertificateDetails(certid.GetValueOrDefault());

                if (mObjCertificateData != null)
                {
                    return File(GlobalDefaultValues.DocumentLocation + mObjCertificateData.SealedPath, "application/force-download", mObjCertificateData.CertificateNumber.Trim() + ".pdf");
                }
                else
                {
                    return Content("Document Not Found");
                }
            }
            else
            {
                return Content("Document Not Found");
            }
        }

        public JsonResult Revoke(MAP_Certificate_Revoke pObjRevoke)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            BLCertificate mObjBLCertificate = new BLCertificate();

            usp_GetCertificateDetails_Result mObjRequestData = mObjBLCertificate.BL_GetCertificateDetails(pObjRevoke.CertificateID.GetValueOrDefault());

            if (mObjRequestData != null)
            {

                if (mObjRequestData.SEDE_OrderID > 0)
                {
                    //Cancel Order
                    IDictionary<string, object> dcAPIResponse = APICall.GetData(GlobalDefaultValues.SEDE_API_CancelDocument, new Dictionary<string, object> { ["OrderID"] = mObjRequestData.SEDE_OrderID.GetValueOrDefault() });

                    if (!TrynParse.parseBool(dcAPIResponse["success"]))
                    {
                        dcResponse["success"] = false;
                        dcResponse["Message"] = dcAPIResponse["Message"];
                        return Json(dcResponse, JsonRequestBehavior.AllowGet);
                    }
                }

                pObjRevoke.CreatedDate = CommUtil.GetCurrentDateTime();
                pObjRevoke.CreatedBy = SessionManager.UserID;

                FuncResponse mObjResponse = mObjBLCertificate.BL_RevokeCertificate(pObjRevoke);

                dcResponse["success"] = mObjResponse.Success;
                dcResponse["Message"] = mObjResponse.Message;
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