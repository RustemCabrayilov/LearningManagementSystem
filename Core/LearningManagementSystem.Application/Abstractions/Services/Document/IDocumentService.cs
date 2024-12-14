using LearningManagementSystem.Persistence.Filters;

namespace LearningManagementSystem.Application.Abstractions.Services.Document;

public interface IDocumentService
{
    Task<DocumentResponse> CreateAsync(DocumentRequest dto);
    Task<DocumentResponse> UpdateAsync(Guid id,DocumentRequest dto);
    Task<DocumentResponse> RemoveAsync(Guid id);
    Task<DocumentResponse> GetAsync(Guid id);
    Task<IList<DocumentResponse>> GetAllAsync(RequestFilter? filter);
}