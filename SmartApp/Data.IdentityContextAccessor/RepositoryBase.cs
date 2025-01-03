using Data.ContextAccessor.Interfaces;
using Data.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Data.ContextAccessor
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : AEntityBase
    {
        private DbContext _context;
        private bool disposedValue;

        public RepositoryBase(DbContext context)
        {
            _context = context;

        }


        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T?> GetSingle(Expression<Func<T, bool>> predicate, bool asNoTracking = false)
        {
            var table =  _context.Set<T>();

            if (asNoTracking)
            {
               return await table.AsNoTracking().SingleOrDefaultAsync(predicate);
            }

            var entity = await table.SingleOrDefaultAsync(predicate);

            return entity;
        }

        public async Task<T?> GetFirstOrDefault(Expression<Func<T, bool>> predicate, bool asNoTracking = false)
        {
            var table = _context.Set<T>();

            if (asNoTracking) 
            { 
                return table.AsNoTracking().FirstOrDefault(predicate);
            }

            return await table.FirstOrDefaultAsync(predicate, CancellationToken.None);
        }

        public async Task<List<T>?> GetAll(Expression<Func<T, bool>> predicate, bool asNoTracking = false)
        {
            var table = asNoTracking ? _context.Set<T>().AsNoTracking() : _context.Set<T>();

            return await table.Where(predicate).ToListAsync();
        }

        public async Task Add(T entity)
        {
            await _context.AddAsync(entity);
        }

        public async Task<T> AddOrUpdate(T entity, Expression<Func<T, bool>> predicate)
        {
            var queryable = _context.Set<T>().AsQueryable();

            var existing = await queryable.FirstOrDefaultAsync(predicate, CancellationToken.None);

            if (existing != null)
            {
                existing = entity;

                var result = _context.Update<T>(existing);

                return result.Entity;
            }

            var insertResult = await _context.AddAsync(entity);

            return insertResult.Entity;
        }

        public async Task AddRange(IEnumerable<T> entities)
        {
            await _context.AddRangeAsync(entities);
        }

        public async Task<bool> Delete(int id)
        {
            var queryable = _context.Set<T>().AsQueryable();

            var entityToDelete = await queryable.FirstOrDefaultAsync(x => x.Id == id);

            if (entityToDelete == null)
            {
                return false;
            }

            var result = _context.Remove(entityToDelete);

            return result.Entity != null ? true : false;
        }

        #region dispose

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _context.Dispose();
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
