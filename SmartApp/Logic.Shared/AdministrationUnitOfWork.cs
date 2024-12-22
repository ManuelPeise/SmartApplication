using Data.AppContext;
using Data.ContextAccessor;
using Data.ContextAccessor.Interfaces;
using Data.Shared.Ai;
using Data.Shared.Logging;
using Data.Shared.Settings;
using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Logic.Shared
{
    public class AdministrationUnitOfWork: AUnitOfWorkBase, IAdministrationUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        private readonly IRepositoryBase<GenericSettingsEntity> _settingsRepository;
        private readonly IRepositoryBase<EmailClassificationTrainingDataEntity> _emailSpamTrainingsRepository;

        private bool disposedValue;

        public AdministrationUnitOfWork(ApplicationDbContext applicationDbContext, IHttpContextAccessor contextAccessor, ILogRepository logRepository): base(contextAccessor, logRepository, null, applicationDbContext)
        {
            _context = applicationDbContext;

            _settingsRepository = new RepositoryBase<GenericSettingsEntity>(applicationDbContext);
            _emailSpamTrainingsRepository = new RepositoryBase<EmailClassificationTrainingDataEntity>(applicationDbContext);
        }

        public IRepositoryBase<GenericSettingsEntity> SettingsRepository => _settingsRepository ?? new RepositoryBase<GenericSettingsEntity>(_context);
        public IRepositoryBase<EmailClassificationTrainingDataEntity> EmailSpamTrainingsRepository => _emailSpamTrainingsRepository ?? new RepositoryBase<EmailClassificationTrainingDataEntity>(_context);

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
                    _emailSpamTrainingsRepository.Dispose();
                    _emailSpamTrainingsRepository?.Dispose();
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
