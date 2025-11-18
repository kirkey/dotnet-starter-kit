namespace FSH.Starter.Blazor.Client.Pages.Accounting.Payees;

/// <summary>
/// Help dialog providing comprehensive guidance on payee management.
/// </summary>
public partial class PayeesHelpDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private Task Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
        return Task.CompletedTask;
    }
}

