using LearningManagementSystem.Persistence.Filters;

namespace LearningManagementSystem.Application.Abstractions.Services.Major;

public interface IMajorService
{
    Task<MajorResponse> CreateAsync(MajorRequest dto);
    Task<MajorResponse> UpdateAsync(Guid id,MajorRequest dto);
    Task<MajorResponse> RemoveAsync(Guid id);
    Task<MajorResponse> GetAsync(Guid id);
    Task<IList<MajorResponse>> GetAllAsync(RequestFilter? filter);
}