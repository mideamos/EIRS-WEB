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

namespace EIRS.API.Controllers.RevenueData
{
    [RoutePrefix("RevenueData/SettlementMethod")]
    //
    public class SettlementMethodController : ApiController
    {
        [HttpGet]
        [Route("List")]
        public IHttpActionResult List()
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                Settlement_Method mObjSettlementMethod = new Settlement_Method()
                {
                    intStatus = 1
                };

                IList<usp_GetSettlementMethodList_Result> lstSettlementMethod = new BLSettlementMethod().BL_GetSettlementMethodList(mObjSettlementMethod);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstSettlementMethod;
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
