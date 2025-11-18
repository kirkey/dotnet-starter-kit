namespace FSH.Starter.Blazor.Client.Pages.Accounting.CreditMemos;

/// <summary>
/// Help dialog providing comprehensive guidance on credit memos.
/// </summary>
public partial class CreditMemoHelpDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private Task Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
        return Task.CompletedTask;
    }
}

