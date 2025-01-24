using Data.ContextAccessor.Interfaces;
using Data.Databases;
using Data.Shared.AccessRights;
using Data.Shared.Identity.Entities;
using Microsoft.AspNetCore.Http;


namespace Data.ContextAccessor
{
    public class IdentityRepository: IIdentityRepository
    {
        private readonly UserIdentityContext _identityDbContext;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IClaimsAccessor _claimsAccessor;
        public IdentityRepository(UserIdentityContext identityDbContext, IHttpContextAccessor contextAccessor, IClaimsAccessor claimsAccessor)
        {
            _identityDbContext = identityDbContext;
            _contextAccessor = contextAccessor;
            _claimsAccessor = claimsAccessor;
        }

        public DbContextRepository<UserIdentity> UserIdentityRepository => new DbContextRepository<UserIdentity>(_identityDbContext, _contextAccessor);
        public DbContextRepository<UserCredentials> UserCredentialsRepository => new DbContextRepository<UserCredentials>(_identityDbContext, _contextAccessor);
        public DbContextRepository<UserRole> UserRoleRepository => new DbContextRepository<UserRole>(_identityDbContext, _contextAccessor);
        public DbContextRepository<AccessRightEntity> AccessRightRepository => new DbContextRepository<AccessRightEntity>(_identityDbContext, _contextAccessor);
        public DbContextRepository<UserAccessRightEntity> UserAccessRightRepository => new DbContextRepository<UserAccessRightEntity>(_identityDbContext, _contextAccessor);
        public IClaimsAccessor ClaimsAccessor => _claimsAccessor;
       

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
