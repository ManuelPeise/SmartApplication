﻿using Data.Shared.Identity.Interfaces;

namespace Data.Shared.Identity.Entities
{
    public class UserCredentials : AEntityBase, IUserCredentials
    {
        public string Password { get; set; } = string.Empty;
        public string? RefreshToken { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }
}
