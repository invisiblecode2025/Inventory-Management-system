using American.Shared.Core.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace IWebToken.JWT
{
   public interface ITokenInfo
    {
        string GetTokenData(Security.TokenInfo tokenInfo, System.Security.Claims.ClaimsPrincipal user);
    }
}
