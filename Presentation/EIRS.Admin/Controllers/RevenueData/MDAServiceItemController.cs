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
    public class MDAServiceItemController : BaseController
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

            if (!string.IsNullOrWhiteSpace(Request.Form["MDAServiceItemReferenceNo"]))
            {
                sbWhereCondition.Append(" AND ISNULL(MDAServiceItemReferenceNo,'') LIKE @MDAServiceItemReferenceNo");
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
            if (!string.IsNullOrWhiteSpace(Request.Form["MDAServiceItemName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(MDAServiceItemName,'') LIKE @MDAServiceItemName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["ComputationName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(ComputationName,'') LIKE @ComputationName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["ServiceBaseAmount"]))
            {
                sbWhereCondition.Append(" AND ISNULL(Convert(varchar(50),ServiceBaseAmount,106),'') LIKE @ServiceBaseAmount");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["ServiceAmount"]))
            {
                sbWhereCondition.Append(" AND ISNULL(Convert(varchar(50),ServiceAmount,106),'') LIKE @ServiceAmount");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["Percentage"]))
            {
                sbWhereCondition.Append(" AND ISNULL(Convert(varchar(50),Percentage,106),'') LIKE @Percentage");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["ActiveText"]))
            {
                sbWhereCondition.Append(" AND CASE WHEN ISNULL(sitem.Active,0) = 0 THEN 'In Active' ELSE 'Active' END LIKE @ActiveText");
            }


            if (!string.IsNullOrEmpty(vFilter))
            {
                sbWhereCondition.Append(" AND ( ISNULL(MDAServiceItemReferenceNo,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(RevenueStreamName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(RevenueSubStreamName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(AssessmentItemCategoryName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(AssessmentItemSubCategoryName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(AgencyName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(MDAServiceItemName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(ComputationName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(Convert(varchar(50),ServiceAmount,106),'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(Convert(varchar(50),ServiceBaseAmount,106),'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(Convert(varchar(50),Percentage,106),'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR CASE WHEN ISNULL(sitem.Active,0) = 0 THEN 'In Active' ELSE 'Active' END LIKE @MainFilter)");
            }

            MDA_Service_Items mObjMDAServiceItem = new MDA_Service_Items()
            {
                WhereCondition = sbWhereCondition.ToString(),
                OrderBy = vSortColumn,
                OrderByDirection = vSortColumnDir,
                PageNumber = (TrynParse.parseInt(vStart) / TrynParse.parseInt(vLength)) + 1,
                PageSize = TrynParse.parseInt(vLength),

                MDAServiceItemReferenceNo = !string.IsNullOrWhiteSpace(Request.Form["MDAServiceItemReferenceNo"]) ? "%" + Request.Form["MDAServiceItemReferenceNo"].Trim() + "%" : TrynParse.parseString(Request.Form["MDAServiceItemReferenceNo"]),
                RevenueStreamName = !string.IsNullOrWhiteSpace(Request.Form["RevenueStreamName"]) ? "%" + Request.Form["RevenueStreamName"].Trim() + "%" : TrynParse.parseString(Request.Form["RevenueStreamName"]),
                RevenueSubStreamName = !string.IsNullOrWhiteSpace(Request.Form["RevenueSubStreamName"]) ? "%" + Request.Form["RevenueSubStreamName"].Trim() + "%" : TrynParse.parseString(Request.Form["RevenueSubStreamName"]),
                AssessmentItemCategoryName = !string.IsNullOrWhiteSpace(Request.Form["AssessmentItemCategoryName"]) ? "%" + Request.Form["AssessmentItemCategoryName"].Trim() + "%" : TrynParse.parseString(Request.Form["AssessmentItemCategoryName"]),
                AssessmentItemSubCategoryName = !string.IsNullOrWhiteSpace(Request.Form["AssessmentItemSubCategoryName"]) ? "%" + Request.Form["AssessmentItemSubCategoryName"].Trim() + "%" : TrynParse.parseString(Request.Form["AssessmentItemSubCategoryName"]),
                AgencyName = !string.IsNullOrWhiteSpace(Request.Form["AgencyName"]) ? "%" + Request.Form["AgencyName"].Trim() + "%" : TrynParse.parseString(Request.Form["AgencyName"]),
                MDAServiceItemName = !string.IsNullOrWhiteSpace(Request.Form["MDAServiceItemName"]) ? "%" + Request.Form["MDAServiceItemName"].Trim() + "%" : TrynParse.parseString(Request.Form["MDAServiceItemName"]),
                ComputationName = !string.IsNullOrWhiteSpace(Request.Form["ComputationName"]) ? "%" + Request.Form["ComputationName"].Trim() + "%" : TrynParse.parseString(Request.Form["ComputationName"]),
                StrServiceAmount = !string.IsNullOrWhiteSpace(Request.Form["ServiceAmount"]) ? "%" + Request.Form["ServiceAmount"].Trim() + "%" : TrynParse.parseString(Request.Form["ServiceAmount"]),
                StrPercentage = !string.IsNullOrWhiteSpace(Request.Form["Percentage"]) ? "%" + Request.Form["Percentage"].Trim() + "%" : TrynParse.parseString(Request.Form["Percentage"]),
                StrServiceBaseAmount = !string.IsNullOrWhiteSpace(Request.Form["ServiceBaseAmount"]) ? "%" + Request.Form["ServiceBaseAmount"].Trim() + "%" : TrynParse.parseString(Request.Form["ServiceBaseAmount"]),
                ActiveText = !string.IsNullOrWhiteSpace(Request.Form["ActiveText"]) ? "%" + Request.Form["ActiveText"].Trim() + "%" : TrynParse.parseString(Request.Form["ActiveText"]),
                MainFilter = !string.IsNullOrWhiteSpace(vFilter) ? "%" + vFilter.Trim() + "%" : vFilter
            };

            IDictionary<string, object> dcData = new BLMDAServiceItem().BL_SearchMDAServiceItem(mObjMDAServiceItem);
            IList<usp_SearchMDAServiceItem_Result> lstMDAServiceItem = (IList<usp_SearchMDAServiceItem_Result>)dcData["MDAServiceItemList"];

            return Json(new
            {
                draw = vDraw,
                recordsFiltered = dcData["FilteredRecords"],
                recordsTotal = dcData["TotalRecords"],
                data = lstMDAServiceItem
            }, JsonRequestBehavior.AllowGet);
        }

        public void UI_FillDropDown(MDAServiceItemViewModel pObjMDAServiceItemModel = null)
        {
            if (pObjMDAServiceItemModel == null)
                pObjMDAServiceItemModel = new MDAServiceItemViewModel();

            UI_FillRevenueStreamDropDown(new Revenue_Stream() { intStatus = 1, /*AssetTypeID = pObjMDAServiceItemModel.AssetTypeID,*/ IncludeRevenueStreamIds = pObjMDAServiceItemModel.RevenueStreamID.ToString() });
            UI_FillRevenueSubStreamDropDown(new Revenue_SubStream() { intStatus = 1, RevenueStreamID = pObjMDAServiceItemModel.RevenueStreamID, IncludeRevenueSubStreamIds = pObjMDAServiceItemModel.RevenueSubStreamID.ToString() });
            UI_FillAssessmentItemCategoryDropDown(new Assessment_Item_Category() { intStatus = 1, IncludeAssessmentItemCategoryIds = pObjMDAServiceItemModel.AssessmentItemCategoryID.ToString() });
            UI_FillAssessmentItemSubCategoryDropDown(new Assessment_Item_SubCategory() { intStatus = 1, AssessmentItemCategoryID = pObjMDAServiceItemModel.AssessmentItemCategoryID, IncludeAssessmentItemSubCategoryIds= pObjMDAServiceItemModel.AssessmentItemSubCategoryID.ToString() });
            UI_FillAgencyDropDown(new Agency() { intStatus = 1, IncludeAgencyIds = pObjMDAServiceItemModel.AgencyID.ToString() });
            UI_FillComputation();
        }

        public ActionResult Add()
        {
            UI_FillDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(MDAServiceItemViewModel pObjMDAServiceItemModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown(pObjMDAServiceItemModel);
                return View(pObjMDAServiceItemModel);
            }
            else
            {
                MDA_Service_Items mObjMDAServiceItem = new MDA_Service_Items()
                {
                    MDAServiceItemID = 0,
                    RevenueStreamID = pObjMDAServiceItemModel.RevenueStreamID,
                    RevenueSubStreamID = pObjMDAServiceItemModel.RevenueSubStreamID,
                    AssessmentItemCategoryID = pObjMDAServiceItemModel.AssessmentItemCategoryID,
                    AssessmentItemSubCategoryID = pObjMDAServiceItemModel.AssessmentItemSubCategoryID,
                    AgencyID = pObjMDAServiceItemModel.AgencyID,
                    MDAServiceItemName = pObjMDAServiceItemModel.MDAServiceItemName.Trim(),
                    ComputationID = pObjMDAServiceItemModel.ComputationID,
                    ServiceBaseAmount = pObjMDAServiceItemModel.ServiceBaseAmount,
                    Percentage = pObjMDAServiceItemModel.Percentage,
                    ServiceAmount = pObjMDAServiceItemModel.ComputationID == 2 ? (pObjMDAServiceItemModel.ServiceBaseAmount * pObjMDAServiceItemModel.Percentage / 100) : pObjMDAServiceItemModel.ServiceBaseAmount,
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLMDAServiceItem().BL_InsertUpdateMDAServiceItem(mObjMDAServiceItem);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "MDAServiceItem");
                    }
                    else
                    {
                        UI_FillDropDown(pObjMDAServiceItemModel);
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjMDAServiceItemModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillDropDown(pObjMDAServiceItemModel);
                    ViewBag.Message = "Error occurred while saving mda service item ";
                    return View(pObjMDAServiceItemModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                MDA_Service_Items mObjMDAServiceItem = new MDA_Service_Items()
                {
                    MDAServiceItemID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetMDAServiceItemList_Result mObjMDAServiceItemData = new BLMDAServiceItem().BL_GetMDAServiceItemDetails(mObjMDAServiceItem);

                if (mObjMDAServiceItemData != null)
                {
                    MDAServiceItemViewModel mObjMDAServiceItemModelView = new MDAServiceItemViewModel()
                    {
                        MDAServiceItemID = mObjMDAServiceItemData.MDAServiceItemID.GetValueOrDefault(),
                        MDAServiceRefNo = mObjMDAServiceItemData.MDAServiceItemReferenceNo,
                        RevenueStreamID = mObjMDAServiceItemData.RevenueStreamID.GetValueOrDefault(),
                        RevenueSubStreamID = mObjMDAServiceItemData.RevenueSubStreamID.GetValueOrDefault(),
                        AssessmentItemCategoryID = mObjMDAServiceItemData.AssessmentItemCategoryID.GetValueOrDefault(),
                        AssessmentItemSubCategoryID = mObjMDAServiceItemData.AssessmentItemSubCategoryID.GetValueOrDefault(),
                        AgencyID = mObjMDAServiceItemData.AgencyID.GetValueOrDefault(),
                        MDAServiceItemName = mObjMDAServiceItemData.MDAServiceItemName.Trim(),
                        ComputationID = mObjMDAServiceItemData.ComputationID.GetValueOrDefault(),
                        ServiceBaseAmount = mObjMDAServiceItemData.ServiceBaseAmount.GetValueOrDefault(),
                        Percentage = mObjMDAServiceItemData.Percentage.GetValueOrDefault(),
                        ServiceAmount = mObjMDAServiceItemData.ServiceAmount.GetValueOrDefault(),
                        Active = mObjMDAServiceItemData.Active.GetValueOrDefault(),
                    };

                    UI_FillDropDown(mObjMDAServiceItemModelView);
                    return View(mObjMDAServiceItemModelView);
                }
                else
                {
                    return RedirectToAction("List", "MDAServiceItem");
                }
            }
            else
            {
                return RedirectToAction("List", "MDAServiceItem");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(MDAServiceItemViewModel pObjMDAServiceItemModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown(pObjMDAServiceItemModel);
                return View(pObjMDAServiceItemModel);
            }
            else
            {
                MDA_Service_Items mObjMDAServiceItem = new MDA_Service_Items()
                {
                    MDAServiceItemID = pObjMDAServiceItemModel.MDAServiceItemID,
                    RevenueStreamID = pObjMDAServiceItemModel.RevenueStreamID,
                    RevenueSubStreamID = pObjMDAServiceItemModel.RevenueSubStreamID,
                    AssessmentItemCategoryID = pObjMDAServiceItemModel.AssessmentItemCategoryID,
                    AssessmentItemSubCategoryID = pObjMDAServiceItemModel.AssessmentItemSubCategoryID,
                    AgencyID = pObjMDAServiceItemModel.AgencyID,
                    MDAServiceItemName = pObjMDAServiceItemModel.MDAServiceItemName.Trim(),
                    ComputationID = pObjMDAServiceItemModel.ComputationID,
                    ServiceBaseAmount = pObjMDAServiceItemModel.ServiceBaseAmount,
                    Percentage = pObjMDAServiceItemModel.Percentage,
                    ServiceAmount = pObjMDAServiceItemModel.ComputationID == 2 ? (pObjMDAServiceItemModel.ServiceBaseAmount * pObjMDAServiceItemModel.Percentage / 100) : pObjMDAServiceItemModel.ServiceBaseAmount,
                    Active = pObjMDAServiceItemModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLMDAServiceItem().BL_InsertUpdateMDAServiceItem(mObjMDAServiceItem);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "MDAServiceItem");
                    }
                    else
                    {
                        UI_FillDropDown(pObjMDAServiceItemModel);
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjMDAServiceItemModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillDropDown(pObjMDAServiceItemModel);
                    ViewBag.Message = "Error occurred while saving mda service item ";
                    return View(pObjMDAServiceItemModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                MDA_Service_Items mObjMDAServiceItem = new MDA_Service_Items()
                {
                    MDAServiceItemID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetMDAServiceItemList_Result mObjMDAServiceItemData = new BLMDAServiceItem().BL_GetMDAServiceItemDetails(mObjMDAServiceItem);

                if (mObjMDAServiceItemData != null)
                {
                    MDAServiceItemViewModel mObjMDAServiceItemModelView = new MDAServiceItemViewModel()
                    {
                        MDAServiceItemID = mObjMDAServiceItemData.MDAServiceItemID.GetValueOrDefault(),
                        MDAServiceRefNo = mObjMDAServiceItemData.MDAServiceItemReferenceNo,
                        RevenueStreamName = mObjMDAServiceItemData.RevenueStreamName,
                        RevenueSubStreamName = mObjMDAServiceItemData.RevenueSubStreamName,
                        AssessmentItemCategoryName = mObjMDAServiceItemData.AssessmentItemCategoryName,
                        AssessmentItemSubCategoryName = mObjMDAServiceItemData.AssessmentItemSubCategoryName,
                        AgencyName = mObjMDAServiceItemData.AgencyName,
                        MDAServiceItemName = mObjMDAServiceItemData.MDAServiceItemName.Trim(),
                        ComputationID = mObjMDAServiceItemData.ComputationID.GetValueOrDefault(),
                        ComputationName = mObjMDAServiceItemData.ComputationName,
                        ServiceBaseAmount = mObjMDAServiceItemData.ServiceBaseAmount.GetValueOrDefault(),
                        Percentage = mObjMDAServiceItemData.Percentage.GetValueOrDefault(),
                        ServiceAmount = mObjMDAServiceItemData.ServiceAmount.GetValueOrDefault(),
                        ActiveText = mObjMDAServiceItemData.ActiveText
                    };

                    return View(mObjMDAServiceItemModelView);
                }
                else
                {
                    return RedirectToAction("List", "MDAServiceItem");
                }
            }
            else
            {
                return RedirectToAction("List", "MDAServiceItem");
            }
        }

        public JsonResult UpdateStatus(MDA_Service_Items pObjMDAServiceItemData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjMDAServiceItemData.MDAServiceItemID != 0)
            {
                FuncResponse mObjFuncResponse = new BLMDAServiceItem().BL_UpdateStatus(pObjMDAServiceItemData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["MDAServiceItemList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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