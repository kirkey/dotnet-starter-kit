namespace FSH.Starter.Blazor.Client.Pages.Accounting.AccountingPeriods;

/// <summary>
/// Help dialog providing comprehensive guidance on accounting periods management.
/// </summary>
public partial class AccountingPeriodsHelpDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private Task Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
        return Task.CompletedTask;
    }
}

