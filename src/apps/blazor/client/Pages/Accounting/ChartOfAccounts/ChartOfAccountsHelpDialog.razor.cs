namespace FSH.Starter.Blazor.Client.Pages.Accounting.ChartOfAccounts;

/// <summary>
/// Help dialog providing comprehensive guidance on chart of accounts management.
/// </summary>
public partial class ChartOfAccountsHelpDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private Task Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
        return Task.CompletedTask;
    }
}

