using Data.Shared.Identity.Interfaces;

namespace Data.Shared.Identity.Entities
{
    public class UserCredentials : AEntityBase, IUserCredentials
    {
        public string Salt { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }
}
