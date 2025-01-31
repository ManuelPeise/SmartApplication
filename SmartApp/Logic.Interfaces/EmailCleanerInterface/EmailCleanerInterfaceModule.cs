using Data.ContextAccessor.Interfaces;
using Data.ContextAccessor.ModuleSettings;
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

        private Logger<EmailAccountInterfaceModule>? _logger;

        public EmailCleanerInterfaceModule(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _logger = new Logger<EmailAccountInterfaceModule>(applicationUnitOfWork);
        }

        public async Task<List<EmailCleanerInterfaceConfigurationUiModel>> GetEmailCleanerConfigurations(bool loadFolderMappings)
        {
            using (var folderMappings = new FolderMappings(_applicationUnitOfWork))
            {
                try
                {
                    if (!_applicationUnitOfWork.IsAuthenticated)
                    {
                        throw new Exception("Could not update email account settings, reason: unauthenticated!");
                    }

                    var configurations = await GetConfigurations();

                    await SaveEmailCleanerSettings(configurations);

                    await LoadEmailData(configurations, folderMappings, loadFolderMappings);

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
        }

        public async Task<EmailCleanerMappingData<EmailFolderMappingData>?> GetFolderMappingData(string settingsGuid)
        {
            using (var folderMapping = new FolderMappings(_applicationUnitOfWork))
            {
                try
                {
                    if (!_applicationUnitOfWork.IsAuthenticated)
                    {
                        throw new Exception("Could not update email account settings, reason: unauthenticated!");
                    }

                    var configurations = await GetConfigurations();

                    if (!configurations.ContainsKey(settingsGuid))
                    {
                        if (_logger != null)
                        {
                            await _logger.Error($"Could not load mapping data, {settingsGuid} not found.");
                        }

                        return new EmailCleanerMappingData<EmailFolderMappingData>();
                    }

                    var mappingData = await folderMapping.GetFolderMappingData(configurations[settingsGuid]);

                    return new EmailCleanerMappingData<EmailFolderMappingData>
                    {
                        SettingsGuid = settingsGuid,
                        AccountName = configurations[settingsGuid].AccountName,
                        EmailAddress = configurations[settingsGuid].EmailAddress,
                        ProviderType = configurations[settingsGuid].ProviderType,
                        MappingData = mappingData,
                    };
                }
                catch (Exception exception)
                {
                    if (_logger != null)
                    {
                        await _logger.Error("Could not load Email cleaner configuration(s)", exception.Message);
                    }

                    return null;
                }
            }
        }

        public async Task<bool> ExecuteFolderMapping(string settingsGuid)
        {
            using (var folderMapping = new FolderMappings(_applicationUnitOfWork))
            {
                try
                {
                    if (!_applicationUnitOfWork.IsAuthenticated)
                    {
                        throw new Exception("Could not update email account settings, reason: unauthenticated!");
                    }

                    var configurations = await GetConfigurations();

                    if (!configurations.ContainsKey(settingsGuid))
                    {
                        if (_logger != null)
                        {
                            await _logger.Error($"Could not execute folder mapping, {settingsGuid} not found.");
                        }

                        return false;
                    }

                    await folderMapping.ExecuteFolderMapping(configurations[settingsGuid]);

                    return true;
                }
                catch (Exception exception)
                {
                    if (_logger != null)
                    {
                        await _logger.Error($"Could not execute folder mapping for [{settingsGuid}]", exception.Message);
                    }

                    return false;
                }
            }
        }

        public async Task<bool> UpdateEmailCleanerConfiguration(EmailCleanerUpdateModel model)
        {
            try
            {
                var configurations = await GetConfigurations();

                if (configurations.ContainsKey(model.SettingsGuid))
                {
                    if (!model.EmailCleanerEnabled)
                    {
                        await HandleDisableEmailCleaner(configurations, model.SettingsGuid);

                        return true;
                    }

                    configurations[model.SettingsGuid].EmailCleanerEnabled = model.EmailCleanerEnabled;
                    configurations[model.SettingsGuid].UseAiSpamPrediction = model.UseAiSpamPrediction;
                    configurations[model.SettingsGuid].UseAiTargetFolderPrediction = model.UseAiTargetFolderPrediction;
                    configurations[model.SettingsGuid].UpdatedAt = DateTime.UtcNow.ToString("dd.MM.yyyy HH:mm");
                    configurations[model.SettingsGuid].UpdatedBy = _applicationUnitOfWork.CurrentUserName;
                    configurations[model.SettingsGuid].FolderMappingEnabled = model.FolderMappingEnabled;
                    configurations[model.SettingsGuid].FolderMappingIsInitialized = model.FolderMappingIsInitialized;

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

        public async Task<bool> UpdateFolderMappings(EmailFolderMappingUpdate folderMappingUpdate)
        {
            using (var folderMapping = new FolderMappings(_applicationUnitOfWork))
            {
                try
                {

                    if (await folderMapping.UpdateFolderMappings(folderMappingUpdate))
                    {
                        if (_logger != null)
                        {
                            await _logger.Info($"Email cleaner folder mapping(s) [{folderMappingUpdate.SettingsGuid}] updated.");
                        }

                        return true;
                    }

                    return false;
                }
                catch (Exception exception)
                {
                    if (_logger != null)
                    {
                        await _logger.Error("Could not update Email cleaner folder mapping(s)", exception.Message);
                    }

                    return false;
                }
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
                    configurationDictionary[settings.SettingsGuid].UpdatedAt = emailCleanerSettings[settings.SettingsGuid].UpdatedAt;
                    configurationDictionary[settings.SettingsGuid].UpdatedBy = emailCleanerSettings[settings.SettingsGuid].UpdatedBy;
                    configurationDictionary[settings.SettingsGuid].FolderMappingEnabled = emailCleanerSettings[settings.SettingsGuid].FolderMappingEnabled;
                    configurationDictionary[settings.SettingsGuid].FolderMappingIsInitialized = emailCleanerSettings[settings.SettingsGuid].FolderMappingIsInitialized;
                }
                else
                {
                    configurationDictionary[settings.SettingsGuid].EmailCleanerEnabled = false;
                }
            }

            return configurationDictionary;
        }

        private async Task SaveEmailCleanerSettings(Dictionary<string, EmailCleanerInterfaceConfiguration> configurations)
        {
            await _applicationUnitOfWork.GenericSettingsRepository.SaveSettings(GenericSettigsModules.EmailCleanerInterfaceModuleName,
                   GenericSettigsModules.EmailCleanerInterfaceModuleType, _applicationUnitOfWork.CurrentUserId, configurations);
        }

        private async Task LoadEmailData(Dictionary<string, EmailCleanerInterfaceConfiguration> configurations, FolderMappings folderMappings, bool loadMails)
        {
            foreach (var key in configurations.Keys)
            {
                configurations[key] = await LoadEmailDataForConfiguration(configurations[key], folderMappings, loadMails, false);
            }
        }

        private async Task<EmailCleanerInterfaceConfiguration> LoadEmailDataForConfiguration(EmailCleanerInterfaceConfiguration configuration, FolderMappings folderMappings, bool loadMails, bool isFolderMappingData)
        {
            var currentEmailData = await folderMappings.GetMailsFromServer(configuration);

            if (!currentEmailData.Any())
            {
                return configuration;
            }

            var databaseHandler = new EmailDatabaseHandler(_applicationUnitOfWork);

            var addressDictionary = await databaseHandler.EnsureAllAddressEntitiesExists(currentEmailData);
            var subjectDictionary = await databaseHandler.EnsureAllSubjectEntitiesExists(currentEmailData);

            var mappingEntities = await databaseHandler.GetEmailMappingEntities(_applicationUnitOfWork.CurrentUserId);

            var mails = (from mail in currentEmailData
                         let addressId = addressDictionary[mail.FromAddress].Id
                         let subjectId = subjectDictionary[mail.Subject].Id
                         let mappingEntity = mappingEntities.FirstOrDefault(x => x.AddressId == addressId && x.SubjectId == subjectId)
                         select new EmailMappingModel
                         {
                             UserId = _applicationUnitOfWork.CurrentUserId,
                             MappingId = mappingEntity != null ? mappingEntity.Id : -1,
                             AddressEntityId = addressId,
                             SubjectEntityId = subjectId,
                             FromAddress = mail.FromAddress,
                             Subject = mail.Subject,
                             UserDefinedTargetFolder = mappingEntity != null ? mappingEntity.UserDefinedTargetFolder : "Unknown",
                             PredictedTargetFolder = mappingEntity != null ? mappingEntity.UserDefinedTargetFolder : "n/a",
                             UserDefinedAsSpam = mappingEntity != null ? mappingEntity.UserDefinedAsSpam : false,
                             PredictedAsSpam = mappingEntity != null ? mappingEntity.PredictedAsSpam : false
                         }).ToList();

            if (isFolderMappingData)
            {
                configuration.Emails = mails
                    .GroupBy(m => m.FromAddress.Split('@')[1])
                    .Select(grp => grp.First())
                    .ToList();
            }
            else
            {
                configuration.Emails = loadMails ? mails : new List<EmailMappingModel>();
            }

            configuration.UnmappedDomains = mails.Count(x => string.IsNullOrEmpty(x.UserDefinedTargetFolder)
                || string.IsNullOrEmpty(x.PredictedTargetFolder));

            return configuration;
        }

        private async Task HandleDisableEmailCleaner(Dictionary<string, EmailCleanerInterfaceConfiguration> configurations, string settingsGuid)
        {
            using(var folderMapping = new FolderMappings(_applicationUnitOfWork))
            {
                configurations[settingsGuid].EmailCleanerEnabled = false;
                configurations[settingsGuid].FolderMappingEnabled = false;
                configurations[settingsGuid].UseAiSpamPrediction = false;
                configurations[settingsGuid].UseAiTargetFolderPrediction = false;
                configurations[settingsGuid].FolderMappingEnabled = false;
                configurations[settingsGuid].FolderMappingIsInitialized = false;
                configurations[settingsGuid].UpdatedAt = DateTime.UtcNow.ToString("dd.MM.yyyy HH:mm");
                configurations[settingsGuid].UpdatedBy = _applicationUnitOfWork.CurrentUserName;

                await folderMapping.DeleteFolderMappings(_applicationUnitOfWork.CurrentUserId, settingsGuid);

                await SaveEmailCleanerSettings(configurations);
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
