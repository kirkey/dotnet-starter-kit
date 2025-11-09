using Accounting.Application.BankReconciliations.Responses;

namespace Accounting.Application.BankReconciliations.Search.v1;

/// <summary>
/// Request to search bank reconciliations with filtering and pagination.
/// </summary>
public class SearchBankReconciliationsRequest : PaginationFilter, IRequest<PagedList<BankReconciliationResponse>>
{
    public DefaultIdType? BankAccountId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string? Status { get; set; }
    public bool? IsReconciled { get; set; }
}
