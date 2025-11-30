namespace FSH.Starter.Blazor.Client.Pages.Accounting.BankReconciliations;

/// <summary>
/// Dialog component for displaying bank reconciliation summary.
/// Shows overview of reconciliation statuses and bank account summaries.
/// </summary>
public partial class BankReconciliationSummaryDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private ReconciliationSummary? Summary { get; set; }
    private ClientPreference _preference = new();

    protected override async Task OnInitializedAsync()
    {
        if (await ClientPreferences.GetPreference() is ClientPreference preference)
        {
            _preference = preference;
        }

        await LoadSummary();
    }

    private async Task LoadSummary()
    {
        try
        {
            // This would call an API endpoint to get summary data
            // For now, we'll create a mock summary
            Summary = new ReconciliationSummary();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error loading summary: {ex.Message}", Severity.Error);
        }
    }

    private Color GetStatusColor(string? status) => status switch
    {
        "Pending" => Color.Default,
        "InProgress" => Color.Info,
        "Completed" => Color.Warning,
        "Approved" => Color.Success,
        _ => Color.Default
    };

    private void Cancel() => MudDialog.Cancel();

    private class ReconciliationSummary
    {
        public int TotalReconciliations { get; set; }
        public int PendingCount { get; set; }
        public int InProgressCount { get; set; }
        public int ApprovedCount { get; set; }
        public List<BankReconciliationResponse> RecentReconciliations { get; set; } = [];
        public List<BankAccountSummaryItem> BankAccountSummaries { get; set; } = [];
    }

    private class BankAccountSummaryItem
    {
        public string AccountName { get; set; } = string.Empty;
        public DateTime? LastReconciliationDate { get; set; }
        public decimal CurrentBalance { get; set; }
        public bool IsCurrentlyReconciled { get; set; }
    }
}

