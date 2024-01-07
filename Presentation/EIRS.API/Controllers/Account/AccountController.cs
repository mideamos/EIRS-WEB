//using EIRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EIRS.API.Utility;
using EIRS.BLL;
using EIRS.BOL;
using System.Security.Claims;
using EIRS.API.Models;
using System.Web;

namespace EIRS.API.Controllers.Account
{
    [RoutePrefix("Account")]

    public class AccountController : BaseController
    {
        [HttpPost]
        [Route("Login")]
        public HttpResponseMessage Login([FromBody]TokenRequest request)
        {
           if (ModelState.IsValid) {
                var user = new MST_Users();
                var token = new TokenResponse();
                var userRepository = new BLUser();
                var password = Utilities.Base64Encode(request.Password);

                var requestObj = new MST_Users() { UserName = request.UserName, Password = password, UserTypeID = 3 };
                var loginResponse = userRepository.BL_CheckUserLoginDetails(requestObj);

                if (loginResponse.Success) {
                    user = loginResponse.AdditionalData;
                    token = Utilities.GetAccessToken(request.UserName, password);

                    if (!string.IsNullOrEmpty(token.AccessToken)) {
                        // insert token into DB
                       
                        var requestObj1 = new MST_UserToken() { CreatedBy = user.UserID, CreatedDate = DateTime.Now, UserID = user.UserID, Token = token.AccessToken, TokenExpiresDate = DateTime.Now, TokenIssuedDate = DateTime.Now };
                        userRepository.BL_InsertLoginToken(requestObj1);

                        // update last login date
                        var response2 = new BLUser().BL_UpdateLastUserDetails(user);
                       // return Request.CreateResponse(HttpStatusCode.OK, token, Configuration.Formatters.JsonFormatter);
                        return Request.CreateResponse(HttpStatusCode.Created, token);
                    }
                    else {
                        var response = new { error = "invalid_credentials", error_description = token.Error };
                        return Request.CreateResponse(HttpStatusCode.BadRequest, response);
                    }
                }
                else {
                    var response = new { error = "invalid_credentials", error_description = token.Error };
                    return Request.CreateResponse(HttpStatusCode.BadRequest, response);
                }
               
            }

            var response1 = new { error = "invalid_request", error_description = "Invaid request object" };
            return Request.CreateResponse(HttpStatusCode.BadRequest, response1);
        }   
        [HttpPost]
        [Route("LoginII")]
        public HttpResponseMessage LoginII(TokenRequest request)
        {
           if (ModelState.IsValid) {
                var user = new MST_Users();
                var token = new TokenResponse();
                var userRepository = new BLUser();
                var password = Utilities.Base64Encode(request.Password);

                var requestObj = new MST_Users() { UserName = request.UserName, Password = password, UserTypeID = 3 };
                var loginResponse = userRepository.BL_CheckUserLoginDetails(requestObj);

                if (loginResponse.Success) {
                    user = loginResponse.AdditionalData;
                    token = Utilities.GetAccessToken(request.UserName, password);

                    if (!string.IsNullOrEmpty(token.AccessToken)) {
                        // insert token into DB
                       
                        var requestObj1 = new MST_UserToken() { CreatedBy = user.UserID, CreatedDate = DateTime.Now, UserID = user.UserID, Token = token.AccessToken, TokenExpiresDate = DateTime.Now, TokenIssuedDate = DateTime.Now };
                        userRepository.BL_InsertLoginToken(requestObj1);

                        // update last login date
                        var response2 = new BLUser().BL_UpdateLastUserDetails(user);
                       // return Request.CreateResponse(HttpStatusCode.OK, token, Configuration.Formatters.JsonFormatter);
                        return Request.CreateResponse(HttpStatusCode.Created, token);
                    }
                    else {
                        var response = new { error = "invalid_credentials", error_description = token.Error };
                        return Request.CreateResponse(HttpStatusCode.BadRequest, response);
                    }
                }
                else {
                    var response = new { error = "invalid_credentials", error_description = token.Error };
                    return Request.CreateResponse(HttpStatusCode.BadRequest, response);
                }
               
            }

            var response1 = new { error = "invalid_request", error_description = "Invaid request object" };
            return Request.CreateResponse(HttpStatusCode.BadRequest, response1);
        }
    }
}