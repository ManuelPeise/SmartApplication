using Data.Shared.Logging;

namespace Data.ContextAccessor.Interfaces
{
    public interface ILogRepository : IDisposable
    {
        Task AddMessage(LogMessageEntity message);
        Task<List<LogMessageEntity>> GetAll();
        Task<List<LogMessageEntity>> GetAllAsync(DateTime from, DateTime? to);
        Task DeleteMessages(DateTime from, DateTime? to);
        Task DeleteMessages(List<int> messageIds);
        Task SaveChanges();
    }
}
