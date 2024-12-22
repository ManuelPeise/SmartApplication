

using Shared.Models.Ai;

namespace Logic.Shared.Interfaces
{
    public interface IAiTrainingService: IDisposable
    {
        Task CollectTrainingData(int maxMailsToProcess);
        Task<List<AiEmailTrainingData>> GetAiTrainingData();
        Task UpdateTrainingData(List<AiEmailTrainingData> trainingDataModels);
    }
}
