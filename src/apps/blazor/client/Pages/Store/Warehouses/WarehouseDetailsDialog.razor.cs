namespace FSH.Starter.Blazor.Client.Pages.Store.Warehouses;

/// <summary>
/// Dialog component for displaying detailed warehouse information.
/// </summary>
public partial class WarehouseDetailsDialog
{
    /// <summary>
    /// Warehouse identifier to load details for.
    /// </summary>
    [Parameter]
    public DefaultIdType WarehouseId { get; set; }

    /// <summary>
    /// Dialog instance reference.
    /// </summary>
    [CascadingParameter]
    public IMudDialogInstance MudDialog { get; set; } = null!;

    private WarehouseResponse? _warehouse;
    private bool _isLoading = true;

    /// <summary>
    /// Gets the status color based on active status.
    /// </summary>
    private static Color GetStatusColor(bool isActive) => isActive ? Color.Success : Color.Default;

    /// <summary>
    /// Loads warehouse details on component initialization.
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        await LoadWarehouseDetails();
    }

    /// <summary>
    /// Loads the warehouse details from the API.
    /// </summary>
    private async Task LoadWarehouseDetails()
    {
        try
        {
            _isLoading = true;
            _warehouse = await Client.GetWarehouseEndpointAsync("1", WarehouseId);
            _isLoading = false;
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error loading warehouse details: {ex.Message}", Severity.Error);
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

