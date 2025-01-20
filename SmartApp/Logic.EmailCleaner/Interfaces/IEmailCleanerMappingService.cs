using Data.Shared.Tools;
using Logic.EmailCleaner.Models;


namespace Logic.EmailCleaner.Interfaces
{
    public interface IEmailCleanerMappingService
    {
        Task<List<EmailMappingModel>> GetMappings(int accountId);
        Task<bool> UpdateAllEmailAddressMappings(EmailAccountEntity account, List<string> folders);
        Task<bool> UpdateMappingEntries(List<EmailMappingModel> mappingEntries);
    }
}
