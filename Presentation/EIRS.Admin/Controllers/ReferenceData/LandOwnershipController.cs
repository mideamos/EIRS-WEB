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
    public class LandOwnershipController : BaseController
    {
        public ActionResult List()
        {
            Land_Ownership mObjLandOwnership = new Land_Ownership()
            {
                intStatus = 2
            };

            IList<usp_GetLandOwnershipList_Result> lstLandOwnership = new BLLandOwnership().BL_GetLandOwnershipList(mObjLandOwnership);
            return View(lstLandOwnership);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(LandOwnershipViewModel pObjLandOwnershipModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjLandOwnershipModel);
            }
            else
            {
                Land_Ownership mObjLandOwnership = new Land_Ownership()
                {
                    LandOwnershipID = 0,
                    LandOwnershipName = pObjLandOwnershipModel.LandOwnershipName.Trim(),                    
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLLandOwnership().BL_InsertUpdateLandOwnership(mObjLandOwnership);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "LandOwnership");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjLandOwnershipModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving land ownership";
                    return View(pObjLandOwnershipModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Land_Ownership mObjLandOwnership = new Land_Ownership()
                {
                    LandOwnershipID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetLandOwnershipList_Result mObjLandOwnershipData = new BLLandOwnership().BL_GetLandOwnershipDetails(mObjLandOwnership);

                if (mObjLandOwnershipData != null)
                {
                    LandOwnershipViewModel mObjLandOwnershipModelView = new LandOwnershipViewModel()
                    {
                        LandOwnershipID = mObjLandOwnershipData.LandOwnershipID.GetValueOrDefault(),
                        LandOwnershipName = mObjLandOwnershipData.LandOwnershipName,
                        Active = mObjLandOwnershipData.Active.GetValueOrDefault(),
                    };

                    return View(mObjLandOwnershipModelView);
                }
                else
                {
                    return RedirectToAction("List", "LandOwnership");
                }
            }
            else
            {
                return RedirectToAction("List", "LandOwnership");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(LandOwnershipViewModel pObjLandOwnershipModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjLandOwnershipModel);
            }
            else
            {
                Land_Ownership mObjLandOwnership = new Land_Ownership()
                {
                    LandOwnershipID = pObjLandOwnershipModel.LandOwnershipID,
                    LandOwnershipName = pObjLandOwnershipModel.LandOwnershipName.Trim(),
                    Active = pObjLandOwnershipModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLLandOwnership().BL_InsertUpdateLandOwnership(mObjLandOwnership);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "LandOwnership");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjLandOwnershipModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving land ownership";
                    return View(pObjLandOwnershipModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Land_Ownership mObjLandOwnership = new Land_Ownership()
                {
                    LandOwnershipID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetLandOwnershipList_Result mObjLandOwnershipData = new BLLandOwnership().BL_GetLandOwnershipDetails(mObjLandOwnership);

                if (mObjLandOwnershipData != null)
                {
                    LandOwnershipViewModel mObjLandOwnershipModelView = new LandOwnershipViewModel()
                    {
                        LandOwnershipID = mObjLandOwnershipData.LandOwnershipID.GetValueOrDefault(),
                        LandOwnershipName = mObjLandOwnershipData.LandOwnershipName,
                        ActiveText = mObjLandOwnershipData.ActiveText
                    };

                    return View(mObjLandOwnershipModelView);
                }
                else
                {
                    return RedirectToAction("List", "LandOwnership");
                }
            }
            else
            {
                return RedirectToAction("List", "LandOwnership");
            }
        }

        public JsonResult UpdateStatus(Land_Ownership pObjLandOwnershipData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjLandOwnershipData.LandOwnershipID != 0)
            {
                FuncResponse mObjFuncResponse = new BLLandOwnership().BL_UpdateStatus(pObjLandOwnershipData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["LandOwnershipList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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