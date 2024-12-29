using Data.AppContext;
using Data.Shared.Ai;
using Data.Shared.Logging;
using Logic.Shared;
using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Shared.Enums;
using Shared.Enums.Ai;
using Shared.Models.Administration.Email;
using Shared.Models.Ai;
using Shared.Models.Identity;

namespace Logic.Ai.Services
{
    public class AITrainingDataCollector: IAiTrainingDataCollector
    {
        private ApplicationDbContext _applicationDbContext;
        private ILogRepository _logRepository;
        private IHttpContextAccessor _httpContextAccessor;
        private IEmailClient _emailClient;
        private readonly IOptions<SecurityData> _securityData;

        public AITrainingDataCollector(ApplicationDbContext applicationDbContext, ILogRepository logRepository, IHttpContextAccessor httpContextAccessor, IEmailClient emailClient, IOptions<SecurityData> securityData)
        {
            _applicationDbContext = applicationDbContext;
            _logRepository = logRepository;
            _httpContextAccessor = httpContextAccessor;
            _emailClient = emailClient;
            _securityData = securityData;
        }

        public async Task CollectTrainingData()
        {
            using (var unitOfWork = new AdministrationUnitOfWork(_applicationDbContext, _httpContextAccessor, _logRepository))
            {
                try
                {
                    var emailConfigurations = await unitOfWork.SettingsRepository.GetAll(x => x.SettingsType == SettingsTypeEnum.EmailProviderConfiguration && !string.IsNullOrWhiteSpace(x.SettingsJson));

                    if (emailConfigurations == null || !emailConfigurations.Any())
                    {
                        return;
                    }

                    var settingsCollection = (from config in emailConfigurations
                                              where !string.IsNullOrWhiteSpace(config.SettingsJson)
                                              let providerSettings = JsonConvert.DeserializeObject<List<EmailProviderSettings>>(config.SettingsJson)
                                              from setting in providerSettings
                                              where setting.ConnectionTestPassed && setting.AllowCollectAiTrainingData
                                              select setting).ToList();


                    if (settingsCollection == null || !settingsCollection.Any())
                    {
                        return;
                    }

                    var trainingDataCollection = new List<AiEmailTrainingData>();

                    var passwordHandler = new PasswordHandler(_securityData);

                    foreach (var setting in settingsCollection)
                    {
                        var providerSettings = setting;

                        providerSettings.Password = passwordHandler.Decrypt(setting.Password);

                        var aiTrainingData = await _emailClient.GetEmailAiTrainingDataModel(providerSettings);

                        if (aiTrainingData != null && aiTrainingData.Any())
                        {
                            trainingDataCollection.AddRange(aiTrainingData);
                        }
                    }

                    var existingDataSets = await unitOfWork.EmailSpamTrainingsRepository.GetAllAsync();

                    var entitiesToAdd = new List<EmailClassificationTrainingDataEntity>();

                    foreach (var data in trainingDataCollection)
                    {
                        if (existingDataSets.Any(x => x.From == data.From && x.Subject == data.Subject))
                        {
                            continue;
                        }

                        entitiesToAdd.Add(new EmailClassificationTrainingDataEntity
                        {
                            From = data.From,
                            Subject = data.Subject,
                            Domain = data.Domain,
                            Classification = SpamClassificationEnum.Unknown
                        });
                    }

                    await unitOfWork.EmailSpamTrainingsRepository.AddRange(entitiesToAdd);

                    await unitOfWork.SaveChangesAsync();

                }
                catch (Exception exception)
                {
                    await _logRepository.AddMessage(new LogMessageEntity
                    {
                        Message = "Collecting training data for spam mail detection failed!",
                        ExceptionMessage = exception.Message,
                        TimeStamp = DateTime.UtcNow,
                        MessageType = LogMessageTypeEnum.Error,
                        Module = nameof(AITrainingDataCollector)
                    });
                }
            }
        }


        #region dispose

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _logRepository?.Dispose();
                    _applicationDbContext?.Dispose();
                    _emailClient?.Dispose();
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
