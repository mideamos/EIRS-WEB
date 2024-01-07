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
    public class GovernmentTypeController : BaseController
    {
        public ActionResult List()
        {
            Government_Types mObjGovernmentType = new Government_Types()
            {
                intStatus = 2
            };

            IList<usp_GetGovernmentTypeList_Result> lstGovernmentType = new BLGovernmentType().BL_GetGovernmentTypeList(mObjGovernmentType);
            return View(lstGovernmentType);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(GovernmentTypeViewModel pObjGovernmentTypeModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjGovernmentTypeModel);
            }
            else
            {
                Government_Types mObjGovernmentType = new Government_Types()
                {
                    GovernmentTypeID = 0,
                    GovernmentTypeName = pObjGovernmentTypeModel.GovernmentTypeName.Trim(),
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLGovernmentType().BL_InsertUpdateGovernmentType(mObjGovernmentType);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "GovernmentType");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjGovernmentTypeModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving government type";
                    return View(pObjGovernmentTypeModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Government_Types mObjGovernmentType = new Government_Types()
                {
                    GovernmentTypeID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetGovernmentTypeList_Result mObjGovernmentTypeData = new BLGovernmentType().BL_GetGovernmentTypeDetails(mObjGovernmentType);

                if (mObjGovernmentTypeData != null)
                {
                    GovernmentTypeViewModel mObjGovernmentTypeModelView = new GovernmentTypeViewModel()
                    {
                        GovernmentTypeID = mObjGovernmentTypeData.GovernmentTypeID.GetValueOrDefault(),
                        GovernmentTypeName = mObjGovernmentTypeData.GovernmentTypeName,
                        Active = mObjGovernmentTypeData.Active.GetValueOrDefault(),
                    };

                    return View(mObjGovernmentTypeModelView);
                }
                else
                {
                    return RedirectToAction("List", "GovernmentType");
                }
            }
            else
            {
                return RedirectToAction("List", "GovernmentType");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(GovernmentTypeViewModel pObjGovernmentTypeModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjGovernmentTypeModel);
            }
            else
            {
                Government_Types mObjGovernmentType = new Government_Types()
                {
                    GovernmentTypeID = pObjGovernmentTypeModel.GovernmentTypeID,
                    GovernmentTypeName = pObjGovernmentTypeModel.GovernmentTypeName.Trim(),
                    Active = pObjGovernmentTypeModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLGovernmentType().BL_InsertUpdateGovernmentType(mObjGovernmentType);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "GovernmentType");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjGovernmentTypeModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving government type";
                    return View(pObjGovernmentTypeModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Government_Types mObjGovernmentType = new Government_Types()
                {
                    GovernmentTypeID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetGovernmentTypeList_Result mObjGovernmentTypeData = new BLGovernmentType().BL_GetGovernmentTypeDetails(mObjGovernmentType);

                if (mObjGovernmentTypeData != null)
                {
                    GovernmentTypeViewModel mObjGovernmentTypeModelView = new GovernmentTypeViewModel()
                    {
                        GovernmentTypeID = mObjGovernmentTypeData.GovernmentTypeID.GetValueOrDefault(),
                        GovernmentTypeName = mObjGovernmentTypeData.GovernmentTypeName,
                        ActiveText = mObjGovernmentTypeData.ActiveText
                    };

                    return View(mObjGovernmentTypeModelView);
                }
                else
                {
                    return RedirectToAction("List", "GovernmentType");
                }
            }
            else
            {
                return RedirectToAction("List", "GovernmentType");
            }
        }

        public JsonResult UpdateStatus(Government_Types pObjGovernmentTypeData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjGovernmentTypeData.GovernmentTypeID != 0)
            {
                FuncResponse mObjFuncResponse = new BLGovernmentType().BL_UpdateStatus(pObjGovernmentTypeData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["GovernmentTypeList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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