using System.Linq.Expressions;
using Application.Common.Models;
using Application.Contracts.V1.Requests.Queries;
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

    public async Task AddAsync(T entity)
    {
        await dbSet.AddAsync(entity);
    }

    public void Remove(T entity)
    {
        dbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        dbSet.RemoveRange(entities);
    }

    public async Task<int> CountAsync()
    {
        return await dbSet.CountAsync();
    }

    public Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, PaginationFilter paginationFilter = null)
    {
        var query = dbSet.AsQueryable();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (paginationFilter == null)
        {
            return query.ToListAsync();
        }

        var skip = (paginationFilter.PageNumber -1) * paginationFilter.PageSize;
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

    public Task<T> GetSingleAsync(int id)
    {
        return dbSet.FirstOrDefaultAsync(t => t.Id == id);
    }

    public Task<T> GetSingleIncludingAsync(int id, params Expression<Func<T, object>>[] includeProperties)
    {
        var entities = IncludeProperties(includeProperties);
        return entities.FirstOrDefaultAsync(x => x.Id == id);
    }


    /// <summary>
    /// Include properties to include
    /// Ex of input: x=> x.Tags
    /// </summary>
    /// <param name="includeProperties">The properties to include</param>
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