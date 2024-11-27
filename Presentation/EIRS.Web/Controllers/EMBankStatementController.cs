using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Web.Models;
using Elmah;
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
using Vereyon.Web;
using System.Linq.Dynamic;

using EIRS.Web.Utility;


namespace EIRS.Web.Controllers
{
    public class EMBankStatementController : BaseController
    {
        // GET: EMBankStatement
        [HttpGet]
        public ActionResult ImportData()
        {
            UI_FillYearDropDown();
            UI_FillMonthDropDown();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult ImportData(EMDataImportViewModel pObjDataImportModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillYearDropDown();
                UI_FillMonthDropDown();
                return View(pObjDataImportModel);
            }
            else
            {
                if (pObjDataImportModel.ExcelFile.ContentLength > 0)
                {
                    string strActualFileName = pObjDataImportModel.ExcelFile.FileName;
                    string strUploadedFileExt = strActualFileName.Substring(strActualFileName.LastIndexOf('.') + 1);
                    string[] strDocFormats = new string[] { "xls", "xlsx" };
                    if (strDocFormats.Contains(strUploadedFileExt.ToLower()))
                    {
                        string strInputFileName = "BankStatementData_" + DateTime.Now.ToString("dd_MM_yyyy") + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + "." + strUploadedFileExt;
                        string strOutputFileName = "BankStatementResult_" + DateTime.Now.ToString("dd_MM_yyyy") + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + "." + strUploadedFileExt;
                        string fileLocation = GlobalDefaultValues.DocumentLocation + "ERASManual/BS/";

                        if (!(Directory.Exists(fileLocation)))
                        {
                            Directory.CreateDirectory(fileLocation);
                        }

                        pObjDataImportModel.ExcelFile.SaveAs(fileLocation + "/" + strInputFileName);

                        DataTable dtExcelData;


                        using (ExcelPackage mObjExcelPackage = new ExcelPackage(new FileInfo(fileLocation + "/" + strInputFileName)))
                        {
                            if (mObjExcelPackage.Workbook.Worksheets.Count == 1)
                            {

                                dtExcelData = mObjExcelPackage.Workbook.Worksheets[1].ToDataTable();

                                if (dtExcelData.Rows.Count > 0)
                                {
                                    // Adding Required Column
                                    dtExcelData.Columns.Add("Status", typeof(string));
                                    dtExcelData.Columns.Add("Result");

                                    //Process and Start Uploading Data
                                    BLEMDataImport mObjBLEMDataImport = new BLEMDataImport();
                                    EM_BankStatement mObjBankStatement;
                                    FuncResponse mObjFuncResponse;
                                    foreach (DataRow drData in dtExcelData.Rows)
                                    {
                                        try
                                        {

                                            //Do Validation

                                            mObjBankStatement = new EM_BankStatement()
                                            {
                                                BSID = 0,
                                                TaxMonth = pObjDataImportModel.TaxMonth,
                                                TaxYear = pObjDataImportModel.TaxYear,
                                                PaymentRefNumber = TrynParse.parseStringForExcel(drData["PaymentRefNumber"]),
                                                PaymentDateTime = TrynParse.parseStringForExcel(drData["PaymentDateTime"]),
                                                CustomerName = TrynParse.parseStringForExcel(drData["CustomerName"]),
                                                Category = TrynParse.parseStringForExcel(drData["Category"]),
                                                RevenueHead = TrynParse.parseStringForExcel(drData["RevenueHead"]),
                                                Bank = TrynParse.parseStringForExcel(drData["Bank"]),
                                                Amount = TrynParse.parseDecimal(drData["Amount"]),
                                                CreatedBy = SessionManager.UserID,
                                                CreatedDate = CommUtil.GetCurrentDateTime()
                                            };

                                            mObjFuncResponse = mObjBLEMDataImport.BL_InsertBankStatement(mObjBankStatement);

                                            if (mObjFuncResponse.Success)
                                            {
                                                drData["Status"] = "Success";
                                                drData["Result"] = mObjFuncResponse.Message;
                                            }
                                            else
                                            {
                                                drData["Status"] = "Failed";
                                                drData["Result"] = mObjFuncResponse.Message;
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

                                    EM_ImportLog mObjImportLog = new EM_ImportLog()
                                    {
                                        DataSourceID = 1,
                                        ImportDate = CommUtil.GetCurrentDateTime(),
                                        ImportFilePath = "ERASManual/PDMA/" + strInputFileName,
                                        TotalRecord = dtExcelData.Rows.Count,
                                        CreatedBy = SessionManager.UserID,
                                        CreatedDate = CommUtil.GetCurrentDateTime()
                                    };

                                    mObjBLEMDataImport.BL_InsertImportLog(mObjImportLog);
                                }
                                else
                                {
                                    UI_FillYearDropDown();
                                    UI_FillMonthDropDown();
                                    ViewBag.Message = "No Records found for Data Import";
                                    return View(pObjDataImportModel);
                                }
                            }
                            else
                            {
                                UI_FillYearDropDown();
                                UI_FillMonthDropDown();
                                ViewBag.Message = "Invalid Excel";
                                return View(pObjDataImportModel);
                            }
                        }

                        using (ExcelPackage mObjExcelPackage = new ExcelPackage())
                        {
                            ExcelWorksheet ObjExcelWorksheet = mObjExcelPackage.Workbook.Worksheets.Add("BankStatement");
                            ObjExcelWorksheet.Cells["A1"].LoadFromDataTable(dtExcelData, true);

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

                            UI_FillYearDropDown();
                            UI_FillMonthDropDown();
                            ModelState.Clear();


                            ViewBag.SMessage = "Upload Completed Successfully.";
                            ViewBag.ResultFilePath = "/ERASManual/PDMA/" + strOutputFileName;
                            return View();
                            //byte[] mByteData = mObjExcelPackage.GetAsByteArray();
                            //string strfilename = "CompanyResult_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xlsx";
                            //return File(mByteData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", strfilename);
                        }


                    }
                    else
                    {
                        UI_FillYearDropDown();
                        UI_FillMonthDropDown();
                        ViewBag.Message = "Select excel document to upload";
                        return View(pObjDataImportModel);
                    }
                }
                else
                {
                    UI_FillYearDropDown();
                    UI_FillMonthDropDown();
                    ViewBag.Message = "Invalid Excel File";
                    return View(pObjDataImportModel);
                }
            }
        }

        [HttpGet]
        public ActionResult AddEntry()
        {
            UI_FillYearDropDown();
            UI_FillMonthDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddEntry(EMBSViewModel pObjEMBSModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillYearDropDown();
                UI_FillMonthDropDown();
                return View(pObjEMBSModel);
            }
            else
            {
                try
                {
                    EM_BankStatement mObjBankStatement = new EM_BankStatement()
                    {
                        BSID = 0,
                        TaxMonth = pObjEMBSModel.TaxMonth,
                        TaxYear = pObjEMBSModel.TaxYear,
                        PaymentRefNumber = pObjEMBSModel.PaymentRefNumber,
                        PaymentDateTime = pObjEMBSModel.PaymentDateTime,
                        CustomerName = pObjEMBSModel.CustomerName,
                        Amount = pObjEMBSModel.Amount,
                        Bank = pObjEMBSModel.Bank,
                        Category = pObjEMBSModel.Category,
                        RevenueHead = pObjEMBSModel.RevenueHead,
                        CreatedBy = SessionManager.UserID,
                        CreatedDate = CommUtil.GetCurrentDateTime()
                    };

                    FuncResponse mObjFuncResponse = new BLEMDataImport().BL_InsertBankStatement(mObjBankStatement);

                    if (mObjFuncResponse.Success)
                    {
                        FlashMessage.Info(mObjFuncResponse.Message);
                        return RedirectToAction("DataSource", "ERASManual");
                    }
                    else
                    {
                        UI_FillYearDropDown();
                        UI_FillMonthDropDown();
                        ViewBag.Message = mObjFuncResponse.Message;
                        return View(pObjEMBSModel);
                    }
                }
                catch (Exception ex)
                {
                    Logger.SendErrorToText(ex);
                    UI_FillYearDropDown();
                    UI_FillMonthDropDown();
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving data";
                    return View(pObjEMBSModel);
                }
            }
        }

        [HttpGet]
        public ActionResult List()
        {
            return View();
        }

        public JsonResult LoadData()
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            try
            {
                string mStrSearchFilter = Request.Form.GetValues("search[value]")[0];
                string mStrDraw = Request.Form.GetValues("draw")[0];
                string mstrOrderBy = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                string mStrOrderByDir = Request.Form.GetValues("order[0][dir]")[0];
                int mIntStartRowNumber = TrynParse.parseInt(Request.Form.GetValues("start")[0]);
                int mIntPageNumber = TrynParse.parseInt(Request.Form.GetValues("length")[0]);

                // Loading. 
                IList<vw_EM_BankStatement> lstData = new BLEMDataImport().BL_GetBankStatementList();

                // Total record count.   
                int totalRecords = lstData.Count;

                //// Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstData = lstData.Where(p =>
                        p.PaymentDateTime.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.PaymentRefNumber.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.CustomerName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.Category.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.RevenueHead.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.Bank.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.Amount.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower())).ToList();
                }

                // Sorting.   
                //Purpose Sorting Data 
                if (!string.IsNullOrEmpty(mstrOrderBy) && !string.IsNullOrEmpty(mStrOrderByDir))
                {
                    lstData = lstData.OrderBy(mstrOrderBy + " " + mStrOrderByDir).ToList();
                }

                // Filter record count.   
                int recFilter = lstData.Count;

                // Apply pagination.   
                lstData = lstData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstData;

            }
            catch (Exception ex)
            {
                Logger.SendErrorToText(ex);
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Details(long? id)
        {
            if (id.GetValueOrDefault() > 0)
            {
                EM_BankStatement mObjDetails = new BLEMDataImport().BL_GetBankStatementDetails(id.GetValueOrDefault());

                if (mObjDetails != null)
                {
                    return View(mObjDetails);
                }
                else
                {
                    return RedirectToAction("List", "EMBankStatement");
                }
            }
            else
            {
                return RedirectToAction("List", "EMBankStatement");
            }
        }
    }
}