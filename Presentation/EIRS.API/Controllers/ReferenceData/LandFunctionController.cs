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
    [RoutePrefix("ReferenceData/LandFunction")]
    public class LandFunctionController : BaseController
    {
        [HttpGet]
        [Route("List")]
        public IHttpActionResult List(int? landpurposeid)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {

                Land_Function mObjLandFunction = new Land_Function()
                {
                    intStatus = 1,
                    LandPurposeID = landpurposeid
                };

                IList<usp_GetLandFunctionList_Result> lstLandFunction = new BLLandFunction().BL_GetLandFunctionList(mObjLandFunction);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstLandFunction;
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
