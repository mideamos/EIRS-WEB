using EIRS.Web.Models;
using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Vereyon.Web;
using Elmah;
using System.IO;
using EIRS.Web.Utility;

namespace EIRS.Web.Controllers
{

    public class DataSubmitterPanelController : Controller
    {
        private void UI_FillDropDown()
        {
            IList<DropDownListResult> lstYear = new List<DropDownListResult>();
            int mIntCurrentYear = 2017;//CommUtil.GetCurrentDateTime().AddYears(-1).Year;
            for (int i = mIntCurrentYear; i <= DateTime.Now.AddYears(1).Year; i++)
            {
                lstYear.Add(new DropDownListResult() { id = i, text = i.ToString() });
            }

            ViewBag.YearList = new SelectList(lstYear, "id", "text");

            IList<DropDownListResult> lstDataSubmissionType = new BLSFTPDataSubmitter().BL_GetDataSubmissionTypeDropDownList(new SFTP_DataSubmitter() { intStatus = 1, DataSubmitterID = SessionManager.DataSubmitterID });
            ViewBag.DataSubmissionTypeList = new SelectList(lstDataSubmissionType, "id", "text");
        }

        [HttpGet]
        public ActionResult SubmitData()
        {
            UI_FillDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult SubmitData(DS_DocumentSubmissionViewModel pObjDocumentSubmission)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown();
                return View(pObjDocumentSubmission);
            }
            else
            {
                string strDocumentPath = "";

                if (pObjDocumentSubmission.DocumentFile != null && pObjDocumentSubmission.DocumentFile.ContentLength > 0)
                {

                    string strDirectory = GlobalDefaultValues.DocumentLocation + "SFTPDataSubmission/" + SessionManager.DataSubmitterID + "/";
                    string mstrFileName = DateTime.Now.ToString("ddMMyyyyhhmmss_") + Path.GetFileName(pObjDocumentSubmission.DocumentFile.FileName);
                    if (!Directory.Exists(strDirectory))
                    {
                        Directory.CreateDirectory(strDirectory);
                    }

                    string mStrDocumentFilePath = Path.Combine(strDirectory, mstrFileName);
                    pObjDocumentSubmission.DocumentFile.SaveAs(mStrDocumentFilePath);

                    strDocumentPath = "SFTPDataSubmission/" + SessionManager.DataSubmitterID + "/" + mstrFileName;
                }

                SFTP_DataSubmission mObjDataSubmission = new SFTP_DataSubmission()
                {
                    DataSubmissionID = 0,
                    DataSubmitterID = SessionManager.DataSubmitterID,
                    TaxYear = pObjDocumentSubmission.TaxYear,
                    DataSubmissionTypeID = pObjDocumentSubmission.DataSubmissionTypeID,
                    DocumentPath = strDocumentPath,
                    SubmissionDate = CommUtil.GetCurrentDateTime(),
                    CreatedBy = SessionManager.UserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLSFTPDataSubmission().BL_InsertUpdateDataSubmission(mObjDataSubmission);

                    if (mObjResponse.Success)
                    {
                        UI_FillDropDown();
                        FlashMessage.Info(mObjResponse.Message);
                        ModelState.Clear();
                        return View();
                    }
                    else
                    {
                        UI_FillDropDown();
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjDocumentSubmission);
                    }
                }
                catch (Exception ex)
                {
                    Logger.SendErrorToText(ex);
                    UI_FillDropDown();
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving data submission";
                    return View(pObjDocumentSubmission);
                }
            }
        }

        // GET: DataSubmitterPanel
        public ActionResult Log()
        {
            IList<usp_SFTP_GetDataSubmissionList_Result> lstDocumentSubmission = new BLSFTPDataSubmission().BL_GetDataSubmissionList(new SFTP_DataSubmission() { DataSubmitterID = SessionManager.DataSubmitterID });
            return View(lstDocumentSubmission);
        }

        [HttpGet]
        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                usp_SFTP_GetDataSubmissionList_Result mObjDataSubmissionData = new BLSFTPDataSubmission().BL_GetDataSubmissionDetails(new SFTP_DataSubmission() { DataSubmissionID = id.GetValueOrDefault(), DataSubmitterID = SessionManager.DataSubmitterID });

                if (mObjDataSubmissionData != null)
                {
                    return View(mObjDataSubmissionData);
                }
                else
                {
                    return RedirectToAction("Log", "DataSubmitterPanel");
                }
            }
            else
            {
                return RedirectToAction("Log", "DataSubmitterPanel");
            }
        }
    }
}