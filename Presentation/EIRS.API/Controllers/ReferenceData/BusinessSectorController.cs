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
    /// Business Sector Sectors
    /// </summary>
    [RoutePrefix("ReferenceData/BusinessSector")]
    public class BusinessSectorController : ApiController
    {
        /// <summary>
        /// Returns a list of business sector
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("List")]
        public IHttpActionResult List(int? businesscategoryid = 0)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {

                Business_Sector mObjBusinessSector = new Business_Sector()
                {
                    intStatus = 1,
                    BusinessCategoryID = businesscategoryid.GetValueOrDefault()
                };

                IList<usp_GetBusinessSectorList_Result> lstBusinessSector = new BLBusinessSector().BL_GetBusinessSectorList(mObjBusinessSector);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstBusinessSector;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        ///// <summary>
        ///// Find business sector by id
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
        //            Business_Sector mObjBusinessSector = new Business_Sector()
        //            {
        //                BusinessSectorID = id.GetValueOrDefault(),
        //                intStatus = 2
        //            };

        //            usp_GetBusinessSectorList_Result mObjBusinessSectorData = new BLBusinessSector().BL_GetBusinessSectorDetails(mObjBusinessSector);

        //            if (mObjBusinessSectorData != null)
        //            {
        //                mObjAPIResponse.Success = true;
        //                mObjAPIResponse.Result = mObjBusinessSectorData;
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
        ///// Add New business Sector
        ///// </summary>
        ///// <param name="pObjBusinessSectorModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Insert")]
        //public IHttpActionResult Insert(BusinessSectorViewModel pObjBusinessSectorModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        Business_Sector mObjBusinessSector = new Business_Sector()
        //        {
        //            BusinessSectorID = 0,
        //            BusinessSectorName = pObjBusinessSectorModel.BusinessSectorName.Trim(),
        //            BusinessTypeID = pObjBusinessSectorModel.BusinessTypeID,
        //            BusinessCategoryID = pObjBusinessSectorModel.BusinessCategoryID,
        //            Active = true,
        //           CreatedBy = 22,
        //            CreatedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLBusinessSector().BL_InsertUpdateBusinessSector(mObjBusinessSector);

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
        //            mObjAPIResponse.Message = "Error occurred while saving business sector";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}

        ///// <summary>
        ///// Update business Sector
        ///// </summary>
        ///// <param name="pObjBusinessSectorModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Update")]
        //public IHttpActionResult Update(BusinessSectorViewModel pObjBusinessSectorModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        Business_Sector mObjBusinessSector = new Business_Sector()
        //        {
        //            BusinessSectorID = pObjBusinessSectorModel.BusinessSectorID,
        //            BusinessSectorName = pObjBusinessSectorModel.BusinessSectorName.Trim(),
        //            BusinessTypeID = pObjBusinessSectorModel.BusinessTypeID,
        //            BusinessCategoryID = pObjBusinessSectorModel.BusinessCategoryID,
        //            Active = pObjBusinessSectorModel.Active,
        //            ModifiedBy = -1,
        //            ModifiedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLBusinessSector().BL_InsertUpdateBusinessSector(mObjBusinessSector);

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
        //            mObjAPIResponse.Message = "Error occurred while updating business sector";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}
    }
}