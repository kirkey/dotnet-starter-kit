namespace FSH.Starter.Blazor.Client.Pages.Warehouse.Locations;

/// <summary>
/// Dialog component for displaying detailed warehouse location information.
/// </summary>
public partial class LocationDetailsDialog
{
    /// <summary>
    /// Location identifier to load details for.
    /// </summary>
    [Parameter]
    public DefaultIdType LocationId { get; set; }

    /// <summary>
    /// Dialog instance reference.
    /// </summary>
    [CascadingParameter]
    public IMudDialogInstance MudDialog { get; set; } = null!;

    private GetWarehouseLocationResponse? _location;
    private bool _isLoading = true;

    /// <summary>
    /// Gets the status color based on active status.
    /// </summary>
    private static Color GetStatusColor(bool isActive) => isActive ? Color.Success : Color.Default;

    /// <summary>
    /// Gets the capacity utilization color.
    /// </summary>
    private static Color GetCapacityColor(decimal used, decimal total)
    {
        if (total == 0) return Color.Default;
        var percentage = (used / total) * 100;
        return percentage > 90 ? Color.Error : percentage > 75 ? Color.Warning : Color.Success;
    }

    /// <summary>
    /// Loads location details on component initialization.
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        await LoadLocationDetails();
    }

    /// <summary>
    /// Loads the warehouse location details from the API.
    /// </summary>
    private async Task LoadLocationDetails()
    {
        try
        {
            _isLoading = true;
            _location = await Client.GetWarehouseLocationEndpointAsync("1", LocationId);
            _isLoading = false;
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error loading location details: {ex.Message}", Severity.Error);
            _isLoading = false;
        }
    }

    /// <summary>
    /// Closes the dialog.
    /// </summary>
    private void Cancel()
    {
        MudDialog.Cancel();
    }
}

