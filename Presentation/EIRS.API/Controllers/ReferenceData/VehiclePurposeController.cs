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
    /// Vehicle Purpose Operations
    /// </summary>
    [RoutePrefix("ReferenceData/VehiclePurpose")]
    public class VehiclePurposeController : BaseController
    {
        /// <summary>
        /// Returns a list of vehicle purpose
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

                List<Vehicle_PurposeDto> _list = new List<Vehicle_PurposeDto>();
                Vehicle_Purpose mObjVehiclePurpose = new Vehicle_Purpose()
                {
                    intStatus = 0
                };

                var lstVehiclePurpose = _db.Vehicle_Purpose.Where(o => o.Active == true).ToList();
                //var lstVehiclePurpose = new BLVehiclePurpose().BL_GetVehiclePurposeList(mObjVehiclePurpose);
                foreach(var v in lstVehiclePurpose)
                {
                    Vehicle_PurposeDto dto = new Vehicle_PurposeDto();
                    dto.VehiclePurposeID = v.VehiclePurposeID;
                    dto.VehiclePurposeName = v.VehiclePurposeName;  
                    _list.Add(dto);

                }
                mObjAPIResponse.Success = true;
                mObjAPIResponse.Message = "Vehicle Purpose(s) Fetched Successfully";
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


        public class Vehicle_PurposeDto
        {
            public int VehiclePurposeID
            { get; set; }
            public string  VehiclePurposeName { get; set; }
        }
        ///// <summary>
        ///// Find vehicle Purpose by id
        ///// </summary>
        ///
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
        //            Vehicle_Purpose mObjVehiclePurpose = new Vehicle_Purpose()
        //            {
        //                VehiclePurposeID = id.GetValueOrDefault(),
        //                intStatus = 2
        //            };

        //            usp_GetVehiclePurposeList_Result mObjVehiclePurposeData = new BLVehiclePurpose().BL_GetVehiclePurposeDetails(mObjVehiclePurpose);

        //            if (mObjVehiclePurposeData != null)
        //            {
        //                mObjAPIResponse.Success = true;
        //                mObjAPIResponse.Result = mObjVehiclePurposeData;
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
        ///// Add New Vehicle Purpose
        ///// </summary>
        ///// <param name="pObjVehiclePurposeModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Insert")]
        //public IHttpActionResult Insert(VehiclePurposeViewModel pObjVehiclePurposeModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        Vehicle_Purpose mObjVehiclePurpose = new Vehicle_Purpose()
        //        {
        //            VehiclePurposeID = 0,
        //            VehiclePurposeName = pObjVehiclePurposeModel.VehiclePurposeName.Trim(),
        //            Active = true,
        //            CreatedBy = ClaimsIdentityExtensions.GetUserID(User.Identity),
        //            CreatedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLVehiclePurpose().BL_InsertUpdateVehiclePurpose(mObjVehiclePurpose);

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
        //            mObjAPIResponse.Message = "Error occurred while saving vehicle purpose";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}

        ///// <summary>
        ///// Update Vehicle Purpose
        ///// </summary>
        ///// <param name="pObjVehiclePurposeModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Update")]
        //public IHttpActionResult Update(VehiclePurposeViewModel pObjVehiclePurposeModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        Vehicle_Purpose mObjVehiclePurpose = new Vehicle_Purpose()
        //        {
        //            VehiclePurposeID = pObjVehiclePurposeModel.VehiclePurposeID,
        //            VehiclePurposeName = pObjVehiclePurposeModel.VehiclePurposeName.Trim(),
        //            Active = pObjVehiclePurposeModel.Active,
        //            ModifiedBy = -1,
        //            ModifiedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLVehiclePurpose().BL_InsertUpdateVehiclePurpose(mObjVehiclePurpose);

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
        //            mObjAPIResponse.Message = "Error occurred while updating vehicle purpose";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}
    }
}