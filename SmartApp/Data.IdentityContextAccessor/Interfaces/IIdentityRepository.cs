using Data.Shared.Identity.Entities;

namespace Data.ContextAccessor.Interfaces
{
    public interface IIdentityRepository: IDisposable
    {
        public RepositoryBase<UserIdentity> UserIdentityRepository { get; }
        public RepositoryBase<UserCredentials> UserCredentialsRepository { get; }
        public RepositoryBase<UserRole> UserRoleRepository { get; }
        public IClaimsAccessor ClaimsAccessor { get; }
        Task SaveChanges();
    }
}
