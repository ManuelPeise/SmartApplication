using Data.ContextAccessor.Interfaces;
using Data.Shared.Identity.Entities;

namespace Logic.Shared.Interfaces
{
    public interface IIdentityUnitOfWork : IDisposable
    {
        public IRepositoryBase<UserIdentity> UserRepository { get; }
        public IRepositoryBase<UserCredentials> UserCredentialsRepository { get; }
        public IRepositoryBase<UserRole> UserRoleRepository { get; }
        
    }
}
