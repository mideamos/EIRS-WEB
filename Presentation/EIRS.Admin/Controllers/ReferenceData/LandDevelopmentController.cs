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
    public class LandDevelopmentController : BaseController
    {
        public ActionResult List()
        {
            Land_Development mObjLandDevelopment = new Land_Development()
            {
                intStatus = 2
            };

            IList<usp_GetLandDevelopmentList_Result> lstLandDevelopment = new BLLandDevelopment().BL_GetLandDevelopmentList(mObjLandDevelopment);
            return View(lstLandDevelopment);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(LandDevelopmentViewModel pObjLandDevelopmentModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjLandDevelopmentModel);
            }
            else
            {
                Land_Development mObjLandDevelopment = new Land_Development()
                {
                    LandDevelopmentID = 0,
                    LandDevelopmentName = pObjLandDevelopmentModel.LandDevelopmentName.Trim(),                    
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLLandDevelopment().BL_InsertUpdateLandDevelopment(mObjLandDevelopment);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "LandDevelopment");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjLandDevelopmentModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving land development";
                    return View(pObjLandDevelopmentModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Land_Development mObjLandDevelopment = new Land_Development()
                {
                    LandDevelopmentID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetLandDevelopmentList_Result mObjLandDevelopmentData = new BLLandDevelopment().BL_GetLandDevelopmentDetails(mObjLandDevelopment);

                if (mObjLandDevelopmentData != null)
                {
                    LandDevelopmentViewModel mObjLandDevelopmentModelView = new LandDevelopmentViewModel()
                    {
                        LandDevelopmentID = mObjLandDevelopmentData.LandDevelopmentID.GetValueOrDefault(),
                        LandDevelopmentName = mObjLandDevelopmentData.LandDevelopmentName,
                        Active = mObjLandDevelopmentData.Active.GetValueOrDefault(),
                    };

                    return View(mObjLandDevelopmentModelView);
                }
                else
                {
                    return RedirectToAction("List", "LandDevelopment");
                }
            }
            else
            {
                return RedirectToAction("List", "LandDevelopment");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(LandDevelopmentViewModel pObjLandDevelopmentModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjLandDevelopmentModel);
            }
            else
            {
                Land_Development mObjLandDevelopment = new Land_Development()
                {
                    LandDevelopmentID = pObjLandDevelopmentModel.LandDevelopmentID,
                    LandDevelopmentName = pObjLandDevelopmentModel.LandDevelopmentName.Trim(),
                    Active = pObjLandDevelopmentModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLLandDevelopment().BL_InsertUpdateLandDevelopment(mObjLandDevelopment);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "LandDevelopment");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjLandDevelopmentModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving land development";
                    return View(pObjLandDevelopmentModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Land_Development mObjLandDevelopment = new Land_Development()
                {
                    LandDevelopmentID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetLandDevelopmentList_Result mObjLandDevelopmentData = new BLLandDevelopment().BL_GetLandDevelopmentDetails(mObjLandDevelopment);

                if (mObjLandDevelopmentData != null)
                {
                    LandDevelopmentViewModel mObjLandDevelopmentModelView = new LandDevelopmentViewModel()
                    {
                        LandDevelopmentID = mObjLandDevelopmentData.LandDevelopmentID.GetValueOrDefault(),
                        LandDevelopmentName = mObjLandDevelopmentData.LandDevelopmentName,
                        ActiveText = mObjLandDevelopmentData.ActiveText
                    };

                    return View(mObjLandDevelopmentModelView);
                }
                else
                {
                    return RedirectToAction("List", "LandDevelopment");
                }
            }
            else
            {
                return RedirectToAction("List", "LandDevelopment");
            }
        }

        public JsonResult UpdateStatus(Land_Development pObjLandDevelopmentData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjLandDevelopmentData.LandDevelopmentID != 0)
            {
                FuncResponse mObjFuncResponse = new BLLandDevelopment().BL_UpdateStatus(pObjLandDevelopmentData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["LandDevelopmentList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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