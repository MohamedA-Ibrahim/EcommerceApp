using System.Linq.Expressions;
using Application.Contracts.V1.Requests.Queries;
using Application.Models;
using Domain.Common;

namespace Infrastructure.Repository;

public interface IRepository<T> where T : AuditableEntity
{
    Task AddAsync(T entity);
    Task<List<T>> FindByAsync(Expression<Func<T, bool>> predicate);
    Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, PaginationQuery paginationFilter = null);
    Task<List<T>> GetAllIncludingAsync(Expression<Func<T, bool>> filter = null, PaginationQuery paginationFilter = null, params Expression<Func<T, object>>[] includeProperties);
    Task<T> GetSingleAsync(int id);
    Task<T> GetSingleIncludingAsync(int id, params Expression<Func<T, object>>[] includeProperties);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
}