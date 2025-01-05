using Data.AppContext;
using Data.ContextAccessor;
using Data.Shared;
using Data.Shared.Logging;
using Logic.Settings.Extensions;
using Logic.Settings.Interfaces;
using Logic.Shared;
using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Shared.Enums;
using Shared.Models.Identity;
using Shared.Models.Settings.EmailAccountSettings;


namespace Logic.Settings
{
    public class EmailAccountSettingsService : ALogicBase, IEmailAccountSettingsService
    {
        private readonly IOptions<SecurityData> _securityData;

        public EmailAccountSettingsService(
            ApplicationDbContext applicationDbContext,
            ILogRepository logRepository,
            IOptions<SecurityData> securityData,
            IHttpContextAccessor? httpContextAccessor)
            : base(applicationDbContext, null, logRepository, httpContextAccessor)
        {
            _securityData = securityData;

        }


        public async Task<List<EmailAccountSettingsModel>> GetSettings()
        {
            using (var settingsRepository = new SettingsRepository(ApplicationContext))
            {
                try
                {
                    var userId = GetClaimsValue<int>("userId");

                    if (userId == 0)
                    {
                        throw new Exception("Could not load user id from claims!");
                    }

                    var entities = await settingsRepository.EmailAccountRepository.GetAll(x => x.UserId == userId) ?? new List<EmailAccountEntity>();

                    if (!entities.Any())
                    {
                        return new List<EmailAccountSettingsModel>();

                    }

                    return (from entity in entities
                            select entity.ToUiModel()).ToList();

                }
                catch (Exception exception)
                {
                    await LogMessage(new LogMessageEntity
                    {
                        Message = "Could not load email account settings.",
                        ExceptionMessage = exception.Message,
                        TimeStamp = DateTime.UtcNow,
                        MessageType = LogMessageTypeEnum.Error,
                        Module = nameof(EmailAccountSettingsService),
                    });

                    return new List<EmailAccountSettingsModel>();
                }
            }
        }

        public async Task<EmailAccountSettingsModel?> SaveOrUpdateConnection(EmailAccountSettingsModel model)
        {
            using (var settingsRepository = new SettingsRepository(ApplicationContext))
            {
                try
                {
                    var userId = GetClaimsValue<int>("userId");
                    var userName = GetClaimsValue<string>("name");

                    if (userId == 0 || string.IsNullOrWhiteSpace(userName))
                    {
                        throw new Exception("Could not load user id from claims!");
                    }

                    var entity = await settingsRepository.EmailAccountRepository.GetSingle(x => x.EmailAddress.ToLower() == model.EmailAddress.ToLower());

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

                    var result = await settingsRepository.EmailAccountRepository.AddOrUpdate(entity, x => x.Id == entity.Id && x.UserId == entity.UserId);

                    await SaveContextChangesAsync();

                    return entity.ToUiModel();
                }
                catch (Exception exception)
                {
                    await LogMessage(new LogMessageEntity
                    {
                        Message = $"Could update entity [{model.Id}].",
                        ExceptionMessage = exception.Message,
                        TimeStamp = DateTime.UtcNow,
                        MessageType = LogMessageTypeEnum.Error,
                        Module = nameof(EmailAccountSettingsService),
                    });

                    return null;
                }
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


