using EIRS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace EIRS.API
{
    public static class ClaimsIdentityExtensions
    {
        public static int GetUserID(this IIdentity pObjIdentity)
        {
            return TrynParse.parseInt(GetClaimsValue(pObjIdentity, EnumList.ClaimTypes.UserID));
        }
        
        public static string GetUserName(this IIdentity pObjIdentity)
        {
            return TrynParse.parseString(GetClaimsValue(pObjIdentity, EnumList.ClaimTypes.UserName));
        }

        public static string GetEmailAddress(this IIdentity pObjIdentity)
        {
            return TrynParse.parseString(GetClaimsValue(pObjIdentity, EnumList.ClaimTypes.EmailAddress));
        }

        private static string GetClaimsValue(IIdentity pObjIdentity, EnumList.ClaimTypes pObjClaimTypes)
        {
            if (pObjIdentity is ClaimsIdentity)
            {
                ClaimsIdentity claimsIdentity = (ClaimsIdentity)pObjIdentity;

                Claim userIdClaim = claimsIdentity.FindFirst(pObjClaimTypes.ToString());
                if (userIdClaim != null)
                {
                    return userIdClaim.Value;
                }
                else
                {
                    throw new SecurityException(string.Format("Identity is missing value for claim type {0}", pObjClaimTypes.ToString()));
                }
            }
            else
            {
                throw new SecurityException("Identity is not a valid Claims Identity");
            }
        }
    }
}