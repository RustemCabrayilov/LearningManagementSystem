using LearningManagementSystem.Persistence.Filters;

namespace LearningManagementSystem.Application.Abstractions.Services.Faculty;

public interface IFacultyService
{
    Task<FacultyResponse> CreateAsync(FacultyRequest dto);
    Task<FacultyResponse> UpdateAsync(Guid id,FacultyRequest dto);
    Task<FacultyResponse> RemoveAsync(Guid id);
    Task<FacultyResponse> GetAsync(Guid id);
    Task<IList<FacultyResponse>> GetAllAsync(RequestFilter? filter);
}