using Data.ContextAccessor.Interfaces;
using Data.ContextAccessor.ModuleSettings;
using Data.Shared;
using Data.Shared.Email;
using Logic.Interfaces.Interfaces;
using Logic.Interfaces.Models;
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

        public async Task<List<EmailAccountSettings>> GetEmailAccountSettings()
        {
            try
            {
                if (!_applicationUnitOfWork.IsAuthenticated)
                {
                    throw new Exception("Could not get email account settings, reason: unauthenticated!");
                }

                var accountSettings = await GetAccountSettings();

                return accountSettings;
            }
            catch (Exception exception)
            {
                if (_logger != null)
                {
                    await _logger.Error("Could not load settings.", exception.Message);
                }
            }

            return new List<EmailAccountSettings>();
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
                    GenericSettigsModules.EmailAccountInterfaceModuleName,
                    GenericSettigsModules.EmailAccountInterfaceModuleType,
                    _applicationUnitOfWork.CurrentUserId) ?? new List<EmailAccountSettings>();

                var settingIsAvailable = settings.FirstOrDefault(s => s.SettingsGuid == accountSettings.SettingsGuid) != null;

                if (!settingIsAvailable)
                {
                    accountSettings.SettingsGuid = Guid.NewGuid().ToString();
                    accountSettings.UserId = _applicationUnitOfWork.CurrentUserId;
                    accountSettings.Password = _passwordHandler.Encrypt(accountSettings.Password);

                    settings.Add(accountSettings);

                    await _applicationUnitOfWork.GenericSettingsRepository.SaveSettings(
                      GenericSettigsModules.EmailAccountInterfaceModuleName,
                    GenericSettigsModules.EmailAccountInterfaceModuleType,
                       _applicationUnitOfWork.CurrentUserId,
                       settings);

                    if (_logger != null)
                    {
                        await _logger.Info($"Added new account [{accountSettings.SettingsGuid}].");
                    }

                    return true;
                }

                settings.ForEach(s =>
                {
                    if (s.SettingsGuid == accountSettings.SettingsGuid)
                    {
                        s.ProviderType = accountSettings.ProviderType;
                        s.AccountName = accountSettings.AccountName;
                        s.Server = accountSettings.Server;
                        s.Port = accountSettings.Port;
                        s.EmailAddress = accountSettings.EmailAddress;

                        // update password only if changed
                        if (s.Password != accountSettings.Password)
                        {
                            s.Password = _passwordHandler.Encrypt(accountSettings.Password);
                        }

                        s.ConnectionTestPassed = accountSettings.ConnectionTestPassed;
                    }
                });

                await _applicationUnitOfWork.GenericSettingsRepository.SaveSettings(
                   GenericSettigsModules.EmailAccountInterfaceModuleName,
                    GenericSettigsModules.EmailAccountInterfaceModuleType,
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

        public async Task<bool> ExcecuteConnectionTest(EmailAccountConnectionData model)
        {
            try
            {
                if (!_applicationUnitOfWork.IsAuthenticated)
                {
                    throw new Exception("Could not execute connection test, reason: unauthenticated!");
                }

                var client = new EmailInterfaceEmailClient(_applicationUnitOfWork);

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

        public async Task<bool> ExecuteEmailMappingTableUpdate(string settingsGuid)
        {
            try
            {
                if (!_applicationUnitOfWork.IsAuthenticated)
                {
                    throw new Exception("Could not execute email mapping update, reason: unauthenticated!");
                }

                var settings = await _applicationUnitOfWork.GenericSettingsRepository.GetSettings<List<EmailAccountSettings>>(
                   GenericSettigsModules.EmailAccountInterfaceModuleName,
                   GenericSettigsModules.EmailAccountInterfaceModuleType,
                   _applicationUnitOfWork.CurrentUserId) ?? new List<EmailAccountSettings>();

                if (!settings.Any())
                {
                    if (_logger != null)
                    {
                        await _logger.Info($"Execute email mapping update for [{settingsGuid}] aborted, reason no settings found.");
                    }

                    return false;
                }

                var settingsToProcess = settings.FirstOrDefault(s => s.SettingsGuid == settingsGuid);

                if (settingsToProcess == null)
                {
                    if (_logger != null)
                    {
                        await _logger.Info($"Could not find setting [{settingsGuid}].");
                    }

                    return false;
                }

                var client = new EmailInterfaceEmailClient(_applicationUnitOfWork);

                var decodedPassword = _passwordHandler.Decrypt(settingsToProcess.Password);

                var emailsToProcess = await client.LoadMailsFromServer(new EmailAccountConnectionData
                {
                    Server = settingsToProcess.Server,
                    Port = settingsToProcess.Port,
                    EmailAddress = settingsToProcess.EmailAddress,
                    Password = decodedPassword
                });

                var addressEntities = await EnsureAllAddressEntitiesExists(emailsToProcess);
                var subjectEntities = await EnsureAllSubjectEntitiesExists(emailsToProcess);

                foreach (var email in emailsToProcess)
                {
                    var addressId = addressEntities.First(x => x.EmailAddress.ToLower() == email.FromAddress.ToLower()).Id;
                    var subjectId = subjectEntities.First(x => x.EmailSubject.ToLower() == email.Subject.ToLower()).Id;

                    await _applicationUnitOfWork.EmailMappingTable.InsertIfNotExists(new EmailMappingEntity
                    {
                        AddressId = addressId,
                        SubjectId = subjectId,
                        UserId = _applicationUnitOfWork.CurrentUserId,
                        UserDefinedTargetFolder = "Unknown",
                        UserDefinedAsSpam = false,
                        PredictedTargetFolder = "Unknown",
                        PredictedAsSpam = false,
                    }, x => x.UserId == _applicationUnitOfWork.CurrentUserId
                    && x.AddressId == addressId
                    && x.SubjectId == subjectId);
                }

                await _applicationUnitOfWork.EmailMappingTable.SaveChangesAsync();

                return true;
            }
            catch (Exception exception)
            {
                if (_logger != null)
                {
                    await _logger.Error($"Could not execute email mapping update for [{settingsGuid}]", exception.Message);
                }

                return false;
            }
        }

        #region private 

        private async Task<List<EmailAddressEntity>> EnsureAllAddressEntitiesExists(List<EmailDataModel> mappingTableList)
        {
            var addressEntities = await _applicationUnitOfWork.EmailAddressTable.GetAllAsync();
            var addedEntities = new List<EmailAddressEntity>();

            foreach (var mapping in mappingTableList)
            {
                if (!addressEntities.Any(x => x.EmailAddress.ToLower() == mapping.FromAddress.ToLower())
                    && !addedEntities.Any(x => x.EmailAddress.ToLower() == mapping.FromAddress.ToLower()))
                {
                    var entity = new EmailAddressEntity
                    {
                        EmailAddress = mapping.FromAddress.Trim(),
                        Domain = mapping.FromAddress.Split('@')[1].Trim(),
                    };

                    await _applicationUnitOfWork.EmailAddressTable.AddAsync(entity);

                    addedEntities.Add(entity);
                }
            }

            if (addedEntities.Any())
            {
                await _applicationUnitOfWork.EmailAddressTable.SaveChangesAsync();
                addressEntities.AddRange(addedEntities);
            }

            return addressEntities;
        }

        private async Task<List<EmailSubjectEntity>> EnsureAllSubjectEntitiesExists(List<EmailDataModel> mappingTableList)
        {
            var subjectEntities = await _applicationUnitOfWork.EmailSubjectTable.GetAllAsync();
            var addedEntities = new List<EmailSubjectEntity>();

            foreach (var mapping in mappingTableList)
            {
                if (!subjectEntities.Any(x => x.EmailSubject.ToLower() == mapping.Subject.ToLower())
                    && !addedEntities.Any(x => x.EmailSubject.ToLower() == mapping.Subject.ToLower()))
                {
                    var entity = new EmailSubjectEntity
                    {
                        EmailSubject = mapping.Subject,
                    };

                    await _applicationUnitOfWork.EmailSubjectTable.AddAsync(entity);

                    addedEntities.Add(entity);
                }
            }

            if (addedEntities.Any())
            {
                await _applicationUnitOfWork.EmailSubjectTable.SaveChangesAsync();
                subjectEntities.AddRange(addedEntities);
            }

            return subjectEntities;
        }

        private async Task<List<EmailAccountSettings>> GetAccountSettings()
        {
            var accountSettings = await _applicationUnitOfWork.GenericSettingsRepository.GetSettings<List<EmailAccountSettings>>(
                   GenericSettigsModules.EmailAccountInterfaceModuleName,
                   GenericSettigsModules.EmailAccountInterfaceModuleType,
                   _applicationUnitOfWork.CurrentUserId) ?? new List<EmailAccountSettings>();

            if (accountSettings == null || !accountSettings.Any())
            {
                var emailAccount = GetDefaultEmailAccountModel();

                await _applicationUnitOfWork.GenericSettingsRepository.SaveSettings(GenericSettigsModules.EmailAccountInterfaceModuleName,
                    GenericSettigsModules.EmailAccountInterfaceModuleType, _applicationUnitOfWork.CurrentUserId, new List<EmailAccountSettings> { emailAccount });

                return new List<EmailAccountSettings> { emailAccount };
            }

            return accountSettings;
        }

        private EmailAccountSettings GetDefaultEmailAccountModel()
        {
            return new EmailAccountSettings
            {
                SettingsGuid = Guid.NewGuid().ToString(),
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
