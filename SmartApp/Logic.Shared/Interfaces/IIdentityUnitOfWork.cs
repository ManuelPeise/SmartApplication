using Data.ContextAccessor.Interfaces;
using Data.Shared.Identity.Entities;

namespace Logic.Shared.Interfaces
{
    public interface IIdentityUnitOfWork : IDisposable
    {
        public IDbContextRepository<UserIdentity> UserRepository { get; }
        public IDbContextRepository<UserCredentials> UserCredentialsRepository { get; }
        public IDbContextRepository<UserRole> UserRoleRepository { get; }
        
    }
}
