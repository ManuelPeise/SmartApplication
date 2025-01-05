using Data.AppContext;
using Data.ContextAccessor;
using Data.ContextAccessor.Interfaces;
using Data.Shared;
using Data.Shared.Logging;
using Logic.Settings.Extensions;
using Logic.Settings.Interfaces;
using Logic.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Shared.Enums;
using Shared.Models.Identity;
using Shared.Models.Settings.EmailAccountSettings;


namespace Logic.Settings
{
    public class EmailAccountSettingsService : IEmailAccountSettingsService
    {
        private readonly IOptions<SecurityData> _securityData;
        private readonly ISettingsRepository _settingsRepository;

        public EmailAccountSettingsService(
            ApplicationDbContext applicationDbContext,
            ISettingsRepository settingsRepository,
            ILogRepository logRepository,
            IOptions<SecurityData> securityData,
            IHttpContextAccessor httpContextAccessor)
        {
            _securityData = securityData;
            _settingsRepository = settingsRepository;
        }


        public async Task<List<EmailAccountSettingsModel>> GetSettings()
        {

            try
            {
                var userId = _settingsRepository.ClaimsAccessor.GetClaimsValue<int>("userId");

                if (userId == 0)
                {
                    throw new Exception("Could not load user id from claims!");
                }

                var entities = await _settingsRepository.EmailAccountRepository.GetAll(x => x.UserId == userId) ?? new List<EmailAccountEntity>();

                if (!entities.Any())
                {
                    return new List<EmailAccountSettingsModel>();

                }

                return (from entity in entities
                        select entity.ToUiModel()).ToList();

            }
            catch (Exception exception)
            {
                await _settingsRepository.LogRepository.AddMessage(new LogMessageEntity
                {
                    Message = "Could not load email account settings.",
                    ExceptionMessage = exception.Message,
                    TimeStamp = DateTime.UtcNow,
                    MessageType = LogMessageTypeEnum.Error,
                    Module = nameof(EmailAccountSettingsService),
                });

                await _settingsRepository.SaveChanges();

                return new List<EmailAccountSettingsModel>();
            }

        }

        public async Task<EmailAccountSettingsModel?> SaveOrUpdateConnection(EmailAccountSettingsModel model)
        {

            try
            {
                var userId = _settingsRepository.ClaimsAccessor.GetClaimsValue<int>("userId");
                var userName = _settingsRepository.ClaimsAccessor.GetClaimsValue<string>("name");

                if (userId == 0 || string.IsNullOrWhiteSpace(userName))
                {
                    throw new Exception("Could not load user id from claims!");
                }

                var entity = await _settingsRepository.EmailAccountRepository.GetSingle(x => x.EmailAddress.ToLower() == model.EmailAddress.ToLower());

                var passwordHandler = new PasswordHandler(_securityData);

                if (entity != null)
                {
                    entity = model.ToUpdatedEntity(entity.EncodedPassword, passwordHandler, GetLogMessageJson(userName));
                }
                else
                {
                    entity = new EmailAccountEntity
                    {
                        UserId = userId,
                        AccountName = model.AccountName,
                        ProviderType = model.ProviderType,
                        Server = model.Server,
                        Port = model.Port,
                        EmailAddress = model.EmailAddress,
                        EncodedPassword = passwordHandler.Encrypt(model.Password),
                        MessageLogJson = GetLogMessageJson(userName)
                    };
                }

                var result = await _settingsRepository.EmailAccountRepository.AddOrUpdate(entity, x => x.Id == entity.Id && x.UserId == entity.UserId);

                await _settingsRepository.SaveChanges();

                return entity.ToUiModel();
            }
            catch (Exception exception)
            {
                await _settingsRepository.LogRepository.AddMessage(new LogMessageEntity
                {
                    Message = $"Could update entity [{model.Id}].",
                    ExceptionMessage = exception.Message,
                    TimeStamp = DateTime.UtcNow,
                    MessageType = LogMessageTypeEnum.Error,
                    Module = nameof(EmailAccountSettingsService),
                });

                await _settingsRepository.SaveChanges();

                return null;
            }

        }

        private string GetLogMessageJson(string userName)
        {
            return JsonConvert.SerializeObject(new MessageLog
            {
                User = userName,
                TimeStamp = DateTime.UtcNow.ToString("dd.MM.yyyy - HH:mm")
            });
        }


    }
}


