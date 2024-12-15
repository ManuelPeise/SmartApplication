using Shared.Models.Administration.Email;

namespace Logic.Shared.Interfaces
{
    public interface IEmailAccountCleanupModule: IDisposable
    {
        Task<EmailAccountSettings?> GetSettings();
        Task UpdateEmailSettings(EmailAccountSettings settings);
    }
}
