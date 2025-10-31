namespace Accounting.Application.InterCompanyTransactions.Queries;

/// <summary>
/// Inter-company transaction data transfer object for list views.
/// </summary>
public record InterCompanyTransactionDto
{
    public DefaultIdType Id { get; init; }
    public string TransactionNumber { get; init; } = string.Empty;
    public DefaultIdType FromEntityId { get; init; }
    public string FromEntityName { get; init; } = string.Empty;
    public DefaultIdType ToEntityId { get; init; }
    public string ToEntityName { get; init; } = string.Empty;
    public DateTime TransactionDate { get; init; }
    public decimal Amount { get; init; }
    public string TransactionType { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public bool IsReconciled { get; init; }
    public DateTime? ReconciliationDate { get; init; }
}

/// <summary>
/// Inter-company transaction data transfer object for detail view with all properties.
/// </summary>
public record InterCompanyTransactionDetailsDto : InterCompanyTransactionDto
{
    public DefaultIdType FromAccountId { get; init; }
    public DefaultIdType ToAccountId { get; init; }
    public string? ReconciledBy { get; init; }
    public DefaultIdType? MatchingTransactionId { get; init; }
    public string? ReferenceNumber { get; init; }
    public DefaultIdType? FromJournalEntryId { get; init; }
    public DefaultIdType? ToJournalEntryId { get; init; }
    public DateTime? DueDate { get; init; }
    public DateTime? SettlementDate { get; init; }
    public DefaultIdType? PeriodId { get; init; }
    public bool RequiresElimination { get; init; }
    public bool IsEliminated { get; init; }
    public DateTime? EliminationDate { get; init; }
    public string? Description { get; init; }
    public string? Notes { get; init; }
}

