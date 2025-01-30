using Logic.Interfaces.EmailCleanerInterface.Models;
using Logic.Interfaces.Models;

namespace Logic.Interfaces.Interfaces
{
    public interface IEmailCleanerInterfaceModule:IDisposable
    {
        Task<List<EmailCleanerInterfaceConfigurationUiModel>> GetEmailCleanerConfigurations(bool loadEmails);
        Task<EmailCleanerMappingData<EmailFolderMappingData>?> GetFolderMappingData(string settingsGuid);

        // Task<EmailCleanerMappingData?> GetMappingData(string settingsGuid);
        Task<bool> UpdateEmailCleanerConfiguration(EmailCleanerUpdateModel model);
        Task<bool> UpdateFolderMappings(EmailFolderMappingUpdate folderMappingUpdate);
    }
}
