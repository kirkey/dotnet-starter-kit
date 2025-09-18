namespace FSH.Starter.Blazor.Infrastructure.Connectivity;

public interface INetworkStatusService
{
    bool IsOnline { get; }
    event Action<bool>? StatusChanged;
    Task InitializeAsync();
}

