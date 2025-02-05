using LearningManagementSystem.Application.Abstractions.Services.Aws;

namespace LearningManagementSystem.Application.Abstractions.Services.Storage;

public interface IStorage
{
    ValueTask<string> GetFileUrlAsync(string keyOrFileName, string prefix); //key is combined with folder name and filename
    ValueTask<string> UploadFileAsync(FileRequest request);
    ValueTask<string> UpdateFileAsync(FileRequest request, string fileName);
}