using Data.ContextAccessor.Interfaces;
using Data.Identity;
using Data.Shared;
using Data.Shared.Identity.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Data.ContextAccessor
{
    public class IdentityRepository: IIdentityRepository
    {
        private readonly IdentityDbContext _identityDbContext;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IClaimsAccessor _claimsAccessor;
        public IdentityRepository(IdentityDbContext identityDbContext, IHttpContextAccessor contextAccessor, IClaimsAccessor claimsAccessor)
        {
            _identityDbContext = identityDbContext;
            _contextAccessor = contextAccessor;
            _claimsAccessor = claimsAccessor;
        }

        public RepositoryBase<UserIdentity> UserIdentityRepository => new RepositoryBase<UserIdentity>(_identityDbContext, _contextAccessor);
        public RepositoryBase<UserCredentials> UserCredentialsRepository => new RepositoryBase<UserCredentials>(_identityDbContext, _contextAccessor);
        public RepositoryBase<UserRole> UserRoleRepository => new RepositoryBase<UserRole>(_identityDbContext, _contextAccessor);
        public IClaimsAccessor ClaimsAccessor => _claimsAccessor;
        public async Task SaveChanges()
        {
            var currentUser = _contextAccessor?.HttpContext.User.Identity;

            var modifiedEntries = _identityDbContext.ChangeTracker.Entries()
              .Where(x => x.State == EntityState.Modified ||
              x.State == EntityState.Added);

            foreach (var entry in modifiedEntries)
            {
                if (entry != null)
                {
                    if (entry.State == EntityState.Added)
                    {
                        ((AEntityBase)entry.Entity).CreatedBy = currentUser?.Name ?? "System";
                        ((AEntityBase)entry.Entity).CreatedAt = DateTime.Now;
                        ((AEntityBase)entry.Entity).UpdatedBy = currentUser?.Name ?? "System";
                        ((AEntityBase)entry.Entity).UpdatedAt = DateTime.Now;

                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        ((AEntityBase)entry.Entity).UpdatedBy = currentUser?.Name ?? "System";
                        ((AEntityBase)entry.Entity).UpdatedAt = DateTime.Now;
                    }
                }
            }

            await _identityDbContext.SaveChangesAsync();
        }

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
