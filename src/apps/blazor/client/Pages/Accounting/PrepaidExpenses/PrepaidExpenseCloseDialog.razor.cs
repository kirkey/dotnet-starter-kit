namespace FSH.Starter.Blazor.Client.Pages.Accounting.PrepaidExpenses;

public partial class PrepaidExpenseCloseDialog
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;
    [Parameter] public DefaultIdType PrepaidExpenseId { get; set; }
    [Parameter] public string PrepaidNumber { get; set; } = string.Empty;

    private DateTime? _closeDate = DateTime.Today;
    private string? _reason;

    private async Task ClosePrepaidExpense()
    {
        if (!_closeDate.HasValue)
        {
            Snackbar.Add("Close date is required", Severity.Warning);
            return;
        }

        try
        {
            var cmd = new ClosePrepaidExpenseCommand
            {
                Id = PrepaidExpenseId
            };
            await Client.PrepaidExpenseCloseEndpointAsync("1", PrepaidExpenseId, cmd);
            Snackbar.Add("Prepaid expense closed successfully", Severity.Success);
            MudDialog.Close(DialogResult.Ok(true));
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error closing prepaid expense: {ex.Message}", Severity.Error);
        }
    }

    private void Cancel() => MudDialog?.Close(DialogResult.Cancel());
}

