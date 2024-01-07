using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Vereyon.Web;
using Elmah;
using EIRS.Web.Utility;

namespace EIRS.Web.Controllers
{
    public class EMBankController : BaseController
    {
        // GET: EMBank
        [HttpGet]
        public ActionResult List()
        {
            EM_Bank mObjBank = new EM_Bank()
            {
                intStatus = 2
            };

            IList<usp_EM_GetBankList_Result> lstBank = new BLEMBank().BL_GetBankList(mObjBank);
            return View(lstBank);
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(EMBankViewModel pObjBankModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjBankModel);
            }
            else
            {
                EM_Bank mObjBank = new EM_Bank()
                {
                    BankID = 0,
                    BankName = pObjBankModel.BankName.Trim(),
                    BankAccountNumber = pObjBankModel.BankAccountNumber,
                    BankDescription = pObjBankModel.BankDescription,
                    Active = true,
                    CreatedBy = SessionManager.UserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLEMBank().BL_InsertUpdateBank(mObjBank);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "EMBank");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjBankModel);
                    }
                }
                catch (Exception ex)
                {
                    Logger.SendErrorToText(ex);
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving Bank";
                    return View(pObjBankModel);
                }
            }
        }

        [HttpGet]
        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                EM_Bank mObjBank = new EM_Bank()
                {
                    BankID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_EM_GetBankList_Result mObjBankData = new BLEMBank().BL_GetBankDetails(mObjBank);

                if (mObjBankData != null)
                {
                    EMBankViewModel mObjBankModelView = new EMBankViewModel()
                    {
                        BankID = mObjBankData.BankID.GetValueOrDefault(),
                        BankName = mObjBankData.BankName,
                        BankAccountNumber = mObjBankData.BankAccountNumber,
                        BankDescription = mObjBankData.BankDescription,
                        Active = mObjBankData.Active.GetValueOrDefault(),
                    };

                    return View(mObjBankModelView);
                }
                else
                {
                    return RedirectToAction("List", "EMBank");
                }
            }
            else
            {
                return RedirectToAction("List", "EMBank");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(EMBankViewModel pObjBankModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjBankModel);
            }
            else
            {
                EM_Bank mObjBank = new EM_Bank()
                {
                    BankID = pObjBankModel.BankID,
                    BankName = pObjBankModel.BankName.Trim(),
                    BankAccountNumber = pObjBankModel.BankAccountNumber,
                    BankDescription = pObjBankModel.BankDescription,
                    Active = pObjBankModel.Active,
                    ModifiedBy = SessionManager.UserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLEMBank().BL_InsertUpdateBank(mObjBank);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "EMBank");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjBankModel);
                    }
                }
                catch (Exception ex)
                {
                    Logger.SendErrorToText(ex);
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving Bank";
                    return View(pObjBankModel);
                }
            }
        }

        [HttpGet]
        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                EM_Bank mObjBank = new EM_Bank()
                {
                    BankID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_EM_GetBankList_Result mObjBankData = new BLEMBank().BL_GetBankDetails(mObjBank);

                if (mObjBankData != null)
                {
                    return View(mObjBankData);
                }
                else
                {
                    return RedirectToAction("List", "EMBank");
                }
            }
            else
            {
                return RedirectToAction("List", "EMBank");
            }
        }

        public JsonResult UpdateStatus(EM_Bank pObjBankData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjBankData.BankID != 0)
            {
                FuncResponse mObjFuncResponse = new BLEMBank().BL_UpdateStatus(pObjBankData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["BankList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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