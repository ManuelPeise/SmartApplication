using Data.AppContext;
using Data.ContextAccessor.Interfaces;
using Data.Shared;

namespace Data.ContextAccessor
{
    public class SettingsRepository : ISettingsRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        

        public SettingsRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public RepositoryBase<EmailAccountEntity> EmailAccountRepository => new RepositoryBase<EmailAccountEntity>(_applicationDbContext);


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
