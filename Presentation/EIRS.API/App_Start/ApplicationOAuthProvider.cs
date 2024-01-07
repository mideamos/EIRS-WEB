using EIRS.BLL;
using EIRS.BOL;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace EIRS.API.App_Start
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null) {
                throw new ArgumentException("publicClientId");
            }

            _publicClientId = publicClientId;

        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var requestObj = new MST_Users() { UserName = context.UserName, Password = context.Password, UserTypeID = 3 };
            var response = new BLUser().BL_CheckUserLoginDetails(requestObj);

            if (!response.Success) {
                context.SetError("invalid_grant", "Username or Password is incorrect");
                return;
            }

            //var response1 = new BLUser().BL_UpdateLastUserDetails(requestObj);

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("UserID", response.AdditionalData.UserID.ToString()));
            identity.AddClaim(new Claim("UserName", response.AdditionalData.UserName));
            identity.AddClaim(new Claim("EmailAddress", response.AdditionalData.EmailAddress));

            //AuthenticationProperties properties = CreateProperties(user);
            //AuthenticationTicket ticket = new AuthenticationTicket(identity, properties);
            context.Validated(identity);
            //context.Validated(ticket);

        }
    }
}