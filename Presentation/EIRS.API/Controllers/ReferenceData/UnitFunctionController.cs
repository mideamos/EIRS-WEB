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
    /// Unit Function Operations
    /// </summary>
    [RoutePrefix("ReferenceData/UnitFunction")]
    public class UnitFunctionController : BaseController
    {
        /// <summary>
        /// Returns a list of unit function
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("List")]
        public IHttpActionResult List(int? unitpurposeid)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                Unit_Function mObjUnitFunction = new Unit_Function()
                {
                    UnitPurposeID = unitpurposeid,
                    intStatus = 1
                };

                IList<usp_GetUnitFunctionList_Result> lstUnitFunction = new BLUnitFunction().BL_GetUnitFunctionList(mObjUnitFunction);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstUnitFunction;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        ///// <summary>
        ///// Find unit function by id
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
        //            Unit_Function mObjUnitFunction = new Unit_Function()
        //            {
        //                UnitFunctionID = id.GetValueOrDefault(),
        //                intStatus = 2
        //            };

        //            usp_GetUnitFunctionList_Result mObjUnitFunctionData = new BLUnitFunction().BL_GetUnitFunctionDetails(mObjUnitFunction);

        //            if (mObjUnitFunctionData != null)
        //            {
        //                mObjAPIResponse.Success = true;
        //                mObjAPIResponse.Result = mObjUnitFunctionData;
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
        ///// <param name="pObjUnitFunctionModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Insert")]
        //public IHttpActionResult Insert(UnitFunctionViewModel pObjUnitFunctionModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        Unit_Function mObjUnitFunction = new Unit_Function()
        //        {
        //            UnitFunctionID = 0,
        //            UnitFunctionName = pObjUnitFunctionModel.UnitFunctionName.Trim(),
        //            UnitPurposeID = pObjUnitFunctionModel.UnitPurposeID,
        //            Active = true,
        //           CreatedBy = 22,
        //            CreatedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLUnitFunction().BL_InsertUpdateUnitFunction(mObjUnitFunction);

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
        //            mObjAPIResponse.Message = "Error occurred while saving unit function";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}

        ///// <summary>
        ///// Update Asset Type
        ///// </summary>
        ///// <param name="pObjUnitFunctionModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Update")]
        //public IHttpActionResult Update(UnitFunctionViewModel pObjUnitFunctionModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        Unit_Function mObjUnitFunction = new Unit_Function()
        //        {
        //            UnitFunctionID = pObjUnitFunctionModel.UnitFunctionID,
        //            UnitFunctionName = pObjUnitFunctionModel.UnitFunctionName.Trim(),
        //            UnitPurposeID = pObjUnitFunctionModel.UnitPurposeID,
        //            Active = pObjUnitFunctionModel.Active,
        //            ModifiedBy = -1,
        //            ModifiedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLUnitFunction().BL_InsertUpdateUnitFunction(mObjUnitFunction);

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
        //            mObjAPIResponse.Message = "Error occurred while updating unit function";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}
    }
}