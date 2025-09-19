using Accounting.Application.AccountingPeriods.Responses;

namespace Accounting.Application.AccountingPeriods.Search.v1;

/// <summary>
/// Query model for searching accounting periods with pagination and filters.
/// </summary>
/// <remarks>
/// Use this query to request a paged list of <see cref="AccountingPeriodResponse"/> based on
/// optional filters like name, fiscal year, and closed state.
/// </remarks>
public class SearchAccountingPeriodsQuery : PaginationFilter, IRequest<PagedList<AccountingPeriodResponse>>
{
    /// <summary>
    /// Optional partial name to match accounting period names.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Optional fiscal year filter.
    /// </summary>
    public int? FiscalYear { get; set; }

    /// <summary>
    /// Filter by closed state. When true, only closed periods are returned; when false, only open periods; default false.
    /// </summary>
    public bool IsClosed { get; set; }
}
