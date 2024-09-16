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
    [RoutePrefix("ReferenceData/AssessmentSubGroup")]
    //
    public class AssessmentSubGroupController : ApiController
    {
        [HttpGet]
        [Route("List")]
        public IHttpActionResult List()
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                Assessment_SubGroup mObjAssessmentSubGroup = new Assessment_SubGroup()
                {
                    //AssessmentGroupID = AssessmentGroupID,
                    intStatus = 1
                };

                IList<usp_GetAssessmentSubGroupList_Result> lstAssessmentSubGroup = new BLAssessmentSubGroup().BL_GetAssessmentSubGroupList(mObjAssessmentSubGroup);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstAssessmentSubGroup;
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
