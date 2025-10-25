namespace FSH.Starter.Blazor.Client.Pages.Store.InventoryReservations;

/// <summary>
/// Dialog component for viewing detailed information about an inventory reservation.
/// Displays all reservation properties including item, warehouse, location, quantity, dates, and status.
/// </summary>
public partial class ReservationDetailsDialog
{
    [Parameter] public DefaultIdType ReservationId { get; set; }
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;

    private InventoryReservationResponse? _reservation;
    private bool _loading = true;
    private string? _errorMessage;

    protected override async Task OnInitializedAsync()
    {
        await LoadReservationAsync();
    }

    /// <summary>
    /// Loads the reservation details from the API.
    /// </summary>
    private async Task LoadReservationAsync()
    {
        try
        {
            _loading = true;
            _reservation = await Client.GetInventoryReservationEndpointAsync("1", ReservationId).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _errorMessage = $"Failed to load reservation details: {ex.Message}";
            Snackbar.Add(_errorMessage, Severity.Error);
        }
        finally
        {
            _loading = false;
        }
    }

    /// <summary>
    /// Gets the appropriate color for displaying reservation status.
    /// </summary>
    private Color GetStatusColor(string status) => status switch
    {
        "Active" => Color.Success,
        "Allocated" => Color.Info,
        "Released" => Color.Default,
        "Cancelled" => Color.Warning,
        "Expired" => Color.Error,
        _ => Color.Default
    };

    /// <summary>
    /// Checks if a reservation has expired based on its expiration date.
    /// </summary>
    private bool IsExpired(DateTime? expirationDate)
    {
        return expirationDate.HasValue && expirationDate.Value < DateTime.UtcNow;
    }

    /// <summary>
    /// Closes the dialog.
    /// </summary>
    private void Cancel()
    {
        MudDialog.Cancel();
    }
}

