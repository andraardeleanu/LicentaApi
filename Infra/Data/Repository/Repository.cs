using System.Linq.Expressions;
using Core.Common;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Repository
{
    public class Repository : IRepository
    {
        private readonly AppDbContext _dbContext;
        public Repository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<T> AddAsync<T>(T entity) where T : BaseEntity
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<int> CountAsync<T>() where T : BaseEntity
        {
            return await _dbContext.Set<T>().CountAsync();
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

        public async Task<T> GetByIdAsync<T>(int id, params Expression<Func<T, object>>[] includeProperties) where T : BaseEntity
        {
            var result = _dbContext.Set<T>();

            IQueryable<T>? query = null;
            foreach (var property in includeProperties)
            {
                query = query == null ? result.Include(property) : query.Include(property);
            }

            return query == null ? await result.SingleAsync(e => e.Id == id) : await query.SingleAsync(e => e.Id == id);
        }

        public async Task<List<T>> ListAsync<T>(params Expression<Func<T, object>>[] includeProperties) where T : BaseEntity
        {
            var result = _dbContext.Set<T>();

            IQueryable<T>? query = null;
            foreach (var property in includeProperties)
            {
                query = query == null ? result.Include(property) : query.Include(property);
            }

            return query == null ? await result.ToListAsync() : await query.ToListAsync();
        }

        public async Task<PagedResult<T>> ListPaginatedAsync<T>(int page, int pageSize, params Expression<Func<T, object>>[] includeProperties) where T : BaseEntity
        {
            var result = _dbContext.Set<T>();

            IQueryable<T>? query = null;
            foreach (var property in includeProperties)
            {
                query = query == null ? result.Include(property) : query.Include(property);
            }

            return query == null ? await result.GetPagedAsync(page, pageSize) : await query.GetPagedAsync(page, pageSize);
        }

        public async Task<PagedResult<T>> ListPaginatedAsync<T>(int page, int pageSize, Expression<Func<T, bool>>[] filters, params Expression<Func<T, object>>[] includeProperties) where T : BaseEntity
        {
            var result = _dbContext.Set<T>();

            IQueryable<T>? query = null;
            foreach (var filter in filters)
            {
                query = query == null ? result.Where(filter) : query.Where(filter);
            }

            foreach (var property in includeProperties)
            {
                query = query == null ? result.Include(property) : query.Include(property);
            }

            return query == null ? await result.GetPagedAsync(page, pageSize) : await query.GetPagedAsync(page, pageSize);
        }

        public async Task UpdateAsync<T>(T entity) where T : BaseEntity
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<T>> WhereAsync<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties) where T : BaseEntity
        {
            var result = _dbContext.Set<T>();

            IQueryable<T>? query = null;
            foreach (var property in includeProperties)
            {
                query = query == null ? result.Include(property) : query.Include(property);
            }

            return query == null ? await result.Where(predicate).ToListAsync() : await query.Where(predicate).ToListAsync();
        }
    }
}
