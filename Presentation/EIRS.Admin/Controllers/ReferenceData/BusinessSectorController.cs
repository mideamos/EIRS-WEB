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
    public class BusinessSectorController : BaseController
    {
        public ActionResult List()
        {
            Business_Sector mObjBusinessSector = new Business_Sector()
            {
                intStatus = 2
            };

            IList<usp_GetBusinessSectorList_Result> lstBusinessSector = new BLBusinessSector().BL_GetBusinessSectorList(mObjBusinessSector);
            return View(lstBusinessSector);
        }

        public ActionResult Add()
        {
            UI_FillBusinessTypeDropDown();
            UI_FillBusinessCategoryDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(BusinessSectorViewModel pObjBusinessSectorModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillBusinessTypeDropDown();
                UI_FillBusinessCategoryDropDown(new Business_Category() { intStatus = 1, BusinessTypeID = pObjBusinessSectorModel.BusinessTypeID });
                return View(pObjBusinessSectorModel);
            }
            else
            {
                Business_Sector mObjBusinessSector = new Business_Sector()
                {
                    BusinessSectorID = 0,
                    BusinessSectorName = pObjBusinessSectorModel.BusinessSectorName.Trim(), 
                    BusinessTypeID = pObjBusinessSectorModel.BusinessTypeID,  
                    BusinessCategoryID = pObjBusinessSectorModel.BusinessCategoryID,                 
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLBusinessSector().BL_InsertUpdateBusinessSector(mObjBusinessSector);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "BusinessSector");
                    }
                    else
                    {
                        UI_FillBusinessTypeDropDown();
                        UI_FillBusinessCategoryDropDown(new Business_Category() { intStatus = 1, BusinessTypeID = pObjBusinessSectorModel.BusinessTypeID });
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjBusinessSectorModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillBusinessTypeDropDown();
                    UI_FillBusinessCategoryDropDown(new Business_Category() { intStatus = 1, BusinessTypeID = pObjBusinessSectorModel.BusinessTypeID });
                    ViewBag.Message = "Error occurred while saving business sector";
                    return View(pObjBusinessSectorModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Business_Sector mObjBusinessSector = new Business_Sector()
                {
                    BusinessSectorID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetBusinessSectorList_Result mObjBusinessSectorData = new BLBusinessSector().BL_GetBusinessSectorDetails(mObjBusinessSector);

                if (mObjBusinessSectorData != null)
                {
                    BusinessSectorViewModel mObjBusinessSectorModelView = new BusinessSectorViewModel()
                    {
                        BusinessSectorID = mObjBusinessSectorData.BusinessSectorID.GetValueOrDefault(),
                        BusinessSectorName = mObjBusinessSectorData.BusinessSectorName,
                        BusinessTypeID = mObjBusinessSectorData.BusinessTypeID.GetValueOrDefault(),
                        BusinessCategoryID = mObjBusinessSectorData.BusinessCategoryID.GetValueOrDefault(),
                        Active = mObjBusinessSectorData.Active.GetValueOrDefault(),
                    };

                    UI_FillBusinessTypeDropDown(new Business_Types() { intStatus = 1, IncludeBusinessTypeIds = mObjBusinessSectorModelView.BusinessTypeID.ToString() });
                    UI_FillBusinessCategoryDropDown(new Business_Category() { intStatus = 1, BusinessTypeID = mObjBusinessSectorModelView.BusinessTypeID, IncludeBusinessCategoryIds = mObjBusinessSectorModelView.BusinessCategoryID.ToString() });

                    return View(mObjBusinessSectorModelView);
                }
                else
                {
                    return RedirectToAction("List", "BusinessSector");
                }
            }
            else
            {
                return RedirectToAction("List", "BusinessSector");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(BusinessSectorViewModel pObjBusinessSectorModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillBusinessTypeDropDown(new Business_Types() { intStatus = 1, IncludeBusinessTypeIds = pObjBusinessSectorModel.BusinessTypeID.ToString() });
                UI_FillBusinessCategoryDropDown(new Business_Category() { intStatus = 1, BusinessTypeID = pObjBusinessSectorModel.BusinessTypeID, IncludeBusinessCategoryIds = pObjBusinessSectorModel.BusinessCategoryID.ToString() });

                return View(pObjBusinessSectorModel);
            }
            else
            {
                Business_Sector mObjBusinessSector = new Business_Sector()
                {
                    BusinessSectorID = pObjBusinessSectorModel.BusinessSectorID,
                    BusinessSectorName = pObjBusinessSectorModel.BusinessSectorName.Trim(),
                    BusinessTypeID = pObjBusinessSectorModel.BusinessTypeID,
                    BusinessCategoryID = pObjBusinessSectorModel.BusinessCategoryID,
                    Active = pObjBusinessSectorModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLBusinessSector().BL_InsertUpdateBusinessSector(mObjBusinessSector);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "BusinessSector");
                    }
                    else
                    {
                        UI_FillBusinessTypeDropDown(new Business_Types() { intStatus = 1, IncludeBusinessTypeIds = pObjBusinessSectorModel.BusinessTypeID.ToString() });
                        UI_FillBusinessCategoryDropDown(new Business_Category() { intStatus = 1, BusinessTypeID = pObjBusinessSectorModel.BusinessTypeID, IncludeBusinessCategoryIds = pObjBusinessSectorModel.BusinessCategoryID.ToString() });
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjBusinessSectorModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillBusinessTypeDropDown(new Business_Types() { intStatus = 1, IncludeBusinessTypeIds = pObjBusinessSectorModel.BusinessTypeID.ToString() });
                    UI_FillBusinessCategoryDropDown(new Business_Category() { intStatus = 1, BusinessTypeID = pObjBusinessSectorModel.BusinessTypeID, IncludeBusinessCategoryIds = pObjBusinessSectorModel.BusinessCategoryID.ToString() });
                    ViewBag.Message = "Error occurred while saving business sector";
                    return View(pObjBusinessSectorModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Business_Sector mObjBusinessSector = new Business_Sector()
                {
                    BusinessSectorID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetBusinessSectorList_Result mObjBusinessSectorData = new BLBusinessSector().BL_GetBusinessSectorDetails(mObjBusinessSector);

                if (mObjBusinessSectorData != null)
                {
                    BusinessSectorViewModel mObjBusinessSectorModelView = new BusinessSectorViewModel()
                    {
                        BusinessSectorID = mObjBusinessSectorData.BusinessSectorID.GetValueOrDefault(),
                        BusinessSectorName = mObjBusinessSectorData.BusinessSectorName,
                        BusinessTypeName = mObjBusinessSectorData.BusinessTypeName,
                        BusinessCategoryName = mObjBusinessSectorData.BusinessCategoryName,
                        ActiveText = mObjBusinessSectorData.ActiveText
                    };

                    return View(mObjBusinessSectorModelView);
                }
                else
                {
                    return RedirectToAction("List", "BusinessSector");
                }
            }
            else
            {
                return RedirectToAction("List", "BusinessSector");
            }
        }

        public JsonResult UpdateStatus(Business_Sector pObjBusinessSectorData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjBusinessSectorData.BusinessSectorID != 0)
            {
                FuncResponse mObjFuncResponse = new BLBusinessSector().BL_UpdateStatus(pObjBusinessSectorData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["BusinessSectorList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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