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
    /// Business Structure Structures
    /// </summary>
    [RoutePrefix("ReferenceData/BusinessStructure")]
    public class BusinessStructureController : ApiController
    {
        /// <summary>
        /// Returns a list of business structure
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("List")]
        public IHttpActionResult List(int? businesstypeid = 0)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                Business_Structure mObjBusinessStructure = new Business_Structure()
                {
                    intStatus = 1,
                    BusinessTypeID = businesstypeid.GetValueOrDefault()
                };

                IList<usp_GetBusinessStructureList_Result> lstBusinessStructure = new BLBusinessStructure().BL_GetBusinessStructureList(mObjBusinessStructure);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstBusinessStructure;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        ///// <summary>
        ///// Find business structure by id
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
        //            Business_Structure mObjBusinessStructure = new Business_Structure()
        //            {
        //                BusinessStructureID = id.GetValueOrDefault(),
        //                intStatus = 2
        //            };

        //            usp_GetBusinessStructureList_Result mObjBusinessStructureData = new BLBusinessStructure().BL_GetBusinessStructureDetails(mObjBusinessStructure);

        //            if (mObjBusinessStructureData != null)
        //            {
        //                mObjAPIResponse.Success = true;
        //                mObjAPIResponse.Result = mObjBusinessStructureData;
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
        ///// Add New business structure
        ///// </summary>
        ///// <param name="pObjBusinessStructureModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Insert")]
        //public IHttpActionResult Insert(BusinessStructureViewModel pObjBusinessStructureModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        Business_Structure mObjBusinessStructure = new Business_Structure()
        //        {
        //            BusinessStructureID = 0,
        //            BusinessStructureName = pObjBusinessStructureModel.BusinessStructureName.Trim(),
        //            BusinessTypeID = pObjBusinessStructureModel.BusinessTypeID,
        //            Active = true,
        //           CreatedBy = 22,
        //            CreatedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLBusinessStructure().BL_InsertUpdateBusinessStructure(mObjBusinessStructure);

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
        //            mObjAPIResponse.Message = "Error occurred while saving business structure";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}

        ///// <summary>
        ///// Update business structure
        ///// </summary>
        ///// <param name="pObjBusinessStructureModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Update")]
        //public IHttpActionResult Update(BusinessStructureViewModel pObjBusinessStructureModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        Business_Structure mObjBusinessStructure = new Business_Structure()
        //        {
        //            BusinessStructureID = pObjBusinessStructureModel.BusinessStructureID,
        //            BusinessStructureName = pObjBusinessStructureModel.BusinessStructureName.Trim(),
        //            BusinessTypeID = pObjBusinessStructureModel.BusinessTypeID,
        //            Active = pObjBusinessStructureModel.Active,
        //            ModifiedBy = -1,
        //            ModifiedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLBusinessStructure().BL_InsertUpdateBusinessStructure(mObjBusinessStructure);

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
        //            mObjAPIResponse.Message = "Error occurred while updating business structure";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}
    }
}