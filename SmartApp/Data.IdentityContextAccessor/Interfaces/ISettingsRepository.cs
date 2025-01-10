using Data.Shared.Tools;

namespace Data.ContextAccessor.Interfaces
{
    public interface ISettingsRepository: IDisposable
    {
        public RepositoryBase<EmailAccountEntity> EmailAccountRepository { get; }
        public RepositoryBase<EmailCleanerSettingsEntity> EmailCleanerSettingsRepository { get; }
        public RepositoryBase<EmailAddressMappingEntity> EmailAddressMappingRepository { get; }
        public ILogRepository LogRepository { get; }
        public IClaimsAccessor ClaimsAccessor { get; }
        Task SaveChanges();
    }
}
