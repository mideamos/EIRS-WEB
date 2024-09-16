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
    [RoutePrefix("ReferenceData/LandDevelopment")]
    public class LandDevelopmentController : BaseController
    {
        [HttpGet]
        [Route("List")]
        public IHttpActionResult List()
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {

                Land_Development mObjLandDevelopment = new Land_Development()
                {
                    intStatus = 1
                };

                IList<usp_GetLandDevelopmentList_Result> lstLandDevelopment = new BLLandDevelopment().BL_GetLandDevelopmentList(mObjLandDevelopment);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstLandDevelopment;
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
