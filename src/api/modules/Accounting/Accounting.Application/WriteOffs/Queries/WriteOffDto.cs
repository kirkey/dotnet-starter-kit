namespace Accounting.Application.WriteOffs.Queries;

/// <summary>
/// Write-off data transfer object for list views.
/// </summary>
public record WriteOffDto
{
    public DefaultIdType Id { get; init; }
    public string ReferenceNumber { get; init; } = string.Empty;
    public DateTime WriteOffDate { get; init; }
    public string WriteOffType { get; init; } = string.Empty;
    public decimal Amount { get; init; }
    public decimal RecoveredAmount { get; init; }
    public bool IsRecovered { get; init; }
    public DefaultIdType? CustomerId { get; init; }
    public string? CustomerName { get; init; }
    public string Status { get; init; } = string.Empty;
    public string ApprovalStatus { get; init; } = string.Empty;
}

/// <summary>
/// Write-off data transfer object for detail view with all properties.
/// </summary>
public record WriteOffDetailsDto : WriteOffDto
{
    public DefaultIdType? InvoiceId { get; init; }
    public string? InvoiceNumber { get; init; }
    public DefaultIdType ReceivableAccountId { get; init; }
    public DefaultIdType ExpenseAccountId { get; init; }
    public DefaultIdType? JournalEntryId { get; init; }
    public string? ApprovedBy { get; init; }
    public DateTime? ApprovedDate { get; init; }
    public string? Reason { get; init; }
    public string? Description { get; init; }
    public string? Notes { get; init; }
}

