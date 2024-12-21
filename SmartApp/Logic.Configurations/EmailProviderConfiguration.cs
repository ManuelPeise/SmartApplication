using Data.Shared.Logging;
using Data.Shared.Settings;
using Logic.Shared;
using Logic.Shared.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Shared.Enums;
using Shared.Models.Administration.Email;
using Shared.Models.Identity;

namespace Logic.Interfaces
{
    public class EmailProviderConfiguration : IEmailProviderConfiguration
    {
        private readonly IAdministrationUnitOfWork _administrationUnitOfWork;
        private readonly IEmailClient _emailClient;
        private readonly IOptions<SecurityData> _securityData;

        private bool disposedValue;

        public EmailProviderConfiguration(IAdministrationUnitOfWork administrationUnitOfWork, IEmailClient emailClient, IOptions<SecurityData> securityData)
        {
            _administrationUnitOfWork = administrationUnitOfWork;
            _emailClient = emailClient;
            _securityData = securityData;
        }

        public async Task<List<EmailProviderSettings>> GetSettings()
        {
            var defaultProviderSettings = new List<EmailProviderSettings>
            {
                new EmailProviderSettings{
                    Id = 0,
                    Name = "",
                    Provider = new EmailProvider
                    {
                        ProviderType = EmailProviderTypeEnum.None,
                        IMapServerAddress = "",
                        ImapPort = null,
                        Logo = "",
                        DisplayName = "None"
                    },
                    EmailAddress = "",
                    Password = "",
                    Status = EmailProviderConfigurationStatusEnum.Pending,
                    ConnectionTestPassed = false,
                    ConnectionInfo = null,
                }
            };

            try
            {
                var userId = _administrationUnitOfWork.GetValueFromClaims<int>("userId");

                if (userId == 0)
                {
                    await _administrationUnitOfWork.AddLogMessage(new LogMessageEntity
                    {
                        Message = "Could not load email provider settings, reason: invalid user id!",
                        ExceptionMessage = string.Empty,
                        TimeStamp = DateTime.UtcNow,
                        MessageType = LogMessageTypeEnum.Error,
                        Module = nameof(EmailProviderConfiguration),
                    });
                    
                    return defaultProviderSettings;
                }

                var emailSettings = await _administrationUnitOfWork
                    .SettingsRepository.GetFirstOrDefault(x => x.UserId == userId && x.SettingsType == SettingsTypeEnum.EmailProviderConfiguration);

                if (emailSettings != null && !string.IsNullOrWhiteSpace(emailSettings.SettingsJson))
                {
                    var model = JsonConvert.DeserializeObject<List<EmailProviderSettings>>(emailSettings.SettingsJson) ?? new List<EmailProviderSettings>();

                    return model;
                }

                return defaultProviderSettings;

            }
            catch (Exception exception)
            {
                await _administrationUnitOfWork.AddLogMessage(new LogMessageEntity
                {
                    Message = "Could not load email provider settings",
                    ExceptionMessage = exception.Message,
                    TimeStamp = DateTime.UtcNow,
                    MessageType = LogMessageTypeEnum.Error,
                    Module = nameof(EmailProviderConfiguration),
                });
            }

            return defaultProviderSettings;
        }

        public async Task<bool> OnEstablishConnection(EmailProviderSettings settings)
        {
            try
            {
                var userId = _administrationUnitOfWork.GetValueFromClaims<int>("userId");

                if (userId == 0)
                {
                    await _administrationUnitOfWork.AddLogMessage(new LogMessageEntity
                    {
                        Message = "Could not establish email provider connection, reason: invalid user id!",
                        ExceptionMessage = string.Empty,
                        TimeStamp = DateTime.UtcNow,
                        MessageType = LogMessageTypeEnum.Error,
                        Module = nameof(EmailProviderConfiguration),
                    });

                    return false;
                }

                var emailSettings = await _administrationUnitOfWork
                   .SettingsRepository.GetFirstOrDefault(x => x.UserId == userId && x.SettingsType == SettingsTypeEnum.EmailProviderConfiguration);

                if (emailSettings == null)
                {
                    emailSettings = new GenericSettingsEntity
                    {
                        UserId = userId,
                        SettingsType = SettingsTypeEnum.EmailProviderConfiguration,
                        SettingsJson = string.Empty
                    };
                }

                var models = JsonConvert.DeserializeObject<List<EmailProviderSettings>>(emailSettings.SettingsJson) ?? new List<EmailProviderSettings>();

                var passwordHandler = new PasswordHandler(_securityData);

                if (models.Where(x => x.EmailAddress.ToLower() == settings.EmailAddress.ToLower()).Any()) 
                {
                    await _administrationUnitOfWork.AddLogMessage(new LogMessageEntity
                    {
                        Message = "Could not establish email provider connection, reason: already exists!",
                        ExceptionMessage = string.Empty,
                        TimeStamp = DateTime.UtcNow,
                        MessageType = LogMessageTypeEnum.Error,
                        Module = nameof(EmailProviderConfiguration),
                    });

                    return false;
                }

                models.Add(new EmailProviderSettings
                {

                    Id = models.Count,
                    Name = settings.Name,
                    Provider = settings.Provider,
                    EmailAddress = settings.EmailAddress,
                    Password = passwordHandler.Encrypt(settings.Password),
                    Status = EmailProviderConfigurationStatusEnum.Established,
                    ConnectionTestPassed = settings.ConnectionTestPassed,
                    ConnectionInfo = new EmailProviderConnectionInfo
                    {
                        UpdatedAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                        UpdatedBy = _administrationUnitOfWork.GetValueFromClaims<string>("name")
                    },
                });

                emailSettings.SettingsJson = JsonConvert.SerializeObject(models);

                await _administrationUnitOfWork.SettingsRepository.AddOrUpdate(emailSettings, x => x.UserId == userId && x.SettingsType == SettingsTypeEnum.EmailProviderConfiguration);

                await _administrationUnitOfWork.SaveChangesAsync();

                return true;
            }
            catch (Exception exception)
            {
                await _administrationUnitOfWork.AddLogMessage(new LogMessageEntity
                {
                    Message = "Could not establish email provider connection.",
                    ExceptionMessage = exception.Message,
                    TimeStamp = DateTime.UtcNow,
                    MessageType = LogMessageTypeEnum.Error,
                    Module = nameof(EmailProviderConfiguration),
                });

                return false;
            }
        }

        public async Task<bool> TestConnection(EmailProviderSettings settings)
        {
            try
            {
                return await _emailClient.TestConnection(settings);
            }
            catch (Exception exception)
            {
                await _administrationUnitOfWork.AddLogMessage(new LogMessageEntity
                {
                    Message = "Could not test email provider settings",
                    ExceptionMessage = exception.Message,
                    TimeStamp = DateTime.UtcNow,
                    MessageType = LogMessageTypeEnum.Error,
                    Module = nameof(EmailProviderConfiguration),
                });

                return false;
            }
        }

        //public async Task<bool> UpdateEmailSettings(EmailProviderSettings providerSettings)
        //{
        //    try
        //    {
        //        var isModified = false;

        //        var userId = _administrationUnitOfWork.GetValueFromClaims<int>("userId");

        //        if (userId == 0)
        //        {
        //            await _administrationUnitOfWork.AddLogMessage(new LogMessageEntity
        //            {
        //                Message = "Could not update email cleanup settungs, reason: invalid user id",
        //                ExceptionMessage = string.Empty,
        //                TimeStamp = DateTime.UtcNow,
        //                MessageType = LogMessageTypeEnum.Error,
        //                Module = nameof(EmailAccountCleanupModule),
        //            });

        //            return false;
        //        }

        //        var emailSettings = await _administrationUnitOfWork
        //            .SettingsRepository.GetFirstOrDefault(x => x.UserId == userId && x.SettingsType == SettingsTypeEnum.EmailAccountCleanupSettings);

        //        var passwordHandler = new PasswordHandler(_securityData);

        //        if (emailSettings == null)
        //        {
        //            await _administrationUnitOfWork.AddLogMessage(new LogMessageEntity
        //            {
        //                Message = "Could not update email provider settings",
        //                ExceptionMessage = string.Empty,
        //                TimeStamp = DateTime.UtcNow,
        //                MessageType = LogMessageTypeEnum.Error,
        //                Module = nameof(EmailAccountCleanupModule),
        //            });

        //            return false;
        //        }
        //        else
        //        {
        //            var settings = JsonConvert.DeserializeObject<List<EmailProviderSettings>>(emailSettings.SettingsJson) ?? new List<EmailProviderSettings>();

        //            var settingsUpdate = new List<EmailProviderSettings>();

        //            foreach (var setting in settings)
        //            {
        //                if (setting.Id == providerSettings.Id)
        //                {
        //                    setting.Name = providerSettings.Name;
        //                    setting.Provider = providerSettings.Provider;
        //                    setting.EmailAddress = providerSettings.EmailAddress;
        //                    setting.Password = setting.UpdatePasswordIfDiffers(providerSettings.Password, passwordHandler.Encrypt);
        //                    setting.Status = providerSettings.Status;
        //                    setting.ConnectionTestPassed = providerSettings.ConnectionTestPassed;
        //                    setting.ConnectionInfo = new EmailProviderConnectionInfo
        //                    {
        //                        UpdatedAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
        //                        UpdatedBy = _administrationUnitOfWork.GetValueFromClaims<string>("name")
        //                    };

        //                    isModified = true;
        //                }
        //            }
                    
        //            emailSettings.SettingsJson = JsonConvert.SerializeObject(settingsUpdate);

        //        }

        //        if (isModified) 
        //        {
        //            await _administrationUnitOfWork.SettingsRepository.AddOrUpdate(emailSettings, x => x.UserId == userId && x.SettingsType == SettingsTypeEnum.EmailAccountCleanupSettings);

        //            await _administrationUnitOfWork.SaveChangesAsync();
        //        }
               
        //        return true;

        //    }
        //    catch (Exception exception)
        //    {
        //        await _administrationUnitOfWork.AddLogMessage(new LogMessageEntity
        //        {
        //            Message = "Could not update email provider settings",
        //            ExceptionMessage = exception.Message,
        //            TimeStamp = DateTime.UtcNow,
        //            MessageType = LogMessageTypeEnum.Error,
        //            Module = nameof(EmailAccountCleanupModule),
        //        });

        //        return false;
        //    }
        //}

        public async Task<bool> DeleteConnection(int connectionId)
        {
            try
            {
                var isModified = false;

                var userId = _administrationUnitOfWork.GetValueFromClaims<int>("userId");

                if (userId == 0)
                {
                    throw new Exception("Could not update email cleanup settungs, reason: invalid user id!");
                }

                var emailSettings = await _administrationUnitOfWork
                    .SettingsRepository.GetFirstOrDefault(x => x.UserId == userId && x.SettingsType == SettingsTypeEnum.EmailProviderConfiguration);

                var passwordHandler = new PasswordHandler(_securityData);

                if (emailSettings == null || string.IsNullOrWhiteSpace(emailSettings.SettingsJson))
                {
                    await _administrationUnitOfWork.AddLogMessage(new LogMessageEntity
                    {
                        Message = $"Could not delete email provider connection [{connectionId}]",
                        ExceptionMessage =string.Empty,
                        TimeStamp = DateTime.UtcNow,
                        MessageType = LogMessageTypeEnum.Error,
                        Module = nameof(EmailProviderConfiguration),
                    });

                    return false;
                }

                var connections = JsonConvert.DeserializeObject<List<EmailProviderSettings>>(emailSettings.SettingsJson) ?? new List<EmailProviderSettings>();
                
                if (connections.Any()) 
                { 
                    var connectionsUpdate = connections.Where(x => x.Id != connectionId).ToList();

                    emailSettings.SettingsJson = JsonConvert.SerializeObject(connectionsUpdate);

                    isModified = true;
                }

                if (isModified)
                {
                    await _administrationUnitOfWork.SettingsRepository.AddOrUpdate(emailSettings, x => x.UserId == userId && x.SettingsType == SettingsTypeEnum.EmailProviderConfiguration);

                    await _administrationUnitOfWork.SaveChangesAsync();

                    await _administrationUnitOfWork.AddLogMessage(new LogMessageEntity
                    {
                        Message = $"Email provider connection [{connectionId}] deleted.",
                        ExceptionMessage = string.Empty,
                        TimeStamp = DateTime.UtcNow,
                        MessageType = LogMessageTypeEnum.Error,
                        Module = nameof(EmailProviderConfiguration),
                    });
                }

                return true;
            }
            catch (Exception exception)
            {
                await _administrationUnitOfWork.AddLogMessage(new LogMessageEntity
                {
                    Message = "Could not update email cleanup settings",
                    ExceptionMessage = exception.Message,
                    TimeStamp = DateTime.UtcNow,
                    MessageType = LogMessageTypeEnum.Error,
                    Module = nameof(EmailProviderConfiguration),
                });

                return false;
            }
        }

        #region dispose

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _administrationUnitOfWork.Dispose();
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
