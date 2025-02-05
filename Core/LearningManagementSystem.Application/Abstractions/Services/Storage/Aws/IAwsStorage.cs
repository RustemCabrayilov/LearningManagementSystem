using Amazon.S3;
using LearningManagementSystem.Application.Abstractions.Services.Storage;
using Microsoft.AspNetCore.Http;

namespace LearningManagementSystem.Application.Abstractions.Services.Storage.Aws;

public interface IAwsStorage:IStorage
{
     ValueTask<bool> DeleteFileAsync(string key);
     Task<IEnumerable<object>> GetAllFilesAsync(string bucketName, string? prefix);
    Task<List<string>> GetAllBucketsAsync();
}