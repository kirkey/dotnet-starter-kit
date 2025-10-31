using Accounting.Application.PrepaidExpenses.Responses;

namespace Accounting.Application.PrepaidExpenses.Search.v1;

/// <summary>
/// Request to search for prepaid expenses with optional filters.
/// </summary>
public record SearchPrepaidExpensesRequest(
    string? PrepaidNumber = null,
    string? Status = null) : IRequest<List<PrepaidExpenseResponse>>;
