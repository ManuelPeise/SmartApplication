using Data.Shared.AccessRights;
using Data.Shared.Logging;

namespace Data.ContextAccessor.Interfaces
{
    public interface IAdministrationRepository: IDisposable
    {
        public RepositoryBase<LogMessageEntity> LogMessageRepository { get; }
        public IIdentityRepository IdentityRepository { get;  }
        public ILogRepository LogRepository { get; }
        public IClaimsAccessor ClaimsAccessor { get; }
        Task SaveChanges();
    }
}
