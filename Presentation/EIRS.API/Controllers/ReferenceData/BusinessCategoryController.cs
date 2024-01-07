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
    /// Business Category Operations
    /// </summary>
    [RoutePrefix("ReferenceData/BusinessCategory")]
    public class BusinessCategoryController : ApiController
    {
        /// <summary>
        /// Returns a list of business category
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("List")]
        public IHttpActionResult List(int? businesstypeid = 0)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                Business_Category mObjBusinessCategory = new Business_Category()
                {
                    intStatus = 1,
                    BusinessTypeID = businesstypeid.GetValueOrDefault()
                };

                IList<usp_GetBusinessCategoryList_Result> lstBusinessCategory = new BLBusinessCategory().BL_GetBusinessCategoryList(mObjBusinessCategory);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstBusinessCategory;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        ///// <summary>
        ///// Find business category by id
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
        //            Business_Category mObjBusinessCategory = new Business_Category()
        //            {
        //                BusinessCategoryID = id.GetValueOrDefault(),
        //                intStatus = 2
        //            };

        //            usp_GetBusinessCategoryList_Result mObjBusinessCategoryData = new BLBusinessCategory().BL_GetBusinessCategoryDetails(mObjBusinessCategory);

        //            if (mObjBusinessCategoryData != null)
        //            {
        //                mObjAPIResponse.Success = true;
        //                mObjAPIResponse.Result = mObjBusinessCategoryData;
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
        ///// Add New business category
        ///// </summary>
        ///// <param name="pObjBusinessCategoryModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Insert")]
        //public IHttpActionResult Insert(BusinessCategoryViewModel pObjBusinessCategoryModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        Business_Category mObjBusinessCategory = new Business_Category()
        //        {
        //            BusinessCategoryID = 0,
        //            BusinessCategoryName = pObjBusinessCategoryModel.BusinessCategoryName.Trim(),
        //            BusinessTypeID = pObjBusinessCategoryModel.BusinessTypeID,
        //            Active = true,
        //           CreatedBy = 22,
        //            CreatedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLBusinessCategory().BL_InsertUpdateBusinessCategory(mObjBusinessCategory);

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
        //            mObjAPIResponse.Message = "Error occurred while saving business category";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}

        ///// <summary>
        ///// Update business category
        ///// </summary>
        ///// <param name="pObjBusinessCategoryModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Update")]
        //public IHttpActionResult Update(BusinessCategoryViewModel pObjBusinessCategoryModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        Business_Category mObjBusinessCategory = new Business_Category()
        //        {
        //            BusinessCategoryID = pObjBusinessCategoryModel.BusinessCategoryID,
        //            BusinessCategoryName = pObjBusinessCategoryModel.BusinessCategoryName.Trim(),
        //            BusinessTypeID = pObjBusinessCategoryModel.BusinessTypeID,
        //            Active = pObjBusinessCategoryModel.Active,
        //            ModifiedBy = -1,
        //            ModifiedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLBusinessCategory().BL_InsertUpdateBusinessCategory(mObjBusinessCategory);

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
        //            mObjAPIResponse.Message = "Error occurred while updating business category";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}
    }
}