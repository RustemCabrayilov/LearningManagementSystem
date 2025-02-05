namespace LearningManagementSystem.Application.Abstractions.Services.Redis;

public interface IRedisCachingService
{
    T? GetData<T>(string key); 
    void SetData<T>(string key, T value);
}