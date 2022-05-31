using Application.Models;
using Domain.Common;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repository;

public class Repository<T> : IRepository<T> where T : AuditableEntity
{
    private readonly ApplicationDbContext _db;
    public DbSet<T> DBSet { get; set; }

    public Repository(ApplicationDbContext db)
    {
        _db = db;
        DBSet = _db.Set<T>();
    }

    public async Task AddAsync(T entity)
    {
        await DBSet.AddAsync(entity);
    }

    public async Task AddRangeAsync(List<T> entities)
    {
        await DBSet.AddRangeAsync(entities);
    }

    public void Remove(T entity)
    {
        DBSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        DBSet.RemoveRange(entities);
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>> filter = null)
    {
        if (filter == null)
            return await DBSet.CountAsync();
        return await DBSet.CountAsync(filter);
    }

    public Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, PaginationFilter paginationFilter = null)
    {
        var query = DBSet.AsQueryable();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (paginationFilter == null)
        {
            return query.ToListAsync();
        }

        var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
        return query
            .Skip(skip)
            .Take(paginationFilter.PageSize)
            .ToListAsync();

    }

    public Task<List<T>> GetAllIncludingAsync(Expression<Func<T, bool>> filter = null, PaginationFilter paginationFilter = null, params Expression<Func<T, object>>[] includeProperties)
    {
        var entities = IncludeProperties(includeProperties);

        if (filter != null)
        {
            entities = entities.Where(filter);
        }

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

    public Task<T> GetFirstOrDefaultAsync(int id)
    {
        return DBSet.FirstOrDefaultAsync(t => t.Id == id);
    }

    public Task<T> GetFirstOrDefaultIncludingAsync(int id, params Expression<Func<T, object>>[] includeProperties)
    {
        var entities = IncludeProperties(includeProperties);
        return entities.FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task<T> FindByAsync(Expression<Func<T, bool>> predicate)
    {
        return DBSet.FirstOrDefaultAsync(predicate);
    }

    /// <summary>
    /// Include properties to include
    /// Ex of input: x=> x.Tags
    /// </summary>
    /// <param name="includeProperties">The properties to include</param>
    private IQueryable<T> IncludeProperties(params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> entities = DBSet;
        foreach (var includeProperty in includeProperties)
        {
            entities = entities.Include(includeProperty);
        }
        return entities;
    }



}