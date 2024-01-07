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
    public class TaxPayerRoleController : BaseController
    {
        public ActionResult List()
        {
            TaxPayer_Roles mObjTaxPayerRole = new TaxPayer_Roles()
            {
                intStatus = 2
            };

            IList<usp_GetTaxPayerRoleList_Result> lstTaxPayerRole = new BLTaxPayerRole().BL_GetTaxPayerRoleList(mObjTaxPayerRole);
            return View(lstTaxPayerRole);
        }

        public ActionResult Add()
        {
            UI_FillAssetTypeDropDown();
            UI_FillTaxPayerTypeDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(TaxPayerRoleViewModel pObjTaxPayerRoleModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillAssetTypeDropDown();
                UI_FillTaxPayerTypeDropDown();
                return View(pObjTaxPayerRoleModel);
            }
            else
            {
                TaxPayer_Roles mObjTaxPayerRole = new TaxPayer_Roles()
                {
                    TaxPayerRoleID = 0,
                    TaxPayerRoleName = pObjTaxPayerRoleModel.TaxPayerRoleName.Trim(),
                    TaxPayerTypeID = pObjTaxPayerRoleModel.TaxPayerTypeID,
                    AssetTypeID = pObjTaxPayerRoleModel.AssetTypeID,
                    IsMultiLinkable = pObjTaxPayerRoleModel.isMultiLinkable,
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLTaxPayerRole().BL_InsertUpdateTaxPayerRole(mObjTaxPayerRole);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "TaxPayerRole");
                    }
                    else
                    {
                        UI_FillAssetTypeDropDown();
                        UI_FillTaxPayerTypeDropDown();
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjTaxPayerRoleModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillAssetTypeDropDown();
                    UI_FillTaxPayerTypeDropDown();
                    ViewBag.Message = "Error occurred while saving tax payer role";
                    return View(pObjTaxPayerRoleModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                TaxPayer_Roles mObjTaxPayerRole = new TaxPayer_Roles()
                {
                    TaxPayerRoleID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetTaxPayerRoleList_Result mObjTaxPayerRoleData = new BLTaxPayerRole().BL_GetTaxPayerRoleDetails(mObjTaxPayerRole);

                if (mObjTaxPayerRoleData != null)
                {
                    TaxPayerRoleViewModel mObjTaxPayerRoleModelView = new TaxPayerRoleViewModel()
                    {
                        TaxPayerRoleID = mObjTaxPayerRoleData.TaxPayerRoleID.GetValueOrDefault(),
                        TaxPayerRoleName = mObjTaxPayerRoleData.TaxPayerRoleName,
                        AssetTypeID = mObjTaxPayerRoleData.AssetTypeID.GetValueOrDefault(),
                        TaxPayerTypeID = mObjTaxPayerRoleData.TaxPayerTypeID.GetValueOrDefault(),
                        isMultiLinkable = mObjTaxPayerRoleData.IsMultiLinkable.GetValueOrDefault(),
                        Active = mObjTaxPayerRoleData.Active.GetValueOrDefault(),
                    };


                    UI_FillAssetTypeDropDown(new Asset_Types() { IncludeAssetTypeIds = mObjTaxPayerRoleModelView.AssetTypeID.ToString(), intStatus = 1 });
                    UI_FillTaxPayerTypeDropDown(new TaxPayer_Types() { IncludeTaxPayerTypeIds = mObjTaxPayerRoleModelView.TaxPayerTypeID.ToString(), intStatus = 1 });

                    return View(mObjTaxPayerRoleModelView);
                }
                else
                {
                    return RedirectToAction("List", "TaxPayerRole");
                }
            }
            else
            {
                return RedirectToAction("List", "TaxPayerRole");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(TaxPayerRoleViewModel pObjTaxPayerRoleModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillAssetTypeDropDown(new Asset_Types() { IncludeAssetTypeIds = pObjTaxPayerRoleModel.AssetTypeID.ToString(), intStatus = 1 });
                UI_FillTaxPayerTypeDropDown(new TaxPayer_Types() { IncludeTaxPayerTypeIds = pObjTaxPayerRoleModel.TaxPayerTypeID.ToString(), intStatus = 1 });
                return View(pObjTaxPayerRoleModel);
            }
            else
            {
                TaxPayer_Roles mObjTaxPayerRole = new TaxPayer_Roles()
                {
                    TaxPayerRoleID = pObjTaxPayerRoleModel.TaxPayerRoleID,
                    TaxPayerRoleName = pObjTaxPayerRoleModel.TaxPayerRoleName.Trim(),
                    TaxPayerTypeID = pObjTaxPayerRoleModel.TaxPayerTypeID,
                    AssetTypeID = pObjTaxPayerRoleModel.AssetTypeID,
                    IsMultiLinkable = pObjTaxPayerRoleModel.isMultiLinkable,
                    Active = pObjTaxPayerRoleModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLTaxPayerRole().BL_InsertUpdateTaxPayerRole(mObjTaxPayerRole);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "TaxPayerRole");
                    }
                    else
                    {
                        UI_FillAssetTypeDropDown(new Asset_Types() { IncludeAssetTypeIds = pObjTaxPayerRoleModel.AssetTypeID.ToString(), intStatus = 1 });
                        UI_FillTaxPayerTypeDropDown(new TaxPayer_Types() { IncludeTaxPayerTypeIds = pObjTaxPayerRoleModel.TaxPayerTypeID.ToString(), intStatus = 1 });

                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjTaxPayerRoleModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillAssetTypeDropDown(new Asset_Types() { IncludeAssetTypeIds = pObjTaxPayerRoleModel.AssetTypeID.ToString(), intStatus = 1 });
                    UI_FillTaxPayerTypeDropDown(new TaxPayer_Types() { IncludeTaxPayerTypeIds = pObjTaxPayerRoleModel.TaxPayerTypeID.ToString(), intStatus = 1 });
                    ViewBag.Message = "Error occurred while saving tax payer role";
                    return View(pObjTaxPayerRoleModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                TaxPayer_Roles mObjTaxPayerRole = new TaxPayer_Roles()
                {
                    TaxPayerRoleID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetTaxPayerRoleList_Result mObjTaxPayerRoleData = new BLTaxPayerRole().BL_GetTaxPayerRoleDetails(mObjTaxPayerRole);

                if (mObjTaxPayerRoleData != null)
                {
                    TaxPayerRoleViewModel mObjTaxPayerRoleModelView = new TaxPayerRoleViewModel()
                    {
                        TaxPayerRoleID = mObjTaxPayerRoleData.TaxPayerRoleID.GetValueOrDefault(),
                        TaxPayerRoleName = mObjTaxPayerRoleData.TaxPayerRoleName,
                        AssetTypeName = mObjTaxPayerRoleData.AssetTypeName,
                        TaxPayerTypeName = mObjTaxPayerRoleData.TaxPayerTypeName,
                        isMultiLinkableText = mObjTaxPayerRoleData.IsMultiLinkableText,
                        ActiveText = mObjTaxPayerRoleData.ActiveText
                    };

                    return View(mObjTaxPayerRoleModelView);
                }
                else
                {
                    return RedirectToAction("List", "TaxPayerRole");
                }
            }
            else
            {
                return RedirectToAction("List", "TaxPayerRole");
            }
        }

        public JsonResult UpdateStatus(TaxPayer_Roles pObjTaxPayerRoleData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjTaxPayerRoleData.TaxPayerRoleID != 0)
            {
                FuncResponse mObjFuncResponse = new BLTaxPayerRole().BL_UpdateStatus(pObjTaxPayerRoleData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["TaxPayerRoleList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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