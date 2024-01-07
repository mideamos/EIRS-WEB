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
    public class BuildingCompletionController : BaseController
    {
        public ActionResult List()
        {
            Building_Completion mObjBuildingCompletion = new Building_Completion()
            {
                intStatus = 2
            };

            IList<usp_GetBuildingCompletionList_Result> lstBuildingCompletion = new BLBuildingCompletion().BL_GetBuildingCompletionList(mObjBuildingCompletion);
            return View(lstBuildingCompletion);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(BuildingCompletionViewModel pObjBuildingCompletionModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjBuildingCompletionModel);
            }
            else
            {
                Building_Completion mObjBuildingCompletion = new Building_Completion()
                {
                    BuildingCompletionID = 0,
                    BuildingCompletionName = pObjBuildingCompletionModel.BuildingCompletionName.Trim(),                    
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLBuildingCompletion().BL_InsertUpdateBuildingCompletion(mObjBuildingCompletion);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "BuildingCompletion");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjBuildingCompletionModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving building completion";
                    return View(pObjBuildingCompletionModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Building_Completion mObjBuildingCompletion = new Building_Completion()
                {
                    BuildingCompletionID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetBuildingCompletionList_Result mObjBuildingCompletionData = new BLBuildingCompletion().BL_GetBuildingCompletionDetails(mObjBuildingCompletion);

                if (mObjBuildingCompletionData != null)
                {
                    BuildingCompletionViewModel mObjBuildingCompletionModelView = new BuildingCompletionViewModel()
                    {
                        BuildingCompletionID = mObjBuildingCompletionData.BuildingCompletionID.GetValueOrDefault(),
                        BuildingCompletionName = mObjBuildingCompletionData.BuildingCompletionName,
                        Active = mObjBuildingCompletionData.Active.GetValueOrDefault(),
                    };

                    return View(mObjBuildingCompletionModelView);
                }
                else
                {
                    return RedirectToAction("List", "BuildingCompletion");
                }
            }
            else
            {
                return RedirectToAction("List", "BuildingCompletion");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(BuildingCompletionViewModel pObjBuildingCompletionModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjBuildingCompletionModel);
            }
            else
            {
                Building_Completion mObjBuildingCompletion = new Building_Completion()
                {
                    BuildingCompletionID = pObjBuildingCompletionModel.BuildingCompletionID,
                    BuildingCompletionName = pObjBuildingCompletionModel.BuildingCompletionName.Trim(),
                    Active = pObjBuildingCompletionModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLBuildingCompletion().BL_InsertUpdateBuildingCompletion(mObjBuildingCompletion);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "BuildingCompletion");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjBuildingCompletionModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving building completion";
                    return View(pObjBuildingCompletionModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Building_Completion mObjBuildingCompletion = new Building_Completion()
                {
                    BuildingCompletionID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetBuildingCompletionList_Result mObjBuildingCompletionData = new BLBuildingCompletion().BL_GetBuildingCompletionDetails(mObjBuildingCompletion);

                if (mObjBuildingCompletionData != null)
                {
                    BuildingCompletionViewModel mObjBuildingCompletionModelView = new BuildingCompletionViewModel()
                    {
                        BuildingCompletionID = mObjBuildingCompletionData.BuildingCompletionID.GetValueOrDefault(),
                        BuildingCompletionName = mObjBuildingCompletionData.BuildingCompletionName,
                        ActiveText = mObjBuildingCompletionData.ActiveText
                    };

                    return View(mObjBuildingCompletionModelView);
                }
                else
                {
                    return RedirectToAction("List", "BuildingCompletion");
                }
            }
            else
            {
                return RedirectToAction("List", "BuildingCompletion");
            }
        }

        public JsonResult UpdateStatus(Building_Completion pObjBuildingCompletionData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjBuildingCompletionData.BuildingCompletionID != 0)
            {
                FuncResponse mObjFuncResponse = new BLBuildingCompletion().BL_UpdateStatus(pObjBuildingCompletionData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["BuildingCompletionList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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