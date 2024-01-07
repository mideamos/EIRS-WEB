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
    /// Building Completion Operations
    /// </summary>
    [RoutePrefix("ReferenceData/BuildingCompletion")]
    public class BuildingCompletionController : BaseController
    {
        /// <summary>
        /// Returns a list of building completion
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("List")]
        public IHttpActionResult List()
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {

                Building_Completion mObjBuildingCompletion = new Building_Completion()
                {
                    intStatus = 1
                };

                IList<usp_GetBuildingCompletionList_Result> lstBuildingCompletion = new BLBuildingCompletion().BL_GetBuildingCompletionList(mObjBuildingCompletion);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstBuildingCompletion;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        ///// <summary>
        ///// Find building completion by id
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
        //            Building_Completion mObjBuildingCompletion = new Building_Completion()
        //            {
        //                BuildingCompletionID = id.GetValueOrDefault(),
        //                intStatus = 2
        //            };

        //            usp_GetBuildingCompletionList_Result mObjBuildingCompletionData = new BLBuildingCompletion().BL_GetBuildingCompletionDetails(mObjBuildingCompletion);

        //            if (mObjBuildingCompletionData != null)
        //            {
        //                mObjAPIResponse.Success = true;
        //                mObjAPIResponse.Result = mObjBuildingCompletionData;
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
        ///// Add New Building Completion
        ///// </summary>
        ///// <param name="pObjBuildingCompletionModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Insert")]
        //public IHttpActionResult Insert(BuildingCompletionViewModel pObjBuildingCompletionModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        Building_Completion mObjBuildingCompletion = new Building_Completion()
        //        {
        //            BuildingCompletionID = 0,
        //            BuildingCompletionName = pObjBuildingCompletionModel.BuildingCompletionName.Trim(),
        //            Active = true,
        //           CreatedBy = 22,
        //            CreatedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLBuildingCompletion().BL_InsertUpdateBuildingCompletion(mObjBuildingCompletion);

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
        //            mObjAPIResponse.Message = "Error occurred while saving building completion";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}

        ///// <summary>
        ///// Update Building Completion
        ///// </summary>
        ///// <param name="pObjBuildingCompletionModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Update")]
        //public IHttpActionResult Update(BuildingCompletionViewModel pObjBuildingCompletionModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        Building_Completion mObjBuildingCompletion = new Building_Completion()
        //        {
        //            BuildingCompletionID = pObjBuildingCompletionModel.BuildingCompletionID,
        //            BuildingCompletionName = pObjBuildingCompletionModel.BuildingCompletionName.Trim(),
        //            Active = pObjBuildingCompletionModel.Active,
        //            ModifiedBy = -1,
        //            ModifiedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLBuildingCompletion().BL_InsertUpdateBuildingCompletion(mObjBuildingCompletion);

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
        //            mObjAPIResponse.Message = "Error occurred while updating building completion";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}
    }
}