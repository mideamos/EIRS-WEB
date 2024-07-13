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
    public class LandPurposeController : BaseController
    {
        public ActionResult List()
        {
            Land_Purpose mObjLandPurpose = new Land_Purpose()
            {
                intStatus = 2
            };

            IList<usp_GetLandPurposeList_Result> lstLandPurpose = new BLLandPurpose().BL_GetLandPurposeList(mObjLandPurpose);
            return View(lstLandPurpose);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(LandPurposeViewModel pObjLandPurposeModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjLandPurposeModel);
            }
            else
            {
                Land_Purpose mObjLandPurpose = new Land_Purpose()
                {
                    LandPurposeID = 0,
                    LandPurposeName = pObjLandPurposeModel.LandPurposeName.Trim(),                    
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLLandPurpose().BL_InsertUpdateLandPurpose(mObjLandPurpose);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "LandPurpose");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjLandPurposeModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving land purpose";
                    return View(pObjLandPurposeModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Land_Purpose mObjLandPurpose = new Land_Purpose()
                {
                    LandPurposeID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetLandPurposeList_Result mObjLandPurposeData = new BLLandPurpose().BL_GetLandPurposeDetails(mObjLandPurpose);

                if (mObjLandPurposeData != null)
                {
                    LandPurposeViewModel mObjLandPurposeModelView = new LandPurposeViewModel()
                    {
                        LandPurposeID = mObjLandPurposeData.LandPurposeID.GetValueOrDefault(),
                        LandPurposeName = mObjLandPurposeData.LandPurposeName,
                        Active = mObjLandPurposeData.Active.GetValueOrDefault(),
                    };

                    return View(mObjLandPurposeModelView);
                }
                else
                {
                    return RedirectToAction("List", "LandPurpose");
                }
            }
            else
            {
                return RedirectToAction("List", "LandPurpose");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(LandPurposeViewModel pObjLandPurposeModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjLandPurposeModel);
            }
            else
            {
                Land_Purpose mObjLandPurpose = new Land_Purpose()
                {
                    LandPurposeID = pObjLandPurposeModel.LandPurposeID,
                    LandPurposeName = pObjLandPurposeModel.LandPurposeName.Trim(),
                    Active = pObjLandPurposeModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLLandPurpose().BL_InsertUpdateLandPurpose(mObjLandPurpose);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "LandPurpose");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjLandPurposeModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving land purpose";
                    return View(pObjLandPurposeModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Land_Purpose mObjLandPurpose = new Land_Purpose()
                {
                    LandPurposeID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetLandPurposeList_Result mObjLandPurposeData = new BLLandPurpose().BL_GetLandPurposeDetails(mObjLandPurpose);

                if (mObjLandPurposeData != null)
                {
                    LandPurposeViewModel mObjLandPurposeModelView = new LandPurposeViewModel()
                    {
                        LandPurposeID = mObjLandPurposeData.LandPurposeID.GetValueOrDefault(),
                        LandPurposeName = mObjLandPurposeData.LandPurposeName,
                        ActiveText = mObjLandPurposeData.ActiveText
                    };

                    return View(mObjLandPurposeModelView);
                }
                else
                {
                    return RedirectToAction("List", "LandPurpose");
                }
            }
            else
            {
                return RedirectToAction("List", "LandPurpose");
            }
        }

        public JsonResult UpdateStatus(Land_Purpose pObjLandPurposeData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjLandPurposeData.LandPurposeID != 0)
            {
                FuncResponse mObjFuncResponse = new BLLandPurpose().BL_UpdateStatus(pObjLandPurposeData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["LandPurposeList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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