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
    public class AssessmentSubGroupController : BaseController
    {
        public ActionResult List()
        {
            Assessment_SubGroup mObjAssessmentSubGroup = new Assessment_SubGroup()
            {
                intStatus = 2
            };

            IList<usp_GetAssessmentSubGroupList_Result> lstAssessmentSubGroup = new BLAssessmentSubGroup().BL_GetAssessmentSubGroupList(mObjAssessmentSubGroup);
            return View(lstAssessmentSubGroup);
        }

        public void UI_FillDropDown(AssessmentSubGroupViewModel pObjAssessmentSubGroupModel = null)
        {
            if (pObjAssessmentSubGroupModel == null)
                pObjAssessmentSubGroupModel = new AssessmentSubGroupViewModel();

            UI_FillAssetTypeDropDown(new Asset_Types() { intStatus = 1, IncludeAssetTypeIds = pObjAssessmentSubGroupModel.AssetTypeID.ToString() });
            UI_FillAssessmentGroupDropDown(new Assessment_Group() { intStatus = 1, AssetTypeID = pObjAssessmentSubGroupModel.AssetTypeID, IncludeAssessmentGroupIds = pObjAssessmentSubGroupModel.AssessmentGroupID.ToString() });
        }

        public ActionResult Add()
        {
            UI_FillDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(AssessmentSubGroupViewModel pObjAssessmentSubGroupModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown(pObjAssessmentSubGroupModel);
                return View(pObjAssessmentSubGroupModel);
            }
            else
            {
                Assessment_SubGroup mObjAssessmentSubGroup = new Assessment_SubGroup()
                {
                    AssessmentSubGroupID = 0,
                    AssessmentSubGroupName = pObjAssessmentSubGroupModel.AssessmentSubGroupName.Trim(),
                    AssessmentGroupID = pObjAssessmentSubGroupModel.AssessmentGroupID,
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLAssessmentSubGroup().BL_InsertUpdateAssessmentSubGroup(mObjAssessmentSubGroup);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "AssessmentSubGroup");
                    }
                    else
                    {
                        UI_FillDropDown(pObjAssessmentSubGroupModel);
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjAssessmentSubGroupModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillDropDown(pObjAssessmentSubGroupModel);
                    ViewBag.Message = "Error occurred while saving assessment sub group";
                    return View(pObjAssessmentSubGroupModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Assessment_SubGroup mObjAssessmentSubGroup = new Assessment_SubGroup()
                {
                    AssessmentSubGroupID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetAssessmentSubGroupList_Result mObjAssessmentSubGroupData = new BLAssessmentSubGroup().BL_GetAssessmentSubGroupDetails(mObjAssessmentSubGroup);

                if (mObjAssessmentSubGroupData != null)
                {
                    AssessmentSubGroupViewModel mObjAssessmentSubGroupModelView = new AssessmentSubGroupViewModel()
                    {
                        AssessmentSubGroupID = mObjAssessmentSubGroupData.AssessmentSubGroupID.GetValueOrDefault(),
                        AssessmentSubGroupName = mObjAssessmentSubGroupData.AssessmentSubGroupName,
                        AssessmentGroupID = mObjAssessmentSubGroupData.AssessmentGroupID.GetValueOrDefault(),
                        AssetTypeID = mObjAssessmentSubGroupData.AssetTypeID.GetValueOrDefault(),
                        Active = mObjAssessmentSubGroupData.Active.GetValueOrDefault(),
                    };

                    UI_FillDropDown(mObjAssessmentSubGroupModelView);
                    return View(mObjAssessmentSubGroupModelView);
                }
                else
                {
                    return RedirectToAction("List", "AssessmentSubGroup");
                }
            }
            else
            {
                return RedirectToAction("List", "AssessmentSubGroup");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(AssessmentSubGroupViewModel pObjAssessmentSubGroupModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown(pObjAssessmentSubGroupModel);
                return View(pObjAssessmentSubGroupModel);
            }
            else
            {
                Assessment_SubGroup mObjAssessmentSubGroup = new Assessment_SubGroup()
                {
                    AssessmentSubGroupID = pObjAssessmentSubGroupModel.AssessmentSubGroupID,
                    AssessmentSubGroupName = pObjAssessmentSubGroupModel.AssessmentSubGroupName.Trim(),
                    AssessmentGroupID = pObjAssessmentSubGroupModel.AssessmentGroupID,
                    Active = pObjAssessmentSubGroupModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLAssessmentSubGroup().BL_InsertUpdateAssessmentSubGroup(mObjAssessmentSubGroup);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "AssessmentSubGroup");
                    }
                    else
                    {
                        UI_FillDropDown(pObjAssessmentSubGroupModel);
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjAssessmentSubGroupModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillDropDown(pObjAssessmentSubGroupModel);
                    ViewBag.Message = "Error occurred while saving assessment sub group";
                    return View(pObjAssessmentSubGroupModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Assessment_SubGroup mObjAssessmentSubGroup = new Assessment_SubGroup()
                {
                    AssessmentSubGroupID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetAssessmentSubGroupList_Result mObjAssessmentSubGroupData = new BLAssessmentSubGroup().BL_GetAssessmentSubGroupDetails(mObjAssessmentSubGroup);

                if (mObjAssessmentSubGroupData != null)
                {
                    AssessmentSubGroupViewModel mObjAssessmentSubGroupModelView = new AssessmentSubGroupViewModel()
                    {
                        AssessmentSubGroupID = mObjAssessmentSubGroupData.AssessmentSubGroupID.GetValueOrDefault(),
                        AssessmentSubGroupName = mObjAssessmentSubGroupData.AssessmentSubGroupName,
                        AssessmentGroupName = mObjAssessmentSubGroupData.AssessmentGroupName,
                        AssetTypeName = mObjAssessmentSubGroupData.AssetTypeName,
                        ActiveText = mObjAssessmentSubGroupData.ActiveText
                    };

                    return View(mObjAssessmentSubGroupModelView);
                }
                else
                {
                    return RedirectToAction("List", "AssessmentSubGroup");
                }
            }
            else
            {
                return RedirectToAction("List", "AssessmentSubGroup");
            }
        }

        public JsonResult UpdateStatus(Assessment_SubGroup pObjAssessmentSubGroupData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjAssessmentSubGroupData.AssessmentSubGroupID != 0)
            {
                FuncResponse mObjFuncResponse = new BLAssessmentSubGroup().BL_UpdateStatus(pObjAssessmentSubGroupData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["AssessmentSubGroupList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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