using Data.AppContext;
using Data.ContextAccessor.Interfaces;
using Data.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Data.ContextAccessor
{
    public class SettingsRepository : ISettingsRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ILogRepository _logRepository;
        private readonly IClaimsAccessor _claimsAccessor;
        public SettingsRepository(ApplicationDbContext applicationDbContext, IHttpContextAccessor contextAccessor, ILogRepository logRepository, IClaimsAccessor claimsAccessor)
        {
            _applicationDbContext = applicationDbContext;
            _contextAccessor = contextAccessor;
            _logRepository = logRepository;
            _claimsAccessor = claimsAccessor;
        }

        public RepositoryBase<EmailAccountEntity> EmailAccountRepository => new RepositoryBase<EmailAccountEntity>(_applicationDbContext, _contextAccessor);
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
