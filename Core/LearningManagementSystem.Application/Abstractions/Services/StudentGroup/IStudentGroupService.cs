using LearningManagementSystem.Application.Abstractions.Services.Student;
using LearningManagementSystem.Persistence.Filters;

namespace LearningManagementSystem.Application.Abstractions.Services.StudentGroup;

public interface IStudentGroupService
{
    Task<StudentGroupResponse> CreateAsync(StudentGroupDto dto);
    Task<StudentGroupResponse> UpdateAsync(Guid id,StudentGroupDto dto);
    Task<StudentGroupResponse> RemoveAsync(Guid id);
    Task<StudentGroupResponse> GetAsync(Guid id);
    Task<IList<StudentGroupResponse>> GetAllAsync(RequestFilter? filter);      
}