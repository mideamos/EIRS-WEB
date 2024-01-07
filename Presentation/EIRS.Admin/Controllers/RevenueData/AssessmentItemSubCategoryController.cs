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
    public class AssessmentItemSubCategoryController : BaseController
    {
        public ActionResult List()
        {
            Assessment_Item_SubCategory mObjAssessmentItemSubCategory = new Assessment_Item_SubCategory()
            {
                intStatus = 2
            };

            IList<usp_GetAssessmentItemSubCategoryList_Result> lstAssessmentItemSubCategory = new BLAssessmentItemSubCategory().BL_GetAssessmentItemSubCategoryList(mObjAssessmentItemSubCategory);
            return View(lstAssessmentItemSubCategory);
        }

        public ActionResult Add()
        {
            UI_FillAssessmentItemCategoryDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(AssessmentItemSubCategoryViewModel pObjAssessmentItemSubCategoryModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillAssessmentItemCategoryDropDown();
                return View(pObjAssessmentItemSubCategoryModel);
            }
            else
            {
                Assessment_Item_SubCategory mObjAssessmentItemSubCategory = new Assessment_Item_SubCategory()
                {
                    AssessmentItemSubCategoryID = 0,
                    AssessmentItemSubCategoryName = pObjAssessmentItemSubCategoryModel.AssessmentItemSubCategoryName.Trim(),
                    AssessmentItemCategoryID = pObjAssessmentItemSubCategoryModel.AssessmentItemCategoryID,
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLAssessmentItemSubCategory().BL_InsertUpdateAssessmentItemSubCategory(mObjAssessmentItemSubCategory);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "AssessmentItemSubCategory");
                    }
                    else
                    {
                        UI_FillAssessmentItemCategoryDropDown();
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjAssessmentItemSubCategoryModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillAssessmentItemCategoryDropDown();
                    ViewBag.Message = "Error occurred while saving assessment item sub category";
                    return View(pObjAssessmentItemSubCategoryModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Assessment_Item_SubCategory mObjAssessmentItemSubCategory = new Assessment_Item_SubCategory()
                {
                    AssessmentItemSubCategoryID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetAssessmentItemSubCategoryList_Result mObjAssessmentItemSubCategoryData = new BLAssessmentItemSubCategory().BL_GetAssessmentItemSubCategoryDetails(mObjAssessmentItemSubCategory);

                if (mObjAssessmentItemSubCategoryData != null)
                {
                    AssessmentItemSubCategoryViewModel mObjAssessmentItemSubCategoryModelView = new AssessmentItemSubCategoryViewModel()
                    {
                        AssessmentItemSubCategoryID = mObjAssessmentItemSubCategoryData.AssessmentItemSubCategoryID.GetValueOrDefault(),
                        AssessmentItemSubCategoryName = mObjAssessmentItemSubCategoryData.AssessmentItemSubCategoryName,
                        AssessmentItemCategoryID = mObjAssessmentItemSubCategoryData.AssessmentItemCategoryID.GetValueOrDefault(),
                        Active = mObjAssessmentItemSubCategoryData.Active.GetValueOrDefault(),
                    };

                    UI_FillAssessmentItemCategoryDropDown(new Assessment_Item_Category() { intStatus = 1, IncludeAssessmentItemCategoryIds = mObjAssessmentItemSubCategoryModelView.AssessmentItemCategoryID.ToString() });
                    return View(mObjAssessmentItemSubCategoryModelView);
                }
                else
                {
                    return RedirectToAction("List", "AssessmentItemSubCategory");
                }
            }
            else
            {
                return RedirectToAction("List", "AssessmentItemSubCategory");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(AssessmentItemSubCategoryViewModel pObjAssessmentItemSubCategoryModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillAssessmentItemCategoryDropDown(new Assessment_Item_Category() { intStatus = 1, IncludeAssessmentItemCategoryIds = pObjAssessmentItemSubCategoryModel.AssessmentItemCategoryID.ToString() });
                return View(pObjAssessmentItemSubCategoryModel);
            }
            else
            {
                Assessment_Item_SubCategory mObjAssessmentItemSubCategory = new Assessment_Item_SubCategory()
                {
                    AssessmentItemSubCategoryID = pObjAssessmentItemSubCategoryModel.AssessmentItemSubCategoryID,
                    AssessmentItemSubCategoryName = pObjAssessmentItemSubCategoryModel.AssessmentItemSubCategoryName.Trim(),
                    AssessmentItemCategoryID = pObjAssessmentItemSubCategoryModel.AssessmentItemCategoryID,
                    Active = pObjAssessmentItemSubCategoryModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLAssessmentItemSubCategory().BL_InsertUpdateAssessmentItemSubCategory(mObjAssessmentItemSubCategory);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "AssessmentItemSubCategory");
                    }
                    else
                    {
                        UI_FillAssessmentItemCategoryDropDown(new Assessment_Item_Category() { intStatus = 1, IncludeAssessmentItemCategoryIds = pObjAssessmentItemSubCategoryModel.AssessmentItemCategoryID.ToString() });
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjAssessmentItemSubCategoryModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillAssessmentItemCategoryDropDown(new Assessment_Item_Category() { intStatus = 1, IncludeAssessmentItemCategoryIds = pObjAssessmentItemSubCategoryModel.AssessmentItemCategoryID.ToString() });
                    ViewBag.Message = "Error occurred while saving assessment item sub category";
                    return View(pObjAssessmentItemSubCategoryModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Assessment_Item_SubCategory mObjAssessmentItemSubCategory = new Assessment_Item_SubCategory()
                {
                    AssessmentItemSubCategoryID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetAssessmentItemSubCategoryList_Result mObjAssessmentItemSubCategoryData = new BLAssessmentItemSubCategory().BL_GetAssessmentItemSubCategoryDetails(mObjAssessmentItemSubCategory);

                if (mObjAssessmentItemSubCategoryData != null)
                {
                    AssessmentItemSubCategoryViewModel mObjAssessmentItemSubCategoryModelView = new AssessmentItemSubCategoryViewModel()
                    {
                        AssessmentItemSubCategoryID = mObjAssessmentItemSubCategoryData.AssessmentItemSubCategoryID.GetValueOrDefault(),
                        AssessmentItemSubCategoryName = mObjAssessmentItemSubCategoryData.AssessmentItemSubCategoryName,
                        AssessmentItemCategoryName = mObjAssessmentItemSubCategoryData.AssessmentItemCategoryName,
                        ActiveText = mObjAssessmentItemSubCategoryData.ActiveText
                    };

                    return View(mObjAssessmentItemSubCategoryModelView);
                }
                else
                {
                    return RedirectToAction("List", "AssessmentItemSubCategory");
                }
            }
            else
            {
                return RedirectToAction("List", "AssessmentItemSubCategory");
            }
        }

        public JsonResult UpdateStatus(Assessment_Item_SubCategory pObjAssessmentItemSubCategoryData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjAssessmentItemSubCategoryData.AssessmentItemSubCategoryID != 0)
            {
                FuncResponse mObjFuncResponse = new BLAssessmentItemSubCategory().BL_UpdateStatus(pObjAssessmentItemSubCategoryData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["AssessmentItemSubCategoryList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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