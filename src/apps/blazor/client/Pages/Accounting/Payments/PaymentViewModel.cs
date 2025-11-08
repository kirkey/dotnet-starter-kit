namespace FSH.Starter.Blazor.Client.Pages.Accounting.Payments;

/// <summary>
/// View model for payment create/edit operations.
/// </summary>
public class PaymentViewModel
{
    public DefaultIdType Id { get; set; }
    public string? PaymentNumber { get; set; }
    public DefaultIdType? MemberId { get; set; }
    public DateTime? PaymentDate { get; set; } = DateTime.Today;
    public decimal? Amount { get; set; }
    public decimal UnappliedAmount { get; set; }
    public string? PaymentMethod { get; set; } = "Cash";
    public string? ReferenceNumber { get; set; }
    public string? DepositToAccountCode { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }
}

