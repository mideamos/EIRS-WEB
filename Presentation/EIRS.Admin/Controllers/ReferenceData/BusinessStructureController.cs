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
    public class BusinessStructureController : BaseController
    {
        public ActionResult List()
        {
            Business_Structure mObjBusinessStructure = new Business_Structure()
            {
                intStatus = 2
            };

            IList<usp_GetBusinessStructureList_Result> lstBusinessStructure = new BLBusinessStructure().BL_GetBusinessStructureList(mObjBusinessStructure);
            return View(lstBusinessStructure);
        }

        public ActionResult Add()
        {
            UI_FillBusinessTypeDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(BusinessStructureViewModel pObjBusinessStructureModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillBusinessTypeDropDown();
                return View(pObjBusinessStructureModel);
            }
            else
            {
                Business_Structure mObjBusinessStructure = new Business_Structure()
                {
                    BusinessStructureID = 0,
                    BusinessStructureName = pObjBusinessStructureModel.BusinessStructureName.Trim(), 
                    BusinessTypeID = pObjBusinessStructureModel.BusinessTypeID,                   
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLBusinessStructure().BL_InsertUpdateBusinessStructure(mObjBusinessStructure);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "BusinessStructure");
                    }
                    else
                    {
                        UI_FillBusinessTypeDropDown();
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjBusinessStructureModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillBusinessTypeDropDown();
                    ViewBag.Message = "Error occurred while saving business structure";
                    return View(pObjBusinessStructureModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Business_Structure mObjBusinessStructure = new Business_Structure()
                {
                    BusinessStructureID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetBusinessStructureList_Result mObjBusinessStructureData = new BLBusinessStructure().BL_GetBusinessStructureDetails(mObjBusinessStructure);

                if (mObjBusinessStructureData != null)
                {
                    BusinessStructureViewModel mObjBusinessStructureModelView = new BusinessStructureViewModel()
                    {
                        BusinessStructureID = mObjBusinessStructureData.BusinessStructureID.GetValueOrDefault(),
                        BusinessStructureName = mObjBusinessStructureData.BusinessStructureName,
                        BusinessTypeID = mObjBusinessStructureData.BusinessTypeID.GetValueOrDefault(),
                        Active = mObjBusinessStructureData.Active.GetValueOrDefault(),
                    };

                    UI_FillBusinessTypeDropDown(new Business_Types() { intStatus = 1, IncludeBusinessTypeIds = mObjBusinessStructureModelView.BusinessTypeID.ToString() });

                    return View(mObjBusinessStructureModelView);
                }
                else
                {
                    return RedirectToAction("List", "BusinessStructure");
                }
            }
            else
            {
                return RedirectToAction("List", "BusinessStructure");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(BusinessStructureViewModel pObjBusinessStructureModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillBusinessTypeDropDown(new Business_Types() { intStatus = 1, IncludeBusinessTypeIds = pObjBusinessStructureModel.BusinessTypeID.ToString() });
                return View(pObjBusinessStructureModel);
            }
            else
            {
                Business_Structure mObjBusinessStructure = new Business_Structure()
                {
                    BusinessStructureID = pObjBusinessStructureModel.BusinessStructureID,
                    BusinessStructureName = pObjBusinessStructureModel.BusinessStructureName.Trim(),
                    BusinessTypeID = pObjBusinessStructureModel.BusinessTypeID,
                    Active = pObjBusinessStructureModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLBusinessStructure().BL_InsertUpdateBusinessStructure(mObjBusinessStructure);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "BusinessStructure");
                    }
                    else
                    {
                        UI_FillBusinessTypeDropDown(new Business_Types() { intStatus = 1, IncludeBusinessTypeIds = pObjBusinessStructureModel.BusinessTypeID.ToString() });
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjBusinessStructureModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillBusinessTypeDropDown(new Business_Types() { intStatus = 1, IncludeBusinessTypeIds = pObjBusinessStructureModel.BusinessTypeID.ToString() });
                    ViewBag.Message = "Error occurred while saving business structure";
                    return View(pObjBusinessStructureModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Business_Structure mObjBusinessStructure = new Business_Structure()
                {
                    BusinessStructureID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetBusinessStructureList_Result mObjBusinessStructureData = new BLBusinessStructure().BL_GetBusinessStructureDetails(mObjBusinessStructure);

                if (mObjBusinessStructureData != null)
                {
                    BusinessStructureViewModel mObjBusinessStructureModelView = new BusinessStructureViewModel()
                    {
                        BusinessStructureID = mObjBusinessStructureData.BusinessStructureID.GetValueOrDefault(),
                        BusinessStructureName = mObjBusinessStructureData.BusinessStructureName,
                        BusinessTypeName = mObjBusinessStructureData.BusinessTypeName,
                        ActiveText = mObjBusinessStructureData.ActiveText
                    };

                    return View(mObjBusinessStructureModelView);
                }
                else
                {
                    return RedirectToAction("List", "BusinessStructure");
                }
            }
            else
            {
                return RedirectToAction("List", "BusinessStructure");
            }
        }

        public JsonResult UpdateStatus(Business_Structure pObjBusinessStructureData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjBusinessStructureData.BusinessStructureID != 0)
            {
                FuncResponse mObjFuncResponse = new BLBusinessStructure().BL_UpdateStatus(pObjBusinessStructureData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["BusinessStructureList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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