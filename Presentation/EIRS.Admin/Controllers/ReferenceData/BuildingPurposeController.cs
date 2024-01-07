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
    public class BuildingPurposeController : BaseController
    {
        public ActionResult List()
        {
            Building_Purpose mObjBuildingPurpose = new Building_Purpose()
            {
                intStatus = 2
            };

            IList<usp_GetBuildingPurposeList_Result> lstBuildingPurpose = new BLBuildingPurpose().BL_GetBuildingPurposeList(mObjBuildingPurpose);
            return View(lstBuildingPurpose);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(BuildingPurposeViewModel pObjBuildingPurposeModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjBuildingPurposeModel);
            }
            else
            {
                Building_Purpose mObjBuildingPurpose = new Building_Purpose()
                {
                    BuildingPurposeID = 0,
                    BuildingPurposeName = pObjBuildingPurposeModel.BuildingPurposeName.Trim(),                    
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLBuildingPurpose().BL_InsertUpdateBuildingPurpose(mObjBuildingPurpose);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "BuildingPurpose");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjBuildingPurposeModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving building purpose";
                    return View(pObjBuildingPurposeModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Building_Purpose mObjBuildingPurpose = new Building_Purpose()
                {
                    BuildingPurposeID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetBuildingPurposeList_Result mObjBuildingPurposeData = new BLBuildingPurpose().BL_GetBuildingPurposeDetails(mObjBuildingPurpose);

                if (mObjBuildingPurposeData != null)
                {
                    BuildingPurposeViewModel mObjBuildingPurposeModelView = new BuildingPurposeViewModel()
                    {
                        BuildingPurposeID = mObjBuildingPurposeData.BuildingPurposeID.GetValueOrDefault(),
                        BuildingPurposeName = mObjBuildingPurposeData.BuildingPurposeName,
                        Active = mObjBuildingPurposeData.Active.GetValueOrDefault(),
                    };

                    return View(mObjBuildingPurposeModelView);
                }
                else
                {
                    return RedirectToAction("List", "BuildingPurpose");
                }
            }
            else
            {
                return RedirectToAction("List", "BuildingPurpose");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(BuildingPurposeViewModel pObjBuildingPurposeModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjBuildingPurposeModel);
            }
            else
            {
                Building_Purpose mObjBuildingPurpose = new Building_Purpose()
                {
                    BuildingPurposeID = pObjBuildingPurposeModel.BuildingPurposeID,
                    BuildingPurposeName = pObjBuildingPurposeModel.BuildingPurposeName.Trim(),
                    Active = pObjBuildingPurposeModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLBuildingPurpose().BL_InsertUpdateBuildingPurpose(mObjBuildingPurpose);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "BuildingPurpose");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjBuildingPurposeModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving building purpose";
                    return View(pObjBuildingPurposeModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Building_Purpose mObjBuildingPurpose = new Building_Purpose()
                {
                    BuildingPurposeID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetBuildingPurposeList_Result mObjBuildingPurposeData = new BLBuildingPurpose().BL_GetBuildingPurposeDetails(mObjBuildingPurpose);

                if (mObjBuildingPurposeData != null)
                {
                    BuildingPurposeViewModel mObjBuildingPurposeModelView = new BuildingPurposeViewModel()
                    {
                        BuildingPurposeID = mObjBuildingPurposeData.BuildingPurposeID.GetValueOrDefault(),
                        BuildingPurposeName = mObjBuildingPurposeData.BuildingPurposeName,
                        ActiveText = mObjBuildingPurposeData.ActiveText
                    };

                    return View(mObjBuildingPurposeModelView);
                }
                else
                {
                    return RedirectToAction("List", "BuildingPurpose");
                }
            }
            else
            {
                return RedirectToAction("List", "BuildingPurpose");
            }
        }

        public JsonResult UpdateStatus(Building_Purpose pObjBuildingPurposeData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjBuildingPurposeData.BuildingPurposeID != 0)
            {
                FuncResponse mObjFuncResponse = new BLBuildingPurpose().BL_UpdateStatus(pObjBuildingPurposeData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["BuildingPurposeList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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