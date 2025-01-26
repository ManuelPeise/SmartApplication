using Data.ContextAccessor.Interfaces;
using Data.ContextAccessor.ModuleSettings;
using Data.Shared;
using Logic.Interfaces.EmailAccountInterface;
using Logic.Interfaces.EmailCleanerInterface.Models;
using Logic.Interfaces.Interfaces;
using Logic.Interfaces.Models;
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

        public async Task<List<EmailCleanerInterfaceConfigurationUiModel>> GetEmailCleanerConfigurations(bool loadFolderMappings)
        {
            try
            {
                if (!_applicationUnitOfWork.IsAuthenticated)
                {
                    throw new Exception("Could not update email account settings, reason: unauthenticated!");
                }

                var configurations = await GetConfigurations();

                await LoadAndUpdateFolderMappings(configurations);

                await SaveEmailCleanerSettings(configurations);

                await LoadEmailData(configurations, loadFolderMappings);

                return configurations.Select(cnf => (EmailCleanerInterfaceConfigurationUiModel)cnf.Value).ToList();
            }
            catch (Exception exception)
            {
                if (_logger != null)
                {
                    await _logger.Error("Could not load Email cleaner configuration(s)", exception.Message);
                }

                return new List<EmailCleanerInterfaceConfigurationUiModel>();
            }
        }

        public async Task<bool> UpdateEmailCleanerConfiguration(EmailCleanerUpdateModel model)
        {
            try
            {
                var configurations = await GetConfigurations();

                if (configurations.ContainsKey(model.SettingsGuid))
                {
                    configurations[model.SettingsGuid].EmailCleanerEnabled = model.EmailCleanerEnabled;
                    configurations[model.SettingsGuid].UseAiSpamPrediction = model.UseAiSpamPrediction;
                    configurations[model.SettingsGuid].UseAiTargetFolderPrediction = model.UseAiTargetFolderPrediction;
                    configurations[model.SettingsGuid].UpdatedAt = DateTime.UtcNow.ToString("dd.MM.yyyy HH:mm");
                    configurations[model.SettingsGuid].UpdatedBy = _applicationUnitOfWork.CurrentUserName;

                    if (_logger != null)
                    {
                        await _logger.Info($"Email cleaner settings {model.SettingsGuid} updated!");
                    }

                    await SaveEmailCleanerSettings(configurations);

                    return true;
                }

                if (_logger != null)
                {
                    await _logger.Error($"Could not update email cleaner configuration {model.SettingsGuid} not found.");
                }

                return false;
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


        #region private members

        private async Task<Dictionary<string, EmailCleanerInterfaceConfiguration>> GetConfigurations()
        {
            var configurationDictionary = new Dictionary<string, EmailCleanerInterfaceConfiguration>();

            var accountSettings = await _applicationUnitOfWork.GenericSettingsRepository.GetSettings<List<EmailAccountSettings>>(
                   GenericSettigsModules.EmailAccountInterfaceModuleName,
                   GenericSettigsModules.EmailAccountInterfaceModuleType,
                   _applicationUnitOfWork.CurrentUserId) ?? new List<EmailAccountSettings>();

            var emailCleanerSettings = await _applicationUnitOfWork.GenericSettingsRepository.GetSettings<Dictionary<string, EmailCleanerInterfaceConfiguration>>(
                   GenericSettigsModules.EmailCleanerInterfaceModuleName,
                   GenericSettigsModules.EmailCleanerInterfaceModuleType,
                   _applicationUnitOfWork.CurrentUserId) ?? new Dictionary<string, EmailCleanerInterfaceConfiguration>();

            foreach (var settings in accountSettings)
            {
                configurationDictionary[settings.SettingsGuid] = new EmailCleanerInterfaceConfiguration
                {
                    SettingsGuid = settings.SettingsGuid,
                    UserId = settings.UserId,
                    AccountName = settings.AccountName,
                    ConnectionTestPassed = settings.ConnectionTestPassed,
                    ProviderType = settings.ProviderType,
                    Server = settings.Server,
                    Port = settings.Port,
                    EmailAddress = settings.EmailAddress,
                    Password = settings.Password,
                    
                };

                if (emailCleanerSettings.ContainsKey(settings.SettingsGuid))
                {
                    configurationDictionary[settings.SettingsGuid].EmailCleanerEnabled = emailCleanerSettings[settings.SettingsGuid].EmailCleanerEnabled;
                    configurationDictionary[settings.SettingsGuid].DomainFolderMapping = emailCleanerSettings[settings.SettingsGuid].DomainFolderMapping;
                    configurationDictionary[settings.SettingsGuid].UpdatedAt = emailCleanerSettings[settings.SettingsGuid].UpdatedAt;
                    configurationDictionary[settings.SettingsGuid].UpdatedBy = emailCleanerSettings[settings.SettingsGuid].UpdatedBy;
                }
                else
                {
                    configurationDictionary[settings.SettingsGuid].EmailCleanerEnabled = false;
                    configurationDictionary[settings.SettingsGuid].DomainFolderMapping = new List<EmailDomainFolderMapping>();
                }
            }

            return configurationDictionary;
        }

        private async Task SaveEmailCleanerSettings(Dictionary<string, EmailCleanerInterfaceConfiguration> configurations)
        {
            await _applicationUnitOfWork.GenericSettingsRepository.SaveSettings(GenericSettigsModules.EmailCleanerInterfaceModuleName,
                   GenericSettigsModules.EmailCleanerInterfaceModuleType, _applicationUnitOfWork.CurrentUserId, configurations);
        }

        private async Task LoadAndUpdateFolderMappings(Dictionary<string, EmailCleanerInterfaceConfiguration> configurations)
        {
            foreach (var key in configurations.Keys)
            {
                var emailMappingModels = new List<EmailMappingModel>();

                var settingsGuid = configurations[key].SettingsGuid;

                var domainMappingDictionary = configurations[key].DomainFolderMapping.ToDictionary(x => x.SourceDomain) ?? new Dictionary<string, EmailDomainFolderMapping>();

                await LoadFolderMappingsAsync(domainMappingDictionary, settingsGuid);

                configurations[key].DomainFolderMapping = domainMappingDictionary.Select(x => x.Value).ToList();
            }
        }

        private async Task LoadFolderMappingsAsync(Dictionary<string, EmailDomainFolderMapping> existingEmailFolderMappings, string settingsGuid)
        {
            var emailMappings = await _applicationUnitOfWork.EmailMappingTable.GetAllAsyncBy(mapping =>
                   mapping.UserId == _applicationUnitOfWork.CurrentUserId
                   && mapping.SettingsGuid == settingsGuid);

            var emailMappingAddressIds = emailMappings.Select(mapping => mapping.AddressId);

            await _applicationUnitOfWork.EmailAddressTable.GetAllAsyncBy(address => emailMappingAddressIds.Contains(address.Id));

            var domainMappings = (from mapping in emailMappings
                                  where !string.IsNullOrEmpty(mapping.AddressEntity.Domain)
                                  group mapping by mapping.AddressEntity.Domain into mappingDomainGroup
                                  select new EmailDomainFolderMapping
                                  {
                                      SourceDomain = mappingDomainGroup.Key,
                                      TargetFolder = string.Empty,
                                      PredictedTargetFolder = string.Empty,
                                      AutomatedCleanupEnabled = false,
                                      IsActive = false,
                                      ForceDeleteSpamMails = false,
                                  }).ToList();

            if (!domainMappings.Any())
            {
                return;
            }

            foreach (var mapping in domainMappings)
            {
                if (!existingEmailFolderMappings.ContainsKey(mapping.SourceDomain))
                {
                    existingEmailFolderMappings[mapping.SourceDomain] = mapping;
                }
            }
        }

        private async Task<List<EmailMappingModel>> LoadEmailsFromServer(EmailCleanerInterfaceConfiguration configuration)
        {
            var client = new EmailInterfaceEmailClient(_applicationUnitOfWork);

            var emailData = await client.LoadMailsFromServer(new EmailAccountConnectionTestRequest
            {
                Server = configuration.Server,
                Port = configuration.Port,
                EmailAddress = configuration.EmailAddress,
                Password = _passwordHandler.Decrypt(configuration.Password),
            });

            return emailData;
        }

        private async Task LoadEmailData(Dictionary<string, EmailCleanerInterfaceConfiguration> configurations, bool loadFolderMappings)
        {
            foreach (var configuration in configurations)
            {
               
                    var currentEmailData = await LoadEmailsFromServer(configuration.Value);

                    if (!currentEmailData.Any())
                    {
                        continue;
                    }

                    foreach (var mapping in configuration.Value.DomainFolderMapping)
                    {
                        if (currentEmailData.Any(mail => mail.FromAddress.Split('@')[1].ToLower() == mapping.SourceDomain.ToLower()))
                        {
                            mapping.CurrentEmailData = currentEmailData
                                .Where(mail => mail.FromAddress.Split('@')[1].ToLower() == mapping.SourceDomain.ToLower())
                                .Select(mail => new EmailDomainModel
                                {
                                    EmailId = mail.MessageId,
                                    SourceAddress = mail.FromAddress,
                                    Subject = mail.Subject,
                                    Domain = mail.FromAddress.Split('@')[1],
                                    PredictionResult = "n/a",
                                    IsNew = mail.IsNew,
                                    IsSpam = false
                                }).ToList();
                        }
                    }
                

                configuration.Value.UnmappedDomains = configuration.Value.DomainFolderMapping.Count(x => string.IsNullOrEmpty(x.TargetFolder));

                if (!loadFolderMappings)
                {
                    configuration.Value.DomainFolderMapping = new List<EmailDomainFolderMapping>();
                }
            }
        }

        #endregion

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
