using LearningManagementSystem.Persistence.Filters;

namespace LearningManagementSystem.Application.Abstractions.Services.Vote;

public interface IVoteService
{
    Task<VoteResponse> CreateAsync(VoteRequest dto);
    Task<VoteResponse[]> CreateAsync(VoteRequest[] dtos);
    Task<VoteResponse> UpdateAsync(Guid id,VoteRequest dto);
    Task<VoteResponse> RemoveAsync(Guid id);
    Task<VoteResponse> GetAsync(Guid id);
    Task<IList<VoteResponse>> GetAllAsync(RequestFilter? filter);
}