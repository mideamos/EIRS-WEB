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
    /// Unit Purpose Operations
    /// </summary>
    [RoutePrefix("ReferenceData/UnitPurpose")]
    public class UnitPurposeController : BaseController
    {
        /// <summary>
        /// Returns a list of unit purpose
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("List")]
        public IHttpActionResult List()
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {

                Unit_Purpose mObjUnitPurpose = new Unit_Purpose()
                {
                    intStatus = 1
                };

                IList<usp_GetUnitPurposeList_Result> lstUnitPurpose = new BLUnitPurpose().BL_GetUnitPurposeList(mObjUnitPurpose);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstUnitPurpose;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        ///// <summary>
        ///// Find unit Purpose by id
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
        //            Unit_Purpose mObjUnitPurpose = new Unit_Purpose()
        //            {
        //                UnitPurposeID = id.GetValueOrDefault(),
        //                intStatus = 2
        //            };

        //            usp_GetUnitPurposeList_Result mObjUnitPurposeData = new BLUnitPurpose().BL_GetUnitPurposeDetails(mObjUnitPurpose);

        //            if (mObjUnitPurposeData != null)
        //            {
        //                mObjAPIResponse.Success = true;
        //                mObjAPIResponse.Result = mObjUnitPurposeData;
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
        ///// Add New Unit Purpose
        ///// </summary>
        ///// <param name="pObjUnitPurposeModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Insert")]
        //public IHttpActionResult Insert(UnitPurposeViewModel pObjUnitPurposeModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        Unit_Purpose mObjUnitPurpose = new Unit_Purpose()
        //        {
        //            UnitPurposeID = 0,
        //            UnitPurposeName = pObjUnitPurposeModel.UnitPurposeName.Trim(),
        //            Active = true,
        //           CreatedBy = 22,
        //            CreatedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLUnitPurpose().BL_InsertUpdateUnitPurpose(mObjUnitPurpose);

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
        //            mObjAPIResponse.Message = "Error occurred while saving unit purpose";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}

        ///// <summary>
        ///// Update Unit Purpose
        ///// </summary>
        ///// <param name="pObjUnitPurposeModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Update")]
        //public IHttpActionResult Update(UnitPurposeViewModel pObjUnitPurposeModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        Unit_Purpose mObjUnitPurpose = new Unit_Purpose()
        //        {
        //            UnitPurposeID = pObjUnitPurposeModel.UnitPurposeID,
        //            UnitPurposeName = pObjUnitPurposeModel.UnitPurposeName.Trim(),
        //            Active = pObjUnitPurposeModel.Active,
        //            ModifiedBy = -1,
        //            ModifiedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLUnitPurpose().BL_InsertUpdateUnitPurpose(mObjUnitPurpose);

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
        //            mObjAPIResponse.Message = "Error occurred while updating unit purpose";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}
    }
}