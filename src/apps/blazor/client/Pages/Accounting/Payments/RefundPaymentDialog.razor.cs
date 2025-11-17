namespace FSH.Starter.Blazor.Client.Pages.Accounting.Payments;

public partial class RefundPaymentDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    [Parameter]
    public DefaultIdType PaymentId { get; set; }

    private decimal _amount;
    private string? _reference;

    private async Task Refund()
    {
        if (_amount <= 0)
        {
            Snackbar.Add("Refund amount must be greater than zero", Severity.Warning);
            return;
        }

        var command = new RefundPaymentCommand
        {
            PaymentId = PaymentId,
            RefundAmount = _amount,
            RefundReference = _reference
        };

        await Client.PaymentRefundEndpointAsync("1", PaymentId, command);
        MudDialog.Close(DialogResult.Ok(true));
    }
}

