using LearningManagementSystem.Application.Abstractions.Services.Aws;
using LearningManagementSystem.Application.Abstractions.Services.Storage;

namespace LearningManagementSystem.Infrastructure.Services.Storage;

public class StorageService : IStorageService
{
    readonly IStorage _storage;

    public StorageService(IStorage storage)
    {
        _storage = storage;
    }

    public async ValueTask<string> GetFileUrlAsync(string keyOrFileName, string prefix)
        => await _storage.GetFileUrlAsync(keyOrFileName, prefix);

    public ValueTask<string> UpdateFileAsync(FileRequest request, string fileName)
        => _storage.UpdateFileAsync(request, fileName);

    public async ValueTask<string> UploadFileAsync(FileRequest request)
        => await _storage.UploadFileAsync(request);
}