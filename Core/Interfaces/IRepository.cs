using Core.Common;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IRepository
    {
        T GetById<T>(int id) where T : BaseEntity;
        Task<T> GetByIdAsync<T>(int id, params Expression<Func<T, object>>[] includeProperties) where T : BaseEntity;
        Task<T> SingleOrDefault<T>(Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includeProperties) where T : BaseEntity;
        List<T> List<T>() where T : BaseEntity;
        Task<List<T>> ListAsync<T>(params Expression<Func<T, object>>[] includeProperties) where T : BaseEntity;

        Task<PagedResult<T>> ListPaginatedAsync<T>(int page, int pageSize, string filter,
            params Expression<Func<T, object>>[] includeProperties) where T : BaseEntity;

        Task<List<T>> WhereAsync<T>(Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includeProperties) where T : BaseEntity;

        T Add<T>(T entity) where T : BaseEntity;
        Task<T> AddAsync<T>(T entity) where T : BaseEntity;
        Task UpdateAsync<T>(T entity) where T : BaseEntity;
        Task DeleteAsync<T>(T entity) where T : BaseEntity;
        Task DeleteRangeAsync<T>(IEnumerable<T> entityList) where T : BaseEntity;
    }
}
