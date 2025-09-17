using System.Net.Http;
using System.Text;
using FSH.Starter.Blazor.Infrastructure.Connectivity;
using FSH.Starter.Blazor.Infrastructure.Metering;
using FSH.Starter.Blazor.Infrastructure.Offline;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.Blazor.Infrastructure.Caching;

public class ApiCachingHandler(
    IApiCacheService cache,
    INetworkStatusService network,
    IOfflineRequestQueue offlineQueue,
    ILogger<ApiCachingHandler> logger,
    IUsageMeterService meter)
    : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Ensure network service initialized (idempotent)
        await network.InitializeAsync();
        var cacheKey = $"{request.Method}:{request.RequestUri}";
        var isGet = request.Method == HttpMethod.Get;

        meter.Increment("api.calls");

        if (!network.IsOnline)
        {
            if (isGet)
            {
                var cached = await cache.GetOrAddAsync<CachedResponse>(cacheKey, () => Task.FromResult<CachedResponse?>(null));
                if (cached is not null)
                {
                    logger.LogInformation("Serving cached GET {Url} while offline", request.RequestUri);
                    meter.Increment("api.cache.hit");
                    return new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                    {
                        Content = new StringContent(cached.Content, Encoding.UTF8, cached.MediaType ?? "application/json")
                    };
                }
            }
            else
            {
                // queue non-GET
                string? body = null;
                if (request.Content != null)
                {
                    body = await request.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                }
                var headers = new Dictionary<string,string>();
                foreach (var h in request.Headers)
                {
                    headers[h.Key] = string.Join(',', h.Value);
                }
                if (request.Content?.Headers?.ContentType != null)
                {
                    headers["Content-Type"] = request.Content.Headers.ContentType.ToString();
                }
                await offlineQueue.EnqueueAsync(new QueuedRequest(request.Method.Method, request.RequestUri!.ToString(), body, headers, DateTime.UtcNow));
                logger.LogWarning("Queued {Method} {Url} while offline", request.Method, request.RequestUri);
                meter.Increment("offline.queued");
                // Return synthetic 202 Accepted
                return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted)
                {
                    Content = new StringContent("{\"queued\":true}", Encoding.UTF8, "application/json")
                };
            }
        }

        HttpResponseMessage response;
        try
        {
            response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
        catch (HttpRequestException) when (!network.IsOnline)
        {
            if (isGet)
            {
                var cached = await cache.GetOrAddAsync<CachedResponse>(cacheKey, () => Task.FromResult<CachedResponse?>(null));
                if (cached is not null)
                {
                    meter.Increment("api.cache.hit");
                    return new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                    {
                        Content = new StringContent(cached.Content, Encoding.UTF8, cached.MediaType ?? "application/json")
                    };
                }
            }
            throw;
        }

        if (isGet && response.IsSuccessStatusCode)
        {
            var mediaType = response.Content.Headers.ContentType?.MediaType;
            var contentString = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            await cache.GetOrAddAsync<CachedResponse>(cacheKey, () => Task.FromResult<CachedResponse?>(new CachedResponse(contentString, mediaType)), TimeSpan.FromMinutes(5));
            meter.Increment("api.cache.store");
            // Re-create response content (since it was consumed)
            var newResp = new HttpResponseMessage(response.StatusCode)
            {
                Content = new StringContent(contentString, Encoding.UTF8, mediaType ?? "application/json"),
                ReasonPhrase = response.ReasonPhrase,
                RequestMessage = request
            };
            foreach (var h in response.Headers)
            {
                newResp.Headers.TryAddWithoutValidation(h.Key, h.Value);
            }
            return newResp;
        }

        return response;
    }

    private sealed class CachedResponse(string content, string? mediaType)
    {
        public string Content { get; } = content;
        public string? MediaType { get; } = mediaType;
    }
}
