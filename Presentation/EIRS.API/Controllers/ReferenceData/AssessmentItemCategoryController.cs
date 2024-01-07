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
    [RoutePrefix("ReferenceData/ItemCategory")]
    //
    public class AssessmentItemCategoryController : ApiController
    {
        [HttpGet]
        [Route("List")]
        public IHttpActionResult List()
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                Assessment_Item_Category mObjAssessmentItemCategory = new Assessment_Item_Category()
                {
                    intStatus = 1
                };

                IList<usp_GetAssessmentItemCategoryList_Result> lstAssessmentItemCategory = new BLAssessmentItemCategory().BL_GetAssessmentItemCategoryList(mObjAssessmentItemCategory);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstAssessmentItemCategory;
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
