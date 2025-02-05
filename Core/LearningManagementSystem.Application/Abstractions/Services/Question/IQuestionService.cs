using LearningManagementSystem.Application.Abstractions.Services.Major;
using LearningManagementSystem.Persistence.Filters;

namespace LearningManagementSystem.Application.Abstractions.Services.Question;

public interface IQuestionService
{
    Task<QuestionResponse> CreateAsync(QuestionRequest dto);
    Task<QuestionResponse> UpdateAsync(Guid id,QuestionRequest dto);
    Task<QuestionResponse> RemoveAsync(Guid id);
    Task<QuestionResponse> GetAsync(Guid id);
    Task<IList<QuestionResponse>> GetAllAsync(RequestFilter? filter);
}