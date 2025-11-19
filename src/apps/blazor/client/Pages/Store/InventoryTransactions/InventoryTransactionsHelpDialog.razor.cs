namespace FSH.Starter.Blazor.Client.Pages.Store.InventoryTransactions;

/// <summary>
/// Help dialog providing comprehensive guidance on inventory transaction management.
/// </summary>
public partial class InventoryTransactionsHelpDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private Task Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
        return Task.CompletedTask;
    }
}

