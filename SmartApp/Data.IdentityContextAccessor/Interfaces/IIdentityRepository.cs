using Data.Shared.Identity.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.ContextAccessor.Interfaces
{
    public interface IIdentityRepository: IDisposable
    {
        public RepositoryBase<UserIdentity> UserIdentityRepository { get; }
        public RepositoryBase<UserCredentials> UserCredentialsRepository { get; }
        public RepositoryBase<UserRole> UserRoleRepository { get; }
    }
}
