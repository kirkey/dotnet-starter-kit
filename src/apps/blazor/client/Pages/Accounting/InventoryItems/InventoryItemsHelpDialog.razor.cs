namespace FSH.Starter.Blazor.Client.Pages.Accounting.InventoryItems;

/// <summary>
/// Help dialog providing comprehensive guidance on inventory item management.
/// </summary>
public partial class InventoryItemsHelpDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private Task Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
        return Task.CompletedTask;
    }
}

