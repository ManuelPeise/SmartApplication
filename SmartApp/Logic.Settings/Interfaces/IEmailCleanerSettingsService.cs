using Shared.Models.Settings.EmailCleanerSettings;

namespace Logic.Settings.Interfaces
{
    public interface IEmailCleanerSettingsService
    {
        Task<EmailCleanerConfiguration?> GetSettings(bool loadMappings);
        Task<bool> SaveOrUpdateEmailCleanerSettings(EmailCleanerSettings settings);
    }
}
