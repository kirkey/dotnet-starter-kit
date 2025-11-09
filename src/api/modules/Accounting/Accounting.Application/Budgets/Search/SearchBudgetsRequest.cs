using Accounting.Application.Budgets.Responses;

namespace Accounting.Application.Budgets.Search;

/// <summary>
/// Request to search budgets with filtering and pagination.
/// </summary>
public sealed class SearchBudgetsRequest : PaginationFilter, IRequest<PagedList<BudgetResponse>>
{
    public string? Name { get; set; }
    public int? FiscalYear { get; set; }
    public string? Status { get; set; }
}
