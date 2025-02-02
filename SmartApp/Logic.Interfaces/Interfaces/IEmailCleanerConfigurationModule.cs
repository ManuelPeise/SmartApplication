using Logic.Interfaces.Models;

namespace Logic.Interfaces.Interfaces
{
    public interface IEmailCleanerConfigurationModule: IDisposable
    {
        Task<EmailClassificationPageModel?> LoadConfigurationData(int accountId);
        Task<bool> UpdateConfigurations(List<EmailClassificationModel> configurations);
    }
}
