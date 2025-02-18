﻿using System;
using System.Collections.Generic;
using System.Text;

namespace IWebToken.DTO
{
   public class UserClaim
    {
        public American.Shared.Core.Enum.Security.TokenInfo Name { get; set; }
        public string? Value { get; set; }

        public User User { get; set; }

    }

    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
    }
}
