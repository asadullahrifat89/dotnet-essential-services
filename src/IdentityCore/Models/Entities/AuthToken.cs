﻿namespace IdentityCore.Models.Entities
{
    public class AuthToken : EntityBase
    {
        public string Jwt { get; set; } = string.Empty;

        public DateTime ExpiresOn { get; set; }

        public string RefreshToken { get; set; } = string.Empty;
    }
}
