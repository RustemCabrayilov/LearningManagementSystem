using System.Linq.Expressions;
using LearningManagementSystem.Application.Abstractions.Repository;
using LearningManagementSystem.Domain.Entities.Common;
using LearningManagementSystem.Persistence.Context;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.Persistence.Repository;

public class GenericRepository<T>(AppDbContext _dbContext): IGenericRepository<T> where T : BaseEntity,new()
{
    private readonly DbSet<T> _dbSet=_dbContext.Set<T>();
    public async ValueTask<bool> AddAsync(T entity)
    {
        var entityEntry = await _dbContext.AddAsync(entity);
        return entityEntry.State == EntityState.Added;
    }

    public bool Update(T entity)
    {
        var entityEntry = _dbSet.Update(entity);
        return entityEntry.State == EntityState.Modified;
    }

    public bool Remove(T entity)
    {
        var entityEntry =  _dbSet.Remove(entity);
        return entityEntry.State == EntityState.Deleted;
    }

  
    public async Task<T> GetAsync(Expression<Func<T, bool>> predicate,string[] includes)
    {
        var query = _dbSet.AsQueryable().Where(predicate).AsNoTracking();
        foreach (var include in includes)
        {
            query = query.Include(include);
        }
        return await query.FirstOrDefaultAsync();
    }


    public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate, RequestFilter? filter)
    {
        var query = _dbSet.Where(predicate).AsNoTracking();
        if (filter != null)
        {
            if(!string.IsNullOrEmpty(filter.FilterField) && !string.IsNullOrEmpty(filter.FilterValue))
            {
                query = query.Where(e=>EF.Property<string>(e,filter.FilterField) == filter.FilterValue);
            }
            if (!string.IsNullOrEmpty(filter.SortField))
            {
                query = filter.IsDescending ? query.OrderByDescending(e => EF.Property<object>(e, filter.SortField)) : query.OrderBy(e => EF.Property<object>(e, filter.SortField));
            }
            query = query.Skip((filter.Page - 1) * filter.Count).Take(filter.Count);
        }
        return query;
    }
}