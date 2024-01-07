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
    [RoutePrefix("ReferenceData/AssessmentItem")]
    //
    public class AssessmentItemController : BaseController
    {

        [HttpGet]
        [Route("List")]
        public IHttpActionResult List()
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                Assessment_Items mObjAssessmentItem = new Assessment_Items()
                {
                    intStatus = 1
                };

                BLAssessmentItem mObjBLAssessmentItem = new BLAssessmentItem();

                IList<usp_GetAssessmentItemList_Result> lstAssessmentItem = new BLAssessmentItem().BL_GetAssessmentItemList(mObjAssessmentItem);


                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstAssessmentItem;
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
