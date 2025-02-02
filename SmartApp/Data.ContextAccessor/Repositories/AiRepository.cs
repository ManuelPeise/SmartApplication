using Data.ContextAccessor.Interfaces;
using Data.Databases;
using Data.Shared.Ai;
using Data.Shared.Logging;
using Data.Shared.Tools;
using Microsoft.AspNetCore.Http;

namespace Data.ContextAccessor.Repositories
{
    public class AiRepository : IAiRepository
    {
        private readonly ApplicationContext _applicationDbContext;
        private readonly AiDbContext _aiDbContext;
        private readonly IHttpContextAccessor _contextAccessor;



        public AiRepository(ApplicationContext applicationDbContext, AiDbContext aiDbContext, IHttpContextAccessor contextAccessor)
        {
            _applicationDbContext = applicationDbContext;
            _aiDbContext = aiDbContext;
            _contextAccessor = contextAccessor;
        }

        public DbContextRepository<EmailDataEntity> EmailDataRepository => new DbContextRepository<EmailDataEntity>(_applicationDbContext, _contextAccessor);
        public DbContextRepository<SpamClassificationTrainingDataEntity> SpamClassificationTrainingDataRepository => new DbContextRepository<SpamClassificationTrainingDataEntity>(_aiDbContext, _contextAccessor);
        public DbContextRepository<AiScore> AiScoreRepository => new DbContextRepository<AiScore>(_aiDbContext, _contextAccessor);
        public DbContextRepository<LogMessageEntity> LogRepository => new DbContextRepository<LogMessageEntity>(_aiDbContext, _contextAccessor);
        public ClaimsAccessor ClaimsAccessor => new ClaimsAccessor();

    }
}
