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
    public class ScratchCardDealerController : BaseController
    {
        // GET: ScratchCardDealer
        public ActionResult List()
        {
            Scratch_Card_Dealers mObjScratchCardDealer = new Scratch_Card_Dealers()
            {
                intStatus = 2
            };

            IList<usp_GetScratchCardDealerList_Result> lstScratchCardDealer = new BLScratchCardDealer().BL_GetScratchCardDealerList(mObjScratchCardDealer);
            return View(lstScratchCardDealer);
        }

        public JsonResult UpdateStatus(Scratch_Card_Dealers pObjScratchCardDealerData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjScratchCardDealerData.ScratchCardDealerID != 0)
            {
                FuncResponse mObjFuncResponse = new BLScratchCardDealer().BL_UpdateStatus(pObjScratchCardDealerData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["ScratchCardDealerList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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
            UI_FillDealerType();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(ScratchCardDealerViewModel pObjScratchCardDealerModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillCompanyDropDown();
                UI_FillDealerType();
                return View(pObjScratchCardDealerModel);
            }
            else
            {
                Scratch_Card_Dealers mObjScratchCardDealer = new Scratch_Card_Dealers()
                {
                    ScratchCardDealerID = 0,
                    ScratchCardDealerName = pObjScratchCardDealerModel.ScratchCardDealerName.Trim(),
                    CompanyID = pObjScratchCardDealerModel.CompanyID,
                    DealerTypeID=pObjScratchCardDealerModel.DealerTypeID,
                    AgreedCommissionPercentage = pObjScratchCardDealerModel.AgreedCommissionPercentage,
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {
                    FuncResponse mObjResponse = new BLScratchCardDealer().BL_InsertUpdateScratchCardDealer(mObjScratchCardDealer);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "ScratchCardDealer");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        UI_FillCompanyDropDown();
                        UI_FillDealerType();
                        return View(pObjScratchCardDealerModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving Scratch Card Dealer";
                    UI_FillCompanyDropDown();
                    UI_FillDealerType();
                    return View(pObjScratchCardDealerModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Scratch_Card_Dealers mObjScratchCardDealer = new Scratch_Card_Dealers()
                {
                    ScratchCardDealerID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetScratchCardDealerList_Result mObjScratchCardDealerData = new BLScratchCardDealer().BL_GetScratchCardDealerDetails(mObjScratchCardDealer);

                if (mObjScratchCardDealerData != null)
                {
                    ScratchCardDealerViewModel mObjScratchCardDealerModelView = new ScratchCardDealerViewModel()
                    {
                        ScratchCardDealerID = mObjScratchCardDealerData.ScratchCardDealerID.GetValueOrDefault(),
                        ScratchCardDealerName = mObjScratchCardDealerData.ScratchCardDealerName,
                        CompanyID = mObjScratchCardDealerData.CompanyID.GetValueOrDefault(),
                        DealerTypeID=mObjScratchCardDealerData.DealerTypeID.GetValueOrDefault(),
                        AgreedCommissionPercentage = mObjScratchCardDealerData.AgreedCommissionPercentage.GetValueOrDefault(),
                        Active = mObjScratchCardDealerData.Active.GetValueOrDefault(),
                    };
                    UI_FillCompanyDropDown();
                    UI_FillDealerType();
                    return View(mObjScratchCardDealerModelView);
                }
                else
                {
                    return RedirectToAction("List", "ScratchCardDealer");
                }
            }
            else
            {
                return RedirectToAction("List", "ScratchCardDealer");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(ScratchCardDealerViewModel pObjScratchCardDealerModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjScratchCardDealerModel);
            }
            else
            {
                Scratch_Card_Dealers mObjScratchCardDealer = new Scratch_Card_Dealers()
                {
                    ScratchCardDealerID = pObjScratchCardDealerModel.ScratchCardDealerID,
                    ScratchCardDealerName = pObjScratchCardDealerModel.ScratchCardDealerName.Trim(),
                    CompanyID = pObjScratchCardDealerModel.CompanyID,
                    DealerTypeID=pObjScratchCardDealerModel.DealerTypeID,
                    AgreedCommissionPercentage = pObjScratchCardDealerModel.AgreedCommissionPercentage,
                    Active = pObjScratchCardDealerModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLScratchCardDealer().BL_InsertUpdateScratchCardDealer(mObjScratchCardDealer);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "ScratchCardDealer");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        UI_FillCompanyDropDown();
                        UI_FillDealerType();
                        return View(pObjScratchCardDealerModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving Scratch Card Dealer";
                    UI_FillCompanyDropDown();
                    UI_FillDealerType();
                    return View(pObjScratchCardDealerModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Scratch_Card_Dealers mObjScratchCardDealer = new Scratch_Card_Dealers()
                {
                    ScratchCardDealerID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetScratchCardDealerList_Result mObjScratchCardDealerData = new BLScratchCardDealer().BL_GetScratchCardDealerDetails(mObjScratchCardDealer);

                if (mObjScratchCardDealerData != null)
                {
                    ScratchCardDealerViewModel mObjScratchCardDealerModelView = new ScratchCardDealerViewModel()
                    {
                        ScratchCardDealerID = mObjScratchCardDealerData.ScratchCardDealerID.GetValueOrDefault(),
                        ScratchCardDealerName = mObjScratchCardDealerData.ScratchCardDealerName,
                        CompanyID = mObjScratchCardDealerData.CompanyID.GetValueOrDefault(),
                        CompanyName = mObjScratchCardDealerData.CompanyName,
                        DealerTypeID = mObjScratchCardDealerData.DealerTypeID.GetValueOrDefault(),
                        DealerTypeName = mObjScratchCardDealerData.DealerTypeName,
                        AgreedCommissionPercentage = mObjScratchCardDealerData.AgreedCommissionPercentage.GetValueOrDefault(),
                        ActiveText = mObjScratchCardDealerData.ActiveText
                    };

                    return View(mObjScratchCardDealerModelView);
                }
                else
                {
                    return RedirectToAction("List", "ScratchCardDealer");
                }
            }
            else
            {
                return RedirectToAction("List", "ScratchCardDealer");
            }
        }
    }
}