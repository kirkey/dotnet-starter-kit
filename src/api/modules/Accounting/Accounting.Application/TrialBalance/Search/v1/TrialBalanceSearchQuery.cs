namespace Accounting.Application.TrialBalance.Search.v1;

/// <summary>
/// Query to search trial balance reports with filtering and pagination.
/// </summary>
public sealed class TrialBalanceSearchQuery : PaginationFilter, IRequest<PagedList<TrialBalanceSearchResponse>>
{
    public DefaultIdType? PeriodId { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public string? Status { get; init; }
    public bool? IsBalanced { get; init; }
    public string? TrialBalanceNumber { get; init; }
}
