using LearningManagementSystem.Application.Abstractions.Services.Redis;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace LearningManagementSystem.Infrastructure.Services.Redis;

public class RedisCachingService(IDistributedCache _distributedCache): IRedisCachingService
{
    public T? GetData<T>(string key)
    {
      var data=_distributedCache.GetString(key);
      if (data is null)
      {
          return default(T);   
      }
      return JsonConvert.DeserializeObject<T>(data);
    }

    public void SetData<T>(string key, T value)
    {
        var options = new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        };
        _distributedCache.SetString(key, JsonConvert.SerializeObject(value), options);
        
    }
}