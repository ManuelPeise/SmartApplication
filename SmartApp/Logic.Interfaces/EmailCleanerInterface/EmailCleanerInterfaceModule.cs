using Data.ContextAccessor.Interfaces;
using Data.Shared;
using Logic.Interfaces.EmailAccountInterface;
using Logic.Interfaces.Interfaces;
using Logic.Shared;

namespace Logic.Interfaces.EmailCleanerInterface
{
    public class EmailCleanerInterfaceModule : IEmailCleanerInterfaceModule
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly PasswordHandler _passwordHandler;
        private Logger<EmailAccountInterfaceModule>? _logger;

        public EmailCleanerInterfaceModule(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _passwordHandler = new PasswordHandler(applicationUnitOfWork.SecurityData);
            _logger = new Logger<EmailAccountInterfaceModule>(applicationUnitOfWork);
        }





        #region dispose

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: Verwalteten Zustand (verwaltete Objekte) bereinigen
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
