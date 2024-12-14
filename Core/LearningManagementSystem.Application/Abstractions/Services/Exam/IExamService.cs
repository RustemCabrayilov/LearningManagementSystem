using LearningManagementSystem.Persistence.Filters;

namespace LearningManagementSystem.Application.Abstractions.Services.Exam;

public interface IExamService
{
    Task<ExamResponse> CreateAsync(ExamRequest dto);
    Task<ExamResponse> UpdateAsync(Guid id,ExamRequest dto);
    Task<ExamResponse> RemoveAsync(Guid id);
    Task<ExamResponse> GetAsync(Guid id);
    Task<IList<ExamResponse>> GetAllAsync(RequestFilter? filter);
}