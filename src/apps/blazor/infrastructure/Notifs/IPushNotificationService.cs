namespace FSH.Starter.Blazor.Infrastructure.Notifs;
public interface IPushNotificationService
{
    bool IsSupported { get; }
    Task<bool> RequestPermissionAsync();
    Task SubscribeAsync();
}
public class PushNotificationService : IPushNotificationService
{
    public bool IsSupported => false; // placeholder (needs JS integration)
    public Task<bool> RequestPermissionAsync() => Task.FromResult(false);
    public Task SubscribeAsync() => Task.CompletedTask;
}

