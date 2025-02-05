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
    public async ValueTask<bool> AddRangeAsync(List<T> datas)
    {
        await _dbSet.AddRangeAsync(datas);
        return true;
    }
    public bool Update(T entity)
    {
        var entityEntry = _dbSet.Update(entity);
        return entityEntry.State == EntityState.Modified;
    }
    public  bool UpdateRange(List<T> datas)
    {
         _dbSet.UpdateRange(datas);
        return true;
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
            else if (!string.IsNullOrEmpty(filter.FilterField) && Guid.Empty != filter.FilterGuidValue)
            {
                query = query.Where(e => EF.Property<Guid>(e, filter.FilterField) == filter.FilterGuidValue);

            }
            else if (!string.IsNullOrEmpty(filter.FilterField) && filter.FilterEnumValue!=null)
            {
                query = query.Where(e => EF.Property<Enum>(e, filter.FilterField) == filter.FilterEnumValue);

            }
            if (!string.IsNullOrEmpty(filter.SortField))
            {
                query = filter.IsDescending ? query.OrderByDescending(e => EF.Property<object>(e, filter.SortField)) : query.OrderBy(e => EF.Property<object>(e, filter.SortField));
            }

            if (!filter.AllUsers)
            {
                query = query.Skip((filter.Page - 1) * filter.Count).Take(filter.Count);
            }
        }
        return query;
    }
}