using IWebToken.JWT;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using WebToken.Setting;

namespace WebToken.Bootstrapper
{
    public static class Bootstrapper
    {
        public static void ResolveTokenGenerator(IServiceCollection services, IConfiguration configuration)
        {
            AuthSettings authAppSettings = new AuthSettings();
            configuration.Bind("AuthSettings", authAppSettings);
            services.AddSingleton(authAppSettings);

            services.AddScoped<ITokenGenerator, JWT.TokenGenerator>();
        }

        public static void ResolveTokenInfo(IServiceCollection services, IConfiguration configuration, bool validateAudience = true)
        {
            AuthSettings authAppSettings = new AuthSettings();
            configuration.Bind("AuthSettings", authAppSettings);
            services.AddSingleton(authAppSettings);

            // configure jwt authenticationAuthSetting            
            byte[] key = Encoding.ASCII.GetBytes(authAppSettings.Secret);
            //services.AddAuthentication(x =>
            //{
            //    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //.AddJwtBearer(x =>
            //{
            //    x.RequireHttpsMetadata = false;
            //    x.SaveToken = true;
            //    x.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(key),
            //        ValidateIssuer = true,
            //        ValidateAudience = validateAudience,
            //        ValidAudience = authAppSettings.Audiance,
            //        ValidIssuer = authAppSettings.Issuer
            //    };
            //});

            services.AddScoped<ITokenInfo, TokenInfo>();
        }

    }
}
