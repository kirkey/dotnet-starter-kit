namespace Accounting.Application.InterCompanyTransactions.Responses;

/// <summary>
/// Response containing inter-company transaction details.
/// </summary>
public record InterCompanyTransactionResponse
{
    public DefaultIdType Id { get; init; }
    public string TransactionNumber { get; init; } = string.Empty;
    public DateTime TransactionDate { get; init; }
    public DefaultIdType FromEntityId { get; init; }
    public string? FromEntityName { get; init; }
    public DefaultIdType ToEntityId { get; init; }
    public string? ToEntityName { get; init; }
    public decimal Amount { get; init; }
    public string TransactionType { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public bool IsReconciled { get; init; }
    public DateTime? ReconciliationDate { get; init; }
    public DefaultIdType? FromAccountId { get; init; }
    public DefaultIdType? ToAccountId { get; init; }
    public string? ReferenceNumber { get; init; }
    public string? Description { get; init; }
    public string? Notes { get; init; }
}

