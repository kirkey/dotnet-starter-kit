namespace FSH.Starter.Blazor.Client.Pages.Accounting.PrepaidExpenses;

public partial class PrepaidExpenseAmortizeDialog
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;
    [Parameter] public DefaultIdType PrepaidExpenseId { get; set; }
    [Parameter] public string PrepaidNumber { get; set; } = string.Empty;
    [Parameter] public decimal RemainingAmount { get; set; }

    private decimal _amortizationAmount;
    private DateTime? _postingDate = DateTime.Today;

    private async Task RecordAmortization()
    {
        if (!_postingDate.HasValue || _amortizationAmount <= 0)
        {
            Snackbar.Add("Posting date and amount are required", Severity.Warning);
            return;
        }

        if (_amortizationAmount > RemainingAmount)
        {
            Snackbar.Add($"Amortization amount cannot exceed remaining balance of {RemainingAmount:C2}", Severity.Warning);
            return;
        }

        try
        {
            var cmd = new RecordAmortizationCommand
            {
                Id = PrepaidExpenseId,
                AmortizationAmount = _amortizationAmount,
                PostingDate = _postingDate.Value,
            };
            await Client.PrepaidExpenseRecordAmortizationEndpointAsync("1", PrepaidExpenseId, cmd);
            Snackbar.Add("Amortization recorded successfully", Severity.Success);
            MudDialog.Close(DialogResult.Ok(true));
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error recording amortization: {ex.Message}", Severity.Error);
        }
    }

    private void Cancel() => MudDialog?.Close(DialogResult.Cancel());
}

