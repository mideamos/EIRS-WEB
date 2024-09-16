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
    /// Land Ownership Operations
    /// </summary>
    [RoutePrefix("ReferenceData/LandOwnership")]
    public class LandOwnershipController : BaseController
    {
        /// <summary>
        /// Returns a list of land ownership
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("List")]
        public IHttpActionResult List()
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {

                Land_Ownership mObjLandOwnership = new Land_Ownership()
                {
                    intStatus = 1
                };

                IList<usp_GetLandOwnershipList_Result> lstLandOwnership = new BLLandOwnership().BL_GetLandOwnershipList(mObjLandOwnership);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstLandOwnership;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        ///// <summary>
        ///// Find land ownership by id
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
        //            Land_Ownership mObjLandOwnership = new Land_Ownership()
        //            {
        //                LandOwnershipID = id.GetValueOrDefault(),
        //                intStatus = 2
        //            };

        //            usp_GetLandOwnershipList_Result mObjLandOwnershipData = new BLLandOwnership().BL_GetLandOwnershipDetails(mObjLandOwnership);

        //            if (mObjLandOwnershipData != null)
        //            {
        //                mObjAPIResponse.Success = true;
        //                mObjAPIResponse.Result = mObjLandOwnershipData;
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
        ///// Add New Land ownership
        ///// </summary>
        ///// <param name="pObjLandOwnershipModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Insert")]
        //public IHttpActionResult Insert(LandOwnershipViewModel pObjLandOwnershipModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        Land_Ownership mObjLandOwnership = new Land_Ownership()
        //        {
        //            LandOwnershipID = 0,
        //            LandOwnershipName = pObjLandOwnershipModel.LandOwnershipName.Trim(),
        //            Active = true,
        //           CreatedBy = 22,
        //            CreatedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLLandOwnership().BL_InsertUpdateLandOwnership(mObjLandOwnership);

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
        //            mObjAPIResponse.Message = "Error occurred while saving land ownership";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}

        ///// <summary>
        ///// Update Land ownership
        ///// </summary>
        ///// <param name="pObjLandOwnershipModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Update")]
        //public IHttpActionResult Update(LandOwnershipViewModel pObjLandOwnershipModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        Land_Ownership mObjLandOwnership = new Land_Ownership()
        //        {
        //            LandOwnershipID = pObjLandOwnershipModel.LandOwnershipID,
        //            LandOwnershipName = pObjLandOwnershipModel.LandOwnershipName.Trim(),
        //            Active = pObjLandOwnershipModel.Active,
        //            ModifiedBy = -1,
        //            ModifiedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLLandOwnership().BL_InsertUpdateLandOwnership(mObjLandOwnership);

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
        //            mObjAPIResponse.Message = "Error occurred while updating land ownership";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}
    }
}