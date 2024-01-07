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
    /// LGA Operations
    /// </summary>
    [RoutePrefix("ReferenceData/Common")]
    public class CommonController : BaseController
    {
        /// <summary>
        /// Returns a list of gender
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Gender")]
        public IHttpActionResult Gender()
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {

                IList<DropDownListResult> lstGender = new BLCommon().BL_GetGenderDropDownList();

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstGender;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        /// <summary>
        /// Returns a list of MaritalStatus
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("MaritalStatus")]
        public IHttpActionResult MaritalStatus()
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {

                IList<DropDownListResult> lstMaritalStatus = new BLCommon().BL_GetMaritalStatusDropDownList();

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstMaritalStatus;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        /// <summary>
        /// Returns a list of Nationality
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Nationality")]
        public IHttpActionResult Nationality()
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {

                IList<DropDownListResult> lstNationality = new BLCommon().BL_GetNationalityDropDownList();

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstNationality;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }


    }
}