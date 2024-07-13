using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Vereyon.Web;
using Elmah;

namespace EIRS.Admin.Controllers
{
    public class RevenueSubStreamController : BaseController
    {
        public ActionResult List()
        {
            Revenue_SubStream mObjRevenueSubStream = new Revenue_SubStream()
            {
                intStatus = 2
            };

            IList<usp_GetRevenueSubStreamList_Result> lstRevenueSubStream = new BLRevenueSubStream().BL_GetRevenueSubStreamList(mObjRevenueSubStream);
            return View(lstRevenueSubStream);
        }

        public void UI_FillDropDown(RevenueSubStreamViewModel pObjRevenueSubStreamModel = null)
        {
            if (pObjRevenueSubStreamModel == null)
                pObjRevenueSubStreamModel = new RevenueSubStreamViewModel();

            //UI_FillAssetTypeDropDown(new Asset_Types() { intStatus = 1, IncludeAssetTypeIds = pObjRevenueSubStreamModel.AssetTypeID.ToString() });
            UI_FillRevenueStreamDropDown(new Revenue_Stream() { intStatus = 1, /*AssetTypeID = pObjRevenueSubStreamModel.AssetTypeID,*/ IncludeRevenueStreamIds = pObjRevenueSubStreamModel.RevenueStreamID.ToString() });
        }

        public ActionResult Add()
        {
            UI_FillDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(RevenueSubStreamViewModel pObjRevenueSubStreamModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown(pObjRevenueSubStreamModel);
                return View(pObjRevenueSubStreamModel);
            }
            else
            {
                Revenue_SubStream mObjRevenueSubStream = new Revenue_SubStream()
                {
                    RevenueSubStreamID = 0,
                    RevenueSubStreamName = pObjRevenueSubStreamModel.RevenueSubStreamName.Trim(),
                    RevenueStreamID = pObjRevenueSubStreamModel.RevenueStreamID,
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLRevenueSubStream().BL_InsertUpdateRevenueSubStream(mObjRevenueSubStream);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "RevenueSubStream");
                    }
                    else
                    {
                        UI_FillDropDown(pObjRevenueSubStreamModel);
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjRevenueSubStreamModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillDropDown(pObjRevenueSubStreamModel);
                    ViewBag.Message = "Error occurred while saving revenue sub stream";
                    return View(pObjRevenueSubStreamModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Revenue_SubStream mObjRevenueSubStream = new Revenue_SubStream()
                {
                    RevenueSubStreamID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetRevenueSubStreamList_Result mObjRevenueSubStreamData = new BLRevenueSubStream().BL_GetRevenueSubStreamDetails(mObjRevenueSubStream);

                if (mObjRevenueSubStreamData != null)
                {
                    RevenueSubStreamViewModel mObjRevenueSubStreamModelView = new RevenueSubStreamViewModel()
                    {
                        RevenueSubStreamID = mObjRevenueSubStreamData.RevenueSubStreamID.GetValueOrDefault(),
                        RevenueSubStreamName = mObjRevenueSubStreamData.RevenueSubStreamName,
                        RevenueStreamID = mObjRevenueSubStreamData.RevenueStreamID.GetValueOrDefault(),
                        //AssetTypeID = mObjRevenueSubStreamData.AssetTypeID.GetValueOrDefault(),
                        Active = mObjRevenueSubStreamData.Active.GetValueOrDefault(),
                    };

                    UI_FillDropDown(mObjRevenueSubStreamModelView);
                    return View(mObjRevenueSubStreamModelView);
                }
                else
                {
                    return RedirectToAction("List", "RevenueSubStream");
                }
            }
            else
            {
                return RedirectToAction("List", "RevenueSubStream");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(RevenueSubStreamViewModel pObjRevenueSubStreamModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown(pObjRevenueSubStreamModel);
                return View(pObjRevenueSubStreamModel);
            }
            else
            {
                Revenue_SubStream mObjRevenueSubStream = new Revenue_SubStream()
                {
                    RevenueSubStreamID = pObjRevenueSubStreamModel.RevenueSubStreamID,
                    RevenueSubStreamName = pObjRevenueSubStreamModel.RevenueSubStreamName.Trim(),
                    RevenueStreamID = pObjRevenueSubStreamModel.RevenueStreamID,
                    Active = pObjRevenueSubStreamModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLRevenueSubStream().BL_InsertUpdateRevenueSubStream(mObjRevenueSubStream);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "RevenueSubStream");
                    }
                    else
                    {
                        UI_FillDropDown(pObjRevenueSubStreamModel);
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjRevenueSubStreamModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillDropDown(pObjRevenueSubStreamModel);
                    ViewBag.Message = "Error occurred while saving revenue sub stream";
                    return View(pObjRevenueSubStreamModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Revenue_SubStream mObjRevenueSubStream = new Revenue_SubStream()
                {
                    RevenueSubStreamID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetRevenueSubStreamList_Result mObjRevenueSubStreamData = new BLRevenueSubStream().BL_GetRevenueSubStreamDetails(mObjRevenueSubStream);

                if (mObjRevenueSubStreamData != null)
                {
                    RevenueSubStreamViewModel mObjRevenueSubStreamModelView = new RevenueSubStreamViewModel()
                    {
                        RevenueSubStreamID = mObjRevenueSubStreamData.RevenueSubStreamID.GetValueOrDefault(),
                        RevenueSubStreamName = mObjRevenueSubStreamData.RevenueSubStreamName,
                        RevenueStreamName = mObjRevenueSubStreamData.RevenueStreamName,
                        //AssetTypeName = mObjRevenueSubStreamData.AssetTypeName,
                        ActiveText = mObjRevenueSubStreamData.ActiveText
                    };

                    return View(mObjRevenueSubStreamModelView);
                }
                else
                {
                    return RedirectToAction("List", "RevenueSubStream");
                }
            }
            else
            {
                return RedirectToAction("List", "RevenueSubStream");
            }
        }

        public JsonResult UpdateStatus(Revenue_SubStream pObjRevenueSubStreamData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjRevenueSubStreamData.RevenueSubStreamID != 0)
            {
                FuncResponse mObjFuncResponse = new BLRevenueSubStream().BL_UpdateStatus(pObjRevenueSubStreamData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["RevenueSubStreamList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
                }

            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Action";
            }



            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }
    }
}