using Data.AppContext;
using Data.ContextAccessor.Interfaces;
using Data.Shared;
using Data.Shared.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Data.ContextAccessor
{
    public class LogRepository : ILogRepository
    {
        private bool disposedValue;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IHttpContextAccessor _contextAccessor;

        public LogRepository(ApplicationDbContext context, IHttpContextAccessor contextAccessor)
        {
            _applicationDbContext = context;
            _contextAccessor = contextAccessor;
        }

        public async Task AddMessage(LogMessageEntity message)
        {
            var table = _applicationDbContext.Set<LogMessageEntity>();

            await table.AddAsync(message);
        }

        public async Task DeleteMessages(DateTime from, DateTime? to)
        {
            var table = _applicationDbContext.Set<LogMessageEntity>();
            var toDate = to ?? from;

            var messagesToDelete = await table.Where(msg => msg.TimeStamp.Date >= from.Date && msg.TimeStamp.Date <= toDate.Date).ToListAsync();

            if (messagesToDelete.Any())
            {
                table.RemoveRange(messagesToDelete);
            }

        }

        public async Task<List<LogMessageEntity>> GetAll()
        {
            var table = _applicationDbContext.Set<LogMessageEntity>();

            return await table.ToListAsync();
        }

        public async Task<List<LogMessageEntity>> GetAllAsync(DateTime from, DateTime? to)
        {
            var table = _applicationDbContext.Set<LogMessageEntity>();
            var toDate = to ?? from;

            var messages = await table.Where(msg => msg.TimeStamp.Date >= from.Date && msg.TimeStamp.Date <= toDate.Date).ToListAsync();

            return messages;
        }

        public async Task DeleteMessages(List<int> messageIds)
        {
            var table = _applicationDbContext.Set<LogMessageEntity>();

            var messagesToDelete = await table.Where(x => messageIds.Contains(x.Id)).ToListAsync();

            table.RemoveRange(messagesToDelete);
        }

        public async Task SaveChanges()
        {
            var currentUser = _contextAccessor?.HttpContext.User.Identity;

            var modifiedEntries = _applicationDbContext.ChangeTracker.Entries()
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

            await _applicationDbContext.SaveChangesAsync();
        }


        #region dispose

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _applicationDbContext.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in der Methode "Dispose(bool disposing)" ein.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
