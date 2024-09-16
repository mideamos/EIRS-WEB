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
    /// Settlement Status Operations
    /// </summary>
    [RoutePrefix("ReferenceData/SettlementStatus")]
    public class SettlementStatusController : BaseController
    {
        /// <summary>
        /// Returns a list of settlement status
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("List")]
        public IHttpActionResult List()
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {

                Settlement_Status mObjSettlementStatus = new Settlement_Status()
                {
                    intStatus = 1
                };

                IList<usp_GetSettlementStatusList_Result> lstSettlementStatus = new BLSettlementStatus().BL_GetSettlementStatusList(mObjSettlementStatus);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstSettlementStatus;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        ///// <summary>
        ///// Find settlement status by id
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
        //            Settlement_Status mObjSettlementStatus = new Settlement_Status()
        //            {
        //                SettlementStatusID = id.GetValueOrDefault(),
        //                intStatus = 2
        //            };

        //            usp_GetSettlementStatusList_Result mObjSettlementStatusData = new BLSettlementStatus().BL_GetSettlementStatusDetails(mObjSettlementStatus);

        //            if (mObjSettlementStatusData != null)
        //            {
        //                mObjAPIResponse.Success = true;
        //                mObjAPIResponse.Result = mObjSettlementStatusData;
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
        ///// <param name="pObjSettlementStatusModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Insert")]
        //public IHttpActionResult Insert(SettlementStatusViewModel pObjSettlementStatusModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        Settlement_Status mObjSettlementStatus = new Settlement_Status()
        //        {
        //            SettlementStatusID = 0,
        //            SettlementStatusName = pObjSettlementStatusModel.SettlementStatusName.Trim(),
        //            Active = true,
        //           CreatedBy = 22,
        //            CreatedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLSettlementStatus().BL_InsertUpdateSettlementStatus(mObjSettlementStatus);

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
        //            mObjAPIResponse.Message = "Error occurred while saving settlement status";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}

        ///// <summary>
        ///// Update Asset Type
        ///// </summary>
        ///// <param name="pObjSettlementStatusModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Update")]
        //public IHttpActionResult Update(SettlementStatusViewModel pObjSettlementStatusModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        Settlement_Status mObjSettlementStatus = new Settlement_Status()
        //        {
        //            SettlementStatusID = pObjSettlementStatusModel.SettlementStatusID,
        //            SettlementStatusName = pObjSettlementStatusModel.SettlementStatusName.Trim(),
        //            Active = pObjSettlementStatusModel.Active,
        //            ModifiedBy = -1,
        //            ModifiedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLSettlementStatus().BL_InsertUpdateSettlementStatus(mObjSettlementStatus);

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
        //            mObjAPIResponse.Message = "Error occurred while updating settlement status";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}
    }
}