using Data.Shared;

namespace Data.ContextAccessor.Interfaces
{
    public interface ISettingsRepository: IDisposable
    {
        public RepositoryBase<EmailAccountEntity> EmailAccountRepository { get; }
    }
}
