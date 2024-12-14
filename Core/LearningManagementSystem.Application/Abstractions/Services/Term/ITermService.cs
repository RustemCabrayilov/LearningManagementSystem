using LearningManagementSystem.Persistence.Filters;

namespace LearningManagementSystem.Application.Abstractions.Services.Term;

public interface ITermService
{
    Task<TermResponse> CreateAsync(TermRequest dto);
    Task<TermResponse> UpdateAsync(Guid id,TermRequest dto);
    Task<TermResponse> RemoveAsync(Guid id);
    Task<TermResponse> GetAsync(Guid id);
    Task<IList<TermResponse>> GetAllAsync(RequestFilter? filter);
}