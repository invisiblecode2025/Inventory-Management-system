using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Auth.EntityService.Dto;
using IWebToken.DTO;
using IWebToken.JWT;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebToken.Setting;

namespace WebToken.JWT
{
    internal class TokenGenerator : ITokenGenerator
    {
        #region Vars
        public AuthSettings AppSetting { get; }
        #endregion
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        private readonly IConfiguration _configuration;
        #region CTRS
        public TokenGenerator(AuthSettings appSettings, IConfiguration configuration)
        {
            AppSetting = appSettings;

            _configuration = configuration; 

            if (_jwtSecurityTokenHandler == null)
            {
                _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            }
        }
        #endregion


        public Task<string> GenerateTokenAsync(int size = 32)
        {
            var randomNumber = new byte[size];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Task.FromResult(Convert.ToBase64String(randomNumber));
        }
        public Task<AccessToken> CreateToken(List<UserClaim> userClaims, string audience, string issuer)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(AppSetting.Secret);

            //List<Claim> claims = new List<Claim>();
            if (userClaims?.Count > default(int))
            {

                var claimsx = userClaims.Select(x => new Claim(x.Name.ToString(), x.Value)).ToList();
            }
            var user = userClaims.Select(a=> a.User).FirstOrDefault();

            var validFor = TimeSpan.FromMinutes((AppSetting.TokenExpirationMinutes));

            var dateTimeNow = DateTime.UtcNow;
            var identity = GenerateClaimsIdentity(user.UserId.ToString(), user.UserName);

            var claims = new[]
            {
                 new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                 new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(dateTimeNow).ToString(), ClaimValueTypes.Integer64),
                 identity.FindFirst("id")
            };


            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(AppSetting.TokenExpirationMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                IssuedAt = DateTime.Now,
                Audience = audience,
                Issuer = issuer
            };

            return Task.FromResult(new AccessToken(_jwtSecurityTokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor)), (int)validFor.TotalSeconds));

           // return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }

        private static ClaimsIdentity GenerateClaimsIdentity(string id, string userName)
        {
            return new ClaimsIdentity(new GenericIdentity(userName, "Token"), new[]
            {
                new Claim("id", id),
            });
        }

        private static long ToUnixEpochDate(DateTime date)
         => (long)Math.Round((date.ToUniversalTime() -
                              new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                             .TotalSeconds);
        public Task<AccessToken> GenerateEncodedTokenAsync(string id, string userName)
        {
            try
            {

           
            var dateTimeNow = DateTime.UtcNow;
            var identity = GenerateClaimsIdentity(id, userName);

            var claims = new[]
            {
                 new Claim(JwtRegisteredClaimNames.Sub, userName),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                 new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(dateTimeNow).ToString(), ClaimValueTypes.Integer64),
                 identity.FindFirst("id")
            };



                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSetting.Secret));
            var validFor = TimeSpan.FromMinutes((double.Parse(_configuration["JwtIssuerOptions:Expiration"])));
                //DateTime.UtcNow.AddMinutes(AppSetting.TokenExpirationMinutes),
                // Create the JWT security token and encode it.
                var jwt = new JwtSecurityToken(
                issuer: _configuration["JwtIssuerOptions:Issuer"],
                audience: _configuration["JwtIssuerOptions:Audience"],
                expires: dateTimeNow.Add(validFor),
                claims: claims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                notBefore: DateTime.UtcNow);

            return Task.FromResult(new AccessToken(_jwtSecurityTokenHandler.WriteToken(jwt), (int)validFor.TotalSeconds));

            }
            catch (Exception ex)
            {
                var sdfdfg = ex.Message + Environment.NewLine + ex.StackTrace;  
           
            }

            return null;
        }

    }
}
