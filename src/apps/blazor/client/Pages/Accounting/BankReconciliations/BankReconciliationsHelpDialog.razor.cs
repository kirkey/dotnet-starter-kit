namespace FSH.Starter.Blazor.Client.Pages.Accounting.BankReconciliations;

/// <summary>
/// Help dialog providing comprehensive guidance on bank reconciliation processes.
/// </summary>
public partial class BankReconciliationsHelpDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private Task Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
        return Task.CompletedTask;
    }
}

