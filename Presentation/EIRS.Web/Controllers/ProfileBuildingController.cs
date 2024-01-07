using EIRS.BLL;
using EIRS.BOL;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using EIRS.Models;
using Elmah;
using EIRS.Common;
using Vereyon.Web;
using System;
using System.Transactions;
using System.Reflection;
using System.Text;
using static EIRS.Web.Controllers.Filters;
using EIRS.Web.Utility;

namespace EIRS.Web.Controllers
{
    [SessionTimeout]
    public class ProfileBuildingController : BaseController
    {

        EIRSEntities _db;
        [HttpGet]
        public ActionResult List()
        {
            return View();
        }

        
        [HttpPost]
        public JsonResult LoadData()
        {
            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            var vFilter = Request.Form.GetValues("search[value]").FirstOrDefault();

            StringBuilder sbWhereCondition = new StringBuilder();

            if (!string.IsNullOrEmpty(vFilter))
            {
                sbWhereCondition.Append(" AND ( ISNULL(BuildingRIN,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(BuildingName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(StreetName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(BuildingPurposeName,'') LIKE @MainFilter )");
            }

            Building mObjBuilding = new Building()
            {
                WhereCondition = sbWhereCondition.ToString(),
                OrderBy = vSortColumn,
                OrderByDirection = vSortColumnDir,
                PageNumber = (TrynParse.parseInt(vStart) / TrynParse.parseInt(vLength)) + 1,
                PageSize = TrynParse.parseInt(vLength),
                MainFilter = !string.IsNullOrWhiteSpace(vFilter) ? "%" + vFilter.Trim() + "%" : vFilter
            };


            IDictionary<string, object> dcData = new BLBuilding().BL_SearchBuildingForSideMenu(mObjBuilding);
            IList<usp_SearchBuildingForSideMenu_Result> lstBuilding = (IList<usp_SearchBuildingForSideMenu_Result>)dcData["BuildingList"];

            return Json(new
            {
                draw = vDraw,
                recordsFiltered = dcData["FilteredRecords"],
                recordsTotal = dcData["TotalRecords"],
                data = lstBuilding
            }, JsonRequestBehavior.AllowGet);
        }

        
        [HttpGet]
        public ActionResult ListWithExport()
        {
            return View();
        }

        
        [HttpPost]
        public JsonResult LoadExportData()
        {
            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            var vFilter = Request.Form.GetValues("search[value]").FirstOrDefault();

            StringBuilder sbWhereCondition = new StringBuilder();

            if (!string.IsNullOrEmpty(vFilter))
            {
                sbWhereCondition.Append(" AND ( ISNULL(BuildingRIN,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(BuildingName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(StreetName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(BuildingPurposeName,'') LIKE @MainFilter )");
            }

            Building mObjBuilding = new Building()
            {
                WhereCondition = sbWhereCondition.ToString(),
                OrderBy = vSortColumn,
                OrderByDirection = vSortColumnDir,
                PageNumber = (TrynParse.parseInt(vStart) / TrynParse.parseInt(vLength)) + 1,
                PageSize = TrynParse.parseInt(vLength),
                MainFilter = !string.IsNullOrWhiteSpace(vFilter) ? "%" + vFilter.Trim() + "%" : vFilter
            };


            IDictionary<string, object> dcData = new BLBuilding().BL_SearchBuildingForSideMenu(mObjBuilding);
            IList<usp_SearchBuildingForSideMenu_Result> lstBuilding = (IList<usp_SearchBuildingForSideMenu_Result>)dcData["BuildingList"];

            return Json(new
            {
                draw = vDraw,
                recordsFiltered = dcData["FilteredRecords"],
                recordsTotal = dcData["TotalRecords"],
                data = lstBuilding
            }, JsonRequestBehavior.AllowGet);
        }

        
        [HttpGet]
        public ActionResult ExportData()
        {
            IList<usp_GetBuildingList_Result> lstBuildingData = new BLBuilding().BL_GetBuildingList(new Building() { intStatus = 2 });
            string[] strColumns = new string[] { "BuildingRIN",
                                                    "BuildingTagNumber",
                                                    "BuildingName",
                                                    "BuildingNumber",
                                                    "StreetName",
                                                    "OffStreetName",
                                                    "TownName",
                                                    "LGAName",
                                                    "WardName",
                                                    "BuildingTypeName",
                                                    "BuildingCompletionName",
                                                    "BuildingPurposeName",
                                                    "BuildingOwnershipName",
                                                    "NoOfUnits",
                                                    "Latitude",
                                                    "Longitude",
                                                    "BuildingSize_Length",
                                                    "BuildingSize_Width",
                                                    "ActiveText"
                                                     };
            return ExportToExcel(lstBuildingData, this.RouteData, strColumns, "Building");
        }

        
        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        
        public ActionResult Search(FormCollection pObjCollection)
        {
            string mStrBuildingName = pObjCollection.Get("txtBuildingName");
            string mStrStreetName = pObjCollection.Get("txtStreetName");
            string mStrRIN = pObjCollection.Get("txtRIN");

            Building mObjBuilding = new Building()
            {
                BuildingRIN = mStrRIN,
                BuildingName = mStrBuildingName,
                StreetName = mStrStreetName,
                intStatus = 1
            };

            IList<usp_GetBuildingList_Result> lstBuilding = new BLBuilding().BL_GetBuildingList(mObjBuilding);
            return PartialView("_BindTable", lstBuilding.Take(5).ToList());
        }


        public void UI_FillDropDown(BuildingViewModel pObjBuildingViewModel = null)
        {
            if (pObjBuildingViewModel != null)
                pObjBuildingViewModel.AssetTypeID = (int)EnumList.AssetTypes.Building;
            else if (pObjBuildingViewModel == null)
                pObjBuildingViewModel = new BuildingViewModel();

            UI_FillTaxPayerRoleDropDown(new TaxPayer_Roles() { TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies, AssetTypeID = (int)EnumList.AssetTypes.Building });
            UI_FillTownDropDown(new Town() { intStatus = 1, IncludeTownIds = pObjBuildingViewModel.TownID.ToString() });
            UI_FillLGADropDown(new LGA() { intStatus = 1, IncludeLGAIds = pObjBuildingViewModel.LGAID.ToString() });
            UI_FillWardDropDown(new Ward() { intStatus = 1, LGAID = pObjBuildingViewModel.LGAID, IncludeWardIds = pObjBuildingViewModel.WardID.ToString() });
            UI_FillAssetTypeDropDown(new Asset_Types() { intStatus = 1, IncludeAssetTypeIds = pObjBuildingViewModel.AssetTypeID.ToString() }, (int)EnumList.AssetTypes.Building);
            UI_FillBuildingTypeDropDown(new Building_Types() { intStatus = 1, IncludeBuildingTypeIds = pObjBuildingViewModel.BuildingTypeID.ToString() });
            UI_FillBuildingCompletionDropDown(new Building_Completion() { intStatus = 1, IncludeBuildingCompletionIds = pObjBuildingViewModel.BuildingCompletionID.ToString() });
            UI_FillBuildingPurposeDropDown(new Building_Purpose() { intStatus = 1, IncludeBuildingPurposeIds = pObjBuildingViewModel.BuildingPurposeID.ToString() });
            UI_FillBuildingOwnershipDropDown(new Building_Ownership() { intStatus = 1, IncludeBuildingOwnershipIds = pObjBuildingViewModel.BuildingOwnershipID.ToString() });

            UI_FillUnitPurposeDropDown(new Unit_Purpose() { intStatus = 1, });
            UI_FillUnitFunctionDropDown(new Unit_Function() { intStatus = 1 });
            UI_FillUnitOccupancyDropDown(new Unit_Occupancy() { intStatus = 1 });
            UI_FillSizeDropDown(new Size() { intStatus = 1 });

            ViewBag.BuildingUnitList = SessionManager.LstBuildingUnit;
        }

        
        public ActionResult Add()
        {
            SessionManager.LstBuildingUnit = new List<Building_BuildingUnit>();
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
                IList<Building_BuildingUnit> lstBuildingUnit = SessionManager.LstBuildingUnit;
                if (lstBuildingUnit.Count == 0)
                {
                    UI_FillDropDown(pObjBuildingModel);
                    ViewBag.Message = "Add atleast one building unit";
                    return View(pObjBuildingModel);
                }
                else
                {
                    using (TransactionScope mObjScope = new TransactionScope())
                    {
                        try
                        {
                            BLBuilding mObjBLBuilding = new BLBuilding();
                            Building mObjBuilding = new Building()
                            {
                                BuildingID = 0,
                                BuildingName = pObjBuildingModel.BuildingName != null ? pObjBuildingModel.BuildingName.Trim() : pObjBuildingModel.BuildingName,
                                BuildingNumber = pObjBuildingModel.BuildingNumber,
                                StreetName = pObjBuildingModel.StreetName,
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
                                CreatedBy = SessionManager.UserID,
                                CreatedDate = CommUtil.GetCurrentDateTime()
                            };

                            FuncResponse<Building> mObjResponse = mObjBLBuilding.BL_InsertUpdateBuilding(mObjBuilding);

                            if (mObjResponse.Success)
                            {
                                BLBuildingUnit mObjBLBuildingUnit = new BLBuildingUnit();

                                //Adding Building Unit
                                foreach (var item in lstBuildingUnit)
                                {
                                    Building_Unit mObjBuildingUnit = new Building_Unit()
                                    {
                                        BuildingUnitID = 0,
                                        UnitNumber = item.UnitNumber,
                                        UnitPurposeID = item.UnitPurposeID,
                                        UnitFunctionID = item.UnitFunctionID,
                                        UnitOccupancyID = item.UnitOccupancyID,
                                        SizeID = item.UnitSizeID,
                                        Active = true,
                                        CreatedBy = SessionManager.UserID,
                                        CreatedDate = CommUtil.GetCurrentDateTime()
                                    };

                                    FuncResponse<Building_Unit> mObjBUResponse = mObjBLBuildingUnit.BL_InsertUpdateBuildingUnit(mObjBuildingUnit);

                                    if (mObjBUResponse.Success)
                                    {
                                        item.BuildingUnitID = mObjBUResponse.AdditionalData.BuildingUnitID;

                                        //Creating Mapping With Building
                                        MAP_Building_BuildingUnit mObjUnit = new MAP_Building_BuildingUnit()
                                        {
                                            BuildingUnitID = mObjBUResponse.AdditionalData.BuildingUnitID,
                                            BuildingID = mObjResponse.AdditionalData.BuildingID,
                                            CreatedBy = SessionManager.UserID,
                                            CreatedDate = CommUtil.GetCurrentDateTime()
                                        };

                                        FuncResponse mObjBBUResponse = mObjBLBuilding.BL_InsertBuildingUnitNumber(mObjUnit);

                                        if (!mObjBBUResponse.Success)
                                        {
                                            throw new Exception(mObjBBUResponse.Message);
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception(mObjBUResponse.Message);
                                    }
                                }

                                mObjScope.Complete();
                                FlashMessage.Info(mObjResponse.Message);
                                return RedirectToAction("Details", "ProfileBuilding", new { id = mObjResponse.AdditionalData.BuildingID, name = mObjResponse.AdditionalData.BuildingName.ToSeoUrl() });

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
                            Logger.SendErrorToText(ex);
                            ErrorSignal.FromCurrentContext().Raise(ex);
                            UI_FillDropDown(pObjBuildingModel);
                            Transaction.Current.Rollback();
                            ViewBag.Message = "Error occurred while saving building";
                            return View(pObjBuildingModel);
                        }
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
                        BuildingNumber = mObjBuildingData.BuildingNumber,
                        StreetName = mObjBuildingData.StreetName,
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
                    return RedirectToAction("Search", "ProfileBuilding");
                }
            }
            else
            {
                return RedirectToAction("Search", "ProfileBuilding");
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
                Building mObjBuilding = new Building()
                {
                    BuildingID = pObjBuildingModel.BuildingID,
                    BuildingName = pObjBuildingModel.BuildingName != null ? pObjBuildingModel.BuildingName.Trim() : pObjBuildingModel.BuildingName,
                    BuildingNumber = pObjBuildingModel.BuildingNumber,
                    StreetName = pObjBuildingModel.StreetName,
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
                    ModifiedBy = SessionManager.UserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };


                try
                {

                    FuncResponse<Building> mObjResponse = new BLBuilding().BL_InsertUpdateBuilding(mObjBuilding);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("Details", "ProfileBuilding", new { id = mObjResponse.AdditionalData.BuildingID, name = mObjResponse.AdditionalData.BuildingRIN.ToSeoUrl() });
                    }
                    else
                    {
                        UI_FillDropDown(pObjBuildingModel);
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjBuildingModel);
                    }
                }
                catch (Exception ex)
                {
                    Logger.SendErrorToText(ex);
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillDropDown(pObjBuildingModel);
                    ViewBag.Message = "Error occurred while saving building";
                    return View(pObjBuildingModel);
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
                        BuildingNumber = mObjBuildingData.BuildingNumber,
                        StreetName = mObjBuildingData.StreetName,
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


                    MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                    {
                        AssetID = id.GetValueOrDefault(),
                        AssetTypeID = (int)EnumList.AssetTypes.Building
                    };

                    IList<usp_GetTaxPayerAssetList_Result> lstTaxPayerAsset = new BLTaxPayerAsset().BL_GetTaxPayerAssetList(mObjTaxPayerAsset);
                    ViewBag.AssetList = lstTaxPayerAsset;

                    IList<usp_GetBuildingUnitNumberList_Result> lstUnitInformation = new BLBuilding().BL_GetBuildingUnitNumberList(new MAP_Building_BuildingUnit() { BuildingID = id.GetValueOrDefault() });
                    ViewBag.UnitList = lstUnitInformation;

                    return View(mObjBuildingModelView);
                }
                else
                {
                    return RedirectToAction("Search", "ProfileBuilding");
                }
            }
            else
            {
                return RedirectToAction("Search", "ProfileBuilding");
            }
        }

        
        public ActionResult SearchIndividual(int? id, string name)
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
                        BuildingNumber = mObjBuildingData.BuildingNumber,
                        StreetName = mObjBuildingData.StreetName,
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
                    return RedirectToAction("Search", "ProfileBuilding");
                }
            }
            else
            {
                return RedirectToAction("Search", "ProfileBuilding");
            }
        }

        [HttpPost]
        
        public ActionResult SearchIndividual(FormCollection pObjCollection)
        {
            string mStrName = pObjCollection.Get("txtName");
            string mStrMobileNumber = pObjCollection.Get("txtMobileNumber");
            string mStrRIN = pObjCollection.Get("txtRIN");

            Individual mObjIndividual = new Individual()
            {
                IndividualName = mStrName,
                MobileNumber1 = mStrMobileNumber,
                IndividualRIN = mStrRIN,
                intStatus = 1
            };

            IList<usp_GetIndividualList_Result> lstIndividual = new BLIndividual().BL_GetIndividualList(mObjIndividual);
            return PartialView("_BindIndividualTable_SingleSelect", lstIndividual.Take(5).ToList());
        }

        
        public ActionResult SearchCorporate(int? id, string name)
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
                        BuildingNumber = mObjBuildingData.BuildingNumber,
                        StreetName = mObjBuildingData.StreetName,
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
                    return RedirectToAction("Search", "ProfileBuilding");
                }
            }
            else
            {
                return RedirectToAction("Search", "ProfileBuilding");
            }
        }

        [HttpPost]
        
        public ActionResult SearchCorporate(FormCollection pObjCollection)
        {
            string mStrCompanyName = pObjCollection.Get("txtCompanyName");
            string mStrMobileNumber = pObjCollection.Get("txtMobileNumber");
            string mStrRIN = pObjCollection.Get("txtRIN");

            Company mObjCompany = new Company()
            {
                CompanyName = mStrCompanyName,
                MobileNumber1 = mStrMobileNumber,
                CompanyRIN = mStrRIN,
                intStatus = 1
            };

            IList<usp_GetCompanyList_Result> lstCompany = new BLCompany().BL_GetCompanyList(mObjCompany);
            return PartialView("_BindCompanyTable_SingleSelect", lstCompany.Take(5).ToList());
        }

        
        public ActionResult SearchGovernment(int? id, string name)
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
                        BuildingNumber = mObjBuildingData.BuildingNumber,
                        StreetName = mObjBuildingData.StreetName,
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
                    return RedirectToAction("Search", "ProfileBuilding");
                }
            }
            else
            {
                return RedirectToAction("Search", "ProfileBuilding");
            }
        }

        [HttpPost]
        
        public ActionResult SearchGovernment(FormCollection pObjCollection)
        {
            string mStrGovernmentName = pObjCollection.Get("txtGovernmentName");
            string mStrMobileNumber = pObjCollection.Get("txtMobileNumber");
            string mStrRIN = pObjCollection.Get("txtRIN");

            Government mObjGovernment = new Government()
            {
                GovernmentName = mStrGovernmentName,
                ContactNumber = mStrMobileNumber,
                GovernmentRIN = mStrRIN,
                intStatus = 1
            };

            IList<usp_GetGovernmentList_Result> lstGovernment = new BLGovernment().BL_GetGovernmentList(mObjGovernment);
            return PartialView("_BindGovernmentTable_SingleSelect", lstGovernment.Take(5).ToList());
        }

        
        public ActionResult SearchSpecial(int? id, string name)
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
                        BuildingNumber = mObjBuildingData.BuildingNumber,
                        StreetName = mObjBuildingData.StreetName,
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
                    return RedirectToAction("Search", "ProfileBuilding");
                }
            }
            else
            {
                return RedirectToAction("Search", "ProfileBuilding");
            }
        }

        [HttpPost]
        
        public ActionResult SearchSpecial(FormCollection pObjCollection)
        {
            string mStrSpecialName = pObjCollection.Get("txtSpecialName");
            string mStrMobileNumber = pObjCollection.Get("txtMobileNumber");
            string mStrRIN = pObjCollection.Get("txtRIN");

            Special mObjSpecial = new Special()
            {
                SpecialTaxPayerName = mStrSpecialName,
                ContactNumber = mStrMobileNumber,
                SpecialRIN = mStrRIN,
                intStatus = 1
            };

            IList<usp_GetSpecialList_Result> lstSpecial = new BLSpecial().BL_GetSpecialList(mObjSpecial);
            return PartialView("_BindSpecialTable_SingleSelect", lstSpecial.Take(5).ToList());
        }

        public void UI_FillIndividualDropDown(TPIndividualViewModel pObjIndividualViewModel = null)
        {
            if (pObjIndividualViewModel != null)
                pObjIndividualViewModel.TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual;
            else if (pObjIndividualViewModel == null)
                pObjIndividualViewModel = new TPIndividualViewModel();

            UI_FillTaxPayerRoleDropDown(new TaxPayer_Roles() { TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual, AssetTypeID = (int)EnumList.AssetTypes.Building });
            UI_FillGender();
            UI_FillTitleDropDown(new Title() { intStatus = 1, IncludeTitleIds = pObjIndividualViewModel.TitleID.ToString(), GenderID = pObjIndividualViewModel.GenderID });
            UI_FillTaxOfficeDropDown(new Tax_Offices() { intStatus = 1, IncludeTaxOfficeIds = pObjIndividualViewModel.TaxOfficeID.ToString() });
            UI_FillMaritalStatus();
            UI_FillNationality();
            UI_FillTaxPayerTypeDropDown(new TaxPayer_Types() { intStatus = 1, IncludeTaxPayerTypeIds = pObjIndividualViewModel.TaxPayerTypeID.ToString() }, (int)EnumList.TaxPayerType.Individual);
            UI_FillEconomicActivitiesDropDown(new Economic_Activities() { intStatus = 1, IncludeEconomicActivitiesIds = pObjIndividualViewModel.EconomicActivitiesID.ToString(), TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual });
            UI_FillNotificationMethodDropDown(new Notification_Method() { intStatus = 1, IncludeNotificationMethodIds = pObjIndividualViewModel.NotificationMethodID.ToString() });

            MAP_Building_BuildingUnit mObjBuildingUnit = new MAP_Building_BuildingUnit()
            {
                BuildingID = pObjIndividualViewModel.AssetID
            };

            IList<usp_GetBuildingUnitNumberList_Result> lstBuildingUnitNumberList = new BLBuilding().BL_GetBuildingUnitNumberList(mObjBuildingUnit);
            ViewBag.BuildingUnitList = new SelectList(lstBuildingUnitNumberList, "BuildingUnitID", "UnitNumber");

        }

        
        public ActionResult AddIndividual(int? id, string name)
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
                    TPIndividualViewModel mObjIndividualModel = new TPIndividualViewModel()
                    {
                        AssetID = mObjBuildingData.BuildingID.GetValueOrDefault(),
                        AssetName = mObjBuildingData.BuildingName,
                        AssetRIN = mObjBuildingData.BuildingRIN,
                        AssetLGAName = mObjBuildingData.LGAName,
                        AssetTypeID = (int)EnumList.AssetTypes.Building,
                        AssetTypeName = mObjBuildingData.AssetTypeName,
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                    };

                    UI_FillIndividualDropDown(mObjIndividualModel);
                    return View(mObjIndividualModel);
                }
                else
                {
                    return RedirectToAction("Search", "ProfileBuilding");
                }
            }
            else
            {
                return RedirectToAction("Search", "ProfileBuilding");
            }
        }

        [HttpPost]
        
        [ValidateAntiForgeryToken()]
        public ActionResult AddIndividual(TPIndividualViewModel pObjIndividualModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillIndividualDropDown(pObjIndividualModel);
                return View(pObjIndividualModel);
            }
            else
            {
                using (TransactionScope mObjScope = new TransactionScope())
                {
                    try
                    {
                        Individual mObjIndividual = new Individual()
                        {
                            IndividualID = 0,
                            GenderID = pObjIndividualModel.GenderID,
                            TitleID = pObjIndividualModel.TitleID,
                            FirstName = pObjIndividualModel.FirstName,
                            LastName = pObjIndividualModel.LastName,
                            MiddleName = pObjIndividualModel.MiddleName,
                            DOB = TrynParse.parseNullableDate(pObjIndividualModel.DOB),
                            TIN = pObjIndividualModel.TIN,
                            MobileNumber1 = pObjIndividualModel.MobileNumber1,
                            MobileNumber2 = pObjIndividualModel.MobileNumber2,
                            EmailAddress1 = pObjIndividualModel.EmailAddress1,
                            EmailAddress2 = pObjIndividualModel.EmailAddress2,
                            BiometricDetails = pObjIndividualModel.BiometricDetails,
                            TaxOfficeID = pObjIndividualModel.TaxOfficeID,
                            MaritalStatusID = pObjIndividualModel.MaritalStatusID,
                            NationalityID = pObjIndividualModel.NationalityID,
                            TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                            EconomicActivitiesID = pObjIndividualModel.EconomicActivitiesID,
                            NotificationMethodID = pObjIndividualModel.NotificationMethodID,
                            ContactAddress = pObjIndividualModel.ContactAddress,
                            Active = true,
                            CreatedBy = SessionManager.UserID,
                            CreatedDate = CommUtil.GetCurrentDateTime()
                        };
                        FuncResponse<Individual> mObjResponse = new BLIndividual().BL_InsertUpdateIndividual(mObjIndividual);

                        if (mObjResponse.Success)
                        {
                            if (GlobalDefaultValues.SendNotification)
                            {
                                //Send Notification
                                EmailDetails mObjEmailDetails = new EmailDetails()
                                {
                                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                                    TaxPayerTypeName = "Individual",
                                    TaxPayerID = mObjIndividual.IndividualID,
                                    TaxPayerName = mObjIndividual.FirstName + " " + mObjIndividual.LastName,
                                    TaxPayerRIN = mObjIndividual.IndividualRIN,
                                    TaxPayerMobileNumber = mObjIndividual.MobileNumber1,
                                    TaxPayerEmail = mObjIndividual.EmailAddress1,
                                    ContactAddress = mObjIndividual.ContactAddress,
                                    TaxPayerTIN = mObjIndividual.TIN,
                                    LoggedInUserID = SessionManager.UserID,
                                };

                                if (!string.IsNullOrWhiteSpace(mObjIndividual.EmailAddress1))
                                {
                                    BLEmailHandler.BL_TaxPayerCreated(mObjEmailDetails);
                                }

                                if (!string.IsNullOrWhiteSpace(mObjIndividual.MobileNumber1))
                                {
                                    UtilityController.BL_TaxPayerCreated(mObjEmailDetails);
                                }
                            }

                            //Creating mapping between individual and Building
                            MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                            {
                                AssetTypeID = pObjIndividualModel.AssetTypeID,
                                AssetID = pObjIndividualModel.AssetID,
                                TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                                TaxPayerRoleID = pObjIndividualModel.TaxPayerRoleID,
                                TaxPayerID = mObjResponse.AdditionalData.IndividualID,
                                BuildingUnitID = pObjIndividualModel.BuildingUnitID,
                                Active = true,
                                CreatedBy = SessionManager.UserID,
                                CreatedDate = CommUtil.GetCurrentDateTime()
                            };

                            FuncResponse mObjTPResponse = new BLTaxPayerAsset().BL_InsertTaxPayerAsset(mObjTaxPayerAsset);

                            if (mObjTPResponse.Success)
                            {
                                var vExists = (from tpa in _db.MAP_TaxPayer_Asset
                                               join aa in _db.Asset_Types on tpa.AssetTypeID equals aa.AssetTypeID
                                               join tp in _db.TaxPayer_Roles on tpa.TaxPayerRoleID equals tp.TaxPayerRoleID
                                               join tpx in _db.TaxPayer_Types on tpa.TaxPayerTypeID equals tpx.TaxPayerTypeID
                                               join idd in _db.Individuals on tpa.TaxPayerID equals idd.IndividualID
                                               where tpa.AssetTypeID == mObjTaxPayerAsset.AssetTypeID && tpa.AssetID == mObjTaxPayerAsset.AssetID
                                                  && tpa.TaxPayerTypeID == mObjTaxPayerAsset.TaxPayerTypeID && tpa.TaxPayerID == mObjTaxPayerAsset.TaxPayerID
                                                  && tpa.TaxPayerRoleID == mObjTaxPayerAsset.TaxPayerRoleID && tpa.Active == true

                                               select new { Post = tpa, Meta = tp, All = tpx, Idd = idd, Aa = aa }).FirstOrDefault();

                                if (GlobalDefaultValues.SendNotification)
                                {
                                    //Send Notification
                                    EmailDetails mObjEmailDetails = new EmailDetails()
                                    {
                                        TaxPayerTypeID = vExists.Post.TaxPayerTypeID.GetValueOrDefault(),
                                        TaxPayerTypeName = vExists.All.TaxPayerTypeName,
                                        TaxPayerID = vExists.Idd.IndividualID,
                                        TaxPayerName = vExists.Idd.FirstName + " " + vExists.Idd.LastName,
                                        TaxPayerRIN = vExists.Idd.IndividualRIN,
                                        TaxPayerRoleName = vExists.Meta.TaxPayerRoleName,
                                        AssetName = vExists.Aa.AssetTypeName,
                                        LoggedInUserID = SessionManager.UserID,
                                    };

                                    if (!string.IsNullOrWhiteSpace(vExists.Idd.EmailAddress1))
                                    {
                                        BLEmailHandler.BL_AssetProfileLinked(mObjEmailDetails);
                                    }

                                    if (!string.IsNullOrWhiteSpace(vExists.Idd.MobileNumber1))
                                    {
                                        UtilityController.BL_AssetProfileLinked(mObjEmailDetails);
                                    }
                                }
                                mObjScope.Complete();
                                FlashMessage.Info("Individual Created Successfully and Linked to Asset");
                                return RedirectToAction("Details", "ProfileBuilding", new { id = pObjIndividualModel.AssetID, name = pObjIndividualModel.AssetRIN });
                            }
                            else
                            {
                                throw new Exception(mObjResponse.Message);
                            }
                        }
                        else
                        {
                            UI_FillIndividualDropDown(pObjIndividualModel);
                            ViewBag.Message = mObjResponse.Message;
                            return View(pObjIndividualModel);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.SendErrorToText(ex);
                        ErrorSignal.FromCurrentContext().Raise(ex);
                        UI_FillIndividualDropDown(pObjIndividualModel);
                        Transaction.Current.Rollback();
                        ViewBag.Message = "Error occurred while saving individual";
                        return View(pObjIndividualModel);
                    }
                }
            }

        }

        public void UI_FillCompanyDropDown(TPCompanyViewModel pObjCompanyViewModel = null)
        {
            if (pObjCompanyViewModel != null)
                pObjCompanyViewModel.TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies;
            else if (pObjCompanyViewModel == null)
                pObjCompanyViewModel = new TPCompanyViewModel();

            UI_FillTaxPayerRoleDropDown(new TaxPayer_Roles() { TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies, AssetTypeID = (int)EnumList.AssetTypes.Building });
            UI_FillTaxOfficeDropDown(new Tax_Offices() { intStatus = 1, IncludeTaxOfficeIds = pObjCompanyViewModel.TaxOfficeID.ToString() });
            UI_FillTaxPayerTypeDropDown(new TaxPayer_Types() { intStatus = 1, IncludeTaxPayerTypeIds = pObjCompanyViewModel.TaxPayerTypeID.ToString() }, (int)EnumList.TaxPayerType.Companies);
            UI_FillEconomicActivitiesDropDown(new Economic_Activities() { intStatus = 1, IncludeEconomicActivitiesIds = pObjCompanyViewModel.EconomicActivitiesID.ToString(), TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies });
            UI_FillNotificationMethodDropDown(new Notification_Method() { intStatus = 1, IncludeNotificationMethodIds = pObjCompanyViewModel.NotificationMethodID.ToString() });

            MAP_Building_BuildingUnit mObjBuildingUnit = new MAP_Building_BuildingUnit()
            {
                BuildingID = pObjCompanyViewModel.AssetID
            };

            IList<usp_GetBuildingUnitNumberList_Result> lstBuildingUnitNumberList = new BLBuilding().BL_GetBuildingUnitNumberList(mObjBuildingUnit);
            ViewBag.BuildingUnitList = new SelectList(lstBuildingUnitNumberList, "BuildingUnitID", "UnitNumber");
        }

        
        public ActionResult AddCorporate(int? id, string name)
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
                    TPCompanyViewModel mObjCompanyModel = new TPCompanyViewModel()
                    {
                        AssetID = mObjBuildingData.BuildingID.GetValueOrDefault(),
                        AssetName = mObjBuildingData.BuildingName,
                        AssetRIN = mObjBuildingData.BuildingRIN,
                        AssetLGAName = mObjBuildingData.LGAName,
                        AssetTypeID = (int)EnumList.AssetTypes.Building,
                        AssetTypeName = mObjBuildingData.AssetTypeName,
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies,
                    };

                    UI_FillCompanyDropDown(mObjCompanyModel);
                    return View(mObjCompanyModel);
                }
                else
                {
                    return RedirectToAction("Search", "ProfileBuilding");
                }
            }
            else
            {
                return RedirectToAction("Search", "ProfileBuilding");
            }
        }

        [HttpPost]
        
        [ValidateAntiForgeryToken()]
        public ActionResult AddCorporate(TPCompanyViewModel pObjCompanyModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillCompanyDropDown(pObjCompanyModel);
                return View(pObjCompanyModel);
            }
            else
            {
                using (TransactionScope mObjScope = new TransactionScope())
                {
                    try
                    {
                        Company mObjCompany = new Company()
                        {
                            CompanyID = 0,
                            CompanyName = pObjCompanyModel.CompanyName,
                            TIN = pObjCompanyModel.TIN,
                            MobileNumber1 = pObjCompanyModel.MobileNumber1,
                            MobileNumber2 = pObjCompanyModel.MobileNumber2,
                            EmailAddress1 = pObjCompanyModel.EmailAddress1,
                            EmailAddress2 = pObjCompanyModel.EmailAddress2,
                            TaxOfficeID = pObjCompanyModel.TaxOfficeID,
                            TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies,
                            EconomicActivitiesID = pObjCompanyModel.EconomicActivitiesID,
                            NotificationMethodID = pObjCompanyModel.NotificationMethodID,
                            ContactAddress = pObjCompanyModel.ContactAddress,
                            CACRegistrationNumber = pObjCompanyModel.CACRegistrationNumber,
                            Active = true,
                            CreatedBy = SessionManager.UserID,
                            CreatedDate = CommUtil.GetCurrentDateTime()
                        };

                        FuncResponse<Company> mObjResponse = new BLCompany().BL_InsertUpdateCompany(mObjCompany);

                        if (mObjResponse.Success)
                        {
                            if (GlobalDefaultValues.SendNotification)
                            {
                                //Send Notification
                                EmailDetails mObjEmailDetails = new EmailDetails()
                                {
                                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies,
                                    TaxPayerTypeName = "Company",
                                    TaxPayerID = pObjCompanyModel.CompanyID,
                                    TaxPayerName = pObjCompanyModel.CompanyName,
                                    TaxPayerRIN = pObjCompanyModel.CompanyRIN,
                                    TaxPayerMobileNumber = pObjCompanyModel.MobileNumber1,
                                    TaxPayerEmail = pObjCompanyModel.EmailAddress1,
                                    ContactAddress = pObjCompanyModel.ContactAddress,
                                    TaxPayerTIN = pObjCompanyModel.TIN,
                                    LoggedInUserID = SessionManager.UserID,
                                };

                                if (!string.IsNullOrWhiteSpace(pObjCompanyModel.EmailAddress1))
                                {
                                    BLEmailHandler.BL_TaxPayerCreated(mObjEmailDetails);
                                }

                                if (!string.IsNullOrWhiteSpace(pObjCompanyModel.MobileNumber1))
                                {
                                    UtilityController.BL_TaxPayerCreated(mObjEmailDetails);
                                }
                            }
                            //Creating mapping between individual and Building
                            MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                            {
                                AssetTypeID = pObjCompanyModel.AssetTypeID,
                                AssetID = pObjCompanyModel.AssetID,
                                TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies,
                                TaxPayerRoleID = pObjCompanyModel.TaxPayerRoleID,
                                TaxPayerID = mObjResponse.AdditionalData.CompanyID,
                                BuildingUnitID = pObjCompanyModel.BuildingUnitID,
                                Active = true,
                                CreatedBy = SessionManager.UserID,
                                CreatedDate = CommUtil.GetCurrentDateTime()
                            };

                            FuncResponse mObjTPResponse = new BLTaxPayerAsset().BL_InsertTaxPayerAsset(mObjTaxPayerAsset);

                            if (mObjTPResponse.Success)
                            {
                                var vExists = (from tpa in _db.MAP_TaxPayer_Asset
                                               join aa in _db.Asset_Types on tpa.AssetTypeID equals aa.AssetTypeID
                                               join tp in _db.TaxPayer_Roles on tpa.TaxPayerRoleID equals tp.TaxPayerRoleID
                                               join tpx in _db.TaxPayer_Types on tpa.TaxPayerTypeID equals tpx.TaxPayerTypeID
                                               join idd in _db.Individuals on tpa.TaxPayerID equals idd.IndividualID
                                               where tpa.AssetTypeID == mObjTaxPayerAsset.AssetTypeID && tpa.AssetID == mObjTaxPayerAsset.AssetID
                                                  && tpa.TaxPayerTypeID == mObjTaxPayerAsset.TaxPayerTypeID && tpa.TaxPayerID == mObjTaxPayerAsset.TaxPayerID
                                                  && tpa.TaxPayerRoleID == mObjTaxPayerAsset.TaxPayerRoleID && tpa.Active == true

                                               select new { Post = tpa, Meta = tp, All = tpx, Idd = idd, Aa = aa }).FirstOrDefault();

                                if (GlobalDefaultValues.SendNotification)
                                {
                                    //Send Notification
                                    EmailDetails mObjEmailDetails = new EmailDetails()
                                    {
                                        TaxPayerTypeID = vExists.Post.TaxPayerTypeID.GetValueOrDefault(),
                                        TaxPayerTypeName = vExists.All.TaxPayerTypeName,
                                        TaxPayerID = vExists.Idd.IndividualID,
                                        TaxPayerName = vExists.Idd.FirstName + " " + vExists.Idd.LastName,
                                        TaxPayerRIN = vExists.Idd.IndividualRIN,
                                        TaxPayerRoleName = vExists.Meta.TaxPayerRoleName,
                                        AssetName = vExists.Aa.AssetTypeName,
                                        LoggedInUserID = SessionManager.UserID,
                                    };

                                    if (!string.IsNullOrWhiteSpace(vExists.Idd.EmailAddress1))
                                    {
                                        BLEmailHandler.BL_AssetProfileLinked(mObjEmailDetails);
                                    }

                                    if (!string.IsNullOrWhiteSpace(vExists.Idd.MobileNumber1))
                                    {
                                        UtilityController.BL_AssetProfileLinked(mObjEmailDetails);
                                    }
                                }
                                mObjScope.Complete();
                                FlashMessage.Info("Corporate Created Successfully and Linked to Asset");
                                return RedirectToAction("Details", "ProfileBuilding", new { id = pObjCompanyModel.AssetID, name = pObjCompanyModel.AssetRIN });
                            }
                            else
                            {
                                throw new Exception(mObjResponse.Message);
                            }
                        }
                        else
                        {
                            UI_FillCompanyDropDown(pObjCompanyModel);
                            ViewBag.Message = mObjResponse.Message;
                            return View(pObjCompanyModel);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.SendErrorToText(ex);
                        ErrorSignal.FromCurrentContext().Raise(ex);
                        UI_FillCompanyDropDown(pObjCompanyModel);
                        Transaction.Current.Rollback();
                        ViewBag.Message = "Error occurred while saving corporate";
                        return View(pObjCompanyModel);
                    }
                }
            }

        }

        public void UI_FillGovernmentDropDown(TPGovernmentViewModel pObjGovernmentViewModel = null)
        {
            if (pObjGovernmentViewModel != null)
                pObjGovernmentViewModel.TaxPayerTypeID = (int)EnumList.TaxPayerType.Government;
            else if (pObjGovernmentViewModel == null)
                pObjGovernmentViewModel = new TPGovernmentViewModel();

            UI_FillTaxOfficeDropDown(new Tax_Offices() { intStatus = 1, IncludeTaxOfficeIds = pObjGovernmentViewModel.TaxOfficeID.ToString() });
            UI_FillTaxPayerTypeDropDown(new TaxPayer_Types() { intStatus = 1, IncludeTaxPayerTypeIds = pObjGovernmentViewModel.TaxPayerTypeID.ToString() }, (int)EnumList.TaxPayerType.Government);
            UI_FillGovernmentTypeDropDown(new Government_Types() { intStatus = 1, IncludeGovernmentTypeIds = pObjGovernmentViewModel.GovernmentTypeID.ToString() });
            UI_FillNotificationMethodDropDown(new Notification_Method() { intStatus = 1, IncludeNotificationMethodIds = pObjGovernmentViewModel.NotificationMethodID.ToString() });
            UI_FillTaxPayerRoleDropDown(new TaxPayer_Roles() { TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual, AssetTypeID = (int)EnumList.AssetTypes.Building });

            MAP_Building_BuildingUnit mObjBuildingUnit = new MAP_Building_BuildingUnit()
            {
                BuildingID = pObjGovernmentViewModel.AssetID
            };

            IList<usp_GetBuildingUnitNumberList_Result> lstBuildingUnitNumberList = new BLBuilding().BL_GetBuildingUnitNumberList(mObjBuildingUnit);
            ViewBag.BuildingUnitList = new SelectList(lstBuildingUnitNumberList, "BuildingUnitID", "UnitNumber");
        }

        
        public ActionResult AddGovernment(int? id, string name)
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
                    TPGovernmentViewModel mObjGovernmentModel = new TPGovernmentViewModel()
                    {
                        AssetID = mObjBuildingData.BuildingID.GetValueOrDefault(),
                        AssetName = mObjBuildingData.BuildingName,
                        AssetRIN = mObjBuildingData.BuildingRIN,
                        AssetLGAName = mObjBuildingData.LGAName,
                        AssetTypeID = (int)EnumList.AssetTypes.Building,
                        AssetTypeName = mObjBuildingData.AssetTypeName,
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Government,
                    };

                    UI_FillGovernmentDropDown(mObjGovernmentModel);
                    return View(mObjGovernmentModel);
                }
                else
                {
                    return RedirectToAction("Search", "ProfileBuilding");
                }
            }
            else
            {
                return RedirectToAction("Search", "ProfileBuilding");
            }
        }

        [HttpPost]
        
        [ValidateAntiForgeryToken()]
        public ActionResult AddGovernment(TPGovernmentViewModel pObjGovernmentModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillGovernmentDropDown(pObjGovernmentModel);
                return View(pObjGovernmentModel);
            }
            else
            {
                using (TransactionScope mObjScope = new TransactionScope())
                {
                    try
                    {
                        Government mObjGovernment = new Government()
                        {
                            GovernmentID = 0,
                            GovernmentName = pObjGovernmentModel.GovernmentName,
                            GovernmentTypeID = pObjGovernmentModel.GovernmentTypeID,
                            TIN = pObjGovernmentModel.TIN,
                            TaxOfficeID = pObjGovernmentModel.TaxOfficeID,
                            TaxPayerTypeID = (int)EnumList.TaxPayerType.Government,
                            ContactNumber = pObjGovernmentModel.ContactNumber,
                            ContactEmail = pObjGovernmentModel.ContactEmail,
                            ContactName = pObjGovernmentModel.ContactName,
                            NotificationMethodID = pObjGovernmentModel.NotificationMethodID,
                            ContactAddress = pObjGovernmentModel.ContactAddress,
                            Active = true,
                            CreatedBy = SessionManager.UserID,
                            CreatedDate = CommUtil.GetCurrentDateTime()
                        };

                        FuncResponse<Government> mObjResponse = new BLGovernment().BL_InsertUpdateGovernment(mObjGovernment);

                        if (mObjResponse.Success)
                        {
                            if (GlobalDefaultValues.SendNotification)
                            {
                                //Send Notification
                                EmailDetails mObjEmailDetails = new EmailDetails()
                                {
                                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Government,
                                    TaxPayerTypeName = "Government",
                                    TaxPayerID = pObjGovernmentModel.GovernmentID,
                                    TaxPayerName = pObjGovernmentModel.GovernmentName,
                                    TaxPayerRIN = pObjGovernmentModel.GovernmentRIN,
                                    TaxPayerMobileNumber = pObjGovernmentModel.ContactNumber,
                                    TaxPayerEmail = pObjGovernmentModel.ContactEmail,
                                    ContactAddress = pObjGovernmentModel.ContactAddress,
                                    TaxPayerTIN = pObjGovernmentModel.TIN,
                                    LoggedInUserID = SessionManager.UserID,
                                };

                                if (!string.IsNullOrWhiteSpace(pObjGovernmentModel.ContactEmail))
                                {
                                    BLEmailHandler.BL_TaxPayerCreated(mObjEmailDetails);
                                }

                                if (!string.IsNullOrWhiteSpace(pObjGovernmentModel.ContactNumber))
                                {
                                    UtilityController.BL_TaxPayerCreated(mObjEmailDetails);
                                }
                            }
                            //Creating mapping between individual and Building
                            MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                            {
                                AssetTypeID = pObjGovernmentModel.AssetTypeID,
                                AssetID = pObjGovernmentModel.AssetID,
                                TaxPayerTypeID = (int)EnumList.TaxPayerType.Government,
                                TaxPayerRoleID = pObjGovernmentModel.TaxPayerRoleID,
                                TaxPayerID = mObjResponse.AdditionalData.GovernmentID,
                                BuildingUnitID = pObjGovernmentModel.BuildingUnitID,
                                Active = true,
                                CreatedBy = SessionManager.UserID,
                                CreatedDate = CommUtil.GetCurrentDateTime()
                            };

                            FuncResponse mObjTPResponse = new BLTaxPayerAsset().BL_InsertTaxPayerAsset(mObjTaxPayerAsset);

                            if (mObjTPResponse.Success)
                            {
                                var vExists = (from tpa in _db.MAP_TaxPayer_Asset
                                               join aa in _db.Asset_Types on tpa.AssetTypeID equals aa.AssetTypeID
                                               join tp in _db.TaxPayer_Roles on tpa.TaxPayerRoleID equals tp.TaxPayerRoleID
                                               join tpx in _db.TaxPayer_Types on tpa.TaxPayerTypeID equals tpx.TaxPayerTypeID
                                               join idd in _db.Individuals on tpa.TaxPayerID equals idd.IndividualID
                                               where tpa.AssetTypeID == mObjTaxPayerAsset.AssetTypeID && tpa.AssetID == mObjTaxPayerAsset.AssetID
                                                  && tpa.TaxPayerTypeID == mObjTaxPayerAsset.TaxPayerTypeID && tpa.TaxPayerID == mObjTaxPayerAsset.TaxPayerID
                                                  && tpa.TaxPayerRoleID == mObjTaxPayerAsset.TaxPayerRoleID && tpa.Active == true

                                               select new { Post = tpa, Meta = tp, All = tpx, Idd = idd, Aa = aa }).FirstOrDefault();

                                if (GlobalDefaultValues.SendNotification)
                                {
                                    //Send Notification
                                    EmailDetails mObjEmailDetails = new EmailDetails()
                                    {
                                        TaxPayerTypeID = vExists.Post.TaxPayerTypeID.GetValueOrDefault(),
                                        TaxPayerTypeName = vExists.All.TaxPayerTypeName,
                                        TaxPayerID = vExists.Idd.IndividualID,
                                        TaxPayerName = vExists.Idd.FirstName + " " + vExists.Idd.LastName,
                                        TaxPayerRIN = vExists.Idd.IndividualRIN,
                                        TaxPayerRoleName = vExists.Meta.TaxPayerRoleName,
                                        AssetName = vExists.Aa.AssetTypeName,
                                        LoggedInUserID = SessionManager.UserID,
                                    };

                                    if (!string.IsNullOrWhiteSpace(vExists.Idd.EmailAddress1))
                                    {
                                        BLEmailHandler.BL_AssetProfileLinked(mObjEmailDetails);
                                    }

                                    if (!string.IsNullOrWhiteSpace(vExists.Idd.MobileNumber1))
                                    {
                                        UtilityController.BL_AssetProfileLinked(mObjEmailDetails);
                                    }
                                }
                                mObjScope.Complete();
                                FlashMessage.Info("Government Created Successfully and Linked to Asset");
                                return RedirectToAction("Details", "ProfileBuilding", new { id = pObjGovernmentModel.AssetID, name = pObjGovernmentModel.AssetRIN });
                            }
                            else
                            {
                                throw new Exception(mObjResponse.Message);
                            }
                        }
                        else
                        {
                            UI_FillGovernmentDropDown(pObjGovernmentModel);
                            ViewBag.Message = mObjResponse.Message;
                            return View(pObjGovernmentModel);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.SendErrorToText(ex);
                        ErrorSignal.FromCurrentContext().Raise(ex);
                        UI_FillGovernmentDropDown(pObjGovernmentModel);
                        Transaction.Current.Rollback();
                        ViewBag.Message = "Error occurred while saving government";
                        return View(pObjGovernmentModel);
                    }
                }
            }

        }

        public void UI_FillSpecialDropDown(TPSpecialViewModel pObjSpecialViewModel = null)
        {
            if (pObjSpecialViewModel != null)
                pObjSpecialViewModel.TaxPayerTypeID = (int)EnumList.TaxPayerType.Special;
            else if (pObjSpecialViewModel == null)
                pObjSpecialViewModel = new TPSpecialViewModel();

            UI_FillTaxOfficeDropDown(new Tax_Offices() { intStatus = 1, IncludeTaxOfficeIds = pObjSpecialViewModel.TaxOfficeID.ToString() });
            UI_FillTaxPayerTypeDropDown(new TaxPayer_Types() { intStatus = 1, IncludeTaxPayerTypeIds = pObjSpecialViewModel.TaxPayerTypeID.ToString() }, (int)EnumList.TaxPayerType.Special);
            UI_FillNotificationMethodDropDown(new Notification_Method() { intStatus = 1, IncludeNotificationMethodIds = pObjSpecialViewModel.NotificationMethodID.ToString() });
            UI_FillTaxPayerRoleDropDown(new TaxPayer_Roles() { TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual, AssetTypeID = (int)EnumList.AssetTypes.Building });

            MAP_Building_BuildingUnit mObjBuildingUnit = new MAP_Building_BuildingUnit()
            {
                BuildingID = pObjSpecialViewModel.AssetID
            };

            IList<usp_GetBuildingUnitNumberList_Result> lstBuildingUnitNumberList = new BLBuilding().BL_GetBuildingUnitNumberList(mObjBuildingUnit);
            ViewBag.BuildingUnitList = new SelectList(lstBuildingUnitNumberList, "BuildingUnitID", "UnitNumber");
        }

        
        public ActionResult AddSpecial(int? id, string name)
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
                    TPSpecialViewModel mObjSpecialModel = new TPSpecialViewModel()
                    {
                        AssetID = mObjBuildingData.BuildingID.GetValueOrDefault(),
                        AssetName = mObjBuildingData.BuildingName,
                        AssetRIN = mObjBuildingData.BuildingRIN,
                        AssetLGAName = mObjBuildingData.LGAName,
                        AssetTypeID = (int)EnumList.AssetTypes.Building,
                        AssetTypeName = mObjBuildingData.AssetTypeName,
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Special,
                    };

                    UI_FillSpecialDropDown(mObjSpecialModel);
                    return View(mObjSpecialModel);
                }
                else
                {
                    return RedirectToAction("Search", "ProfileBuilding");
                }
            }
            else
            {
                return RedirectToAction("Search", "ProfileBuilding");
            }
        }

        [HttpPost]
        
        [ValidateAntiForgeryToken()]
        public ActionResult AddSpecial(TPSpecialViewModel pObjSpecialModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillSpecialDropDown(pObjSpecialModel);
                return View(pObjSpecialModel);
            }
            else
            {
                using (TransactionScope mObjScope = new TransactionScope())
                {
                    try
                    {
                        Special mObjSpecial = new Special()
                        {
                            SpecialID = 0,
                            SpecialTaxPayerName = pObjSpecialModel.SpecialName,
                            TIN = pObjSpecialModel.TIN,
                            TaxOfficeID = pObjSpecialModel.TaxOfficeID,
                            TaxPayerTypeID = (int)EnumList.TaxPayerType.Special,
                            ContactNumber = pObjSpecialModel.ContactNumber,
                            ContactEmail = pObjSpecialModel.ContactEmail,
                            ContactName = pObjSpecialModel.ContactName,
                            Description = pObjSpecialModel.Description,
                            NotificationMethodID = pObjSpecialModel.NotificationMethodID,
                            Active = true,
                            CreatedBy = SessionManager.UserID,
                            CreatedDate = CommUtil.GetCurrentDateTime()
                        };

                        FuncResponse<Special> mObjResponse = new BLSpecial().BL_InsertUpdateSpecial(mObjSpecial);

                        if (mObjResponse.Success)
                        {
                            //Creating mapping between individual and Building
                            MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                            {
                                AssetTypeID = pObjSpecialModel.AssetTypeID,
                                AssetID = pObjSpecialModel.AssetID,
                                TaxPayerTypeID = (int)EnumList.TaxPayerType.Special,
                                TaxPayerRoleID = pObjSpecialModel.TaxPayerRoleID,
                                TaxPayerID = mObjResponse.AdditionalData.SpecialID,
                                BuildingUnitID = pObjSpecialModel.BuildingUnitID,
                                Active = true,
                                CreatedBy = SessionManager.UserID,
                                CreatedDate = CommUtil.GetCurrentDateTime()
                            };

                            FuncResponse mObjTPResponse = new BLTaxPayerAsset().BL_InsertTaxPayerAsset(mObjTaxPayerAsset);

                            if (mObjTPResponse.Success)
                            {
                                var vExists = (from tpa in _db.MAP_TaxPayer_Asset
                                               join aa in _db.Asset_Types on tpa.AssetTypeID equals aa.AssetTypeID
                                               join tp in _db.TaxPayer_Roles on tpa.TaxPayerRoleID equals tp.TaxPayerRoleID
                                               join tpx in _db.TaxPayer_Types on tpa.TaxPayerTypeID equals tpx.TaxPayerTypeID
                                               join idd in _db.Individuals on tpa.TaxPayerID equals idd.IndividualID
                                               where tpa.AssetTypeID == mObjTaxPayerAsset.AssetTypeID && tpa.AssetID == mObjTaxPayerAsset.AssetID
                                                  && tpa.TaxPayerTypeID == mObjTaxPayerAsset.TaxPayerTypeID && tpa.TaxPayerID == mObjTaxPayerAsset.TaxPayerID
                                                  && tpa.TaxPayerRoleID == mObjTaxPayerAsset.TaxPayerRoleID && tpa.Active == true

                                               select new { Post = tpa, Meta = tp, All = tpx, Idd = idd, Aa = aa }).FirstOrDefault();

                                if (GlobalDefaultValues.SendNotification)
                                {
                                    //Send Notification
                                    EmailDetails mObjEmailDetails = new EmailDetails()
                                    {
                                        TaxPayerTypeID = vExists.Post.TaxPayerTypeID.GetValueOrDefault(),
                                        TaxPayerTypeName = vExists.All.TaxPayerTypeName,
                                        TaxPayerID = vExists.Idd.IndividualID,
                                        TaxPayerName = vExists.Idd.FirstName + " " + vExists.Idd.LastName,
                                        TaxPayerRIN = vExists.Idd.IndividualRIN,
                                        TaxPayerRoleName = vExists.Meta.TaxPayerRoleName,
                                        AssetName = vExists.Aa.AssetTypeName,
                                        LoggedInUserID = SessionManager.UserID,
                                    };

                                    if (!string.IsNullOrWhiteSpace(vExists.Idd.EmailAddress1))
                                    {
                                        BLEmailHandler.BL_AssetProfileLinked(mObjEmailDetails);
                                    }

                                    if (!string.IsNullOrWhiteSpace(vExists.Idd.MobileNumber1))
                                    {
                                        UtilityController.BL_AssetProfileLinked(mObjEmailDetails);
                                    }
                                }
                                mObjScope.Complete();
                                FlashMessage.Info("Special Created Successfully and Linked to Asset");
                                return RedirectToAction("Details", "ProfileBuilding", new { id = pObjSpecialModel.AssetID, name = pObjSpecialModel.AssetRIN });
                            }
                            else
                            {
                                throw new Exception(mObjResponse.Message);
                            }
                        }
                        else
                        {
                            UI_FillSpecialDropDown(pObjSpecialModel);
                            ViewBag.Message = mObjResponse.Message;
                            return View(pObjSpecialModel);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.SendErrorToText(ex);
                        ErrorSignal.FromCurrentContext().Raise(ex);
                        UI_FillSpecialDropDown(pObjSpecialModel);
                        Transaction.Current.Rollback();
                        ViewBag.Message = "Error occurred while saving special";
                        return View(pObjSpecialModel);
                    }
                }
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