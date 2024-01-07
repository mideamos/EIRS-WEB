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
    /// Unit Occupancy Operations
    /// </summary>
    [RoutePrefix("ReferenceData/UnitOccupancy")]
    public class UnitOccupancyController : BaseController
    {
        /// <summary>
        /// Returns a list of unit occupancy
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("List")]
        public IHttpActionResult List()
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                Unit_Occupancy mObjUnitOccupancy = new Unit_Occupancy()
                {
                    intStatus = 1
                };

                IList<usp_GetUnitOccupancyList_Result> lstUnitOccupancy = new BLUnitOccupancy().BL_GetUnitOccupancyList(mObjUnitOccupancy);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstUnitOccupancy;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        ///// <summary>
        ///// Find unit occupancy by id
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
        //            Unit_Occupancy mObjUnitOccupancy = new Unit_Occupancy()
        //            {
        //                UnitOccupancyID = id.GetValueOrDefault(),
        //                intStatus = 2
        //            };

        //            usp_GetUnitOccupancyList_Result mObjUnitOccupancyData = new BLUnitOccupancy().BL_GetUnitOccupancyDetails(mObjUnitOccupancy);

        //            if (mObjUnitOccupancyData != null)
        //            {
        //                mObjAPIResponse.Success = true;
        //                mObjAPIResponse.Result = mObjUnitOccupancyData;
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
        ///// Add New Unit Occupancy
        ///// </summary>
        ///// <param name="pObjUnitOccupancyModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Insert")]
        //public IHttpActionResult Insert(UnitOccupancyViewModel pObjUnitOccupancyModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        Unit_Occupancy mObjUnitOccupancy = new Unit_Occupancy()
        //        {
        //            UnitOccupancyID = 0,
        //            UnitOccupancyName = pObjUnitOccupancyModel.UnitOccupancyName.Trim(),
        //            Active = true,
        //           CreatedBy = 22,
        //            CreatedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLUnitOccupancy().BL_InsertUpdateUnitOccupancy(mObjUnitOccupancy);

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
        //            mObjAPIResponse.Message = "Error occurred while saving unit occupancy";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}

        ///// <summary>
        ///// Update Unit Occupancy
        ///// </summary>
        ///// <param name="pObjUnitOccupancyModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Update")]
        //public IHttpActionResult Update(UnitOccupancyViewModel pObjUnitOccupancyModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        Unit_Occupancy mObjUnitOccupancy = new Unit_Occupancy()
        //        {
        //            UnitOccupancyID = pObjUnitOccupancyModel.UnitOccupancyID,
        //            UnitOccupancyName = pObjUnitOccupancyModel.UnitOccupancyName.Trim(),
        //            Active = pObjUnitOccupancyModel.Active,
        //            ModifiedBy = -1,
        //            ModifiedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLUnitOccupancy().BL_InsertUpdateUnitOccupancy(mObjUnitOccupancy);

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
        //            mObjAPIResponse.Message = "Error occurred while updating unit occupancy";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}



    }
}