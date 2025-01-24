using Data.ContextAccessor.Interfaces;
using Data.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Data.ContextAccessor
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : AEntityBase
    {
        private DbContext _context;
        private IHttpContextAccessor _contextAccessor;
        private bool disposedValue;

        public RepositoryBase(DbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }


        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
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

        public async Task<bool> AddIfNotExists(T entity, Expression<Func<T, bool>> predicate)
        {
            var table = _context.Set<T>();

            var existingEntity = table.FirstOrDefault(predicate);

            if(existingEntity == null)
            {
                await table.AddAsync(entity);

                return true;
            }

            return false;
        }

        public async Task<T> AddOrUpdate(T entity, Expression<Func<T, bool>> predicate)
        {
            var queryable = _context.Set<T>().AsNoTracking().AsQueryable();

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

        public void UpdateRange(List<T> entities)
        {
            foreach (var entity in entities)
            {
                _context.Entry(entity).State = EntityState.Modified;
            }
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

        public async Task SaveChanges()
        {
            var currentUser = _contextAccessor?.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "name")?.Value;

            var modifiedEntries = _context.ChangeTracker.Entries()
              .Where(x => x.State == EntityState.Modified ||
              x.State == EntityState.Added);

            foreach (var entry in modifiedEntries)
            {
                if (entry != null)
                {
                    if (entry.State == EntityState.Added)
                    {
                        ((AEntityBase)entry.Entity).CreatedBy = currentUser ?? "System";
                        ((AEntityBase)entry.Entity).CreatedAt = DateTime.Now;
                        ((AEntityBase)entry.Entity).UpdatedBy = currentUser ?? "System";
                        ((AEntityBase)entry.Entity).UpdatedAt = DateTime.Now;

                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        ((AEntityBase)entry.Entity).UpdatedBy = currentUser ?? "System";
                        ((AEntityBase)entry.Entity).UpdatedAt = DateTime.Now;
                    }
                }
            }

            await _context.SaveChangesAsync();
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
