namespace Accounting.Application.Payments.Create.v1;

/// <summary>
/// Response returned after successfully creating a payment.
/// </summary>
public sealed record PaymentCreateResponse
{
    /// <summary>
    /// The unique identifier of the created payment.
    /// </summary>
    public DefaultIdType Id { get; init; }

    /// <summary>
    /// The payment number (receipt number).
    /// </summary>
    public string PaymentNumber { get; init; } = string.Empty;

    /// <summary>
    /// The payment amount.
    /// </summary>
    public decimal Amount { get; init; }

    /// <summary>
    /// The unapplied amount available for allocation.
    /// </summary>
    public decimal UnappliedAmount { get; init; }

    /// <summary>
    /// The payment date.
    /// </summary>
    public DateTime PaymentDate { get; init; }

    /// <summary>
    /// The payment method used.
    /// </summary>
    public string PaymentMethod { get; init; } = string.Empty;
}

