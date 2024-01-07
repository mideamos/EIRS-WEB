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
    public class AgencyTypeController : BaseController
    {
        public ActionResult List()
        {
            Agency_Types mObjAgencyType = new Agency_Types()
            {
                intStatus = 2
            };

            IList<usp_GetAgencyTypeList_Result> lstAgencyType = new BLAgencyType().BL_GetAgencyTypeList(mObjAgencyType);
            return View(lstAgencyType);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(AgencyTypeViewModel pObjAgencyTypeModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjAgencyTypeModel);
            }
            else
            {
                Agency_Types mObjAgencyType = new Agency_Types()
                {
                    AgencyTypeID = 0,
                    AgencyTypeName = pObjAgencyTypeModel.AgencyTypeName.Trim(),                    
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLAgencyType().BL_InsertUpdateAgencyType(mObjAgencyType);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "AgencyType");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjAgencyTypeModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving agency type";
                    return View(pObjAgencyTypeModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Agency_Types mObjAgencyType = new Agency_Types()
                {
                    AgencyTypeID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetAgencyTypeList_Result mObjAgencyTypeData = new BLAgencyType().BL_GetAgencyTypeDetails(mObjAgencyType);

                if (mObjAgencyTypeData != null)
                {
                    AgencyTypeViewModel mObjAgencyTypeModelView = new AgencyTypeViewModel()
                    {
                        AgencyTypeID = mObjAgencyTypeData.AgencyTypeID.GetValueOrDefault(),
                        AgencyTypeName = mObjAgencyTypeData.AgencyTypeName,
                        Active = mObjAgencyTypeData.Active.GetValueOrDefault(),
                    };

                    return View(mObjAgencyTypeModelView);
                }
                else
                {
                    return RedirectToAction("List", "AgencyType");
                }
            }
            else
            {
                return RedirectToAction("List", "AgencyType");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(AgencyTypeViewModel pObjAgencyTypeModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjAgencyTypeModel);
            }
            else
            {
                Agency_Types mObjAgencyType = new Agency_Types()
                {
                    AgencyTypeID = pObjAgencyTypeModel.AgencyTypeID,
                    AgencyTypeName = pObjAgencyTypeModel.AgencyTypeName.Trim(),
                    Active = pObjAgencyTypeModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLAgencyType().BL_InsertUpdateAgencyType(mObjAgencyType);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "AgencyType");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjAgencyTypeModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving agency type";
                    return View(pObjAgencyTypeModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Agency_Types mObjAgencyType = new Agency_Types()
                {
                    AgencyTypeID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetAgencyTypeList_Result mObjAgencyTypeData = new BLAgencyType().BL_GetAgencyTypeDetails(mObjAgencyType);

                if (mObjAgencyTypeData != null)
                {
                    AgencyTypeViewModel mObjAgencyTypeModelView = new AgencyTypeViewModel()
                    {
                        AgencyTypeID = mObjAgencyTypeData.AgencyTypeID.GetValueOrDefault(),
                        AgencyTypeName = mObjAgencyTypeData.AgencyTypeName,
                        ActiveText = mObjAgencyTypeData.ActiveText
                    };

                    return View(mObjAgencyTypeModelView);
                }
                else
                {
                    return RedirectToAction("List", "AgencyType");
                }
            }
            else
            {
                return RedirectToAction("List", "AgencyType");
            }
        }

        public JsonResult UpdateStatus(Agency_Types pObjAgencyTypeData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjAgencyTypeData.AgencyTypeID != 0)
            {
                FuncResponse mObjFuncResponse = new BLAgencyType().BL_UpdateStatus(pObjAgencyTypeData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["AgencyTypeList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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