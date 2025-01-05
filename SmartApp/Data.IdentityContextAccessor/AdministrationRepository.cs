using Data.AppContext;
using Data.ContextAccessor.Interfaces;
using Data.Shared.AccessRights;
using Data.Shared.Logging;

namespace Data.ContextAccessor
{
    public class AdministrationRepository: IAdministrationRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IIdentityRepository _identityRepository;
        private readonly ILogRepository _logRepository;

        public AdministrationRepository(ApplicationDbContext applicationDbContext, IIdentityRepository identityRepository, ILogRepository logRepository)
        {
            _applicationDbContext = applicationDbContext;
            _identityRepository = identityRepository;
            _logRepository = logRepository;
        }

        public RepositoryBase<LogMessageEntity> LogMessageRepository =>  new RepositoryBase<LogMessageEntity>(_applicationDbContext);
        public RepositoryBase<AccessRightEntity> AccessRightRepository => new RepositoryBase<AccessRightEntity>(_applicationDbContext);
        public RepositoryBase<UserAccessRightEntity> UserAccessRightRepository => new RepositoryBase<UserAccessRightEntity>(_applicationDbContext);
        public IIdentityRepository IdentityRepository => _identityRepository;
        public ILogRepository LogRepository => _logRepository;

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
