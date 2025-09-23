namespace FSH.Starter.Blazor.Infrastructure.Connectivity;

public class NetworkStatusService(IJSRuntime js) : INetworkStatusService, IAsyncDisposable
{
    private DotNetObjectReference<NetworkStatusService>? _objRef;
    public bool IsOnline { get; private set; } = true;
    public event Action<bool>? StatusChanged;
    private bool _initialized;

    public async Task InitializeAsync()
    {
        if (_initialized) return;
        _initialized = true;
        _objRef = DotNetObjectReference.Create(this);
        IsOnline = await js.InvokeAsync<bool>("fshNetwork.init", _objRef);
    }

    [JSInvokable]
    public void SetOnlineStatus(bool online)
    {
        if (IsOnline == online) return;
        IsOnline = online;
        StatusChanged?.Invoke(IsOnline);
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_objRef is not null)
            {
                await js.InvokeVoidAsync("fshNetwork.dispose", _objRef);
                _objRef.Dispose();
            }
        }
        catch { /* ignore */ }
    }
}

