namespace FSH.Starter.Blazor.Client.Components.ThemeManager;

public partial class RadiusPanel
{
    private ClientPreference _clientPreference = new();

    [Parameter]
    public double Radius { get; set; }

    [Parameter]
    public double MaxValue { get; set; } = 25;

    [Parameter]
    public EventCallback<double> OnSliderChanged { get; set; }

    [Inject] 
    protected INotificationPublisher Notifications { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        if (await ClientPreferences.GetPreference() is ClientPreference themePreference)
        {
            _clientPreference = themePreference;
            Radius = _clientPreference.BorderRadius;
        }
    }

    private async Task ChangedSelection(ChangeEventArgs args)
    {
        Radius = int.Parse(args?.Value?.ToString() ?? "0");
        _clientPreference.BorderRadius = Radius;
        
        // Notify parent component (ThemeDrawer) via callback
        await OnSliderChanged.InvokeAsync(Radius);
        
        // Publish notification to all subscribers
        await Notifications.PublishAsync(_clientPreference);
        
        StateHasChanged();
    }
}
