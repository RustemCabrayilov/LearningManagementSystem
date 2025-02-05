using LearningManagementSystem.Application.Abstractions.Services.Student;
using LearningManagementSystem.Persistence.Filters;

namespace LearningManagementSystem.Application.Abstractions.Services.StudentRetakeExam;

public interface IStudentRetakeExamService
{
    Task<StudentRetakeExamResponse> CreateAsync(StudentRetakeExamDto dto);
    Task<StudentRetakeExamResponse> UpdateAsync(Guid id,StudentRetakeExamDto dto);
    Task<StudentRetakeExamResponse> RemoveAsync(Guid id);
    Task<StudentRetakeExamResponse> GetAsync(Guid id);
    Task<IList<StudentRetakeExamResponse>> GetAllAsync(RequestFilter? filter);
}