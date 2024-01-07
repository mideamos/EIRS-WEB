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
    public class BuildingOwnershipController : BaseController
    {
        public ActionResult List()
        {
            Building_Ownership mObjBuildingOwnership = new Building_Ownership()
            {
                intStatus = 2
            };

            IList<usp_GetBuildingOwnershipList_Result> lstBuildingOwnership = new BLBuildingOwnership().BL_GetBuildingOwnershipList(mObjBuildingOwnership);
            return View(lstBuildingOwnership);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(BuildingOwnershipViewModel pObjBuildingOwnershipModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjBuildingOwnershipModel);
            }
            else
            {
                Building_Ownership mObjBuildingOwnership = new Building_Ownership()
                {
                    BuildingOwnershipID = 0,
                    BuildingOwnershipName = pObjBuildingOwnershipModel.BuildingOwnershipName.Trim(),                    
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLBuildingOwnership().BL_InsertUpdateBuildingOwnership(mObjBuildingOwnership);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "BuildingOwnership");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjBuildingOwnershipModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving building ownership";
                    return View(pObjBuildingOwnershipModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Building_Ownership mObjBuildingOwnership = new Building_Ownership()
                {
                    BuildingOwnershipID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetBuildingOwnershipList_Result mObjBuildingOwnershipData = new BLBuildingOwnership().BL_GetBuildingOwnershipDetails(mObjBuildingOwnership);

                if (mObjBuildingOwnershipData != null)
                {
                    BuildingOwnershipViewModel mObjBuildingOwnershipModelView = new BuildingOwnershipViewModel()
                    {
                        BuildingOwnershipID = mObjBuildingOwnershipData.BuildingOwnershipID.GetValueOrDefault(),
                        BuildingOwnershipName = mObjBuildingOwnershipData.BuildingOwnershipName,
                        Active = mObjBuildingOwnershipData.Active.GetValueOrDefault(),
                    };

                    return View(mObjBuildingOwnershipModelView);
                }
                else
                {
                    return RedirectToAction("List", "BuildingOwnership");
                }
            }
            else
            {
                return RedirectToAction("List", "BuildingOwnership");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(BuildingOwnershipViewModel pObjBuildingOwnershipModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjBuildingOwnershipModel);
            }
            else
            {
                Building_Ownership mObjBuildingOwnership = new Building_Ownership()
                {
                    BuildingOwnershipID = pObjBuildingOwnershipModel.BuildingOwnershipID,
                    BuildingOwnershipName = pObjBuildingOwnershipModel.BuildingOwnershipName.Trim(),
                    Active = pObjBuildingOwnershipModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLBuildingOwnership().BL_InsertUpdateBuildingOwnership(mObjBuildingOwnership);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "BuildingOwnership");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjBuildingOwnershipModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving building ownership";
                    return View(pObjBuildingOwnershipModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Building_Ownership mObjBuildingOwnership = new Building_Ownership()
                {
                    BuildingOwnershipID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetBuildingOwnershipList_Result mObjBuildingOwnershipData = new BLBuildingOwnership().BL_GetBuildingOwnershipDetails(mObjBuildingOwnership);

                if (mObjBuildingOwnershipData != null)
                {
                    BuildingOwnershipViewModel mObjBuildingOwnershipModelView = new BuildingOwnershipViewModel()
                    {
                        BuildingOwnershipID = mObjBuildingOwnershipData.BuildingOwnershipID.GetValueOrDefault(),
                        BuildingOwnershipName = mObjBuildingOwnershipData.BuildingOwnershipName,
                        ActiveText = mObjBuildingOwnershipData.ActiveText
                    };

                    return View(mObjBuildingOwnershipModelView);
                }
                else
                {
                    return RedirectToAction("List", "BuildingOwnership");
                }
            }
            else
            {
                return RedirectToAction("List", "BuildingOwnership");
            }
        }

        public JsonResult UpdateStatus(Building_Ownership pObjBuildingOwnershipData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjBuildingOwnershipData.BuildingOwnershipID != 0)
            {
                FuncResponse mObjFuncResponse = new BLBuildingOwnership().BL_UpdateStatus(pObjBuildingOwnershipData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["BuildingOwnershipList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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