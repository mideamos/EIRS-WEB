using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Vereyon.Web;
using Elmah;
using EIRS.Web.Utility;

namespace EIRS.Web.Controllers
{
    public class EMRevenueHeadController : BaseController
    {
        // GET: EMRevenueHead
        [HttpGet]
        public ActionResult List()
        {
            EM_RevenueHead mObjRevenueHead = new EM_RevenueHead()
            {
                intStatus = 2
            };

            IList<usp_EM_GetRevenueHeadList_Result> lstRevenueHead = new BLEMRevenueHead().BL_GetRevenueHeadList(mObjRevenueHead);
            return View(lstRevenueHead);
        }

        [HttpGet]
        public ActionResult Add()
        {
            UI_FillEMCategoryDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(EMRevenueHeadViewModel pObjRevenueHeadModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillEMCategoryDropDown();
                return View(pObjRevenueHeadModel);
            }
            else
            {
                EM_RevenueHead mObjRevenueHead = new EM_RevenueHead()
                {
                    RevenueHeadID = 0,
                    CategoryID = pObjRevenueHeadModel.CategoryID,
                    RevenueHeadName = pObjRevenueHeadModel.RevenueHeadName.Trim(),
                    Active = true,
                    CreatedBy = SessionManager.UserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLEMRevenueHead().BL_InsertUpdateRevenueHead(mObjRevenueHead);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "EMRevenueHead");
                    }
                    else
                    {
                        UI_FillEMCategoryDropDown();
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjRevenueHeadModel);
                    }
                }
                catch (Exception ex)
                {
                    Logger.SendErrorToText(ex);
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillEMCategoryDropDown();
                    ViewBag.Message = "Error occurred while saving revenue head";
                    return View(pObjRevenueHeadModel);
                }
            }
        }

        [HttpGet]
        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                EM_RevenueHead mObjRevenueHead = new EM_RevenueHead()
                {
                    RevenueHeadID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_EM_GetRevenueHeadList_Result mObjRevenueHeadData = new BLEMRevenueHead().BL_GetRevenueHeadDetails(mObjRevenueHead);

                if (mObjRevenueHeadData != null)
                {
                    EMRevenueHeadViewModel mObjRevenueHeadModelView = new EMRevenueHeadViewModel()
                    {
                        RevenueHeadID = mObjRevenueHeadData.RevenueHeadID.GetValueOrDefault(),
                        RevenueHeadName = mObjRevenueHeadData.RevenueHeadName,
                        CategoryID = mObjRevenueHeadData.CategoryID.GetValueOrDefault(),
                        Active = mObjRevenueHeadData.Active.GetValueOrDefault(),
                    };

                    UI_FillEMCategoryDropDown();
                    return View(mObjRevenueHeadModelView);
                }
                else
                {
                    return RedirectToAction("List", "EMRevenueHead");
                }
            }
            else
            {
                return RedirectToAction("List", "EMRevenueHead");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(EMRevenueHeadViewModel pObjRevenueHeadModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillEMCategoryDropDown();
                return View(pObjRevenueHeadModel);
            }
            else
            {
                EM_RevenueHead mObjRevenueHead = new EM_RevenueHead()
                {
                    RevenueHeadID = pObjRevenueHeadModel.RevenueHeadID,
                    CategoryID = pObjRevenueHeadModel.CategoryID,
                    RevenueHeadName = pObjRevenueHeadModel.RevenueHeadName.Trim(),
                    Active = pObjRevenueHeadModel.Active,
                    ModifiedBy = SessionManager.UserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLEMRevenueHead().BL_InsertUpdateRevenueHead(mObjRevenueHead);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "EMRevenueHead");
                    }
                    else
                    {
                        UI_FillEMCategoryDropDown();
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjRevenueHeadModel);
                    }
                }
                catch (Exception ex)
                {
                    Logger.SendErrorToText(ex);
                    UI_FillEMCategoryDropDown();
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving revenue head";
                    return View(pObjRevenueHeadModel);
                }
            }
        }

        [HttpGet]
        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                EM_RevenueHead mObjRevenueHead = new EM_RevenueHead()
                {
                    RevenueHeadID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_EM_GetRevenueHeadList_Result mObjRevenueHeadData = new BLEMRevenueHead().BL_GetRevenueHeadDetails(mObjRevenueHead);

                if (mObjRevenueHeadData != null)
                {
                    return View(mObjRevenueHeadData);
                }
                else
                {
                    return RedirectToAction("List", "EMRevenueHead");
                }
            }
            else
            {
                return RedirectToAction("List", "EMRevenueHead");
            }
        }

        public JsonResult UpdateStatus(EM_RevenueHead pObjRevenueHeadData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjRevenueHeadData.RevenueHeadID != 0)
            {
                FuncResponse mObjFuncResponse = new BLEMRevenueHead().BL_UpdateStatus(pObjRevenueHeadData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["RevenueHeadList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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