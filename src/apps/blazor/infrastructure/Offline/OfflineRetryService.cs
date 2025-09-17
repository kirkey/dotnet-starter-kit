using FSH.Starter.Blazor.Infrastructure.Connectivity;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.Blazor.Infrastructure.Offline;

public interface IOfflineRetryService : IDisposable
{
    Task StartAsync();
    Task StopAsync();
    bool IsRunning { get; }
}

public class OfflineRetryService(
    IOfflineRequestQueue queue,
    INetworkStatusService network,
    HttpClient httpClient,
    ILogger<OfflineRetryService> logger) : IOfflineRetryService
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private Task? _retryTask;
    private readonly SemaphoreSlim _mutex = new(1, 1);
    private bool _disposed;
    
    public bool IsRunning { get; private set; }

    public async Task StartAsync()
    {
        await _mutex.WaitAsync();
        try
        {
            if (IsRunning || _disposed) return;
            
            IsRunning = true;
            _retryTask = RunRetryLoopAsync(_cancellationTokenSource.Token);
            
            // Subscribe to network status changes
            network.StatusChanged += OnNetworkStatusChanged;
            
            logger.LogInformation("Offline retry service started");
        }
        finally
        {
            _mutex.Release();
        }
    }

    public async Task StopAsync()
    {
        await _mutex.WaitAsync();
        try
        {
            if (!IsRunning || _disposed) return;
            
            IsRunning = false;
            network.StatusChanged -= OnNetworkStatusChanged;
            
            await _cancellationTokenSource.CancelAsync();
            
            if (_retryTask != null)
            {
                try
                {
                    await _retryTask;
                }
                catch (OperationCanceledException)
                {
                    // Expected when cancellation is requested
                }
            }
            
            logger.LogInformation("Offline retry service stopped");
        }
        finally
        {
            _mutex.Release();
        }
    }

    private void OnNetworkStatusChanged(bool isOnline)
    {
        if (isOnline && IsRunning)
        {
            logger.LogInformation("Network came back online, triggering immediate retry");
            // The retry loop will pick this up automatically
        }
    }

    private async Task RunRetryLoopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting offline request retry loop");
        
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await network.InitializeAsync();
                
                if (network.IsOnline && queue.PendingCount > 0)
                {
                    logger.LogInformation("Processing {Count} queued requests", queue.PendingCount);
                    
                    await queue.FlushAsync(async queuedRequest =>
                    {
                        try
                        {
                            return await SendQueuedRequestAsync(queuedRequest, cancellationToken);
                        }
                        catch (Exception ex)
                        {
                            logger.LogWarning(ex, "Failed to send queued request {Method} {Url}", 
                                queuedRequest.Method, queuedRequest.Url);
                            return false;
                        }
                    });
                    
                    if (queue.PendingCount == 0)
                    {
                        logger.LogInformation("All queued requests processed successfully");
                    }
                }
                
                // Wait before next retry attempt
                // Use exponential backoff when offline, shorter intervals when online
                var delay = network.IsOnline ? 
                    TimeSpan.FromSeconds(5) :  // Check every 5 seconds when online
                    TimeSpan.FromSeconds(30);  // Check every 30 seconds when offline
                    
                await Task.Delay(delay, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                // Expected when service is stopped
                break;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in offline retry loop");
                // Wait before retrying to avoid tight error loops
                await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);
            }
        }
        
        logger.LogInformation("Offline request retry loop stopped");
    }

    private async Task<bool> SendQueuedRequestAsync(QueuedRequest queuedRequest, CancellationToken cancellationToken)
    {
        try
        {
            using var request = new HttpRequestMessage(new HttpMethod(queuedRequest.Method), queuedRequest.Url);
            
            // Add body if present
            if (!string.IsNullOrEmpty(queuedRequest.Body))
            {
                var contentType = queuedRequest.Headers.TryGetValue("Content-Type", out var ct) 
                    ? ct 
                    : "application/json";
                request.Content = new StringContent(queuedRequest.Body, System.Text.Encoding.UTF8, contentType);
            }
            
            // Add headers (excluding Content-Type and Authorization as they're handled separately)
            foreach (var header in queuedRequest.Headers)
            {
                if (header.Key.Equals("Content-Type", StringComparison.OrdinalIgnoreCase) ||
                    header.Key.Equals("Authorization", StringComparison.OrdinalIgnoreCase))
                    continue;
                    
                request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
            
            using var response = await httpClient.SendAsync(request, cancellationToken);
            
            if (response.IsSuccessStatusCode)
            {
                logger.LogDebug("Successfully sent queued request {Method} {Url}", 
                    queuedRequest.Method, queuedRequest.Url);
                return true;
            }
            else
            {
                logger.LogWarning("Queued request failed with status {StatusCode}: {Method} {Url}", 
                    response.StatusCode, queuedRequest.Method, queuedRequest.Url);
                
                // Don't retry client errors (4xx), but retry server errors (5xx)
                return (int)response.StatusCode < 500;
            }
        }
        catch (HttpRequestException ex)
        {
            logger.LogWarning(ex, "Network error sending queued request {Method} {Url}", 
                queuedRequest.Method, queuedRequest.Url);
            return false;
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            logger.LogWarning("Timeout sending queued request {Method} {Url}", 
                queuedRequest.Method, queuedRequest.Url);
            return false;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            try
            {
                StopAsync().GetAwaiter().GetResult();
            }
            catch { /* ignore disposal errors */ }
            
            _cancellationTokenSource.Dispose();
            _mutex.Dispose();
            _disposed = true;
        }
    }
}
