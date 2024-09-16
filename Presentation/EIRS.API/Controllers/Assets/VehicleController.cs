using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using EIRS.API.Utility;
using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;

namespace EIRS.API.Controllers.Assets
{
    [RoutePrefix("Asset/Vehicle")]

    public class VehicleController : BaseController
    {
        /// <summary>
        /// Returns a list of vehicle
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("List")]
        public IHttpActionResult List()
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {

                Vehicle mObjVehicle = new Vehicle()
                {
                    intStatus = 1
                };

                IList<usp_GetVehicleList_Result> lstVehicle = new BLVehicle().BL_GetVehicleList(mObjVehicle);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstVehicle;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpPost]
        [Route("Insert")]
        public IHttpActionResult Insert(VehicleViewModel pObjVehicleModel)
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
                Vehicle mObjVehicle = new Vehicle()
                {
                    VehicleID = 0,
                    VehicleRegNumber = pObjVehicleModel.VehicleRegNumber,
                    VIN = pObjVehicleModel.VIN != null ? pObjVehicleModel.VIN.Trim() : pObjVehicleModel.VIN,
                    AssetTypeID = (int)EnumList.AssetTypes.Vehicles,
                    VehicleTypeID = pObjVehicleModel.VehicleTypeID,
                    VehicleSubTypeID = pObjVehicleModel.VehicleSubTypeID,
                    LGAID = pObjVehicleModel.LGAID,
                    VehiclePurposeID = pObjVehicleModel.VehiclePurposeID,
                    VehicleFunctionID = pObjVehicleModel.VehicleFunctionID,
                    VehicleOwnershipID = pObjVehicleModel.VehicleOwnershipID,
                    VehicleDescription = pObjVehicleModel.VehicleDescription,
                    Active = true,
                    CreatedBy = userId.HasValue ? userId : 22,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse<Vehicle> mObjFuncResponse = new BLVehicle().BL_InsertUpdateVehicle(mObjVehicle);

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
                    mObjAPIResponse.Success = false;
                    mObjAPIResponse.Message = "Error occurred while saving vehicle";
                }
            }

            return Ok(mObjAPIResponse);
        } 

        [HttpPost]
        [Route("Update")]
        public IHttpActionResult Update(VehicleViewModel pObjVehicleModel)
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
                Vehicle mObjVehicle = new Vehicle()
                {
                    VehicleID = pObjVehicleModel.VehicleID,
                    VehicleRegNumber = pObjVehicleModel.VehicleRegNumber,
                    VIN = pObjVehicleModel.VIN != null ? pObjVehicleModel.VIN.Trim() : pObjVehicleModel.VIN,
                    AssetTypeID = (int)EnumList.AssetTypes.Vehicles,
                    VehicleTypeID = pObjVehicleModel.VehicleTypeID,
                    VehicleSubTypeID = pObjVehicleModel.VehicleSubTypeID,
                    LGAID = pObjVehicleModel.LGAID,
                    VehiclePurposeID = pObjVehicleModel.VehiclePurposeID,
                    VehicleFunctionID = pObjVehicleModel.VehicleFunctionID,
                    VehicleOwnershipID = pObjVehicleModel.VehicleOwnershipID,
                    VehicleDescription = pObjVehicleModel.VehicleDescription,
                    Active = true,
                    ModifiedBy = userId.HasValue ? userId : 22,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse<Vehicle> mObjFuncResponse = new BLVehicle().BL_InsertUpdateVehicle(mObjVehicle);

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
                    mObjAPIResponse.Success = false;
                    mObjAPIResponse.Message = "Error occurred while saving vehicle";
                }
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("SearchVehicleByRegNumber")]
        public IHttpActionResult SearchVehicleByRegNumber(string RegNumber)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_SearchVehicleByRegNumber_Result> lstVehicle = new BLVehicle().BL_SearchVehicleByRegNumber(RegNumber);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstVehicle;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("GetVehicleTaxPayerList")]
        public IHttpActionResult GetVehicleTaxPayerList(int VehicleID)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetVehicleTaxPayerList_Result> lstVehicleTaxPayer = new BLVehicle().BL_GetVehicleTaxPayerList(VehicleID);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstVehicleTaxPayer;
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
