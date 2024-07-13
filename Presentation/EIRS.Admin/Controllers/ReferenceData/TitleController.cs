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
    public class TitleController : BaseController
    {
        public ActionResult List()
        {
            Title mObjTitle = new Title()
            {
                intStatus = 2
            };

            IList<usp_GetTitleList_Result> lstTitle = new BLTitle().BL_GetTitleList(mObjTitle);
            return View(lstTitle);
        }

        public ActionResult Add()
        {
            UI_FillGender();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(TitleViewModel pObjTitleModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillGender();
                return View(pObjTitleModel);
            }
            else
            {
                Title mObjTitle = new Title()
                {
                    TitleID = 0,
                    TitleName = pObjTitleModel.TitleName.Trim(),  
                    GenderID = pObjTitleModel.GenderID,                  
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLTitle().BL_InsertUpdateTitle(mObjTitle);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "Title");
                    }
                    else
                    {
                        UI_FillGender();
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjTitleModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillGender();
                    ViewBag.Message = "Error occurred while saving title";
                    return View(pObjTitleModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Title mObjTitle = new Title()
                {
                    TitleID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetTitleList_Result mObjTitleData = new BLTitle().BL_GetTitleDetails(mObjTitle);

                if (mObjTitleData != null)
                {
                    TitleViewModel mObjTitleModelView = new TitleViewModel()
                    {
                        TitleID = mObjTitleData.TitleID.GetValueOrDefault(),
                        TitleName = mObjTitleData.TitleName,
                        GenderID = mObjTitleData.GenderID.GetValueOrDefault(),
                        Active = mObjTitleData.Active.GetValueOrDefault(),
                    };

                    UI_FillGender();
                    return View(mObjTitleModelView);
                }
                else
                {
                    return RedirectToAction("List", "Title");
                }
            }
            else
            {
                return RedirectToAction("List", "Title");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(TitleViewModel pObjTitleModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillGender();
                return View(pObjTitleModel);
            }
            else
            {
                Title mObjTitle = new Title()
                {
                    TitleID = pObjTitleModel.TitleID,
                    TitleName = pObjTitleModel.TitleName.Trim(),
                    GenderID = pObjTitleModel.GenderID,
                    Active = pObjTitleModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLTitle().BL_InsertUpdateTitle(mObjTitle);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "Title");
                    }
                    else
                    {
                        UI_FillGender();
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjTitleModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillGender();
                    ViewBag.Message = "Error occurred while saving title";
                    return View(pObjTitleModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Title mObjTitle = new Title()
                {
                    TitleID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetTitleList_Result mObjTitleData = new BLTitle().BL_GetTitleDetails(mObjTitle);

                if (mObjTitleData != null)
                {
                    TitleViewModel mObjTitleModelView = new TitleViewModel()
                    {
                        TitleID = mObjTitleData.TitleID.GetValueOrDefault(),
                        TitleName = mObjTitleData.TitleName,
                        GenderName = mObjTitleData.GenderName,
                        ActiveText = mObjTitleData.ActiveText
                    };

                    return View(mObjTitleModelView);
                }
                else
                {
                    return RedirectToAction("List", "Title");
                }
            }
            else
            {
                return RedirectToAction("List", "Title");
            }
        }

        public JsonResult UpdateStatus(Title pObjTitleData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjTitleData.TitleID != 0)
            {
                FuncResponse mObjFuncResponse = new BLTitle().BL_UpdateStatus(pObjTitleData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["TitleList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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