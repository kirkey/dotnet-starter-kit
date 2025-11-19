namespace FSH.Starter.Blazor.Client.Pages.Store.InventoryReservations;

/// <summary>
/// Help dialog providing comprehensive guidance on inventory reservation management.
/// </summary>
public partial class InventoryReservationsHelpDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private Task Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
        return Task.CompletedTask;
    }
}

