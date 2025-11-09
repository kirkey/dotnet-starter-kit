using Accounting.Application.PrepaidExpenses.Responses;

namespace Accounting.Application.PrepaidExpenses.Search.v1;

/// <summary>
/// Request to search for prepaid expenses with optional filters.
/// </summary>
public sealed class SearchPrepaidExpensesRequest : PaginationFilter, IRequest<PagedList<PrepaidExpenseResponse>>
{
    public string? PrepaidNumber { get; init; }
    public string? Status { get; init; }
    public DateTime? StartDateFrom { get; init; }
    public DateTime? StartDateTo { get; init; }
    public DefaultIdType? VendorId { get; init; }
    public bool? IsFullyAmortized { get; init; }
}

