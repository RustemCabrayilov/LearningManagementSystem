using LearningManagementSystem.Persistence.Filters;

namespace LearningManagementSystem.Application.Abstractions.Services.Dean;

public interface IDeanService
{
    Task<DeanResponse> CreateAsync(DeanRequest dto);
    Task<DeanResponse> UpdateAsync(Guid id,DeanRequest dto);
    Task<DeanResponse> RemoveAsync(Guid id);
    Task<DeanResponse> GetAsync(Guid id);
    Task<IList<DeanResponse>> GetAllAsync(RequestFilter? filter);
}