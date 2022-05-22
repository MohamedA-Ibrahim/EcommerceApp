using Application.Models;
using Domain.Common;
using System.Linq.Expressions;

namespace Infrastructure.Repository;

public interface IRepository<T> where T : AuditableEntity
{
    Task AddAsync(T entity);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
    Task<int> CountAsync();
    Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, PaginationFilter paginationFilter = null);
    Task<List<T>> GetAllIncludingAsync(Expression<Func<T, bool>> filter = null, PaginationFilter paginationFilter = null, params Expression<Func<T, object>>[] includeProperties);
    Task<T> GetFirstOrDefaultAsync(int id);
    Task<T> GetFirstOrDefaultIncludingAsync(int id, params Expression<Func<T, object>>[] includeProperties);

    Task<T> FindByAsync(Expression<Func<T, bool>> predicate);


}