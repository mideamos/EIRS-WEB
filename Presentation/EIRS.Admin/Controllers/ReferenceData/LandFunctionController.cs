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
    public class LandFunctionController : BaseController
    {
        public ActionResult List()
        {
            Land_Function mObjLandFunction = new Land_Function()
            {
                intStatus = 2
            };

            IList<usp_GetLandFunctionList_Result> lstLandFunction = new BLLandFunction().BL_GetLandFunctionList(mObjLandFunction);
            return View(lstLandFunction);
        }

        public ActionResult Add()
        {
            UI_FillLandPurposeDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(LandFunctionViewModel pObjLandFunctionModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillLandPurposeDropDown();
                return View(pObjLandFunctionModel);
            }
            else
            {
                Land_Function mObjLandFunction = new Land_Function()
                {
                    LandFunctionID = 0,
                    LandFunctionName = pObjLandFunctionModel.LandFunctionName.Trim(),  
                    LandPurposeID = pObjLandFunctionModel.LandPurposeID,                  
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLLandFunction().BL_InsertUpdateLandFunction(mObjLandFunction);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "LandFunction");
                    }
                    else
                    {
                        UI_FillLandPurposeDropDown();
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjLandFunctionModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillLandPurposeDropDown();
                    ViewBag.Message = "Error occurred while saving Land function";
                    return View(pObjLandFunctionModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Land_Function mObjLandFunction = new Land_Function()
                {
                    LandFunctionID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetLandFunctionList_Result mObjLandFunctionData = new BLLandFunction().BL_GetLandFunctionDetails(mObjLandFunction);

                if (mObjLandFunctionData != null)
                {
                    LandFunctionViewModel mObjLandFunctionModelView = new LandFunctionViewModel()
                    {
                        LandFunctionID = mObjLandFunctionData.LandFunctionID.GetValueOrDefault(),
                        LandFunctionName = mObjLandFunctionData.LandFunctionName,
                        LandPurposeID = mObjLandFunctionData.LandPurposeID.GetValueOrDefault(),
                        Active = mObjLandFunctionData.Active.GetValueOrDefault(),
                    };

                    UI_FillLandPurposeDropDown(new Land_Purpose() { intStatus = 1, IncludeLandPurposeIds = mObjLandFunctionModelView.LandPurposeID.ToString() });
                    return View(mObjLandFunctionModelView);
                }
                else
                {
                    return RedirectToAction("List", "LandFunction");
                }
            }
            else
            {
                return RedirectToAction("List", "LandFunction");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(LandFunctionViewModel pObjLandFunctionModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillLandPurposeDropDown(new Land_Purpose() { intStatus = 1, IncludeLandPurposeIds = pObjLandFunctionModel.LandPurposeID.ToString() });
                return View(pObjLandFunctionModel);
            }
            else
            {
                Land_Function mObjLandFunction = new Land_Function()
                {
                    LandFunctionID = pObjLandFunctionModel.LandFunctionID,
                    LandFunctionName = pObjLandFunctionModel.LandFunctionName.Trim(),
                    LandPurposeID = pObjLandFunctionModel.LandPurposeID,
                    Active = pObjLandFunctionModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLLandFunction().BL_InsertUpdateLandFunction(mObjLandFunction);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "LandFunction");
                    }
                    else
                    {
                        UI_FillLandPurposeDropDown(new Land_Purpose() { intStatus = 1, IncludeLandPurposeIds = pObjLandFunctionModel.LandPurposeID.ToString() });
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjLandFunctionModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillLandPurposeDropDown(new Land_Purpose() { intStatus = 1, IncludeLandPurposeIds = pObjLandFunctionModel.LandPurposeID.ToString() });
                    ViewBag.Message = "Error occurred while saving Land function";
                    return View(pObjLandFunctionModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Land_Function mObjLandFunction = new Land_Function()
                {
                    LandFunctionID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetLandFunctionList_Result mObjLandFunctionData = new BLLandFunction().BL_GetLandFunctionDetails(mObjLandFunction);

                if (mObjLandFunctionData != null)
                {
                    LandFunctionViewModel mObjLandFunctionModelView = new LandFunctionViewModel()
                    {
                        LandFunctionID = mObjLandFunctionData.LandFunctionID.GetValueOrDefault(),
                        LandFunctionName = mObjLandFunctionData.LandFunctionName,
                        LandPurposeName = mObjLandFunctionData.LandPurposeName,
                        ActiveText = mObjLandFunctionData.ActiveText
                    };

                    return View(mObjLandFunctionModelView);
                }
                else
                {
                    return RedirectToAction("List", "LandFunction");
                }
            }
            else
            {
                return RedirectToAction("List", "LandFunction");
            }
        }

        public JsonResult UpdateStatus(Land_Function pObjLandFunctionData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjLandFunctionData.LandFunctionID != 0)
            {
                FuncResponse mObjFuncResponse = new BLLandFunction().BL_UpdateStatus(pObjLandFunctionData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["LandFunctionList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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