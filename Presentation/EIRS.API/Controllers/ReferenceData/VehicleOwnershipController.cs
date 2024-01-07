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
    /// Vehicle Ownership Operations
    /// </summary>
    [RoutePrefix("ReferenceData/VehicleOwnership")]
    public class VehicleOwnershipController : BaseController
    {
        /// <summary>
        /// Returns a list of vehicle ownership
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

                List<Vehicle_OwnerDto> _list = new List<Vehicle_OwnerDto>();
                Vehicle_Ownership mObjVehicleOwnership = new Vehicle_Ownership()
                {
                    intStatus = 0
                };

                var lstVehicleOwnership = _db.Vehicle_Ownership.Where(x=>x.Active==true).ToList();
                foreach (var v in lstVehicleOwnership)
                {
                    Vehicle_OwnerDto dto = new Vehicle_OwnerDto();
                    dto.VehicleOwnershipID = v.VehicleOwnershipID;
                    dto.VehicleOwnershipName = v.VehicleOwnershipName;
                    _list.Add(dto);

                }
                mObjAPIResponse.Success = true;
                mObjAPIResponse.Message = "Vehicle Ownership(s) Fetched Successfully";
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

        public class Vehicle_OwnerDto
        {
            public int VehicleOwnershipID
            { get; set; }
            public string VehicleOwnershipName { get; set; }
        }
        ///// <summary>
        ///// Find vehicle ownership by id
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
        //            Vehicle_Ownership mObjVehicleOwnership = new Vehicle_Ownership()
        //            {
        //                VehicleOwnershipID = id.GetValueOrDefault(),
        //                intStatus = 2
        //            };

        //            usp_GetVehicleOwnershipList_Result mObjVehicleOwnershipData = new BLVehicleOwnership().BL_GetVehicleOwnershipDetails(mObjVehicleOwnership);

        //            if (mObjVehicleOwnershipData != null)
        //            {
        //                mObjAPIResponse.Success = true;
        //                mObjAPIResponse.Result = mObjVehicleOwnershipData;
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
        ///// Add New Vehicle ownership
        ///// </summary>
        ///// <param name="pObjVehicleOwnershipModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Insert")]
        //public IHttpActionResult Insert(VehicleOwnershipViewModel pObjVehicleOwnershipModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        Vehicle_Ownership mObjVehicleOwnership = new Vehicle_Ownership()
        //        {
        //            VehicleOwnershipID = 0,
        //            VehicleOwnershipName = pObjVehicleOwnershipModel.VehicleOwnershipName.Trim(),
        //            Active = true,
        //            CreatedBy = ClaimsIdentityExtensions.GetUserID(User.Identity),
        //            CreatedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLVehicleOwnership().BL_InsertUpdateVehicleOwnership(mObjVehicleOwnership);

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
        //            mObjAPIResponse.Message = "Error occurred while saving vehicle ownership";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}

        ///// <summary>
        ///// Update Vehicle ownership
        ///// </summary>
        ///// <param name="pObjVehicleOwnershipModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Update")]
        //public IHttpActionResult Update(VehicleOwnershipViewModel pObjVehicleOwnershipModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        Vehicle_Ownership mObjVehicleOwnership = new Vehicle_Ownership()
        //        {
        //            VehicleOwnershipID = pObjVehicleOwnershipModel.VehicleOwnershipID,
        //            VehicleOwnershipName = pObjVehicleOwnershipModel.VehicleOwnershipName.Trim(),
        //            Active = pObjVehicleOwnershipModel.Active,
        //            ModifiedBy = -1,
        //            ModifiedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLVehicleOwnership().BL_InsertUpdateVehicleOwnership(mObjVehicleOwnership);

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
        //            mObjAPIResponse.Message = "Error occurred while updating vehicle ownership";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}
    }
}