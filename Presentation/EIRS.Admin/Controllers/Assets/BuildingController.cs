using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;
using Elmah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using Vereyon.Web;
using System.Linq.Dynamic;
using System.Text;

namespace EIRS.Admin.Controllers
{
    public class BuildingController : BaseController
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

            if (!string.IsNullOrWhiteSpace(Request.Form["BuildingRIN"]))
            {
                sbWhereCondition.Append(" AND ISNULL(BuildingRIN,'') LIKE @BuildingRIN");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["BuildingTagNumber"]))
            {
                sbWhereCondition.Append(" AND ISNULL(BuildingTagNumber,'') LIKE @BuildingTagNumber");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["BuildingName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(BuildingName,'') LIKE @BuildingName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["BuildingNumber"]))
            {
                sbWhereCondition.Append(" AND ISNULL(BuildingNumber,'') LIKE @BuildingNumber");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["StreetName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(StreetName,'') LIKE @StreetName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["OffStreetName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(OffStreetName,'') LIKE @OffStreetName");
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
            if (!string.IsNullOrWhiteSpace(Request.Form["BuildingTypeName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(BuildingTypeName,'') LIKE @BuildingTypeName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["BuildingCompletionName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(BuildingCompletionName,'') LIKE @BuildingCompletionName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["BuildingPurposeName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(BuildingPurposeName,'') LIKE @BuildingPurposeName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["BuildingOwnershipName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(BuildingOwnershipName,'') LIKE @BuildingOwnershipName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["NoOfUnits"]))
            {
                sbWhereCondition.Append(" AND ISNULL(NoOfUnits,'') LIKE @NoOfUnits");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["Latitude"]))
            {
                sbWhereCondition.Append(" AND ISNULL(Latitude,'') LIKE @Latitude");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["Longitude"]))
            {
                sbWhereCondition.Append(" AND ISNULL(Longitude,'') LIKE @Longitude");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["BuildingSize_Length"]))
            {
                sbWhereCondition.Append(" AND ISNULL(Convert(varchar(50),BuildingSize_Length,106),'') LIKE @BuildingSize_Length");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["BuildingSize_Width"]))
            {
                sbWhereCondition.Append(" AND ISNULL(Convert(varchar(50),BuildingSize_Width,106),'') LIKE @BuildingSize_Width");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["ActiveText"]))
            {
                sbWhereCondition.Append(" AND CASE WHEN ISNULL(bld.Active,0) = 0 THEN 'In Active' ELSE 'Active' END LIKE @ActiveText");
            }


            if (!string.IsNullOrEmpty(vFilter))
            {
                sbWhereCondition.Append(" AND ( ISNULL(BuildingRIN,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(BuildingTagNumber,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(BuildingName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(BuildingNumber,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(StreetName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(OffStreetName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(TownName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(LGAName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(WardName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(BuildingTypeName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(BuildingCompletionName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(BuildingPurposeName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(BuildingOwnershipName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(NoOfUnits,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(Latitude,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(Longitude,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(Convert(varchar(50),BuildingSize_Length,106),'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(Convert(varchar(50),BuildingSize_Width,106),'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR CASE WHEN ISNULL(bld.Active,0) = 0 THEN 'In Active' ELSE 'Active' END LIKE @MainFilter)");
            }

            Building mObjBuilding = new Building()
            {
                WhereCondition = sbWhereCondition.ToString(),
                OrderBy = vSortColumn,
                OrderByDirection = vSortColumnDir,
                PageNumber = (TrynParse.parseInt(vStart) / TrynParse.parseInt(vLength)) + 1,
                PageSize = TrynParse.parseInt(vLength),


                BuildingRIN = !string.IsNullOrWhiteSpace(Request.Form["BuildingRIN"]) ? "%" + Request.Form["BuildingRIN"].Trim() + "%" : TrynParse.parseString(Request.Form["BuildingRIN"]),
                BuildingTagNumber = !string.IsNullOrWhiteSpace(Request.Form["BuildingTagNumber"]) ? "%" + Request.Form["BuildingTagNumber"].Trim() + "%" : TrynParse.parseString(Request.Form["BuildingTagNumber"]),
                BuildingName = !string.IsNullOrWhiteSpace(Request.Form["BuildingName"]) ? "%" + Request.Form["BuildingName"].Trim() + "%" : TrynParse.parseString(Request.Form["BuildingName"]),
                BuildingNumber = !string.IsNullOrWhiteSpace(Request.Form["BuildingNumber"]) ? "%" + Request.Form["BuildingNumber"].Trim() + "%" : TrynParse.parseString(Request.Form["BuildingNumber"]),
                StreetName = !string.IsNullOrWhiteSpace(Request.Form["StreetName"]) ? "%" + Request.Form["StreetName"].Trim() + "%" : TrynParse.parseString(Request.Form["StreetName"]),
                OffStreetName = !string.IsNullOrWhiteSpace(Request.Form["OffStreetName"]) ? "%" + Request.Form["OffStreetName"].Trim() + "%" : TrynParse.parseString(Request.Form["OffStreetName"]),
                TownName = !string.IsNullOrWhiteSpace(Request.Form["TownName"]) ? "%" + Request.Form["TownName"].Trim() + "%" : TrynParse.parseString(Request.Form["TownName"]),
                LGAName = !string.IsNullOrWhiteSpace(Request.Form["LGAName"]) ? "%" + Request.Form["LGAName"].Trim() + "%" : TrynParse.parseString(Request.Form["LGAName"]),
                WardName = !string.IsNullOrWhiteSpace(Request.Form["WardName"]) ? "%" + Request.Form["WardName"].Trim() + "%" : TrynParse.parseString(Request.Form["WardName"]),
                BuildingTypeName = !string.IsNullOrWhiteSpace(Request.Form["BuildingTypeName"]) ? "%" + Request.Form["BuildingTypeName"].Trim() + "%" : TrynParse.parseString(Request.Form["BuildingTypeName"]),
                BuildingCompletionName = !string.IsNullOrWhiteSpace(Request.Form["BuildingCompletionName"]) ? "%" + Request.Form["BuildingCompletionName"].Trim() + "%" : TrynParse.parseString(Request.Form["BuildingCompletionName"]),
                BuildingPurposeName = !string.IsNullOrWhiteSpace(Request.Form["BuildingPurposeName"]) ? "%" + Request.Form["BuildingPurposeName"].Trim() + "%" : TrynParse.parseString(Request.Form["BuildingPurposeName"]),
                BuildingOwnershipName = !string.IsNullOrWhiteSpace(Request.Form["BuildingOwnershipName"]) ? "%" + Request.Form["BuildingOwnershipName"].Trim() + "%" : TrynParse.parseString(Request.Form["BuildingOwnershipName"]),
                strNoOfUnits = !string.IsNullOrWhiteSpace(Request.Form["NoOfUnits"]) ? "%" + Request.Form["NoOfUnits"].Trim() + "%" : TrynParse.parseString(Request.Form["NoOfUnits"]),
                strBuildingSize_Length = !string.IsNullOrWhiteSpace(Request.Form["BuildingSize_Length"]) ? "%" + Request.Form["BuildingSize_Length"].Trim() + "%" : TrynParse.parseString(Request.Form["BuildingSize_Length"]),
                strBuildingSize_Width = !string.IsNullOrWhiteSpace(Request.Form["BuildingSize_Width"]) ? "%" + Request.Form["BuildingSize_Width"].Trim() + "%" : TrynParse.parseString(Request.Form["BuildingSize_Width"]),
                Latitude = !string.IsNullOrWhiteSpace(Request.Form["Latitude"]) ? "%" + Request.Form["Latitude"].Trim() + "%" : TrynParse.parseString(Request.Form["Latitude"]),
                Longitude = !string.IsNullOrWhiteSpace(Request.Form["Longitude"]) ? "%" + Request.Form["Longitude"].Trim() + "%" : TrynParse.parseString(Request.Form["Longitude"]),
                ActiveText = !string.IsNullOrWhiteSpace(Request.Form["ActiveText"]) ? "%" + Request.Form["ActiveText"].Trim() + "%" : TrynParse.parseString(Request.Form["ActiveText"]),
                MainFilter = !string.IsNullOrWhiteSpace(vFilter) ? "%" + vFilter.Trim() + "%" : vFilter
            };

            IDictionary<string, object> dcData = new BLBuilding().BL_SearchBuilding(mObjBuilding);
            IList<usp_SearchBuilding_Result> lstBuilding = (IList<usp_SearchBuilding_Result>)dcData["BuildingList"];

            return Json(new
            {
                draw = vDraw,
                recordsFiltered = dcData["FilteredRecords"],
                recordsTotal = dcData["TotalRecords"],
                data = lstBuilding
            }, JsonRequestBehavior.AllowGet);
        }


        public void UI_FillDropDown(BuildingViewModel pObjBuildingViewModel = null)
        {
            if (pObjBuildingViewModel != null)
                pObjBuildingViewModel.AssetTypeID = (int)EnumList.AssetTypes.Building;
            else if (pObjBuildingViewModel == null)
                pObjBuildingViewModel = new BuildingViewModel();

            UI_FillTownDropDown(new Town() { intStatus = 1, IncludeTownIds = pObjBuildingViewModel.TownID.ToString() });
            UI_FillLGADropDown(new LGA() { intStatus = 1, IncludeLGAIds = pObjBuildingViewModel.LGAID.ToString() });
            UI_FillWardDropDown(new Ward() { intStatus = 1, LGAID = pObjBuildingViewModel.LGAID, IncludeWardIds = pObjBuildingViewModel.WardID.ToString() });
            UI_FillAssetTypeDropDown(new Asset_Types() { intStatus = 1, IncludeAssetTypeIds = pObjBuildingViewModel.AssetTypeID.ToString() }, (int)EnumList.AssetTypes.Building);
            UI_FillBuildingTypeDropDown(new Building_Types() { intStatus = 1, IncludeBuildingTypeIds = pObjBuildingViewModel.BuildingTypeID.ToString() });
            UI_FillBuildingCompletionDropDown(new Building_Completion() { intStatus = 1, IncludeBuildingCompletionIds = pObjBuildingViewModel.BuildingCompletionID.ToString() });
            UI_FillBuildingPurposeDropDown(new Building_Purpose() { intStatus = 1, IncludeBuildingPurposeIds = pObjBuildingViewModel.BuildingPurposeID.ToString() });
            UI_FillBuildingOwnershipDropDown(new Building_Ownership() { intStatus = 1, IncludeBuildingOwnershipIds = pObjBuildingViewModel.BuildingOwnershipID.ToString() });
        }

        public ActionResult Add()
        {
            UI_FillDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(BuildingViewModel pObjBuildingModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown(pObjBuildingModel);
                return View(pObjBuildingModel);
            }
            else
            {
                using (TransactionScope mObjTransactionScope = new TransactionScope())
                {
                    BLBuilding mObjBLBuilding = new BLBuilding();

                    Building mObjBuilding = new Building()
                    {
                        BuildingID = 0,
                        BuildingName = pObjBuildingModel.BuildingName != null ? pObjBuildingModel.BuildingName.Trim() : pObjBuildingModel.BuildingName,
                        BuildingNumber = pObjBuildingModel.BuildingNumber.Trim(),
                        StreetName = pObjBuildingModel.StreetName.Trim(),
                        OffStreetName = pObjBuildingModel.OffStreetName != null ? pObjBuildingModel.OffStreetName.Trim() : pObjBuildingModel.OffStreetName,
                        TownID = pObjBuildingModel.TownID,
                        LGAID = pObjBuildingModel.LGAID,
                        WardID = pObjBuildingModel.WardID,
                        AssetTypeID = (int)EnumList.AssetTypes.Building,
                        BuildingTypeID = pObjBuildingModel.BuildingTypeID,
                        BuildingCompletionID = pObjBuildingModel.BuildingCompletionID,
                        BuildingPurposeID = pObjBuildingModel.BuildingPurposeID,
                        BuildingOwnershipID = pObjBuildingModel.BuildingOwnershipID,
                        NoOfUnits = pObjBuildingModel.NoOfUnits,
                        BuildingSize_Length = pObjBuildingModel.BuildingSize_Length,
                        BuildingSize_Width = pObjBuildingModel.BuildingSize_Width,
                        Latitude = pObjBuildingModel.Latitude,
                        Longitude = pObjBuildingModel.Longitude,
                        Active = true,
                        CreatedBy = SessionManager.SystemUserID,
                        CreatedDate = CommUtil.GetCurrentDateTime()
                    };

                    try
                    {

                        FuncResponse<Building> mObjResponse = mObjBLBuilding.BL_InsertUpdateBuilding(mObjBuilding);

                        if (mObjResponse.Success)
                        {
                            mObjTransactionScope.Complete();
                            FlashMessage.Info(mObjResponse.Message);
                            return RedirectToAction("List", "Building");
                        }
                        else
                        {
                            UI_FillDropDown(pObjBuildingModel);
                            Transaction.Current.Rollback();
                            ViewBag.Message = mObjResponse.Message;
                            return View(pObjBuildingModel);
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorSignal.FromCurrentContext().Raise(ex);
                        UI_FillDropDown(pObjBuildingModel);
                        Transaction.Current.Rollback();
                        ViewBag.Message = "Error occurred while saving building";
                        return View(pObjBuildingModel);
                    }
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Building mObjBuilding = new Building()
                {
                    BuildingID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetBuildingList_Result mObjBuildingData = new BLBuilding().BL_GetBuildingDetails(mObjBuilding);

                if (mObjBuildingData != null)
                {
                    BuildingViewModel mObjBuildingModelView = new BuildingViewModel()
                    {
                        BuildingID = mObjBuildingData.BuildingID.GetValueOrDefault(),
                        BuildingRIN = mObjBuildingData.BuildingRIN,
                        BuildingTAGNumber = mObjBuildingData.BuildingTagNumber,
                        BuildingName = mObjBuildingData.BuildingName,
                        BuildingNumber = mObjBuildingData.BuildingNumber.Trim(),
                        StreetName = mObjBuildingData.StreetName.Trim(),
                        OffStreetName = mObjBuildingData.OffStreetName,
                        TownID = mObjBuildingData.TownID.GetValueOrDefault(),
                        LGAID = mObjBuildingData.LGAID.GetValueOrDefault(),
                        WardID = mObjBuildingData.WardID.GetValueOrDefault(),
                        AssetTypeID = mObjBuildingData.AssetTypeID.GetValueOrDefault(),
                        BuildingTypeID = mObjBuildingData.BuildingTypeID.GetValueOrDefault(),
                        BuildingCompletionID = mObjBuildingData.BuildingCompletionID.GetValueOrDefault(),
                        BuildingPurposeID = mObjBuildingData.BuildingPurposeID.GetValueOrDefault(),
                        NoOfUnits = mObjBuildingData.NoOfUnits.GetValueOrDefault(),
                        BuildingOwnershipID = mObjBuildingData.BuildingOwnershipID.GetValueOrDefault(),
                        BuildingSize_Length = mObjBuildingData.BuildingSize_Length,
                        BuildingSize_Width = mObjBuildingData.BuildingSize_Width,
                        Latitude = mObjBuildingData.Latitude,
                        Longitude = mObjBuildingData.Longitude,
                        Active = mObjBuildingData.Active.GetValueOrDefault(),
                    };

                    UI_FillDropDown(mObjBuildingModelView);
                    return View(mObjBuildingModelView);
                }
                else
                {
                    return RedirectToAction("List", "Building");
                }
            }
            else
            {
                return RedirectToAction("List", "Building");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(BuildingViewModel pObjBuildingModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown(pObjBuildingModel);
                return View(pObjBuildingModel);
            }
            else
            {
                using (TransactionScope mObjTransactionScope = new TransactionScope())
                {
                    BLBuilding mObjBLBuilding = new BLBuilding();

                    Building mObjBuilding = new Building()
                    {
                        BuildingID = pObjBuildingModel.BuildingID,
                        BuildingName = pObjBuildingModel.BuildingName != null ? pObjBuildingModel.BuildingName.Trim() : pObjBuildingModel.BuildingName,
                        BuildingNumber = pObjBuildingModel.BuildingNumber.Trim(),
                        StreetName = pObjBuildingModel.StreetName.Trim(),
                        OffStreetName = pObjBuildingModel.OffStreetName != null ? pObjBuildingModel.OffStreetName.Trim() : pObjBuildingModel.OffStreetName,
                        TownID = pObjBuildingModel.TownID,
                        LGAID = pObjBuildingModel.LGAID,
                        WardID = pObjBuildingModel.WardID,
                        AssetTypeID = (int)EnumList.AssetTypes.Building,
                        BuildingTypeID = pObjBuildingModel.BuildingTypeID,
                        BuildingCompletionID = pObjBuildingModel.BuildingCompletionID,
                        BuildingPurposeID = pObjBuildingModel.BuildingPurposeID,
                        BuildingOwnershipID = pObjBuildingModel.BuildingOwnershipID,
                        NoOfUnits = pObjBuildingModel.NoOfUnits,
                        BuildingSize_Length = pObjBuildingModel.BuildingSize_Length,
                        BuildingSize_Width = pObjBuildingModel.BuildingSize_Width,
                        Latitude = pObjBuildingModel.Latitude,
                        Longitude = pObjBuildingModel.Longitude,
                        Active = pObjBuildingModel.Active,
                        ModifiedBy = SessionManager.SystemUserID,
                        ModifiedDate = CommUtil.GetCurrentDateTime()
                    };

                    try
                    {

                        FuncResponse<Building> mObjResponse = mObjBLBuilding.BL_InsertUpdateBuilding(mObjBuilding);

                        if (mObjResponse.Success)
                        {
                            mObjTransactionScope.Complete();
                            FlashMessage.Info(mObjResponse.Message);
                            return RedirectToAction("List", "Building");
                        }
                        else
                        {
                            UI_FillDropDown(pObjBuildingModel);
                            Transaction.Current.Rollback();
                            ViewBag.Message = mObjResponse.Message;
                            return View(pObjBuildingModel);
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorSignal.FromCurrentContext().Raise(ex);
                        UI_FillDropDown(pObjBuildingModel);
                        Transaction.Current.Rollback();
                        ViewBag.Message = "Error occurred while saving building";
                        return View(pObjBuildingModel);
                    }
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Building mObjBuilding = new Building()
                {
                    BuildingID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetBuildingList_Result mObjBuildingData = new BLBuilding().BL_GetBuildingDetails(mObjBuilding);

                if (mObjBuildingData != null)
                {
                    BuildingViewModel mObjBuildingModelView = new BuildingViewModel()
                    {
                        BuildingID = mObjBuildingData.BuildingID.GetValueOrDefault(),
                        BuildingRIN = mObjBuildingData.BuildingRIN,
                        BuildingTAGNumber = mObjBuildingData.BuildingTagNumber,
                        BuildingName = mObjBuildingData.BuildingName,
                        BuildingNumber = mObjBuildingData.BuildingNumber.Trim(),
                        StreetName = mObjBuildingData.StreetName.Trim(),
                        OffStreetName = mObjBuildingData.OffStreetName,
                        TownName = mObjBuildingData.TownName,
                        LGAName = mObjBuildingData.LGAName,
                        WardName = mObjBuildingData.WardName,
                        AssetTypeName = mObjBuildingData.AssetTypeName,
                        BuildingTypeName = mObjBuildingData.BuildingTypeName,
                        BuildingCompletionName = mObjBuildingData.BuildingCompletionName,
                        BuildingPurposeName = mObjBuildingData.BuildingPurposeName,
                        NoOfUnits = mObjBuildingData.NoOfUnits.GetValueOrDefault(),
                        BuildingOwnershipName = mObjBuildingData.BuildingOwnershipName,
                        Latitude = mObjBuildingData.Latitude,
                        Longitude = mObjBuildingData.Longitude,
                        ActiveText = mObjBuildingData.ActiveText
                    };

                    return View(mObjBuildingModelView);
                }
                else
                {
                    return RedirectToAction("List", "Building");
                }
            }
            else
            {
                return RedirectToAction("List", "Building");
            }
        }

        public JsonResult UpdateStatus(Building pObjBuildingData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjBuildingData.BuildingID != 0)
            {
                FuncResponse mObjFuncResponse = new BLBuilding().BL_UpdateStatus(pObjBuildingData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["BuildingList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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
                    AssetTypeID = (int)EnumList.AssetTypes.Building
                };

                IList<usp_GetTaxPayerAssetList_Result> lstTaxPayerAsset = new BLTaxPayerAsset().BL_GetTaxPayerAssetList(mObjTaxPayerAsset);

                usp_GetBuildingList_Result mObjBuildingDetails = new BLBuilding().BL_GetBuildingDetails(new Building() { intStatus = 2, BuildingID = id.GetValueOrDefault() });

                if (mObjBuildingDetails != null)
                {
                    ViewBag.AssetID = id;
                    ViewBag.AssetRIN = name;
                    ViewBag.AssetName = mObjBuildingDetails.BuildingName;
                    return View(lstTaxPayerAsset);
                }
                else
                {
                    return RedirectToAction("List", "Building");
                }
            }
            else
            {
                return RedirectToAction("List", "Building");
            }
        }

        public void UI_FillDropDown(TaxPayerAssetViewModel pObjTPAModel)
        {
            UI_FillAssetTypeDropDown(new Asset_Types() { intStatus = 1 }, (int)EnumList.AssetTypes.Building);
            UI_FillTaxPayerTypeDropDown(new TaxPayer_Types() { intStatus = 1 });
            UI_FillTaxPayerRoleDropDown(new TaxPayer_Roles() { intStatus = 1, AssetTypeID = (int)EnumList.AssetTypes.Building });

            IList<usp_GetBuildingUnitNumberList_Result> lstBuildingUnit = new BLBuilding().BL_GetBuildingUnitNumberList(new MAP_Building_BuildingUnit() { BuildingID = pObjTPAModel.AssetID });
            ViewBag.BuildingUnitList = new SelectList(lstBuildingUnit, "BuildingUnitID", "UnitNumber");
        }

        public ActionResult AddTaxPayer(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                usp_GetBuildingList_Result mObjBuildingDetails = new BLBuilding().BL_GetBuildingDetails(new Building() { intStatus = 2, BuildingID = id.GetValueOrDefault() });
                TaxPayerAssetViewModel mObjTaxPayerAssetModel = new TaxPayerAssetViewModel()
                {
                    AssetID = id.GetValueOrDefault(),
                    AssetRIN = name,
                    AssetName = mObjBuildingDetails.BuildingName,
                    AssetTypeID = (int)EnumList.AssetTypes.Building
                };

                UI_FillDropDown(mObjTaxPayerAssetModel);
                return View(mObjTaxPayerAssetModel);
            }
            else
            {
                return RedirectToAction("TaxPayerList", "Building");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddTaxPayer(TaxPayerAssetViewModel pObjAssetModel)
        {
            if (!ModelState.IsValid)
            {
                pObjAssetModel.AssetTypeID = (int)EnumList.AssetTypes.Building;
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
                            AssetTypeID = (int)EnumList.AssetTypes.Building,
                            AssetID = pObjAssetModel.AssetID,
                            TaxPayerTypeID = pObjAssetModel.TaxPayerTypeID,
                            TaxPayerRoleID = pObjAssetModel.TaxPayerRoleID,
                            TaxPayerID = TrynParse.parseInt(vTaxPayerID),
                            BuildingUnitID = pObjAssetModel.BuildingUnitID,
                            Active = true,
                            CreatedBy = SessionManager.SystemUserID,
                            CreatedDate = CommUtil.GetCurrentDateTime()
                        };

                        FuncResponse mObjResponse = mobjBLTaxPayerAsset.BL_InsertTaxPayerAsset(mObjTaxPayerAsset);
                    }
                }

                FlashMessage.Info("Tax Payer Linked Successfully");
                return RedirectToAction("TaxPayerList", "Building", new { id = pObjAssetModel.AssetID, name = pObjAssetModel.AssetName });
            }
        }


        public ActionResult BusinessInformation(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                IList<usp_GetBusinessBuildingList_Result> lstBusinessInformation = new BLBusiness().BL_GetBusinessBuildingList(new MAP_Business_Building() { BuildingID = id.GetValueOrDefault() });
                usp_GetBuildingList_Result mObjBuildingDetails = new BLBuilding().BL_GetBuildingDetails(new Building() { intStatus = 2, BuildingID = id.GetValueOrDefault() });

                if (mObjBuildingDetails != null)
                {
                    ViewBag.BuildingID = id;
                    ViewBag.BuildingRIN = name;
                    ViewBag.BuildingName = mObjBuildingDetails.BuildingName;
                    return View(lstBusinessInformation);
                }
                else
                {
                    return RedirectToAction("List", "Building");
                }
            }
            else
            {
                return RedirectToAction("List", "Building");
            }
        }

        public ActionResult AddBusiness(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                usp_GetBuildingList_Result mObjBuildingDetails = new BLBuilding().BL_GetBuildingDetails(new Building() { intStatus = 2, BuildingID = id.GetValueOrDefault() });

                BusinessBuildingViewModel mObjBusinessModel = new BusinessBuildingViewModel()
                {
                    BuildingID = id.GetValueOrDefault(),
                    BuildingRIN = name,
                    BuildingName = mObjBuildingDetails.BuildingName,
                };

                IList<usp_GetBuildingUnitNumberList_Result> lstBuildingUnitNumberList = new BLBuilding().BL_GetBuildingUnitNumberList(new MAP_Building_BuildingUnit() { BuildingID = id.GetValueOrDefault() });
                ViewBag.BuildingUnitList = lstBuildingUnitNumberList;

                return View(mObjBusinessModel);
            }
            else
            {
                return RedirectToAction("List", "Building");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddBusiness(BusinessBuildingViewModel pObjBusinessModel)
        {
            if (!ModelState.IsValid)
            {
                IList<usp_GetBuildingUnitNumberList_Result> lstBuildingUnitNumberList = new BLBuilding().BL_GetBuildingUnitNumberList(new MAP_Building_BuildingUnit() { BuildingID = pObjBusinessModel.BuildingID });
                ViewBag.BuildingUnitList = lstBuildingUnitNumberList;
                return View(pObjBusinessModel);
            }
            else
            {
                MAP_Business_Building mObjBusiness = new MAP_Business_Building()
                {
                    BusinessID = pObjBusinessModel.BusinessID,
                    BuildingID = pObjBusinessModel.BuildingID,
                    BuildingUnitID = pObjBusinessModel.BuildingUnitID,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                FuncResponse mObjResponse = new BLBusiness().BL_InsertBusinessBuilding(mObjBusiness);

                if (mObjResponse.Success)
                {
                    FlashMessage.Info("Business Information Added Successfully");
                    return RedirectToAction("BusinessInformation", "Building", new { id = pObjBusinessModel.BuildingID, name = pObjBusinessModel.BuildingRIN });
                }
                else
                {
                    IList<usp_GetBuildingUnitNumberList_Result> lstBuildingUnitNumberList = new BLBuilding().BL_GetBuildingUnitNumberList(new MAP_Building_BuildingUnit() { BuildingID = pObjBusinessModel.BuildingID });
                    ViewBag.BuildingUnitList = lstBuildingUnitNumberList;
                    ViewBag.Message = mObjResponse.Message;
                    return View(pObjBusinessModel);
                }
            }
        }


        public ActionResult LandInformation(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                IList<usp_GetBuildingLandList_Result> lstLandInformation = new BLBuilding().BL_GetBuildingLandList(new MAP_Building_Land() { BuildingID = id.GetValueOrDefault() });
                usp_GetBuildingList_Result mObjBuildingDetails = new BLBuilding().BL_GetBuildingDetails(new Building() { intStatus = 2, BuildingID = id.GetValueOrDefault() });

                if (mObjBuildingDetails != null)
                {
                    ViewBag.BuildingID = id;
                    ViewBag.BuildingRIN = name;
                    ViewBag.BuildingName = mObjBuildingDetails.BuildingName;
                    return View(lstLandInformation);
                }
                else
                {
                    return RedirectToAction("List", "Building");
                }
            }
            else
            {
                return RedirectToAction("List", "Building");
            }
        }

        public ActionResult AddLand(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                usp_GetBuildingList_Result mObjBuildingDetails = new BLBuilding().BL_GetBuildingDetails(new Building() { intStatus = 2, BuildingID = id.GetValueOrDefault() });
                BuildingLandViewModel mObjLandModel = new BuildingLandViewModel()
                {
                    BuildingID = id.GetValueOrDefault(),
                    BuildingRIN = name,
                    BuildingName = mObjBuildingDetails.BuildingName,
                };

                IList<usp_GetLandList_Result> lstLand = new BLLand().BL_GetLandList(new Land() { intStatus = 1 });
                ViewBag.LandList = lstLand;
                return View(mObjLandModel);
            }
            else
            {
                return RedirectToAction("List", "Building");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddLand(BuildingLandViewModel pObjLandModel)
        {
            if (!ModelState.IsValid)
            {
                IList<usp_GetLandList_Result> lstLand = new BLLand().BL_GetLandList(new Land() { intStatus = 1 });
                ViewBag.LandList = lstLand;
                return View(pObjLandModel);
            }
            else
            {
                MAP_Building_Land mObjLand = new MAP_Building_Land()
                {
                    LandID = pObjLandModel.LandID,
                    BuildingID = pObjLandModel.BuildingID,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                FuncResponse mObjResponse = new BLBuilding().BL_InsertBuildingLand(mObjLand);

                if (mObjResponse.Success)
                {
                    FlashMessage.Info("Land Information Added Successfully");
                    return RedirectToAction("LandInformation", "Building", new { id = pObjLandModel.BuildingID, name = pObjLandModel.BuildingRIN });
                }
                else
                {
                    IList<usp_GetLandList_Result> lstLand = new BLLand().BL_GetLandList(new Land() { intStatus = 1 });
                    ViewBag.LandList = lstLand;
                    ViewBag.Message = mObjResponse.Message;
                    return View(pObjLandModel);
                }
            }
        }

        public ActionResult UnitInformation(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                IList<usp_GetBuildingUnitNumberList_Result> lstUnitInformation = new BLBuilding().BL_GetBuildingUnitNumberList(new MAP_Building_BuildingUnit() { BuildingID = id.GetValueOrDefault() });
                usp_GetBuildingList_Result mObjBuildingDetails = new BLBuilding().BL_GetBuildingDetails(new Building() { intStatus = 2, BuildingID = id.GetValueOrDefault() });

                if (mObjBuildingDetails != null)
                {
                    ViewBag.BuildingID = id;
                    ViewBag.BuildingRIN = name;
                    ViewBag.BuildingName = mObjBuildingDetails.BuildingName;
                    return View(lstUnitInformation);
                }
                else
                {
                    return RedirectToAction("List", "Building");
                }
            }
            else
            {
                return RedirectToAction("List", "Building");
            }
        }

        public ActionResult AddUnit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                usp_GetBuildingList_Result mObjBuildingDetails = new BLBuilding().BL_GetBuildingDetails(new Building() { intStatus = 2, BuildingID = id.GetValueOrDefault() });
                BuildingUnitNumberViewModel mObjUnitModel = new BuildingUnitNumberViewModel()
                {
                    BuildingID = id.GetValueOrDefault(),
                    BuildingRIN = name,
                    BuildingName = mObjBuildingDetails.BuildingName,
                };

                IList<usp_GetBuildingUnitList_Result> lstBuildingUnit = new BLBuildingUnit().BL_GetBuildingUnitList(new Building_Unit() { intStatus = 1 });
                ViewBag.BuildingUnitList = lstBuildingUnit;

                return View(mObjUnitModel);
            }
            else
            {
                return RedirectToAction("List", "Building");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddUnit(BuildingUnitNumberViewModel pObjUnitModel)
        {
            if (!ModelState.IsValid)
            {
                IList<usp_GetBuildingUnitList_Result> lstBuildingUnit = new BLBuildingUnit().BL_GetBuildingUnitList(new Building_Unit() { intStatus = 1 });
                ViewBag.BuildingUnitList = lstBuildingUnit;
                return View(pObjUnitModel);
            }
            else
            {
                string[] strUnitBuildingIds = pObjUnitModel.BuildingUnitIds.Split(',');

                foreach (var vBuildingUnitID in strUnitBuildingIds)
                {
                    if (!string.IsNullOrWhiteSpace(vBuildingUnitID))
                    {
                        MAP_Building_BuildingUnit mObjUnit = new MAP_Building_BuildingUnit()
                        {
                            BuildingUnitID = TrynParse.parseInt(vBuildingUnitID),
                            BuildingID = pObjUnitModel.BuildingID,
                            CreatedBy = SessionManager.SystemUserID,
                            CreatedDate = CommUtil.GetCurrentDateTime()
                        };

                        FuncResponse mObjResponse = new BLBuilding().BL_InsertBuildingUnitNumber(mObjUnit);
                    }
                }

                FlashMessage.Info("Unit Information Added Successfully");
                return RedirectToAction("UnitInformation", "Building", new { id = pObjUnitModel.BuildingID, name = pObjUnitModel.BuildingRIN });
            }
        }

        public JsonResult GetBuildingUnitList(int BuildingID)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            MAP_Building_BuildingUnit mObjBuildingUnit = new MAP_Building_BuildingUnit()
            {
                BuildingID = BuildingID
            };

            IList<usp_GetBuildingUnitNumberList_Result> lstBuildingUnitNumberList = new BLBuilding().BL_GetBuildingUnitNumberList(mObjBuildingUnit);
            dcResponse["success"] = true;
            dcResponse["BuildingUnitList"] = CommUtil.RenderPartialToString("_BindBuildingUnitTable_SingleSelect", lstBuildingUnitNumberList, this.ControllerContext);
            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTaxPayerList(MAP_TaxPayer_Asset pObjAssetModel)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            //Get Role Details
            usp_GetTaxPayerRoleList_Result mObjTaxPayerRoleDetails = new BLTaxPayerRole().BL_GetTaxPayerRoleDetails(new TaxPayer_Roles() { intStatus = 2, TaxPayerRoleID = pObjAssetModel.TaxPayerRoleID.GetValueOrDefault() });

            bool blnisAlreadyLinked = false;

            if (!mObjTaxPayerRoleDetails.IsMultiLinkable.GetValueOrDefault())
            {
                FuncResponse mObjFuncResponse = new BLTaxPayerAsset().BL_CheckAssetAlreadyLinked(new MAP_TaxPayer_Asset() { TaxPayerTypeID = pObjAssetModel.TaxPayerTypeID, TaxPayerID = pObjAssetModel.TaxPayerID, AssetTypeID = (int)EnumList.AssetTypes.Building, AssetID = pObjAssetModel.AssetID, TaxPayerRoleID = pObjAssetModel.TaxPayerRoleID });

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
                pObjTaxPayerData.AssetTypeID = (int)EnumList.AssetTypes.Building;
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
                pObjTaxPayerData.AssetTypeID = (int)EnumList.AssetTypes.Building;
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


        public JsonResult GetBusinessInformation(int BBID)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            MAP_Business_Building mObjBusiness = new BLBusiness().BL_GetBusinessBuildingDetails(BBID);

            if (mObjBusiness != null)
            {
                dcResponse["success"] = true;
                dcResponse["BusinessDetails"] = new BLBusiness().BL_GetBusinessDetails(new Business() { intStatus = 2, BusinessID = mObjBusiness.BuildingID.GetValueOrDefault() });

            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Response";
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBusinessList()
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            Business mObjBusiness = new Business()
            {
                intStatus = 1
            };

            IList<usp_GetBusinessListNewTy_Result> lstBusiness = new BLBusiness().BL_GetBusinessList(mObjBusiness);
            dcResponse["success"] = true;
            dcResponse["BusinessList"] = CommUtil.RenderPartialToString("_BindBusinessTable_SingleSelect", lstBusiness, this.ControllerContext);
            return Json(dcResponse, JsonRequestBehavior.AllowGet);

        }

        public JsonResult RemoveBusiness(MAP_Business_Building pObjBusinessData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjBusinessData.BBID != 0)
            {
                FuncResponse<IList<usp_GetBusinessBuildingList_Result>> mObjFuncResponse = new BLBusiness().BL_RemoveBusinessBuilding(pObjBusinessData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["BusinessList"] = CommUtil.RenderPartialToString("_BindBusinessTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
                }

            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Action";
            }



            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetLandInformation(int BLID)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            MAP_Building_Land mObjLand = new BLBuilding().BL_GetBuildingLandDetails(BLID);

            if (mObjLand != null)
            {
                dcResponse["success"] = true;
                dcResponse["LandDetails"] = new BLLand().BL_GetLandDetails(new Land() { intStatus = 2, LandID = mObjLand.BuildingID.GetValueOrDefault() });

            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Response";
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLandList()
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            Land mObjLand = new Land()
            {
                intStatus = 1
            };

            IList<usp_GetLandList_Result> lstLand = new BLLand().BL_GetLandList(mObjLand);
            dcResponse["success"] = true;
            dcResponse["LandList"] = CommUtil.RenderPartialToString("_BindLandTable_SingleSelect", lstLand, this.ControllerContext);
            return Json(dcResponse, JsonRequestBehavior.AllowGet);

        }

        public JsonResult RemoveLand(MAP_Building_Land pObjLandData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjLandData.BLID != 0)
            {
                FuncResponse<IList<usp_GetBuildingLandList_Result>> mObjFuncResponse = new BLBuilding().BL_RemoveBuildingLand(pObjLandData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["LandList"] = CommUtil.RenderPartialToString("_BindLandTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
                }

            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Action";
            }



            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetUnitInformation(int BBUID)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            MAP_Building_BuildingUnit mObjUnitNumber = new BLBuilding().BL_GetBuildingUnitNumberDetails(BBUID);

            if (mObjUnitNumber != null)
            {
                dcResponse["success"] = true;
                dcResponse["UnitDetails"] = new BLBuildingUnit().BL_GetBuildingUnitDetails(new Building_Unit() { intStatus = 2, BuildingUnitID = mObjUnitNumber.BuildingUnitID.GetValueOrDefault() });

            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Response";
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RemoveBuildingUnit(MAP_Building_BuildingUnit pObjUnitData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjUnitData.BBUID != 0)
            {
                FuncResponse<IList<usp_GetBuildingUnitNumberList_Result>> mObjFuncResponse = new BLBuilding().BL_RemoveBuildingUnitNumber(pObjUnitData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["BuildingUnitList"] = CommUtil.RenderPartialToString("_BindUnitTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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