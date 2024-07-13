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
    public class TownController : BaseController
    {
        public ActionResult List()
        {
            Town mObjTown = new Town()
            {
                intStatus = 2
            };

            IList<usp_GetTownList_Result> lstTown = new BLTown().BL_GetTownList(mObjTown);
            return View(lstTown);
        }

        public ActionResult Add()
        {
            UI_FillLGADropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(TownViewModel pObjTownModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillLGADropDown();
                return View(pObjTownModel);
            }
            else
            {
                Town mObjTown = new Town()
                {
                    TownID = 0,
                    TownName = pObjTownModel.TownName.Trim(), 
                    LGAID = pObjTownModel.LGAID,                   
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLTown().BL_InsertUpdateTown(mObjTown);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "Town");
                    }
                    else
                    {
                        UI_FillLGADropDown();
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjTownModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillLGADropDown();
                    ViewBag.Message = "Error occurred while saving town";
                    return View(pObjTownModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Town mObjTown = new Town()
                {
                    TownID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetTownList_Result mObjTownData = new BLTown().BL_GetTownDetails(mObjTown);

                if (mObjTownData != null)
                {
                    TownViewModel mObjTownModelView = new TownViewModel()
                    {
                        TownID = mObjTownData.TownID.GetValueOrDefault(),
                        TownName = mObjTownData.TownName,
                        LGAID = mObjTownData.LGAID.GetValueOrDefault(),
                        Active = mObjTownData.Active.GetValueOrDefault(),
                    };

                    UI_FillLGADropDown(new LGA() { intStatus = 1, IncludeLGAIds = mObjTownData.LGAID.ToString() });
                    return View(mObjTownModelView);
                }
                else
                {
                    return RedirectToAction("List", "Town");
                }
            }
            else
            {
                return RedirectToAction("List", "Town");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(TownViewModel pObjTownModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillLGADropDown(new LGA() { intStatus = 1, IncludeLGAIds = pObjTownModel.LGAID.ToString() });
                return View(pObjTownModel);
            }
            else
            {
                Town mObjTown = new Town()
                {
                    TownID = pObjTownModel.TownID,
                    TownName = pObjTownModel.TownName.Trim(),
                    LGAID = pObjTownModel.LGAID,
                    Active = pObjTownModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLTown().BL_InsertUpdateTown(mObjTown);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "Town");
                    }
                    else
                    {
                        UI_FillLGADropDown(new LGA() { intStatus = 1, IncludeLGAIds = pObjTownModel.LGAID.ToString() });
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjTownModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillLGADropDown(new LGA() { intStatus = 1, IncludeLGAIds = pObjTownModel.LGAID.ToString() });
                    ViewBag.Message = "Error occurred while saving town";
                    return View(pObjTownModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Town mObjTown = new Town()
                {
                    TownID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetTownList_Result mObjTownData = new BLTown().BL_GetTownDetails(mObjTown);

                if (mObjTownData != null)
                {
                    TownViewModel mObjTownModelView = new TownViewModel()
                    {
                        TownID = mObjTownData.TownID.GetValueOrDefault(),
                        TownName = mObjTownData.TownName,
                        LGAName = mObjTownData.LGAName,
                        ActiveText = mObjTownData.ActiveText
                    };

                    return View(mObjTownModelView);
                }
                else
                {
                    return RedirectToAction("List", "Town");
                }
            }
            else
            {
                return RedirectToAction("List", "Town");
            }
        }

        public JsonResult UpdateStatus(Town pObjTownData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjTownData.TownID != 0)
            {
                FuncResponse mObjFuncResponse = new BLTown().BL_UpdateStatus(pObjTownData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["TownList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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