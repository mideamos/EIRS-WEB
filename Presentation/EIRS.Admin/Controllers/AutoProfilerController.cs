using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;
using Elmah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vereyon.Web;

namespace EIRS.Admin.Controllers
{
    public class AutoProfilerController : BaseController
    {
        // GET: AutoProfiler


        public void UI_FillDropDown(APBuildingViewModel pObjAPBuildingModelView = null)
        {
            if (pObjAPBuildingModelView != null)
                pObjAPBuildingModelView.AssetTypeID = (int)EnumList.AssetTypes.Building;
            else if (pObjAPBuildingModelView == null)
                pObjAPBuildingModelView = new APBuildingViewModel();

            UI_FillTownDropDown(new Town() { intStatus = 1, IncludeTownIds = pObjAPBuildingModelView.TownID.ToString() });
            UI_FillLGADropDown(new LGA() { intStatus = 1, IncludeLGAIds = pObjAPBuildingModelView.LGAID.ToString() });
            UI_FillWardDropDown(new Ward() { intStatus = 1, LGAID = pObjAPBuildingModelView.LGAID, IncludeWardIds = pObjAPBuildingModelView.WardID.ToString() });
            UI_FillAssetTypeDropDown(new Asset_Types() { intStatus = 1, IncludeAssetTypeIds = pObjAPBuildingModelView.AssetTypeID.ToString() }, (int)EnumList.AssetTypes.Building);
            UI_FillBuildingTypeDropDown(new Building_Types() { intStatus = 1, IncludeBuildingTypeIds = pObjAPBuildingModelView.BuildingTypeID.ToString() });
            UI_FillBuildingCompletionDropDown(new Building_Completion() { intStatus = 1, IncludeBuildingCompletionIds = pObjAPBuildingModelView.BuildingCompletionID.ToString() });
            UI_FillBuildingPurposeDropDown(new Building_Purpose() { intStatus = 1, IncludeBuildingPurposeIds = pObjAPBuildingModelView.BuildingPurposeID.ToString() });
            UI_FillBuildingOwnershipDropDown(new Building_Ownership() { intStatus = 1, IncludeBuildingOwnershipIds = pObjAPBuildingModelView.BuildingOwnershipID.ToString() });
        }

        public void UI_FillLandDropDown(APLandViewModel pObjAPLandModelView = null)
        {
            if (pObjAPLandModelView != null)
                pObjAPLandModelView.AssetTypeID = (int)EnumList.AssetTypes.Building;
            else if (pObjAPLandModelView == null)
                pObjAPLandModelView = new APLandViewModel();

            UI_FillTownDropDown(new Town() { intStatus = 1, IncludeTownIds = pObjAPLandModelView.TownID.ToString() });
            UI_FillLGADropDown(new LGA() { intStatus = 1, IncludeLGAIds = pObjAPLandModelView.LGAID.ToString() });
            UI_FillWardDropDown(new Ward() { intStatus = 1, LGAID = pObjAPLandModelView.LGAID, IncludeWardIds = pObjAPLandModelView.WardID.ToString() });
            UI_FillAssetTypeDropDown(new Asset_Types() { intStatus = 1, IncludeAssetTypeIds = pObjAPLandModelView.AssetTypeID.ToString() }, (int)EnumList.AssetTypes.Building);
            UI_FillBuildingCompletionDropDown(new Building_Completion() { intStatus = 1, IncludeBuildingCompletionIds = pObjAPLandModelView.LandStreetConditionID.ToString() });
            UI_FillLandPurposeDropDown(new Land_Purpose { intStatus = 1, IncludeLandPurposeIds = pObjAPLandModelView.LandPurposeID.ToString() });
            UI_FillLandOwnershipDropDown(new Land_Ownership() { intStatus = 1, IncludeLandOwnershipIds = pObjAPLandModelView.LandOwnershipID.ToString() });
            UI_FillLandStreetConditionDropDown(new Land_StreetCondition() { intStatus = 1, IncludeLandStreetConditionIds = pObjAPLandModelView.LandStreetConditionID.ToString() });
            UI_FillLandFunctionDropDown(new Land_Function() { intStatus = 1, IncludeLandFunctionIds = pObjAPLandModelView.LandFunctionID.ToString() });
            UI_FillLandDevelopmentDropDown( new Land_Development(){ intStatus = 1, IncludeLandDevelopmentIds = pObjAPLandModelView.LandDevelopmentID.ToString() });
        }

        public void UI_FillBusinessDropDown(APBusinessViewModel pObjAPBusinessViewModel = null)
        {
            if (pObjAPBusinessViewModel != null)
                pObjAPBusinessViewModel.AssetTypeID = (int)EnumList.AssetTypes.Business;
            else if (pObjAPBusinessViewModel == null)
                pObjAPBusinessViewModel = new APBusinessViewModel();

            UI_FillAssetTypeDropDown(new Asset_Types() { intStatus = 1, IncludeAssetTypeIds = pObjAPBusinessViewModel.AssetTypeID.ToString() }, (int)EnumList.AssetTypes.Business);
            UI_FillBusinessTypeDropDown(new Business_Types() { intStatus = 1, IncludeBusinessTypeIds = pObjAPBusinessViewModel.BusinessTypeID.ToString() });
            UI_FillLGADropDown(new LGA() { intStatus = 1, IncludeLGAIds = pObjAPBusinessViewModel.LGAID.ToString() });
            UI_FillBusinessCategoryDropDown(new Business_Category() { intStatus = 1, IncludeBusinessCategoryIds = pObjAPBusinessViewModel.BusinessCategoryID.ToString(), BusinessTypeID = pObjAPBusinessViewModel.BusinessTypeID });
            UI_FillBusinessSectorDropDown(new Business_Sector() { intStatus = 1, IncludeBusinessSectorIds = pObjAPBusinessViewModel.BusinessSectorID.ToString(), BusinessTypeID = pObjAPBusinessViewModel.BusinessTypeID, BusinessCategoryID = pObjAPBusinessViewModel.BusinessCategoryID });
            UI_FillBusinessSubSectorDropDown(new Business_SubSector() { intStatus = 1, IncludeBusinessSubSectorIds = pObjAPBusinessViewModel.BusinessSubSectorID.ToString(), BusinessSectorID = pObjAPBusinessViewModel.BusinessSectorID });
            UI_FillBusinessStructureDropDown(new Business_Structure() { intStatus = 1, IncludeBusinessStructureIds = pObjAPBusinessViewModel.BusinessStructureID.ToString(), BusinessTypeID = pObjAPBusinessViewModel.BusinessTypeID });
            UI_FillBusinessOperationDropDown(new Business_Operation() { intStatus = 1, IncludeBusinessOperationIds = pObjAPBusinessViewModel.BusinessOperationID.ToString(), BusinessTypeID = pObjAPBusinessViewModel.BusinessTypeID });
            UI_FillSizeDropDown(new Size() { intStatus = 1, IncludeSizeIds = pObjAPBusinessViewModel.SizeID.ToString() });
        }

        public void UI_FillBuildingUnitDropDown(APBuildingUnitViewModel pObjAPBuildingUnitViewModel = null)
        {
            if (pObjAPBuildingUnitViewModel == null)
                pObjAPBuildingUnitViewModel = new APBuildingUnitViewModel();

            UI_FillUnitPurposeDropDown(new Unit_Purpose() { intStatus = 1, IncludeUnitPurposeIds = pObjAPBuildingUnitViewModel.UnitPurposeID.ToString() });
            UI_FillUnitFunctionDropDown(new Unit_Function() { intStatus = 1, UnitPurposeID = pObjAPBuildingUnitViewModel.UnitPurposeID, IncludeUnitFunctionIds = pObjAPBuildingUnitViewModel.UnitFunctionID.ToString() });
            UI_FillUnitOccupancyDropDown(new Unit_Occupancy() { intStatus = 1, IncludeUnitOccupancyIds = pObjAPBuildingUnitViewModel.UnitOccupancyID.ToString() });
            UI_FillSizeDropDown(new Size() { intStatus = 1, IncludeSizeIds = pObjAPBuildingUnitViewModel.SizeID.ToString() });
        }

        public void UI_FillVehicleDropDown(APVehicleViewModel pObjAPVehicleViewModel = null)
        {
            if (pObjAPVehicleViewModel != null)
                pObjAPVehicleViewModel.AssetTypeID = (int)EnumList.AssetTypes.Vehicles;
            else if (pObjAPVehicleViewModel == null)
                pObjAPVehicleViewModel = new APVehicleViewModel();

            UI_FillLGADropDown(new LGA() { intStatus = 1, IncludeLGAIds = pObjAPVehicleViewModel.LGAID.ToString() });
            UI_FillAssetTypeDropDown(new Asset_Types() { intStatus = 1, IncludeAssetTypeIds = pObjAPVehicleViewModel.AssetTypeID.ToString() }, (int)EnumList.AssetTypes.Vehicles);
            UI_FillVehicleTypeDropDown(new Vehicle_Types() { intStatus = 1, IncludeVehicleTypeIds = pObjAPVehicleViewModel.VehicleTypeID.ToString() });
            UI_FillVehicleSubTypeDropDown(new Vehicle_SubTypes() { intStatus = 1, IncludeVehicleSubTypeIds = pObjAPVehicleViewModel.VehicleSubTypeID.ToString(), VehicleTypeID = pObjAPVehicleViewModel.VehicleTypeID });
            UI_FillVehiclePurposeDropDown(new Vehicle_Purpose() { intStatus = 1, IncludeVehiclePurposeIds = pObjAPVehicleViewModel.VehiclePurposeID.ToString() });
            UI_FillVehicleFunctionDropDown(new Vehicle_Function() { intStatus = 1, IncludeVehicleFunctionIds = pObjAPVehicleViewModel.VehicleFunctionID.ToString(), VehiclePurposeID = pObjAPVehicleViewModel.VehiclePurposeID });
            UI_FillVehicleOwnershipDropDown(new Vehicle_Ownership() { intStatus = 1, IncludeVehicleOwnershipIds = pObjAPVehicleViewModel.VehicleOwnershipID.ToString() });
        }
        public ActionResult UpdateBuilding()
        {
            AP_Building mObjBuilding = new AP_Building();
            AP_Building mObjBuildingData = new BLAutoProfiler().BL_GetBuildingDefaultValue(mObjBuilding);

            if (mObjBuildingData != null)
            {
                APBuildingViewModel mObjAPBuildingModelView = new APBuildingViewModel()
                {
                    BuildingID = mObjBuildingData.BuildingID,
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
                UI_FillDropDown(mObjAPBuildingModelView);
                return View(mObjAPBuildingModelView);
            }
            else
            {
                return View();
            }
        }
        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult UpdateBuilding(APBuildingViewModel pObjAPBuildingModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown(pObjAPBuildingModel);
                return View(pObjAPBuildingModel);
            }
            else
            {
                AP_Building mObjBuilding = new AP_Building()
                {
                    BuildingID = pObjAPBuildingModel.BuildingID,
                    BuildingNumber = pObjAPBuildingModel.BuildingNumber.Trim(),
                    StreetName = pObjAPBuildingModel.StreetName.Trim(),
                    OffStreetName = pObjAPBuildingModel.OffStreetName != null ? pObjAPBuildingModel.OffStreetName.Trim() : pObjAPBuildingModel.OffStreetName,
                    TownID = pObjAPBuildingModel.TownID,
                    LGAID = pObjAPBuildingModel.LGAID,
                    WardID = pObjAPBuildingModel.WardID,
                    AssetTypeID = (int)EnumList.AssetTypes.Building,
                    BuildingTypeID = pObjAPBuildingModel.BuildingTypeID,
                    BuildingCompletionID = pObjAPBuildingModel.BuildingCompletionID,
                    BuildingPurposeID = pObjAPBuildingModel.BuildingPurposeID,
                    BuildingOwnershipID = pObjAPBuildingModel.BuildingOwnershipID,
                    NoOfUnits = pObjAPBuildingModel.NoOfUnits,
                    BuildingSize_Length = pObjAPBuildingModel.BuildingSize_Length,
                    BuildingSize_Width = pObjAPBuildingModel.BuildingSize_Width,
                    Latitude = pObjAPBuildingModel.Latitude,
                    Longitude = pObjAPBuildingModel.Longitude,
                    Active = pObjAPBuildingModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };
                try
                {
                    FuncResponse mObjResponse = new BLAutoProfiler().BL_UpdateBuildingDefaultValue(mObjBuilding);
                    if (mObjResponse.Success)
                    {

                        FlashMessage.Info(mObjResponse.Message);
                        
                        UI_FillDropDown(pObjAPBuildingModel);
                        return View(pObjAPBuildingModel);
                    }
                    else
                    {
                        UI_FillDropDown(pObjAPBuildingModel);
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjAPBuildingModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillDropDown(pObjAPBuildingModel);
                    ViewBag.Message = "Error occurred while saving AP building";
                    return View(pObjAPBuildingModel);
                }
            }
        }

        public ActionResult UpdateLand()
        {
            AP_Land mObjLand = new AP_Land();
            AP_Land mObjLandData = new BLAutoProfiler().BL_GetLandDefaultValue(mObjLand);
            if (mObjLandData != null)
            {
                APLandViewModel mObjLandViewModel = new APLandViewModel()
                {
                    Active = mObjLandData.Active.GetValueOrDefault(),
                    LandDevelopmentID = mObjLandData.LandDevelopmentID.GetValueOrDefault(),
                    LandID = mObjLandData.LandID,
                    PlotNumber = mObjLandData.PlotNumber,
                    LandPurposeID = mObjLandData.LandPurposeID.GetValueOrDefault(),

                    C_OF_O_Ref = mObjLandData.C_OF_O_Ref,
                    LandOccupier = mObjLandData.LandOccupier,
                    LandRIN = mObjLandData.LandRIN,
                    ValueOfLand=mObjLandData.ValueOfLand,
                    Neighborhood=mObjLandData.Neighborhood,
                    LandStreetConditionID = mObjLandData.LandStreetConditionID.GetValueOrDefault(),
                    AssetTypeID = mObjLandData.AssetTypeID.GetValueOrDefault(),
                    LandFunctionID = mObjLandData.LandFunctionID.GetValueOrDefault(),
                    LandOwnershipID = mObjLandData.LandOwnershipID.GetValueOrDefault(),
                    LandSize_Length = mObjLandData.LandSize_Length,
                    Latitude = mObjLandData.Latitude,
                    LGAID = mObjLandData.LGAID.GetValueOrDefault(),
                    Longitude = mObjLandData.Longitude,
                    StreetName = mObjLandData.StreetName,
                    TownID = mObjLandData.TownID.GetValueOrDefault(),
                    WardID = mObjLandData.WardID.GetValueOrDefault(),
                    LandSize_Width = mObjLandData.LandSize_Width,
                };
                UI_FillLandDropDown(mObjLandViewModel);
                return View(mObjLandViewModel);

            }
            else
            {
                return View();
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult UpdateLand(APLandViewModel pObjAPLandModel)
        {
            if(!ModelState.IsValid)
            {
                UI_FillLandDropDown(pObjAPLandModel);
                return View(pObjAPLandModel);
            }
            else
            {
                AP_Land mObjLand = new AP_Land()
                {
                    LandID = pObjAPLandModel.LandID,
                    LandRIN=pObjAPLandModel.LandRIN,
                    ValueOfLand = pObjAPLandModel.ValueOfLand,
                    Neighborhood = pObjAPLandModel.Neighborhood,
                    LandOccupier =pObjAPLandModel.LandOccupier,
                    PlotNumber = pObjAPLandModel.PlotNumber.Trim(),
                    StreetName = pObjAPLandModel.StreetName.Trim(),
                    TownID = pObjAPLandModel.TownID,
                    LGAID = pObjAPLandModel.LGAID,
                    WardID = pObjAPLandModel.WardID,
                    AssetTypeID = (int)EnumList.AssetTypes.Building,
                    LandSize_Length = pObjAPLandModel.LandSize_Length,
                    LandSize_Width = pObjAPLandModel.LandSize_Width,
                    C_OF_O_Ref = pObjAPLandModel.C_OF_O_Ref,
                    LandPurposeID = pObjAPLandModel.LandPurposeID,
                    LandFunctionID = pObjAPLandModel.LandFunctionID,
                    LandOwnershipID = pObjAPLandModel.LandOwnershipID,
                    LandDevelopmentID = pObjAPLandModel.LandDevelopmentID,
                    Latitude = pObjAPLandModel.Latitude,
                    Longitude = pObjAPLandModel.Longitude,
                    LandStreetConditionID = pObjAPLandModel.LandStreetConditionID,
                    Active = pObjAPLandModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };
                try
                {
                    FuncResponse mObjResponse = new BLAutoProfiler().BL_UpdateLandDefaultValue(mObjLand);
                    if (mObjResponse.Success)
                    {

                        FlashMessage.Info(mObjResponse.Message);
                        UI_FillLandDropDown(pObjAPLandModel);
                        return View(pObjAPLandModel);
                    }
                    else
                    {
                        UI_FillLandDropDown(pObjAPLandModel);
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjAPLandModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillLandDropDown(pObjAPLandModel);
                    ViewBag.Message = "Error occurred while saving AP Land";
                    return View(pObjAPLandModel);
                }
            }
        }


        public ActionResult UpdateVehicle()
        {
            AP_Vehicle mObjVehicle = new AP_Vehicle();
            AP_Vehicle mObjVehicleData = new BLAutoProfiler().BL_GetVehicleDefaultValue(mObjVehicle);
            if (mObjVehicleData != null)
            {
                APVehicleViewModel mObjVehicleViewModel = new APVehicleViewModel()
                {
                    VehicleID = mObjVehicleData.VehicleID,
                    VehicleRIN = mObjVehicleData.VehicleRIN,
                    VehicleRegNumber = mObjVehicleData.VehicleRegNumber,
                    VIN = mObjVehicleData.VIN,
                    AssetTypeID = mObjVehicleData.AssetTypeID.GetValueOrDefault(),
                    VehicleTypeID = mObjVehicleData.VehicleTypeID.GetValueOrDefault(),
                    VehicleSubTypeID = mObjVehicleData.VehicleSubTypeID.GetValueOrDefault(),
                    LGAID = mObjVehicleData.LGAID.GetValueOrDefault(),
                    VehiclePurposeID = mObjVehicleData.VehiclePurposeID.GetValueOrDefault(),
                    VehicleFunctionID = mObjVehicleData.VehicleFunctionID.GetValueOrDefault(),
                    VehicleOwnershipID = mObjVehicleData.VehicleOwnershipID.GetValueOrDefault(),
                    VehicleDescription = mObjVehicleData.VehicleDescription,
                    Active = mObjVehicleData.Active.GetValueOrDefault(),

                };
                UI_FillVehicleDropDown(mObjVehicleViewModel);
                return View(mObjVehicleViewModel);

            }
            else
            {
                return View();
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult UpdateVehicle(APVehicleViewModel pObjAPVehicleModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillVehicleDropDown(pObjAPVehicleModel);
                return View(pObjAPVehicleModel);
            }
            else
            {
                AP_Vehicle mObjVehicle = new AP_Vehicle()
                {
                    VehicleID = pObjAPVehicleModel.VehicleID,
                    VehicleRIN=pObjAPVehicleModel.VehicleRIN,
                    VehicleRegNumber = pObjAPVehicleModel.VehicleRegNumber,
                    VIN = pObjAPVehicleModel.VIN != null ? pObjAPVehicleModel.VIN.Trim() : pObjAPVehicleModel.VIN,
                    AssetTypeID = (int)EnumList.AssetTypes.Vehicles,
                    VehicleTypeID = pObjAPVehicleModel.VehicleTypeID,
                    VehicleSubTypeID = pObjAPVehicleModel.VehicleSubTypeID,
                    LGAID = pObjAPVehicleModel.LGAID,
                    VehiclePurposeID = pObjAPVehicleModel.VehiclePurposeID,
                    VehicleFunctionID = pObjAPVehicleModel.VehicleFunctionID,
                    VehicleOwnershipID = pObjAPVehicleModel.VehicleOwnershipID,
                    VehicleDescription = pObjAPVehicleModel.VehicleDescription,
                    Active = pObjAPVehicleModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };
                try
                {
                    FuncResponse mObjResponse = new BLAutoProfiler().BL_UpdateVehicleDefaultValue(mObjVehicle);
                    if (mObjResponse.Success)
                    {

                        FlashMessage.Info(mObjResponse.Message);
                        UI_FillVehicleDropDown(pObjAPVehicleModel);
                        return View(pObjAPVehicleModel);
                    }
                    else
                    {
                        UI_FillVehicleDropDown(pObjAPVehicleModel);
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjAPVehicleModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillVehicleDropDown(pObjAPVehicleModel);
                    ViewBag.Message = "Error occurred while saving AP Vehicle";
                    return View(pObjAPVehicleModel);
                }
            }
        }

        public ActionResult UpdateBusiness()
        {
            AP_Business mObjBusiness = new AP_Business();
            AP_Business mObjBusinessData = new BLAutoProfiler().BL_GetBusinessDefaultValue(mObjBusiness);

            if (mObjBusinessData != null)
            {
                APBusinessViewModel mObjBusinessModelView = new APBusinessViewModel()
                {
                    BusinessID = mObjBusinessData.BusinessID,
                    BusinessRIN = mObjBusinessData.BusinessRIN,
                    AssetTypeID = mObjBusinessData.AssetTypeID.GetValueOrDefault(),
                    BusinessTypeID = mObjBusinessData.BusinessTypeID.GetValueOrDefault(),
                    BusinessName = mObjBusinessData.BusinessName,
                    LGAID = mObjBusinessData.LGAID.GetValueOrDefault(),
                    BusinessCategoryID = mObjBusinessData.BusinessCategoryID.GetValueOrDefault(),
                    BusinessSectorID = mObjBusinessData.BusinessSectorID.GetValueOrDefault(),
                    BusinessSubSectorID = mObjBusinessData.BusinessSubSectorID.GetValueOrDefault(),
                    BusinessStructureID = mObjBusinessData.BusinessStructureID.GetValueOrDefault(),
                    BusinessOperationID = mObjBusinessData.BusinessOperationID.GetValueOrDefault(),
                    SizeID = mObjBusinessData.SizeID.GetValueOrDefault(),
                    Active = mObjBusinessData.Active.GetValueOrDefault(),
                    ContactName = mObjBusinessData.ContactName,
                    BusinessAddress = mObjBusinessData.BusinessAddress,
                    BusinessNumber = mObjBusinessData.BusinessNumber,
                };
                UI_FillBusinessDropDown(mObjBusinessModelView);
                return View(mObjBusinessModelView);

            }
            else
            {
                return View();
            }
        }


        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult UpdateBusiness(APBusinessViewModel pObjAPBusinessModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillBusinessDropDown(pObjAPBusinessModel);
                return View(pObjAPBusinessModel);
            }
            else
            {
                AP_Business mObjBusiness = new AP_Business()
                {
                    BusinessID = pObjAPBusinessModel.BusinessID,
                    BusinessRIN=pObjAPBusinessModel.BusinessRIN,
                    AssetTypeID = (int)EnumList.AssetTypes.Business,
                    BusinessTypeID = pObjAPBusinessModel.BusinessTypeID,
                    BusinessName = pObjAPBusinessModel.BusinessName,
                    LGAID = pObjAPBusinessModel.LGAID,
                    BusinessCategoryID = pObjAPBusinessModel.BusinessCategoryID,
                    BusinessSectorID = pObjAPBusinessModel.BusinessSectorID,
                    BusinessSubSectorID = pObjAPBusinessModel.BusinessSubSectorID,
                    BusinessStructureID = pObjAPBusinessModel.BusinessStructureID,
                    BusinessOperationID = pObjAPBusinessModel.BusinessOperationID,
                    SizeID = pObjAPBusinessModel.SizeID,
                    ContactName = pObjAPBusinessModel.ContactName,
                    BusinessAddress = pObjAPBusinessModel.BusinessAddress,
                    BusinessNumber = pObjAPBusinessModel.BusinessNumber,
                    Active = pObjAPBusinessModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {
                    FuncResponse mObjResponse = new BLAutoProfiler().BL_UpdateBusinessDefaultValue(mObjBusiness);
                    if (mObjResponse.Success)
                    {

                        FlashMessage.Info(mObjResponse.Message);
                        UI_FillBusinessDropDown(pObjAPBusinessModel);
                        return View(pObjAPBusinessModel);
                    }
                    else
                    {
                        UI_FillBusinessDropDown(pObjAPBusinessModel);
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjAPBusinessModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillBusinessDropDown(pObjAPBusinessModel);
                    ViewBag.Message = "Error occurred while saving AP Business";
                    return View(pObjAPBusinessModel);
                }
            }
        }



        public ActionResult UpdateBuildingUnit()
        {
            AP_Building_Unit mObjBuildingUnit = new AP_Building_Unit();
            AP_Building_Unit mObjBuildingUnitData = new BLAutoProfiler().BL_GetBuildingUnitDefaultValue(mObjBuildingUnit);

            if (mObjBuildingUnitData != null)
            {
                APBuildingUnitViewModel mObjBuildingUnitModelView = new APBuildingUnitViewModel()
                {
                    BuildingUnitID = mObjBuildingUnitData.BuildingUnitID,
                    UnitNumber = mObjBuildingUnitData.UnitNumber,
                    UnitPurposeID = mObjBuildingUnitData.UnitPurposeID.GetValueOrDefault(),
                    UnitFunctionID = mObjBuildingUnitData.UnitFunctionID.GetValueOrDefault(),
                    UnitOccupancyID = mObjBuildingUnitData.UnitOccupancyID.GetValueOrDefault(),
                    SizeID = mObjBuildingUnitData.SizeID.GetValueOrDefault(),
                    Active = mObjBuildingUnitData.Active.GetValueOrDefault(),
                };
                UI_FillBuildingUnitDropDown(mObjBuildingUnitModelView);
                return View(mObjBuildingUnitModelView);

            }
            else
            {
                return View();
            }
        }


        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult UpdateBuildingUnit(APBuildingUnitViewModel pObjAPBuildingUnitModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillBuildingUnitDropDown(pObjAPBuildingUnitModel);
                return View(pObjAPBuildingUnitModel);
            }
            else
            {
                AP_Building_Unit mObjBuildingUnit = new AP_Building_Unit()
                {
                    BuildingUnitID = pObjAPBuildingUnitModel.BuildingUnitID,
                    UnitNumber = pObjAPBuildingUnitModel.UnitNumber,
                    UnitPurposeID = pObjAPBuildingUnitModel.UnitPurposeID,
                    UnitFunctionID = pObjAPBuildingUnitModel.UnitFunctionID,
                    UnitOccupancyID = pObjAPBuildingUnitModel.UnitOccupancyID,
                    SizeID = pObjAPBuildingUnitModel.SizeID,
                    Active = pObjAPBuildingUnitModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {
                    FuncResponse mObjResponse = new BLAutoProfiler().BL_UpdateBuildingUnitDefaultValue(mObjBuildingUnit);
                    if (mObjResponse.Success)
                    {

                        FlashMessage.Info(mObjResponse.Message);
                        UI_FillBuildingUnitDropDown(pObjAPBuildingUnitModel);
                        return View(pObjAPBuildingUnitModel);
                    }
                    else
                    {
                        UI_FillBuildingUnitDropDown(pObjAPBuildingUnitModel);
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjAPBuildingUnitModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillBuildingUnitDropDown(pObjAPBuildingUnitModel);
                    ViewBag.Message = "Error occurred while saving AP BuildingUnit";
                    return View(pObjAPBuildingUnitModel);
                }
            }
        }

    }
}