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
    [RoutePrefix("ReferenceData/RevenueStream")]
    //
    public class RevenueStreamController : ApiController
    {
        [HttpGet]
        [Route("List")]
        public IHttpActionResult List()
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                Revenue_Stream mObjRevenueStream = new Revenue_Stream()
                {
                    intStatus = 1
                };

                IList<usp_GetRevenueStreamList_Result> lstRevenueStream = new BLRevenueStream().BL_GetRevenueStreamList(mObjRevenueStream);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstRevenueStream;
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
