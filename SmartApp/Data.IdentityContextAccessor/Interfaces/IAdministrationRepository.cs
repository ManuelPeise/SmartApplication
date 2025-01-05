using Data.Shared.AccessRights;
using Data.Shared.Logging;

namespace Data.ContextAccessor.Interfaces
{
    public interface IAdministrationRepository: IDisposable
    {
        public RepositoryBase<LogMessageEntity> LogMessageRepository { get; }
        public RepositoryBase<AccessRightEntity> AccessRightRepository { get; }
        public RepositoryBase<UserAccessRightEntity> UserAccessRightRepository { get; }
        public IIdentityRepository IdentityRepository { get;  }
        public ILogRepository LogRepository { get; }
    }
}
