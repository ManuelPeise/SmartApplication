using Shared.Enums;

namespace Data.Shared.Identity.Interfaces
{
    public interface IUserRole
    {
        public string ResourceKey { get; set; }
        public string RoleName { get; set; }
        public UserRoleEnum RoleType { get; set; }
    }
}
