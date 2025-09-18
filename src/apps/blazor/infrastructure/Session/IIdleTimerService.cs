namespace FSH.Starter.Blazor.Infrastructure.Session;
public interface IIdleTimerService : IAsyncDisposable
{
    event Action? TimedOut;
    event Action<TimeSpan>? Warning; // time remaining
    Task StartAsync(int timeoutSeconds = 900, int warningSeconds = 60);
    Task ResetAsync();
}

