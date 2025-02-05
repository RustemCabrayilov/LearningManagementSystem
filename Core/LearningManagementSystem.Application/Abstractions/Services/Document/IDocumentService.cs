using LearningManagementSystem.Domain.Enums;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.AspNetCore.Http;

namespace LearningManagementSystem.Application.Abstractions.Services.Document;

public interface IDocumentService
{
    Task<DocumentResponse> CreateAsync(DocumentRequest dto);
    Task<List<DocumentResponse>> CreateByOwnerAsync(DocumentByOwner documentByOwner);
    Task<DocumentResponse> UpdateAsync(Guid id,DocumentRequest dto);
    Task<DocumentResponse> RemoveAsync(Guid id);
    Task<DocumentResponse> GetAsync(Guid id);
    Task<DocumentResponse> GetByOwnerId(Guid ownerId);
    Task<IList<DocumentResponse>> GetAllAsync(RequestFilter? filter);
}