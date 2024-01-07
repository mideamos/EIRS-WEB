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
    /// Business Sub Sector SubSectors
    /// </summary>
    [RoutePrefix("ReferenceData/BusinessSubSector")]
    public class BusinessSubSectorController : ApiController
    {
        /// <summary>
        /// Returns a list of business sub sector
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("List")]
        public IHttpActionResult List(int? businesssectorid = 0)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {

                Business_SubSector mObjBusinessSubSector = new Business_SubSector()
                {
                    intStatus = 1,
                    BusinessSectorID = businesssectorid.GetValueOrDefault()
                };

                IList<usp_GetBusinessSubSectorList_Result> lstBusinessSubSector = new BLBusinessSubSector().BL_GetBusinessSubSectorList(mObjBusinessSubSector);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstBusinessSubSector;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        ///// <summary>
        ///// Find business sub sector by id
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
        //            Business_SubSector mObjBusinessSubSector = new Business_SubSector()
        //            {
        //                BusinessSubSectorID = id.GetValueOrDefault(),
        //                intStatus = 2
        //            };

        //            usp_GetBusinessSubSectorList_Result mObjBusinessSubSectorData = new BLBusinessSubSector().BL_GetBusinessSubSectorDetails(mObjBusinessSubSector);

        //            if (mObjBusinessSubSectorData != null)
        //            {
        //                mObjAPIResponse.Success = true;
        //                mObjAPIResponse.Result = mObjBusinessSubSectorData;
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
        ///// Add New business sub sector
        ///// </summary>
        ///// <param name="pObjBusinessSubSectorModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Insert")]
        //public IHttpActionResult Insert(BusinessSubSectorViewModel pObjBusinessSubSectorModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        Business_SubSector mObjBusinessSubSector = new Business_SubSector()
        //        {
        //            BusinessSubSectorID = 0,
        //            BusinessSubSectorName = pObjBusinessSubSectorModel.BusinessSubSectorName.Trim(),
        //            BusinessTypeID = pObjBusinessSubSectorModel.BusinessTypeID,
        //            BusinessCategoryID = pObjBusinessSubSectorModel.BusinessCategoryID,
        //            BusinessSectorID = pObjBusinessSubSectorModel.BusinessSectorID,
        //            Active = true,
        //           CreatedBy = 22,
        //            CreatedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLBusinessSubSector().BL_InsertUpdateBusinessSubSector(mObjBusinessSubSector);

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
        //            mObjAPIResponse.Message = "Error occurred while saving business sub sector";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}

        ///// <summary>
        ///// Update business SubSector
        ///// </summary>
        ///// <param name="pObjBusinessSubSectorModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Update")]
        //public IHttpActionResult Update(BusinessSubSectorViewModel pObjBusinessSubSectorModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        Business_SubSector mObjBusinessSubSector = new Business_SubSector()
        //        {
        //            BusinessSubSectorID = pObjBusinessSubSectorModel.BusinessSubSectorID,
        //            BusinessSubSectorName = pObjBusinessSubSectorModel.BusinessSubSectorName.Trim(),
        //            BusinessTypeID = pObjBusinessSubSectorModel.BusinessTypeID,
        //            BusinessCategoryID = pObjBusinessSubSectorModel.BusinessCategoryID,
        //            BusinessSectorID = pObjBusinessSubSectorModel.BusinessSectorID,
        //            Active = pObjBusinessSubSectorModel.Active,
        //            ModifiedBy = -1,
        //            ModifiedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLBusinessSubSector().BL_InsertUpdateBusinessSubSector(mObjBusinessSubSector);

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
        //            mObjAPIResponse.Message = "Error occurred while updating business sub sector";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}
    }
}