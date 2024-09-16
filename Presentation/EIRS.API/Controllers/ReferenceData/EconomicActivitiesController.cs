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
    /// Economic Activities Operations
    /// </summary>
    [RoutePrefix("ReferenceData/EconomicActivities")]
    //
    public class EconomicActivitiesController : BaseController
    {
        /// <summary>
        /// Returns a list of economic activities
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("List")]
        public IHttpActionResult List(int? TaxPayerTypeID)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {

                Economic_Activities mObjEconomicActivities = new Economic_Activities()
                {
                    intStatus = 1,
                    TaxPayerTypeID = TaxPayerTypeID
                };

                IList<usp_GetEconomicActivitiesList_Result> lstEconomicActivities = new BLEconomicActivities().BL_GetEconomicActivitiesList(mObjEconomicActivities);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstEconomicActivities;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        ///// <summary>
        ///// Find economic activities by id
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
        //            Economic_Activities mObjEconomicActivities = new Economic_Activities()
        //            {
        //                EconomicActivitiesID = id.GetValueOrDefault(),
        //                intStatus = 2
        //            };

        //            usp_GetEconomicActivitiesList_Result mObjEconomicActivitiesData = new BLEconomicActivities().BL_GetEconomicActivitiesDetails(mObjEconomicActivities);

        //            if (mObjEconomicActivitiesData != null)
        //            {
        //                mObjAPIResponse.Success = true;
        //                mObjAPIResponse.Result = mObjEconomicActivitiesData;
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
        ///// Add New economic activities
        ///// </summary>
        ///// <param name="pObjEconomicActivitiesModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Insert")]
        //public IHttpActionResult Insert(EconomicActivitiesViewModel pObjEconomicActivitiesModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        Economic_Activities mObjEconomicActivities = new Economic_Activities()
        //        {
        //            EconomicActivitiesID = 0,
        //            EconomicActivitiesName = pObjEconomicActivitiesModel.EconomicActivitiesName.Trim(),
        //            TaxPayerTypeID = pObjEconomicActivitiesModel.TaxPayerTypeID,
        //            Active = true,
        //           CreatedBy = 22,
        //            CreatedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLEconomicActivities().BL_InsertUpdateEconomicActivities(mObjEconomicActivities);

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
        //            mObjAPIResponse.Message = "Error occurred while saving economic activities";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}

        ///// <summary>
        ///// Update economic activities
        ///// </summary>
        ///// <param name="pObjEconomicActivitiesModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Update")]
        //public IHttpActionResult Update(EconomicActivitiesViewModel pObjEconomicActivitiesModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        Economic_Activities mObjEconomicActivities = new Economic_Activities()
        //        {
        //            EconomicActivitiesID = pObjEconomicActivitiesModel.EconomicActivitiesID,
        //            EconomicActivitiesName = pObjEconomicActivitiesModel.EconomicActivitiesName.Trim(),
        //            TaxPayerTypeID = pObjEconomicActivitiesModel.TaxPayerTypeID,
        //            Active = pObjEconomicActivitiesModel.Active,
        //            ModifiedBy = -1,
        //            ModifiedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLEconomicActivities().BL_InsertUpdateEconomicActivities(mObjEconomicActivities);

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
        //            mObjAPIResponse.Message = "Error occurred while updating economic activities";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}
    }
}