using Shared.Models.Administration.Email;
using Shared.Models.Ai;

namespace Logic.Shared.Interfaces
{
    public interface IEmailClient: IDisposable
    {
        Task<bool> TestConnection(EmailProviderSettings settings);
        Task<List<AiEmailTrainingData>> GetEmailAiTrainingDataModel(EmailProviderSettings settings, int maxMessages);


    }
}
