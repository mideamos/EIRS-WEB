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
    [RoutePrefix("ReferenceData/AssessmentGroup")]
    //
    public class AssessmentGroupController : ApiController
    {
        [HttpGet]
        [Route("List")]
        public IHttpActionResult List()
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                Assessment_Group mObjAssessmentGroup = new Assessment_Group()
                {
                    intStatus = 1
                };

                IList<usp_GetAssessmentGroupList_Result> lstAssessmentGroup = new BLAssessmentGroup().BL_GetAssessmentGroupList(mObjAssessmentGroup);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstAssessmentGroup;
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
