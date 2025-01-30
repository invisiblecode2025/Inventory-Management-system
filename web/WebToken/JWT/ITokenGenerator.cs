
using Auth.EntityService.Dto;
using IWebToken.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IWebToken.JWT
{
    public interface ITokenGenerator
    {
        // string CreateToken(List<UserClaim> userClaims, string audience, string issuer);
        Task<AccessToken> CreateToken(List<UserClaim> userClaims, string audience, string issuer);
        public Task<AccessToken> GenerateEncodedTokenAsync(string id, string userName);

        Task<string> GenerateTokenAsync(int size = 32);
    }
}
