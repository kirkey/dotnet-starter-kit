namespace Accounting.Application.Payments.Get.v1;

/// <summary>
/// Response containing payment details.
/// </summary>
public sealed record PaymentGetResponse
{
    /// <summary>
    /// The unique identifier of the payment.
    /// </summary>
    public DefaultIdType Id { get; init; }

    /// <summary>
    /// The payment number (receipt number).
    /// </summary>
    public string PaymentNumber { get; init; } = string.Empty;

    /// <summary>
    /// Optional member identifier.
    /// </summary>
    public DefaultIdType? MemberId { get; init; }

    /// <summary>
    /// The payment date.
    /// </summary>
    public DateTime PaymentDate { get; init; }

    /// <summary>
    /// Total payment amount.
    /// </summary>
    public decimal Amount { get; init; }

    /// <summary>
    /// Unapplied amount available for allocation.
    /// </summary>
    public decimal UnappliedAmount { get; init; }

    /// <summary>
    /// Payment method (Cash, Check, EFT, CreditCard, etc.).
    /// </summary>
    public string PaymentMethod { get; init; } = string.Empty;

    /// <summary>
    /// Reference number (check number or transaction ID).
    /// </summary>
    public string? ReferenceNumber { get; init; }

    /// <summary>
    /// Deposit account code.
    /// </summary>
    public string? DepositToAccountCode { get; init; }

    /// <summary>
    /// Payment description.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Additional notes.
    /// </summary>
    public string? Notes { get; init; }

    /// <summary>
    /// Collection of payment allocations to invoices.
    /// </summary>
    public List<PaymentAllocationDto> Allocations { get; init; } = new();

    /// <summary>
    /// When the payment was created.
    /// </summary>
    public DateTime CreatedOn { get; init; }

    /// <summary>
    /// When the payment was last modified.
    /// </summary>
    public DateTime? LastModifiedOn { get; init; }
}

/// <summary>
/// Payment allocation details.
/// </summary>
public sealed record PaymentAllocationDto
{
    /// <summary>
    /// The allocation ID.
    /// </summary>
    public DefaultIdType Id { get; init; }

    /// <summary>
    /// The invoice ID this payment is allocated to.
    /// </summary>
    public DefaultIdType InvoiceId { get; init; }

    /// <summary>
    /// The amount allocated to this invoice.
    /// </summary>
    public decimal Amount { get; init; }
}

