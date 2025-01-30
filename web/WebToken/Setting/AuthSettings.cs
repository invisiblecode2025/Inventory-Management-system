using System;
using System.Collections.Generic;
using System.Text;

namespace WebToken.Setting
{
   public class AuthSettings
    {
        public string Secret { get; set; }
        public int TokenExpirationMinutes { get; set; }
        public string Audiance { get; set; }
        public string Issuer { get; set; }
        public string AuthScheme { get; set; }
    }
}
