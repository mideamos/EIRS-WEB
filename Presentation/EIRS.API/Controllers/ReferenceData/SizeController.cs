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
    /// Size Operations
    /// </summary>
    [RoutePrefix("ReferenceData/Size")]
    public class SizeController : ApiController
    {
        /// <summary>
        /// Returns a list of Size
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("List")]
        public IHttpActionResult List()
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {

                Size mObjSize = new Size()
                {
                    intStatus = 1
                };

                IList<usp_GetSizeList_Result> lstSize = new BLSize().BL_GetSizeList(mObjSize);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstSize;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        ///// <summary>
        ///// Find Size by id
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
        //            Size mObjSize = new Size()
        //            {
        //                SizeID = id.GetValueOrDefault(),
        //                intStatus = 2
        //            };

        //            usp_GetSizeList_Result mObjSizeData = new BLSize().BL_GetSizeDetails(mObjSize);

        //            if (mObjSizeData != null)
        //            {
        //                mObjAPIResponse.Success = true;
        //                mObjAPIResponse.Result = mObjSizeData;
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
        ///// Add New Asset Type
        ///// </summary>
        ///// <param name="pObjSizeModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Insert")]
        //public IHttpActionResult Insert(SizeViewModel pObjSizeModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        Size mObjSize = new Size()
        //        {
        //            SizeID = 0,
        //            SizeName = pObjSizeModel.SizeName.Trim(),
        //            Active = true,
        //           CreatedBy = 22,
        //            CreatedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLSize().BL_InsertUpdateSize(mObjSize);

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
        //            mObjAPIResponse.Message = "Error occurred while saving Size";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}

        ///// <summary>
        ///// Update Asset Type
        ///// </summary>
        ///// <param name="pObjSizeModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Update")]
        //public IHttpActionResult Update(SizeViewModel pObjSizeModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        Size mObjSize = new Size()
        //        {
        //            SizeID = pObjSizeModel.SizeID,
        //            SizeName = pObjSizeModel.SizeName.Trim(),
        //            Active = pObjSizeModel.Active,
        //            ModifiedBy = -1,
        //            ModifiedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLSize().BL_InsertUpdateSize(mObjSize);

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
        //            mObjAPIResponse.Message = "Error occurred while updating Size";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}
    }
}