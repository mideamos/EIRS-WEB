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
using System.Linq;
using EIRS.Web.Utility;

namespace EIRS.Web.Controllers
{
    public class SFTPDataSubmitterController : BaseController
    {
        // GET: SFTPDataSubmitter
        [HttpGet]
        public ActionResult List()
        {
            SFTP_DataSubmitter mObjDataSubmitter = new SFTP_DataSubmitter()
            {
                intStatus = 2
            };

            IList<usp_SFTP_GetDataSubmitterList_Result> lstDataSubmitter = new BLSFTPDataSubmitter().BL_GetDataSubmitterList(mObjDataSubmitter);
            return View(lstDataSubmitter);
        }

        [HttpGet]
        public ActionResult Add()
        {
            UI_FillSFTPDataSubmissionTypeDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(SFTPDataSubmitterViewModel pObjDataSubmitterModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillSFTPDataSubmissionTypeDropDown();
                return View(pObjDataSubmitterModel);
            }
            else
            {
                BLSFTPDataSubmitter mObjBLSFTPDataSubmitter = new BLSFTPDataSubmitter();
                SFTP_DataSubmitter mObjDataSubmitter = new SFTP_DataSubmitter()
                {
                    DataSubmitterID = 0,
                    RIN = pObjDataSubmitterModel.SubmitterRIN,
                    UserName = pObjDataSubmitterModel.UserName,
                    Password = EncryptDecrypt.Encrypt(pObjDataSubmitterModel.Password),
                    Active = true,
                    CreatedBy = SessionManager.UserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse<SFTP_DataSubmitter> mObjFuncResponse = mObjBLSFTPDataSubmitter.BL_InsertUpdateDataSubmitter(mObjDataSubmitter);

                    if (mObjFuncResponse.Success)
                    {
                        if (pObjDataSubmitterModel.DataSubmissionTypeID != null && pObjDataSubmitterModel.DataSubmissionTypeID.Length > 0)
                        {
                            SFTP_MAP_DataSubmitter_DataSubmissionType mObjDSDST;
                            FuncResponse mObjDSDSTFuncResponse;
                            foreach (int intDataSubmissionTypeID in pObjDataSubmitterModel.DataSubmissionTypeID)
                            {
                                mObjDSDST = new SFTP_MAP_DataSubmitter_DataSubmissionType()
                                {

                                    DataSubmitterID = mObjFuncResponse.AdditionalData.DataSubmitterID,
                                    DataSubmissionTypeID = intDataSubmissionTypeID,
                                    Active = true,
                                    CreatedBy = SessionManager.UserID,
                                    CreatedDate = CommUtil.GetCurrentDateTime()
                                };

                                mObjDSDSTFuncResponse = mObjBLSFTPDataSubmitter.BL_InsertDataSubmissionType(mObjDSDST);

                                if (mObjDSDSTFuncResponse.Success == false)
                                {
                                    throw (mObjDSDSTFuncResponse.Exception);
                                }
                            }

                        }

                        FlashMessage.Info(mObjFuncResponse.Message);
                        return RedirectToAction("List", "SFTPDataSubmitter");
                    }
                    else
                    {
                        UI_FillSFTPDataSubmissionTypeDropDown();
                        ViewBag.Message = mObjFuncResponse.Message;
                        return View(pObjDataSubmitterModel);
                    }
                }
                catch (Exception ex)
                {
                    Logger.SendErrorToText(ex);
                    UI_FillSFTPDataSubmissionTypeDropDown();
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving DataSubmitter";
                    return View(pObjDataSubmitterModel);
                }
            }
        }

        [HttpGet]
        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                SFTP_DataSubmitter mObjDataSubmitter = new SFTP_DataSubmitter()
                {
                    DataSubmitterID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_SFTP_GetDataSubmitterList_Result mObjDataSubmitterData = new BLSFTPDataSubmitter().BL_GetDataSubmitterDetails(mObjDataSubmitter);

                if (mObjDataSubmitterData != null)
                {
                    SFTPDataSubmitterViewModel mObjDataSubmitterModelView = new SFTPDataSubmitterViewModel()
                    {
                        DataSubmitterID = mObjDataSubmitterData.DataSubmitterID.GetValueOrDefault(),
                        SubmitterRIN = mObjDataSubmitterData.RIN,
                        UserName = mObjDataSubmitterData.UserName,
                        Password = EncryptDecrypt.Decrypt(mObjDataSubmitterData.Password),
                        ConfirmPassword = EncryptDecrypt.Decrypt(mObjDataSubmitterData.Password),
                        DataSubmissionTypeID = TrynParse.parseIntArray(mObjDataSubmitterData.DataSubmissionTypeIds),
                        Active = mObjDataSubmitterData.Active.GetValueOrDefault(),
                    };

                    UI_FillSFTPDataSubmissionTypeDropDown();
                    return View(mObjDataSubmitterModelView);
                }
                else
                {
                    return RedirectToAction("List", "SFTPDataSubmitter");
                }
            }
            else
            {
                return RedirectToAction("List", "SFTPDataSubmitter");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(SFTPDataSubmitterViewModel pObjDataSubmitterModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillSFTPDataSubmissionTypeDropDown();
                return View(pObjDataSubmitterModel);
            }
            else
            {
                BLSFTPDataSubmitter mObjBLSFTPDataSubmitter = new BLSFTPDataSubmitter();
                SFTP_DataSubmitter mObjDataSubmitter = new SFTP_DataSubmitter()
                {
                    DataSubmitterID = pObjDataSubmitterModel.DataSubmitterID,
                    RIN = pObjDataSubmitterModel.SubmitterRIN,
                    UserName = pObjDataSubmitterModel.UserName,
                    Password = EncryptDecrypt.Encrypt(pObjDataSubmitterModel.Password),
                    Active = pObjDataSubmitterModel.Active,
                    ModifiedBy = SessionManager.UserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse<SFTP_DataSubmitter> mObjFuncResponse = mObjBLSFTPDataSubmitter.BL_InsertUpdateDataSubmitter(mObjDataSubmitter);

                    if (mObjFuncResponse.Success)
                    {
                        if (pObjDataSubmitterModel.DataSubmissionTypeID != null && pObjDataSubmitterModel.DataSubmissionTypeID.Length > 0)
                        {
                            SFTP_MAP_DataSubmitter_DataSubmissionType mObjDataSubmissionType;
                            IList<SFTP_MAP_DataSubmitter_DataSubmissionType> lstDataSubmissionType = mObjBLSFTPDataSubmitter.BL_GetDataSubmissionTypeList(pObjDataSubmitterModel.DataSubmitterID);

                            int[] intInsertDataSubmissionType = new int[] { };
                            long[] intRemoveDataSubmissionType = new long[] { };

                            if (pObjDataSubmitterModel.DataSubmissionTypeID == null)
                            {
                                intRemoveDataSubmissionType = lstDataSubmissionType.Select(t => t.DSTDSID).ToArray();
                            }
                            else
                            {
                                intRemoveDataSubmissionType = lstDataSubmissionType.Where(t => !pObjDataSubmitterModel.DataSubmissionTypeID.Contains(t.DataSubmissionTypeID.GetValueOrDefault())).Select(t => t.DSTDSID).ToArray();

                                if (lstDataSubmissionType == null || lstDataSubmissionType.Count == 0)
                                {
                                    intInsertDataSubmissionType = pObjDataSubmitterModel.DataSubmissionTypeID;
                                }
                                else
                                {
                                    int[] intDataSubmissionTypeID = lstDataSubmissionType.Select(t => t.DataSubmissionTypeID.GetValueOrDefault()).ToArray();
                                    intInsertDataSubmissionType = pObjDataSubmitterModel.DataSubmissionTypeID.Except(intDataSubmissionTypeID).ToArray();
                                }
                            }

                            foreach (var intDSTDSID in intRemoveDataSubmissionType)
                            {
                                mObjDataSubmissionType = new SFTP_MAP_DataSubmitter_DataSubmissionType()
                                {
                                    DSTDSID = intDSTDSID
                                };

                                mObjBLSFTPDataSubmitter.BL_RemoveDataSubmissionType(mObjDataSubmissionType);
                            }
                            foreach (int intDataSubmissionTypeID in intInsertDataSubmissionType)
                            {
                                mObjDataSubmissionType = new SFTP_MAP_DataSubmitter_DataSubmissionType()
                                {
                                    DSTDSID = 0,
                                    DataSubmitterID = mObjFuncResponse.AdditionalData.DataSubmitterID,
                                    DataSubmissionTypeID = intDataSubmissionTypeID,
                                    Active = true,
                                    CreatedBy = SessionManager.UserID,
                                    CreatedDate = CommUtil.GetCurrentDateTime()
                                };

                                FuncResponse mObjDSDSTFuncResponse = mObjBLSFTPDataSubmitter.BL_InsertDataSubmissionType(mObjDataSubmissionType);

                                if (mObjDSDSTFuncResponse.Success == false)
                                {
                                    throw (mObjDSDSTFuncResponse.Exception);
                                }
                            }
                        }

                        FlashMessage.Info(mObjFuncResponse.Message);
                        return RedirectToAction("List", "SFTPDataSubmitter");
                    }
                    else
                    {
                        UI_FillSFTPDataSubmissionTypeDropDown();
                        ViewBag.Message = mObjFuncResponse.Message;
                        return View(pObjDataSubmitterModel);
                    }
                }
                catch (Exception ex)
                {
                    Logger.SendErrorToText(ex);
                    UI_FillSFTPDataSubmissionTypeDropDown();
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving DataSubmitter";
                    return View(pObjDataSubmitterModel);
                }
            }
        }

        [HttpGet]
        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                SFTP_DataSubmitter mObjDataSubmitter = new SFTP_DataSubmitter()
                {
                    DataSubmitterID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_SFTP_GetDataSubmitterList_Result mObjDataSubmitterData = new BLSFTPDataSubmitter().BL_GetDataSubmitterDetails(mObjDataSubmitter);

                if (mObjDataSubmitterData != null)
                {
                    return View(mObjDataSubmitterData);
                }
                else
                {
                    return RedirectToAction("List", "SFTPDataSubmitter");
                }
            }
            else
            {
                return RedirectToAction("List", "SFTPDataSubmitter");
            }
        }
    }
}