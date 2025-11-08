namespace FSH.Starter.Blazor.Client.Pages.Accounting.JournalEntries.Components;

/// <summary>
/// Component for editing journal entry lines with inline grid editing.
/// Supports adding/removing lines and real-time balance calculation.
/// </summary>
public partial class JournalEntryLineEditor
{
    /// <summary>
    /// The list of journal entry lines to edit.
    /// </summary>
    [Parameter]
    public List<JournalEntryLineViewModel> Lines { get; set; } = [];

    /// <summary>
    /// Event callback when lines are modified.
    /// </summary>
    [Parameter]
    public EventCallback<List<JournalEntryLineViewModel>> LinesChanged { get; set; }

    /// <summary>
    /// Total debit amount across all lines.
    /// </summary>
    private decimal TotalDebits => Lines.Sum(l => l.DebitAmount);

    /// <summary>
    /// Total credit amount across all lines.
    /// </summary>
    private decimal TotalCredits => Lines.Sum(l => l.CreditAmount);

    /// <summary>
    /// Difference between debits and credits.
    /// </summary>
    private decimal Difference => TotalDebits - TotalCredits;

    /// <summary>
    /// Indicates whether the entry is balanced.
    /// </summary>
    private bool IsBalanced => Math.Abs(Difference) < 0.01m;

    /// <summary>
    /// Adds a new line to the entry.
    /// </summary>
    private async Task AddLine()
    {
        Lines.Add(new JournalEntryLineViewModel());
        await LinesChanged.InvokeAsync(Lines);
    }

    /// <summary>
    /// Removes a line from the entry.
    /// </summary>
    private async Task RemoveLine(JournalEntryLineViewModel line)
    {
        Lines.Remove(line);
        await LinesChanged.InvokeAsync(Lines);
    }

    /// <summary>
    /// Handles account selection, capturing the full account details.
    /// </summary>
    private async Task OnAccountSelected(JournalEntryLineViewModel line, ChartOfAccountResponse? account)
    {
        if (account != null)
        {
            line.AccountCode = account.AccountCode ?? string.Empty;
            line.AccountName = account.Name ?? string.Empty;
        }
        else
        {
            // Clear account details if no account is selected
            line.AccountCode = string.Empty;
            line.AccountName = string.Empty;
        }
        
        await LinesChanged.InvokeAsync(Lines);
    }
}

