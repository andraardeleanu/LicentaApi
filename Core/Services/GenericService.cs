using System.Linq.Expressions;
using Core.Common;
using Core.Entities;
using Core.Interfaces;
using Core.Services.Interfaces;

namespace Core.Services
{
    public class GenericService<T> : IGenericService, IGenericService<T> where T : BaseEntity
    {
        private readonly IRepository _repository;

        public GenericService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<T> AddAsync(T entity)
        {
            return await _repository.AddAsync(entity);
        }

        public async Task<T1> AddAsync<T1>(T1 entity) where T1 : BaseEntity
        {
            return await _repository.AddAsync(entity);
        }

        public async Task<int> CountAsync()
        {
            return await _repository.CountAsync<T>();
        }

        public async Task<int> CountAsync<T1>() where T1 : BaseEntity
        {
            return await _repository.CountAsync<T>();
        }

        public async Task DeleteAsync(T entity)
        {
            await _repository.DeleteAsync(entity);
        }

        public async Task DeleteAsync<T1>(T1 entity) where T1 : BaseEntity
        {
            await _repository.DeleteAsync(entity);
        }

        public async Task DeleteRangeAsync(IEnumerable<T> entityList)
        {
            await _repository.DeleteRangeAsync(entityList);
        }

        public async Task DeleteRangeAsync<T1>(IEnumerable<T1> entityList) where T1 : BaseEntity
        {
            await _repository.DeleteRangeAsync(entityList);
        }

        public async Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includeProperties)
        {
            return await _repository.GetByIdAsync(id, includeProperties);
        }

        public async Task<T1> GetByIdAsync<T1>(int id, params Expression<Func<T1, object>>[] includeProperties) where T1 : BaseEntity
        {
            return await _repository.GetByIdAsync(id, includeProperties);
        }

        public async Task<List<T>> ListAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            return await _repository.ListAsync(includeProperties);
        }

        public async Task<List<T1>> ListAsync<T1>(params Expression<Func<T1, object>>[] includeProperties) where T1 : BaseEntity
        {
            return await _repository.ListAsync(includeProperties);
        }

        public async Task<PagedResult<T>> ListPaginatedAsync(int page, int pageSize, params Expression<Func<T, object>>[] includeProperties)
        {
            return await _repository.ListPaginatedAsync(page, pageSize, includeProperties);
        }

        public async Task<PagedResult<T>> ListPaginatedAsync(int page, int pageSize, Expression<Func<T, bool>>[] filters, params Expression<Func<T, object>>[] includeProperties)
        {
            return await _repository.ListPaginatedAsync(page, pageSize, filters, includeProperties);
        }

        public async Task<PagedResult<T1>> ListPaginatedAsync<T1>(int page, int pageSize, params Expression<Func<T1, object>>[] includeProperties) where T1 : BaseEntity
        {
            return await _repository.ListPaginatedAsync(page, pageSize, includeProperties);
        }

        public async Task<PagedResult<T1>> ListPaginatedAsync<T1>(int page, int pageSize, Expression<Func<T1, bool>>[] filters, params Expression<Func<T1, object>>[] includeProperties) where T1 : BaseEntity
        {
            return await _repository.ListPaginatedAsync(page, pageSize, filters, includeProperties);
        }

        public async Task UpdateAsync(T entity)
        {
            await _repository.UpdateAsync(entity);
        }

        public async Task UpdateAsync<T1>(T1 entity) where T1 : BaseEntity
        {
            await _repository.UpdateAsync(entity);
        }

        public async Task<List<T>> WhereAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            return await _repository.WhereAsync(predicate, includeProperties);
        }

        public async Task<List<T1>> WhereAsync<T1>(Expression<Func<T1, bool>> predicate, params Expression<Func<T1, object>>[] includeProperties) where T1 : BaseEntity
        {
            return await _repository.WhereAsync(predicate, includeProperties);
        }
    }
}
