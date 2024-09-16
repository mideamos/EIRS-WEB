using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;
using EIRS.BOL;
using EIRS.BLL;
using EIRS.Common;
using System.Web;
using System.Linq;
using System.IO;
using System;
using EIRS.Web.Models;
using Microsoft.Reporting.WebForms;

namespace EIRS.Web.Controllers
{

    public class ReportController : BaseController
    {
        // GET: Report
        public ActionResult List()
        {
            return View();
        }

        public ActionResult RevenueStream()
        {
            UI_FillYearDropDown();
            UI_FillMonthDropDown();
            return View();
        }

        [HttpPost]
        public ActionResult RevenueStream(FormCollection p_ObjFormCollection)
        {
            int intTaxYear = TrynParse.parseInt(p_ObjFormCollection.Get("cboYear"));
            int intMonth = TrynParse.parseInt(p_ObjFormCollection.Get("cboMonth"));


            IList<usp_RPT_GetRevenueStreamReport_Result> lstRevenueStreamReport = new BLReport().BL_GetRevenueStreamReport(intTaxYear, intMonth);
            return PartialView("GenerateRevenueStreamReport", lstRevenueStreamReport);
        }

        public ActionResult AssetType()
        {
            UI_FillYearDropDown();
            UI_FillMonthDropDown();
            return View();
        }

        [HttpPost]
        public ActionResult AssetType(FormCollection p_ObjFormCollection)
        {
            int intTaxYear = TrynParse.parseInt(p_ObjFormCollection.Get("cboYear"));
            int intMonth = TrynParse.parseInt(p_ObjFormCollection.Get("cboMonth"));


            IList<usp_RPT_GetAssetTypeReport_Result> lstAssetTypeReport = new BLReport().BL_GetAssetTypeReport(intTaxYear, intMonth);
            return PartialView("GenerateAssetTypeReport", lstAssetTypeReport);
        }

        public ActionResult Directorate()
        {
            UI_FillYearDropDown();
            UI_FillMonthDropDown();
            return View();
        }

        [HttpPost]
        public ActionResult Directorate(FormCollection p_ObjFormCollection)
        {
            int intTaxYear = TrynParse.parseInt(p_ObjFormCollection.Get("cboYear"));
            int intMonth = TrynParse.parseInt(p_ObjFormCollection.Get("cboMonth"));


            IList<usp_RPT_GetDirectorateReport_Result> lstDirectorateReport = new BLReport().BL_GetDirectorateReport(intTaxYear, intMonth);
            return PartialView("GenerateDirectorateReport", lstDirectorateReport);
        }

        public ActionResult TaxPayer()
        {
            UI_FillYearDropDown();
            UI_FillMonthDropDown();

            IList<DropDownListResult> lstTaxPayerType = new BLTaxPayerType().BL_GetTaxPayerTypeDropDownList(new TaxPayer_Types() { intStatus = 1 });
            ViewBag.TaxPayerTypeList = new SelectList(lstTaxPayerType, "id", "text");

            return View();
        }

        [HttpPost]
        public ActionResult TaxPayer(TaxPayerReportViewModel pObjTaxPayerReportModel)
        {
            if (pObjTaxPayerReportModel.TaxPayerTypeID == (int)EnumList.TaxPayerType.Individual)
            {
                usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(new Individual() { intStatus = 2, IndividualID = pObjTaxPayerReportModel.TaxPayerID });
                ViewBag.TaxPayerName = mObjIndividualData.FirstName + " " + mObjIndividualData.LastName;
            }
            else if (pObjTaxPayerReportModel.TaxPayerTypeID == (int)EnumList.TaxPayerType.Companies)
            {
                usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(new Company() { intStatus = 2, CompanyID = pObjTaxPayerReportModel.TaxPayerID });
                ViewBag.TaxPayerName = mObjCompanyData.CompanyName;
            }
            else if (pObjTaxPayerReportModel.TaxPayerTypeID == (int)EnumList.TaxPayerType.Government)
            {
                usp_GetGovernmentList_Result mObjGovernmentData = new BLGovernment().BL_GetGovernmentDetails(new Government() { intStatus = 2, GovernmentID = pObjTaxPayerReportModel.TaxPayerID });
                ViewBag.TaxPayerName = mObjGovernmentData.GovernmentName;
            }
            else if (pObjTaxPayerReportModel.TaxPayerTypeID == (int)EnumList.TaxPayerType.Special)
            {
                usp_GetSpecialList_Result mObjSpecialData = new BLSpecial().BL_GetSpecialDetails(new Special() { intStatus = 2, SpecialID = pObjTaxPayerReportModel.TaxPayerID });
                ViewBag.TaxPayerName = mObjSpecialData.SpecialTaxPayerName;
            }

            IList<usp_RPT_GetTaxPayerReport_Result> lstTaxPayerReport = new BLReport().BL_GetTaxPayerReport(pObjTaxPayerReportModel.TaxYear, pObjTaxPayerReportModel.TaxMonth.GetValueOrDefault(), pObjTaxPayerReportModel.TaxPayerTypeID, pObjTaxPayerReportModel.TaxPayerID);
            return PartialView("GenerateTaxPayerReport", lstTaxPayerReport);
        }

        public ActionResult DailySummary()
        {
            IList<SelectListItem> lstReference = new List<SelectListItem>()
            {
                new SelectListItem(){ Text = "Revenue Stream",Value = "1"},
                new SelectListItem(){ Text = "Asset Type",Value = "2"},
                new SelectListItem(){ Text = "Tax Office",Value = "3"},
                new SelectListItem(){ Text = "LGA",Value = "4"},
                new SelectListItem(){ Text = "Directorate",Value = "5"}
        };

            ViewBag.ReferenceList = lstReference;

            return View();
        }

        [HttpPost]
        public ActionResult DailySummary(FormCollection p_ObjFormCollection)
        {
            DateTime mdtReportDate = TrynParse.parseDatetime(p_ObjFormCollection.Get("txtReportDate"));
            int mIntReference = TrynParse.parseInt(p_ObjFormCollection.Get("cboReference"));

            IList<usp_GetDailySummaryReport_Result> lstDailySummaryReport = new BLReport().BL_GetDailySummaryReport(mdtReportDate, mIntReference);

            ViewBag.ReportDate = CommUtil.GetFormatedDate(mdtReportDate);

            return PartialView("GenerateDailySummary", lstDailySummaryReport);
        }

        public ActionResult DownloadDailySummary(string reportdate, int referenceid)
        {
            DateTime mdtReportDate = TrynParse.parseDatetime(reportdate);
            int mIntReference = referenceid;

            IList<usp_GetDailySummaryReport_Result> lstDailySummaryReport = new BLReport().BL_GetDailySummaryReport(mdtReportDate, mIntReference);

            string strReferenceName = "";
            switch (mIntReference)
            {
                case 1: strReferenceName = "Revenue Stream"; break;
                case 2: strReferenceName = "Asset Type"; break;
                case 3: strReferenceName = "Tax Office"; break;
                case 4: strReferenceName = "LGA"; break;
                case 5: strReferenceName = "Directorate"; break;
            }

            //Generating Invoice
            LocalReport localReport = new LocalReport
            {
                EnableExternalImages = true,
                EnableHyperlinks = true,
                ReportPath = Server.MapPath("~\\RDLC\\DailyReport.rdlc")
            };

            localReport.Refresh();
            localReport.DataSources.Clear();

            ReportParameter[] mObjReportParameter = new ReportParameter[2];
            mObjReportParameter[0] = new ReportParameter("rptReferenceName", strReferenceName);
            mObjReportParameter[1] = new ReportParameter("rptReportDate", CommUtil.GetFormatedDate(mdtReportDate));

            localReport.SetParameters(mObjReportParameter);
            localReport.Refresh();

            ReportDataSource mObjReportDataSource = new ReportDataSource("dsDailyReport");
            mObjReportDataSource.Value = lstDailySummaryReport;
            localReport.DataSources.Add(mObjReportDataSource);




            string strDirectory = "/DailySummaryReport/";
            string strfilename = DateTime.Now.ToString("ddMMyyyymmss_") + "DailyReport.pdf";

            string strExportFilePath = GlobalDefaultValues.DocumentLocation + strDirectory + strfilename;

            if (!Directory.Exists(GlobalDefaultValues.DocumentLocation + strDirectory))
            {
                Directory.CreateDirectory(GlobalDefaultValues.DocumentLocation + strDirectory);
            }

            if (System.IO.File.Exists(strExportFilePath))
                System.IO.File.Delete(strExportFilePath);

            // //CommUtil.RenderReportNStoreInFile(strExportFilePath, localReport, "PDF");

            return File(strExportFilePath, "application/pdf", strfilename);
        }

        public ActionResult MonthlySummary()
        {
            IList<SelectListItem> lstReference = new List<SelectListItem>()
            {
                new SelectListItem(){ Text = "Revenue Stream",Value = "1"},
                new SelectListItem(){ Text = "Asset Type",Value = "2"},
                new SelectListItem(){ Text = "Tax Office",Value = "3"},
                new SelectListItem(){ Text = "LGA",Value = "4"},
                new SelectListItem(){ Text = "Directorate",Value = "5"}
        };

            ViewBag.ReferenceList = lstReference;

            return View();
        }

        [HttpPost]
        public ActionResult MonthlySummary(FormCollection p_ObjFormCollection)
        {

            DateTime mdtReportDate = TrynParse.parseDatetime(p_ObjFormCollection.Get("txtReportDate"));
            int mIntReference = TrynParse.parseInt(p_ObjFormCollection.Get("cboReference"));

            IList<usp_GetMonthlySummaryReport_Result> lstMonthlySummaryReport = new BLReport().BL_GetMonthlySummaryReport(mdtReportDate, mIntReference);
            return PartialView("GenerateMonthlySummary", lstMonthlySummaryReport);
        }

        public ActionResult DownloadMonthlySummary(string reportdate, int referenceid)
        {
            DateTime mdtReportDate = TrynParse.parseDatetime(reportdate);
            int mIntReference = referenceid;

            string strReferenceName = "";
            switch (mIntReference)
            {
                case 1: strReferenceName = "Revenue Stream"; break;
                case 2: strReferenceName = "Asset Type"; break;
                case 3: strReferenceName = "Tax Office"; break;
                case 4: strReferenceName = "LGA"; break;
                case 5: strReferenceName = "Directorate"; break;
            }

            IList<usp_GetMonthlySummaryReport_Result> lstMonthlySummaryReport = new BLReport().BL_GetMonthlySummaryReport(mdtReportDate, mIntReference);

            //Generating Invoice
            LocalReport localReport = new LocalReport
            {
                EnableExternalImages = true,
                EnableHyperlinks = true,
                ReportPath = Server.MapPath("~\\RDLC\\MonthlyReport.rdlc")
            };

            localReport.Refresh();
            localReport.DataSources.Clear();

            ReportParameter[] mObjReportParameter = new ReportParameter[2];
            mObjReportParameter[0] = new ReportParameter("rptReferenceName", strReferenceName);
            mObjReportParameter[1] = new ReportParameter("rptReportMonth", mdtReportDate.ToString("MMM-yyyy"));

            localReport.SetParameters(mObjReportParameter);
            localReport.Refresh();

            ReportDataSource mObjReportDataSource = new ReportDataSource("dsMonthlyReport");
            mObjReportDataSource.Value = lstMonthlySummaryReport;
            localReport.DataSources.Add(mObjReportDataSource);

            string strDirectory = "/MonthlySummaryReport/";
            string strfilename = DateTime.Now.ToString("ddMMyyyymmss_") + "MonthlyReport.pdf";

            string strExportFilePath = GlobalDefaultValues.DocumentLocation + strDirectory + strfilename;

            if (!Directory.Exists(GlobalDefaultValues.DocumentLocation + strDirectory))
            {
                Directory.CreateDirectory(GlobalDefaultValues.DocumentLocation + strDirectory);
            }

            if (System.IO.File.Exists(strExportFilePath))
                System.IO.File.Delete(strExportFilePath);

            ////CommUtil.RenderReportNStoreInFile(strExportFilePath, localReport, "PDF");

            return File(strExportFilePath, "application/pdf", strfilename);
        }
    }
}