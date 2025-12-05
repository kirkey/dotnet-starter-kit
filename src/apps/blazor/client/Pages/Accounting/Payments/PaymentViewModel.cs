namespace FSH.Starter.Blazor.Client.Pages.Accounting.Payments;

/// <summary>
/// ViewModel used by the Payments page for add/edit operations.
/// Mirrors the shape of the API's CreatePaymentCommand and UpdatePaymentCommand.
/// </summary>
public sealed class PaymentViewModel
{
    public DefaultIdType? Id { get; set; }

    // Core fields (Create)
    public string PaymentNumber { get; set; } = string.Empty;
    public DefaultIdType? MemberId { get; set; }
    public DateTime? PaymentDate { get; set; } = DateTime.Today;
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string? ReferenceNumber { get; set; }
    public string? DepositToAccountCode { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }

    // Display-only fields mapped from search response
    public decimal UnappliedAmount { get; set; }
    public int AllocationCount { get; set; }
}
