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
    public class AssessmentItemCategoryController : BaseController
    {
        public ActionResult List()
        {
            Assessment_Item_Category mObjAssessmentItemCategory = new Assessment_Item_Category()
            {
                intStatus = 2
            };

            IList<usp_GetAssessmentItemCategoryList_Result> lstAssessmentItemCategory = new BLAssessmentItemCategory().BL_GetAssessmentItemCategoryList(mObjAssessmentItemCategory);
            return View(lstAssessmentItemCategory);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(AssessmentItemCategoryViewModel pObjAssessmentItemCategoryModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjAssessmentItemCategoryModel);
            }
            else
            {
                Assessment_Item_Category mObjAssessmentItemCategory = new Assessment_Item_Category()
                {
                    AssessmentItemCategoryID = 0,
                    AssessmentItemCategoryName = pObjAssessmentItemCategoryModel.AssessmentItemCategoryName.Trim(),                    
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLAssessmentItemCategory().BL_InsertUpdateAssessmentItemCategory(mObjAssessmentItemCategory);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "AssessmentItemCategory");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjAssessmentItemCategoryModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving assessment item category";
                    return View(pObjAssessmentItemCategoryModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Assessment_Item_Category mObjAssessmentItemCategory = new Assessment_Item_Category()
                {
                    AssessmentItemCategoryID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetAssessmentItemCategoryList_Result mObjAssessmentItemCategoryData = new BLAssessmentItemCategory().BL_GetAssessmentItemCategoryDetails(mObjAssessmentItemCategory);

                if (mObjAssessmentItemCategoryData != null)
                {
                    AssessmentItemCategoryViewModel mObjAssessmentItemCategoryModelView = new AssessmentItemCategoryViewModel()
                    {
                        AssessmentItemCategoryID = mObjAssessmentItemCategoryData.AssessmentItemCategoryID.GetValueOrDefault(),
                        AssessmentItemCategoryName = mObjAssessmentItemCategoryData.AssessmentItemCategoryName,
                        Active = mObjAssessmentItemCategoryData.Active.GetValueOrDefault(),
                    };

                    return View(mObjAssessmentItemCategoryModelView);
                }
                else
                {
                    return RedirectToAction("List", "AssessmentItemCategory");
                }
            }
            else
            {
                return RedirectToAction("List", "AssessmentItemCategory");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(AssessmentItemCategoryViewModel pObjAssessmentItemCategoryModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjAssessmentItemCategoryModel);
            }
            else
            {
                Assessment_Item_Category mObjAssessmentItemCategory = new Assessment_Item_Category()
                {
                    AssessmentItemCategoryID = pObjAssessmentItemCategoryModel.AssessmentItemCategoryID,
                    AssessmentItemCategoryName = pObjAssessmentItemCategoryModel.AssessmentItemCategoryName.Trim(),
                    Active = pObjAssessmentItemCategoryModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLAssessmentItemCategory().BL_InsertUpdateAssessmentItemCategory(mObjAssessmentItemCategory);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "AssessmentItemCategory");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjAssessmentItemCategoryModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving assessment item category";
                    return View(pObjAssessmentItemCategoryModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Assessment_Item_Category mObjAssessmentItemCategory = new Assessment_Item_Category()
                {
                    AssessmentItemCategoryID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetAssessmentItemCategoryList_Result mObjAssessmentItemCategoryData = new BLAssessmentItemCategory().BL_GetAssessmentItemCategoryDetails(mObjAssessmentItemCategory);

                if (mObjAssessmentItemCategoryData != null)
                {
                    AssessmentItemCategoryViewModel mObjAssessmentItemCategoryModelView = new AssessmentItemCategoryViewModel()
                    {
                        AssessmentItemCategoryID = mObjAssessmentItemCategoryData.AssessmentItemCategoryID.GetValueOrDefault(),
                        AssessmentItemCategoryName = mObjAssessmentItemCategoryData.AssessmentItemCategoryName,
                        ActiveText = mObjAssessmentItemCategoryData.ActiveText
                    };

                    return View(mObjAssessmentItemCategoryModelView);
                }
                else
                {
                    return RedirectToAction("List", "AssessmentItemCategory");
                }
            }
            else
            {
                return RedirectToAction("List", "AssessmentItemCategory");
            }
        }

        public JsonResult UpdateStatus(Assessment_Item_Category pObjAssessmentItemCategoryData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjAssessmentItemCategoryData.AssessmentItemCategoryID != 0)
            {
                FuncResponse mObjFuncResponse = new BLAssessmentItemCategory().BL_UpdateStatus(pObjAssessmentItemCategoryData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["AssessmentItemCategoryList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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