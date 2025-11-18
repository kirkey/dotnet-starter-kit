namespace FSH.Starter.Blazor.Client.Pages.Accounting.RecurringJournalEntries;

/// <summary>
/// Help dialog providing comprehensive guidance on recurring journal entries.
/// </summary>
public partial class RecurringJournalEntriesHelpDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private Task Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
        return Task.CompletedTask;
    }
}

