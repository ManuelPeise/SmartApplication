using Data.ContextAccessor.Interfaces;
using Data.Shared.Ai;
using Logic.Ai.Models.Input;
using Logic.Ai.Models.Prediction;
using Microsoft.ML;
using Microsoft.ML.Calibrators;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms;
using Shared.Models.Administration.SpamClassification;

namespace Logic.Ai.SpamPrediction
{
    public class SpamClassification : IDisposable
    {
        private readonly IAiRepository _aiRepository;
        private const char Separator = '\t';
        private const string ModelFileName = "SpamClassification.zip";
        private readonly string _modelFilePath;
        private readonly string _trainModelPath;
        private const string SpamPrediction = "SpamPrediction";

        private readonly MLContext _mlContext;
        public SpamClassification(IAiRepository aiRepository)
        {
            _aiRepository = aiRepository;
            _modelFilePath = Path.Combine(Environment.CurrentDirectory, "AiData\\Models\\EmailTrainingModel.zip");
            _trainModelPath = Path.Combine(Environment.CurrentDirectory, "AiData\\TrainingData\\EmailTrainingData.csv");

            _mlContext = new MLContext();
        }

        public async Task TrainSpamPredictionModel()
        {
            var trainingData = LoadDataView(_trainModelPath);
            var testDataView = LoadDataView(_trainModelPath); // await LoadTrainingDataFromDatabase();

            var pipeLine = GetPipeline();

            var trainer = GetTrainer();

            var trainingsPipline = GetTrainingPipeline(pipeLine, trainer);

            var model = TrainModel(trainingsPipline, trainingData);

            var predictions = Predict(model, testDataView);

            var metrics = GetMetrics(predictions);
            try
            {
                await SaveMetrics(metrics);
            }
            catch (Exception e) 
            {
                var error = e.Message;
            }
            SaveModel(trainingData, model);
        }

        public async Task<SpamPredictionStatisticData> GetStatisticData()
        {
            var trainingEntities = await _aiRepository.SpamClassificationTrainingDataRepository.GetAllAsync()
                ?? new List<SpamClassificationTrainingDataEntity>();
            var metrics = await GetSpamPredictionMetrics();

            return new SpamPredictionStatisticData
            {
                AverageEntrophy = metrics.Sum(x => x.Entropy) > 0 ? (decimal)metrics.Sum(x => x.Entropy)/ metrics.Count : 0.00m,
                Metrics = metrics,
                TrainingEntityCount = trainingEntities.Count,
                ModelsFileTimeStamp = File.Exists(_modelFilePath) ? File.GetLastWriteTime(_modelFilePath).ToString("dd.MM.yyyy HH:mm:ss") : null,
                TrainingFileTimeStamp = File.Exists(_modelFilePath) ? File.GetLastWriteTime(_trainModelPath).ToString("dd.MM.yyyy HH:mm:ss") : null,
            };
        }

        public void SaveTrainingDataCsv(List<string> csv)
        {
            using (var writer = new StreamWriter(_trainModelPath))
            {
                csv.ForEach(row => writer.WriteLine(row));
            }
        }

        public SpamPredictionModel Predict(SpamClassificationModel model)
        {
            var trainedModel = _mlContext.Model.Load(_modelFilePath, out var schema);

            var predictor = _mlContext.Model.CreatePredictionEngine<SpamClassificationModel, SpamPredictionModel>(trainedModel, schema);

            return predictor.Predict(model);
        }

        #region private members

        private IDataView LoadDataView(string path)
        {
            return _mlContext.Data.LoadFromTextFile<SpamClassificationModel>(path, hasHeader: false, separatorChar: '\t');
        }

        private async Task<IDataView> LoadTrainingDataFromDatabase()
        {
            var entities = await _aiRepository.SpamClassificationTrainingDataRepository.GetAllAsync();

            var spamClassificationModels = entities.Select(e => new SpamClassificationModel
            {
                Label = e.IsSpam,
                EmailAddress = e.EmailAddress,
                Subject = e.Subject
            });

            return _mlContext.Data.LoadFromEnumerable<SpamClassificationModel>(spamClassificationModels);
        }

        private EstimatorChain<TypeConvertingTransformer> GetPipeline()
        {
            var pipeline = _mlContext.Transforms.Text.FeaturizeText("EmailFeatures", nameof(SpamClassificationModel.EmailAddress))
                 .Append(_mlContext.Transforms.Text.FeaturizeText("SubjectFeatures", nameof(SpamClassificationModel.Subject)))
                 .Append(_mlContext.Transforms.Concatenate("Features", "EmailFeatures", "SubjectFeatures"))
                 .Append(_mlContext.Transforms.Conversion.ConvertType("Label", outputKind: DataKind.Boolean));

            return pipeline;
        }

        private SdcaLogisticRegressionBinaryTrainer GetTrainer()
        {
            var trainer = _mlContext.BinaryClassification.Trainers.SdcaLogisticRegression("Label", "Features");

            return trainer;
        }

        private EstimatorChain<BinaryPredictionTransformer<CalibratedModelParametersBase<LinearBinaryModelParameters, PlattCalibrator>>> GetTrainingPipeline(
           EstimatorChain<TypeConvertingTransformer> pipeline, SdcaLogisticRegressionBinaryTrainer trainer)
        {
            return pipeline.Append(trainer);
        }

        private TransformerChain<BinaryPredictionTransformer<CalibratedModelParametersBase<LinearBinaryModelParameters, PlattCalibrator>>> TrainModel(
            EstimatorChain<BinaryPredictionTransformer<CalibratedModelParametersBase<LinearBinaryModelParameters, PlattCalibrator>>> trainingPipeline,
            IDataView trainingData)
        {
            var model = trainingPipeline.Fit(trainingData);

            return model;
        }

        private IDataView Predict(TransformerChain<BinaryPredictionTransformer<CalibratedModelParametersBase<LinearBinaryModelParameters, PlattCalibrator>>> model,
            IDataView testData)
        {
            var predictions = model.Transform(testData);

            return predictions;
        }

        private CalibratedBinaryClassificationMetrics GetMetrics(IDataView predictions)
        {
            var metrics = _mlContext.BinaryClassification.Evaluate(predictions, "Label");

            Console.WriteLine($"Accuracy: {metrics.Accuracy:P2}");
            Console.WriteLine($"AUC: {metrics.AreaUnderRocCurve:P2}");
            Console.WriteLine($"F1 Score: {metrics.F1Score:P2}");

            return metrics;
        }

        private void SaveModel(IDataView trainingData,
            TransformerChain<BinaryPredictionTransformer<CalibratedModelParametersBase<LinearBinaryModelParameters, PlattCalibrator>>> model)
        {
            _mlContext.Model.Save(model, trainingData.Schema, _modelFilePath);
        }

        private PredictionEngine<SpamClassificationModel, SpamPredictionModel> GetPredictor(
            TransformerChain<BinaryPredictionTransformer<CalibratedModelParametersBase<LinearBinaryModelParameters, PlattCalibrator>>> model)
        {
            var predictor = _mlContext.Model.CreatePredictionEngine<SpamClassificationModel, SpamPredictionModel>(model);

            return predictor;
        }

        private SpamPredictionModel Predict(PredictionEngine<SpamClassificationModel, SpamPredictionModel> predictionEngine,
            SpamClassificationModel inputModel)
        {
            return predictionEngine.Predict(inputModel);
        }

        private async Task SaveMetrics(CalibratedBinaryClassificationMetrics metrics)
        {
            await _aiRepository.AiScoreRepository.AddAsync(new AiScore
            {
                TimeStamp = DateTime.UtcNow,
                Accuracy = metrics.Accuracy,
                Entropy = metrics.Entropy,
                F1Score = metrics.F1Score,
                LogLoss = metrics.LogLoss,
                LogLossReduction = metrics.LogLossReduction,
                Type = SpamPrediction
            });

            await _aiRepository.AiScoreRepository.SaveChangesAsync();
        }

        private async Task<List<SpamPredictionMetric>> GetSpamPredictionMetrics()
        {
            var metrics = await _aiRepository.AiScoreRepository.GetAllAsync();

            return metrics?
                .Where(x => x.TimeStamp.Date > DateTime.UtcNow.AddDays(-30).Date).GroupBy(x => x.TimeStamp)
                .Select(grp => new SpamPredictionMetric
                {
                    TimeStamp = grp.Key.ToString("dd.MM.yyyy"),
                    Accuracy = grp.Sum(x => x.Accuracy) / grp.Count(),
                    Entropy = grp.Sum(x => x.Entropy) / grp.Count(),
                    F1Score = grp.Sum(x => x.Entropy) / grp.Count(),
                    LogLoss = grp.Sum(x => x.LogLoss) / grp.Count(),
                    LogLossReduction = grp.Sum(x => x.LogLossReduction) / grp.Count(),
                }).ToList() ?? new List<SpamPredictionMetric>();
        }

        #endregion


        #region dispose 

        private bool disposedValue;

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

        public void Dispose()
        {
            // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in der Methode "Dispose(bool disposing)" ein.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}