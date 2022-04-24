using System.Linq.Expressions;
using Domain.Common;

namespace Infrastructure.Repository;

public interface IRepository<T> where T : AuditableEntity
{
    T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null);
    IEnumerable<T> GetAll(string? includeProperties = null);
    void Add(T entity);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
}