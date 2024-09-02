using Core.Common;
using Core.Entities;
using System.Linq.Expressions;

namespace Core.Services.Interfaces
{
    public interface IGenericService
    {
        Task<T> GetByIdAsync<T>(int id, params Expression<Func<T, object>>[] includeProperties) where T : BaseEntity;
        Task<List<T>> ListAsync<T>(params Expression<Func<T, object>>[] includeProperties) where T : BaseEntity;
        Task<PagedResult<T>> ListPaginatedAsync<T>(int page, int pageSize, params Expression<Func<T, object>>[] includeProperties) where T : BaseEntity;
        Task<PagedResult<T>> ListPaginatedAsync<T>(int page, int pageSize, Expression<Func<T, bool>>[] filters, params Expression<Func<T, object>>[] includeProperties) where T : BaseEntity;

        Task<List<T>> WhereAsync<T>(Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includeProperties) where T : BaseEntity;

        Task<T> AddAsync<T>(T entity) where T : BaseEntity;
        Task UpdateAsync<T>(T entity) where T : BaseEntity;
        Task DeleteAsync<T>(T entity) where T : BaseEntity;
        Task DeleteRangeAsync<T>(IEnumerable<T> entityList) where T : BaseEntity;
        Task<int> CountAsync<T>() where T : BaseEntity;
    }

    public interface IGenericService<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includeProperties);
        Task<List<T>> ListAsync(params Expression<Func<T, object>>[] includeProperties);
        Task<PagedResult<T>> ListPaginatedAsync(int page, int pageSize, params Expression<Func<T, object>>[] includeProperties);
        Task<PagedResult<T>> ListPaginatedAsync(int page, int pageSize, Expression<Func<T, bool>>[] filters, params Expression<Func<T, object>>[] includeProperties);

        Task<List<T>> WhereAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task DeleteRangeAsync(IEnumerable<T> entityList);
        Task<int> CountAsync();
    }
}
