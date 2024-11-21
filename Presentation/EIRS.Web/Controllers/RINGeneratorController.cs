using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Web.Models;
using EIRS.Web.Utility;
using GemBox.Spreadsheet;
using OfficeOpenXml;
using OfficeOpenXml.DataValidation;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using ExcelWorksheet = OfficeOpenXml.ExcelWorksheet;

namespace EIRS.Web.Controllers
{
    public class RINGeneratorController : BaseController
    {

        public ActionResult List()
        {
            return View();
        }


        public ActionResult UploadCompany()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]

        public ActionResult UploadCompany(RINGeneratorViewModel pCompanyRGViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            else
            {
                if (pCompanyRGViewModel.ExcelFile.ContentLength > 0)
                {
                    string strActualFileName = pCompanyRGViewModel.ExcelFile.FileName;
                    string strUploadedFileExt = strActualFileName.Substring(strActualFileName.LastIndexOf('.') + 1);
                    string[] strDocFormats = new string[] { "xls", "xlsx" };
                    if (strDocFormats.Contains(strUploadedFileExt.ToLower()))
                    {
                        string strInputFileName = "CompanyData_" + DateTime.Now.ToString("dd_MM_yyyy") + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + "." + strUploadedFileExt;
                        string strOutputFileName = "CompanyResult_" + DateTime.Now.ToString("dd_MM_yyyy") + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + "." + strUploadedFileExt;
                        string fileLocation = GlobalDefaultValues.DocumentLocation + "RINGenerator/Company";

                        if (!(Directory.Exists(fileLocation)))
                        {
                            Directory.CreateDirectory(fileLocation);
                        }

                        pCompanyRGViewModel.ExcelFile.SaveAs(fileLocation + "/" + strInputFileName);

                        DataTable dtCompanyDetails;

                        using (ExcelPackage mObjExcelPackage = new ExcelPackage(new FileInfo(fileLocation + "/" + strInputFileName)))
                        {
                            if (mObjExcelPackage.Workbook.Worksheets.Count == 2)
                            {

                                dtCompanyDetails = mObjExcelPackage.Workbook.Worksheets[2].ToDataTable();

                                if (dtCompanyDetails.Rows.Count > 0)
                                {

                                    //check if column exists
                                    if (dtCompanyDetails.Columns.Contains("CompanyName") && dtCompanyDetails.Columns.Contains("CompanyTIN") && dtCompanyDetails.Columns.Contains("MobileNo1")
                                        && dtCompanyDetails.Columns.Contains("MobileNo2") && dtCompanyDetails.Columns.Contains("EmailAddress1") && dtCompanyDetails.Columns.Contains("EmailAddress2")
                                        && dtCompanyDetails.Columns.Contains("TaxOffice") && dtCompanyDetails.Columns.Contains("EconomicActivity") && dtCompanyDetails.Columns.Contains("PreferredNotification")
                                        && dtCompanyDetails.Columns.Contains("ContactAddress"))
                                    {

                                        // Adding Required Column
                                        dtCompanyDetails.Columns.Add("TaxOfficeID", typeof(int));
                                        dtCompanyDetails.Columns.Add("EconomicActivitiesID", typeof(int));
                                        dtCompanyDetails.Columns.Add("NotificationMethodID", typeof(int));
                                        dtCompanyDetails.Columns.Add("Status", typeof(string));
                                        dtCompanyDetails.Columns.Add("RIN");
                                        dtCompanyDetails.Columns.Add("Result");

                                        //Process and Start Uploading Data
                                        BLCompany mObjBLCompany = new BLCompany();
                                        Company mObjCompany;
                                        FuncResponse<Company> mObjFuncResponse;
                                        foreach (DataRow drData in dtCompanyDetails.Rows)
                                        {
                                            try
                                            {



                                                string strTaxOffice = drData["TaxOffice"].ToString();
                                                string[] strArrTaxOffice = strTaxOffice.Split(':');

                                                if (strArrTaxOffice.Length == 2)
                                                {
                                                    drData["TaxOfficeID"] = TrynParse.parseInt(strArrTaxOffice[0].Trim());
                                                }
                                                else
                                                {
                                                    drData["TaxOfficeID"] = 0;
                                                }

                                                string strEconomicActivity = drData["EconomicActivity"].ToString();
                                                string[] strArrEconomicActivity = strEconomicActivity.Split(':');

                                                if (strArrEconomicActivity.Length == 2)
                                                {
                                                    drData["EconomicActivitiesID"] = TrynParse.parseInt(strArrEconomicActivity[0].Trim());
                                                }
                                                else
                                                {
                                                    drData["EconomicActivitiesID"] = 0;
                                                }

                                                string strNotificationMethod = drData["PreferredNotification"].ToString();
                                                string[] strArrNotificationMethod = strNotificationMethod.Split(':');

                                                if (strArrNotificationMethod.Length == 2)
                                                {
                                                    drData["NotificationMethodID"] = TrynParse.parseInt(strArrNotificationMethod[0].Trim());
                                                }
                                                else
                                                {
                                                    drData["NotificationMethodID"] = 0;
                                                }

                                                //Do Validation
                                                if (!string.IsNullOrEmpty(TrynParse.parseStringForExcel(drData["CompanyName"]))
                                                    && !string.IsNullOrEmpty(TrynParse.parseStringForExcel(drData["MobileNo1"]))
                                                    && !string.IsNullOrEmpty(TrynParse.parseStringForExcel(drData["ContactAddress"]))
                                                    && TrynParse.parseInt(drData["EconomicActivitiesID"]) > 0 && TrynParse.parseInt(drData["NotificationMethodID"]) > 0)
                                                {
                                                    mObjCompany = new Company()
                                                    {
                                                        CompanyID = 0,
                                                        CompanyName = TrynParse.parseStringForExcel(drData["CompanyName"]),
                                                        TIN = TrynParse.parseStringForExcel(drData["CompanyTIN"]),
                                                        MobileNumber1 = TrynParse.parseStringForExcel(drData["MobileNo1"]),
                                                        MobileNumber2 = drData.IsNull("MobileNo2") ? null : TrynParse.parseStringForExcel(drData["MobileNo2"]),
                                                        EmailAddress1 = drData.IsNull("EmailAddress1") ? null : TrynParse.parseStringForExcel(drData["EmailAddress1"]),
                                                        EmailAddress2 = drData.IsNull("EmailAddress2") ? null : TrynParse.parseStringForExcel(drData["EmailAddress2"]),
                                                        TaxOfficeID = TrynParse.parseInt(drData["TaxOfficeID"]),
                                                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies,
                                                        EconomicActivitiesID = TrynParse.parseInt(drData["EconomicActivitiesID"]),
                                                        NotificationMethodID = TrynParse.parseInt(drData["NotificationMethodID"]),
                                                        ContactAddress = TrynParse.parseStringForExcel(drData["ContactAddress"]),
                                                        Active = true,
                                                        CreatedBy = SessionManager.UserID,
                                                        CreatedDate = CommUtil.GetCurrentDateTime()
                                                    };

                                                    mObjFuncResponse = mObjBLCompany.BL_InsertUpdateCompany(mObjCompany, false);

                                                    if (mObjFuncResponse.Success)
                                                    {
                                                        drData["Status"] = "Success";
                                                        drData["Result"] = mObjFuncResponse.Message;
                                                        drData["RIN"] = mObjFuncResponse.AdditionalData.CompanyRIN;
                                                    }
                                                    else
                                                    {
                                                        drData["Status"] = "Failed";
                                                        drData["Result"] = mObjFuncResponse.Message;
                                                    }
                                                }
                                                else
                                                {
                                                    drData["Status"] = "Failed";
                                                    drData["Result"] = "Invalid Data";
                                                }
                                            }
                                            catch (Exception Ex)
                                            {
                                                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
                                                drData["Status"] = "Failed";
                                                drData["Result"] = "Error Occurred";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ViewBag.Message = "Invalid Excel";
                                        return View();
                                    }
                                }
                                else
                                {
                                    ViewBag.Message = "No Records found for RIN Generation";
                                    return View();
                                }
                            }
                            else
                            {
                                ViewBag.Message = "Invalid Excel";
                                return View();
                            }
                        }

                        using (ExcelPackage mObjExcelPackage = new ExcelPackage())
                        {
                            dtCompanyDetails.Columns.Remove("TaxOfficeID");
                            dtCompanyDetails.Columns.Remove("EconomicActivitiesID");
                            dtCompanyDetails.Columns.Remove("NotificationMethodID");
                            OfficeOpenXml.ExcelWorksheet ObjExcelWorksheet = mObjExcelPackage.Workbook.Worksheets.Add("Company");
                            ObjExcelWorksheet.Cells["A1"].LoadFromDataTable(dtCompanyDetails, true);

                            var vPHeaderRow = ObjExcelWorksheet.Row(1);

                            var vPHeaderStyle = vPHeaderRow.Style.Fill;
                            vPHeaderStyle.PatternType = ExcelFillStyle.Solid;
                            vPHeaderStyle.BackgroundColor.SetColor(Color.Green);

                            vPHeaderRow.Style.Font.Color.SetColor(Color.White);
                            vPHeaderRow.Style.Font.Bold = true;
                            vPHeaderRow.Style.Font.Size = 12;
                            vPHeaderRow.Style.Font.Name = "Calibri";

                            vPHeaderRow.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            vPHeaderRow.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            ObjExcelWorksheet.Row(1).Height = 22.50;

                            var vPHeaderBorder = vPHeaderRow.Style.Border;
                            vPHeaderBorder.Bottom.Style = vPHeaderBorder.Top.Style = vPHeaderBorder.Left.Style = vPHeaderBorder.Right.Style = ExcelBorderStyle.Thin;

                            vPHeaderRow.Style.WrapText = false;
                            vPHeaderRow.Style.ShrinkToFit = false;

                            ObjExcelWorksheet.Cells.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            ObjExcelWorksheet.Cells.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            ObjExcelWorksheet.Cells.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            ObjExcelWorksheet.Cells.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                            //var vDataRow = ObjExcelWorksheet.Cells[2, 1, dtCompanyDetails.Rows.Count + 1, 13];
                            //vDataRow.Style..Height = 22.50;
                            //var vDataBorder = vDataRow.Style.Border;
                            //vDataBorder.Bottom.Style = vDataBorder.Top.Style = vDataBorder.Left.Style = vDataBorder.Right.Style = ExcelBorderStyle.Thin;

                            for (int intColCount = 1; intColCount <= 13; intColCount++)
                            {
                                ObjExcelWorksheet.Column(intColCount).Style.WrapText = false;
                                ObjExcelWorksheet.Column(intColCount).Style.ShrinkToFit = false;
                                ObjExcelWorksheet.Column(intColCount).BestFit = true;
                                ObjExcelWorksheet.Column(intColCount).AutoFit();
                            }

                            mObjExcelPackage.SaveAs(new FileInfo(fileLocation + "/" + strOutputFileName));

                            ViewBag.SMessage = "Upload Completed Successfully.";
                            ViewBag.ResultFilePath = "/Document/RINGenerator/Company/" + strOutputFileName;
                            return View();
                            //byte[] mByteData = mObjExcelPackage.GetAsByteArray();
                            //string strfilename = "CompanyResult_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xlsx";
                            //return File(mByteData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", strfilename);
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Select excel document to upload";
                        return View();
                    }
                }
                else
                {
                    ViewBag.Message = "Invalid Excel File";
                    return View();
                }
            }
        }

        public ActionResult DownloadCompanyTemplate()
        {
            using (ExcelPackage mObjExcelPackage = new ExcelPackage())
            {
                mObjExcelPackage.Workbook.Worksheets.Add("ReferenceData");
                mObjExcelPackage.Workbook.Worksheets.Add("Company");

                mObjExcelPackage.Workbook.Worksheets[1].Hidden = eWorkSheetHidden.VeryHidden;

                mObjExcelPackage.Workbook.Protection.LockStructure = true;
                mObjExcelPackage.Workbook.Protection.SetPassword(CommUtil.GenerateUniqueNumber().ToString());

                //Updating Reference Data Sheet
                IList<ExcelFormulaModel> lstExcelFormula = AddCompanyReferenceData(mObjExcelPackage.Workbook.Worksheets[1]);

                //Creating Names for Formula
                foreach (var item in lstExcelFormula)
                {
                    var vExcelRange = mObjExcelPackage.Workbook.Worksheets[1].Cells[item.FromRow, item.FromCol, item.ToRow, item.ToCol];
                    mObjExcelPackage.Workbook.Names.Add(item.Name, vExcelRange);
                }

                AddCompanyScheme(mObjExcelPackage.Workbook.Worksheets[2]);


                //Generate A File with Random name
                byte[] mByteData = mObjExcelPackage.GetAsByteArray();
                string strfilename = "CompanyData_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xlsx";
                return File(mByteData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", strfilename);
            }
        }

        public IList<ExcelFormulaModel> AddCompanyReferenceData(ExcelWorksheet pObjExcelWorksheet)
        {
            IList<ExcelFormulaModel> lstExcelFormula = new List<ExcelFormulaModel>();

            var vPHeaderRow = pObjExcelWorksheet.Row(1);

            var vPHeaderStyle = vPHeaderRow.Style.Fill;
            vPHeaderStyle.PatternType = ExcelFillStyle.Solid;
            vPHeaderStyle.BackgroundColor.SetColor(Color.Green);

            vPHeaderRow.Style.Font.Color.SetColor(Color.White);
            vPHeaderRow.Style.Font.Bold = true;
            vPHeaderRow.Style.Font.Size = 12;
            vPHeaderRow.Style.Font.Name = "Calibri";

            vPHeaderRow.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            vPHeaderRow.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            pObjExcelWorksheet.Row(1).Height = 22.50;

            var vPHeaderBorder = vPHeaderRow.Style.Border;
            vPHeaderBorder.Bottom.Style = vPHeaderBorder.Top.Style = vPHeaderBorder.Left.Style = vPHeaderBorder.Right.Style = ExcelBorderStyle.Thin;

            vPHeaderRow.Style.WrapText = false;
            vPHeaderRow.Style.ShrinkToFit = false;

            pObjExcelWorksheet.Cells[1, 1].Value = "Tax Office";
            pObjExcelWorksheet.Cells[1, 2].Value = "Economic Activities";
            pObjExcelWorksheet.Cells[1, 3].Value = "Notification Method";

            int intRowIndex = 1;
            IList<DropDownListResult> lstTaxOffice = new BLTaxOffice().BL_GetTaxOfficeDropDownList(new BOL.Tax_Offices() { intStatus = 1 });
            foreach (var item in lstTaxOffice)
            {
                intRowIndex++;
                var vDataRow = pObjExcelWorksheet.Row(intRowIndex);
                vDataRow.Height = 22.50;
                var vDataBorder = vDataRow.Style.Border;
                vDataBorder.Bottom.Style = vDataBorder.Top.Style = vDataBorder.Left.Style = vDataBorder.Right.Style = ExcelBorderStyle.Thin;

                pObjExcelWorksheet.Cells[intRowIndex, 1].Value = item.id + " : " + item.text;
            }
            lstExcelFormula.Add(new ExcelFormulaModel() { Name = "TaxOffice", FromRow = 2, FromCol = 1, ToRow = intRowIndex, ToCol = 1 });

            IList<DropDownListResult> lstEconomicActivities = new BLEconomicActivities().BL_GetEconomicActivitiesDropDownList(new BOL.Economic_Activities() { intStatus = 1, TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies });

            intRowIndex = 1;

            foreach (var item in lstEconomicActivities)
            {
                intRowIndex++;
                var vDataRow = pObjExcelWorksheet.Row(intRowIndex);
                vDataRow.Height = 22.50;
                var vDataBorder = vDataRow.Style.Border;
                vDataBorder.Bottom.Style = vDataBorder.Top.Style = vDataBorder.Left.Style = vDataBorder.Right.Style = ExcelBorderStyle.Thin;

                pObjExcelWorksheet.Cells[intRowIndex, 2].Value = item.id + " : " + item.text;
            }

            lstExcelFormula.Add(new ExcelFormulaModel() { Name = "EconomicActivities", FromRow = 2, FromCol = 2, ToRow = intRowIndex, ToCol = 2 });

            IList<DropDownListResult> lstNotificationMethod = new BLNotificationMethod().BL_GetNotificationMethodDropDownList(new BOL.Notification_Method() { intStatus = 1 });

            intRowIndex = 1;

            foreach (var item in lstNotificationMethod)
            {
                intRowIndex++;
                var vDataRow = pObjExcelWorksheet.Row(intRowIndex);
                vDataRow.Height = 22.50;
                var vDataBorder = vDataRow.Style.Border;
                vDataBorder.Bottom.Style = vDataBorder.Top.Style = vDataBorder.Left.Style = vDataBorder.Right.Style = ExcelBorderStyle.Thin;

                pObjExcelWorksheet.Cells[intRowIndex, 3].Value = item.id + " : " + item.text;
            }

            lstExcelFormula.Add(new ExcelFormulaModel() { Name = "NotificationMethod", FromRow = 2, FromCol = 3, ToRow = intRowIndex, ToCol = 3 });

            pObjExcelWorksheet.Cells.AutoFitColumns(100);
            pObjExcelWorksheet.Protection.IsProtected = true;
            pObjExcelWorksheet.Protection.AllowDeleteColumns = false;
            pObjExcelWorksheet.Protection.AllowDeleteRows = false;
            pObjExcelWorksheet.Protection.AllowEditObject = false;
            pObjExcelWorksheet.Protection.AllowEditScenarios = false;
            pObjExcelWorksheet.Protection.AllowFormatCells = false;
            pObjExcelWorksheet.Protection.AllowFormatColumns = false;
            pObjExcelWorksheet.Protection.AllowFormatRows = false;
            pObjExcelWorksheet.Protection.AllowInsertColumns = false;
            pObjExcelWorksheet.Protection.AllowInsertHyperlinks = false;
            pObjExcelWorksheet.Protection.AllowInsertRows = false;
            pObjExcelWorksheet.Protection.AllowPivotTables = false;
            pObjExcelWorksheet.Protection.AllowSelectLockedCells = false;
            pObjExcelWorksheet.Protection.AllowSelectUnlockedCells = false;
            pObjExcelWorksheet.Protection.AllowSort = false;
            pObjExcelWorksheet.Protection.SetPassword(CommUtil.GenerateUniqueNumber().ToString());

            return lstExcelFormula;
        }

        public void AddCompanyScheme(OfficeOpenXml.ExcelWorksheet pObjExcelWorksheet)
        {
            var vPHeaderRow = pObjExcelWorksheet.Row(1);

            var vPHeaderStyle = vPHeaderRow.Style.Fill;
            vPHeaderStyle.PatternType = ExcelFillStyle.Solid;
            vPHeaderStyle.BackgroundColor.SetColor(Color.Green);

            vPHeaderRow.Style.Font.Color.SetColor(Color.White);
            vPHeaderRow.Style.Font.Bold = true;
            vPHeaderRow.Style.Font.Size = 12;
            vPHeaderRow.Style.Font.Name = "Calibri";

            vPHeaderRow.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            vPHeaderRow.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            pObjExcelWorksheet.Row(1).Height = 22.50;

            var vPHeaderBorder = vPHeaderRow.Style.Border;
            vPHeaderBorder.Bottom.Style = vPHeaderBorder.Top.Style = vPHeaderBorder.Left.Style = vPHeaderBorder.Right.Style = ExcelBorderStyle.Thin;

            vPHeaderRow.Style.WrapText = false;
            vPHeaderRow.Style.ShrinkToFit = false;

            pObjExcelWorksheet.Cells[1, 1].Value = "CompanyName";
            pObjExcelWorksheet.Cells[1, 2].Value = "CompanyTIN";
            pObjExcelWorksheet.Cells[1, 3].Value = "MobileNo1";
            pObjExcelWorksheet.Cells[1, 4].Value = "MobileNo2";
            pObjExcelWorksheet.Cells[1, 5].Value = "EmailAddress1";
            pObjExcelWorksheet.Cells[1, 6].Value = "EmailAddress2";
            pObjExcelWorksheet.Cells[1, 7].Value = "TaxOffice";
            pObjExcelWorksheet.Cells[1, 8].Value = "EconomicActivity";
            pObjExcelWorksheet.Cells[1, 9].Value = "PreferredNotification";
            pObjExcelWorksheet.Cells[1, 10].Value = "ContactAddress";

            //Adding DropDown Validation
            var vTaxOfficeValidation = pObjExcelWorksheet.DataValidations.AddListValidation(ExcelRange.GetAddress(2, 7, ExcelPackage.MaxRows, 7));
            vTaxOfficeValidation.ShowErrorMessage = true;
            vTaxOfficeValidation.ErrorStyle = ExcelDataValidationWarningStyle.stop;
            vTaxOfficeValidation.ErrorTitle = "Error";
            vTaxOfficeValidation.Error = "Select Tax Office";
            vTaxOfficeValidation.Formula.ExcelFormula = "=TaxOffice";

            var vEconomicActivitiesValidation = pObjExcelWorksheet.DataValidations.AddListValidation(ExcelRange.GetAddress(2, 8, ExcelPackage.MaxRows, 8));
            vEconomicActivitiesValidation.ShowErrorMessage = true;
            vEconomicActivitiesValidation.ErrorStyle = ExcelDataValidationWarningStyle.stop;
            vEconomicActivitiesValidation.ErrorTitle = "Error";
            vEconomicActivitiesValidation.Error = "Select Economic Activities";
            vEconomicActivitiesValidation.Formula.ExcelFormula = "=EconomicActivities";

            var vNotificaionMethodValidation = pObjExcelWorksheet.DataValidations.AddListValidation(ExcelRange.GetAddress(2, 9, ExcelPackage.MaxRows, 9));
            vNotificaionMethodValidation.ShowErrorMessage = true;
            vNotificaionMethodValidation.ErrorStyle = ExcelDataValidationWarningStyle.stop;
            vNotificaionMethodValidation.ErrorTitle = "Error";
            vNotificaionMethodValidation.Error = "Select Preferred Notification";
            vNotificaionMethodValidation.Formula.ExcelFormula = "=NotificationMethod";

            for (int intColCount = 1; intColCount <= 10; intColCount++)
            {
                pObjExcelWorksheet.Column(intColCount).Style.WrapText = false;
                pObjExcelWorksheet.Column(intColCount).Style.ShrinkToFit = false;
                pObjExcelWorksheet.Column(intColCount).BestFit = true;
                pObjExcelWorksheet.Column(intColCount).AutoFit();
            }
        }


        public ActionResult UploadIndividual()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]

        public ActionResult UploadIndividual(RINGeneratorViewModel pIndividualRGViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            else
            {
                if (pIndividualRGViewModel.ExcelFile.ContentLength > 0)
                {
                    string strActualFileName = pIndividualRGViewModel.ExcelFile.FileName;
                    string strUploadedFileExt = strActualFileName.Substring(strActualFileName.LastIndexOf('.') + 1);
                    string[] strDocFormats = new string[] { "xls", "xlsx" };
                    if (strDocFormats.Contains(strUploadedFileExt.ToLower()))
                    {
                        string strInputFileName = "IndividualData_" + DateTime.Now.ToString("dd_MM_yyyy") + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + "." + strUploadedFileExt;
                        string strOutputFileName = "IndividualResult_" + DateTime.Now.ToString("dd_MM_yyyy") + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + "." + strUploadedFileExt;
                        string fileLocation = GlobalDefaultValues.DocumentLocation + "RINGenerator/Individual";

                        if (!(Directory.Exists(fileLocation)))
                        {
                            Directory.CreateDirectory(fileLocation);
                        }

                        pIndividualRGViewModel.ExcelFile.SaveAs(fileLocation + "/" + strInputFileName);

                        DataTable dtIndividualDetails;

                        using (ExcelPackage mObjExcelPackage = new ExcelPackage(new FileInfo(fileLocation + "/" + strInputFileName)))
                        {
                            if (mObjExcelPackage.Workbook.Worksheets.Count == 2)
                            {

                                dtIndividualDetails = mObjExcelPackage.Workbook.Worksheets[2].ToDataTable();

                                if (dtIndividualDetails.Rows.Count > 0)
                                {

                                    //check if column exists
                                    if (dtIndividualDetails.Columns.Contains("FirstName") && dtIndividualDetails.Columns.Contains("MiddleName") && dtIndividualDetails.Columns.Contains("LastName")
                                        && dtIndividualDetails.Columns.Contains("Gender") && dtIndividualDetails.Columns.Contains("Title") && dtIndividualDetails.Columns.Contains("DateofBirth")
                                        && dtIndividualDetails.Columns.Contains("TIN") && dtIndividualDetails.Columns.Contains("MobileNo1") && dtIndividualDetails.Columns.Contains("MobileNo2")
                                        && dtIndividualDetails.Columns.Contains("EmailAddress1") && dtIndividualDetails.Columns.Contains("EmailAddress2") && dtIndividualDetails.Columns.Contains("BiometricDetails")
                                        && dtIndividualDetails.Columns.Contains("TaxOffice") && dtIndividualDetails.Columns.Contains("MaritalStatus") && dtIndividualDetails.Columns.Contains("Nationality")
                                        && dtIndividualDetails.Columns.Contains("EconomicActivity") && dtIndividualDetails.Columns.Contains("PreferredNotification") && dtIndividualDetails.Columns.Contains("ContactAddress"))
                                    {

                                        // Adding Required Column
                                        dtIndividualDetails.Columns.Add("GenderID", typeof(int));
                                        dtIndividualDetails.Columns.Add("TitleID", typeof(int));
                                        dtIndividualDetails.Columns.Add("MaritalStatusID", typeof(int));
                                        dtIndividualDetails.Columns.Add("NationalityID", typeof(int));
                                        dtIndividualDetails.Columns.Add("TaxOfficeID", typeof(int));
                                        dtIndividualDetails.Columns.Add("EconomicActivitiesID", typeof(int));
                                        dtIndividualDetails.Columns.Add("NotificationMethodID", typeof(int));
                                        dtIndividualDetails.Columns.Add("Status", typeof(string));
                                        dtIndividualDetails.Columns.Add("RIN");
                                        dtIndividualDetails.Columns.Add("Result");

                                        //Process and Start Uploading Data
                                        BLIndividual mObjBLIndividual = new BLIndividual();
                                        Individual mObjIndividual;
                                        FuncResponse<Individual> mObjFuncResponse;
                                        IList<DropDownListResult> lstGender = new BLCommon().BL_GetGenderDropDownList();
                                        foreach (DataRow drData in dtIndividualDetails.Rows)
                                        {
                                            try
                                            {
                                                string strGender = drData["Gender"].ToString();
                                                drData["GenderID"] = lstGender.Where(t => t.text.ToLower().Equals(strGender.ToLower())).FirstOrDefault().id;

                                                string strTitle = drData["Title"].ToString();
                                                string[] strArrTitle = strTitle.Split(':');

                                                if (strArrTitle.Length == 2)
                                                {
                                                    drData["TitleID"] = TrynParse.parseInt(strArrTitle[0].Trim());
                                                }
                                                else
                                                {
                                                    drData["TitleID"] = 0;
                                                }

                                                string strMaritalStatus = drData["MaritalStatus"].ToString();
                                                string[] strArrMaritalStatus = strMaritalStatus.Split(':');

                                                if (strArrMaritalStatus.Length == 2)
                                                {
                                                    drData["MaritalStatusID"] = TrynParse.parseInt(strArrMaritalStatus[0].Trim());
                                                }
                                                else
                                                {
                                                    drData["MaritalStatusID"] = 0;
                                                }

                                                string strNationality = drData["Nationality"].ToString();
                                                string[] strArrNationality = strNationality.Split(':');

                                                if (strArrNationality.Length == 2)
                                                {
                                                    drData["NationalityID"] = TrynParse.parseInt(strArrNationality[0].Trim());
                                                }
                                                else
                                                {
                                                    drData["NationalityID"] = 0;
                                                }

                                                string strTaxOffice = drData["TaxOffice"].ToString();
                                                string[] strArrTaxOffice = strTaxOffice.Split(':');

                                                if (strArrTaxOffice.Length == 2)
                                                {
                                                    drData["TaxOfficeID"] = TrynParse.parseInt(strArrTaxOffice[0].Trim());
                                                }
                                                else
                                                {
                                                    drData["TaxOfficeID"] = 0;
                                                }

                                                string strEconomicActivity = drData["EconomicActivity"].ToString();
                                                string[] strArrEconomicActivity = strEconomicActivity.Split(':');

                                                if (strArrEconomicActivity.Length == 2)
                                                {
                                                    drData["EconomicActivitiesID"] = TrynParse.parseInt(strArrEconomicActivity[0].Trim());
                                                }
                                                else
                                                {
                                                    drData["EconomicActivitiesID"] = 0;
                                                }

                                                string strNotificationMethod = drData["PreferredNotification"].ToString();
                                                string[] strArrNotificationMethod = strNotificationMethod.Split(':');

                                                if (strArrNotificationMethod.Length == 2)
                                                {
                                                    drData["NotificationMethodID"] = TrynParse.parseInt(strArrNotificationMethod[0].Trim());
                                                }
                                                else
                                                {
                                                    drData["NotificationMethodID"] = 0;
                                                }

                                                //Do Validation
                                                if (TrynParse.parseInt(drData["GenderID"]) > 0
                                                    && TrynParse.parseInt(drData["TitleID"]) > 0 &&
                                                    !string.IsNullOrEmpty(TrynParse.parseStringForExcel(drData["FirstName"])) && !string.IsNullOrEmpty(TrynParse.parseStringForExcel(drData["LastName"]))
                                                    && !string.IsNullOrEmpty(TrynParse.parseStringForExcel(drData["MobileNo1"])) && TrynParse.parseInt(drData["TaxOfficeID"]) > 0 && TrynParse.parseInt(drData["NationalityID"]) > 0 &&
                                                    !string.IsNullOrEmpty(TrynParse.parseStringForExcel(drData["ContactAddress"])) && TrynParse.parseInt(drData["EconomicActivitiesID"]) > 0 && TrynParse.parseInt(drData["NotificationMethodID"]) > 0)
                                                {
                                                    mObjIndividual = new Individual()
                                                    {
                                                        IndividualID = 0,
                                                        GenderID = TrynParse.parseInt(drData["GenderID"]),
                                                        TitleID = TrynParse.parseInt(drData["TitleID"]),
                                                        FirstName = TrynParse.parseStringForExcel(drData["FirstName"]),
                                                        LastName = TrynParse.parseStringForExcel(drData["MiddleName"]),
                                                        MiddleName = TrynParse.parseStringForExcel(drData["LastName"]),
                                                        DOB = TrynParse.parseNullableDate(drData["DateofBirth"]),
                                                        TIN = TrynParse.parseStringForExcel(drData["TIN"]),
                                                        MobileNumber1 = TrynParse.parseStringForExcel(drData["MobileNo1"]),
                                                        MobileNumber2 = drData.IsNull("MobileNo2") ? null : TrynParse.parseStringForExcel(drData["MobileNo2"]),
                                                        EmailAddress1 = drData.IsNull("EmailAddress1") ? null : TrynParse.parseStringForExcel(drData["EmailAddress1"]),
                                                        EmailAddress2 = drData.IsNull("EmailAddress2") ? null : TrynParse.parseStringForExcel(drData["EmailAddress2"]),
                                                        BiometricDetails = drData.IsNull("BiometricDetails") ? null : TrynParse.parseStringForExcel(drData["BiometricDetails"]),
                                                        TaxOfficeID = TrynParse.parseInt(drData["TaxOfficeID"]),
                                                        MaritalStatusID = TrynParse.parseInt(drData["MaritalStatusID"]),
                                                        NationalityID = TrynParse.parseInt(drData["NationalityID"]),
                                                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                                                        EconomicActivitiesID = TrynParse.parseInt(drData["EconomicActivitiesID"]),
                                                        NotificationMethodID = TrynParse.parseInt(drData["NotificationMethodID"]),
                                                        ContactAddress = TrynParse.parseStringForExcel(drData["ContactAddress"]),
                                                        Active = true,
                                                        CreatedBy = SessionManager.UserID,
                                                        CreatedDate = CommUtil.GetCurrentDateTime()
                                                    };

                                                    mObjFuncResponse = mObjBLIndividual.BL_InsertUpdateIndividual(mObjIndividual, false);

                                                    if (mObjFuncResponse.Success)
                                                    {
                                                        if (GlobalDefaultValues.SendNotification)
                                                        {
                                                            //Send Notification
                                                            EmailDetails mObjEmailDetails = new EmailDetails()
                                                            {
                                                                TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                                                                TaxPayerTypeName = "Individual",
                                                                TaxPayerID = mObjIndividual.IndividualID,
                                                                TaxPayerName = mObjIndividual.FirstName + " " + mObjIndividual.LastName,
                                                                TaxPayerRIN = mObjIndividual.IndividualRIN,
                                                                TaxPayerMobileNumber = mObjIndividual.MobileNumber1,
                                                                TaxPayerEmail = mObjIndividual.EmailAddress1,
                                                                ContactAddress = mObjIndividual.ContactAddress,
                                                                TaxPayerTIN = mObjIndividual.TIN,
                                                                LoggedInUserID = SessionManager.UserID,
                                                            };

                                                            if (!string.IsNullOrWhiteSpace(mObjIndividual.EmailAddress1))
                                                            {
                                                                BLEmailHandler.BL_TaxPayerCreated(mObjEmailDetails);
                                                            }

                                                            if (!string.IsNullOrWhiteSpace(mObjIndividual.MobileNumber1))
                                                            {
                                                                UtilityController.BL_TaxPayerCreated(mObjEmailDetails);
                                                            }
                                                        }

                                                        drData["Status"] = "Success";
                                                        drData["Result"] = mObjFuncResponse.Message;
                                                        drData["RIN"] = mObjFuncResponse.AdditionalData.IndividualRIN;
                                                    }
                                                    else
                                                    {
                                                        drData["Status"] = "Failed";
                                                        drData["Result"] = mObjFuncResponse.Message;
                                                    }
                                                }
                                                else
                                                {
                                                    drData["Status"] = "Failed";
                                                    drData["Result"] = "Invalid Data";
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Logger.SendErrorToText(ex);
                                                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                                                drData["Status"] = "Failed";
                                                drData["Result"] = "Error Occurred";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ViewBag.Message = "Invalid Excel";
                                        return View();
                                    }
                                }
                                else
                                {
                                    ViewBag.Message = "No Records found for RIN Generation";
                                    return View();
                                }
                            }
                            else
                            {
                                ViewBag.Message = "Invalid Excel";
                                return View();
                            }
                        }

                        using (ExcelPackage mObjExcelPackage = new ExcelPackage())
                        {
                            dtIndividualDetails.Columns.Remove("GenderID");
                            dtIndividualDetails.Columns.Remove("TitleID");
                            dtIndividualDetails.Columns.Remove("MaritalStatusID");
                            dtIndividualDetails.Columns.Remove("NationalityID");
                            dtIndividualDetails.Columns.Remove("TaxOfficeID");
                            dtIndividualDetails.Columns.Remove("EconomicActivitiesID");
                            dtIndividualDetails.Columns.Remove("NotificationMethodID");
                            ExcelWorksheet ObjExcelWorksheet = mObjExcelPackage.Workbook.Worksheets.Add("Individual");
                            ObjExcelWorksheet.Cells["A1"].LoadFromDataTable(dtIndividualDetails, true);

                            var vPHeaderRow = ObjExcelWorksheet.Row(1);

                            var vPHeaderStyle = vPHeaderRow.Style.Fill;
                            vPHeaderStyle.PatternType = ExcelFillStyle.Solid;
                            vPHeaderStyle.BackgroundColor.SetColor(Color.Green);

                            vPHeaderRow.Style.Font.Color.SetColor(Color.White);
                            vPHeaderRow.Style.Font.Bold = true;
                            vPHeaderRow.Style.Font.Size = 12;
                            vPHeaderRow.Style.Font.Name = "Calibri";

                            vPHeaderRow.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            vPHeaderRow.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            ObjExcelWorksheet.Row(1).Height = 22.50;

                            var vPHeaderBorder = vPHeaderRow.Style.Border;
                            vPHeaderBorder.Bottom.Style = vPHeaderBorder.Top.Style = vPHeaderBorder.Left.Style = vPHeaderBorder.Right.Style = ExcelBorderStyle.Thin;

                            vPHeaderRow.Style.WrapText = false;
                            vPHeaderRow.Style.ShrinkToFit = false;

                            ObjExcelWorksheet.Cells.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            ObjExcelWorksheet.Cells.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            ObjExcelWorksheet.Cells.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            ObjExcelWorksheet.Cells.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                            //var vDataRow = ObjExcelWorksheet.Cells[2, 1, dtIndividualDetails.Rows.Count + 1, 13];
                            //vDataRow.Style..Height = 22.50;
                            //var vDataBorder = vDataRow.Style.Border;
                            //vDataBorder.Bottom.Style = vDataBorder.Top.Style = vDataBorder.Left.Style = vDataBorder.Right.Style = ExcelBorderStyle.Thin;

                            for (int intColCount = 1; intColCount <= 25; intColCount++)
                            {
                                ObjExcelWorksheet.Column(intColCount).Style.WrapText = false;
                                ObjExcelWorksheet.Column(intColCount).Style.ShrinkToFit = false;
                                ObjExcelWorksheet.Column(intColCount).BestFit = true;
                                ObjExcelWorksheet.Column(intColCount).AutoFit();
                            }

                            mObjExcelPackage.SaveAs(new FileInfo(fileLocation + "/" + strOutputFileName));

                            ViewBag.SMessage = "Upload Completed Successfully.";
                            ViewBag.ResultFilePath = "/Document/RINGenerator/Individual/" + strOutputFileName;
                            return View();
                            //byte[] mByteData = mObjExcelPackage.GetAsByteArray();
                            //string strfilename = "IndividualResult_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xlsx";
                            //return File(mByteData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", strfilename);
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Select excel document to upload";
                        return View();
                    }
                }
                else
                {
                    ViewBag.Message = "Invalid Excel File";
                    return View();
                }
            }
        }

        public ActionResult DownloadIndividualTemplate()
        {
            using (ExcelPackage mObjExcelPackage = new ExcelPackage())
            {
                mObjExcelPackage.Workbook.Worksheets.Add("ReferenceData");
                mObjExcelPackage.Workbook.Worksheets.Add("Individual");

                mObjExcelPackage.Workbook.Worksheets[1].Hidden = eWorkSheetHidden.VeryHidden;

                mObjExcelPackage.Workbook.Protection.LockStructure = true;
                mObjExcelPackage.Workbook.Protection.SetPassword(CommUtil.GenerateUniqueNumber().ToString());

                //Updating Reference Data Sheet
                IList<ExcelFormulaModel> lstExcelFormula = AddIndividualReferenceData(mObjExcelPackage.Workbook.Worksheets[1]);

                //Creating Names for Formula
                foreach (var item in lstExcelFormula)
                {
                    var vExcelRange = mObjExcelPackage.Workbook.Worksheets[1].Cells[item.FromRow, item.FromCol, item.ToRow, item.ToCol];
                    mObjExcelPackage.Workbook.Names.Add(item.Name, vExcelRange);
                }

                AddIndividualScheme(mObjExcelPackage.Workbook.Worksheets[2]);

                //Generate A File with Random name
                byte[] mByteData = mObjExcelPackage.GetAsByteArray();
                string strfilename = "IndividualData_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xlsx";
                return File(mByteData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", strfilename);
            }
        }

        public IList<ExcelFormulaModel> AddIndividualReferenceData(ExcelWorksheet pObjExcelWorksheet)
        {
            IList<ExcelFormulaModel> lstExcelFormula = new List<ExcelFormulaModel>();

            var vPHeaderRow = pObjExcelWorksheet.Row(1);

            var vPHeaderStyle = vPHeaderRow.Style.Fill;
            vPHeaderStyle.PatternType = ExcelFillStyle.Solid;
            vPHeaderStyle.BackgroundColor.SetColor(Color.Green);

            vPHeaderRow.Style.Font.Color.SetColor(Color.White);
            vPHeaderRow.Style.Font.Bold = true;
            vPHeaderRow.Style.Font.Size = 12;
            vPHeaderRow.Style.Font.Name = "Calibri";

            vPHeaderRow.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            vPHeaderRow.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            pObjExcelWorksheet.Row(1).Height = 22.50;

            var vPHeaderBorder = vPHeaderRow.Style.Border;
            vPHeaderBorder.Bottom.Style = vPHeaderBorder.Top.Style = vPHeaderBorder.Left.Style = vPHeaderBorder.Right.Style = ExcelBorderStyle.Thin;

            vPHeaderRow.Style.WrapText = false;
            vPHeaderRow.Style.ShrinkToFit = false;

            pObjExcelWorksheet.Cells[1, 1].Value = "Gender";
            pObjExcelWorksheet.Cells[1, 2].Value = "Tax Office";
            pObjExcelWorksheet.Cells[1, 3].Value = "Marital Status";
            pObjExcelWorksheet.Cells[1, 4].Value = "Nationality";
            pObjExcelWorksheet.Cells[1, 5].Value = "Economic Activities";
            pObjExcelWorksheet.Cells[1, 6].Value = "Notification Method";


            int intRowIndex = 2;
            IList<DropDownListResult> lstGender = new BLCommon().BL_GetGenderDropDownList();
            IList<DropDownListResult> lstTitle;
            foreach (var item in lstGender)
            {
                var vDataRow = pObjExcelWorksheet.Row(intRowIndex);
                vDataRow.Height = 22.50;
                var vDataBorder = vDataRow.Style.Border;
                vDataBorder.Bottom.Style = vDataBorder.Top.Style = vDataBorder.Left.Style = vDataBorder.Right.Style = ExcelBorderStyle.Thin;

                pObjExcelWorksheet.Cells[intRowIndex, 1].Value = item.text.Replace(" ", "");
                pObjExcelWorksheet.Cells[1, 6 + (intRowIndex - 1)].Value = item.text.Replace(" ", "");

                int intSubRowIndex = 2;
                lstTitle = new BLTitle().BL_GetTitleDropDownList(new Title() { intStatus = 1, GenderID = item.id });
                foreach (var title in lstTitle)
                {
                    var vTitleDataRow = pObjExcelWorksheet.Row(intRowIndex);
                    vTitleDataRow.Height = 22.50;
                    var vTitleDataBorder = vTitleDataRow.Style.Border;
                    vTitleDataBorder.Bottom.Style = vTitleDataBorder.Top.Style = vTitleDataBorder.Left.Style = vTitleDataBorder.Right.Style = ExcelBorderStyle.Thin;

                    pObjExcelWorksheet.Cells[intSubRowIndex, 6 + (intRowIndex - 1)].Value = title.id + " : " + title.text;
                    intSubRowIndex++;
                }

                lstExcelFormula.Add(new ExcelFormulaModel() { Name = item.text.Replace(" ", ""), FromRow = 2, FromCol = 6 + (intRowIndex - 1), ToRow = intSubRowIndex, ToCol = 6 + (intRowIndex - 1) });
                intRowIndex++;
            }

            lstExcelFormula.Add(new ExcelFormulaModel() { Name = "Gender", FromRow = 2, FromCol = 1, ToRow = intRowIndex, ToCol = 1 });

            intRowIndex = 1;
            IList<DropDownListResult> lstTaxOffice = new BLTaxOffice().BL_GetTaxOfficeDropDownList(new BOL.Tax_Offices() { intStatus = 1 });
            foreach (var item in lstTaxOffice)
            {
                intRowIndex++;
                var vDataRow = pObjExcelWorksheet.Row(intRowIndex);
                vDataRow.Height = 22.50;
                var vDataBorder = vDataRow.Style.Border;
                vDataBorder.Bottom.Style = vDataBorder.Top.Style = vDataBorder.Left.Style = vDataBorder.Right.Style = ExcelBorderStyle.Thin;

                pObjExcelWorksheet.Cells[intRowIndex, 2].Value = item.id + " : " + item.text;
            }

            lstExcelFormula.Add(new ExcelFormulaModel() { Name = "TaxOffice", FromRow = 2, FromCol = 2, ToRow = intRowIndex, ToCol = 2 });

            intRowIndex = 1;
            IList<DropDownListResult> lstMaritalStatus = new BLCommon().BL_GetMaritalStatusDropDownList();
            foreach (var item in lstMaritalStatus)
            {
                intRowIndex++;
                var vDataRow = pObjExcelWorksheet.Row(intRowIndex);
                vDataRow.Height = 22.50;
                var vDataBorder = vDataRow.Style.Border;
                vDataBorder.Bottom.Style = vDataBorder.Top.Style = vDataBorder.Left.Style = vDataBorder.Right.Style = ExcelBorderStyle.Thin;

                pObjExcelWorksheet.Cells[intRowIndex, 3].Value = item.id + " : " + item.text;
            }

            lstExcelFormula.Add(new ExcelFormulaModel() { Name = "MaritalStatus", FromRow = 2, FromCol = 3, ToRow = intRowIndex, ToCol = 3 });

            intRowIndex = 1;
            IList<DropDownListResult> lstNationality = new BLCommon().BL_GetNationalityDropDownList();
            foreach (var item in lstNationality)
            {
                intRowIndex++;
                var vDataRow = pObjExcelWorksheet.Row(intRowIndex);
                vDataRow.Height = 22.50;
                var vDataBorder = vDataRow.Style.Border;
                vDataBorder.Bottom.Style = vDataBorder.Top.Style = vDataBorder.Left.Style = vDataBorder.Right.Style = ExcelBorderStyle.Thin;

                pObjExcelWorksheet.Cells[intRowIndex, 4].Value = item.id + " : " + item.text;
            }

            lstExcelFormula.Add(new ExcelFormulaModel() { Name = "Nationality", FromRow = 2, FromCol = 4, ToRow = intRowIndex, ToCol = 4 });

            intRowIndex = 1;
            IList<DropDownListResult> lstEconomicActivities = new BLEconomicActivities().BL_GetEconomicActivitiesDropDownList(new BOL.Economic_Activities() { intStatus = 1, TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual });
            foreach (var item in lstEconomicActivities)
            {
                intRowIndex++;
                var vDataRow = pObjExcelWorksheet.Row(intRowIndex);
                vDataRow.Height = 22.50;
                var vDataBorder = vDataRow.Style.Border;
                vDataBorder.Bottom.Style = vDataBorder.Top.Style = vDataBorder.Left.Style = vDataBorder.Right.Style = ExcelBorderStyle.Thin;

                pObjExcelWorksheet.Cells[intRowIndex, 5].Value = item.id + " : " + item.text;
            }

            lstExcelFormula.Add(new ExcelFormulaModel() { Name = "EconomicActivities", FromRow = 2, FromCol = 5, ToRow = intRowIndex, ToCol = 5 });

            intRowIndex = 1;
            IList<DropDownListResult> lstNotificationMethod = new BLNotificationMethod().BL_GetNotificationMethodDropDownList(new BOL.Notification_Method() { intStatus = 1 });
            foreach (var item in lstNotificationMethod)
            {
                intRowIndex++;
                var vDataRow = pObjExcelWorksheet.Row(intRowIndex);
                vDataRow.Height = 22.50;
                var vDataBorder = vDataRow.Style.Border;
                vDataBorder.Bottom.Style = vDataBorder.Top.Style = vDataBorder.Left.Style = vDataBorder.Right.Style = ExcelBorderStyle.Thin;

                pObjExcelWorksheet.Cells[intRowIndex, 6].Value = item.id + " : " + item.text;
            }

            lstExcelFormula.Add(new ExcelFormulaModel() { Name = "NotificationMethod", FromRow = 2, FromCol = 6, ToRow = intRowIndex, ToCol = 6 });

            pObjExcelWorksheet.Cells.AutoFitColumns(100);
            pObjExcelWorksheet.Protection.IsProtected = true;
            pObjExcelWorksheet.Protection.AllowDeleteColumns = false;
            pObjExcelWorksheet.Protection.AllowDeleteRows = false;
            pObjExcelWorksheet.Protection.AllowEditObject = false;
            pObjExcelWorksheet.Protection.AllowEditScenarios = false;
            pObjExcelWorksheet.Protection.AllowFormatCells = false;
            pObjExcelWorksheet.Protection.AllowFormatColumns = false;
            pObjExcelWorksheet.Protection.AllowFormatRows = false;
            pObjExcelWorksheet.Protection.AllowInsertColumns = false;
            pObjExcelWorksheet.Protection.AllowInsertHyperlinks = false;
            pObjExcelWorksheet.Protection.AllowInsertRows = false;
            pObjExcelWorksheet.Protection.AllowPivotTables = false;
            pObjExcelWorksheet.Protection.AllowSelectLockedCells = false;
            pObjExcelWorksheet.Protection.AllowSelectUnlockedCells = false;
            pObjExcelWorksheet.Protection.AllowSort = false;
            pObjExcelWorksheet.Protection.SetPassword(CommUtil.GenerateUniqueNumber().ToString());

            return lstExcelFormula;
        }

        public void AddIndividualScheme(ExcelWorksheet pObjExcelWorksheet)
        {
            var vPHeaderRow = pObjExcelWorksheet.Row(1);

            var vPHeaderStyle = vPHeaderRow.Style.Fill;
            vPHeaderStyle.PatternType = ExcelFillStyle.Solid;
            vPHeaderStyle.BackgroundColor.SetColor(Color.Green);

            vPHeaderRow.Style.Font.Color.SetColor(Color.White);
            vPHeaderRow.Style.Font.Bold = true;
            vPHeaderRow.Style.Font.Size = 12;
            vPHeaderRow.Style.Font.Name = "Calibri";

            vPHeaderRow.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            vPHeaderRow.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            pObjExcelWorksheet.Row(1).Height = 22.50;

            var vPHeaderBorder = vPHeaderRow.Style.Border;
            vPHeaderBorder.Bottom.Style = vPHeaderBorder.Top.Style = vPHeaderBorder.Left.Style = vPHeaderBorder.Right.Style = ExcelBorderStyle.Thin;

            vPHeaderRow.Style.WrapText = false;
            vPHeaderRow.Style.ShrinkToFit = false;

            pObjExcelWorksheet.Cells[1, 1].Value = "FirstName";
            pObjExcelWorksheet.Cells[1, 2].Value = "MiddleName";
            pObjExcelWorksheet.Cells[1, 3].Value = "LastName";
            pObjExcelWorksheet.Cells[1, 4].Value = "Gender";
            pObjExcelWorksheet.Cells[1, 5].Value = "Title";
            pObjExcelWorksheet.Cells[1, 6].Value = "DateofBirth";
            pObjExcelWorksheet.Cells[1, 7].Value = "TIN";
            pObjExcelWorksheet.Cells[1, 8].Value = "MobileNo1";
            pObjExcelWorksheet.Cells[1, 9].Value = "MobileNo2";
            pObjExcelWorksheet.Cells[1, 10].Value = "EmailAddress1";
            pObjExcelWorksheet.Cells[1, 11].Value = "EmailAddress2";
            pObjExcelWorksheet.Cells[1, 12].Value = "BiometricDetails";
            pObjExcelWorksheet.Cells[1, 13].Value = "TaxOffice";
            pObjExcelWorksheet.Cells[1, 14].Value = "MaritalStatus";
            pObjExcelWorksheet.Cells[1, 15].Value = "Nationality";
            pObjExcelWorksheet.Cells[1, 16].Value = "EconomicActivity";
            pObjExcelWorksheet.Cells[1, 17].Value = "PreferredNotification";
            pObjExcelWorksheet.Cells[1, 18].Value = "ContactAddress";

            //Adding DropDown Validation
            var vGenderValidation = pObjExcelWorksheet.DataValidations.AddListValidation(ExcelRange.GetAddress(2, 4, ExcelPackage.MaxRows, 4));
            vGenderValidation.ShowErrorMessage = true;
            vGenderValidation.ErrorStyle = ExcelDataValidationWarningStyle.stop;
            vGenderValidation.ErrorTitle = "Error";
            vGenderValidation.Error = "Select Gender";
            vGenderValidation.Formula.ExcelFormula = "=Gender";

            var vTitleValidation = pObjExcelWorksheet.DataValidations.AddListValidation(ExcelRange.GetAddress(2, 5, ExcelPackage.MaxRows, 5));
            vTitleValidation.ShowErrorMessage = true;
            vTitleValidation.ErrorStyle = ExcelDataValidationWarningStyle.stop;
            vTitleValidation.ErrorTitle = "Error";
            vTitleValidation.Error = "Select Title";
            vTitleValidation.Formula.ExcelFormula = "=INDIRECT(D2)";


            var vTaxOfficeValidation = pObjExcelWorksheet.DataValidations.AddListValidation(ExcelRange.GetAddress(2, 13, ExcelPackage.MaxRows, 13));
            vTaxOfficeValidation.ShowErrorMessage = true;
            vTaxOfficeValidation.ErrorStyle = ExcelDataValidationWarningStyle.stop;
            vTaxOfficeValidation.ErrorTitle = "Error";
            vTaxOfficeValidation.Error = "Select Tax Office";
            vTaxOfficeValidation.Formula.ExcelFormula = "=TaxOffice";

            var vMaritalStatusValidation = pObjExcelWorksheet.DataValidations.AddListValidation(ExcelRange.GetAddress(2, 14, ExcelPackage.MaxRows, 14));
            vMaritalStatusValidation.ShowErrorMessage = true;
            vMaritalStatusValidation.ErrorStyle = ExcelDataValidationWarningStyle.stop;
            vMaritalStatusValidation.ErrorTitle = "Error";
            vMaritalStatusValidation.Error = "Select Marital Status";
            vMaritalStatusValidation.Formula.ExcelFormula = "=MaritalStatus";

            var vNationalityValidation = pObjExcelWorksheet.DataValidations.AddListValidation(ExcelRange.GetAddress(2, 15, ExcelPackage.MaxRows, 15));
            vNationalityValidation.ShowErrorMessage = true;
            vNationalityValidation.ErrorStyle = ExcelDataValidationWarningStyle.stop;
            vNationalityValidation.ErrorTitle = "Error";
            vNationalityValidation.Error = "Select Nationality";
            vNationalityValidation.Formula.ExcelFormula = "=Nationality";

            var vEconomicActivitiesValidation = pObjExcelWorksheet.DataValidations.AddListValidation(ExcelRange.GetAddress(2, 16, ExcelPackage.MaxRows, 16));
            vEconomicActivitiesValidation.ShowErrorMessage = true;
            vEconomicActivitiesValidation.ErrorStyle = ExcelDataValidationWarningStyle.stop;
            vEconomicActivitiesValidation.ErrorTitle = "Error";
            vEconomicActivitiesValidation.Error = "Select Economic Activities";
            vEconomicActivitiesValidation.Formula.ExcelFormula = "=EconomicActivities";

            var vNotificaionMethodValidation = pObjExcelWorksheet.DataValidations.AddListValidation(ExcelRange.GetAddress(2, 17, ExcelPackage.MaxRows, 17));
            vNotificaionMethodValidation.ShowErrorMessage = true;
            vNotificaionMethodValidation.ErrorStyle = ExcelDataValidationWarningStyle.stop;
            vNotificaionMethodValidation.ErrorTitle = "Error";
            vNotificaionMethodValidation.Error = "Select Preferred Notification";
            vNotificaionMethodValidation.Formula.ExcelFormula = "=NotificationMethod";

            for (int intColCount = 1; intColCount <= 18; intColCount++)
            {
                pObjExcelWorksheet.Column(intColCount).Style.WrapText = false;
                pObjExcelWorksheet.Column(intColCount).Style.ShrinkToFit = false;
                pObjExcelWorksheet.Column(intColCount).BestFit = true;
                pObjExcelWorksheet.Column(intColCount).AutoFit();
            }

            pObjExcelWorksheet.Column(6).Style.Numberformat.Format = "dd-mmm-yyyy";
        }
    }
}