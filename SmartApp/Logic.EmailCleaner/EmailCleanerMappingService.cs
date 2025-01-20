using Data.ContextAccessor.Interfaces;
using Data.Shared;
using Data.Shared.Logging;
using Data.Shared.Tools;
using Logic.EmailCleaner.Interfaces;
using Logic.EmailCleaner.Models;
using Logic.Shared.Interfaces;
using MailKit;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Shared.Enums;
using Shared.Models.Identity;


namespace Logic.EmailCleaner
{
    public class EmailCleanerMappingService : IEmailCleanerMappingService
    {
        private readonly IEmailClient _emailClient;
        private readonly ISettingsRepository _settingsRepository;
        private readonly PasswordHandler _passwordHandler;

        public EmailCleanerMappingService(
          ISettingsRepository settingsRepository,
          IEmailClient emailClient,
          IOptions<SecurityData> securityData)
        {
            _emailClient = emailClient;
            _settingsRepository = settingsRepository;
            _passwordHandler = new PasswordHandler(securityData);
        }


        public async Task<List<EmailMappingModel>> GetMappings(int accountId)
        {
            try
            {
                var userId = _settingsRepository.ClaimsAccessor.GetClaimsValue<int>("userId");

                if (userId == 0)
                {
                    throw new Exception("Could not load user id from claims!");
                }

                var accountEntity = await LoadAccount(accountId);

                if (accountEntity == null)
                {
                    throw new Exception($"Could not find account!");
                }

                var mappingEntities = await LoadMappingEntities(accountEntity.Id);

                if (mappingEntities == null || !mappingEntities.Any())
                {
                    return new List<EmailMappingModel>();
                }

                var emailDataEntities = await LoadEmailDataEntities();

                return (from mapping in mappingEntities
                        let emailData = emailDataEntities.First(x => x.Id == mapping.EmailDataId)
                        select new EmailMappingModel
                        {
                            Id = mapping.Id,
                            IsActive = mapping.IsActive,
                            AccountId = mapping.AccountId,
                            Action = mapping.Action,
                            EmailFolder = mapping.EmailFolder,
                            TargetFolder = mapping.TargetFolder,
                            Domain = string.IsNullOrWhiteSpace(emailData.FromAddress) || !emailData.FromAddress.Contains("@")
                                ? string.Empty
                                : emailData.FromAddress.Split("@")[1],
                            SourceAddress = emailData.FromAddress,
                            Subject = emailData.Subject,
                            IsSpam = mapping.IsSpam,
                            PredictedValue = mapping.PredictedValue,
                        }).ToList();

            }
            catch (Exception exception)
            {
                await _settingsRepository.LogRepository.AddMessage(new LogMessageEntity
                {
                    Message = "Could not load email address mappings!",
                    ExceptionMessage = exception.Message,
                    TimeStamp = DateTime.UtcNow,
                    MessageType = LogMessageTypeEnum.Error,
                    Module = nameof(EmailCleanerMappingService),
                });

                await _settingsRepository.SaveChanges();

                return new List<EmailMappingModel>();
            }
        }

        public async Task<bool> UpdateAllEmailAddressMappings(EmailAccountEntity account, List<string> folders)
        {
            try
            {
                foreach (var folder in folders)
                {
                    await UpdateEmailAddressMappingTable(account, folder);
                }

                return true;
            }
            catch (Exception exception)
            {
                await _settingsRepository.LogRepository.AddMessage(new LogMessageEntity
                {
                    Message = "Could not update email address mappings!",
                    ExceptionMessage = exception.Message,
                    TimeStamp = DateTime.UtcNow,
                    MessageType = LogMessageTypeEnum.Error,
                    Module = nameof(EmailCleanerMappingService),
                });

                await _settingsRepository.SaveChanges();

                return false;
            }
        }

        public async Task<bool> UpdateMappingEntries(List<EmailMappingModel> mappingEntries)
        {
            var mappingIds = mappingEntries.Select(x => x.Id).ToList();

            var mappingUpdateDictionary = mappingEntries.ToDictionary(x => x.Id);

            var entitiesToUpdate = await _settingsRepository.EmailAddressMappingRepository.GetAll(x => mappingIds.Contains(x.Id)) ?? new List<EmailAddressMappingEntity>();

            if (!entitiesToUpdate.Any())
            {
                return false;
            }

            foreach (var entity in entitiesToUpdate)
            {
                entity.IsActive = mappingUpdateDictionary[entity.Id].IsActive;
                entity.IsSpam = mappingUpdateDictionary[entity.Id].IsSpam;
                entity.TargetFolder = mappingUpdateDictionary[entity.Id].TargetFolder;
                entity.PredictedValue = mappingUpdateDictionary[entity.Id].PredictedValue;
                entity.Action = mappingUpdateDictionary[entity.Id].Action;

                await _settingsRepository.EmailAddressMappingRepository.AddOrUpdate(entity, x => x.Id == entity.Id);
            }

            await _settingsRepository.SaveChanges();

            return true;
        }

        #region private members

        private async Task<bool> UpdateEmailAddressMappingTable(EmailAccountEntity account, string folder)
        {
            try
            {
                if (account == null)
                {
                    throw new Exception("Could not initialize mappings, reason account is null!");
                }

                await LoadEmailAccountSettings(account.SettingsId);

                if (account.EmailCleanerSettings == null || !account.EmailCleanerSettings.EmailCleanerEnabled)
                {
                    return false;
                }

                var dataEntities = await LoadEmailDataEntities();

                var emailMetaDataEntityDataDictionary = GetEmailMetaDataEntityDataDictionary(dataEntities);

                var activeFolders = (folder == null
                    ? JsonConvert.DeserializeObject<List<FolderSettings>>(account.EmailCleanerSettings.FolderConfigurationJson)
                    ?.Where(f => f.IsInbox)?.Select(f => f.FolderName) ?? new List<string>()
                    : new List<string> { folder }).ToList();

                var messageSummaries = await LoadEnvalopeData(account, activeFolders);

                if (!messageSummaries.Any())
                {
                    return false;
                }

                // ensure all email data exists in database
                await EnsureEmailDataExists(messageSummaries, dataEntities, emailMetaDataEntityDataDictionary);

                var existingMappingEntities = await LoadMappingEntities(account.Id);

                var emailDataLookup = GetEmailMetaDataEntityDataDictionary(dataEntities).Values.ToLookup(x => x.Id);

                var existingMappingLookupTable = existingMappingEntities.ToLookup(mapping => $"[{emailDataLookup[mapping.EmailDataId].First().FromAddress}{emailDataLookup[mapping.EmailDataId].First().Subject}]");

                var newMappingEntities = new List<EmailAddressMappingEntity>();

                foreach (var summary in messageSummaries)
                {
                    if (!existingMappingLookupTable.Any(x => x.Key == $"[{summary.Envelope.From.Mailboxes.First().Address}{summary.Envelope.Subject}]"))
                    {
                        var dataEntity = emailDataLookup.SelectMany(x =>
                                x.Where(y => y.FromAddress == summary.Envelope.From.Mailboxes.First().Address
                                             && y.Subject == summary.Envelope.Subject)).FirstOrDefault();

                        if (dataEntity != null)
                        {
                            newMappingEntities.Add(new EmailAddressMappingEntity
                            {
                                AccountId = account.Id,
                                IsActive = false,
                                Action = (int)EmailCleanerAction.Ignore,
                                EmailData = dataEntity,
                                EmailFolder = summary.Folder.Name,
                                TargetFolder = string.Empty,
                                IsSpam = false,
                                PredictedValue = null,
                            });
                        }
                    }
                }

                if (newMappingEntities.Any())
                {
                    await _settingsRepository.EmailAddressMappingRepository.AddRange(newMappingEntities);

                    await _settingsRepository.EmailAddressMappingRepository.SaveChanges();
                }

                return true;
            }
            catch (Exception exception)
            {
                await _settingsRepository.LogRepository.AddMessage(new LogMessageEntity
                {
                    Message = $"Could not update email address mappings of folder [{folder}]!",
                    ExceptionMessage = exception.Message,
                    TimeStamp = DateTime.UtcNow,
                    MessageType = LogMessageTypeEnum.Error,
                    Module = nameof(EmailCleanerMappingService),
                });

                await _settingsRepository.SaveChanges();

                return false;
            }
        }

        private async Task<EmailAccountEntity?> LoadAccount(int accountId)
        {
            return await _settingsRepository.EmailAccountRepository.GetSingle(x => x.Id == accountId);
        }

        private async Task<List<EmailDataEntity>> LoadEmailDataEntities()
        {
            return await _settingsRepository.EmailDataRepository.GetAllAsync();
        }

        private async Task<List<EmailAddressMappingEntity>> LoadMappingEntities(int accountId)
        {
            return await _settingsRepository.EmailAddressMappingRepository.GetAll(x => x.AccountId == accountId, true) ?? new List<EmailAddressMappingEntity>();
        }

        private Dictionary<string, EmailDataEntity> GetEmailMetaDataEntityDataDictionary(List<EmailDataEntity> dataEntities)
        {
            return dataEntities.GroupBy(x => new { x.FromAddress, x.Subject })
                    .Select(grp => grp.First())
                    .ToDictionary(x => $"[{x.FromAddress}{x.Subject}]");
        }

        private async Task LoadEmailAccountSettings(int? settingsId)
        {
            if (settingsId == null) { throw new Exception("Could not load email account settings, Reason: settings id is null!"); }

            await _settingsRepository.EmailCleanerSettingsRepository.GetSingle(x => x.Id == settingsId);
        }

        private async Task<List<IMessageSummary>> LoadEnvalopeData(EmailAccountEntity account, List<string>? foldernames = null)
        {
            var messageSummaries = new List<IMessageSummary>();
            if (await _emailClient.IsConnectedToServer(account.Server, account.Port)
                && await _emailClient.IsAuthenticated(account.EmailAddress, _passwordHandler.Decrypt(account.EncodedPassword)))
            {
                messageSummaries.AddRange(await _emailClient.LoadEnvelopeData(MessageSummaryItems.Envelope, foldernames));

                await _emailClient.Disconnect();
            }

            return messageSummaries;
        }

        private async Task EnsureEmailDataExists(List<IMessageSummary> messageSummaries, List<EmailDataEntity> dataEntities, Dictionary<string, EmailDataEntity> emailMetaDataEntityDataDictionary)
        {
            var newEntities = new List<EmailDataEntity>();

            foreach (var summary in messageSummaries)
            {
                if (!emailMetaDataEntityDataDictionary.ContainsKey($"[{summary.Envelope.From.Mailboxes.First().Address}{summary.Envelope.Subject}]"))
                {
                    newEntities.Add(new EmailDataEntity
                    {
                        FromAddress = summary.Envelope.From.Mailboxes.First().Address,
                        Subject = summary.Envelope.Subject,
                    });
                }

            }

            if (newEntities.Any())
            {
                await _settingsRepository.EmailDataRepository.AddRange(newEntities);

                await _settingsRepository.EmailDataRepository.SaveChanges();

                dataEntities.AddRange(newEntities);
            }
        }

        #endregion
    }
}
