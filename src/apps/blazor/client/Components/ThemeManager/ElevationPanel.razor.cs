namespace FSH.Starter.Blazor.Client.Components.ThemeManager;

public partial class ElevationPanel
{
    private ClientPreference _clientPreference = new();

    [Parameter]
    public int Elevation { get; set; }

    [Parameter]
    public int MaxValue { get; set; } = 25;

    [Parameter]
    public EventCallback<int> OnSliderChanged { get; set; }

    [Inject] 
    protected INotificationPublisher Notifications { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        if (await ClientPreferences.GetPreference() is ClientPreference themePreference)
        {
            _clientPreference = themePreference;
            Elevation = _clientPreference.Elevation;
        }
    }

    private async Task ChangedSelection(ChangeEventArgs args)
    {
        Elevation = int.Parse(args?.Value?.ToString() ?? "0");
        _clientPreference.Elevation = Elevation;
        
        // Notify parent component (ThemeDrawer) via callback
        await OnSliderChanged.InvokeAsync(Elevation);
        
        // Publish notification to all subscribers
        await Notifications.PublishAsync(_clientPreference);
        
        StateHasChanged();
    }
}



