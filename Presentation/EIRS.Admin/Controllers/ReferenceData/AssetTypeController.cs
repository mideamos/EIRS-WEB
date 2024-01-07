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
    public class AssetTypeController : BaseController
    {
        public ActionResult List()
        {
            Asset_Types mObjAssetType = new Asset_Types()
            {
                intStatus = 2
            };

            IList<usp_GetAssetTypeList_Result> lstAssetType = new BLAssetType().BL_GetAssetTypeList(mObjAssetType);
            return View(lstAssetType);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(AssetTypeViewModel pObjAssetTypeModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjAssetTypeModel);
            }
            else
            {
                Asset_Types mObjAssetType = new Asset_Types()
                {
                    AssetTypeID = 0,
                    AssetTypeName = pObjAssetTypeModel.AssetTypeName.Trim(),
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLAssetType().BL_InsertUpdateAssetType(mObjAssetType);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "AssetType");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjAssetTypeModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving asset type";
                    return View(pObjAssetTypeModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Asset_Types mObjAssetType = new Asset_Types()
                {
                    AssetTypeID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetAssetTypeList_Result mObjAssetTypeData = new BLAssetType().BL_GetAssetTypeDetails(mObjAssetType);

                if (mObjAssetTypeData != null)
                {
                    AssetTypeViewModel mObjAssetTypeModelView = new AssetTypeViewModel()
                    {
                        AssetTypeID = mObjAssetTypeData.AssetTypeID.GetValueOrDefault(),
                        AssetTypeName = mObjAssetTypeData.AssetTypeName,
                        Active = mObjAssetTypeData.Active.GetValueOrDefault(),
                    };

                    return View(mObjAssetTypeModelView);
                }
                else
                {
                    return RedirectToAction("List", "AssetType");
                }
            }
            else
            {
                return RedirectToAction("List", "AssetType");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(AssetTypeViewModel pObjAssetTypeModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjAssetTypeModel);
            }
            else
            {
                Asset_Types mObjAssetType = new Asset_Types()
                {
                    AssetTypeID = pObjAssetTypeModel.AssetTypeID,
                    AssetTypeName = pObjAssetTypeModel.AssetTypeName.Trim(),
                    Active = pObjAssetTypeModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLAssetType().BL_InsertUpdateAssetType(mObjAssetType);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "AssetType");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjAssetTypeModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving asset type";
                    return View(pObjAssetTypeModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Asset_Types mObjAssetType = new Asset_Types()
                {
                    AssetTypeID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetAssetTypeList_Result mObjAssetTypeData = new BLAssetType().BL_GetAssetTypeDetails(mObjAssetType);

                if (mObjAssetTypeData != null)
                {
                    AssetTypeViewModel mObjAssetTypeModelView = new AssetTypeViewModel()
                    {
                        AssetTypeID = mObjAssetTypeData.AssetTypeID.GetValueOrDefault(),
                        AssetTypeName = mObjAssetTypeData.AssetTypeName,
                        ActiveText = mObjAssetTypeData.ActiveText
                    };

                    return View(mObjAssetTypeModelView);
                }
                else
                {
                    return RedirectToAction("List", "AssetType");
                }
            }
            else
            {
                return RedirectToAction("List", "AssetType");
            }
        }

        public JsonResult UpdateStatus(Asset_Types pObjAssetTypeData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjAssetTypeData.AssetTypeID != 0)
            {
                FuncResponse mObjFuncResponse = new BLAssetType().BL_UpdateStatus(pObjAssetTypeData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["AssetTypeList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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