using EIRS.API.Models;
using EIRS.BLL;
using EIRS.BOL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EIRS.API.Controllers.Account
{
    [RoutePrefix("PayeUser")]
    public class PayeUserController : ApiController
    {
        ERASEntities _db = new ERASEntities();
        [HttpPost]
        [Route("ValidateUser")]
        public ApiResponse ValidateUser(ApiRequest request)
        {
            ApiResponse resp = new ApiResponse();
            if (ModelState.IsValid)
            {
                MST_Users mST_Users = new MST_Users();
                mST_Users = _db.MST_Users.FirstOrDefault(x => x.EmailAddress == request.Email && x.ContactNumber == request.PhoneNumber);
                if (mST_Users != null)
                {
                    resp.Status = true;
                    resp.Message = "User Exist";
                    resp.Data= mST_Users;   
                    return resp;
                }
                resp.Status = false;
                resp.Message = "User Doesnt Exist";
                return resp;
            }
            resp.Status = false;
            resp.Message = "Invalid Request Type";
            return resp;
        }
    }
}
