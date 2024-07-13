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
    public class AssessmentGroupController : BaseController
    {
        public ActionResult List()
        {
            Assessment_Group mObjAssessmentGroup = new Assessment_Group()
            {
                intStatus = 2
            };

            IList<usp_GetAssessmentGroupList_Result> lstAssessmentGroup = new BLAssessmentGroup().BL_GetAssessmentGroupList(mObjAssessmentGroup);
            return View(lstAssessmentGroup);
        }

        public ActionResult Add()
        {
            UI_FillAssetTypeDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(AssessmentGroupViewModel pObjAssessmentGroupModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillAssetTypeDropDown();
                return View(pObjAssessmentGroupModel);
            }
            else
            {
                Assessment_Group mObjAssessmentGroup = new Assessment_Group()
                {
                    AssessmentGroupID = 0,
                    AssessmentGroupName = pObjAssessmentGroupModel.AssessmentGroupName.Trim(),  
                    AssetTypeID = pObjAssessmentGroupModel.AssetTypeID,                 
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLAssessmentGroup().BL_InsertUpdateAssessmentGroup(mObjAssessmentGroup);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "AssessmentGroup");
                    }
                    else
                    {
                        UI_FillAssetTypeDropDown();
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjAssessmentGroupModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillAssetTypeDropDown();
                    ViewBag.Message = "Error occurred while saving assessment group";
                    return View(pObjAssessmentGroupModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Assessment_Group mObjAssessmentGroup = new Assessment_Group()
                {
                    AssessmentGroupID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetAssessmentGroupList_Result mObjAssessmentGroupData = new BLAssessmentGroup().BL_GetAssessmentGroupDetails(mObjAssessmentGroup);

                if (mObjAssessmentGroupData != null)
                {
                    AssessmentGroupViewModel mObjAssessmentGroupModelView = new AssessmentGroupViewModel()
                    {
                        AssessmentGroupID = mObjAssessmentGroupData.AssessmentGroupID.GetValueOrDefault(),
                        AssessmentGroupName = mObjAssessmentGroupData.AssessmentGroupName,
                        AssetTypeID = mObjAssessmentGroupData.AssetTypeID.GetValueOrDefault(),
                        Active = mObjAssessmentGroupData.Active.GetValueOrDefault(),
                    };

                    UI_FillAssetTypeDropDown(new Asset_Types() { intStatus = 1, IncludeAssetTypeIds = mObjAssessmentGroupModelView.AssetTypeID.ToString() });
                    return View(mObjAssessmentGroupModelView);
                }
                else
                {
                    return RedirectToAction("List", "AssessmentGroup");
                }
            }
            else
            {
                return RedirectToAction("List", "AssessmentGroup");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(AssessmentGroupViewModel pObjAssessmentGroupModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillAssetTypeDropDown(new Asset_Types() { intStatus = 1, IncludeAssetTypeIds = pObjAssessmentGroupModel.AssetTypeID.ToString() });
                return View(pObjAssessmentGroupModel);
            }
            else
            {
                Assessment_Group mObjAssessmentGroup = new Assessment_Group()
                {
                    AssessmentGroupID = pObjAssessmentGroupModel.AssessmentGroupID,
                    AssessmentGroupName = pObjAssessmentGroupModel.AssessmentGroupName.Trim(),
                    AssetTypeID = pObjAssessmentGroupModel.AssetTypeID,
                    Active = pObjAssessmentGroupModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLAssessmentGroup().BL_InsertUpdateAssessmentGroup(mObjAssessmentGroup);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "AssessmentGroup");
                    }
                    else
                    {
                        UI_FillAssetTypeDropDown(new Asset_Types() { intStatus = 1, IncludeAssetTypeIds = pObjAssessmentGroupModel.AssetTypeID.ToString() });
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjAssessmentGroupModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillAssetTypeDropDown(new Asset_Types() { intStatus = 1, IncludeAssetTypeIds = pObjAssessmentGroupModel.AssetTypeID.ToString() });
                    ViewBag.Message = "Error occurred while saving assessment group";
                    return View(pObjAssessmentGroupModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Assessment_Group mObjAssessmentGroup = new Assessment_Group()
                {
                    AssessmentGroupID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetAssessmentGroupList_Result mObjAssessmentGroupData = new BLAssessmentGroup().BL_GetAssessmentGroupDetails(mObjAssessmentGroup);

                if (mObjAssessmentGroupData != null)
                {
                    AssessmentGroupViewModel mObjAssessmentGroupModelView = new AssessmentGroupViewModel()
                    {
                        AssessmentGroupID = mObjAssessmentGroupData.AssessmentGroupID.GetValueOrDefault(),
                        AssessmentGroupName = mObjAssessmentGroupData.AssessmentGroupName,
                        AssetTypeName = mObjAssessmentGroupData.AssetTypeName,
                        ActiveText = mObjAssessmentGroupData.ActiveText
                    };

                    return View(mObjAssessmentGroupModelView);
                }
                else
                {
                    return RedirectToAction("List", "AssessmentGroup");
                }
            }
            else
            {
                return RedirectToAction("List", "AssessmentGroup");
            }
        }

        public JsonResult UpdateStatus(Assessment_Group pObjAssessmentGroupData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjAssessmentGroupData.AssessmentGroupID != 0)
            {
                FuncResponse mObjFuncResponse = new BLAssessmentGroup().BL_UpdateStatus(pObjAssessmentGroupData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["AssessmentGroupList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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