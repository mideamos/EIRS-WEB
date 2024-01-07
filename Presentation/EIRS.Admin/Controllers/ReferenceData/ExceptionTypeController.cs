using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vereyon.Web;
using Elmah;

namespace EIRS.Admin.Controllers.ReferenceData
{
    public class ExceptionTypeController : BaseController
    {
        // GET: ExceptionType
        public ActionResult List()
        {
            Exception_Type mObjExceptionType = new Exception_Type()
            {
                intStatus = 2
            };

            IList<usp_GetExceptionTypeList_Result> lstExceptionType = new BLExceptionType().BL_GetExceptionTypeList(mObjExceptionType);
            return View(lstExceptionType);
        }

        public JsonResult UpdateStatus(Exception_Type pObjExceptionTypeData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjExceptionTypeData.ExceptionTypeID != 0)
            {
                FuncResponse mObjFuncResponse = new BLExceptionType().BL_UpdateStatus(pObjExceptionTypeData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["ExceptionTypeList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
                }

            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Action";
            }
            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(ExceptionTypeViewModel pObjExceptionTypeModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjExceptionTypeModel);
            }
            else
            {
                Exception_Type mObjExceptionType = new Exception_Type()
                {
                    ExceptionTypeID = 0,
                    ExceptionTypeName = pObjExceptionTypeModel.ExceptionTypeName.Trim(),
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLExceptionType().BL_InsertUpdateExceptionType(mObjExceptionType);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "ExceptionType");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjExceptionTypeModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving Exception Type";
                    return View(pObjExceptionTypeModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Exception_Type mObjExceptionType = new Exception_Type()
                {
                    ExceptionTypeID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetExceptionTypeList_Result mObjExceptionTypeData = new BLExceptionType().BL_GetExceptionTypeDetails(mObjExceptionType);

                if (mObjExceptionTypeData != null)
                {
                    ExceptionTypeViewModel mObjExceptionTypeModelView = new ExceptionTypeViewModel()
                    {
                        ExceptionTypeID = mObjExceptionTypeData.ExceptionTypeID.GetValueOrDefault(),
                        ExceptionTypeName = mObjExceptionTypeData.ExceptionTypeName,
                        Active = mObjExceptionTypeData.Active.GetValueOrDefault(),
                    };

                    return View(mObjExceptionTypeModelView);
                }
                else
                {
                    return RedirectToAction("List", "ExceptionType");
                }
            }
            else
            {
                return RedirectToAction("List", "ExceptionType");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(ExceptionTypeViewModel pObjExceptionTypeModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjExceptionTypeModel);
            }
            else
            {
                Exception_Type mObjExceptionType = new Exception_Type()
                {
                    ExceptionTypeID = pObjExceptionTypeModel.ExceptionTypeID,
                    ExceptionTypeName = pObjExceptionTypeModel.ExceptionTypeName.Trim(),
                    Active = pObjExceptionTypeModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLExceptionType().BL_InsertUpdateExceptionType(mObjExceptionType);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "ExceptionType");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjExceptionTypeModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving Exception Type";
                    return View(pObjExceptionTypeModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Exception_Type mObjExceptionType = new Exception_Type()
                {
                    ExceptionTypeID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetExceptionTypeList_Result mObjExceptionTypeData = new BLExceptionType().BL_GetExceptionTypeDetails(mObjExceptionType);

                if (mObjExceptionTypeData != null)
                {
                    ExceptionTypeViewModel mObjExceptionTypeModelView = new ExceptionTypeViewModel()
                    {
                        ExceptionTypeID = mObjExceptionTypeData.ExceptionTypeID.GetValueOrDefault(),
                        ExceptionTypeName = mObjExceptionTypeData.ExceptionTypeName,
                        ActiveText = mObjExceptionTypeData.ActiveText
                    };

                    return View(mObjExceptionTypeModelView);
                }
                else
                {
                    return RedirectToAction("List", "ExceptionType");
                }
            }
            else
            {
                return RedirectToAction("List", "ExceptionType");
            }
        }
    }
}