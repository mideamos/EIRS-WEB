using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
namespace EIRS.Web.Controllers
{
    public class TaxClearanceCertificateController : BaseController
    {
        // GET: TaxClearanceCertificate
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

            if (!string.IsNullOrEmpty(vFilter))
            {
                sbWhereCondition.Append(" AND ( dbo.GetTaxPayerName(tcc.TaxPayerID,tcc.TaxPayerTypeID) LIKE @MainFilter ");
                sbWhereCondition.Append(" OR tcc.TCCNumber LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(REPLACE(CONVERT(varchar(50),tcc.TCCDate,106),' ','-'),'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR Convert(varchar(50),tcc.TaxYear) LIKE @MainFilter ");
                sbWhereCondition.Append(" OR tstat.StatusName LIKE @MainFilter )");

            }

            TaxClearanceCertificate mObjTaxClearanceCertificate = new TaxClearanceCertificate()
            {
                WhereCondition = sbWhereCondition.ToString(),
                OrderBy = vSortColumn,
                OrderByDirection = vSortColumnDir,
                PageNumber = (TrynParse.parseInt(vStart) / TrynParse.parseInt(vLength)) + 1,
                PageSize = TrynParse.parseInt(vLength),
                MainFilter = !string.IsNullOrWhiteSpace(vFilter) ? "%" + vFilter.Trim() + "%" : vFilter

            };

            IDictionary<string, object> dcData = new BLTCC().BL_SearchTaxClearanceCertificate(mObjTaxClearanceCertificate);
            IList<usp_SearchTaxClearanceCertificate_Result> lstTCCDetails = (IList<usp_SearchTaxClearanceCertificate_Result>)dcData["TaxClearanceCertificateList"];

            return Json(new
            {
                draw = vDraw,
                recordsFiltered = dcData["FilteredRecords"],
                recordsTotal = dcData["TotalRecords"],
                data = lstTCCDetails
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add()
        {
            UI_FillYearDropDown();
            UI_FillTaxPayerTypeDropDown();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(TaxClearanceCertificateViewModel pObjTCCModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillYearDropDown();
                UI_FillTaxPayerTypeDropDown();
                return View(pObjTCCModel);
            }
            else
            {
                usp_GetTaxPayerLiabilityByTaxYear_Result mObjLiabilityData = new BLReport().BL_GetTaxPayerLiabilityByTaxYear(pObjTCCModel.TaxPayerTypeID, pObjTCCModel.TaxPayerID, pObjTCCModel.TaxYear);

                if (mObjLiabilityData.TotalPaymentAmount >= mObjLiabilityData.TotalAssessmentAmount)
                {

                    TaxClearanceCertificate mObjTCC = new TaxClearanceCertificate()
                    {
                        TCCID = 0,
                        TaxPayerID = pObjTCCModel.TaxPayerID,
                        TaxPayerTypeID = pObjTCCModel.TaxPayerTypeID,
                        TaxYear = pObjTCCModel.TaxYear,
                        RequestRefNo = pObjTCCModel.RequestRefNo,
                        IncomeSource = pObjTCCModel.SourceOfIncome,
                        SerialNumber = pObjTCCModel.SerialNumber,
                        TaxPayerDetails = pObjTCCModel.TaxPayerDetails,
                        StatusID = 1,
                        TCCDate = CommUtil.GetCurrentDateTime(),
                        CreatedBy = SessionManager.UserID,
                        CreatedDate = CommUtil.GetCurrentDateTime()
                    };

                    FuncResponse mObjFuncResponse = new BLTCC().BL_InsertTaxClearanceCertificate(mObjTCC);

                    if (mObjFuncResponse.Success)
                    {
                        return RedirectToAction("List", "TaxClearanceCertificate");
                    }
                    else
                    {
                        ViewBag.Message = mObjFuncResponse.Message;
                        UI_FillYearDropDown();
                        UI_FillTaxPayerTypeDropDown();
                        return View(pObjTCCModel);
                    }
                }
                else
                {
                    ViewBag.Message = "There is liability on tax payer. tcc cannot be generated";
                    UI_FillYearDropDown();
                    UI_FillTaxPayerTypeDropDown();
                    return View(pObjTCCModel);
                }
            }
        }

        public ActionResult Details(long? tccid)
        {
            if (tccid.GetValueOrDefault() > 0)
            {
                BLTCC mObjBLTCC = new BLTCC();

                usp_GetTaxClearanceCertificateDetails_Result mObjTCCData = mObjBLTCC.BL_GetTaxClearanceCertificateDetail(new TaxClearanceCertificate() { TCCID = tccid.GetValueOrDefault() });

                if (mObjTCCData != null)
                {
                    return View(mObjTCCData);
                }
                else
                {
                    return RedirectToAction("List", "TaxClearanceCertificate");
                }
            }
            else
            {
                return RedirectToAction("List", "TaxClearanceCertificate");
            }
        }

        public ActionResult GeneratePDF(long? tccid)
        {
            if (tccid.GetValueOrDefault() > 0)
            {
                BLTCC mObjBLTCC = new BLTCC();

                usp_GetTaxClearanceCertificateDetails_Result mObjTCCData = mObjBLTCC.BL_GetTaxClearanceCertificateDetail(new TaxClearanceCertificate() { TCCID = tccid.GetValueOrDefault() });

                if (mObjTCCData != null)
                {
                    usp_GetTCCDetailForGenerate_Result mObjTCCDetailForGenerateProcessData = mObjBLTCC.BL_GetTCCDetailForGenerateProcess(new TaxClearanceCertificate() { TaxPayerID = mObjTCCData.TaxPayerID, TaxPayerTypeID = mObjTCCData.TaxPayerTypeID, TaxYear = mObjTCCData.TaxYear, RequestRefNo = mObjTCCData.RequestRefNo });

                    string mStrDirectory = GlobalDefaultValues.DocumentLocation + "TaxClearance/";
                    string mStrGeneratedFileName = mObjTCCData.TCCID + "_" + DateTime.Now.ToString("_ddMMyyyy") + "_TCC.pdf";
                    string mStrGeneratedDocumentPath = Path.Combine(mStrDirectory, mStrGeneratedFileName);

                    if (!Directory.Exists(mStrDirectory))
                    {
                        Directory.CreateDirectory(mStrDirectory);
                    }

                    if (System.IO.File.Exists(mStrGeneratedDocumentPath))
                    {
                        System.IO.File.Delete(mStrGeneratedDocumentPath);
                    }

                    return File(mStrGeneratedDocumentPath, "application/pdf", mStrGeneratedFileName);
                }
                else
                {
                    return RedirectToAction("List", "TaxClearanceCertificate");
                }
            }
            else
            {
                return RedirectToAction("List", "TaxClearanceCertificate");
            }
        }
    }
}