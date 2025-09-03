namespace FSH.Starter.Blazor.Infrastructure.Sync;

public interface IBroadcastSyncService : IAsyncDisposable
{
    event Action<string,string>? Received; // channel, payload
    void Publish(string channel, string payload);
    void Initialize();
}

public class BroadcastSyncService : IBroadcastSyncService
{
    private BroadcastChannel? _channel;
    private bool _initialized;
    public event Action<string,string>? Received;
    public void Initialize()
    {
        if (_initialized) return;
        _channel = new BroadcastChannel("fsh-sync");
        _channel.OnMessage = (ch, data) => Received?.Invoke(ch, data);
        _initialized = true;
    }
    public void Publish(string channel, string payload)
    {
        if(!_initialized) Initialize();
        _channel?.PostMessage(channel, payload);
    }
    public ValueTask DisposeAsync()
    {
        _channel?.Dispose();
        return ValueTask.CompletedTask;
    }

    // Minimal JS interop simulation wrapper
    private sealed class BroadcastChannel(string name) : IDisposable
    {
        private readonly string _name = name;
        public Action<string,string>? OnMessage;
        public void PostMessage(string channel, string payload) { OnMessage?.Invoke(channel, payload); }
        public void Dispose() { OnMessage = null; }
    }
}

