namespace FSH.Starter.Blazor.Client.Pages.Warehouse;

/// <summary>
/// Dialog component for displaying detailed cycle count information including items.
/// </summary>
public partial class CycleCountDetailsDialog
{

    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

    [Parameter] public DefaultIdType CycleCountId { get; set; }

    private CycleCountResponse? _cycleCount;
    private bool _loading;

    protected override async Task OnInitializedAsync()
    {
        await LoadCycleCountAsync();
    }

    /// <summary>
    /// Loads the cycle count details from the API.
    /// </summary>
    private async Task LoadCycleCountAsync()
    {
        _loading = true;
        try
        {
            _cycleCount = await Client.GetCycleCountEndpointAsync("1", CycleCountId).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Failed to load cycle count: {ex.Message}", Severity.Error);
        }
        finally
        {
            _loading = false;
        }
    }

    /// <summary>
    /// Handles when items are changed to reload the cycle count summary.
    /// </summary>
    private async Task HandleItemsChanged()
    {
        await LoadCycleCountAsync();
    }

    /// <summary>
    /// Closes the dialog.
    /// </summary>
    private void Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
    }
}
