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
    [RoutePrefix("ReferenceData/AssessmentRule")]
    //
    public class AssessmentRuleController : BaseController
    {

        [HttpGet]
        [Route("List")]
        public IHttpActionResult List()
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                Assessment_Rules mObjAssessmentRule = new Assessment_Rules()
                {
                    IntStatus = 1
                };

                BLAssessmentItem mObjBLAssessmentItem = new BLAssessmentItem();

                IList<usp_GetAssessmentRuleList_Result> lstAssessmentRule = new BLAssessmentRule().BL_GetAssessmentRuleList(mObjAssessmentRule);


                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstAssessmentRule;
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
