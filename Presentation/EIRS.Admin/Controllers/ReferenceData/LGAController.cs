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
    public class LGAController : BaseController
    {
        public ActionResult List()
        {
            LGA mObjLGA = new LGA()
            {
                intStatus = 2
            };

            IList<usp_GetLGAList_Result> lstLGA = new BLLGA().BL_GetLGAList(mObjLGA);
            return View(lstLGA);
        }

        public ActionResult Add()
        {
            UI_FillLGAClass();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(LGAViewModel pObjLGAModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillLGAClass();
                return View(pObjLGAModel);
            }
            else
            {
                LGA mObjLGA = new LGA()
                {
                    LGAID = 0,
                    LGAName = pObjLGAModel.LGAName.Trim(), 
                    LGAClassID = pObjLGAModel.LGAClassID,                   
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLLGA().BL_InsertUpdateLGA(mObjLGA);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "LGA");
                    }
                    else
                    {
                        UI_FillLGAClass();
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjLGAModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillLGAClass();
                    ViewBag.Message = "Error occurred while saving local government area";
                    return View(pObjLGAModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                LGA mObjLGA = new LGA()
                {
                    LGAID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetLGAList_Result mObjLGAData = new BLLGA().BL_GetLGADetails(mObjLGA);

                if (mObjLGAData != null)
                {
                    LGAViewModel mObjLGAModelView = new LGAViewModel()
                    {
                        LGAID = mObjLGAData.LGAID.GetValueOrDefault(),
                        LGAName = mObjLGAData.LGAName,
                        LGAClassID = mObjLGAData.LGAClassID.GetValueOrDefault(),
                        Active = mObjLGAData.Active.GetValueOrDefault(),
                    };

                    UI_FillLGAClass();
                    return View(mObjLGAModelView);
                }
                else
                {
                    return RedirectToAction("List", "LGA");
                }
            }
            else
            {
                return RedirectToAction("List", "LGA");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(LGAViewModel pObjLGAModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillLGAClass();
                return View(pObjLGAModel);
            }
            else
            {
                LGA mObjLGA = new LGA()
                {
                    LGAID = pObjLGAModel.LGAID,
                    LGAName = pObjLGAModel.LGAName.Trim(),
                    LGAClassID = pObjLGAModel.LGAClassID,
                    Active = pObjLGAModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLLGA().BL_InsertUpdateLGA(mObjLGA);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "LGA");
                    }
                    else
                    {
                        UI_FillLGAClass();
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjLGAModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillLGAClass();
                    ViewBag.Message = "Error occurred while saving local government area";
                    return View(pObjLGAModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                LGA mObjLGA = new LGA()
                {
                    LGAID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetLGAList_Result mObjLGAData = new BLLGA().BL_GetLGADetails(mObjLGA);

                if (mObjLGAData != null)
                {
                    LGAViewModel mObjLGAModelView = new LGAViewModel()
                    {
                        LGAID = mObjLGAData.LGAID.GetValueOrDefault(),
                        LGAName = mObjLGAData.LGAName,
                        LGAClassName = mObjLGAData.LGAClassName,
                        ActiveText = mObjLGAData.ActiveText
                    };

                    return View(mObjLGAModelView);
                }
                else
                {
                    return RedirectToAction("List", "LGA");
                }
            }
            else
            {
                return RedirectToAction("List", "LGA");
            }
        }

        public JsonResult UpdateStatus(LGA pObjLGAData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjLGAData.LGAID != 0)
            {
                FuncResponse mObjFuncResponse = new BLLGA().BL_UpdateStatus(pObjLGAData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["LGAList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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