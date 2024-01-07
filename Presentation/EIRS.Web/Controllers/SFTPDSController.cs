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
    public class SFTPDSController : BaseController
    {
        private void UI_FillDropDown(DocumentSubmissionViewModel pObjDocumentSubmission = null)
        {
            if (pObjDocumentSubmission == null)
            {
                pObjDocumentSubmission = new DocumentSubmissionViewModel()
                {
                    DataSubmitterID = -1
                };
            }

            UI_FillSFTPDataSubmitterDropDown();
            UI_FillYearDropDown();
            UI_FillSFTPDSDSTDropDown(new SFTP_DataSubmitter() { intStatus = 1, DataSubmitterID = pObjDocumentSubmission.DataSubmitterID });

        }

        [HttpGet]
        public ActionResult SubmitData()
        {
            UI_FillDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult SubmitData(DocumentSubmissionViewModel pObjDocumentSubmission)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown(pObjDocumentSubmission);
                return View(pObjDocumentSubmission);
            }
            else
            {
                string strDocumentPath = "";

                if (pObjDocumentSubmission.DocumentFile != null && pObjDocumentSubmission.DocumentFile.ContentLength > 0)
                {

                    string strDirectory = GlobalDefaultValues.DocumentLocation + "SFTPDataSubmission/" + pObjDocumentSubmission.DataSubmitterID + "/";
                    string mstrFileName = DateTime.Now.ToString("ddMMyyyyhhmmss_") + Path.GetFileName(pObjDocumentSubmission.DocumentFile.FileName);
                    if (!Directory.Exists(strDirectory))
                    {
                        Directory.CreateDirectory(strDirectory);
                    }

                    string mStrDocumentFilePath = Path.Combine(strDirectory, mstrFileName);
                    pObjDocumentSubmission.DocumentFile.SaveAs(mStrDocumentFilePath);

                    strDocumentPath = "SFTPDataSubmission/" + pObjDocumentSubmission.DataSubmitterID + "/" + mstrFileName;
                }

                SFTP_DataSubmission mObjDataSubmission = new SFTP_DataSubmission()
                {
                    DataSubmissionID = 0,
                    DataSubmitterID = pObjDocumentSubmission.DataSubmitterID,
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
                        UI_FillDropDown(pObjDocumentSubmission);
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjDocumentSubmission);
                    }
                }
                catch (Exception ex)
                {
                    Logger.SendErrorToText(ex);
                    UI_FillDropDown(pObjDocumentSubmission);
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving data submission";
                    return View(pObjDocumentSubmission);
                }
            }
        }

        [HttpGet]
        public ActionResult DocumentSubmission()
        {
            IList<usp_SFTP_GetDataSubmissionList_Result> lstDocumentSubmission = new BLSFTPDataSubmission().BL_GetDataSubmissionList(new SFTP_DataSubmission() { });
            return View(lstDocumentSubmission);
        }

        [HttpGet]
        public ActionResult SubmissionDetails(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                usp_SFTP_GetDataSubmissionList_Result mObjDataSubmissionData = new BLSFTPDataSubmission().BL_GetDataSubmissionDetails(new SFTP_DataSubmission() { DataSubmissionID = id.GetValueOrDefault() });

                if (mObjDataSubmissionData != null)
                {
                    return View(mObjDataSubmissionData);
                }
                else
                {
                    return RedirectToAction("DocumentSubmission", "SFTPDS");
                }
            }
            else
            {
                return RedirectToAction("DocumentSubmission", "SFTPDS");
            }
        }
    }
}