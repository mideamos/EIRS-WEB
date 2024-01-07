using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Vereyon.Web;
using Elmah;
using EIRS.Web.Utility;

namespace EIRS.Web.Controllers
{
    public class EMCategoryController : BaseController
    {
        // GET: EMCategory
        [HttpGet]
        public ActionResult List()
        {
            EM_Category mObjCategory = new EM_Category()
            {
                intStatus = 2
            };

            IList<usp_EM_GetCategoryList_Result> lstCategory = new BLEMCategory().BL_GetCategoryList(mObjCategory);
            return View(lstCategory);
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(EMCategoryViewModel pObjCategoryModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjCategoryModel);
            }
            else
            {
                EM_Category mObjCategory = new EM_Category()
                {
                    CategoryID = 0,
                    CategoryName = pObjCategoryModel.CategoryName.Trim(),
                    Active = true,
                    CreatedBy = SessionManager.UserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLEMCategory().BL_InsertUpdateCategory(mObjCategory);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "EMCategory");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjCategoryModel);
                    }
                }
                catch (Exception ex)
                {
                    Logger.SendErrorToText(ex);
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving category";
                    return View(pObjCategoryModel);
                }
            }
        }

        [HttpGet]
        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                EM_Category mObjCategory = new EM_Category()
                {
                    CategoryID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_EM_GetCategoryList_Result mObjCategoryData = new BLEMCategory().BL_GetCategoryDetails(mObjCategory);

                if (mObjCategoryData != null)
                {
                    EMCategoryViewModel mObjCategoryModelView = new EMCategoryViewModel()
                    {
                        CategoryID = mObjCategoryData.CategoryID.GetValueOrDefault(),
                        CategoryName = mObjCategoryData.CategoryName,
                        Active = mObjCategoryData.Active.GetValueOrDefault(),
                    };

                    return View(mObjCategoryModelView);
                }
                else
                {
                    return RedirectToAction("List", "EMCategory");
                }
            }
            else
            {
                return RedirectToAction("List", "EMCategory");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(EMCategoryViewModel pObjCategoryModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjCategoryModel);
            }
            else
            {
                EM_Category mObjCategory = new EM_Category()
                {
                    CategoryID = pObjCategoryModel.CategoryID,
                    CategoryName = pObjCategoryModel.CategoryName.Trim(),
                    Active = pObjCategoryModel.Active,
                    ModifiedBy = SessionManager.UserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLEMCategory().BL_InsertUpdateCategory(mObjCategory);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "EMCategory");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjCategoryModel);
                    }
                }
                catch (Exception ex)
                {
                    Logger.SendErrorToText(ex);
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving category";
                    return View(pObjCategoryModel);
                }
            }
        }

        [HttpGet]
        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                EM_Category mObjCategory = new EM_Category()
                {
                    CategoryID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_EM_GetCategoryList_Result mObjCategoryData = new BLEMCategory().BL_GetCategoryDetails(mObjCategory);

                if (mObjCategoryData != null)
                {
                    return View(mObjCategoryData);
                }
                else
                {
                    return RedirectToAction("List", "EMCategory");
                }
            }
            else
            {
                return RedirectToAction("List", "EMCategory");
            }
        }

        public JsonResult UpdateStatus(EM_Category pObjCategoryData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjCategoryData.CategoryID != 0)
            {
                FuncResponse mObjFuncResponse = new BLEMCategory().BL_UpdateStatus(pObjCategoryData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["CategoryList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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