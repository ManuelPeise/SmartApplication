using Data.Shared.Identity.Interfaces;
using Shared.Enums;

namespace Data.Shared.Identity.Entities
{
    public class UserRole : AEntityBase, IUserRole
    {
        public string ResourceKey { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public UserRoleEnum RoleType { get; set; }
    }
}
