﻿using Data.Shared.Ai;
using Data.Shared.Logging;
using Data.Shared.Tools;

namespace Data.ContextAccessor.Interfaces
{
    public interface IAiRepository
    {
        DbContextRepository<SpamClassificationTrainingDataEntity> SpamClassificationTrainingDataRepository { get; }
        DbContextRepository<AiScore> AiScoreRepository { get; }
        DbContextRepository<EmailAddressMappingEntity> EmailAddressMappingRepository { get; }
        DbContextRepository<EmailDataEntity> EmailDataRepository { get; }
        DbContextRepository<LogMessageEntity> LogRepository { get; }
        IClaimsAccessor ClaimsAccessor { get; }
    }
}
