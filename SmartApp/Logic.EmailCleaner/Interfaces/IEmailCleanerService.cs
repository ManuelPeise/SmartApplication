using Data.Shared.Tools;
using Logic.EmailCleaner.Models;
using Shared.Models.Settings.EmailAccountMappings;


namespace Logic.EmailCleaner.Interfaces
{
    public interface IEmailCleanerService
    {
        Task<List<EmailAccountModel>> GetSettings();
        Task<List<FolderSettings>> GetUpdatedFolderSettings(int accountId);
        Task<bool> TestConnection(ConnectionTestModel model);
        Task<bool> SaveAccount(EmailAccountModel model);
        Task<bool> UpdateAccount(EmailAccountModel model);
        Task<bool> UpdateSettings(EmailCleanerSettings model);
        //Task<bool> UpdateEmailAddressMappings(List<EmailMappingModel> model);
        Task<bool> UpdateEmailAddressMappingEntries(List<EmailMappingModel> mappingEntries);
    }
}
