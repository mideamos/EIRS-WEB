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
    /// Business Operations Operations
    /// </summary>
    [RoutePrefix("ReferenceData/BusinessOperation")]
    public class BusinessOperationController : ApiController
    {
        /// <summary>
        /// Returns a list of business operation
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("List")]
        public IHttpActionResult List(int? businesstypeid = 0)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                Business_Operation mObjBusinessOperation = new Business_Operation()
                {
                    intStatus = 1,
                    BusinessTypeID = businesstypeid.GetValueOrDefault()
                };

                IList<usp_GetBusinessOperationList_Result> lstBusinessOperation = new BLBusinessOperation().BL_GetBusinessOperationList(mObjBusinessOperation);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstBusinessOperation;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        ///// <summary>
        ///// Find business operation by id
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
        //            Business_Operation mObjBusinessOperation = new Business_Operation()
        //            {
        //                BusinessOperationID = id.GetValueOrDefault(),
        //                intStatus = 2
        //            };

        //            usp_GetBusinessOperationList_Result mObjBusinessOperationData = new BLBusinessOperation().BL_GetBusinessOperationDetails(mObjBusinessOperation);

        //            if (mObjBusinessOperationData != null)
        //            {
        //                mObjAPIResponse.Success = true;
        //                mObjAPIResponse.Result = mObjBusinessOperationData;
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
        ///// Add New business operation
        ///// </summary>
        ///// <param name="pObjBusinessOperationModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Insert")]
        //public IHttpActionResult Insert(BusinessOperationViewModel pObjBusinessOperationModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        Business_Operation mObjBusinessOperation = new Business_Operation()
        //        {
        //            BusinessOperationID = 0,
        //            BusinessOperationName = pObjBusinessOperationModel.BusinessOperationName.Trim(),
        //            BusinessTypeID = pObjBusinessOperationModel.BusinessTypeID,
        //            Active = true,
        //           CreatedBy = 22,
        //            CreatedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLBusinessOperation().BL_InsertUpdateBusinessOperation(mObjBusinessOperation);

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
        //            mObjAPIResponse.Message = "Error occurred while saving business operation";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}

        ///// <summary>
        ///// Update business Operation
        ///// </summary>
        ///// <param name="pObjBusinessOperationModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Update")]
        //public IHttpActionResult Update(BusinessOperationViewModel pObjBusinessOperationModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        Business_Operation mObjBusinessOperation = new Business_Operation()
        //        {
        //            BusinessOperationID = pObjBusinessOperationModel.BusinessOperationID,
        //            BusinessOperationName = pObjBusinessOperationModel.BusinessOperationName.Trim(),
        //            BusinessTypeID = pObjBusinessOperationModel.BusinessTypeID,
        //            Active = pObjBusinessOperationModel.Active,
        //            ModifiedBy = -1,
        //            ModifiedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLBusinessOperation().BL_InsertUpdateBusinessOperation(mObjBusinessOperation);

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
        //            mObjAPIResponse.Message = "Error occurred while updating business operation";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}
    }
}