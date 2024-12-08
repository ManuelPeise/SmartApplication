using Data.ContextAccessor;
using Data.ContextAccessor.Interfaces;
using Data.Identity;
using Data.Shared.Identity.Entities;
using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Logic.Shared
{
    public class IdentityUnitOfWork : AUnitOfWorkBase, IIdentityUnitOfWork
    {
        private readonly IdentityDbContext _identityContext;
        private bool disposedValue;

        private readonly IRepositoryBase<UserIdentity> _userRepository;
        public IRepositoryBase<UserIdentity> UserRepository => _userRepository ?? new RepositoryBase<UserIdentity>(_identityContext);

        private readonly IRepositoryBase<UserCredentials> _userCredentialRepository;
        public IRepositoryBase<UserCredentials> UserCredentialsRepository => _userCredentialRepository ?? new RepositoryBase<UserCredentials>(_identityContext);

        private readonly IRepositoryBase<UserRole> _userRoleRepository;
        public IRepositoryBase<UserRole> UserRoleRepository => _userRoleRepository ?? new RepositoryBase<UserRole>(_identityContext);

        public IdentityUnitOfWork(IdentityDbContext identityContext, IHttpContextAccessor httpContextAccessor) : base(identityContext, httpContextAccessor)
        {
            _identityContext = identityContext;
            _userRepository = new RepositoryBase<UserIdentity>(_identityContext);
            _userCredentialRepository = new RepositoryBase<UserCredentials>(_identityContext);
            _userRoleRepository = new RepositoryBase<UserRole>(_identityContext);
        }

        #region dispose
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _identityContext.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in der Methode "Dispose(bool disposing)" ein.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
