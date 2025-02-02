using Data.ContextAccessor.Interfaces;
using Data.Shared;
using Logic.Interfaces.Interfaces;
using Logic.Interfaces.Models;
using Logic.Shared;

namespace Logic.Interfaces.EmailCleaner
{
    public class EmailCleanerConfigurationModule : IEmailCleanerConfigurationModule
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly PasswordHandler _passwordHandler;
        private Logger<EmailCleanerConfigurationModule>? _logger;
        private bool disposedValue;

        public EmailCleanerConfigurationModule(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _passwordHandler = new PasswordHandler(applicationUnitOfWork.SecurityData);
            _logger = new Logger<EmailCleanerConfigurationModule>(applicationUnitOfWork);
        }

        public async Task<EmailClassificationPageModel?> LoadConfigurationData(int accountId)
        {
            using (var configuration = new EmailCleanerConfiguration(_applicationUnitOfWork))
            {
                try
                {
                    if (!_applicationUnitOfWork.IsAuthenticated)
                    {
                        throw new Exception("Could not get email cleaner configuartions, reason: unauthenticated!");
                    }

                    var targetFolderEntities = await configuration.GetAllTargetFolderEntities();
                    var configurationEntities = await configuration.LoadEntities(accountId);

                    return new EmailClassificationPageModel
                    {
                        AccountId = accountId,
                        Folders = (targetFolderEntities.Select(e => new EmailFolderModel
                        {
                            FolderId = e.Id,
                            ResourceKey = e.ResourceKey,
                        })).ToList(),
                        ClassificationModels = configurationEntities.Select(e => new EmailClassificationModel
                        {
                            Id = e.Id,
                            EmailAddress = e.Address.EmailAddress,
                            Subject = e.Subject.EmailSubject,
                            IsSpam = e.IsSpam,
                            TargetFolderId = e.TargetFolderId,
                            PredictedAsSpam = e.IsPredictedAsSpam,
                            PredictedTargetFolderId = e.PredictedTargetFolderId
                        }).ToList(),
                    };

                }
                catch (Exception exception)
                {
                    if (_logger != null)
                    {
                        await _logger.Error($"Could not load spam classification data for account: [{accountId}]", exception.Message);
                    }

                    return null;
                }
            }
        }

        #region dispose

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _applicationUnitOfWork.Dispose();
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
