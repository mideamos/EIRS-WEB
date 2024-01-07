using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Admin.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Vereyon.Web;
using Elmah;
using System.Linq.Dynamic;
using System.Linq;
using System.Text;

namespace EIRS.Admin.Controllers
{
    public class AssessmentItemController : BaseController
    {
        public ActionResult List()
        {
            return View();
        }

        [HttpPost]
        public JsonResult LoadData()
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            var vFilter = Request.Form.GetValues("search[value]").FirstOrDefault();

            StringBuilder sbWhereCondition = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(Request.Form["AssessmentItemReferenceNo"]))
            {
                sbWhereCondition.Append(" AND ISNULL(AssessmentItemReferenceNo,'') LIKE @AssessmentItemReferenceNo");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["AssetTypeName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(AssetTypeName,'') LIKE @AssetTypeName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["AssessmentGroupName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(AssessmentGroupName,'') LIKE @AssessmentGroupName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["AssessmentSubGroupName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(AssessmentSubGroupName,'') LIKE @AssessmentSubGroupName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["RevenueStreamName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(RevenueStreamName,'') LIKE @RevenueStreamName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["RevenueSubStreamName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(RevenueSubStreamName,'') LIKE @RevenueSubStreamName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["AssessmentItemCategoryName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(AssessmentItemCategoryName,'') LIKE @AssessmentItemCategoryName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["AssessmentItemSubCategoryName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(AssessmentItemSubCategoryName,'') LIKE @AssessmentItemSubCategoryName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["AgencyName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(AgencyName,'') LIKE @AgencyName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["AssessmentItemName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(AssessmentItemName,'') LIKE @AssessmentItemName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["ComputationName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(ComputationName,'') LIKE @ComputationName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["TaxBaseAmount"]))
            {
                sbWhereCondition.Append(" AND ISNULL(Convert(varchar(50),TaxBaseAmount,106),'') LIKE @TaxBaseAmount");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["TaxAmount"]))
            {
                sbWhereCondition.Append(" AND ISNULL(Convert(varchar(50),TaxAmount,106),'') LIKE @TaxAmount");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["Percentage"]))
            {
                sbWhereCondition.Append(" AND ISNULL(Convert(varchar(50),Percentage,106),'') LIKE @Percentage");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["ActiveText"]))
            {
                sbWhereCondition.Append(" AND CASE WHEN ISNULL(aitem.Active,0) = 0 THEN 'In Active' ELSE 'Active' END LIKE @ActiveText");
            }

            if (!string.IsNullOrEmpty(vFilter))
            {
                sbWhereCondition.Append(" AND ( ISNULL(AssessmentItemReferenceNo,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(AssetTypeName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(AssessmentGroupName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(AssessmentSubGroupName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(AssessmentItemSubCategoryName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(RevenueStreamName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(RevenueSubStreamName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(AssessmentItemCategoryName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(AssessmentItemSubCategoryName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(AgencyName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(AssessmentItemName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(ComputationName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(Convert(varchar(50),TaxBaseAmount,106),'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(Convert(varchar(50),TaxAmount,106),'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(Convert(varchar(50),Percentage,106),'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR CASE WHEN ISNULL(aitem.Active,0) = 0 THEN 'In Active' ELSE 'Active' END LIKE @MainFilter)");
            }

            Assessment_Items mObjAssessmentItem = new Assessment_Items()
            {
                WhereCondition = sbWhereCondition.ToString(),
                OrderBy = vSortColumn,
                OrderByDirection = vSortColumnDir,
                PageNumber = (TrynParse.parseInt(vStart) / TrynParse.parseInt(vLength)) + 1,
                PageSize = TrynParse.parseInt(vLength),

                AssessmentItemReferenceNo = !string.IsNullOrWhiteSpace(Request.Form["AssessmentItemReferenceNo"]) ? "%" + Request.Form["AssessmentItemReferenceNo"].Trim() + "%" : TrynParse.parseString(Request.Form["AssessmentItemReferenceNo"]),
                AssetTypeName = !string.IsNullOrWhiteSpace(Request.Form["AssetTypeName"]) ? "%" + Request.Form["AssetTypeName"].Trim() + "%" : TrynParse.parseString(Request.Form["AssetTypeName"]),
                AssessmentGroupName = !string.IsNullOrWhiteSpace(Request.Form["AssessmentGroupName"]) ? "%" + Request.Form["AssessmentGroupName"].Trim() + "%" : TrynParse.parseString(Request.Form["AssessmentGroupName"]),
                AssessmentSubGroupName = !string.IsNullOrWhiteSpace(Request.Form["AssessmentSubGroupName"]) ? "%" + Request.Form["AssessmentSubGroupName"].Trim() + "%" : TrynParse.parseString(Request.Form["AssessmentSubGroupName"]),
                RevenueStreamName = !string.IsNullOrWhiteSpace(Request.Form["RevenueStreamName"]) ? "%" + Request.Form["RevenueStreamName"].Trim() + "%" : TrynParse.parseString(Request.Form["RevenueStreamName"]),
                RevenueSubStreamName = !string.IsNullOrWhiteSpace(Request.Form["RevenueSubStreamName"]) ? "%" + Request.Form["RevenueSubStreamName"].Trim() + "%" : TrynParse.parseString(Request.Form["RevenueSubStreamName"]),
                AssessmentItemCategoryName = !string.IsNullOrWhiteSpace(Request.Form["AssessmentItemCategoryName"]) ? "%" + Request.Form["AssessmentItemCategoryName"].Trim() + "%" : TrynParse.parseString(Request.Form["AssessmentItemCategoryName"]),
                AssessmentItemSubCategoryName = !string.IsNullOrWhiteSpace(Request.Form["AssessmentItemSubCategoryName"]) ? "%" + Request.Form["AssessmentItemSubCategoryName"].Trim() + "%" : TrynParse.parseString(Request.Form["AssessmentItemSubCategoryName"]),
                AgencyName = !string.IsNullOrWhiteSpace(Request.Form["AgencyName"]) ? "%" + Request.Form["AgencyName"].Trim() + "%" : TrynParse.parseString(Request.Form["AgencyName"]),
                AssessmentItemName = !string.IsNullOrWhiteSpace(Request.Form["AssessmentItemName"]) ? "%" + Request.Form["AssessmentItemName"].Trim() + "%" : TrynParse.parseString(Request.Form["AssessmentItemName"]),
                ComputationName = !string.IsNullOrWhiteSpace(Request.Form["ComputationName"]) ? "%" + Request.Form["ComputationName"].Trim() + "%" : TrynParse.parseString(Request.Form["ComputationName"]),
                StrTaxBaseAmount = !string.IsNullOrWhiteSpace(Request.Form["TaxBaseAmount"]) ? "%" + Request.Form["TaxBaseAmount"].Trim() + "%" : TrynParse.parseString(Request.Form["TaxBaseAmount"]),
                StrPercentage = !string.IsNullOrWhiteSpace(Request.Form["Percentage"]) ? "%" + Request.Form["Percentage"].Trim() + "%" : TrynParse.parseString(Request.Form["Percentage"]),
                StrTaxAmount = !string.IsNullOrWhiteSpace(Request.Form["TaxAmount"]) ? "%" + Request.Form["TaxAmount"].Trim() + "%" : TrynParse.parseString(Request.Form["TaxAmount"]),
                ActiveText = !string.IsNullOrWhiteSpace(Request.Form["ActiveText"]) ? "%" + Request.Form["ActiveText"].Trim() + "%" : TrynParse.parseString(Request.Form["ActiveText"]),
                MainFilter = !string.IsNullOrWhiteSpace(vFilter) ? "%" + vFilter.Trim() + "%" : vFilter
            };

            IDictionary<string, object> dcData = new BLAssessmentItem().BL_SearchAssessmentItem(mObjAssessmentItem);
            IList<usp_SearchAssessmentItem_Result> lstAssessmentItem = (IList<usp_SearchAssessmentItem_Result>)dcData["AssessmentItemList"];

            return Json(new
            {
                draw = vDraw,
                recordsFiltered = dcData["FilteredRecords"],
                recordsTotal = dcData["TotalRecords"],
                data = lstAssessmentItem
            }, JsonRequestBehavior.AllowGet);
        }

        public void UI_FillDropDown(AssessmentItemViewModel pObjAssessmentItemModel = null)
        {
            if (pObjAssessmentItemModel == null)
                pObjAssessmentItemModel = new AssessmentItemViewModel();

            UI_FillAssetTypeDropDown(new Asset_Types() { intStatus = 1, IncludeAssetTypeIds = pObjAssessmentItemModel.AssetTypeID.ToString() });
            UI_FillAssessmentGroupDropDown(new Assessment_Group() { intStatus = 1, AssetTypeID = pObjAssessmentItemModel.AssetTypeID, IncludeAssessmentGroupIds = pObjAssessmentItemModel.AssessmentGroupID.ToString() });
            UI_FillAssessmentSubGroupDropDown(new Assessment_SubGroup() { intStatus = 1, AssessmentGroupID = pObjAssessmentItemModel.AssessmentGroupID, IncludeAssessmentSubGroupIds = pObjAssessmentItemModel.AssessmentSubGroupID.ToString() });
            UI_FillRevenueStreamDropDown(new Revenue_Stream() { intStatus = 1, /*AssetTypeID = pObjAssessmentItemModel.AssetTypeID,*/ IncludeRevenueStreamIds = pObjAssessmentItemModel.RevenueStreamID.ToString() });
            UI_FillRevenueSubStreamDropDown(new Revenue_SubStream() { intStatus = 1, RevenueStreamID = pObjAssessmentItemModel.RevenueStreamID, IncludeRevenueSubStreamIds = pObjAssessmentItemModel.RevenueSubStreamID.ToString() });
            UI_FillAssessmentItemCategoryDropDown(new Assessment_Item_Category() { intStatus = 1, IncludeAssessmentItemCategoryIds = pObjAssessmentItemModel.AssessmentItemCategoryID.ToString() });
            UI_FillAssessmentItemSubCategoryDropDown(new Assessment_Item_SubCategory() { intStatus = 1, AssessmentItemCategoryID = pObjAssessmentItemModel.AssessmentItemCategoryID, IncludeAssessmentItemSubCategoryIds = pObjAssessmentItemModel.AssessmentItemSubCategoryID.ToString() });
            UI_FillAgencyDropDown(new Agency() { intStatus = 1, IncludeAgencyIds = pObjAssessmentItemModel.AgencyID.ToString() });
            UI_FillComputation();
        }

        public ActionResult Add()
        {
            UI_FillDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(AssessmentItemViewModel pObjAssessmentItemModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown(pObjAssessmentItemModel);
                return View(pObjAssessmentItemModel);
            }
            else
            {
                Assessment_Items mObjAssessmentItem = new Assessment_Items()
                {
                    AssessmentItemID = 0,
                    AssetTypeID = pObjAssessmentItemModel.AssetTypeID,
                    AssessmentGroupID = pObjAssessmentItemModel.AssessmentGroupID,
                    AssessmentSubGroupID = pObjAssessmentItemModel.AssessmentSubGroupID,
                    RevenueStreamID = pObjAssessmentItemModel.RevenueStreamID,
                    RevenueSubStreamID = pObjAssessmentItemModel.RevenueSubStreamID,
                    AssessmentItemCategoryID = pObjAssessmentItemModel.AssessmentItemCategoryID,
                    AssessmentItemSubCategoryID = pObjAssessmentItemModel.AssessmentItemSubCategoryID,
                    AgencyID = pObjAssessmentItemModel.AgencyID,
                    AssessmentItemName = pObjAssessmentItemModel.AssessmentItemName.Trim(),
                    ComputationID = pObjAssessmentItemModel.ComputationID,
                    TaxBaseAmount = pObjAssessmentItemModel.TaxBaseAmount,
                    Percentage = pObjAssessmentItemModel.Percentage,
                    TaxAmount = pObjAssessmentItemModel.ComputationID == 2 ? (pObjAssessmentItemModel.TaxBaseAmount * pObjAssessmentItemModel.Percentage / 100) : pObjAssessmentItemModel.TaxBaseAmount,
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLAssessmentItem().BL_InsertUpdateAssessmentItem(mObjAssessmentItem);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "AssessmentItem");
                    }
                    else
                    {
                        UI_FillDropDown(pObjAssessmentItemModel);
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjAssessmentItemModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillDropDown(pObjAssessmentItemModel);
                    ViewBag.Message = "Error occurred while saving assessment item ";
                    return View(pObjAssessmentItemModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Assessment_Items mObjAssessmentItem = new Assessment_Items()
                {
                    AssessmentItemID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetAssessmentItemList_Result mObjAssessmentItemData = new BLAssessmentItem().BL_GetAssessmentItemDetails(mObjAssessmentItem);

                if (mObjAssessmentItemData != null)
                {
                    AssessmentItemViewModel mObjAssessmentItemModelView = new AssessmentItemViewModel()
                    {
                        AssessmentItemID = mObjAssessmentItemData.AssessmentItemID.GetValueOrDefault(),
                        AssessmentRefNo = mObjAssessmentItemData.AssessmentItemReferenceNo,
                        AssetTypeID = mObjAssessmentItemData.AssetTypeID.GetValueOrDefault(),
                        AssessmentGroupID = mObjAssessmentItemData.AssessmentGroupID.GetValueOrDefault(),
                        AssessmentSubGroupID = mObjAssessmentItemData.AssessmentSubGroupID.GetValueOrDefault(),
                        RevenueStreamID = mObjAssessmentItemData.RevenueStreamID.GetValueOrDefault(),
                        RevenueSubStreamID = mObjAssessmentItemData.RevenueSubStreamID.GetValueOrDefault(),
                        AssessmentItemCategoryID = mObjAssessmentItemData.AssessmentItemCategoryID.GetValueOrDefault(),
                        AssessmentItemSubCategoryID = mObjAssessmentItemData.AssessmentItemSubCategoryID.GetValueOrDefault(),
                        AgencyID = mObjAssessmentItemData.AgencyID.GetValueOrDefault(),
                        AssessmentItemName = mObjAssessmentItemData.AssessmentItemName.Trim(),
                        ComputationID = mObjAssessmentItemData.ComputationID.GetValueOrDefault(),
                        TaxBaseAmount = mObjAssessmentItemData.TaxBaseAmount.GetValueOrDefault(),
                        Percentage = mObjAssessmentItemData.Percentage.GetValueOrDefault(),
                        TaxAmount = mObjAssessmentItemData.TaxAmount.GetValueOrDefault(),
                        Active = mObjAssessmentItemData.Active.GetValueOrDefault(),
                    };

                    UI_FillDropDown(mObjAssessmentItemModelView);
                    return View(mObjAssessmentItemModelView);
                }
                else
                {
                    return RedirectToAction("List", "AssessmentItem");
                }
            }
            else
            {
                return RedirectToAction("List", "AssessmentItem");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(AssessmentItemViewModel pObjAssessmentItemModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown(pObjAssessmentItemModel);
                return View(pObjAssessmentItemModel);
            }
            else
            {
                Assessment_Items mObjAssessmentItem = new Assessment_Items()
                {
                    AssessmentItemID = pObjAssessmentItemModel.AssessmentItemID,
                    AssetTypeID = pObjAssessmentItemModel.AssetTypeID,
                    AssessmentGroupID = pObjAssessmentItemModel.AssessmentGroupID,
                    AssessmentSubGroupID = pObjAssessmentItemModel.AssessmentSubGroupID,
                    RevenueStreamID = pObjAssessmentItemModel.RevenueStreamID,
                    RevenueSubStreamID = pObjAssessmentItemModel.RevenueSubStreamID,
                    AssessmentItemCategoryID = pObjAssessmentItemModel.AssessmentItemCategoryID,
                    AssessmentItemSubCategoryID = pObjAssessmentItemModel.AssessmentItemSubCategoryID,
                    AgencyID = pObjAssessmentItemModel.AgencyID,
                    AssessmentItemName = pObjAssessmentItemModel.AssessmentItemName.Trim(),
                    ComputationID = pObjAssessmentItemModel.ComputationID,
                    TaxBaseAmount = pObjAssessmentItemModel.TaxBaseAmount,
                    Percentage = pObjAssessmentItemModel.Percentage,
                    TaxAmount = pObjAssessmentItemModel.ComputationID == 2 ? (pObjAssessmentItemModel.TaxBaseAmount * pObjAssessmentItemModel.Percentage / 100) : pObjAssessmentItemModel.TaxBaseAmount,
                    Active = pObjAssessmentItemModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLAssessmentItem().BL_InsertUpdateAssessmentItem(mObjAssessmentItem);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "AssessmentItem");
                    }
                    else
                    {
                        UI_FillDropDown(pObjAssessmentItemModel);
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjAssessmentItemModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillDropDown(pObjAssessmentItemModel);
                    ViewBag.Message = "Error occurred while saving assessment item ";
                    return View(pObjAssessmentItemModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Assessment_Items mObjAssessmentItem = new Assessment_Items()
                {
                    AssessmentItemID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetAssessmentItemList_Result mObjAssessmentItemData = new BLAssessmentItem().BL_GetAssessmentItemDetails(mObjAssessmentItem);

                if (mObjAssessmentItemData != null)
                {
                    AssessmentItemViewModel mObjAssessmentItemModelView = new AssessmentItemViewModel()
                    {
                        AssessmentItemID = mObjAssessmentItemData.AssessmentItemID.GetValueOrDefault(),
                        AssessmentRefNo = mObjAssessmentItemData.AssessmentItemReferenceNo,
                        AssetTypeName = mObjAssessmentItemData.AssetTypeName,
                        AssessmentGroupName = mObjAssessmentItemData.AssessmentGroupName,
                        AssessmentSubGroupName = mObjAssessmentItemData.AssessmentSubGroupName,
                        RevenueStreamName = mObjAssessmentItemData.RevenueStreamName,
                        RevenueSubStreamName = mObjAssessmentItemData.RevenueSubStreamName,
                        AssessmentItemCategoryName = mObjAssessmentItemData.AssessmentItemCategoryName,
                        AssessmentItemSubCategoryName = mObjAssessmentItemData.AssessmentItemSubCategoryName,
                        AgencyName = mObjAssessmentItemData.AgencyName,
                        AssessmentItemName = mObjAssessmentItemData.AssessmentItemName.Trim(),
                        ComputationID = mObjAssessmentItemData.ComputationID.GetValueOrDefault(),
                        ComputationName = mObjAssessmentItemData.ComputationName,
                        TaxBaseAmount = mObjAssessmentItemData.TaxBaseAmount.GetValueOrDefault(),
                        Percentage = mObjAssessmentItemData.Percentage,
                        TaxAmount = mObjAssessmentItemData.TaxAmount,
                        ActiveText = mObjAssessmentItemData.ActiveText
                    };

                    return View(mObjAssessmentItemModelView);
                }
                else
                {
                    return RedirectToAction("List", "AssessmentItem");
                }
            }
            else
            {
                return RedirectToAction("List", "AssessmentItem");
            }
        }

        public JsonResult UpdateStatus(Assessment_Items pObjAssessmentItemData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjAssessmentItemData.AssessmentItemID != 0)
            {
                FuncResponse mObjFuncResponse = new BLAssessmentItem().BL_UpdateStatus(pObjAssessmentItemData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["AssessmentItemList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
                }

            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Action";
            }



            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDropDownList(int AssetTypeID)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>
            {
                ["AssessmentGroupList"] = new BLAssessmentGroup().BL_GetAssessmentGroupDropDownList(new Assessment_Group() { intStatus = 1, AssetTypeID = AssetTypeID })
            };
            //dcResponse["RevenueStreamList"] = new BLRevenueStream().BL_GetRevenueStreamDropDownList(new Revenue_Stream() { intStatus = 1, /*AssetTypeID = AssetTypeID*/ });

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }
    }
}