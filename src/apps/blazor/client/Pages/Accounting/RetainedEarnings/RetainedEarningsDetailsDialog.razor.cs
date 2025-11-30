namespace FSH.Starter.Blazor.Client.Pages.Accounting.RetainedEarnings;

/// <summary>
/// Dialog for displaying detailed retained earnings information.
/// </summary>
public partial class RetainedEarningsDetailsDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    [Parameter]
    public DefaultIdType RetainedEarningsId { get; set; }

    private RetainedEarningsDetailsResponse? _retainedEarnings;
    private bool _loading;
    private ClientPreference _preference = new();

    /// <summary>
    /// Initializes the dialog and loads data.
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        if (await ClientPreferences.GetPreference() is ClientPreference preference)
        {
            _preference = preference;
        }
        
        await LoadDataAsync();
    }

    /// <summary>
    /// Loads the retained earnings details.
    /// </summary>
    private async Task LoadDataAsync()
    {
        _loading = true;
        StateHasChanged();

        try
        {
            _retainedEarnings = await Client.RetainedEarningsGetEndpointAsync("1", RetainedEarningsId);
        }
        finally
        {
            _loading = false;
            StateHasChanged();
        }
    }

    /// <summary>
    /// Closes the dialog.
    /// </summary>
    private void Close()
    {
        MudDialog.Close();
    }

    /// <summary>
    /// Gets the status color.
    /// </summary>
    private static Color GetStatusColor(string? status) => status switch
    {
        "Closed" => Color.Success,
        "Open" => Color.Info,
        "Locked" => Color.Error,
        _ => Color.Default
    };

    /// <summary>
    /// Gets the color style for income display (green for positive, red for negative).
    /// </summary>
    private static string GetIncomeColor(decimal amount)
    {
        return amount >= 0 
            ? "color: var(--mud-palette-success);" 
            : "color: var(--mud-palette-error);";
    }
}
