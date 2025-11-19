namespace FSH.Starter.Blazor.Client.Pages.Store.InventoryTransfers;

/// <summary>
/// Help dialog providing comprehensive guidance on inventory transfer management.
/// </summary>
public partial class InventoryTransfersHelpDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private Task Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
        return Task.CompletedTask;
    }
}

