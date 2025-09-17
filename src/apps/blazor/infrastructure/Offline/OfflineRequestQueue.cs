using System.Collections.Concurrent;
using Blazored.LocalStorage;

namespace FSH.Starter.Blazor.Infrastructure.Offline;

public class OfflineRequestQueue(ILocalStorageService localStorage) : IOfflineRequestQueue, IDisposable
{
    private const string StorageKey = "offline-requests";
    private readonly SemaphoreSlim _mutex = new(1,1);
    private readonly ConcurrentQueue<QueuedRequest> _queue = new();
    private bool _loaded;

    public event Action? QueueChanged; // Added event

    public int PendingCount => _queue.Count;

    private async Task EnsureLoadedAsync()
    {
        if (_loaded) return;
        await _mutex.WaitAsync();
        try
        {
            if (_loaded) return;
            var existing = await localStorage.GetItemAsync<List<QueuedRequest>>(StorageKey);
            if (existing is { Count: >0 })
            {
                foreach (var r in existing.OrderBy(r=>r.CreatedUtc)) _queue.Enqueue(r);
            }
            _loaded = true;
        }
        finally { _mutex.Release(); }
    }

    public async Task EnqueueAsync(QueuedRequest request)
    {
        await EnsureLoadedAsync();
        _queue.Enqueue(request);
        await PersistAsync();
        QueueChanged?.Invoke();
    }

    public async Task<IReadOnlyList<QueuedRequest>> GetAllAsync()
    {
        await EnsureLoadedAsync();
        return _queue.ToArray();
    }

    public async Task FlushAsync(Func<QueuedRequest, Task<bool>> sender)
    {
        await EnsureLoadedAsync();
        var requeue = new List<QueuedRequest>();
        bool changed = false;
        while (_queue.TryDequeue(out var item))
        {
            var ok = await sender(item);
            if (!ok) requeue.Add(item); else changed = true;
        }
        foreach (var r in requeue) _queue.Enqueue(r);
        if (changed) await PersistAsync();
        if (changed) QueueChanged?.Invoke();
    }

    public async Task ClearAsync()
    {
        await EnsureLoadedAsync();
        while (_queue.TryDequeue(out _)) { }
        await PersistAsync();
        QueueChanged?.Invoke();
    }

    private async Task PersistAsync()
    {
        await _mutex.WaitAsync();
        try
        {
            await localStorage.SetItemAsync(StorageKey, _queue.ToArray());
        }
        finally { _mutex.Release(); }
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
