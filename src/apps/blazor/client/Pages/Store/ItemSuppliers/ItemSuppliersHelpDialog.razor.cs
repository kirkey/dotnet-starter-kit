namespace FSH.Starter.Blazor.Client.Pages.Store.ItemSuppliers;

/// <summary>
/// Help dialog providing comprehensive guidance on item supplier management.
/// </summary>
public partial class ItemSuppliersHelpDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private Task Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
        return Task.CompletedTask;
    }
}

