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
    /// Land Operations
    /// </summary>
    [RoutePrefix("Asset/Land")]
    
    public class LandController : BaseController
    {
        /// <summary>
        /// Returns a list of Land
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("List")]
        public IHttpActionResult List()
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {

                Land mObjLand = new Land()
                {
                    intStatus = 1
                };

                IList<usp_GetLandList_Result> lstLand = new BLLand().BL_GetLandList(mObjLand);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstLand;
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
        /// <param name="pObjLandModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Insert")]
        public IHttpActionResult Insert(LandViewModel pObjLandModel)
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
                   CreatedBy = userId.HasValue ? userId : 22,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse<Land> mObjFuncResponse = new BLLand().BL_InsertUpdateLand(mObjLand);

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
                    mObjAPIResponse.Message = "Error occurred while saving land";
                }
            }

            return Ok(mObjAPIResponse);
        }

        /// <summary>
        /// Find Land by id
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
                    Land mObjLand = new Land()
                    {
                        LandID = id.GetValueOrDefault(),
                        intStatus = 2
                    };

                    usp_GetLandList_Result mObjLandData = new BLLand().BL_GetLandDetails(mObjLand);

                    if (mObjLandData != null)
                    {
                        mObjAPIResponse.Success = true;
                        mObjAPIResponse.Result = mObjLandData;
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
        [Route("SearchLandForEdoGIS")]
        public IHttpActionResult SearchLandForEdoGIS(string PlotNumber, string LandOccupier, string LandRIN, int TaxPayerTypeID, int? TaxPayerID)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                Land mObjLand = new Land()
                {
                    LandRIN = LandRIN,
                    PlotNumber = PlotNumber,
                    LandOccupier = LandOccupier,
                    TaxPayerID = TaxPayerID.GetValueOrDefault(),
                    TaxPayerTypeID = TaxPayerTypeID,
                    intStatus = 1
                };

                IList<usp_GetSearchLandForEdoGIS_Result> lstLand = new BLLand().BL_SearchLandForEdoGIS(mObjLand);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstLand;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("BuildingInformation")]
        public IHttpActionResult BuildingInformation(int? landid, int taxpayertypeid, int taxpayerid)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetBuildingLandList_Result> lstBuildingInformation = new BLBuilding().BL_GetBuildingLandList(new MAP_Building_Land() { LandID = landid.GetValueOrDefault(), TaxPayerID = taxpayerid, TaxPayerTypeID = taxpayertypeid });

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstBuildingInformation;
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
