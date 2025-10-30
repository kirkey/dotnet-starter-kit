namespace FSH.Starter.Blazor.Client.Components.Connectivity;

/// <summary>
/// Component that provides cascading connection state from SignalR messaging hub.
/// Manages the lifecycle of the messaging connection and makes connection state
/// available to child components.
/// </summary>
public sealed partial class MessagingConnection : IDisposable, IAsyncDisposable
{
    private readonly CancellationTokenSource _cts = new();

    /// <summary>
    /// Gets or sets the child content to render within the cascading value.
    /// </summary>
    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    /// <summary>
    /// Gets or sets the connection hub service.
    /// </summary>
    [Inject]
    private IConnectionHubService ConnectionHub { get; set; } = default!;

    /// <summary>
    /// Gets or sets the logger instance.
    /// </summary>
    [Inject]
    private ILogger<MessagingConnection> Logger { get; set; } = default!;

    /// <summary>
    /// Gets the current connection state.
    /// </summary>
    public ConnectionState ConnectionState => ConnectionHub.ConnectionState;

    /// <summary>
    /// Gets the SignalR connection ID.
    /// </summary>
    public string? ConnectionId => ConnectionHub.ConnectionId;

    /// <inheritdoc/>
    public ValueTask DisposeAsync()
    {
        _cts.Cancel();
        _cts.Dispose();
        return ValueTask.CompletedTask;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        try
        {
            // Start the connection hub if not already connected
            if (!ConnectionHub.IsConnected)
            {
                await ConnectionHub.StartAsync();
                Logger.LogInformation("Connection hub initialized");
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to initialize connection hub");
        }
    }
}

