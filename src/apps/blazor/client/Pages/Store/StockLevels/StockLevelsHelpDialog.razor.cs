namespace FSH.Starter.Blazor.Client.Pages.Store.StockLevels;

/// <summary>
/// Help dialog providing comprehensive guidance on stock level management.
/// </summary>
public partial class StockLevelsHelpDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private Task Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
        return Task.CompletedTask;
    }
}

