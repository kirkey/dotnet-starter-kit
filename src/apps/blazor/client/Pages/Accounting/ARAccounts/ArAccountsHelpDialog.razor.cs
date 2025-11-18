namespace FSH.Starter.Blazor.Client.Pages.Accounting.ArAccounts;

/// <summary>
/// Help dialog providing comprehensive guidance on accounts receivable management.
/// </summary>
public partial class ArAccountsHelpDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private Task Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
        return Task.CompletedTask;
    }
}

