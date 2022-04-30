using System.Linq.Expressions;
using Application.Common.Models;
using Application.Models;
using Domain.Common;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class Repository<T> : IRepository<T> where T : AuditableEntity
{
    private readonly ApplicationDbContext _db;
    internal DbSet<T> dbSet;

    public Repository(ApplicationDbContext db)
    {
        _db = db;
        dbSet = _db.Set<T>();
    }

    public void Add(T entity)
    {
        dbSet.Add(entity);
    }

    public void Remove(T entity)
    {
        dbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        dbSet.RemoveRange(entities);
    }

    public Task<List<T>> GetAllAsync(PaginationQuery paginationFilter = null)
    {
        if (paginationFilter == null)
        {
            return dbSet.ToListAsync();
        }

        var skip = (paginationFilter.PageNumber -1) * paginationFilter.PageSize;
        return dbSet
            .Skip(skip)
            .Take(paginationFilter.PageSize)
            .ToListAsync();

    }

    public Task<List<T>> GetAllIncludingAsync(PaginationQuery paginationFilter = null, params Expression<Func<T, object>>[] includeProperties)
    {
        var entities = IncludeProperties(includeProperties);

        if (paginationFilter == null)
        {
            return entities.ToListAsync();
        }

        var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
        return entities
            .Skip(skip)
            .Take(paginationFilter.PageSize)
            .ToListAsync();

    }

    public Task<T> GetSingleAsync(int id)
    {
        return dbSet.FirstOrDefaultAsync(t => t.Id == id);
    }

    public Task<T> GetSingleIncludingAsync(int id, params Expression<Func<T, object>>[] includeProperties)
    {
        var entities = IncludeProperties(includeProperties);
        return entities.FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task<List<T>> FindByAsync(Expression<Func<T, bool>> predicate)
    {
        return dbSet.Where(predicate).ToListAsync();
    }


    private IQueryable<T> IncludeProperties(params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> entities = dbSet;
        foreach (var includeProperty in includeProperties)
        {
            entities = entities.Include(includeProperty);
        }
        return entities;
    }

}