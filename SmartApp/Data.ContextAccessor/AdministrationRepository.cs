using Data.ContextAccessor.Interfaces;
using Data.Databases;
using Data.Shared.Logging;
using Microsoft.AspNetCore.Http;

namespace Data.ContextAccessor
{
    public class AdministrationRepository: IAdministrationRepository
    {
        private readonly ApplicationContext _applicationDbContext;
        private readonly UserIdentityContext _identityDbContext;
        private readonly IIdentityRepository _identityRepository;
       
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IClaimsAccessor _claimsAccessor;
        
        public AdministrationRepository(ApplicationContext applicationDbContext, UserIdentityContext identityDbContext, IIdentityRepository identityRepository, IHttpContextAccessor contectAccessor, IClaimsAccessor claimsAccessor)
        {
            _applicationDbContext = applicationDbContext;
            _identityDbContext = identityDbContext;
            _identityRepository = identityRepository;
            _contextAccessor = contectAccessor;
            _claimsAccessor = claimsAccessor;
        }

        public DbContextRepository<LogMessageEntity> LogMessageRepository =>  new DbContextRepository<LogMessageEntity>(_applicationDbContext, _contextAccessor);
        public IIdentityRepository IdentityRepository => _identityRepository;
        public IClaimsAccessor ClaimsAccessor => _claimsAccessor;

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
