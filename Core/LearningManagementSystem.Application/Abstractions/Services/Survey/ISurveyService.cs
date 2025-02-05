using LearningManagementSystem.Persistence.Filters;

namespace LearningManagementSystem.Application.Abstractions.Services.Survey;

public interface ISurveyService
{
    Task<SurveyResponse> CreateAsync(SurveyRequest dto);
    Task<SurveyResponse> UpdateAsync(Guid id,SurveyRequest dto);
    Task<SurveyResponse> RemoveAsync(Guid id);
    Task<SurveyResponse> GetAsync(Guid id);
    Task<IList<SurveyResponse>> GetAllAsync(RequestFilter? filter);
    Task<SurveyResponse> Activate(Guid id);
}