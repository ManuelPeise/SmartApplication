using Data.Shared.AccessRights;
using Data.Shared.Identity.Entities;

namespace Data.ContextAccessor.Interfaces
{
    public interface IIdentityRepository : IDisposable
    {
        public RepositoryBase<UserIdentity> UserIdentityRepository { get; }
        public RepositoryBase<UserCredentials> UserCredentialsRepository { get; }
        public RepositoryBase<UserRole> UserRoleRepository { get; }
        public RepositoryBase<AccessRightEntity> AccessRightRepository { get; }
        public RepositoryBase<UserAccessRightEntity> UserAccessRightRepository { get; }
        public IClaimsAccessor ClaimsAccessor { get; }
        Task SaveChanges();
    }
}
