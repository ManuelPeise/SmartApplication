using Data.ContextAccessor.Interfaces;
using Data.Shared.Logging;
using Data.Shared.Tools;
using Data.Shared;
using Logic.EmailCleaner.Interfaces;
using Microsoft.Extensions.Options;
using Shared.Enums;
using Shared.Models.Identity;
using Logic.EmailCleaner.Models;
using Newtonsoft.Json;
using MessageLog = Logic.EmailCleaner.Models.MessageLog;
using Logic.Shared.Interfaces;
using Shared.Models.Settings.EmailAccountMappings;


namespace Logic.EmailCleaner
{
    public class EmailCleanerService : IEmailCleanerService
    {
        private readonly IOptions<SecurityData> _securityData;
        private readonly IEmailClient _emailClient;
        private readonly ISettingsRepository _settingsRepository;
        private readonly IEmailCleanerMappingService _mappingService;

        public EmailCleanerService(ISettingsRepository settingsRepository, IEmailClient emailClient, IEmailCleanerMappingService mappingService, IOptions<SecurityData> securityData)
        {
            _securityData = securityData;
            _emailClient = emailClient;
            _settingsRepository = settingsRepository;
            _mappingService = mappingService;
        }

        public async Task<List<EmailAccountModel>> GetSettings()
        {
            try
            {
                var userId = _settingsRepository.ClaimsAccessor.GetClaimsValue<int>("userId");

                if (userId == 0)
                {
                    throw new Exception("Could not load user id from claims!");
                }

                var allEntities = await _settingsRepository.EmailAccountRepository.GetAllAsync() ?? new List<EmailAccountEntity>();

                var entities = allEntities.Where(x => x.UserId == userId);

                if (!entities.Any())
                {
                    return new List<EmailAccountModel>();

                }

                var models = new List<EmailAccountModel>();
                var passwordHandler = new PasswordHandler(_securityData);

                foreach (var entity in entities)
                {
                    if (entity == null)
                    {
                        continue;
                    }

                    var settingsEntity = await _settingsRepository.EmailCleanerSettingsRepository.GetFirstOrDefault(x => x.Id == entity.SettingsId);

                    if (settingsEntity == null)
                    {
                        models.Add(new EmailAccountModel
                        {
                            Id = entity.Id,
                            UserId = entity.UserId,
                            AccountName = entity.AccountName,
                            ProviderType = entity.ProviderType,
                            Server = entity.Server,
                            Port = entity.Port,
                            EmailAddress = entity.EmailAddress,
                            Password = null,
                            ConnectionTestPassed = entity.ConnectionTestPassed,
                            MessageLog = GetDeserializedModel<MessageLog>(entity.MessageLogJson) ?? null,
                            EmailAddressMappings = new List<EmailMappingModel>(),
                            Settings = new EmailCleanerSettings()
                        });

                        continue;
                    }

                    if (settingsEntity.EmailCleanerEnabled)
                    {
                        await UpdateFolders(settingsEntity, entity.Server, entity.Port, entity.EmailAddress, passwordHandler.Decrypt(entity.EncodedPassword));

                        _settingsRepository.EmailCleanerSettingsRepository.Update(settingsEntity);

                        await _settingsRepository.EmailCleanerSettingsRepository.SaveChangesAsync();
                    }

                    var emailAddressMappings = await _mappingService.GetMappings(entity.Id);

                    var model = new EmailAccountModel
                    {
                        Id = entity.Id,
                        UserId = entity.UserId,
                        AccountName = entity.AccountName,
                        ProviderType = entity.ProviderType,
                        Server = entity.Server,
                        Port = entity.Port,
                        EmailAddress = entity.EmailAddress,
                        Password = null,
                        ConnectionTestPassed = entity.ConnectionTestPassed,
                        MessageLog = GetDeserializedModel<MessageLog>(entity.MessageLogJson) ?? null,
                        EmailAddressMappings = emailAddressMappings,
                        Settings = new EmailCleanerSettings
                        {
                            SettingsId = settingsEntity.Id,
                            EmailCleanerEnabled = settingsEntity.EmailCleanerEnabled,
                            EmailCleanerAiEnabled = settingsEntity.EmailCleanerAiEnabled,
                            IsAgreed = settingsEntity.IsAgreed,
                            FolderConfiguration = JsonConvert.DeserializeObject<List<FolderSettings>>(settingsEntity.FolderConfigurationJson) ?? new List<FolderSettings>(),
                            MessageLog = JsonConvert.DeserializeObject<MessageLog>(settingsEntity.MessageLogJson) ?? new MessageLog()
                        }
                    };

                    models.Add(model);
                }


                return models;
            }
            catch (Exception exception)
            {
                await _settingsRepository.LogMessageRepository.AddAsync(new LogMessageEntity
                {
                    Message = "Could not load email account settings.",
                    ExceptionMessage = exception.Message,
                    TimeStamp = DateTime.UtcNow,
                    MessageType = LogMessageTypeEnum.Error,
                    Module = nameof(EmailCleanerService),
                });

                await _settingsRepository.LogMessageRepository.SaveChangesAsync();

                return new List<EmailAccountModel>();
            }

        }

        public async Task<List<FolderSettings>> GetUpdatedFolderSettings(int accountId)
        {
            try
            {
                var userId = _settingsRepository.ClaimsAccessor.GetClaimsValue<int>("userId");

                if (userId == 0)
                {
                    throw new Exception("Could not load user id from claims!");
                }

                var entitiy = await _settingsRepository.EmailAccountRepository.GetFirstOrDefault(x => x.Id == accountId);

                if (entitiy != null)
                {
                    await _settingsRepository.EmailCleanerSettingsRepository.GetFirstOrDefault(x => x.Id == entitiy.SettingsId);

                    var passwordHandler = new PasswordHandler(_securityData);

                    if (entitiy.EmailCleanerSettings == null)
                    {
                        throw new Exception("Could not update email folders!");
                    }

                    await UpdateFolders(entitiy.EmailCleanerSettings, entitiy.Server, entitiy.Port, entitiy.EmailAddress, passwordHandler.Decrypt(entitiy.EncodedPassword));

                    return JsonConvert.DeserializeObject<List<FolderSettings>>(entitiy.EmailCleanerSettings.FolderConfigurationJson) ?? new List<FolderSettings>();

                }

                return new List<FolderSettings>();
            }
            catch (Exception exception)
            {
                await _settingsRepository.LogMessageRepository.AddAsync(new LogMessageEntity
                {
                    Message = $"Could not get email folders from account.",
                    ExceptionMessage = exception.Message,
                    TimeStamp = DateTime.UtcNow,
                    MessageType = LogMessageTypeEnum.Error,
                    Module = nameof(EmailCleanerService),
                });

                await _settingsRepository.LogMessageRepository.SaveChangesAsync();

                return new List<FolderSettings>();
            }
        }

        public async Task<bool> TestConnection(ConnectionTestModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Password))
                {
                    throw new Exception("Cannot test provider connection, missing password!");
                }

                var userId = _settingsRepository.ClaimsAccessor.GetClaimsValue<int>("userId");
                var userName = _settingsRepository.ClaimsAccessor.GetClaimsValue<string>("name");

                if (userId == 0 || string.IsNullOrWhiteSpace(userName))
                {
                    throw new Exception("Could not load user id from claims!");
                }

                if (await _emailClient.IsConnectedToServer(model.Server, model.Port) && await _emailClient.IsAuthenticated(model.EmailAddress, model.Password))
                {
                    await _emailClient.Disconnect();

                    return true;
                }

                return false;
            }
            catch (Exception exception)
            {
                await _settingsRepository.LogMessageRepository.AddAsync(new LogMessageEntity
                {
                    Message = $"Could not test email account connection.",
                    ExceptionMessage = exception.Message,
                    TimeStamp = DateTime.UtcNow,
                    MessageType = LogMessageTypeEnum.Error,
                    Module = nameof(EmailCleanerService),
                });

                await _settingsRepository.LogMessageRepository.SaveChangesAsync();

                return false;
            }
        }

        public async Task<bool> SaveAccount(EmailAccountModel model)
        {
            try
            {
                var userId = _settingsRepository.ClaimsAccessor.GetClaimsValue<int>("userId");
                var userName = _settingsRepository.ClaimsAccessor.GetClaimsValue<string>("name");

                if (userId == 0 || string.IsNullOrWhiteSpace(userName))
                {
                    throw new Exception("Could not load user id from claims!");
                }

                var entity = await _settingsRepository.EmailAccountRepository.GetFirstOrDefault(x => x.EmailAddress.ToLower() == model.EmailAddress.ToLower(), true);

                var passwordHandler = new PasswordHandler(_securityData);

                if (entity == null)
                {
                    var folderNames = await LoadFolders(model.Server, model.Port, model.EmailAddress, model.Password);

                    entity = new EmailAccountEntity
                    {
                        UserId = userId,
                        AccountName = model.AccountName,
                        ProviderType = model.ProviderType,
                        Server = model.Server,
                        Port = model.Port,
                        EmailAddress = model.EmailAddress,
                        EncodedPassword = passwordHandler.Encrypt(model.Password),
                        ConnectionTestPassed = model.ConnectionTestPassed,
                        MessageLogJson = GetLogMessageJson(userName),
                        EmailCleanerSettings = new EmailCleanerSettingsEntity
                        {
                            EmailCleanerEnabled = false,
                            EmailCleanerAiEnabled = false,
                            IsAgreed = false,
                            MessageLogJson = GetLogMessageJson(userName),
                            FolderConfigurationJson = JsonConvert.SerializeObject(from folder in folderNames
                                                                                  select new FolderSettings
                                                                                  {
                                                                                      FolderId = Guid.NewGuid(),
                                                                                      FolderName = folder,
                                                                                      IsInbox = false
                                                                                  }),
                        }
                    };

                    if (await _settingsRepository.EmailAccountRepository.AddIfNotExists(entity, x => x.Id == entity.Id && x.UserId == entity.UserId))
                    {
                        await _settingsRepository.EmailAccountRepository.SaveChangesAsync();

                        await _mappingService.UpdateAllEmailAddressMappings(entity, folderNames);

                        return true;
                    }
                }

                return false;
            }
            catch (Exception exception)
            {
                await _settingsRepository.LogMessageRepository.AddAsync(new LogMessageEntity
                {
                    Message = $"Could update entity [{model.Id}].",
                    ExceptionMessage = exception.Message,
                    TimeStamp = DateTime.UtcNow,
                    MessageType = LogMessageTypeEnum.Error,
                    Module = nameof(EmailCleanerService),
                });

                await _settingsRepository.LogMessageRepository.SaveChangesAsync();

                return false;
            }
        }

        public async Task<bool> UpdateAccount(EmailAccountModel model)
        {
            try
            {
                var userId = _settingsRepository.ClaimsAccessor.GetClaimsValue<int>("userId");
                var userName = _settingsRepository.ClaimsAccessor.GetClaimsValue<string>("name");

                if (userId == 0 || string.IsNullOrWhiteSpace(userName))
                {
                    throw new Exception("Could not load user id from claims!");
                }

                var entity = await _settingsRepository.EmailAccountRepository.GetFirstOrDefault(x => x.EmailAddress.ToLower() == model.EmailAddress.ToLower(), true);

                if (entity == null)
                {
                    throw new Exception("Could not update email account connection!");
                }

                var passwordHandler = new PasswordHandler(_securityData);

                var timeStamp = DateTime.UtcNow.ToString("dd.MM.yyyy - HH:mm");

                entity = new EmailAccountEntity
                {
                    Id = model.Id,
                    UserId = model.UserId,
                    AccountName = model.AccountName,
                    ProviderType = model.ProviderType,
                    Server = model.Server,
                    Port = model.Port,
                    EmailAddress = model.EmailAddress,
                    EncodedPassword = model.Password == null ? entity.EncodedPassword : passwordHandler.Encrypt(model.Password),
                    ConnectionTestPassed = model.ConnectionTestPassed,
                    MessageLogJson = JsonConvert.SerializeObject(new MessageLog
                    {
                        User = userName,
                        TimeStamp = timeStamp
                    }),
                    SettingsId = entity.SettingsId,

                };

                _settingsRepository.EmailAccountRepository.Update(entity);

                await _settingsRepository.EmailAccountRepository.SaveChangesAsync();

                return true;

            }
            catch (Exception exception)
            {
                await _settingsRepository.LogMessageRepository.AddAsync(new LogMessageEntity
                {
                    Message = "Could not load email account settings.",
                    ExceptionMessage = exception.Message,
                    TimeStamp = DateTime.UtcNow,
                    MessageType = LogMessageTypeEnum.Error,
                    Module = nameof(EmailCleanerService),
                });

                await _settingsRepository.LogMessageRepository.SaveChangesAsync();

                return false;
            }
        }

        public async Task<bool> UpdateSettings(EmailCleanerSettings model)
        {
            try
            {
                var userId = _settingsRepository.ClaimsAccessor.GetClaimsValue<int>("userId");
                var userName = _settingsRepository.ClaimsAccessor.GetClaimsValue<string>("name");

                if (userId == 0 || string.IsNullOrWhiteSpace(userName))
                {
                    throw new Exception("Could not load user id from claims!");
                }

                var accountEntity = await _settingsRepository.EmailAccountRepository.GetFirstOrDefault(x => x.UserId == userId);

                if (accountEntity == null)
                {
                    return false;
                }

                var settingsEntity = await _settingsRepository.EmailCleanerSettingsRepository.GetFirstOrDefault(x => x.Id == model.SettingsId, true);

                if (settingsEntity != null)
                {
                    settingsEntity.EmailCleanerEnabled = model.EmailCleanerEnabled;
                    settingsEntity.EmailCleanerAiEnabled = model.EmailCleanerAiEnabled;
                    settingsEntity.IsAgreed = model.IsAgreed;
                    settingsEntity.FolderConfigurationJson = JsonConvert.SerializeObject(model.FolderConfiguration);
                    settingsEntity.MessageLogJson = GetLogMessageJson(userName);

                    _settingsRepository.EmailCleanerSettingsRepository.Update(settingsEntity);

                    await _settingsRepository.EmailCleanerSettingsRepository.SaveChangesAsync();

                    await _mappingService.UpdateAllEmailAddressMappings(accountEntity,
                        model.FolderConfiguration.Where(folder => folder.IsInbox).Select(folder => folder.FolderName).ToList());

                    return true;

                }

                return false;
            }
            catch (Exception exception)
            {
                await _settingsRepository.LogMessageRepository.AddAsync(new LogMessageEntity
                {
                    Message = $"Could update email cleaner general settings [{model.SettingsId}].",
                    ExceptionMessage = exception.Message,
                    TimeStamp = DateTime.UtcNow,
                    MessageType = LogMessageTypeEnum.Error,
                    Module = nameof(EmailCleanerService),
                });

                await _settingsRepository.LogMessageRepository.SaveChangesAsync();

                return false;
            }
        }

        //public async Task<bool> UpdateEmailAddressMappings(EmailMappingModel model)
        //{
        //    try
        //    {
        //        var userId = _settingsRepository.ClaimsAccessor.GetClaimsValue<int>("userId");
        //        var userName = _settingsRepository.ClaimsAccessor.GetClaimsValue<string>("name");

        //        if (userId == 0 || string.IsNullOrWhiteSpace(userName))
        //        {
        //            throw new Exception("Could not load user id from claims!");
        //        }



        //        var entity = await _settingsRepository.EmailAccountRepository.GetSingle(x => x.Id == model.AccountId, true);

        //        if (entity == null)
        //        {
        //            throw new Exception("Could not load account entity to update email address mappings!");
        //        }

        //        return await _mappingService.UpdateMappingEntries(models.);

        //    }
        //    catch (Exception exception)
        //    {
        //        await _settingsRepository.LogRepository.AddMessage(new LogMessageEntity
        //        {
        //            Message = $"Could update email cleaner email address mappings.",
        //            ExceptionMessage = exception.Message,
        //            TimeStamp = DateTime.UtcNow,
        //            MessageType = LogMessageTypeEnum.Error,
        //            Module = nameof(EmailCleanerService),
        //        });

        //        await _settingsRepository.SaveChanges();

        //        return false;
        //    }
        //}

        public async Task<bool> UpdateEmailAddressMappingEntries(List<EmailMappingModel> mappingEntries)
        {
            try
            {
                var userId = _settingsRepository.ClaimsAccessor.GetClaimsValue<int>("userId");
                var userName = _settingsRepository.ClaimsAccessor.GetClaimsValue<string>("name");

                if (userId == 0 || string.IsNullOrWhiteSpace(userName))
                {
                    throw new Exception("Could not load user id from claims!");
                }

                return await _mappingService.UpdateMappingEntries(mappingEntries);
            }
            catch (Exception exception)
            {
                await _settingsRepository.LogMessageRepository.AddAsync(new LogMessageEntity
                {
                    Message = $"Could update email cleaner email address mappings entries.",
                    ExceptionMessage = exception.Message,
                    TimeStamp = DateTime.UtcNow,
                    MessageType = LogMessageTypeEnum.Error,
                    Module = nameof(EmailCleanerService),
                });

                await _settingsRepository.LogMessageRepository.SaveChangesAsync();

                return false;
            }
        }

        #region private members

        private async Task UpdateFolders(EmailCleanerSettingsEntity settings, string server, int port, string email, string password)
        {
            var folders = await LoadFolders(server, port, email, password);

            var existingFolders = JsonConvert.DeserializeObject<List<FolderSettings>>(settings.FolderConfigurationJson) ?? new List<FolderSettings>();

            var existingFolderNames = existingFolders.Select(x => x.FolderName).ToList();

            if (folders.Any())
            {
                var foldersToRemove = existingFolderNames.FindAll(folder => !folders.Contains(folder));

                existingFolders = existingFolders
                    .Where(x => !foldersToRemove.Contains(x.FolderName.ToLower())).ToList();

                folders.ForEach(folder =>
                {
                    var existingFolder = existingFolders.FirstOrDefault(x => x.FolderName.ToLower() == folder.ToLower());

                    if (existingFolder == null)
                    {
                        existingFolders.Add(new FolderSettings
                        {
                            FolderId = Guid.NewGuid(),
                            FolderName = folder,
                            IsInbox = false
                        });
                    }
                });

                settings.FolderConfigurationJson = JsonConvert.SerializeObject(existingFolders);
            }


        }

        private async Task<List<string>> LoadFolders(string server, int port, string email, string password)
        {
            var folders = new List<string>();

            if (await _emailClient.IsConnectedToServer(server, port) && await _emailClient.IsAuthenticated(email, password))
            {
                folders = await _emailClient.GetEmailFolders();

                await _emailClient.Disconnect();
            }

            return folders;
        }

        private string GetLogMessageJson(string userName)
        {
            return JsonConvert.SerializeObject(new MessageLog
            {
                User = userName,
                TimeStamp = DateTime.UtcNow.ToString("dd.MM.yyyy - HH:mm")
            });
        }

        private static T? GetDeserializedModel<T>(string? json)
        {
            if (string.IsNullOrWhiteSpace(json)) { return default(T); }

            return JsonConvert.DeserializeObject<T>(json);
        }

        #endregion
    }
}
