namespace FSH.Starter.Blazor.Client.Pages.Accounting.GeneralLedgers;

/// <summary>
/// Help dialog providing comprehensive guidance on general ledger management.
/// </summary>
public partial class GeneralLedgersHelpDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private Task Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
        return Task.CompletedTask;
    }
}

