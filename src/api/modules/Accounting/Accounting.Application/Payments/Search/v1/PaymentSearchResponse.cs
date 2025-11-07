namespace Accounting.Application.Payments.Search.v1;

/// <summary>
/// Response for payment search results.
/// </summary>
public sealed record PaymentSearchResponse
{
    /// <summary>
    /// The payment ID.
    /// </summary>
    public DefaultIdType Id { get; init; }

    /// <summary>
    /// The payment number.
    /// </summary>
    public string PaymentNumber { get; init; } = string.Empty;

    /// <summary>
    /// Optional member ID.
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
    /// Unapplied amount.
    /// </summary>
    public decimal UnappliedAmount { get; init; }

    /// <summary>
    /// Payment method.
    /// </summary>
    public string PaymentMethod { get; init; } = string.Empty;

    /// <summary>
    /// Reference number.
    /// </summary>
    public string? ReferenceNumber { get; init; }

    /// <summary>
    /// Deposit account code.
    /// </summary>
    public string? DepositToAccountCode { get; init; }

    /// <summary>
    /// Description.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Number of allocations.
    /// </summary>
    public int AllocationCount { get; init; }

    /// <summary>
    /// When created.
    /// </summary>
    public DateTime CreatedOn { get; init; }
}

