using Data.AiContext;
using Data.AppContext;
using Data.ContextAccessor.Interfaces;
using Data.Shared;
using Data.Shared.Ai;
using Data.Shared.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Data.ContextAccessor
{
    public class AiRepository: IAiRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly AiDbContext _aiDbContext;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ILogRepository _logRepository;
        private readonly IClaimsAccessor _claimsAccessor;

        public AiRepository(ApplicationDbContext applicationDbContext, AiDbContext aiDbContext, IHttpContextAccessor contextAccessor, ILogRepository logRepository, IClaimsAccessor claimsAccessor)
        {
            _applicationDbContext = applicationDbContext;
            _aiDbContext = aiDbContext;
            _contextAccessor = contextAccessor;
            _logRepository = logRepository;
            _claimsAccessor = claimsAccessor;
            
        }

        public RepositoryBase<EmailDataEntity> EmailDataRepository => new RepositoryBase<EmailDataEntity>(_applicationDbContext, _contextAccessor);
        public RepositoryBase<EmailAddressMappingEntity> EmailAddressMappingRepository => new RepositoryBase<EmailAddressMappingEntity>(_applicationDbContext, _contextAccessor);
        public RepositoryBase<SpamClassificationTrainingDataEntity> SpamClassificationTrainingDataRepository => new RepositoryBase<SpamClassificationTrainingDataEntity>(_aiDbContext, _contextAccessor);
        public RepositoryBase<AiScore> AiScoreRepository => new RepositoryBase<AiScore>(_aiDbContext, _contextAccessor);
        
        public ILogRepository LogRepository => _logRepository;
        public IClaimsAccessor ClaimsAccessor => _claimsAccessor;
        public async Task SaveChanges()
        {
            var currentUser = _contextAccessor?.HttpContext.User.Identity;

            var modifiedEntries = _aiDbContext.ChangeTracker.Entries()
              .Where(x => x.State == EntityState.Modified ||
              x.State == EntityState.Added);

            foreach (var entry in modifiedEntries)
            {
                if (entry != null)
                {
                    if (entry.State == EntityState.Added)
                    {
                        ((AEntityBase)entry.Entity).CreatedBy = currentUser?.Name ?? "System";
                        ((AEntityBase)entry.Entity).CreatedAt = DateTime.Now;
                        ((AEntityBase)entry.Entity).UpdatedBy = currentUser?.Name ?? "System";
                        ((AEntityBase)entry.Entity).UpdatedAt = DateTime.Now;

                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        ((AEntityBase)entry.Entity).UpdatedBy = currentUser?.Name ?? "System";
                        ((AEntityBase)entry.Entity).UpdatedAt = DateTime.Now;
                    }
                }
            }

            await _aiDbContext.SaveChangesAsync();
        }
    }
}
