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
    /// Government Type Operations
    /// </summary>
    [RoutePrefix("ReferenceData/GovernmentType")]
    public class GovernmentTypeController : BaseController
    {
        /// <summary>
        /// Returns a list of Government type
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("List")]
        public IHttpActionResult List()
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {

                Government_Types mObjGovernmentType = new Government_Types()
                {
                    intStatus = 1
                };

                IList<usp_GetGovernmentTypeList_Result> lstGovernmentType = new BLGovernmentType().BL_GetGovernmentTypeList(mObjGovernmentType);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstGovernmentType;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        ///// <summary>
        ///// Find Government type by id
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
        //            Government_Types mObjGovernmentType = new Government_Types()
        //            {
        //                GovernmentTypeID = id.GetValueOrDefault(),
        //                intStatus = 2
        //            };

        //            usp_GetGovernmentTypeList_Result mObjGovernmentTypeData = new BLGovernmentType().BL_GetGovernmentTypeDetails(mObjGovernmentType);

        //            if (mObjGovernmentTypeData != null)
        //            {
        //                mObjAPIResponse.Success = true;
        //                mObjAPIResponse.Result = mObjGovernmentTypeData;
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
        ///// Add New Government Type
        ///// </summary>
        ///// <param name="pObjGovernmentTypeModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Insert")]
        //public IHttpActionResult Insert(GovernmentTypeViewModel pObjGovernmentTypeModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        Government_Types mObjGovernmentType = new Government_Types()
        //        {
        //            GovernmentTypeID = 0,
        //            GovernmentTypeName = pObjGovernmentTypeModel.GovernmentTypeName.Trim(),
        //            Active = true,
        //           CreatedBy = 22,
        //            CreatedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLGovernmentType().BL_InsertUpdateGovernmentType(mObjGovernmentType);

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
        //            mObjAPIResponse.Message = "Error occurred while saving Government type";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}

        ///// <summary>
        ///// Update Government Type
        ///// </summary>
        ///// <param name="pObjGovernmentTypeModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Update")]
        //public IHttpActionResult Update(GovernmentTypeViewModel pObjGovernmentTypeModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        Government_Types mObjGovernmentType = new Government_Types()
        //        {
        //            GovernmentTypeID = pObjGovernmentTypeModel.GovernmentTypeID,
        //            GovernmentTypeName = pObjGovernmentTypeModel.GovernmentTypeName.Trim(),
        //            Active = pObjGovernmentTypeModel.Active,
        //            ModifiedBy = -1,
        //            ModifiedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLGovernmentType().BL_InsertUpdateGovernmentType(mObjGovernmentType);

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
        //            mObjAPIResponse.Message = "Error occurred while updating Government type";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}
    }
}