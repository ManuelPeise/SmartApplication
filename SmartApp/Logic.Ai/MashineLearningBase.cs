using Data.ContextAccessor.Interfaces;
using Microsoft.ML;

namespace Logic.Ai
{
    public abstract class MashineLearningBase<TInput> where TInput : class
    {
        private MLContext _mlContext;
        private readonly IAiRepository _aiRepository;
        public MLContext MLContext { get => _mlContext; }
        public IAiRepository AiRepository { get => _aiRepository; }

        protected MashineLearningBase(IAiRepository aiRepository)
        {
            _mlContext = new MLContext(seed: 200);
            _aiRepository = aiRepository;
        }

        protected IDataView LoadIDataViewFromFile(string inputDataFilePath, char separatorChar, bool hasHeader, bool allowQuoting)
        {
            return _mlContext.Data.LoadFromTextFile<TInput>(inputDataFilePath, separatorChar, hasHeader, allowQuoting);
        }

        protected void TrainAndSaveModel(IEstimator<ITransformer> pipeline, IDataView data, string modelOutputPath)
        {
            var model = pipeline.Fit(data);

            var dataViewSchema = data.Schema;

            using (var stream = File.Create(modelOutputPath))
            {
                _mlContext.Model.Save(model, dataViewSchema, stream);
            }
        }

        #region private



        private void SaveModel(ITransformer model, IDataView data, string modelSavePath)
        {
            var dataViewSchema = data.Schema;

            using (var fs = File.Create(modelSavePath))
            {
                _mlContext.Model.Save(model, dataViewSchema, fs);
            }
        }

        #endregion
    }
}
