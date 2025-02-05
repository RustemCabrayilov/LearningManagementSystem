using LearningManagementSystem.Application.Abstractions.Services.User;
using LearningManagementSystem.Persistence.Filters;

namespace LearningManagementSystem.Application.Abstractions.Services.Transcript;

public interface ITranscriptService
{
    Task<TranscriptResponse> CreateAsync(TranscriptRequest dto);
    Task<TranscriptResponse> UpdateAsync(Guid id, TranscriptRequest dto);
    Task<TranscriptResponse> RemoveAsync(Guid id);
    Task<TranscriptResponse> GetAsync(Guid id);
    Task<IList<TranscriptResponse>> GetAllAsync(RequestFilter? filter);
}