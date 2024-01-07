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
    /// Building Purpose Operations
    /// </summary>
    [RoutePrefix("ReferenceData/BuildingPurpose")]
    public class BuildingPurposeController : BaseController
    {
        /// <summary>
        /// Returns a list of building purpose
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("List")]
        public IHttpActionResult List()
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {

                Building_Purpose mObjBuildingPurpose = new Building_Purpose()
                {
                    intStatus = 1
                };

                IList<usp_GetBuildingPurposeList_Result> lstBuildingPurpose = new BLBuildingPurpose().BL_GetBuildingPurposeList(mObjBuildingPurpose);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstBuildingPurpose;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        ///// <summary>
        ///// Find building Purpose by id
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
        //            Building_Purpose mObjBuildingPurpose = new Building_Purpose()
        //            {
        //                BuildingPurposeID = id.GetValueOrDefault(),
        //                intStatus = 2
        //            };

        //            usp_GetBuildingPurposeList_Result mObjBuildingPurposeData = new BLBuildingPurpose().BL_GetBuildingPurposeDetails(mObjBuildingPurpose);

        //            if (mObjBuildingPurposeData != null)
        //            {
        //                mObjAPIResponse.Success = true;
        //                mObjAPIResponse.Result = mObjBuildingPurposeData;
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
        ///// Add New Building Purpose
        ///// </summary>
        ///// <param name="pObjBuildingPurposeModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Insert")]
        //public IHttpActionResult Insert(BuildingPurposeViewModel pObjBuildingPurposeModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        Building_Purpose mObjBuildingPurpose = new Building_Purpose()
        //        {
        //            BuildingPurposeID = 0,
        //            BuildingPurposeName = pObjBuildingPurposeModel.BuildingPurposeName.Trim(),
        //            Active = true,
        //           CreatedBy = 22,
        //            CreatedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLBuildingPurpose().BL_InsertUpdateBuildingPurpose(mObjBuildingPurpose);

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
        //            mObjAPIResponse.Message = "Error occurred while saving building purpose";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}

        ///// <summary>
        ///// Update Building Purpose
        ///// </summary>
        ///// <param name="pObjBuildingPurposeModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Update")]
        //public IHttpActionResult Update(BuildingPurposeViewModel pObjBuildingPurposeModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        Building_Purpose mObjBuildingPurpose = new Building_Purpose()
        //        {
        //            BuildingPurposeID = pObjBuildingPurposeModel.BuildingPurposeID,
        //            BuildingPurposeName = pObjBuildingPurposeModel.BuildingPurposeName.Trim(),
        //            Active = pObjBuildingPurposeModel.Active,
        //            ModifiedBy = -1,
        //            ModifiedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLBuildingPurpose().BL_InsertUpdateBuildingPurpose(mObjBuildingPurpose);

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
        //            mObjAPIResponse.Message = "Error occurred while updating building purpose";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}
    }
}