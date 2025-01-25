using Data.Shared;
using System.Linq.Expressions;

namespace Data.ContextAccessor.Interfaces
{
    public interface IDbContextRepository<T> : IDisposable where T : AEntityBase
    {
        int GetEntityCount(Expression<Func<T, bool>>? predicate);
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<List<T>> GetAllAsyncBy(Expression<Func<T, bool>> predicate);
        Task<T?> GetFirstOrDefault(Expression<Func<T, bool>> predicate, bool asNoTracking = false);
        Task AddAsync(T entity);
        Task<bool> AddIfNotExists(T entity, Expression<Func<T, bool>> predicate);
        void Update(T entity);
        Task AddRange(List<T> entities);
        void Delete(T entity);
        Task SaveChangesAsync();
    }
}
