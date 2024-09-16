using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using Vereyon.Web;
using Elmah;
using EIRS.Models;
using System.Transactions;
using System.IO;
using static EIRS.Web.Controllers.Filters;
using EIRS.Web.Utility;

namespace EIRS.Web.Controllers
{
    [SessionTimeout]
    public class APIController : BaseController
    {
        public ActionResult List()
        {
            IList<SelectListItem> lstStatus = new List<SelectListItem>();
            lstStatus.Add(new SelectListItem() { Value = "2", Text = "All" });
            lstStatus.Add(new SelectListItem() { Value = "1", Text = "Active", Selected = true });
            lstStatus.Add(new SelectListItem() { Value = "0", Text = "In Active" });

            ViewBag.StatusList = lstStatus;

            MST_API mObjAPI = new MST_API()
            {
                intStatus = 1
            };

            IList<usp_GetAPIList_Result> lstAPI = new BLAPI().BL_GetAPIList(mObjAPI);
            return View(lstAPI);
        }

        public ActionResult SearchData(FormCollection p_ObjFormCollection)
        {
            string strTitleFilter = p_ObjFormCollection.Get("txtFilter");
            int intStatus = TrynParse.parseInt(p_ObjFormCollection.Get("cboStatus"));

            MST_API mObjAPI = new MST_API()
            {
                intStatus = intStatus,
                APIName = strTitleFilter
            };

            IList<usp_GetAPIList_Result> lstAPI = new BLAPI().BL_GetAPIList(mObjAPI);
            return PartialView(lstAPI);
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                MST_API mObjAPI = new MST_API()
                {
                    APIID = id.GetValueOrDefault(),
                    intStatus = 2,
                };

                usp_GetAPIList_Result mObjAPIData = new BLAPI().BL_GetAPIDetails(mObjAPI);

                if (mObjAPIData != null)
                {
                    APIViewModel mObjAPIModelView = new APIViewModel()
                    {
                        APIID = mObjAPIData.APIID.GetValueOrDefault(),
                        APIName = mObjAPIData.APIName,
                        APIDescription = mObjAPIData.APIDescription,
                        DocumentPath = mObjAPIData.DocumentPath,
                        Active = mObjAPIData.Active.GetValueOrDefault(),
                    };

                    return View(mObjAPIModelView);
                }
                else
                {
                    return RedirectToAction("List", "API");
                }
            }
            else
            {
                return RedirectToAction("List", "API");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        [ValidateInput(false)]
        public ActionResult Edit(APIViewModel pObjAPIModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjAPIModel);
            }
            else
            {
                string strDocumentPath = pObjAPIModel.DocumentPath;

                if (pObjAPIModel.APIDocument != null && pObjAPIModel.APIDocument.ContentLength > 0)
                {
                    string strDirectory = GlobalDefaultValues.DocumentLocation + "API/";
                    string mStrFileName = DateTime.Now.ToString("ddMMyyyyhhmmss_") + Path.GetFileName(pObjAPIModel.APIDocument.FileName);
                    if (!Directory.Exists(strDirectory))
                        Directory.CreateDirectory(strDirectory);
                    string mstrDocumentPath = Path.Combine(strDirectory, mStrFileName);
                    pObjAPIModel.APIDocument.SaveAs(mstrDocumentPath);

                    //Delete Existing File
                    if (System.IO.File.Exists(GlobalDefaultValues.DocumentLocation + pObjAPIModel.DocumentPath))
                    {
                        System.IO.File.Delete(GlobalDefaultValues.DocumentLocation + pObjAPIModel.DocumentPath);
                    }

                    strDocumentPath = "API/" + mStrFileName;
                }


                MST_API mObjAPI = new MST_API()
                {
                    APIID = pObjAPIModel.APIID,
                    APIName = pObjAPIModel.APIName,
                    APIDescription = pObjAPIModel.APIDescription,
                    DocumentPath = strDocumentPath,
                    Active = pObjAPIModel.Active,
                    ModifiedBy = SessionManager.UserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLAPI().BL_UpdateAPI(mObjAPI);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "API");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjAPIModel);
                    }
                }
                catch (Exception ex)
                {
                    Logger.SendErrorToText(ex);
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving API";
                    return View(pObjAPIModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                MST_API mObjAPI = new MST_API()
                {
                    APIID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetAPIList_Result mObjAPIData = new BLAPI().BL_GetAPIDetails(mObjAPI);

                if (mObjAPIData != null)
                {
                    APIViewModel mObjAPIModelView = new APIViewModel()
                    {
                        APIName = mObjAPIData.APIName,
                        APIDescription = mObjAPIData.APIDescription,
                        DocumentPath = mObjAPIData.DocumentPath,
                        ActiveText = mObjAPIData.ActiveText
                    };

                    return View(mObjAPIModelView);
                }
                else
                {
                    return RedirectToAction("List", "API");
                }
            }
            else
            {
                return RedirectToAction("List", "API");
            }
        }

        public ActionResult UserAccess(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                MST_API mObjAPI = new MST_API()
                {
                    APIID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetAPIList_Result mObjAPIData = new BLAPI().BL_GetAPIDetails(mObjAPI);

                if (mObjAPIData != null)
                {
                    ViewBag.APIDetails = mObjAPIData;

                    IList<usp_GetAPIUserRightList_Result> lstUserAccess = new BLAPI().BL_GetAPIAccessList(0, id.GetValueOrDefault());
                    return View(lstUserAccess);
                }
                else
                {
                    return RedirectToAction("List", "API");
                }
            }
            else
            {
                return RedirectToAction("List", "API");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult UserAccess(FormCollection pObjFormCollection)
        {
            //Get API ID
            int mIntAPIID = TrynParse.parseInt(pObjFormCollection.Get("hdnAPIID"));

            if (mIntAPIID > 0)
            {
                MST_API mObjAPI = new MST_API()
                {
                    APIID = mIntAPIID,
                    intStatus = 2
                };

                usp_GetAPIList_Result mObjAPIData = new BLAPI().BL_GetAPIDetails(mObjAPI);

                if (mObjAPIData != null)
                {
                    ViewBag.APIDetails = mObjAPIData;

                    IList<usp_GetAPIUserRightList_Result> lstUserAccess = new BLAPI().BL_GetAPIAccessList(0, mIntAPIID);

                    using (TransactionScope mobjScope = new TransactionScope())
                    {
                        try
                        {
                            //Start Saving Access Right
                            foreach (var vAPI in lstUserAccess)
                            {
                                string mStrUserAccess = "chkUserAccess_" + vAPI.UAID;


                                MAP_API_Users_Rights mObjUserRight;
                                BLAPI mObjBLAPI = new BLAPI();
                                if (pObjFormCollection.Get(mStrUserAccess) != null)
                                {
                                    mObjUserRight = new MAP_API_Users_Rights()
                                    {
                                        UAID = vAPI.UAID.GetValueOrDefault(),
                                        APIAccess = TrynParse.parseBool(pObjFormCollection.Get(mStrUserAccess).Split(',')[0])
                                    };

                                    FuncResponse mObjFuncResponse = mObjBLAPI.BL_UpdateAPIAccess(mObjUserRight);

                                    if (mObjFuncResponse.Success == false)
                                    {
                                        throw new Exception(mObjFuncResponse.Message);
                                    }
                                }
                            }

                            mobjScope.Complete();
                            FlashMessage.Info("API Access Updated Successfully");
                            return RedirectToAction("List", "API");
                        }
                        catch (Exception ex)
                        {
                            Logger.SendErrorToText(ex);
                            Transaction.Current.Rollback();
                            ViewBag.APIDetails = mObjAPIData;
                            return View(lstUserAccess);
                        }
                    }
                }
                else
                {
                    return RedirectToAction("List", "API");
                }
            }
            else
            {
                return RedirectToAction("List", "API");
            }
        }
    }
}