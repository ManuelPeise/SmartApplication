using Data.Shared.Logging;
using Data.Shared.Tools;
using Microsoft.Extensions.Options;
using Shared.Models.Identity;

namespace Data.ContextAccessor.Interfaces
{
    public interface ISettingsRepository : IDisposable
    {
        DbContextRepository<EmailAccountEntity> EmailAccountRepository { get; }
        DbContextRepository<EmailCleanerSettingsEntity> EmailCleanerSettingsRepository { get; }
        DbContextRepository<EmailAddressMappingEntity> EmailAddressMappingRepository { get; }
        DbContextRepository<EmailDataEntity> EmailDataRepository { get; }
        DbContextRepository<LogMessageEntity> LogMessageRepository { get; }
        IClaimsAccessor ClaimsAccessor { get; }
        IOptions<SecurityData> SecurityData { get; }
    }
}
