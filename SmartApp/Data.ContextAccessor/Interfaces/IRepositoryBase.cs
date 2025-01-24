using Data.Shared;
using System.Linq.Expressions;

namespace Data.ContextAccessor.Interfaces
{
    public interface IRepositoryBase<T> : IDisposable where T : AEntityBase
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetSingle(Expression<Func<T, bool>> predicate, bool asNoTracking = false);
        Task<T?> GetFirstOrDefault(Expression<Func<T, bool>> predicate, bool asNoTracking = false);
        Task<List<T>?> GetAll(Expression<Func<T, bool>> predicate, bool asNoTracking = false);
        Task<bool> AddIfNotExists(T entity, Expression<Func<T, bool>> predicate);
        Task<T> AddOrUpdate(T entity, Expression<Func<T, bool>> predicate);
        Task Add(T entity);
        Task AddRange(IEnumerable<T> entities);
        void UpdateRange(List<T> entities);
        Task<bool> Delete(int id);
        Task SaveChanges();
    }
}
