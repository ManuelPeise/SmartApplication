using Data.Shared.Identity.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Shared.Identity.Entities
{
    public class UserIdentity : AEntityBase, IUserIdentity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        public int CredentialsId { get; set; }
        [ForeignKey(nameof(CredentialsId))]
        public UserCredentials? UserCredentials { get; set; }

        public int RoleId { get; set; }
        [ForeignKey(nameof(RoleId))]
        public UserRole? UserRole { get; set; }

        public List<ModuleEntity> Modules { get; set; } = [];
    }
}
