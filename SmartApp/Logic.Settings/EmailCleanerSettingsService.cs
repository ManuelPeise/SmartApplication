using Data.ContextAccessor;
using Data.ContextAccessor.Interfaces;
using Data.Shared.Logging;
using Data.Shared.Tools;
using Logic.Settings.Interfaces;
using Shared.Enums;
using Shared.Models.Settings.EmailCleanerSettings;


namespace Logic.Settings
{
    public class EmailCleanerSettingsService : IEmailCleanerSettingsService
    {
        private readonly ISettingsRepository _settingsRepository;

        public EmailCleanerSettingsService(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        public async Task<EmailCleanerConfiguration?> GetSettings(bool loadMappings)
        {
            var configuration = new EmailCleanerConfiguration();

            try
            {
                LoadCurrentUserId(out var userId);

                if (userId == -1)
                {
                    throw new Exception("Could not load user id from claims!");
                }
                else
                {
                    configuration.UserId = userId;
                }

                var emailAccountEntities = await _settingsRepository.EmailAccountRepository.GetAll(x => x.UserId == configuration.UserId);

                if (emailAccountEntities == null)
                {
                    return null;
                }

                await LoadEmailAccounts(configuration);

                if (!configuration.Accounts.Any())
                {
                    return null;
                }

                await LoadCleanupSettings(emailAccountEntities, loadMappings);


                return LoadConfigurationExportModel(emailAccountEntities, configuration);
            }
            catch (Exception exception)
            {
                await _settingsRepository.LogRepository.AddMessage(new LogMessageEntity
                {
                    Message = "Could not load email cleaner settings.",
                    ExceptionMessage = exception.Message,
                    TimeStamp = DateTime.UtcNow,
                    MessageType = LogMessageTypeEnum.Error,
                    Module = nameof(EmailCleanerConfiguration),
                });

                await _settingsRepository.SaveChanges();
            }

            return configuration;
        }

        public async Task<bool> SaveOrUpdateEmailCleanerSettings(EmailCleanerSettings settings)
        {
            try
            {
                LoadCurrentUserId(out var userId);

                if (userId == -1)
                {
                    throw new Exception("Could not load user id from claims!");
                }

                var emailSettingsEntity = await _settingsRepository.EmailCleanerSettingsRepository.GetSingle(x => x.UserId == settings.UserId && x.Id == settings.SettingsId);

                if (emailSettingsEntity == null)
                {
                    return false;
                }

                UpdateSettingsEntity(emailSettingsEntity, settings);


                var updatedEntits = await _settingsRepository.EmailCleanerSettingsRepository.AddOrUpdate(emailSettingsEntity, x => x.Id == emailSettingsEntity.Id && x.UserId == emailSettingsEntity.UserId);

                if (updatedEntits != null)
                {
                    await _settingsRepository.EmailCleanerSettingsRepository.SaveChanges();

                    return true;
                }

                return false;
            }
            catch (Exception exception)
            {
                await _settingsRepository.LogRepository.AddMessage(new LogMessageEntity
                {
                    Message = "Could not save or update email cleaner settings.",
                    ExceptionMessage = exception.Message,
                    TimeStamp = DateTime.UtcNow,
                    MessageType = LogMessageTypeEnum.Error,
                    Module = nameof(EmailCleanerConfiguration),
                });

                await _settingsRepository.SaveChanges();

                return false;
            }
        }


        #region private members

        private async Task LoadEmailCleanupSettings(
            RepositoryBase<EmailCleanerSettingsEntity> emailCleanerSettingsRepository,
            RepositoryBase<EmailAddressMappingEntity> emailAddressMappingRepository,
            int emailCleanerSettingsId, bool loadMappings)
        {
            var settingsEntity = await emailCleanerSettingsRepository.GetSingle(x => x.Id == emailCleanerSettingsId);

            if (loadMappings)
            {
                await LoadEmailAddressMappings(emailAddressMappingRepository, settingsEntity.Id);
            }
        }

        private async Task LoadEmailAddressMappings(RepositoryBase<EmailAddressMappingEntity> emailAddressMappingRepository, int settingsId)
        {
            await emailAddressMappingRepository.GetAll(x => x.EmailCleanerSettingsId == settingsId);
        }

        private void LoadCurrentUserId(out int userId)
        {
            userId = _settingsRepository.ClaimsAccessor.GetClaimsValue<int>("userId");
        }

        private async Task LoadEmailAccounts(EmailCleanerConfiguration configuration)
        {
            var emailAccountEntities = await _settingsRepository.EmailAccountRepository.GetAll(x => x.UserId == configuration.UserId);

            if (emailAccountEntities == null || !emailAccountEntities.Any())
            {
                configuration.Accounts = new List<EmailCleanerAccount>();

                return;
            }

            configuration.Accounts = emailAccountEntities
                    .Select(x => new EmailCleanerAccount { Id = x.Id, AccountName = x.AccountName }).ToList();
        }

        private async Task LoadCleanupSettings(List<EmailAccountEntity> emailAccountEntities, bool loadMappings)
        {
            var databaseChanged = false;

            foreach (var emailAccountEntity in emailAccountEntities)
            {
                if (emailAccountEntity.EmailCleanerSettingsId == null)
                {
                    emailAccountEntity.EmailCleanerSettings = new EmailCleanerSettingsEntity
                    {
                        UserId = emailAccountEntity.UserId,
                        Enabled = false,
                        Account = emailAccountEntity.EmailAddress,
                        AccountName = emailAccountEntity.AccountName,
                        AllowReadEmails = false,
                        AllowMoveEmails = false,
                        AllowDeleteEmails = false,
                        AllowCreateEmailFolder = false,
                        ShareEmailDataToTrainAi = false,
                        ScheduleCleanup = false,
                        ScheduleCleanupAtHour = 0,
                        LastCleanupTime = null,
                        NextCleanupTime = null,
                        EmailAddressMappings = new List<EmailAddressMappingEntity>()
                    };

                    await _settingsRepository.EmailAccountRepository.AddOrUpdate(emailAccountEntity, x => x.Id == emailAccountEntity.Id);

                    databaseChanged = true;
                }
                else
                {

                    await LoadEmailCleanupSettings(_settingsRepository.EmailCleanerSettingsRepository,
                       _settingsRepository.EmailAddressMappingRepository,
                       (int)emailAccountEntity.EmailCleanerSettingsId,
                       loadMappings);
                }
            }

            if (databaseChanged)
            {
                await _settingsRepository.EmailAccountRepository.SaveChanges();
            }
        }

        private EmailCleanerConfiguration LoadConfigurationExportModel(List<EmailAccountEntity> emailAccountEntities, EmailCleanerConfiguration configuration)
        {
            configuration.Settings = (from entity in emailAccountEntities
                                      select new EmailCleanerSettings
                                      {
                                          UserId = entity.UserId,
                                          SettingsId = entity.EmailCleanerSettings?.Id ?? -1,
                                          AllowReadEmails = entity.EmailCleanerSettings?.AllowReadEmails ?? false,
                                          AllowDeleteEmails = entity.EmailCleanerSettings?.AllowDeleteEmails ?? false,
                                          AllowCreateEmailFolder = entity.EmailCleanerSettings?.AllowCreateEmailFolder ?? false,
                                          AllowMoveEmails = entity.EmailCleanerSettings?.AllowMoveEmails ?? false,
                                          ShareEmailDataToTrainAi = entity.EmailCleanerSettings?.ShareEmailDataToTrainAi ?? false,
                                          Enabled = entity.EmailCleanerSettings?.Enabled ?? false,
                                          ScheduleCleanup = entity.EmailCleanerSettings?.ScheduleCleanup ?? false,
                                          ScheduleCleanupAtHour = entity.EmailCleanerSettings?.ScheduleCleanupAtHour ?? 0,
                                          LastCleanupTime = entity.EmailCleanerSettings?.LastCleanupTime ?? null,
                                          HasMappings = entity.EmailCleanerSettings?.EmailAddressMappings.Any() ?? false,
                                          Mappings = (from mapping in entity.EmailCleanerSettings?.EmailAddressMappings
                                                      select new EmailAddressMapping
                                                      {
                                                          SourceAddress = mapping.SourceAddress,
                                                          Domain = mapping.Domain,
                                                          ShouldCleanup = mapping.ShouldCleanup,
                                                          IsSpam = mapping.IsSpam,
                                                          PredictedAs = mapping.PredictedAs
                                                      }).ToList(), 
                                      }).ToList();

            return configuration;
        }

        private void UpdateSettingsEntity(EmailCleanerSettingsEntity emailSettingsEntity, EmailCleanerSettings settings)
        {
            emailSettingsEntity.Enabled = settings.Enabled;
            emailSettingsEntity.AllowReadEmails = settings.AllowReadEmails;
            emailSettingsEntity.AllowCreateEmailFolder = settings.AllowCreateEmailFolder;
            emailSettingsEntity.AllowMoveEmails = settings.AllowMoveEmails;
            emailSettingsEntity.AllowDeleteEmails = settings.AllowDeleteEmails;
            emailSettingsEntity.ScheduleCleanup = settings.ScheduleCleanup;
            emailSettingsEntity.ScheduleCleanupAtHour = settings.ScheduleCleanupAtHour;
            emailSettingsEntity.ShareEmailDataToTrainAi = settings.ShareEmailDataToTrainAi;
        }


        #endregion
    }
}
