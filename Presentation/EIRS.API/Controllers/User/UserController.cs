using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace EIRS.API.Controllers.User
{
    [RoutePrefix("User")]
    
    public class UserController : BaseController
    {
        [HttpGet]
        [Route("TaxOfficers")]
        public IHttpActionResult TaxOfficers()
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {

                IList<usp_GetUserList_Result> lstTaxOfficer = new BLUser().BL_GetUserList(new MST_Users() { intStatus = 2, UserTypeID = 2 });

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstTaxOfficer;
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
