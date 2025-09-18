namespace FSH.Starter.Blazor.Infrastructure.Offline;

public interface IOfflineRequestQueue
{
    int PendingCount { get; }
    event Action? QueueChanged;
    Task EnqueueAsync(QueuedRequest request);
    Task<IReadOnlyList<QueuedRequest>> GetAllAsync();
    Task FlushAsync(Func<QueuedRequest, Task<bool>> sender);
    Task ClearAsync();
}

public record QueuedRequest(string Method, string Url, string? Body, Dictionary<string,string> Headers, DateTime CreatedUtc);
