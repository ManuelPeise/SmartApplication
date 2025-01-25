using Data.ContextAccessor.Repositories;
using Data.Shared.Email;
using Data.Shared.Logging;
using Microsoft.Extensions.Options;
using Shared.Models.Identity;

namespace Data.ContextAccessor.Interfaces
{
    public interface IApplicationUnitOfWork : IDisposable
    {
        int CurrentUserId { get; }
        DbContextRepository<LogMessageEntity> LogMessageRepository { get; }
        // email mapping for ai and email cleaner
        DbContextRepository<EmailSubjectEntity> EmailSubjectTable { get; }
        DbContextRepository<EmailAddressEntity> EmailAddressTable { get; }
        DbContextRepository<EmailMappingEntity> EmailMappingTable { get; }
        IdentityRepository IdentityRepository { get; }
        GenericSettingsRepository GenericSettingsRepository { get; }
        ClaimsAccessor ClaimsAccessor { get; }
        bool IsAuthenticated { get; }
        IOptions<SecurityData> SecurityData { get; }
        Task<int> SaveApplicationContextChangesAsync();
        Task<int> SaveIdentityContextChangesAsync();
    }
}
