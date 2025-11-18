namespace FSH.Starter.Blazor.Client.Pages.Accounting.ApAccounts;

/// <summary>
/// Help dialog providing comprehensive guidance on accounts payable management.
/// </summary>
public partial class ApAccountsHelpDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private Task Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
        return Task.CompletedTask;
    }
}

