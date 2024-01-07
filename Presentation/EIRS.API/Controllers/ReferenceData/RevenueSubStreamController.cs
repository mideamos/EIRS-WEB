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
    [RoutePrefix("ReferenceData/RevenueSubStream")]
    //
    public class RevenueSubStreamController : ApiController
    {
        [HttpGet]
        [Route("List")]
        public IHttpActionResult List()
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                Revenue_SubStream mObjRevenueSubStream = new Revenue_SubStream()
                {
                    //RevenueStreamID = RevenueStreamID,
                    intStatus = 1
                };

                IList<usp_GetRevenueSubStreamList_Result> lstRevenueSubStream = new BLRevenueSubStream().BL_GetRevenueSubStreamList(mObjRevenueSubStream);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstRevenueSubStream;
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
