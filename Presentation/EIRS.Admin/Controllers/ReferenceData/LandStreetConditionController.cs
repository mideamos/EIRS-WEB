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
    public class LandStreetConditionController : BaseController
    {
        public ActionResult List()
        {
            Land_StreetCondition mObjLandStreetCondition = new Land_StreetCondition()
            {
                intStatus = 2
            };

            IList<usp_GetLandStreetConditionList_Result> lstLandStreetCondition = new BLLandStreetCondition().BL_GetLandStreetConditionList(mObjLandStreetCondition);
            return View(lstLandStreetCondition);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(LandStreetConditionViewModel pObjLandStreetConditionModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjLandStreetConditionModel);
            }
            else
            {
                Land_StreetCondition mObjLandStreetCondition = new Land_StreetCondition()
                {
                    LandStreetConditionID = 0,
                    LandStreetConditionName = pObjLandStreetConditionModel.LandStreetConditionName.Trim(),  
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLLandStreetCondition().BL_InsertUpdateLandStreetCondition(mObjLandStreetCondition);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "LandStreetCondition");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjLandStreetConditionModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving land street condition";
                    return View(pObjLandStreetConditionModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Land_StreetCondition mObjLandStreetCondition = new Land_StreetCondition()
                {
                    LandStreetConditionID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetLandStreetConditionList_Result mObjLandStreetConditionData = new BLLandStreetCondition().BL_GetLandStreetConditionDetails(mObjLandStreetCondition);

                if (mObjLandStreetConditionData != null)
                {
                    LandStreetConditionViewModel mObjLandStreetConditionModelView = new LandStreetConditionViewModel()
                    {
                        LandStreetConditionID = mObjLandStreetConditionData.LandStreetConditionID.GetValueOrDefault(),
                        LandStreetConditionName = mObjLandStreetConditionData.LandStreetConditionName,
                        Active = mObjLandStreetConditionData.Active.GetValueOrDefault(),
                    };

                    return View(mObjLandStreetConditionModelView);
                }
                else
                {
                    return RedirectToAction("List", "LandStreetCondition");
                }
            }
            else
            {
                return RedirectToAction("List", "LandStreetCondition");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(LandStreetConditionViewModel pObjLandStreetConditionModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjLandStreetConditionModel);
            }
            else
            {
                Land_StreetCondition mObjLandStreetCondition = new Land_StreetCondition()
                {
                    LandStreetConditionID = pObjLandStreetConditionModel.LandStreetConditionID,
                    LandStreetConditionName = pObjLandStreetConditionModel.LandStreetConditionName.Trim(),
                    Active = pObjLandStreetConditionModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLLandStreetCondition().BL_InsertUpdateLandStreetCondition(mObjLandStreetCondition);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "LandStreetCondition");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjLandStreetConditionModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving land street condition";
                    return View(pObjLandStreetConditionModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Land_StreetCondition mObjLandStreetCondition = new Land_StreetCondition()
                {
                    LandStreetConditionID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetLandStreetConditionList_Result mObjLandStreetConditionData = new BLLandStreetCondition().BL_GetLandStreetConditionDetails(mObjLandStreetCondition);

                if (mObjLandStreetConditionData != null)
                {
                    LandStreetConditionViewModel mObjLandStreetConditionModelView = new LandStreetConditionViewModel()
                    {
                        LandStreetConditionID = mObjLandStreetConditionData.LandStreetConditionID.GetValueOrDefault(),
                        LandStreetConditionName = mObjLandStreetConditionData.LandStreetConditionName,
                        ActiveText = mObjLandStreetConditionData.ActiveText
                    };

                    return View(mObjLandStreetConditionModelView);
                }
                else
                {
                    return RedirectToAction("List", "LandStreetCondition");
                }
            }
            else
            {
                return RedirectToAction("List", "LandStreetCondition");
            }
        }

        public JsonResult UpdateStatus(Land_StreetCondition pObjLandStreetConditionData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjLandStreetConditionData.LandStreetConditionID != 0)
            {
                FuncResponse mObjFuncResponse = new BLLandStreetCondition().BL_UpdateStatus(pObjLandStreetConditionData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["LandStreetConditionList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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