namespace FSH.Starter.Blazor.Client.Pages.Accounting.Payments;

/// <summary>
/// Dialog for allocating payment amounts to invoices.
/// Currently a placeholder - full implementation coming soon.
/// </summary>
public partial class PaymentAllocationDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    [Parameter]
    public DefaultIdType PaymentId { get; set; }

    [Parameter]
    public decimal UnappliedAmount { get; set; }

    private decimal _allocationAmount;

    private async Task Allocate()
    {
        if (_allocationAmount <= 0 || _allocationAmount > UnappliedAmount)
        {
            Snackbar.Add("Invalid allocation amount", Severity.Warning);
            return;
        }

        try
        {
            // TODO: Implement full allocation logic with invoice selection
            // For now, this is a placeholder that shows the allocation workflow
            Snackbar.Add("Allocation feature coming soon - invoice selection UI in progress", Severity.Info);
            MudDialog.Close(DialogResult.Ok(true));
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error allocating payment: {ex.Message}", Severity.Error);
        }
    }
}
