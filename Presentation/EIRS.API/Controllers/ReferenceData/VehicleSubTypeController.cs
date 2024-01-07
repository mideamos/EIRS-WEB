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
    /// Vehicle Sub Type Operations
    /// </summary>
    [RoutePrefix("ReferenceData/VehicleSubType")]
    public class VehicleSubTypeController : BaseController
    {
        /// <summary>
        /// Returns a list of vehicle sub type
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("List")]
        public IHttpActionResult List(int? vehicletypeid = 0)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {

                Vehicle_SubTypes mObjVehicleSubType = new Vehicle_SubTypes()
                {
                    intStatus = 1,
                    VehicleTypeID = vehicletypeid.GetValueOrDefault()
                };

                IList<usp_GetVehicleSubTypeList_Result> lstVehicleSubType = new BLVehicleSubType().BL_GetVehicleSubTypeList(mObjVehicleSubType);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstVehicleSubType;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        ///// <summary>
        ///// Find vehicle sub type by id
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
        //            Vehicle_SubTypes mObjVehicleSubType = new Vehicle_SubTypes()
        //            {
        //                VehicleSubTypeID = id.GetValueOrDefault(),
        //                intStatus = 2
        //            };

        //            usp_GetVehicleSubTypeList_Result mObjVehicleSubTypeData = new BLVehicleSubType().BL_GetVehicleSubTypeDetails(mObjVehicleSubType);

        //            if (mObjVehicleSubTypeData != null)
        //            {
        //                mObjAPIResponse.Success = true;
        //                mObjAPIResponse.Result = mObjVehicleSubTypeData;
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
        ///// Add New VehicleSub Type
        ///// </summary>
        ///// <param name="pObjVehicleSubTypeModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Insert")]
        //public IHttpActionResult Insert(VehicleSubTypeViewModel pObjVehicleSubTypeModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        Vehicle_SubTypes mObjVehicleSubType = new Vehicle_SubTypes()
        //        {
        //            VehicleSubTypeID = 0,
        //            VehicleSubTypeName = pObjVehicleSubTypeModel.VehicleSubTypeName.Trim(),
        //            VehicleTypeID = pObjVehicleSubTypeModel.VehicleTypeID,
        //            Active = true,
        //           CreatedBy = 22,
        //            CreatedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLVehicleSubType().BL_InsertUpdateVehicleSubType(mObjVehicleSubType);

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
        //            mObjAPIResponse.Message = "Error occurred while saving vehicle sub type";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}

        ///// <summary>
        ///// Update VehicleSub Type
        ///// </summary>
        ///// <param name="pObjVehicleSubTypeModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Update")]
        //public IHttpActionResult Update(VehicleSubTypeViewModel pObjVehicleSubTypeModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        Vehicle_SubTypes mObjVehicleSubType = new Vehicle_SubTypes()
        //        {
        //            VehicleSubTypeID = pObjVehicleSubTypeModel.VehicleSubTypeID,
        //            VehicleSubTypeName = pObjVehicleSubTypeModel.VehicleSubTypeName.Trim(),
        //            VehicleTypeID = pObjVehicleSubTypeModel.VehicleTypeID,
        //            Active = pObjVehicleSubTypeModel.Active,
        //            ModifiedBy = -1,
        //            ModifiedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLVehicleSubType().BL_InsertUpdateVehicleSubType(mObjVehicleSubType);

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
        //            mObjAPIResponse.Message = "Error occurred while updating vehicle sub type";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}
    }
}