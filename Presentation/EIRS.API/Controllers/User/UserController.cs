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
        [Route("TaxOffices")]
        public IHttpActionResult TaxOffices()
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {

                IList<usp_GetTaxOfficeList_Result> lstTaxOffices = new BLTaxOffice().BL_GetTaxOfficeList(new Tax_Offices() { intStatus = 1 });

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Message = "Success";
                mObjAPIResponse.Result = lstTaxOffices;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("TaxOfficer")]
        public IHttpActionResult TaxOfficer(string EmailAddress)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {

                // IList<usp_GetUserList_Result> lstTaxOfficer = new BLUser().BL_GetUserList(new MST_Users() { intStatus = 2, UserTypeID = 2 });
                usp_GetUserList_Result lstTaxOfficer = new BLUser().BL_GetUserDetails(new MST_Users() { intStatus = 2, UserTypeID = 2, EmailAddress = EmailAddress });

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Message = "Success";
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
