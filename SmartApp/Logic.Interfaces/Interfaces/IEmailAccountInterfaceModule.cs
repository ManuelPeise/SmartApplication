using Logic.Interfaces.EmailAccountInterface.Models;

namespace Logic.Interfaces.Interfaces
{
    public interface IEmailAccountInterfaceModule: IDisposable
    {
        Task<List<EmailAccountSettingsUiModel>> GetEmailAccountSettings();
        Task<bool> UpdateEmailAccountSettings(EmailAccountSettings accountSettings);
        Task<bool> ExcecuteConnectionTest(EmailAccountConnectionTestRequest model);
    }
}
