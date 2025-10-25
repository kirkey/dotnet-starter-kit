namespace FSH.Starter.Blazor.Client.Pages.Store.InventoryReservations;

/// <summary>
/// Dialog component for releasing an active inventory reservation.
/// Allows the user to provide a reason for releasing the reservation, which returns the quantity to available stock.
/// </summary>
public partial class ReleaseReservationDialog
{
    [Parameter] public DefaultIdType ReservationId { get; set; }
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;

    private InventoryReservationResponse? _reservation;
    private string _releaseReason = string.Empty;
    private bool _loading = true;
    private bool _releasing;
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
            _errorMessage = $"Failed to load reservation: {ex.Message}";
            Snackbar.Add(_errorMessage, Severity.Error);
        }
        finally
        {
            _loading = false;
        }
    }

    /// <summary>
    /// Releases the reservation by calling the API with the provided reason.
    /// </summary>
    private async Task Release()
    {
        if (string.IsNullOrWhiteSpace(_releaseReason))
        {
            Snackbar.Add("Please provide a reason for releasing this reservation", Severity.Warning);
            return;
        }

        try
        {
            _releasing = true;
            _errorMessage = null;

            var command = new ReleaseInventoryReservationCommand
            {
                Id = ReservationId,
                Reason = _releaseReason
            };

            await Client.ReleaseInventoryReservationEndpointAsync("1", ReservationId, command).ConfigureAwait(false);
            
            Snackbar.Add("Reservation released successfully", Severity.Success);
            MudDialog.Close(DialogResult.Ok(true));
        }
        catch (Exception ex)
        {
            _errorMessage = $"Failed to release reservation: {ex.Message}";
            Snackbar.Add(_errorMessage, Severity.Error);
        }
        finally
        {
            _releasing = false;
        }
    }

    /// <summary>
    /// Closes the dialog without releasing the reservation.
    /// </summary>
    private void Cancel()
    {
        MudDialog.Cancel();
    }
}

