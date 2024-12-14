using LearningManagementSystem.Persistence.Filters;

namespace LearningManagementSystem.Application.Abstractions.Services.Subject;

public interface ISubjectService
{
    Task<SubjectResponse> CreateAsync(SubjectRequest dto);
    Task<SubjectResponse> UpdateAsync(Guid id,SubjectRequest dto);
    Task<SubjectResponse> RemoveAsync(Guid id);
    Task<SubjectResponse> GetAsync(Guid id);
    Task<IList<SubjectResponse>> GetAllAsync(RequestFilter? filter);
}