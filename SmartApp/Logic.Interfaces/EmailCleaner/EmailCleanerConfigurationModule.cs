using Data.ContextAccessor.Interfaces;
using Data.Shared;
using Data.Shared.Email;
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
            using (var settings = new EmailCleanerSettings(_applicationUnitOfWork))
            {
                try
                {
                    if (!_applicationUnitOfWork.IsAuthenticated)
                    {
                        throw new Exception("Could not get email cleaner configuartions, reason: unauthenticated!");
                    }

                    var targetFolderEntities = await configuration.GetAllTargetFolderEntities();
                    var configurationEntities = await configuration.LoadEntities(accountId);
                    var emailCleanerSettings = await settings.GetEmailCleanerSettings();

                    var relatedSettings = emailCleanerSettings.Single(x => x.AccountId == accountId);

                    return new EmailClassificationPageModel
                    {
                        AccountId = accountId,
                        SpamPredictionEnabled = relatedSettings.SpamPredictionEnabled,
                        FolderPredictionEnabled = relatedSettings.FolderPredictionEnabled,
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
                            PredictedTargetFolderId = e.PredictedTargetFolderId,
                            Backup = e.Backup,
                            Delete = e.Delete,
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

        public async Task<bool> UpdateConfigurations(List<EmailClassificationModel> configurations)
        {
            using (var configuration = new EmailCleanerConfiguration(_applicationUnitOfWork))
            {
                try
                {
                    var affectedConfigurations = await configuration.LoadEntities(configurations.Select(x => x.Id)) ?? new List<EmailCleanupConfigurationEntity>();

                    foreach (var affectedConfiguration in affectedConfigurations)
                    {
                        var relatedConfig = configurations.Single(x => x.Id == affectedConfiguration.Id);

                        affectedConfiguration.IsSpam = relatedConfig.IsSpam;
                        affectedConfiguration.IsPredictedAsSpam = relatedConfig.PredictedAsSpam;
                        affectedConfiguration.TargetFolderId = relatedConfig.TargetFolderId;
                        affectedConfiguration.PredictedTargetFolderId = affectedConfiguration.PredictedTargetFolderId;
                        affectedConfiguration.Backup = relatedConfig.Backup;
                        affectedConfiguration.Delete = relatedConfig.Delete;
                    }

                    await configuration.UpdateConfigurations(affectedConfigurations);

                    return true;
                }
                catch (Exception exception)
                {
                    if (_logger != null)
                    {
                        await _logger.Error($"Could not update spam classification data.", exception.Message);
                    }

                    return false;
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
