namespace FSH.Starter.Blazor.Client.Components.ThemeManager;

public partial class ElevationPanel
{
    [Parameter]
    public int Elevation { get; set; }

    [Parameter]
    public int MaxValue { get; set; } = 25;

    [Parameter]
    public EventCallback<int> OnSliderChanged { get; set; }

    protected override async Task OnInitializedAsync()
    {
        if (await ClientPreferences.GetPreference() is not ClientPreference themePreference) themePreference = new ClientPreference();
        Elevation = themePreference.Elevation;
    }

    private async Task ChangedSelection(ChangeEventArgs args)
    {
        Elevation = int.Parse(args?.Value?.ToString() ?? "0");
        await OnSliderChanged.InvokeAsync(Elevation);
        StateHasChanged();
    }
}



