
using Data.Shared.Email;
using Data.Shared.Logging;
using Logic.Shared.Interfaces;
using Shared.Enums;
using Shared.Models.Administration.Email;

namespace Logic.Interfaces
{
    public class EmailAccountCleanupModule: IEmailAccountCleanupModule
    {
        private readonly IAdministrationUnitOfWork _administrationUnitOfWork;
        private readonly IEmailClient _emailClient;
        private bool disposedValue;

        public EmailAccountCleanupModule(IAdministrationUnitOfWork administrationUnitOfWork, IEmailClient emailClient)
        {
            _administrationUnitOfWork = administrationUnitOfWork;
            _emailClient = emailClient;
        }

        public async Task<EmailAccountSettings?> GetSettings()
        {
            try
            {
                var userId = _administrationUnitOfWork.GetValueFromClaims<int>("userId");

                if (userId == 0)
                {
                    throw new Exception("Could not load email cleanup settungs, reason: invalid user id!");
                }

                var emailSettings = await _administrationUnitOfWork.EmailAccountSettingsRepository.GetFirstOrDefault(x => x.UserId == userId);

                if (emailSettings != null)
                {
                    return new EmailAccountSettings
                    {
                        EmailAddress = emailSettings.EmailAddress,
                        EmailServerAddress = emailSettings.EmailServerAddress,
                        Password = emailSettings.Password,
                        Port = emailSettings.Port,
                    };
                }
                else
                {
                    return new EmailAccountSettings
                    {
                        EmailAddress = string.Empty,
                        EmailServerAddress = string.Empty,
                        Password = string.Empty,
                        Port = null,
                    };
                }
            }
            catch (Exception exception)
            {
                await _administrationUnitOfWork.AddLogMessage(new LogMessageEntity
                {
                    Message = "Could not email cleanup settings",
                    ExceptionMessage = exception.Message,
                    TimeStamp = DateTime.UtcNow,
                    MessageType = LogMessageTypeEnum.Error,
                    Module = nameof(EmailAccountCleanupModule),
                });
            }

            return null;
        }

        public async Task UpdateEmailSettings(EmailAccountSettings settings)
        {
            try
            {
                var userId = _administrationUnitOfWork.GetValueFromClaims<int>("userId");

                if (userId == 0)
                {
                    throw new Exception("Could not update email cleanup settungs, reason: invalid user id!");
                }

                var emailSettings = await _administrationUnitOfWork.EmailAccountSettingsRepository.GetFirstOrDefault(x => x.UserId == userId);

                if (emailSettings == null)
                {
                    emailSettings = new EmailAccountSettingsEntity
                    {
                        UserId = userId,
                        EmailAddress = settings.EmailAddress,
                        EmailServerAddress = settings.EmailServerAddress,
                        Password = settings.Password,
                        Port = (int)settings.Port,
                    };
                }
                else
                {
                    emailSettings.EmailServerAddress = settings.EmailServerAddress;
                    emailSettings.Password = settings.Password;
                    emailSettings.Port = (int)settings.Port;
                    emailSettings.EmailAddress = settings.EmailAddress;
                }

                await _administrationUnitOfWork.EmailAccountSettingsRepository.AddOrUpdate(emailSettings, x => x.UserId == userId);

                await _administrationUnitOfWork.SaveChangesAsync();

            }
            catch (Exception exception)
            {
                await _administrationUnitOfWork.AddLogMessage(new LogMessageEntity
                {
                    Message = "Could not update email cleanup settings",
                    ExceptionMessage = exception.Message,
                    TimeStamp = DateTime.UtcNow,
                    MessageType = LogMessageTypeEnum.Error,
                    Module = nameof(EmailAccountCleanupModule),
                });
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
