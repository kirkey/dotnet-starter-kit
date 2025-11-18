namespace FSH.Starter.Blazor.Client.Pages.Accounting.DebitMemos;

/// <summary>
/// Help dialog providing comprehensive guidance on debit memos.
/// </summary>
public partial class DebitMemoHelpDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private Task Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
        return Task.CompletedTask;
    }
}

