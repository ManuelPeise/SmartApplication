using Data.ContextAccessor.Interfaces;
using Data.ContextAccessor.Repositories;
using Data.Databases;
using Data.Shared;
using Data.Shared.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shared.Models.Identity;

namespace Data.ContextAccessor
{
    public class ApplicationUnitOfWork : IApplicationUnitOfWork
    {
        private readonly ApplicationContext _applicationContext;
        private readonly UserIdentityContext _userIdentityContext;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IOptions<SecurityData> _securityData;
        private readonly ClaimsAccessor _claimsAccessor;
        private readonly int _currentUserId;

        public ApplicationUnitOfWork(ApplicationContext applicationContext, UserIdentityContext userIdentityContext, IOptions<SecurityData> securityData, IHttpContextAccessor contextAccessor)
        {
            _applicationContext = applicationContext;
            _userIdentityContext = userIdentityContext;
            _contextAccessor = contextAccessor;
            _securityData = securityData;
            _claimsAccessor = new ClaimsAccessor();
            _currentUserId = _claimsAccessor.GetClaimsValue<int>("userId");
        }

        public int CurrentUserId => _currentUserId;
        public bool IsAuthenticated => _currentUserId != 0;
        public DbContextRepository<LogMessageEntity> LogMessageRepository => new DbContextRepository<LogMessageEntity>(_applicationContext, _contextAccessor);
        public IdentityRepository IdentityRepository => new IdentityRepository(_userIdentityContext, _contextAccessor);
        public GenericSettingsRepository GenericSettingsRepository => new GenericSettingsRepository(_applicationContext, _contextAccessor);
        public ClaimsAccessor ClaimsAccessor => new ClaimsAccessor();
        
        public IOptions<SecurityData> SecurityData => _securityData;

        public async Task<int> SaveApplicationContextChangesAsync()
        {
            var currentUser = _contextAccessor?.HttpContext.User.Identity;

            var modifiedEntries = _applicationContext.ChangeTracker.Entries()
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

            return await _applicationContext.SaveChangesAsync();
        }

        public async Task<int> SaveIdentityContextChangesAsync()
        {
            var currentUser = _contextAccessor?.HttpContext.User.Identity;

            var modifiedEntries = _userIdentityContext.ChangeTracker.Entries()
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

            return await _userIdentityContext.SaveChangesAsync();
        }

        #region dispose

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _applicationContext?.Dispose();
                    _userIdentityContext?.Dispose();
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
