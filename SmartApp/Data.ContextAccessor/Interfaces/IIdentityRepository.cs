using Data.Shared.AccessRights;
using Data.Shared.Identity.Entities;

namespace Data.ContextAccessor.Interfaces
{
    public interface IIdentityRepository : IDisposable
    {
        public DbContextRepository<UserIdentity> UserIdentityRepository { get; }
        public DbContextRepository<UserCredentials> UserCredentialsRepository { get; }
        public DbContextRepository<UserRole> UserRoleRepository { get; }
        public DbContextRepository<AccessRightEntity> AccessRightRepository { get; }
        public DbContextRepository<UserAccessRightEntity> UserAccessRightRepository { get; }
        public IClaimsAccessor ClaimsAccessor { get; }
    }
}
