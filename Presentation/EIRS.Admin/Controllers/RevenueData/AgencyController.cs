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
    public class AgencyController : BaseController
    {
        public ActionResult List()
        {
            Agency mObjAgency = new Agency()
            {
                intStatus = 2
            };

            IList<usp_GetAgencyList_Result> lstAgency = new BLAgency().BL_GetAgencyList(mObjAgency);
            return View(lstAgency);
        }

        public ActionResult Add()
        {
            UI_FillAgencyTypeDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(AgencyViewModel pObjAgencyModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillAgencyTypeDropDown();
                return View(pObjAgencyModel);
            }
            else
            {
                Agency mObjAgency = new Agency()
                {
                    AgencyID = 0,
                    AgencyName = pObjAgencyModel.AgencyName.Trim(),                    
                    AgencyTypeID = pObjAgencyModel.AgencyTypeID,
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLAgency().BL_InsertUpdateAgency(mObjAgency);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "Agency");
                    }
                    else
                    {
                        UI_FillAgencyTypeDropDown();
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjAgencyModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillAgencyTypeDropDown();
                    ViewBag.Message = "Error occurred while saving agency";
                    return View(pObjAgencyModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Agency mObjAgency = new Agency()
                {
                    AgencyID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetAgencyList_Result mObjAgencyData = new BLAgency().BL_GetAgencyDetails(mObjAgency);

                if (mObjAgencyData != null)
                {
                    AgencyViewModel mObjAgencyModelView = new AgencyViewModel()
                    {
                        AgencyID = mObjAgencyData.AgencyID.GetValueOrDefault(),
                        AgencyName = mObjAgencyData.AgencyName,
                        AgencyTypeID = mObjAgencyData.AgencyTypeID.GetValueOrDefault(),
                        Active = mObjAgencyData.Active.GetValueOrDefault(),
                    };

                    UI_FillAgencyTypeDropDown(new Agency_Types() { intStatus = 1, IncludeAgencyTypeIds = mObjAgencyModelView.AgencyTypeID.ToString() });
                    return View(mObjAgencyModelView);
                }
                else
                {
                    return RedirectToAction("List", "Agency");
                }
            }
            else
            {
                return RedirectToAction("List", "Agency");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(AgencyViewModel pObjAgencyModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillAgencyTypeDropDown(new Agency_Types() { intStatus = 1, IncludeAgencyTypeIds = pObjAgencyModel.AgencyTypeID.ToString() });
                return View(pObjAgencyModel);
            }
            else
            {
                Agency mObjAgency = new Agency()
                {
                    AgencyID = pObjAgencyModel.AgencyID,
                    AgencyName = pObjAgencyModel.AgencyName.Trim(),
                    AgencyTypeID = pObjAgencyModel.AgencyTypeID,
                    Active = pObjAgencyModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLAgency().BL_InsertUpdateAgency(mObjAgency);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "Agency");
                    }
                    else
                    {
                        UI_FillAgencyTypeDropDown(new Agency_Types() { intStatus = 1, IncludeAgencyTypeIds = pObjAgencyModel.AgencyTypeID.ToString() });
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjAgencyModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillAgencyTypeDropDown(new Agency_Types() { intStatus = 1, IncludeAgencyTypeIds = pObjAgencyModel.AgencyTypeID.ToString() });
                    ViewBag.Message = "Error occurred while saving agency";
                    return View(pObjAgencyModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Agency mObjAgency = new Agency()
                {
                    AgencyID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetAgencyList_Result mObjAgencyData = new BLAgency().BL_GetAgencyDetails(mObjAgency);

                if (mObjAgencyData != null)
                {
                    AgencyViewModel mObjAgencyModelView = new AgencyViewModel()
                    {
                        AgencyID = mObjAgencyData.AgencyID.GetValueOrDefault(),
                        AgencyName = mObjAgencyData.AgencyName,
                        AgencyTypeName = mObjAgencyData.AgencyTypeName,
                        ActiveText = mObjAgencyData.ActiveText
                    };

                    return View(mObjAgencyModelView);
                }
                else
                {
                    return RedirectToAction("List", "Agency");
                }
            }
            else
            {
                return RedirectToAction("List", "Agency");
            }
        }

        public JsonResult UpdateStatus(Agency pObjAgencyData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjAgencyData.AgencyID != 0)
            {
                FuncResponse mObjFuncResponse = new BLAgency().BL_UpdateStatus(pObjAgencyData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["AgencyList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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