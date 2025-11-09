namespace FSH.Starter.Blazor.Client.Pages.Accounting.PrepaidExpenses;

public partial class PrepaidExpenseDetailsDialog
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;
    [Parameter] public PrepaidExpenseResponse Expense { get; set; } = default!;

    private void Cancel() => MudDialog?.Close();

    private Color GetStatusColor(string status) => status switch
    {
        "Active" => Color.Success,
        "FullyAmortized" => Color.Info,
        "Closed" => Color.Default,
        "Cancelled" => Color.Error,
        _ => Color.Default
    };
}

