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
    public class BusinessSubSectorController : BaseController
    {
        public ActionResult List()
        {
            Business_SubSector mObjBusinessSubSector = new Business_SubSector()
            {
                intStatus = 2
            };

            IList<usp_GetBusinessSubSectorList_Result> lstBusinessSubSector = new BLBusinessSubSector().BL_GetBusinessSubSectorList(mObjBusinessSubSector);
            return View(lstBusinessSubSector);
        }

        public void UI_FillDropDown(BusinessSubSectorViewModel pObjBusinessSubSectorModel = null)
        {
            if (pObjBusinessSubSectorModel == null)
                pObjBusinessSubSectorModel = new BusinessSubSectorViewModel();

            UI_FillBusinessTypeDropDown(new Business_Types() { intStatus = 1, IncludeBusinessTypeIds = pObjBusinessSubSectorModel.BusinessTypeID.ToString() });
            UI_FillBusinessCategoryDropDown(new Business_Category() { intStatus = 1, IncludeBusinessCategoryIds = pObjBusinessSubSectorModel.BusinessCategoryID.ToString(), BusinessTypeID = pObjBusinessSubSectorModel.BusinessTypeID });
            UI_FillBusinessSectorDropDown(new Business_Sector() { intStatus = 1, IncludeBusinessSectorIds = pObjBusinessSubSectorModel.BusinessSectorID.ToString(), BusinessTypeID = pObjBusinessSubSectorModel.BusinessTypeID });

        }

        public ActionResult Add()
        {
            UI_FillDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(BusinessSubSectorViewModel pObjBusinessSubSectorModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown(pObjBusinessSubSectorModel);
                return View(pObjBusinessSubSectorModel);
            }
            else
            {
                Business_SubSector mObjBusinessSubSector = new Business_SubSector()
                {
                    BusinessSubSectorID = 0,
                    BusinessSubSectorName = pObjBusinessSubSectorModel.BusinessSubSectorName.Trim(), 
                    BusinessTypeID = pObjBusinessSubSectorModel.BusinessTypeID,  
                    BusinessCategoryID = pObjBusinessSubSectorModel.BusinessCategoryID,  
                    BusinessSectorID = pObjBusinessSubSectorModel.BusinessSectorID,               
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLBusinessSubSector().BL_InsertUpdateBusinessSubSector(mObjBusinessSubSector);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "BusinessSubSector");
                    }
                    else
                    {
                        UI_FillDropDown(pObjBusinessSubSectorModel);
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjBusinessSubSectorModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillDropDown(pObjBusinessSubSectorModel);
                    ViewBag.Message = "Error occurred while saving business sub sector";
                    return View(pObjBusinessSubSectorModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Business_SubSector mObjBusinessSubSector = new Business_SubSector()
                {
                    BusinessSubSectorID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetBusinessSubSectorList_Result mObjBusinessSubSectorData = new BLBusinessSubSector().BL_GetBusinessSubSectorDetails(mObjBusinessSubSector);

                if (mObjBusinessSubSectorData != null)
                {
                    BusinessSubSectorViewModel mObjBusinessSubSectorModelView = new BusinessSubSectorViewModel()
                    {
                        BusinessSubSectorID = mObjBusinessSubSectorData.BusinessSubSectorID.GetValueOrDefault(),
                        BusinessSubSectorName = mObjBusinessSubSectorData.BusinessSubSectorName,
                        BusinessTypeID = mObjBusinessSubSectorData.BusinessTypeID.GetValueOrDefault(),
                        BusinessCategoryID = mObjBusinessSubSectorData.BusinessCategoryID.GetValueOrDefault(),
                        BusinessSectorID = mObjBusinessSubSectorData.BusinessSectorID.GetValueOrDefault(),
                        Active = mObjBusinessSubSectorData.Active.GetValueOrDefault(),
                    };

                    UI_FillDropDown(mObjBusinessSubSectorModelView);

                    return View(mObjBusinessSubSectorModelView);
                }
                else
                {
                    return RedirectToAction("List", "BusinessSubSector");
                }
            }
            else
            {
                return RedirectToAction("List", "BusinessSubSector");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(BusinessSubSectorViewModel pObjBusinessSubSectorModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown(pObjBusinessSubSectorModel);
                return View(pObjBusinessSubSectorModel);
            }
            else
            {
                Business_SubSector mObjBusinessSubSector = new Business_SubSector()
                {
                    BusinessSubSectorID = pObjBusinessSubSectorModel.BusinessSubSectorID,
                    BusinessSubSectorName = pObjBusinessSubSectorModel.BusinessSubSectorName.Trim(),
                    BusinessTypeID = pObjBusinessSubSectorModel.BusinessTypeID,
                    BusinessCategoryID = pObjBusinessSubSectorModel.BusinessCategoryID,
                    BusinessSectorID = pObjBusinessSubSectorModel.BusinessSectorID,
                    Active = pObjBusinessSubSectorModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLBusinessSubSector().BL_InsertUpdateBusinessSubSector(mObjBusinessSubSector);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "BusinessSubSector");
                    }
                    else
                    {
                        UI_FillDropDown(pObjBusinessSubSectorModel);
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjBusinessSubSectorModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillDropDown(pObjBusinessSubSectorModel);
                    ViewBag.Message = "Error occurred while saving business sub sector";
                    return View(pObjBusinessSubSectorModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Business_SubSector mObjBusinessSubSector = new Business_SubSector()
                {
                    BusinessSubSectorID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetBusinessSubSectorList_Result mObjBusinessSubSectorData = new BLBusinessSubSector().BL_GetBusinessSubSectorDetails(mObjBusinessSubSector);

                if (mObjBusinessSubSectorData != null)
                {
                    BusinessSubSectorViewModel mObjBusinessSubSectorModelView = new BusinessSubSectorViewModel()
                    {
                        BusinessSubSectorID = mObjBusinessSubSectorData.BusinessSubSectorID.GetValueOrDefault(),
                        BusinessSubSectorName = mObjBusinessSubSectorData.BusinessSubSectorName,
                        BusinessTypeName = mObjBusinessSubSectorData.BusinessTypeName,
                        BusinessCategoryName = mObjBusinessSubSectorData.BusinessCategoryName,
                        BusinessSectorName = mObjBusinessSubSectorData.BusinessSectorName,
                        ActiveText = mObjBusinessSubSectorData.ActiveText
                    };

                    return View(mObjBusinessSubSectorModelView);
                }
                else
                {
                    return RedirectToAction("List", "BusinessSubSector");
                }
            }
            else
            {
                return RedirectToAction("List", "BusinessSubSector");
            }
        }

        public JsonResult UpdateStatus(Business_SubSector pObjBusinessSubSectorData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjBusinessSubSectorData.BusinessSubSectorID != 0)
            {
                FuncResponse mObjFuncResponse = new BLBusinessSubSector().BL_UpdateStatus(pObjBusinessSubSectorData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["BusinessSubSectorList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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