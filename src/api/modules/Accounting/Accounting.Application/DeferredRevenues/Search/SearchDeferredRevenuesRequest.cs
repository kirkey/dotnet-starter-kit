using Accounting.Application.DeferredRevenues.Responses;

namespace Accounting.Application.DeferredRevenues.Search;

/// <summary>
/// Request to search deferred revenue entries.
/// </summary>
public sealed class SearchDeferredRevenuesRequest : PaginationFilter, IRequest<PagedList<DeferredRevenueResponse>>
{
    public string? DeferredRevenueNumber { get; init; }
    public bool? IsRecognized { get; init; }
    public DateTime? RecognitionDateFrom { get; init; }
    public DateTime? RecognitionDateTo { get; init; }
}

