using Data.Shared;
using Data.Shared.Logging;
using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Logic.Shared
{
    public abstract class AUnitOfWorkBase
    {
        private readonly DbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogRepository? _logRepository;

        protected AUnitOfWorkBase(DbContext context, IHttpContextAccessor httpContextAccessor, ILogRepository? logRepository)
        {
            _dbContext = context;
            _httpContextAccessor = httpContextAccessor;
            _logRepository = logRepository;
        }

        public async Task LogMessage(LogMessageEntity logMessage)
        {
            if(_logRepository != null)
            {
                await _logRepository.AddMessage(logMessage);
            }
        }

        public async Task<List<LogMessageEntity>> GetLogmessages(DateTime? from, DateTime? to)
        {
            if(_logRepository == null)
            {
                return new List<LogMessageEntity>();
            }

            if (from != null)
            {
                return await _logRepository.GetAllAsync((DateTime)from, to);
            }

            return await _logRepository.GetAll();

        }

        public async Task DeleteLogMessages(DateTime from, DateTime? to)
        {
           if(_logRepository != null)
            {
                await _logRepository.DeleteMessages(from, to);
            }
        }

        public async Task SaveChanges()
        {
            var currentUser = _httpContextAccessor.HttpContext.User.Identity;

           

            var modifiedEntries = _dbContext.ChangeTracker.Entries()
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

            await _dbContext.SaveChangesAsync();
        }
    }
}
