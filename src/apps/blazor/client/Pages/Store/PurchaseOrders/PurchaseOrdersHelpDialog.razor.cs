namespace FSH.Starter.Blazor.Client.Pages.Store.PurchaseOrders;

/// <summary>
/// Help dialog providing comprehensive guidance on purchase order management.
/// </summary>
public partial class PurchaseOrdersHelpDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private Task Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
        return Task.CompletedTask;
    }
}

