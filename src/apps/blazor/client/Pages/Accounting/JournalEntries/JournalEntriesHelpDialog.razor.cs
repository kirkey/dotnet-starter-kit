namespace FSH.Starter.Blazor.Client.Pages.Accounting.JournalEntries;

/// <summary>
/// Help dialog providing comprehensive guidance on journal entries.
/// </summary>
public partial class JournalEntriesHelpDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private Task Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
        return Task.CompletedTask;
    }
}

