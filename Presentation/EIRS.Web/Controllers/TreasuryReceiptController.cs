using Aspose.BarCode.Generation;
using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Web.Models;
using iTextSharp.text.pdf.qrcode;
using Microsoft.Extensions.Primitives;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Web.Configuration;
using System.Web.Mvc;

namespace EIRS.Web.Controllers
{
    public class TreasuryReceiptController : BaseController
    {
        BLUser mObjBLUser;
        // GET: TreasuryReceipt
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(ReviseBillViewModel pObjAddModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjAddModel);
            }
            else
            {
                if (pObjAddModel.BillRefNo.StartsWith("AB"))
                {
                    BLAssessment mObjBLAssessment = new BLAssessment();
                    usp_GetAssessmentList_Result mObjAssessmentData = mObjBLAssessment.BL_GetAssessmentDetails(new Assessment() { AssessmentRefNo = pObjAddModel.BillRefNo, IntStatus = 2 });

                    if (mObjAssessmentData != null)
                    {
                        return RedirectToAction("Assessment", "TreasuryReceipt", new { id = mObjAssessmentData.AssessmentID, name = mObjAssessmentData.AssessmentRefNo.ToSeoUrl() });
                    }
                    else
                    {
                        ViewBag.Message = "Invalid Bill Ref No";
                        return View(pObjAddModel);
                    }

                }
                else if (pObjAddModel.BillRefNo.StartsWith("SB"))
                {
                    BLServiceBill mObjBLServiceBill = new BLServiceBill();
                    usp_GetServiceBillList_Result mObjServiceBillData = mObjBLServiceBill.BL_GetServiceBillDetails(new ServiceBill() { ServiceBillRefNo = pObjAddModel.BillRefNo, IntStatus = 2 });

                    if (mObjServiceBillData != null)
                    {
                        if (mObjServiceBillData.SettlementStatusID == (int)EnumList.SettlementStatus.Settled)
                        {
                            return RedirectToAction("ServiceBill", "TreasuryReceipt", new { id = mObjServiceBillData.ServiceBillID, name = mObjServiceBillData.ServiceBillRefNo.ToSeoUrl() });
                        }
                        else
                        {
                            ViewBag.Message = "Bill is not settled";
                            return View(pObjAddModel);
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Invalid Bill Ref No";
                        return View(pObjAddModel);
                    }
                }
                else
                {
                    ViewBag.Message = "Invalid Bill Ref No";
                    return View(pObjAddModel);
                }
            }
        }

        [HttpGet]
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
                    IList<usp_GetTreasuryReceiptList_Result> lstReceipt = new BLTreasuryReceipt().BL_GetTreasuryReceiptList(new Treasury_Receipt() { ServiceBillID = -1, AssessmentID = mObjAssessmentData.AssessmentID.GetValueOrDefault() });
                    IList<usp_GetSettlementWithoutReceipt_Result> lstSettlementWithoutReceipt = new BLTreasuryReceipt().BL_GetSettlementWithoutReceipt(mObjAssessmentData.AssessmentID.GetValueOrDefault(), -1);

                    ViewBag.MAPAssessmentRules = lstMAPAssessmentRules;
                    ViewBag.AssessmentItems = lstAssessmentItems;
                    ViewBag.AssessmentRuleSettlement = lstAssessmentRuleSettlement;
                    ViewBag.SettlementList = lstSettlement;
                    ViewBag.AdjustmentList = lstAssessmentAdjustment;
                    ViewBag.LateChargeList = lstAssessmentLateCharge;
                    ViewBag.ReceiptList = lstReceipt;
                    ViewBag.SettlementWithoutReceipt = lstSettlementWithoutReceipt;

                    return View(mObjAssessmentData);
                }
                else
                {
                    return RedirectToAction("Add", "TreasuryReceipt");
                }
            }
            else
            {
                return RedirectToAction("Add", "TreasuryReceipt");
            }
        }

        [HttpGet]
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
                    IList<usp_GetTreasuryReceiptList_Result> lstReceipt = new BLTreasuryReceipt().BL_GetTreasuryReceiptList(new Treasury_Receipt() { AssessmentID = -1, ServiceBillID = mObjServiceBillData.ServiceBillID.GetValueOrDefault() });
                    IList<usp_GetSettlementWithoutReceipt_Result> lstSettlementWithoutReceipt = new BLTreasuryReceipt().BL_GetSettlementWithoutReceipt(-1, mObjServiceBillData.ServiceBillID.GetValueOrDefault());

                    ViewBag.MAPServiceBillRules = lstMAPServiceBillServices;
                    ViewBag.ServiceBillItems = lstServiceBillItems;
                    ViewBag.ServiceBillRuleSettlement = lstMDAServiceSettlement;
                    ViewBag.SettlementList = lstSettlement;
                    ViewBag.AdjustmentList = lstServiceBillAdjustment;
                    ViewBag.LateChargeList = lstServiceBillLateCharge;
                    ViewBag.ReceiptList = lstReceipt;
                    ViewBag.SettlementWithoutReceipt = lstSettlementWithoutReceipt;

                    return View(mObjServiceBillData);
                }
                else
                {
                    return RedirectToAction("Add", "TreasuryReceipt");
                }
            }
            else
            {
                return RedirectToAction("Add", "TreasuryReceipt");
            }
        }

        [HttpPost]
        public JsonResult AddReceipt(Treasury_Receipt pObjReceipt)
        {
            BLTreasuryReceipt mObjBLTreasuryReceipt = new BLTreasuryReceipt();
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            pObjReceipt.ReceiptDate = CommUtil.GetCurrentDateTime();
            pObjReceipt.StatusID = 3;
            pObjReceipt.CreatedDate = CommUtil.GetCurrentDateTime();
            pObjReceipt.CreatedBy = SessionManager.UserID;

            FuncResponse<Treasury_Receipt> mObjResponse = mObjBLTreasuryReceipt.BL_InsertTreasuryReceipt(pObjReceipt);

            if (mObjResponse.Success)
            {
                if (!string.IsNullOrWhiteSpace(pObjReceipt.SettlementIds))
                {
                    string[] strSettlement = pObjReceipt.SettlementIds.Split(new char[] { ',' });
                    MAP_TreasuryReceipt_Settlement mObjTRS;
                    foreach (var item in strSettlement)
                    {
                        mObjTRS = new MAP_TreasuryReceipt_Settlement()
                        {
                            ReceiptID = mObjResponse.AdditionalData.ReceiptID,
                            SettlementID = TrynParse.parseInt(item),
                            CreatedBy = SessionManager.UserID,
                            CreatedDate = CommUtil.GetCurrentDateTime()
                        };

                        mObjBLTreasuryReceipt.BL_InsertReceiptSettlement(mObjTRS);
                    }
                }

                //Generate Treasury Receipt
                usp_GetTreasuryReceiptList_Result mObjTreasuryData = mObjBLTreasuryReceipt.BL_GetTreasuryReceiptDetails(new Treasury_Receipt() { ReceiptID = mObjResponse.AdditionalData.ReceiptID });
                if (mObjTreasuryData != null)
                {
                    string mStrDirectory = $"{GlobalDefaultValues.DocumentLocation}/TreasuryReceipt/{mObjTreasuryData.ReceiptID}/";
                    string mStrGeneratedFileName = $"{mObjTreasuryData.ReceiptRefNo}_{DateTime.Now:ddMMyyyy}_Generated.pdf";//mObjTreasuryData.ReceiptRefNo + "_" + DateTime.Now.ToString("_ddMMyyyy") + "_TR.pdf";
                    string mStrGeneratedDocumentPath = Path.Combine(mStrDirectory, mStrGeneratedFileName);

                    if (!Directory.Exists(mStrDirectory))
                    {
                        Directory.CreateDirectory(mStrDirectory);
                    }
                    string strRevenueStreamName = "", SigBase = "", SigBase64 = "", strRevenueSubStreamName = "", strAgencyName = "", strContactAddress = "", strContactNumber = "", strTaxYear = "";
                    decimal dcBillAmount = 0, dcOutstandingAmount = 0;

                    if (mObjTreasuryData.BillTypeID == 1)
                    {
                        BLAssessment mObjBLAssessment = new BLAssessment();

                        usp_GetAssessmentList_Result mObjAssessmentData = mObjBLAssessment.BL_GetAssessmentDetails(new BOL.Assessment() { AssessmentID = mObjTreasuryData.ASID.GetValueOrDefault(), IntStatus = 2 });
                        IList<usp_GetAssessment_AssessmentRuleList_Result> lstMAPAssessmentRules = mObjBLAssessment.BL_GetAssessmentRules(mObjAssessmentData.AssessmentID.GetValueOrDefault());
                        IList<usp_GetAssessmentRuleItemList_Result> lstAssessmentItems = mObjBLAssessment.BL_GetAssessmentRuleItem((int)mObjTreasuryData.ASID.GetValueOrDefault());
                        IList<usp_GetAssessmentAdjustmentList_Result> lstAssessmentAdjustment = mObjBLAssessment.BL_GetAssessmentAdjustment(mObjAssessmentData.AssessmentID.GetValueOrDefault());
                        IList<usp_GetAssessmentLateChargeList_Result> lstAssessmentLateCharge = mObjBLAssessment.BL_GetAssessmentLateCharge(mObjAssessmentData.AssessmentID.GetValueOrDefault());

                        strRevenueStreamName = string.Join(",", lstAssessmentItems.Select(t => t.RevenueStreamName).Distinct().ToArray());
                        strRevenueSubStreamName = string.Join(",", lstAssessmentItems.Select(t => t.RevenueSubStreamName).Distinct().ToArray());
                        strAgencyName = string.Join(",", lstAssessmentItems.Select(t => t.AgencyName).Distinct().ToArray());
                        strContactNumber = mObjAssessmentData.TaxPayerMobile;
                        strContactAddress = mObjAssessmentData.TaxPayerAddress;
                        dcBillAmount = mObjAssessmentData.AssessmentAmount.GetValueOrDefault() + lstAssessmentAdjustment.Sum(o => o.Amount.Value) + lstAssessmentLateCharge.Sum(o => o.TotalAmount.Value);
                        dcOutstandingAmount = mObjAssessmentData.AssessmentAmount.GetValueOrDefault() + lstAssessmentAdjustment.Sum(o=>o.Amount.Value)+ lstAssessmentLateCharge.Sum(o=>o.TotalAmount.Value) - mObjAssessmentData.SettlementAmount.GetValueOrDefault();
                        strTaxYear = lstMAPAssessmentRules.FirstOrDefault().TaxYear.ToString();

                        SigBase = BrCode(mObjAssessmentData.AssessmentRefNo, mObjAssessmentData.TaxPayerName, mObjAssessmentData.TaxPayerRIN, dcBillAmount.ToString(), mObjResponse.AdditionalData.ReceiptAmount.ToString(), dcOutstandingAmount.ToString(), 1);
                        SigBase64 = $"data:image/png;base64,{SigBase}";
                        string mHtmlDirectory = $"{DocumentHTMLLocation}/TreasuryReceipt.html";
                        //string mHtmlDirectory = $"{DocumentHTMLLocation}/newTreasuryReceipt.html";
                        HtmlToPdf pdf = new HtmlToPdf();

                        // set converter options
                        pdf.Options.PdfPageSize = PdfPageSize.A4;
                        pdf.Options.PdfPageOrientation = PdfPageOrientation.Portrait;

                        pdf.Options.WebPageWidth = 0;
                        pdf.Options.WebPageHeight = 0;
                        pdf.Options.WebPageFixedSize = false;

                        pdf.Options.AutoFitWidth = HtmlToPdfPageFitMode.NoAdjustment;
                        pdf.Options.AutoFitHeight = HtmlToPdfPageFitMode.NoAdjustment;
                        string marksheet = string.Empty;
                        marksheet = System.IO.File.ReadAllText(mHtmlDirectory);
                        marksheet = marksheet.Replace("@@BillAmount@@", CommUtil.GetFormatedCurrency(dcBillAmount))
                                             .Replace("@@AgencyName@@", strAgencyName)
                                             .Replace("@@TaxPayerRIN@@", mObjAssessmentData.TaxPayerRIN)
                                             .Replace("@@TaxPayerName@@", mObjAssessmentData.TaxPayerName)
                                             .Replace("@@ContactAddress@@", strContactAddress)
                                             .Replace("@@ContactNumber@@", strContactNumber)
                                             .Replace("@@TaxPayerTypeName@@", mObjAssessmentData.TaxPayerTypeName)
                                             .Replace("@@BillRef@@", mObjAssessmentData.AssessmentRefNo.ToString())
                                             .Replace("@@ReceiptAmount@@", CommUtil.GetFormatedCurrency(mObjResponse.AdditionalData.ReceiptAmount))
                                             .Replace("@@BillOutstandingAmount@@", CommUtil.GetFormatedCurrency(dcOutstandingAmount))
                                             .Replace("@@Notes@@", mObjResponse.AdditionalData.Notes)
                                             .Replace("@@SignerName@@", "")
                                             .Replace("@@ReceiptYear@@", strTaxYear)
                                             .Replace("@@QRCode@@", SigBase64)
                                             .Replace("@@ReceiptNumber@@", mObjResponse.AdditionalData.ReceiptRefNo)
                                             .Replace("@@ReceiptDate@@", CommUtil.GetFormatedFullDate(mObjResponse.AdditionalData.ReceiptDate));

                        PdfDocument doc = pdf.ConvertHtmlString(marksheet);
                        var bytes = doc.Save();
                        //return File(bytes, "application/pdf", "TR.pdf");

                        System.IO.File.WriteAllBytes(mStrGeneratedDocumentPath, bytes);

                    }
                    else if (mObjTreasuryData.BillTypeID == 2)
                    {
                        BLServiceBill mObjBLServiceBill = new BLServiceBill();

                        usp_GetServiceBillList_Result mObjServiceBillData = mObjBLServiceBill.BL_GetServiceBillDetails(new BOL.ServiceBill() { ServiceBillID = mObjTreasuryData.ASID.GetValueOrDefault(), IntStatus = 2 });
                        IList<usp_GetServiceBill_MDAServiceList_Result> lstMAPServiceBillServices = mObjBLServiceBill.BL_GetMDAServiceList(mObjServiceBillData.ServiceBillID.GetValueOrDefault());
                        IList<usp_GetServiceBillItemList_Result> lstServiceBillItems = mObjBLServiceBill.BL_GetServiceBillItem((int)mObjTreasuryData.ASID.GetValueOrDefault());
                        IList<usp_GetServiceBillAdjustmentList_Result> lstServiceBillAdjustment = mObjBLServiceBill.BL_GetServiceBillAdjustment(mObjServiceBillData.ServiceBillID.GetValueOrDefault());
                        IList<usp_GetServiceBillLateChargeList_Result> lstServiceBillLateCharge = mObjBLServiceBill.BL_GetServiceBillLateCharge(mObjServiceBillData.ServiceBillID.GetValueOrDefault());
                        strRevenueStreamName = string.Join(",", lstServiceBillItems.Select(t => t.RevenueStreamName).Distinct().ToArray());
                        strRevenueSubStreamName = string.Join(",", lstServiceBillItems.Select(t => t.RevenueSubStreamName).Distinct().ToArray());
                        strAgencyName = string.Join(",", lstServiceBillItems.Select(t => t.AgencyName).Distinct().ToArray());

                        strContactNumber = mObjServiceBillData.TaxPayerMobile;
                        strContactAddress = mObjServiceBillData.TaxPayerAddress;
                        dcBillAmount = mObjServiceBillData.ServiceBillAmount.GetValueOrDefault() + lstServiceBillAdjustment.Sum(o => o.Amount.Value) + lstServiceBillLateCharge.Sum(o => o.TotalAmount.Value);
                        dcOutstandingAmount = mObjServiceBillData.ServiceBillAmount.GetValueOrDefault() + lstServiceBillAdjustment .Sum(o=>o.Amount.Value)+ lstServiceBillLateCharge .Sum(o=>o.TotalAmount.Value)- mObjServiceBillData.SettlementAmount.GetValueOrDefault();
                        strTaxYear = lstMAPServiceBillServices.FirstOrDefault().TaxYear.ToString();
                        string mHtmlDirectory = $"{DocumentHTMLLocation}/TreasuryReceipt.html";
                        //string mHtmlDirectory = $"{DocumentHTMLLocation}/newTreasuryReceipt.html";
                        SigBase = BrCode(mObjServiceBillData.ServiceBillRefNo, mObjServiceBillData.TaxPayerName, mObjServiceBillData.TaxPayerRIN, CommUtil.GetFormatedCurrency(dcBillAmount), CommUtil.GetFormatedCurrency(mObjResponse.AdditionalData.ReceiptAmount), CommUtil.GetFormatedCurrency(dcOutstandingAmount), 2);
                        SigBase64 = $"data:image/png;base64,{SigBase}";
                        HtmlToPdf pdf = new HtmlToPdf();
                        // set converter options
                        pdf.Options.PdfPageSize = PdfPageSize.A4;
                        pdf.Options.PdfPageOrientation = PdfPageOrientation.Portrait;

                        pdf.Options.WebPageWidth = 1024;
                        pdf.Options.WebPageHeight = 0;
                        pdf.Options.WebPageFixedSize = false;

                        pdf.Options.AutoFitWidth = HtmlToPdfPageFitMode.AutoFit;
                        pdf.Options.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
                        string marksheet = string.Empty;
                        marksheet = System.IO.File.ReadAllText(mHtmlDirectory);
                        marksheet = marksheet.Replace("@@BillAmount@@", CommUtil.GetFormatedCurrency(dcBillAmount))
                                             .Replace("@@AgencyName@@", strAgencyName)
                                             .Replace("@@TaxPayerRIN@@", mObjServiceBillData.TaxPayerRIN)
                                             .Replace("@@TaxPayerName@@", mObjServiceBillData.TaxPayerName)
                                             .Replace("@@ContactAddress@@", strContactAddress)
                                             .Replace("@@ContactNumber@@", strContactNumber)
                                             .Replace("@@TaxPayerTypeName@@", mObjServiceBillData.TaxPayerTypeName)
                                             .Replace("@@BillRef@@", mObjServiceBillData.ServiceBillRefNo.ToString())
                                             .Replace("@@ReceiptAmount@@", CommUtil.GetFormatedCurrency(mObjResponse.AdditionalData.ReceiptAmount))
                                             .Replace("@@BillOutstandingAmount@@", CommUtil.GetFormatedCurrency(dcOutstandingAmount))
                                             .Replace("@@Notes@@", mObjResponse.AdditionalData.Notes)
                                             .Replace("@@SignerName@@", "")
                                             .Replace("@@ReceiptYear@@", strTaxYear)
                                             .Replace("@@QRCode@@", SigBase64)
                                             .Replace("@@ReceiptNumber@@", mObjResponse.AdditionalData.ReceiptRefNo)
                                             .Replace("@@ReceiptDate@@", CommUtil.GetFormatedFullDate(mObjResponse.AdditionalData.ReceiptDate));

                        PdfDocument doc = pdf.ConvertHtmlString(marksheet);
                        var bytes = doc.Save();

                        if (!System.IO.File.Exists(mStrGeneratedDocumentPath))
                        {
                            System.IO.File.WriteAllBytes(mStrGeneratedDocumentPath, bytes);
                        }
                    }

                    mObjBLTreasuryReceipt.BL_UpdateTRGenerated(new Treasury_Receipt { ReceiptID = mObjResponse.AdditionalData.ReceiptID, GeneratedPath = $"TreasuryReceipt/{mObjTreasuryData.ReceiptID}/{mStrGeneratedFileName}" });

                }



                Audit_Log mObjAuditLog = new Audit_Log()
                {
                    LogDate = CommUtil.GetCurrentDateTime(),
                    ASLID = (int)EnumList.ALScreen.Settle_Treasury_Receipt_Add,
                    Comment = $"Treasury Receipt added with ref no. {mObjResponse.AdditionalData.ReceiptRefNo} and Amount {mObjResponse.AdditionalData.ReceiptAmount}",
                    IPAddress = CommUtil.GetIPAddress(),
                    StaffID = SessionManager.UserID,
                };

                new BLAuditLog().BL_InsertAuditLog(mObjAuditLog);
            }


            dcResponse["success"] = mObjResponse.Success;
            dcResponse["Message"] = mObjResponse.Message;

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SignReceipt(int? id, string name)
        {
            usp_GetTreasuryReceiptList_Result mObjTreasuryReceiptData = new BLTreasuryReceipt().BL_GetTreasuryReceiptDetails(new Treasury_Receipt() { ReceiptID = id.GetValueOrDefault() });
            if (mObjTreasuryReceiptData != null)
            {
                ViewBag.TreasuryReceiptData = mObjTreasuryReceiptData;


                TreasuryReceiptSignViewModel mObjTreasuryReceiptModel = new TreasuryReceiptSignViewModel()
                {
                    ReceiptID = mObjTreasuryReceiptData.ReceiptID.GetValueOrDefault()
                };

                usp_GetUserList_Result mObjUserData = new BLUser().BL_GetUserDetails(new MST_Users() { UserID = SessionManager.UserID, intStatus = 2 });

                if (mObjUserData != null)
                {

                    mObjTreasuryReceiptModel.SavedSignaturePath = !string.IsNullOrWhiteSpace(mObjUserData.SignaturePath) ? mObjUserData.SignaturePath : "";
                }

                return View(mObjTreasuryReceiptModel);
            }
            else
            {
                return RedirectToAction("List", "TreasuryReceipt");
            }
        }

        [HttpPost]
        public JsonResult SignReceipt(TreasuryReceiptSignViewModel pObjSignModel)
        {

            //Stream image = LoadBase64(pObjSignModel.ImgSrc).s;
            //var reducedSize = ReduceImageSize(0.5, image, imageString);
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (!ModelState.IsValid)
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "All Fields are Required";
            }
            else if (pObjSignModel.SignSourceID == 0)
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Treasury Receipt is not Signed";
            }
            else
            {
                if (pObjSignModel.SignSourceID == 1)
                {
                    MST_Users mObjUsers = new MST_Users()
                    {
                        UserID = SessionManager.UserID,
                        intStatus = 2
                    };

                    if (mObjBLUser == null)
                        mObjBLUser = new BLUser();

                    usp_GetUserList_Result mObjUserData = mObjBLUser.BL_GetUserDetails(mObjUsers);

                    if (mObjUserData != null)
                    {
                        pObjSignModel.ImgSrc = GlobalDefaultValues.DocumentLocation + "/" + mObjUserData.SignaturePath;
                    }
                }
                BLTreasuryReceipt mObjBLTreasuryReceipt = new BLTreasuryReceipt();
                usp_GetTreasuryReceiptList_Result mObjTreasuryReceiptData = mObjBLTreasuryReceipt.BL_GetTreasuryReceiptDetails(new Treasury_Receipt() { ReceiptID = pObjSignModel.ReceiptID });

                string mStrTempDirectory = $"{GlobalDefaultValues.DocumentLocation}/TreasuryReceipt/{mObjTreasuryReceiptData.ReceiptID}/Temp";
                string mStrTempSignFileName = $"{Guid.NewGuid():N}.pdf";
                string mStrTempSignDocumentPath = Path.Combine(mStrTempDirectory, mStrTempSignFileName);

                if (!Directory.Exists(mStrTempDirectory))
                {
                    Directory.CreateDirectory(mStrTempDirectory);
                }

                string strRevenueStreamName = "", strRevenueSubStreamName = "", SigBase = "", SigBase64 = "", strAgencyName = "", strContactAddress = "", strContactNumber = "", strTaxYear = "";
                decimal dcBillAmount = 0, dcOutstandingAmount = 0;
                string mStrSignDirectory = $"{GlobalDefaultValues.DocumentLocation}/TreasuryReceipt/{mObjTreasuryReceiptData.ReceiptID}/";
                string mStrSignedFileName = $"{mObjTreasuryReceiptData.ReceiptRefNo}_{DateTime.Now:ddMMyyyy}_Signed.pdf";
                string mStrSingedDocumentPath = Path.Combine(mStrSignDirectory, mStrSignedFileName);

                if (!Directory.Exists(mStrSignDirectory))
                {
                    Directory.CreateDirectory(mStrSignDirectory);
                }

                if (System.IO.File.Exists(mStrSingedDocumentPath))
                {
                    System.IO.File.Delete(mStrSingedDocumentPath);
                }
                //Final Save and Return
                Treasury_Receipt mObjUpdateTR = new Treasury_Receipt()
                {
                    ReceiptID = mObjTreasuryReceiptData.ReceiptID.GetValueOrDefault(),
                    SignedPath = $"TreasuryReceipt/{mObjTreasuryReceiptData.ReceiptID}/{mStrSignedFileName}",
                    StatusID = 1,
                    Notes = pObjSignModel.SignNotes,
                    SignImgSrc = pObjSignModel.ImgSrc,
                    SignSourceID = pObjSignModel.SignSourceID,
                    ModifiedDate = CommUtil.GetCurrentDateTime(),
                    ModifiedBy = SessionManager.UserID,

                };

                FuncResponse mObjFuncResponse = mObjBLTreasuryReceipt.BL_UpdateTRSigned(mObjUpdateTR);
                if (mObjFuncResponse.Success)
                {
                    string mStrDirectory = $"{GlobalDefaultValues.DocumentLocation}/TreasuryReceipt/{mObjTreasuryReceiptData.ReceiptID}/";
                    string mStrGeneratedFileName = $"{mObjTreasuryReceiptData.ReceiptRefNo}_{DateTime.Now:ddMMyyyy}_Generated.pdf";//mObjTreasuryData.ReceiptRefNo + "_" + DateTime.Now.ToString("_ddMMyyyy") + "_TR.pdf";
                    string mStrSignFileName = $"{mObjTreasuryReceiptData.ReceiptRefNo}_{DateTime.Now:ddMMyyyy}_Signed.pdf";//mObjTreasuryData.ReceiptRefNo + "_" + DateTime.Now.ToString("_ddMMyyyy") + "_TR.pdf";
                    string mStrGeneratedDocumentPath = Path.Combine(mStrDirectory, mStrGeneratedFileName);
                    string mStrSignDocumentPath = Path.Combine(mStrDirectory, mStrSignFileName);

                    if (!Directory.Exists(mStrDirectory))
                    {
                        Directory.CreateDirectory(mStrDirectory);
                    }
                    //to delete the generated one
                    if (System.IO.File.Exists(mStrGeneratedDocumentPath))
                    {
                        System.IO.File.Delete(mStrGeneratedDocumentPath);
                    }

                    if (mObjTreasuryReceiptData.BillTypeID == 1)
                    {
                        BLAssessment mObjBLAssessment = new BLAssessment();

                        usp_GetAssessmentList_Result mObjAssessmentData = mObjBLAssessment.BL_GetAssessmentDetails(new BOL.Assessment() { AssessmentID = mObjTreasuryReceiptData.ASID.GetValueOrDefault(), IntStatus = 2 });
                        IList<usp_GetAssessment_AssessmentRuleList_Result> lstMAPAssessmentRules = mObjBLAssessment.BL_GetAssessmentRules(mObjAssessmentData.AssessmentID.GetValueOrDefault());
                        IList<usp_GetAssessmentRuleItemList_Result> lstAssessmentItems = mObjBLAssessment.BL_GetAssessmentRuleItem((int)mObjTreasuryReceiptData.ASID.GetValueOrDefault());
                        IList<usp_GetAssessmentAdjustmentList_Result> lstAssessmentAdjustment = mObjBLAssessment.BL_GetAssessmentAdjustment(mObjAssessmentData.AssessmentID.GetValueOrDefault());
                        IList<usp_GetAssessmentLateChargeList_Result> lstAssessmentLateCharge = mObjBLAssessment.BL_GetAssessmentLateCharge(mObjAssessmentData.AssessmentID.GetValueOrDefault());

                        strRevenueStreamName = string.Join(",", lstAssessmentItems.Select(t => t.RevenueStreamName).Distinct().ToArray());
                        strRevenueSubStreamName = string.Join(",", lstAssessmentItems.Select(t => t.RevenueSubStreamName).Distinct().ToArray());
                        strAgencyName = string.Join(",", lstAssessmentItems.Select(t => t.AgencyName).Distinct().ToArray());
                        strContactNumber = mObjAssessmentData.TaxPayerMobile;
                        strContactAddress = mObjAssessmentData.TaxPayerAddress;
                        dcBillAmount = mObjAssessmentData.AssessmentAmount.GetValueOrDefault() + lstAssessmentAdjustment.Sum(o => o.Amount.Value) + lstAssessmentLateCharge.Sum(o => o.TotalAmount.Value);
                        dcOutstandingAmount = mObjAssessmentData.AssessmentAmount.GetValueOrDefault() + lstAssessmentAdjustment.Sum(o => o.Amount.Value) + lstAssessmentLateCharge.Sum(o => o.TotalAmount.Value) - mObjAssessmentData.SettlementAmount.GetValueOrDefault();

                        //dcBillAmount = mObjAssessmentData.AssessmentAmount.GetValueOrDefault();
                        //dcOutstandingAmount = mObjAssessmentData.AssessmentAmount.GetValueOrDefault() - mObjAssessmentData.SettlementAmount.GetValueOrDefault();
                        strTaxYear = lstMAPAssessmentRules.FirstOrDefault().TaxYear.ToString();
                        SigBase = BrCode(mObjAssessmentData.AssessmentRefNo, mObjAssessmentData.TaxPayerName, mObjAssessmentData.TaxPayerRIN, dcBillAmount.ToString(), mObjTreasuryReceiptData.ReceiptAmount.ToString(), dcOutstandingAmount.ToString(), 1);
                        SigBase64 = $"data:image/png;base64,{SigBase}";

                        string mHtmlDirectory = $"{DocumentHTMLLocation}/TreasuryReceipt.html";
                        HtmlToPdf pdf = new HtmlToPdf();
                        // set converter options
                        pdf.Options.PdfPageSize = PdfPageSize.A4;
                        pdf.Options.PdfPageOrientation = PdfPageOrientation.Portrait;

                        pdf.Options.WebPageWidth = 1024;
                        pdf.Options.WebPageHeight = 0;
                        pdf.Options.WebPageFixedSize = false;

                        pdf.Options.AutoFitWidth = HtmlToPdfPageFitMode.ShrinkOnly;
                        pdf.Options.AutoFitHeight = HtmlToPdfPageFitMode.NoAdjustment;
                        string marksheet = string.Empty;
                        marksheet = System.IO.File.ReadAllText(mHtmlDirectory);
                        marksheet = marksheet.Replace("@@BillAmount@@", CommUtil.GetFormatedCurrency(dcBillAmount))
                                             .Replace("@@AgencyName@@", strAgencyName)
                                             .Replace("@@TaxPayerRIN@@", mObjAssessmentData.TaxPayerRIN)
                                             .Replace("@@TaxPayerName@@", mObjAssessmentData.TaxPayerName)
                                             .Replace("@@ContactAddress@@", strContactAddress)
                                             .Replace("@@ContactNumber@@", strContactNumber)
                                             .Replace("@@TaxPayerTypeName@@", mObjAssessmentData.TaxPayerTypeName)
                                             .Replace("@@BillRef@@", mObjAssessmentData.AssessmentRefNo)
                                             .Replace("@@ReceiptAmount@@", CommUtil.GetFormatedCurrency(mObjTreasuryReceiptData.ReceiptAmount))
                                             .Replace("@@BillOutstandingAmount@@", CommUtil.GetFormatedCurrency(dcOutstandingAmount))
                                             .Replace("@@Notes@@", mObjAssessmentData.AssessmentNotes)
                                             .Replace("@@ReceiptYear@@", strTaxYear)
                                             .Replace("@@SignerName@@", SessionManager.ContactName)
                                             .Replace("@@Signature@@", pObjSignModel.ImgSrc)
                                             .Replace("@@QRCode@@", SigBase64)
                                             .Replace("@@ReceiptNumber@@", mObjTreasuryReceiptData.ReceiptRefNo)
                                             .Replace("@@ReceiptDate@@", CommUtil.GetFormatedFullDate(mObjTreasuryReceiptData.ReceiptDate));

                        PdfDocument doc = pdf.ConvertHtmlString(marksheet);
                        var bytes = doc.Save();
                        //return File(bytes, "application/pdf", "TR.pdf");

                        System.IO.File.WriteAllBytes(mStrSignDocumentPath, bytes);
                    }
                    else if (mObjTreasuryReceiptData.BillTypeID == 2)
                    {
                        BLServiceBill mObjBLServiceBill = new BLServiceBill();

                        usp_GetServiceBillList_Result mObjServiceBillData = mObjBLServiceBill.BL_GetServiceBillDetails(new BOL.ServiceBill() { ServiceBillID = mObjTreasuryReceiptData.ASID.GetValueOrDefault(), IntStatus = 2 });
                        IList<usp_GetServiceBill_MDAServiceList_Result> lstMAPServiceBillServices = mObjBLServiceBill.BL_GetMDAServiceList(mObjServiceBillData.ServiceBillID.GetValueOrDefault());
                        IList<usp_GetServiceBillItemList_Result> lstServiceBillItems = mObjBLServiceBill.BL_GetServiceBillItem((int)mObjTreasuryReceiptData.ASID.GetValueOrDefault());
                        IList<usp_GetServiceBillAdjustmentList_Result> lstServiceBillAdjustment = mObjBLServiceBill.BL_GetServiceBillAdjustment(mObjServiceBillData.ServiceBillID.GetValueOrDefault());
                        IList<usp_GetServiceBillLateChargeList_Result> lstServiceBillLateCharge = mObjBLServiceBill.BL_GetServiceBillLateCharge(mObjServiceBillData.ServiceBillID.GetValueOrDefault());
                        strRevenueStreamName = string.Join(",", lstServiceBillItems.Select(t => t.RevenueStreamName).Distinct().ToArray());
                        strRevenueSubStreamName = string.Join(",", lstServiceBillItems.Select(t => t.RevenueSubStreamName).Distinct().ToArray());
                        strAgencyName = string.Join(",", lstServiceBillItems.Select(t => t.AgencyName).Distinct().ToArray());
                        dcBillAmount = mObjServiceBillData.ServiceBillAmount.GetValueOrDefault() + lstServiceBillAdjustment.Sum(o => o.Amount.Value) + lstServiceBillLateCharge.Sum(o => o.TotalAmount.Value);
                        dcOutstandingAmount = mObjServiceBillData.ServiceBillAmount.GetValueOrDefault() + lstServiceBillAdjustment.Sum(o => o.Amount.Value) + lstServiceBillLateCharge.Sum(o => o.TotalAmount.Value) - mObjServiceBillData.SettlementAmount.GetValueOrDefault();

                        strContactNumber = mObjServiceBillData.TaxPayerMobile;
                        strContactAddress = mObjServiceBillData.TaxPayerAddress;
                        dcBillAmount = mObjServiceBillData.ServiceBillAmount.GetValueOrDefault();
                        dcOutstandingAmount = mObjServiceBillData.ServiceBillAmount.GetValueOrDefault() - mObjServiceBillData.SettlementAmount.GetValueOrDefault();
                        strTaxYear = lstMAPServiceBillServices.FirstOrDefault().TaxYear.ToString();
                        SigBase = BrCode(mObjServiceBillData.ServiceBillRefNo, mObjServiceBillData.TaxPayerName, mObjServiceBillData.TaxPayerRIN, CommUtil.GetFormatedCurrency(dcBillAmount), CommUtil.GetFormatedCurrency(mObjTreasuryReceiptData.ReceiptAmount), CommUtil.GetFormatedCurrency(dcOutstandingAmount), 2);
                        SigBase64 = $"data:image/png;base64,{SigBase}";
                        string mHtmlDirectory = $"{DocumentHTMLLocation}/TreasuryReceipt.html";
                        HtmlToPdf pdf = new HtmlToPdf();
                        // set converter options
                        pdf.Options.PdfPageSize = PdfPageSize.A4;
                        pdf.Options.PdfPageOrientation = PdfPageOrientation.Portrait;

                        pdf.Options.WebPageWidth = 1024;
                        pdf.Options.WebPageHeight = 0;
                        pdf.Options.WebPageFixedSize = false;

                        pdf.Options.AutoFitWidth = HtmlToPdfPageFitMode.ShrinkOnly;
                        pdf.Options.AutoFitHeight = HtmlToPdfPageFitMode.NoAdjustment;
                        string marksheet = string.Empty;
                        marksheet = System.IO.File.ReadAllText(mHtmlDirectory);
                        marksheet = marksheet.Replace("@@BillAmount@@", CommUtil.GetFormatedCurrency(dcBillAmount))
                                             .Replace("@@AgencyName@@", strAgencyName)
                                             .Replace("@@TaxPayerRIN@@", mObjServiceBillData.TaxPayerRIN)
                                             .Replace("@@TaxPayerName@@", mObjServiceBillData.TaxPayerName)
                                             .Replace("@@ContactAddress@@", strContactAddress)
                                             .Replace("@@ContactNumber@@", strContactNumber)
                                             .Replace("@@TaxPayerTypeName@@", mObjServiceBillData.TaxPayerTypeName)
                                             .Replace("@@BillRef@@", mObjServiceBillData.ServiceBillRefNo.ToString())
                                             .Replace("@@ReceiptAmount@@", CommUtil.GetFormatedCurrency(mObjTreasuryReceiptData.ReceiptAmount))
                                             .Replace("@@BillOutstandingAmount@@", CommUtil.GetFormatedCurrency(dcOutstandingAmount))
                                             .Replace("@@Notes@@", mObjServiceBillData.Notes)
                                             .Replace("@@ReceiptYear@@", strTaxYear)
                                             .Replace("@@SignerName@@", SessionManager.ContactName)
                                             .Replace("@@Signature@@", pObjSignModel.ImgSrc)
                                             .Replace("@@QRCode@@", SigBase64)
                                             .Replace("@@ReceiptNumber@@", mObjTreasuryReceiptData.ReceiptRefNo)
                                             .Replace("@@ReceiptDate@@", CommUtil.GetFormatedFullDate(mObjTreasuryReceiptData.ReceiptDate));

                        PdfDocument doc = pdf.ConvertHtmlString(marksheet);
                        var bytes = doc.Save();
                        //return File(bytes, "application/pdf", "TR.pdf");

                        System.IO.File.WriteAllBytes(mStrSignDocumentPath, bytes);
                    }


                }
                if (mObjFuncResponse.Success)
                {
                    dcResponse["success"] = true;
                    dcResponse["Message"] = "Receipt Signed Succcessfully";

                    if (Directory.Exists(mStrTempDirectory))
                    {
                        Directory.Delete(mStrTempDirectory, true);
                    }
                }
                else
                {
                    dcResponse["success"] = false;
                    dcResponse["Message"] = mObjFuncResponse.Message;
                }



            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult FixedSign(int ReceiptID, string ImgSrc, int SignSourceID)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            usp_GetTreasuryReceiptList_Result mObjTreasuryReceiptData = new BLTreasuryReceipt().BL_GetTreasuryReceiptDetails(new Treasury_Receipt() { ReceiptID = ReceiptID });

            if (mObjTreasuryReceiptData != null)
            {
                string mStrDirectory = $"{GlobalDefaultValues.DocumentLocation}/TreasuryReceipt/{mObjTreasuryReceiptData.ReceiptID}/Temp";
                string mStrSignFileName = $"{Guid.NewGuid():N}.pdf";
                string mStrSignDocumentPath = Path.Combine(mStrDirectory, mStrSignFileName);

                if (!Directory.Exists(mStrDirectory))
                {
                    Directory.CreateDirectory(mStrDirectory);
                }

                string strRevenueStreamName = "", strRevenueSubStreamName = "", strAgencyName = "", strContactAddress = "", strContactNumber = "", strTaxYear = "";
                decimal dcBillAmount = 0, dcOutstandingAmount = 0;

                if (mObjTreasuryReceiptData.BillTypeID == 1)
                {
                    BLAssessment mObjBLAssessment = new BLAssessment();

                    usp_GetAssessmentList_Result mObjAssessmentData = mObjBLAssessment.BL_GetAssessmentDetails(new BOL.Assessment() { AssessmentID = mObjTreasuryReceiptData.ASID.GetValueOrDefault(), IntStatus = 2 });
                    IList<usp_GetAssessment_AssessmentRuleList_Result> lstMAPAssessmentRules = mObjBLAssessment.BL_GetAssessmentRules(mObjAssessmentData.AssessmentID.GetValueOrDefault());
                    IList<usp_GetAssessmentRuleItemList_Result> lstAssessmentItems = mObjBLAssessment.BL_GetAssessmentRuleItem((int)mObjTreasuryReceiptData.ASID.GetValueOrDefault());

                    strRevenueStreamName = string.Join(",", lstAssessmentItems.Select(t => t.RevenueStreamName).Distinct().ToArray());
                    strRevenueSubStreamName = string.Join(",", lstAssessmentItems.Select(t => t.RevenueSubStreamName).Distinct().ToArray());
                    strAgencyName = string.Join(",", lstAssessmentItems.Select(t => t.AgencyName).Distinct().ToArray());
                    strContactNumber = mObjAssessmentData.TaxPayerMobile;
                    strContactAddress = mObjAssessmentData.TaxPayerAddress;
                    dcBillAmount = mObjAssessmentData.AssessmentAmount.GetValueOrDefault();
                    dcOutstandingAmount = mObjAssessmentData.AssessmentAmount.GetValueOrDefault() - mObjAssessmentData.SettlementAmount.GetValueOrDefault();
                    strTaxYear = lstMAPAssessmentRules.FirstOrDefault().TaxYear.ToString();
                }
                else if (mObjTreasuryReceiptData.BillTypeID == 2)
                {
                    BLServiceBill mObjBLServiceBill = new BLServiceBill();

                    usp_GetServiceBillList_Result mObjServiceBillData = mObjBLServiceBill.BL_GetServiceBillDetails(new BOL.ServiceBill() { ServiceBillID = mObjTreasuryReceiptData.ASID.GetValueOrDefault(), IntStatus = 2 });

                    IList<usp_GetServiceBill_MDAServiceList_Result> lstMAPServiceBillServices = mObjBLServiceBill.BL_GetMDAServiceList(mObjServiceBillData.ServiceBillID.GetValueOrDefault());
                    IList<usp_GetServiceBillItemList_Result> lstServiceBillItems = mObjBLServiceBill.BL_GetServiceBillItem((int)mObjTreasuryReceiptData.ASID.GetValueOrDefault());
                    strRevenueStreamName = string.Join(",", lstServiceBillItems.Select(t => t.RevenueStreamName).Distinct().ToArray());
                    strRevenueSubStreamName = string.Join(",", lstServiceBillItems.Select(t => t.RevenueSubStreamName).Distinct().ToArray());
                    strAgencyName = string.Join(",", lstServiceBillItems.Select(t => t.AgencyName).Distinct().ToArray());

                    strContactNumber = mObjServiceBillData.TaxPayerMobile;
                    strContactAddress = mObjServiceBillData.TaxPayerAddress;
                    dcBillAmount = mObjServiceBillData.ServiceBillAmount.GetValueOrDefault();
                    dcOutstandingAmount = mObjServiceBillData.ServiceBillAmount.GetValueOrDefault() - mObjServiceBillData.SettlementAmount.GetValueOrDefault();
                    strTaxYear = lstMAPServiceBillServices.FirstOrDefault().TaxYear.ToString();
                }
                //var bytes = doc.Save();
                ////return File(bytes, "application/pdf", "TR.pdf");

                //System.IO.File.WriteAllBytes(mStrSignDocumentPath, bytes);
                dcResponse["PDFPath"] = $"/Document/TreasuryReceipt/{mObjTreasuryReceiptData.ReceiptID}/Temp/{mStrSignFileName}";
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Details(int? id, string name)
        {
            usp_GetTreasuryReceiptList_Result mObjTreasuryReceiptData = new BLTreasuryReceipt().BL_GetTreasuryReceiptDetails(new Treasury_Receipt() { ReceiptID = id.GetValueOrDefault(-1) });
            if (mObjTreasuryReceiptData != null)
            {
                return View(mObjTreasuryReceiptData);
            }
            else
            {
                return RedirectToAction("List", "TreasuryReceipt");
            }
        }

        [HttpGet]
        public ActionResult Download(int? id, string name)
        {
            usp_GetTreasuryReceiptList_Result mObjTreasuryReceiptData = new BLTreasuryReceipt().BL_GetTreasuryReceiptDetails(new Treasury_Receipt() { ReceiptID = id.GetValueOrDefault(-1) });

            if (mObjTreasuryReceiptData != null)
            {
                if (mObjTreasuryReceiptData.ReceiptStatusID == 1 && string.IsNullOrWhiteSpace(mObjTreasuryReceiptData.SignedPath) && string.IsNullOrWhiteSpace(mObjTreasuryReceiptData.GeneratedPath))
                {

                    return Content("Old Treasury Receipt - No PDF File");

                }
                else
                {
                    return File(GlobalDefaultValues.DocumentLocation + "/" + (mObjTreasuryReceiptData.ReceiptStatusID == 1 ? mObjTreasuryReceiptData.SignedPath : mObjTreasuryReceiptData.GeneratedPath), "application/force-download", $"{mObjTreasuryReceiptData.ReceiptRefNo}.pdf");
                }

            }
            else
            {
                return Content("File Not Found");
            }
        }

        private string BrCode(string param1Refno, string param2txpName, string param3Rin, string billAmount, string recAmount, string outstandingAmount, int type)
        {
            string SigBase64, billType = "";
            if (type == 1)
                billType = "Assessment";
            else
                billType = "Service";

            StringBuilder sbBillSummary = new StringBuilder();
            sbBillSummary.Append("EIRS : BILLS");
            sbBillSummary.Append("\n");
            sbBillSummary.Append("Tax Payer Name");
            sbBillSummary.Append(" : ");
            sbBillSummary.Append(param2txpName);
            sbBillSummary.Append("\n");
            sbBillSummary.Append("Tax Payer RIN");
            sbBillSummary.Append(" : ");
            sbBillSummary.Append(param3Rin);
            sbBillSummary.Append("\n");
            sbBillSummary.Append("Bill ID");
            sbBillSummary.Append(" : ");
            sbBillSummary.Append(param1Refno);
            sbBillSummary.Append("\n");
            sbBillSummary.Append("Bill Amount");
            sbBillSummary.Append(" :  ");
            sbBillSummary.Append($"₦ {billAmount}");
            sbBillSummary.Append("\n");
            sbBillSummary.Append(" Receipt Amount ");
            sbBillSummary.Append(" : ");
            sbBillSummary.Append($"₦ {recAmount}");
            string dataDir = "directoryPath";
            string mbarcodeHtmlDirectory = $"{documentLocation}/BarCodes/{param1Refno}/";
            string mStrGeneratedFileName = "output.Jpeg";
            string mStrGeneratedDocumentPath = Path.Combine(mbarcodeHtmlDirectory, mStrGeneratedFileName);

            BarcodeGenerator generator = new BarcodeGenerator(EncodeTypes.QR, sbBillSummary.ToString());
            generator.Parameters.Barcode.XDimension.Millimeters = 1f;

            if (!Directory.Exists(mbarcodeHtmlDirectory))
            {
                Directory.CreateDirectory(mbarcodeHtmlDirectory);
                generator.Save(mStrGeneratedDocumentPath, BarCodeImageFormat.Jpeg);
            }
            using (System.Drawing.Image image = System.Drawing.Image.FromFile(mStrGeneratedDocumentPath))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();
                    SigBase64 = Convert.ToBase64String(imageBytes);
                }
            }
            return SigBase64;
        }
        private static Image LoadBase64(string base64)
        {
            byte[] bytes = Convert.FromBase64String(base64);
            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
            }
            return image;
        }
        private void ReduceImageSize(double scaleFactor, Stream sourcePath, string targetPath)
        {
            using (var image = System.Drawing.Image.FromStream(sourcePath))
            {
                var newWidth = (int)(image.Width * scaleFactor);
                var newHeight = (int)(image.Height * scaleFactor);
                var thumbnailImg = new Bitmap(newWidth, newHeight);
                var thumbGraph = Graphics.FromImage(thumbnailImg);
                thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
                thumbGraph.DrawImage(image, imageRectangle);
                thumbnailImg.Save(targetPath, image.RawFormat);
            }
        }

        static string DocumentHTMLLocation = WebConfigurationManager.AppSettings["documentHTMLLocation"] ?? "";
        static string documentLocation = WebConfigurationManager.AppSettings["documentLocation"] ?? "";
    }
}