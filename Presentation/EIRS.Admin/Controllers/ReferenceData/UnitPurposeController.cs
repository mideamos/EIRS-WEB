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
    public class UnitPurposeController : BaseController
    {
        public ActionResult List()
        {
            Unit_Purpose mObjUnitPurpose = new Unit_Purpose()
            {
                intStatus = 2
            };

            IList<usp_GetUnitPurposeList_Result> lstUnitPurpose = new BLUnitPurpose().BL_GetUnitPurposeList(mObjUnitPurpose);
            return View(lstUnitPurpose);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(UnitPurposeViewModel pObjUnitPurposeModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjUnitPurposeModel);
            }
            else
            {
                Unit_Purpose mObjUnitPurpose = new Unit_Purpose()
                {
                    UnitPurposeID = 0,
                    UnitPurposeName = pObjUnitPurposeModel.UnitPurposeName.Trim(),                    
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLUnitPurpose().BL_InsertUpdateUnitPurpose(mObjUnitPurpose);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "UnitPurpose");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjUnitPurposeModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving unit purpose";
                    return View(pObjUnitPurposeModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Unit_Purpose mObjUnitPurpose = new Unit_Purpose()
                {
                    UnitPurposeID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetUnitPurposeList_Result mObjUnitPurposeData = new BLUnitPurpose().BL_GetUnitPurposeDetails(mObjUnitPurpose);

                if (mObjUnitPurposeData != null)
                {
                    UnitPurposeViewModel mObjUnitPurposeModelView = new UnitPurposeViewModel()
                    {
                        UnitPurposeID = mObjUnitPurposeData.UnitPurposeID.GetValueOrDefault(),
                        UnitPurposeName = mObjUnitPurposeData.UnitPurposeName,
                        Active = mObjUnitPurposeData.Active.GetValueOrDefault(),
                    };

                    return View(mObjUnitPurposeModelView);
                }
                else
                {
                    return RedirectToAction("List", "UnitPurpose");
                }
            }
            else
            {
                return RedirectToAction("List", "UnitPurpose");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(UnitPurposeViewModel pObjUnitPurposeModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjUnitPurposeModel);
            }
            else
            {
                Unit_Purpose mObjUnitPurpose = new Unit_Purpose()
                {
                    UnitPurposeID = pObjUnitPurposeModel.UnitPurposeID,
                    UnitPurposeName = pObjUnitPurposeModel.UnitPurposeName.Trim(),
                    Active = pObjUnitPurposeModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLUnitPurpose().BL_InsertUpdateUnitPurpose(mObjUnitPurpose);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "UnitPurpose");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjUnitPurposeModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving unit purpose";
                    return View(pObjUnitPurposeModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Unit_Purpose mObjUnitPurpose = new Unit_Purpose()
                {
                    UnitPurposeID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetUnitPurposeList_Result mObjUnitPurposeData = new BLUnitPurpose().BL_GetUnitPurposeDetails(mObjUnitPurpose);

                if (mObjUnitPurposeData != null)
                {
                    UnitPurposeViewModel mObjUnitPurposeModelView = new UnitPurposeViewModel()
                    {
                        UnitPurposeID = mObjUnitPurposeData.UnitPurposeID.GetValueOrDefault(),
                        UnitPurposeName = mObjUnitPurposeData.UnitPurposeName,
                        ActiveText = mObjUnitPurposeData.ActiveText
                    };

                    return View(mObjUnitPurposeModelView);
                }
                else
                {
                    return RedirectToAction("List", "UnitPurpose");
                }
            }
            else
            {
                return RedirectToAction("List", "UnitPurpose");
            }
        }

        public JsonResult UpdateStatus(Unit_Purpose pObjUnitPurposeData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjUnitPurposeData.UnitPurposeID != 0)
            {
                FuncResponse mObjFuncResponse = new BLUnitPurpose().BL_UpdateStatus(pObjUnitPurposeData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["UnitPurposeList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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