using Data.AppContext;
using Data.Identity;
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

namespace Logic.Ai
{
    public class AiTrainingService : IAiTrainingService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogRepository _logRepository;
        private readonly IEmailClient _emailClient;
        private readonly IOptions<SecurityData> _securityData;

        private bool disposedValue;


        public AiTrainingService(IdentityDbContext identityContext,
            ApplicationDbContext applicationDbContext,
            IHttpContextAccessor httpContextAccessor,
            IOptions<SecurityData> securityData,
            ILogRepository logRepository,
            IEmailClient emailClient)
        {
            _applicationDbContext = applicationDbContext;
            _httpContextAccessor = httpContextAccessor;
            _logRepository = logRepository;
            _emailClient = emailClient;

            _securityData = securityData;
        }

        public async Task CollectTrainingData(int maxMailsToProcess)
        {
            var trainingDataEntities = new List<EmailClassificationTrainingDataEntity>();

            using (var unitOfWork = new AdministrationUnitOfWork(_applicationDbContext, _httpContextAccessor, _logRepository))
            {
                try
                {
                    var existingProviderConfigurations = await unitOfWork.SettingsRepository.GetAll(x => x.SettingsType == SettingsTypeEnum.EmailProviderConfiguration);

                    if (existingProviderConfigurations == null || !existingProviderConfigurations.Any())
                    {
                        return;
                    }

                    var passwordHandler = new PasswordHandler(_securityData);

                    foreach (var config in existingProviderConfigurations)
                    {
                        var models = TryParseModel<List<EmailProviderSettings>>(config.SettingsJson);

                        if (models == null)
                        {
                            continue;
                        }

                        foreach (var model in models)
                        {
                            if (!model.ConnectionTestPassed || !model.AllowCollectAiTrainingData)
                            {
                                continue;
                            }

                            model.Password = passwordHandler.Decrypt(model.Password);

                            var emails = await _emailClient.GetEmailAiTrainingDataModel(model, maxMailsToProcess);

                            if (emails.Any())
                            {
                                trainingDataEntities.AddRange(emails.Select(mail => new EmailClassificationTrainingDataEntity
                                {
                                    UserId = config.UserId,
                                    From = mail.From,
                                    Subject = mail.Subject,
                                    Domain = mail.Domain,
                                    Classification = SpamClassificationEnum.Unknown,
                                }));
                            }

                        }

                    }

                    if (trainingDataEntities.Count != 0)
                    {
                        var existing = await unitOfWork.EmailSpamTrainingsRepository.GetAllAsync();

                        trainingDataEntities = trainingDataEntities
                            .Where(x => !string.IsNullOrWhiteSpace(x.From)
                                && !string.IsNullOrWhiteSpace(x.Subject)
                                && existing.Select(ex => ex).Where(ex => ex.From == x.From && ex.Subject == x.Subject)
                                .GroupBy(ex => new { ex.From, ex.Subject, }).FirstOrDefault() == null)
                            .GroupBy(x => new { x.From, x.Subject, x.UserId })
                            .Select(x => x.First())
                            .ToList();

                        await unitOfWork.EmailSpamTrainingsRepository.AddRange(trainingDataEntities);

                        await unitOfWork.SaveChangesAsync();
                    }

                }
                catch (Exception exception)
                {
                    await unitOfWork.AddLogMessage(new LogMessageEntity
                    {
                        Message = "Could not get training data for email spam detection.",
                        ExceptionMessage = exception.Message,
                        TimeStamp = DateTime.UtcNow,
                        Module = nameof(AiTrainingService),
                        MessageType = LogMessageTypeEnum.Error
                    });
                }
            }
        }

        public async Task<List<AiEmailTrainingData>> GetAiTrainingData()
        {
            using (var unitOfWork = new AdministrationUnitOfWork(_applicationDbContext, _httpContextAccessor, _logRepository))
            {
                try
                {
                    var userId = unitOfWork.GetValueFromClaims<int>("userId");

                    var entities = await unitOfWork.EmailSpamTrainingsRepository.GetAll(x => x.UserId == userId) ?? new List<EmailClassificationTrainingDataEntity>();

                    return (from entity in entities
                            select new AiEmailTrainingData
                            {
                                Id = entity.Id,
                                From = entity.From,
                                Subject = entity.Subject,
                                Domain = entity.Domain,
                                Classification = entity.Classification,
                            }).ToList();
                }
                catch (Exception exception)
                {
                    await unitOfWork.AddLogMessage(new LogMessageEntity
                    {
                        Message = "Could not load ai training data.",
                        ExceptionMessage = exception.Message,
                        Module = nameof(AiTrainingService),
                        TimeStamp = DateTime.UtcNow,
                        MessageType = LogMessageTypeEnum.Error,
                    });

                    return new List<AiEmailTrainingData>();
                }
            }
        }

        public async Task UpdateTrainingData(List<AiEmailTrainingData> trainingDataModels)
        {
            using (var unitOfWork = new AdministrationUnitOfWork(_applicationDbContext, _httpContextAccessor, _logRepository))
            {
                try
                {

                    var affectedExistingEntities = await unitOfWork.EmailSpamTrainingsRepository.GetAll(x => trainingDataModels.Select(x => x.Id).Contains(x.Id)) ?? new List<EmailClassificationTrainingDataEntity>();

                    if (affectedExistingEntities.Any())
                    {
                        foreach (var entity in affectedExistingEntities)
                        {
                            var model = trainingDataModels.FirstOrDefault(x => x.Id == entity.Id);

                            if (model == null)
                            {

                                continue;
                            }

                            entity.Classification = model.Classification;

                            await unitOfWork.EmailSpamTrainingsRepository.AddOrUpdate(entity, x => x.Id == entity.Id);
                        }
                    }

                    await unitOfWork.SaveChangesAsync();

                }
                catch (Exception exception)
                {
                    await unitOfWork.AddLogMessage(new LogMessageEntity
                    {
                        Message = "Could not update ai training data.",
                        ExceptionMessage = exception.Message,
                        Module = nameof(AiTrainingService),
                        TimeStamp = DateTime.UtcNow,
                        MessageType = LogMessageTypeEnum.Error,
                    });
                }
            }
        }

        private T? TryParseModel<T>(string? json)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(json))
                {
                    return default(T?);
                }

                return JsonConvert.DeserializeObject<T>(json);
            }
            catch
            {
                return default(T?);
            }
        }


        #region dispose

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _applicationDbContext?.Dispose();
                    _logRepository?.Dispose();
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
