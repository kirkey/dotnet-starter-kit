namespace FSH.Starter.Blazor.Client.Pages.Accounting.TrialBalance;

/// <summary>
/// Help dialog providing comprehensive guidance on trial balance.
/// </summary>
public partial class TrialBalanceHelpDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private Task Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
        return Task.CompletedTask;
    }
}

