using Microsoft.JSInterop;

namespace FSH.Starter.Blazor.Infrastructure.Session;

public class IdleTimerService(IJSRuntime js) : IIdleTimerService
{
    private DotNetObjectReference<IdleTimerService>? _ref;
    private bool _started;
    private int _timeoutSeconds;
    private int _warningSeconds;

    public event Action? TimedOut;
    public event Action<TimeSpan>? Warning;

    public async Task StartAsync(int timeoutSeconds = 900, int warningSeconds = 60)
    {
        _timeoutSeconds = timeoutSeconds;
        _warningSeconds = warningSeconds;
        if (_started) await ResetAsync();
        _ref ??= DotNetObjectReference.Create(this);
        await js.InvokeVoidAsync("fshIdle.start", _ref, timeoutSeconds, warningSeconds);
        _started = true;
    }

    public async Task ResetAsync()
    {
        if (!_started) return;
        await js.InvokeVoidAsync("fshIdle.reset");
    }

    [JSInvokable]
    public void OnWarn(int secondsRemaining)
    {
        Warning?.Invoke(TimeSpan.FromSeconds(secondsRemaining));
    }

    [JSInvokable]
    public void OnTimeout()
    {
        TimedOut?.Invoke();
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_started)
                await js.InvokeVoidAsync("fshIdle.dispose");
        }
        catch { }
        _ref?.Dispose();
    }
}

