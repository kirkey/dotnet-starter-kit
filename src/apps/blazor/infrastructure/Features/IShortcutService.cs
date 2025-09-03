namespace FSH.Starter.Blazor.Infrastructure.Features;

public interface IShortcutService : IAsyncDisposable
{
    event Action<string>? Triggered; // command id
    void Register(string key, string commandId, string description);
    IReadOnlyList<ShortcutDescriptor> All();
    void Invoke(string key);
    Task InitializeAsync(Microsoft.JSInterop.IJSRuntime js);
}

public record ShortcutDescriptor(string Key, string CommandId, string Description);
