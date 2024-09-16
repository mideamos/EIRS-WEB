using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EIRS.API.Models;
using EIRS.API.Utility;
using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;

namespace EIRS.API.Controllers
{
    /// <summary>
    /// Government Operations
    /// </summary>
    [RoutePrefix("TaxPayer/Government")]
    
    public class GovernmentController : BaseController
    {
        /// <summary>
        /// Returns a list of Government
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("List")]
        public IHttpActionResult List()
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {

                Government mObjGovernment = new Government()
                {
                    intStatus = 1
                };

                IList<usp_GetGovernmentList_Result> lstGovernment = new BLGovernment().BL_GetGovernmentList(mObjGovernment);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstGovernment;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        /// <summary>
        /// Find Government by id
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
                    Government mObjGovernment = new Government()
                    {
                        GovernmentID = id.GetValueOrDefault(),
                        intStatus = 2
                    };

                    usp_GetGovernmentList_Result mObjGovernmentData = new BLGovernment().BL_GetGovernmentDetails(mObjGovernment);

                    if (mObjGovernmentData != null)
                    {
                        mObjAPIResponse.Success = true;
                        mObjAPIResponse.Result = mObjGovernmentData;
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
        /// Add New Government
        /// </summary>
        /// <param name="pObjGovernmentModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Insert")]
        public IHttpActionResult Insert(GovernmentViewModel pObjGovernmentModel)
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
                Government mObjGovernment = new Government()
                {
                    GovernmentID = 0,
                    GovernmentName = pObjGovernmentModel.GovernmentName.Trim(),
                    TIN = pObjGovernmentModel.TIN,
                    GovernmentTypeID = pObjGovernmentModel.GovernmentTypeID,
                    TaxOfficeID = pObjGovernmentModel.TaxOfficeID,
                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Government,
                    ContactNumber = pObjGovernmentModel.ContactNumber,
                    ContactEmail = pObjGovernmentModel.ContactEmail,
                    ContactName = pObjGovernmentModel.ContactName,
                    NotificationMethodID = pObjGovernmentModel.NotificationMethodID,
                    ContactAddress = pObjGovernmentModel.ContactAddress,
                    Active = true,
                   CreatedBy = userId.HasValue ? userId : 22,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse<Government> mObjFuncResponse = new BLGovernment().BL_InsertUpdateGovernment(mObjGovernment);

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
                    mObjAPIResponse.Message = "Error occurred while saving Government";
                }
            }

            return Ok(mObjAPIResponse);
        }

        /// <summary>
        /// Update Government
        /// </summary>
        /// <param name="pObjGovernmentModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Update")]
        public IHttpActionResult Update(GovernmentViewModel pObjGovernmentModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            if (!ModelState.IsValid)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            }
            else
            {
                Government mObjGovernment = new Government()
                {
                    GovernmentID = pObjGovernmentModel.GovernmentID,
                    GovernmentName = pObjGovernmentModel.GovernmentName.Trim(),
                    TIN = pObjGovernmentModel.TIN,
                    GovernmentTypeID = pObjGovernmentModel.GovernmentTypeID,
                    TaxOfficeID = pObjGovernmentModel.TaxOfficeID,
                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Government,
                    ContactNumber = pObjGovernmentModel.ContactNumber,
                    ContactEmail = pObjGovernmentModel.ContactEmail,
                    ContactName = pObjGovernmentModel.ContactName,
                    NotificationMethodID = pObjGovernmentModel.NotificationMethodID,
                    Active = pObjGovernmentModel.Active,
                    ModifiedBy = -1,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse<Government> mObjFuncResponse = new BLGovernment().BL_InsertUpdateGovernment(mObjGovernment);

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
                    mObjAPIResponse.Message = "Error occurred while updating Government";
                }
            }

            return Ok(mObjAPIResponse);
        }

        [HttpPost]
        [Route("SavePassword")]
        public IHttpActionResult SavePassword(TaxPayerPasswordViewModel pObjTaxPayerPasswordModel)
        {

            APIResponse mObjAPIResponse = new APIResponse();

            if (!ModelState.IsValid)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            }
            else
            {
                Government mObjGovernment = new Government()
                {
                    GovernmentID = pObjTaxPayerPasswordModel.TaxPayerID,
                    Password = EncryptDecrypt.Encrypt(pObjTaxPayerPasswordModel.Password),
                    RegisterationStatusID = (int)EnumList.RegisterationStatus.Completed,
                    RegisterationDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjFuncResponse = new BLGovernment().BL_UpdatePassword(mObjGovernment);

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
                    mObjAPIResponse.Message = "Error occurred while updating password";
                }
            }

            return Ok(mObjAPIResponse);
        }
    }
}
