using Data.ContextAccessor.Interfaces;
using Data.Shared.Ai;
using Data.Shared.Logging;
using Data.Shared.Tools;
using Logic.Ai.Models.Input;
using Logic.Ai.Models.Prediction;
using Logic.Ai.SpamPrediction;
using Shared.Enums;
using Shared.Models.Administration.SpamClassification;

namespace Logic.Administration
{
    public class SpamClassificationDataService : IDisposable
    {
        private readonly IAiRepository _aiRepository;
        private bool disposedValue;

        public SpamClassificationDataService(IAiRepository aiRepository)
        {
            _aiRepository = aiRepository;
        }

        public async Task<SpamClassificationPageData> GetSpamClassificationPageData()
        {
            try
            {
                using (var classification = new SpamClassification(_aiRepository))
                {
                    var dataResult = await GetSpamClassificationData();

                    return new SpamClassificationPageData
                    {
                        Statistics = await classification.GetStatisticData(),
                        Domains = dataResult.domainData
                    };
                }

                throw new Exception("Could not load Spam classification page data.");
            }
            catch (Exception exception)
            {
                await _aiRepository.LogRepository.AddMessage(new LogMessageEntity
                {
                    Message = "Could not load spam classification page data.",
                    ExceptionMessage = exception.Message,
                    MessageType = LogMessageTypeEnum.Error,
                    TimeStamp = DateTime.UtcNow,
                    Module = nameof(SpamClassificationDataService)
                });

                await _aiRepository.LogRepository.SaveChanges();

                return new SpamClassificationPageData
                {
                    Domains = new List<EmailDomainModel>()
                };
            }
        }

        public async Task<bool> SaveTrainingData(SaveTrainingDataRequest request)
        {
            try
            {
                var newTrainingEntities = new List<SpamClassificationTrainingDataEntity>();
                var updatedTrainingEntities = new List<SpamClassificationTrainingDataEntity>();

                var existingTrainingData = await _aiRepository.SpamClassificationTrainingDataRepository.GetAllAsync();

                foreach (var model in request.Models)
                {
                    var label = model.Classification == SpamClassificationEnum.Spam ? true : false;

                    var existingEntity = existingTrainingData.FirstOrDefault(e => e.EmailAddress.ToLower() == model.Email.ToLower()
                        && e.Subject.ToLower() == model.Subject.ToLower());

                    if (existingEntity != null)
                    {
                        existingEntity.IsSpam = label;

                        updatedTrainingEntities.Add(existingEntity);

                        continue;
                    }

                    newTrainingEntities.Add(new SpamClassificationTrainingDataEntity
                    {
                        EmailAddress = model.Email,
                        Subject = model.Subject,
                        IsSpam = label,
                    });
                }

                var hasModifications = false;

                if (updatedTrainingEntities.Any())
                {
                    _aiRepository.SpamClassificationTrainingDataRepository.UpdateRange(updatedTrainingEntities);
                    hasModifications = true;
                }

                if (newTrainingEntities.Any())
                {
                    await _aiRepository.SpamClassificationTrainingDataRepository.AddRange(newTrainingEntities);

                    hasModifications = true;
                }


                if (hasModifications)
                {
                    await _aiRepository.SpamClassificationTrainingDataRepository.SaveChanges();
                }

                return true;
            }
            catch (Exception exception)
            {
                await _aiRepository.LogRepository.AddMessage(new LogMessageEntity
                {
                    Message = "Could not save spam prediction training data.",
                    ExceptionMessage = exception.Message,
                    MessageType = LogMessageTypeEnum.Error,
                    TimeStamp = DateTime.UtcNow,
                    Module = nameof(SpamClassificationDataService)
                });

                await _aiRepository.LogRepository.SaveChanges();

                return false;
            }
        }

        public async Task UpdateTrainingDataCsv()
        {
            using (var spamAi = new SpamClassification(_aiRepository))
            {
                try
                {
                    var csv = await GetUpdatedTrainingCsvData();

                    spamAi.SaveTrainingDataCsv(csv);
                }
                catch (Exception exception)
                {
                    await _aiRepository.LogRepository.AddMessage(new LogMessageEntity
                    {
                        Message = "Could not update spam prediction training csv.",
                        ExceptionMessage = exception.Message,
                        MessageType = LogMessageTypeEnum.Error,
                        TimeStamp = DateTime.UtcNow,
                        Module = nameof(SpamClassificationDataService)
                    });

                    await _aiRepository.LogRepository.SaveChanges();
                }
            }
        }

        public SpamPredictionModel PredictEmail(SpamClassificationModel model)
        {
            using (var classification = new SpamClassification(_aiRepository))
            {
                return classification.Predict(model);
            }
        }

        #region private

        private async Task<(int dataEntities, int trainingEntities, List<EmailDomainModel> domainData)> GetSpamClassificationData()
        {
            try
            {
                var emailDataEntities = await LoadEmailDataEntities();
                var existingTrainingData = await _aiRepository.SpamClassificationTrainingDataRepository.GetAllAsync();

                if (emailDataEntities == null || existingTrainingData == null)
                {
                    return (emailDataEntities?.Count ?? 0, existingTrainingData?.Count ?? 0, new List<EmailDomainModel>());
                }

                var id = 0;

                var domainData = (from data in emailDataEntities
                                  where data != null && !string.IsNullOrWhiteSpace(data.Subject)
                                  group data by data.FromAddress.Split('@')[1] into domainGroup
                                  select new EmailDomainModel
                                  {
                                      Id = id++,
                                      DomainName = domainGroup.Key,
                                      ClassificationDataSets = (from entity in domainGroup.Select(set => set).Distinct()
                                                                let relatedTrainingData = GetRelatedTriningDataEntity(existingTrainingData, entity)
                                                                select new SpamClassificationDataSet
                                                                {
                                                                    Id = entity.Id,
                                                                    Email = entity.FromAddress,
                                                                    Subject = WithoutSpecialChars(entity.Subject),
                                                                    Classification = relatedTrainingData != null && relatedTrainingData.IsSpam
                                                                    ? SpamClassificationEnum.Spam : SpamClassificationEnum.Unknown
                                                                }).ToList()
                                  }).ToList();

                return (emailDataEntities?.Count ?? 0, existingTrainingData?.Count ?? 0, domainData);

            }
            catch (Exception exception)
            {
                await _aiRepository.LogRepository.AddMessage(new LogMessageEntity
                {
                    Message = "Could not load available domain data for spam classification.",
                    ExceptionMessage = exception.Message,
                    MessageType = LogMessageTypeEnum.Error,
                    TimeStamp = DateTime.UtcNow,
                    Module = nameof(SpamClassificationDataService)
                });

                await _aiRepository.LogRepository.SaveChanges();

                return (0, 0, new List<EmailDomainModel>());
            }
        }

        private async Task<List<EmailDataEntity>> LoadEmailDataEntities()
        {
            var entities = await _aiRepository.EmailDataRepository.GetAllAsync() ?? new List<EmailDataEntity>();

            return entities
                .GroupBy(e => new { e.FromAddress, e.Subject })
                .Select(grp => grp.First())
                .ToList();
        }

        private SpamClassificationTrainingDataEntity? GetRelatedTriningDataEntity(List<SpamClassificationTrainingDataEntity> trainingDataCollection,
            EmailDataEntity dataEntity)
        {
            return trainingDataCollection
                .Where(x => !string.IsNullOrWhiteSpace(x.Subject))
                .FirstOrDefault(x => x.EmailAddress.ToLower() == dataEntity.FromAddress.ToLower()
                && x.Subject.ToLower() == dataEntity.Subject.ToLower());
        }

        private async Task<List<string>> GetUpdatedTrainingCsvData()
        {
            var csv = new List<string>();

            var existingTrainingData = await _aiRepository.SpamClassificationTrainingDataRepository.GetAllAsync() ?? new List<SpamClassificationTrainingDataEntity>();

            var distinctedTrainingData = existingTrainingData
                .GroupBy(t => new { t.IsSpam, t.EmailAddress, t.Subject })
                .Select(grp => grp.First())
                .ToList();

            distinctedTrainingData.ForEach(set =>
            {
                csv.Add($"{set.IsSpam}\t{set.EmailAddress}\t{set.Subject}");
            });

            return csv;
        }

        private string WithoutSpecialChars(string value)
        {
            return value.Replace("\\", "").Trim('"');
        }

        #endregion

        #region dispose

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                }

                disposedValue = true;
            }
        }


        void IDisposable.Dispose()
        {
            // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in der Methode "Dispose(bool disposing)" ein.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
