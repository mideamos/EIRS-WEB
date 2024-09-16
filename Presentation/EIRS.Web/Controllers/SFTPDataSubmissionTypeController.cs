using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Vereyon.Web;
using Elmah;
using EIRS.Web.Models;
using System.IO;
using EIRS.Web.Utility;

namespace EIRS.Web.Controllers
{
    public class SFTPDataSubmissionTypeController : BaseController
    {
        // GET: SFTPDataSubmissionType
        [HttpGet]
        public ActionResult List()
        {
            SFTP_DataSubmissionType mObjDataSubmissionType = new SFTP_DataSubmissionType()
            {
                intStatus = 2
            };

            IList<usp_SFTP_GetDataSubmissionTypeList_Result> lstDataSubmissionType = new BLSFTPDataSubmissionType().BL_GetDataSubmissionTypeList(mObjDataSubmissionType);
            return View(lstDataSubmissionType);
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(SFTPDataSubmissionTypeViewModel pObjDataSubmissionTypeModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjDataSubmissionTypeModel);
            }
            else
            {
                string strTemplateFilePath = "";

                if (pObjDataSubmissionTypeModel.TemplateFile != null && pObjDataSubmissionTypeModel.TemplateFile.ContentLength > 0)
                {

                    string strDirectory = GlobalDefaultValues.DocumentLocation + "SFTPDataSubmissionType/";
                    string mstrFileName = DateTime.Now.ToString("ddMMyyyyhhmmss_") + Path.GetFileName(pObjDataSubmissionTypeModel.TemplateFile.FileName);
                    if (!Directory.Exists(strDirectory))
                    {
                        Directory.CreateDirectory(strDirectory);
                    }

                    string mStrTemplateFilePath = Path.Combine(strDirectory, mstrFileName);
                    pObjDataSubmissionTypeModel.TemplateFile.SaveAs(mStrTemplateFilePath);

                    strTemplateFilePath = "SFTPDataSubmissionType/" + mstrFileName;
                }

                SFTP_DataSubmissionType mObjDataSubmissionType = new SFTP_DataSubmissionType()
                {
                    DataSubmissionTypeID = 0,
                    DataSubmissionTypeName = pObjDataSubmissionTypeModel.DataSubmissionTypeName.Trim(),
                    TemplateFilePath = strTemplateFilePath,
                    Active = true,
                    CreatedBy = SessionManager.UserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLSFTPDataSubmissionType().BL_InsertUpdateDataSubmissionType(mObjDataSubmissionType);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "SFTPDataSubmissionType");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjDataSubmissionTypeModel);
                    }
                }
                catch (Exception ex)
                {
                    Logger.SendErrorToText(ex);
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving DataSubmissionType";
                    return View(pObjDataSubmissionTypeModel);
                }
            }
        }

        [HttpGet]
        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                SFTP_DataSubmissionType mObjDataSubmissionType = new SFTP_DataSubmissionType()
                {
                    DataSubmissionTypeID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_SFTP_GetDataSubmissionTypeList_Result mObjDataSubmissionTypeData = new BLSFTPDataSubmissionType().BL_GetDataSubmissionTypeDetails(mObjDataSubmissionType);

                if (mObjDataSubmissionTypeData != null)
                {
                    SFTPDataSubmissionTypeViewModel mObjDataSubmissionTypeModelView = new SFTPDataSubmissionTypeViewModel()
                    {
                        DataSubmissionTypeID = mObjDataSubmissionTypeData.DataSubmissionTypeID.GetValueOrDefault(),
                        DataSubmissionTypeName = mObjDataSubmissionTypeData.DataSubmissionTypeName,
                        TemplatePath = mObjDataSubmissionTypeData.TemplateFilePath,
                        Active = mObjDataSubmissionTypeData.Active.GetValueOrDefault(),
                    };

                    return View(mObjDataSubmissionTypeModelView);
                }
                else
                {
                    return RedirectToAction("List", "SFTPDataSubmissionType");
                }
            }
            else
            {
                return RedirectToAction("List", "SFTPDataSubmissionType");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(SFTPDataSubmissionTypeViewModel pObjDataSubmissionTypeModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjDataSubmissionTypeModel);
            }
            else
            {
                string strTemplateFilePath = pObjDataSubmissionTypeModel.TemplatePath;

                if (pObjDataSubmissionTypeModel.TemplateFile != null && pObjDataSubmissionTypeModel.TemplateFile.ContentLength > 0)
                {

                    string strDirectory = GlobalDefaultValues.DocumentLocation + "SFTPDataSubmissionType/";
                    string mstrFileName = DateTime.Now.ToString("ddMMyyyyhhmmss_") + Path.GetFileName(pObjDataSubmissionTypeModel.TemplateFile.FileName);
                    if (!Directory.Exists(strDirectory))
                    {
                        Directory.CreateDirectory(strDirectory);
                    }

                    string mStrTemplateFilePath = Path.Combine(strDirectory, mstrFileName);
                    pObjDataSubmissionTypeModel.TemplateFile.SaveAs(mStrTemplateFilePath);

                    strTemplateFilePath = "SFTPDataSubmissionType/" + mstrFileName;
                }

                SFTP_DataSubmissionType mObjDataSubmissionType = new SFTP_DataSubmissionType()
                {
                    DataSubmissionTypeID = pObjDataSubmissionTypeModel.DataSubmissionTypeID,
                    DataSubmissionTypeName = pObjDataSubmissionTypeModel.DataSubmissionTypeName.Trim(),
                    TemplateFilePath = strTemplateFilePath,
                    Active = pObjDataSubmissionTypeModel.Active,
                    ModifiedBy = SessionManager.UserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLSFTPDataSubmissionType().BL_InsertUpdateDataSubmissionType(mObjDataSubmissionType);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "SFTPDataSubmissionType");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjDataSubmissionTypeModel);
                    }
                }
                catch (Exception ex)
                {
                    Logger.SendErrorToText(ex);
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving DataSubmissionType";
                    return View(pObjDataSubmissionTypeModel);
                }
            }
        }

        [HttpGet]
        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                SFTP_DataSubmissionType mObjDataSubmissionType = new SFTP_DataSubmissionType()
                {
                    DataSubmissionTypeID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_SFTP_GetDataSubmissionTypeList_Result mObjDataSubmissionTypeData = new BLSFTPDataSubmissionType().BL_GetDataSubmissionTypeDetails(mObjDataSubmissionType);

                if (mObjDataSubmissionTypeData != null)
                {
                    return View(mObjDataSubmissionTypeData);
                }
                else
                {
                    return RedirectToAction("List", "SFTPDataSubmissionType");
                }
            }
            else
            {
                return RedirectToAction("List", "SFTPDataSubmissionType");
            }
        }
    }
}