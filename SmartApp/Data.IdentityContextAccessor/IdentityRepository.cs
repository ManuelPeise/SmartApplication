using Data.ContextAccessor.Interfaces;
using Data.Identity;
using Data.Shared.Identity.Entities;

namespace Data.ContextAccessor
{
    public class IdentityRepository: IIdentityRepository
    {
        private readonly IdentityDbContext _identityDbContext;

        public IdentityRepository(IdentityDbContext identityDbContext)
        {
            _identityDbContext = identityDbContext;
        }

        public RepositoryBase<UserIdentity> UserIdentityRepository => new RepositoryBase<UserIdentity>(_identityDbContext);
        public RepositoryBase<UserCredentials> UserCredentialsRepository => new RepositoryBase<UserCredentials>(_identityDbContext);
        public RepositoryBase<UserRole> UserRoleRepository => new RepositoryBase<UserRole>(_identityDbContext);

        #region dispose

        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _identityDbContext?.Dispose();
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
