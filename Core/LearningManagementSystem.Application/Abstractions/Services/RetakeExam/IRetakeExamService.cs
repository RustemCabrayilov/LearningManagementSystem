using LearningManagementSystem.Persistence.Filters;

namespace LearningManagementSystem.Application.Abstractions.Services.RetakeExam;

public interface IRetakeExamService
{
    Task<RetakeExamResponse> CreateAsync(RetakeExamRequest dto);
    Task<RetakeExamResponse> UpdateAsync(Guid id,RetakeExamRequest dto);
    Task<RetakeExamResponse> RemoveAsync(Guid id);
    Task<RetakeExamResponse> GetAsync(Guid id);
    Task<IList<RetakeExamResponse>> GetAllAsync(RequestFilter? filter);
}