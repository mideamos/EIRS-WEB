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
    /// Building Ownership Operations
    /// </summary>
    [RoutePrefix("ReferenceData/BuildingOwnership")]
    public class BuildingOwnershipController : BaseController
    {
        /// <summary>
        /// Returns a list of building ownership
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("List")]
        public IHttpActionResult List()
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {

                Building_Ownership mObjBuildingOwnership = new Building_Ownership()
                {
                    intStatus = 1
                };

                IList<usp_GetBuildingOwnershipList_Result> lstBuildingOwnership = new BLBuildingOwnership().BL_GetBuildingOwnershipList(mObjBuildingOwnership);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstBuildingOwnership;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        ///// <summary>
        ///// Find building ownership by id
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
        //            Building_Ownership mObjBuildingOwnership = new Building_Ownership()
        //            {
        //                BuildingOwnershipID = id.GetValueOrDefault(),
        //                intStatus = 2
        //            };

        //            usp_GetBuildingOwnershipList_Result mObjBuildingOwnershipData = new BLBuildingOwnership().BL_GetBuildingOwnershipDetails(mObjBuildingOwnership);

        //            if (mObjBuildingOwnershipData != null)
        //            {
        //                mObjAPIResponse.Success = true;
        //                mObjAPIResponse.Result = mObjBuildingOwnershipData;
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
        ///// Add New Building ownership
        ///// </summary>
        ///// <param name="pObjBuildingOwnershipModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Insert")]
        //public IHttpActionResult Insert(BuildingOwnershipViewModel pObjBuildingOwnershipModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        Building_Ownership mObjBuildingOwnership = new Building_Ownership()
        //        {
        //            BuildingOwnershipID = 0,
        //            BuildingOwnershipName = pObjBuildingOwnershipModel.BuildingOwnershipName.Trim(),
        //            Active = true,
        //           CreatedBy = 22,
        //            CreatedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLBuildingOwnership().BL_InsertUpdateBuildingOwnership(mObjBuildingOwnership);

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
        //            mObjAPIResponse.Message = "Error occurred while saving building ownership";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}

        ///// <summary>
        ///// Update Building ownership
        ///// </summary>
        ///// <param name="pObjBuildingOwnershipModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Update")]
        //public IHttpActionResult Update(BuildingOwnershipViewModel pObjBuildingOwnershipModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        Building_Ownership mObjBuildingOwnership = new Building_Ownership()
        //        {
        //            BuildingOwnershipID = pObjBuildingOwnershipModel.BuildingOwnershipID,
        //            BuildingOwnershipName = pObjBuildingOwnershipModel.BuildingOwnershipName.Trim(),
        //            Active = pObjBuildingOwnershipModel.Active,
        //            ModifiedBy = -1,
        //            ModifiedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLBuildingOwnership().BL_InsertUpdateBuildingOwnership(mObjBuildingOwnership);

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
        //            mObjAPIResponse.Message = "Error occurred while updating building ownership";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}
    }
}