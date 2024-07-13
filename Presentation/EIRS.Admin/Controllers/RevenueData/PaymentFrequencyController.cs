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
    public class PaymentFrequencyController : BaseController
    {
        public ActionResult List()
        {
            Payment_Frequency mObjPaymentFrequency = new Payment_Frequency()
            {
                intStatus = 2
            };

            IList<usp_GetPaymentFrequencyList_Result> lstPaymentFrequency = new BLPaymentFrequency().BL_GetPaymentFrequencyList(mObjPaymentFrequency);
            return View(lstPaymentFrequency);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(PaymentFrequencyViewModel pObjPaymentFrequencyModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjPaymentFrequencyModel);
            }
            else
            {
                Payment_Frequency mObjPaymentFrequency = new Payment_Frequency()
                {
                    PaymentFrequencyID = 0,
                    PaymentFrequencyName = pObjPaymentFrequencyModel.PaymentFrequencyName.Trim(),                    
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLPaymentFrequency().BL_InsertUpdatePaymentFrequency(mObjPaymentFrequency);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "PaymentFrequency");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjPaymentFrequencyModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving payment frequency";
                    return View(pObjPaymentFrequencyModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Payment_Frequency mObjPaymentFrequency = new Payment_Frequency()
                {
                    PaymentFrequencyID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetPaymentFrequencyList_Result mObjPaymentFrequencyData = new BLPaymentFrequency().BL_GetPaymentFrequencyDetails(mObjPaymentFrequency);

                if (mObjPaymentFrequencyData != null)
                {
                    PaymentFrequencyViewModel mObjPaymentFrequencyModelView = new PaymentFrequencyViewModel()
                    {
                        PaymentFrequencyID = mObjPaymentFrequencyData.PaymentFrequencyID.GetValueOrDefault(),
                        PaymentFrequencyName = mObjPaymentFrequencyData.PaymentFrequencyName,
                        Active = mObjPaymentFrequencyData.Active.GetValueOrDefault(),
                    };

                    return View(mObjPaymentFrequencyModelView);
                }
                else
                {
                    return RedirectToAction("List", "PaymentFrequency");
                }
            }
            else
            {
                return RedirectToAction("List", "PaymentFrequency");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(PaymentFrequencyViewModel pObjPaymentFrequencyModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjPaymentFrequencyModel);
            }
            else
            {
                Payment_Frequency mObjPaymentFrequency = new Payment_Frequency()
                {
                    PaymentFrequencyID = pObjPaymentFrequencyModel.PaymentFrequencyID,
                    PaymentFrequencyName = pObjPaymentFrequencyModel.PaymentFrequencyName.Trim(),
                    Active = pObjPaymentFrequencyModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLPaymentFrequency().BL_InsertUpdatePaymentFrequency(mObjPaymentFrequency);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "PaymentFrequency");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjPaymentFrequencyModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving payment frequency";
                    return View(pObjPaymentFrequencyModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Payment_Frequency mObjPaymentFrequency = new Payment_Frequency()
                {
                    PaymentFrequencyID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetPaymentFrequencyList_Result mObjPaymentFrequencyData = new BLPaymentFrequency().BL_GetPaymentFrequencyDetails(mObjPaymentFrequency);

                if (mObjPaymentFrequencyData != null)
                {
                    PaymentFrequencyViewModel mObjPaymentFrequencyModelView = new PaymentFrequencyViewModel()
                    {
                        PaymentFrequencyID = mObjPaymentFrequencyData.PaymentFrequencyID.GetValueOrDefault(),
                        PaymentFrequencyName = mObjPaymentFrequencyData.PaymentFrequencyName,
                        ActiveText = mObjPaymentFrequencyData.ActiveText
                    };

                    return View(mObjPaymentFrequencyModelView);
                }
                else
                {
                    return RedirectToAction("List", "PaymentFrequency");
                }
            }
            else
            {
                return RedirectToAction("List", "PaymentFrequency");
            }
        }

        public JsonResult UpdateStatus(Payment_Frequency pObjPaymentFrequencyData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjPaymentFrequencyData.PaymentFrequencyID != 0)
            {
                FuncResponse mObjFuncResponse = new BLPaymentFrequency().BL_UpdateStatus(pObjPaymentFrequencyData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["PaymentFrequencyList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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