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
    public class BusinessOperationController : BaseController
    {
        public ActionResult List()
        {
            Business_Operation mObjBusinessOperation = new Business_Operation()
            {
                intStatus = 2
            };

            IList<usp_GetBusinessOperationList_Result> lstBusinessOperation = new BLBusinessOperation().BL_GetBusinessOperationList(mObjBusinessOperation);
            return View(lstBusinessOperation);
        }

        public ActionResult Add()
        {
            UI_FillBusinessTypeDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(BusinessOperationViewModel pObjBusinessOperationModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillBusinessTypeDropDown();
                return View(pObjBusinessOperationModel);
            }
            else
            {
                Business_Operation mObjBusinessOperation = new Business_Operation()
                {
                    BusinessOperationID = 0,
                    BusinessOperationName = pObjBusinessOperationModel.BusinessOperationName.Trim(), 
                    BusinessTypeID = pObjBusinessOperationModel.BusinessTypeID,                   
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLBusinessOperation().BL_InsertUpdateBusinessOperation(mObjBusinessOperation);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "BusinessOperation");
                    }
                    else
                    {
                        UI_FillBusinessTypeDropDown();
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjBusinessOperationModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillBusinessTypeDropDown();
                    ViewBag.Message = "Error occurred while saving business operation";
                    return View(pObjBusinessOperationModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Business_Operation mObjBusinessOperation = new Business_Operation()
                {
                    BusinessOperationID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetBusinessOperationList_Result mObjBusinessOperationData = new BLBusinessOperation().BL_GetBusinessOperationDetails(mObjBusinessOperation);

                if (mObjBusinessOperationData != null)
                {
                    BusinessOperationViewModel mObjBusinessOperationModelView = new BusinessOperationViewModel()
                    {
                        BusinessOperationID = mObjBusinessOperationData.BusinessOperationID.GetValueOrDefault(),
                        BusinessOperationName = mObjBusinessOperationData.BusinessOperationName,
                        BusinessTypeID = mObjBusinessOperationData.BusinessTypeID.GetValueOrDefault(),
                        Active = mObjBusinessOperationData.Active.GetValueOrDefault(),
                    };

                    UI_FillBusinessTypeDropDown(new Business_Types() { intStatus = 1, IncludeBusinessTypeIds = mObjBusinessOperationModelView.BusinessTypeID.ToString() });

                    return View(mObjBusinessOperationModelView);
                }
                else
                {
                    return RedirectToAction("List", "BusinessOperation");
                }
            }
            else
            {
                return RedirectToAction("List", "BusinessOperation");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(BusinessOperationViewModel pObjBusinessOperationModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillBusinessTypeDropDown(new Business_Types() { intStatus = 1, IncludeBusinessTypeIds = pObjBusinessOperationModel.BusinessTypeID.ToString() });
                return View(pObjBusinessOperationModel);
            }
            else
            {
                Business_Operation mObjBusinessOperation = new Business_Operation()
                {
                    BusinessOperationID = pObjBusinessOperationModel.BusinessOperationID,
                    BusinessOperationName = pObjBusinessOperationModel.BusinessOperationName.Trim(),
                    BusinessTypeID = pObjBusinessOperationModel.BusinessTypeID,
                    Active = pObjBusinessOperationModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLBusinessOperation().BL_InsertUpdateBusinessOperation(mObjBusinessOperation);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "BusinessOperation");
                    }
                    else
                    {
                        UI_FillBusinessTypeDropDown(new Business_Types() { intStatus = 1, IncludeBusinessTypeIds = pObjBusinessOperationModel.BusinessTypeID.ToString() });
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjBusinessOperationModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillBusinessTypeDropDown(new Business_Types() { intStatus = 1, IncludeBusinessTypeIds = pObjBusinessOperationModel.BusinessTypeID.ToString() });
                    ViewBag.Message = "Error occurred while saving business operation";
                    return View(pObjBusinessOperationModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Business_Operation mObjBusinessOperation = new Business_Operation()
                {
                    BusinessOperationID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetBusinessOperationList_Result mObjBusinessOperationData = new BLBusinessOperation().BL_GetBusinessOperationDetails(mObjBusinessOperation);

                if (mObjBusinessOperationData != null)
                {
                    BusinessOperationViewModel mObjBusinessOperationModelView = new BusinessOperationViewModel()
                    {
                        BusinessOperationID = mObjBusinessOperationData.BusinessOperationID.GetValueOrDefault(),
                        BusinessOperationName = mObjBusinessOperationData.BusinessOperationName,
                        BusinessTypeName = mObjBusinessOperationData.BusinessTypeName,
                        ActiveText = mObjBusinessOperationData.ActiveText
                    };

                    return View(mObjBusinessOperationModelView);
                }
                else
                {
                    return RedirectToAction("List", "BusinessOperation");
                }
            }
            else
            {
                return RedirectToAction("List", "BusinessOperation");
            }
        }

        public JsonResult UpdateStatus(Business_Operation pObjBusinessOperationData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjBusinessOperationData.BusinessOperationID != 0)
            {
                FuncResponse mObjFuncResponse = new BLBusinessOperation().BL_UpdateStatus(pObjBusinessOperationData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["BusinessOperationList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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