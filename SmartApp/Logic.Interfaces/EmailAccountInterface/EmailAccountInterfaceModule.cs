using Data.ContextAccessor.Interfaces;
using Data.ContextAccessor.ModuleSettings;
using Data.Shared;
using Logic.Interfaces.EmailAccountInterface.Models;
using Logic.Interfaces.Interfaces;
using Logic.Shared;
using Shared.Enums;

namespace Logic.Interfaces.EmailAccountInterface
{
    public class EmailAccountInterfaceModule : IEmailAccountInterfaceModule
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly PasswordHandler _passwordHandler;
        private Logger<EmailAccountInterfaceModule>? _logger;

        private bool disposedValue;

        public EmailAccountInterfaceModule(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _passwordHandler = new PasswordHandler(applicationUnitOfWork.SecurityData);
            _logger = new Logger<EmailAccountInterfaceModule>(applicationUnitOfWork);
        }

        public async Task<List<EmailAccountSettingsUiModel>> GetEmailAccountSettings()
        {
            try
            {
                if (!_applicationUnitOfWork.IsAuthenticated)
                {
                    throw new Exception("Could not get email account settings, reason: unauthenticated!");
                }

                var accountSettings = await GetAccountSettings();

                return accountSettings.Select(setting => (EmailAccountSettingsUiModel)setting).ToList();
            }
            catch (Exception exception)
            {
                if (_logger != null)
                {
                    await _logger.Error("Could not load settings.", exception.Message);
                }
            }

            return new List<EmailAccountSettingsUiModel>();
        }

        public async Task<bool> UpdateEmailAccountSettings(EmailAccountSettings accountSettings)
        {
            try
            {
                if (!_applicationUnitOfWork.IsAuthenticated)
                {
                    throw new Exception("Could not update email account settings, reason: unauthenticated!");
                }

                var settings = await _applicationUnitOfWork.GenericSettingsRepository.GetSettings<List<EmailAccountSettings>>(
                    EmailAccountInterfaceSettings.ModuleName,
                    EmailAccountInterfaceSettings.ModuleType,
                    _applicationUnitOfWork.CurrentUserId) ?? new List<EmailAccountSettings>();

                var settingIsAvailable = settings.FirstOrDefault(s => s.SettingsGuid == accountSettings.SettingsGuid) != null;

                if (!settingIsAvailable)
                {
                    if (_logger != null)
                    {
                        await _logger.Error($"Could not update email account settings for [{accountSettings.SettingsGuid}].", null);
                    }

                    return false;
                }

                settings.ForEach(s =>
                {
                    if (s.SettingsGuid == accountSettings.SettingsGuid)
                    {
                        s.AccountName = accountSettings.AccountName;
                        s.Server = accountSettings.Server;
                        s.Port = accountSettings.Port;
                        s.EmailAddress = accountSettings.EmailAddress;
                        s.Password = _passwordHandler.Encrypt(s.Password);
                        s.ConnectionTestPassed = accountSettings.ConnectionTestPassed;
                    }
                });

                await _applicationUnitOfWork.GenericSettingsRepository.SaveSettings(
                    EmailAccountInterfaceSettings.ModuleName,
                    EmailAccountInterfaceSettings.ModuleType,
                    _applicationUnitOfWork.CurrentUserId,
                    settings);

                return true;
            }
            catch (Exception exception)
            {
                if (_logger != null)
                {
                    await _logger.Error($"Could not update account settings [{accountSettings.SettingsGuid}].", exception.Message);
                }

                return false;
            }
        }

        public async Task<bool> ExcecuteConnectionTest(EmailAccountConnectionTestRequest model)
        {
            try
            {
                if (!_applicationUnitOfWork.IsAuthenticated)
                {
                    throw new Exception("Could not execute connection test, reason: unauthenticated!");
                }

                var client = new EmailAccountInterfaceClient(_applicationUnitOfWork);

                return await client.ExecuteConnectionTest(model);
            }
            catch (Exception exception) 
            {
                if (_logger != null)
                {
                    await _logger.Error($"Could test account connection for [{model.EmailAddress}].", exception.Message);
                }

                return false;
            }
        }

        #region private 

        private async Task<List<EmailAccountSettings>> GetAccountSettings()
        {
            var accountSettings = await _applicationUnitOfWork.GenericSettingsRepository.GetSettings<List<EmailAccountSettings>>(
                   EmailAccountInterfaceSettings.ModuleName,
                   EmailAccountInterfaceSettings.ModuleType
                , _applicationUnitOfWork.CurrentUserId) ?? new List<EmailAccountSettings>();

            if (accountSettings == null || !accountSettings.Any())
            {
                var emailAccount = GetDefaultEmailAccountModel();

                await _applicationUnitOfWork.GenericSettingsRepository.SaveSettings(EmailAccountInterfaceSettings.ModuleName,
                    ModuleTypeEnum.EmailAccountInterface, _applicationUnitOfWork.CurrentUserId, emailAccount);

                return new List<EmailAccountSettings> { emailAccount };
            }

            return accountSettings;
        }
        private EmailAccountSettings GetDefaultEmailAccountModel()
        {
            return new EmailAccountSettings
            {
                SettingsGuid = Guid.NewGuid(),
                UserId = _applicationUnitOfWork.CurrentUserId,
                AccountName = string.Empty,
                Server = string.Empty,
                Port = -1,
                EmailAddress = string.Empty,
                Password = string.Empty,
                ProviderType = EmailProviderTypeEnum.None,
                ConnectionTestPassed = false,
            };
        }

        #endregion

        #region dispose 

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
