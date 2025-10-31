namespace Accounting.Application.WriteOffs.Responses;

/// <summary>
/// Response containing write-off details.
/// </summary>
public record WriteOffResponse
{
    /// <summary>
    /// Write-off unique identifier.
    /// </summary>
    public DefaultIdType Id { get; init; }

    /// <summary>
    /// Unique write-off reference number.
    /// </summary>
    public string ReferenceNumber { get; init; } = string.Empty;

    /// <summary>
    /// Date when write-off is recorded.
    /// </summary>
    public DateTime WriteOffDate { get; init; }

    /// <summary>
    /// Write-off type: BadDebt, CollectionAdjustment, Discount, Other.
    /// </summary>
    public string WriteOffType { get; init; } = string.Empty;

    /// <summary>
    /// Amount being written off.
    /// </summary>
    public decimal Amount { get; init; }

    /// <summary>
    /// Amount recovered if customer later pays.
    /// </summary>
    public decimal RecoveredAmount { get; init; }

    /// <summary>
    /// Whether the write-off has been recovered (fully or partially).
    /// </summary>
    public bool IsRecovered { get; init; }

    /// <summary>
    /// Customer whose account is being written off.
    /// </summary>
    public DefaultIdType? CustomerId { get; init; }

    /// <summary>
    /// Customer name for display purposes.
    /// </summary>
    public string? CustomerName { get; init; }

    /// <summary>
    /// Optional specific invoice being written off.
    /// </summary>
    public DefaultIdType? InvoiceId { get; init; }

    /// <summary>
    /// Invoice number for reference.
    /// </summary>
    public string? InvoiceNumber { get; init; }

    /// <summary>
    /// Accounts receivable account to credit.
    /// </summary>
    public DefaultIdType ReceivableAccountId { get; init; }

    /// <summary>
    /// Bad debt expense account to debit.
    /// </summary>
    public DefaultIdType ExpenseAccountId { get; init; }

    /// <summary>
    /// Optional journal entry created for this write-off.
    /// </summary>
    public DefaultIdType? JournalEntryId { get; init; }

    /// <summary>
    /// Write-off status: Pending, Approved, Posted, Reversed.
    /// </summary>
    public string Status { get; init; } = string.Empty;

    /// <summary>
    /// Approval status: Pending, Approved, Rejected.
    /// </summary>
    public string ApprovalStatus { get; init; } = string.Empty;

    /// <summary>
    /// User who approved the write-off.
    /// </summary>
    public string? ApprovedBy { get; init; }

    /// <summary>
    /// Date when write-off was approved.
    /// </summary>
    public DateTime? ApprovedDate { get; init; }

    /// <summary>
    /// Reason for the write-off.
    /// </summary>
    public string? Reason { get; init; }

    /// <summary>
    /// Optional detailed description.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Optional notes about collection attempts or circumstances.
    /// </summary>
    public string? Notes { get; init; }
}

