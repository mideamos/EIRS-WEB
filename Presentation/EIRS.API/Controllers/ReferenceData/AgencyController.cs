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
    [RoutePrefix("ReferenceData/Agency")]
    //
    public class AgencyController : ApiController
    {
        [HttpGet]
        [Route("List")]
        public IHttpActionResult List()
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                Agency mObjAgency = new Agency()
                {
                    intStatus = 1
                };

                IList<usp_GetAgencyList_Result> lstAgency = new BLAgency().BL_GetAgencyList(mObjAgency);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstAgency;
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
