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
    public class WardController : BaseController
    {
        public ActionResult List()
        {
            Ward mObjWard = new Ward()
            {
                intStatus = 2
            };

            IList<usp_GetWardList_Result> lstWard = new BLWard().BL_GetWardList(mObjWard);
            return View(lstWard);
        }

        public ActionResult Add()
        {
            UI_FillLGADropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(WardViewModel pObjWardModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillLGADropDown();
                return View(pObjWardModel);
            }
            else
            {
                Ward mObjWard = new Ward()
                {
                    WardID = 0,
                    WardName = pObjWardModel.WardName.Trim(),  
                    LGAID = pObjWardModel.LGAID,                  
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLWard().BL_InsertUpdateWard(mObjWard);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "Ward");
                    }
                    else
                    {
                        UI_FillLGADropDown();
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjWardModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillLGADropDown();
                    ViewBag.Message = "Error occurred while saving ward";
                    return View(pObjWardModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Ward mObjWard = new Ward()
                {
                    WardID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetWardList_Result mObjWardData = new BLWard().BL_GetWardDetails(mObjWard);

                if (mObjWardData != null)
                {
                    WardViewModel mObjWardModelView = new WardViewModel()
                    {
                        WardID = mObjWardData.WardID.GetValueOrDefault(),
                        WardName = mObjWardData.WardName,
                        LGAID = mObjWardData.LGAID.GetValueOrDefault(),
                        Active = mObjWardData.Active.GetValueOrDefault(),
                    };

                    UI_FillLGADropDown(new LGA() { intStatus = 1, IncludeLGAIds= mObjWardData.LGAID.ToString() });
                    return View(mObjWardModelView);
                }
                else
                {
                    return RedirectToAction("List", "Ward");
                }
            }
            else
            {
                return RedirectToAction("List", "Ward");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(WardViewModel pObjWardModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillLGADropDown(new LGA() { intStatus = 1, IncludeLGAIds = pObjWardModel.LGAID.ToString() });
                return View(pObjWardModel);
            }
            else
            {
                Ward mObjWard = new Ward()
                {
                    WardID = pObjWardModel.WardID,
                    WardName = pObjWardModel.WardName.Trim(),
                    LGAID = pObjWardModel.LGAID,
                    Active = pObjWardModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLWard().BL_InsertUpdateWard(mObjWard);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "Ward");
                    }
                    else
                    {
                        UI_FillLGADropDown(new LGA() { intStatus = 1, IncludeLGAIds = pObjWardModel.LGAID.ToString() });
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjWardModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillLGADropDown(new LGA() { intStatus = 1, IncludeLGAIds = pObjWardModel.LGAID.ToString() });
                    ViewBag.Message = "Error occurred while saving ward";
                    return View(pObjWardModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Ward mObjWard = new Ward()
                {
                    WardID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetWardList_Result mObjWardData = new BLWard().BL_GetWardDetails(mObjWard);

                if (mObjWardData != null)
                {
                    WardViewModel mObjWardModelView = new WardViewModel()
                    {
                        WardID = mObjWardData.WardID.GetValueOrDefault(),
                        WardName = mObjWardData.WardName,
                        LGAName = mObjWardData.LGAName,
                        ActiveText = mObjWardData.ActiveText
                    };

                    return View(mObjWardModelView);
                }
                else
                {
                    return RedirectToAction("List", "Ward");
                }
            }
            else
            {
                return RedirectToAction("List", "Ward");
            }
        }

        public JsonResult UpdateStatus(Ward pObjWardData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjWardData.WardID != 0)
            {
                FuncResponse mObjFuncResponse = new BLWard().BL_UpdateStatus(pObjWardData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["WardList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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