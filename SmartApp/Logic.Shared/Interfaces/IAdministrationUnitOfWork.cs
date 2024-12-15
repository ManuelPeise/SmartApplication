using Data.ContextAccessor.Interfaces;
using Data.Shared.Email;
using Data.Shared.Logging;

namespace Logic.Shared.Interfaces
{
    public interface IAdministrationUnitOfWork: IDisposable
    {
        public IRepositoryBase<EmailAccountSettingsEntity> EmailAccountSettingsRepository { get; }
        T? GetValueFromClaims<T>(string key);
        Task AddLogMessage(LogMessageEntity logMessageEntity);
    }
}
