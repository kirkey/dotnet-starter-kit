namespace FSH.Starter.Blazor.Client.Pages.Store.StockAdjustments;

/// <summary>
/// Help dialog providing comprehensive guidance on stock adjustment management.
/// </summary>
public partial class StockAdjustmentsHelpDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private Task Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
        return Task.CompletedTask;
    }
}

