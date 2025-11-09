namespace Accounting.Application.PrepaidExpenses.Search.v1;

/// <summary>
/// Specification for searching prepaid expenses with filters and pagination.
/// </summary>
public sealed class SearchPrepaidExpensesSpec : EntitiesByPaginationFilterSpec<PrepaidExpense, Responses.PrepaidExpenseResponse>
{
    public SearchPrepaidExpensesSpec(SearchPrepaidExpensesRequest request)
        : base(request)
    {
        Query
            .Where(p => p.PrepaidNumber.Contains(request.PrepaidNumber!), !string.IsNullOrWhiteSpace(request.PrepaidNumber))
            .Where(p => p.Status == request.Status, !string.IsNullOrWhiteSpace(request.Status))
            .Where(p => p.IsFullyAmortized == request.IsFullyAmortized, request.IsFullyAmortized.HasValue)
            .Where(p => p.StartDate >= request.StartDateFrom, request.StartDateFrom.HasValue)
            .Where(p => p.StartDate <= request.StartDateTo, request.StartDateTo.HasValue)
            .Where(p => p.VendorId == request.VendorId, request.VendorId.HasValue);

        Query.OrderByDescending(p => p.StartDate).ThenBy(p => p.PrepaidNumber);
    }
}

