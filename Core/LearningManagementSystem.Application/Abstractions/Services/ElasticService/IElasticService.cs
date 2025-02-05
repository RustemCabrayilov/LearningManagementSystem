using LearningManagementSystem.Application.Abstractions.Services.OCR;
using LearningManagementSystem.Application.Abstractions.Services.User;
using LearningManagementSystem.Domain.Entities.Identity;

namespace LearningManagementSystem.Application.Abstractions.Services.ElasticService;

public interface IElasticService
{
    Task CreateIndexAsync(string indexName);
    ValueTask<bool> AddOrUpdateAsync(UserResponse request);
    ValueTask<bool> AddOrUpdateBulkAsync(List<UserResponse> requests,string indexName);
    Task<UserResponse> Get(string key);
    Task<List<UserResponse>?> GetAll(string? fieldName,string? fieldValue);
    ValueTask<bool> Remove(string key);
    ValueTask<long?> RemoveAll();
    ValueTask<bool> RemoveIndexAsync(string indexName);
}