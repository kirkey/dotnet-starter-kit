using Accounting.Application.InterCompanyTransactions.Responses;

namespace Accounting.Application.InterCompanyTransactions.Search.v1;

/// <summary>
/// Request to search for inter-company transactions with optional filters.
/// </summary>
public record SearchInterCompanyTransactionsRequest(
    string? TransactionNumber = null,
    DefaultIdType? FromEntityId = null,
    DefaultIdType? ToEntityId = null,
    string? TransactionType = null,
    string? Status = null,
    bool? IsReconciled = null) : IRequest<List<InterCompanyTransactionResponse>>;
