using Logic.Interfaces.Models;

namespace Logic.Interfaces.Interfaces
{
    public interface IEmailCleanerInterfaceModule:IDisposable
    {
        Task<List<EmailCleanerUiSettings>> GetEmailCleanerSettings();
        Task<bool> UpdateEmailCleanerSetting(EmailCleanerUiSettings model);
    }
}
