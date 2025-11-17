namespace FSH.Starter.Blazor.Client.Pages.Accounting.Payments;

public partial class VoidPaymentDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    [Parameter]
    public DefaultIdType PaymentId { get; set; }

    private string? _reason;

    private async Task Void()
    {
        var command = new VoidPaymentCommand
        {
            PaymentId = PaymentId,
            VoidReason = _reason
        };

        await Client.PaymentVoidEndpointAsync("1", PaymentId, command);
        MudDialog.Close(DialogResult.Ok(true));
    }
}

