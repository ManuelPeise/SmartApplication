using Data.ContextAccessor.Interfaces;
using Data.Databases;
using Data.Shared.Logging;
using Data.Shared.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Shared.Models.Identity;

namespace Data.ContextAccessor.Repositories
{
    public class SettingsRepository : ISettingsRepository
    {
        private readonly ApplicationContext _applicationDbContext;
        private readonly IOptions<SecurityData> _securityData;
        private readonly IHttpContextAccessor _contextAccessor;

        public SettingsRepository(ApplicationContext applicationDbContext, IOptions<SecurityData> securityData, IHttpContextAccessor contextAccessor)
        {
            _applicationDbContext = applicationDbContext;
            _securityData = securityData;
            _contextAccessor = contextAccessor;

        }

        public DbContextRepository<EmailAccountEntity> EmailAccountRepository => new DbContextRepository<EmailAccountEntity>(_applicationDbContext, _contextAccessor);
        public DbContextRepository<EmailCleanerSettingsEntity> EmailCleanerSettingsRepository => new DbContextRepository<EmailCleanerSettingsEntity>(_applicationDbContext, _contextAccessor);
        public DbContextRepository<EmailDataEntity> EmailDataRepository => new DbContextRepository<EmailDataEntity>(_applicationDbContext, _contextAccessor);
        public DbContextRepository<EmailAddressMappingEntity> EmailAddressMappingRepository => new DbContextRepository<EmailAddressMappingEntity>(_applicationDbContext, _contextAccessor);
        public DbContextRepository<LogMessageEntity> LogMessageRepository => new DbContextRepository<LogMessageEntity>(_applicationDbContext, _contextAccessor);
        public ClaimsAccessor ClaimsAccessor => new ClaimsAccessor();
        public IOptions<SecurityData> SecurityData => _securityData;

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
