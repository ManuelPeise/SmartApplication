

using Shared.Models.Ai;

namespace Logic.Shared.Interfaces
{
    public interface IAiTrainingService: IDisposable
    {
        Task TrainSpamDetectionAiModel();
    }
}
