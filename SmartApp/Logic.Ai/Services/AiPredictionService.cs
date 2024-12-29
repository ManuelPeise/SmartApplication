using Data.Shared.Logging;
using Logic.Ai.Models;
using Logic.Shared.Interfaces;
using Shared.Enums;
using Shared.Enums.Ai;
using Shared.Models.Ai;

namespace Logic.Ai.Services
{
    public class AiPredictionService : AMashineLearningBase, IAiPredictionService
    {
        private readonly string _modelFilePath = Path.Combine(Environment.CurrentDirectory, "AiData\\Models\\EmailTrainingModel.zip");

        private ILogRepository _logRepository;

        public AiPredictionService(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public async Task<AiSpamEmailPredictionResult> PredictSpamMail(string from, string subject)
        {
            var defaultResult = new AiSpamEmailPredictionResult
            {
                Classification = SpamClassificationEnum.Unknown
            };

            try
            {
                if (MlContext == null)
                {
                    await _logRepository.AddMessage(new LogMessageEntity
                    {
                        Message = "MlContext is null, abort spam prediction!",
                        ExceptionMessage = null,
                        MessageType = LogMessageTypeEnum.CriticalError,
                        Module = nameof(AiTrainingService),
                        TimeStamp = DateTime.UtcNow,
                    });

                    return defaultResult;
                }

                var model = LoadAiModel(_modelFilePath, out var shema);

                var predictionEngine = MlContext.Model.CreatePredictionEngine<EmailAiInputModel, SpamPrediction>(model);

                var result = predictionEngine.Predict(new EmailAiInputModel
                {
                    From = from,
                    Subject = subject,
                });

                return new AiSpamEmailPredictionResult
                {
                    Label = result.PredictedLabel,
                    Scores = result.Scores,
                    Classification = (SpamClassificationEnum)Enum.Parse(typeof(SpamClassificationEnum), result.PredictedLabel),
                };
            }
            catch (Exception exception)
            {
                await _logRepository.AddMessage(new LogMessageEntity
                {
                    Message = exception.Message,
                    ExceptionMessage = exception.StackTrace,
                    MessageType = LogMessageTypeEnum.CriticalError,
                    Module = nameof(AiTrainingService),
                    TimeStamp = DateTime.UtcNow,
                });

                return defaultResult;
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
