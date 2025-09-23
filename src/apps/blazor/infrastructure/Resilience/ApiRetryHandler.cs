namespace FSH.Starter.Blazor.Infrastructure.Resilience;

public class ApiRetryHandler(ILogger<ApiRetryHandler> logger) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        const int maxAttempts = 3;
        int attempt = 0;
        HttpResponseMessage? last = null;
        for (; attempt < maxAttempts; attempt++)
        {
            try
            {
                last = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
                if ((int)last.StatusCode >= 500 || last.StatusCode == HttpStatusCode.RequestTimeout)
                {
                    logger.LogWarning("Server error {Status} on {Url} attempt {Attempt}", (int)last.StatusCode, request.RequestUri, attempt+1);
                }
                else return last;
            }
            catch (HttpRequestException ex) when (attempt < maxAttempts -1)
            {
                logger.LogWarning(ex, "Transient exception on {Url} attempt {Attempt}", request.RequestUri, attempt+1);
            }
            await Task.Delay(TimeSpan.FromMilliseconds(100 * (attempt + 1)), cancellationToken).ConfigureAwait(false);
        }
        return last!;
    }
}

