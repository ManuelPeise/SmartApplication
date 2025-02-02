using Data.ContextAccessor.Interfaces;
using Logic.Interfaces.EmailAccountInterface;
using Logic.Interfaces.Interfaces;
using Logic.Interfaces.Models;
using Logic.Shared;

namespace Logic.Interfaces.EmailCleanerInterface
{
    public class EmailCleanerInterfaceModule : IEmailCleanerInterfaceModule
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;

        private Logger<EmailAccountInterfaceModule>? _logger;

        public EmailCleanerInterfaceModule(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _logger = new Logger<EmailAccountInterfaceModule>(applicationUnitOfWork);
        }

        public async Task<List<EmailCleanerUiSettings>> GetEmailCleanerSettings()
        {
            using (var settings = new EmailCleanerSettings(_applicationUnitOfWork))
            {
                try
                {
                    var entities = await settings.GetEmailCleanerSettings(true);

                    if (!entities.Any())
                    {
                        await settings.CreateEmailCleanerSettings();

                        entities = await settings.GetEmailCleanerSettings(true);
                    }

                    return entities.Select(e => new EmailCleanerUiSettings
                    {
                        AccountId = e.AccountId,
                        SettingsId = e.Id,
                        UserId = e.UserId,
                        AccountName = e.Account.AccountName,
                        EmailAddress = e.Account.EmailAddress,
                        EmailCleanerEnabled = e.EmailCleanerEnabled,
                        UseScheduledEmailDataImport = e.UseScheduledEmailDataImport,
                        ShareDataWithAi = e.ShareDataWithAi,
                        ProviderType = e.Account.ProviderType,
                        ConnectionTestPassed = e.Account.ConnectionTestPassed,
                        UpdatedAt = e.UpdatedAt?.ToString("dd.MM.yyyy HH:mm"),
                        UpdatedBy = e.UpdatedBy
                    }).ToList();

                }
                catch (Exception exception)
                {
                    if (_logger != null)
                    {
                        await _logger.Error("Could not load Email cleaner settings(s)", exception.Message);
                    }
                }

                return new List<EmailCleanerUiSettings>();
            }
        }

        public async Task<bool> UpdateEmailCleanerSetting(EmailCleanerUiSettings model)
        {
            using (var settings = new EmailCleanerSettings(_applicationUnitOfWork))
            {
                try
                {
                    var entity = await settings.GetEmailCleanerSetting(model.SettingsId);

                    if (entity == null)
                    {
                        return false;
                    }

                    entity.EmailCleanerEnabled = model.EmailCleanerEnabled;
                    entity.UseScheduledEmailDataImport = model.UseScheduledEmailDataImport;
                    entity.ShareDataWithAi = model.ShareDataWithAi;

                    await settings.UpdateEmailCleanerSetting(entity);

                    await _applicationUnitOfWork.EmailCleanerSettingsTable.SaveChangesAsync();

                    if (_logger != null)
                    {
                        await _logger.Info($"Email cleaner settings {model.SettingsId} updated!");
                    }

                    return true;
                }
                catch (Exception exception)
                {

                    if (_logger != null)
                    {
                        await _logger.Error("Could not update email cleaner configuration.", exception.Message);
                    }

                    return false;
                }
            }
        }


        #region dispose

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _applicationUnitOfWork?.Dispose();
                }

                _logger = null;
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
