using System.Linq.Expressions;
using LearningManagementSystem.Domain.Entities.Common;
using LearningManagementSystem.Persistence.Filters;

namespace LearningManagementSystem.Application.Abstractions.Repository;

public interface IGenericRepository<T> where T : BaseEntity, new()
{
    ValueTask<bool> AddAsync(T entity);
    bool Update(T entity);
    bool Remove(T entity);
    Task<T> GetAsync(Expression<Func<T, bool>> predicate, params string[] includes);
    IQueryable<T> GetAll(Expression<Func<T, bool>> predicate, RequestFilter? filter);
}