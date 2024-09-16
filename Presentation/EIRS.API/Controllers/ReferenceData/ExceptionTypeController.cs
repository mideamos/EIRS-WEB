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

namespace EIRS.API.Controllers.ReferenceData
{
    /// <summary>
    /// Exception Type Operations
    /// </summary>
    [RoutePrefix("ReferenceData/ExceptionType")]
    public class ExceptionTypeController : BaseController
    {
        /// <summary>
        /// Returns a list of exception type
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("List")]
        public IHttpActionResult List()
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {

                Exception_Type mObjExceptionType = new Exception_Type()
                {
                    intStatus = 1
                };

                IList<usp_GetExceptionTypeList_Result> lstExceptionType = new BLExceptionType().BL_GetExceptionTypeList(mObjExceptionType);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstExceptionType;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        ///// <summary>
        ///// Find exception type by id
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
        //            Exception_Type mObjExceptionType = new Exception_Type()
        //            {
        //                ExceptionTypeID = id.GetValueOrDefault(),
        //                intStatus = 2
        //            };

        //            usp_GetExceptionTypeList_Result mObjExceptionTypeData = new BLExceptionType().BL_GetExceptionTypeDetails(mObjExceptionType);

        //            if (mObjExceptionTypeData != null)
        //            {
        //                mObjAPIResponse.Success = true;
        //                mObjAPIResponse.Result = mObjExceptionTypeData;
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
        ///// Add New Exception Type
        ///// </summary>
        ///// <param name="pObjExceptionTypeModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Insert")]
        //public IHttpActionResult Insert(ExceptionTypeViewModel pObjExceptionTypeModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        Exception_Type mObjExceptionType = new Exception_Type()
        //        {
        //            ExceptionTypeID = 0,
        //            ExceptionTypeName = pObjExceptionTypeModel.ExceptionTypeName.Trim(),
        //            Active = true,
        //           CreatedBy = 22,
        //            CreatedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLExceptionType().BL_InsertUpdateExceptionType(mObjExceptionType);

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
        //            mObjAPIResponse.Message = "Error occurred while saving exception type";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}

        ///// <summary>
        ///// Update Exception Type
        ///// </summary>
        ///// <param name="pObjExceptionTypeModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Update")]
        //public IHttpActionResult Update(ExceptionTypeViewModel pObjExceptionTypeModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        Exception_Type mObjExceptionType = new Exception_Type()
        //        {
        //            ExceptionTypeID = pObjExceptionTypeModel.ExceptionTypeID,
        //            ExceptionTypeName = pObjExceptionTypeModel.ExceptionTypeName.Trim(),
        //            Active = pObjExceptionTypeModel.Active,
        //            ModifiedBy = -1,
        //            ModifiedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLExceptionType().BL_InsertUpdateExceptionType(mObjExceptionType);

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
        //            mObjAPIResponse.Message = "Error occurred while updating exception type";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}
    }
}