using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.API.Models;

namespace EIRS.API.Controllers
{
    
    [RoutePrefix("Profile")]
    public class ProfileController : BaseController
    {
        [HttpGet]
        [Route("PAYEPCME")]
        public IHttpActionResult PAYEPCME()
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetProfileTaxPayerData_Result> lstTaxPayerData = new BLProfile().BL_GetProfileTaxPayerData(new Profile() { ProfileDescription = "Pay As You Earn - PAYE Collections - Multiple Employees" });

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstTaxPayerData;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PAYEPCFBE")]
        public IHttpActionResult PAYEPCFBE()
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetProfileTaxPayerData_Result> lstTaxPayerData = new BLProfile().BL_GetProfileTaxPayerData(new Profile() { ProfileDescription = "Pay As You Earn - PAYE Contribution - Formal Business Employee" });

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstTaxPayerData;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("DirectAssessment")]
        public IHttpActionResult DirectAssessment()
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetProfileTaxPayerData_Result> lstTaxPayerData = new BLProfile().BL_GetProfileTaxPayerData(new Profile() { ProfileDescription = "Direct Assessment" });

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstTaxPayerData;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("DAFBSE")]
        public IHttpActionResult DAFBSE()
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetProfileTaxPayerData_Result> lstTaxPayerData = new BLProfile().BL_GetProfileTaxPayerData(new Profile() { ProfileDescription = "Direct Assessment - Formal Business - Self Employed" });

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstTaxPayerData;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("DAFBSE")]
        public IHttpActionResult DAMEBO()
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetProfileTaxPayerData_Result> lstTaxPayerData = new BLProfile().BL_GetProfileTaxPayerData(new Profile() { ProfileDescription = "Direct Assessment for Multi Employees Business Owners" });

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstTaxPayerData;
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
