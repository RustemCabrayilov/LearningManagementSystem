using LearningManagementSystem.Persistence.Filters;

namespace LearningManagementSystem.Application.Abstractions.Services.Result;

public interface IResultService
{
    Task<ResultResponse> CreateAsync(ResultRequest dto);
    Task<ResultResponse> UpdateAsync(Guid id,ResultRequest dto);
    Task<ResultResponse> RemoveAsync(Guid id);
    Task<ResultResponse> GetAsync(Guid id);
    Task<IList<ResultResponse>> GetAllAsync(RequestFilter? filter);
}