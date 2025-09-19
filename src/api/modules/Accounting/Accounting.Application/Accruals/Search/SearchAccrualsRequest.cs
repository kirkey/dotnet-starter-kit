using Accounting.Application.Accruals.Responses;

namespace Accounting.Application.Accruals.Search;

// NOTE: Legacy request shape kept for backward-compat reference only. Prefer Queries.SearchAccrualsQuery.
public class LegacySearchAccrualsRequest : PaginationFilter, IRequest<PagedList<AccrualResponse>>
{
    public string? ReferenceNumber { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}
