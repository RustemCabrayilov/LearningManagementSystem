using LearningManagementSystem.Persistence.Filters;

namespace LearningManagementSystem.Application.Abstractions.Services.StudentExam;

public interface IStudentExamService
{
    Task<StudentExamResponse> CreateAsync(StudentExamRequest dto);
    Task<StudentExamResponse[]> UpdateRangeAsync(StudentExamRequest[] dtos);
    Task<StudentExamResponse> UpdateAsync(Guid id,StudentExamRequest dto);
    Task<StudentExamResponse> RemoveAsync(Guid id);
    Task<StudentExamResponse> GetAsync(Guid id);
    Task<IList<StudentExamResponse>> GetAllAsync(RequestFilter? filter);   
}