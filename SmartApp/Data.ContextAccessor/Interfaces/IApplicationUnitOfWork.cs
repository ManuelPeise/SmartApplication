using Data.ContextAccessor.Repositories;
using Data.Shared.Logging;
using Microsoft.Extensions.Options;
using Shared.Models.Identity;

namespace Data.ContextAccessor.Interfaces
{
    public interface IApplicationUnitOfWork : IDisposable
    {
        int CurrentUserId { get; }
        DbContextRepository<LogMessageEntity> LogMessageRepository { get; }
        IdentityRepository IdentityRepository { get; }
        GenericSettingsRepository GenericSettingsRepository { get; }
        ClaimsAccessor ClaimsAccessor { get; }
        IOptions<SecurityData> SecurityData { get; }
    }
}
