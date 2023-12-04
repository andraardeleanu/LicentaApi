using Core.Common;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infra.Data
{
    public class EfRepository : IRepository
    {
        private readonly AppDbContext _dbContext;

        public EfRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public T GetById<T>(int id) where T : BaseEntity
        {
            return _dbContext.Set<T>().SingleOrDefault(e => e.Id == id);
        }

        public async Task<T> GetByIdAsync<T>(int id, params Expression<Func<T, object>>[] includeProperties) where T : BaseEntity
        {
            var result = _dbContext.Set<T>();

            IQueryable<T> query = null;
            foreach (var property in includeProperties)
            {
                query = query == null ? result.Include(property) : query.Include(property);
            }

            return query == null ? await result.SingleOrDefaultAsync(e => e.Id == id) : await query.SingleOrDefaultAsync(e => e.Id == id);
        }

        public async Task<T> SingleOrDefault<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties) where T : BaseEntity
        {
            var result = _dbContext.Set<T>();

            IQueryable<T> query = null;
            foreach (var property in includeProperties)
            {
                query = query == null ? result.Include(property) : query.Include(property);
            }

            return query == null ? await result.SingleOrDefaultAsync(predicate) : await query.SingleOrDefaultAsync(predicate);
        }

        public List<T> List<T>() where T : BaseEntity
        {
            return _dbContext.Set<T>().ToList();
        }

        public async Task<List<T>> ListAsync<T>(params Expression<Func<T, object>>[] includeProperties) where T : BaseEntity
        {
            var result = _dbContext.Set<T>();

            IQueryable<T> query = null;
            foreach (var property in includeProperties)
            {
                query = query == null ? result.Include(property) : query.Include(property);
            }

            return query == null ? await result.ToListAsync() : await query.ToListAsync();
        }

        public async Task<PagedResult<T>> ListPaginatedAsync<T>(int page, int pageSize, string filter, params Expression<Func<T, object>>[] includeProperties) where T : BaseEntity
        {
            var result = _dbContext.Set<T>();

            IQueryable<T> query = null;
            foreach (var property in includeProperties)
            {
                query = query == null ? result.Include(property) : query.Include(property);
            }

            return query == null ? await result.GetPagedAsync(page, pageSize) : await query.GetPagedAsync(page, pageSize);
        }

        public async Task<List<T>> WhereAsync<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties) where T : BaseEntity
        {
            var result = _dbContext.Set<T>();

            IQueryable<T> query = null;
            foreach (var property in includeProperties)
            {
                query = query == null ? result.Include(property) : query.Include(property);
            }

            return query == null ? await result.Where(predicate).ToListAsync() : await query.Where(predicate).ToListAsync();
        }

        public T Add<T>(T entity) where T : BaseEntity
        {
            _dbContext.Set<T>().Add(entity);
            _dbContext.SaveChanges();

            return entity;
        }

        public async Task<T> AddAsync<T>(T entity) where T : BaseEntity
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task DeleteAsync<T>(T entity) where T : BaseEntity
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync<T>(IEnumerable<T> entityList) where T : BaseEntity
        {
            _dbContext.Set<T>().RemoveRange(entityList);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync<T>(T entity) where T : BaseEntity
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
