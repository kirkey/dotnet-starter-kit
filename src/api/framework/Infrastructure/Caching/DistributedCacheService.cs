using System.Text;
using System.Text.Json;
using FSH.Framework.Core.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace FSH.Framework.Infrastructure.Caching;

public class DistributedCacheService(IDistributedCache cache, ILogger<DistributedCacheService> logger)
    : ICacheService
{
    public T? Get<T>(string key) =>
        Get(key) is { } data
            ? Deserialize<T>(data)
            : default;

    private byte[]? Get(string key)
    {
        ArgumentNullException.ThrowIfNull(key);

        try
        {
            return cache.Get(key);
        }
        catch
        {
            return null;
        }
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken token = default) =>
        await GetAsync(key, token).ConfigureAwait(false) is { } data
            ? Deserialize<T>(data)
            : default;

    private async Task<byte[]?> GetAsync(string key, CancellationToken token = default)
    {
        try
        {
            return await cache.GetAsync(key, token).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return null;
        }
    }

    public void Refresh(string key)
    {
        try
        {
            cache.Refresh(key);
        }
        catch
        {
            // can be ignored
        }
    }

    public async Task RefreshAsync(string key, CancellationToken token = default)
    {
        try
        {
            await cache.RefreshAsync(key, token).ConfigureAwait(false);
            logger.LogDebug("refreshed cache with key : {Key}", key);
        }
        catch
        {
            // can be ignored
        }
    }

    public void Remove(string key)
    {
        try
        {
            cache.Remove(key);
        }
        catch
        {
            // can be ignored
        }
    }

    public async Task RemoveAsync(string key, CancellationToken token = default)
    {
        try
        {
            await cache.RemoveAsync(key, token).ConfigureAwait(false);
        }
        catch
        {
            // can be ignored
        }
    }

    public void Set<T>(string key, T value, TimeSpan? slidingExpiration = null) =>
        Set(key, Serialize(value), slidingExpiration);

    private void Set(string key, byte[] value, TimeSpan? slidingExpiration = null)
    {
        try
        {
            cache.Set(key, value, GetOptions(slidingExpiration));
            logger.LogDebug("cached data with key : {Key}", key);
        }
        catch
        {
            // can be ignored
        }
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? slidingExpiration = null, CancellationToken cancellationToken = default) =>
        SetAsync(key, Serialize(value), slidingExpiration, cancellationToken);

    private async Task SetAsync(string key, byte[] value, TimeSpan? slidingExpiration = null, CancellationToken token = default)
    {
        try
        {
            await cache.SetAsync(key, value, GetOptions(slidingExpiration), token).ConfigureAwait(false);
            logger.LogDebug("cached data with key : {Key}", key);
        }
        catch
        {
            // can be ignored
        }
    }

    private static byte[] Serialize<T>(T item)
    {
        return Encoding.Default.GetBytes(JsonSerializer.Serialize(item));
    }

    private static T Deserialize<T>(byte[] cachedData)
    {
        return JsonSerializer.Deserialize<T>(Encoding.Default.GetString(cachedData))!;
    }

    private static DistributedCacheEntryOptions GetOptions(TimeSpan? slidingExpiration)
    {
        var options = new DistributedCacheEntryOptions();
        options.SetSlidingExpiration(slidingExpiration ?? TimeSpan.FromMinutes(5));
        options.SetAbsoluteExpiration(TimeSpan.FromMinutes(15));
        return options;
    }
}
