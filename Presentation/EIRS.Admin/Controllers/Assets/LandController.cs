using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using Vereyon.Web;
using Elmah;
using System.Linq.Dynamic;
using System.Text;

namespace EIRS.Admin.Controllers
{
    public class LandController : BaseController
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
            //var vColumnFilter = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault(); ;


            StringBuilder sbWhereCondition = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(Request.Form["LandRIN"]))
            {
                sbWhereCondition.Append(" AND ISNULL(LandRIN,'') LIKE @LandRIN");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["PlotNumber"]))
            {
                sbWhereCondition.Append(" AND ISNULL(PlotNumber,'') LIKE @PlotNumber");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["StreetName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(StreetName,'') LIKE @StreetName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["TownName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(TownName,'') LIKE @TownName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["LGAName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(LGAName,'') LIKE @LGAName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["WardName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(WardName,'') LIKE @WardName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["LandSize_Length"]))
            {
                sbWhereCondition.Append(" AND ISNULL(Convert(varchar(50),LandSize_Length,106),'') LIKE @LandSize_Length");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["LandSize_Width"]))
            {
                sbWhereCondition.Append(" AND ISNULL(Convert(varchar(50),LandSize_Width,106),'') LIKE @LandSize_Width");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["C_OF_O_Ref"]))
            {
                sbWhereCondition.Append(" AND ISNULL(C_OF_O_Ref,'') LIKE @C_OF_O_Ref");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["LandPurposeName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(LandPurposeName,'') LIKE @LandPurposeName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["LandFunctionName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(LandFunctionName,'') LIKE @LandFunctionName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["LandOwnershipName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(LandOwnershipName,'') LIKE @LandOwnershipName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["LandDevelopmentName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(LandDevelopmentName,'') LIKE @LandDevelopmentName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["Latitude"]))
            {
                sbWhereCondition.Append(" AND ISNULL(Latitude,'') LIKE @Latitude");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["Longitude"]))
            {
                sbWhereCondition.Append(" AND ISNULL(Longitude,'') LIKE @Longitude");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["LandStreetConditionName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(LandStreetConditionName,'') LIKE @LandStreetConditionName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["ValueOfLand"]))
            {
                sbWhereCondition.Append(" AND ISNULL(Convert(varchar(50),ValueOfLand,106),'') LIKE @ValueOfLand");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["Neighborhood"]))
            {
                sbWhereCondition.Append(" AND ISNULL(Neighborhood,'') LIKE @Neighborhood");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["ActiveText"]))
            {
                sbWhereCondition.Append(" AND CASE WHEN ISNULL(lnd.Active,0) = 0 THEN 'In Active' ELSE 'Active' END LIKE @ActiveText");
            }

            if (!string.IsNullOrEmpty(vFilter))
            {
                sbWhereCondition.Append(" AND ( ISNULL(LandRIN,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(PlotNumber,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(StreetName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(TownName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(LGAName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(WardName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(Convert(varchar(50),LandSize_Length,106),'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(Convert(varchar(50),LandSize_Width,106),'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(C_OF_O_Ref,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(LandPurposeName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(LandFunctionName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(LandOwnershipName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(LandDevelopmentName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(Latitude,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(Longitude,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(LandStreetConditionName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(Convert(varchar(50),ValueOfLand,106),'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(Neighborhood,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR CASE WHEN ISNULL(lnd.Active,0) = 0 THEN 'In Active' ELSE 'Active' END LIKE @MainFilter)");
            }

            Land mObjLand = new Land()
            {
                WhereCondition = sbWhereCondition.ToString(),
                OrderBy = vSortColumn,
                OrderByDirection = vSortColumnDir,
                PageNumber = (TrynParse.parseInt(vStart) / TrynParse.parseInt(vLength)) + 1,
                PageSize = TrynParse.parseInt(vLength),

                LandRIN = !string.IsNullOrWhiteSpace(Request.Form["LandRIN"]) ? "%" + Request.Form["LandRIN"].Trim() + "%" : TrynParse.parseString(Request.Form["LandRIN"]),
                PlotNumber = !string.IsNullOrWhiteSpace(Request.Form["PlotNumber"]) ? "%" + Request.Form["PlotNumber"].Trim() + "%" : TrynParse.parseString(Request.Form["PlotNumber"]),
                StreetName = !string.IsNullOrWhiteSpace(Request.Form["StreetName"]) ? "%" + Request.Form["StreetName"].Trim() + "%" : TrynParse.parseString(Request.Form["StreetName"]),
                TownName = !string.IsNullOrWhiteSpace(Request.Form["TownName"]) ? "%" + Request.Form["TownName"].Trim() + "%" : TrynParse.parseString(Request.Form["TownName"]),
                LGAName = !string.IsNullOrWhiteSpace(Request.Form["LGAName"]) ? "%" + Request.Form["LGAName"].Trim() + "%" : TrynParse.parseString(Request.Form["LGAName"]),
                WardName = !string.IsNullOrWhiteSpace(Request.Form["WardName"]) ? "%" + Request.Form["WardName"].Trim() + "%" : TrynParse.parseString(Request.Form["WardName"]),
                strLandSize_Length = !string.IsNullOrWhiteSpace(Request.Form["LandSize_Length"]) ? "%" + Request.Form["LandSize_Length"].Trim() + "%" : TrynParse.parseString(Request.Form["LandSize_Length"]),
                strLandSize_Width = !string.IsNullOrWhiteSpace(Request.Form["LandSize_Width"]) ? "%" + Request.Form["LandSize_Width"].Trim() + "%" : TrynParse.parseString(Request.Form["LandSize_Width"]),
                C_OF_O_Ref = !string.IsNullOrWhiteSpace(Request.Form["C_OF_O_Ref"]) ? "%" + Request.Form["C_OF_O_Ref"].Trim() + "%" : TrynParse.parseString(Request.Form["C_OF_O_Ref"]),
                LandPurposeName = !string.IsNullOrWhiteSpace(Request.Form["LandPurposeName"]) ? "%" + Request.Form["LandPurposeName"].Trim() + "%" : TrynParse.parseString(Request.Form["LandPurposeName"]),
                LandFunctionName = !string.IsNullOrWhiteSpace(Request.Form["LandFunctionName"]) ? "%" + Request.Form["LandFunctionName"].Trim() + "%" : TrynParse.parseString(Request.Form["LandFunctionName"]),
                LandOwnershipName = !string.IsNullOrWhiteSpace(Request.Form["LandOwnershipName"]) ? "%" + Request.Form["LandOwnershipName"].Trim() + "%" : TrynParse.parseString(Request.Form["LandOwnershipName"]),
                LandDevelopmentName = !string.IsNullOrWhiteSpace(Request.Form["LandDevelopmentName"]) ? "%" + Request.Form["LandDevelopmentName"].Trim() + "%" : TrynParse.parseString(Request.Form["LandDevelopmentName"]),
                Latitude = !string.IsNullOrWhiteSpace(Request.Form["Latitude"]) ? "%" + Request.Form["Latitude"].Trim() + "%" : TrynParse.parseString(Request.Form["Latitude"]),
                Longitude = !string.IsNullOrWhiteSpace(Request.Form["Longitude"]) ? "%" + Request.Form["Longitude"].Trim() + "%" : TrynParse.parseString(Request.Form["Longitude"]),
                LandStreetConditionName = !string.IsNullOrWhiteSpace(Request.Form["LandStreetConditionName"]) ? "%" + Request.Form["LandStreetConditionName"].Trim() + "%" : TrynParse.parseString(Request.Form["LandStreetConditionName"]),
                strValueOfLand = !string.IsNullOrWhiteSpace(Request.Form["ValueOfLand"]) ? "%" + Request.Form["ValueOfLand"].Trim() + "%" : TrynParse.parseString(Request.Form["ValueOfLand"]),
                Neighborhood = !string.IsNullOrWhiteSpace(Request.Form["Neighborhood"]) ? "%" + Request.Form["Neighborhood"].Trim() + "%" : TrynParse.parseString(Request.Form["Neighborhood"]),
                ActiveText = !string.IsNullOrWhiteSpace(Request.Form["ActiveText"]) ? "%" + Request.Form["ActiveText"].Trim() + "%" : TrynParse.parseString(Request.Form["ActiveText"]),
                MainFilter = !string.IsNullOrWhiteSpace(vFilter) ? "%" + vFilter.Trim() + "%" : vFilter
            };

            IDictionary<string, object> dcData = new BLLand().BL_SearchLand(mObjLand);
            IList<usp_SearchLand_Result> lstLand = (IList<usp_SearchLand_Result>)dcData["LandList"];

            return Json(new
            {
                draw = vDraw,
                recordsFiltered = dcData["FilteredRecords"],
                recordsTotal = dcData["TotalRecords"],
                data = lstLand
            }, JsonRequestBehavior.AllowGet);
        }


        public void UI_FillDropDown(LandViewModel pObjLandViewModel = null)
        {
            if (pObjLandViewModel != null)
                pObjLandViewModel.AssetTypeID = (int)EnumList.AssetTypes.Land;
            else if (pObjLandViewModel == null)
                pObjLandViewModel = new LandViewModel();

            UI_FillTownDropDown(new Town() { intStatus = 1, IncludeTownIds = pObjLandViewModel.TownID.ToString() });
            UI_FillLGADropDown(new LGA() { intStatus = 1, IncludeLGAIds = pObjLandViewModel.LGAID.ToString() });
            UI_FillWardDropDown(new Ward() { intStatus = 1, LGAID = pObjLandViewModel.LGAID, IncludeWardIds = pObjLandViewModel.WardID.ToString() });
            UI_FillAssetTypeDropDown(new Asset_Types() { intStatus = 1, IncludeAssetTypeIds = pObjLandViewModel.AssetTypeID.ToString() }, (int)EnumList.AssetTypes.Land);
            UI_FillLandPurposeDropDown(new Land_Purpose() { intStatus = 1, IncludeLandPurposeIds = pObjLandViewModel.LandPurposeID.ToString() });
            UI_FillLandFunctionDropDown(new Land_Function() { intStatus = 1, IncludeLandFunctionIds = pObjLandViewModel.LandFunctionID.ToString(), LandPurposeID = pObjLandViewModel.LandPurposeID });
            UI_FillLandDevelopmentDropDown(new Land_Development() { intStatus = 1, IncludeLandDevelopmentIds = pObjLandViewModel.LandDevelopmentID.ToString() });
            UI_FillLandOwnershipDropDown(new Land_Ownership() { intStatus = 1, IncludeLandOwnershipIds = pObjLandViewModel.LandOwnershipID.ToString() });
            UI_FillLandStreetConditionDropDown(new Land_StreetCondition() { intStatus = 1, IncludeLandStreetConditionIds = pObjLandViewModel.LandStreetConditionID.ToString() });
        }

        public ActionResult Add()
        {
            UI_FillDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(LandViewModel pObjLandModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown(pObjLandModel);
                return View(pObjLandModel);
            }
            else
            {
                Land mObjLand = new Land()
                {
                    LandID = 0,
                    PlotNumber = pObjLandModel.PlotNumber,
                    StreetName = pObjLandModel.StreetName.Trim(),
                    TownID = pObjLandModel.TownID,
                    LGAID = pObjLandModel.LGAID,
                    WardID = pObjLandModel.WardID,
                    AssetTypeID = (int)EnumList.AssetTypes.Land,
                    LandSize_Length = pObjLandModel.LandSize_Length,
                    LandSize_Width = pObjLandModel.LandSize_Width,
                    C_OF_O_Ref = pObjLandModel.C_OF_O_Ref,
                    LandPurposeID = pObjLandModel.LandPurposeID,
                    LandFunctionID = pObjLandModel.LandFunctionID,
                    LandOwnershipID = pObjLandModel.LandOwnershipID,
                    LandDevelopmentID = pObjLandModel.LandDevelopmentID,
                    Latitude = pObjLandModel.Latitude,
                    Longitude = pObjLandModel.Longitude,
                    ValueOfLand = pObjLandModel.ValueOfLand,
                    LandStreetConditionID = pObjLandModel.LandStreetConditionID,
                    Neighborhood = pObjLandModel.Neighborhood,
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse<Land> mObjResponse = new BLLand().BL_InsertUpdateLand(mObjLand);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "Land");
                    }
                    else
                    {
                        UI_FillDropDown(pObjLandModel);
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjLandModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillDropDown(pObjLandModel);
                    ViewBag.Message = "Error occurred while saving land";
                    return View(pObjLandModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Land mObjLand = new Land()
                {
                    LandID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetLandList_Result mObjLandData = new BLLand().BL_GetLandDetails(mObjLand);

                if (mObjLandData != null)
                {
                    LandViewModel mObjLandModelView = new LandViewModel()
                    {
                        LandID = mObjLandData.LandID.GetValueOrDefault(),
                        LandRIN = mObjLandData.LandRIN,
                        PlotNumber = mObjLandData.PlotNumber,
                        StreetName = mObjLandData.StreetName.Trim(),
                        TownID = mObjLandData.TownID.GetValueOrDefault(),
                        LGAID = mObjLandData.LGAID.GetValueOrDefault(),
                        WardID = mObjLandData.WardID.GetValueOrDefault(),
                        AssetTypeID = mObjLandData.AssetTypeID.GetValueOrDefault(),
                        LandSize_Length = mObjLandData.LandSize_Length,
                        LandSize_Width = mObjLandData.LandSize_Width,
                        C_OF_O_Ref = mObjLandData.C_OF_O_Ref,
                        LandPurposeID = mObjLandData.LandPurposeID.GetValueOrDefault(),
                        LandFunctionID = mObjLandData.LandFunctionID.GetValueOrDefault(),
                        LandOwnershipID = mObjLandData.LandOwnershipID.GetValueOrDefault(),
                        LandDevelopmentID = mObjLandData.LandDevelopmentID.GetValueOrDefault(),
                        Latitude = mObjLandData.Latitude,
                        Longitude = mObjLandData.Longitude,
                        ValueOfLand = mObjLandData.ValueOfLand.GetValueOrDefault(),
                        LandStreetConditionID = mObjLandData.LandStreetConditionID.GetValueOrDefault(),
                        Neighborhood = mObjLandData.Neighborhood,
                        Active = mObjLandData.Active.GetValueOrDefault(),
                    };

                    UI_FillDropDown(mObjLandModelView);
                    return View(mObjLandModelView);
                }
                else
                {
                    return RedirectToAction("List", "Land");
                }
            }
            else
            {
                return RedirectToAction("List", "Land");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(LandViewModel pObjLandModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown(pObjLandModel);
                return View(pObjLandModel);
            }
            else
            {
                Land mObjLand = new Land()
                {
                    LandID = pObjLandModel.LandID,
                    PlotNumber = pObjLandModel.PlotNumber.Trim(),
                    StreetName = pObjLandModel.StreetName.Trim(),
                    TownID = pObjLandModel.TownID,
                    LGAID = pObjLandModel.LGAID,
                    WardID = pObjLandModel.WardID,
                    AssetTypeID = (int)EnumList.AssetTypes.Land,
                    LandSize_Length = pObjLandModel.LandSize_Length,
                    LandSize_Width = pObjLandModel.LandSize_Width,
                    C_OF_O_Ref = pObjLandModel.C_OF_O_Ref,
                    LandPurposeID = pObjLandModel.LandPurposeID,
                    LandFunctionID = pObjLandModel.LandFunctionID,
                    LandOwnershipID = pObjLandModel.LandOwnershipID,
                    LandDevelopmentID = pObjLandModel.LandDevelopmentID,
                    Latitude = pObjLandModel.Latitude,
                    Longitude = pObjLandModel.Longitude,
                    ValueOfLand = pObjLandModel.ValueOfLand,
                    LandStreetConditionID = pObjLandModel.LandStreetConditionID,
                    Neighborhood = pObjLandModel.Neighborhood,
                    Active = pObjLandModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse<Land> mObjResponse = new BLLand().BL_InsertUpdateLand(mObjLand);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "Land");
                    }
                    else
                    {
                        UI_FillDropDown(pObjLandModel);
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjLandModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillDropDown(pObjLandModel);
                    ViewBag.Message = "Error occurred while saving land";
                    return View(pObjLandModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Land mObjLand = new Land()
                {
                    LandID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetLandList_Result mObjLandData = new BLLand().BL_GetLandDetails(mObjLand);

                if (mObjLandData != null)
                {
                    LandViewModel mObjLandModelView = new LandViewModel()
                    {
                        LandID = mObjLandData.LandID.GetValueOrDefault(),
                        LandRIN = mObjLandData.LandRIN,
                        PlotNumber = mObjLandData.PlotNumber,
                        StreetName = mObjLandData.StreetName.Trim(),
                        TownName = mObjLandData.TownName,
                        LGAName = mObjLandData.LGAName,
                        WardName = mObjLandData.WardName,
                        AssetTypeName = mObjLandData.AssetTypeName,
                        LandSize_Length = mObjLandData.LandSize_Length,
                        LandSize_Width = mObjLandData.LandSize_Width,
                        C_OF_O_Ref = mObjLandData.C_OF_O_Ref,
                        LandPurposeName = mObjLandData.LandPurposeName,
                        LandFunctionName = mObjLandData.LandFunctionName,
                        LandOwnershipName = mObjLandData.LandOwnershipName,
                        LandDevelopmentName = mObjLandData.LandDevelopmentName,
                        Latitude = mObjLandData.Latitude,
                        Longitude = mObjLandData.Longitude,
                        ValueOfLand = mObjLandData.ValueOfLand.GetValueOrDefault(),
                        LandStreetConditionName = mObjLandData.LandStreetConditionName,
                        Neighborhood = mObjLandData.Neighborhood,
                        ActiveText = mObjLandData.ActiveText
                    };

                    return View(mObjLandModelView);
                }
                else
                {
                    return RedirectToAction("List", "Land");
                }
            }
            else
            {
                return RedirectToAction("List", "Land");
            }
        }

        public JsonResult UpdateStatus(Land pObjLandData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjLandData.LandID != 0)
            {
                FuncResponse mObjFuncResponse = new BLLand().BL_UpdateStatus(pObjLandData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["LandList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
                }

            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Action";
            }



            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TaxPayerList(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                {
                    AssetID = id.GetValueOrDefault(),
                    AssetTypeID = (int)EnumList.AssetTypes.Land
                };

                IList<usp_GetTaxPayerAssetList_Result> lstTaxPayerAsset = new BLTaxPayerAsset().BL_GetTaxPayerAssetList(mObjTaxPayerAsset);
                usp_GetLandList_Result mObjLandDetails = new BLLand().BL_GetLandDetails(new Land() { intStatus = 2, LandID = id.GetValueOrDefault() });
                if (lstTaxPayerAsset != null)
                {
                    ViewBag.AssetID = id;
                    ViewBag.AssetRIN = name;
                    ViewBag.AssetName = mObjLandDetails.LandRIN;
                    return View(lstTaxPayerAsset);
                }
                else
                {
                    return RedirectToAction("List", "Land");
                }
            }
            else
            {
                return RedirectToAction("List", "Land");
            }
        }

        public void UI_FillDropDown(TaxPayerAssetViewModel pObjTPAModel)
        {
            UI_FillAssetTypeDropDown(new Asset_Types() { intStatus = 1 }, (int)EnumList.AssetTypes.Land);
            UI_FillTaxPayerTypeDropDown(new TaxPayer_Types() { intStatus = 1 });
            UI_FillTaxPayerRoleDropDown(new TaxPayer_Roles() { intStatus = 1, AssetTypeID = (int)EnumList.AssetTypes.Land });
        }

        public ActionResult AddTaxPayer(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                usp_GetLandList_Result mObjLandDetails = new BLLand().BL_GetLandDetails(new Land() { intStatus = 2, LandID = id.GetValueOrDefault() });
                TaxPayerAssetViewModel mObjTaxPayerAssetModel = new TaxPayerAssetViewModel()
                {
                    AssetID = id.GetValueOrDefault(),
                    AssetRIN = name,
                    AssetName = mObjLandDetails.LandRIN,
                    AssetTypeID = (int)EnumList.AssetTypes.Land
                };

                UI_FillDropDown(mObjTaxPayerAssetModel);
                return View(mObjTaxPayerAssetModel);
            }
            else
            {
                return RedirectToAction("TaxPayerList", "Land");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddTaxPayer(TaxPayerAssetViewModel pObjAssetModel)
        {
            if (!ModelState.IsValid)
            {
                pObjAssetModel.AssetTypeID = (int)EnumList.AssetTypes.Land;
                UI_FillDropDown(pObjAssetModel);
                return View(pObjAssetModel);
            }
            else
            {
                BLTaxPayerAsset mobjBLTaxPayerAsset = new BLTaxPayerAsset();
                string[] strTaxPayerIds = pObjAssetModel.TaxPayerIds.Split(',');

                foreach (var vTaxPayerID in strTaxPayerIds)
                {
                    if (!string.IsNullOrWhiteSpace(vTaxPayerID))
                    {
                        MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                        {
                            AssetTypeID = (int)EnumList.AssetTypes.Land,
                            AssetID = pObjAssetModel.AssetID,
                            TaxPayerTypeID = pObjAssetModel.TaxPayerTypeID,
                            TaxPayerRoleID = pObjAssetModel.TaxPayerRoleID,
                            TaxPayerID = TrynParse.parseInt(vTaxPayerID),
                            Active = true,
                            CreatedBy = SessionManager.SystemUserID,
                            CreatedDate = CommUtil.GetCurrentDateTime()
                        };

                        FuncResponse mObjResponse = mobjBLTaxPayerAsset.BL_InsertTaxPayerAsset(mObjTaxPayerAsset);
                    }
                }

                FlashMessage.Info("Tax Payer Linked Successfully");
                return RedirectToAction("TaxPayerList", "Land", new { id = pObjAssetModel.AssetID, name = pObjAssetModel.AssetName });
            }
        }

        public JsonResult GetTaxPayerList(MAP_TaxPayer_Asset pObjAssetModel)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            //Get Role Details
            usp_GetTaxPayerRoleList_Result mObjTaxPayerRoleDetails = new BLTaxPayerRole().BL_GetTaxPayerRoleDetails(new TaxPayer_Roles() { intStatus = 2, TaxPayerRoleID = pObjAssetModel.TaxPayerRoleID.GetValueOrDefault() });

            bool blnisAlreadyLinked = false;

            if (!mObjTaxPayerRoleDetails.IsMultiLinkable.GetValueOrDefault())
            {
                FuncResponse mObjFuncResponse = new BLTaxPayerAsset().BL_CheckAssetAlreadyLinked(new MAP_TaxPayer_Asset() { TaxPayerTypeID = pObjAssetModel.TaxPayerTypeID, TaxPayerID = pObjAssetModel.TaxPayerID, AssetTypeID = (int)EnumList.AssetTypes.Land, AssetID = pObjAssetModel.AssetID, TaxPayerRoleID = pObjAssetModel.TaxPayerRoleID });

                if (!mObjFuncResponse.Success)
                {
                    blnisAlreadyLinked = true;
                    dcResponse["success"] = false;
                    dcResponse["Message"] = "Single Role allowed - " + mObjTaxPayerRoleDetails.TaxPayerRoleName + " already exist for this asset";
                }
            }

            if (!blnisAlreadyLinked)
            {
                if (pObjAssetModel.TaxPayerTypeID == (int)EnumList.TaxPayerType.Individual)
                {
                    Individual mObjIndividual = new Individual()
                    {
                        intStatus = 2
                    };

                    IList<usp_GetIndividualList_Result> lstIndividual = new BLIndividual().BL_GetIndividualList(mObjIndividual);
                    dcResponse["success"] = true;
                    if (!mObjTaxPayerRoleDetails.IsMultiLinkable.GetValueOrDefault())
                    {
                        dcResponse["TaxPayerList"] = CommUtil.RenderPartialToString("_BindIndividualTable_SingleSelect", lstIndividual, this.ControllerContext);
                    }
                    else
                    {
                        dcResponse["TaxPayerList"] = CommUtil.RenderPartialToString("_BindIndividualTable_MultiSelect", lstIndividual, this.ControllerContext);
                    }
                }
                else if (pObjAssetModel.TaxPayerTypeID == (int)EnumList.TaxPayerType.Companies)
                {
                    Company mObjCompany = new Company()
                    {
                        intStatus = 2
                    };

                    IList<usp_GetCompanyList_Result> lstCompany = new BLCompany().BL_GetCompanyList(mObjCompany);
                    dcResponse["success"] = true;

                    if (!mObjTaxPayerRoleDetails.IsMultiLinkable.GetValueOrDefault())
                    {
                        dcResponse["TaxPayerList"] = CommUtil.RenderPartialToString("_BindCompanyTable_SingleSelect", lstCompany, this.ControllerContext);
                    }
                    else
                    {
                        dcResponse["TaxPayerList"] = CommUtil.RenderPartialToString("_BindCompanyTable_MultiSelect", lstCompany, this.ControllerContext);
                    }


                }
                else if (pObjAssetModel.TaxPayerTypeID == (int)EnumList.TaxPayerType.Government)
                {
                    Government mObjGovernment = new Government()
                    {
                        intStatus = 2
                    };

                    IList<usp_GetGovernmentList_Result> lstGovernment = new BLGovernment().BL_GetGovernmentList(mObjGovernment);
                    dcResponse["success"] = true;

                    if (!mObjTaxPayerRoleDetails.IsMultiLinkable.GetValueOrDefault())
                    {
                        dcResponse["TaxPayerList"] = CommUtil.RenderPartialToString("_BindGovernmentTable_SingleSelect", lstGovernment, this.ControllerContext);
                    }
                    else
                    {
                        dcResponse["TaxPayerList"] = CommUtil.RenderPartialToString("_BindGovernmentTable_MultiSelect", lstGovernment, this.ControllerContext);
                    }
                }
                else if (pObjAssetModel.TaxPayerTypeID == (int)EnumList.TaxPayerType.Special)
                {
                    Special mObjSpecial = new Special()
                    {
                        intStatus = 2
                    };

                    IList<usp_GetSpecialList_Result> lstSpecial = new BLSpecial().BL_GetSpecialList(mObjSpecial);
                    dcResponse["success"] = true;

                    if (!mObjTaxPayerRoleDetails.IsMultiLinkable.GetValueOrDefault())
                    {
                        dcResponse["TaxPayerList"] = CommUtil.RenderPartialToString("_BindSpecialTable_SingleSelect", lstSpecial, this.ControllerContext);
                    }
                    else
                    {
                        dcResponse["TaxPayerList"] = CommUtil.RenderPartialToString("_BindSpecialTable_MultiSelect", lstSpecial, this.ControllerContext);
                    }
                }
                else
                {
                    dcResponse["success"] = false;
                    dcResponse["Message"] = "Invalid Request";
                }
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateTaxPayerStatus(MAP_TaxPayer_Asset pObjTaxPayerData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjTaxPayerData.TPAID != 0)
            {
                pObjTaxPayerData.AssetTypeID = (int)EnumList.AssetTypes.Land;
                pObjTaxPayerData.IntStatus = 2;
                FuncResponse<IList<usp_GetTaxPayerAssetList_Result>> mObjFuncResponse = new BLTaxPayerAsset().BL_UpdateTaxPayerAssetStatus(pObjTaxPayerData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["AssetList"] = CommUtil.RenderPartialToString("_BindTaxPayerTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
                }

            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Action";
            }



            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RemoveTaxPayer(MAP_TaxPayer_Asset pObjTaxPayerData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjTaxPayerData.TPAID != 0)
            {
                pObjTaxPayerData.AssetTypeID = (int)EnumList.AssetTypes.Land;
                FuncResponse<IList<usp_GetTaxPayerAssetList_Result>> mObjFuncResponse = new BLTaxPayerAsset().BL_RemoveTaxPayerAsset(pObjTaxPayerData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["AssetList"] = CommUtil.RenderPartialToString("_BindTaxPayerTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
                }

            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Action";
            }



            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTaxPayerDetails(int TPAID)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            MAP_TaxPayer_Asset mObjTaxPayer = new BLTaxPayerAsset().BL_GetTaxPayerAssetDetails(TPAID);

            if (mObjTaxPayer != null)
            {
                dcResponse["TaxPayerTypeID"] = mObjTaxPayer.TaxPayerTypeID;
                dcResponse["success"] = true;
                if (mObjTaxPayer.TaxPayerTypeID == (int)EnumList.TaxPayerType.Individual)
                {
                    dcResponse["TaxPayerDetails"] = new BLIndividual().BL_GetIndividualDetails(new Individual() { intStatus = 2, IndividualID = mObjTaxPayer.TaxPayerID.GetValueOrDefault() });
                }
                else if (mObjTaxPayer.TaxPayerTypeID == (int)EnumList.TaxPayerType.Companies)
                {
                    dcResponse["TaxPayerDetails"] = new BLCompany().BL_GetCompanyDetails(new Company() { intStatus = 2, CompanyID = mObjTaxPayer.TaxPayerID.GetValueOrDefault() });
                }
                else if (mObjTaxPayer.TaxPayerTypeID == (int)EnumList.TaxPayerType.Government)
                {
                    dcResponse["TaxPayerDetails"] = new BLGovernment().BL_GetGovernmentDetails(new Government() { intStatus = 2, GovernmentID = mObjTaxPayer.TaxPayerID.GetValueOrDefault() });
                }
                else if (mObjTaxPayer.TaxPayerTypeID == (int)EnumList.TaxPayerType.Special)
                {
                    dcResponse["TaxPayerDetails"] = new BLSpecial().BL_GetSpecialDetails(new Special() { intStatus = 2, SpecialID = mObjTaxPayer.TaxPayerID.GetValueOrDefault() });
                }
            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Response";
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }
    }
}