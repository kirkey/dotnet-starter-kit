using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Memory;

namespace FSH.Starter.Blazor.Infrastructure.Caching;

public class ApiCacheService(IMemoryCache cache) : IApiCacheService
{
    private readonly ConcurrentDictionary<string, byte> _keys = new();

    public async Task<T?> GetOrAddAsync<T>(string cacheKey, Func<Task<T?>> factory, TimeSpan? absoluteExpire = null) where T : class
    {
        if (cache.TryGetValue(cacheKey, out T? existing)) return existing;
        var created = await factory().ConfigureAwait(false);
        if (created is not null)
        {
            var options = new MemoryCacheEntryOptions();
            if (absoluteExpire.HasValue)
                options.SetAbsoluteExpiration(absoluteExpire.Value);
            cache.Set(cacheKey, created, options);
            _keys.TryAdd(cacheKey, 0);
        }
        return created;
    }

    public void Remove(string cacheKey)
    {
        cache.Remove(cacheKey);
        _keys.TryRemove(cacheKey, out _);
    }

    public void Clear()
    {
        foreach (var key in _keys.Keys)
        {
            cache.Remove(key);
        }
        _keys.Clear();
    }
}
