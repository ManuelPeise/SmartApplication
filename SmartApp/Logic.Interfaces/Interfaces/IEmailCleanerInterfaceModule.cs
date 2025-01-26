using Logic.Interfaces.EmailCleanerInterface.Models;
using Logic.Interfaces.Models;

namespace Logic.Interfaces.Interfaces
{
    public interface IEmailCleanerInterfaceModule:IDisposable
    {
        Task<List<EmailCleanerInterfaceConfigurationUiModel>> GetEmailCleanerConfigurations(bool loadEmails);
        Task<bool> UpdateEmailCleanerConfiguration(EmailCleanerUpdateModel model);
    }
}
