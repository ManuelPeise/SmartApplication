using Data.AppContext;
using Data.Shared.Logging;
using Logic.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Logic.Shared.Repositories
{
    public class LogRepository : ILogRepository
    {
        private bool disposedValue;
        private readonly ApplicationDbContext _applicationDbContext;

        public LogRepository(ApplicationDbContext context)
        {
            _applicationDbContext = context;
        }

        public async Task AddMessage(LogMessageEntity message)
        {
            var table = _applicationDbContext.Set<LogMessageEntity>();

            table.Add(message);

            await _applicationDbContext.SaveChangesAsync();
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

            await _applicationDbContext.SaveChangesAsync();
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
