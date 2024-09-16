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
    /// Vehicle Type Operations
    /// </summary>
    [RoutePrefix("ReferenceData/VehicleType")]
    public class VehicleTypeController : BaseController
    {
        /// <summary>
        /// Returns a list of vehicle type
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("List")]
        public IHttpActionResult List()
        {
            APIResponse mObjAPIResponse = new APIResponse();

            EIRSEntities _db = new EIRSEntities();
            try
            {
                List<Vehicle_TypeDto> _list = new List<Vehicle_TypeDto>();
                Vehicle_Types mObjVehicleType = new Vehicle_Types()
                {
                    intStatus = 0
                };

                var lstVehicleType = _db.Vehicle_Types.Where(o=>o.Active==true).ToList();
                foreach (var v in lstVehicleType)
                {
                    Vehicle_TypeDto dto = new Vehicle_TypeDto();
                    dto.VehicleTypeID = v.VehicleTypeID;
                    dto.VehicleTypeName = v.VehicleTypeName;
                    _list.Add(dto);

                }
                mObjAPIResponse.Success = true;
                mObjAPIResponse.Message = "Vehicle Type(s) Fetched Successfully";
                mObjAPIResponse.Result = _list;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = "An Error Ocurred ";
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        public class Vehicle_TypeDto
        {
            public int VehicleTypeID
            { get; set; }
            public string VehicleTypeName { get; set; }
        }
        ///// <summary>
        ///// Find vehicle type by id
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
        //            Vehicle_Types mObjVehicleType = new Vehicle_Types()
        //            {
        //                VehicleTypeID = id.GetValueOrDefault(),
        //                intStatus = 2
        //            };

        //            usp_GetVehicleTypeList_Result mObjVehicleTypeData = new BLVehicleType().BL_GetVehicleTypeDetails(mObjVehicleType);

        //            if (mObjVehicleTypeData != null)
        //            {
        //                mObjAPIResponse.Success = true;
        //                mObjAPIResponse.Result = mObjVehicleTypeData;
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
        ///// Add New Vehicle Type
        ///// </summary>
        ///// <param name="pObjVehicleTypeModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Insert")]
        //public IHttpActionResult Insert(VehicleTypeViewModel pObjVehicleTypeModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        Vehicle_Types mObjVehicleType = new Vehicle_Types()
        //        {
        //            VehicleTypeID = 0,
        //            VehicleTypeName = pObjVehicleTypeModel.VehicleTypeName.Trim(),
        //            Active = true,
        //            CreatedBy = ClaimsIdentityExtensions.GetUserID(User.Identity),
        //            CreatedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLVehicleType().BL_InsertUpdateVehicleType(mObjVehicleType);

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
        //            mObjAPIResponse.Message = "Error occurred while saving vehicle type";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}

        ///// <summary>
        ///// Update Vehicle Type
        ///// </summary>
        ///// <param name="pObjVehicleTypeModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Update")]
        //public IHttpActionResult Update(VehicleTypeViewModel pObjVehicleTypeModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        Vehicle_Types mObjVehicleType = new Vehicle_Types()
        //        {
        //            VehicleTypeID = pObjVehicleTypeModel.VehicleTypeID,
        //            VehicleTypeName = pObjVehicleTypeModel.VehicleTypeName.Trim(),
        //            Active = pObjVehicleTypeModel.Active,
        //            ModifiedBy = -1,
        //            ModifiedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLVehicleType().BL_InsertUpdateVehicleType(mObjVehicleType);

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
        //            mObjAPIResponse.Message = "Error occurred while updating vehicle type";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}
    }
}