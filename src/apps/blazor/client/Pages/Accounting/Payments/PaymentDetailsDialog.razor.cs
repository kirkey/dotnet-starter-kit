namespace FSH.Starter.Blazor.Client.Pages.Accounting.Payments;

/// <summary>
/// Dialog for displaying payment details including allocations and history.
/// </summary>
public partial class PaymentDetailsDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    [Parameter]
    public dynamic? Payment { get; set; }

    private void Close() => MudDialog.Close();
}

