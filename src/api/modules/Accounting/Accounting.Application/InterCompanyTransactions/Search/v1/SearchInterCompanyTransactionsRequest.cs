using Accounting.Application.InterCompanyTransactions.Responses;

namespace Accounting.Application.InterCompanyTransactions.Search.v1;

/// <summary>
/// Request to search for inter-company transactions with optional filters and pagination.
/// </summary>
public class SearchInterCompanyTransactionsRequest : PaginationFilter, IRequest<PagedList<InterCompanyTransactionResponse>>
{
    public string? TransactionNumber { get; set; }
    public DefaultIdType? FromEntityId { get; set; }
    public DefaultIdType? ToEntityId { get; set; }
    public string? TransactionType { get; set; }
    public string? Status { get; set; }
    public bool? IsReconciled { get; set; }
}
