using EIRS.API.Utility;
using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace EIRS.API.Controllers.Assets
{
    /// <summary>
    /// Building Operations
    /// </summary>
    [RoutePrefix("Asset/Building")]
    
    public class BuildingController : BaseController
    {
        /// <summary>
        /// Returns a list of Building
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("List")]
        public IHttpActionResult List()
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {

                Building mObjBuilding = new Building()
                {
                    intStatus = 1
                };

                IList<usp_GetBuildingList_Result> lstBuilding = new BLBuilding().BL_GetBuildingList(mObjBuilding);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstBuilding;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        /// <summary>
        /// Add New Building
        /// </summary>
        /// <param name="pObjBuildingModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Insert")]
        public IHttpActionResult Insert(BuildingViewModel pObjBuildingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();
            String token = Request.Headers.Authorization.Parameter;
            int? userId = Utilities.GetUserId(token);
            if (!ModelState.IsValid)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            }
            else
            {
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
                   CreatedBy = userId.HasValue ? userId : 22,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse<Building> mObjFuncResponse = new BLBuilding().BL_InsertUpdateBuilding(mObjBuilding);

                    if (mObjFuncResponse.Success)
                    {
                        mObjAPIResponse.Success = true;
                        mObjAPIResponse.Message = mObjFuncResponse.Message;
                        mObjAPIResponse.Result = mObjFuncResponse.AdditionalData;
                    }
                    else
                    {
                        mObjAPIResponse.Success = false;
                        mObjAPIResponse.Message = mObjFuncResponse.Message;
                    }
                }
                catch (Exception ex)
                {
                    mObjAPIResponse.Success = true;
                    mObjAPIResponse.Message = "Error occurred while saving building";
                }
            }

            return Ok(mObjAPIResponse);
        }

        [HttpPost]
        [Route("InsertWithLand")]
        public IHttpActionResult InsertWithLand(BuildingWithLandViewModel pObjBuildingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();
            String token = Request.Headers.Authorization.Parameter;
            int? userId = Utilities.GetUserId(token);
            if (!ModelState.IsValid)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            }
            else
            {
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
                   CreatedBy = userId.HasValue ? userId : 22,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {
                    BLBuilding mObjBLBuilding = new BLBuilding();
                    FuncResponse<Building> mObjFuncResponse = mObjBLBuilding.BL_InsertUpdateBuilding(mObjBuilding);

                    if (mObjFuncResponse.Success)
                    {
                        // Add Mapping Between Land and Building
                        MAP_Building_Land mObjLandBuilding = new MAP_Building_Land()
                        {
                            LandID = pObjBuildingModel.LandID,
                            BuildingID = mObjFuncResponse.AdditionalData.BuildingID,
                           CreatedBy = userId.HasValue ? userId : 22,
                            CreatedDate = CommUtil.GetCurrentDateTime()
                        };

                        FuncResponse mObjBLFuncResponse = mObjBLBuilding.BL_InsertBuildingLand(mObjLandBuilding);

                        if (!mObjBLFuncResponse.Success)
                        {
                            throw (mObjBLFuncResponse.Exception);
                        }

                        foreach (int mIntBuildingUnitID in pObjBuildingModel.BuildingUnitID)
                        {
                            MAP_Building_BuildingUnit mObjBBU = new MAP_Building_BuildingUnit()
                            {
                                BuildingID = mObjFuncResponse.AdditionalData.BuildingID,
                                BuildingUnitID = mIntBuildingUnitID,
                               CreatedBy = userId.HasValue ? userId : 22,
                                CreatedDate = CommUtil.GetCurrentDateTime()
                            };

                           FuncResponse mObjBBFuncResponse = mObjBLBuilding.BL_InsertBuildingUnitNumber(mObjBBU);

                            if (!mObjBBFuncResponse.Success)
                            {
                                throw (mObjBBFuncResponse.Exception);
                            }
                        }


                        mObjAPIResponse.Success = true;
                        mObjAPIResponse.Message = mObjFuncResponse.Message;
                        mObjAPIResponse.Result = mObjFuncResponse.AdditionalData;
                    }
                    else
                    {
                        mObjAPIResponse.Success = false;
                        mObjAPIResponse.Message = mObjFuncResponse.Message;
                    }
                }
                catch (Exception ex)
                {
                    mObjAPIResponse.Success = true;
                    mObjAPIResponse.Message = "Error occurred while saving building";
                }
            }

            return Ok(mObjAPIResponse);
        }

        /// <summary>
        /// Find Building by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Details/{id}")]
        public IHttpActionResult Details(int? id)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
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
                        mObjAPIResponse.Success = true;
                        mObjAPIResponse.Result = mObjBuildingData;
                    }
                    else
                    {
                        mObjAPIResponse.Success = false;
                        mObjAPIResponse.Result = "Invalid Request";
                    }
                }
                else
                {
                    mObjAPIResponse.Success = false;
                    mObjAPIResponse.Result = "Invalid Request";
                }


            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("UnitInformation/{id}")]
        public IHttpActionResult UnitInformation(int? id)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                if (id.GetValueOrDefault() > 0)
                {
                    IList<usp_GetBuildingUnitNumberList_Result> lstUnitInformation = new BLBuilding().BL_GetBuildingUnitNumberList(new MAP_Building_BuildingUnit() { BuildingID = id.GetValueOrDefault() });

                    mObjAPIResponse.Success = true;
                    mObjAPIResponse.Result = lstUnitInformation;
                }
                else
                {
                    mObjAPIResponse.Success = false;
                    mObjAPIResponse.Result = "Invalid Request";
                }


            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

    }
}
