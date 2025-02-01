using Data.ContextAccessor.Interfaces;
using Data.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Data.ContextAccessor.Repositories
{

    public class DbContextRepository<T> : IDbContextRepository<T> where T : AEntityBase
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;
        private readonly IHttpContextAccessor _contextAccessor;

        public DbContextRepository(DbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _dbSet = _context.Set<T>();
            _contextAccessor = contextAccessor;
        }

        public int GetEntityCount(Expression<Func<T, bool>>? predicate)
        {
            var table = _context.Set<T>();

            if (predicate != null)
            {
                return table.Where(predicate).Count();
            }

            return table.Count();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<List<T>> GetAllAsyncBy(Expression<Func<T, bool>> predicate)
        {
            var table = _context.Set<T>();

            return await table.AsNoTracking().Where(predicate).ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
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

        public async Task<T> AddAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Added;

            var result = await _dbSet.AddAsync(entity);

            return result.Entity;
        }

        public async Task<bool> AddIfNotExists(T entity, Expression<Func<T, bool>> predicate)
        {
            var table = _context.Set<T>();

            var existingEntity = table.FirstOrDefault(predicate);

            if (existingEntity == null)
            {
                _context.Entry(entity).State |= EntityState.Added;
                await table.AddAsync(entity);

                return true;
            }

            return false;
        }

        public async Task<T> InsertIfNotExists(T entity, Expression<Func<T, bool>> predicate)
        {
            var table = _context.Set<T>();

            var existingEntity = table.FirstOrDefault(predicate);

            if (existingEntity == null)
            {
                _context.Entry(entity).State |= EntityState.Added;
                var result = await table.AddAsync(entity);

                await _context.SaveChangesAsync();

                return result.Entity;
            }

            return existingEntity;
        }

        public void Update(T entity)
        {
            var table = _context.Set<T>();

            _context.Entry(entity).State = EntityState.Modified;

            table.Update(entity);

        }

        public async Task AddRange(List<T> entities)
        {
            entities.ForEach(e => _context.Entry(e).State = EntityState.Added);

            await _context.AddRangeAsync(entities);
        }

        public async Task Delete(T entity)
        {
            if (_context.Entry(entity).State == EntityState.Deleted)
            {
                _dbSet.Attach(entity);
            }

            _dbSet.Remove(entity);
        }

        public async Task DeleteRange(List<T> entities)
        {
            foreach (var entity in entities)
            {
                if (_context.Entry(entity).State == EntityState.Deleted)
                {
                    _dbSet.Attach(entity);
                }
            }

            _dbSet.RemoveRange(entities);
        }

        public async Task SaveChangesAsync()
        {
            var currentUser = _contextAccessor?.HttpContext.User;
            var userName = currentUser?.Claims.FirstOrDefault(x => x.Type == "name")?.Value ?? "System";
            
            var modifiedEntries = _context.ChangeTracker.Entries()
              .Where(x => x.State == EntityState.Modified ||
              x.State == EntityState.Added);

            foreach (var entry in modifiedEntries)
            {
                if (entry != null)
                {
                    if (entry.State == EntityState.Added)
                    {
                        ((AEntityBase)entry.Entity).CreatedBy = userName;
                        ((AEntityBase)entry.Entity).CreatedAt = DateTime.UtcNow;
                        ((AEntityBase)entry.Entity).UpdatedBy = userName;
                        ((AEntityBase)entry.Entity).UpdatedAt = DateTime.UtcNow;

                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        ((AEntityBase)entry.Entity).UpdatedBy = userName;
                        ((AEntityBase)entry.Entity).UpdatedAt = DateTime.UtcNow;
                    }
                }
            }

            await _context.SaveChangesAsync();
        }


        #region dispose

        private bool disposedValue;

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

            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
