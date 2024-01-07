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
    public class RevenueStreamController : BaseController
    {
        public ActionResult List()
        {
            Revenue_Stream mObjRevenueStream = new Revenue_Stream()
            {
                intStatus = 2
            };

            IList<usp_GetRevenueStreamList_Result> lstRevenueStream = new BLRevenueStream().BL_GetRevenueStreamList(mObjRevenueStream);
            return View(lstRevenueStream);
        }

        public ActionResult Add()
        {
            UI_FillAssetTypeDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(RevenueStreamViewModel pObjRevenueStreamModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillAssetTypeDropDown();
                return View(pObjRevenueStreamModel);
            }
            else
            {
                Revenue_Stream mObjRevenueStream = new Revenue_Stream()
                {
                    RevenueStreamID = 0,
                    RevenueStreamName = pObjRevenueStreamModel.RevenueStreamName.Trim(),  
                    //AssetTypeID = pObjRevenueStreamModel.AssetTypeID,                 
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLRevenueStream().BL_InsertUpdateRevenueStream(mObjRevenueStream);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "RevenueStream");
                    }
                    else
                    {
                        UI_FillAssetTypeDropDown();
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjRevenueStreamModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillAssetTypeDropDown();
                    ViewBag.Message = "Error occurred while saving revenue stream";
                    return View(pObjRevenueStreamModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Revenue_Stream mObjRevenueStream = new Revenue_Stream()
                {
                    RevenueStreamID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetRevenueStreamList_Result mObjRevenueStreamData = new BLRevenueStream().BL_GetRevenueStreamDetails(mObjRevenueStream);

                if (mObjRevenueStreamData != null)
                {
                    RevenueStreamViewModel mObjRevenueStreamModelView = new RevenueStreamViewModel()
                    {
                        RevenueStreamID = mObjRevenueStreamData.RevenueStreamID.GetValueOrDefault(),
                        RevenueStreamName = mObjRevenueStreamData.RevenueStreamName,
                        //AssetTypeID = mObjRevenueStreamData.AssetTypeID.GetValueOrDefault(),
                        Active = mObjRevenueStreamData.Active.GetValueOrDefault(),
                    };

                    //UI_FillAssetTypeDropDown(new Asset_Types() { intStatus = 1, IncludeAssetTypeIds = mObjRevenueStreamModelView.AssetTypeID.ToString() });
                    return View(mObjRevenueStreamModelView);
                }
                else
                {
                    return RedirectToAction("List", "RevenueStream");
                }
            }
            else
            {
                return RedirectToAction("List", "RevenueStream");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(RevenueStreamViewModel pObjRevenueStreamModel)
        {
            if (!ModelState.IsValid)
            {
                //UI_FillAssetTypeDropDown(new Asset_Types() { intStatus = 1, IncludeAssetTypeIds = pObjRevenueStreamModel.AssetTypeID.ToString() });
                return View(pObjRevenueStreamModel);
            }
            else
            {
                Revenue_Stream mObjRevenueStream = new Revenue_Stream()
                {
                    RevenueStreamID = pObjRevenueStreamModel.RevenueStreamID,
                    RevenueStreamName = pObjRevenueStreamModel.RevenueStreamName.Trim(),
                    //AssetTypeID = pObjRevenueStreamModel.AssetTypeID,
                    Active = pObjRevenueStreamModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLRevenueStream().BL_InsertUpdateRevenueStream(mObjRevenueStream);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "RevenueStream");
                    }
                    else
                    {
                        //UI_FillAssetTypeDropDown(new Asset_Types() { intStatus = 1, IncludeAssetTypeIds = pObjRevenueStreamModel.AssetTypeID.ToString() });
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjRevenueStreamModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    //UI_FillAssetTypeDropDown(new Asset_Types() { intStatus = 1, IncludeAssetTypeIds = pObjRevenueStreamModel.AssetTypeID.ToString() });
                    ViewBag.Message = "Error occurred while saving revenue stream";
                    return View(pObjRevenueStreamModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Revenue_Stream mObjRevenueStream = new Revenue_Stream()
                {
                    RevenueStreamID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetRevenueStreamList_Result mObjRevenueStreamData = new BLRevenueStream().BL_GetRevenueStreamDetails(mObjRevenueStream);

                if (mObjRevenueStreamData != null)
                {
                    RevenueStreamViewModel mObjRevenueStreamModelView = new RevenueStreamViewModel()
                    {
                        RevenueStreamID = mObjRevenueStreamData.RevenueStreamID.GetValueOrDefault(),
                        RevenueStreamName = mObjRevenueStreamData.RevenueStreamName,
                        //AssetTypeName = mObjRevenueStreamData.AssetTypeName,
                        ActiveText = mObjRevenueStreamData.ActiveText
                    };

                    return View(mObjRevenueStreamModelView);
                }
                else
                {
                    return RedirectToAction("List", "RevenueStream");
                }
            }
            else
            {
                return RedirectToAction("List", "RevenueStream");
            }
        }

        public JsonResult UpdateStatus(Revenue_Stream pObjRevenueStreamData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjRevenueStreamData.RevenueStreamID != 0)
            {
                FuncResponse mObjFuncResponse = new BLRevenueStream().BL_UpdateStatus(pObjRevenueStreamData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["RevenueStreamList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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