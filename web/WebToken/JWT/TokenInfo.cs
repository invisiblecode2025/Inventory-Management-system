
using American.Shared.Core.Enum;

using IWebToken.JWT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace WebToken
{
    internal class TokenInfo : ITokenInfo
    {
        #region CTRS
        public TokenInfo()
        {
        }
        #endregion

        public string GetTokenData(Security.TokenInfo tokenInfo, ClaimsPrincipal user)
        {
            string tData = string.Empty;

            Claim userIdClaim = user.Claims.FirstOrDefault(x => x.Type == tokenInfo.ToString());
            if (userIdClaim != null)
                tData = userIdClaim.Value;

            return tData;
        }

     
    }
}
