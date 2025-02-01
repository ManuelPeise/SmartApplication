using Logic.Interfaces.Models;

namespace Logic.Interfaces.Interfaces
{
    public interface IEmailAccountInterfaceModule: IDisposable
    {
        Task<List<EmailAccount>> GetEmailAccounts();
        Task<bool> UpdateEmailAccount(EmailAccount account);
        Task<bool> ExcecuteConnectionTest(EmailAccountConnectionData model);
        //Task<bool> ExecuteEmailMappingTableUpdate(string settingsGuid);
    }
}
