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
    /// Business Type Operations
    /// </summary>
    [RoutePrefix("ReferenceData/BusinessType")]
    public class BusinessTypeController : ApiController
    {
        /// <summary>
        /// Returns a list of business type
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("List")]
        public IHttpActionResult List()
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {

                Business_Types mObjBusinessType = new Business_Types()
                {
                    intStatus = 1
                };

                IList<usp_GetBusinessTypeList_Result> lstBusinessType = new BLBusinessType().BL_GetBusinessTypeList(mObjBusinessType);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstBusinessType;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        ///// <summary>
        ///// Find business type by id
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
        //            Business_Types mObjBusinessType = new Business_Types()
        //            {
        //                BusinessTypeID = id.GetValueOrDefault(),
        //                intStatus = 2
        //            };

        //            usp_GetBusinessTypeList_Result mObjBusinessTypeData = new BLBusinessType().BL_GetBusinessTypeDetails(mObjBusinessType);

        //            if (mObjBusinessTypeData != null)
        //            {
        //                mObjAPIResponse.Success = true;
        //                mObjAPIResponse.Result = mObjBusinessTypeData;
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
        ///// Add New Business Type
        ///// </summary>
        ///// <param name="pObjBusinessTypeModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Insert")]
        //public IHttpActionResult Insert(BusinessTypeViewModel pObjBusinessTypeModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        Business_Types mObjBusinessType = new Business_Types()
        //        {
        //            BusinessTypeID = 0,
        //            BusinessTypeName = pObjBusinessTypeModel.BusinessTypeName.Trim(),
        //            Active = true,
        //           CreatedBy = 22,
        //            CreatedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLBusinessType().BL_InsertUpdateBusinessType(mObjBusinessType);

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
        //            mObjAPIResponse.Message = "Error occurred while saving business type";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}

        ///// <summary>
        ///// Update Business Type
        ///// </summary>
        ///// <param name="pObjBusinessTypeModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Update")]
        //public IHttpActionResult Update(BusinessTypeViewModel pObjBusinessTypeModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        Business_Types mObjBusinessType = new Business_Types()
        //        {
        //            BusinessTypeID = pObjBusinessTypeModel.BusinessTypeID,
        //            BusinessTypeName = pObjBusinessTypeModel.BusinessTypeName.Trim(),
        //            Active = pObjBusinessTypeModel.Active,
        //            ModifiedBy = -1,
        //            ModifiedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLBusinessType().BL_InsertUpdateBusinessType(mObjBusinessType);

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
        //            mObjAPIResponse.Message = "Error occurred while updating business type";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}
    }
}