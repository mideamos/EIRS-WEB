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
    public class BusinessTypeController : BaseController
    {
        public ActionResult List()
        {
            Business_Types mObjBusinessType = new Business_Types()
            {
                intStatus = 2
            };

            IList<usp_GetBusinessTypeList_Result> lstBusinessType = new BLBusinessType().BL_GetBusinessTypeList(mObjBusinessType);
            return View(lstBusinessType);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(BusinessTypeViewModel pObjBusinessTypeModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjBusinessTypeModel);
            }
            else
            {
                Business_Types mObjBusinessType = new Business_Types()
                {
                    BusinessTypeID = 0,
                    BusinessTypeName = pObjBusinessTypeModel.BusinessTypeName.Trim(),                    
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLBusinessType().BL_InsertUpdateBusinessType(mObjBusinessType);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "BusinessType");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjBusinessTypeModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving business type";
                    return View(pObjBusinessTypeModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Business_Types mObjBusinessType = new Business_Types()
                {
                    BusinessTypeID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetBusinessTypeList_Result mObjBusinessTypeData = new BLBusinessType().BL_GetBusinessTypeDetails(mObjBusinessType);

                if (mObjBusinessTypeData != null)
                {
                    BusinessTypeViewModel mObjBusinessTypeModelView = new BusinessTypeViewModel()
                    {
                        BusinessTypeID = mObjBusinessTypeData.BusinessTypeID.GetValueOrDefault(),
                        BusinessTypeName = mObjBusinessTypeData.BusinessTypeName,
                        Active = mObjBusinessTypeData.Active.GetValueOrDefault(),
                    };

                    return View(mObjBusinessTypeModelView);
                }
                else
                {
                    return RedirectToAction("List", "BusinessType");
                }
            }
            else
            {
                return RedirectToAction("List", "BusinessType");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(BusinessTypeViewModel pObjBusinessTypeModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjBusinessTypeModel);
            }
            else
            {
                Business_Types mObjBusinessType = new Business_Types()
                {
                    BusinessTypeID = pObjBusinessTypeModel.BusinessTypeID,
                    BusinessTypeName = pObjBusinessTypeModel.BusinessTypeName.Trim(),
                    Active = pObjBusinessTypeModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLBusinessType().BL_InsertUpdateBusinessType(mObjBusinessType);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "BusinessType");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjBusinessTypeModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving business type";
                    return View(pObjBusinessTypeModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Business_Types mObjBusinessType = new Business_Types()
                {
                    BusinessTypeID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetBusinessTypeList_Result mObjBusinessTypeData = new BLBusinessType().BL_GetBusinessTypeDetails(mObjBusinessType);

                if (mObjBusinessTypeData != null)
                {
                    BusinessTypeViewModel mObjBusinessTypeModelView = new BusinessTypeViewModel()
                    {
                        BusinessTypeID = mObjBusinessTypeData.BusinessTypeID.GetValueOrDefault(),
                        BusinessTypeName = mObjBusinessTypeData.BusinessTypeName,
                        ActiveText = mObjBusinessTypeData.ActiveText
                    };

                    return View(mObjBusinessTypeModelView);
                }
                else
                {
                    return RedirectToAction("List", "BusinessType");
                }
            }
            else
            {
                return RedirectToAction("List", "BusinessType");
            }
        }

        public JsonResult UpdateStatus(Business_Types pObjBusinessTypeData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjBusinessTypeData.BusinessTypeID != 0)
            {
                FuncResponse mObjFuncResponse = new BLBusinessType().BL_UpdateStatus(pObjBusinessTypeData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["BusinessTypeList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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