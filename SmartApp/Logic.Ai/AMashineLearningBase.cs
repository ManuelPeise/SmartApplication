using Microsoft.ML;

namespace Logic.Ai
{
    public abstract class AMashineLearningBase
    {
        private MLContext? _mlContext;
        private bool disposedValue;

        public MLContext? MlContext { get => _mlContext; }

        public AMashineLearningBase()
        {
            _mlContext = new MLContext(seed: 0);
        }

        public IDataView? LoadTrainingData<TModel>(string trainingFilePath, bool hasHeader, char separator)
        {
            if (_mlContext != null)
            {
                return _mlContext.Data.LoadFromTextFile<TModel>(path: trainingFilePath, hasHeader: hasHeader, separatorChar: separator);
            }

            return null;
        }

        public IDataView? LoadAndUpdateTrainingData<TModel>(List<string> csvRows, string trainingFilePath, bool replace = false, bool hasHeader = true, char separator = '\t')
        {
            //if (File.Exists(trainingFilePath) && replace)
            //{
            //    File.Delete(trainingFilePath);
            //}

            if (replace)
            {
                using (var file = File.CreateText(trainingFilePath))
                {
                    csvRows.ForEach(line => file.WriteLine(line));
                }
            }

            return _mlContext == null ? null : _mlContext.Data.LoadFromTextFile<TModel>(path: trainingFilePath, hasHeader: hasHeader, separatorChar: separator);
        }

        public ITransformer? LoadAiModel(string modelFilePath, out DataViewSchema? shema)
        {
            shema = null;

            if (_mlContext != null)
            {
                return _mlContext.Model.Load(modelFilePath, out shema);
            }

            return null;
        }

        public void SaveAiModelAsFile(ITransformer? model, IDataView dataView, string targetFilePath)
        {
            if (_mlContext != null)
            {
                _mlContext?.Model.Save(model, dataView?.Schema, targetFilePath);
            }
        }

    }

}
