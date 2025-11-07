namespace Accounting.Application.Payments.Update.v1;

/// <summary>
/// Response returned after successfully updating a payment.
/// </summary>
public sealed record PaymentUpdateResponse
{
    /// <summary>
    /// The unique identifier of the updated payment.
    /// </summary>
    public DefaultIdType Id { get; init; }

    /// <summary>
    /// The payment number.
    /// </summary>
    public string PaymentNumber { get; init; } = string.Empty;

    /// <summary>
    /// When the payment was last modified.
    /// </summary>
    public DateTime? LastModifiedOn { get; init; }
}

