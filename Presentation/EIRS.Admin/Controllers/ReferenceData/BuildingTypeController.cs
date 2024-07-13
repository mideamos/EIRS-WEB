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
    public class BuildingTypeController : BaseController
    {
        public ActionResult List()
        {
            Building_Types mObjBuildingType = new Building_Types()
            {
                intStatus = 2
            };

            IList<usp_GetBuildingTypeList_Result> lstBuildingType = new BLBuildingType().BL_GetBuildingTypeList(mObjBuildingType);
            return View(lstBuildingType);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(BuildingTypeViewModel pObjBuildingTypeModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjBuildingTypeModel);
            }
            else
            {
                Building_Types mObjBuildingType = new Building_Types()
                {
                    BuildingTypeID = 0,
                    BuildingTypeName = pObjBuildingTypeModel.BuildingTypeName.Trim(),                    
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLBuildingType().BL_InsertUpdateBuildingType(mObjBuildingType);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "BuildingType");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjBuildingTypeModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving building type";
                    return View(pObjBuildingTypeModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Building_Types mObjBuildingType = new Building_Types()
                {
                    BuildingTypeID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetBuildingTypeList_Result mObjBuildingTypeData = new BLBuildingType().BL_GetBuildingTypeDetails(mObjBuildingType);

                if (mObjBuildingTypeData != null)
                {
                    BuildingTypeViewModel mObjBuildingTypeModelView = new BuildingTypeViewModel()
                    {
                        BuildingTypeID = mObjBuildingTypeData.BuildingTypeID.GetValueOrDefault(),
                        BuildingTypeName = mObjBuildingTypeData.BuildingTypeName,
                        Active = mObjBuildingTypeData.Active.GetValueOrDefault(),
                    };

                    return View(mObjBuildingTypeModelView);
                }
                else
                {
                    return RedirectToAction("List", "BuildingType");
                }
            }
            else
            {
                return RedirectToAction("List", "BuildingType");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(BuildingTypeViewModel pObjBuildingTypeModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjBuildingTypeModel);
            }
            else
            {
                Building_Types mObjBuildingType = new Building_Types()
                {
                    BuildingTypeID = pObjBuildingTypeModel.BuildingTypeID,
                    BuildingTypeName = pObjBuildingTypeModel.BuildingTypeName.Trim(),
                    Active = pObjBuildingTypeModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLBuildingType().BL_InsertUpdateBuildingType(mObjBuildingType);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "BuildingType");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjBuildingTypeModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving building type";
                    return View(pObjBuildingTypeModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Building_Types mObjBuildingType = new Building_Types()
                {
                    BuildingTypeID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetBuildingTypeList_Result mObjBuildingTypeData = new BLBuildingType().BL_GetBuildingTypeDetails(mObjBuildingType);

                if (mObjBuildingTypeData != null)
                {
                    BuildingTypeViewModel mObjBuildingTypeModelView = new BuildingTypeViewModel()
                    {
                        BuildingTypeID = mObjBuildingTypeData.BuildingTypeID.GetValueOrDefault(),
                        BuildingTypeName = mObjBuildingTypeData.BuildingTypeName,
                        ActiveText = mObjBuildingTypeData.ActiveText
                    };

                    return View(mObjBuildingTypeModelView);
                }
                else
                {
                    return RedirectToAction("List", "BuildingType");
                }
            }
            else
            {
                return RedirectToAction("List", "BuildingType");
            }
        }

        public JsonResult UpdateStatus(Building_Types pObjBuildingTypeData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjBuildingTypeData.BuildingTypeID != 0)
            {
                FuncResponse mObjFuncResponse = new BLBuildingType().BL_UpdateStatus(pObjBuildingTypeData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["BuildingTypeList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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