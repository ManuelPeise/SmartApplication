using Data.Shared.Tools;
using Microsoft.Extensions.Options;
using Shared.Models.Identity;

namespace Data.ContextAccessor.Interfaces
{
    public interface ISettingsRepository : IDisposable
    {
        RepositoryBase<EmailAccountEntity> EmailAccountRepository { get; }
        RepositoryBase<EmailCleanerSettingsEntity> EmailCleanerSettingsRepository { get; }
        RepositoryBase<EmailAddressMappingEntity> EmailAddressMappingRepository { get; }
        RepositoryBase<EmailDataEntity> EmailDataRepository { get; }
        ILogRepository LogRepository { get; }
        IClaimsAccessor ClaimsAccessor { get; }
        IOptions<SecurityData> SecurityData { get; }
        Task SaveChanges();
    }
}
