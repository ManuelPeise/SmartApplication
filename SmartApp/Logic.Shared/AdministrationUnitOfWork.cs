using Data.AppContext;
using Data.ContextAccessor;
using Data.ContextAccessor.Interfaces;
using Data.Identity;
using Data.Shared.Email;
using Data.Shared.Logging;
using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Logic.Shared
{
    public class AdministrationUnitOfWork: AUnitOfWorkBase, IAdministrationUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly IdentityDbContext _identityContext;

        private readonly IRepositoryBase<EmailAccountSettingsEntity> _emailAccountSettingsRepository;
        private bool disposedValue;

        public AdministrationUnitOfWork(IdentityDbContext identityContext, ApplicationDbContext applicationDbContext, IHttpContextAccessor contextAccessor, ILogRepository logRepository): base(identityContext, contextAccessor, logRepository)
        {
            _context = applicationDbContext;
            _identityContext = identityContext;
            _emailAccountSettingsRepository = new RepositoryBase<EmailAccountSettingsEntity>(applicationDbContext);
        }

        public IRepositoryBase<EmailAccountSettingsEntity> EmailAccountSettingsRepository => _emailAccountSettingsRepository ?? new RepositoryBase<EmailAccountSettingsEntity>(_context);

        public async Task AddLogMessage (LogMessageEntity logMessageEntity) => await LogMessage(logMessageEntity);

        public T? GetValueFromClaims<T>(string key) => GetClaimsValue<T>(key);

        public async Task SaveChangesAsync() => await SaveChanges();

        #region dispose

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _context.Dispose();
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
