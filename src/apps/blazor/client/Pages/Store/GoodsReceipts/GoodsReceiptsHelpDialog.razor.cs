namespace FSH.Starter.Blazor.Client.Pages.Store.GoodsReceipts;

/// <summary>
/// Help dialog providing comprehensive guidance on goods receipt management.
/// </summary>
public partial class GoodsReceiptsHelpDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private Task Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
        return Task.CompletedTask;
    }
}

