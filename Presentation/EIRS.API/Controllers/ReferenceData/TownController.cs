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
    /// Town Operations
    /// </summary>
    [RoutePrefix("ReferenceData/Town")]
    public class TownController : BaseController
    {
        /// <summary>
        /// Returns a list of town
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("List")]
        public IHttpActionResult List()
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {

                Town mObjTown = new Town()
                {
                    intStatus = 1
                };

                IList<usp_GetTownList_Result> lstTown = new BLTown().BL_GetTownList(mObjTown);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstTown;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        /// <summary>
        /// Find town by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Details/{id}")]
        public IHttpActionResult Details(int? id)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                if (id.GetValueOrDefault() > 0)
                {
                    Town mObjTown = new Town()
                    {
                        TownID = id.GetValueOrDefault(),
                        intStatus = 2
                    };

                    usp_GetTownList_Result mObjTownData = new BLTown().BL_GetTownDetails(mObjTown);

                    if (mObjTownData != null)
                    {
                        mObjAPIResponse.Success = true;
                        mObjAPIResponse.Result = mObjTownData;
                    }
                    else
                    {
                        mObjAPIResponse.Success = false;
                        mObjAPIResponse.Result = "Invalid Request";
                    }
                }
                else
                {
                    mObjAPIResponse.Success = false;
                    mObjAPIResponse.Result = "Invalid Request";
                }


            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        ///// <summary>
        ///// Add New Asset Type
        ///// </summary>
        ///// <param name="pObjTownModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Insert")]
        //public IHttpActionResult Insert(TownViewModel pObjTownModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        Town mObjTown = new Town()
        //        {
        //            TownID = 0,
        //            TownName = pObjTownModel.TownName.Trim(),
        //            LGAID = pObjTownModel.LGAID,
        //            Active = true,
        //           CreatedBy = 22,
        //            CreatedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLTown().BL_InsertUpdateTown(mObjTown);

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
        //            mObjAPIResponse.Message = "Error occurred while saving town";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}

        ///// <summary>
        ///// Update Asset Type
        ///// </summary>
        ///// <param name="pObjTownModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Update")]
        //public IHttpActionResult Update(TownViewModel pObjTownModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        Town mObjTown = new Town()
        //        {
        //            TownID = pObjTownModel.TownID,
        //            TownName = pObjTownModel.TownName.Trim(),
        //            LGAID = pObjTownModel.LGAID,
        //            Active = pObjTownModel.Active,
        //            ModifiedBy = -1,
        //            ModifiedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLTown().BL_InsertUpdateTown(mObjTown);

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
        //            mObjAPIResponse.Message = "Error occurred while updating town";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}
    }
}