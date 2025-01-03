using Logic.Modules.EmailCleanerModule.Models;
using Shared.Models.Email;

namespace Logic.Modules.Interfaces
{
    public interface IEmailCleaner: IDisposable
    {
        Task<List<EmailCleanupSettings>> GetEmailCleanerSettings();
        Task<bool> TestAccountConnection(EmailAccountSettings settings);
        Task<EmailCleanupSettings> InitializeAccountInboxSettings(EmailCleanupSettings settings);
        Task AddNewAccountSettings(EmailCleanupSettings settings);
        Task UpdateSettings(EmailCleanupSettings settings);
        Task HandleReportEmailClassification(SpamReport spamReport);
    }
}
