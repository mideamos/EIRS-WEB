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
    [RoutePrefix("ReferenceData/ItemSubCategory")]
   // 
    public class AssessmentItemSubCategoryController : ApiController
    {
        [HttpGet]
        [Route("List")]
        public IHttpActionResult List()
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                Assessment_Item_SubCategory mObjAssessmentItemSubCategory = new Assessment_Item_SubCategory()
                {
                    //AssessmentItemCategoryID = ItemCategoryID,
                    intStatus = 1
                };

                IList<usp_GetAssessmentItemSubCategoryList_Result> lstAssessmentItemSubCategory = new BLAssessmentItemSubCategory().BL_GetAssessmentItemSubCategoryList(mObjAssessmentItemSubCategory);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstAssessmentItemSubCategory;
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
