namespace FSH.Starter.Blazor.Client.Pages.Store.Items;

/// <summary>
/// Help dialog providing comprehensive guidance on item/SKU management.
/// </summary>
public partial class ItemsHelpDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private Task Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
        return Task.CompletedTask;
    }
}

