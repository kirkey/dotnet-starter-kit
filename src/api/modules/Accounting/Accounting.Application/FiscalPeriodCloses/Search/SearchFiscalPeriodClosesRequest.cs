using Accounting.Application.FiscalPeriodCloses.Responses;

namespace Accounting.Application.FiscalPeriodCloses.Search;

/// <summary>
/// Request to search for fiscal period closes with optional filters and pagination support.
/// </summary>
public class SearchFiscalPeriodClosesRequest : PaginationFilter, IRequest<PagedList<FiscalPeriodCloseResponse>>
{
    public string? CloseNumber { get; set; }
    public string? Status { get; set; }
    public string? CloseType { get; set; }
}
