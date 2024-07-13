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
    public class TaxPayerTypeController : BaseController
    {
        public ActionResult List()
        {
            TaxPayer_Types mObjTaxPayerType = new TaxPayer_Types()
            {
                intStatus = 2
            };

            IList<usp_GetTaxPayerTypeList_Result> lstTaxPayerType = new BLTaxPayerType().BL_GetTaxPayerTypeList(mObjTaxPayerType);
            return View(lstTaxPayerType);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(TaxPayerTypeViewModel pObjTaxPayerTypeModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjTaxPayerTypeModel);
            }
            else
            {
                TaxPayer_Types mObjTaxPayerType = new TaxPayer_Types()
                {
                    TaxPayerTypeID = 0,
                    TaxPayerTypeName = pObjTaxPayerTypeModel.TaxPayerTypeName.Trim(),                    
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLTaxPayerType().BL_InsertUpdateTaxPayerType(mObjTaxPayerType);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "TaxPayerType");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjTaxPayerTypeModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving tax payer type";
                    return View(pObjTaxPayerTypeModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                TaxPayer_Types mObjTaxPayerType = new TaxPayer_Types()
                {
                    TaxPayerTypeID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetTaxPayerTypeList_Result mObjTaxPayerTypeData = new BLTaxPayerType().BL_GetTaxPayerTypeDetails(mObjTaxPayerType);

                if (mObjTaxPayerTypeData != null)
                {
                    TaxPayerTypeViewModel mObjTaxPayerTypeModelView = new TaxPayerTypeViewModel()
                    {
                        TaxPayerTypeID = mObjTaxPayerTypeData.TaxPayerTypeID.GetValueOrDefault(),
                        TaxPayerTypeName = mObjTaxPayerTypeData.TaxPayerTypeName,
                        Active = mObjTaxPayerTypeData.Active.GetValueOrDefault(),
                    };

                    return View(mObjTaxPayerTypeModelView);
                }
                else
                {
                    return RedirectToAction("List", "TaxPayerType");
                }
            }
            else
            {
                return RedirectToAction("List", "TaxPayerType");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(TaxPayerTypeViewModel pObjTaxPayerTypeModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjTaxPayerTypeModel);
            }
            else
            {
                TaxPayer_Types mObjTaxPayerType = new TaxPayer_Types()
                {
                    TaxPayerTypeID = pObjTaxPayerTypeModel.TaxPayerTypeID,
                    TaxPayerTypeName = pObjTaxPayerTypeModel.TaxPayerTypeName.Trim(),
                    Active = pObjTaxPayerTypeModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLTaxPayerType().BL_InsertUpdateTaxPayerType(mObjTaxPayerType);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "TaxPayerType");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjTaxPayerTypeModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving tax payer type";
                    return View(pObjTaxPayerTypeModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                TaxPayer_Types mObjTaxPayerType = new TaxPayer_Types()
                {
                    TaxPayerTypeID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetTaxPayerTypeList_Result mObjTaxPayerTypeData = new BLTaxPayerType().BL_GetTaxPayerTypeDetails(mObjTaxPayerType);

                if (mObjTaxPayerTypeData != null)
                {
                    TaxPayerTypeViewModel mObjTaxPayerTypeModelView = new TaxPayerTypeViewModel()
                    {
                        TaxPayerTypeID = mObjTaxPayerTypeData.TaxPayerTypeID.GetValueOrDefault(),
                        TaxPayerTypeName = mObjTaxPayerTypeData.TaxPayerTypeName,
                        ActiveText = mObjTaxPayerTypeData.ActiveText
                    };

                    return View(mObjTaxPayerTypeModelView);
                }
                else
                {
                    return RedirectToAction("List", "TaxPayerType");
                }
            }
            else
            {
                return RedirectToAction("List", "TaxPayerType");
            }
        }

        public JsonResult UpdateStatus(TaxPayer_Types pObjTaxPayerTypeData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjTaxPayerTypeData.TaxPayerTypeID != 0)
            {
                FuncResponse mObjFuncResponse = new BLTaxPayerType().BL_UpdateStatus(pObjTaxPayerTypeData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["TaxPayerTypeList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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