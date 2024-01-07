using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;


namespace EIRS.API.Controllers
{
    /// <summary>
    /// Vehicle Function Operations
    /// </summary>
    [RoutePrefix("ReferenceData/VehicleFunction")]
    public class VehicleFunctionController : BaseController
    {
        /// <summary>
        /// Returns a list of vehicle function
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("List")]
        public IHttpActionResult List(int? VehiclePurposeId = 0)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                Vehicle_Function mObjVehicleFunction = new Vehicle_Function()
                {
                    intStatus = 1,
                    VehiclePurposeID = VehiclePurposeId
                };

                IList<usp_GetVehicleFunctionList_Result> lstVehicleFunction = new BLVehicleFunction().BL_GetVehicleFunctionList(mObjVehicleFunction);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstVehicleFunction;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        ///// <summary>
        ///// Find vehicle function by id
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[HttpGet]
        //[Route("Details/{id}")]
        //public IHttpActionResult Details(int? id)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    try
        //    {
        //        if (id.GetValueOrDefault() > 0)
        //        {
        //            Vehicle_Function mObjVehicleFunction = new Vehicle_Function()
        //            {
        //                VehicleFunctionID = id.GetValueOrDefault(),
        //                intStatus = 2
        //            };

        //            usp_GetVehicleFunctionList_Result mObjVehicleFunctionData = new BLVehicleFunction().BL_GetVehicleFunctionDetails(mObjVehicleFunction);

        //            if (mObjVehicleFunctionData != null)
        //            {
        //                mObjAPIResponse.Success = true;
        //                mObjAPIResponse.Result = mObjVehicleFunctionData;
        //            }
        //            else
        //            {
        //                mObjAPIResponse.Success = false;
        //                mObjAPIResponse.Result = "Invalid Request";
        //            }
        //        }
        //        else
        //        {
        //            mObjAPIResponse.Success = false;
        //            mObjAPIResponse.Result = "Invalid Request";
        //        }


        //    }
        //    catch (Exception Ex)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = Ex.Message;
        //    }

        //    return Ok(mObjAPIResponse);
        //}

        ///// <summary>
        ///// Add New Asset Type
        ///// </summary>
        ///// <param name="pObjVehicleFunctionModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Insert")]
        //public IHttpActionResult Insert(VehicleFunctionViewModel pObjVehicleFunctionModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        Vehicle_Function mObjVehicleFunction = new Vehicle_Function()
        //        {
        //            VehicleFunctionID = 0,
        //            VehicleFunctionName = pObjVehicleFunctionModel.VehicleFunctionName.Trim(),
        //            VehiclePurposeID = pObjVehicleFunctionModel.VehiclePurposeID,
        //            Active = true,
        //           CreatedBy = 22,
        //            CreatedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLVehicleFunction().BL_InsertUpdateVehicleFunction(mObjVehicleFunction);

        //            if (mObjFuncResponse.Success)
        //            {
        //                mObjAPIResponse.Success = true;
        //                mObjAPIResponse.Message = mObjFuncResponse.Message;
        //            }
        //            else
        //            {
        //                mObjAPIResponse.Success = false;
        //                mObjAPIResponse.Message = mObjFuncResponse.Message;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            mObjAPIResponse.Success = true;
        //            mObjAPIResponse.Message = "Error occurred while saving vehicle function";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}

        ///// <summary>
        ///// Update Asset Type
        ///// </summary>
        ///// <param name="pObjVehicleFunctionModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Update")]
        //public IHttpActionResult Update(VehicleFunctionViewModel pObjVehicleFunctionModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        Vehicle_Function mObjVehicleFunction = new Vehicle_Function()
        //        {
        //            VehicleFunctionID = pObjVehicleFunctionModel.VehicleFunctionID,
        //            VehicleFunctionName = pObjVehicleFunctionModel.VehicleFunctionName.Trim(),
        //            VehiclePurposeID = pObjVehicleFunctionModel.VehiclePurposeID,
        //            Active = pObjVehicleFunctionModel.Active,
        //            ModifiedBy = -1,
        //            ModifiedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLVehicleFunction().BL_InsertUpdateVehicleFunction(mObjVehicleFunction);

        //            if (mObjFuncResponse.Success)
        //            {
        //                mObjAPIResponse.Success = true;
        //                mObjAPIResponse.Message = mObjFuncResponse.Message;
        //            }
        //            else
        //            {
        //                mObjAPIResponse.Success = false;
        //                mObjAPIResponse.Message = mObjFuncResponse.Message;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            mObjAPIResponse.Success = true;
        //            mObjAPIResponse.Message = "Error occurred while updating vehicle function";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}
    }
}