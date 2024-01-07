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
    public class PaymentOptionController : BaseController
    {
        public ActionResult List()
        {
            Payment_Options mObjPaymentOption = new Payment_Options()
            {
                intStatus = 2
            };

            IList<usp_GetPaymentOptionList_Result> lstPaymentOption = new BLPaymentOption().BL_GetPaymentOptionList(mObjPaymentOption);
            return View(lstPaymentOption);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(PaymentOptionViewModel pObjPaymentOptionModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjPaymentOptionModel);
            }
            else
            {
                Payment_Options mObjPaymentOption = new Payment_Options()
                {
                    PaymentOptionID = 0,
                    PaymentOptionName = pObjPaymentOptionModel.PaymentOptionName.Trim(),                    
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLPaymentOption().BL_InsertUpdatePaymentOption(mObjPaymentOption);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "PaymentOption");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjPaymentOptionModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving payment option";
                    return View(pObjPaymentOptionModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Payment_Options mObjPaymentOption = new Payment_Options()
                {
                    PaymentOptionID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetPaymentOptionList_Result mObjPaymentOptionData = new BLPaymentOption().BL_GetPaymentOptionDetails(mObjPaymentOption);

                if (mObjPaymentOptionData != null)
                {
                    PaymentOptionViewModel mObjPaymentOptionModelView = new PaymentOptionViewModel()
                    {
                        PaymentOptionID = mObjPaymentOptionData.PaymentOptionID.GetValueOrDefault(),
                        PaymentOptionName = mObjPaymentOptionData.PaymentOptionName,
                        Active = mObjPaymentOptionData.Active.GetValueOrDefault(),
                    };

                    return View(mObjPaymentOptionModelView);
                }
                else
                {
                    return RedirectToAction("List", "PaymentOption");
                }
            }
            else
            {
                return RedirectToAction("List", "PaymentOption");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(PaymentOptionViewModel pObjPaymentOptionModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjPaymentOptionModel);
            }
            else
            {
                Payment_Options mObjPaymentOption = new Payment_Options()
                {
                    PaymentOptionID = pObjPaymentOptionModel.PaymentOptionID,
                    PaymentOptionName = pObjPaymentOptionModel.PaymentOptionName.Trim(),
                    Active = pObjPaymentOptionModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLPaymentOption().BL_InsertUpdatePaymentOption(mObjPaymentOption);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "PaymentOption");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjPaymentOptionModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving payment option";
                    return View(pObjPaymentOptionModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Payment_Options mObjPaymentOption = new Payment_Options()
                {
                    PaymentOptionID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetPaymentOptionList_Result mObjPaymentOptionData = new BLPaymentOption().BL_GetPaymentOptionDetails(mObjPaymentOption);

                if (mObjPaymentOptionData != null)
                {
                    PaymentOptionViewModel mObjPaymentOptionModelView = new PaymentOptionViewModel()
                    {
                        PaymentOptionID = mObjPaymentOptionData.PaymentOptionID.GetValueOrDefault(),
                        PaymentOptionName = mObjPaymentOptionData.PaymentOptionName,
                        ActiveText = mObjPaymentOptionData.ActiveText
                    };

                    return View(mObjPaymentOptionModelView);
                }
                else
                {
                    return RedirectToAction("List", "PaymentOption");
                }
            }
            else
            {
                return RedirectToAction("List", "PaymentOption");
            }
        }

        public JsonResult UpdateStatus(Payment_Options pObjPaymentOptionData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjPaymentOptionData.PaymentOptionID != 0)
            {
                FuncResponse mObjFuncResponse = new BLPaymentOption().BL_UpdateStatus(pObjPaymentOptionData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["PaymentOptionList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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