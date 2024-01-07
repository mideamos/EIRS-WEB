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
    public class SizeController : BaseController
    {
        public ActionResult List()
        {
            Size mObjSize = new Size()
            {
                intStatus = 2
            };

            IList<usp_GetSizeList_Result> lstSize = new BLSize().BL_GetSizeList(mObjSize);
            return View(lstSize);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(SizeViewModel pObjSizeModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjSizeModel);
            }
            else
            {
                Size mObjSize = new Size()
                {
                    SizeID = 0,
                    SizeName = pObjSizeModel.SizeName.Trim(),
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLSize().BL_InsertUpdateSize(mObjSize);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "Size");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjSizeModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving size";
                    return View(pObjSizeModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Size mObjSize = new Size()
                {
                    SizeID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetSizeList_Result mObjSizeData = new BLSize().BL_GetSizeDetails(mObjSize);

                if (mObjSizeData != null)
                {
                    SizeViewModel mObjSizeModelView = new SizeViewModel()
                    {
                        SizeID = mObjSizeData.SizeID.GetValueOrDefault(),
                        SizeName = mObjSizeData.SizeName,
                        Active = mObjSizeData.Active.GetValueOrDefault(),
                    };

                    return View(mObjSizeModelView);
                }
                else
                {
                    return RedirectToAction("List", "Size");
                }
            }
            else
            {
                return RedirectToAction("List", "Size");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(SizeViewModel pObjSizeModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjSizeModel);
            }
            else
            {
                Size mObjSize = new Size()
                {
                    SizeID = pObjSizeModel.SizeID,
                    SizeName = pObjSizeModel.SizeName.Trim(),
                    Active = pObjSizeModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLSize().BL_InsertUpdateSize(mObjSize);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "Size");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjSizeModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving size";
                    return View(pObjSizeModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Size mObjSize = new Size()
                {
                    SizeID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetSizeList_Result mObjSizeData = new BLSize().BL_GetSizeDetails(mObjSize);

                if (mObjSizeData != null)
                {
                    SizeViewModel mObjSizeModelView = new SizeViewModel()
                    {
                        SizeID = mObjSizeData.SizeID.GetValueOrDefault(),
                        SizeName = mObjSizeData.SizeName,
                        ActiveText = mObjSizeData.ActiveText
                    };

                    return View(mObjSizeModelView);
                }
                else
                {
                    return RedirectToAction("List", "Size");
                }
            }
            else
            {
                return RedirectToAction("List", "Size");
            }
        }

        public JsonResult UpdateStatus(Size pObjSizeData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjSizeData.SizeID != 0)
            {
                FuncResponse mObjFuncResponse = new BLSize().BL_UpdateStatus(pObjSizeData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["SizeList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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