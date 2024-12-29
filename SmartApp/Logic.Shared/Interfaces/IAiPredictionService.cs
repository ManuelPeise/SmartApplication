using Shared.Models.Ai;

namespace Logic.Shared.Interfaces
{
    public interface IAiPredictionService: IDisposable
    {
        Task<AiSpamEmailPredictionResult> PredictSpamMail(string from, string subject);
    }
}
