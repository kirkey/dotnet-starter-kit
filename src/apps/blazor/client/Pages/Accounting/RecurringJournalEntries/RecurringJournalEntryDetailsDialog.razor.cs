namespace FSH.Starter.Blazor.Client.Pages.Accounting.RecurringJournalEntries;

public partial class RecurringJournalEntryDetailsDialog
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;
    [Parameter] public RecurringJournalEntryResponse Template { get; set; } = default!;

    private void Cancel() => MudDialog?.Close();

    private Color GetStatusColor(string status) => status switch
    {
        "Draft" => Color.Warning,
        "Approved" => Color.Success,
        "Suspended" => Color.Default,
        _ => Color.Default
    };
}

