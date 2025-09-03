namespace FSH.Starter.Blazor.Infrastructure.Caching;

public interface IApiCacheService
{
    Task<T?> GetOrAddAsync<T>(string cacheKey, Func<Task<T?>> factory, TimeSpan? absoluteExpire = null) where T : class;

    void Remove(string cacheKey);

    void Clear();
}
