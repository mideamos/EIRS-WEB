using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Vereyon.Web;
using Elmah;

namespace EIRS.Admin.Controllers.ReferenceData
{
    public class ScratchCardPrinterController : BaseController
    {
        // GET: ScratchCardPrinter
        public ActionResult List()
        {
            Scratch_Card_Printer mObjScratchCardPrinter = new Scratch_Card_Printer()
            {
                intStatus = 2
            };

            IList<usp_GetScratchCardPrinterList_Result> lstScratchCardPrinter = new BLScratchCardPrinter().BL_GetScratchCardPrinterList(mObjScratchCardPrinter);
            return View(lstScratchCardPrinter);
        }

        public JsonResult UpdateStatus(Scratch_Card_Printer pObjScratchCardPrinterData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjScratchCardPrinterData.ScratchCardPrinterID != 0)
            {
                FuncResponse mObjFuncResponse = new BLScratchCardPrinter().BL_UpdateStatus(pObjScratchCardPrinterData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["ScratchCardPrinterList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
                }
            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Action";
            }
            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add()
        {
            UI_FillCompanyDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(ScratchCardPrinterViewModel pObjScratchCardPrinterModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillCompanyDropDown();
                return View(pObjScratchCardPrinterModel);
            }
            else
            {
                Scratch_Card_Printer mObjScratchCardPrinter = new Scratch_Card_Printer()
                {
                    ScratchCardPrinterID = 0,
                    ScratchCardPrinterName = pObjScratchCardPrinterModel.ScratchCardPrinterName.Trim(),
                    CompanyID = pObjScratchCardPrinterModel.CompanyID,
                    AgreedUnitPrice=pObjScratchCardPrinterModel.AgreedUnitPrice,
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLScratchCardPrinter().BL_InsertUpdateScratchCardPrinter(mObjScratchCardPrinter);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "ScratchCardPrinter");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        UI_FillCompanyDropDown();
                        return View(pObjScratchCardPrinterModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving Scratch Card Printer";
                    UI_FillCompanyDropDown();
                    return View(pObjScratchCardPrinterModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Scratch_Card_Printer mObjScratchCardPrinter = new Scratch_Card_Printer()
                {
                    ScratchCardPrinterID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetScratchCardPrinterList_Result mObjScratchCardPrinterData = new BLScratchCardPrinter().BL_GetScratchCardPrinterDetails(mObjScratchCardPrinter);

                if (mObjScratchCardPrinterData != null)
                {
                    ScratchCardPrinterViewModel mObjScratchCardPrinterModelView = new ScratchCardPrinterViewModel()
                    {
                        ScratchCardPrinterID = mObjScratchCardPrinterData.ScratchCardPrinterID.GetValueOrDefault(),
                        ScratchCardPrinterName = mObjScratchCardPrinterData.ScratchCardPrinterName,
                        CompanyID = mObjScratchCardPrinterData.CompanyID.GetValueOrDefault(),
                        AgreedUnitPrice=mObjScratchCardPrinterData.AgreedUnitPrice.GetValueOrDefault(),
                        Active = mObjScratchCardPrinterData.Active.GetValueOrDefault(),
                    };
                    UI_FillCompanyDropDown();
                    return View(mObjScratchCardPrinterModelView);
                }
                else
                {
                    return RedirectToAction("List", "ScratchCardPrinter");
                }
            }
            else
            {
                return RedirectToAction("List", "ScratchCardPrinter");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(ScratchCardPrinterViewModel pObjScratchCardPrinterModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjScratchCardPrinterModel);
            }
            else
            {
                Scratch_Card_Printer mObjScratchCardPrinter = new Scratch_Card_Printer()
                {
                    ScratchCardPrinterID = pObjScratchCardPrinterModel.ScratchCardPrinterID,
                    ScratchCardPrinterName = pObjScratchCardPrinterModel.ScratchCardPrinterName.Trim(),
                    CompanyID = pObjScratchCardPrinterModel.CompanyID,
                    AgreedUnitPrice=pObjScratchCardPrinterModel.AgreedUnitPrice,
                    Active = pObjScratchCardPrinterModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLScratchCardPrinter().BL_InsertUpdateScratchCardPrinter(mObjScratchCardPrinter);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "ScratchCardPrinter");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        UI_FillCompanyDropDown();
                        return View(pObjScratchCardPrinterModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving Scratch Card Printer";
                    UI_FillCompanyDropDown();
                    return View(pObjScratchCardPrinterModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Scratch_Card_Printer mObjScratchCardPrinter = new Scratch_Card_Printer()
                {
                    ScratchCardPrinterID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetScratchCardPrinterList_Result mObjScratchCardPrinterData = new BLScratchCardPrinter().BL_GetScratchCardPrinterDetails(mObjScratchCardPrinter);

                if (mObjScratchCardPrinterData != null)
                {
                    ScratchCardPrinterViewModel mObjScratchCardPrinterModelView = new ScratchCardPrinterViewModel()
                    {
                        ScratchCardPrinterID = mObjScratchCardPrinterData.ScratchCardPrinterID.GetValueOrDefault(),
                        ScratchCardPrinterName = mObjScratchCardPrinterData.ScratchCardPrinterName,
                        CompanyID = mObjScratchCardPrinterData.CompanyID.GetValueOrDefault(),
                        CompanyName=mObjScratchCardPrinterData.CompanyName,
                        AgreedUnitPrice=mObjScratchCardPrinterData.AgreedUnitPrice.GetValueOrDefault(),
                        ActiveText = mObjScratchCardPrinterData.ActiveText
                    };

                    return View(mObjScratchCardPrinterModelView);
                }
                else
                {
                    return RedirectToAction("List", "ScratchCardPrinter");
                }
            }
            else
            {
                return RedirectToAction("List", "ScratchCardPrinter");
            }
        }
    }
}