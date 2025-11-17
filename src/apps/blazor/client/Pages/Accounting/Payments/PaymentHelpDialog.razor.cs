namespace FSH.Starter.Blazor.Client.Pages.Accounting.Payments;

/// <summary>
/// Help dialog providing comprehensive guidance on payment management.
/// </summary>
public partial class PaymentHelpDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private async Task Close() => await MudDialog.CloseAsync(DialogResult.Ok(true));
}

