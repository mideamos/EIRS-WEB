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

namespace EIRS.API.Controllers.ReferenceData
{
    [RoutePrefix("ReferenceData/LandStreetCondition")]
    public class LandStreetConditionController : BaseController
    {
        [HttpGet]
        [Route("List")]
        public IHttpActionResult List()
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {

                Land_StreetCondition mObjLandStreetCondition = new Land_StreetCondition()
                {
                    intStatus = 1
                };

                IList<usp_GetLandStreetConditionList_Result> lstLandStreetCondition = new BLLandStreetCondition().BL_GetLandStreetConditionList(mObjLandStreetCondition);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstLandStreetCondition;
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
