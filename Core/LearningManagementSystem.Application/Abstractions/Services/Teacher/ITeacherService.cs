using LearningManagementSystem.Persistence.Filters;

namespace LearningManagementSystem.Application.Abstractions.Services.Teacher;

public interface ITeacherService
{
    Task<TeacherResponse> CreateAsync(TeacherRequest dto);
    Task<TeacherResponse> UpdateAsync(Guid id,TeacherRequest dto);
    Task<TeacherResponse> RemoveAsync(Guid id);
    Task<TeacherResponse> GetAsync(Guid id);
    Task<IList<TeacherResponse>> GetAllAsync(RequestFilter? filter);
    Task<TeacherResponse> AssignGroupAsync(TeacherGroupDto dto);
}