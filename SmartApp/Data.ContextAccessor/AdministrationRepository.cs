using Data.AppContext;
using Data.ContextAccessor.Interfaces;
using Data.Identity;
using Data.Shared;
using Data.Shared.AccessRights;
using Data.Shared.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Data.ContextAccessor
{
    public class AdministrationRepository: IAdministrationRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IdentityDbContext _identityDbContext;
        private readonly IIdentityRepository _identityRepository;
        private readonly ILogRepository _logRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IClaimsAccessor _claimsAccessor;
        public AdministrationRepository(ApplicationDbContext applicationDbContext, IdentityDbContext identityDbContext, IIdentityRepository identityRepository, ILogRepository logRepository, IHttpContextAccessor contectAccessor, IClaimsAccessor claimsAccessor)
        {
            _applicationDbContext = applicationDbContext;
            _identityDbContext = identityDbContext;
            _identityRepository = identityRepository;
            _logRepository = logRepository;
            _contextAccessor = contectAccessor;
            _claimsAccessor = claimsAccessor;
        }

        public RepositoryBase<LogMessageEntity> LogMessageRepository =>  new RepositoryBase<LogMessageEntity>(_applicationDbContext, _contextAccessor);
        public IIdentityRepository IdentityRepository => _identityRepository;
        public ILogRepository LogRepository => _logRepository;
        public IClaimsAccessor ClaimsAccessor => _claimsAccessor;

        public async Task SaveChanges()
        {
            var currentUser = _contextAccessor?.HttpContext.User.Identity;

            var modifiedEntries = _applicationDbContext.ChangeTracker.Entries()
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

            await _applicationDbContext.SaveChangesAsync();
        }

        #region dispose

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _applicationDbContext?.Dispose();
                    _identityRepository?.Dispose();
                    _logRepository?.Dispose();
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
