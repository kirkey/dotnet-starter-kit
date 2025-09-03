using Microsoft.JSInterop;

namespace FSH.Starter.Blazor.Infrastructure.Features;

public class ShortcutService : IShortcutService, IAsyncDisposable
{
    private readonly Dictionary<string, ShortcutDescriptor> _map = new(StringComparer.OrdinalIgnoreCase);
    public event Action<string>? Triggered;
    private DotNetObjectReference<ShortcutService>? _ref;
    private IJSRuntime? _js;
    private bool _initialized;

    public IReadOnlyList<ShortcutDescriptor> All() => _map.Values.ToList();

    public void Register(string key, string commandId, string description)
    {
        _map[key] = new ShortcutDescriptor(key, commandId, description);
    }

    public void Invoke(string key)
    {
        if (_map.TryGetValue(key, out var desc))
        {
            Triggered?.Invoke(desc.CommandId);
        }
    }

    public async Task InitializeAsync(IJSRuntime js)
    {
        if (_initialized) return;
        _initialized = true;
        _js = js;
        _ref = DotNetObjectReference.Create(this);
        await js.InvokeVoidAsync("fshShortcuts.init", _ref, _map.Keys.ToArray());
    }

    [JSInvokable]
    public void OnShortcut(string key)
    {
        Invoke(key);
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_initialized && _js is not null && _ref is not null)
            {
                await _js.InvokeVoidAsync("fshShortcuts.dispose");
                _ref.Dispose();
            }
        }
        catch { }
    }
}

