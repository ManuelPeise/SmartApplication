using Data.Shared;
using System.Linq.Expressions;

namespace Data.ContextAccessor.Interfaces
{
    public interface IRepositoryBase<T> : IDisposable where T : AEntityBase
    {
        List<T> Entities { get; }
        Task<T?> GetSingle(Expression<Func<T, bool>> predicate, bool asNoTracking = false);
        Task<T?> GetFirstOrDefault(Expression<Func<T, bool>> predicate, bool asNoTracking = false);
        Task<List<T>?> GetAll(Expression<Func<T, bool>> predicate, bool asNoTracking = false);
        Task<T> AddOrUpdate(T entity, Expression<Func<T, bool>> predicate);
        Task<bool> Delete(int id);
    }
}
