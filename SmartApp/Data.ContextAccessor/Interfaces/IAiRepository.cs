using Data.Shared.Ai;
using Data.Shared.Tools;

namespace Data.ContextAccessor.Interfaces
{
    public interface IAiRepository
    {
        RepositoryBase<SpamClassificationTrainingDataEntity> SpamClassificationTrainingDataRepository { get; }
        RepositoryBase<AiScore> AiScoreRepository { get; }
        RepositoryBase<EmailAddressMappingEntity> EmailAddressMappingRepository { get; }
        RepositoryBase<EmailDataEntity> EmailDataRepository { get; }
        ILogRepository LogRepository { get; }
        IClaimsAccessor ClaimsAccessor { get; }
        Task SaveChanges();
    }
}
