namespace LearningManagementSystem.Application.Abstractions.Services.Storage.Azure;

public interface IAzureStorage:IStorage
{
    ValueTask<bool> DeleteFileAsync(string containerName, string fileName);

}