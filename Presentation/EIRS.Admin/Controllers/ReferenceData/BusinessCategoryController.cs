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
    public class BusinessCategoryController : BaseController
    {
        public ActionResult List()
        {
            Business_Category mObjBusinessCategory = new Business_Category()
            {
                intStatus = 2
            };

            IList<usp_GetBusinessCategoryList_Result> lstBusinessCategory = new BLBusinessCategory().BL_GetBusinessCategoryList(mObjBusinessCategory);
            return View(lstBusinessCategory);
        }

        public ActionResult Add()
        {
            UI_FillBusinessTypeDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(BusinessCategoryViewModel pObjBusinessCategoryModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillBusinessTypeDropDown();
                return View(pObjBusinessCategoryModel);
            }
            else
            {
                Business_Category mObjBusinessCategory = new Business_Category()
                {
                    BusinessCategoryID = 0,
                    BusinessCategoryName = pObjBusinessCategoryModel.BusinessCategoryName.Trim(), 
                    BusinessTypeID = pObjBusinessCategoryModel.BusinessTypeID,                   
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLBusinessCategory().BL_InsertUpdateBusinessCategory(mObjBusinessCategory);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "BusinessCategory");
                    }
                    else
                    {
                        UI_FillBusinessTypeDropDown();
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjBusinessCategoryModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillBusinessTypeDropDown();
                    ViewBag.Message = "Error occurred while saving business category";
                    return View(pObjBusinessCategoryModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Business_Category mObjBusinessCategory = new Business_Category()
                {
                    BusinessCategoryID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetBusinessCategoryList_Result mObjBusinessCategoryData = new BLBusinessCategory().BL_GetBusinessCategoryDetails(mObjBusinessCategory);

                if (mObjBusinessCategoryData != null)
                {
                    BusinessCategoryViewModel mObjBusinessCategoryModelView = new BusinessCategoryViewModel()
                    {
                        BusinessCategoryID = mObjBusinessCategoryData.BusinessCategoryID.GetValueOrDefault(),
                        BusinessCategoryName = mObjBusinessCategoryData.BusinessCategoryName,
                        BusinessTypeID = mObjBusinessCategoryData.BusinessTypeID.GetValueOrDefault(),
                        Active = mObjBusinessCategoryData.Active.GetValueOrDefault(),
                    };

                    UI_FillBusinessTypeDropDown(new Business_Types() { intStatus = 1, IncludeBusinessTypeIds = mObjBusinessCategoryModelView.BusinessTypeID.ToString() });

                    return View(mObjBusinessCategoryModelView);
                }
                else
                {
                    return RedirectToAction("List", "BusinessCategory");
                }
            }
            else
            {
                return RedirectToAction("List", "BusinessCategory");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(BusinessCategoryViewModel pObjBusinessCategoryModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillBusinessTypeDropDown(new Business_Types() { intStatus = 1, IncludeBusinessTypeIds = pObjBusinessCategoryModel.BusinessTypeID.ToString() });
                return View(pObjBusinessCategoryModel);
            }
            else
            {
                Business_Category mObjBusinessCategory = new Business_Category()
                {
                    BusinessCategoryID = pObjBusinessCategoryModel.BusinessCategoryID,
                    BusinessCategoryName = pObjBusinessCategoryModel.BusinessCategoryName.Trim(),
                    BusinessTypeID = pObjBusinessCategoryModel.BusinessTypeID,
                    Active = pObjBusinessCategoryModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLBusinessCategory().BL_InsertUpdateBusinessCategory(mObjBusinessCategory);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "BusinessCategory");
                    }
                    else
                    {
                        UI_FillBusinessTypeDropDown(new Business_Types() { intStatus = 1, IncludeBusinessTypeIds = pObjBusinessCategoryModel.BusinessTypeID.ToString() });
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjBusinessCategoryModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillBusinessTypeDropDown(new Business_Types() { intStatus = 1, IncludeBusinessTypeIds = pObjBusinessCategoryModel.BusinessTypeID.ToString() });
                    ViewBag.Message = "Error occurred while saving business category";
                    return View(pObjBusinessCategoryModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Business_Category mObjBusinessCategory = new Business_Category()
                {
                    BusinessCategoryID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetBusinessCategoryList_Result mObjBusinessCategoryData = new BLBusinessCategory().BL_GetBusinessCategoryDetails(mObjBusinessCategory);

                if (mObjBusinessCategoryData != null)
                {
                    BusinessCategoryViewModel mObjBusinessCategoryModelView = new BusinessCategoryViewModel()
                    {
                        BusinessCategoryID = mObjBusinessCategoryData.BusinessCategoryID.GetValueOrDefault(),
                        BusinessCategoryName = mObjBusinessCategoryData.BusinessCategoryName,
                        BusinessTypeName = mObjBusinessCategoryData.BusinessTypeName,
                        ActiveText = mObjBusinessCategoryData.ActiveText
                    };

                    return View(mObjBusinessCategoryModelView);
                }
                else
                {
                    return RedirectToAction("List", "BusinessCategory");
                }
            }
            else
            {
                return RedirectToAction("List", "BusinessCategory");
            }
        }

        public JsonResult UpdateStatus(Business_Category pObjBusinessCategoryData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjBusinessCategoryData.BusinessCategoryID != 0)
            {
                FuncResponse mObjFuncResponse = new BLBusinessCategory().BL_UpdateStatus(pObjBusinessCategoryData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["BusinessCategoryList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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