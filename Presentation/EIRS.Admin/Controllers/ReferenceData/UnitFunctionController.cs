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
    public class UnitFunctionController : BaseController
    {
        public ActionResult List()
        {
            Unit_Function mObjUnitFunction = new Unit_Function()
            {
                intStatus = 2
            };

            IList<usp_GetUnitFunctionList_Result> lstUnitFunction = new BLUnitFunction().BL_GetUnitFunctionList(mObjUnitFunction);
            return View(lstUnitFunction);
        }

        public ActionResult Add()
        {
            UI_FillUnitPurposeDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(UnitFunctionViewModel pObjUnitFunctionModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillUnitPurposeDropDown();
                return View(pObjUnitFunctionModel);
            }
            else
            {
                Unit_Function mObjUnitFunction = new Unit_Function()
                {
                    UnitFunctionID = 0,
                    UnitFunctionName = pObjUnitFunctionModel.UnitFunctionName.Trim(),  
                    UnitPurposeID = pObjUnitFunctionModel.UnitPurposeID,                  
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLUnitFunction().BL_InsertUpdateUnitFunction(mObjUnitFunction);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "UnitFunction");
                    }
                    else
                    {
                        UI_FillUnitPurposeDropDown();
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjUnitFunctionModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillUnitPurposeDropDown();
                    ViewBag.Message = "Error occurred while saving unit function";
                    return View(pObjUnitFunctionModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Unit_Function mObjUnitFunction = new Unit_Function()
                {
                    UnitFunctionID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetUnitFunctionList_Result mObjUnitFunctionData = new BLUnitFunction().BL_GetUnitFunctionDetails(mObjUnitFunction);

                if (mObjUnitFunctionData != null)
                {
                    UnitFunctionViewModel mObjUnitFunctionModelView = new UnitFunctionViewModel()
                    {
                        UnitFunctionID = mObjUnitFunctionData.UnitFunctionID.GetValueOrDefault(),
                        UnitFunctionName = mObjUnitFunctionData.UnitFunctionName,
                        UnitPurposeID = mObjUnitFunctionData.UnitPurposeID.GetValueOrDefault(),
                        Active = mObjUnitFunctionData.Active.GetValueOrDefault(),
                    };

                    UI_FillUnitPurposeDropDown(new Unit_Purpose() { intStatus = 1, IncludeUnitPurposeIds = mObjUnitFunctionModelView.UnitPurposeID.ToString() });
                    return View(mObjUnitFunctionModelView);
                }
                else
                {
                    return RedirectToAction("List", "UnitFunction");
                }
            }
            else
            {
                return RedirectToAction("List", "UnitFunction");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(UnitFunctionViewModel pObjUnitFunctionModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillUnitPurposeDropDown(new Unit_Purpose() { intStatus = 1, IncludeUnitPurposeIds = pObjUnitFunctionModel.UnitPurposeID.ToString() });
                return View(pObjUnitFunctionModel);
            }
            else
            {
                Unit_Function mObjUnitFunction = new Unit_Function()
                {
                    UnitFunctionID = pObjUnitFunctionModel.UnitFunctionID,
                    UnitFunctionName = pObjUnitFunctionModel.UnitFunctionName.Trim(),
                    UnitPurposeID = pObjUnitFunctionModel.UnitPurposeID,
                    Active = pObjUnitFunctionModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLUnitFunction().BL_InsertUpdateUnitFunction(mObjUnitFunction);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "UnitFunction");
                    }
                    else
                    {
                        UI_FillUnitPurposeDropDown(new Unit_Purpose() { intStatus = 1, IncludeUnitPurposeIds = pObjUnitFunctionModel.UnitPurposeID.ToString() });
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjUnitFunctionModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillUnitPurposeDropDown(new Unit_Purpose() { intStatus = 1, IncludeUnitPurposeIds = pObjUnitFunctionModel.UnitPurposeID.ToString() });
                    ViewBag.Message = "Error occurred while saving unit function";
                    return View(pObjUnitFunctionModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Unit_Function mObjUnitFunction = new Unit_Function()
                {
                    UnitFunctionID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetUnitFunctionList_Result mObjUnitFunctionData = new BLUnitFunction().BL_GetUnitFunctionDetails(mObjUnitFunction);

                if (mObjUnitFunctionData != null)
                {
                    UnitFunctionViewModel mObjUnitFunctionModelView = new UnitFunctionViewModel()
                    {
                        UnitFunctionID = mObjUnitFunctionData.UnitFunctionID.GetValueOrDefault(),
                        UnitFunctionName = mObjUnitFunctionData.UnitFunctionName,
                        UnitPurposeName = mObjUnitFunctionData.UnitPurposeName,
                        ActiveText = mObjUnitFunctionData.ActiveText
                    };

                    return View(mObjUnitFunctionModelView);
                }
                else
                {
                    return RedirectToAction("List", "UnitFunction");
                }
            }
            else
            {
                return RedirectToAction("List", "UnitFunction");
            }
        }

        public JsonResult UpdateStatus(Unit_Function pObjUnitFunctionData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjUnitFunctionData.UnitFunctionID != 0)
            {
                FuncResponse mObjFuncResponse = new BLUnitFunction().BL_UpdateStatus(pObjUnitFunctionData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["UnitFunctionList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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