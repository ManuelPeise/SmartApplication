﻿namespace Data.Shared.Identity.Interfaces
{
    public interface IUserIdentity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
    }
}
