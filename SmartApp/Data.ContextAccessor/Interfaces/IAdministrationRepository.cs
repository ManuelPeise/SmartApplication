using Data.Shared.Logging;

namespace Data.ContextAccessor.Interfaces
{
    public interface IAdministrationRepository: IDisposable
    {
        public DbContextRepository<LogMessageEntity> LogMessageRepository { get; }
        public IIdentityRepository IdentityRepository { get;  }
        public IClaimsAccessor ClaimsAccessor { get; }
    }
}
