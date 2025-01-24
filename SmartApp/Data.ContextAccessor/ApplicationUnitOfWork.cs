using Data.ContextAccessor.Interfaces;
using Data.ContextAccessor.Repositories;
using Data.Databases;
using Data.Shared.Logging;
using Microsoft.AspNetCore.Http;
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

        public DbContextRepository<LogMessageEntity> LogMessageRepository => new DbContextRepository<LogMessageEntity>(_applicationContext, _contextAccessor);
        public IdentityRepository IdentityRepository => new IdentityRepository(_userIdentityContext, _contextAccessor);
        public GenericSettingsRepository GenericSettingsRepository => new GenericSettingsRepository(_applicationContext, _contextAccessor);
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
