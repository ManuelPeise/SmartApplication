using Data.AppContext;
using Data.Shared.Logging;
using Logic.Ai.Models;
using Logic.Shared;
using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.ML;
using Shared.Enums;
using Shared.Enums.Ai;

namespace Logic.Ai.Services
{
    public class AiTrainingService : AMashineLearningBase, IAiTrainingService
    {
        private const string EmailSpamDetection = "EmailSpamDetection";
        private const string Features = "Features";

        private readonly string _trainingDataFilePath = Path.Combine(Environment.CurrentDirectory, "AiData\\TrainingData\\EmailTrainingData.csv");
        private readonly string _modelFilePath = Path.Combine(Environment.CurrentDirectory, "AiData\\Models\\EmailTrainingModel.zip");

        private ApplicationDbContext _applicationDbContext;
        private ILogRepository _logRepository;
        private IHttpContextAccessor _httpContextAccessor;

        public AiTrainingService(ApplicationDbContext applicationDbContext, ILogRepository logRepository, IHttpContextAccessor httpContextAccessor)
        {
            _applicationDbContext = applicationDbContext;
            _logRepository = logRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task TrainSpamDetectionAiModel()
        {
            using (var unitOfWork = new AdministrationUnitOfWork(_applicationDbContext, _httpContextAccessor, _logRepository))
            {
                try
                {
                    if (MlContext == null)
                    {
                        await _logRepository.AddMessage(new LogMessageEntity
                        {
                            Message = "MlContext is null, abort train model!",
                            ExceptionMessage = null,
                            MessageType = LogMessageTypeEnum.CriticalError,
                            Module = nameof(AiTrainingService),
                            TimeStamp = DateTime.UtcNow,
                        });

                        return;
                    }

                    var entities = await unitOfWork.EmailSpamTrainingsRepository.GetAllAsync();

                    if (!entities.Any())
                    {
                        return;
                    }

                    var csv = new List<string>();

                    entities.ForEach(model => csv.Add($"{model.Classification}\t{model.From}\t{model.Subject}"));

                    var dataView = LoadAndUpdateTrainingData<EmailAiInputModel>(csv, _trainingDataFilePath);

                    if (dataView == null)
                    {
                        throw new Exception("Could not initialize training data to train model!");
                    }

                    var partitions = MlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);

                    var pipeline = MlContext.Transforms.Text.FeaturizeText("SubjectFeatures", "Subject") // Featurize subject
                      .Append(MlContext.Transforms.Text.FeaturizeText("FromAddressFeatures", "From")) // Featurize from address
                      .Append(MlContext.Transforms.Concatenate("Features", "SubjectFeatures", "FromAddressFeatures")) // Combine features
                      .Append(MlContext.Transforms.Conversion.MapValueToKey("Label", "Label")) // Map labels to key values
                      .Append(MlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy("Label", "Features")) // Multi-class classifier
                      .Append(MlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel", "PredictedLabel")); // Map predictions back to original labels

                    var trainedModel = pipeline.Fit(partitions.TrainSet);

                    // var metrics = MlContext.MulticlassClassification.Evaluate(trainedModel.Transform(dataView));

                    SaveAiModelAsFile(trainedModel, dataView, _modelFilePath);

                    return;
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
                }
            }
        }


        private SpamEmailAiMapping GetSpamEmailMapping(string? classification)
        {
            switch (classification)
            {
                case "Unknown":
                    return new SpamEmailAiMapping
                    {
                        Label = classification,
                        Classification = SpamClassificationEnum.Unknown
                    };
                case "Ham":
                    return new SpamEmailAiMapping
                    {
                        Label = classification,
                        Classification = SpamClassificationEnum.Ham
                    };
                case "Spam":
                    return new SpamEmailAiMapping
                    {
                        Label = classification,
                        Classification = SpamClassificationEnum.Spam
                    };

                default:
                    return new SpamEmailAiMapping
                    {
                        Label = classification,
                        Classification = SpamClassificationEnum.Unknown
                    };
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
                    _applicationDbContext?.Dispose();
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
