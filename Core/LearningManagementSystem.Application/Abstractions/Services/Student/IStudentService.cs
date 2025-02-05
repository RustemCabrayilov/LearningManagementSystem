using LearningManagementSystem.Persistence.Filters;

namespace LearningManagementSystem.Application.Abstractions.Services.Student;

public interface IStudentService
{
    Task<StudentResponse> CreateAsync(StudentRequest dto);
    Task<StudentResponse> UpdateAsync(Guid id,StudentRequest dto);
    Task<StudentResponse> RemoveAsync(Guid id);
    Task<StudentResponse> GetAsync(Guid id);
    Task<IList<StudentResponse>> GetAllAsync(RequestFilter? filter);
    Task<StudentResponse> AssignGroupAsync(StudentGroupDto dto);
    Task<StudentResponse> AssignMajorAsync(StudentMajorDto dto);
    Task<StudentResponse> AssignSubjectAsync(StudentSubjectDto dto);
    Task<StudentResponse> AssignExamAsync(StudentExamDto dto);
    Task<StudentResponse> AssignRetakeExamAsync(StudentRetakeExamDto dto);
    Task<StudentGroupDto[]> AssignGroupsAsync(StudentGroupDto[] dtos);
}