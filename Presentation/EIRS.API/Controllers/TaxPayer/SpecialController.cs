using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EIRS.API.Utility;
using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;

namespace EIRS.API.Controllers
{
    /// <summary>
    /// Special Operations
    /// </summary>
    [RoutePrefix("TaxPayer/Special")]
    
    public class SpecialController : BaseController
    {
        /// <summary>
        /// Returns a list of Special
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("List")]
        public IHttpActionResult List()
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {

                Special mObjSpecial = new Special()
                {
                    intStatus = 1
                };

                IList<usp_GetSpecialList_Result> lstSpecial = new BLSpecial().BL_GetSpecialList(mObjSpecial);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstSpecial;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        /// <summary>
        /// Find Special by id
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
                    Special mObjSpecial = new Special()
                    {
                        SpecialID = id.GetValueOrDefault(),
                        intStatus = 2
                    };

                    usp_GetSpecialList_Result mObjSpecialData = new BLSpecial().BL_GetSpecialDetails(mObjSpecial);

                    if (mObjSpecialData != null)
                    {
                        mObjAPIResponse.Success = true;
                        mObjAPIResponse.Result = mObjSpecialData;
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

        /// <summary>
        /// Add New Special
        /// </summary>
        /// <param name="pObjSpecialModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Insert")]
        public IHttpActionResult Insert(SpecialViewModel pObjSpecialModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();
            String token = Request.Headers.Authorization.Parameter;
            int? userId = Utilities.GetUserId(token);
            if (!ModelState.IsValid)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            }
            else
            {
                Special mObjSpecial = new Special()
                {
                    SpecialID = 0,
                    SpecialTaxPayerName = pObjSpecialModel.SpecialName.Trim(),
                    TIN = pObjSpecialModel.TIN,
                    TaxOfficeID = pObjSpecialModel.TaxOfficeID,
                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Special,
                    ContactNumber = pObjSpecialModel.ContactNumber,
                    ContactEmail = pObjSpecialModel.ContactEmail,
                    ContactName = pObjSpecialModel.ContactName,
                    Description = pObjSpecialModel.Description,
                    NotificationMethodID = pObjSpecialModel.NotificationMethodID,
                    Active = true,
                   CreatedBy = userId.HasValue ? userId : 22,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse<Special> mObjFuncResponse = new BLSpecial().BL_InsertUpdateSpecial(mObjSpecial);

                    if (mObjFuncResponse.Success)
                    {
                        mObjAPIResponse.Success = true;
                        mObjAPIResponse.Message = mObjFuncResponse.Message;
                        mObjAPIResponse.Result = mObjFuncResponse.AdditionalData;
                    }
                    else
                    {
                        mObjAPIResponse.Success = false;
                        mObjAPIResponse.Message = mObjFuncResponse.Message;
                    }
                }
                catch (Exception ex)
                {
                    mObjAPIResponse.Success = true;
                    mObjAPIResponse.Message = "Error occurred while saving Special";
                }
            }

            return Ok(mObjAPIResponse);
        }

        /// <summary>
        /// Update Special
        /// </summary>
        /// <param name="pObjSpecialModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Update")]
        public IHttpActionResult Update(SpecialViewModel pObjSpecialModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            if (!ModelState.IsValid)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            }
            else
            {
                Special mObjSpecial = new Special()
                {
                    SpecialID = pObjSpecialModel.SpecialID,
                    SpecialTaxPayerName = pObjSpecialModel.SpecialName.Trim(),
                    TIN = pObjSpecialModel.TIN,
                    Description = pObjSpecialModel.Description,
                    TaxOfficeID = pObjSpecialModel.TaxOfficeID,
                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Special,
                    ContactNumber = pObjSpecialModel.ContactNumber,
                    ContactEmail = pObjSpecialModel.ContactEmail,
                    ContactName = pObjSpecialModel.ContactName,
                    NotificationMethodID = pObjSpecialModel.NotificationMethodID,
                    Active = pObjSpecialModel.Active,
                    ModifiedBy = -1,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse<Special> mObjFuncResponse = new BLSpecial().BL_InsertUpdateSpecial(mObjSpecial);

                    if (mObjFuncResponse.Success)
                    {
                        mObjAPIResponse.Success = true;
                        mObjAPIResponse.Message = mObjFuncResponse.Message;
                    }
                    else
                    {
                        mObjAPIResponse.Success = false;
                        mObjAPIResponse.Message = mObjFuncResponse.Message;
                    }
                }
                catch (Exception ex)
                {
                    mObjAPIResponse.Success = true;
                    mObjAPIResponse.Message = "Error occurred while updating Special";
                }
            }

            return Ok(mObjAPIResponse);
        }
    }
}
