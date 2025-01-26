using Logic.Interfaces.Models;

namespace Logic.Interfaces.Interfaces
{
    public interface IEmailAccountInterfaceModule: IDisposable
    {
        Task<List<EmailAccountSettings>> GetEmailAccountSettings();
        Task<bool> UpdateEmailAccountSettings(EmailAccountSettings accountSettings);
        Task<bool> ExcecuteConnectionTest(EmailAccountConnectionTestRequest model);
        Task<bool> ExecuteEmailMappingTableUpdate(string settingsGuid);
    }
}
