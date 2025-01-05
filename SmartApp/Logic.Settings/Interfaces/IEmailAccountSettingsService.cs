using Shared.Enums;
using Shared.Models.Settings.EmailAccountSettings;

namespace Logic.Settings.Interfaces
{
    public interface IEmailAccountSettingsService
    {
        Task<List<EmailAccountSettingsModel>> GetSettings();
        Task<EmailAccountSettingsModel?> SaveOrUpdateConnection(EmailAccountSettingsModel model);
       
       
    }
}
